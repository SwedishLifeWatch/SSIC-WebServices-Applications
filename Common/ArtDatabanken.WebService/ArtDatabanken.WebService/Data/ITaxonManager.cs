using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Definition of the TaxonManager interface.
    /// </summary>
    public interface ITaxonManager
    {
        /// <summary>
        /// Get dictionary where taxon id is key.
        /// </summary>
        /// <param name="taxa">Taxa that should be inserted into dictionary.</param>
        /// <returns>Dictionary where taxon id is key.</returns>
        Dictionary<Int32, WebTaxon> GetDictionary(List<WebTaxon> taxa);

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
        List<Int32> GetChildTaxonIds(WebServiceContext context,
                                     List<Int32> parentTaxonIds);

        /// <summary>
        /// Get taxa that belongs to authority.
        /// All child taxa are also included.
        /// Taxon id is key in dictionary.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>Taxa that belongs to authority.</returns>
        Dictionary<Int32, WebTaxon> GetTaxaByAuthority(WebServiceContext context,
                                                       WebAuthority authority);

        /// <summary>
        /// Get taxa mathching a list of GUIDS.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonGuids"></param>
        /// <returns>All taxoncategories.</returns>
        List<WebTaxon> GetTaxaByGUIDs(WebServiceContext context, List<String> taxonGuids);
        
        /// <summary>
        /// Get all taxoncategories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All taxoncategories.</returns>
        List<WebTaxonCategory> GetTaxonCategories(WebServiceContext context);

        /// <summary>
        /// Get all taxon changes.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>All taxoncategories.</returns>
        List<WebTaxonChange> GetTaxonChange(WebServiceContext context, DateTime fromDate, DateTime toDate);
        
         /// <summary>
        /// Get taxa matching a list of Ids.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonIds"></param>
        /// <returns>Taxa with Ids avaliable in the input list of Ids.</returns>
        List<WebTaxon> GetTaxaByIds(WebServiceContext context, List<int> taxonIds);

        /// <summary>
        /// Get taxa that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Taxon search criteria.</param>
        /// <returns>Taxa with Ids avaliable in the input list of Ids.</returns>
        List<WebTaxon> GetTaxaBySearchCriteria(WebServiceContext context, WebTaxonSearchCriteria searchCriteria);

        /// <summary>
        /// Get Lump Split information for a Taxon
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId"></param>
        /// <returns>List of LumpSplitEvent information</returns>
        List<WebLumpSplitEvent> GetLumpSplitEventsByOldReplacedTaxon(WebServiceContext context, int taxonId);

        /// <summary>
        /// Get information about possbile status for taxon names.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about possbile status for taxon names.</returns>
        List<WebTaxonNameStatus> GetTaxonNameStatuses(WebServiceContext context);

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// All taxon tree nodes without parents are returned
        /// if no taxon ids are specified.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(WebServiceContext context,
                                                             WebTaxonTreeSearchCriteria searchCriteria);

        /// <summary>
        /// Get taxon names that matches search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon names.</returns>
        List<WebTaxonName> GetTaxonNamesBySearchCriteria(WebServiceContext context,
                                                         WebTaxonNameSearchCriteria searchCriteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="taxon"></param>
        /// <returns></returns>
        String GetTaxonConceptDefinition(WebServiceContext context, WebTaxon taxon);
    }
}
