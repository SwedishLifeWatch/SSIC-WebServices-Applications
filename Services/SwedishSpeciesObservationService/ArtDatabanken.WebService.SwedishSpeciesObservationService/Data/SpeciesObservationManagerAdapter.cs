using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Modify behaviour of the base class.
    /// </summary>
    public class SpeciesObservationManagerAdapter : WebService.Data.SpeciesObservationManager
    {
        /// <summary>
        /// Get all county regions
        /// </summary>
        /// <param name="context"></param>
        /// <returns>All county regions</returns>
        public override List<WebRegion> GetCountyRegions(WebServiceContext context)
        {
            return SpeciesObservationManager.GetCountyRegions(context);
        }

        /// <summary>
        /// Get all province regions
        /// </summary>
        /// <param name="context"></param>
        /// <returns>All county regions</returns>
        public override List<WebRegion> GetProvinceRegions(WebServiceContext context)
        {
            return SpeciesObservationManager.GetProvinceRegions(context);
        }
    }
}
