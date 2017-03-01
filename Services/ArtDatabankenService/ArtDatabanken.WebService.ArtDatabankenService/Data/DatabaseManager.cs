using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Caching;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Manager of meta information about databases.
    /// </summary>
    public class DatabaseManager
    {
        /// <summary>
        /// Check that the database is not updating right now.
        /// This only applies to observationdatabas.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <exception cref="ApplicationException">Thrown if database is beeing updated.</exception>
        public static void CheckDatabaseUpdate(WebServiceContext context)
        {
            DateTime now;
            WebDatabaseUpdate databaseUpdate;

            now = DateTime.Now;
            databaseUpdate = GetDatabaseUpdate(context);
            if ((databaseUpdate.UpdateStart.Hour <= now.Hour) &&
                (now.Hour <= databaseUpdate.UpdateEnd.Hour) &&
                (databaseUpdate.UpdateStart.Minute <= now.Minute) &&
                (now.Minute <= databaseUpdate.UpdateEnd.Minute))
            {
                throw new ApplicationException("Database is beeing updated!");
            }
        }

        /// <summary>
        /// Get information about databases.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about databases.</returns>
        public static List<WebDatabase> GetDatabases(WebServiceContext context)
        {
            List<WebDatabase> databases;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllDatabases";
            databases = (List<WebDatabase>)context.GetCachedObject(cacheKey);

            if (databases.IsNull())
            {
                // Get information from database.
                databases = new List<WebDatabase>();
                using (DataReader dataReader = DataServer.GetDatabases(context))
                {
                    while (dataReader.Read())
                    {
                        databases.Add(new WebDatabase(dataReader));
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, databases, DateTime.Now + new TimeSpan(24, 0, 0), CacheItemPriority.BelowNormal);
                }
            }
            return databases;
        }

        /// <summary>
        /// Get information about database update.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Database update information.</returns>
        public static WebDatabaseUpdate GetDatabaseUpdate(WebServiceContext context)
        {
            String cacheKey;
            WebDatabaseUpdate databaseUpdate;

            // Get cached information.
            cacheKey = "DatabaseUpdate";
            databaseUpdate = (WebDatabaseUpdate)context.GetCachedObject(cacheKey);

            if (databaseUpdate.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = DataServer.GetDatabaseUpdate(context))
                {
                    if (dataReader.Read())
                    {
                        databaseUpdate = new WebDatabaseUpdate(dataReader);

                        // Add information to cache.
                        context.AddCachedObject(cacheKey, databaseUpdate, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.BelowNormal);
                    }
                    else
                    {
                        throw new ApplicationException("Could not get database update information!");
                    }
                }
            }
            return databaseUpdate;
        }
    }
}
