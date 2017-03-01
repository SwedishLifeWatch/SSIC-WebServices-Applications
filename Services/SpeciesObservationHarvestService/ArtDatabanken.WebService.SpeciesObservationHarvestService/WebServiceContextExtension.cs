using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService
{
    /// <summary>
    /// Extension methods to the WebServiceContext class.
    /// </summary>
    public static class WebServiceContextExtension
    {
        /// <summary>
        /// Get species observation database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation database.</returns>
        public static SpeciesObservationHarvestServer GetSpeciesObservationDatabase(this WebServiceContext context)
        {
            return (SpeciesObservationHarvestServer)(context.GetDatabase());
        }
    }
}
