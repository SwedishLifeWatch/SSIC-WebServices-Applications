using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// Extension methods to the IDatabaseManager interface.
    /// </summary>
    public static class IDatabaseManagerExtension
    {
        /// <summary>
        /// Get an elasticsearch proxy instance.
        /// </summary>
        /// <param name="databaseManager">Database manager.</param>
        /// <returns>An elasticsearch proxy instance.</returns>
        public static ElasticsearchSpeciesObservationProxy GetElastisearchSpeciesObservationProxy(this IDatabaseManager databaseManager)
        {
            return (ElasticsearchSpeciesObservationProxy)(WebServiceData.DatabaseManager.GetElasticSearchProxy());
        }

    }
}
