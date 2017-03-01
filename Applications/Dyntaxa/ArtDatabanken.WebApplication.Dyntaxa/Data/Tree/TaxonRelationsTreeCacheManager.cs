using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Tree
{
    /// <summary>
    /// Taxon relations cache manager.
    /// </summary>
    public static class TaxonRelationsTreeCacheManager
    {                
        /// <summary>
        /// Taxon relation list.
        /// </summary>
        public static TaxonRelationList TaxonRelationList { get; set; }

        /// <summary>
        /// The cached taxon relation tree.
        /// </summary>
        public static TaxonRelationsTree CachedTaxonRelationTree { get; set; }

        /// <summary>
        /// The cached taxon relation tree last updated time.
        /// </summary>
        public static DateTime CacheLastUpdatedTime { get; set; }

        /// <summary>
        /// The locking target.
        /// </summary>
        private static readonly object LockingTarget = new object();

        /// <summary>
        /// Updates the cache by getting all relations and all taxa from Taxon service 
        /// and create a TaxonRelationTree.
        /// This method is thread safe.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        public static void UpdateCache(IUserContext userContext)
        {
            lock (LockingTarget)
            {
                Debug.WriteLine("Start update Cache {0}", DateTime.Now);
                ITaxonSearchCriteria taxonSearchCriteria = new TaxonSearchCriteria();
                var allTaxa = CoreData.TaxonManager.GetTaxa(userContext, taxonSearchCriteria);

                TaxonRelationSearchCriteria searchCriteria = new TaxonRelationSearchCriteria();
                TaxonRelationList allRelations = CoreData.TaxonManager.GetTaxonRelations(userContext, searchCriteria);
                TaxonRelationList = allRelations;
                CachedTaxonRelationTree = TaxonRelationsTreeManager.CreateTaxonRelationsTree(
                    userContext, 
                    allRelations,
                    allTaxa);
                CacheLastUpdatedTime = DateTime.Now;
                Debug.WriteLine("End update Cache {0}", DateTime.Now);
                DyntaxaLogger.WriteMessage("DyntaxaTree updated {0}", DateTime.Now);
            }
        }     
    }
}
