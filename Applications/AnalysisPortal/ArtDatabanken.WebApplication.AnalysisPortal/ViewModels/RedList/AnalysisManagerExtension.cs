using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Web.Hosting;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Logging;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Extension methods for the interface IAnalysisManager.
    /// </summary>    
    // ReSharper disable once InconsistentNaming
    public static class AnalysisManagerExtension
    {
        //private static readonly ILog Logger = LogManager.GetLogger(typeof(AnalysisManagerExtension));

        private static AnalysisCache mCache;
        private static bool mStopUpdate;

        /// <summary>
        /// Get analysis cache file name.
        /// </summary>
        /// <returns>Analysis cache file name.</returns>
        private static string GetCacheFileName()
        {
            string cacheFileName = @"~/Cache/AnalysisCache.Data";
            cacheFileName = HostingEnvironment.MapPath(cacheFileName);
            return cacheFileName;
        }

        /// <summary>
        /// Get ids for taxa that matches search criteria.
        /// </summary>
        /// <param name="analysisManager">Analysis manager.</param>
        /// <param name="searchCriteria">Analysis search critera.</param>
        /// <returns>Ids for taxa that matches search criteria.</returns>
        public static TaxonIdList GetTaxonIds(this IAnalysisManager analysisManager, AnalysisSearchCriteria searchCriteria)
        {
            return mCache.GetTaxonIds(searchCriteria);
        }

        /// <summary>
        /// Get ids for taxa that matches search criteria.
        /// </summary>
        /// <param name="analysisManager">Analysis manager.</param>
        /// <param name="searchCriteria">Analysis search critera.</param>
        /// <param name="taxonIds">
        /// Limit search to these taxa.
        /// This parameter is ignored if value is null.
        /// </param>
        /// <returns>Ids for taxa that matches search criteria.</returns>
        public static TaxonIdList GetTaxonIds(
            this IAnalysisManager analysisManager,
            AnalysisSearchCriteria searchCriteria,
            TaxonIdList taxonIds)
        {
            return mCache.GetTaxonIds(searchCriteria, taxonIds);
        }

        /// <summary>
        /// Get ids for taxa that matches search criteria.
        /// </summary>
        /// <param name="analysisManager">Analysis manager.</param>
        /// <param name="searchCriteria">Analysis search critera.</param>
        /// <param name="taxa">
        /// Limit search to these taxa.
        /// This parameter is ignored if value is null.
        /// </param>
        /// <returns>Ids for taxa that matches search criteria.</returns>
        public static TaxonIdList GetTaxonIds(
            this IAnalysisManager analysisManager,
            AnalysisSearchCriteria searchCriteria,
            TaxonList taxa)
        {
            TaxonIdList taxonIds = null;
            if (taxa.IsNotNull())
            {
                taxonIds = new TaxonIdList();
                taxonIds.Merge(taxa);
            }

            return mCache.GetTaxonIds(searchCriteria, taxonIds);
        }

        ///// <summary>
        ///// The get taxon ids selected by scope.
        ///// </summary>
        ///// <param name="analysisManager"></param>
        ///// <param name="searchCriteria">
        ///// The search criteria.
        ///// </param>
        ///// <returns>
        ///// Ids for taxa that matches scope/search criteria.
        ///// </returns>
        //public static TaxonIdList GetTaxonIdsByScope(this IAnalysisManager analysisManager, AnalysisSearchCriteria searchCriteria)
        //{
        //    return mCache.GetTaxonIdsByScope(searchCriteria);
        //}

        /// <summary>
        /// Gets the redlist category for a specific taxon
        /// </summary>
        /// <param name="analysisManager"></param>
        /// <param name="taxonId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool GetRedListCategoryForTaxonId(this IAnalysisManager analysisManager, int taxonId, out RedListCategory category)
        {
            return mCache.GetRedListCategoryForTaxonId(taxonId, out category);
        }

        ///// <summary>
        ///// Calculate total taxa count by red list category.
        ///// </summary>
        ///// <param name="analysisManager">Analysis manager.</param>
        ///// <param name="redListTaxonCategories"></param>
        ///// <returns>Red listed taxa count.</returns>
        //public static long GetTotalRedListTaxaCount(this IAnalysisManager analysisManager, List<string> redListTaxonCategories)
        //{
        //    var criteria = new AnalysisSearchCriteria();

        //    if (redListTaxonCategories.IsNotNull() && redListTaxonCategories.Count > 0)
        //    {
        //        criteria.TaxonCategories = redListTaxonCategories;
        //    }

        //    criteria.RedListCategories = new List<int>();

        //    var categories = RedListedHelper.GetRedListCategoriesDdToNt();
        //    foreach (var cat in categories)
        //    {
        //        criteria.RedListCategories.Add((int)cat);
        //    }

        //    var result = GetTaxonIds(analysisManager, criteria);
        //    if (result.IsNotEmpty())
        //    {
        //        return result.Count;
        //    }

        //    return 0;
        //}

        /// <summary>
        /// Init analysis cache.
        /// </summary>
        /// <param name="analysisManager">Analysis manager.</param>
        /// <param name="userContext">The user context.</param>
        public static void InitAnalysisCache(this IAnalysisManager analysisManager, IUserContext userContext)
        {
            mCache = null;
            string cacheFileName = GetCacheFileName();

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
                        mCache = (AnalysisCache)formatter.Deserialize(stream);
                        stream.Close();
                        if (!mCache.IsOk())
                        {
                            mCache = null;
                        }
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }

            if (mCache.IsNull())
            {
                //Logger.Debug("Invalid definition of AnalysisCache, start updating...");
                UpdateCache();
                //Logger.Debug("AnalysisCache updated...");
            }

            if (!Configuration.Debug)
            {
                // Start cache update thread.
                var thread = new Thread(StartCacheUpdate);
                thread.Start();
            }
        }

        /// <summary>
        /// Start automatic update of analysis cache.
        /// </summary>
        private static void StartCacheUpdate()
        {
            while (!mStopUpdate)
            {
                if (mCache.IsNotNull() &&
                    ((mCache.CachedDate + new TimeSpan(0, 12, 0, 0)) < DateTime.Now) &&
                    (DateTime.Now.TimeOfDay.Hours == 2))
                {
                    // Update cached information.
                    //Logger.Debug("Start automatic update of analysis cache and taxoninformation cache...");
                    CacheManager.FireRefreshCache(CoreData.UserManager.GetApplicationContext());
                    UpdateCache();
                    //TaxonListInformationManager.Instance.UpdateCache();
                    
                    //TaxonNameSearchManager.Instance.UpdateCache();
                    //Logger.Debug("Automatic update of analysis cache, taxoninformation cache and taxonnamesearch cache completed...");
                }

                // Wait a minute...
                Thread.Sleep(60000);
            }
        }

        /// <summary>
        /// Stop automatic analysis cache update.
        /// </summary>
        /// <param name="analysisManager">Analysis manager.</param>
        public static void StopCacheUpdate(this IAnalysisManager analysisManager)
        {
            mStopUpdate = true;
        }

        /// <summary>
        /// Update analysis cache.
        /// </summary>
        private static void UpdateCache()
        {
            var cache = new AnalysisCache();
            cache.Init(CoreData.UserManager.GetApplicationContext());
            mCache = cache;

            try
            {
                // Save cache to file.
                string cacheFileName = GetCacheFileName();
                if (File.Exists(cacheFileName))
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    File.Delete(cacheFileName);
                }

                FileSystemManager.EnsureFolderExists(cacheFileName);
                using (FileStream stream = File.Open(
                    cacheFileName,
                    FileMode.OpenOrCreate,
                    FileAccess.Write,
                    FileShare.None))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, mCache);
                    stream.Flush(true);
                    stream.Close();
                }

                Logger.WriteMessage("Analysis redlist cache updated");
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }
        }
    }
}