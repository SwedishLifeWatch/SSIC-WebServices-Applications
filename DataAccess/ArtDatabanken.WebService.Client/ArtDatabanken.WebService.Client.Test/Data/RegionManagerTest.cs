using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.GeoReferenceService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RegionManagerTest : TestBase
    {
        private RegionManager _regionManager;

        public RegionManagerTest()
        {
            _regionManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            RegionManager regionManager;

            regionManager = new RegionManager(GetCoordinateSystem());
            Assert.IsNotNull(regionManager);
        }

        private RegionManager GetRegionManager()
        {
            return GetRegionManager(false);
        }

        private RegionManager GetRegionManager(Boolean refresh)
        {
            if (_regionManager.IsNull() || refresh)
            {
                _regionManager = new RegionManager(GetCoordinateSystem());
                _regionManager.DataSource = new RegionDataSource();
            }
            return _regionManager;
        }

        [TestMethod]
        public void GetRegionCategories()
        {
            RegionCategoryList regionCategories;
            Int32? countryIsoCode = 752;

            regionCategories = GetRegionManager(true).GetRegionCategories(GetUserContext(), countryIsoCode);
            Assert.IsTrue(regionCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionsByCategories()
        {
            RegionList regions;
            RegionCategoryList regionCategories;
            regionCategories = new RegionCategoryList();
            // Add id 1 = Political Boundry
            IRegionCategory regionCategory = new RegionCategory(1, null, null, null, null, null, 0, 0, new DataContext(GetUserContext()));
            regionCategories.Add(regionCategory);
            // Add id 2 = Interest area
            regionCategory = new RegionCategory(2, null, null, null, null, null, 0, 0, new DataContext(GetUserContext()));
            regionCategories.Add(regionCategory);
            regions = GetRegionManager(true).GetRegionsByCategories(GetUserContext(), regionCategories);
            Assert.IsTrue(regions.IsNotEmpty());

            // Get all provinces.
            regionCategory = CoreData.RegionManager.GetRegionCategory(GetUserContext(), 16);
            regions = CoreData.RegionManager.GetRegionsByCategory(GetUserContext(), regionCategory);
            Assert.IsTrue(regions.IsNotEmpty());
            // Get all counties.
            regionCategory = CoreData.RegionManager.GetRegionCategory(GetUserContext(), 21);
            regions = CoreData.RegionManager.GetRegionsByCategory(GetUserContext(), regionCategory);
            Assert.IsTrue(regions.IsNotEmpty());
            // Get all municipalities.
            regionCategory = CoreData.RegionManager.GetRegionCategory(GetUserContext(), 1);
            regions = CoreData.RegionManager.GetRegionsByCategory(GetUserContext(), regionCategory);
            Assert.IsTrue(regions.IsNotEmpty());

        }

        [TestMethod]
        public void GetRegionsBySearchCriteria()
        {
            RegionList regions;
            RegionSearchCriteria regionSearchCriteria;

            // Test all parameters empty
            regionSearchCriteria = new RegionSearchCriteria();
            regions = GetRegionManager(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test region category list
            RegionCategoryList regionCategories = new RegionCategoryList();
            regionCategories.Add(new RegionCategory(1, null, null, null, null, null, 0, 0, new DataContext(GetUserContext())));
            regionSearchCriteria = new RegionSearchCriteria();
            regionSearchCriteria.Categories = regionCategories;
            regions = GetRegionManager(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test Country ISO Codes
            List<Int32> countryIsoCodes = new List<Int32>();
            countryIsoCodes.Add(752);
            regionSearchCriteria = new RegionSearchCriteria();
            regionSearchCriteria.CountryIsoCodes = countryIsoCodes;
            regions = GetRegionManager(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test name search string
            String searchString = "U%";
            StringSearchCriteria nameSearchString = new StringSearchCriteria();
            List<StringCompareOperator> compareOperators = new List<StringCompareOperator>();
            compareOperators.Add(StringCompareOperator.Equal);
            nameSearchString.CompareOperators = compareOperators;
            nameSearchString.SearchString = searchString;
            regionSearchCriteria = new RegionSearchCriteria();
            regionSearchCriteria.NameSearchString = nameSearchString;
            regions = GetRegionManager(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test region type
            RegionType regionType = new RegionType(1, "", new DataContext(GetUserContext()));
            regionSearchCriteria = new RegionSearchCriteria();
            regionSearchCriteria.Type = regionType;
            regions = GetRegionManager(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionTypes()
        {
            RegionTypeList regionTypes;

            regionTypes = GetRegionManager(true).GetRegionTypes(GetUserContext());
            Assert.IsTrue(regionTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionsByGUIDs()
        {
            IRegionCategory category = GetRegionManager(true).GetRegionCategory(GetUserContext(), 1);
            RegionList regions1 = GetRegionManager(true).GetRegionsByCategory(GetUserContext(), category);
            List<string> GUIDs = new List<string>();
            foreach (Region region in regions1)
            {
                GUIDs.Add(region.GUID);
            }
            RegionList regions2 = GetRegionManager(true).GetRegionsByGUIDs(GetUserContext(), GUIDs);
            Assert.AreEqual(regions1[0].CategoryId, regions2[2].CategoryId);
            Assert.AreEqual(regions1.Count, regions2.Count);
        }
    }
}
