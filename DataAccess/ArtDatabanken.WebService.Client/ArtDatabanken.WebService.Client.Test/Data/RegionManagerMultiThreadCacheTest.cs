using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.GeoReferenceService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RegionManagerMultiThreadCacheTest : TestBase
    {
       private RegionManagerMultiThreadCache _regionManager;

        public RegionManagerMultiThreadCacheTest()
        {
            _regionManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            RegionManagerMultiThreadCache regionManager;

            regionManager = new RegionManagerMultiThreadCache(GetCoordinateSystem());
            Assert.IsNotNull(regionManager);
        }

        [TestMethod]
        public void GetRegionCategories()
        {
            RegionCategoryList regionCategories;
            Int32? countryIsoCode = 752;
            regionCategories = GetRegionManager(true).GetRegionCategories(GetUserContext(), countryIsoCode);
            Assert.IsTrue(regionCategories.IsNotEmpty());
            Assert.IsNotNull(regionCategories[0].CountryIsoCode);
        }

        [TestMethod]
        public void GetRegionTypes()
        {
            RegionTypeList regionTypes;

            regionTypes = GetRegionManager(true).GetRegionTypes(GetUserContext());
            Assert.IsTrue(regionTypes.IsNotEmpty());
        }

        private RegionManagerMultiThreadCache GetRegionManager()
        {
            return GetRegionManager(false);
        }

        private RegionManagerMultiThreadCache GetRegionManager(Boolean refresh)
        {
            if (_regionManager.IsNull() || refresh)
            {
                _regionManager = new RegionManagerMultiThreadCache(GetCoordinateSystem());
                _regionManager.DataSource = new RegionDataSource();
            }
            return _regionManager;
        }
    }
}
