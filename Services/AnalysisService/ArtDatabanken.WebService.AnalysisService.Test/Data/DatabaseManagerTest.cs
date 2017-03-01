using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.AnalysisService.Data;

namespace ArtDatabanken.WebService.AnalysisService.Test.Data
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
        [TestCategory("NightlyTest")]
        public void Constructor()
        {
            DatabaseManager databaseManager;

            databaseManager = new DatabaseManager();
            Assert.IsNotNull(databaseManager);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetDatabase()
        {
            WebServiceDataServer databaseServer;

            databaseServer = GetDatabaseManager(true).GetDatabase(Context);
            Assert.IsNotNull(databaseServer);
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
