using System.Collections.Generic;
using AnalysisPortal.Controllers;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.Enums;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using System;
using AnalysisPortal.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Polygon = ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon;

namespace AnalysisPortal.Tests
{
    using ArtDatabanken.GIS.GeoJSON.Net.Feature;
    using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
    using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Field;

    /// <summary>
    ///This is a test class for FilterControllerTest and is intended
    ///to contain all FilterControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FilterControllerTest : DBTestControllerBaseTest
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
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        
        //[TestInitialize()]
        //public void TestInitialize()
        //{
        //    SessionHandler.MySettings = new MySettings();
        //    SessionHandler.Language = Thread.CurrentThread.CurrentUICulture.Name;
        //}


        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void GetWfsFilterTest()
        {
            using (ShimsContext.Create())
            {
                DataController dataProvidersController;
                JsonNetResult result;
                JsonModel jsonResult;
                WfsLayerViewModel wfsLayerResult;

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

                dataProvidersController = new DataController();
                SessionHandler.MySettings.DataProvider.MapLayers = new MapLayersSetting();
                SessionHandler.MySettings.DataProvider.MapLayers.AddWfsLayer(layersSetting);
                SessionHandler.MySettings.DataProvider.MapLayers.AddWfsLayer(layersSetting2);
                int noOfWfsLayer = SessionHandler.MySettings.DataProvider.MapLayers.WfsLayers.Count;

                result = dataProvidersController.GetWfsLayerSettings(noOfWfsLayer - 2);
                jsonResult = (JsonModel)result.Data;
                wfsLayerResult = (WfsLayerViewModel)jsonResult.Data;

                Assert.IsTrue(jsonResult.Success);
                Assert.IsNotNull(wfsLayerResult);
                Assert.IsTrue(wfsLayerResult.Name.Equals(layersSetting.Name));
                Assert.IsTrue(wfsLayerResult.Filter.Equals(layersSetting.Filter));
                Assert.IsTrue(wfsLayerResult.ServerUrl.Equals(layersSetting.ServerUrl));
                Assert.IsTrue(wfsLayerResult.GeometryName.Equals(layersSetting.GeometryName));
                Assert.IsTrue(wfsLayerResult.Color.Equals(layersSetting.Color));
                Assert.IsTrue(wfsLayerResult.TypeName.Equals(layersSetting.TypeName));
                Assert.IsTrue(wfsLayerResult.GeometryType.Equals(layersSetting.GeometryType));
            }
        }


        
        /// <summary>
        /// Test add and remove of TaxonIds in MySettings
        /// </summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void AddAndRemoveFilteredTaxaTest()
        {
            using (ShimsContext.Create())
            {                
                int[] taxaIds;
                string strJson;
                FilterController filterController;
                RedirectResult result;
                JsonNetResult result2;
                JsonModel jsonResult;
                List<TaxonViewModel> taxaListResult;

                filterController = new FilterController();
                taxaIds = new int[] { 1, 2, 5 };
                strJson = JsonConvert.SerializeObject(taxaIds);

                ActionResult viewResult = filterController.TaxonFromIds();
                Assert.IsNotNull(viewResult);

                // Test add taxa to MySettings
                result = filterController.AddTaxaToFilter(strJson, "Home/Index");
                Assert.IsTrue(result.Url == "Home/Index");
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(1));
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(2));
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(5));
                Assert.IsFalse(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(3));

                // Test remove taxon from MySettings
                result = filterController.RemoveFilteredTaxon("Home/Index", 2);
                Assert.IsTrue(result.Url == "Home/Index");
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(1));
                Assert.IsTrue(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(5));
                Assert.IsFalse(SessionHandler.MySettings.Filter.Taxa.TaxonIds.Contains(2));


                // Test Get filtered taxa
                result2 = filterController.GetFilteredTaxa();                
                jsonResult = (JsonModel)result2.Data;
                taxaListResult = (List<TaxonViewModel>)jsonResult.Data;
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(jsonResult.Total == 2);
                Assert.IsTrue(taxaListResult.Count == 2);
                int[] taxonIdsInList = new int[] {taxaListResult[0].TaxonId, taxaListResult[1].TaxonId};
                Assert.IsTrue(taxonIdsInList.Contains(1));
                Assert.IsTrue(taxonIdsInList.Contains(5));
                Assert.IsFalse(taxonIdsInList.Contains(2));
                
                // Remove all filtered taxa
                Assert.AreEqual(2, SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count);
                result = filterController.RemoveAllFilteredTaxon("Home/Index");
                Assert.AreEqual("Home/Index", result.Url);
                Assert.AreEqual(0, SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count);


                // Test Get filtered taxa with unexpected error
                MakeGetCurrentUserFunctionCallThrowException();
                result2 = filterController.GetFilteredTaxa();
                jsonResult = (JsonModel)result2.Data;
                Assert.IsFalse(jsonResult.Success);
                Assert.IsNull(jsonResult.Data);                

            }
        }

        /// <summary>
        /// Test to search for taxa in different ways
        /// </summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void GetMatchingTaxaTest()
        {
            using (ShimsContext.Create())
            {
                FilterController filterController;
                JsonNetResult result;
                JsonModel jsonResult;
                List<TaxonViewModel> taxaListResult;
                String strClipboard;

                // Test get data using RowDelimiter.ReturnLinefeed
                filterController = new FilterController();
                strClipboard = "1" + Environment.NewLine + "2" + Environment.NewLine + "4";
                result = filterController.GetMatchingTaxa(strClipboard, RowDelimiter.ReturnLinefeed);
                jsonResult = (JsonModel) result.Data;
                taxaListResult = (List<TaxonViewModel>)jsonResult.Data;                
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(jsonResult.Total == 3);
                Assert.IsTrue(taxaListResult.Count == 3);
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 1));
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 2));
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 4));                


                // Test get data using RowDelimiter.Semicolon
                strClipboard = "1;2;4";
                result = filterController.GetMatchingTaxa(strClipboard, RowDelimiter.Semicolon);
                jsonResult = (JsonModel)result.Data;
                taxaListResult = (List<TaxonViewModel>)jsonResult.Data;
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(jsonResult.Total == 3);
                Assert.IsTrue(taxaListResult.Count == 3);
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 1));
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 2));
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 4));                



                // Test get data using RowDelimiter.Tab                
                strClipboard = "1\t2\t4";
                result = filterController.GetMatchingTaxa(strClipboard, RowDelimiter.Tab);
                jsonResult = (JsonModel)result.Data;
                taxaListResult = (List<TaxonViewModel>)jsonResult.Data;
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(jsonResult.Total == 3);
                Assert.IsTrue(taxaListResult.Count == 3);
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 1));
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 2));
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 4));                


                // Test get data using RowDelimiter.Tab                
                strClipboard = "1|2|4";
                result = filterController.GetMatchingTaxa(strClipboard, RowDelimiter.VerticalBar);
                jsonResult = (JsonModel)result.Data;
                taxaListResult = (List<TaxonViewModel>)jsonResult.Data;
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(jsonResult.Total == 3);
                Assert.IsTrue(taxaListResult.Count == 3);
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 1));
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 2));
                Assert.IsTrue(taxaListResult.Any(x => x.TaxonId == 4));


                // Test get taxon with invalid taxon ids
                filterController = new FilterController();
                strClipboard = "47894789473849784937849;323343443344;-342342";
                result = filterController.GetMatchingTaxa(strClipboard, RowDelimiter.Semicolon);
                jsonResult = (JsonModel)result.Data;
                taxaListResult = (List<TaxonViewModel>)jsonResult.Data;
                Assert.IsTrue(jsonResult.Success);
                Assert.IsTrue(jsonResult.Total == 0);
                Assert.IsTrue(taxaListResult.Count == 0);                

            }
        }


        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void GetMatchingTaxaUnexpectedErrorTest()
        {
            using (ShimsContext.Create())
            {
                FilterController filterController;
                JsonNetResult result;
                JsonModel jsonResult;
                String strClipboard;
                MakeGetCurrentUserFunctionCallThrowException();

                filterController = new FilterController();
                strClipboard = "1;2;4";
                result = filterController.GetMatchingTaxa(strClipboard, RowDelimiter.Semicolon);
                jsonResult = (JsonModel)result.Data;
                
                Assert.IsFalse(jsonResult.Success);
                Assert.IsNull(jsonResult.Data);                

            }
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Search_taxa()
        {
            using (ShimsContext.Create())
            {
                JsonNetResult result;
                JsonModel jsonResult;
                var filterController = new FilterController();
                result = filterController.GetTaxaBySearch("gullviva", SearchStringCompareOperator.Contains, null, null,
                                                          null, null,
                                                          null, null, null);
                jsonResult = (JsonModel) result.Data;
                Assert.IsTrue(jsonResult.Success);
                List<TaxonSearchResultItemViewModel> resultList = (List<TaxonSearchResultItemViewModel>) jsonResult.Data;
                
                Assert.IsTrue(resultList.Count >= 3);
                Assert.IsTrue(resultList[0].SearchMatchName.Contains("gullviva"));

            }
        }




        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Add_and_remove_spatial_filter()
        {
            var filterController = new FilterController();
            Polygon polygon1 = CreateTestPolygon1();
            Polygon polygon2 = CreateTestPolygon2();
            var feature1 = new Feature(polygon1);
            var feature2 = new Feature(polygon2);
            var features = new List<Feature> {feature1, feature2};
            var featureCollection = new FeatureCollection(features);            
            string geojson = JsonConvert.SerializeObject(featureCollection, JsonHelper.GetDefaultJsonSerializerSettings());

            // Update spatial filter
            JsonNetResult result = filterController.UpdateSpatialFilter(geojson);
            JsonModel jsonResult = (JsonModel) result.Data;
            Assert.IsTrue(jsonResult.Success);
            Assert.IsTrue(SessionHandler.MySettings.Filter.Spatial.IsActive == true);
            Assert.IsTrue(SessionHandler.MySettings.Filter.Spatial.Polygons.Count == 2);

            DataPolygon dataPolygon1 = SessionHandler.MySettings.Filter.Spatial.Polygons[0];
            Assert.IsTrue(Math.Abs(dataPolygon1.LinearRings[0].Points[0].X - ((GeographicPosition) polygon1.Coordinates[0].Coordinates[0]).Longitude) < 0.001);
            Assert.IsTrue(Math.Abs(dataPolygon1.LinearRings[0].Points[1].X - ((GeographicPosition)polygon1.Coordinates[0].Coordinates[1]).Longitude) < 0.001);
            Assert.IsTrue(Math.Abs(dataPolygon1.LinearRings[0].Points[2].X - ((GeographicPosition)polygon1.Coordinates[0].Coordinates[2]).Longitude) < 0.001);
            Assert.IsTrue(Math.Abs(dataPolygon1.LinearRings[0].Points[3].X - ((GeographicPosition)polygon1.Coordinates[0].Coordinates[3]).Longitude) < 0.001);
            Assert.IsTrue(Math.Abs(dataPolygon1.LinearRings[0].Points[0].Y - ((GeographicPosition)polygon1.Coordinates[0].Coordinates[0]).Latitude) < 0.001);
            Assert.IsTrue(Math.Abs(dataPolygon1.LinearRings[0].Points[1].Y - ((GeographicPosition)polygon1.Coordinates[0].Coordinates[1]).Latitude) < 0.001);
            Assert.IsTrue(Math.Abs(dataPolygon1.LinearRings[0].Points[2].Y - ((GeographicPosition)polygon1.Coordinates[0].Coordinates[2]).Latitude) < 0.001);
            Assert.IsTrue(Math.Abs(dataPolygon1.LinearRings[0].Points[3].Y - ((GeographicPosition)polygon1.Coordinates[0].Coordinates[3]).Latitude) < 0.001);


            // Get spatial filter
            result = filterController.GetSpatialFilterAsGeoJSON();
            jsonResult = (JsonModel) result.Data;
            Assert.IsTrue(jsonResult.Success);
            FeatureCollection featureCollectionFromServer = (FeatureCollection) jsonResult.Data;             
            Assert.IsTrue(featureCollectionFromServer.Features.Count == 2);


            // Clear spatial filter
            result = filterController.ClearSpatialFilter();
            jsonResult = (JsonModel) result.Data;
            Assert.IsTrue(jsonResult.Success);
            Assert.IsTrue(SessionHandler.MySettings.Filter.Spatial.Polygons.Count == 0);
        }
        


        private Polygon CreateTestPolygon1()
        {
            var lineString = new LineString(new List<GeographicPosition>()
                                               {
                                                   new GeographicPosition(11.0878902207, 45.1602390564),
                                                   new GeographicPosition(15.01953125, 48.1298828125),
                                                   new GeographicPosition(18.01953125, 49.1298828125),
                                                   new GeographicPosition(11.0878902207, 45.1602390564)
                                               });
            var lineStrings = new List<LineString>();
            lineStrings.Add(lineString);
            var polygon = new Polygon(lineStrings);
            return polygon;
        }

        private Polygon CreateTestPolygon2()
        {
            var lineString = new LineString(new List<GeographicPosition>()
                                               {
                                                   new GeographicPosition(10, 11),
                                                   new GeographicPosition(12, 13),
                                                   new GeographicPosition(14, 15),
                                                   new GeographicPosition(10, 11)
                                               });
            var lineStrings = new List<LineString>();
            lineStrings.Add(lineString);
            var polygon = new Polygon(lineStrings);
            return polygon;
        }

        /// <summary>
        /// Test add and remove of fields in MySettings.
        /// </summary>
        [TestMethod()]
        [TestCategory("NightlyTestApp")]
        public void AddAndRemoveFilteredFieldTest()
        {
            using (ShimsContext.Create())
            {
                // Assign
                string strClass;
                string strFieldValue;
                string strFieldId;
                string strFieldName;
                string strCompareOperator;
                string strLogicalOperator;
                FilterController filterController;
                RedirectResult result;
                FieldViewModel model;
                ISpeciesObservationFieldDescription fieldDescription;

                ISpeciesObservationFieldSearchCriteria expectedField =
                    new SpeciesObservationFieldSearchCriteria()
                    {
                        Class = new SpeciesObservationClass() { Id = SpeciesObservationClassId.DarwinCore },
                        Operator = CompareOperator.Like,
                        Property = new SpeciesObservationProperty() { Id = SpeciesObservationPropertyId.Owner },
                        Type = DataType.String,
                        Value = "flodin"
                    };

                filterController = new FilterController();
                strClass = "DarwinCore";
                strFieldValue = "flodin";
                strFieldId = "146";
                strFieldName = "Owner";
                strCompareOperator = "LIKE";
                strLogicalOperator = "OR";

                model = new FieldViewModel();
                model.FieldDescriptionTypes = new FieldViewModel.SpeciesObservationFieldDescriptionType();
                model.FieldDescriptionTypes.FieldDescriptionTypes = new Dictionary<string, List<ISpeciesObservationFieldDescription>>();
                fieldDescription = new SpeciesObservationFieldDescription();
                fieldDescription.Type = DataType.String;
                model.FieldDescriptionTypes.FieldDescriptionTypes.Add(strClass, new List<ISpeciesObservationFieldDescription> { fieldDescription });

                // Act
                ActionResult viewResult = filterController.Field();
                
                // Test add field to MySettings
                result = filterController.AddFieldToFilter(
                    strClass,
                    strFieldValue,
                    strFieldId,
                    strFieldName,
                    strCompareOperator,
                    strLogicalOperator,
                    "Home/Index");

                // Assert
                Assert.IsNotNull(viewResult);
                Assert.IsTrue(result.Url == "Home/Index");
                Assert.AreEqual(expectedField.Type, fieldDescription.Type);
                Assert.IsTrue(SessionHandler.MySettings.Filter.Field.FieldFilterExpressions.Count == 1);
                Assert.IsTrue(SessionHandler.MySettings.Filter.Field.FieldFilterExpressions[0].Class.Id.Equals(expectedField.Class.Id));
                Assert.IsTrue(SessionHandler.MySettings.Filter.Field.FieldFilterExpressions[0].Value.Equals(expectedField.Value));
                Assert.IsTrue(SessionHandler.MySettings.Filter.Field.FieldLogicalOperator == LogicalOperator.Or);

                // Test remove field from MySettings
                result = filterController.ResetFields("Home/Index");
                Assert.IsTrue(result.Url == "Home/Index");
                Assert.IsTrue(SessionHandler.MySettings.Filter.Field.FieldFilterExpressions.Count == 0);
                Assert.IsTrue(SessionHandler.MySettings.Filter.Field.FieldLogicalOperator == LogicalOperator.And);

            }
        }

    }
}
