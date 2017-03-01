using ArtDatabanken.Data;
using ArtDatabanken.WebService.AnalysisService.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Microsoft.SqlServer.Types;



namespace ArtDatabanken.WebService.AnalysisService.Test.Data
{
    [TestClass]
    public class GeometryManagerTest : TestBase

{
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ConvertGeoJsonPointToSqlGeometry()
        {
            SqlGeometry point;
            point = new SqlGeometry();
            List<SqlGeometry> pointList;
            pointList = new List<SqlGeometry>();
            int srid;

            ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point geoJsonPoint;
            List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point> geoJsonPointList;
            geoJsonPointList = new List<ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point>();

            GeographicPosition jsonPosition;
            jsonPosition = new GeographicPosition(6559063.003362669, 1314995.0030134742, null);

            geoJsonPoint = new ArtDatabanken.GIS.GeoJSON.Net.Geometry.Point(jsonPosition);
            geoJsonPointList.Add(geoJsonPoint);
            srid = 3857; //Google Mercator
            pointList = ArtDatabanken.WebService.AnalysisService.Data.GeometryManager.ConvertGeoJsonPointToSqlGeometry(geoJsonPointList, srid);
            Assert.IsTrue(pointList.IsNotNull());
            Assert.AreEqual(6559063.00336267, pointList[0].STX.Value, 0.001);
            Assert.AreEqual(1314995.00301347, pointList[0].STY.Value, 0.001);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void CreateMultiPolygonFromPoint()
        {
            SqlGeometry polygon, multiPolygon, firstPolygon, secondPolygon, validPoint;
            validPoint = new SqlGeometry();
            polygon = new SqlGeometry();
            firstPolygon = new SqlGeometry();
            secondPolygon = new SqlGeometry();
            List<SqlGeometry> pointList, polygonList;
            pointList = new List<SqlGeometry>();
            polygonList = new List<SqlGeometry>();
            double firstArea, secondArea, area;
            int srid;

            srid = 3021; //RT902,5gV
            SqlGeometryBuilder geometryBuilder = new SqlGeometryBuilder();
            geometryBuilder.SetSrid(srid);
            geometryBuilder.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder.BeginFigure(1, 1);
            geometryBuilder.EndFigure();
            geometryBuilder.EndGeometry();
            validPoint = geometryBuilder.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder1 = new SqlGeometryBuilder();
            geometryBuilder1.SetSrid(srid);
            geometryBuilder1.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder1.BeginFigure(1, 50);
            geometryBuilder1.EndFigure();
            geometryBuilder1.EndGeometry();
            validPoint = geometryBuilder1.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder2 = new SqlGeometryBuilder();
            geometryBuilder2.SetSrid(srid);
            geometryBuilder2.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder2.BeginFigure(50, 50);
            geometryBuilder2.EndFigure();
            geometryBuilder2.EndGeometry();
            validPoint = geometryBuilder2.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder3 = new SqlGeometryBuilder();
            geometryBuilder3.SetSrid(srid);
            geometryBuilder3.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder3.BeginFigure(50, 1);
            geometryBuilder3.EndFigure();
            geometryBuilder3.EndGeometry();
            validPoint = geometryBuilder3.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder4 = new SqlGeometryBuilder();
            geometryBuilder4.SetSrid(srid);
            geometryBuilder4.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder4.BeginFigure(1, 1);
            geometryBuilder4.EndFigure();
            geometryBuilder4.EndGeometry();
            validPoint = geometryBuilder4.ConstructedGeometry;
            pointList.Add(validPoint);

            //Create a polygon and add to list
            polygon = ArtDatabanken.WebService.AnalysisService.Data.GeometryManager.CreatePolygon(pointList);
            polygonList.Add(polygon);
            pointList = new List<SqlGeometry>();

            //First inner ring is added to multipolygon  
            firstPolygon = ArtDatabanken.WebService.AnalysisService.Data.GeometryManager.CreateMultiGeometry(polygonList);
            firstArea = firstPolygon.STArea().Value;    
                
            SqlGeometryBuilder geometryBuilder5 = new SqlGeometryBuilder();
            geometryBuilder5.SetSrid(srid);
            geometryBuilder5.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder5.BeginFigure(3, 3);
            geometryBuilder5.EndFigure();
            geometryBuilder5.EndGeometry();
            validPoint = geometryBuilder5.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder6 = new SqlGeometryBuilder();
            geometryBuilder6.SetSrid(srid);
            geometryBuilder6.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder6.BeginFigure(3, 30);
            geometryBuilder6.EndFigure();
            geometryBuilder6.EndGeometry();
            validPoint = geometryBuilder6.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder7 = new SqlGeometryBuilder();
            geometryBuilder7.SetSrid(srid);
            geometryBuilder7.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder7.BeginFigure(30, 30);
            geometryBuilder7.EndFigure();
            geometryBuilder7.EndGeometry();
            validPoint = geometryBuilder7.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder8 = new SqlGeometryBuilder();
            geometryBuilder8.SetSrid(srid);
            geometryBuilder8.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder8.BeginFigure(30, 3);
            geometryBuilder8.EndFigure();
            geometryBuilder8.EndGeometry();
            validPoint = geometryBuilder8.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder9 = new SqlGeometryBuilder();
            geometryBuilder9.SetSrid(srid);
            geometryBuilder9.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder9.BeginFigure(3, 3);
            geometryBuilder9.EndFigure();
            geometryBuilder9.EndGeometry();
            validPoint = geometryBuilder9.ConstructedGeometry;
            pointList.Add(validPoint);

            //Second polygon is created and added to list
            polygon = GeometryManager.CreatePolygon(pointList);
            polygonList.Add(polygon);
            
            //Two polygons are added to multipolygon 
            secondPolygon = GeometryManager.CreateMultiGeometry(polygonList);
            secondArea = secondPolygon.STArea().Value;
            
            pointList = new List<SqlGeometry>();
            SqlGeometryBuilder geometryBuilder10 = new SqlGeometryBuilder();
            geometryBuilder10.SetSrid(srid);
            geometryBuilder10.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder10.BeginFigure(100, 100);
            geometryBuilder10.EndFigure();
            geometryBuilder10.EndGeometry();
            validPoint = geometryBuilder10.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder11 = new SqlGeometryBuilder();
            geometryBuilder11.SetSrid(srid);
            geometryBuilder11.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder11.BeginFigure(100, 190);
            geometryBuilder11.EndFigure();
            geometryBuilder11.EndGeometry();
            validPoint = geometryBuilder11.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder12 = new SqlGeometryBuilder();
            geometryBuilder12.SetSrid(srid);
            geometryBuilder12.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder12.BeginFigure(180, 125);
            geometryBuilder12.EndFigure();
            geometryBuilder12.EndGeometry();
            validPoint = geometryBuilder12.ConstructedGeometry;
            pointList.Add(validPoint);

            SqlGeometryBuilder geometryBuilder13 = new SqlGeometryBuilder();
            geometryBuilder13.SetSrid(srid);
            geometryBuilder13.BeginGeometry(OpenGisGeometryType.Point);
            geometryBuilder13.BeginFigure(100, 100);
            geometryBuilder13.EndFigure();
            geometryBuilder13.EndGeometry();
            validPoint = geometryBuilder13.ConstructedGeometry;
            pointList.Add(validPoint);

            //Second polygon is created and added to list
            polygon = GeometryManager.CreatePolygon(pointList);
            polygonList.Add(polygon);
            
            //Add all three polygons to a multipolygon. The content of the multipolygon should be 
            //one polygon with an interior ring (doughnut or torus) plus another polygon beside it.
            multiPolygon = GeometryManager.CreateMultiGeometry(polygonList);
            area = multiPolygon.STArea().Value; 
            Assert.IsTrue(multiPolygon.IsNotNull());
            Assert.IsTrue(secondArea < firstArea);
            Assert.IsTrue(area > secondArea);
        }

        //[TestMethod]
        //public void CheckGeometryData()
        //{
        //    SqlGeometry point, polygon, multipolygon;
        //    point = new SqlGeometry();
        //    polygon = new SqlGeometry();
        //    multipolygon = new SqlGeometry();
            
        //    SqlGeometryBuilder geometryBuilder = new SqlGeometryBuilder();
        //    geometryBuilder.SetSrid(3021);
        //    geometryBuilder.BeginGeometry(OpenGisGeometryType.Point);
        //    geometryBuilder.BeginFigure(1, 1);
        //    geometryBuilder.EndFigure();
        //    geometryBuilder.EndGeometry();
        //    point = geometryBuilder.ConstructedGeometry;


        //}

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetMultiPolygonFeaturesBasedOnGridCells()
        {
            List<WebGridCellFeatureStatistics> webGridCellFeatureStatisticsList;
           WebGridSpecification gridSpecification;
            SqlGeometryBuilder geomBuilder, geomBuilder2, geomBuilder3, geomBuilder4;
            SqlGeometry feature, feature2, feature3, feature4;
            List<SqlGeometry> polygonList;
            WebCoordinateSystem webCoordinateSystem;
            webCoordinateSystem = new WebCoordinateSystem();

            //Create parameter WebGridSpecification
            polygonList = new List<SqlGeometry>();
            gridSpecification= new WebGridSpecification();
            gridSpecification.BoundingBox = new WebBoundingBox();
            gridSpecification.BoundingBox.Max = new WebPoint();
            gridSpecification.BoundingBox.Min = new WebPoint();
            gridSpecification.BoundingBox.Max.X = 1000;
            gridSpecification.BoundingBox.Max.Y = 1000;
            gridSpecification.BoundingBox.Min.X = 0;
            gridSpecification.BoundingBox.Min.Y = 0;
            gridSpecification.GridCellSize = 100;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;

            //Create multipolygon to measure
            geomBuilder = new SqlGeometryBuilder();
            //Test with SWEREF99TM
            geomBuilder.SetSrid(3006);
            geomBuilder.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder.BeginFigure(1, 1);
            geomBuilder.AddLine(195, 1);
            geomBuilder.AddLine(195, 150);
            geomBuilder.AddLine(1, 150 );
            geomBuilder.AddLine(1, 1);
            geomBuilder.EndFigure();
            geomBuilder.EndGeometry();
            feature = geomBuilder.ConstructedGeometry;

            geomBuilder2 = new SqlGeometryBuilder();
            geomBuilder2.SetSrid(3006);
            geomBuilder2.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder2.BeginFigure(401, 401);
            geomBuilder2.AddLine(595, 401);
            geomBuilder2.AddLine(595, 550);
            geomBuilder2.AddLine(401, 550);
            geomBuilder2.AddLine(401, 401);
            geomBuilder2.EndFigure();
            geomBuilder2.EndGeometry();
            feature2 = geomBuilder2.ConstructedGeometry;
            
            //Make polygons into multipolygon
            feature = feature.STUnion(feature2);
            polygonList.Add(feature);
           
            //Next multipolygon
            geomBuilder3 = new SqlGeometryBuilder();
            geomBuilder3.SetSrid(3006);
            geomBuilder3.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder3.BeginFigure(901, 901);
            geomBuilder3.AddLine(901, 910);
            geomBuilder3.AddLine(910, 910);
            geomBuilder3.AddLine(910, 901);
            geomBuilder3.AddLine(901, 901);
            geomBuilder3.EndFigure();
            geomBuilder3.EndGeometry();
            feature3 = geomBuilder3.ConstructedGeometry;
            
            geomBuilder4 = new SqlGeometryBuilder();
            geomBuilder4.SetSrid(3006);
            geomBuilder4.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder4.BeginFigure(951, 951);
            geomBuilder4.AddLine(951, 960);
            geomBuilder4.AddLine(960, 960);
            geomBuilder4.AddLine(960, 951);
            geomBuilder4.AddLine(951, 951);
            geomBuilder4.EndFigure();
            geomBuilder4.EndGeometry();
            feature4 = geomBuilder4.ConstructedGeometry;

            feature3 = feature3.STUnion(feature4);
            polygonList.Add(feature3);
            webCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            webGridCellFeatureStatisticsList = GeometryManager.GetFeaturesBasedOnGridCells(polygonList, gridSpecification, 3006);
            Assert.IsTrue(webGridCellFeatureStatisticsList.IsNotNull());
            Assert.IsTrue(webGridCellFeatureStatisticsList.Count.Equals(9));
            Assert.IsTrue(webGridCellFeatureStatisticsList[0].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[8].FeatureCount.Equals(1));

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetBigMultiPolygonFeaturesBasedOnGridCells()
        {
            List<WebGridCellFeatureStatistics> webGridCellFeatureStatisticsList;
            WebGridSpecification gridSpecification;
            SqlGeometryBuilder geomBuilder;
            SqlGeometry feature;
            List<SqlGeometry> polygonList;
            WebCoordinateSystem webCoordinateSystem;
            webCoordinateSystem = new WebCoordinateSystem();

            //Create parameter WebGridSpecification
            polygonList = new List<SqlGeometry>();
            gridSpecification = new WebGridSpecification();
            gridSpecification.BoundingBox = new WebBoundingBox();
            gridSpecification.BoundingBox.Max = new WebPoint();
            gridSpecification.BoundingBox.Min = new WebPoint();
            gridSpecification.BoundingBox.Max.X = 1000;
            gridSpecification.BoundingBox.Max.Y = 1000;
            gridSpecification.BoundingBox.Min.X = 0;
            gridSpecification.BoundingBox.Min.Y = 0;
            gridSpecification.GridCellSize = 100;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;

            //Create multipolygon to measure
            geomBuilder = new SqlGeometryBuilder();
            //Test with SWEREF99TM
            geomBuilder.SetSrid(3857);
            geomBuilder.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder.BeginFigure(150, 650);
            geomBuilder.AddLine(350, 650);
            geomBuilder.AddLine(350, 850);
            geomBuilder.AddLine(150, 850 );
            geomBuilder.AddLine(150, 650);
            geomBuilder.EndFigure();
            geomBuilder.EndGeometry();
            feature = geomBuilder.ConstructedGeometry;
            polygonList.Add(feature);
           
            webCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            webGridCellFeatureStatisticsList = ArtDatabanken.WebService.AnalysisService.Data.GeometryManager.GetFeaturesBasedOnGridCells(polygonList, gridSpecification, 3857);
            Assert.IsTrue(webGridCellFeatureStatisticsList.IsNotNull());
            Assert.IsTrue(webGridCellFeatureStatisticsList.Count.Equals(9));
            Assert.IsTrue(webGridCellFeatureStatisticsList[0].FeatureArea.Equals(2500));
            Assert.IsTrue(webGridCellFeatureStatisticsList[4].FeatureArea.Equals(10000));
            

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetPolygonFeaturesBasedOnGridCells()
        {
            List<WebGridCellFeatureStatistics> webGridCellFeatureStatisticsList;
            WebGridSpecification gridSpecification;
            SqlGeometryBuilder geomBuilder, geomBuilder2, geomBuilder3, geomBuilder4;
            SqlGeometry feature, feature2, feature3, feature4;
            List<SqlGeometry> polygonList;
            WebCoordinateSystem webCoordinateSystem;
            webCoordinateSystem = new WebCoordinateSystem();

            //Create parameter WebGridSpecification
            polygonList = new List<SqlGeometry>();
            gridSpecification = new WebGridSpecification();
            gridSpecification.BoundingBox = new WebBoundingBox();
            gridSpecification.BoundingBox.Max = new WebPoint();
            gridSpecification.BoundingBox.Min = new WebPoint();
            gridSpecification.BoundingBox.Max.X = 1000;
            gridSpecification.BoundingBox.Max.Y = 1000;
            gridSpecification.BoundingBox.Min.X = 0;
            gridSpecification.BoundingBox.Min.Y = 0;
            gridSpecification.GridCellSize = 100;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;

            //Create four polygons to measure
            geomBuilder = new SqlGeometryBuilder();
            //Test with SWEREF99TM
            geomBuilder.SetSrid(3006);
            geomBuilder.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder.BeginFigure(1, 1);
            geomBuilder.AddLine(195, 1);
            geomBuilder.AddLine(195, 150);
            geomBuilder.AddLine(1, 150);
            geomBuilder.AddLine(1, 1);
            geomBuilder.EndFigure();
            geomBuilder.EndGeometry();
            feature = geomBuilder.ConstructedGeometry;
            polygonList.Add(feature);

            geomBuilder4 = new SqlGeometryBuilder();
            geomBuilder4.SetSrid(3006);
            geomBuilder4.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder4.BeginFigure(951, 951);
            geomBuilder4.AddLine(951, 960);
            geomBuilder4.AddLine(960, 960);
            geomBuilder4.AddLine(960, 951);
            geomBuilder4.AddLine(951, 951);
            geomBuilder4.EndFigure();
            geomBuilder4.EndGeometry();
            feature4 = geomBuilder4.ConstructedGeometry;

            polygonList.Add(feature4); 
            
            geomBuilder2 = new SqlGeometryBuilder();
            geomBuilder2.SetSrid(3006);
            geomBuilder2.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder2.BeginFigure(401, 401);
            geomBuilder2.AddLine(595, 401);
            geomBuilder2.AddLine(595, 550);
            geomBuilder2.AddLine(401, 550);
            geomBuilder2.AddLine(401, 401);
            geomBuilder2.EndFigure();
            geomBuilder2.EndGeometry();
            feature2 = geomBuilder2.ConstructedGeometry;
            polygonList.Add(feature2);

            geomBuilder3 = new SqlGeometryBuilder();
            geomBuilder3.SetSrid(3006);
            geomBuilder3.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder3.BeginFigure(901, 901);
            geomBuilder3.AddLine(901, 910);
            geomBuilder3.AddLine(910, 910);
            geomBuilder3.AddLine(910, 901);
            geomBuilder3.AddLine(901, 901);
            geomBuilder3.EndFigure();
            geomBuilder3.EndGeometry();
            feature3 = geomBuilder3.ConstructedGeometry;
            polygonList.Add(feature3);

            webCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            webGridCellFeatureStatisticsList = GeometryManager.GetFeaturesBasedOnGridCells(polygonList, gridSpecification, 3006);
            Assert.IsTrue(webGridCellFeatureStatisticsList.IsNotNull());
            Assert.IsTrue(webGridCellFeatureStatisticsList.Count.Equals(9));
            Assert.IsTrue(webGridCellFeatureStatisticsList[0].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[1].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[2].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[3].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[4].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[5].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[6].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[7].FeatureCount.Equals(1));
            Assert.IsTrue(webGridCellFeatureStatisticsList[8].FeatureCount.Equals(2));
            Assert.IsTrue(webGridCellFeatureStatisticsList[0].CentreCoordinate.X.Equals(50));
            Assert.IsTrue(webGridCellFeatureStatisticsList[4].CentreCoordinate.Y.Equals(450));
            Assert.IsTrue(webGridCellFeatureStatisticsList[8].CentreCoordinate.X.Equals(950));
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetPointFeaturesBasedOnGridCells()
        {
            List<WebGridCellFeatureStatistics> webGridCellFeatureStatisticsList;
             WebGridSpecification gridSpecification;
            SqlGeometryBuilder geomBuilder, geomBuilder2, geomBuilder3, geomBuilder4;
            SqlGeometry feature, feature2, feature3, feature4;
            List<SqlGeometry> polygonList;
            WebCoordinateSystem webCoordinateSystem;
            webCoordinateSystem = new WebCoordinateSystem();

            //Create parameter WebGridSpecification
            polygonList = new List<SqlGeometry>();
            gridSpecification = new WebGridSpecification();
            gridSpecification.BoundingBox = new WebBoundingBox();
            gridSpecification.BoundingBox.Max = new WebPoint();
            gridSpecification.BoundingBox.Min = new WebPoint();
            gridSpecification.BoundingBox.Max.X = 1000;
            gridSpecification.BoundingBox.Max.Y = 1000;
            gridSpecification.BoundingBox.Min.X = 0;
            gridSpecification.BoundingBox.Min.Y = 0;
            gridSpecification.GridCellSize = 100;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;

            //Create four points to measure
            geomBuilder = new SqlGeometryBuilder();
            //Test with SWEREF99TM
            geomBuilder.SetSrid(3006);
            geomBuilder.BeginGeometry(OpenGisGeometryType.Point);
            geomBuilder.BeginFigure(1, 1);
            geomBuilder.EndFigure();
            geomBuilder.EndGeometry();
            feature = geomBuilder.ConstructedGeometry;
            polygonList.Add(feature);

            geomBuilder2 = new SqlGeometryBuilder();
            //Test with SWEREF99TM
            geomBuilder2.SetSrid(3006);
            geomBuilder2.BeginGeometry(OpenGisGeometryType.Point);
            geomBuilder2.BeginFigure(801, 801);
            geomBuilder2.EndFigure();
            geomBuilder2.EndGeometry();
            feature2 = geomBuilder2.ConstructedGeometry;
            polygonList.Add(feature2);

            //Create four points to measure
            geomBuilder3 = new SqlGeometryBuilder();
            //Test with SWEREF99TM
            geomBuilder3.SetSrid(3006);
            geomBuilder3.BeginGeometry(OpenGisGeometryType.Point);
            geomBuilder3.BeginFigure(301, 301);
            geomBuilder3.EndFigure();
            geomBuilder3.EndGeometry();
            feature3 = geomBuilder3.ConstructedGeometry;
            polygonList.Add(feature3);

            //Create four points to measure
            geomBuilder4 = new SqlGeometryBuilder();
            //Test with SWEREF99TM
            geomBuilder4.SetSrid(3006);
            geomBuilder4.BeginGeometry(OpenGisGeometryType.Point);
            geomBuilder4.BeginFigure(810, 810);
            geomBuilder4.EndFigure();
            geomBuilder4.EndGeometry();
            feature4 = geomBuilder4.ConstructedGeometry;
            polygonList.Add(feature4);

            webCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            webGridCellFeatureStatisticsList = GeometryManager.GetFeaturesBasedOnGridCells(polygonList, gridSpecification, 3006);
            Assert.IsTrue(webGridCellFeatureStatisticsList.IsNotNull());
            Assert.IsTrue(webGridCellFeatureStatisticsList.Count.Equals(3));
            
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ReProjectMultiPolygon()
        {
            List<WebMultiPolygon> convertedMultiPolygonList;
            WebCoordinateSystem toCoordinateSystem;
            WebCoordinateSystem fromCoordinateSystem;
            List<SqlGeometry> polygonList;
            
            convertedMultiPolygonList = new List<WebMultiPolygon>();
            toCoordinateSystem = new WebCoordinateSystem();
            fromCoordinateSystem = new WebCoordinateSystem();
           
            SqlGeometryBuilder geomBuilder, geomBuilder2, geomBuilder3, geomBuilder4;
            SqlGeometry feature, feature2, feature3, feature4;
            polygonList = new List<SqlGeometry>();

            toCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            fromCoordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;

            //Create a MultiGeometry
            geomBuilder = new SqlGeometryBuilder();
            
            geomBuilder.SetSrid(3006);
            geomBuilder.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder.BeginFigure(1, 1);
            geomBuilder.AddLine(195, 1);
            geomBuilder.AddLine(195, 150);
            geomBuilder.AddLine(1, 150);
            geomBuilder.AddLine(1, 1);
            geomBuilder.EndFigure();
            geomBuilder.EndGeometry();
            feature = geomBuilder.ConstructedGeometry;

            geomBuilder2 = new SqlGeometryBuilder();
            geomBuilder2.SetSrid(3006);
            geomBuilder2.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder2.BeginFigure(401, 401);
            geomBuilder2.AddLine(595, 401);
            geomBuilder2.AddLine(595, 550);
            geomBuilder2.AddLine(401, 550);
            geomBuilder2.AddLine(401, 401);
            geomBuilder2.EndFigure();
            geomBuilder2.EndGeometry();
            feature2 = geomBuilder2.ConstructedGeometry;

            //Make polygons into multipolygon 1
            feature = feature.STUnion(feature2);
            polygonList.Add(feature);

            //Next multipolygon
            geomBuilder3 = new SqlGeometryBuilder();
            geomBuilder3.SetSrid(3006);
            geomBuilder3.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder3.BeginFigure(901, 901);
            geomBuilder3.AddLine(901, 910);
            geomBuilder3.AddLine(910, 910);
            geomBuilder3.AddLine(910, 901);
            geomBuilder3.AddLine(901, 901);
            geomBuilder3.EndFigure();
            geomBuilder3.EndGeometry();
            feature3 = geomBuilder3.ConstructedGeometry;

            geomBuilder4 = new SqlGeometryBuilder();
            geomBuilder4.SetSrid(3006);
            geomBuilder4.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder4.BeginFigure(951, 951);
            geomBuilder4.AddLine(951, 960);
            geomBuilder4.AddLine(960, 960);
            geomBuilder4.AddLine(960, 951);
            geomBuilder4.AddLine(951, 951);
            geomBuilder4.EndFigure();
            geomBuilder4.EndGeometry();
            feature4 = geomBuilder4.ConstructedGeometry;
            //Make polygons into multipolygon 2
            feature3 = feature3.STUnion(feature4);
            polygonList.Add(feature3);

            convertedMultiPolygonList = GeometryManager.ReProjectMultiPolygon(toCoordinateSystem, fromCoordinateSystem,
                                                                              polygonList);
            Assert.IsTrue(convertedMultiPolygonList.IsNotNull());
            Assert.IsTrue(convertedMultiPolygonList.Count.Equals(2));

            //Lägg till innan HELA TESTET!: [ExpectedException(typeof(System.ArgumentNullException))]
            //fromCoordinateSystem.Id = CoordinateSystemId.SWEREF99;
            //convertedMultiPolygonList = GeometryManager.ReProjectMultiPolygon(toCoordinateSystem, fromCoordinateSystem,
            //                                                                  polygonList);
            //Assert.Fail("There is a mismatch between coordinate systems in Sql Geometry list and fromCoordinateSystem.");
        }
        
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void ReProjectPolygon()
        {
            List<WebPolygon> convertedPolygonList;
            WebCoordinateSystem toCoordinateSystem;
            WebCoordinateSystem fromCoordinateSystem;
            List<SqlGeometry> polygonList;

            convertedPolygonList = new List<WebPolygon>();
            toCoordinateSystem = new WebCoordinateSystem();
            fromCoordinateSystem = new WebCoordinateSystem();

            SqlGeometryBuilder geomBuilder, geomBuilder2, geomBuilder3, geomBuilder4;
            SqlGeometry feature, feature2, feature3, feature4;
            polygonList = new List<SqlGeometry>();

            toCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            fromCoordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;

            //Create a Geometry
            geomBuilder = new SqlGeometryBuilder();

            geomBuilder.SetSrid(3006);
            geomBuilder.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder.BeginFigure(1, 1);
            geomBuilder.AddLine(195, 1);
            geomBuilder.AddLine(195, 150);
            geomBuilder.AddLine(1, 150);
            geomBuilder.AddLine(1, 1);
            geomBuilder.EndFigure();
            geomBuilder.EndGeometry();
            feature = geomBuilder.ConstructedGeometry;
            polygonList.Add(feature);

            geomBuilder2 = new SqlGeometryBuilder();
            geomBuilder2.SetSrid(3006);
            geomBuilder2.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder2.BeginFigure(401, 401);
            geomBuilder2.AddLine(595, 401);
            geomBuilder2.AddLine(595, 550);
            geomBuilder2.AddLine(401, 550);
            geomBuilder2.AddLine(401, 401);
            geomBuilder2.EndFigure();
            geomBuilder2.EndGeometry();
            feature2 = geomBuilder2.ConstructedGeometry;
            polygonList.Add(feature2);

            geomBuilder3 = new SqlGeometryBuilder();
            geomBuilder3.SetSrid(3006);
            geomBuilder3.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder3.BeginFigure(901, 901);
            geomBuilder3.AddLine(901, 910);
            geomBuilder3.AddLine(910, 910);
            geomBuilder3.AddLine(910, 901);
            geomBuilder3.AddLine(901, 901);
            geomBuilder3.EndFigure();
            geomBuilder3.EndGeometry();
            feature3 = geomBuilder3.ConstructedGeometry;
            polygonList.Add(feature3);

            geomBuilder4 = new SqlGeometryBuilder();
            geomBuilder4.SetSrid(3006);
            geomBuilder4.BeginGeometry(OpenGisGeometryType.Polygon);
            geomBuilder4.BeginFigure(951, 951);
            geomBuilder4.AddLine(951, 960);
            geomBuilder4.AddLine(960, 960);
            geomBuilder4.AddLine(960, 951);
            geomBuilder4.AddLine(951, 951);
            geomBuilder4.EndFigure();
            geomBuilder4.EndGeometry();
            feature4 = geomBuilder4.ConstructedGeometry; 
            polygonList.Add(feature4);

            convertedPolygonList = GeometryManager.ReProjectPolygon(toCoordinateSystem, fromCoordinateSystem,
                                                                              polygonList);
            Assert.IsTrue(convertedPolygonList.IsNotNull());
            Assert.IsTrue(convertedPolygonList.Count.Equals(4));

            //Todo: How to test fail?
            //fromCoordinateSystem.Id = CoordinateSystemId.SWEREF99;
            //convertedMultiPolygonList = GeometryManager.ReProjectMultiPolygon(toCoordinateSystem, fromCoordinateSystem,
            //                                                                  polygonList);
            //Assert.Fail("There is a mismatch between coordinate systems in Sql Geometry list and fromCoordinateSystem.");
        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSridFromWebCoordinateSystem()
        {
            int srid = 0;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            srid = GeometryManager.GetSridFromWebCoordinateSystem(coordinateSystem);
            Assert.IsTrue(srid.Equals(3857));

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99;
            srid = GeometryManager.GetSridFromWebCoordinateSystem(coordinateSystem);
            Assert.IsTrue(srid.Equals(4378));

        }

            
    }
}
