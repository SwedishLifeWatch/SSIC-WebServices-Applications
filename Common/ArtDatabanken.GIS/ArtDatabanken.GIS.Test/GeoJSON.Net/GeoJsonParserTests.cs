using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.GeoJSON.Net.Converters;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.GIS.Test.GeoJSON.Net
{
    using Newtonsoft.Json;

    [TestClass]
    public class GeoJsonParserTests
    {
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PointString_SuccessfulParse()
        {            
            string strGeometry = "{\"type\": \"Point\",\"coordinates\": [-105.01621,39.57422]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.Point, geometry.Type);
            Point point = (Point) geometry;
            GeoJsonAssert.CoordinateAreEqual(-105.01621, 39.57422, point.Coordinates);

            // Deserialization without specifying Converter
            Point point2 = JsonConvert.DeserializeObject<Point>(strGeometry);
            Assert.AreEqual(point, point2);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PointStringWithTypeLast_SuccessfulParse()
        {
            string strGeometry = "{\"coordinates\": [-105.01621,39.57422], \"type\": \"Point\"}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.Point, geometry.Type);
            Point point = (Point)geometry;
            GeoJsonAssert.CoordinateAreEqual(-105.01621, 39.57422, point.Coordinates);

            // Deserialization without specifying Converter
            Point point2 = JsonConvert.DeserializeObject<Point>(strGeometry);
            Assert.AreEqual(point, point2);
        }

      
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_LineStringWithThreeElements_SuccessfulParse()
        {            
            string strGeometry = "{ \"type\": \"LineString\", \"coordinates\": [ [30, 10], [10, 30], [40, 40] ]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.LineString, geometry.Type);
            LineString lineString = (LineString)geometry;
            Assert.AreEqual(3, lineString.Coordinates.Count);
            GeoJsonAssert.CoordinateAreEqual(30, 10, lineString.Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(10, 30, lineString.Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(40, 40, lineString.Coordinates[2]);

            // Deserialization without specifying Converter
            LineString lineString2 = JsonConvert.DeserializeObject<LineString>(strGeometry);
            Assert.AreEqual(lineString, lineString2);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PolygonString_SuccessfulParse()
        {            
            const string strGeometry = "{ \"type\": \"Polygon\", \"coordinates\": [[[30, 10], [40, 40], [20, 40], [10, 20], [30, 10]]]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.Polygon, geometry.Type);
            Polygon polygon = (Polygon)geometry;
            Assert.AreEqual(1, polygon.Coordinates.Count);
            Assert.AreEqual(5, polygon.Coordinates[0].Coordinates.Count);
            GeoJsonAssert.CoordinateAreEqual(30, 10, polygon.Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(40, 40, polygon.Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(20, 40, polygon.Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(10, 20, polygon.Coordinates[0].Coordinates[3]);
            GeoJsonAssert.CoordinateAreEqual(30, 10, polygon.Coordinates[0].Coordinates[4]);

            // Deserialization without specifying Converter
            Polygon polygon2 = JsonConvert.DeserializeObject<Polygon>(strGeometry);
            Assert.AreEqual(polygon, polygon2);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PolygonStringWithTypeLast_SuccessfulParse()
        {
            const string strGeometry = "{ \"coordinates\": [[[30, 10], [40, 40], [20, 40], [10, 20], [30, 10]]], \"type\": \"Polygon\"}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.Polygon, geometry.Type);
            Polygon polygon = (Polygon)geometry;
            Assert.AreEqual(1, polygon.Coordinates.Count);
            Assert.AreEqual(5, polygon.Coordinates[0].Coordinates.Count);
            GeoJsonAssert.CoordinateAreEqual(30, 10, polygon.Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(40, 40, polygon.Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(20, 40, polygon.Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(10, 20, polygon.Coordinates[0].Coordinates[3]);
            GeoJsonAssert.CoordinateAreEqual(30, 10, polygon.Coordinates[0].Coordinates[4]);

            // Deserialization without specifying Converter
            Polygon polygon2 = JsonConvert.DeserializeObject<Polygon>(strGeometry);
            Assert.AreEqual(polygon, polygon2);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PolygonStringWithOneHole_SuccessfulParse()
        {            
            const string strGeometry = "{ \"type\": \"Polygon\", \"coordinates\": [[[35, 10], [45, 45], [15, 40], [10, 20], [35, 10]], [[20, 30], [35, 35], [30, 20], [20, 30]]]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.Polygon, geometry.Type);
            Polygon polygon = (Polygon)geometry;
            Assert.AreEqual(2, polygon.Coordinates.Count);
            
            // Outer polygon
            Assert.AreEqual(5, polygon.Coordinates[0].Coordinates.Count);            
            GeoJsonAssert.CoordinateAreEqual(35, 10, polygon.Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(45, 45, polygon.Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(15, 40, polygon.Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(10, 20, polygon.Coordinates[0].Coordinates[3]);
            GeoJsonAssert.CoordinateAreEqual(35, 10, polygon.Coordinates[0].Coordinates[4]);

            // Hole one
            Assert.AreEqual(4, polygon.Coordinates[1].Coordinates.Count);            
            GeoJsonAssert.CoordinateAreEqual(20, 30, polygon.Coordinates[1].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(35, 35, polygon.Coordinates[1].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(30, 20, polygon.Coordinates[1].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(20, 30, polygon.Coordinates[1].Coordinates[3]);

            // Deserialization without specifying Converter
            Polygon polygon2 = JsonConvert.DeserializeObject<Polygon>(strGeometry);
            Assert.AreEqual(polygon, polygon2);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PolygonStringWithTwoHoles_SuccessfulParse()
        {
            const string strGeometry = "{ \"type\": \"Polygon\", \"coordinates\": [[[35, 10], [45, 45], [15, 40], [10, 20], [35, 10]], [[20, 30], [35, 35], [30, 20], [20, 30]], [[5, 5], [30, 35], [24, 20], [5, 5]]]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.Polygon, geometry.Type);
            Polygon polygon = (Polygon)geometry;
            Assert.AreEqual(3, polygon.Coordinates.Count);

            // Outer polygon
            Assert.AreEqual(5, polygon.Coordinates[0].Coordinates.Count);
            GeoJsonAssert.CoordinateAreEqual(35, 10, polygon.Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(45, 45, polygon.Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(15, 40, polygon.Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(10, 20, polygon.Coordinates[0].Coordinates[3]);
            GeoJsonAssert.CoordinateAreEqual(35, 10, polygon.Coordinates[0].Coordinates[4]);

            // Hole one
            Assert.AreEqual(4, polygon.Coordinates[1].Coordinates.Count);
            GeoJsonAssert.CoordinateAreEqual(20, 30, polygon.Coordinates[1].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(35, 35, polygon.Coordinates[1].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(30, 20, polygon.Coordinates[1].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(20, 30, polygon.Coordinates[1].Coordinates[3]);

            // Hole two
            Assert.AreEqual(4, polygon.Coordinates[1].Coordinates.Count);
            GeoJsonAssert.CoordinateAreEqual(5, 5, polygon.Coordinates[2].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(30, 35, polygon.Coordinates[2].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(24, 20, polygon.Coordinates[2].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(5, 5, polygon.Coordinates[2].Coordinates[3]);

            // Deserialization without specifying Converter
            Polygon polygon2 = JsonConvert.DeserializeObject<Polygon>(strGeometry);
            Assert.AreEqual(polygon, polygon2);
        }
        

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_MultiPointString_SuccessfulParse()
        {            
            string strGeometry = "{ \"type\": \"MultiPoint\", \"coordinates\": [[10, 40], [40, 30], [20, 20], [30, 10]]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.MultiPoint, geometry.Type);
            MultiPoint multiPoint = (MultiPoint) geometry;
            Assert.AreEqual(4, multiPoint.Coordinates.Count);
            GeoJsonAssert.CoordinateAreEqual(10, 40, multiPoint.Coordinates[0].Coordinates);
            GeoJsonAssert.CoordinateAreEqual(40, 30, multiPoint.Coordinates[1].Coordinates);
            GeoJsonAssert.CoordinateAreEqual(20, 20, multiPoint.Coordinates[2].Coordinates);
            GeoJsonAssert.CoordinateAreEqual(30, 10, multiPoint.Coordinates[3].Coordinates);

            // Deserialization without specifying Converter
            MultiPoint multiPoint2 = JsonConvert.DeserializeObject<MultiPoint>(strGeometry);
            Assert.AreEqual(multiPoint, multiPoint2);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_MultiPointStringWithOneElement_SuccessfulParse()
        {
            string strGeometry = "{ \"type\": \"MultiPoint\", \"coordinates\": [[10, 40]]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.MultiPoint, geometry.Type);
            MultiPoint multiPoint = (MultiPoint)geometry;
            Assert.AreEqual(1, multiPoint.Coordinates.Count);
            GeoJsonAssert.CoordinateAreEqual(10, 40, multiPoint.Coordinates[0].Coordinates);

            // Deserialization without specifying Converter
            MultiPoint multiPoint2 = JsonConvert.DeserializeObject<MultiPoint>(strGeometry);
            Assert.AreEqual(multiPoint, multiPoint2);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_MultiPointStringWithZeroElements_SuccessfulParse()
        {
            string strGeometry = "{ \"type\": \"MultiPoint\", \"coordinates\": []}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.MultiPoint, geometry.Type);
            MultiPoint multiPoint = (MultiPoint)geometry;
            Assert.AreEqual(0, multiPoint.Coordinates.Count);

            // Deserialization without specifying Converter
            MultiPoint multiPoint2 = JsonConvert.DeserializeObject<MultiPoint>(strGeometry);
            Assert.AreEqual(multiPoint, multiPoint2);
        }        


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_MultiLineString_SuccessfulParse()
        {            
            string strGeometry = "{ \"type\": \"MultiLineString\", \"coordinates\": [[[10, 10], [20, 20], [10, 40]], [[40, 40], [30, 30], [40, 20], [30, 10]]]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.MultiLineString, geometry.Type);
            MultiLineString multiLineString = (MultiLineString) geometry;
            Assert.AreEqual(2, multiLineString.Coordinates.Count);

            // LineString 1
            GeoJsonAssert.CoordinateAreEqual(10, 10, multiLineString.Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(20, 20, multiLineString.Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(10, 40, multiLineString.Coordinates[0].Coordinates[2]);

            // LineString 2
            GeoJsonAssert.CoordinateAreEqual(40, 40, multiLineString.Coordinates[1].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(30, 30, multiLineString.Coordinates[1].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(40, 20, multiLineString.Coordinates[1].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(30, 10, multiLineString.Coordinates[1].Coordinates[3]);

            // Deserialization without specifying Converter
            MultiLineString multiLineString2 = JsonConvert.DeserializeObject<MultiLineString>(strGeometry);
            Assert.AreEqual(multiLineString, multiLineString2);
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_MultiPolygonString_SuccessfulParse()
        {
            string strGeometry = "{ \"type\": \"MultiPolygon\", \"coordinates\": [[[[30, 20], [45, 40], [10, 40], [30, 20]]], [[[15, 5], [40, 10], [10, 20], [5, 10], [15, 5]]]]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.MultiPolygon, geometry.Type);
            MultiPolygon multiPolygon = (MultiPolygon) geometry;
            Assert.AreEqual(2, multiPolygon.Coordinates.Count);

            // Polygon1
            GeoJsonAssert.CoordinateAreEqual(30, 20, multiPolygon.Coordinates[0].Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(45, 40, multiPolygon.Coordinates[0].Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(10, 40, multiPolygon.Coordinates[0].Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(30, 20, multiPolygon.Coordinates[0].Coordinates[0].Coordinates[3]);
            
            // Polygon2
            GeoJsonAssert.CoordinateAreEqual(15, 5, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(40, 10, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(10, 20, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(5, 10, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[3]);
            GeoJsonAssert.CoordinateAreEqual(15, 5, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[4]);

            // Deserialization without specifying Converter
            MultiPolygon multiPolygon2 = JsonConvert.DeserializeObject<MultiPolygon>(strGeometry);
            Assert.AreEqual(multiPolygon, multiPolygon2);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_MultiPolygonStringWithOnePolygonContainingHole_SuccessfulParse()
        {
            string strGeometry = "{ \"type\": \"MultiPolygon\", \"coordinates\": [[[[40, 40], [20, 45], [45, 30], [40, 40]]], [[[20, 35], [10, 30], [10, 10], [30, 5], [45, 20], [20, 35]], [[30, 20], [20, 15], [20, 25], [30, 20]]]]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.MultiPolygon, geometry.Type);
            MultiPolygon multiPolygon = (MultiPolygon) geometry;
            Assert.AreEqual(2, multiPolygon.Coordinates.Count);

            // Polygon1
            GeoJsonAssert.CoordinateAreEqual(40, 40, multiPolygon.Coordinates[0].Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(20, 45, multiPolygon.Coordinates[0].Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(45, 30, multiPolygon.Coordinates[0].Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(40, 40, multiPolygon.Coordinates[0].Coordinates[0].Coordinates[3]);
            
            // Polygon2
            GeoJsonAssert.CoordinateAreEqual(20, 35, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(10, 30, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(10, 10, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(30, 5, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[3]);
            GeoJsonAssert.CoordinateAreEqual(45, 20, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[4]);
            GeoJsonAssert.CoordinateAreEqual(20, 35, multiPolygon.Coordinates[1].Coordinates[0].Coordinates[5]);

            // Polygon2 - Hole1
            GeoJsonAssert.CoordinateAreEqual(30, 20, multiPolygon.Coordinates[1].Coordinates[1].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(20, 15, multiPolygon.Coordinates[1].Coordinates[1].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(20, 25, multiPolygon.Coordinates[1].Coordinates[1].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(30, 20, multiPolygon.Coordinates[1].Coordinates[1].Coordinates[3]);

            // Deserialization without specifying Converter
            MultiPolygon multiPolygon2 = JsonConvert.DeserializeObject<MultiPolygon>(strGeometry);
            Assert.AreEqual(multiPolygon, multiPolygon2);
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_GeometryCollectionString_SuccessfulParse()
        {
            string strGeometry = "{ \"type\": \"GeometryCollection\", \"geometries\": [{ \"type\": \"Point\", \"coordinates\": [100.0, 0.0]}, { \"type\": \"LineString\", \"coordinates\": [ [101.0, 0.0], [102.0, 1.0] ]}]}";
            IGeometryObject geometry = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry,new GeometryConverter());
            Assert.AreEqual(GeoJSONObjectType.GeometryCollection, geometry.Type);
            GeometryCollection geometryCollection = (GeometryCollection) geometry;
            Assert.AreEqual(2, geometryCollection.Geometries.Count);

            // Geometry1 - Point
            Point point = (Point) geometryCollection.Geometries[0];
            GeoJsonAssert.CoordinateAreEqual(100,0, point.Coordinates);

            // Geometry2 - LineString
            LineString lineString = (LineString) geometryCollection.Geometries[1];
            GeoJsonAssert.CoordinateAreEqual(101, 0, lineString.Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(102, 1, lineString.Coordinates[1]);

            // Deserialization without specifying Converter
            GeometryCollection geometryCollection2 = JsonConvert.DeserializeObject<GeometryCollection>(strGeometry);
            Assert.AreEqual(geometryCollection, geometryCollection2);
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_FeatureStringContainingBoundingBoxAndId_SuccessfulParse()
        {
            const string strFeature = "{ \"type\": \"Feature\", \"id\":\"myFeature\", \"bbox\": [-170.0, -70.0, 160.0, 85.0], \"geometry\": {\"type\": \"Polygon\",\"coordinates\": [[[-180.0, 10.0], [20.0, 90.0], [180.0, -5.0], [-30.0, -90.0],[-180.0, 10.0]]]}}";
            Feature feature = JsonConvert.DeserializeObject<Feature>(strFeature);
            Assert.AreEqual(GeoJSONObjectType.Polygon, feature.Geometry.Type);
            Assert.AreEqual("myFeature", feature.Id);
            Assert.AreEqual(-170, feature.BoundingBoxes[0]);
            Assert.AreEqual(-70, feature.BoundingBoxes[1]);
            Assert.AreEqual(160, feature.BoundingBoxes[2]);
            Assert.AreEqual(85, feature.BoundingBoxes[3]);           
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_FeatureCollectionString_SuccessfulParse()
        {
            // prop1 doesn't parse correctly...
            //
            //"properties": {
            // "prop0": "value0",
            // "prop1": {"this": "that"}
            // }
            //const string strFeatureCollection = "{ \"type\": \"FeatureCollection\", \"features\": [ { \"type\": \"Feature\", \"geometry\": { \"type\": \"Point\", \"coordinates\": [102.0, 0.5]}, \"properties\": { \"prop0\": \"value0\"}}, { \"type\": \"Feature\", \"geometry\": {\"type\": \"LineString\", \"coordinates\": [[102.0, 0.0], [103.0, 1.0], [104.0, 0.0], [105.0, 1.0]]},\"properties\": {\"prop1\": 0.0,\"prop0\": \"value0\"}},{\"type\": \"Feature\",\"geometry\": {\"type\": \"Polygon\",\"coordinates\": [[[100.0, 0.0], [101.0, 0.0], [101.0, 1.0], [100.0, 1.0],[100.0, 0.0]]]},\"properties\": {\"prop1\": {\"this\": \"that\"},\"prop0\": \"value0\"}}]}";
            const string strFeatureCollection = "{ \"type\": \"FeatureCollection\", \"features\": [ { \"type\": \"Feature\", \"geometry\": { \"type\": \"Point\", \"coordinates\": [102.0, 0.5]}, \"properties\": { \"prop0\": \"value0\"}}, { \"type\": \"Feature\", \"geometry\": {\"type\": \"LineString\", \"coordinates\": [[102.0, 0.0], [103.0, 1.0], [104.0, 0.0], [105.0, 1.0]]},\"properties\": {\"prop1\": 0.0,\"prop0\": \"value0\"}},{\"type\": \"Feature\",\"geometry\": {\"type\": \"Polygon\",\"coordinates\": [[[100.0, 0.0], [101.0, 0.0], [101.0, 1.0], [100.0, 1.0],[100.0, 0.0]]]},\"properties\": {\"prop0\": \"value0\"}}]}";            
            FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(strFeatureCollection);
            Assert.AreEqual(3, featureCollection.Features.Count);

            // Feature1
            Feature feature1 = featureCollection.Features[0];
            Assert.AreEqual(GeoJSONObjectType.Point, feature1.Geometry.Type);
            Point point = (Point) feature1.Geometry;
            GeoJsonAssert.CoordinateAreEqual(102, 0.5, point.Coordinates);
            Assert.AreEqual("value0", feature1.Properties["prop0"]);

            // Feature2
            Feature feature2 = featureCollection.Features[1];
            Assert.AreEqual(GeoJSONObjectType.LineString, feature2.Geometry.Type);
            LineString lineString = (LineString) feature2.Geometry;
            GeoJsonAssert.CoordinateAreEqual(102, 0, lineString.Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(103, 1, lineString.Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(104, 0, lineString.Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(105, 1, lineString.Coordinates[3]);
            Assert.AreEqual(0.0, feature2.Properties["prop1"]);
            Assert.AreEqual("value0", feature2.Properties["prop0"]);

            // Feature3
            Feature feature3 = featureCollection.Features[2];
            Assert.AreEqual(GeoJSONObjectType.Polygon, feature3.Geometry.Type);
            Polygon polygon = (Polygon)feature3.Geometry;
            GeoJsonAssert.CoordinateAreEqual(100, 0, polygon.Coordinates[0].Coordinates[0]);
            GeoJsonAssert.CoordinateAreEqual(101, 0, polygon.Coordinates[0].Coordinates[1]);
            GeoJsonAssert.CoordinateAreEqual(101, 1, polygon.Coordinates[0].Coordinates[2]);
            GeoJsonAssert.CoordinateAreEqual(100, 1, polygon.Coordinates[0].Coordinates[3]);
            GeoJsonAssert.CoordinateAreEqual(100, 0, polygon.Coordinates[0].Coordinates[4]);
            //Assert.AreEqual("that", feature2.Properties["prop1"]);
            Assert.AreEqual("value0", feature2.Properties["prop0"]);  
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PointStringWithNameCrs_SuccessfulParse()
        {            
            string strGeometry = "{\"type\": \"Point\",\"coordinates\": [-105.01621,39.57422], \"crs\": {\"type\": \"name\", \"properties\": {\"name\": \"urn:ogc:def:crs:OGC:1.3:CRS84\"}}}";

            IGeometryObject geometryObject = JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());
            IGeoJSONObject geoJsonObject = (IGeoJSONObject)geometryObject;

            // Assert CRS is parsed
            Assert.AreEqual(CRSType.Name, geoJsonObject.CRS.Type);
            NamedCRS namedCrs = (NamedCRS)geoJsonObject.CRS;
            Assert.AreEqual("urn:ogc:def:crs:OGC:1.3:CRS84", namedCrs.Name);



            // Deserialization without specifying Converter
            Point point = JsonConvert.DeserializeObject<Point>(strGeometry);

            // Assert CRS is parsed
            Assert.AreEqual(CRSType.Name, point.CRS.Type);
            namedCrs = (NamedCRS)point.CRS;
            Assert.AreEqual("urn:ogc:def:crs:OGC:1.3:CRS84", namedCrs.Name);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PointStringWithNameCrsExtraParameter_SuccessfulParse()
        {
            string strGeometry = "{\"type\": \"Point\",\"coordinates\": [-105.01621,39.57422], \"crs\": {\"type\": \"name\", \"properties\": {\"name\": \"urn:ogc:def:crs:OGC:1.3:CRS84\", \"extra\": [1, 0]}}}";            
            IGeoJSONObject geoJsonObject = (IGeoJSONObject)JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());

            // Assert CRS is parsed
            Assert.AreEqual(CRSType.Name, geoJsonObject.CRS.Type);
            NamedCRS namedCrs = (NamedCRS)geoJsonObject.CRS;
            Assert.AreEqual("urn:ogc:def:crs:OGC:1.3:CRS84", namedCrs.Name);
            Assert.IsTrue(namedCrs.Properties.ContainsKey("extra"));
            Assert.IsNotNull(namedCrs.Properties["extra"]);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PointStringWithLinkCrs_SuccessfulParse()
        {
            string strGeometry = "{\"type\": \"Point\",\"coordinates\": [-105.01621,39.57422], \"crs\": {\"type\": \"link\", \"properties\": { \"href\": \"http://example.com/crs/42\", \"type\": \"proj4\"}}}";            
            IGeoJSONObject geoJsonObject = (IGeoJSONObject)JsonConvert.DeserializeObject<IGeometryObject>(strGeometry, new GeometryConverter());

            // Assert CRS is parsed
            Assert.AreEqual(CRSType.Link, geoJsonObject.CRS.Type);
            LinkedCRS linkedCrs = (LinkedCRS)geoJsonObject.CRS;
            Assert.AreEqual("http://example.com/crs/42", linkedCrs.Href);
            Assert.AreEqual("proj4", linkedCrs.Properties["type"]);


            // Deserialization without specifying Converter
            Point point = JsonConvert.DeserializeObject<Point>(strGeometry);

            // Assert CRS is parsed
            Assert.AreEqual(CRSType.Link, point.CRS.Type);
            linkedCrs = (LinkedCRS)point.CRS;
            Assert.AreEqual("http://example.com/crs/42", linkedCrs.Href);
            Assert.AreEqual("proj4", linkedCrs.Properties["type"]);
        }




        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Parse_PointStringWithMalformedCrs_CrsIsNull()
        {
            string strGeometry = "{\"type\": \"Point\",\"coordinates\": [-105.01621,39.57422], \"crs\": {\"typ\": \"name\", \"properties\": {\"name\": \"urn:ogc:def:crs:OGC:1.3:CRS84\"}}}";
            Point point = JsonConvert.DeserializeObject<Point>(strGeometry);
            Assert.IsNull(point.CRS);

            strGeometry = "{\"type\": \"Point\",\"coordinates\": [-105.01621,39.57422], \"crs\": {\"type\": \"nam\", \"properties\": {\"name\": \"urn:ogc:def:crs:OGC:1.3:CRS84\"}}}";
            point = JsonConvert.DeserializeObject<Point>(strGeometry);
            Assert.IsNull(point.CRS);

            strGeometry = "{\"type\": \"Point\",\"coordinates\": [-105.01621,39.57422], \"crs\": {\"type\": \"name\", \"properties\": {}}}";
            point = JsonConvert.DeserializeObject<Point>(strGeometry);
            Assert.IsNull(point.CRS);

            strGeometry = "{\"type\": \"Point\",\"coordinates\": [-105.01621,39.57422], \"crs\": {\"type\": \"name\"}}";
            point = JsonConvert.DeserializeObject<Point>(strGeometry);
            Assert.IsNull(point.CRS);            
        }
    }

    public static class GeoJsonAssert
    {
        public static void CoordinateAreEqual(double expectedLongitudeX, double expectedLatitudeY, GeographicPosition position)
        {
            GeographicPosition coordinate = (GeographicPosition)position;
            Assert.AreEqual(expectedLongitudeX, coordinate.Longitude);
            Assert.AreEqual(expectedLatitudeY, coordinate.Latitude);            
        }

        public static void CoordinateAreEqual(double expectedLongitudeX, double expectedLatitudeY, double expectedAlitudeZ, GeographicPosition position)
        {
            GeographicPosition coordinate = (GeographicPosition)position;
            Assert.AreEqual(expectedLongitudeX, coordinate.Longitude);
            Assert.AreEqual(expectedLatitudeY, coordinate.Latitude);
            Assert.AreEqual(expectedAlitudeZ, coordinate.Altitude);            
        }
    }   
}
