using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class that handles taxa related information.
    /// </summary>
    public class TaxonManager : ManagerBase, ITaxonManager
    {
        /// <summary>
        /// Get all child taxa.
        /// Parent taxa are also included in the result.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="parentTaxonGuids">GUIDs for parent taxa.</param>
        /// <returns>All child taxa.</returns>
        private List<WebTaxon> GetChildTaxaByGuids(WebServiceContext context,
                                                   List<String> parentTaxonGuids)
        {
            List<Int32> parentTaxonIds;
            WebTaxonSearchCriteria searchCriteria;

            // Convert taxon GUIDs to taxon ids.
            parentTaxonIds = new List<Int32>();
            if (parentTaxonGuids.IsNotEmpty())
            {
                foreach (String parentTaxonGuid in parentTaxonGuids)
                {
                    // TODO: This assumption about taxon GUIDs may
                    // change in the future.
                    parentTaxonIds.Add(parentTaxonGuid.WebParseInt32());
                }
            }

            // Create search criteria.
            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.IsIsValidTaxonSpecified = true;
            searchCriteria.IsValidTaxon = true;
            searchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
            searchCriteria.TaxonIds = parentTaxonIds;

            // Get child taxa.
            return GetTaxaBySearchCriteria(context, searchCriteria);
        }

        /// <summary>
        /// Get child taxon ids.
        /// Parent taxon ids are included in the result.
        /// Only valid taxon relations and child taxa are included.
        /// This method only works in web services that
        /// handles species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="parentTaxonIds">Parent taxon ids.</param>
        /// <returns>Child taxon ids.</returns>
        public virtual List<Int32> GetChildTaxonIds(WebServiceContext context,
                                                    List<Int32> parentTaxonIds)
        {
            DataIdInt32List childTaxonIds;
            Dictionary<Int32, DataIdInt32List> taxonTreeRelations;

            childTaxonIds = new DataIdInt32List();
            if (parentTaxonIds.IsNotEmpty())
            {
                taxonTreeRelations = GetTaxonTreeRelations(context);
                foreach (Int32 taxonId in parentTaxonIds)
                {
                    if (taxonTreeRelations.ContainsKey(taxonId))
                    {
                        childTaxonIds.Merge(taxonTreeRelations[taxonId]);
                    }
                    else
                    {
                        childTaxonIds.Merge(taxonId);
                    }
                }
            }

            return childTaxonIds.GetInt32List();
        }

        /// <summary>
        /// Get dictionary where taxon id is key.
        /// </summary>
        /// <param name="taxa">Taxa that should be inserted into dictionary.</param>
        /// <returns>Dictionary where taxon id is key.</returns>
        public virtual Dictionary<Int32, WebTaxon> GetDictionary(List<WebTaxon> taxa)
        {
            Dictionary<Int32, WebTaxon> taxonDictionary;

            taxonDictionary = new Dictionary<Int32, WebTaxon>();
            if (taxa.IsNotEmpty())
            {
                foreach (WebTaxon taxon in taxa)
                {
                    if (!taxonDictionary.ContainsKey(taxon.Id))
                    {
                        taxonDictionary[taxon.Id] = taxon;
                    }
                }
            }

            return taxonDictionary;
        }

        /// <summary>
        /// Get taxa that belongs to authority.
        /// All child taxa are also included.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>Taxa that belongs to authority.</returns>
        public virtual Dictionary<Int32, WebTaxon> GetTaxaByAuthority(WebServiceContext context,
                                                                      WebAuthority authority)
        {
            Dictionary<Int32, WebTaxon> taxonDictionary;
            List<WebTaxon> taxa;
            String taxaInAuthorityCacheKey;

            // Get cached information.
            taxaInAuthorityCacheKey = Settings.Default.TaxaInAuthorityCacheKey +
                                      Settings.Default.CacheKeyDelimiter +
                                      authority.Id;
            taxonDictionary = (Dictionary<Int32, WebTaxon>)(context.GetCachedObject(taxaInAuthorityCacheKey));

            if (taxonDictionary.IsNull())
            {
                // Get taxa from taxon service.
                if (authority.TaxonGUIDs.IsEmpty())
                {
                    taxonDictionary = new Dictionary<Int32, WebTaxon>();
                }
                else
                {
                    taxa = GetChildTaxaByGuids(context, authority.TaxonGUIDs);
                    taxonDictionary = GetDictionary(taxa);
                }

                // Add information to cache.
                context.AddCachedObject(taxaInAuthorityCacheKey,
                                        taxonDictionary,
                                        DateTime.Now + new TimeSpan(0, 1, 0, 0),
                                        CacheItemPriority.BelowNormal);
            }

            return taxonDictionary;
        }

        /// <summary>
        /// Get taxa avaliable in a list of GUIDS.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonGuids"></param>
        /// <returns>All taxa with GUIDs avaliable in the list of GUIDs.</returns>
        public virtual List<WebTaxon> GetTaxaByGUIDs(WebServiceContext context,
                                                     List<String> taxonGuids)
        {
            WebClientInformation clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            List<WebTaxon> taxa = null;
            List<Int32> taxonIds = new List<Int32>();
            foreach (String taxonGuid in taxonGuids)
            {
                LSID lsid = new LSID(taxonGuid);

                int taxonId = Convert.ToInt32(lsid.ObjectID);
                taxonIds.Add(taxonId);
               
            }
            if(taxonIds.IsNotEmpty())
            {
                taxa = WebServiceProxy.TaxonService.GetTaxaByIds(clientInformation, taxonIds);
            }
            
            return taxa; 
        }
        
        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxoncategories.</returns>
        public List<WebTaxonCategory> GetTaxonCategories(WebServiceContext context)
        {
            WebClientInformation clientInformation;
            clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            return WebServiceProxy.TaxonService.GetTaxonCategories(clientInformation);
        }

        /// <summary>
        /// Get all taxon changes.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>All taxoncategories.</returns>
        public List<WebTaxonChange> GetTaxonChange(WebServiceContext context, DateTime fromDate, DateTime toDate)
        {
            WebClientInformation clientInformation;
            clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            return WebServiceProxy.TaxonService.GetTaxonChange(clientInformation, 0, false, fromDate, toDate);
        }

        /// <summary>
        /// Get taxa matching a list of Ids.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonIds"></param>
        /// <returns>Taxa with Ids avaliable in the input list of Ids.</returns>
        public virtual List<WebTaxon> GetTaxaByIds(WebServiceContext context, List<int> taxonIds)
        {
            WebClientInformation clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            List<WebTaxon> taxa = new List<WebTaxon>();
           
            if (taxonIds.IsNotEmpty())
            {
                taxa = WebServiceProxy.TaxonService.GetTaxaByIds(clientInformation, taxonIds);
            }

            return taxa;
        }

        /// <summary>
        /// Get taxa by SearchCriteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>List of taxons.</returns> 
        public virtual List<WebTaxon> GetTaxaBySearchCriteria(WebServiceContext context, WebTaxonSearchCriteria searchCriteria)
        {
            WebClientInformation clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            List<WebTaxon> taxa = new List<WebTaxon>();
           
            if (searchCriteria.IsNotNull())
            {
                taxa = WebServiceProxy.TaxonService.GetTaxaBySearchCriteria(clientInformation, searchCriteria);
            }

            return taxa;
        }
        

        /// <summary>
        /// Get Lump Split information for a Taxon
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId"></param>
        /// <returns>List of LumpSplitEvent information</returns>
        public virtual List<WebLumpSplitEvent> GetLumpSplitEventsByOldReplacedTaxon(WebServiceContext context, int taxonId)
        {
            WebClientInformation clientInformation = GetClientInformation(context, WebServiceId.TaxonService);

            List<WebLumpSplitEvent> taxa = WebServiceProxy.TaxonService.GetLumpSplitEventsByOldReplacedTaxon(clientInformation, taxonId);

            return taxa;
        }

        /// <summary>
        /// Get information about possbile status for taxon names.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about possbile status for taxon names.</returns>
        public List<WebTaxonNameStatus> GetTaxonNameStatuses(WebServiceContext context)
        {
            WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            return WebServiceProxy.TaxonService.GetTaxonNameStatuses(clientInformation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webTaxon"></param>
        /// <returns>Information about possbile status for taxon names.</returns>
        public String GetTaxonConceptDefinition(WebServiceContext context, WebTaxon webTaxon)
        {
            WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            return WebServiceProxy.TaxonService.GetTaxonConceptDefinition(clientInformation, webTaxon);
        }

        /// <summary>
        /// The taxon tree relations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon tree relations.</returns>
        private Dictionary<Int32, DataIdInt32List> GetTaxonTreeRelations(WebServiceContext context)
        {
            String cacheKey;
            Dictionary<Int32, DataIdInt32List> taxonTreeRelations;
            Int32 childTaxonId, parentTaxonId;

            // Get cached information.
            cacheKey = Settings.Default.TaxonTreeRelationCacheKey;
            taxonTreeRelations = (Dictionary<Int32, DataIdInt32List>)(context.GetCachedObject(cacheKey));

            if (taxonTreeRelations.IsEmpty())
            {
                // Data not in cache. Get information from database.
                taxonTreeRelations = new Dictionary<Int32, DataIdInt32List>();
                using (DataReader dataReader = context.GetDatabase().GetTaxonTree())
                {
                    while (dataReader.Read())
                    {
                        childTaxonId = dataReader.GetInt32("ChildTaxonId");
                        parentTaxonId = dataReader.GetInt32("ParentTaxonId");
                        if (!taxonTreeRelations.ContainsKey(parentTaxonId))
                        {
                            taxonTreeRelations[parentTaxonId] = new DataIdInt32List();
                        }

                        taxonTreeRelations[parentTaxonId].Add(childTaxonId);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonTreeRelations,
                                        DateTime.Now + new TimeSpan(0, 12, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonTreeRelations;
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// All taxon tree nodes without parents are returned
        /// if no taxon ids are specified.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        public List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(WebServiceContext context,
                                                                    WebTaxonTreeSearchCriteria searchCriteria)
        {
            WebClientInformation clientInformation;
            clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            return WebServiceProxy.TaxonService.GetTaxonTreesBySearchCriteria(clientInformation, searchCriteria);
        }

        /// <summary>
        /// Get taxon names that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon names.</returns>
        public List<WebTaxonName> GetTaxonNamesBySearchCriteria(WebServiceContext context,
                                                                WebTaxonNameSearchCriteria searchCriteria)
        {
            WebClientInformation clientInformation;
            clientInformation = GetClientInformation(context, WebServiceId.TaxonService);
            return WebServiceProxy.TaxonService.GetTaxonNamesBySearchCriteria(clientInformation, searchCriteria);
        }
    }
}
