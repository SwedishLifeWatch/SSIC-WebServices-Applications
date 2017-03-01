using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class GeoReferenceServiceProxyTest
    {
        private WebClientInformation _clientInformation;

        public GeoReferenceServiceProxyTest()
        {
            _clientInformation = null;
        }

        [TestMethod]
        public void ClearCache()
        {
            WebServiceProxy.GeoReferenceService.ClearCache(GetClientInformation());
        }

        [TestMethod]
        public void DeleteTrace()
        {
            // Create some trace information.
            WebServiceProxy.GeoReferenceService.StartTrace(GetClientInformation(), Settings.Default.TestUserName);
            WebServiceProxy.GeoReferenceService.GetRegionTypes(GetClientInformation());
            WebServiceProxy.GeoReferenceService.StopTrace(GetClientInformation());

            // Delete trace information.
            WebServiceProxy.GeoReferenceService.DeleteTrace(GetClientInformation());
        }

        [TestMethod]
        public void GetCitiesByNameSearchString()
        {
            List<WebCityInformation> cities;
            WebStringSearchCriteria searchCriteria = new WebStringSearchCriteria() {SearchString = "Uppsala%"};
            WebCoordinateSystem coordinateSystemRt90 = new WebCoordinateSystem() {Id = CoordinateSystemId.Rt90_25_gon_v};
            WebCoordinateSystem coordinateSystemGoogleMercator = new WebCoordinateSystem() { Id = CoordinateSystemId.GoogleMercator };
            WebCoordinateSystem coordinateSystemSweref99TM = new WebCoordinateSystem() { Id = CoordinateSystemId.SWEREF99_TM };

            // Get result in RT90
            cities = WebServiceProxy.GeoReferenceService.GetCitiesByNameSearchString(GetClientInformation(), 
                                                                                     searchCriteria, 
                                                                                     coordinateSystemRt90);
            Assert.IsTrue(cities.IsNotEmpty());

            // Get result in GoogleMercator
            cities = WebServiceProxy.GeoReferenceService.GetCitiesByNameSearchString(GetClientInformation(),
                                                                                     searchCriteria,
                                                                                     coordinateSystemGoogleMercator);
            Assert.IsTrue(cities.IsNotEmpty());

            // Get result in Sweref99
            cities = WebServiceProxy.GeoReferenceService.GetCitiesByNameSearchString(GetClientInformation(),
                                                                                     searchCriteria,
                                                                                     coordinateSystemSweref99TM);
            Assert.IsTrue(cities.IsNotEmpty());
        }

        protected WebClientInformation GetClientInformation(ApplicationIdentifier applicationIdentifier = ApplicationIdentifier.AnalysisPortal)
        {
            WebLoginResponse loginResponse;

            if (_clientInformation.IsNull())
            {
                loginResponse = WebServiceProxy.GeoReferenceService.Login(Settings.Default.TestUserName,
                                                                          Settings.Default.TestPassword,
                                                                          applicationIdentifier.ToString(),
                                                                          false);
                _clientInformation = new WebClientInformation();
                _clientInformation.Locale = loginResponse.Locale;
                _clientInformation.Role = null;
                _clientInformation.Token = loginResponse.Token;
            }

            return _clientInformation;
        }

        [TestMethod]
        public void GetConvertedPoints()
        {
            Double fromX1, fromX2, fromY1, fromY2;
            List<WebPoint> fromPoints, toPoints;
            WebCoordinateSystem fromCoordinateSystem, toCoordinateSystem;

            fromX1 = 1644820;
            fromY1 = 6680450;
            fromX2 = 1634243;
            fromY2 = 6653434;
            fromPoints = new List<WebPoint>();
            fromPoints.Add(new WebPoint(fromX1, fromY1));
            fromPoints.Add(new WebPoint(fromX2, fromY2));
            fromCoordinateSystem = new WebCoordinateSystem();
            fromCoordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            foreach (CoordinateSystemId coordinateSystemId in Enum.GetValues(typeof(CoordinateSystemId)))
            {
                toCoordinateSystem = new WebCoordinateSystem();
                toCoordinateSystem.Id = coordinateSystemId;
                if (toCoordinateSystem.Id == CoordinateSystemId.None)
                {
                    toCoordinateSystem.WKT = ArtDatabanken.Settings.Default.Rt90_25_gon_v_WKT;
                }
                toPoints = WebServiceProxy.GeoReferenceService.GetConvertedPoints(GetClientInformation(), fromPoints, fromCoordinateSystem, toCoordinateSystem);
                Assert.IsTrue(toPoints.IsNotEmpty());
                Assert.AreEqual(fromPoints.Count, toPoints.Count);
                if ((fromCoordinateSystem.Id == toCoordinateSystem.Id) ||
                    (toCoordinateSystem.Id == CoordinateSystemId.None) ||
                    (fromCoordinateSystem.Id.ToString().ToUpper().Substring(0, 4) == 
                        toCoordinateSystem.Id.ToString().ToUpper().Substring(0, 4)))
                {
                    Assert.IsTrue(Math.Abs(fromX1 - toPoints[0].X) < 2);
                    Assert.IsTrue(Math.Abs(fromY1 - toPoints[0].Y) < 2);
                    Assert.IsTrue(Math.Abs(fromX2 - toPoints[1].X) < 2);
                    Assert.IsTrue(Math.Abs(fromY2 - toPoints[1].Y) < 2);
                }
                else
                {
                    Assert.IsTrue(Math.Abs(fromX1 - toPoints[0].X) > 1);
                    Assert.IsTrue(Math.Abs(fromY1 - toPoints[0].Y) > 1);
                    Assert.IsTrue(Math.Abs(fromX2 - toPoints[1].X) > 1);
                    Assert.IsTrue(Math.Abs(fromY2 - toPoints[1].Y) > 1);
                }
            }
        }

        [TestMethod]
        public void GetLog()
        {
            List<WebLogRow> logRows;

            logRows = WebServiceProxy.GeoReferenceService.GetLog(GetClientInformation(), LogType.None, null, 100);
            Assert.IsTrue(logRows.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionCategories()
        {
            List<WebRegionCategory> regionCategories1, regionCategories2;

            regionCategories1 = WebServiceProxy.GeoReferenceService.GetRegionCategories(GetClientInformation(), true, Settings.Default.TestCountryISOCode);
            Assert.IsTrue(regionCategories1.IsNotEmpty());
            foreach (WebRegionCategory regionCategory in regionCategories1)
            {
                Assert.AreEqual(Settings.Default.TestCountryISOCode, regionCategory.CountryIsoCode);
            }

            regionCategories2 = WebServiceProxy.GeoReferenceService.GetRegionCategories(GetClientInformation(), false, Settings.Default.TestCountryISOCode);
            Assert.IsTrue(regionCategories2.IsNotEmpty());
            Assert.IsTrue(regionCategories1.Count < regionCategories2.Count);
        }

        [TestMethod]
        public void GetRegionsByCategories()
        {
            List<WebRegionCategory> regionCategories;
            List<WebRegion> regions;
            WebRegionCategory regionCategory;

            regionCategory = new WebRegionCategory();
            regionCategory.Id = 1;
            regionCategories = new List<WebRegionCategory>();
            regionCategories.Add(regionCategory);
            regions = WebServiceProxy.GeoReferenceService.GetRegionsByCategories(GetClientInformation(), regionCategories);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionsByGUIDs()
        {
            List<String> regionGuids;
            List<WebRegion> regions;

            regionGuids = new List<String>();
            regionGuids.Add("URN:LSID:artportalen.se:area:DataSet16Feature1");
            regionGuids.Add("URN:LSID:artportalen.se:area:DataSet16Feature2");
            regions = WebServiceProxy.GeoReferenceService.GetRegionsByGUIDs(GetClientInformation(), regionGuids);
            Assert.IsTrue(regions.IsNotEmpty());
            Assert.AreEqual(regionGuids.Count, regions.Count);
        }

        [TestMethod]
        public void GetRegionsByIds()
        {
            List<Int32> regionIds;
            List<WebRegion> regions;

            regionIds = new List<Int32>();
            regionIds.Add(1);
            regionIds.Add(2);
            regionIds.Add(3);
            regions = WebServiceProxy.GeoReferenceService.GetRegionsByIds(GetClientInformation(), regionIds);
            Assert.IsTrue(regions.IsNotEmpty());
            Assert.AreEqual(regions[0].Name, "Blekinge");
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

            allRegionCategories = WebServiceProxy.GeoReferenceService.GetRegionCategories(GetClientInformation(), false, 0);

            // Test - All parameters are empty.
            // All regions are returned.
            searchCriteria = new WebRegionSearchCriteria();
            regions = WebServiceProxy.GeoReferenceService.GetRegionsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Test - CountryIsoCodes.
            searchCriteria = new WebRegionSearchCriteria();
            countryIsoCodes = new List<Int32>();
            countryIsoCodes.Add(allRegionCategories[0].CountryIsoCode);
            searchCriteria.CountryIsoCodes = countryIsoCodes;
            regions = WebServiceProxy.GeoReferenceService.GetRegionsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());

            // Name search string.
            searchCriteria = new WebRegionSearchCriteria();
            nameSearchString = "U%";
            stringSearchCriteria = new WebStringSearchCriteria();
            stringSearchCriteria.SearchString = nameSearchString;
            searchCriteria.NameSearchString = stringSearchCriteria;
            regions = WebServiceProxy.GeoReferenceService.GetRegionsBySearchCriteria(GetClientInformation(), searchCriteria);
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
            regions = WebServiceProxy.GeoReferenceService.GetRegionsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());
            // Check that all regions belong to the specified category.
            foreach (WebRegion region in regions)
            {
                Assert.AreEqual(regionCategory.Id, region.CategoryId);
            }

            // Test - Region type.
            searchCriteria = new WebRegionSearchCriteria();
            regionType = WebServiceProxy.GeoReferenceService.GetRegionTypes(GetClientInformation())[0];
            searchCriteria.Type = regionType;
            regions = WebServiceProxy.GeoReferenceService.GetRegionsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(regions.IsNotEmpty());
        }

        [TestMethod]
        public void GetRegionsGeographyByGuids()
        {
            List<String> regionGuids;
            List<WebRegionGeography> regionsGeography;
            WebCoordinateSystem coordinateSystem;

            regionGuids = new List<String>();
            regionGuids.Add("URN:LSID:artportalen.se:area:DataSet16Feature1");
            regionGuids.Add("URN:LSID:artportalen.se:area:DataSet16Feature2");
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            regionsGeography = WebServiceProxy.GeoReferenceService.GetRegionsGeographyByGUIDs(GetClientInformation(ApplicationIdentifier.ArtDatabankenSOA), regionGuids, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsNotEmpty());
            Assert.AreEqual(regionGuids.Count, regionsGeography.Count);
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
            regionsGeography = WebServiceProxy.GeoReferenceService.GetRegionsGeographyByGUIDs(GetClientInformation(ApplicationIdentifier.ArtDatabankenSOA), regionGuids, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsNotEmpty());
            Assert.AreEqual(regionGuids.Count, regionsGeography.Count);
            regionIds = new List<Int32>();
            foreach (WebRegionGeography webRegionGeography in regionsGeography)
            {
                regionIds.Add(webRegionGeography.Id);
            }
            regionsGeography = WebServiceProxy.GeoReferenceService.GetRegionsGeographyByIds(GetClientInformation(ApplicationIdentifier.ArtDatabankenSOA), regionIds, coordinateSystem);
            Assert.IsTrue(regionsGeography.IsNotEmpty());
            Assert.AreEqual(regionIds.Count, regionsGeography.Count);
        }

        [TestMethod]
        public void GetRegionTypes()
        {
            List<WebRegionType> regionTypes;

            regionTypes = WebServiceProxy.GeoReferenceService.GetRegionTypes(GetClientInformation());
            Assert.IsTrue(regionTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceProxy.GeoReferenceService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceProxy.GeoReferenceService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        public void Login()
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.GeoReferenceService.Login(Settings.Default.TestUserName,
                                                                      Settings.Default.TestPassword,
                                                                      Settings.Default.DyntaxaApplicationIdentifier,
                                                                      false);
            Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        public void Logout()
        {
            WebClientInformation clientInformation;
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.GeoReferenceService.Login(Settings.Default.TestUserName,
                                                                      Settings.Default.TestPassword,
                                                                      Settings.Default.DyntaxaApplicationIdentifier,
                                                                      false);
            Assert.IsNotNull(loginResponse);
            clientInformation = new WebClientInformation();
            clientInformation.Token = loginResponse.Token;
            WebServiceProxy.GeoReferenceService.Logout(clientInformation);
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceProxy.GeoReferenceService.Ping();
            Assert.IsTrue(ping);
        }

        [TestMethod]
        public void StartTrace()
        {
            WebServiceProxy.GeoReferenceService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.GeoReferenceService.StopTrace(GetClientInformation());
        }

        [TestMethod]
        public void StopTrace()
        {
            WebServiceProxy.GeoReferenceService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.GeoReferenceService.StopTrace(GetClientInformation());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                WebServiceProxy.GeoReferenceService.Logout(_clientInformation);
                _clientInformation = null;
                WebServiceProxy.CloseClients();
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            Configuration.InstallationType = InstallationType.ServerTest;
            // WebServiceProxy.GeoReferenceService.WebServiceAddress = @"silurus2-1.artdata.slu.se/GeoReferenceService/GeoReferenceService.svc";
            // WebServiceProxy.GeoReferenceService.WebServiceAddress = @"GeoReference.artdatabankensoa.se/GeoReferenceService.svc";
        }
    }
}
