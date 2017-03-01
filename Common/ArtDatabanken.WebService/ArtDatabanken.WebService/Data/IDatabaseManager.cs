using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Interface to database related functionality.
    /// </summary>
    public interface IDatabaseManager
    {
        /// <summary>
        /// Get database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>A database instance.</returns>
        WebServiceDataServer GetDatabase(WebServiceContext context);

        /// <summary>
        /// Get an elasticsearch proxy instance.
        /// </summary>
        /// <returns>An elasticsearch proxy instance.</returns>
        ElasticsearchProxy GetElasticSearchProxy();
    }
}
