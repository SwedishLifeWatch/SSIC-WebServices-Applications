using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.ReferenceService.Database;

namespace ArtDatabanken.WebService.ReferenceService.Data
{
    /// <summary>
    /// Database related functionality.
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
            return new ReferenceServer();
        }

        /// <summary>
        /// Get an elasticsearch proxy instance.
        /// </summary>
        /// <returns>An elasticsearch proxy instance.</returns>
        public ElasticsearchProxy GetElasticSearchProxy()
        {
            throw new NotImplementedException();
        }
    }
}
