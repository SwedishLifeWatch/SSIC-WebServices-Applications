using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Database
{
    [TestClass]
    public class WebServiceDataServerTest
    {
        private WebServiceDataServer _database;

        public WebServiceDataServerTest()
        {
            _database = null;
        }

        [TestMethod]
        public void Constructor()
        {
            using (WebServiceDataServer database = new SpeciesObservationHarvestServer())
            {
                Assert.IsNotNull(database);
            }
        }

        private WebServiceDataServer GetDatabase(Boolean refresh = false)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.Dispose();
                }

                _database = new SpeciesObservationHarvestServer();
            }

            return _database;
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviders()
        {
            using (DataReader dataReader = GetDatabase().GetSpeciesObservationDataProviders((Int32)(LocaleId.sv_SE)))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            if (_database.IsNotNull())
            {
                _database.Dispose();
                _database = null;
            }
        }
    }
}
