using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// This interface contains handling of taxon related objects.
    /// </summary>
    public interface ITaxonManager
    {
        /// <summary>
        /// The taxon related information.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon related information.</returns>
        Dictionary<Int32, TaxonInformation> GetTaxonInformation(WebServiceContext context);


        /// <summary>
        /// Get ids for requested taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="includeChildTaxa">Include child taxa in returned taxa.</param>
        /// <returns>Ids for requested taxa.</returns>
        List<Int32> GetTaxonIds(WebServiceContext context, WebSpeciesObservationSearchCriteria searchCriteria, Boolean includeChildTaxa);


        /// <summary>
        /// Get taxon ids for red listed taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="includeRedlistedTaxa">If true all red listed taxa should be returned.</param>
        /// <param name="redlistCategories">Taxa belonging to specified red list categories should be returned.</param>
        /// <returns>Requested red listed taxa.</returns>
        List<Int32> GetRedlistedTaxonIds(WebServiceContext context,
                                            Boolean includeRedlistedTaxa,
                                            List<RedListCategory> redlistCategories);

    }
}
