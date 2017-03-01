using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Hosting;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// This class handles caching of information that is
    /// presented in taxon list view.
    /// </summary>
    public class TaxonListInformationManager
    {
        ///// <summary>
        ///// Logger
        ///// </summary>
        //private readonly ILog mLogger = LogManager.GetLogger(typeof(TaxonListInformationManager));

        /// <summary>
        /// The one and only instance
        /// </summary>
        private static volatile TaxonListInformationManager taxonListInformationManagerInstance;

        /// <summary>
        /// Lock object for the instance access
        /// </summary>
        private static readonly object InstanceLock = new object();

        /// <summary>
        /// Lock for the cache handled by TaxonListInformationManager
        /// </summary>
        private static readonly object CacheLock = new object();

        /// <summary>
        /// The cache
        /// </summary>
        private TaxonInformationCache mTaxonInformationCache;

        /// <summary>
        /// The context
        /// </summary>
        private readonly IUserContext mContext;

        /// <summary>
        /// Flag to indicate if the cache is initialized or not
        /// </summary>
        private bool mIsInitialized;

        /// <summary>
        /// Gets the one and only instance
        /// </summary>
        public static TaxonListInformationManager Instance
        {
            get
            {
                if (taxonListInformationManagerInstance == null)
                {
                    lock (InstanceLock)
                    {
                        if (taxonListInformationManagerInstance == null)
                        {
                            taxonListInformationManagerInstance = new TaxonListInformationManager();
                        }
                    }
                }

                return taxonListInformationManagerInstance;
            }
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        private TaxonListInformationManager()
        {
            mTaxonInformationCache = new TaxonInformationCache();
            mContext = CoreData.UserManager.GetApplicationContext();
        }

        /// <summary>
        /// Private property that handles concurrent access to cache.
        /// </summary>
        private TaxonInformationCache TaxonInformationCache
        {
            get
            {
                lock (CacheLock)
                {
                    return mTaxonInformationCache;
                }
            }

            set
            {
                lock (CacheLock)
                {
                    mTaxonInformationCache = value;
                }
            }
        }

        /// <summary>
        /// Init taxon list information cache.
        /// </summary>
        public void InitCache()
        {
            var cacheFileName = GetCacheFileName();

            // Reset old cache
            TaxonInformationCache = null;

            try
            {
                // Try to read from cached file.
                if (File.Exists(cacheFileName))
                {
                    using (FileStream stream = File.Open(
                        cacheFileName,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read))
                    {
                        var formatter = new BinaryFormatter();
                        TaxonInformationCache = (TaxonInformationCache)formatter.Deserialize(stream);
                        stream.Close();
                        if (!TaxonInformationCache.IsOk())
                        {
                            TaxonInformationCache = null;
                        }
                    }
                }
            }
            catch
            {
                TaxonInformationCache = null;
            }

            if (TaxonInformationCache.IsNull())
            {
                //mLogger.Debug("Invalid definition of TaxonInformationCache, start updating...");
                UpdateCache();
                //mLogger.Debug("TaxonInformationCache updated...");
            }

            mIsInitialized = true;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        public void UpdateCache()
        {
            TaxonInformationCache = new TaxonInformationCache();
            TaxonInformationCache.Init();

            LoadTaxonListInformation();

            try
            {
                // Save cache to file.
                var cacheFileName = GetCacheFileName();
                if (File.Exists(cacheFileName))
                {
                    File.Delete(cacheFileName);
                }

                FileSystemManager.EnsureFolderExists(cacheFileName);
                using (var stream = File.Open(
                    cacheFileName,
                    FileMode.OpenOrCreate,
                    FileAccess.Write,
                    FileShare.None))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, TaxonInformationCache);
                    stream.Flush(true);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                mIsInitialized = false;
                throw new Exception("Failed to refresh TaxonInformationListManager cache", ex);
            }
        }

        /// <summary>
        /// Gets the filename for the cache
        /// </summary>
        /// <returns></returns>
        private string GetCacheFileName()
        {
            string cacheFileName = @"~/Cache/TaxonInformationCache.Data";
            cacheFileName = HostingEnvironment.MapPath(cacheFileName);
            return cacheFileName;
        }

        /// <summary>
        /// Check that taxon list information about specified taxa has been fetched in the cache.
        /// Cache the information if it is not cached.
        /// </summary>
        /// <param name="taxonIds">Taxon ids.</param>
        private void CheckCachedInformation(IEnumerable<ITaxonId> taxonIds)
        {
            var notCachedTaxa = new TaxonIdList();
            foreach (ITaxonId taxonId in taxonIds)
            {
                if (!mTaxonInformationCache.TaxonInformation.ContainsKey(taxonId.Id))
                {
                    notCachedTaxa.Add(taxonId);
                }
            }

            if (notCachedTaxa.IsNotEmpty())
            {
                LoadTaxonListInformation(notCachedTaxa);
            }
        }

        /// <summary>
        /// Check that taxon list information about specified taxa has been cached.
        /// Cache the information if it is not cached.
        /// </summary>
        /// <param name="taxa">The taxa.</param>
        private void CheckCachedInformation(IEnumerable<ITaxon> taxa)
        {
            var notCachedTaxa = new TaxonList();
            foreach (ITaxon taxon in taxa)
            {
                if (!TaxonInformationCache.TaxonInformation.ContainsKey(taxon.Id))
                {
                    notCachedTaxa.Add(taxon);
                }
            }

            if (notCachedTaxa.IsNotEmpty())
            {
                LoadTaxonListInformation(notCachedTaxa);
            }
        }

        /// <summary>
        /// Get landscape type species facts.
        /// </summary>
        private void GetLandscapeTypeSpeciesFactsToCache()
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            FactorList factors = CoreData.FactorManager.GetFactorTree(mContext, FactorId.LandscapeFactors).GetAllLeafFactors();
            searchCriteria.Factors.AddRange(factors);
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();
            SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(mContext, searchCriteria);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (TaxonInformationCache.TaxonInformation.ContainsKey(speciesFact.Taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact);
                    }
                }
            }
        }

        /// <summary>
        /// Get organism group species facts.
        /// </summary>>
        private void GetOrganismGroupSpeciesFactsToCache()
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            IFactor factor = CoreData.FactorManager.GetFactor(mContext, FactorId.Redlist_OrganismLabel1);
            searchCriteria.Factors.Add(factor);
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();
            SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(mContext, searchCriteria);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (TaxonInformationCache.TaxonInformation.ContainsKey(speciesFact.Taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact);
                    }
                }
            }
        }

        /// <summary>
        /// Get red list category species facts.
        /// </summary>
        private void GetRedListCategorySpeciesFactsToCache()
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            IFactor factor = CoreData.FactorManager.GetFactor(mContext, FactorId.RedlistCategory);
            searchCriteria.Factors.Add(factor);
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            searchCriteria.Periods = new PeriodList { CoreData.FactorManager.GetCurrentRedListPeriod(mContext) };
            ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();
            SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(mContext, searchCriteria);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (TaxonInformationCache.TaxonInformation.ContainsKey(speciesFact.Taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact);
                    }
                }
            }
        }

        /// <summary>
        /// Get red list criteria species facts.
        /// </summary>
        private void GetRedListCriteriaSpeciesFactsToCache()
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            IFactor factor = CoreData.FactorManager.GetFactor(mContext, FactorId.RedlistCriteriaString);
            searchCriteria.Factors.Add(factor);
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            searchCriteria.Periods = new PeriodList { CoreData.FactorManager.GetCurrentRedListPeriod(mContext) };
            ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();
            SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(mContext, searchCriteria);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (TaxonInformationCache.TaxonInformation.ContainsKey(speciesFact.Taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact);
                    }
                }
            }
        }

        /// <summary>
        /// Get previouly red list categories species facts.
        /// </summary>
        private void GetRedListOldCategorySpeciesFactsToCache()
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            IFactor factor = CoreData.FactorManager.GetFactor(mContext, FactorId.RedlistCategory);
            searchCriteria.Factors.Add(factor);
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            PeriodList periodsList = CoreData.FactorManager.GetPublicPeriods(mContext);
            IPeriod currentPeriod = CoreData.FactorManager.GetCurrentRedListPeriod(mContext);
            var oldPeriods = new PeriodList();
            foreach (IPeriod period in periodsList)
            {
                if (period.Id != currentPeriod.Id)
                {
                    oldPeriods.Add(period);
                }
            }
            searchCriteria.Periods = oldPeriods;
            ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();
            SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(mContext, searchCriteria);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (TaxonInformationCache.TaxonInformation.ContainsKey(speciesFact.Taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact, currentPeriod);
                    }
                }
            }
        }

        /// <summary>
        /// Get red list taxon category species facts.
        /// </summary>
        private void GetRedListTaxonCategorySpeciesFactsToCache()
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            IFactor factor = CoreData.FactorManager.GetFactor(mContext, FactorId.Redlist_TaxonType);
            searchCriteria.Factors.Add(factor);
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            searchCriteria.Periods = new PeriodList { CoreData.FactorManager.GetCurrentRedListPeriod(mContext) };
            ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();
            SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(mContext, searchCriteria);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (TaxonInformationCache.TaxonInformation.ContainsKey(speciesFact.Taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact);
                    }
                }
            }
        }

        /// <summary>
        /// Get swedish occurrence type species facts.
        /// </summary>
        private void GetSwedishOccurrenceSpeciesFactsToCache()
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            IFactor factor = CoreData.FactorManager.GetFactor(mContext, FactorId.SwedishOccurrence);
            searchCriteria.Factors.Add(factor);
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();
            SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(mContext, searchCriteria);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    if (TaxonInformationCache.TaxonInformation.ContainsKey(speciesFact.Taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact);
                    }
                }
            }
        }

        /// <summary>
        /// Get all species facts that are needed to initialize
        /// properties in class TaxonListInformation.
        /// </summary>
        /// <param name="taxa">The taxa.</param>
        /// <returns>Taxon list information about specified taxa.</returns>
        private SpeciesFactList GetSpeciesFacts(IEnumerable<ITaxon> taxa)
        {
            ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            FactorList landscapeFactors = CoreData.FactorManager.GetFactorTree(mContext, FactorId.LandscapeFactors).GetAllLeafFactors();
            searchCriteria.Factors.AddRange(landscapeFactors);
            IFactor redlistCategoryFactor = CoreData.FactorManager.GetFactor(mContext, FactorId.RedlistCategory);
            searchCriteria.Factors.Add(redlistCategoryFactor);
            IFactor redlistTaxonTypeFactor = CoreData.FactorManager.GetFactor(mContext, FactorId.Redlist_TaxonType);
            searchCriteria.Factors.Add(redlistTaxonTypeFactor);
            IFactor redlistOrganismLabel1 = CoreData.FactorManager.GetFactor(mContext, FactorId.Redlist_OrganismLabel1);
            searchCriteria.Factors.Add(redlistOrganismLabel1);
            IFactor redlistCriteriaFactor = CoreData.FactorManager.GetFactor(mContext, FactorId.RedlistCriteriaString);
            searchCriteria.Factors.Add(redlistCriteriaFactor);
            searchCriteria.IndividualCategories = new IndividualCategoryList
            {
                CoreData.FactorManager.GetDefaultIndividualCategory(mContext)
            };
            searchCriteria.Periods = new PeriodList { CoreData.FactorManager.GetCurrentRedListPeriod(mContext) };
            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.AddRange(taxa);
            ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();
            SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(mContext, searchCriteria);

            return speciesFacts;
        }

        /// <summary>
        /// Get taxon list information about specified taxa.
        /// Returned list has the same order as the input taxon list.
        /// </summary>
        /// <param name="taxa">The taxa.</param>
        /// <returns>Taxon list information about specified taxa.</returns>
        public List<TaxonListInformation> GetTaxonListInformation(TaxonList taxa)
        {
            if (!mIsInitialized)
            {
                throw new Exception("TaxonListInformation cache is not initialized");
            }

            var taxonListInformation = new List<TaxonListInformation>();
            if (taxa.IsNotEmpty())
            {
                CheckCachedInformation(taxa);
                taxonListInformation.AddRange(taxa.Select(taxon => TaxonInformationCache.TaxonInformation[taxon.Id]));
            }

            return taxonListInformation;
        }

        /// <summary>
        /// Get taxon information about specified taxon.
        /// Returned taxon has the same order id as the input taxon id.
        /// </summary>
        /// <param name="taxon">Taxon to get chached information on.</param>
        /// <returns>Taxon list information about specified taxa.</returns>
        public TaxonListInformation GetTaxonInformationFromCache(ITaxon taxon)
        {
            if (!mIsInitialized)
            {
                throw new Exception("TaxonListInformation cache is not initialized");
            }

            TaxonListInformation tempTaxon = null;

            if (TaxonInformationCache.TaxonInformation.IsNotNull() && TaxonInformationCache.TaxonInformation.ContainsKey(taxon.Id) && taxon.IsNotNull())
            {
                var taxa = new TaxonList { taxon };

                // Check is performed on list therefore a temporary list is created with one item.
                CheckCachedInformation(taxa);
                tempTaxon = TaxonInformationCache.TaxonInformation[taxon.Id];
            }

            return tempTaxon;
        }

        /// <summary>
        /// Get taxon information about specified taxon.
        /// Returned taxon has the same order id as the input taxon id.
        /// </summary>
        /// <param name="taxonId">Taxon to get chached information on.</param>
        /// <returns>Taxon list information about specified taxa.</returns>
        public TaxonListInformation GetTaxonInformationFromCache(int taxonId)
        {
            if (!mIsInitialized)
            {
                throw new Exception("TaxonListInformation cache is not initialized");
            }

            ITaxon taxon = CoreData.TaxonManager.GetTaxon(mContext, taxonId);
            TaxonListInformation tempTaxon = null;

            if (TaxonInformationCache.TaxonInformation.IsNotNull() && TaxonInformationCache.TaxonInformation.ContainsKey(taxonId) && taxon.IsNotNull())
            {
                var taxa = new TaxonList { taxon };

                // Check is performed on list therefore a temporary list is created with one item.
                CheckCachedInformation(taxa);
                tempTaxon = TaxonInformationCache.TaxonInformation[taxon.Id];
            }

            return tempTaxon;
        }

        /// <summary>
        /// Get taxon list information about specified taxa.
        /// Returned list has the same order as the input taxon list.
        /// </summary>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <param name="useNonCategorizedTaxaIdOnly">Indicates if filtering on only data without category 743 is to be returned.</param>
        /// <param name="selectedCategories">Selected categories.</param>
        /// <returns>Taxon list information about specified taxa.</returns>
        public List<TaxonListInformation> GetTaxonListInformation(
            TaxonIdList taxonIds,
            bool useNonCategorizedTaxaIdOnly,
            IList<RedListCategoryItemViewModel> selectedCategories)
        {
            if (!mIsInitialized)
            {
                throw new Exception("TaxonListInformation cache is not initialized");
            }

            var taxonListInformation = new List<TaxonListInformation>();

            if (taxonIds.IsNotEmpty())
            {
                CheckCachedInformation(taxonIds);
                foreach (ITaxonId taxonId in taxonIds)
                {
                    TaxonListInformation taxonInfo = TaxonInformationCache.TaxonInformation[taxonId.Id];

                    // Check if only non categorized taxa are to be shown.
                    if (useNonCategorizedTaxaIdOnly)
                    {
                        bool taxonFound = selectedCategories.Any(category => category.OrderNumber == taxonInfo.RedListCategoryId);

                        // First we check if taxon is beloning to a selected category if so add this taxon.
                        if (taxonFound)
                        {
                            taxonListInformation.Add(taxonInfo);
                        }
                        else if (!taxonInfo.IsRedListed && !taxonInfo.IsRedListedEnsured)
                        {
                            // Not add taxon since it is ensured or redlisted and selected catgories is not choosen
                            // This option is needed to select taxon which is not redlisted or ensured. These taxa should not be added.
                            // This selcetion is keept for debugging purposes.
                            // Only non categorized species are used here
                            taxonListInformation.Add(taxonInfo);
                        }
                    }
                    else
                    {
                        taxonListInformation.Add(taxonInfo);
                    }
                }
            }

            return taxonListInformation;
        }

        /// <summary>
        /// Cache taxon list information about all valid taxa.
        /// </summary>
        private void LoadTaxonListInformation()
        {
            // Get taxa.
            ITaxonSearchCriteria searchCriteria = new TaxonSearchCriteria();
            searchCriteria.IsValidTaxon = true;
            TaxonList taxa = CoreData.TaxonManager.GetTaxa(mContext, searchCriteria);

            // Init new taxon information items.
            foreach (ITaxon taxon in taxa)
            {
                var taxonListInformation = new TaxonListInformation
                {
                    CommonName = taxon.CommonName,
                    Id = taxon.Id,
                    CategoryId = taxon.Category.Id,
                    ParentCategoryId = taxon.Category.ParentId,
                    ScientificName = taxon.ScientificName
                };
                mTaxonInformationCache.TaxonInformation[taxon.Id] = taxonListInformation;
            }

            // Add species fact information.
            //GetLandscapeTypeSpeciesFactsToCache();
            //GetOrganismGroupSpeciesFactsToCache();
            GetRedListCategorySpeciesFactsToCache();
            //GetRedListCriteriaSpeciesFactsToCache();
            //GetRedListTaxonCategorySpeciesFactsToCache();
            //GetSwedishOccurrenceSpeciesFactsToCache();
            //GetRedListOldCategorySpeciesFactsToCache();
        }

        /// <summary>
        /// Cache taxon list information about specified taxa.
        /// </summary>
        /// <param name="taxonIds">Taxon ids.</param>
        private void LoadTaxonListInformation(IEnumerable<ITaxonId> taxonIds)
        {
            // Get taxa.
            var taxonIdList = taxonIds.Select(taxonId => taxonId.Id).ToList();

            TaxonList taxa = CoreData.TaxonManager.GetTaxa(mContext, taxonIdList);

            // Init new taxon information items.
            var tempTaxonInformation = new Dictionary<Int32, TaxonListInformation>();
            foreach (ITaxon taxon in taxa)
            {
                var taxonListInformation = new TaxonListInformation
                {
                    CommonName = taxon.CommonName,
                    Id = taxon.Id,
                    CategoryId = taxon.Category.Id,
                    ParentCategoryId = taxon.Category.ParentId,
                    ScientificName = taxon.ScientificName
                };
                tempTaxonInformation[taxon.Id] = taxonListInformation;
            }

            // Add species fact information.
            var speciesFacts = GetSpeciesFacts(taxa);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    tempTaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact);
                }
            }

            // Add new taxon information items to cache.
            lock (CacheLock)
            {
                foreach (ITaxon taxon in taxa)
                {
                    if (!TaxonInformationCache.TaxonInformation.ContainsKey(taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[taxon.Id] = tempTaxonInformation[taxon.Id];
                    }
                }
            }
        }

        /// <summary>
        /// Cache taxon list information about specified taxa.
        /// </summary>
        /// <param name="taxa">The taxa.</param>
        private void LoadTaxonListInformation(TaxonList taxa)
        {
            // Init new taxon information items.
            var tempTaxonInformation = new Dictionary<Int32, TaxonListInformation>();
            foreach (ITaxon taxon in taxa)
            {
                var taxonListInformation = new TaxonListInformation
                {
                    CommonName = taxon.CommonName,
                    Id = taxon.Id,
                    CategoryId = taxon.Category.Id,
                    ParentCategoryId = taxon.Category.ParentId,
                    ScientificName = taxon.ScientificName
                };
                tempTaxonInformation[taxon.Id] = taxonListInformation;
            }

            // Add species fact information.
            SpeciesFactList speciesFacts = GetSpeciesFacts(taxa);
            if (speciesFacts.IsNotEmpty())
            {
                foreach (ISpeciesFact speciesFact in speciesFacts)
                {
                    tempTaxonInformation[speciesFact.Taxon.Id].SetSpeciesFact(speciesFact);
                }
            }

            // Add new taxon information items
            lock (CacheLock)
            {
                foreach (ITaxon taxon in taxa)
                {
                    if (!TaxonInformationCache.TaxonInformation.ContainsKey(taxon.Id))
                    {
                        TaxonInformationCache.TaxonInformation[taxon.Id] = tempTaxonInformation[taxon.Id];
                    }
                }
            }
        }
    }
}
