using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Definition of the SpeciesFactManager interface.
    /// </summary>
    public interface ISpeciesFactManager
    {
        /// <summary>
        /// Get information about species facts that matches search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        List<WebSpeciesFact> GetSpeciesFactsBySearchCriteria(WebServiceContext context,
                                                             WebSpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get taxa count of taxa that matches search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa count of taxa that matches search criteria.</returns>
        Int32 GetTaxaCountBySearchCriteria(WebServiceContext context, WebSpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get taxa that matches fact search criteria.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa that matches fact search criteria.</returns>
        List<WebTaxon> GetTaxaBySearchCriteria(WebServiceContext context, WebSpeciesFactSearchCriteria searchCriteria);
    }
}
