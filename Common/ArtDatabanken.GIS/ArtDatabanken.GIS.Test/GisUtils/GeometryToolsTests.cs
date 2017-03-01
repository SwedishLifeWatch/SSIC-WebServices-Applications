using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GisUtils;
using ArtDatabanken.GIS.Grid;
using ArtDatabanken.GIS.SwedenExtent;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ArtDatabanken.GIS.Test.GisUtils
{
    [TestClass]
    public class GeometryToolsTests
    {
        [TestMethod]
        public void ValidateGeometry_WhenValidPolygon_ThenValidResult()
        {
            SqlGeometry polygon = SqlGeometry.Parse("POLYGON((1 1, 4 1, 4 4, 1 4, 1 1))");

            GeometryTools geometryTools = new GeometryTools();
            GeometryValidationResult result = geometryTools.ValidateGeometry(polygon);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(24400, result.StatusCode);
            Assert.AreEqual(GeometryValidationStatusId.Valid, result.GeometryValidationStatusId);
            Assert.AreEqual("Valid", result.Description);
        }

        [TestMethod]
        public void ValidateGeometry_WhenInvalidPolygon_ThenInvalidResult()
        {
            SqlGeometry polygon = SqlGeometry.Parse("POLYGON((1 1, 4 1, 1 4, 4 4, 1 1))");

            GeometryTools geometryTools = new GeometryTools();
            GeometryValidationResult result = geometryTools.ValidateGeometry(polygon);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(24404, result.StatusCode);
            Assert.AreEqual(GeometryValidationStatusId.NotValidBecausePolygonRingIntersectsItself, result.GeometryValidationStatusId);
            Assert.AreEqual("Not valid because polygon ring (1) intersects itself or some other ring.", result.Description);
        }


        [TestMethod]
        public void ValidateGeometry_WhenConvertedInvalidPolygon_ThenInvalidResult()
        {
            Polygon pol = GetInvalidPolygon();
            GeometryConversionTool geometryConversionTool = new GeometryConversionTool();
            SqlGeometry polygon = geometryConversionTool.PolygonToSqlGeometry(pol);

            GeometryTools geometryTools = new GeometryTools();
            GeometryValidationResult result = geometryTools.ValidateGeometry(polygon);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(24404, result.StatusCode);
            Assert.AreEqual(GeometryValidationStatusId.NotValidBecausePolygonRingIntersectsItself, result.GeometryValidationStatusId);
            Assert.AreEqual("Not valid because polygon ring (1) intersects itself or some other ring.", result.Description);
        }

        private Polygon GetInvalidPolygon()
        {
            Polygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            LinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1, 1));
            linearRing.Points.Add(new Point(4, 1));
            linearRing.Points.Add(new Point(1, 4));
            linearRing.Points.Add(new Point(4, 4));
            linearRing.Points.Add(new Point(1, 1));
            polygon.LinearRings.Add(linearRing);
            return polygon;
        }

        [TestMethod]
        public void ValidateFeatureGeometry_WhenValidFeatureGeometry_ThenValidResult()
        {
            //Arrange
            const string strFeature = "{ \"type\": \"Feature\", \"id\":\"myFeature\", \"bbox\": [-170.0, -70.0, 160.0, 85.0], \"geometry\": {\"type\": \"Polygon\",\"coordinates\": [[[-180.0, 10.0], [20.0, 90.0], [180.0, -5.0], [-30.0, -90.0],[-180.0, 10.0]]]}}";
            Feature feature = JsonConvert.DeserializeObject<Feature>(strFeature);
            GeometryTools geometryTools = new GeometryTools();

            //Act
            GeometryValidationResult result = geometryTools.ValidateFeatureGeometry(feature);

            //Assert
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(24400, result.StatusCode);
            Assert.AreEqual(GeometryValidationStatusId.Valid, result.GeometryValidationStatusId);
            Assert.AreEqual("Valid", result.Description);
        }

        [TestMethod]
        public void ValidateFeatureGeometry_WhenPolygonIntersectsItself_ThenInvalidResult()
        {
            //Arrange
            const string strFeature = "{ \"type\": \"Feature\", \"id\":\"myFeature\", \"geometry\": {\"type\": \"Polygon\",\"coordinates\": [[[1, 1], [4, 1], [1, 4], [4, 4], [1, 1]]]}}";
            Feature feature = JsonConvert.DeserializeObject<Feature>(strFeature);
            GeometryTools geometryTools = new GeometryTools();

            //Act
            GeometryValidationResult result = geometryTools.ValidateFeatureGeometry(feature);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(24404, result.StatusCode);
            Assert.AreEqual(GeometryValidationStatusId.NotValidBecausePolygonRingIntersectsItself, result.GeometryValidationStatusId);
            Assert.AreEqual("Not valid because polygon ring (1) intersects itself or some other ring.", result.Description);
        }

        [TestMethod]
        public void ValidateFeatureCollectionGeometries_WhenValidFeatureGeometry_ThenValidResult()
        {
            //Arrange
            const string strFeatureCollection = "{ \"type\": \"FeatureCollection\", \"features\": [ { \"type\": \"Feature\", \"geometry\": { \"type\": \"Point\", \"coordinates\": [102.0, 0.5]}, \"properties\": { \"prop0\": \"value0\"}}, { \"type\": \"Feature\", \"geometry\": {\"type\": \"LineString\", \"coordinates\": [[102.0, 0.0], [103.0, 1.0], [104.0, 0.0], [105.0, 1.0]]},\"properties\": {\"prop1\": 0.0,\"prop0\": \"value0\"}},{\"type\": \"Feature\",\"geometry\": {\"type\": \"Polygon\",\"coordinates\": [[[100.0, 0.0], [101.0, 0.0], [101.0, 1.0], [100.0, 1.0],[100.0, 0.0]]]},\"properties\": {\"prop0\": \"value0\"}}]}";
            FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(strFeatureCollection);
            GeometryTools geometryTools = new GeometryTools();

            //Act
            FeatureCollectionValidationResult result = geometryTools.ValidateFeatureCollectionGeometries(featureCollection);

            //Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.InvalidFeatureResults.Any());            
        }

        [TestMethod]
        public void ValidateFeatureCollectionGeometries_WhenInvalidFeatureGeometry_ThenInvalidResult()
        {
            //Arrange
            const string strFeatureCollection = "{ \"type\": \"FeatureCollection\", \"features\": [ { \"type\": \"Feature\", \"geometry\": { \"type\": \"Point\", \"coordinates\": [102.0, 0.5]}, \"properties\": { \"prop0\": \"value0\"}}, { \"type\": \"Feature\", \"geometry\": {\"type\": \"LineString\", \"coordinates\": [[102.0, 0.0], [103.0, 1.0], [104.0, 0.0], [105.0, 1.0]]},\"properties\": {\"prop1\": 0.0,\"prop0\": \"value0\"}},{\"type\": \"Feature\",\"geometry\": {\"type\": \"Polygon\",\"coordinates\": [[[1, 1], [4, 1], [1, 4], [4, 4], [1, 1]]]},\"properties\": {\"prop0\": \"value0\"}}]}";
            FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(strFeatureCollection);
            GeometryTools geometryTools = new GeometryTools();

            //Act
            FeatureCollectionValidationResult result = geometryTools.ValidateFeatureCollectionGeometries(featureCollection);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.InvalidFeatureResults.Any());
        }
        
    }
}
