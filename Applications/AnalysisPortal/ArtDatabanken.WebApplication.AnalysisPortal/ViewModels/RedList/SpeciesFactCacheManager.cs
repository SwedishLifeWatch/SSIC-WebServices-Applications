using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web.Caching;
using System.Web.Hosting;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using Resources;
using Exception = System.Exception;
using Timer = System.Timers.Timer;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Defines the different caching types
    /// </summary>
    internal enum CacheType
    {
        SpeciesFactObject,
        ObservationPicture,
        CountyMapPicture,
        Pdf
    }

    public class SpeciesFactCacheManager
    {
        //private readonly ILog mLogger = LogManager.GetLogger(typeof(SpeciesFactCacheManager));

        /// <summary>
        /// The SpeciesFactCacheManager instance
        /// </summary>
        private static SpeciesFactCacheManager mSpeciesFactCacheManagerInstance;

        /// <summary>
        /// Instancelock
        /// </summary>
        private static readonly object InstanceLock = new object();

        // Sync objects for the cache types
        private readonly Dictionary<int, object> mObservationSyncObject = new Dictionary<int, object>();

        /// <summary>
        /// WebRequest lock
        /// </summary>
        private static readonly object WebRequestLock = new object();

        /// <summary>
        /// Is cache initialized or not
        /// </summary>
        private static bool mIsInitialized;

        /// <summary>
        /// Flag that indicates if cache is updating or not
        /// </summary>
        private static bool mUpdatingCache;

        /// <summary>
        /// Flag that indicates that cahce is updating or not
        /// </summary>
        private static bool mAutomaticCacheUpdateEnabled;

        /// <summary>
        /// Indicates that cache update is stopped
        /// </summary>
        private readonly ManualResetEvent mCacheStoppedUpdateEvent;

        /// <summary>
        /// Timer that stops the the cache update
        /// </summary>
        private readonly Timer mCacheUpdateStopTimer;

        /// <summary>
        /// Handle to searchmanager
        /// </summary>
        private RedListSearchManager mTaxonSearchManager;

        /// <summary>
        /// User context
        /// </summary>
        private IUserContext mContext;

        /// <summary>
        /// Gets a list of all protected taxa
        /// </summary>
        private TaxonList ProtectedTaxa
        {
            get
            {
                var cachedObject = AspNetCache.GetCachedObject("protectedTaxa");
                if (cachedObject != null)
                {
                    return (TaxonList)cachedObject;
                }

                var taxonList = GetProtectedTaxa();
                AspNetCache.AddCachedObject(
                    "protectedTaxa",
                    taxonList,
                    DateTime.Now + new TimeSpan(0, 12, 0, 0),
                    CacheItemPriority.Normal);

                return taxonList;
            }
        }

        /// <summary>
        /// Filelists for each cache type (object, map, observation, picture and pdf)
        /// </summary>
        private IList<FileInfo> mObjectFileInfoList = new List<FileInfo>();

        /// <summary>
        /// Constructor
        /// </summary>
        private SpeciesFactCacheManager()
        {
            mCacheUpdateStopTimer = new Timer(14400000); // 4 hours
            mCacheUpdateStopTimer.Elapsed += OnStopCacheUpdateTimerElapsed;
            mCacheStoppedUpdateEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// Gets the instance (the one and only)
        /// </summary>
        public static SpeciesFactCacheManager Instance
        {
            get
            {
                if (mSpeciesFactCacheManagerInstance == null)
                {
                    lock (InstanceLock)
                    {
                        if (mSpeciesFactCacheManagerInstance == null)
                        {
                            mSpeciesFactCacheManagerInstance = new SpeciesFactCacheManager();
                        }
                    }
                }

                return mSpeciesFactCacheManagerInstance;
            }
        }

        /// <summary>
        /// Initialize the cache
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="taxonSpeciesFactManager"></param>
        /// <param name="taxonSearchManager"></param>
        public void InitCacheManager(
            IUserContext userContext, 
            TaxonSpeciesFactManager taxonSpeciesFactManager,
            RedListSearchManager taxonSearchManager)
        {
            if (userContext == null)
            {
                throw new Exception("Failed to create SpeciesFactCacheManager", new ArgumentNullException("userContext"));
            }

            if (taxonSpeciesFactManager == null)
            {
                throw new Exception("Failed to create SpeciesFactCacheManger", new ArgumentNullException("taxonSpeciesFactManager"));
            }

            if (taxonSearchManager == null)
            {
                throw new Exception("Failed to create SpeciesFactCacheManger", new ArgumentNullException("taxonSearchManager"));
            }

            mContext = userContext;
            mContext = CoreData.UserManager.GetApplicationContext();
            mContext.Locale = CoreData.LocaleManager.GetLocale(mContext, LocaleId.sv_SE);

            mTaxonSearchManager = taxonSearchManager;

            //// Get all valid taxa
            //var validTaxonIds = taxonSearchManager.GetAllValidTaxaIds();
            
            //// Create sync objects for all taxa
            //foreach (var taxa in validTaxonIds)
            //{
            //    mObservationSyncObject.Add(taxa, new object());
            //}

            mIsInitialized = true;
            //mLogger.Debug("SpeciesFactCacheManager initialized...");

            mAutomaticCacheUpdateEnabled = true;
            StartAutomaticCacheUpdate();
        }

        /// <summary>
        /// Returns status if the cache is updating or not
        /// </summary>
        /// <returns></returns>
        public bool IsUpdating()
        {
            return mUpdatingCache;
        }

        /// <summary>
        /// Starts automatic updating of the cache (called in global.asax)
        /// </summary>
        public void StartAutomaticCacheUpdate()
        {
            if (!IsCacheInitialized)
            {
                throw new Exception("SpeciesFactCacheManager is not initialized");
            }

            // Create the update thread
            var thread = new Thread(UpdateCache)
            {
                Name = "SpeciesFactCacheUpdateThread"
            };

            // Start the cache update thread
            thread.Start();
        }

        /// <summary>
        /// Stops the automatic updating of the cache (called in Application_End)
        /// </summary>
        public void StopAutomaticCacheUpdate()
        {
            mAutomaticCacheUpdateEnabled = false;
        }

        /// <summary>
        /// Timer for stopping the update after a certain timeperiod (4 hours)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        private void OnStopCacheUpdateTimerElapsed(object o, ElapsedEventArgs args)
        {
            //mLogger.Debug("StopCacheUpdateTimer triggered...");
            StopUpdateCache();
            mCacheUpdateStopTimer.Enabled = false;
        }

        /// <summary>
        /// Stops the cache update
        /// </summary>
        private void StopUpdateCache()
        {
            mUpdatingCache = false;
            mCacheStoppedUpdateEvent.WaitOne();
            mCacheStoppedUpdateEvent.Reset();
            //mLogger.Debug("Cache update stopped...");
        }

        /// <summary>
        /// Updates the cache (runs as a separate thread started at startup of the main application)
        /// </summary>
        private void UpdateCache()
        {
            //mLogger.Debug("UpdateCache thread started...");

            if (!IsCacheInitialized)
            {
                throw new Exception("SpeciesFactCacheManager is not initialized");
            }

            // Sleep 15 seconds to let the webserver start (pdfgeneration relies on the webserver)
            Thread.Sleep(15000);

            // Loop until disabled
            while (!Configuration.Debug && mAutomaticCacheUpdateEnabled)
            {
                if (DateTime.Now.Hour == 22 && !mUpdatingCache)
                {
                    // Log that the cache update has started
                    //mLogger.Debug("Cache update started...");

                    // Set status
                    mUpdatingCache = true;

                    // Enable the stoptimer
                    if (!mCacheUpdateStopTimer.Enabled)
                    {
                        mCacheUpdateStopTimer.Enabled = true;
                    }

                    // Cleanup invalid taxa and update filelist
                    try
                    {
                        CleanupInvalidTaxa();
                        UpdateObjectFileList();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to update filelists for speciesfact cache", ex);
                    }

                    // Update each of the cache types (object, observation, distribution and pdf)
                    foreach (FileInfo fileInfo in mObjectFileInfoList)
                    {
                        string taxonId = fileInfo.Name.Split(new[] { "." }, StringSplitOptions.None)[0];

                        bool taxaIsValid = true;
                        try
                        {
                            var taxa = CoreData.TaxonManager.DataSource.GetTaxon(mContext, int.Parse(taxonId));
                            taxaIsValid = taxa.IsValid;
                        }
                        catch (Exception)
                        {
                            taxaIsValid = false;
                        }

                        if (taxaIsValid)
                        {
                            try
                            {
                                //UpdateTaxonCache(int.Parse(taxonId), CacheType.SpeciesFactObject);
                                //UpdateTaxonCache(int.Parse(taxonId), CacheType.CountyMapPicture);
                                //UpdateTaxonCache(int.Parse(taxonId), CacheType.ObservationPicture);
                                //UpdateTaxonCache(int.Parse(taxonId), CacheType.Pdf);
                            }
                            catch (Exception)
                            {
                                //mLogger.Error(string.Format("Failed to update cache for taxon {0}, error: {1}", taxonId, ex.Message));
                            }
                        }

                        if (!mUpdatingCache)
                        {
                            break;
                        }

                        // Wait between each taxon
                        Thread.Sleep(3000);
                    }

                    // Updating finished
                    mUpdatingCache = false;

                    // Signal that the update has stopped
                    mCacheStoppedUpdateEvent.Set();
                }

                // Do the timecheck once a minute
                Thread.Sleep(60000);
            }

            //mLogger.Debug("Automatic cache update thread stopped...");
        }

        /// <summary>
        /// Checks if the cachemanager is initialized or not
        /// </summary>
        private bool IsCacheInitialized
        {
            get { return mIsInitialized; }
        }

        ///// <summary>
        ///// Gets the taxon object from a taxon id or taxon name.
        ///// </summary>
        ///// <param name="taxonIdentifier">Id of the taxon.</param>
        ///// <returns>Taxon.</returns>
        //private ITaxon GetTaxon(string taxonIdentifier)
        //{
        //    TaxonLookupResultViewModel taxonLookupResult = mTaxonSearchManager.LookupTaxon(taxonIdentifier);

        //    if (taxonLookupResult.OneTaxonIsFound)
        //    {
        //        var taxonObject = taxonLookupResult.Taxon;

        //        return taxonObject;
        //    }

        //    // Check error msg.
        //    string errosMsg = taxonLookupResult.ErrorMessage;
        //    if (errosMsg.IsEmpty())
        //    {
        //        errosMsg = "Failed to get taxon, multiple taxons found for taxon: {0}";
        //    }
        //    else
        //    {
        //        errosMsg = errosMsg + ": {0}";
        //    }

        //    throw new Exception(string.Format(errosMsg, taxonIdentifier));
        //}

        ///// <summary>
        ///// Gets the observation map as a png from the cache.
        ///// </summary>
        ///// <param name="taxonId">The taxon id or name (scientific name or common name).</param>
        ///// <param name="forceUpdate">Forces an update even if the cache is present</param>
        ///// <returns>The observation map as a png.</returns>
        //public byte[] GetObservationMap(string taxonId, bool forceUpdate = false)
        //{
        //    if (!IsCacheInitialized)
        //        throw new Exception("SpeciesFactCacheManager is not initialized");

        //    ITaxon taxon = GetTaxon(taxonId);
        //    if (!SpeciesFilter.IsTaxaSpeciesOrBelow(taxon) || IsProtectedTaxa(taxon))
        //    {
        //        return null;
        //    }

        //    byte[] buffer;
        //    lock (mObservationSyncObject[taxon.Id])
        //    {
        //        if (IsProtectedTaxa(taxon))
        //        {
        //            if (File.Exists(GetObservationPictureFilePath(taxon)))
        //            {
        //                mLogger.Error(string.Format("Observationmap file for protected taxa found, id: {0}", taxon.Id));
        //            }

        //            return null;
        //        }

        //        if (forceUpdate || !File.Exists(GetObservationPictureFilePath(taxon)))
        //        {
        //            try
        //            {
        //                var map = MapManager.CreateSpeciesObservationPointMap(mContext, taxon.Id);

        //                buffer = UpdateObservationMapCache(taxon);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(string.Format("Failed to get observationmap from cache for taxon {0}", taxon.Id), ex);
        //            }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                buffer = ReadObservationMapCacheFromFile(taxon);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(string.Format("Failed to get observationmap from cache for taxon {0}", taxon.Id), ex);
        //            }
        //        }
        //    }

        //    return buffer;
        //}

        /// <summary>
        /// Checks if a taxa is protected
        /// </summary>
        /// <param name="taxon"></param>
        /// <returns></returns>
        public bool IsProtectedTaxa(ITaxon taxon)
        {
            return ProtectedTaxa.Contains(taxon);
        }

        ///// <summary>
        ///// Reads the ObservationMapCache from file
        ///// </summary>
        ///// <param name="taxon"></param>
        ///// <returns></returns>
        //private byte[] ReadObservationMapCacheFromFile(ITaxon taxon)
        //{
        //    if (taxon == null)
        //    {
        //        throw new ArgumentNullException("taxon");
        //    }

        //    byte[] buffer;
        //    try
        //    {
        //        using (var fileStream = new FileStream(GetObservationPictureFilePath(taxon), FileMode.Open, FileAccess.Read))
        //        {
        //            buffer = new byte[(int)fileStream.Length];
        //            fileStream.Read(buffer, 0, (int)fileStream.Length);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(string.Format("Failed to read observationmap cache from file for taxon {0}", taxon.Id), ex);
        //    }

        //    return buffer;
        //}

        ///// <summary>
        ///// Updates the cache for the observation map.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <returns>The observation map as a png encapsulated in a FileResult object.</returns>
        //private byte[] UpdateObservationMapCache(ITaxon taxon)
        //{
        //    if (!IsCacheInitialized)
        //        throw new Exception("SpeciesFactCacheManager is not initialized");

        //    // Set swedish locale
        //    SetSwedishLangugage();

        //    string url = AppSettings.Default.SpeciesObservationPointMapUrl + taxon.Id;
        //    Stream stream = null;
        //    HttpWebResponse response = null;
        //    byte[] buffer;

        //    lock (WebRequestLock)
        //    {
        //        try
        //        {
        //            // Create request
        //            var req = (HttpWebRequest)WebRequest.Create(url);

        //            // Set timeout for the request
        //            req.Timeout = 50000;

        //            // Fetch the response
        //            response = (HttpWebResponse)req.GetResponse();
        //            stream = response.GetResponseStream();
        //            if (response.ContentLength == 0 || stream == null)
        //            {
        //                return null;
        //            }

        //            using (var br = new BinaryReader(stream))
        //            {
        //                var len = (int)(response.ContentLength);
        //                buffer = br.ReadBytes(len);
        //                br.Close();
        //            }

        //            stream.Close();
        //            response.Close();

        //            if (File.Exists(GetObservationPictureFilePath(taxon)))
        //            {
        //                File.Delete(GetObservationPictureFilePath(taxon));
        //            }

        //            using (var fileStream = new FileStream(GetObservationPictureFilePath(taxon), FileMode.Create, FileAccess.Write))
        //            {
        //                fileStream.Write(buffer, 0, buffer.Length);
        //                fileStream.Flush(true);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(string.Format("Failed to update observationmap cache for taxon {0}", taxon.Id), ex);
        //        }
        //        finally
        //        {
        //            if (stream != null)
        //            {
        //                stream.Close();
        //            }

        //            if (response != null)
        //            {
        //                response.Close();
        //            }
        //        }
        //    }
        //    return buffer;
        //}

        ///// <summary>
        ///// Gets the path to the cached observation pictures.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <returns>Full path of the cached observation pictures.</returns>
        //private string GetObservationPictureFilePath(ITaxon taxon)
        //{
        //    string path = HostingEnvironment.MapPath(@"~/" + AppSettings.Default.SpeciesFactCacheObservationFolder) + @"\" + taxon.Id + ".png";
        //    return path;
        //}

        /// <summary>
        /// Gets the observationpicture path
        /// </summary>
        /// <returns></returns>
        private string GetObservationPicturePath()
        {
            string path = HostingEnvironment.MapPath(@"~/" + AppSettings.Default.SpeciesFactCacheObservationFolder) +
                          @"\";
            return path;
        }

        /// <summary>
        /// Updates the filelist of present cache objects (objects, taxonpictures, pdf files etc.)
        /// </summary>
        private void UpdateObjectFileList()
        {
            var objectDirectoryInfo = new DirectoryInfo(GetObjectPath());

            // Get all objectcache filenames and sort them by last writetime
            foreach (FileInfo fileInfo in objectDirectoryInfo.GetFiles())
            {
                if (!fileInfo.Name.Contains("dummy"))
                {
                    mObjectFileInfoList.Add(fileInfo);
                }
            }
            mObjectFileInfoList = mObjectFileInfoList.OrderBy(x => x.LastWriteTime).ToList();
        }

        /// <summary>
        /// Gets the object folder
        /// </summary>
        /// <returns></returns>
        private string GetObjectPath()
        {
            return HostingEnvironment.MapPath(@"~/" + AppSettings.Default.SpeciesFactCacheObjectFolder) + @"\";
        }

        /// <summary>
        /// Remove all invalid cache files (cached files that have taxon ids that aren't valid any longer)
        /// </summary>
        private void CleanupInvalidTaxa()
        {
            // Get all cachedirectories
            var observationPictureDirectoryInfo = new DirectoryInfo(GetObservationPicturePath());

            // Get all valid taxa
            var validTaxonIds = mTaxonSearchManager.GetAllValidTaxaIds();

            // Cleanup invalid files for each cachetype
            CleanupFiles(observationPictureDirectoryInfo.GetFiles(), validTaxonIds);
        }

        private void CleanupFiles(IEnumerable<FileInfo> fileInfos, HashSet<int> validTaxa)
        {
            // Delete invalid files
            foreach (var fileInfo in fileInfos)
            {
                int val;
                string taxonId = fileInfo.Name.Split(new[] { "." }, StringSplitOptions.None)[0];
                if (!int.TryParse(taxonId, out val))
                {
                    continue;
                }

                if (!validTaxa.Contains(val))
                {
                    try
                    {
                        File.Delete(fileInfo.FullName);
                    }
                    catch (Exception)
                    {
                        //mLogger.Error(string.Format("Failed to delete filename {0}", fileInfo.Name));
                        throw;
                    }
                }
            }
        }

        ///// <summary>
        ///// Updates the specific cache part, object, pictures etc.
        ///// </summary>
        ///// <param name="taxonId"></param>
        ///// <param name="cacheType"></param>
        //private void UpdateTaxonCache(int taxonId, CacheType cacheType)
        //{
        //    switch (cacheType)
        //    {
        //        case CacheType.ObservationPicture:
        //            GetObservationMap(taxonId.ToString(CultureInfo.InvariantCulture), true);
        //            break;
        //        default:
        //            throw new Exception("Invalid cache type");
        //    }
        //}

        /// <summary>
        /// Sets the language of the thread to swedish
        /// </summary>
        private void SetSwedishLangugage()
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "sv-SE" ||
                Thread.CurrentThread.CurrentUICulture.Name != "sv-SE")
            {
                //mLogger.Debug("Changed language to swedish (sv-SE) for cache update...");

                Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            }
        }

        /// <summary>
        /// Get all protected taxa
        /// </summary>
        /// <returns>A list of all protected taxa</returns>
        private TaxonList GetProtectedTaxa()
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(mContext, FactorId.ProtectionLevel));

            var speciesFactList = CoreData.SpeciesFactManager.GetSpeciesFacts(mContext, searchCriteria);

            var taxonList = new TaxonList(true);
            foreach (var speciesFact in speciesFactList)
            {
                if (speciesFact.MainField != null && speciesFact.MainField.HasValue &&
                speciesFact.MainField.EnumValue != null && speciesFact.MainField.EnumValue.Enum != null)
                {
                    const int RedListProtectionLevel = 1;
                    if (speciesFact.MainField.EnumValue.KeyInt > RedListProtectionLevel)
                    {
                        taxonList.Add(speciesFact.Taxon);
                    }
                }
            }

            ITaxonTreeSearchCriteria taxonTreeSearchCriteria = new TaxonTreeSearchCriteria();
            taxonTreeSearchCriteria.TaxonIds = taxonList.GetIds();
            taxonTreeSearchCriteria.Scope = TaxonTreeSearchScope.AllChildTaxa;
            taxonTreeSearchCriteria.IsValidRequired = true;
            TaxonTreeNodeList taxonTreeNodeList = CoreData.TaxonManager.GetTaxonTrees(mContext, taxonTreeSearchCriteria);

            foreach (var taxonTreeNode in taxonTreeNodeList)
            {
                taxonList.Merge(taxonTreeNode.GetChildTaxa());
            }

            return taxonList;
        }
    }
}
