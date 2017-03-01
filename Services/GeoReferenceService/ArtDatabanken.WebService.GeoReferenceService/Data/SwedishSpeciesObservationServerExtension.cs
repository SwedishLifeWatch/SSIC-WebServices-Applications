using ArtDatabanken.WebService.GeoReferenceService.Database;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class SwedishSpeciesObservationServerExtension
    {
        /// <summary>
        /// Get species observation database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation database.</returns>
        public static SwedishSpeciesObservationServer GetSpeciesObservationDatabase(this WebServiceContext context)
        {
            return new SwedishSpeciesObservationServer();
        }
    }
}
