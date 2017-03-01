using System;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class contains handling of database information objects.
    /// </summary>
    public class DatabaseManager : ManagerBase
    {
        private static DatabaseList _databases;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static DatabaseManager()
        {
            RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _databases thread safe.
        /// </summary>
        private static DatabaseList Databases
        {
            get
            {
                DatabaseList databases;

                lock (_lockObject)
                {
                    databases = _databases;
                }
                return databases;
            }
            set
            {
                lock (_lockObject)
                {
                    _databases = value;
                }
            }
        }

        /// <summary>
        /// Get all database information objects.
        /// </summary>
        /// <returns>All database information objects.</returns>
        public static DatabaseList GetDatabases()
        {
            DatabaseList databases = null;

            for (Int32 getAttempts = 0; (databases.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadDatabases();
                databases = Databases;
            }
            return databases;
        }

        /// <summary>
        /// Get databases from web service.
        /// </summary>
        private static void LoadDatabases()
        {
            DatabaseList databases;

            if (Databases.IsNull())
            {
                // Get data from web service.
                databases = new DatabaseList();
                foreach (WebDatabase webDatabase in WebServiceClient.GetDatabases())
                {
                    databases.Add(new Database(webDatabase.Id, webDatabase.LongName, webDatabase.ShortName));
                }

                Databases = databases;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            Databases = null;
        }
    }
}
