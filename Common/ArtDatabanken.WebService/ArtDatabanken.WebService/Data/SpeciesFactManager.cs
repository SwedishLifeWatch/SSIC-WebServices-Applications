using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Implementation of the SpeciesFactManager class.
    /// </summary>
    public class SpeciesFactManager : ManagerBase, ISpeciesFactManager
    {
        /// <summary>
        /// Get information about species facts that matches search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        public virtual List<WebSpeciesFact> GetSpeciesFactsBySearchCriteria(WebServiceContext context,
                                                                            WebSpeciesFactSearchCriteria searchCriteria)
        {
            WebClientInformation clientInformation;
            
            clientInformation = GetClientInformation(context, WebServiceId.TaxonAttributeService);
            return WebServiceProxy.TaxonAttributeService.GetSpeciesFactsBySearchCriteria(clientInformation,
                                                                                         searchCriteria);
        }

        /// <summary>
        /// Get taxa count of taxa that matches search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa count of taxa that matches search criteria.</returns>
        public virtual Int32 GetTaxaCountBySearchCriteria(WebServiceContext context, WebSpeciesFactSearchCriteria searchCriteria)
        {
            WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonAttributeService);
            return WebServiceProxy.TaxonAttributeService.GetTaxaCountBySearchCriteria(clientInformation, searchCriteria);
        }

        /// <summary>
        /// Get taxa that matches fact search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa that matches fact search criteria.</returns>
        public virtual List<WebTaxon> GetTaxaBySearchCriteria(WebServiceContext context, WebSpeciesFactSearchCriteria searchCriteria)
        {
            WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.TaxonAttributeService);
            return WebServiceProxy.TaxonAttributeService.GetTaxaBySearchCriteria(clientInformation, searchCriteria);
        }
    }
}
