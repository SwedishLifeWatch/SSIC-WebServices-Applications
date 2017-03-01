using ArtDatabanken.WebService.SpeciesObservation.Database;

namespace ArtDatabanken.WebService.SpeciesObservation
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
        public static SpeciesObservationServerBase GetSpeciesObservationDatabase(this WebServiceContext context)
        {
            return (SpeciesObservationServerBase)(context.GetDatabase());
        }
    }
}
