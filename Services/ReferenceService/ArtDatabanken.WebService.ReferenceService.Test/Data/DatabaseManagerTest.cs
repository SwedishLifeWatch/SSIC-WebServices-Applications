using System;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.ReferenceService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.ReferenceService.Test.Data
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
            WebServiceDataServer databaseServer;

            databaseServer = GetDatabaseManager(true).GetDatabase(GetContext());
            Assert.IsNotNull(databaseServer);
        }

        private DatabaseManager GetDatabaseManager(Boolean refresh = false)
        {
            if (_databaseManager.IsNull() || refresh)
            {
                _databaseManager = new DatabaseManager();
            }

            return _databaseManager;
        }
    }
}
