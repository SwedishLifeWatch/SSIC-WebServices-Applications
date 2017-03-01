using System;
using ArtDatabanken.WebService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.GeoReferenceService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Data
{
    [TestClass]
    public class DatabaseManagerTest : TestBase
    {
        private DatabaseManager _databaseManager;

        public DatabaseManagerTest()
        {
            _databaseManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            DatabaseManager databaseManager;

            databaseManager = new DatabaseManager();
            Assert.IsNotNull(databaseManager);
        }

        [TestMethod]
        public void GetDatabase()
        {
            WebServiceDataServer dataServer;

            dataServer = GetDatabaseManager(true).GetDatabase(GetContext());
            Assert.IsNotNull(dataServer);
        }

        private DatabaseManager GetDatabaseManager()
        {
            return GetDatabaseManager(false);
        }

        private DatabaseManager GetDatabaseManager(Boolean refresh)
        {
            if (_databaseManager.IsNull() || refresh)
            {
                _databaseManager = new DatabaseManager();
            }
            return _databaseManager;
        }
    }
}
