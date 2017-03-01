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
    public class GeoJsonSerializationTests
    {

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_PointObject_SuccessfulSerialization()
        {
            Point point = new Point(new GeographicPosition(-105.01621, 39.57422));
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(point, serializerSettings);
            
            Point parsedPoint = (Point) JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());            
            Assert.AreEqual(point, parsedPoint);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_LineStringObject_SuccessfulSerialization()
        {
            List<GeographicPosition> coordinates = new List<GeographicPosition>
            {
                new GeographicPosition(30, 10),
                new GeographicPosition(10, 30),
                new GeographicPosition(40, 40)
            };
            LineString lineString = new LineString(coordinates);
            
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(lineString, serializerSettings);

            LineString parsedLineString = (LineString)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(lineString, parsedLineString);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_LineStringObjectWithAltitudeValues_SuccessfulSerialization()
        {
            List<GeographicPosition> coordinates = new List<GeographicPosition>
            {
                new GeographicPosition(30, 10, 55),
                new GeographicPosition(10, 30),
                new GeographicPosition(40, 40, 25.34)
            };
            LineString lineString = new LineString(coordinates);
            
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(lineString, serializerSettings);

            LineString parsedLineString = (LineString)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(lineString, parsedLineString);
        }

        [TestMethod]
        //[Ignore] // bounding box doesn't work
        public void Serialize_LineStringObjectWithBoundingBox_SuccessfulSerialization()
        {
            List<GeographicPosition> coordinates = new List<GeographicPosition>
            {
                new GeographicPosition(30, 10),
                new GeographicPosition(10, 30),
                new GeographicPosition(40, 40)
            };
            LineString lineString = new LineString(coordinates);
            lineString.BoundingBoxes = new[] {10.256, 20.0, 30.0, 40.0};

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(lineString, serializerSettings);

            LineString parsedLineString = (LineString)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            //parsedLineString.BoundingBoxes = new[] { 10.0, 20.0, 30.0, 40.0 };
            Assert.AreEqual(lineString, parsedLineString);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_PolygonObject_SuccessfulSerialization()
        {
            List<LineString> lineStrings = new List<LineString>();
            List<GeographicPosition> coordinates = new List<GeographicPosition>();
            coordinates.Add(new GeographicPosition(30,10));
            coordinates.Add(new GeographicPosition(40,40));
            coordinates.Add(new GeographicPosition(20,40));
            coordinates.Add(new GeographicPosition(10,20));
            coordinates.Add(new GeographicPosition(30,10));
            LineString lineString = new LineString(coordinates);
            lineStrings.Add(lineString);
            Polygon polygon = new Polygon(lineStrings);
          
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(polygon, serializerSettings);
            Polygon parsedPolygon = (Polygon)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(polygon, parsedPolygon);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_PolygonObjectWithOneHole_SuccessfulSerialization()
        {
            List<LineString> lineStrings = new List<LineString>();
            List<GeographicPosition> outerCoordinates = new List<GeographicPosition>();
            outerCoordinates.Add(new GeographicPosition(35, 10));
            outerCoordinates.Add(new GeographicPosition(45, 45));
            outerCoordinates.Add(new GeographicPosition(15, 40));
            outerCoordinates.Add(new GeographicPosition(10, 20));
            outerCoordinates.Add(new GeographicPosition(35, 10));
            lineStrings.Add(new LineString(outerCoordinates));

            List<GeographicPosition> holeOneCoordinates = new List<GeographicPosition>();
            holeOneCoordinates.Add(new GeographicPosition(20, 30));
            holeOneCoordinates.Add(new GeographicPosition(35, 35));
            holeOneCoordinates.Add(new GeographicPosition(30, 20));
            holeOneCoordinates.Add(new GeographicPosition(20, 30));            
            lineStrings.Add(new LineString(holeOneCoordinates));

            Polygon polygon = new Polygon(lineStrings);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(polygon, serializerSettings);
            Polygon parsedPolygon = (Polygon)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(polygon, parsedPolygon);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_PolygonObjectWithTwoHoles_SuccessfulSerialization()
        {
            List<LineString> lineStrings = new List<LineString>();
            List<GeographicPosition> outerCoordinates = new List<GeographicPosition>();
            outerCoordinates.Add(new GeographicPosition(35, 10));
            outerCoordinates.Add(new GeographicPosition(45, 45));
            outerCoordinates.Add(new GeographicPosition(15, 40));
            outerCoordinates.Add(new GeographicPosition(10, 20));
            outerCoordinates.Add(new GeographicPosition(35, 10));
            lineStrings.Add(new LineString(outerCoordinates));

            List<GeographicPosition> holeOneCoordinates = new List<GeographicPosition>();
            holeOneCoordinates.Add(new GeographicPosition(20, 30));
            holeOneCoordinates.Add(new GeographicPosition(35, 35));
            holeOneCoordinates.Add(new GeographicPosition(30, 20));
            holeOneCoordinates.Add(new GeographicPosition(20, 30));
            lineStrings.Add(new LineString(holeOneCoordinates));

            List<GeographicPosition> holeTwoCoordinates = new List<GeographicPosition>();
            holeTwoCoordinates.Add(new GeographicPosition(25, 25));
            holeTwoCoordinates.Add(new GeographicPosition(30, 35));
            holeTwoCoordinates.Add(new GeographicPosition(10, 20));
            holeTwoCoordinates.Add(new GeographicPosition(25, 25));
            lineStrings.Add(new LineString(holeTwoCoordinates));

            Polygon polygon = new Polygon(lineStrings);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(polygon, serializerSettings);
            Polygon parsedPolygon = (Polygon)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(polygon, parsedPolygon);
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiPointObject_SuccessfulSerialization()
        {
            List<Point> coordinates = new List<Point>();
            coordinates.Add(new Point(new GeographicPosition(10, 40)));
            coordinates.Add(new Point(new GeographicPosition(40, 30)));
            coordinates.Add(new Point(new GeographicPosition(20, 20)));
            coordinates.Add(new Point(new GeographicPosition(30, 10)));
            MultiPoint multiPoint = new MultiPoint(coordinates);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiPoint, serializerSettings);
            MultiPoint parsedMultiPoint = (MultiPoint)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiPoint, parsedMultiPoint);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiPointObjectWithOneElement_SuccessfulSerialization()
        {
            List<Point> coordinates = new List<Point>();
            coordinates.Add(new Point(new GeographicPosition(10, 40)));            
            MultiPoint multiPoint = new MultiPoint(coordinates);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiPoint, serializerSettings);
            MultiPoint parsedMultiPoint = (MultiPoint)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiPoint, parsedMultiPoint);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiPointObjectWithZeroElements_SuccessfulSerialization()
        {
            List<Point> coordinates = new List<Point>();            
            MultiPoint multiPoint = new MultiPoint(coordinates);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiPoint, serializerSettings);
            MultiPoint parsedMultiPoint = (MultiPoint)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiPoint, parsedMultiPoint);
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiLineStringObject_SuccessfulSerialization()
        {
            List<LineString> lineStrings = new List<LineString>();
            List<GeographicPosition> lineStringOneCoordinates = new List<GeographicPosition>
            {
                new GeographicPosition(10, 10),
                new GeographicPosition(20, 20),
                new GeographicPosition(10, 40),
                new GeographicPosition(40, 40)
            };
            lineStrings.Add(new LineString(lineStringOneCoordinates));

            List<GeographicPosition> lineStringTwoCoordinates = new List<GeographicPosition>
            {
                new GeographicPosition(40, 40),
                new GeographicPosition(30, 30),
                new GeographicPosition(40, 20),
                new GeographicPosition(30, 10)
            };
            lineStrings.Add(new LineString(lineStringTwoCoordinates));
            MultiLineString multiLineString = new MultiLineString(lineStrings);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiLineString, serializerSettings);
            MultiLineString parsedMultiLineString = (MultiLineString)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiLineString, parsedMultiLineString);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiLineStringObjectWithOneElement_SuccessfulSerialization()
        {
            List<LineString> lineStrings = new List<LineString>();
            List<GeographicPosition> lineStringOneCoordinates = new List<GeographicPosition>
            {
                new GeographicPosition(10, 10),
                new GeographicPosition(20, 20),
                new GeographicPosition(10, 40),
                new GeographicPosition(40, 40)
            };
            lineStrings.Add(new LineString(lineStringOneCoordinates));            
            MultiLineString multiLineString = new MultiLineString(lineStrings);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiLineString, serializerSettings);
            MultiLineString parsedMultiLineString = (MultiLineString)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiLineString, parsedMultiLineString);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiLineStringObjectWithZeroElements_SuccessfulSerialization()
        {
            List<LineString> lineStrings = new List<LineString>();            
            MultiLineString multiLineString = new MultiLineString(lineStrings);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiLineString, serializerSettings);
            MultiLineString parsedMultiLineString = (MultiLineString)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiLineString, parsedMultiLineString);
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiPolygonObject_SuccessfulSerialization()
        {            
            List<Polygon> polygons = new List<Polygon>();

            // Polygon 1
            List<LineString> lineStringsPolygonOne = new List<LineString>();
            List<GeographicPosition> coordinatesPolygonOne = new List<GeographicPosition>
            {
                new GeographicPosition(30, 10),
                new GeographicPosition(40, 40),
                new GeographicPosition(20, 40),
                new GeographicPosition(10, 20),
                new GeographicPosition(30, 10)
            };
            lineStringsPolygonOne.Add(new LineString(coordinatesPolygonOne));
            Polygon polygonOne = new Polygon(lineStringsPolygonOne);
            polygons.Add(polygonOne);

            // Polygon 2
            List<LineString> lineStringsPolygonTwo = new List<LineString>();
            List<GeographicPosition> coordinatesPolygonTwo = new List<GeographicPosition>
            {
                new GeographicPosition(100, 50),
                new GeographicPosition(5, 45),
                new GeographicPosition(20, 40),                
                new GeographicPosition(100, 50)
            };
            lineStringsPolygonTwo.Add(new LineString(coordinatesPolygonTwo));
            Polygon polygonTwo = new Polygon(lineStringsPolygonTwo);
            polygons.Add(polygonTwo);            
            MultiPolygon multiPolygon = new MultiPolygon(polygons);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiPolygon, serializerSettings);
            MultiPolygon parsedMultiPolygon = (MultiPolygon)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiPolygon, parsedMultiPolygon);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiPolygonObjectWithOneElement_SuccessfulSerialization()
        {
            List<Polygon> polygons = new List<Polygon>();

            // Polygon 1
            List<LineString> lineStringsPolygonOne = new List<LineString>();
            List<GeographicPosition> coordinatesPolygonOne = new List<GeographicPosition>
            {
                new GeographicPosition(30, 10),
                new GeographicPosition(40, 40),
                new GeographicPosition(20, 40),
                new GeographicPosition(10, 20),
                new GeographicPosition(30, 10)
            };
            lineStringsPolygonOne.Add(new LineString(coordinatesPolygonOne));
            Polygon polygonOne = new Polygon(lineStringsPolygonOne);
            polygons.Add(polygonOne);
            MultiPolygon multiPolygon = new MultiPolygon(polygons);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiPolygon, serializerSettings);
            MultiPolygon parsedMultiPolygon = (MultiPolygon)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiPolygon, parsedMultiPolygon);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_MultiPolygonObjectWithZeroElements_SuccessfulSerialization()
        {
            List<Polygon> polygons = new List<Polygon>();
            MultiPolygon multiPolygon = new MultiPolygon(polygons);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(multiPolygon, serializerSettings);
            MultiPolygon parsedMultiPolygon = (MultiPolygon)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(multiPolygon, parsedMultiPolygon);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_GeometryCollectionObject_SuccessfulSerialization()
        {
            List<IGeometryObject> geometries = new List<IGeometryObject>();
            
            // Geometry1 - Point
            Point point = new Point(new GeographicPosition(100, 0));
            geometries.Add(point);

            // Geometry2 - LineString
            List<GeographicPosition> positions = new List<GeographicPosition>
            {
                new GeographicPosition(101, 0),
                new GeographicPosition(102, 1)
            };
            LineString lineString = new LineString(positions);
            geometries.Add(lineString);
            GeometryCollection geometryCollection = new GeometryCollection(geometries);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(geometryCollection, serializerSettings);
            GeometryCollection parsedGeometryCollection = (GeometryCollection)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(geometryCollection, parsedGeometryCollection);
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_FeatureCollectionObject_SuccessfulSerialization()
        {
            List<Feature> features = new List<Feature>();
            
            // Feature1
            Point point = new Point(new GeographicPosition(102, 0.5));
            Dictionary<string, object> propertiesFeature1 = new Dictionary<string, object>();
            propertiesFeature1.Add("prop0", "value0");
            Feature feature1 = new Feature(point, propertiesFeature1);
            features.Add(feature1);

            // Feature2
            List<GeographicPosition> geographicPositions = new List<GeographicPosition>();
            geographicPositions.Add(new GeographicPosition(102, 0));
            geographicPositions.Add(new GeographicPosition(103, 1));
            geographicPositions.Add(new GeographicPosition(104, 0));
            geographicPositions.Add(new GeographicPosition(105, 1));
            LineString lineString = new LineString(geographicPositions);
            Dictionary<string, object> propertiesFeature2 = new Dictionary<string, object>();
            propertiesFeature2.Add("prop0", "value0");
            propertiesFeature2.Add("prop1", 0.0);
            Feature feature2 = new Feature(lineString, propertiesFeature2);
            features.Add(feature2);

            // Feature3
            List<GeographicPosition> polygonCoordinates = new List<GeographicPosition>
            {
                new GeographicPosition(100, 0),
                new GeographicPosition(101, 0),
                new GeographicPosition(101, 1),
                new GeographicPosition(100, 1),
                new GeographicPosition(100, 0)
            };            
            Polygon polygon = new Polygon(new List<LineString> {new LineString(polygonCoordinates)});
            Dictionary<string, object> propertiesFeature3 = new Dictionary<string, object>();
            propertiesFeature3.Add("prop0", "value0");
            Feature feature3 = new Feature(polygon, propertiesFeature3);
            features.Add(feature3);
            
            FeatureCollection featureCollection = new FeatureCollection(features);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(featureCollection, serializerSettings);
            FeatureCollection parsedFeatureCollection = JsonConvert.DeserializeObject<FeatureCollection>(strJson);
            Assert.AreEqual(featureCollection, parsedFeatureCollection);         
        }


        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_FeatureCollectionObjectWithBoundingBox_SuccessfulSerialization()
        {
            List<Feature> features = new List<Feature>();

            // Feature1
            Point point = new Point(new GeographicPosition(102, 0.5));
            point.BoundingBoxes = new[] { 45, 20.0, 30.0, 10.0 };
            Dictionary<string, object> propertiesFeature1 = new Dictionary<string, object>();
            propertiesFeature1.Add("prop0", "value0");
            Feature feature1 = new Feature(point, propertiesFeature1);
            feature1.BoundingBoxes = new[] { 45, 20.0, 30.0, 40.0 };
            features.Add(feature1);

            // Feature2
            List<GeographicPosition> geographicPositions = new List<GeographicPosition>();
            geographicPositions.Add(new GeographicPosition(102, 0));
            geographicPositions.Add(new GeographicPosition(103, 1));
            geographicPositions.Add(new GeographicPosition(104, 0));
            geographicPositions.Add(new GeographicPosition(105, 1));
            LineString lineString = new LineString(geographicPositions);
            Dictionary<string, object> propertiesFeature2 = new Dictionary<string, object>();
            propertiesFeature2.Add("prop0", "value0");
            propertiesFeature2.Add("prop1", 0.0);
            lineString.BoundingBoxes = new[] { 45, 20.0, 30.0, 15.0 };
            Feature feature2 = new Feature(lineString, propertiesFeature2);
            feature2.BoundingBoxes = new[] { 45, 20.0, 30.0, 16.0 };
            features.Add(feature2);

            // Feature3
            List<GeographicPosition> polygonCoordinates = new List<GeographicPosition>
            {
                new GeographicPosition(100, 0),
                new GeographicPosition(101, 0),
                new GeographicPosition(101, 1),
                new GeographicPosition(100, 1),
                new GeographicPosition(100, 0)
            };
            Polygon polygon = new Polygon(new List<LineString> { new LineString(polygonCoordinates) });
            Dictionary<string, object> propertiesFeature3 = new Dictionary<string, object>();
            propertiesFeature3.Add("prop0", "value0");
            polygon.BoundingBoxes = new[] { 45, 20.0, 30.0, 17.0 };
            Feature feature3 = new Feature(polygon, propertiesFeature3);
            feature3.BoundingBoxes = new[] { 45, 20.0, 30.0, 18.0 };
            features.Add(feature3);

            FeatureCollection featureCollection = new FeatureCollection(features);
            featureCollection.BoundingBoxes = new[] { 10.256, 20.0, 30.0, 40.0 };
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(featureCollection, serializerSettings);
            FeatureCollection parsedFeatureCollection = JsonConvert.DeserializeObject<FeatureCollection>(strJson);
            Assert.AreEqual(featureCollection, parsedFeatureCollection);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_PointObjectWithNameCrs_SuccessfulSerialization()
        {
            Point point = new Point(new GeographicPosition(-105.01621, 39.57422));
            point.CRS = new NamedCRS("urn:ogc:def:crs:OGC:1.3:CRS84");
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(point, serializerSettings);

            Point parsedPoint = (Point)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(point, parsedPoint);
            Assert.AreEqual(CRSType.Name, point.CRS.Type);
            Assert.AreEqual("urn:ogc:def:crs:OGC:1.3:CRS84", ((NamedCRS)point.CRS).Name);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_PointObjectWithLinkCrs_SuccessfulSerialization()
        {
            Point point = new Point(new GeographicPosition(-105.01621, 39.57422));
            point.CRS = new LinkedCRS("http://example.com/crs/42", "proj4");        
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(point, serializerSettings);

            Point parsedPoint = (Point)JsonConvert.DeserializeObject<IGeometryObject>(strJson, new GeometryConverter());
            Assert.AreEqual(point, parsedPoint);
            Assert.AreEqual(CRSType.Link, point.CRS.Type);
            Assert.AreEqual("http://example.com/crs/42", ((LinkedCRS)point.CRS).Href);
            Assert.AreEqual("proj4", ((LinkedCRS)point.CRS).Properties["type"]);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void Serialize_FeatureCollectionObjectWithMultipleCrs_SuccessfulSerialization()
        {
            List<Feature> features = new List<Feature>();

            // Feature1
            Point point = new Point(new GeographicPosition(102, 0.5));
            Dictionary<string, object> propertiesFeature1 = new Dictionary<string, object>();
            propertiesFeature1.Add("prop0", "value0");
            Feature feature1 = new Feature(point, propertiesFeature1);
            features.Add(feature1);

            // Feature2
            List<GeographicPosition> geographicPositions = new List<GeographicPosition>();
            geographicPositions.Add(new GeographicPosition(102, 0));
            geographicPositions.Add(new GeographicPosition(103, 1));
            geographicPositions.Add(new GeographicPosition(104, 0));
            geographicPositions.Add(new GeographicPosition(105, 1));
            LineString lineString = new LineString(geographicPositions);
            Dictionary<string, object> propertiesFeature2 = new Dictionary<string, object>();
            propertiesFeature2.Add("prop0", "value0");
            propertiesFeature2.Add("prop1", 0.0);
            Feature feature2 = new Feature(lineString, propertiesFeature2);
            features.Add(feature2);

            // Feature3
            List<GeographicPosition> polygonCoordinates = new List<GeographicPosition>
            {
                new GeographicPosition(100, 0),
                new GeographicPosition(101, 0),
                new GeographicPosition(101, 1),
                new GeographicPosition(100, 1),
                new GeographicPosition(100, 0)
            };
            Polygon polygon = new Polygon(new List<LineString> { new LineString(polygonCoordinates) });
            polygon.CRS = new LinkedCRS("www.ogc.se", "proj4");
            Dictionary<string, object> propertiesFeature3 = new Dictionary<string, object>();
            propertiesFeature3.Add("prop0", "value0");
            Feature feature3 = new Feature(polygon, propertiesFeature3);
            feature3.CRS = new NamedCRS("testCrsName");
            features.Add(feature3);

            FeatureCollection featureCollection = new FeatureCollection(features);
            featureCollection.CRS = new NamedCRS("myCrsName");
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            string strJson = JsonConvert.SerializeObject(featureCollection, serializerSettings);
            FeatureCollection parsedFeatureCollection = JsonConvert.DeserializeObject<FeatureCollection>(strJson);
            Assert.AreEqual(featureCollection, parsedFeatureCollection);
            Assert.AreEqual("myCrsName", ((NamedCRS)featureCollection.CRS).Name);
            Assert.AreEqual("testCrsName", ((NamedCRS)featureCollection.Features[2].CRS).Name);
            Assert.AreEqual("www.ogc.se", ((LinkedCRS)((Polygon)featureCollection.Features[2].Geometry).CRS).Href);
        }

    }
}
