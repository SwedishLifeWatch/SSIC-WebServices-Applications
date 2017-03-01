using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService
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
        public static SpeciesObservationServer GetSpeciesObservationDatabase(this WebServiceContext context)
        {
            return (SpeciesObservationServer)(context.GetDatabase());
        }
    }
}
