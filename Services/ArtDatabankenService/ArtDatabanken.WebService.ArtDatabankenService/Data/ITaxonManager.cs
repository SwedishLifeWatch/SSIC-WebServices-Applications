using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Interface for handling of taxa information.
    /// </summary>
    public interface ITaxonManager
    {
        /// <summary>
        /// Get information about taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonIds">Ids for taxa to get information about.</param>
        /// <returns>Taxa information.</returns>
        List<WebTaxon> GetTaxaByIds(WebServiceContext context,
                                    List<Int32> taxonIds);

        /// <summary>
        /// Get information about a taxon.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <returns>Taxon information.</returns>
        WebTaxon GetTaxon(WebServiceContext context, Int32 taxonId);
    }
}
