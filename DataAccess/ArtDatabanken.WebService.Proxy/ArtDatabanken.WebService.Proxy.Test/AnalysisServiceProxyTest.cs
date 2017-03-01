using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Proxy.Test
{
    /// <summary>
    /// This class handles analysis service proxy tests.
    /// </summary>
    [TestClass]
    public class AnalysisServiceProxyTest
    {
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        private WebClientInformation _clientInformation;

        public AnalysisServiceProxyTest()
        {
            _clientInformation = null;
        }

        private WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                WebServiceProxy.AnalysisService.Logout(_clientInformation);
                _clientInformation = null;
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
            TestInitialize(Settings.Default.AnalysisPortalApplicationIdentifier);
        }

        public void TestInitialize(String applicationIdentifier)
        {
            WebLoginResponse loginResponse;

            // Production test.
            //Configuration.InstallationType = InstallationType.Production;
            //WebServiceProxy.AnalysisService.WebServiceAddress = @"analysis.artdatabankensoa.se/AnalysisService.svc";

            // Local test
            //WebServiceProxy.AnalysisService.InternetProtocol = InternetProtocol.Http;
            //WebServiceProxy.AnalysisService.WebServiceAddress = @"localhost:6063/AnalysisService.svc";
            //WebServiceProxy.AnalysisService.WebServiceComputer = WebServiceComputer.Local;
            //WebServiceProxy.AnalysisService.WebServiceProtocol = WebServiceProtocol.SOAP11;

            // Normal test server tests.
            Configuration.InstallationType = InstallationType.ServerTest;

            loginResponse = WebServiceProxy.AnalysisService.Login(Settings.Default.TestUserName,
                                                                  Settings.Default.TestPassword,
                                                                  applicationIdentifier,
                                                                  false);
            _clientInformation = new WebClientInformation();
            _clientInformation.Locale = loginResponse.Locale;
            _clientInformation.Role = loginResponse.Roles[0];
            _clientInformation.Token = loginResponse.Token;
        }


        #region Other tests

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void ClearCache()
        {
           TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.AnalysisService.ClearCache(GetClientInformation());
        }

      

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void DeleteTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            // Create some trace information.
            WebServiceProxy.AnalysisService.StartTrace(GetClientInformation(), Settings.Default.TestUserName);
            WebServiceProxy.AnalysisService.GetLog(GetClientInformation(), LogType.None, null, 100);
            WebServiceProxy.AnalysisService.StopTrace(GetClientInformation());

            // Delete trace information.
            WebServiceProxy.AnalysisService.DeleteTrace(GetClientInformation());
        }

        [TestMethod]
        public void GetHostsBySpeciesFactSearchCriteria()
        {
            List<WebTaxon> hosts;
            WebSpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add(1222); // Värddjur
            hosts = WebServiceProxy.AnalysisService.GetHostsBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.HostIds = new List<Int32>();
            searchCriteria.HostIds.Add((Int32)(TaxonId.Insects));
            hosts = WebServiceProxy.AnalysisService.GetHostsBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100011); // Kungsörn
            hosts = WebServiceProxy.AnalysisService.GetHostsBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetLog()
        {
            List<WebLogRow> logRows;
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            logRows = WebServiceProxy.AnalysisService.GetLog(GetClientInformation(), LogType.None, null, 100);
            Assert.IsTrue(logRows.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Login()
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.AnalysisService.Login(Settings.Default.TestUserName,
                                                                  Settings.Default.TestPassword,
                                                                  Settings.Default.AnalysisPortalApplicationIdentifier,
                                                                  false);
            Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Logout()
        {
            WebClientInformation clientInformation;
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.AnalysisService.Login(Settings.Default.TestUserName,
                                                                      Settings.Default.TestPassword,
                                                                      Settings.Default.AnalysisPortalApplicationIdentifier,
                                                                      false);
            Assert.IsNotNull(loginResponse);
            clientInformation = new WebClientInformation();
            clientInformation.Token = loginResponse.Token;
            WebServiceProxy.AnalysisService.Logout(clientInformation);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void StartTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.AnalysisService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.AnalysisService.StopTrace(GetClientInformation());
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void StopTrace()
        {
            TestInitialize(Settings.Default.UserAdminApplicationIdentifier);
            WebServiceProxy.AnalysisService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.AnalysisService.StopTrace(GetClientInformation());
        }

#endregion


        #region GetGridCellSpeciesCounts

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesCountsTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 5000;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<WebGridCellSpeciesCount> noOfGridCellObservations = WebServiceProxy.AnalysisService.GetGridSpeciesCounts(GetClientInformation(), searchCriteria, webGridSpecification, coordinateSystem);
           
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.X.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.Y.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].Size == 5000);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.Rt90_25_gon_v.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.LinearRings[0].Points[0].X.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.LinearRings[0].Points[2].X.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.LinearRings[0].Points[0].Y.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.LinearRings[0].Points[2].Y.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesObservationCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesObservationCount >= noOfGridCellObservations[0].SpeciesCount);

        }


        #endregion


        #region GetGridCellSpeciesObservationCounts


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 5000;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = WebServiceProxy.AnalysisService.GetGridSpeciesObservationCounts(GetClientInformation(), searchCriteria, webGridSpecification, coordinateSystem);
           
           Assert.IsTrue(noOfGridCellObservations.Count > 0);
           Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.X > 0);
           Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.Y > 0);
           Assert.IsTrue(noOfGridCellObservations[0].Size == 5000);
           //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
           //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.SWEREF99_TM.ToString()));
           Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.LinearRings[0].Points[0].X > 0);
           Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.LinearRings[0].Points[2].X > 0);
           Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.LinearRings[0].Points[0].Y > 0);
           Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.LinearRings[0].Points[2].Y > 0);

        }
        #endregion


        #region GetSpeciesObservationCountBySearchCriteria

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteria()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");

            Int64 noOfObservations = WebServiceProxy.AnalysisService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteria_UsesIndividualCountFieldSearchCriteria_ExpectedObservations()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(208245); // vanlig padda
            searchCriteria.IncludePositiveObservations = true;
            SetIndividualCountFieldSearchCriteria(searchCriteria);

            Int64 noOfObservations = WebServiceProxy.AnalysisService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        private static void SetIndividualCountFieldSearchCriteria(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Occurrence);
            fieldSearchCriteria.Operator = CompareOperator.LessOrEqual;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.IndividualCount);
            fieldSearchCriteria.Type = WebDataType.Int32;
            fieldSearchCriteria.Value = "10";
            fieldSearchCriterias.Add(fieldSearchCriteria);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }
#endif

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetDarwinCoreBySearchCriteriaPage_Taxon_Region()
        {
            WebCoordinateSystem coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria;


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(4000072); //insekter

       

            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;

            // Test one region.
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Blekinge);

            Int64 noOfObservations = WebServiceProxy.AnalysisService.GetSpeciesObservationCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }
#endif
        #endregion


        #region GetSpeciesCountBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesCountBySearchCriteria()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            Int64 noOfObservations = WebServiceProxy.AnalysisService.GetSpeciesCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesCountBySearchCriteria_UsingOwnerFieldSearchCriteria_ExpectSpeciesCountMoreThanZero()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            SetOwnerFieldSearchCriterias(searchCriteria);

            Int64 noOfObservations = WebServiceProxy.AnalysisService.GetSpeciesCountBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        #endregion

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceProxy.AnalysisService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceProxy.AnalysisService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }


        #region GetTimeSpeciesObservationCounts

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetMonthlyTimeSpeciesObservationCounts()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            Periodicity type = Periodicity.Monthly;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            List<WebTimeStepSpeciesObservationCount> timeSerie = 
                WebServiceProxy.AnalysisService.GetTimeSpeciesObservationCounts(GetClientInformation(), searchCriteria, type, coordinateSystem);
            Assert.IsTrue(timeSerie.Count > 0);
            //Assert.AreEqual(timeSerie[0].Periodicity, type);
            Assert.IsTrue(timeSerie[0].Name.Length > 2);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetMonthOfTheYearTimeSpeciesObservationCounts()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            Periodicity type = Periodicity.MonthOfTheYear;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            List<WebTimeStepSpeciesObservationCount> timeSerie =
                WebServiceProxy.AnalysisService.GetTimeSpeciesObservationCounts(GetClientInformation(), searchCriteria, type, coordinateSystem);
            Assert.IsTrue(timeSerie.Count > 0);
            //Assert.AreEqual(timeSerie[0].Periodicity, type);
            Assert.IsTrue(timeSerie[0].Name.Length > 2);
        }

        #endregion

        #region GetTaxaBySearchCriteria


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetTaxaBySearchCriteriaTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.IncludePositiveObservations = true;

            IList<WebTaxon> taxonList = WebServiceProxy.AnalysisService.GetTaxaBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);
        }

        [TestMethod]
        public void GetTaxaBySpeciesFactSearchCriteria()
        {
            List<WebTaxon> taxa;
            WebSpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(4); // 2015
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.HostIds = new List<Int32>();
            searchCriteria.HostIds.Add(102656); // Hedsidenbi.
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add(9); // Ungar (juveniler)
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            searchCriteria.IndividualCategoryIds.Add(10); // Vuxna (imago).
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(4); // 2015
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            searchCriteria.PeriodIds.Add(5); // 2013 HELCOM
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(4); // 2015
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add(10); // Vuxna (imago).
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            taxa = WebServiceProxy.AnalysisService.GetTaxaBySpeciesFactSearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetTaxaWithSpeciesObservationCountsBySearchCriteriaTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.IncludePositiveObservations = true;

            IList<WebTaxonSpeciesObservationCount> taxonList = WebServiceProxy.AnalysisService.GetTaxaWithSpeciesObservationCountsBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);
        }
        #endregion

        #region GetGridCellFeatureStatistics
       
        [Ignore]
        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void GetGridCellFeatureStatisticsWithCompleteUrlTest()
        {
            // Test accuracy
            String featuresUrl;
            WebCoordinateSystem coordinateSystem;

            WebFeatureStatisticsSpecification featureStatistics;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();  
            coordinateSystem = new WebCoordinateSystem();
            featureStatistics = new WebFeatureStatisticsSpecification();
            
            

            gridSpecification.BoundingBox = new WebBoundingBox();
            gridSpecification.BoundingBox.Max = new WebPoint();
            gridSpecification.BoundingBox.Min = new WebPoint();

            //Defined in RT90, X=Northing, Y=Easting
            //This bounding box is parsed and extracted from the url?            

            gridSpecification.BoundingBox.Max.Y = 1489104;
            gridSpecification.BoundingBox.Max.X = 6858363;
            gridSpecification.BoundingBox.Min.Y = 1400000;
            gridSpecification.BoundingBox.Min.X = 6800000;

            gridSpecification.GridCellSize = 100000;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;

            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:MapOfSwedishCounties&srsName=EPSG:3021";

            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                               WebServiceProxy.AnalysisService.GetGridCellFeatureStatistics(GetClientInformation(), featureStatistics, featuresUrl, null, //typeName, 
                                                                         gridSpecification, coordinateSystem);


            Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
            Assert.IsTrue(gridCellFeatureStatistics.Count.Equals(4));
            Assert.IsTrue(gridCellFeatureStatistics[0].BoundingBox.LinearRings[0].Points[0].Y.Equals(1400000));
            Assert.IsTrue(gridCellFeatureStatistics[3].BoundingBox.LinearRings[0].Points[2].X.Equals(7000000));

        }
        //public void GetGridCellFeatureStatisticsWithCompleteUrlTest()
        //{
        //    // Test accuracy
        //    String featuresUrl;
        //    WebCoordinateSystem coordinateSystem;
        //    //WfsTypeName typeName;
        //    WebFeatureStatisticsSpecification featureStatisticsSpecification;
        //    WebGridSpecification gridSpecification;

        //    gridSpecification = new WebGridSpecification();
        //    coordinateSystem = new WebCoordinateSystem();
        //    featureStatisticsSpecification = new WebFeatureStatisticsSpecification();
            

        //    featureStatisticsSpecification.BoundingBox = new WebBoundingBox();
        //    featureStatisticsSpecification.BoundingBox.Max = new WebPoint();
        //    featureStatisticsSpecification.BoundingBox.Min = new WebPoint();

        //    gridSpecification.BoundingBox = new WebBoundingBox();
        //    gridSpecification.BoundingBox.Max = new WebPoint();
        //    gridSpecification.BoundingBox.Min = new WebPoint();

        //    featureStatisticsSpecification.BoundingBox.Max.X = 1521024; //= RT90 Y
        //    featureStatisticsSpecification.BoundingBox.Max.Y = 6937341; //= RT90 X
        //    featureStatisticsSpecification.BoundingBox.Min.X = 1457184;
        //    featureStatisticsSpecification.BoundingBox.Min.Y = 6875163;

        //    gridSpecification.BoundingBox.Max.X = 1489104;
        //    gridSpecification.BoundingBox.Max.Y = 6858363;
        //    gridSpecification.BoundingBox.Min.X = 1456064;
        //    gridSpecification.BoundingBox.Min.Y = 6842683;

        //    gridSpecification.GridCellSize = 10000;
        //    gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;
        //    gridSpecification.IsGridCellSizeSpecified = true;
        //    coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

        //    featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:MapOfSwedishCounties&srs=3857";
        //    //&Filter><BBOX><PropertyName>the_geom</ogc:PropertyName><gml:coordinates>6400000, 1400000, 6500000, 6500000</gml:coordinates></gml:Box></BBOX></Filter>";


        //    IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
        //                    WebServiceProxy.AnalysisService.GetGridCellFeatureStatistics(GetClientInformation(), featureStatisticsSpecification, featuresUrl, //typeName, 
        //                                                              gridSpecification, coordinateSystem);


        //    Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
        //    Assert.IsTrue(gridCellFeatureStatistics.Count.Equals(70));
        //    Assert.IsTrue(gridCellFeatureStatistics[0].BoundingBox.LinearRings[0].Points[0].X.Equals(1456064));
        //    Assert.IsTrue(gridCellFeatureStatistics[69].BoundingBox.LinearRings[0].Points[2].Y.Equals(6942683));

        //}
        [Ignore]
        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void GetGridCellFeatureStatisticsWithCompleteUrlTestAndTransformCoordinates()
        {
            // Test accuracy
            String featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebFeatureStatisticsSpecification featureStatisticsSpecification;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            featureStatisticsSpecification = new WebFeatureStatisticsSpecification();            

            gridSpecification.BoundingBox = new WebBoundingBox();
            gridSpecification.BoundingBox.Max = new WebPoint();
            gridSpecification.BoundingBox.Min = new WebPoint();            

            gridSpecification.GridCellSize = 1000;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            //Set returning coordinate system to something different to test the transformations
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            featuresUrl =
                "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:MapOfSwedishCounties&srsName=EPSG:3006";
                //&srs=3006";

            //IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
            //                 AnalysisManager.GetGridCellFeatureStatistics(Context, featureStatistics, featuresUrl, //typeName, 
            //                                                          gridSpecification, coordinateSystem //,isCompleteUrl
            //                                                          );

            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                WebServiceProxy.AnalysisService.GetGridCellFeatureStatistics(GetClientInformation(),
                                                                             featureStatisticsSpecification,
                                                                             featuresUrl,
                                                                             null,
                                                                             gridSpecification,
                                                                             coordinateSystem);

            double y1 = gridCellFeatureStatistics[0].BoundingBox.LinearRings[0].Points[0].Y;
            double y2 = gridCellFeatureStatistics[0].BoundingBox.LinearRings[0].Points[2].Y;
            double y3 = y1 + ((y2 - y1)/2); //371
            double y4 = gridCellFeatureStatistics[1].CentreCoordinate.Y;
            Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
            Assert.IsTrue(gridCellFeatureStatistics.Count.Equals(100));

            // Assert.IsTrue(y4.Equals(y3));

        }

        #endregion

        #region GetProvenancesBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetProvenancesBySearchCriteria_UsesTaxonIds_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");

            // Act
            var actual = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetProvenancesBySearchCriteria_UsesOwnerFieldSearchCriteria_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");

            SetOwnerFieldSearchCriterias(searchCriteria);

            // Act
            var actual = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetProvenancesBySearchCriteria_UsesOrCombinedFieldSearchCriterias_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            SetOrCombinedFieldSearchCriterias(searchCriteria);

            // Act
            var actual = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetProvenancesBySearchCriteria_DefaultDisplayLanguageIsUsed_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:6");
            searchCriteria.TaxonIds = null; // use no taxon ids

            // Act
            var actual = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetProvenancesBySearchCriteria_UseDisplayLanguageSwedish_ExpectsListAndNoException()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:6");
            GetClientInformation().Locale.Id = 175; // Swedish
            searchCriteria.TaxonIds = null; // use no taxon ids

            // Act
            var actual = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetProvenancesBySearchCriteria_UseDisplayLanguageEnglish_ExpectsListAndNoException()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:6");
            GetClientInformation().Locale.Id = 49; // English
            searchCriteria.TaxonIds = null; // use no taxon ids

            // Act
            var actual = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetProvenancesBySearchCriteria_UseDisplayLanguageNonExisting_ExpectsListAndNoException()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:6");
            GetClientInformation().Locale.Id = -1; // non-existing language
            searchCriteria.TaxonIds = null; // use no taxon ids

            // Act
            var actual = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetProvenancesBySearchCriteria_UsesCoordinateUncertaintyInMetersFieldSearchCriteria_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<String>();
            searchCriteria.DataProviderGuids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");

            SetCoordinateUncertaintyInMetersFieldSearchCriteria(searchCriteria);

            // Act
            var actual = WebServiceProxy.AnalysisService.GetProvenancesBySearchCriteria(GetClientInformation(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        #endregion

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetWebServiceName()
        {
            String webServiceName;

            webServiceName = WebServiceProxy.AnalysisService.GetWebServiceName();
            Assert.AreEqual(webServiceName, ApplicationIdentifier.AnalysisService.ToString());
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceProxy.AnalysisService.Ping();
            Assert.IsTrue(ping);
        }
        #region Helper methods

        private static void SetDefaultSearchCriteria(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 08, 01);
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
#endif
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = false;
        }

        private static void SetOwnerFieldSearchCriterias(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias;
            fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();

            SetOwnerFieldSearchCriteria(fieldSearchCriterias);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        private static void SetOwnerFieldSearchCriteria(List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.DarwinCore);
            //fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Owner);
            fieldSearchCriteria.Type = WebDataType.String;
            //fieldSearchCriteria.Value = "Länsstyrelsen Östergötland";
            //fieldSearchCriteria.Value = "Per Flodin";
            fieldSearchCriteria.Value = "%Flodin";

          

            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        private static void SetOrCombinedFieldSearchCriterias(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias;
            fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();

            SetHabitatFieldSearchCriteria(fieldSearchCriterias);
            SetSubstrateFieldSearchCriteria(fieldSearchCriterias);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;

            if (searchCriteria.DataFields.IsNull())
            {
                searchCriteria.DataFields = new List<WebDataField>();
            }

            searchCriteria.DataFields.SetString("FieldLogicalOperator", LogicalOperator.Or.ToString());
        }

        private static void SetHabitatFieldSearchCriteria(List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Event);
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Habitat);
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "%Bokskog";
            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        private static void SetSubstrateFieldSearchCriteria(List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Occurrence);
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.Substrate);
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "%Bokskog";
            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        private static void SetCoordinateUncertaintyInMetersFieldSearchCriteria(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Location);
            fieldSearchCriteria.Operator = CompareOperator.LessOrEqual;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.CoordinateUncertaintyInMeters);
            fieldSearchCriteria.Type = WebDataType.Float64;
            fieldSearchCriteria.Value = "10000";
            fieldSearchCriterias.Add(fieldSearchCriteria);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        #endregion
#endif
    }
}
