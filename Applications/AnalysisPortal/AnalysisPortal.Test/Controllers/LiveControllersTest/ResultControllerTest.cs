using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using AnalysisPortal.Controllers;
using AnalysisPortal.Helpers;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable once CheckNamespace
namespace AnalysisPortal.Tests
{
    using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
    using Microsoft.QualityTools.Testing.Fakes;

    using TaxonId = ArtDatabanken.Data.TaxonId;

    /// <summary>
    /// This is a test class for ResultControllerTest and is intended
    /// to contain all ResultControllerTest Unit Tests.
    /// </summary>
    [TestClass]
    public class ResultControllerTest : DBTestControllerBaseTest
    {
        #region Additional test attributes
        // You can use the following additional attributes as you write your tests:
        // 
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext)
        // {
        // }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize()
        // {
        // }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        #endregion

        /// <summary>
        /// A test for ResultController Constructor.
        /// </summary>
        [TestCategory("NightlyTestApp")]
        [TestMethod]
        public void ResultControllerConstructorTest()
        {
            ResultController target = new ResultController();
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for testing observation in table. 
        /// Requirement VR1.
        /// </summary>
        [TestCategory("NightlyTestApp")] [TestMethod]
        public void TableGetTest()
        {
            // Arrange
            // Set language to swedish since reference data is on that language
            SetSwedishLanguage();

            ResultController resultController = new ResultController();
            var taxaIds = new ObservableCollection<int> { 100573, Convert.ToInt32(TaxonId.Butterflies) };
            SessionHandler.MySettings = new MySettings();
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;
            SessionHandler.MySettings.Presentation.Table.IsActive = true;

            // Act
            var viewResult = resultController.Tables();
            Assert.IsNotNull(viewResult);

            var result = resultController.SpeciesObservationTable();
            Assert.IsNotNull(result);

            var obsResult = resultController.GetPagedObservationListAsJSON(1, 0, 25);
            Assert.IsNotNull(obsResult);

            JsonModel jsonResult = (JsonModel)obsResult.Data;
            List<Dictionary<string, string>> observationListResult = (List<Dictionary<string, string>>)jsonResult.Data;

            // Assert
            Assert.IsNotNull(jsonResult, "jsonResult is null");
            Assert.IsTrue(jsonResult.Success, "jsonResult.Success is not true");
            var data = jsonResult.Data as IList;
            Assert.IsNotNull(data, "jsonResult.Data is null");
            Assert.IsTrue(data.Count >= 0, "jsonResult.Data.Count is not gerater than 0");
            Assert.IsTrue(observationListResult.Count >= 0, "List<Dictionary<string, string>> observationListResult.Count is not greater than 0");
            bool testPerformed = false;
            foreach (Dictionary<string, string> item in observationListResult)
            {
                if (item.ContainsKey("VernacularName"))
                {
                    if (item.Any(keyValuePair => keyValuePair.Key.Equals("VernacularName") && keyValuePair.Value.Equals("griffelblomfluga")))
                    {
                        BaseDataTest.VerifyObservationDataForGriffelblomfluga1000573(observationListResult);
                        testPerformed = true;
                    }
                }

                if (testPerformed)
                {
                    break;
                }
            }
            
            // Reset to english language
            SetEnglishLanguage();
        }
  
        /// <summary>
        /// A test for testing observation in table. 
        /// Requirement VR1 and TBD.
        /// </summary>
        [TestMethod][TestCategory("NightlyTestApp")]
        public void TableGetNoDataTest()
        {
            // Arrange
            ResultController resultController = new ResultController();
            SessionHandler.MySettings = new MySettings();
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(2000000000);
  
            // Act
            var result = resultController.SpeciesObservationTable();
            Assert.IsNotNull(result);

            var obsResult = resultController.GetPagedObservationListAsJSON(1, 0, 25);
            Assert.IsNotNull(obsResult);

            JsonModel jsonResult = (JsonModel)obsResult.Data;
            List<Dictionary<string, string>> observationListResult = (List<Dictionary<string, string>>)jsonResult.Data;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.Success);
            Assert.IsTrue(jsonResult.Total == 0);
            Assert.IsNotNull(observationListResult);
        }

        /// <summary>
        ///  A test for testing observation in table when exception occurs. 
        ///  Requirement VR1 and TBD.
        /// </summary>
        [TestMethod] [TestCategory("NightlyTestApp")]
        public void TableUnexpectedErrorTest()
        {
            // Arrange
                ResultController resultController = new ResultController();
            var taxaIds = new ObservableCollection<int> { 100573 };
                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
                SessionHandler.MySettings.Filter.Taxa.IsActive = true;
                SessionHandler.MySettings.Presentation.Table.IsActive = true;

            // Act
                var result = resultController.SpeciesObservationTable();
                MakeGetCurrentUserFunctionCallThrowException();  
            var obsResult = resultController.GetPagedObservationListAsJSON(1, 0, 25);
                
                JsonModel jsonResult = (JsonModel)obsResult.Data;
                
            // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(obsResult);
                Assert.IsFalse(jsonResult.Success);
                Assert.IsNull(jsonResult.Data);
                Assert.AreEqual("Login to UserService or other service failed.", jsonResult.Msg);
        }

        /// <summary>
        /// Test of map.
        /// </summary>
        [TestMethod] 
        [TestCategory("NightlyTestApp")]
        public void MapSuccessTest()
        {
            ResultController resultController;
            JsonNetResult result;
            JsonModel jsonResult;
            SpeciesObservationsGeoJsonModel geoJsonModel;

            resultController = new ResultController();
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int> { 100573 };
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;

            // TODO add more tests on returning view model is null....
            var viewResult = resultController.Maps();
           
            var viewObsResult = resultController.SpeciesObservationMap();
            var model = viewObsResult.ViewData.Model;

            // Assert.IsNotNull(viewResult);
            result = resultController.GetObservationsAsGeoJSON();
            jsonResult = (JsonModel)result.Data;
            geoJsonModel = (SpeciesObservationsGeoJsonModel)jsonResult.Data;

            Assert.IsNotNull(model);
            Assert.IsNotNull(viewResult);
            Assert.IsTrue(jsonResult.Success);
            Assert.IsNotNull(geoJsonModel);
            Assert.IsTrue(geoJsonModel.Points.Features.Count > 0);
        }

        /// <summary>
        /// Teat of Map when no taxa is selected.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void MapNoTaxaSelectedTest()
        {
                ResultController resultController;
                JsonNetResult result;
                JsonModel jsonResult;

                resultController = new ResultController();
                SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>();

                result = resultController.GetObservationsAsGeoJSON();
                jsonResult = (JsonModel)result.Data;

                Assert.IsFalse(jsonResult.Success);
                Assert.IsNull(jsonResult.Data);
        }

        /// <summary>
        /// Map test of unexpected error.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void MapUnexpectedErrorTest()
        {
                ResultController resultController;
                JsonNetResult result;
                JsonModel jsonResult;
                MakeGetCurrentUserFunctionCallThrowException();

                resultController = new ResultController();
                SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int> { 100573 };
                result = resultController.GetObservationsAsGeoJSON();
                jsonResult = (JsonModel)result.Data;
                Assert.IsFalse(jsonResult.Success);
                Assert.IsNull(jsonResult.Data);
                Assert.AreEqual("Login to UserService or other service failed.", jsonResult.Msg);
        }

        /// <summary>
        /// Test of getting filters.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        // ReSharper disable once InconsistentNaming
        public void GetWfsFiltersAsGeoJSONTest()
        {
                ResultController resultController;
                JsonNetResult result;
                JsonModel jsonResult;
                List<WfsLayerViewModel> wfsLayersResult;

                 var layersSetting = new WfsLayerSetting
            {
                Name = "Småland & Östergötland",
                GeometryType = GeometryType.Polygon,
                Color = "#CCCCFF",
                Filter = "<Filter><Or><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>5</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>6</Literal></PropertyIsEqualTo></Or></Filter>",
                ServerUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs",
                TypeName = "SLW:MapOfSwedishCounties",
                GeometryName = "the_geom"
            };
          
           var layersSetting2 = new WfsLayerSetting
            {
                Name = "Blekinge",
                GeometryType = GeometryType.Polygon,
                Color = "#336699",
                Filter = "<Filter><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>10</Literal></PropertyIsEqualTo></Filter>",
                ServerUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs",
                TypeName = "SLW:MapOfSwedishCounties",
                GeometryName = "the_geom"
            };
           
                resultController = new ResultController();
                SessionHandler.MySettings.DataProvider.MapLayers = new MapLayersSetting();
                SessionHandler.MySettings.DataProvider.MapLayers.AddWfsLayer(layersSetting);
                SessionHandler.MySettings.DataProvider.MapLayers.AddWfsLayer(layersSetting2);
              
                result = resultController.GetWfsLayersAsJSON();
                jsonResult = (JsonModel)result.Data;
                wfsLayersResult = (List<WfsLayerViewModel>)jsonResult.Data;

                Assert.IsTrue(jsonResult.Success);
                Assert.IsNotNull(wfsLayersResult);
                Assert.IsTrue(wfsLayersResult.Count >= 2);
               
                Assert.IsTrue(wfsLayersResult.Last().Id == layersSetting2.Id);
                Assert.IsTrue(wfsLayersResult.Last().Name.Equals(layersSetting2.Name));
                Assert.IsTrue(wfsLayersResult.Last().Filter.Equals(layersSetting2.Filter));
                Assert.IsTrue(wfsLayersResult.Last().ServerUrl.Equals(layersSetting2.ServerUrl));
                Assert.IsTrue(wfsLayersResult.Last().GeometryName.Equals(layersSetting2.GeometryName));
                Assert.IsTrue(wfsLayersResult.Last().Color.Equals(layersSetting2.Color));
                Assert.IsTrue(wfsLayersResult.Last().TypeName.Equals(layersSetting2.TypeName));
                Assert.IsTrue(wfsLayersResult.Last().GeometryType.Equals(layersSetting2.GeometryType));
        }

        /// <summary>
        /// A test for testing observation in grid table. 
        /// </summary>
        [TestCategory("NightlyTestApp")]
        [TestMethod]
        public void GridStatisticsTableOnSpeciesRichnessGetTest()
        { 
            // Arrange
            ResultController resultController = new ResultController();
            var taxaIds = new ObservableCollection<int> { 100573 };

            SessionHandler.MySettings = new MySettings();
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;
            SessionHandler.MySettings.Presentation.Table.IsActive = true;
            SessionHandler.MySettings.Calculation.GridStatistics.CalculateNumberOfObservations = false;
            SessionHandler.MySettings.Calculation.GridStatistics.CalculateNumberOfTaxa = true;
            SessionHandler.MySettings.Calculation.GridStatistics.GridSize = 50000;
            SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId = (int)GridCoordinateSystem.Rt90_25_gon_v;
            SessionHandler.MySettings.Calculation.GridStatistics.IsActive = true;

            // Act
            // Get Views and model
            var viewResult = resultController.Tables();
            Assert.IsNotNull(viewResult);

            var result = resultController.GridStatisticsTableOnSpeciesRichness() as ViewResult;
            Assert.IsNotNull(result);
            var model = (ResultTaxonGridTableViewModel)result.ViewData.Model;
   
            // Get json result from server
            JsonNetResult taxonResult = resultController.GetTaxonGridCountAsJSON();
            JsonModel jsonResult = (JsonModel)taxonResult.Data;
            TaxonGridResult taxonListResult = (TaxonGridResult)jsonResult.Data;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(model.OriginalCoordinateSystemName.Equals("RT 90"));

            // Todo: should be fixed when naming convention of RT 90 and SWEREF99 is resolved.
            // Assert.IsTrue(model.OriginalCoordinateSystemName.Equals(GridCoordinateSystem.Rt90_25_gon_v.ToString()));
            Assert.IsTrue(model.CoordinateSystemName.Equals("Google Mercator"));

            Assert.IsNotNull(taxonResult);
            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.Success);
            Assert.IsTrue(taxonListResult.Cells.Count > 0);
            Assert.IsTrue(taxonListResult.Cells[0].SpeciesCount > 0);
            Assert.IsTrue(taxonListResult.Cells[0].ObservationCount > 0);

            // Not used Assert.IsNotNull(taxonListResult.Cells[0].CentreCoordinate);
            Assert.IsNotNull(taxonListResult.Cells[0].CentreCoordinateX);
            Assert.IsNotNull(taxonListResult.Cells[0].CentreCoordinateY);
            Assert.IsNotNull(taxonListResult.Cells[0].BoundingBox);
            Assert.IsNotNull(taxonListResult.Cells[0].OriginalCentreCoordinateX);
            Assert.IsNotNull(taxonListResult.Cells[0].OriginalCentreCoordinateY);             
       }

        /// <summary>
        /// A test for testing taxa in grid table. 
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GridStatisticsTableOnSpeciesRichnessGetNoTaxaSetTest()
        {
            // Arrange
            ResultController resultController = new ResultController();
            var taxaIds = new ObservableCollection<int>();
           
            SessionHandler.MySettings = new MySettings();
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;
            SessionHandler.MySettings.Presentation.Table.IsActive = false;
            SessionHandler.MySettings.DataProvider.DataProviders.IsActive = false;
            SessionHandler.MySettings.Calculation.GridStatistics.IsActive = false;
            SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive = false;
            SessionHandler.MySettings.Filter.Occurrence.IsActive = false;
            SessionHandler.MySettings.Filter.Quality.IsActive = false;
            SessionHandler.MySettings.Filter.Spatial.IsActive = false;
            SessionHandler.MySettings.Filter.Temporal.IsActive = false;
            SessionHandler.MySettings.Presentation.Map.IsActive = false;
            SessionHandler.MySettings.Presentation.Report.IsActive = false;
            SessionHandler.MySettings.DataProvider.MapLayers.IsActive = false;

            // Act
            var result = resultController.GridStatisticsTableOnSpeciesRichness();
            Assert.IsNotNull(result);

            var taxonResult = resultController.GetTaxonGridCountAsJSON();
            Assert.IsNotNull(taxonResult);

            JsonModel jsonResult = (JsonModel)taxonResult.Data;
            TaxonGridResult taxonListResult = (TaxonGridResult)jsonResult.Data;

            // Add one taxa
            taxaIds.Add(100573);
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
            
            var taxonResult2 = resultController.GetTaxonGridCountAsJSON();
            Assert.IsNotNull(taxonResult2);

            JsonModel jsonResult2 = (JsonModel)taxonResult2.Data;
            TaxonGridResult taxonListResult2 = (TaxonGridResult)jsonResult2.Data;

            // Assert
            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.Success);
            Assert.IsNotNull(jsonResult.Data);

            // If no taxa is set then all taxa will be returned
            Assert.IsTrue(taxonListResult.Cells.Count > 0);
            Assert.IsTrue(taxonListResult2.Cells.Count > 0);
            Assert.IsTrue(taxonListResult.Cells.Count > taxonListResult2.Cells.Count);
        }

        /// <summary>
        ///  A test for testing taxa in table when exception occurs. 
        /// 
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GridStatisticsTableOnSpeciesRichnessUnexpectedErrorTest()
        {
            // Arrange
                ResultController resultController = new ResultController();
            var taxaIds = new ObservableCollection<int> { 100573 };
                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
                SessionHandler.MySettings.Filter.Taxa.IsActive = true;
                SessionHandler.MySettings.Presentation.Table.IsActive = true;

            // Act
                var result = resultController.GridStatisticsTableOnSpeciesRichness();
                
                MakeGetCurrentUserFunctionCallThrowException();
                var txonResult = resultController.GetTaxonGridCountAsJSON();
                
                JsonModel jsonResult = (JsonModel)txonResult.Data;

            // Assert
                Assert.IsNotNull(txonResult);
                Assert.IsNotNull(result);
                Assert.IsFalse(jsonResult.Success);
                Assert.IsNull(jsonResult.Data);
                Assert.AreEqual("Login to UserService or other service failed.", jsonResult.Msg);
        }

        /// <summary>
        /// A test for testing summary statistics report. 
        /// </summary>
        [TestCategory("NightlyTestApp")]
        [TestMethod]
        public void SummaryStatisticsGetTest()
        {
            // Arrange
            ResultController resultController = new ResultController();
            
            SessionHandler.MySettings = new MySettings();
            SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = true;
            SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData = true;
            SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive = true;
            IList<int> taxonIds = new List<int>();
            taxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            SessionHandler.MySettings.Filter.Taxa.AddTaxonIds(taxonIds);

            // Act
            // Get View 
            var viewResult = resultController.Reports();

            var result = resultController.SummaryStatisticsReport() as ViewResult;
           
            // Get json result from server
            JsonNetResult statResult = resultController.GetObservationsSummaryCountAsJSON();
            JsonModel jsonResult = (JsonModel)statResult.Data;
            List<KeyValuePair<string, string>> statisticsResult = (List<KeyValuePair<string, string>>)jsonResult.Data;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("SummaryStatisticsReport"));
           
            Assert.IsNotNull(statResult);
            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.Success);
            Assert.IsTrue(statisticsResult.Count == 2);
            Assert.IsTrue(statisticsResult[0].Key != string.Empty);
            Assert.IsTrue(Convert.ToInt32(statisticsResult[0].Value) > 0);
            Assert.IsTrue(statisticsResult[1].Key != string.Empty);
            Assert.IsTrue(Convert.ToInt32(statisticsResult[1].Value) > 0);
        }

        /// <summary>
        /// A test for SummaryStatistics report. 
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SummaryStatisticsNoDataTest()
        {
            // Arrange
            ResultController resultController = new ResultController();
           
            SessionHandler.MySettings = new MySettings();
            SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = false;
            SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData = false;
            SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive = true;

            // Act
            // Get View 
            var result = resultController.SummaryStatisticsReport() as ViewResult;

            // Get json result from server
            JsonNetResult statResult = resultController.GetObservationsSummaryCountAsJSON();
            JsonModel jsonResult = (JsonModel)statResult.Data;
            List<KeyValuePair<string, string>> statisticsResult = (List<KeyValuePair<string, string>>)jsonResult.Data;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ViewName.Equals("SummaryStatisticsReport"));

            Assert.IsNotNull(statResult);
            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.Success);
            Assert.IsTrue(statisticsResult.Count == 0);
        }

        /// <summary>
        ///  A test for SummaryStatistics report on error.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SummaryStatisticsErrorTest()
        {
            // Arrange
                ResultController resultController = new ResultController();

                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = true;
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData = true;
                SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive = true;

            // Act
                MakeGetCurrentUserFunctionCallThrowException();

                // Get View 
                var result = resultController.SummaryStatisticsReport() as ViewResult;

                // Get json result from server
                JsonNetResult statResult = resultController.GetObservationsSummaryCountAsJSON();
                JsonModel jsonResult = (JsonModel)statResult.Data;

            // List<KeyValuePair<string, string>> statisticsResult = (List<KeyValuePair<string, string>>)jsonResult.Data;

            // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.ViewName.Equals("SummaryStatisticsReport"));
                Assert.AreEqual("Login to UserService or other service failed.", jsonResult.Msg);
        }

        /// <summary>
        /// A test for testing summary statistics per polygon. 
        /// Requirement.
        /// </summary>
        //[TestCategory("NightlyTestApp")]
        [TestCategory("TimeoutNightlyTestApp")]
        [TestMethod]        
        public void SummaryStatisticsPerPolygonGetTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ResultController resultController = new ResultController();

                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = true;
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData = true;
                SessionHandler.MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId = 0;
                SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive = true;

                // Act
                // Get View 
                var viewResult = resultController.Tables();
                var result = resultController.SummaryStatisticsPerPolygonTable();

                // Get json result from server
                JsonNetResult statResult = resultController.GetSummaryStatisticsPerPolygonAsJSON();
                JsonModel jsonResult = (JsonModel)statResult.Data;
                List<SpeciesObservationsCountPerPolygon> statisticsResult = (List<SpeciesObservationsCountPerPolygon>)jsonResult.Data;

                // Assert
                Assert.IsNotNull(viewResult);
                Assert.IsNotNull(result);
                Assert.IsNotNull(statResult);
                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(statisticsResult.Count > 0);
                Assert.IsTrue(statisticsResult[0].Properties.Split(new[] { '\n' }).Length > 0);
                Assert.IsTrue(Convert.ToInt64(statisticsResult[0].SpeciesObservationsCount) > -1);
                Assert.IsTrue(Convert.ToInt64(statisticsResult[0].SpeciesCount) > -1);
            }
        }

        /// <summary>
        /// A test for SummaryStatistics report. 
        /// Requirement.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SummaryStatisticsPerPolygonNoDataTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ResultController resultController = new ResultController();

                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = false;
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData = false;
                SessionHandler.MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId = 0;
                SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive = true;

                // Act
                // Get View 
                var result = resultController.SummaryStatisticsPerPolygonTable();

                // Get json result from server
                JsonNetResult statResult = resultController.GetSummaryStatisticsPerPolygonAsJSON();
                JsonModel jsonResult = (JsonModel)statResult.Data;
                List<SpeciesObservationsCountPerPolygon> statisticsResult = (List<SpeciesObservationsCountPerPolygon>)jsonResult.Data;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(statResult);
                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(statisticsResult.Count > 0);
                Assert.IsTrue(statisticsResult[0].Properties.Split(new[] { '\n' }).Length > 0);
                Assert.IsTrue(statisticsResult[0].SpeciesObservationsCount == "-");
                Assert.IsTrue(statisticsResult[0].SpeciesCount == "-");
            }
        }

        /// <summary>
        /// A test for SummaryStatistics report on error.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SummaryStatisticsPerPolygonErrorTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ResultController resultController = new ResultController();

                ShimControllerContextForLogin(true, resultController);

                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfObservationsfromObsData = true;
                SessionHandler.MySettings.Calculation.SummaryStatistics.CalculateNumberOfSpeciesfromObsData = true;
                SessionHandler.MySettings.Calculation.SummaryStatistics.WfsSummaryStatisticsLayerId = 0;
                SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive = true;

                // Act
                // Get View 
                var result = resultController.SummaryStatisticsPerPolygonTable();

                MakeGetCurrentUserFunctionCallThrowException();

                // Get json result from server
                JsonNetResult statResult = resultController.GetObservationsSummaryCountAsJSON();
                JsonModel jsonResult = (JsonModel)statResult.Data;
                List<SpeciesObservationsCountPerPolygon> statisticsResult = (List<SpeciesObservationsCountPerPolygon>)jsonResult.Data;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual("Login to UserService or other service failed.", jsonResult.Msg);
            }
        }

        /// <summary>
        /// A test for species richness.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SpeciesRichnessGridMapSuccessTest()
        {
            // Arrange
            ResultController resultController = new ResultController();
            var taxaIds = new ObservableCollection<int> { 100573 };

            SessionHandler.MySettings = new MySettings();
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;
            SessionHandler.MySettings.Presentation.Table.IsActive = true;
            SessionHandler.MySettings.Calculation.GridStatistics.CalculateNumberOfObservations = false;
            SessionHandler.MySettings.Calculation.GridStatistics.CalculateNumberOfTaxa = true;
            SessionHandler.MySettings.Calculation.GridStatistics.GridSize = 50000;
            SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId = (int)GridCoordinateSystem.Rt90_25_gon_v;
            SessionHandler.MySettings.Calculation.GridStatistics.IsActive = true;

            // Act
            // Get Views and model
            var viewResult = resultController.Maps();
            Assert.IsNotNull(viewResult);

            var result = resultController.SpeciesRichnessGridMap() as ViewResult;
            Assert.IsNotNull(result);
            var viewName = result.ViewName;
   
            // Get json result from server
            JsonNetResult taxonResult = resultController.GetTaxonGridCountAsJSON();
            JsonModel jsonResult = (JsonModel)taxonResult.Data;
            TaxonGridResult taxonMapResult = (TaxonGridResult)jsonResult.Data;

            // Assert
            Assert.IsNotNull(result);
           // Assert.IsTrue(viewName.Equals("SpeciesRichnessGridMap"));

            Assert.IsNotNull(taxonResult);
            Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.Success);
            Assert.IsNotNull(taxonMapResult);
            Assert.IsTrue(taxonMapResult.Cells.Count > 0);
            Assert.IsTrue(taxonMapResult.Cells[0].SpeciesCount > 0);
            Assert.IsTrue(taxonMapResult.Cells[0].ObservationCount > 0);

            // Not used Assert.IsNotNull(taxonListResult.Cells[0].CentreCoordinate);
            Assert.IsNotNull(taxonMapResult.Cells[0].CentreCoordinateX);
            Assert.IsNotNull(taxonMapResult.Cells[0].CentreCoordinateY);
            Assert.IsNotNull(taxonMapResult.Cells[0].BoundingBox);
            Assert.IsNotNull(taxonMapResult.Cells[0].OriginalCentreCoordinateX);
            Assert.IsNotNull(taxonMapResult.Cells[0].OriginalCentreCoordinateY);             

            Assert.IsTrue(taxonMapResult.GridCellCoordinateSystem.Equals(GridCoordinateSystem.Rt90_25_gon_v.ToString()));
        }

        /// <summary>
        /// A test for species richness when no taxa are selected.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SpeciesRichnessGridMapNoTaxaSelectedTest()
        {
           ResultController resultController;
                JsonNetResult result;
                JsonModel jsonResult;
                TaxonGridResult model;

                resultController = new ResultController();
                SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>();

                result = resultController.GetTaxonGridCountAsJSON();
                jsonResult = (JsonModel)result.Data;
                model = jsonResult.Data as TaxonGridResult;

                Assert.IsTrue(jsonResult.Success);
                Assert.IsNotNull(jsonResult.Data);
            Assert.IsNotNull(model);
                Assert.IsTrue(model.Cells.Count > 0);
        }

        /// <summary>
        /// A test for species richness when exception has thrown.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SpeciesRichnessGridMapUnexpectedErrorTest()
        {
                using (ShimsContext.Create())
                {
                    // Arrange
                    ResultController resultController = new ResultController();

                    ShimControllerContextForLogin(true, resultController);

                    var taxaIds = new ObservableCollection<int> { 100573 };
                    SessionHandler.MySettings = new MySettings();
                    SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
                    SessionHandler.MySettings.Filter.Taxa.IsActive = true;
                    SessionHandler.MySettings.Presentation.Table.IsActive = true;

                    // Act
                    var result = resultController.SpeciesObservationMap();

                    MakeGetCurrentUserFunctionCallThrowException();

                    var obsResult = resultController.GetTaxonGridCountAsJSON();

                    JsonModel jsonResult = (JsonModel)obsResult.Data;

                    // Assert
                    Assert.IsNotNull(obsResult);
                    Assert.IsNotNull(result);
                    Assert.IsFalse(jsonResult.Success);
                    Assert.IsNull(jsonResult.Data);
                    Assert.AreEqual("Login to UserService or other service failed.", jsonResult.Msg);
                }
        }

        /// <summary>
        /// A test of getting data from diagram.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void DiagramSuccessTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ResultController resultController = new ResultController();

                ShimControllerContextForLogin(true, resultController);

                var taxaIds = new ObservableCollection<int> { 100573 };

                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
                SessionHandler.MySettings.Filter.Taxa.IsActive = true;

                // IUserContext context = SessionHandler.UserContext;

                // Act
                // Get Views and model
                var overViewResult = resultController.Diagrams();
                var overViewName = overViewResult.ViewName;

                var viewResult = resultController.TimeSeriesHistogramOnSpeciesObservationCounts();
                var viewName = viewResult.ViewName;

                // Get json result from server
                JsonNetResult diagramResult = resultController.GetObservationsDiagramAsJSON(1);
                JsonModel jsonResult = (JsonModel)diagramResult.Data;
                List<KeyValuePair<string, Int64>> diagramListResult = (List<KeyValuePair<string, Int64>>)jsonResult.Data;

                // Assert
                Assert.IsNotNull(overViewResult);
                Assert.IsTrue(overViewName.Equals("Diagrams"));
                Assert.IsNotNull(viewResult);
                Assert.IsTrue(viewName.Equals("TimeSeriesHistogramOnSpeciesObservationCounts"));

                Assert.IsNotNull(diagramResult);
                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.Success);
                Assert.IsNotNull(diagramListResult);
                Assert.IsTrue(diagramListResult.Count > 0);
                Assert.IsNotNull(diagramListResult[0].Key);
                Assert.IsTrue(diagramListResult[0].Value > 0);
            }
        }

        /// <summary>
        /// A test for getting data to Abundance index diagram.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void AbundanceIndexDiagramSuccessTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ResultController resultController = new ResultController();

                ShimControllerContextForLogin(true, resultController);

                var taxaIds = new ObservableCollection<int>
                                  {
                                      Convert.ToInt32(TaxonId.DrumGrasshopper),
                                      Convert.ToInt32(TaxonId.Butterflies)
                                  };

                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
                SessionHandler.MySettings.Filter.Taxa.IsActive = true;

                // Act
                // Get Views and model
                var overViewResult = resultController.Diagrams();
                var overViewName = overViewResult.ViewName;

                var viewResult = resultController.TimeSeriesDiagramOnSpeciesObservationAbundanceIndex() as ViewResult;
                var viewName = viewResult.ViewName;

                // Get json result from server
                JsonNetResult diagramResult = resultController.GetObservationsAbundanceIndexDiagramAsJSON(1, taxaIds[0]);
                JsonModel jsonResult = (JsonModel)diagramResult.Data;
                List<KeyValuePair<string, object>> diagramListResult = (List<KeyValuePair<string, object>>)jsonResult.Data;

                // Assert
                Assert.IsNotNull(overViewResult);
                Assert.IsTrue(overViewName.Equals("Diagrams"));
                Assert.IsNotNull(viewResult);
                Assert.IsTrue(viewName.Equals("TimeSeriesDiagramOnSpeciesObservationAbundanceIndex"));

                Assert.IsNotNull(diagramResult);
                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.Success);
                Assert.IsNotNull(diagramListResult);
                Assert.IsTrue(diagramListResult.Count > 0);
                Assert.IsNotNull(diagramListResult[0].Key);
                Assert.IsNotNull(diagramListResult[0].Value);
            }
        }

        /// <summary>
        /// A test for getting diagram when no taxa are selected.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void DiagramNoTaxaSelectedTest()
        {
            ResultController resultController;
            JsonNetResult result;
            JsonModel jsonResult;
            List<KeyValuePair<string, Int64>> diagramResult;

            resultController = new ResultController();
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>();

            result = resultController.GetObservationsDiagramAsJSON(1);
            jsonResult = (JsonModel)result.Data;
            diagramResult = jsonResult.Data as List<KeyValuePair<string, Int64>>;

            Assert.IsTrue(jsonResult.Success);
            Assert.IsNotNull(jsonResult.Data);
            Assert.IsNotNull(diagramResult);
            Assert.IsTrue(diagramResult.Count > 0);
        }

        /// <summary>
        /// A test for getting abundance index when no taxa are selected.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void AbundanceIndexDiagramNoTaxaSelectedTest()
        {
            ResultController resultController;
            JsonNetResult result;
            JsonModel jsonResult;
            List<KeyValuePair<string, object>> diagramResult;

            resultController = new ResultController();
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>();

            result = resultController.GetObservationsAbundanceIndexDiagramAsJSON(1, -1);
            jsonResult = (JsonModel)result.Data;
            diagramResult = jsonResult.Data as List<KeyValuePair<string, object>>;

            Assert.IsTrue(jsonResult.Success);
            Assert.IsNotNull(jsonResult.Data);
            Assert.IsNotNull(diagramResult);
            Assert.IsTrue(diagramResult.Count > 0);
        }

        /// <summary>
        /// A test for diagram when an unexpected error has thrown.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void DiagramUnexpectedErrorTest()
        {
                using (ShimsContext.Create())
                {
                // Arrange
                    ResultController resultController = new ResultController();

                    ShimControllerContextForLogin(true, resultController);

                var taxaIds = new ObservableCollection<int> { 100573 };
                    SessionHandler.MySettings = new MySettings();
                    SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
                    SessionHandler.MySettings.Filter.Taxa.IsActive = true;
                    SessionHandler.MySettings.Presentation.Table.IsActive = true;

                // Act
                    var result = resultController.Diagrams();

                    MakeGetCurrentUserFunctionCallThrowException();
                    var obsResult = resultController.GetObservationsDiagramAsJSON(1);

                    JsonModel jsonResult = (JsonModel)obsResult.Data;

                // Assert
                    Assert.IsNotNull(obsResult);
                    Assert.IsNotNull(result);
                    Assert.IsFalse(jsonResult.Success);
                    Assert.IsNull(jsonResult.Data);
                    Assert.AreEqual("Login to UserService or other service failed.", jsonResult.Msg);
                }
        }

        /// <summary>
        /// A test for abundance index diagram when an unexpected error has thrown.
        /// </summary>
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void AbundanceIndexDiagramUnexpectedErrorTest()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                ResultController resultController = new ResultController();

                ShimControllerContextForLogin(true, resultController);
                var taxaIds = new ObservableCollection<int> { 100573 };
                SessionHandler.MySettings = new MySettings();
                SessionHandler.MySettings.Filter.Taxa.TaxonIds = taxaIds;
                SessionHandler.MySettings.Filter.Taxa.IsActive = true;
                SessionHandler.MySettings.Presentation.Table.IsActive = true;

                // Act
                var result = resultController.Diagrams();

                MakeGetCurrentUserFunctionCallThrowException();
                var obsResult = resultController.GetObservationsAbundanceIndexDiagramAsJSON(1, taxaIds[0]);

                JsonModel jsonResult = (JsonModel)obsResult.Data;

                // Assert
                Assert.IsNotNull(obsResult);
                Assert.IsNotNull(result);
                Assert.IsFalse(jsonResult.Success);
                Assert.IsNull(jsonResult.Data);
                Assert.AreEqual("Login to UserService or other service failed.", jsonResult.Msg);
            }
        }
    }
}
