using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.GeoReferenceService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.GeoReferenceService.Test.Database
{
    [TestClass]
    public class GeoReferenceServerTest
    {
        private GeoReferenceServer _database;

        public GeoReferenceServerTest()
        {
            _database = null;
        }

        [TestMethod]
        public void Constructor()
        {
            using (WebServiceDataServer database = new GeoReferenceServer())
            {
                Assert.IsNotNull(database);
            }
        }

        [TestMethod]
        public void GetAddress()
        {
            String address;

            address = GeoReferenceServer.GetAddress();
            Assert.IsTrue(address.IsNotEmpty());
        }

        [TestMethod]
        public void GetCitiesByNameSearchString()
        {
            using (DataReader dataReader = GetDatabase(true).GetCitiesByNameSearchString("Uppsala%"))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        private GeoReferenceServer GetDatabase(Boolean refresh = false)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.RollbackTransaction();
                    _database.Dispose();
                }
                _database = new GeoReferenceServer();
                _database.BeginTransaction();
            }
            return _database;
        }

        [TestMethod]
        public void GetRegionCategories()
        {
            using (DataReader dataReader = GetDatabase(true).GetRegionCategories())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRegionsByCategories()
        {
            List<Int32> regionCategoryIds;

            regionCategoryIds = new List<Int32>();
            regionCategoryIds.Add(Settings.Default.RegionCategoryProvinceId);
            regionCategoryIds.Add(Settings.Default.RegionCategoryCountyId);
            using (DataReader dataReader = GetDatabase(true).GetRegionsByCategories(regionCategoryIds))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRegionsByGUIDs()
        {
            List<String> regionGUIDs;

            regionGUIDs = new List<String>();
            regionGUIDs.Add(Settings.Default.ProvinceSkaneGUID);
            regionGUIDs.Add(Settings.Default.ProvinceBlekingeGUID);
            using (DataReader dataReader = GetDatabase(true).GetRegionsByGUIDs(regionGUIDs))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRegionsByIds()
        {
            List<Int32> regionIds;

            regionIds = new List<Int32>();
            regionIds.Add(1);
            regionIds.Add(2);
            using (DataReader dataReader = GetDatabase(true).GetRegionsByIds(regionIds))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRegionsBySearchCriteria()
        {
            Int32? typeId;
            List<Int32> categoryIds, countryISOCodes;
            String nameSearchString;

            categoryIds = new List<Int32>();
            categoryIds.Add(Settings.Default.RegionCategoryCountyId);
            categoryIds.Add(Settings.Default.RegionCategoryProvinceId);
            countryISOCodes = new List<Int32>();
            countryISOCodes.Add(Settings.Default.SwedenCountryISOCode);
            nameSearchString = "U%";
            typeId = Settings.Default.RegionTypePolicticalBoundaryId;
            using (DataReader dataReader = GetDatabase(true).GetRegionsBySearchCriteria(nameSearchString, typeId, categoryIds, countryISOCodes))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRegionsGeographyByGUIDs()
        {
            List<String> regionGUIDs;

            regionGUIDs = new List<String>();
            regionGUIDs.Add(Settings.Default.ProvinceSkaneGUID);
            regionGUIDs.Add(Settings.Default.ProvinceBlekingeGUID);
            using (DataReader dataReader = GetDatabase(true).GetRegionsGeographyByGUIDs(regionGUIDs))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRegionsGeographyByIds()
        {
            List<Int32> regionIds;

            regionIds = new List<Int32>();
            regionIds.Add(1);
            regionIds.Add(2);
            using (DataReader dataReader = GetDatabase(true).GetRegionsGeographyByIds(regionIds))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRegionTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetRegionTypes())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void Ping()
        {
            using (WebServiceDataServer database = new GeoReferenceServer())
            {
                Assert.IsTrue(database.Ping());
            }
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            if (_database.IsNotNull())
            {
                _database.RollbackTransaction();
                _database.Dispose();
                _database = null;
            }
        }
    }
}
