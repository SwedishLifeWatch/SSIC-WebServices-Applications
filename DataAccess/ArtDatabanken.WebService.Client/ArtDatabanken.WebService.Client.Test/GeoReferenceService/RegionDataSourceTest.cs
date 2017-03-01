using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.GeoReferenceService;

namespace ArtDatabanken.WebService.Client.Test.GeoReferenceService
{
    [TestClass]
    public class RegionDataSourceTest : TestBase
    {
        private RegionDataSource _regionDataSource;

        public RegionDataSourceTest()
        {
            _regionDataSource = null;
        }

        [TestMethod]
        public void Constructor()
        {
            RegionDataSource regionDataSource;

            regionDataSource = new RegionDataSource();
            Assert.IsNotNull(regionDataSource);
        }

        private RegionDataSource GetRegionDataSource()
        {
            return GetRegionDataSource(false);
        }

        private RegionDataSource GetRegionDataSource(Boolean refresh)
        {
            if (_regionDataSource.IsNull() || refresh)
            {
                _regionDataSource = new RegionDataSource();
            }
            return _regionDataSource;
        }

        [TestMethod]
        public void GetRegionCategories()
        {
            RegionCategoryList regionCategories;
            Int32 countryIsoCode = 752;
            regionCategories = GetRegionDataSource(true).GetRegionCategories(GetUserContext(), countryIsoCode);
            Assert.IsTrue(regionCategories.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionsByCategories()
        {
            RegionList regions;
            RegionCategoryList regionCategories;
            regionCategories = new RegionCategoryList();
            // Add id 1 = Political Boundry
            RegionCategory regionCategory = new RegionCategory(1, null, null, null, null, null, 0, 0, new DataContext(GetUserContext()));
            regionCategories.Add(regionCategory);
            // Add id 2 = Interest area
            regionCategory = new RegionCategory(2, null, null, null, null, null, 0, 0, new DataContext(GetUserContext()));
            regionCategories.Add(regionCategory);
            regions = GetRegionDataSource(true).GetRegionsByCategories(GetUserContext(), regionCategories);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionsByIds()
        {
            RegionList regions;
            List<Int32> regionIds = new List<Int32>();
            regionIds.Add(3551);
            regions = GetRegionDataSource(true).GetRegionsByIds(GetUserContext(), regionIds);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionsBySearchCriteria()
        {
            RegionList regions;
            RegionSearchCriteria regionSearchCriteria;

            // Test all parameters empty
            regionSearchCriteria = new RegionSearchCriteria();
            regions = GetRegionDataSource(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test region category list
            RegionCategoryList regionCategories = new RegionCategoryList();
            regionCategories.Add(new RegionCategory(1, null, null, null, null, null, 0, 0, new DataContext(GetUserContext())));
            regionSearchCriteria = new RegionSearchCriteria();
            regionSearchCriteria.Categories = regionCategories;
            regions = GetRegionDataSource(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test Country ISO Codes
            List<Int32> countryIsoCodes = new List<Int32>();
            countryIsoCodes.Add(752);
            regionSearchCriteria = new RegionSearchCriteria();
            regionSearchCriteria.CountryIsoCodes = countryIsoCodes;
            regions = GetRegionDataSource(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
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
            regions = GetRegionDataSource(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test region type
            RegionType regionType = new RegionType(1, "", new DataContext(GetUserContext()));
            regionSearchCriteria = new RegionSearchCriteria();
            regionSearchCriteria.Type = regionType;
            regions = GetRegionDataSource(true).GetRegionsBySearchCriteria(GetUserContext(), regionSearchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionTypes()
        {
            RegionTypeList regionTypes;

            regionTypes = GetRegionDataSource(true).GetRegionTypes(GetUserContext());
            Assert.IsTrue(regionTypes.IsNotEmpty());
        }
    }
}
