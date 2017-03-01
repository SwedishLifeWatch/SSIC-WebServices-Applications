using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.GeoReferenceService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Database
{
    [TestClass]
    public class SwdishSpeciesObservationServerTest : TestBase
    {
        private SwedishSpeciesObservationServer _database;

        [TestMethod]
        public void Constructor()
        {
            using (SwedishSpeciesObservationServer swedishSpeciesObservationServer = new SwedishSpeciesObservationServer())
            {
                Assert.IsNotNull(swedishSpeciesObservationServer);
            }
        }

        private SwedishSpeciesObservationServer GetDatabase(Boolean refresh)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.RollbackTransaction();
                    _database.Dispose();
                }
                _database = new SwedishSpeciesObservationServer();
                _database.BeginTransaction();
            }
            return _database;
        }

        
    }
}
