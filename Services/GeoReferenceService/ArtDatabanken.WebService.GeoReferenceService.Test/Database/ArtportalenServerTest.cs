using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.GeoReferenceService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Database
{
    [TestClass]
    public class ArtportalenServerTest : TestBase
    {
        private ArtportalenServer _database;

        [TestMethod]
        public void Constructor()
        {
            using (ArtportalenServer artportalenServer = new ArtportalenServer())
            {
                Assert.IsNotNull(artportalenServer);
            }
        }

        private ArtportalenServer GetDatabase(Boolean refresh)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.RollbackTransaction();
                    _database.Dispose();
                }
                _database = new ArtportalenServer();
                _database.BeginTransaction();
            }
            return _database;
        }

        [TestMethod]
        public void GetRegionInformation()
        {
            using (DataReader dataReader = GetDatabase(true).GetRegionInformation())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }
    }
}
