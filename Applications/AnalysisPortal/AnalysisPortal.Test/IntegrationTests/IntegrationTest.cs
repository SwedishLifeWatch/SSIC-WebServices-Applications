using System.Collections.Generic;
using System.Web.Mvc;
using AnalysisPortal.Controllers;
using AnalysisPortal.Helpers;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Polygon = ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon;

namespace AnalysisPortal.Tests
{
    using Microsoft.QualityTools.Testing.Fakes;

    /// <summary>
    ///This is a test class for Integrationtests to be exceuted 
    /// in the analysis portal
    ///</summary>
    [TestClass()]
    public class IntegrationTest : DBTestControllerBaseTest
    {

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


    

        /// <summary>
        /// An intergation test for viewing map and observations in table. 
        /// Test is performed for logged in user and for different test settings. 
        /// Test for saved user settings and reloaded user settings is also performed.
        /// Requirements VR1, TBD
        ///</summary>
        [TestMethod]
        [TestCategory("AnalysisPortalIntegrationTest")]
        public void ViewObservationsForLoggedInUser()
        {
            using (ShimsContext.Create())
            {
                //Login user
                LoginTestUserAnalyser();
                ShimFilePath();

                // Add another role to user context
                base.AddSightingRoleToUserContext();
                                
                UserRoleModel roleModel = new UserRoleModel();
                IRole newAnalyzerRole = SessionHandler.UserContext.CurrentRoles[0];
                AccountController.ChangeUserRole(Convert.ToString(newAnalyzerRole.Id), roleModel);

                // Verify role
                Assert.IsTrue(SessionHandler.UserContext.CurrentRole.Id == newAnalyzerRole.Id);

                // Set language to swedish since reference data is on that language
                SetSwedishLanguage();

                // Create controllers
                FilterController filterController = new FilterController();
                ResultController resultController = new ResultController();
                StubResultController(resultController);
                MySettingsController settingsController = new MySettingsController();
               
                // TODO Add tests for presentation controller data
                FormatController presentationController = new FormatController();

                //Check that no data is set to mysettings, except for map and table that should be checked by default
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 0);
                Assert.IsTrue(SessionHandler.MySettings.Presentation.Map.IsActive);
                Assert.IsTrue(SessionHandler.MySettings.Presentation.Table.IsActive);
                Assert.IsFalse(SessionHandler.MySettings.Filter.Taxa.IsActive);

                //*********************************   Set and test filter data ***************************
                //Select to filter on taxa and select taxa ids
                //SessionHandler.MySettings.Filter.TaxaSetting.IsSelected = true;

                filterController.RemoveAllFilteredTaxon("Home/Index");
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 0);

                //Use taxon 100573-Griffelblomfluga
                int[] taxaIds = new int[] { 100573 };
                string strJson = JsonConvert.SerializeObject(taxaIds);
                filterController.AddTaxaToFilter(strJson, "Home/Index");
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(100573));
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 1);

                //****************************  Set spatial data ***************************************************
                Polygon polygon1 = CreateTestPolygon1();
                var feature1 = new Feature(polygon1);
                var features = new List<Feature> { feature1};
                var featureCollection = new FeatureCollection(features);
                string geojson = JsonConvert.SerializeObject(featureCollection, JsonHelper.GetDefaultJsonSerializerSettings());

                // Update spatial filter
                JsonNetResult filterResult = filterController.UpdateSpatialFilter(geojson);
                JsonModel jsonFilterResult = (JsonModel)filterResult.Data;
                Assert.IsTrue(jsonFilterResult.Success);
                Assert.IsTrue(SessionHandler.MySettings.Filter.Spatial.IsActive == true);
                Assert.IsTrue(SessionHandler.MySettings.Filter.Spatial.Polygons.Count == 1);

                //**************************** Check map data *******************************************************
                // Sow map view for Griffelblomfluga
                int numberOfObservations = 0;
                ViewResult result = resultController.SpeciesObservationMap();
                Assert.IsNotNull(result);
               
                //Get the map data to be shown
                JsonNetResult obsMap = resultController.GetObservationsAsGeoJSON();
                Assert.IsNotNull(obsMap.Data);
                var jsonMapResult = (JsonModel)obsMap.Data;
                var noObs = (SpeciesObservationsGeoJsonModel)jsonMapResult.Data;
                numberOfObservations = noObs.Points.Features.Count;
                Assert.IsTrue(SessionHandler.MySettings.Presentation.Map.IsActive);
                Assert.IsTrue(SessionHandler.MySettings.Presentation.Table.IsActive);
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.IsActive);

                //************************ Check table data *********************************************
                //Select to show table data from observations
                result = resultController.SpeciesObservationTable() as ViewResult;
                Assert.IsNotNull(result);
                var viewModel = result.ViewData.Model as ViewTableViewModel;
                Assert.IsNotNull(viewModel);
                //Get observations
                var obsResult = resultController.GetPagedObservationListAsJSON(1,0,25);
                Assert.IsNotNull(obsResult);
                JsonModel jsonResult = (JsonModel) obsResult.Data;
                List<Dictionary<string, string>> observationListResult = (List<Dictionary<string, string>>)jsonResult.Data;
               
                //Check observations
                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(jsonResult.Total > 5);
                Assert.IsTrue(observationListResult.Count > 5);
                Assert.IsTrue(numberOfObservations == observationListResult.Count,"Number of observations differs between Map and ObsTable");
                //Verfy observations
                BaseDataTest.VerifyObservationDataForGriffelblomfluga1000573(observationListResult);

                //*********************** Check detailsPanel ****************************************************
                string observationGUID = observationListResult[5]["ObservationId"];
                DetailsController detailsController = new DetailsController();
               
                //Act
                // Returning "hard coded" value för first observation for griffelblomfluga
                var detailsResult = detailsController.ObservationDetail(observationGUID) as ViewResult;

                //Assert
                Assert.IsNotNull(detailsResult);
              
                //*********************** Verfy user settings, save and load *************************************

                // Save settings 
                settingsController.SaveMySettings("Home/Index");

                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count > 0);

                //Logout TestUser
                LogoutTestUser();

                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 0);
                Assert.IsTrue(SessionHandler.MySettings.Presentation.Map.IsActive);
                Assert.IsTrue(SessionHandler.MySettings.Presentation.Table.IsActive);
                Assert.IsFalse(SessionHandler.MySettings.Filter.Taxa.IsActive);

                LoginTestUserAnalyser();
                //Use taxon 100573-Griffelblomfluga
                taxaIds = new int[] { 100573 };
                strJson = JsonConvert.SerializeObject(taxaIds);
                filterController.AddTaxaToFilter(strJson, "Home/Index");
                
                result = resultController.SpeciesObservationTable() as ViewResult;
                Assert.IsNotNull(result);
               
                // Check observations in table
                obsResult = resultController.GetPagedObservationListAsJSON(1,0,25);
                Assert.IsNotNull(obsResult);
                jsonResult = (JsonModel) obsResult.Data;
                observationListResult = (List<Dictionary<string, string>>)jsonResult.Data;
                Assert.IsNotNull(jsonResult);
                Assert.IsTrue(jsonResult.Success);

                Assert.IsTrue(jsonResult.Total > 5);
                Assert.IsTrue(observationListResult.Count > 5);
                Assert.IsTrue(numberOfObservations == observationListResult.Count, "Number of observations differs between saved settings and not saved settings for ObsTable");

                BaseDataTest.VerifyObservationDataForGriffelblomfluga1000573(observationListResult);

 
                // Reset to english language
                SetEnglishLanguage();
           }
        }



        /// <summary>
        /// An intergation test for viewing map and observations in table. 
        /// Test is performed for non logged in user.
        /// Requirements VR1, TBD
        ///</summary>
        [TestMethod]
        [TestCategory("AnalysisPortalIntegrationTest")]
        public void ViewObservations()
        {
            //Arrange
            // Set language to swedish since reference data is on that language
            SetSwedishLanguage();
            FilterController filterController = new FilterController();
            ResultController resultController = new ResultController();
            
            // TODO Add tests for presentation controller data
            FormatController presentationController = new FormatController();

            //Act and Assert
            Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 0);

            filterController.RemoveAllFilteredTaxon("Home/Index");
            Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 0);

            int[] taxaIds = new int[] { 100573 };
            string strJson = JsonConvert.SerializeObject(taxaIds);
            filterController.AddTaxaToFilter(strJson, "Home/Index");
            Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(100573));
            Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count == 1);

            //****************************  Set spatial data ***************************************************

            Polygon polygon1 = CreateTestPolygon1();
            var feature1 = new Feature(polygon1);
            var features = new List<Feature> { feature1 };
            var featureCollection = new FeatureCollection(features);
            string geojson = JsonConvert.SerializeObject(featureCollection, JsonHelper.GetDefaultJsonSerializerSettings());
            // Update spatial filter
            JsonNetResult filterResult = filterController.UpdateSpatialFilter(geojson);
            JsonModel jsonFilterResult = (JsonModel)filterResult.Data;
            Assert.IsTrue(jsonFilterResult.Success);
            Assert.IsTrue(SessionHandler.MySettings.Filter.Spatial.IsActive == true);
            Assert.IsTrue(SessionHandler.MySettings.Filter.Spatial.Polygons.Count == 1);
        
            //**************************** Check map data *******************************************************
            // Sow map view for Griffelblomfluga
            ViewResult pvResult = resultController.SpeciesObservationMap();
            Assert.IsNotNull(pvResult);
            // Test that correct view is returned, Todo: No view is returned, delete?
            // Assert.AreEqual("SpeciesObservationMap", pvResult.ViewName);
            //Get the map data to be shown
            JsonNetResult obsMap = resultController.GetObservationsAsGeoJSON();
            Assert.IsNotNull(obsMap.Data);
            var jsonMapResult = (JsonModel)obsMap.Data;
            var noObs = (SpeciesObservationsGeoJsonModel)jsonMapResult.Data;
            int numberOfObservations = noObs.Points.Features.Count;
            //Check settings
            Assert.IsTrue(SessionHandler.MySettings.Presentation.Map.IsActive);
            Assert.IsFalse(SessionHandler.MySettings.Presentation.Table.IsActive);
            Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.IsActive);

            //************************ Check table data *********************************************

            var result = resultController.SpeciesObservationTable() as ViewResult;
            Assert.IsNotNull(result);
            // Test that correct view is returned
            Assert.AreEqual("Table", result.ViewName);

            var viewModel = result.ViewData.Model as ViewTableViewModel;
            Assert.IsNotNull(viewModel);


            var obsResult = resultController.GetPagedObservationListAsJSON(1,0,25);
            Assert.IsNotNull(obsResult);

            JsonModel jsonResult = (JsonModel)obsResult.Data;
            List<Dictionary<string, string>> observationListResult = (List<Dictionary<string, string>>)jsonResult.Data;

             Assert.IsNotNull(jsonResult);
            Assert.IsTrue(jsonResult.Success);

            Assert.IsTrue(jsonResult.Total > 20);
            Assert.IsTrue(observationListResult.Count > 20);
            Assert.IsTrue(numberOfObservations == observationListResult.Count, "Number of observations differs between Map and ObsTable");
                

            BaseDataTest.VerifyObservationDataForGriffelblomfluga1000573(observationListResult);

            //*********************** Check detailsPanel ****************************************************
            string observationGUID = observationListResult[20]["ObservationId"];
            DetailsController detailsController = new DetailsController();

            //Act
            // Returning "hard coded" value för first observation for griffelblomfluga
            var detailsResult = detailsController.ObservationDetail(observationGUID) as ViewResult;

            //Assert
            Assert.IsNotNull(detailsResult);
            var detailsViewModel = detailsResult.ViewData.Model as ObservationDetailViewModel;

            Assert.IsNotNull(detailsViewModel);
            // Test that correct view is returned
            Assert.AreEqual("Detail", detailsResult.ViewName);

            //Check number of properties
            Assert.IsTrue(detailsViewModel.Fields.Count > 10);


            // Reset to english language
            SetEnglishLanguage();
          
        }

        /// <summary>
        /// A rectangle over the southernparts of Sweden
        /// </summary>
        /// <returns></returns>
        private Polygon CreateTestPolygon1()
        {
            var lineString = new LineString(new List<GeographicPosition>()
                                               {
                                                   new GeographicPosition(1334896.2617864, 7413599.5228778),
                                                   new GeographicPosition(1334896.2617864, 9067085.3185126),
                                                   new GeographicPosition(2249699.6161761, 9067085.3185126),
                                                   new GeographicPosition(2249699.6161761, 7413599.5228778),
                                                   new GeographicPosition(1334896.2617864, 7413599.5228778)
                                               });
            var lineStrings = new List<LineString>();
            lineStrings.Add(lineString);
            var polygon = new Polygon(lineStrings);
            return polygon;
        }
     
    }
 

}
