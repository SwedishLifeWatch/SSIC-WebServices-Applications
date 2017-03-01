using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.GeoReferenceService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RegionManagerSingleThreadCacheTest : TestBase
    {
       private RegionManagerSingleThreadCache _regionManager;

        public RegionManagerSingleThreadCacheTest()
        {
            _regionManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            RegionManagerSingleThreadCache regionManager;

            regionManager = new RegionManagerSingleThreadCache(GetCoordinateSystem());
            Assert.IsNotNull(regionManager);
        }

        [TestMethod]
        public void GetRegionCategories()
        {
            RegionCategoryList regionCategories;

            Int32? countryIsoCode = 752;
            regionCategories = GetRegionManager(true).GetRegionCategories(GetUserContext(), countryIsoCode);
            Assert.IsTrue(regionCategories.IsNotEmpty());
            Assert.IsTrue(regionCategories[0].CountryIsoCode.IsNotNull());
        }

        [TestMethod]
        public void GetRegionTypes()
        {
            RegionTypeList regionTypes;

            regionTypes = GetRegionManager(true).GetRegionTypes(GetUserContext());
            Assert.IsTrue(regionTypes.IsNotEmpty());
        }

        private RegionManagerSingleThreadCache GetRegionManager()
        {
            return GetRegionManager(false);
        }

        private RegionManagerSingleThreadCache GetRegionManager(Boolean refresh)
        {
            if (_regionManager.IsNull() || refresh)
            {
                _regionManager = new RegionManagerSingleThreadCache(GetCoordinateSystem());
                _regionManager.DataSource = new RegionDataSource();
            }
            return _regionManager;
        }
    }
}
