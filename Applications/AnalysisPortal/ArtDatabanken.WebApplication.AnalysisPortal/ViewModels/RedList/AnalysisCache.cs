using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Cache with information that is used when taxa are search
    /// and when data for analysis views are created.
    /// </summary>    
    [Serializable]
    public class AnalysisCache
    {
        /// <summary>
        /// The current cache version.
        /// </summary>
        // ReSharper disable InconsistentNaming
        private const string CURRENT_VERSION = "2015-05-28";
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Date and time when information was last read from data source.
        /// </summary>    
        public DateTime CachedDate { get; private set; }

        ///// <summary>
        ///// Cached information about biotope.
        ///// </summary>    
        //private BiotopeCache Biotope { get; set; }

        ///// <summary>
        ///// Cached information about swedish occurrence.
        ///// </summary>
        //private SwedishOccurrenceCache SwedishOccurrence { get; set; }

        ///// <summary>
        ///// Cached information about county occurrence.
        ///// </summary>    
        //private CountyOccurrenceCache CountyOccurrence { get; set; }

        ///// <summary>
        ///// Cached information about host.
        ///// </summary>    
        //private HostCache Host { get; set; }

        ///// <summary>
        ///// Cached information about impact.
        ///// </summary>    
        //private ImpactCache Impact { get; set; }

        ///// <summary>
        ///// Cached information about landscape type.
        ///// </summary>    
        //private LandscapeTypeCache LandscapeType { get; set; }

        ///// <summary>
        ///// Cached information about life form.
        ///// </summary>    
        //private LifeFormCache LifeForm { get; set; }

        ///// <summary>
        ///// Cached information about organism group 1.
        ///// </summary>    
        //private OrganismGroupCache OrganismGroup { get; set; }

        /// <summary>
        /// Cached information about red list category.
        /// </summary>    
        private RedListCategoryCache RedListCategory { get; set; }

        ///// <summary>
        ///// Cached information about taxon scope.
        ///// </summary>    
        //private TaxonScopeCache TaxonScope { get; set; }

        ///// <summary>
        ///// Cached information about substrate.
        ///// </summary>    
        //private SubstrateCache Substrate { get; set; }

        ///// <summary>
        ///// Cached information about thematic lists.
        ///// </summary>
        //private ThematicListCache ThematicLists { get; set; }

        /// <summary>
        /// Cache handling version.
        /// </summary>    
        private string Version { get; set; }

        /// <summary>
        /// Get ids for taxa that matches search criteria.
        /// </summary>
        /// <param name="searchCriteria">Analysis search criteria.</param>
        /// <param name="taxonIds">
        /// Limit search to these taxon ids.
        /// This parameter is ignored if value is null.
        /// </param>
        /// <returns>Ids for taxa that matches search criteria.</returns>
        public TaxonIdList GetTaxonIds(AnalysisSearchCriteria searchCriteria, TaxonIdList taxonIds = null)
        {
            TaxonIdList tempTaxonIds;

            tempTaxonIds = null;
            if (taxonIds.IsNotNull())
            {
                tempTaxonIds = new TaxonIdList();
                tempTaxonIds.AddRange(taxonIds);
            }

            //if (searchCriteria.Biotopes.IsNotNull())
            //{
            //    tempTaxonIds = Biotope.GetTaxonIds(searchCriteria.Biotopes,
            //                                       tempTaxonIds);
            //}

            //if (searchCriteria.SwedishOccurrence.IsNotNull())
            //{
            //    tempTaxonIds = SwedishOccurrence.GetTaxonIds(searchCriteria.SwedishOccurrence,
            //                                                 tempTaxonIds);
            //}

            //if (searchCriteria.CountyOccurrence.IsNotNull())
            //{
            //    tempTaxonIds = CountyOccurrence.GetTaxonIds(searchCriteria.CountyOccurrence,
            //                                                tempTaxonIds);
            //}

            //if (searchCriteria.Host.IsNotNull())
            //{
            //    tempTaxonIds = Host.GetTaxonIds(searchCriteria.Host,
            //                                    tempTaxonIds);
            //}

            //if (searchCriteria.Impact.IsNotNull())
            //{
            //    tempTaxonIds = Impact.GetTaxonIds(searchCriteria.Impact,
            //                                      tempTaxonIds);
            //}

            //if (searchCriteria.LandscapeTypes.IsNotNull())
            //{
            //    tempTaxonIds = LandscapeType.GetTaxonIds(searchCriteria.LandscapeTypes,
            //                                             tempTaxonIds);
            //}

            //if (searchCriteria.LifeForms.IsNotNull())
            //{
            //    tempTaxonIds = LifeForm.GetTaxonIds(searchCriteria.LifeForms,
            //                                        tempTaxonIds);
            //}

            //if (searchCriteria.OrganismGroups.IsNotNull())
            //{
            //    tempTaxonIds = OrganismGroup.GetTaxonIds(searchCriteria.OrganismGroups,
            //                                             tempTaxonIds);
            //}

            if (searchCriteria.RedListCategories.IsNotNull())
            {
                tempTaxonIds = RedListCategory.GetTaxonIds(searchCriteria.RedListCategories, tempTaxonIds);
            }

            //if (searchCriteria.TaxonCategories.IsNotEmpty())
            //{
            //    tempTaxonIds = TaxonScope.GetTaxonIds(searchCriteria.TaxonCategories,
            //                                                    tempTaxonIds);
            //}
            //else if (searchCriteria.TaxonScope.IsNotNull())
            //{
            //    tempTaxonIds = TaxonScope.GetTaxonIds(searchCriteria.TaxonScope,
            //                                                    tempTaxonIds);
            //}

            //if (searchCriteria.Substrate.IsNotNull())
            //{
            //    tempTaxonIds = Substrate.GetTaxonIds(searchCriteria.Substrate,
            //                                         tempTaxonIds);
            //}

            //if (searchCriteria.ThematicLists.IsNotNull())
            //{
            //    tempTaxonIds = ThematicLists.GetTaxonIds(searchCriteria.ThematicLists,
            //                                             tempTaxonIds);
            //}

            return tempTaxonIds;
        }

        ///// <summary>
        ///// Get ids for taxa that matches scope criteria.
        ///// </summary>
        ///// <param name="searchCriteria">Analysis search criteria.</param>
        ///// <returns>Ids for taxa that matches selected scope/search criteria.</returns>
        //public TaxonIdList GetTaxonIdsByScope(AnalysisSearchCriteria searchCriteria)
        //{
        //    TaxonIdList tempTaxonIds = null;
        //    if (searchCriteria.TaxonScope.IsNotNull())
        //    {
        //        tempTaxonIds = TaxonScope.GetTaxonIds(searchCriteria.TaxonScope, null);
        //    }

        //    return tempTaxonIds;
        //}

        /// <summary>
        /// Gets the redlistcategory for a specific taxon
        /// </summary>
        /// <param name="taxonId"></param>
        /// <param name="redListCategory"></param>
        /// <returns></returns>
        public bool GetRedListCategoryForTaxonId(int taxonId, out RedListCategory redListCategory)
        {
            return RedListCategory.GetRedListCategoryForTaxonId(taxonId, out redListCategory);
        }

        /// <summary>
        /// Test if cache has been initialized with correct data.
        /// </summary>
        /// <returns>True, if cache has been initialized with correct data.</returns>
        public bool IsOk()
        {
            return Version.IsNotEmpty() &&
                   (Version == CURRENT_VERSION);
        }

        /// <summary>
        /// Initialize analysis cache.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        public void Init(IUserContext userContext)
        {
            CachedDate = DateTime.Now;
            //Biotope = new BiotopeCache(userContext);
            //Biotope.Init(userContext);
            //SwedishOccurrence = new SwedishOccurrenceCache(userContext);
            //SwedishOccurrence.Init(userContext);
            //CountyOccurrence = new CountyOccurrenceCache(userContext);
            //CountyOccurrence.Init(userContext);
            //Host = new HostCache(userContext);
            //Host.Init(userContext);
            //Impact = new ImpactCache(userContext);
            //Impact.Init(userContext);
            //LandscapeType = new LandscapeTypeCache(userContext);
            //LandscapeType.Init(userContext);
            //LifeForm = new LifeFormCache(userContext);
            //LifeForm.Init(userContext);
            //OrganismGroup = new OrganismGroupCache(userContext);
            //OrganismGroup.Init(userContext);
            RedListCategory = new RedListCategoryCache();
            RedListCategory.Init(userContext);
            //TaxonScope = new TaxonScopeCache();
            //TaxonScope.Init(userContext);
            //Substrate = new SubstrateCache(userContext);
            //Substrate.Init(userContext);
            //ThematicLists = new ThematicListCache();
            //ThematicLists.Init(userContext);
            Version = CURRENT_VERSION;
        }
    }
}
