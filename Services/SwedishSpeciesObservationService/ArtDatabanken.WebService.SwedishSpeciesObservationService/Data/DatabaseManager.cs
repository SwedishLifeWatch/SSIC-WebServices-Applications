using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Class that handles information related to 
    /// the web service that this project is included into.
    /// </summary>
    public class DatabaseManager : IDatabaseManager
    {
        /// <summary>
        /// Get database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>A database instance.</returns>
        public WebServiceDataServer GetDatabase(WebServiceContext context)
        {
            return new SpeciesObservationServer();
        }

        /// <summary>
        /// Get an elasticsearch proxy instance.
        /// </summary>
        /// <returns>An elasticsearch proxy instance.</returns>
        public ElasticsearchProxy GetElasticSearchProxy()
        {
            return new ElasticsearchSpeciesObservationProxy(SpeciesObservation.Settings.Default.IndexNameElasticsearch);
        }
    }
}
