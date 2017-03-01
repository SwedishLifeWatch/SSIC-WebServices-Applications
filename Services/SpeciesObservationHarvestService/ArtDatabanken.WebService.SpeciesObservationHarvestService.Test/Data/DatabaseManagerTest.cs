using System;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
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
            DatabaseManager databaseManager = new DatabaseManager();
            Assert.IsNotNull(databaseManager);
        }

        [TestMethod]
        public void GetDatabase()
        {
            WebServiceDataServer databaseServer = GetDatabaseManager(true).GetDatabase(GetContext());
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
