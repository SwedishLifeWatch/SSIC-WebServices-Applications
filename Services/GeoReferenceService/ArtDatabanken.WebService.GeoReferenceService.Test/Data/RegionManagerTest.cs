using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using NotUsedRegionManager = ArtDatabanken.WebService.Data.RegionManager;
using RegionManager = ArtDatabanken.WebService.GeoReferenceService.Data.RegionManager;


namespace ArtDatabanken.WebService.GeoReferenceService.Test.Data
{
    [TestClass]
    public class RegionManagerTest : TestBase
    {
        [TestMethod]
        public void Constructor()
        {
            RegionManager regionManager;

            regionManager = new RegionManager();
            Assert.IsNotNull(regionManager);
        }

        [TestMethod]
        public void GetCitiesByNameSearchString()
        {
            WebStringSearchCriteria searchCriteria = new WebStringSearchCriteria() { SearchString = "Uppsala%" };

            // Get cities in Google Mercator
            WebCoordinateSystem coordinateSystem = new WebCoordinateSystem() { Id = CoordinateSystemId.GoogleMercator };
            List<WebCityInformation> cityInformations = RegionManager.GetCitiesByNameSearchString(GetContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(cityInformations.IsNotEmpty());

            // Get cities in RT90 (same as in database)
            WebCoordinateSystem coordinateSystemRt90 = new WebCoordinateSystem() { Id = CoordinateSystemId.Rt90_25_gon_v };
            List<WebCityInformation> cityInformationsRt90 = RegionManager.GetCitiesByNameSearchString(GetContext(), searchCriteria, coordinateSystemRt90);
            Assert.IsTrue(cityInformationsRt90.IsNotEmpty());

            // Get cities in Sweref99TM
            WebCoordinateSystem coordinateSystemSweref99TM = new WebCoordinateSystem() { Id = CoordinateSystemId.SWEREF99_TM };
            List<WebCityInformation> cityInformationsSweref99TM = RegionManager.GetCitiesByNameSearchString(GetContext(), searchCriteria, coordinateSystemSweref99TM);
            Assert.IsTrue(cityInformationsSweref99TM.IsNotEmpty());

            // Test with character '.
            searchCriteria = new WebStringSearchCriteria();
            searchCriteria.SearchString = "Hej ' hopp";
            coordinateSystemSweref99TM = new WebCoordinateSystem() { Id = CoordinateSystemId.SWEREF99_TM };
            cityInformationsSweref99TM = RegionManager.GetCitiesByNameSearchString(GetContext(), searchCriteria, coordinateSystemSweref99TM);
            Assert.IsTrue(cityInformationsSweref99TM.IsEmpty());
        }

        [TestMethod]
        public void GetRegionCategories()
        {
            List<WebRegionCategory> regionCategories1, regionCategories2;

            regionCategories1 = RegionManager.GetRegionCategories(GetContext(), true, Settings.Default.SwedenCountryISOCode);
            Assert.IsTrue(regionCategories1.IsNotEmpty());
            foreach (WebRegionCategory regionCategory in regionCategories1)
            {
                Assert.AreEqual(Settings.Default.SwedenCountryISOCode, regionCategory.CountryIsoCode);
            }

            regionCategories2 = RegionManager.GetRegionCategories(GetContext(), false, Settings.Default.SwedenCountryISOCode);
            Assert.IsTrue(regionCategories2.IsNotEmpty());
            Assert.IsTrue(regionCategories1.Count < regionCategories2.Count);

            // Test none existing country iso code.
            regionCategories1 = RegionManager.GetRegionCategories(GetContext(), true, Int32.MinValue);
            Assert.IsTrue(regionCategories1.IsEmpty());
        }

        [TestMethod]
        public void GetRegionsByCategories()
        {
            List<WebRegionCategory> regionCategories;
            List<WebRegion> regions;
            WebRegionCategory regionCategory;

            regionCategories = null;
            regions = RegionManager.GetRegionsByCategories(GetContext(), regionCategories);
            Assert.IsTrue(regions.IsEmpty());

            regionCategories = new List<WebRegionCategory>();
            regions = RegionManager.GetRegionsByCategories(GetContext(), regionCategories);
            Assert.IsTrue(regions.IsEmpty());

            regionCategories = new List<WebRegionCategory>();
            regionCategory = new WebRegionCategory();
            regionCategory.Id = 1; // Kommuner
            regionCategories.Add(regionCategory);
            regions = RegionManager.GetRegionsByCategories(GetContext(), regionCategories);
            Assert.IsFalse(regions.IsEmpty());

            regionCategories = RegionManager.GetRegionCategories(GetContext(), true, Settings.Default.SwedenCountryISOCode);
            regions = RegionManager.GetRegionsByCategories(GetContext(), regionCategories);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionsByGuids()
        {
            List<String> guids;
            List<WebRegion> regions;

            guids = null;
            regions = RegionManager.GetRegionsByGuids(GetContext(), guids);
            Assert.IsTrue(regions.IsEmpty());

            guids = new List<String>();
            regions = RegionManager.GetRegionsByGuids(GetContext(), guids);
            Assert.IsTrue(regions.IsEmpty());

            guids = new List<String>();
            guids.Add(Settings.Default.ProvinceSkaneGUID);
            guids.Add(Settings.Default.ProvinceBlekingeGUID);
            regions = RegionManager.GetRegionsByGuids(GetContext(), guids);
            Assert.IsTrue(regions.IsNotEmpty());
            Assert.AreEqual(guids.Count, regions.Count);
        }

        [TestMethod]
        public void GetRegionsByIds()
        {
            List<Int32> regionIds;
            List<String> regionGuids;
            List<WebRegion> regions;

            regionGuids = new List<String>();
            regionGuids.Add(Settings.Default.ProvinceBlekingeGUID);
            regionGuids.Add(Settings.Default.ProvinceSkaneGUID);
            regions = RegionManager.GetRegionsByGuids(GetContext(ApplicationIdentifier.ArtDatabankenSOA), regionGuids);
            Assert.IsTrue(regions.IsNotEmpty());
            Assert.AreEqual(regionGuids.Count, regions.Count);
            regionIds = new List<Int32>();
            foreach (WebRegion webRegion in regions)
            {
                regionIds.Add(webRegion.Id);
            }
            regions = RegionManager.GetRegionsByIds(GetContext(ApplicationIdentifier.ArtDatabankenSOA), regionIds);
            Assert.IsTrue(regions.IsNotEmpty());
            Assert.AreEqual(regionIds.Count, regions.Count);
        }

        [TestMethod]
        public void GetRegionsBySearchCriteria()
        {
            List<Int32> countryIsoCodes;
            List<WebRegion> regions;
            List<WebRegionCategory> allRegionCategories, regionCategories;
            String nameSearchString;
            WebRegionCategory regionCategory;
            WebRegionSearchCriteria searchCriteria;
            WebRegionType regionType;
            WebStringSearchCriteria stringSearchCriteria;

            allRegionCategories = RegionManager.GetRegionCategories(GetContext(), false, 0);

            // Test - All parameters are empty.
            // All regions are returned.
            searchCriteria = new WebRegionSearchCriteria();
            regions = RegionManager.GetRegionsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test - CountryIsoCodes.
            searchCriteria = new WebRegionSearchCriteria();
            countryIsoCodes = new List<Int32>();
            countryIsoCodes.Add(allRegionCategories[0].CountryIsoCode);
            searchCriteria.CountryIsoCodes = countryIsoCodes;
            regions = RegionManager.GetRegionsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Name search string.
            searchCriteria = new WebRegionSearchCriteria();
            nameSearchString = "U%";
            stringSearchCriteria = new WebStringSearchCriteria();
            stringSearchCriteria.SearchString = nameSearchString;
            searchCriteria.NameSearchString = stringSearchCriteria;
            regions = RegionManager.GetRegionsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());
            // Check that every region name starts with letter "U"
            foreach (WebRegion region in regions)
            {
                Assert.IsTrue(region.Name.Substring(0, 1).Equals("U"));
            }

            // Test - Region categories.
            searchCriteria = new WebRegionSearchCriteria();
            regionCategories = new List<WebRegionCategory>();
            regionCategory = allRegionCategories[2];
            regionCategories.Add(regionCategory);
            searchCriteria.Categories = regionCategories;
            regions = RegionManager.GetRegionsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());
            // Check that all regions belong to the specified category.
            foreach (WebRegion region in regions)
            {
                Assert.AreEqual(regionCategory.Id, region.CategoryId);
            }

            // Test - Region type.
            searchCriteria = new WebRegionSearchCriteria();
            regionType = RegionManager.GetRegionTypes(GetContext())[0];
            searchCriteria.Type = regionType;
            regions = RegionManager.GetRegionsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Name search string with character '.
            searchCriteria = new WebRegionSearchCriteria();
            nameSearchString = "Hej ' hopp";
            stringSearchCriteria = new WebStringSearchCriteria();
            stringSearchCriteria.SearchString = nameSearchString;
            searchCriteria.NameSearchString = stringSearchCriteria;
            regions = RegionManager.GetRegionsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(regions.IsEmpty());
        }

        [TestMethod]
        public void GetRegionsGeographyByGuids()
        {
            List<String> guids;
            List<WebRegionGeography> regionsGeography;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            guids = null;
            regionsGeography = RegionManager.GetRegionsGeographyByGuids(GetContext(ApplicationIdentifier.ArtDatabankenSOA), guids, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsEmpty());

            guids = new List<String>();
            regionsGeography = RegionManager.GetRegionsGeographyByGuids(GetContext(), guids, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsEmpty());

            guids = new List<String>();
            guids.Add(Settings.Default.ProvinceSkaneGUID);
            guids.Add(Settings.Default.ProvinceBlekingeGUID);
            regionsGeography = RegionManager.GetRegionsGeographyByGuids(GetContext(), guids, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsNotEmpty());
            Assert.AreEqual(guids.Count, regionsGeography.Count);

            // Test GUID with character '.
            //guids = new List<String>();
            //guids.Add("URN:LSID:artportalen.se:area:DataSet16'Feature2");
            //regionsGeography = RegionManager.GetRegionsGeographyByGuids(GetContext(), guids, coordinateSystem);
            //Assert.IsTrue(regionsGeography.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetRegionsGeographyByGuidsAccessRightsError()
        {
            List<String> guids;
            List<WebRegionGeography> regionsGeography;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            guids = null;
            regionsGeography = RegionManager.GetRegionsGeographyByGuids(GetContext(ApplicationIdentifier.UserAdmin),
                                                                        guids,
                                                                        coordinateSystem);
            Assert.IsTrue(regionsGeography.IsEmpty());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRegionsGeographyByGuidsNullCoordinateSystemError()
        {
            List<String> guids;
            List<WebRegionGeography> regionsGeography;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = null;
            guids = null;
            regionsGeography = RegionManager.GetRegionsGeographyByGuids(GetContext(ApplicationIdentifier.ArtDatabankenSOA), guids, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsEmpty());
        }

        [TestMethod]
        public void GetRegionsGeographyByIds()
        {
            List<Int32> regionIds;
            List<String> regionGuids;
            List<WebRegionGeography> regionsGeography;
            WebCoordinateSystem coordinateSystem;

            regionGuids = new List<String>();
            regionGuids.Add("URN:LSID:artportalen.se:area:DataSet16Feature1");
            regionGuids.Add("URN:LSID:artportalen.se:area:DataSet16Feature2");
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            regionsGeography = RegionManager.GetRegionsGeographyByGuids(GetContext(ApplicationIdentifier.ArtDatabankenSOA), regionGuids, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsNotEmpty());
            Assert.AreEqual(regionGuids.Count, regionsGeography.Count);
            regionIds = new List<Int32>();
            foreach (WebRegionGeography webRegionGeography in regionsGeography)
            {
                regionIds.Add(webRegionGeography.Id);
            }
            regionsGeography = RegionManager.GetRegionsGeographyByIds(GetContext(ApplicationIdentifier.ArtDatabankenSOA), regionIds, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsNotEmpty());
            Assert.AreEqual(regionIds.Count, regionsGeography.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void GetRegionsGeographyByIdsAccessRightsError()
        {
            List<Int32> regionIds;
            List<WebRegionGeography> regionsGeography;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            regionIds = new List<Int32>();
            regionIds.Add(1);
            regionsGeography = RegionManager.GetRegionsGeographyByIds(GetContext(ApplicationIdentifier.UserAdmin), regionIds, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionTypes()
        {
            List<WebRegionType> regionTypes;

            regionTypes = RegionManager.GetRegionTypes(GetContext());
            Assert.IsTrue(regionTypes.IsNotEmpty());
        }

        [Ignore]
        [TestMethod]
        public void TestRegionGeometry()
        {
            // Test geometry for all regions in database.
            List<Int32> regionIds;
            List<WebRegion> regions;
            List<WebRegionCategory> regionCategories, categories;
            List<WebRegionGeography> regionsGeography;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            regionCategories = RegionManager.GetRegionCategories(GetContext(), false, 0);
            foreach (WebRegionCategory regionCategory in regionCategories)
            {
                categories = new List<WebRegionCategory>();
                categories.Add(regionCategory);
                regions = RegionManager.GetRegionsByCategories(GetContext(), categories);
                if (regions.IsNotEmpty())
                {
                    regionIds = new List<Int32>();
                    foreach (WebRegion region in regions)
                    {
                        regionIds.Add(region.Id);
                    }
                    regionsGeography = RegionManager.GetRegionsGeographyByIds(GetContext(), regionIds, coordinateSystem);
                    Assert.IsTrue(regionsGeography.IsNotEmpty());
                    Assert.AreEqual(regions.Count, regionsGeography.Count);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateRegionInformationFromArtportalen()
        {
            // This test should only be run when we want to update all
            // region information used in GeoReferenceService.
            RegionManager.UpdateRegionInformationFromArtportalen(GetContext());
        }
    }
}
