using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.AnalysisService.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalysisManager = ArtDatabanken.WebService.AnalysisService.Data.AnalysisManager;

namespace ArtDatabanken.WebService.AnalysisService.Test.Data
{
    [TestClass]
    public class AnalysisManagerTest : TestBase
    {
        #region GetGridCellSpeciesCounts

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesCountsTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 5000;



            IList<WebGridCellSpeciesCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.X.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.Y.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].Size == 5000);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.Rt90_25_gon_v.ToString()));
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Min.X.IsNotNull());
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Max.X.IsNotNull());
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Min.Y.IsNotNull());
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Max.Y.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesObservationCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesObservationCount >= noOfGridCellObservations[0].SpeciesCount);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesCountsTestElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 5000;



            IList<WebGridCellSpeciesCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesCountsElasticsearch(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.X.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.Y.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].Size == 5000);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.Rt90_25_gon_v.ToString()));
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Min.X.IsNotNull());
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Max.X.IsNotNull());
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Min.Y.IsNotNull());
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Max.Y.IsNotNull());
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesObservationCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesObservationCount >= noOfGridCellObservations[0].SpeciesCount);
        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridSpeciesCountsBoundingPolygonTypeTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 5000;

            IList<WebGridCellSpeciesCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GeometryType.Equals(GridCellGeometryType.Polygon));
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.IsNotNull());

        }
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridSpeciesCountsCentrePointTypeTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.CentrePoint;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 5000;

            IList<WebGridCellSpeciesCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GeometryType.Equals(GridCellGeometryType.CentrePoint));
            Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.IsNull());

        }

        //[TestMethod]
        //   [Ignore]
        //   [TestCategory("NightlyTest")]
        //   public void GetGridCellFeatureStatisticsWithUrlComponentsTest()
        //   {
        //       String featuresUrl;
        //       bool isCompleteUrl;
        //       WebCoordinateSystem coordinateSystem;
        //       WfsTypeName typeName;
        //       WebFeatureStatisticsSpecification featureStatistics;
        //       WebGridSpecification gridSpecification;

        //       gridSpecification = new WebGridSpecification();
        //       typeName = new WfsTypeName();
        //       coordinateSystem = new WebCoordinateSystem();
        //       WFSVersion version = WFSVersion.Ver110;
        //       featureStatistics = new WebFeatureStatisticsSpecification();

        //       string bbox = "";
        //       string parameter = "SLW:LÃ¤nSKOD";
        //       string parameterValue = "17";


        //       featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?";
        //       ////Todo: vilken är det:?
        //       typeName.Namespace = "SLW:MapOfSwedenCounties";
        //       typeName.Name = "SLW:MapOfSwedenCounties";

        //       featureStatistics.BoundingBox = new WebBoundingBox();
        //       featureStatistics.BoundingBox.Max = new WebPoint();
        //       featureStatistics.BoundingBox.Min = new WebPoint();

        //       gridSpecification.BoundingBox = new WebBoundingBox();
        //       gridSpecification.BoundingBox.Max = new WebPoint();
        //       gridSpecification.BoundingBox.Min = new WebPoint();

        //       featureStatistics.BoundingBox.Max.X = 1521024; //= RT90 Y
        //       featureStatistics.BoundingBox.Max.Y = 6937341; //= RT90 X
        //       featureStatistics.BoundingBox.Min.X = 1457184;
        //       featureStatistics.BoundingBox.Min.Y = 6875163;

        //       gridSpecification.BoundingBox.Max.X = 1489104;
        //       gridSpecification.BoundingBox.Max.Y = 6858363;
        //       gridSpecification.BoundingBox.Min.X = 1456064;
        //       gridSpecification.BoundingBox.Min.Y = 6842683;

        //       gridSpecification.GridCellSize = 10000;
        //       gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;
        //       gridSpecification.IsGridCellSizeSpecified = true;
        //       coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
        //       isCompleteUrl = false;
        //       IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
        //                        AnalysisManager.GetGridCellFeatureStatistics(Context, featureStatistics, featuresUrl, //typeName, 
        //                                                                 gridSpecification, coordinateSystem //,isCompleteUrl
        //                                                                 );


        //       Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
        //       Assert.IsTrue(gridCellFeatureStatistics.Count.Equals(70));
        //       Assert.IsTrue(gridCellFeatureStatistics[0].GridCellBoundingBox.Min.X.Equals(1456064));
        //       Assert.IsTrue(gridCellFeatureStatistics[69].GridCellBoundingBox.Max.Y.Equals(6942683));

        //   }

        #endregion


        #region GetGridCellSpeciesObservationCounts


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 5000;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;




            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            // Use another setting than default


            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations[0].Size == 5000);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.SWEREF99_TM.ToString()));
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Min.X > 0);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Max.X > 0);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Min.Y > 0);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Max.Y > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsTestElasticsearch()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 5000;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;




            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCountsElasticsearch(Context, searchCriteria, webGridSpecification, coordinateSystem);
            // Use another setting than default


            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations[0].Size == 5000);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.SWEREF99_TM.ToString()));
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Min.X > 0);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Max.X > 0);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Min.Y > 0);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.Max.Y > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountFromDifferentMethodsTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification gridSpecification = new WebGridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            //taxa.Add(2002088);//Duvor
            //taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago
            searchCriteria.TaxonIds = taxa;


            List<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);
            // Use another method than default
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            //taxa.Add(2002088);//Duvor
            //taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago
            searchCriteria.TaxonIds = taxa;
            IList<WebGridCellSpeciesCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, gridSpecification, coordinateSystem);


            List<WebGridCellSpeciesObservationCount> noOfGridCellObservationsSortedList = noOfGridCellObservations.OrderBy(o => o.Count).ThenBy(o => o.CentreCoordinate.X).ToList();
            List<WebGridCellSpeciesCount> noOfGridCellObservations2SortedList = noOfGridCellObservations2.OrderBy(o => o.SpeciesObservationCount).ThenBy(o => o.CentreCoordinate.X).ToList();


            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].Count > 0);

            Assert.IsTrue(noOfGridCellObservations2.Count > 0);

            Assert.IsTrue(noOfGridCellObservations.Count >= noOfGridCellObservations2.Count);
            if (noOfGridCellObservations.Count == noOfGridCellObservations2.Count)
            {
                for (int i = 0; i < noOfGridCellObservations2SortedList.Count; i++)
                {
                    Assert.IsTrue(noOfGridCellObservationsSortedList[i].Count >= noOfGridCellObservations2SortedList[i].SpeciesObservationCount);
                }
            }


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetGridCellSpeciesObservationCountsFailedNoCriteriasSetTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;

            WebSpeciesObservationSearchCriteria searchCriteria = null;
            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetGridCellSpeciesObservationCountsFailedNoCoordinateSystemSetTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = null;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsAccurrancyTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.Accuracy = 50;


            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

            // Increase Accurancy
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.Accuracy = 5000;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetGridCellSpeciesObservationCountsAccurancyFailedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            searchCriteria.Accuracy = -3;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.IncludePositiveObservations = true;
            WebGridSpecification webGridSpecification = null;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsIsAccurrancySpecifiedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Don't use accurancy, all positiv observations should be collected
            searchCriteria.IsAccuracySpecified = false;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);


            // Enable Accurancy
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IsAccuracySpecified = true;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count < noOfGridCellObservations.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetGridCellSpeciesObservationCountsAccurracyIsLessThanZeroTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = -1;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;

            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsGridSpecificationBoundingBoxTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 5;
            SetDefaultSearchCriteria(searchCriteria);

            // Test BoundingBbox
            WebBoundingBox testBox = new WebBoundingBox();
            testBox.Max = new WebPoint(75, 75);
            testBox.Min = new WebPoint(0, 0);
            WebBoundingBox testBox2 = new WebBoundingBox();
            testBox2.Max = new WebPoint(820000, 6781000);
            testBox2.Min = new WebPoint(560000, 6122000);


            searchCriteria.BoundingBox = testBox;
            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            searchCriteria.BoundingBox = null;
            webGridSpecification.BoundingBox = testBox2;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetGridCellSpeciesObservationCountsGridSpecificationBoundingBoxFailedTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            // searchCriteria.Accuracy = 5;
            SetDefaultSearchCriteria(searchCriteria);

            // Test BoundingBox
            WebBoundingBox testBox = new WebBoundingBox();
            testBox.Max = new WebPoint(30, 30);
            testBox.Min = new WebPoint(0, 0);
            WebBoundingBox testBox2 = new WebBoundingBox();
            testBox.Max = new WebPoint(90, 90);
            testBox.Min = new WebPoint(0, 0);
            searchCriteria.BoundingBox = testBox;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.BoundingBox = testBox;
            webGridSpecification.BoundingBox = testBox2;
            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_GoogleMercator_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 1;
            SetDefaultSearchCriteria(searchCriteria);

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }

        private static void SetDefaultSearchCriteria(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            searchCriteria.TaxonIds = new List<int>();
            //            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            // searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.IsAccuracySpecified = false;
            searchCriteria.Accuracy = 50;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 08, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.IncludePositiveObservations = true;
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

        private static void SetProjectParameterFieldSearchCriterias(WebSpeciesObservationSearchCriteria searchCriteria)
        {
        var    fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();

            SetProjectParameterFieldSearchCriterias(fieldSearchCriterias);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        private static void SetProjectParameterFieldSearchCriterias(List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Project);
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.None) { Identifier = "ProjectParameterSpeciesGateway_ProjectParameter93" };
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "obefintligt";
            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        private static void SetLocationIdFieldSearchCriterias(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias;
            fieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();

            SetLocationIdFieldSearchCriteria(fieldSearchCriterias);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        private static void SetLocationIdFieldSearchCriteria(List<WebSpeciesObservationFieldSearchCriteria> fieldSearchCriterias)
        {
            WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new WebSpeciesObservationClass(SpeciesObservationClassId.Location);
            fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Property = new WebSpeciesObservationProperty(SpeciesObservationPropertyId.LocationId);
            fieldSearchCriteria.Type = WebDataType.String;
            fieldSearchCriteria.Value = "1334";
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


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_WGS84_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 1;
            //searchCriteria.IsAccuracySpecified = true;
            SetDefaultSearchCriteria(searchCriteria);

            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(89, 89);
            searchCriteria.BoundingBox.Min = new WebPoint(10, 10);

            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);



        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_SWEREF99_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 10;
            //searchCriteria.IsAccuracySpecified = true;
            SetDefaultSearchCriteria(searchCriteria);

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            // Test BoundingBox
            searchCriteria.BoundingBox = new WebBoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            // SWEREF 99	6110000 – 7680000	260000 – 920000

            searchCriteria.BoundingBox.Max = new WebPoint(820000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(560000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_RT90_25_gon_v_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 1;
            //searchCriteria.IsAccuracySpecified = true;
            SetDefaultSearchCriteria(searchCriteria);

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new WebPoint(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_RT90_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 1;
            //searchCriteria.IsAccuracySpecified = true;
            SetDefaultSearchCriteria(searchCriteria);

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();


            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new WebPoint(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetGridCellSpeciesObservationCountsBoundingBoxNoneTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.None;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 1;
            //searchCriteria.IsAccuracySpecified = true;
            SetDefaultSearchCriteria(searchCriteria);

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;
            //webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;

            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(90, 90);
            searchCriteria.BoundingBox.Min = new WebPoint(0, 0);

            searchCriteria.IncludePositiveObservations = true;

            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBoxInvalidMaxMinValuesTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 1;
            //searchCriteria.IsAccuracySpecified = true;
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            // searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();
            //Ok boundig box values
            //searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            //searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);
            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                searchCriteria.BoundingBox.Min = new WebPoint(9993195, 1118890);
                AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);


            }
            catch (ArgumentException)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = new WebPoint(1113195, 31118890);
                    AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

                }
                catch (ArgumentException)
                {

                    // Ok if we get here
                    return;
                }
                catch (Exception)
                {
                    Assert.Fail("No argument exception thrown that YMin value is larger that YMax value for bounding box.");
                }
                Assert.Fail("No argument exception thrown that YMin value is larger that YMax value for bounding box.");

            }
            catch (Exception)
            {
                Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");
            }
            Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBoxNullMaxMinValuesTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();

            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = null;
                searchCriteria.BoundingBox.Min = new WebPoint(9993195, 1118890);
                AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            }
            catch (ArgumentException)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = null;
                    AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

                }
                catch (ArgumentException)
                {

                    // Ok if we get here
                    return;
                }
                catch (Exception)
                {
                    Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");
                }
                Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");

            }
            catch (Exception)
            {
                Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");
            }
            Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsChangeDateTest()
        {
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservations, noOfGridCellObservations2;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 07, 11);
            noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2016, 01, 01);

            noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsChangePartOfYearTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            // Get all data without using any intervals
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 08, 01);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get data from Feb March and April
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 08, 01);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;

            // Get less amount of data since only two mounth.
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get data from April
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 08, 01);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get data from April and May
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 08, 01);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 05, 01);
            interval2.End = new DateTime(2012, 05, 10);
            intervals.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get data from May
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 08, 01);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 05, 01);
            interval2.End = new DateTime(2015, 05, 10);
            intervals.Add(interval2);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get April and May
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2015, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 08, 01);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 04, 01);
            interval3.End = new DateTime(2012, 05, 10);
            intervals3.Add(interval3);
            searchCriteria.ChangeDateTime.PartOfYear = intervals3;
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservations6 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations6.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations2.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations3.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations4.Count);
            Assert.IsTrue(noOfGridCellObservations.Count >= noOfGridCellObservations5.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations6.Count);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations3.Count);
            Assert.IsTrue(noOfGridCellObservations3.Count < noOfGridCellObservations5.Count);
            Assert.IsTrue(noOfGridCellObservations4.Count < noOfGridCellObservations5.Count);
            Assert.IsTrue(noOfGridCellObservations5.Count >= noOfGridCellObservations6.Count);
        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsDataProvidersTest()
        {

            WebCoordinateSystem coordinateSystem;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //webGridSpecifications.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            searchCriteria.DataProviderGuids = guids as List<string>;
            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            searchCriteria.DataProviderGuids = guids as List<string>;
            searchCriteria.IncludePositiveObservations = true;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetGridCellSpeciesObservationCountsDataProviderInvalidTest()
        {
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 1;
            //searchCriteria.IsAccuracySpecified = true;
            SetDefaultSearchCriteria(searchCriteria);
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataInvalidProvider:1");
            searchCriteria.DataProviderGuids = guids as List<string>;

            searchCriteria.IncludePositiveObservations = true;

            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsDifferentGridCoordinateSystemsTestToWGS84()
        {
            // Test 
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservationsRT90;
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservationsSWEREF99;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebGridSpecification webGridSpecification;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 10;
            //searchCriteria.IsAccuracySpecified = true;
            //searchCriteria.IncludePositiveObservations = true;
            //taxonIds = new List<Int32>();
            //taxonIds.Add(3000176); // Hopprätvingar
            //searchCriteria.TaxonIds = taxonIds;
            SetDefaultSearchCriteria(searchCriteria);

            webGridSpecification = new WebGridSpecification();
            //gridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            noOfGridCellObservationsRT90 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservationsRT90.Count > 0);

            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            noOfGridCellObservationsSWEREF99 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservationsSWEREF99.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsDifferentGridCoordinateSystemsTestToGoogleMercatorTest()
        {

            // Giltiga värden för RT90 och SWEREF99 (Sverge värden)
            // System	    N-värde	             E-värde
            // RT90	        6110000 – 7680000	1200000 – 1900000
            // SWEREF 99	6110000 – 7680000	260000 – 920000

            List<WebGridCellSpeciesObservationCount> noOfGridCellObservationsRT90;
            List<WebGridCellSpeciesObservationCount> noOfGridCellObservationsSWEREF99;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            WebGridSpecification webGridSpecification;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.IncludePositiveObservations = true;
            //searchCriteria.Accuracy = 1;
            //searchCriteria.IsAccuracySpecified = true;
            SetDefaultSearchCriteria(searchCriteria);


            webGridSpecification = new WebGridSpecification();
            //gridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            noOfGridCellObservationsRT90 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservationsRT90.Count > 0);


            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            noOfGridCellObservationsSWEREF99 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservationsSWEREF99.Count > 0);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsIsNaturalOccuranceTest()
        {
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //gridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 10000;
            webGridSpecification.IsGridCellSizeSpecified = false;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 500;
            //searchCriteria.IsAccuracySpecified = false;

            //searchCriteria.IncludePositiveObservations = true;
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(233790); // Större flamingo

            searchCriteria.TaxonIds = taxa;
            searchCriteria.IsNaturalOccurrence = false;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            taxa = new List<int>();
            taxa.Add(233790); // Större flamingo
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);


            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations2.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsLocalityTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 100;
            searchCriteria.TaxonIds = null;
            searchCriteria.IsAccuracySpecified = true;
            WebCoordinateSystem coordinateSystem;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //gridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsLocalityAllConditionsTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebCoordinateSystem coordinateSystem;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.TaxonIds = null;
            WebStringSearchCriteria localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Can only set one stringCompareOperator 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            localityString.CompareOperators = stringOperators;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            localityString.CompareOperators = stringOperators;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            localityString.CompareOperators = stringOperators;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            localityString.CompareOperators = stringOperators;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            localityString.CompareOperators = stringOperators;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations6 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations6.Count > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsCriteriaObservationTypeTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations6 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations7 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations8 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count >= noOfGridCellObservations.Count);
            Assert.IsTrue(noOfGridCellObservations5.Count >= noOfGridCellObservations4.Count);
            Assert.IsTrue(noOfGridCellObservations6.Count == noOfGridCellObservations.Count);
            Assert.IsTrue(noOfGridCellObservations7.Count >= noOfGridCellObservations.Count);
            Assert.IsTrue(noOfGridCellObservations8.Count >= noOfGridCellObservations3.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObservationDateTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);
            Assert.IsTrue(noOfGridCellObservations3.Count >= noOfGridCellObservations2.Count);


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetGridCellSpeciesObservationCountsObservationDateCompareOperatorFailedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.NotEqual;
            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetGridCellSpeciesObservationCountsObservationDateInvalidDatesTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 08, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 09, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2001, 01, 01);
            interval2.End = new DateTime(2001, 08, 31);
            intervals2.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Adding one more time interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 08, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2001, 01, 01);
            interval2.End = new DateTime(2001, 08, 31);
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get the last two intervals but as one interval
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2000, 12, 01);
            interval3.End = new DateTime(2001, 08, 31);
            intervals3.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations6 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations6.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations2.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations3.Count);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations3.Count);
            Assert.IsTrue((noOfGridCellObservations5.Count + noOfGridCellObservations4.Count) >= noOfGridCellObservations6.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearIsDayOfYearSpecifiedTest1()
        {
            // Test 
            int taxonId = 201062; //Nässelfjäril
            //  taxonId = Convert.ToInt32(TaxonId.Grasshoppers);
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification gridSpecification = new WebGridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(taxonId);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 01, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2010, 06, 30);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Get less amount of data since only two mounth within a year
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(taxonId);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 01, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;

            // Get small part of a year data 
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 07, 01);
            interval2.End = new DateTime(2011, 08, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearIsDayOfYearSpecifiedTest2()
        {
            const int ButterflyTaxonId = 201062; // Nässelfjäril
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification gridSpecification = new WebGridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(ButterflyTaxonId);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 08, 01);
            interval.End = new DateTime(2010, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Get small part of a year data 
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2011, 01, 01);
            interval2.End = new DateTime(2011, 07, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);

            // Adding one more time interval to the first one 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(ButterflyTaxonId);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);

            // Get the last two intervals but as one interval compare that on interval and two interval is equal.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(ButterflyTaxonId);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 08, 01);
            interval3.End = new DateTime(2011, 07, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);

            // Not using day of year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(ButterflyTaxonId);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;
            intervals3 = new List<WebDateTimeInterval>();
            interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 08, 01);
            interval3.End = new DateTime(2011, 07, 31);
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count >= noOfGridCellObservations3.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearIsDayOfYearSpecifiedTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 08, 01);
            interval.End = new DateTime(2010, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Get less amount of data since only two mounth within a year
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 09, 30);
            interval2.End = new DateTime(2011, 01, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Adding one more time interval to the first one 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 08, 01);
            interval.End = new DateTime(2010, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2011, 01, 01);
            interval2.End = new DateTime(2011, 05, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get the last two intervals but as one interval compare that on einterval and two interval is equal.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 08, 01);
            interval3.End = new DateTime(2011, 05, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Not using day of year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals3 = new List<WebDateTimeInterval>();
            interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 08, 01);
            interval3.End = new DateTime(2011, 05, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count >= noOfGridCellObservations3.Count);
        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridSpeciesCounts_DalarnaRegion_DalarnaGridCellsReturned()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.SWEREF99_TM };
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet16Feature16");

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 10000;

            IList<WebGridCellSpeciesCount> gridCells = AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(gridCells.Count < 10000);
        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridSpeciesObservationCounts_DalarnaRegion_DalarnaGridCellsReturned()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.SWEREF99_TM };
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:Artportalen.se:Area:DataSet16Feature16");

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 10000;

            List<WebGridCellSpeciesObservationCount> gridCells = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(gridCells.Count < 10000);
        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridSpeciesCounts_SwedishExtent_NoCellsReturnedOutsideSwedishExtent()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.SWEREF99_TM };
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 10000;
            webGridSpecification.BoundingBox = GeometryManager.GetSwedenExtentBoundingBox(webGridSpecification.GridCoordinateSystem.ToWebCoordinateSystem());

            WebBoundingBox swedenExtentBoundingBox = GeometryManager.GetSwedenExtentBoundingBox(coordinateSystem);
            IList<WebGridCellSpeciesCount> gridCells = AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            int cellSize = webGridSpecification.GridCellSize;
            double xMin = Math.Floor(swedenExtentBoundingBox.Min.X / cellSize) * cellSize;
            double xMax = Math.Ceiling(swedenExtentBoundingBox.Max.X / cellSize) * cellSize;
            double yMin = Math.Floor(swedenExtentBoundingBox.Min.Y / cellSize) * cellSize;
            double yMax = Math.Ceiling(swedenExtentBoundingBox.Max.Y / cellSize) * cellSize;

            for (int i = 0; i < gridCells.Count; i++)
            {
                WebGridCellSpeciesCount gridCell = gridCells[i];
                Assert.IsFalse(gridCell.OrginalBoundingBox.Min.X < xMin);
                Assert.IsFalse(gridCell.OrginalBoundingBox.Min.Y < xMax);
                Assert.IsFalse(gridCell.OrginalBoundingBox.Max.X > yMin);
                Assert.IsFalse(gridCell.OrginalBoundingBox.Max.Y > yMax);
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridSpeciesObservationCounts_SwedishExtent_NoCellsReturnedOutsideSwedishExtent()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.SWEREF99_TM };
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 10000;
            webGridSpecification.BoundingBox = GeometryManager.GetSwedenExtentBoundingBox(webGridSpecification.GridCoordinateSystem.ToWebCoordinateSystem());

            WebBoundingBox swedenExtentBoundingBox = GeometryManager.GetSwedenExtentBoundingBox(coordinateSystem);
            List<WebGridCellSpeciesObservationCount> gridCells = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            int cellSize = webGridSpecification.GridCellSize;
            double xMin = Math.Floor(swedenExtentBoundingBox.Min.X / cellSize) * cellSize;
            double xMax = Math.Ceiling(swedenExtentBoundingBox.Max.X / cellSize) * cellSize;
            double yMin = Math.Floor(swedenExtentBoundingBox.Min.Y / cellSize) * cellSize;
            double yMax = Math.Ceiling(swedenExtentBoundingBox.Max.Y / cellSize) * cellSize;

            for (int i = 0; i < gridCells.Count; i++)
            {
                WebGridCellSpeciesObservationCount gridCell = gridCells[i];
                Assert.IsFalse(gridCell.OrginalBoundingBox.Min.X < xMin);
                Assert.IsFalse(gridCell.OrginalBoundingBox.Min.Y < xMax);
                Assert.IsFalse(gridCell.OrginalBoundingBox.Max.X > yMin);
                Assert.IsFalse(gridCell.OrginalBoundingBox.Max.Y > yMax);
            }
        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearFailedTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 04, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2008, 06, 01);
            interval.End = new DateTime(2000, 03, 01);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObserverSearchStringAllConditionsTest()
        {
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            WebStringSearchCriteria operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Can only set one stringCompareOperator 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations6 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations6.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObserverSearchStringTest()
        {

            WebCoordinateSystem coordinateSystem;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            //SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;

            WebStringSearchCriteria operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;

            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsPolygonsTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test search criteria Polygons.
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations2.Count);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsPolygonsDifferentCoordinateSystemsTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            CoordinateConversionManager coordinateConversionManager = new CoordinateConversionManager();
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Create polygon
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            searchCriteria.IncludePositiveObservations = true;
            // WGS84
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            //GoogleMercator
            WebCoordinateSystem coordinateSystemMercator;
            coordinateSystemMercator = new WebCoordinateSystem();
            coordinateSystemMercator.Id = CoordinateSystemId.GoogleMercator;
            WebPolygon polygonMercator = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem, coordinateSystemMercator);
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonMercator);
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystemMercator);

            //RT90
            WebCoordinateSystem coordinateSystemRT90;
            coordinateSystemRT90 = new WebCoordinateSystem();
            coordinateSystemRT90.Id = CoordinateSystemId.Rt90_25_gon_v;
            WebPolygon polygonRT90 = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem, coordinateSystemRT90);
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonRT90);
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystemRT90);

            //Rt90_25_gon_v
            WebCoordinateSystem coordinateSystemRT90_25_gon_v;

            coordinateSystemRT90_25_gon_v = new WebCoordinateSystem();
            coordinateSystemRT90_25_gon_v.Id = CoordinateSystemId.Rt90_25_gon_v;
            WebPolygon polygonRT90_25_gon_v = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem, coordinateSystemRT90_25_gon_v);
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonRT90_25_gon_v);
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystemRT90_25_gon_v);

            //SWEREF99
            WebCoordinateSystem coordinateSystemSWEREF99;
            coordinateSystemSWEREF99 = new WebCoordinateSystem();
            coordinateSystemSWEREF99.Id = CoordinateSystemId.SWEREF99_TM;
            WebPolygon polygonSWEREF99 = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem, coordinateSystemSWEREF99);
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonSWEREF99);
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystemSWEREF99);


            // Since conversion between coordinate systems are not excact we have a bit of
            // difference in number of observations in our db searches. If conversion of
            // coordinate systems were exact the number of observations should not differ.
            // Allowing 3 % difference in result
            double delta = noOfGridCellObservations.Count * 0.03;
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count == noOfGridCellObservations2.Count);
            Assert.AreEqual(noOfGridCellObservations.Count, noOfGridCellObservations3.Count, delta);
            Assert.AreEqual(noOfGridCellObservations.Count, noOfGridCellObservations4.Count, delta);
            Assert.AreEqual(noOfGridCellObservations.Count, noOfGridCellObservations5.Count, delta);


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRegionTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRegionAccessRightsTest()
        {
            Int64 speciesObservationCount1, speciesObservationCount2;
            WebCoordinateSystem coordinateSystem;
            WebClientInformation clientInformation;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            clientInformation = new WebClientInformation();
            clientInformation.Locale = LoginResponse.Locale;
            clientInformation.Token = LoginResponse.Token;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(440); // cinnoberspindling

            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                speciesObservationCount1 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(context,
                                                                                                      searchCriteria,
                                                                                                      coordinateSystem);
            }
            Assert.IsTrue(speciesObservationCount1 > 0);

            clientInformation.Role = LoginResponse.Roles[0];
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(440); // cinnoberspindling
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                speciesObservationCount2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(context,
                                                                                                      searchCriteria,
                                                                                                      coordinateSystem);
            }
            Assert.IsTrue(speciesObservationCount2 >= 0);
            Assert.IsTrue(speciesObservationCount1 > speciesObservationCount2);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountsRegionAndTaxaTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsRegistationDateTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 07, 25);
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 01, 01);

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsRegistationPartOfYearTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            // Get complete years data
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2012, 03, 31);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get small part of a year data only one month
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations3 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 05, 01);
            interval2.End = new DateTime(2012, 05, 10);
            intervals2.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals2;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations4 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Adding one more time interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 05, 01);
            interval2.End = new DateTime(2012, 05, 10);
            intervals.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations5 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            // Get the last two intervals but as one interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 04, 01);
            interval3.End = new DateTime(2012, 05, 10);
            intervals3.Add(interval3);
            searchCriteria.ChangeDateTime.PartOfYear = intervals3;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations6 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations6.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations2.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations3.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations4.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations5.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations6.Count);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations3.Count);
            Assert.IsTrue(noOfGridCellObservations3.Count < noOfGridCellObservations5.Count);
            Assert.IsTrue(noOfGridCellObservations4.Count < noOfGridCellObservations5.Count);
            Assert.IsTrue(noOfGridCellObservations5.Count <= noOfGridCellObservations6.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsRedListTaxaTest()
        {

            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);



            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test taxa list
            taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true; searchCriteria.IncludeRedlistedTaxa = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsRedListCategoriesTest()
        {

            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            //webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test taxa list
            taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;


            List<RedListCategory> redListCategories = new List<RedListCategory>();
            RedListCategory redListCategory;
            redListCategory = RedListCategory.EN;
            redListCategories.Add(redListCategory);
            searchCriteria.IncludeRedListCategories = redListCategories;


            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations2 = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsTaxaTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(2001274); // Myggor
            taxa.Add(2002088);// Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsUsedAllCriteriasTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellSize = 50000;
            webGridSpecification.IsGridCellSizeSpecified = true;


            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            List<int> taxa = new List<int>();
            taxa.Add((Int32)(TaxonId.DrumGrasshopper));
            taxa.Add((Int32)(TaxonId.Carnivore));


            searchCriteria.TaxonIds = taxa;

            // Test BoundingBox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(89, 89);
            searchCriteria.BoundingBox.Min = new WebPoint(10, 10);

            // Create polygon in WGS84
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            // Set Observation date and time interval.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 03, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Set dataproviders
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            searchCriteria.DataProviderGuids = guids as List<string>;

            // Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.


            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }

        [TestCategory("NightlyTest")]
        [TestMethod]
        public void GetGridSpeciesObservationCountsBoundingPolygonTypeTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 5000;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GeometryType.Equals(GridCellGeometryType.Polygon));
            Assert.IsTrue(noOfGridCellObservations[0].CentreCoordinate.IsNotNull());

        }

        [TestCategory("NightlyTest")]
        [TestMethod]
        public void GetGridSpeciesObservationCountsCentrePointTypeTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 5000;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.CentrePoint;

            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GeometryType.Equals(GridCellGeometryType.CentrePoint));
            Assert.IsTrue(noOfGridCellObservations[0].BoundingBox.IsNull());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridSpeciesObservationCountsMaxGridCellsCount()
        {
            var webGridSpecification = new WebGridSpecification
            {
                IsGridCellSizeSpecified = true,
                GridCellSize = 50000,
                GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM,
                GridCellGeometryType = GridCellGeometryType.CentrePoint
            };
            var searchCriteria = new WebSpeciesObservationSearchCriteria
            {
                ObservationDateTime = new WebDateTimeSearchCriteria()
                {
                    Begin = new DateTime(2010, 08, 01),
                    End = new DateTime(2010, 08, 01),
                    Operator = CompareOperator.Including
                },
                IncludePositiveObservations = true
            };
            var t = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator });
            Assert.IsTrue(t.Any());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridSpeciesObservationCountsMaxGridCellsCountFails()
        {
            var webGridSpecification = new WebGridSpecification
            {
                IsGridCellSizeSpecified = true,
                GridCellSize = 1000,
                GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM,
                GridCellGeometryType = GridCellGeometryType.CentrePoint
            };
            var searchCriteria = new WebSpeciesObservationSearchCriteria
            {
                ObservationDateTime = new WebDateTimeSearchCriteria()
                {
                    Begin = new DateTime(2010, 08, 01),
                    End = new DateTime(2010, 08, 01),
                    Operator = CompareOperator.Including
                },
                IncludePositiveObservations = true
            };
            try
            {
                var t = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification,
                    new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator });
                Assert.Fail("Didn't get an ArgumentException");
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("Grid cell size is too"));
            }
            catch
            {
                Assert.Fail("Didn't get an ArgumentException");
            }
        }

        [TestMethod]
        public void GetGridCellCount_WhenMultiPolygon_ThenReturnCorrectNumberOfCells()
        {
            //Arrange
            var gridSpecification = new WebGridSpecification
            {
                BoundingBox = null,                
                GridCellGeometryType = GridCellGeometryType.Polygon,
                GridCellSize = 10000,
                GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM,
                IsDataChecked = false,
                IsGridCellSizeSpecified = true
            };

            var coordinateSystem = new WebCoordinateSystem
            {                
                Id = CoordinateSystemId.GoogleMercator,
                IsDataChecked = false,
                WKT = ""
            };

            var searchCriteria = new WebSpeciesObservationSearchCriteria
            {              
                Polygons = GetTwoNonOverlappingPolygonsInGoogleMercator()
            };
            
            //Act
            var count = searchCriteria.GetGridCellCount(coordinateSystem, gridSpecification);

            //Assert
            Assert.IsTrue(count > 0);
        }

        /// <summary>
        /// Returns a list with 2 polygons that isn't overlapping each other.
        /// </summary>
        private List<WebPolygon> GetTwoNonOverlappingPolygonsInGoogleMercator()
        {
            var polygons = new List<WebPolygon>
            {
                new WebPolygon
                {                    
                    LinearRings = new List<WebLinearRing>
                    {
                        new WebLinearRing
                        {                            
                            Points = new List<WebPoint>
                            {
                                new WebPoint
                                {                                    
                                    X = 1575904.8076744,
                                    Y = 8560104.3847779,
                                },
                                new WebPoint
                                {                                    
                                    X = 1575904.8076744,
                                    Y = 8662835.7507789,
                                },
                                new WebPoint
                                {                                    
                                    X = 1695758.0680089002,
                                    Y = 8662835.7507789,
                                },
                                new WebPoint
                                {                                    
                                    X = 1695758.0680089002,
                                    Y = 8560104.3847779,
                                },
                                new WebPoint
                                {                                    
                                    X = 1575904.8076744,
                                    Y = 8560104.3847779,                                    
                                }
                            }
                        }
                    }
                },
                new WebPolygon
                {                    
                    LinearRings = new List<WebLinearRing>
                    {
                        new WebLinearRing
                        {                            
                            Points = new List<WebPoint>
                            {
                                new WebPoint
                                {                                    
                                    X = 1754461.7057238,
                                    Y = 7982851.9472485995,
                                },
                                new WebPoint
                                {                                    
                                    X = 1754461.7057238,
                                    Y = 8100259.2226782972,
                                },
                                new WebPoint
                                {                                    
                                    X = 1871868.9811535003,
                                    Y = 8100259.2226782972,
                                },
                                new WebPoint
                                {                                    
                                    X = 1871868.9811535003,
                                    Y = 7982851.9472485995,
                                },
                                new WebPoint
                                {                                                                        
                                    X = 1754461.7057238,
                                    Y = 7982851.9472485995,
                                }
                            }
                        }
                    }
                }
            };

            return polygons;
        }
            
            
            
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void SearchCriteria_GetGridCellCount_WorksWithLocalityRegionsAndTaxonIds()
        {
            //When searching without GridCellSize no cell count validation shall be performed
            Assert.IsTrue(SearchCriteria_GetGridCellCount(false, null, null, null).Equals(0));

            //When searching with Region(s) no cell count validation shall be performed
            Assert.IsTrue(SearchCriteria_GetGridCellCount(true, null, new List<string> { "Dummy text" }, null).Equals(0));

            //When searching with Locality no cell count validation shall be performed
            Assert.IsTrue(SearchCriteria_GetGridCellCount(true, new WebStringSearchCriteria(), null, null).Equals(0));

            //When searching with TaxonIds no cell count validation shall be performed
            Assert.IsTrue(SearchCriteria_GetGridCellCount(true, null, null, new List<int> { -1 }).Equals(0));

            //When searching without Locality and Region(s) cell count validation shall be performed (and should return more than 1 in grid count)
            var count1 = SearchCriteria_GetGridCellCount(true, null, null, null);
            var count2 = SearchCriteria_GetGridCellCountWgs84(true, null, null, null);
            Assert.IsTrue(SearchCriteria_GetGridCellCount(true, null, null, null).CompareTo(0) == 1);
        }

        private double SearchCriteria_GetGridCellCount(bool isGridCellSizeSpecified, WebStringSearchCriteria localityNameSearchString, List<string> regionGuids, List<int> taxonIds)
        {
            var webGridSpecification = new WebGridSpecification
            {
                IsGridCellSizeSpecified = isGridCellSizeSpecified,
                GridCellSize = 10000,
                GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM,
                GridCellGeometryType = GridCellGeometryType.CentrePoint
            };
            var searchCriteria = new WebSpeciesObservationSearchCriteria
            {
                LocalityNameSearchString = localityNameSearchString,
                RegionGuids = regionGuids,
                TaxonIds = taxonIds,
                ObservationDateTime = new WebDateTimeSearchCriteria()
                {
                    Begin = new DateTime(2010, 08, 01),
                    End = new DateTime(2010, 08, 01),
                    Operator = CompareOperator.Including
                },
                IncludePositiveObservations = true,
                Polygons = new List<WebPolygon> { GeometryManager.GetSwedenExtentBoundingBoxPolygon(new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator }) }
            };

            return searchCriteria.GetGridCellCount(new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator }, webGridSpecification);
        }

        private double SearchCriteria_GetGridCellCountWgs84(bool isGridCellSizeSpecified, WebStringSearchCriteria localityNameSearchString, List<string> regionGuids, List<int> taxonIds)
        {
            var webGridSpecification = new WebGridSpecification
            {
                IsGridCellSizeSpecified = isGridCellSizeSpecified,
                GridCellSize = 10000,
                GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM,
                GridCellGeometryType = GridCellGeometryType.CentrePoint
            };
            var searchCriteria = new WebSpeciesObservationSearchCriteria
            {
                LocalityNameSearchString = localityNameSearchString,
                RegionGuids = regionGuids,
                TaxonIds = taxonIds,
                ObservationDateTime = new WebDateTimeSearchCriteria()
                {
                    Begin = new DateTime(2010, 08, 01),
                    End = new DateTime(2010, 08, 01),
                    Operator = CompareOperator.Including
                },
                IncludePositiveObservations = true,
                Polygons = new List<WebPolygon> { GeometryManager.GetSwedenExtentBoundingBoxPolygon(new WebCoordinateSystem { Id = CoordinateSystemId.WGS84 }) }
            };

            return searchCriteria.GetGridCellCount(new WebCoordinateSystem { Id = CoordinateSystemId.WGS84 }, webGridSpecification);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetAOOEOOConvexHullTest()
        {
            var coordinateSystem = new WebCoordinateSystem()
            {
                Id = CoordinateSystemId.GoogleMercator
            };

            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds = new List<int>()
            {
               /* 101509 ,  //Apollofjäril Redlisted NE-category
                2002088, //Duvor
                2002118, //Kråkfåglar*/
                220396 //Tussilago*/
            };
            var webGridSpecification = new WebGridSpecification()
            {
                IsGridCellSizeSpecified = true,
                GridCellSize = 5000,
                GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM,
                GridCellGeometryType = GridCellGeometryType.Polygon
            };

            var gridCells = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);

            var geoJson = AnalysisManager.GetSpeciesObservationAOOEOOAsGeoJson(gridCells);
            
            Assert.IsTrue(geoJson != null);     
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetAOOEOOConcaveHullTest()
        {
            var coordinateSystem = new WebCoordinateSystem()
            {
                Id = CoordinateSystemId.GoogleMercator
            };
            
            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            searchCriteria.TaxonIds = new List<int>()
            {
               /* 101509 ,  //Apollofjäril Redlisted NE-category
                2002088, //Duvor
                2002118, //Kråkfåglar*/
                220396 //Tussilago
                //103038 //Gråsparv
            };

            var webGridSpecification = new WebGridSpecification()
            {
                IsGridCellSizeSpecified = true,
                GridCellSize = 5000,
                GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM,
                GridCellGeometryType = GridCellGeometryType.Polygon
            };
            var gridCells = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
           
            var geoJson = AnalysisManager.GetSpeciesObservationAOOEOOAsGeoJson(gridCells, 150);

            Assert.IsTrue(geoJson != null);
        }

        //[TestMethod]
        //[Ignore]
        //[TestCategory("NightlyTest")]
        //public void GetGridCellSpeciesObservationCountsPublicAutorityTest()
        //{
        //    WebCoordinateSystem coordinateSystem;

        //    coordinateSystem = new WebCoordinateSystem();
        //    coordinateSystem.Id = CoordinateSystemId.WGS84;

        //    WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
        //    searchCriteria.Accuracy = 1;
        //    searchCriteria.IsAccuracySpecified = true;
        //    searchCriteria.IncludePositiveObservations = true;
        //    searchCriteria.MaxProtectionLevel = 1;
        //    searchCriteria.MinProtectionLevel = 1;
        //    IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridCellSpeciesObservationCounts(Context, searchCriteria, null, coordinateSystem);

        //    Assert.IsTrue(noOfGridCellObservations.Count > 0);

        //}

        //[TestMethod]
        //[Ignore]
        //[TestCategory("NightlyTest")]
        //public void GetGridCellSpeciesObservationCountsHigherAutorityTest()
        //{
        //    WebCoordinateSystem coordinateSystem;

        //    coordinateSystem = new WebCoordinateSystem();
        //    coordinateSystem.Id = CoordinateSystemId.WGS84;

        //    WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
        //    searchCriteria.Accuracy = 1;
        //    searchCriteria.IsAccuracySpecified = true;
        //    searchCriteria.IncludePositiveObservations = true;
        //    searchCriteria.MaxProtectionLevel = 5;
        //    //Todo Should be able to run from level 2 and higher
        //    searchCriteria.MinProtectionLevel = 1;
        //    IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridCellSpeciesObservationCounts(Context, searchCriteria, null, coordinateSystem);

        //    Assert.IsTrue(noOfGridCellObservations.Count > 0);

        //}

        //[TestMethod]
        //[Ignore]
        //[TestCategory("NightlyTest")]
        //public void GetGridCellSpeciesObservationCountsTaxaAutorityTest()
        //{
        //    WebCoordinateSystem coordinateSystem;

        //    coordinateSystem = new WebCoordinateSystem();
        //    coordinateSystem.Id = CoordinateSystemId.WGS84;

        //    WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
        //    searchCriteria.MaxProtectionLevel = 1;
        //    searchCriteria.MinProtectionLevel = 1;

        //    List<int> taxa = new List<int>();
        //    taxa.Add(Convert.ToInt32(TaxonId.GreenhouseMoths));
        //    taxa.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));

        //    List<int> authorityTaxa = new List<int>();
        //    taxa.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));

        //    searchCriteria.TaxonIds = taxa;
        //    // TODO make this autority work for taxa
        //    WebServiceContext testContext = Context;
        //    foreach (WebAuthority autority in testContext.CurrentRole.Authorities)
        //    {
        //        autority.TaxonGUIDs.Add("urn:lsid:dyntaxa.se:Taxon:101656");
        //    }

        //    IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridCellSpeciesObservationCounts(Context, searchCriteria, null, coordinateSystem);

        //    Assert.IsTrue(noOfGridCellObservations.Count > 0);

        //}

        //[TestMethod]
        //[Ignore]
        //[TestCategory("NightlyTest")]
        //public void GetSpeciesObservationCountBySearchCriteriaAuthorityRegionsTest()
        //{
        //    WebCoordinateSystem coordinateSystem;

        //    coordinateSystem = new WebCoordinateSystem();
        //    coordinateSystem.Id = CoordinateSystemId.WGS84;

        //    WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
        //    // Test bounding box
        //    List<int> regionIdTable = new List<int>();
        //    regionIdTable.Add(13); //Uppland

        //    searchCriteria.RegionGuids = new List<string>();
        //    // searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
        //    searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.

        //    WebServiceContext testContext = Context;
        //    WebRole role = testContext.CurrentRoles[0];
        //    WebAuthority authority = role.Authorities[0];
        //    authority.RegionGUIDs = new List<string>();
        //    authority.RegionGUIDs.Add("URN:LSID:artportalen.se:area:DataSet21Feature4");
        //    searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.

        //    // TODO RegionID is missing - failed - Dont work for this
        //    // IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridCellSpeciesObservationCounts(testContext, searchCriteria, null, coordinateSystem);
        //    Int64 noOfGridCellObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(testContext, searchCriteria, coordinateSystem);


        //    Assert.IsTrue(noOfGridCellObservations > 0);

        //}


        #endregion


        #region GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts


        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_Sweref99CoordinateSystem_ReturnListSuccessfully()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; // Grid specified in SWEREF99. Do calculations in this coordinate system.
            gridSpecification.GridCellSize = 100000; // Each square is 100 * 100 km.
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator; // Return the result converted to Google Mercator coordinates.
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties";

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            List<WebGridCellCombinedStatistics> result = AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem, featuresUrl);
            Assert.IsNotNull(result);
            Assert.AreEqual(74, result.Count);
            //Assert.AreEqual(80, result.Count);            
        }

        [TestMethod]
        //[Ignore]
        public void GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_Sweref99CoordinateSystem_ReturnListSuccessfullyElasticsearch()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; // Grid specified in SWEREF99. Do calculations in this coordinate system.
            gridSpecification.GridCellSize = 100000; // Each square is 100 * 100 km.
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator; // Return the result converted to Google Mercator coordinates.
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:artdatabankenslanskarta";

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            List<WebGridCellCombinedStatistics> result = AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCountsElasticsearch(Context, searchCriteria, gridSpecification, coordinateSystem, featuresUrl);
            Assert.IsNotNull(result);
            Assert.AreEqual(74, result.Count);
            //Assert.AreEqual(80, result.Count);            
        }

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_SearchCriteriaWithFilterResultingInNoSpeciesCountResult_ReturnListSuccessfully()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; // Grid specified in SWEREF99. Do calculations in this coordinate system.
            gridSpecification.GridCellSize = 100000; // Each square is 100 * 100 km.
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator; // Return the result converted to Google Mercator coordinates.
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties";

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(int.MaxValue - 1);

            List<WebGridCellCombinedStatistics> result = AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem, featuresUrl);
            Assert.IsNotNull(result);
            Assert.AreEqual(74, result.Count);
        }

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_FeaturesUrlResultingInNoFeatures_ReturnListSuccessfully()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; // Grid specified in SWEREF99. Do calculations in this coordinate system.
            gridSpecification.GridCellSize = 100000; // Each square is 100 * 100 km.
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator; // Return the result converted to Google Mercator coordinates.
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties&filter=<Filter><Or><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>300</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>301</Literal></PropertyIsEqualTo></Or></Filter>";
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            List<WebGridCellCombinedStatistics> result = AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem, featuresUrl);
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_FilterAndUrlResultingInNoFeatures_ReturnEmptyListSuccessfully()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; // Grid specified in SWEREF99. Do calculations in this coordinate system.
            gridSpecification.GridCellSize = 100000; // Each square is 100 * 100 km.
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator; // Return the result converted to Google Mercator coordinates.
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties&filter=<Filter><Or><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>300</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>301</Literal></PropertyIsEqualTo></Or></Filter>";
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(int.MaxValue - 1);

            List<WebGridCellCombinedStatistics> result = AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem, featuresUrl);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_WfsFilteringDalarnaPolygon_ReturnListSuccessfully()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            WebSpeciesObservationSearchCriteria searchCriteria;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; // Grid specified in SWEREF99. Do calculations in this coordinate system.
            gridSpecification.GridCellSize = 10000; // Each square is 10 * 10 km.
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator; // Return the result converted to Google Mercator coordinates.
            //featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties&filter=<Filter><PropertyIsEqualTo><PropertyName>L?nSKOD</PropertyName><Literal>20</Literal></PropertyIsEqualTo></Filter>";
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties&filter=<Filter><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>20</Literal></PropertyIsEqualTo></Filter>";

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            List<WebGridCellCombinedStatistics> result = AnalysisManager.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem, featuresUrl);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 370);
        }


        // Not true, they can be different
        //[TestMethod]
        //public void GetGridSpeciesCountsAndSpeciesObservations_SearchCriteraWithoutFilter_NumberOfGridsShouldBeEqual()
        //{
        //    WebCoordinateSystem coordinateSystem;
        //    WebGridSpecification gridSpecification;
        //    WebSpeciesObservationSearchCriteria searchCriteria;

        //    gridSpecification = new WebGridSpecification();
        //    coordinateSystem = new WebCoordinateSystem();
        //    gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; // Grid specified in SWEREF99. Do calculations in this coordinate system.
        //    gridSpecification.GridCellSize = 100000; // Each square is 100 * 100 km.
        //    gridSpecification.IsGridCellSizeSpecified = true;
        //    gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
        //    coordinateSystem.Id = CoordinateSystemId.GoogleMercator; // Return the result converted to Google Mercator coordinates.
        //    searchCriteria = new WebSpeciesObservationSearchCriteria();

        //    List<WebGridCellSpeciesCount> speciesCounts = AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, gridSpecification, coordinateSystem);
        //    List<WebGridCellSpeciesObservationCount> speciesObservationCounts = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);
        //    Assert.IsTrue(speciesCounts.Count == speciesObservationCounts.Count);

        //}


        #endregion

        #region  GetGridCellFeatureStatistics

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatistics_PointLayer_SomeSquaresWillHaveMoreThanOneCount()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 10000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            gridSpecification.BoundingBox = null;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            featuresUrl = "http://geodata.havochvatten.se/geoservices/hav-miljoovervakning/ows?service=wfs&version=1.1.0&request=GetFeature&typeName=hav-miljoovervakning:miljoovervakningsstationer";

            Uri featuresUri = new Uri(featuresUrl);
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);

            Assert.IsNotNull(gridCellFeatureStatistics);
            long maxCount = 0;
            foreach (WebGridCellFeatureStatistics cell in gridCellFeatureStatistics)
            {
                if (cell.FeatureCount > maxCount)
                {
                    maxCount = cell.FeatureCount;
                }
            }

            Assert.AreEqual(21, maxCount);
            Assert.IsTrue(maxCount > 1);
        }



        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatistics_WfsUrlWithFilter_ReturnsListWithFeatureStatistics()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            Stopwatch sp = new Stopwatch();

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            //gridSpecification.BoundingBox = new WebBoundingBox { Max = new WebPoint(), Min = new WebPoint() };

            // Sweden extent i SWEREF99
            //gridSpecification.BoundingBox.Max.X = 869927.0414948005;
            //gridSpecification.BoundingBox.Max.Y = 7737973.607602615;
            //gridSpecification.BoundingBox.Min.X = 195816.17626999575;
            //gridSpecification.BoundingBox.Min.Y = 6029444.002008331;

            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 100000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            //featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties";
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties&filter=<Filter><Or><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>5</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>6</Literal></PropertyIsEqualTo></Or></Filter>";
            sp.Start();
            Uri featuresUri = new Uri(featuresUrl);
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);
            sp.Stop();
            //Assert.AreEqual(0, 1, string.Format("Elapsed time: {0}ms", sp.ElapsedMilliseconds));
            System.Console.WriteLine("Elapsed time: {0}ms", sp.ElapsedMilliseconds);
            Assert.IsNotNull(gridCellFeatureStatistics);
        }

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatistics_WfsServiceWithSweref99CoordinateSystem_ReturnsListWithFeatureStatisticsCountGreaterThanZero()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.BoundingBox = new WebBoundingBox { Max = new WebPoint(), Min = new WebPoint() };

            // Sweden extent i SWEREF99
            gridSpecification.BoundingBox.Max.X = 869927.0414948005;
            gridSpecification.BoundingBox.Max.Y = 7737973.607602615;
            gridSpecification.BoundingBox.Min.X = 195816.17626999575;
            gridSpecification.BoundingBox.Min.Y = 6029444.002008331;

            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 100000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties";

            Uri featuresUri = new Uri(featuresUrl);
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);
            Assert.IsTrue(gridCellFeatureStatistics.Count > 72);
        }

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatistics_WfsServiceWithGoogleMercatorCoordinateSystem_ReturnsListWithFeatureStatisticsCountGreaterThanZero()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.BoundingBox = new WebBoundingBox { Max = new WebPoint(), Min = new WebPoint() };

            //// Sweden extent i Google Mercator
            gridSpecification.BoundingBox.Max.X = 2726661.6726091;
            gridSpecification.BoundingBox.Max.Y = 10905242.974458;
            gridSpecification.BoundingBox.Min.X = 1149001.4090228;
            gridSpecification.BoundingBox.Min.Y = 7231373.6474714;

            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;
            gridSpecification.GridCellSize = 100000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedenCounties";

            Uri featuresUri = new Uri(featuresUrl);
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);
            Assert.IsTrue(gridCellFeatureStatistics.Count > 283);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void GetGridCellFeatureStatistics_WfsServiceWithAllCounties_NoCalculatedAreaIsGreaterThanTheGridArea()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();

            gridSpecification.BoundingBox = null;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 10000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:artdatabankenslanskarta";
            //featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan";
            Uri featuresUri = new Uri(featuresUrl);
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);

            int maxGridCellArea = gridSpecification.GridCellSize * gridSpecification.GridCellSize;
            double maxCalculatedGridCellArea = 0;
            foreach (WebGridCellFeatureStatistics gridCell in gridCellFeatureStatistics)
            {
                if (gridCell.FeatureArea > maxCalculatedGridCellArea)
                {
                    maxCalculatedGridCellArea = gridCell.FeatureArea;
                }
            }

            Assert.IsTrue(maxCalculatedGridCellArea <= maxGridCellArea, string.Format("gridCell.FeatureArea = {0}", maxCalculatedGridCellArea));
        }

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatistics_WfsServiceWithLotsOfDataAndInvalidPolygon_ReturnsGridStatisticsSuccessfully()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();

            gridSpecification.BoundingBox = null;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 10000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            //for (int i = 100; i < 150; i++)
            //{
            //    featuresUrl = string.Format("http://pandora.slu.se:8080/geoserver/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=slu:RT_XX_G_05&filter=<Filter><PropertyIsEqualTo><PropertyName>Age_mean</PropertyName><Literal>{0}</Literal></PropertyIsEqualTo></Filter>",i);
            //    Uri featuresUri = new Uri(featuresUrl);
            //    IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
            //        AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, gridSpecification, coordinateSystem);
            //    Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
            //}


            for (int i = 120; i > 100; i--)
            {
                System.Diagnostics.Debug.WriteLine("Testing with start number: {0}", i);
                //featuresUrl = string.Format("http://pandora.slu.se:8080/geoserver/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=slu:RT_XX_G_05&filter=<Filter><PropertyIsEqualTo><PropertyName>Age_mean</PropertyName><Literal>{0}</Literal></PropertyIsEqualTo></Filter>", i);
                featuresUrl = string.Format("http://pandora.slu.se:8080/geoserver/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=slu:RT_XX_G_05&filter=<Filter><And><PropertyIsGreaterThan><PropertyName>Age_mean</PropertyName><Literal>{0}</Literal></PropertyIsGreaterThan><PropertyIsLessThanOrEqualTo><PropertyName>Age_mean</PropertyName><Literal>150</Literal></PropertyIsLessThanOrEqualTo></And></Filter>", i);
                Uri featuresUri = new Uri(featuresUrl);
                IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                    AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);
                //Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
            }

            //featuresUrl = "http://pandora.slu.se:8080/geoserver/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=slu:RT_XX_G_05&filter=<Filter><And><PropertyIsGreaterThan><PropertyName>Age_mean</PropertyName><Literal>100</Literal></PropertyIsGreaterThan><PropertyIsLessThanOrEqualTo><PropertyName>Age_mean</PropertyName><Literal>110</Literal></PropertyIsLessThanOrEqualTo></And></Filter>";
            ////featuresUrl = "http://pandora.slu.se:8080/geoserver/ows?service=wfs&version=1.1.0&request=GetFeature&typeName=slu:RT_XX_G_05&filter=<Filter><PropertyIsGreaterThanOrEqualTo><PropertyName>Age_mean</PropertyName><Literal>100</Literal></PropertyIsGreaterThanOrEqualTo></Filter>";
            ////featuresUrl = "http://pandora.slu.se:8080/geoserver/ows?service=wfs&version=1.1.0&request=GetFeature&typeName=slu:RT_XX_G_05&filter=<Filter><PropertyIsGreaterThanOrEqualTo><PropertyName>Age_mean</PropertyName><Literal>135</Literal></PropertyIsGreaterThanOrEqualTo></Filter>";

            //Uri featuresUri = new Uri(featuresUrl);
            //IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
            //    AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, gridSpecification, coordinateSystem);
            //Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
        }


        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatistics_WfsServiceWithGoogleMercatorCoordinateSystem_ReturnsListWithFeatureStatisticsOfPoints()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.BoundingBox = new WebBoundingBox { Max = new WebPoint(), Min = new WebPoint() };

            //// Sweden extent i Google Mercator
            gridSpecification.BoundingBox.Max.X = 2726661.6726091;
            gridSpecification.BoundingBox.Max.Y = 10905242.974458;
            gridSpecification.BoundingBox.Min.X = 1149001.4090228;
            gridSpecification.BoundingBox.Min.Y = 7231373.6474714;

            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;
            gridSpecification.GridCellSize = 100000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            // Feature of points 
            featuresUrl = " http://geodata.havochvatten.se/geoservices/hav-miljoovervakning/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=hav-miljoovervakning:miljoovervakningsstationer";

            Uri featuresUri = new Uri(featuresUrl);
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);
            Assert.IsTrue(gridCellFeatureStatistics.Count > 200);
        }


        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void GetGridCellFeatureStatistics_WfsServiceWithGoogleMercatorCoordinateSystem_ReturnsListWithFeatureStatisticsOfPolygons()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.BoundingBox = new WebBoundingBox { Max = new WebPoint(), Min = new WebPoint() };

            //// Sweden extent i Google Mercator
            gridSpecification.BoundingBox.Max.X = 2726661.6726091;
            gridSpecification.BoundingBox.Max.Y = 10905242.974458;
            gridSpecification.BoundingBox.Min.X = 1149001.4090228;
            gridSpecification.BoundingBox.Min.Y = 7231373.6474714;

            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;
            gridSpecification.GridCellSize = 100000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            // Feature of 28 polygons 2013-12-19 
            featuresUrl = "http://geodata.havochvatten.se/geoservices/hav-omradesskydd/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=hav-omradesskydd:helcom-bspa";

            Uri featuresUri = new Uri(featuresUrl);
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);
            Assert.IsTrue(gridCellFeatureStatistics.Count > 20);
        }


        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatistics_GridBoundingBoxSpecified_ReturnsListWithFeatureStatistics()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;
            Stopwatch sp = new Stopwatch();

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();
            gridSpecification.BoundingBox = new WebBoundingBox { Max = new WebPoint(), Min = new WebPoint() };

            // West of Uppsala            
            gridSpecification.BoundingBox.Min.X = 639837.03387354151;
            gridSpecification.BoundingBox.Min.Y = 6631476.1214831518;
            gridSpecification.BoundingBox.Max.X = 647161.17963371589;
            gridSpecification.BoundingBox.Max.Y = 6638050.0581689989;

            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 1000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            featuresUrl = "http://pandora.slu.se:8080/geoserver/ows?service=wfs&version=1.1.0&request=GetFeature&typeName=slu:RT_XX_G_05";
            Uri featuresUri = new Uri(featuresUrl);
            sp.Start();
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);
            sp.Stop();
            System.Console.WriteLine("Elapsed time: {0}ms", sp.ElapsedMilliseconds);
            Assert.IsNotNull(gridCellFeatureStatistics);
            Assert.IsTrue(gridCellFeatureStatistics.Count > 66);
        }

        [TestMethod]
        [Ignore]
        public void GetGridCellFeatureStatistics_WfsLineLayer_ReturnsListWithFeatureStatistics()
        {
            string featuresUrl;
            WebCoordinateSystem coordinateSystem;
            WebGridSpecification gridSpecification;

            gridSpecification = new WebGridSpecification();
            coordinateSystem = new WebCoordinateSystem();

            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 100000; // 100km * 10km
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            featuresUrl = "http://map.smhi.se/geoserver/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SMHI_klimatdata:askdagar_1_iso";
            Uri featuresUri = new Uri(featuresUrl);
            IList<WebGridCellFeatureStatistics> gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, featuresUri, null, gridSpecification, coordinateSystem);
            Assert.IsNotNull(gridCellFeatureStatistics);
            Assert.AreEqual(67, gridCellFeatureStatistics.Count);
        }

        [TestMethod]
        public void GetGridCellFeatureStatistics_WfsFileServiceWithGoogleMercatorCoordinateSystem_ReturnsListWithFeatureStatisticsCountGreaterThanZero()
        {
            var gridSpecification = new WebGridSpecification
            {
                GridCoordinateSystem = GridCoordinateSystem.GoogleMercator,
                GridCellSize = 100000,
                IsGridCellSizeSpecified = true,
                GridCellGeometryType = GridCellGeometryType.Polygon
            };

            var featureCollecationJson =
                "{\r\n  \"crs\": {\r\n    \"type\": \"name\",\r\n    \"properties\": {\r\n      \"name\": \"urn:ogc:def:crs:EPSG::3857\"\r\n    }\r\n  },\r\n  \"bbox\": null,\r\n  \"type\": \"FeatureCollection\",\r\n  \"features\": [\r\n    {\r\n      \"id\": null,\r\n      \"geometry\": {\r\n        \"coordinates\": [\r\n          [\r\n            [\r\n              1963184.08072046,\r\n              8546375.58068736\r\n            ],\r\n            [\r\n              1970386.88663648,\r\n              8542016.23539924\r\n            ],\r\n            [\r\n              1971242.93707021,\r\n              8541497.48720285\r\n            ],\r\n            [\r\n              1971120.43484571,\r\n              8535362.64918818\r\n            ],\r\n            [\r\n              1968419.29740174,\r\n              8526163.86644848\r\n            ],\r\n            [\r\n              1998753.47207178,\r\n              8544533.3284946\r\n            ],\r\n            [\r\n              2006062.62061001,\r\n              8544397.99966915\r\n            ],\r\n            [\r\n              2022911.53633264,\r\n              8505960.1834324\r\n            ],\r\n            [\r\n              2023520.85227737,\r\n              8500201.7355651\r\n            ],\r\n            [\r\n              2031830.84079531,\r\n              8497676.55695483\r\n            ],\r\n            [\r\n              2038165.07095877,\r\n              8489905.3901804\r\n            ],\r\n            [\r\n              2038926.05733798,\r\n              8484710.45109099\r\n            ],\r\n            [\r\n              2043832.16375161,\r\n              8481809.8399258\r\n            ],\r\n            [\r\n              2047541.82980404,\r\n              8479613.67039821\r\n            ],\r\n            [\r\n              2041449.0149635,\r\n              8510062.14240811\r\n            ],\r\n            [\r\n              2043830.90787986,\r\n              8520774.41022408\r\n            ],\r\n            [\r\n              2048206.30403595,\r\n              8521105.15737157\r\n            ],\r\n            [\r\n              2067776.79333744,\r\n              8507276.58004655\r\n            ],\r\n            [\r\n              2076067.01679604,\r\n              8500138.70152406\r\n            ],\r\n            [\r\n              2080295.10194945,\r\n              8486185.2569281\r\n            ],\r\n            [\r\n              2080020.90227329,\r\n              8477447.55395423\r\n            ],\r\n            [\r\n              2073181.86464221,\r\n              8477819.81301474\r\n            ],\r\n            [\r\n              2073148.49023435,\r\n              8472412.67020287\r\n            ],\r\n            [\r\n              2080119.26475403,\r\n              8466222.49729808\r\n            ],\r\n            [\r\n              2086886.49073877,\r\n              8466512.49831905\r\n            ],\r\n            [\r\n              2101103.29834546,\r\n              8469233.45467892\r\n            ],\r\n            [\r\n              2104589.97391594,\r\n              8461901.44142583\r\n            ],\r\n            [\r\n              2091081.72169811,\r\n              8457386.33461245\r\n            ],\r\n            [\r\n              2083699.59448813,\r\n              8454994.42759022\r\n            ],\r\n            [\r\n              2081030.20380576,\r\n              8453761.09673105\r\n            ],\r\n            [\r\n              2079849.80105199,\r\n              8453215.80731433\r\n            ],\r\n            [\r\n              2079291.71806793,\r\n              8451407.53188223\r\n            ],\r\n            [\r\n              2079766.38494202,\r\n              8450130.21311253\r\n            ],\r\n            [\r\n              2080778.24458608,\r\n              8447995.2277172\r\n            ],\r\n            [\r\n              2080914.78454188,\r\n              8447624.75599361\r\n            ],\r\n            [\r\n              2081301.29400708,\r\n              8445941.0121699\r\n            ],\r\n            [\r\n              2081235.74282464,\r\n              8444650.47660749\r\n            ],\r\n            [\r\n              2081229.05532265,\r\n              8444560.84519035\r\n            ],\r\n            [\r\n              2081160.67114673,\r\n              8444535.82557589\r\n            ],\r\n            [\r\n              2080618.54118815,\r\n              8444337.45564716\r\n            ],\r\n            [\r\n              2078639.00831021,\r\n              8443196.88784981\r\n            ],\r\n            [\r\n              2078733.53134978,\r\n              8441807.79642884\r\n            ],\r\n            [\r\n              2079636.33673796,\r\n              8440297.04327362\r\n            ],\r\n            [\r\n              2080408.80952585,\r\n              8439530.21119418\r\n            ],\r\n            [\r\n              2080893.7523102,\r\n              8439048.72246359\r\n            ],\r\n            [\r\n              2080905.56701065,\r\n              8436716.33464746\r\n            ],\r\n            [\r\n              2080286.44520936,\r\n              8436723.51078739\r\n            ],\r\n            [\r\n              2079116.839977,\r\n              8437013.54386913\r\n            ],\r\n            [\r\n              2079107.18455351,\r\n              8435983.06543189\r\n            ],\r\n            [\r\n              2079091.87382473,\r\n              8434348.25068168\r\n            ],\r\n            [\r\n              2076888.73748506,\r\n              8435071.05923527\r\n            ],\r\n            [\r\n              2076637.14034779,\r\n              8435153.55447936\r\n            ],\r\n            [\r\n              2075033.42276578,\r\n              8435679.18551281\r\n            ],\r\n            [\r\n              2072402.62848486,\r\n              8435924.31984529\r\n            ],\r\n            [\r\n              2072156.83520576,\r\n              8435947.17136464\r\n            ],\r\n            [\r\n              2070230.05483669,\r\n              8436126.68565082\r\n            ],\r\n            [\r\n              2069983.6411709,\r\n              8436167.61958982\r\n            ],\r\n            [\r\n              2069808.40715252,\r\n              8436196.26361012\r\n            ],\r\n            [\r\n              2069782.33783888,\r\n              8436200.52485425\r\n            ],\r\n            [\r\n              2069612.85721591,\r\n              8436188.97988475\r\n            ],\r\n            [\r\n              2062391.98683946,\r\n              8435693.46686668\r\n            ],\r\n            [\r\n              2062142.14878778,\r\n              8435676.19529967\r\n            ],\r\n            [\r\n              2061206.46020805,\r\n              8435611.43100331\r\n            ],\r\n            [\r\n              2060992.87930295,\r\n              8435596.63163603\r\n            ],\r\n            [\r\n              2060957.01121623,\r\n              8435487.55365281\r\n            ],\r\n            [\r\n              2060918.9720244,\r\n              8435371.87848677\r\n            ],\r\n            [\r\n              2060735.60510282,\r\n              8434814.21524619\r\n            ],\r\n            [\r\n              2060725.86528708,\r\n              8434784.59335021\r\n            ],\r\n            [\r\n              2060695.09701261,\r\n              8434691.01493886\r\n            ],\r\n            [\r\n              2060689.152724,\r\n              8434672.935522\r\n            ],\r\n            [\r\n              2060639.08122296,\r\n              8434520.64154215\r\n            ],\r\n            [\r\n              2060662.62510648,\r\n              8434439.20447627\r\n            ],\r\n            [\r\n              2060684.93047702,\r\n              8434362.05137336\r\n            ],\r\n            [\r\n              2060703.00677633,\r\n              8434299.52582137\r\n            ],\r\n            [\r\n              2060706.18385735,\r\n              8434196.36642586\r\n            ],\r\n            [\r\n              2060708.37973149,\r\n              8434125.00066443\r\n            ],\r\n            [\r\n              2060709.31816766,\r\n              8434094.52366332\r\n            ],\r\n            [\r\n              2060750.84012968,\r\n              8432745.75061264\r\n            ],\r\n            [\r\n              2060754.63893612,\r\n              8432622.36060122\r\n            ],\r\n            [\r\n              2060755.90184542,\r\n              8432581.30056365\r\n            ],\r\n            [\r\n              2060760.4290672,\r\n              8432434.25973789\r\n            ],\r\n            [\r\n              2060812.3298037,\r\n              8432321.304905\r\n            ],\r\n            [\r\n              2060827.82023394,\r\n              8432298.18651825\r\n            ],\r\n            [\r\n              2060832.28372375,\r\n              8432279.93457853\r\n            ],\r\n            [\r\n              2060832.09167892,\r\n              8432265.48505089\r\n            ],\r\n            [\r\n              2060826.02782387,\r\n              8432254.49385252\r\n            ],\r\n            [\r\n              2060818.9764384,\r\n              8432248.75866467\r\n            ],\r\n            [\r\n              2060816.50212551,\r\n              8432248.25967032\r\n            ],\r\n            [\r\n              2060806.07450006,\r\n              8432246.15740008\r\n            ],\r\n            [\r\n              2060800.78228488,\r\n              8432245.08954462\r\n            ],\r\n            [\r\n              2060763.12450913,\r\n              8432245.83675338\r\n            ],\r\n            [\r\n              2060703.29261874,\r\n              8432263.1479453\r\n            ],\r\n            [\r\n              2060647.88004674,\r\n              8432280.68339603\r\n            ],\r\n            [\r\n              2060623.21052342,\r\n              8432285.30643047\r\n            ],\r\n            [\r\n              2060592.88955262,\r\n              8432288.96294984\r\n            ],\r\n            [\r\n              2060569.49298961,\r\n              8432285.50671466\r\n            ],\r\n            [\r\n              2060557.98918116,\r\n              8432278.75092291\r\n            ],\r\n            [\r\n              2060546.51818591,\r\n              8432272.79704629\r\n            ],\r\n            [\r\n              2060535.81478683,\r\n              8432266.01035613\r\n            ],\r\n            [\r\n              2060535.03777276,\r\n              8432256.80831273\r\n            ],\r\n            [\r\n              2060528.49732974,\r\n              8432234.19093204\r\n            ],\r\n            [\r\n              2060526.62219618,\r\n              8432217.80382017\r\n            ],\r\n            [\r\n              2060518.31153198,\r\n              8432200.88006426\r\n            ],\r\n            [\r\n              2060500.00967122,\r\n              8432184.76806244\r\n            ],\r\n            [\r\n              2060484.241201,\r\n              8432171.76472604\r\n            ],\r\n            [\r\n              2060471.13625324,\r\n              8432165.07247319\r\n            ],\r\n            [\r\n              2060453.61380794,\r\n              8432158.16391429\r\n            ],\r\n            [\r\n              2060440.42037252,\r\n              8432159.10901007\r\n            ],\r\n            [\r\n              2060433.26624774,\r\n              8432159.28583932\r\n            ],\r\n            [\r\n              2060415.58685153,\r\n              8432159.72351465\r\n            ],\r\n            [\r\n              2060392.251316,\r\n              8432167.50872017\r\n            ],\r\n            [\r\n              2060383.6773052,\r\n              8432168.89629032\r\n            ],\r\n            [\r\n              2060371.07005902,\r\n              8432169.1782704\r\n            ],\r\n            [\r\n              2060352.91615176,\r\n              8432156.67677171\r\n            ],\r\n            [\r\n              2060343.29946645,\r\n              8432147.02949943\r\n            ],\r\n            [\r\n              2060332.32942623,\r\n              8432143.46720598\r\n            ],\r\n            [\r\n              2060317.38727989,\r\n              8432143.85871577\r\n            ],\r\n            [\r\n              2060283.40860686,\r\n              8432144.74741552\r\n            ],\r\n            [\r\n              2060278.25953162,\r\n              8432144.88025976\r\n            ],\r\n            [\r\n              2060247.32562848,\r\n              8432143.33871132\r\n            ],\r\n            [\r\n              2060235.77460602,\r\n              8432141.74382461\r\n            ],\r\n            [\r\n              2060227.1649872,\r\n              8432140.5543592\r\n            ],\r\n            [\r\n              2060215.72713858,\r\n              8432135.39987394\r\n            ],\r\n            [\r\n              2060205.04123395,\r\n              8432129.01412979\r\n            ],\r\n            [\r\n              2060189.20736047,\r\n              8432114.40448291\r\n            ],\r\n            [\r\n              2060174.50959574,\r\n              8432098.14569876\r\n            ],\r\n            [\r\n              2060155.12232556,\r\n              8432084.88660298\r\n            ],\r\n            [\r\n              2060136.08086631,\r\n              8432080.04615529\r\n            ],\r\n            [\r\n              2060123.36054866,\r\n              8432072.94014625\r\n            ],\r\n            [\r\n              2060102.60339005,\r\n              8432065.35831551\r\n            ],\r\n            [\r\n              2060089.38444724,\r\n              8432055.86053074\r\n            ],\r\n            [\r\n              2060079.05583336,\r\n              8432038.61976309\r\n            ],\r\n            [\r\n              2060070.16604139,\r\n              8432017.30341243\r\n            ],\r\n            [\r\n              2060065.61180389,\r\n              8431994.20092438\r\n            ],\r\n            [\r\n              2060066.85894339,\r\n              8431975.68256757\r\n            ],\r\n            [\r\n              2060068.85715668,\r\n              8431955.92689884\r\n            ],\r\n            [\r\n              2060073.15139519,\r\n              8431943.30438236\r\n            ],\r\n            [\r\n              2060086.09397877,\r\n              8431916.67581407\r\n            ],\r\n            [\r\n              2060092.0928663,\r\n              8431896.75852061\r\n            ],\r\n            [\r\n              2060100.24280851,\r\n              8431880.36503904\r\n            ],\r\n            [\r\n              2060103.83425801,\r\n              8431870.17944339\r\n            ],\r\n            [\r\n              2060111.82961968,\r\n              8431859.81540295\r\n            ],\r\n            [\r\n              2060117.05682444,\r\n              8431850.36573744\r\n            ],\r\n            [\r\n              2060117.88679778,\r\n              8431831.46328318\r\n            ],\r\n            [\r\n              2060113.17837227,\r\n              8431814.392259\r\n            ],\r\n            [\r\n              2060106.76461814,\r\n              8431804.61392476\r\n            ],\r\n            [\r\n              2060098.2606509,\r\n              8431802.55687433\r\n            ],\r\n            [\r\n              2060081.19752003,\r\n              8431806.86832396\r\n            ],\r\n            [\r\n              2060043.512985,\r\n              8431811.3175202\r\n            ],\r\n            [\r\n              2060023.17182939,\r\n              8431794.85702491\r\n            ],\r\n            [\r\n              2060004.84594991,\r\n              8431788.31380806\r\n            ],\r\n            [\r\n              2059965.02704116,\r\n              8431795.14171972\r\n            ],\r\n            [\r\n              2059929.58062085,\r\n              8431800.98642144\r\n            ],\r\n            [\r\n              2059911.29553445,\r\n              8431804.93630411\r\n            ],\r\n            [\r\n              2059874.69708135,\r\n              8431812.03342141\r\n            ],\r\n            [\r\n              2059862.43130885,\r\n              8431816.1398228\r\n            ],\r\n            [\r\n              2059853.08321937,\r\n              8431812.90171365\r\n            ],\r\n            [\r\n              2059820.26327,\r\n              8431794.1357272\r\n            ],\r\n            [\r\n              2059785.13757356,\r\n              8431777.87365551\r\n            ],\r\n            [\r\n              2059744.55267284,\r\n              8431755.40096839\r\n            ],\r\n            [\r\n              2059728.14226236,\r\n              8431746.01965446\r\n            ],\r\n            [\r\n              2059715.28739504,\r\n              8431735.29051988\r\n            ],\r\n            [\r\n              2059709.81331188,\r\n              8431728.68077001\r\n            ],\r\n            [\r\n              2059692.55671205,\r\n              8431708.0830623\r\n            ],\r\n            [\r\n              2059681.8949311,\r\n              8431692.0426795\r\n            ],\r\n            [\r\n              2059671.92101093,\r\n              8431673.16062006\r\n            ],\r\n            [\r\n              2059658.10777192,\r\n              8431648.40778015\r\n            ],\r\n            [\r\n              2059647.20528745,\r\n              8431636.39166909\r\n            ],\r\n            [\r\n              2059637.51967788,\r\n              8431624.72942665\r\n            ],\r\n            [\r\n              2059623.92990238,\r\n              8431605.59385573\r\n            ],\r\n            [\r\n              2059615.76595168,\r\n              8431591.86133186\r\n            ],\r\n            [\r\n              2059607.61882305,\r\n              8431578.52965895\r\n            ],\r\n            [\r\n              2059604.03186161,\r\n              8431569.03365585\r\n            ],\r\n            [\r\n              2059603.17960721,\r\n              8431561.15497011\r\n            ],\r\n            [\r\n              2059602.13009562,\r\n              8431551.43637035\r\n            ],\r\n            [\r\n              2059602.21233289,\r\n              8431523.31230022\r\n            ],\r\n            [\r\n              2059603.35403492,\r\n              8431471.44843575\r\n            ],\r\n            [\r\n              2059600.13798838,\r\n              8431441.04972284\r\n            ],\r\n            [\r\n              2059597.35452821,\r\n              8431431.5187689\r\n            ],\r\n            [\r\n              2059588.29445302,\r\n              8431415.41547581\r\n            ],\r\n            [\r\n              2059582.88584492,\r\n              8431400.36575534\r\n            ],\r\n            [\r\n              2059581.42917096,\r\n              8431383.95565297\r\n            ],\r\n            [\r\n              2059577.76640592,\r\n              8431362.41088904\r\n            ],\r\n            [\r\n              2059572.21292179,\r\n              8431343.75708729\r\n            ],\r\n            [\r\n              2059563.82447863,\r\n              8431324.40733409\r\n            ],\r\n            [\r\n              2059556.99475582,\r\n              8431309.10287475\r\n            ],\r\n            [\r\n              2059555.90232346,\r\n              8431306.65137811\r\n            ],\r\n            [\r\n              2059557.42562148,\r\n              8431294.54132589\r\n            ],\r\n            [\r\n              2059559.1072782,\r\n              8431276.39680121\r\n            ],\r\n            [\r\n              2059565.90029052,\r\n              8431225.51156994\r\n            ],\r\n            [\r\n              2059566.43240302,\r\n              8431198.57726755\r\n            ],\r\n            [\r\n              2059565.29752033,\r\n              8431170.10296834\r\n            ],\r\n            [\r\n              2059560.64298717,\r\n              8431154.75337364\r\n            ],\r\n            [\r\n              2059559.76089618,\r\n              8431151.84773889\r\n            ],\r\n            [\r\n              2059553.42309363,\r\n              8431133.62576505\r\n            ],\r\n            [\r\n              2059547.79009747,\r\n              8431112.96376899\r\n            ],\r\n            [\r\n              2059532.40960005,\r\n              8431079.03597651\r\n            ],\r\n            [\r\n              2059513.31242433,\r\n              8431052.48895124\r\n            ],\r\n            [\r\n              2059495.01827118,\r\n              8431025.91322258\r\n            ],\r\n            [\r\n              2059483.23724,\r\n              8431011.92562178\r\n            ],\r\n            [\r\n              2059468.92707582,\r\n              8430994.82512186\r\n            ],\r\n            [\r\n              2059458.93918648,\r\n              8430985.58769758\r\n            ],\r\n            [\r\n              2059448.64658185,\r\n              8430978.77150924\r\n            ],\r\n            [\r\n              2059432.68737273,\r\n              8430970.57539243\r\n            ],\r\n            [\r\n              2059413.86291149,\r\n              8430960.88753843\r\n            ],\r\n            [\r\n              2059368.49696964,\r\n              8430939.0124557\r\n            ],\r\n            [\r\n              2059360.4506134,\r\n              8430897.96274997\r\n            ],\r\n            [\r\n              2059320.15808197,\r\n              8430892.75010483\r\n            ],\r\n            [\r\n              2059291.48876715,\r\n              8430887.47821782\r\n            ],\r\n            [\r\n              2059250.34857794,\r\n              8430881.09507841\r\n            ],\r\n            [\r\n              2059193.05578849,\r\n              8430871.74539749\r\n            ],\r\n            [\r\n              2059166.73932423,\r\n              8430865.17220016\r\n            ],\r\n            [\r\n              2059134.05195195,\r\n              8430859.6541747\r\n            ],\r\n            [\r\n              2059104.9009269,\r\n              8430852.38735875\r\n            ],\r\n            [\r\n              2059090.57910887,\r\n              8430850.3259992\r\n            ],\r\n            [\r\n              2059046.04027267,\r\n              8430843.90762977\r\n            ],\r\n            [\r\n              2058997.21627142,\r\n              8430835.82333621\r\n            ],\r\n            [\r\n              2058953.96355221,\r\n              8430826.7141538\r\n            ],\r\n            [\r\n              2058866.0340429,\r\n              8430812.96792072\r\n            ],\r\n            [\r\n              2058795.82429263,\r\n              8430797.97629148\r\n            ],\r\n            [\r\n              2058677.7166666,\r\n              8430582.73586972\r\n            ],\r\n            [\r\n              2058663.72084024,\r\n              8430462.39820209\r\n            ],\r\n            [\r\n              2058674.1487542,\r\n              8430425.27365277\r\n            ],\r\n            [\r\n              2058771.53317944,\r\n              8430078.53627708\r\n            ],\r\n            [\r\n              2058843.08970333,\r\n              8429801.36382349\r\n            ],\r\n            [\r\n              2058888.29168214,\r\n              8429641.92089119\r\n            ],\r\n            [\r\n              2058943.62574137,\r\n              8429446.73819809\r\n            ],\r\n            [\r\n              2058964.00957611,\r\n              8429368.73324118\r\n            ],\r\n            [\r\n              2058970.34302004,\r\n              8429344.49525635\r\n            ],\r\n            [\r\n              2058976.41436683,\r\n              8429321.25832647\r\n            ],\r\n            [\r\n              2058989.54548364,\r\n              8429271.01055251\r\n            ],\r\n            [\r\n              2058988.13062403,\r\n              8429220.11541845\r\n            ],\r\n            [\r\n              2058981.24332699,\r\n              8428972.24194447\r\n            ],\r\n            [\r\n              2058981.0859552,\r\n              8428966.6053627\r\n            ],\r\n            [\r\n              2058976.18418474,\r\n              8428790.10893139\r\n            ],\r\n            [\r\n              2058971.8357586,\r\n              8428633.63603272\r\n            ],\r\n            [\r\n              2059004.07361853,\r\n              8428451.07294517\r\n            ],\r\n            [\r\n              2059057.1726554,\r\n              8428136.30360309\r\n            ],\r\n            [\r\n              2059009.55327382,\r\n              8427795.60905187\r\n            ],\r\n            [\r\n              2059007.91145438,\r\n              8427783.86073716\r\n            ],\r\n            [\r\n              2058949.17551907,\r\n              8427449.36762134\r\n            ],\r\n            [\r\n              2058925.44913177,\r\n              8427188.02326574\r\n            ],\r\n            [\r\n              2058885.26007483,\r\n              8426795.60725741\r\n            ],\r\n            [\r\n              2058863.90065378,\r\n              8426587.02192714\r\n            ],\r\n            [\r\n              2058856.05045238,\r\n              8426508.39105515\r\n            ],\r\n            [\r\n              2058547.98156319,\r\n              8426378.23382464\r\n            ],\r\n            [\r\n              2058619.26241659,\r\n              8425992.9475866\r\n            ],\r\n            [\r\n              2058801.05066113,\r\n              8425975.28315325\r\n            ],\r\n            [\r\n              2058763.66115836,\r\n              8425553.870208\r\n            ],\r\n            [\r\n              2058696.99083216,\r\n              8424802.36879877\r\n            ],\r\n            [\r\n              2058642.7406335,\r\n              8424190.79404954\r\n            ],\r\n            [\r\n              2058640.40932459,\r\n              8424164.50955186\r\n            ],\r\n            [\r\n              2058628.701824,\r\n              8424032.52062748\r\n            ],\r\n            [\r\n              2058621.05114763,\r\n              8423946.26990138\r\n            ],\r\n            [\r\n              2058595.97216531,\r\n              8423663.5069599\r\n            ],\r\n            [\r\n              2058529.84374285,\r\n              8422917.88732343\r\n            ],\r\n            [\r\n              2057908.90243671,\r\n              8422127.72961446\r\n            ],\r\n            [\r\n              2057367.68162836,\r\n              8421164.71336905\r\n            ],\r\n            [\r\n              2056817.78598931,\r\n              8420445.42322537\r\n            ],\r\n            [\r\n              2056707.15782721,\r\n              8420300.70502755\r\n            ],\r\n            [\r\n              2056704.68475948,\r\n              8420296.01524665\r\n            ],\r\n            [\r\n              2056051.64079415,\r\n              8419057.69743932\r\n            ],\r\n            [\r\n              2056048.95838742,\r\n              8419052.60968789\r\n            ],\r\n            [\r\n              2056000.70168778,\r\n              8418970.82555523\r\n            ],\r\n            [\r\n              2055912.93503773,\r\n              8418822.07895601\r\n            ],\r\n            [\r\n              2055767.81032915,\r\n              8418576.10594638\r\n            ],\r\n            [\r\n              2055283.91794657,\r\n              8417755.88021531\r\n            ],\r\n            [\r\n              2054714.38740789,\r\n              8417349.77130825\r\n            ],\r\n            [\r\n              2054205.1400833,\r\n              8416986.59293684\r\n            ],\r\n            [\r\n              2054058.1570532,\r\n              8416963.85283594\r\n            ],\r\n            [\r\n              2053930.81160407,\r\n              8416944.14626124\r\n            ],\r\n            [\r\n              2053918.21052306,\r\n              8416865.73406253\r\n            ],\r\n            [\r\n              2053582.53781609,\r\n              8414776.65888954\r\n            ],\r\n            [\r\n              2053264.73119466,\r\n              8412798.15769451\r\n            ],\r\n            [\r\n              2051756.34087008,\r\n              8412311.63037954\r\n            ],\r\n            [\r\n              2049681.28652886,\r\n              8410302.15463675\r\n            ],\r\n            [\r\n              2049140.70730261,\r\n              8409986.82380414\r\n            ],\r\n            [\r\n              2047389.83990936,\r\n              8409039.69540415\r\n            ],\r\n            [\r\n              2047095.07436609,\r\n              8408956.74496228\r\n            ],\r\n            [\r\n              2047001.06497417,\r\n              8408925.31686509\r\n            ],\r\n            [\r\n              2047000.83593756,\r\n              8408909.71350537\r\n            ],\r\n            [\r\n              2047001.8996948,\r\n              8408896.45805292\r\n            ],\r\n            [\r\n              2047005.1541541,\r\n              8408887.92315402\r\n            ],\r\n            [\r\n              2047011.85337865,\r\n              8408875.64000359\r\n            ],\r\n            [\r\n              2047023.37448334,\r\n              8408863.96460807\r\n            ],\r\n            [\r\n              2047030.68355875,\r\n              8408856.86428145\r\n            ],\r\n            [\r\n              2047036.66715315,\r\n              8408846.61714179\r\n            ],\r\n            [\r\n              2047042.69676011,\r\n              8408837.56525429\r\n            ],\r\n            [\r\n              2047046.69589309,\r\n              8408817.78906849\r\n            ],\r\n            [\r\n              2047048.04944499,\r\n              8408801.7217601\r\n            ],\r\n            [\r\n              2047050.71512699,\r\n              8408768.78788557\r\n            ],\r\n            [\r\n              2047051.89613126,\r\n              8408758.33265003\r\n            ],\r\n            [\r\n              2047055.18413072,\r\n              8408749.01967688\r\n            ],\r\n            [\r\n              2047061.62409396,\r\n              8408741.92575041\r\n            ],\r\n            [\r\n              2047071.83911661,\r\n              8408737.51027175\r\n            ],\r\n            [\r\n              2047081.33639886,\r\n              8408735.12465063\r\n            ],\r\n            [\r\n              2047091.31468607,\r\n              8408734.7216552\r\n            ],\r\n            [\r\n              2047122.74441255,\r\n              8408731.04916218\r\n            ],\r\n            [\r\n              2047134.19248288,\r\n              8408727.38223571\r\n            ],\r\n            [\r\n              2047141.5792368,\r\n              8408722.27854163\r\n            ],\r\n            [\r\n              2047163.63236471,\r\n              8408704.1740086\r\n            ],\r\n            [\r\n              2047193.64804008,\r\n              8408675.33998035\r\n            ],\r\n            [\r\n              2047213.2755114,\r\n              8408656.5294348\r\n            ],\r\n            [\r\n              2047231.35581462,\r\n              8408638.98628188\r\n            ],\r\n            [\r\n              2047240.43292703,\r\n              8408626.21202834\r\n            ],\r\n            [\r\n              2047251.50555185,\r\n              8408613.35359472\r\n            ],\r\n            [\r\n              2047258.98692218,\r\n              8408600.63991292\r\n            ],\r\n            [\r\n              2047265.07263689,\r\n              8408588.21453934\r\n            ],\r\n            [\r\n              2047268.5893503,\r\n              8408581.03880299\r\n            ],\r\n            [\r\n              2047278.04660912,\r\n              8408557.83646093\r\n            ],\r\n            [\r\n              2047284.22345896,\r\n              8408543.54027253\r\n            ],\r\n            [\r\n              2047287.55116636,\r\n              8408535.83549317\r\n            ],\r\n            [\r\n              2047294.7625034,\r\n              8408526.33932208\r\n            ],\r\n            [\r\n              2047301.93346316,\r\n              8408519.15006458\r\n            ],\r\n            [\r\n              2047304.75096814,\r\n              8408516.32536659\r\n            ],\r\n            [\r\n              2047309.2936041,\r\n              8408513.67331935\r\n            ],\r\n            [\r\n              2047323.8857696,\r\n              8408505.14290554\r\n            ],\r\n            [\r\n              2047336.74779583,\r\n              8408497.02045654\r\n            ],\r\n            [\r\n              2047344.37554955,\r\n              8408487.90415364\r\n            ],\r\n            [\r\n              2047360.69148108,\r\n              8408456.42011411\r\n            ],\r\n            [\r\n              2047372.41586107,\r\n              8408429.92705519\r\n            ],\r\n            [\r\n              2047381.0223553,\r\n              8408405.55914755\r\n            ],\r\n            [\r\n              2047394.24821078,\r\n              8408386.61340189\r\n            ],\r\n            [\r\n              2047401.37706991,\r\n              8408375.11691048\r\n            ],\r\n            [\r\n              2047415.53013837,\r\n              8408359.33319983\r\n            ],\r\n            [\r\n              2047428.16680317,\r\n              8408345.60864166\r\n            ],\r\n            [\r\n              2047440.83504771,\r\n              8408332.68980562\r\n            ],\r\n            [\r\n              2047457.10647026,\r\n              8408310.01354203\r\n            ],\r\n            [\r\n              2047463.16029157,\r\n              8408302.20612\r\n            ],\r\n            [\r\n              2047467.61158108,\r\n              8408296.4631747\r\n            ],\r\n            [\r\n              2047473.44456672,\r\n              8408288.93866654\r\n            ],\r\n            [\r\n              2047475.17993884,\r\n              8408282.46318343\r\n            ],\r\n            [\r\n              2047474.61355348,\r\n              8408280.29738962\r\n            ],\r\n            [\r\n              2047466.84163763,\r\n              8408250.57150141\r\n            ],\r\n            [\r\n              2047464.30656364,\r\n              8408240.87445992\r\n            ],\r\n            [\r\n              2047456.04242542,\r\n              8408214.38661256\r\n            ],\r\n            [\r\n              2047453.51599795,\r\n              8408201.28313557\r\n            ],\r\n            [\r\n              2047449.21379477,\r\n              8408183.84309678\r\n            ],\r\n            [\r\n              2047447.33897762,\r\n              8408167.10932\r\n            ],\r\n            [\r\n              2047445.68875209,\r\n              8408155.96657028\r\n            ],\r\n            [\r\n              2047445.71678799,\r\n              8408146.76042063\r\n            ],\r\n            [\r\n              2047450.15183888,\r\n              8408137.77303608\r\n            ],\r\n            [\r\n              2047454.69974513,\r\n              8408131.58616792\r\n            ],\r\n            [\r\n              2047470.54707415,\r\n              8408128.14059647\r\n            ],\r\n            [\r\n              2047499.00288171,\r\n              8408120.18527558\r\n            ],\r\n            [\r\n              2047523.42187135,\r\n              8408111.19180394\r\n            ],\r\n            [\r\n              2047545.33332017,\r\n              8408099.49557715\r\n            ],\r\n            [\r\n              2047565.36086219,\r\n              8408090.68035379\r\n            ],\r\n            [\r\n              2047581.32846481,\r\n              8408079.56979549\r\n            ],\r\n            [\r\n              2047586.74435764,\r\n              8408075.80230275\r\n            ],\r\n            [\r\n              2047600.01580599,\r\n              8408058.05198425\r\n            ],\r\n            [\r\n              2047611.9029411,\r\n              8408045.56419626\r\n            ],\r\n            [\r\n              2047634.60645221,\r\n              8408023.8289677\r\n            ],\r\n            [\r\n              2047637.17822176,\r\n              8408018.71631327\r\n            ],\r\n            [\r\n              2047642.0065074,\r\n              8408009.12170244\r\n            ],\r\n            [\r\n              2047649.98299417,\r\n              8407997.40986253\r\n            ],\r\n            [\r\n              2047652.14523103,\r\n              8407993.67982766\r\n            ],\r\n            [\r\n              2047660.32659407,\r\n              8407967.94943402\r\n            ],\r\n            [\r\n              2047669.95854268,\r\n              8407949.1487134\r\n            ],\r\n            [\r\n              2047677.33048996,\r\n              8407943.64579817\r\n            ],\r\n            [\r\n              2047694.17933931,\r\n              8407934.75626018\r\n            ],\r\n            [\r\n              2047701.98566896,\r\n              8407930.64074981\r\n            ],\r\n            [\r\n              2047719.97068152,\r\n              8407920.70430944\r\n            ],\r\n            [\r\n              2047737.01040788,\r\n              8407907.20370057\r\n            ],\r\n            [\r\n              2047745.46610663,\r\n              8407898.85743797\r\n            ],\r\n            [\r\n              2047752.35694248,\r\n              8407891.37151208\r\n            ],\r\n            [\r\n              2047756.87167526,\r\n              8407884.38521566\r\n            ],\r\n            [\r\n              2047761.33686449,\r\n              8407876.19765343\r\n            ],\r\n            [\r\n              2047761.36471842,\r\n              8407866.99335292\r\n            ],\r\n            [\r\n              2047753.00725127,\r\n              8407848.11885165\r\n            ],\r\n            [\r\n              2047741.08656175,\r\n              8407820.18466808\r\n            ],\r\n            [\r\n              2047729.34403591,\r\n              8407796.64292374\r\n            ],\r\n            [\r\n              2047720.65241832,\r\n              8407779.38283288\r\n            ],\r\n            [\r\n              2047713.58825555,\r\n              8407762.85934571\r\n            ],\r\n            [\r\n              2047711.52730108,\r\n              8407751.33511871\r\n            ],\r\n            [\r\n              2047713.0824228,\r\n              8407740.46629821\r\n            ],\r\n            [\r\n              2047717.58092714,\r\n              8407733.07803471\r\n            ],\r\n            [\r\n              2047727.2667585,\r\n              8407725.48098057\r\n            ],\r\n            [\r\n              2047741.5348957,\r\n              8407722.50332885\r\n            ],\r\n            [\r\n              2047759.47389844,\r\n              8407721.37512917\r\n            ],\r\n            [\r\n              2047796.58307277,\r\n              8407719.87070059\r\n            ],\r\n            [\r\n              2047817.28393285,\r\n              8407717.83114168\r\n            ],\r\n            [\r\n              2047830.02052066,\r\n              8407716.51502108\r\n            ],\r\n            [\r\n              2047858.55663988,\r\n              8407710.55493687\r\n            ],\r\n            [\r\n              2047893.26702856,\r\n              8407699.13880248\r\n            ],\r\n            [\r\n              2047922.29382795,\r\n              8407685.55390492\r\n            ],\r\n            [\r\n              2047928.35538933,\r\n              8407677.30588308\r\n            ],\r\n            [\r\n              2047943.22181398,\r\n              8407659.49128023\r\n            ],\r\n            [\r\n              2047965.17684282,\r\n              8407625.32856224\r\n            ],\r\n            [\r\n              2047987.57943479,\r\n              8407600.05713159\r\n            ],\r\n            [\r\n              2048001.32868181,\r\n              8407593.37068443\r\n            ],\r\n            [\r\n              2048009.13829252,\r\n              8407589.57517762\r\n            ],\r\n            [\r\n              2048025.89220995,\r\n              8407578.88997074\r\n            ],\r\n            [\r\n              2048036.50455216,\r\n              8407574.45818219\r\n            ],\r\n            [\r\n              2048043.00834064,\r\n              8407570.63808135\r\n            ],\r\n            [\r\n              2048048.21591259,\r\n              8407567.57914039\r\n            ],\r\n            [\r\n              2048055.13901278,\r\n              8407560.89479359\r\n            ],\r\n            [\r\n              2048060.43326366,\r\n              8407553.47568204\r\n            ],\r\n            [\r\n              2048062.12903593,\r\n              8407545.8991687\r\n            ],\r\n            [\r\n              2048063.14195397,\r\n              8407541.35618198\r\n            ],\r\n            [\r\n              2048066.51723458,\r\n              8407526.0107235\r\n            ],\r\n            [\r\n              2048067.55531922,\r\n              8407492.34767785\r\n            ],\r\n            [\r\n              2048068.72867964,\r\n              8407481.89388968\r\n            ],\r\n            [\r\n              2048069.10580821,\r\n              8407471.47380175\r\n            ],\r\n            [\r\n              2048078.70862982,\r\n              8407461.87806103\r\n            ],\r\n            [\r\n              2048097.36047496,\r\n              8407448.714275\r\n            ],\r\n            [\r\n              2048115.40822112,\r\n              8407440.37352729\r\n            ],\r\n            [\r\n              2048131.22261166,\r\n              8407436.13192341\r\n            ],\r\n            [\r\n              2048138.46908318,\r\n              8407437.43804977\r\n            ],\r\n            [\r\n              2048159.77969436,\r\n              8407440.57766297\r\n            ],\r\n            [\r\n              2048185.09915917,\r\n              8407444.71661797\r\n            ],\r\n            [\r\n              2048191.16041124,\r\n              8407445.70715315\r\n            ],\r\n            [\r\n              2048204.24629307,\r\n              8407443.17565311\r\n            ],\r\n            [\r\n              2048210.53413572,\r\n              8407440.5183336\r\n            ],\r\n            [\r\n              2048226.66737309,\r\n              8407434.26060835\r\n            ],\r\n            [\r\n              2048240.03848038,\r\n              8407428.9157213\r\n            ],\r\n            [\r\n              2048254.64043617,\r\n              8407424.31860692\r\n            ],\r\n            [\r\n              2048274.64014786,\r\n              8407424.70940152\r\n            ],\r\n            [\r\n              2048297.83215438,\r\n              8407424.96636567\r\n            ],\r\n            [\r\n              2048339.96953268,\r\n              8407429.25978806\r\n            ],\r\n            [\r\n              2048361.93112337,\r\n              8407428.76875314\r\n            ],\r\n            [\r\n              2048377.39542505,\r\n              8407425.73871391\r\n            ],\r\n            [\r\n              2048388.47296741,\r\n              8407422.88988484\r\n            ],\r\n            [\r\n              2048395.61658529,\r\n              8407419.63872674\r\n            ],\r\n            [\r\n              2048401.02848813,\r\n              8407417.17613664\r\n            ],\r\n            [\r\n              2048407.28901287,\r\n              8407412.1987847\r\n            ],\r\n            [\r\n              2048415.35785286,\r\n              8407405.78477638\r\n            ],\r\n            [\r\n              2048430.06419801,\r\n              8407393.9804915\r\n            ],\r\n            [\r\n              2048447.83511698,\r\n              8407378.85125661\r\n            ],\r\n            [\r\n              2048463.97923554,\r\n              8407362.98742224\r\n            ],\r\n            [\r\n              2048480.07155457,\r\n              8407345.9229435\r\n            ],\r\n            [\r\n              2048499.70355743,\r\n              8407317.51135495\r\n            ],\r\n            [\r\n              2048513.27473525,\r\n              8407297.3454844\r\n            ],\r\n            [\r\n              2048526.81256209,\r\n              8407276.38464383\r\n            ],\r\n            [\r\n              2048537.24991464,\r\n              8407247.94470875\r\n            ],\r\n            [\r\n              2048540.53199209,\r\n              8407241.40493534\r\n            ],\r\n            [\r\n              2048551.56248631,\r\n              8407219.43019707\r\n            ],\r\n            [\r\n              2048559.80745131,\r\n              8407203.00302299\r\n            ],\r\n            [\r\n              2048586.61581929,\r\n              8407167.72675419\r\n            ],\r\n            [\r\n              2048653.45777532,\r\n              8407049.38222302\r\n            ],\r\n            [\r\n              2048692.55035264,\r\n              8406991.68852096\r\n            ],\r\n            [\r\n              2048757.87394288,\r\n              8406930.86839864\r\n            ],\r\n            [\r\n              2048791.96816241,\r\n              8406896.59488638\r\n            ],\r\n            [\r\n              2048819.25043481,\r\n              8406880.61875779\r\n            ],\r\n            [\r\n              2048828.33926524,\r\n              8406877.06489674\r\n            ],\r\n            [\r\n              2048849.11212109,\r\n              8406868.9337505\r\n            ],\r\n            [\r\n              2048908.30390384,\r\n              8406873.04057421\r\n            ],\r\n            [\r\n              2048916.82253726,\r\n              8406873.63127019\r\n            ],\r\n            [\r\n              2048946.85259649,\r\n              8406865.93998658\r\n            ],\r\n            [\r\n              2048972.20121119,\r\n              8406842.04693233\r\n            ],\r\n            [\r\n              2048994.51783323,\r\n              8406793.87128593\r\n            ],\r\n            [\r\n              2049012.13566844,\r\n              8406716.41699379\r\n            ],\r\n            [\r\n              2049044.64306236,\r\n              8406639.80974357\r\n            ],\r\n            [\r\n              2049061.56990056,\r\n              8406601.5434802\r\n            ],\r\n            [\r\n              2049118.53701732,\r\n              8406512.93225367\r\n            ],\r\n            [\r\n              2049271.8658087,\r\n              8406352.63799078\r\n            ],\r\n            [\r\n              2049797.61980636,\r\n              8405868.87981728\r\n            ],\r\n            [\r\n              2049898.96955206,\r\n              8405802.61096568\r\n            ],\r\n            [\r\n              2049960.12234016,\r\n              8405760.82410732\r\n            ],\r\n            [\r\n              2049989.3145903,\r\n              8405740.87632064\r\n            ],\r\n            [\r\n              2049998.97091418,\r\n              8405734.2757803\r\n            ],\r\n            [\r\n              2050015.38692727,\r\n              8405723.0629\r\n            ],\r\n            [\r\n              2050029.79881267,\r\n              8405713.21052136\r\n            ],\r\n            [\r\n              2050182.60104675,\r\n              8405435.26700275\r\n            ],\r\n            [\r\n              2050254.13079253,\r\n              8405372.92569123\r\n            ],\r\n            [\r\n              2051071.67395822,\r\n              8404833.96051435\r\n            ],\r\n            [\r\n              2051807.61275417,\r\n              8404348.68368381\r\n            ],\r\n            [\r\n              2052333.24122021,\r\n              8403900.88683364\r\n            ],\r\n            [\r\n              2052858.05410177,\r\n              8403807.23443901\r\n            ],\r\n            [\r\n              2053202.1544536,\r\n              8403667.64203822\r\n            ],\r\n            [\r\n              2053674.03559046,\r\n              8403248.24712769\r\n            ],\r\n            [\r\n              2053683.74352518,\r\n              8402702.92133632\r\n            ],\r\n            [\r\n              2053725.33889578,\r\n              8402404.46381306\r\n            ],\r\n            [\r\n              2053870.05967634,\r\n              8402263.20129973\r\n            ],\r\n            [\r\n              2054385.33565991,\r\n              8401927.07935744\r\n            ],\r\n            [\r\n              2054772.53008205,\r\n              8401830.99492174\r\n            ],\r\n            [\r\n              2054961.08194567,\r\n              8401853.7467262\r\n            ],\r\n            [\r\n              2055184.69235428,\r\n              8401689.1237835\r\n            ],\r\n            [\r\n              2055245.47562788,\r\n              8401526.76928815\r\n            ],\r\n            [\r\n              2055371.12602303,\r\n              8401454.52933984\r\n            ],\r\n            [\r\n              2055376.41260346,\r\n              8401451.48841904\r\n            ],\r\n            [\r\n              2055484.10994136,\r\n              8401389.56747828\r\n            ],\r\n            [\r\n              2055548.79212211,\r\n              8401394.69681165\r\n            ],\r\n            [\r\n              2055600.508098,\r\n              8401398.80257598\r\n            ],\r\n            [\r\n              2055670.74649764,\r\n              8401404.37315502\r\n            ],\r\n            [\r\n              2055839.02137561,\r\n              8401337.07944071\r\n            ],\r\n            [\r\n              2056196.96060705,\r\n              8400757.24169587\r\n            ],\r\n            [\r\n              2056444.8897052,\r\n              8400694.74305614\r\n            ],\r\n            [\r\n              2056442.49597572,\r\n              8400688.65479557\r\n            ],\r\n            [\r\n              2056385.08860447,\r\n              8400478.84765083\r\n            ],\r\n            [\r\n              2056381.27573876,\r\n              8400245.45902203\r\n            ],\r\n            [\r\n              2056247.9711137,\r\n              8399784.3750723\r\n            ],\r\n            [\r\n              2056201.20756848,\r\n              8399622.61887918\r\n            ],\r\n            [\r\n              2056003.48251351,\r\n              8399368.24033715\r\n            ],\r\n            [\r\n              2055438.25146104,\r\n              8398640.98800275\r\n            ],\r\n            [\r\n              2055196.76969961,\r\n              8398332.56426307\r\n            ],\r\n            [\r\n              2054918.58007888,\r\n              8398325.36347374\r\n            ],\r\n            [\r\n              2054821.17162122,\r\n              8398620.79080089\r\n            ],\r\n            [\r\n              2053981.52471048,\r\n              8398444.85917759\r\n            ],\r\n            [\r\n              2053901.32055207,\r\n              8398202.67455971\r\n            ],\r\n            [\r\n              2053665.19000389,\r\n              8397937.19034044\r\n            ],\r\n            [\r\n              2053694.7198027,\r\n              8397864.11073962\r\n            ],\r\n            [\r\n              2053726.02588611,\r\n              8397858.51546467\r\n            ],\r\n            [\r\n              2053653.03046342,\r\n              8397754.5742337\r\n            ],\r\n            [\r\n              2053618.35909321,\r\n              8397724.31606851\r\n            ],\r\n            [\r\n              2053569.64662646,\r\n              8397639.83203333\r\n            ],\r\n            [\r\n              2053481.61691792,\r\n              8397560.05020554\r\n            ],\r\n            [\r\n              2053405.95429502,\r\n              8397480.19046271\r\n            ],\r\n            [\r\n              2053288.90964379,\r\n              8397570.5982543\r\n            ],\r\n            [\r\n              2052720.45755098,\r\n              8397003.92840709\r\n            ],\r\n            [\r\n              2052719.80788821,\r\n              8396994.7067661\r\n            ],\r\n            [\r\n              2052719.9343338,\r\n              8396988.30603325\r\n            ],\r\n            [\r\n              2052718.8139932,\r\n              8396980.7598986\r\n            ],\r\n            [\r\n              2052713.47428055,\r\n              8396967.79803591\r\n            ],\r\n            [\r\n              2052712.53205383,\r\n              8396955.04836161\r\n            ],\r\n            [\r\n              2052714.46419817,\r\n              8396944.17453715\r\n            ],\r\n            [\r\n              2052715.99785841,\r\n              8396933.31662622\r\n            ],\r\n            [\r\n              2052717.65046871,\r\n              8396925.25412071\r\n            ],\r\n            [\r\n              2052717.34499329,\r\n              8396918.07351921\r\n            ],\r\n            [\r\n              2052712.4531341,\r\n              8396906.2911415\r\n            ],\r\n            [\r\n              2052703.03663948,\r\n              8396900.69698878\r\n            ],\r\n            [\r\n              2052689.53324018,\r\n              8396892.87739937\r\n            ],\r\n            [\r\n              2052616.53028769,\r\n              8396856.01329249\r\n            ],\r\n            [\r\n              2052600.78156241,\r\n              8396842.29337345\r\n            ],\r\n            [\r\n              2052594.5520181,\r\n              8396836.56181569\r\n            ],\r\n            [\r\n              2052587.67864809,\r\n              8396834.4576926\r\n            ],\r\n            [\r\n              2052580.26174282,\r\n              8396838.36875608\r\n            ],\r\n            [\r\n              2052570.5087872,\r\n              8396843.58041994\r\n            ],\r\n            [\r\n              2052553.18467922,\r\n              8396849.10975364\r\n            ],\r\n            [\r\n              2051585.60455123,\r\n              8395895.03725345\r\n            ],\r\n            [\r\n              2051573.44152693,\r\n              8395898.36648078\r\n            ],\r\n            [\r\n              2051465.42745469,\r\n              8395788.25025655\r\n            ],\r\n            [\r\n              2051461.45853652,\r\n              8395741.66366418\r\n            ],\r\n            [\r\n              2051308.44061019,\r\n              8395699.39044967\r\n            ],\r\n            [\r\n              2051303.44429083,\r\n              8395659.68309102\r\n            ],\r\n            [\r\n              2051306.82088338,\r\n              8395628.75558101\r\n            ],\r\n            [\r\n              2051317.35410401,\r\n              8395597.1028737\r\n            ],\r\n            [\r\n              2051332.02145006,\r\n              8395393.82701298\r\n            ],\r\n            [\r\n              2051350.90184237,\r\n              8394933.04318262\r\n            ],\r\n            [\r\n              2051399.00173519,\r\n              8393984.70738644\r\n            ],\r\n            [\r\n              2051466.93151991,\r\n              8393051.55830104\r\n            ],\r\n            [\r\n              2051517.74089136,\r\n              8392303.04823429\r\n            ],\r\n            [\r\n              2051538.9645425,\r\n              8392016.48396104\r\n            ],\r\n            [\r\n              2051539.20020686,\r\n              8391948.30976542\r\n            ],\r\n            [\r\n              2050880.56654119,\r\n              8390020.56466516\r\n            ],\r\n            [\r\n              2048651.7748938,\r\n              8390142.7254691\r\n            ],\r\n            [\r\n              2048357.56391169,\r\n              8390023.54667859\r\n            ],\r\n            [\r\n              2047711.10120397,\r\n              8389833.79213373\r\n            ],\r\n            [\r\n              2047196.98726615,\r\n              8389417.86053829\r\n            ],\r\n            [\r\n              2047029.38157631,\r\n              8388620.62497625\r\n            ],\r\n            [\r\n              2046485.32507084,\r\n              8387652.84802723\r\n            ],\r\n            [\r\n              2046429.21882498,\r\n              8387265.67046744\r\n            ],\r\n            [\r\n              2046305.15357507,\r\n              8386565.29755447\r\n            ],\r\n            [\r\n              2046136.30831778,\r\n              8386185.89026481\r\n            ],\r\n            [\r\n              2045885.94226327,\r\n              8385794.59827027\r\n            ],\r\n            [\r\n              2045749.93152214,\r\n              8385582.01710633\r\n            ],\r\n            [\r\n              2045765.51814328,\r\n              8385386.29440425\r\n            ],\r\n            [\r\n              2045846.04810966,\r\n              8384375.07650786\r\n            ],\r\n            [\r\n              2046203.7126894,\r\n              8383724.69926445\r\n            ],\r\n            [\r\n              2046559.61707812,\r\n              8383183.32208858\r\n            ],\r\n            [\r\n              2046862.91199164,\r\n              8382586.39668283\r\n            ],\r\n            [\r\n              2047212.89029939,\r\n              8382168.94581197\r\n            ],\r\n            [\r\n              2047585.83521756,\r\n              8381777.23207355\r\n            ],\r\n            [\r\n              2047624.95171655,\r\n              8380789.83935899\r\n            ],\r\n            [\r\n              2047649.02947864,\r\n              8380378.12827086\r\n            ],\r\n            [\r\n              2047960.01410933,\r\n              8379861.79483877\r\n            ],\r\n            [\r\n              2048011.50489153,\r\n              8379776.29783772\r\n            ],\r\n            [\r\n              2048060.8290077,\r\n              8379088.45129727\r\n            ],\r\n            [\r\n              2048085.28884039,\r\n              8379011.12054611\r\n            ],\r\n            [\r\n              2048710.89459967,\r\n              8377336.57371355\r\n            ],\r\n            [\r\n              2048731.27755717,\r\n              8377202.41123944\r\n            ],\r\n            [\r\n              2048747.67724041,\r\n              8377032.07521608\r\n            ],\r\n            [\r\n              2048788.95463739,\r\n              8376560.88563995\r\n            ],\r\n            [\r\n              2048788.1369129,\r\n              8376370.35175886\r\n            ],\r\n            [\r\n              2048779.86438192,\r\n              8376263.13924744\r\n            ],\r\n            [\r\n              2048745.93303396,\r\n              8376183.12107745\r\n            ],\r\n            [\r\n              2048694.35989055,\r\n              8376062.56852421\r\n            ],\r\n            [\r\n              2048636.02121219,\r\n              8375926.194611\r\n            ],\r\n            [\r\n              2047787.45437434,\r\n              8375445.10253454\r\n            ],\r\n            [\r\n              2047462.61354564,\r\n              8375253.12010922\r\n            ],\r\n            [\r\n              2047462.09201639,\r\n              8375250.45412525\r\n            ],\r\n            [\r\n              2047456.5135632,\r\n              8375221.98530874\r\n            ],\r\n            [\r\n              2047207.5348621,\r\n              8373951.00915204\r\n            ],\r\n            [\r\n              2047196.45829175,\r\n              8373894.4561997\r\n            ],\r\n            [\r\n              2046770.33488275,\r\n              8373411.50908167\r\n            ],\r\n            [\r\n              2046770.81805509,\r\n              8373292.92965382\r\n            ],\r\n            [\r\n              2046748.33626323,\r\n              8373306.37381615\r\n            ],\r\n            [\r\n              2046720.90081459,\r\n              8373223.26508245\r\n            ],\r\n            [\r\n              2046703.34743426,\r\n              8373199.99817761\r\n            ],\r\n            [\r\n              2046549.66392578,\r\n              8373064.54043574\r\n            ],\r\n            [\r\n              2046521.91587962,\r\n              8373045.22911559\r\n            ],\r\n            [\r\n              2046475.75373876,\r\n              8373022.59841313\r\n            ],\r\n            [\r\n              2046441.26935577,\r\n              8373003.93060675\r\n            ],\r\n            [\r\n              2046396.06029943,\r\n              8372985.64658402\r\n            ],\r\n            [\r\n              2046211.81980779,\r\n              8372764.44959023\r\n            ],\r\n            [\r\n              2044174.94019341,\r\n              8371456.48389557\r\n            ],\r\n            [\r\n              2044203.28068252,\r\n              8371167.35092515\r\n            ],\r\n            [\r\n              2044230.73253932,\r\n              8370887.25906617\r\n            ],\r\n            [\r\n              2044257.8161263,\r\n              8370610.91349454\r\n            ],\r\n            [\r\n              2044306.99499001,\r\n              8370109.12184478\r\n            ],\r\n            [\r\n              2044314.04025571,\r\n              8370037.22747038\r\n            ],\r\n            [\r\n              2044353.47332688,\r\n              8369634.85908462\r\n            ],\r\n            [\r\n              2043145.33781497,\r\n              8369178.85474031\r\n            ],\r\n            [\r\n              2042699.4460043,\r\n              8369246.99177219\r\n            ],\r\n            [\r\n              2042121.50540964,\r\n              8369335.26461635\r\n            ],\r\n            [\r\n              2041299.59450051,\r\n              8369392.78032382\r\n            ],\r\n            [\r\n              2040876.77822413,\r\n              8369173.26051683\r\n            ],\r\n            [\r\n              2040201.50839495,\r\n              8368861.92884623\r\n            ],\r\n            [\r\n              2039273.47800504,\r\n              8368578.05882975\r\n            ],\r\n            [\r\n              2039075.01555618,\r\n              8368321.21067628\r\n            ],\r\n            [\r\n              2039053.44241738,\r\n              8368497.143796\r\n            ],\r\n            [\r\n              2038498.21888085,\r\n              8368317.2168522\r\n            ],\r\n            [\r\n              2038713.65128863,\r\n              8367880.88021102\r\n            ],\r\n            [\r\n              2038008.77896508,\r\n              8367656.35122909\r\n            ],\r\n            [\r\n              2037541.18829024,\r\n              8367499.88522342\r\n            ],\r\n            [\r\n              2036926.34593862,\r\n              8367287.34595814\r\n            ],\r\n            [\r\n              2035399.93529336,\r\n              8366875.71626912\r\n            ],\r\n            [\r\n              2034187.07585199,\r\n              8366547.29777126\r\n            ],\r\n            [\r\n              2033824.53821121,\r\n              8366180.47722749\r\n            ],\r\n            [\r\n              2033827.77874931,\r\n              8366008.35746134\r\n            ],\r\n            [\r\n              2033703.99604832,\r\n              8365845.61523561\r\n            ],\r\n            [\r\n              2033669.62664624,\r\n              8365846.39420468\r\n            ],\r\n            [\r\n              2033637.66745175,\r\n              8365842.99737232\r\n            ],\r\n            [\r\n              2033603.82869344,\r\n              8365842.06846475\r\n            ],\r\n            [\r\n              2033566.34862455,\r\n              8365839.70675686\r\n            ],\r\n            [\r\n              2033512.70013495,\r\n              8365831.27579529\r\n            ],\r\n            [\r\n              2033475.84029176,\r\n              8365815.74791148\r\n            ],\r\n            [\r\n              2033448.95474994,\r\n              8365800.98317669\r\n            ],\r\n            [\r\n              2033426.67552606,\r\n              8365773.2824197\r\n            ],\r\n            [\r\n              2033190.2326228,\r\n              8365515.35892059\r\n            ],\r\n            [\r\n              2032991.3989355,\r\n              8364703.00369919\r\n            ],\r\n            [\r\n              2032812.84520662,\r\n              8363948.43863923\r\n            ],\r\n            [\r\n              2032806.05343962,\r\n              8363920.23988584\r\n            ],\r\n            [\r\n              2032646.83491147,\r\n              8363296.96497421\r\n            ],\r\n            [\r\n              2032620.24306211,\r\n              8363276.97092511\r\n            ],\r\n            [\r\n              2032509.12225151,\r\n              8363195.38739349\r\n            ],\r\n            [\r\n              2032283.23455473,\r\n              8363025.00267497\r\n            ],\r\n            [\r\n              2032079.46395902,\r\n              8362871.29231273\r\n            ],\r\n            [\r\n              2031855.84401964,\r\n              8362929.48096547\r\n            ],\r\n            [\r\n              2031346.81747928,\r\n              8363061.90842057\r\n            ],\r\n            [\r\n              2031437.50119988,\r\n              8362723.05345441\r\n            ],\r\n            [\r\n              2031031.03015976,\r\n              8362672.55325339\r\n            ],\r\n            [\r\n              2030646.9873889,\r\n              8362622.32079951\r\n            ],\r\n            [\r\n              2030191.36385098,\r\n              8362720.58191391\r\n            ],\r\n            [\r\n              2029871.99831112,\r\n              8362530.96139022\r\n            ],\r\n            [\r\n              2029233.73846847,\r\n              8362151.94469512\r\n            ],\r\n            [\r\n              2028775.29812123,\r\n              8361898.50464822\r\n            ],\r\n            [\r\n              2028205.10522365,\r\n              8361583.22699433\r\n            ],\r\n            [\r\n              2028048.60348693,\r\n              8361612.32556048\r\n            ],\r\n            [\r\n              2027632.60091816,\r\n              8361689.21729003\r\n            ],\r\n            [\r\n              2027396.03687522,\r\n              8361731.72917323\r\n            ],\r\n            [\r\n              2027289.3081604,\r\n              8361749.93469181\r\n            ],\r\n            [\r\n              2027079.33217918,\r\n              8361788.63737293\r\n            ],\r\n            [\r\n              2027042.24756438,\r\n              8361795.23235518\r\n            ],\r\n            [\r\n              2026881.94376998,\r\n              8361823.74011675\r\n            ],\r\n            [\r\n              2026846.02352114,\r\n              8361829.10231155\r\n            ],\r\n            [\r\n              2026666.98102099,\r\n              8361855.8341795\r\n            ],\r\n            [\r\n              2026638.14060699,\r\n              8361860.35751208\r\n            ],\r\n            [\r\n              2026486.86976784,\r\n              8361945.65370831\r\n            ],\r\n            [\r\n              2026160.62007675,\r\n              8362120.19924872\r\n            ],\r\n            [\r\n              2025989.79642369,\r\n              8362202.94435295\r\n            ],\r\n            [\r\n              2025758.79343116,\r\n              8362327.82815749\r\n            ],\r\n            [\r\n              2025756.81059427,\r\n              8362327.08406262\r\n            ],\r\n            [\r\n              2024135.97173259,\r\n              8361727.99414626\r\n            ],\r\n            [\r\n              2024114.12563299,\r\n              8361719.71563246\r\n            ],\r\n            [\r\n              2023894.52645126,\r\n              8361636.49470313\r\n            ],\r\n            [\r\n              2023526.75298977,\r\n              8361505.31271134\r\n            ],\r\n            [\r\n              2023503.06154797,\r\n              8361496.86058985\r\n            ],\r\n            [\r\n              2023377.11271634,\r\n              8361451.93058993\r\n            ],\r\n            [\r\n              2023019.96044215,\r\n              8361298.76747027\r\n            ],\r\n            [\r\n              2022675.73359858,\r\n              8361151.93653466\r\n            ],\r\n            [\r\n              2022219.58882451,\r\n              8360952.63504504\r\n            ],\r\n            [\r\n              2021116.73520392,\r\n              8360478.03889433\r\n            ],\r\n            [\r\n              2020778.88580059,\r\n              8360327.35529278\r\n            ],\r\n            [\r\n              2020240.63458695,\r\n              8360158.38585442\r\n            ],\r\n            [\r\n              2019621.57121738,\r\n              8359966.88972424\r\n            ],\r\n            [\r\n              2019082.44290093,\r\n              8359803.83312217\r\n            ],\r\n            [\r\n              2018496.31023689,\r\n              8359622.34605567\r\n            ],\r\n            [\r\n              2018299.44266511,\r\n              8359560.56361759\r\n            ],\r\n            [\r\n              2017910.37598403,\r\n              8358442.67270076\r\n            ],\r\n            [\r\n              2017769.67497024,\r\n              8358075.78459729\r\n            ],\r\n            [\r\n              2017747.76888526,\r\n              8358005.07151554\r\n            ],\r\n            [\r\n              2017689.05142692,\r\n              8357845.78788529\r\n            ],\r\n            [\r\n              2017610.83111624,\r\n              8357663.71280417\r\n            ],\r\n            [\r\n              2017605.65356911,\r\n              8357651.38102318\r\n            ],\r\n            [\r\n              2017300.46996495,\r\n              8356924.3845738\r\n            ],\r\n            [\r\n              2017067.52051494,\r\n              8356407.16301772\r\n            ],\r\n            [\r\n              2016898.33226217,\r\n              8354893.44269731\r\n            ],\r\n            [\r\n              2016879.5572636,\r\n              8354725.45217473\r\n            ],\r\n            [\r\n              2017065.67940872,\r\n              8354426.63696823\r\n            ],\r\n            [\r\n              2016899.06279215,\r\n              8353973.11736621\r\n            ],\r\n            [\r\n              2016560.50615663,\r\n              8353403.19655554\r\n            ],\r\n            [\r\n              2016602.66964261,\r\n              8353103.98745639\r\n            ],\r\n            [\r\n              2016614.13895698,\r\n              8353022.59846215\r\n            ],\r\n            [\r\n              2016627.89554801,\r\n              8352999.36844745\r\n            ],\r\n            [\r\n              2016756.39734448,\r\n              8352782.37420644\r\n            ],\r\n            [\r\n              2016758.73869161,\r\n              8352521.48389666\r\n            ],\r\n            [\r\n              2016758.77525567,\r\n              8352517.35486611\r\n            ],\r\n            [\r\n              2016759.95975454,\r\n              8352385.31682008\r\n            ],\r\n            [\r\n              2016760.66878027,\r\n              8352306.26743669\r\n            ],\r\n            [\r\n              2016768.28500335,\r\n              8351457.32163349\r\n            ],\r\n            [\r\n              2016754.32946895,\r\n              8351423.46121334\r\n            ],\r\n            [\r\n              2016717.5099839,\r\n              8351334.13206417\r\n            ],\r\n            [\r\n              2016699.90594822,\r\n              8351291.41772829\r\n            ],\r\n            [\r\n              2016682.47149578,\r\n              8351249.11930095\r\n            ],\r\n            [\r\n              2016643.8414161,\r\n              8351155.3922587\r\n            ],\r\n            [\r\n              2016413.21444519,\r\n              8350595.78962248\r\n            ],\r\n            [\r\n              2016071.8777087,\r\n              8350138.96704653\r\n            ],\r\n            [\r\n              2016047.02724912,\r\n              8349848.04475577\r\n            ],\r\n            [\r\n              2016043.91561047,\r\n              8349814.81507718\r\n            ],\r\n            [\r\n              2016031.88559299,\r\n              8349670.79199497\r\n            ],\r\n            [\r\n              2016008.57783186,\r\n              8349397.90714453\r\n            ],\r\n            [\r\n              2015784.84320799,\r\n              8348763.34295996\r\n            ],\r\n            [\r\n              2015684.10639192,\r\n              8348586.97463637\r\n            ],\r\n            [\r\n              2015387.12822324,\r\n              8348066.98534056\r\n            ],\r\n            [\r\n              2015378.29349694,\r\n              8348051.59121848\r\n            ],\r\n            [\r\n              2015416.01703917,\r\n              8347005.60964809\r\n            ],\r\n            [\r\n              2015666.67305678,\r\n              8346797.83954112\r\n            ],\r\n            [\r\n              2015715.45446006,\r\n              8346757.55481467\r\n            ],\r\n            [\r\n              2015718.16092842,\r\n              8346755.32103427\r\n            ],\r\n            [\r\n              2015813.96618843,\r\n              8346697.59716372\r\n            ],\r\n            [\r\n              2015833.50549304,\r\n              8346561.98966573\r\n            ],\r\n            [\r\n              2015871.26420006,\r\n              8346299.92623034\r\n            ],\r\n            [\r\n              2015804.26389016,\r\n              8346139.55666074\r\n            ],\r\n            [\r\n              2015747.57226967,\r\n              8346003.85209041\r\n            ],\r\n            [\r\n              2015383.26938673,\r\n              8345690.46002913\r\n            ],\r\n            [\r\n              2014846.77455602,\r\n              8345228.88296703\r\n            ],\r\n            [\r\n              2014173.81274735,\r\n              8344649.79627711\r\n            ],\r\n            [\r\n              2013875.80415681,\r\n              8344303.09294162\r\n            ],\r\n            [\r\n              2013890.48269008,\r\n              8343995.97861447\r\n            ],\r\n            [\r\n              2013900.08575611,\r\n              8343795.07626451\r\n            ],\r\n            [\r\n              2013900.96999345,\r\n              8343776.56441944\r\n            ],\r\n            [\r\n              2013891.55531775,\r\n              8343758.65153231\r\n            ],\r\n            [\r\n              2013877.21737992,\r\n              8343731.37195795\r\n            ],\r\n            [\r\n              2013857.47109485,\r\n              8343676.12344579\r\n            ],\r\n            [\r\n              2013846.35451854,\r\n              8343641.20306629\r\n            ],\r\n            [\r\n              2013835.87821688,\r\n              8343579.28629288\r\n            ],\r\n            [\r\n              2013830.0561117,\r\n              8343525.93267743\r\n            ],\r\n            [\r\n              2013823.92823056,\r\n              8343463.86467978\r\n            ],\r\n            [\r\n              2013793.31148869,\r\n              8343289.99135048\r\n            ],\r\n            [\r\n              2013767.80198625,\r\n              8343239.70619238\r\n            ],\r\n            [\r\n              2013678.44011471,\r\n              8343084.14445725\r\n            ],\r\n            [\r\n              2013676.48604385,\r\n              8343081.17845664\r\n            ],\r\n            [\r\n              2013654.21534598,\r\n              8343047.40158769\r\n            ],\r\n            [\r\n              2013652.1045866,\r\n              8343044.20066225\r\n            ],\r\n            [\r\n              2013624.3201612,\r\n              8342996.77060515\r\n            ],\r\n            [\r\n              2013572.29473094,\r\n              8342889.89536692\r\n            ],\r\n            [\r\n              2013566.4717097,\r\n              8342859.15832446\r\n            ],\r\n            [\r\n              2013562.06207937,\r\n              8342789.50051383\r\n            ],\r\n            [\r\n              2013559.96138838,\r\n              8342729.28137073\r\n            ],\r\n            [\r\n              2013550.51189672,\r\n              8342696.68788125\r\n            ],\r\n            [\r\n              2013547.52666991,\r\n              8342667.83606887\r\n            ],\r\n            [\r\n              2013510.14665317,\r\n              8342594.96186735\r\n            ],\r\n            [\r\n              2013478.19466575,\r\n              8342518.7250475\r\n            ],\r\n            [\r\n              2013469.56751732,\r\n              8342498.39720496\r\n            ],\r\n            [\r\n              2013466.90961052,\r\n              8342467.5527871\r\n            ],\r\n            [\r\n              2013460.65127275,\r\n              8342435.64456595\r\n            ],\r\n            [\r\n              2013459.25157374,\r\n              8342418.23986623\r\n            ],\r\n            [\r\n              2013427.14446955,\r\n              8342326.14462848\r\n            ],\r\n            [\r\n              2013408.29157324,\r\n              8342273.65393998\r\n            ],\r\n            [\r\n              2013385.1186192,\r\n              8342222.10361133\r\n            ],\r\n            [\r\n              2013369.37095223,\r\n              8342190.5230101\r\n            ],\r\n            [\r\n              2013354.22646794,\r\n              8342164.87053712\r\n            ],\r\n            [\r\n              2013334.26906651,\r\n              8342137.40401522\r\n            ],\r\n            [\r\n              2013315.26849778,\r\n              8342114.65971986\r\n            ],\r\n            [\r\n              2013296.06445658,\r\n              8342097.48098491\r\n            ],\r\n            [\r\n              2013234.80459103,\r\n              8341827.53251772\r\n            ],\r\n            [\r\n              2013229.62323549,\r\n              8341792.41165178\r\n            ],\r\n            [\r\n              2013231.62622163,\r\n              8341759.02949605\r\n            ],\r\n            [\r\n              2013178.83416007,\r\n              8341777.51856841\r\n            ],\r\n            [\r\n              2013151.53205593,\r\n              8341789.17117838\r\n            ],\r\n            [\r\n              2013098.29239595,\r\n              8341817.58909419\r\n            ],\r\n            [\r\n              2013072.63962969,\r\n              8341831.17048678\r\n            ],\r\n            [\r\n              2013000.15203779,\r\n              8341897.9292793\r\n            ],\r\n            [\r\n              2012929.0082333,\r\n              8341957.90162132\r\n            ],\r\n            [\r\n              2012918.72470578,\r\n              8341964.36514834\r\n            ],\r\n            [\r\n              2012909.6395067,\r\n              8341970.07490433\r\n            ],\r\n            [\r\n              2012882.34930573,\r\n              8341982.12494952\r\n            ],\r\n            [\r\n              2012841.0749208,\r\n              8342001.7972069\r\n            ],\r\n            [\r\n              2012813.67640897,\r\n              8342010.67908657\r\n            ],\r\n            [\r\n              2012779.17283332,\r\n              8342020.20215458\r\n            ],\r\n            [\r\n              2012706.9757219,\r\n              8342038.5640375\r\n            ],\r\n            [\r\n              2012684.53695107,\r\n              8342042.11782622\r\n            ],\r\n            [\r\n              2012649.90881143,\r\n              8342048.07209982\r\n            ],\r\n            [\r\n              2012617.14479289,\r\n              8342039.29136747\r\n            ],\r\n            [\r\n              2012572.12382671,\r\n              8342030.93352818\r\n            ],\r\n            [\r\n              2012533.00776963,\r\n              8342021.57747751\r\n            ],\r\n            [\r\n              2012481.03836049,\r\n              8342029.72137737\r\n            ],\r\n            [\r\n              2012427.13383562,\r\n              8342039.12122142\r\n            ],\r\n            [\r\n              2012405.61011816,\r\n              8342046.20942991\r\n            ],\r\n            [\r\n              2012354.80368595,\r\n              8342065.01752644\r\n            ],\r\n            [\r\n              2012338.77389613,\r\n              8342082.23153818\r\n            ],\r\n            [\r\n              2012251.00921937,\r\n              8342062.65728445\r\n            ],\r\n            [\r\n              2012214.90764863,\r\n              8342060.33276478\r\n            ],\r\n            [\r\n              2012188.55814556,\r\n              8342076.70849274\r\n            ],\r\n            [\r\n              2012150.90763584,\r\n              8342098.23851049\r\n            ],\r\n            [\r\n              2012106.5507012,\r\n              8342120.39223301\r\n            ],\r\n            [\r\n              2012065.82873432,\r\n              8342133.29888908\r\n            ],\r\n            [\r\n              2011986.91875076,\r\n              8342140.78346493\r\n            ],\r\n            [\r\n              2011887.62548435,\r\n              8342142.61979187\r\n            ],\r\n            [\r\n              2011872.68447638,\r\n              8342145.51815869\r\n            ],\r\n            [\r\n              2011796.99753942,\r\n              8342177.47750979\r\n            ],\r\n            [\r\n              2011761.6644569,\r\n              8342208.838197\r\n            ],\r\n            [\r\n              2011738.06718148,\r\n              8342224.72145505\r\n            ],\r\n            [\r\n              2011639.73194326,\r\n              8342254.28873825\r\n            ],\r\n            [\r\n              2011628.3448805,\r\n              8342508.52418763\r\n            ],\r\n            [\r\n              2011503.97322402,\r\n              8342517.96388556\r\n            ],\r\n            [\r\n              2011441.62540984,\r\n              8342523.67997182\r\n            ],\r\n            [\r\n              2011370.5773893,\r\n              8342541.19692162\r\n            ],\r\n            [\r\n              2011289.72815106,\r\n              8342561.43297534\r\n            ],\r\n            [\r\n              2011270.79541454,\r\n              8342564.90239059\r\n            ],\r\n            [\r\n              2011255.11207874,\r\n              8342567.7808516\r\n            ],\r\n            [\r\n              2011214.22222281,\r\n              8342575.93039353\r\n            ],\r\n            [\r\n              2011194.86286835,\r\n              8342576.99233571\r\n            ],\r\n            [\r\n              2011075.74569253,\r\n              8342532.30222703\r\n            ],\r\n            [\r\n              2011022.95136854,\r\n              8342516.66554028\r\n            ],\r\n            [\r\n              2010992.35549071,\r\n              8342513.35361578\r\n            ],\r\n            [\r\n              2010946.69219445,\r\n              8342509.37142245\r\n            ],\r\n            [\r\n              2010837.43929771,\r\n              8342498.05201019\r\n            ],\r\n            [\r\n              2010791.43653721,\r\n              8342495.66540896\r\n            ],\r\n            [\r\n              2010714.93452525,\r\n              8342492.73693301\r\n            ],\r\n            [\r\n              2010589.49534382,\r\n              8342541.44525411\r\n            ],\r\n            [\r\n              2010587.19511392,\r\n              8342542.33916451\r\n            ],\r\n            [\r\n              2010535.89112972,\r\n              8342569.88153614\r\n            ],\r\n            [\r\n              2010457.64355677,\r\n              8342619.76313817\r\n            ],\r\n            [\r\n              2010388.31408499,\r\n              8342664.18715059\r\n            ],\r\n            [\r\n              2010377.43786317,\r\n              8342671.12985127\r\n            ],\r\n            [\r\n              2010304.25718986,\r\n              8342717.83680201\r\n            ],\r\n            [\r\n              2010284.13103864,\r\n              8342731.21990526\r\n            ],\r\n            [\r\n              2010259.01741315,\r\n              8342737.63483524\r\n            ],\r\n            [\r\n              2010195.28876365,\r\n              8342764.84866776\r\n            ],\r\n            [\r\n              2010166.46336927,\r\n              8342774.51994115\r\n            ],\r\n            [\r\n              2009967.38907611,\r\n              8342789.55739419\r\n            ],\r\n            [\r\n              2009964.69261393,\r\n              8342789.75824329\r\n            ],\r\n            [\r\n              2009761.80327088,\r\n              8342807.01459091\r\n            ],\r\n            [\r\n              2009711.191608,\r\n              8342808.74534221\r\n            ],\r\n            [\r\n              2009683.12990093,\r\n              8342810.10283253\r\n            ],\r\n            [\r\n              2009612.99305514,\r\n              8342808.13497827\r\n            ],\r\n            [\r\n              2009415.38700754,\r\n              8342875.57327634\r\n            ],\r\n            [\r\n              2009392.78792374,\r\n              8342805.34578024\r\n            ],\r\n            [\r\n              2009386.24570227,\r\n              8342764.71589755\r\n            ],\r\n            [\r\n              2009387.23041409,\r\n              8342735.72519244\r\n            ],\r\n            [\r\n              2009395.64567506,\r\n              8342704.10235968\r\n            ],\r\n            [\r\n              2009414.73857359,\r\n              8342672.11559745\r\n            ],\r\n            [\r\n              2009442.10685854,\r\n              8342639.05492791\r\n            ],\r\n            [\r\n              2009462.6834259,\r\n              8342627.24611642\r\n            ],\r\n            [\r\n              2009555.41635962,\r\n              8342238.1587175\r\n            ],\r\n            [\r\n              2009526.36818366,\r\n              8341925.41554939\r\n            ],\r\n            [\r\n              2009512.08594009,\r\n              8341771.64334073\r\n            ],\r\n            [\r\n              2009498.10727399,\r\n              8341762.99939427\r\n            ],\r\n            [\r\n              2009231.57761402,\r\n              8341598.18411193\r\n            ],\r\n            [\r\n              2008884.88004557,\r\n              8341383.77445814\r\n            ],\r\n            [\r\n              2008592.64532019,\r\n              8340731.87873407\r\n            ],\r\n            [\r\n              2008242.84569148,\r\n              8341556.32571942\r\n            ],\r\n            [\r\n              2007505.63538981,\r\n              8341840.71819116\r\n            ],\r\n            [\r\n              2007220.46400867,\r\n              8341979.61973517\r\n            ],\r\n            [\r\n              2007031.72546369,\r\n              8342071.54434486\r\n            ],\r\n            [\r\n              2006884.31270152,\r\n              8342143.3330847\r\n            ],\r\n            [\r\n              2006842.25323644,\r\n              8342162.75201059\r\n            ],\r\n            [\r\n              2006838.1836986,\r\n              8342173.87786464\r\n            ],\r\n            [\r\n              2006517.96807482,\r\n              8343048.97382566\r\n            ],\r\n            [\r\n              2006182.82565163,\r\n              8343173.33131966\r\n            ],\r\n            [\r\n              2006015.85289296,\r\n              8343254.73456766\r\n            ],\r\n            [\r\n              2006133.20521897,\r\n              8342461.07892015\r\n            ],\r\n            [\r\n              2006081.67471273,\r\n              8342484.90521216\r\n            ],\r\n            [\r\n              2006032.55361124,\r\n              8342509.84711117\r\n            ],\r\n            [\r\n              2005984.93964979,\r\n              8342519.66605047\r\n            ],\r\n            [\r\n              2005937.7824091,\r\n              8342518.75994823\r\n            ],\r\n            [\r\n              2005892.38153562,\r\n              8342510.65488524\r\n            ],\r\n            [\r\n              2005848.06783397,\r\n              8342499.33974626\r\n            ],\r\n            [\r\n              2005815.94371522,\r\n              8342485.26421649\r\n            ],\r\n            [\r\n              2005779.98898046,\r\n              8342462.5805866\r\n            ],\r\n            [\r\n              2005731.4078189,\r\n              8342428.78376738\r\n            ],\r\n            [\r\n              2005707.62775889,\r\n              8342631.08308893\r\n            ],\r\n            [\r\n              2005182.45055715,\r\n              8342655.77370635\r\n            ],\r\n            [\r\n              2005177.69518942,\r\n              8342614.56183217\r\n            ],\r\n            [\r\n              2005171.41271576,\r\n              8342560.12254642\r\n            ],\r\n            [\r\n              2005161.8315225,\r\n              8342477.07591312\r\n            ],\r\n            [\r\n              2005152.58066982,\r\n              8342447.1791735\r\n            ],\r\n            [\r\n              2005142.01423122,\r\n              8342413.02376734\r\n            ],\r\n            [\r\n              2005730.89588404,\r\n              8342298.26915673\r\n            ],\r\n            [\r\n              2005724.63411566,\r\n              8342211.9753668\r\n            ],\r\n            [\r\n              2005724.15220938,\r\n              8342183.81825379\r\n            ],\r\n            [\r\n              2005734.44851523,\r\n              8342145.80564471\r\n            ],\r\n            [\r\n              2005098.47166326,\r\n              8342312.01608771\r\n            ],\r\n            [\r\n              2004986.20866807,\r\n              8342064.36602789\r\n            ],\r\n            [\r\n              2004974.28705792,\r\n              8342004.73901814\r\n            ],\r\n            [\r\n              2004952.02306594,\r\n              8341893.38959923\r\n            ],\r\n            [\r\n              2004928.59153384,\r\n              8341776.19751888\r\n            ],\r\n            [\r\n              2004902.25052049,\r\n              8341644.46037397\r\n            ],\r\n            [\r\n              2004477.5390508,\r\n              8341694.91567251\r\n            ],\r\n            [\r\n              2004418.54946238,\r\n              8341701.92462812\r\n            ],\r\n            [\r\n              2004413.61608058,\r\n              8341530.30150129\r\n            ],\r\n            [\r\n              2004412.3740669,\r\n              8341525.32250996\r\n            ],\r\n            [\r\n              2004409.09298067,\r\n              8341512.17623458\r\n            ],\r\n            [\r\n              2004353.38532415,\r\n              8341289.00915683\r\n            ],\r\n            [\r\n              2004340.80297915,\r\n              8341238.60243766\r\n            ],\r\n            [\r\n              2004175.01854517,\r\n              8341293.7236768\r\n            ],\r\n            [\r\n              2004195.84783242,\r\n              8341377.97138787\r\n            ],\r\n            [\r\n              2003961.03014249,\r\n              8341416.18527758\r\n            ],\r\n            [\r\n              2003853.70360664,\r\n              8341402.84435853\r\n            ],\r\n            [\r\n              2003801.35284375,\r\n              8341042.28177078\r\n            ],\r\n            [\r\n              2003638.02522584,\r\n              8340844.23451994\r\n            ],\r\n            [\r\n              2003987.24605548,\r\n              8340792.97369815\r\n            ],\r\n            [\r\n              2003884.38043727,\r\n              8340348.33204181\r\n            ],\r\n            [\r\n              2004114.11218798,\r\n              8340287.67295346\r\n            ],\r\n            [\r\n              2004010.85402378,\r\n              8340021.95092516\r\n            ],\r\n            [\r\n              2003921.26465555,\r\n              8339979.90640984\r\n            ],\r\n            [\r\n              2003704.19911174,\r\n              8339707.79645498\r\n            ],\r\n            [\r\n              2003642.90357289,\r\n              8339569.29130264\r\n            ],\r\n            [\r\n              2003500.63801312,\r\n              8339603.03374141\r\n            ],\r\n            [\r\n              2002818.65816368,\r\n              8339889.3963639\r\n            ],\r\n            [\r\n              2002725.89306476,\r\n              8340142.12565979\r\n            ],\r\n            [\r\n              2002172.08633571,\r\n              8339819.46817657\r\n            ],\r\n            [\r\n              2002113.30047542,\r\n              8339785.21491064\r\n            ],\r\n            [\r\n              2002032.0881565,\r\n              8339737.89648074\r\n            ],\r\n            [\r\n              2001989.91808486,\r\n              8339713.32158094\r\n            ],\r\n            [\r\n              2002026.47361713,\r\n              8339709.08924985\r\n            ],\r\n            [\r\n              2002273.36409705,\r\n              8339717.37507983\r\n            ],\r\n            [\r\n              2002354.81605626,\r\n              8339714.07894907\r\n            ],\r\n            [\r\n              2002441.33591754,\r\n              8339708.24724832\r\n            ],\r\n            [\r\n              2002477.292559,\r\n              8339448.95512603\r\n            ],\r\n            [\r\n              2002475.59812212,\r\n              8339393.87785441\r\n            ],\r\n            [\r\n              2002417.16208128,\r\n              8339244.577628\r\n            ],\r\n            [\r\n              2002404.44811407,\r\n              8339209.30421115\r\n            ],\r\n            [\r\n              2002402.88000492,\r\n              8339204.95823862\r\n            ],\r\n            [\r\n              2002311.50799218,\r\n              8339242.64208607\r\n            ],\r\n            [\r\n              2002281.31105818,\r\n              8339255.09517438\r\n            ],\r\n            [\r\n              2002192.92436422,\r\n              8339290.12110135\r\n            ],\r\n            [\r\n              2002190.18468289,\r\n              8339291.20673638\r\n            ],\r\n            [\r\n              2002141.70515502,\r\n              8339310.93778297\r\n            ],\r\n            [\r\n              2002122.44115039,\r\n              8339328.18571629\r\n            ],\r\n            [\r\n              2002110.72515929,\r\n              8339359.08321118\r\n            ],\r\n            [\r\n              2002106.63031126,\r\n              8339393.31672389\r\n            ],\r\n            [\r\n              2002102.23524102,\r\n              8339408.01692063\r\n            ],\r\n            [\r\n              2002098.37541798,\r\n              8339420.93349725\r\n            ],\r\n            [\r\n              2002073.45969968,\r\n              8339447.47657351\r\n            ],\r\n            [\r\n              2002025.20803,\r\n              8339474.73672311\r\n            ],\r\n            [\r\n              2001913.05725986,\r\n              8339535.2841918\r\n            ],\r\n            [\r\n              2001965.59930205,\r\n              8339609.03020107\r\n            ],\r\n            [\r\n              2001973.39424029,\r\n              8339669.46791466\r\n            ],\r\n            [\r\n              2001961.56620818,\r\n              8339696.80257835\r\n            ],\r\n            [\r\n              2001864.02827272,\r\n              8339562.96563317\r\n            ],\r\n            [\r\n              2001862.75051268,\r\n              8339560.5904256\r\n            ],\r\n            [\r\n              2001637.18022289,\r\n              8339141.64213326\r\n            ],\r\n            [\r\n              2001431.84236288,\r\n              8338760.24288483\r\n            ],\r\n            [\r\n              2001415.18716106,\r\n              8338729.76512105\r\n            ],\r\n            [\r\n              2001026.20627364,\r\n              8338102.23363094\r\n            ],\r\n            [\r\n              2000653.67591996,\r\n              8338184.43085392\r\n            ],\r\n            [\r\n              2000260.23922984,\r\n              8338271.22003776\r\n            ],\r\n            [\r\n              2000217.50492785,\r\n              8338232.94177373\r\n            ],\r\n            [\r\n              1999671.84021518,\r\n              8337744.10968977\r\n            ],\r\n            [\r\n              1999613.54517677,\r\n              8337643.74901331\r\n            ],\r\n            [\r\n              1999343.90881261,\r\n              8337547.14284599\r\n            ],\r\n            [\r\n              1999318.69914981,\r\n              8337540.42662351\r\n            ],\r\n            [\r\n              1999049.55384177,\r\n              8337468.70260794\r\n            ],\r\n            [\r\n              1998639.32449092,\r\n              8337359.3593543\r\n            ],\r\n            [\r\n              1998271.26727036,\r\n              8336849.43463576\r\n            ],\r\n            [\r\n              1997817.177562,\r\n              8336220.2416389\r\n            ],\r\n            [\r\n              1997801.8604077,\r\n              8335688.00484511\r\n            ],\r\n            [\r\n              1997380.29988047,\r\n              8336187.77299596\r\n            ],\r\n            [\r\n              1996874.23756857,\r\n              8336267.46692854\r\n            ],\r\n            [\r\n              1996793.70940203,\r\n              8336231.73043302\r\n            ],\r\n            [\r\n              1995908.00093596,\r\n              8335896.15540459\r\n            ],\r\n            [\r\n              1994687.53155566,\r\n              8335562.92484061\r\n            ],\r\n            [\r\n              1994425.26285659,\r\n              8335514.38894826\r\n            ],\r\n            [\r\n              1994343.00824059,\r\n              8335499.16450977\r\n            ],\r\n            [\r\n              1993713.65127878,\r\n              8335382.64730935\r\n            ],\r\n            [\r\n              1993077.42456746,\r\n              8335903.02720613\r\n            ],\r\n            [\r\n              1992823.14661985,\r\n              8335364.43784408\r\n            ],\r\n            [\r\n              1992745.74497082,\r\n              8335385.81460634\r\n            ],\r\n            [\r\n              1992035.0787576,\r\n              8335582.02894684\r\n            ],\r\n            [\r\n              1991953.72472039,\r\n              8335044.94551403\r\n            ],\r\n            [\r\n              1991720.68169593,\r\n              8334893.91359031\r\n            ],\r\n            [\r\n              1990619.55472873,\r\n              8334180.14712618\r\n            ],\r\n            [\r\n              1990327.4221948,\r\n              8333990.74691287\r\n            ],\r\n            [\r\n              1990198.25523405,\r\n              8333906.99421758\r\n            ],\r\n            [\r\n              1989820.09708991,\r\n              8333661.78489092\r\n            ],\r\n            [\r\n              1989594.95256439,\r\n              8333515.77619329\r\n            ],\r\n            [\r\n              1989088.45706584,\r\n              8333187.27919799\r\n            ],\r\n            [\r\n              1988983.98343671,\r\n              8333122.25344536\r\n            ],\r\n            [\r\n              1988745.48627428,\r\n              8333042.84314865\r\n            ],\r\n            [\r\n              1988653.99496195,\r\n              8333052.70973709\r\n            ],\r\n            [\r\n              1988461.73610165,\r\n              8333009.37663153\r\n            ],\r\n            [\r\n              1988087.27648558,\r\n              8332924.95658078\r\n            ],\r\n            [\r\n              1987625.5194729,\r\n              8332820.82772872\r\n            ],\r\n            [\r\n              1987124.42804435,\r\n              8332754.78711733\r\n            ],\r\n            [\r\n              1986942.30536026,\r\n              8332589.59232523\r\n            ],\r\n            [\r\n              1986784.43711992,\r\n              8332436.96082461\r\n            ],\r\n            [\r\n              1986139.03496015,\r\n              8331849.80245516\r\n            ],\r\n            [\r\n              1985865.9248789,\r\n              8331694.94867364\r\n            ],\r\n            [\r\n              1985637.13606407,\r\n              8331565.21003226\r\n            ],\r\n            [\r\n              1985579.57568079,\r\n              8331532.57163583\r\n            ],\r\n            [\r\n              1985557.89730117,\r\n              8331520.27892412\r\n            ],\r\n            [\r\n              1985559.78127962,\r\n              8331475.08241833\r\n            ],\r\n            [\r\n              1985553.78685451,\r\n              8331430.90250331\r\n            ],\r\n            [\r\n              1985460.04043334,\r\n              8331359.91943424\r\n            ],\r\n            [\r\n              1985433.11305394,\r\n              8331330.59409279\r\n            ],\r\n            [\r\n              1985336.85169828,\r\n              8331268.3921181\r\n            ],\r\n            [\r\n              1985317.73411783,\r\n              8331249.53470449\r\n            ],\r\n            [\r\n              1985308.58275104,\r\n              8331233.55828537\r\n            ],\r\n            [\r\n              1985242.80830139,\r\n              8331102.78348346\r\n            ],\r\n            [\r\n              1985188.53381109,\r\n              8331071.0652568\r\n            ],\r\n            [\r\n              1985160.03822625,\r\n              8331042.17845011\r\n            ],\r\n            [\r\n              1985142.68291276,\r\n              8331025.75425709\r\n            ],\r\n            [\r\n              1985136.88283651,\r\n              8331020.26742368\r\n            ],\r\n            [\r\n              1985112.26515103,\r\n              8331016.61021658\r\n            ],\r\n            [\r\n              1985128.8688928,\r\n              8330862.51283909\r\n            ],\r\n            [\r\n              1985214.67209196,\r\n              8330848.5899538\r\n            ],\r\n            [\r\n              1985360.54043391,\r\n              8330861.46633596\r\n            ],\r\n            [\r\n              1985507.82844859,\r\n              8330840.64641806\r\n            ],\r\n            [\r\n              1985557.44517209,\r\n              8330665.81500488\r\n            ],\r\n            [\r\n              1985354.72387893,\r\n              8329810.98570639\r\n            ],\r\n            [\r\n              1985340.05069768,\r\n              8329751.60807358\r\n            ],\r\n            [\r\n              1985301.93890975,\r\n              8329597.37993312\r\n            ],\r\n            [\r\n              1985248.88885811,\r\n              8329534.77328406\r\n            ],\r\n            [\r\n              1985155.17909875,\r\n              8329424.17715277\r\n            ],\r\n            [\r\n              1985109.37188036,\r\n              8329291.86171646\r\n            ],\r\n            [\r\n              1985152.64992255,\r\n              8328985.70163989\r\n            ],\r\n            [\r\n              1985042.50865452,\r\n              8329044.28840568\r\n            ],\r\n            [\r\n              1984967.62304703,\r\n              8329082.57392322\r\n            ],\r\n            [\r\n              1984853.45398779,\r\n              8329136.07192491\r\n            ],\r\n            [\r\n              1984807.1057824,\r\n              8329154.15587971\r\n            ],\r\n            [\r\n              1984756.47607113,\r\n              8329176.65286389\r\n            ],\r\n            [\r\n              1984718.02047375,\r\n              8329194.23007875\r\n            ],\r\n            [\r\n              1984660.80312401,\r\n              8329225.53057186\r\n            ],\r\n            [\r\n              1984599.02309394,\r\n              8329268.76928578\r\n            ],\r\n            [\r\n              1984580.59935259,\r\n              8329278.53312468\r\n            ],\r\n            [\r\n              1984452.17628704,\r\n              8329058.6523012\r\n            ],\r\n            [\r\n              1984368.32339762,\r\n              8328915.07992456\r\n            ],\r\n            [\r\n              1984363.13341092,\r\n              8328906.18766719\r\n            ],\r\n            [\r\n              1984362.04954425,\r\n              8328904.33771926\r\n            ],\r\n            [\r\n              1984357.11636936,\r\n              8328895.88921941\r\n            ],\r\n            [\r\n              1984345.87202832,\r\n              8328876.63283484\r\n            ],\r\n            [\r\n              1984336.56022037,\r\n              8328860.69166234\r\n            ],\r\n            [\r\n              1984325.12596497,\r\n              8328841.11015275\r\n            ],\r\n            [\r\n              1984316.45897961,\r\n              8328826.2736218\r\n            ],\r\n            [\r\n              1984297.7526091,\r\n              8328794.24091438\r\n            ],\r\n            [\r\n              1984105.18037464,\r\n              8328728.1534482\r\n            ],\r\n            [\r\n              1983989.09854875,\r\n              8328687.58666974\r\n            ],\r\n            [\r\n              1983746.36401026,\r\n              8328577.29097334\r\n            ],\r\n            [\r\n              1983269.15792731,\r\n              8328389.49106044\r\n            ],\r\n            [\r\n              1983185.50764738,\r\n              8328532.43262275\r\n            ],\r\n            [\r\n              1982956.74433997,\r\n              8328923.31829441\r\n            ],\r\n            [\r\n              1982895.25053315,\r\n              8329028.38941721\r\n            ],\r\n            [\r\n              1982107.53740992,\r\n              8329550.07932056\r\n            ],\r\n            [\r\n              1981947.16407135,\r\n              8329496.53396636\r\n            ],\r\n            [\r\n              1981852.69055883,\r\n              8329520.42132648\r\n            ],\r\n            [\r\n              1981822.38585849,\r\n              8329528.36621968\r\n            ],\r\n            [\r\n              1981782.15413788,\r\n              8329532.89218105\r\n            ],\r\n            [\r\n              1981748.97557336,\r\n              8329533.35941418\r\n            ],\r\n            [\r\n              1981683.60353033,\r\n              8329520.42288288\r\n            ],\r\n            [\r\n              1981476.10982449,\r\n              8329499.61981256\r\n            ],\r\n            [\r\n              1981432.58153281,\r\n              8329592.4139283\r\n            ],\r\n            [\r\n              1981336.73932423,\r\n              8329873.12374573\r\n            ],\r\n            [\r\n              1981243.30441049,\r\n              8330146.77821568\r\n            ],\r\n            [\r\n              1981238.47509316,\r\n              8330172.25080349\r\n            ],\r\n            [\r\n              1981161.4064108,\r\n              8330215.96899813\r\n            ],\r\n            [\r\n              1981121.1968086,\r\n              8330234.51306736\r\n            ],\r\n            [\r\n              1981088.1986575,\r\n              8330242.16191139\r\n            ],\r\n            [\r\n              1981066.14311546,\r\n              8330245.14947449\r\n            ],\r\n            [\r\n              1981046.46595415,\r\n              8330248.46967221\r\n            ],\r\n            [\r\n              1981042.84822798,\r\n              8330249.86140644\r\n            ],\r\n            [\r\n              1980733.06189843,\r\n              8330368.8575954\r\n            ],\r\n            [\r\n              1980397.70817722,\r\n              8330497.66030217\r\n            ],\r\n            [\r\n              1980168.58202433,\r\n              8330585.6513739\r\n            ],\r\n            [\r\n              1979919.1316165,\r\n              8330213.2794757\r\n            ],\r\n            [\r\n              1979906.81117153,\r\n              8330196.59628239\r\n            ],\r\n            [\r\n              1979883.58220266,\r\n              8330186.20873031\r\n            ],\r\n            [\r\n              1979869.84849077,\r\n              8330210.42363427\r\n            ],\r\n            [\r\n              1979733.73310392,\r\n              8330291.75787971\r\n            ],\r\n            [\r\n              1979705.9550899,\r\n              8330308.53457309\r\n            ],\r\n            [\r\n              1979692.32646774,\r\n              8330316.76985202\r\n            ],\r\n            [\r\n              1979676.17296787,\r\n              8330327.18333583\r\n            ],\r\n            [\r\n              1979671.10346531,\r\n              8330331.60365681\r\n            ],\r\n            [\r\n              1979652.54894889,\r\n              8330347.78035655\r\n            ],\r\n            [\r\n              1979490.84498817,\r\n              8330507.15145734\r\n            ],\r\n            [\r\n              1979470.94696274,\r\n              8330531.43153583\r\n            ],\r\n            [\r\n              1979462.98506769,\r\n              8330545.90351507\r\n            ],\r\n            [\r\n              1979461.79719373,\r\n              8330551.96335029\r\n            ],\r\n            [\r\n              1979458.96696008,\r\n              8330566.41319322\r\n            ],\r\n            [\r\n              1979460.58775712,\r\n              8330598.39719903\r\n            ],\r\n            [\r\n              1979462.17117987,\r\n              8330613.74511974\r\n            ],\r\n            [\r\n              1979466.42320258,\r\n              8330654.90454849\r\n            ],\r\n            [\r\n              1979468.60165458,\r\n              8330706.44611621\r\n            ],\r\n            [\r\n              1979465.09517043,\r\n              8330913.68790547\r\n            ],\r\n            [\r\n              1979460.55274808,\r\n              8330973.91187627\r\n            ],\r\n            [\r\n              1979450.84178145,\r\n              8331026.80038889\r\n            ],\r\n            [\r\n              1979173.51834938,\r\n              8331139.69629767\r\n            ],\r\n            [\r\n              1979076.23399807,\r\n              8331179.29418757\r\n            ],\r\n            [\r\n              1979028.62629154,\r\n              8331072.51146599\r\n            ],\r\n            [\r\n              1978993.06177652,\r\n              8331087.74861625\r\n            ],\r\n            [\r\n              1978926.45801711,\r\n              8330928.95319164\r\n            ],\r\n            [\r\n              1978728.26178161,\r\n              8330456.40355402\r\n            ],\r\n            [\r\n              1978500.21811684,\r\n              8330199.00699623\r\n            ],\r\n            [\r\n              1978430.95317365,\r\n              8330225.0618426\r\n            ],\r\n            [\r\n              1978417.79552203,\r\n              8330221.06846275\r\n            ],\r\n            [\r\n              1978361.69653496,\r\n              8329962.01891699\r\n            ],\r\n            [\r\n              1978313.86730852,\r\n              8329767.44133595\r\n            ],\r\n            [\r\n              1978088.78565462,\r\n              8328852.87478538\r\n            ],\r\n            [\r\n              1978032.29394654,\r\n              8328623.31440258\r\n            ],\r\n            [\r\n              1977837.13792435,\r\n              8328478.74502388\r\n            ],\r\n            [\r\n              1977824.15420609,\r\n              8328456.07304932\r\n            ],\r\n            [\r\n              1977497.58182217,\r\n              8327885.79940863\r\n            ],\r\n            [\r\n              1977474.60740288,\r\n              8327823.58805627\r\n            ],\r\n            [\r\n              1977457.96277324,\r\n              8327762.08506609\r\n            ],\r\n            [\r\n              1977452.6281892,\r\n              8327718.62892341\r\n            ],\r\n            [\r\n              1977458.11614435,\r\n              8327686.90250193\r\n            ],\r\n            [\r\n              1977464.56441012,\r\n              8327669.58884921\r\n            ],\r\n            [\r\n              1977468.78208607,\r\n              8327658.26969207\r\n            ],\r\n            [\r\n              1977501.88936424,\r\n              8327595.69941404\r\n            ],\r\n            [\r\n              1977512.25257104,\r\n              8327286.53951988\r\n            ],\r\n            [\r\n              1977459.78008205,\r\n              8327145.02453282\r\n            ],\r\n            [\r\n              1977324.8303171,\r\n              8326781.04615511\r\n            ],\r\n            [\r\n              1977015.46759315,\r\n              8326135.19171666\r\n            ],\r\n            [\r\n              1977008.1253265,\r\n              8326117.88691173\r\n            ],\r\n            [\r\n              1977003.0955817,\r\n              8326096.59079913\r\n            ],\r\n            [\r\n              1976984.16074609,\r\n              8326040.27280151\r\n            ],\r\n            [\r\n              1976977.78862325,\r\n              8326007.52458462\r\n            ],\r\n            [\r\n              1976973.50652209,\r\n              8325954.17736795\r\n            ],\r\n            [\r\n              1976971.9126874,\r\n              8325924.13404632\r\n            ],\r\n            [\r\n              1976970.78650981,\r\n              8325870.74393705\r\n            ],\r\n            [\r\n              1976986.75297339,\r\n              8325681.04605586\r\n            ],\r\n            [\r\n              1976995.38987758,\r\n              8325590.73827415\r\n            ],\r\n            [\r\n              1977008.73022011,\r\n              8325497.99528505\r\n            ],\r\n            [\r\n              1977004.82119901,\r\n              8325471.93820659\r\n            ],\r\n            [\r\n              1976981.66468804,\r\n              8325424.38569815\r\n            ],\r\n            [\r\n              1976908.03379879,\r\n              8325323.71292772\r\n            ],\r\n            [\r\n              1976871.70187833,\r\n              8325265.25923377\r\n            ],\r\n            [\r\n              1976870.51817813,\r\n              8325263.37264596\r\n            ],\r\n            [\r\n              1976780.48499217,\r\n              8325119.72756271\r\n            ],\r\n            [\r\n              1976677.87911225,\r\n              8324960.72449506\r\n            ],\r\n            [\r\n              1976638.25390574,\r\n              8324899.32096495\r\n            ],\r\n            [\r\n              1976619.19205497,\r\n              8324878.004141\r\n            ],\r\n            [\r\n              1976588.60789485,\r\n              8324853.79337606\r\n            ],\r\n            [\r\n              1976522.22458999,\r\n              8324801.92961229\r\n            ],\r\n            [\r\n              1976519.47803981,\r\n              8324799.96229021\r\n            ],\r\n            [\r\n              1976425.41854773,\r\n              8324732.57691654\r\n            ],\r\n            [\r\n              1976327.27980686,\r\n              8324656.92137682\r\n            ],\r\n            [\r\n              1976289.91037955,\r\n              8324612.6929041\r\n            ],\r\n            [\r\n              1976238.4015202,\r\n              8324554.94366758\r\n            ],\r\n            [\r\n              1976220.24019017,\r\n              8324538.35328525\r\n            ],\r\n            [\r\n              1976199.67870293,\r\n              8324537.2497566\r\n            ],\r\n            [\r\n              1976169.36952685,\r\n              8324541.51365608\r\n            ],\r\n            [\r\n              1976139.32173181,\r\n              8324556.85076156\r\n            ],\r\n            [\r\n              1976114.03546306,\r\n              8324573.26720424\r\n            ],\r\n            [\r\n              1976099.86723963,\r\n              8324592.19011489\r\n            ],\r\n            [\r\n              1976074.11740704,\r\n              8324622.4592053\r\n            ],\r\n            [\r\n              1976045.25879632,\r\n              8324671.78985963\r\n            ],\r\n            [\r\n              1976024.06837401,\r\n              8324694.43711837\r\n            ],\r\n            [\r\n              1975994.66296274,\r\n              8324720.44259288\r\n            ],\r\n            [\r\n              1975977.66505711,\r\n              8324736.66266498\r\n            ],\r\n            [\r\n              1975958.53240497,\r\n              8324762.82138664\r\n            ],\r\n            [\r\n              1975907.03592912,\r\n              8324823.7595488\r\n            ],\r\n            [\r\n              1975835.70783039,\r\n              8324897.81467848\r\n            ],\r\n            [\r\n              1975824.48208059,\r\n              8324907.57339332\r\n            ],\r\n            [\r\n              1975769.7801535,\r\n              8324949.59470741\r\n            ],\r\n            [\r\n              1975685.67535583,\r\n              8325018.01058436\r\n            ],\r\n            [\r\n              1975667.92691983,\r\n              8325035.83215761\r\n            ],\r\n            [\r\n              1975653.0039213,\r\n              8325073.36675521\r\n            ],\r\n            [\r\n              1975631.41506643,\r\n              8325146.66724235\r\n            ],\r\n            [\r\n              1975594.85995354,\r\n              8325204.88673128\r\n            ],\r\n            [\r\n              1975545.62234735,\r\n              8325278.03658251\r\n            ],\r\n            [\r\n              1975477.78281561,\r\n              8325366.65072684\r\n            ],\r\n            [\r\n              1975456.53278119,\r\n              8325386.92982646\r\n            ],\r\n            [\r\n              1975434.2188141,\r\n              8325412.37299791\r\n            ],\r\n            [\r\n              1975387.03718045,\r\n              8325455.4031264\r\n            ],\r\n            [\r\n              1975369.6606378,\r\n              8325472.42627866\r\n            ],\r\n            [\r\n              1975334.07930399,\r\n              8325487.89038121\r\n            ],\r\n            [\r\n              1975308.60540715,\r\n              8325496.39103051\r\n            ],\r\n            [\r\n              1975269.26015912,\r\n              8325502.83982398\r\n            ],\r\n            [\r\n              1975200.43291071,\r\n              8325498.49631927\r\n            ],\r\n            [\r\n              1975186.69919015,\r\n              8325502.37741466\r\n            ],\r\n            [\r\n              1975161.04170157,\r\n              8325519.98221346\r\n            ],\r\n            [\r\n              1975136.25574315,\r\n              8325541.13221451\r\n            ],\r\n            [\r\n              1975061.87270174,\r\n              8325603.39016343\r\n            ],\r\n            [\r\n              1975059.00120627,\r\n              8325606.40283836\r\n            ],\r\n            [\r\n              1975033.72720535,\r\n              8325632.92404605\r\n            ],\r\n            [\r\n              1975001.37814125,\r\n              8325685.50562536\r\n            ],\r\n            [\r\n              1974956.92216283,\r\n              8325743.90730574\r\n            ],\r\n            [\r\n              1974940.46937361,\r\n              8325749.82404485\r\n            ],\r\n            [\r\n              1974917.1400537,\r\n              8325748.77983168\r\n            ],\r\n            [\r\n              1974898.6867598,\r\n              8325753.55763357\r\n            ],\r\n            [\r\n              1974888.65471214,\r\n              8325763.68295976\r\n            ],\r\n            [\r\n              1974859.37668308,\r\n              8325778.602453\r\n            ],\r\n            [\r\n              1974832.77195231,\r\n              8325789.50641476\r\n            ],\r\n            [\r\n              1974804.4959787,\r\n              8325796.4904231\r\n            ],\r\n            [\r\n              1974765.92719318,\r\n              8325802.52402252\r\n            ],\r\n            [\r\n              1974746.37380385,\r\n              8325801.49764104\r\n            ],\r\n            [\r\n              1974727.78084423,\r\n              8325792.71869159\r\n            ],\r\n            [\r\n              1974718.42675138,\r\n              8325781.06440665\r\n            ],\r\n            [\r\n              1974711.80467171,\r\n              8325768.15799566\r\n            ],\r\n            [\r\n              1974717.78404494,\r\n              8325753.38246143\r\n            ],\r\n            [\r\n              1974721.74667543,\r\n              8325737.06626425\r\n            ],\r\n            [\r\n              1974719.71865644,\r\n              8325717.72494908\r\n            ],\r\n            [\r\n              1974708.04895205,\r\n              8325708.49670048\r\n            ],\r\n            [\r\n              1974678.61636217,\r\n              8325699.68233685\r\n            ],\r\n            [\r\n              1974657.35601833,\r\n              8325696.87474371\r\n            ],\r\n            [\r\n              1974632.65715428,\r\n              8325693.61748927\r\n            ],\r\n            [\r\n              1974562.84670544,\r\n              8325680.98435788\r\n            ],\r\n            [\r\n              1974290.29397621,\r\n              8325629.0952684\r\n            ],\r\n            [\r\n              1973992.0114257,\r\n              8325506.17066811\r\n            ],\r\n            [\r\n              1973824.75779765,\r\n              8325400.22528872\r\n            ],\r\n            [\r\n              1973512.21550897,\r\n              8325202.2284111\r\n            ],\r\n            [\r\n              1973277.93621174,\r\n              8326244.26988628\r\n            ],\r\n            [\r\n              1973220.84989276,\r\n              8326543.94377753\r\n            ],\r\n            [\r\n              1973220.5944515,\r\n              8326547.41074879\r\n            ],\r\n            [\r\n              1973143.41737986,\r\n              8327589.70315129\r\n            ],\r\n            [\r\n              1973142.88872672,\r\n              8327593.4893367\r\n            ],\r\n            [\r\n              1972898.50296736,\r\n              8329343.25575078\r\n            ],\r\n            [\r\n              1972898.1264521,\r\n              8329345.62109231\r\n            ],\r\n            [\r\n              1972845.55308783,\r\n              8329675.1989707\r\n            ],\r\n            [\r\n              1972837.02347609,\r\n              8329728.66416182\r\n            ],\r\n            [\r\n              1972668.47381251,\r\n              8329702.81945648\r\n            ],\r\n            [\r\n              1972623.30737575,\r\n              8329707.1677941\r\n            ],\r\n            [\r\n              1971662.48845706,\r\n              8329831.37999324\r\n            ],\r\n            [\r\n              1971584.14518777,\r\n              8329775.35504071\r\n            ],\r\n            [\r\n              1971545.5817444,\r\n              8329765.14374761\r\n            ],\r\n            [\r\n              1971352.62459268,\r\n              8329760.79916743\r\n            ],\r\n            [\r\n              1971315.44291921,\r\n              8329759.26529085\r\n            ],\r\n            [\r\n              1971260.54687111,\r\n              8329768.90448394\r\n            ],\r\n            [\r\n              1971243.01704619,\r\n              8329771.98395666\r\n            ],\r\n            [\r\n              1970980.61941682,\r\n              8329807.20888733\r\n            ],\r\n            [\r\n              1970927.97657935,\r\n              8329818.8625358\r\n            ],\r\n            [\r\n              1970855.18186716,\r\n              8329834.97780103\r\n            ],\r\n            [\r\n              1970737.37665719,\r\n              8330184.35280658\r\n            ],\r\n            [\r\n              1970717.88634738,\r\n              8330238.34476209\r\n            ],\r\n            [\r\n              1970581.0954555,\r\n              8330188.82764538\r\n            ],\r\n            [\r\n              1970557.48023369,\r\n              8330180.27787609\r\n            ],\r\n            [\r\n              1970382.61839301,\r\n              8330117.99929065\r\n            ],\r\n            [\r\n              1970347.63365239,\r\n              8330195.50624233\r\n            ],\r\n            [\r\n              1970347.22985268,\r\n              8330201.79103706\r\n            ],\r\n            [\r\n              1970346.21270378,\r\n              8330209.94878531\r\n            ],\r\n            [\r\n              1970340.05130356,\r\n              8330224.45392114\r\n            ],\r\n            [\r\n              1970335.26812716,\r\n              8330244.05380059\r\n            ],\r\n            [\r\n              1970331.50459898,\r\n              8330259.25688315\r\n            ],\r\n            [\r\n              1970324.49057536,\r\n              8330272.21189423\r\n            ],\r\n            [\r\n              1970319.25252121,\r\n              8330279.94387831\r\n            ],\r\n            [\r\n              1970307.12330059,\r\n              8330293.48665735\r\n            ],\r\n            [\r\n              1970303.26313926,\r\n              8330296.01710589\r\n            ],\r\n            [\r\n              1970296.54116966,\r\n              8330306.18124548\r\n            ],\r\n            [\r\n              1970293.55021095,\r\n              8330320.9604378\r\n            ],\r\n            [\r\n              1970293.14151236,\r\n              8330341.18631551\r\n            ],\r\n            [\r\n              1970292.08374721,\r\n              8330354.7001073\r\n            ],\r\n            [\r\n              1970292.13304302,\r\n              8330376.49116348\r\n            ],\r\n            [\r\n              1970295.7818366,\r\n              8330409.63932182\r\n            ],\r\n            [\r\n              1970294.88896482,\r\n              8330427.50678532\r\n            ],\r\n            [\r\n              1970284.56247626,\r\n              8330436.62182594\r\n            ],\r\n            [\r\n              1970263.09534797,\r\n              8330474.7059696\r\n            ],\r\n            [\r\n              1970233.13842498,\r\n              8330518.26514798\r\n            ],\r\n            [\r\n              1970226.50443703,\r\n              8330541.10884096\r\n            ],\r\n            [\r\n              1970211.65628592,\r\n              8330566.25530978\r\n            ],\r\n            [\r\n              1970192.54320651,\r\n              8330614.15018171\r\n            ],\r\n            [\r\n              1970186.71392617,\r\n              8330637.36160211\r\n            ],\r\n            [\r\n              1970172.35162732,\r\n              8330664.86506513\r\n            ],\r\n            [\r\n              1969449.51455507,\r\n              8330964.5043006\r\n            ],\r\n            [\r\n              1969061.11039917,\r\n              8331125.47578662\r\n            ],\r\n            [\r\n              1968674.49082793,\r\n              8331285.68370755\r\n            ],\r\n            [\r\n              1967945.57445145,\r\n              8331587.67011885\r\n            ],\r\n            [\r\n              1967520.69281404,\r\n              8331374.94560314\r\n            ],\r\n            [\r\n              1967358.08625612,\r\n              8331293.52593569\r\n            ],\r\n            [\r\n              1966982.31712117,\r\n              8331447.26451804\r\n            ],\r\n            [\r\n              1965840.38099043,\r\n              8331914.31496273\r\n            ],\r\n            [\r\n              1965419.74418823,\r\n              8331657.79261124\r\n            ],\r\n            [\r\n              1964390.93101146,\r\n              8331794.38502302\r\n            ],\r\n            [\r\n              1964388.56829228,\r\n              8331794.46079753\r\n            ],\r\n            [\r\n              1964124.44037247,\r\n              8331802.98318285\r\n            ],\r\n            [\r\n              1963396.62984105,\r\n              8331826.41067366\r\n            ],\r\n            [\r\n              1963393.69014721,\r\n              8331826.50565688\r\n            ],\r\n            [\r\n              1963378.9792018,\r\n              8331826.97568745\r\n            ],\r\n            [\r\n              1963313.20407506,\r\n              8331829.08894182\r\n            ],\r\n            [\r\n              1963262.04209,\r\n              8331838.92472469\r\n            ],\r\n            [\r\n              1963253.79334055,\r\n              8331840.51097291\r\n            ],\r\n            [\r\n              1962511.00211961,\r\n              8331969.93919442\r\n            ],\r\n            [\r\n              1962592.53255221,\r\n              8330413.15178704\r\n            ],\r\n            [\r\n              1962751.19561107,\r\n              8329819.06371186\r\n            ],\r\n            [\r\n              1962730.66703844,\r\n              8329416.25112219\r\n            ],\r\n            [\r\n              1962720.85402927,\r\n              8329223.67046314\r\n            ],\r\n            [\r\n              1962203.64015022,\r\n              8327700.5902828\r\n            ],\r\n            [\r\n              1961122.31371355,\r\n              8327511.34696558\r\n            ],\r\n            [\r\n              1960317.12598775,\r\n              8326943.62030693\r\n            ],\r\n            [\r\n              1960235.55997374,\r\n              8326849.29024672\r\n            ],\r\n            [\r\n              1960067.23034595,\r\n              8326654.61026462\r\n            ],\r\n            [\r\n              1959144.65914247,\r\n              8325587.45295265\r\n            ],\r\n            [\r\n              1958516.15924143,\r\n              8325337.88320699\r\n            ],\r\n            [\r\n              1957826.95657674,\r\n              8325326.20256071\r\n            ],\r\n            [\r\n              1956933.20700125,\r\n              8325316.06986087\r\n            ],\r\n            [\r\n              1956523.33239867,\r\n              8324896.55798818\r\n            ],\r\n            [\r\n              1956281.56500317,\r\n              8324016.88559039\r\n            ],\r\n            [\r\n              1956382.50704756,\r\n              8323513.81760412\r\n            ],\r\n            [\r\n              1956870.03128122,\r\n              8322946.99475995\r\n            ],\r\n            [\r\n              1957468.06178492,\r\n              8322469.97868314\r\n            ],\r\n            [\r\n              1957943.51310699,\r\n              8322265.7124521\r\n            ],\r\n            [\r\n              1957948.52058638,\r\n              8322263.48907415\r\n            ],\r\n            [\r\n              1958160.29193822,\r\n              8322169.41379774\r\n            ],\r\n            [\r\n              1957949.94532849,\r\n              8321637.2664622\r\n            ],\r\n            [\r\n              1957782.80108302,\r\n              8321376.21059175\r\n            ],\r\n            [\r\n              1957516.08722649,\r\n              8321069.50157226\r\n            ],\r\n            [\r\n              1957171.23330199,\r\n              8320665.89677879\r\n            ],\r\n            [\r\n              1957085.83761251,\r\n              8320554.87087979\r\n            ],\r\n            [\r\n              1956793.70679251,\r\n              8320083.48034735\r\n            ],\r\n            [\r\n              1956699.49705484,\r\n              8319983.83944638\r\n            ],\r\n            [\r\n              1956493.05043837,\r\n              8319797.52218671\r\n            ],\r\n            [\r\n              1956474.41091627,\r\n              8319780.69795386\r\n            ],\r\n            [\r\n              1956453.49582006,\r\n              8319761.82268764\r\n            ],\r\n            [\r\n              1956421.45740953,\r\n              8319732.90718544\r\n            ],\r\n            [\r\n              1956235.22294908,\r\n              8319644.02121225\r\n            ],\r\n            [\r\n              1956141.1438452,\r\n              8319599.11704621\r\n            ],\r\n            [\r\n              1956061.69052925,\r\n              8319543.88240106\r\n            ],\r\n            [\r\n              1954819.8589087,\r\n              8319518.22473578\r\n            ],\r\n            [\r\n              1953154.81297666,\r\n              8318869.88967465\r\n            ],\r\n            [\r\n              1953132.3469712,\r\n              8318861.14149738\r\n            ],\r\n            [\r\n              1953099.16643462,\r\n              8318871.47235895\r\n            ],\r\n            [\r\n              1953073.10137044,\r\n              8318881.93310847\r\n            ],\r\n            [\r\n              1953036.54889987,\r\n              8318897.13271216\r\n            ],\r\n            [\r\n              1953009.02366111,\r\n              8318910.80863895\r\n            ],\r\n            [\r\n              1952981.86225916,\r\n              8318923.68398463\r\n            ],\r\n            [\r\n              1952949.77988619,\r\n              8318942.27600207\r\n            ],\r\n            [\r\n              1952926.35906055,\r\n              8318960.14980043\r\n            ],\r\n            [\r\n              1952906.19218716,\r\n              8318969.99649452\r\n            ],\r\n            [\r\n              1952892.77414506,\r\n              8318980.77377503\r\n            ],\r\n            [\r\n              1952874.91644752,\r\n              8318999.62704209\r\n            ],\r\n            [\r\n              1952857.80520098,\r\n              8319017.2686452\r\n            ],\r\n            [\r\n              1952500.86832657,\r\n              8318975.96731053\r\n            ],\r\n            [\r\n              1951640.7136855,\r\n              8317940.55863228\r\n            ],\r\n            [\r\n              1951437.47186988,\r\n              8317894.68317226\r\n            ],\r\n            [\r\n              1951203.26682394,\r\n              8317643.73633892\r\n            ],\r\n            [\r\n              1951106.38802793,\r\n              8317330.64771345\r\n            ],\r\n            [\r\n              1949888.0322992,\r\n              8316409.4226192\r\n            ],\r\n            [\r\n              1949601.07246188,\r\n              8313757.20460829\r\n            ],\r\n            [\r\n              1949782.27938446,\r\n              8313343.12791026\r\n            ],\r\n            [\r\n              1950353.73355651,\r\n              8313105.80695933\r\n            ],\r\n            [\r\n              1950423.31011832,\r\n              8312821.55874613\r\n            ],\r\n            [\r\n              1950518.29476841,\r\n              8312433.48648083\r\n            ],\r\n            [\r\n              1950294.64184456,\r\n              8311513.27048722\r\n            ],\r\n            [\r\n              1950519.87501113,\r\n              8310959.62264423\r\n            ],\r\n            [\r\n              1950845.25325278,\r\n              8310159.67003908\r\n            ],\r\n            [\r\n              1950987.86488713,\r\n              8309615.05907167\r\n            ],\r\n            [\r\n              1951144.78507251,\r\n              8309028.72854402\r\n            ],\r\n            [\r\n              1951203.74951602,\r\n              8308815.91247326\r\n            ],\r\n            [\r\n              1951495.5970757,\r\n              8308497.56809026\r\n            ],\r\n            [\r\n              1952078.1395009,\r\n              8307865.25004133\r\n            ],\r\n            [\r\n              1952117.9578524,\r\n              8307816.62163193\r\n            ],\r\n            [\r\n              1952219.26401439,\r\n              8307692.90031684\r\n            ],\r\n            [\r\n              1952289.82526934,\r\n              8307606.72214013\r\n            ],\r\n            [\r\n              1952366.50309242,\r\n              8307513.07278484\r\n            ],\r\n            [\r\n              1952600.25568396,\r\n              8307247.31757095\r\n            ],\r\n            [\r\n              1952657.22488744,\r\n              8307172.91957467\r\n            ],\r\n            [\r\n              1952768.33843077,\r\n              8307027.30417606\r\n            ],\r\n            [\r\n              1952772.32120805,\r\n              8307022.16026391\r\n            ],\r\n            [\r\n              1952844.32218399,\r\n              8306929.18994282\r\n            ],\r\n            [\r\n              1952932.89518937,\r\n              8306812.67782008\r\n            ],\r\n            [\r\n              1952933.89173051,\r\n              8306810.31267561\r\n            ],\r\n            [\r\n              1953030.9597894,\r\n              8306553.16904879\r\n            ],\r\n            [\r\n              1953326.74440235,\r\n              8305933.55103836\r\n            ],\r\n            [\r\n              1953591.45399484,\r\n              8305765.61778775\r\n            ],\r\n            [\r\n              1953740.47748431,\r\n              8305665.39266769\r\n            ],\r\n            [\r\n              1953719.76429357,\r\n              8305418.39101006\r\n            ],\r\n            [\r\n              1953787.84657335,\r\n              8305253.37503396\r\n            ],\r\n            [\r\n              1953811.45256422,\r\n              8305196.16094362\r\n            ],\r\n            [\r\n              1954842.45164208,\r\n              8304191.14288493\r\n            ],\r\n            [\r\n              1954929.91274719,\r\n              8304105.87205035\r\n            ],\r\n            [\r\n              1954979.74503344,\r\n              8304057.28726012\r\n            ],\r\n            [\r\n              1956332.23855663,\r\n              8302738.403526\r\n            ],\r\n            [\r\n              1956820.4662843,\r\n              8302262.17523033\r\n            ],\r\n            [\r\n              1956606.48062913,\r\n              8302168.13135182\r\n            ],\r\n            [\r\n              1956760.10775772,\r\n              8302019.92305006\r\n            ],\r\n            [\r\n              1956470.22288846,\r\n              8300934.97021268\r\n            ],\r\n            [\r\n              1956263.82738397,\r\n              8300162.38099431\r\n            ],\r\n            [\r\n              1956091.51292079,\r\n              8299520.47335181\r\n            ],\r\n            [\r\n              1954742.82158999,\r\n              8299884.02915275\r\n            ],\r\n            [\r\n              1954715.7227212,\r\n              8299891.33018708\r\n            ],\r\n            [\r\n              1954646.30237166,\r\n              8299910.03780514\r\n            ],\r\n            [\r\n              1954564.56055378,\r\n              8299932.0615767\r\n            ],\r\n            [\r\n              1954562.34217425,\r\n              8299932.65878695\r\n            ],\r\n            [\r\n              1954008.7237356,\r\n              8300077.44965091\r\n            ],\r\n            [\r\n              1953481.54102035,\r\n              8300215.28640597\r\n            ],\r\n            [\r\n              1953481.26371085,\r\n              8300214.16584301\r\n            ],\r\n            [\r\n              1953343.66121847,\r\n              8299659.08256717\r\n            ],\r\n            [\r\n              1953292.43701859,\r\n              8299496.56176939\r\n            ],\r\n            [\r\n              1953125.1340556,\r\n              8298965.72510919\r\n            ],\r\n            [\r\n              1952672.39013699,\r\n              8297468.21345775\r\n            ],\r\n            [\r\n              1952213.76000342,\r\n              8297093.39353196\r\n            ],\r\n            [\r\n              1951470.53522466,\r\n              8296485.87934705\r\n            ],\r\n            [\r\n              1951087.60866831,\r\n              8296172.82664794\r\n            ],\r\n            [\r\n              1950726.94370375,\r\n              8295255.12880153\r\n            ],\r\n            [\r\n              1949915.94432782,\r\n              8293191.08143308\r\n            ],\r\n            [\r\n              1949199.54235227,\r\n              8292697.21088571\r\n            ],\r\n            [\r\n              1949174.57925269,\r\n              8292680.00277315\r\n            ],\r\n            [\r\n              1949142.53532192,\r\n              8292672.4657691\r\n            ],\r\n            [\r\n              1946771.83301595,\r\n              8292114.50766589\r\n            ],\r\n            [\r\n              1946211.2650067,\r\n              8291868.80039068\r\n            ],\r\n            [\r\n              1945196.22378437,\r\n              8291371.73059088\r\n            ],\r\n            [\r\n              1944174.6058713,\r\n              8290872.60384163\r\n            ],\r\n            [\r\n              1944194.63473798,\r\n              8290821.56097859\r\n            ],\r\n            [\r\n              1946064.88402582,\r\n              8286053.07750334\r\n            ],\r\n            [\r\n              1946332.98826767,\r\n              8285376.00779831\r\n            ],\r\n            [\r\n              1946334.61724077,\r\n              8285371.89247833\r\n            ],\r\n            [\r\n              1946500.35328015,\r\n              8284953.30695733\r\n            ],\r\n            [\r\n              1946772.79984875,\r\n              8284265.15080318\r\n            ],\r\n            [\r\n              1946961.83839663,\r\n              8283787.62690797\r\n            ],\r\n            [\r\n              1947710.88945597,\r\n              8281895.13735701\r\n            ],\r\n            [\r\n              1947895.09869084,\r\n              8281461.15716581\r\n            ],\r\n            [\r\n              1948321.09358716,\r\n              8280441.38081159\r\n            ],\r\n            [\r\n              1948361.28457973,\r\n              8280320.55020286\r\n            ],\r\n            [\r\n              1948384.78259873,\r\n              8279929.98141196\r\n            ],\r\n            [\r\n              1948575.29777599,\r\n              8276219.74122649\r\n            ],\r\n            [\r\n              1946873.89297609,\r\n              8276270.26045694\r\n            ],\r\n            [\r\n              1945835.13658141,\r\n              8276912.7464758\r\n            ],\r\n            [\r\n              1942871.17163719,\r\n              8277531.75548199\r\n            ],\r\n            [\r\n              1940934.90426443,\r\n              8276482.7755009\r\n            ],\r\n            [\r\n              1939877.336267,\r\n              8273447.91218139\r\n            ],\r\n            [\r\n              1939876.59774014,\r\n              8273445.79024638\r\n            ],\r\n            [\r\n              1939829.44295017,\r\n              8271832.97842437\r\n            ],\r\n            [\r\n              1939863.42618512,\r\n              8270380.49511809\r\n            ],\r\n            [\r\n              1940525.53840478,\r\n              8266705.60190211\r\n            ],\r\n            [\r\n              1938639.54885779,\r\n              8263734.11708912\r\n            ],\r\n            [\r\n              1935765.43934181,\r\n              8266091.00972598\r\n            ],\r\n            [\r\n              1935106.69980457,\r\n              8266660.40976619\r\n            ],\r\n            [\r\n              1935743.34980961,\r\n              8270427.02545787\r\n            ],\r\n            [\r\n              1933058.01571915,\r\n              8270986.37682369\r\n            ],\r\n            [\r\n              1932139.84571408,\r\n              8274836.27512531\r\n            ],\r\n            [\r\n              1931828.96126131,\r\n              8275616.06604324\r\n            ],\r\n            [\r\n              1929876.94107328,\r\n              8276375.87128206\r\n            ],\r\n            [\r\n              1927073.03147527,\r\n              8275593.91999683\r\n            ],\r\n            [\r\n              1926184.99374336,\r\n              8276530.63894804\r\n            ],\r\n            [\r\n              1925951.5021447,\r\n              8276691.90064886\r\n            ],\r\n            [\r\n              1924787.71685955,\r\n              8277465.6771404\r\n            ],\r\n            [\r\n              1922169.26376883,\r\n              8278482.6685233\r\n            ],\r\n            [\r\n              1918278.08352541,\r\n              8279997.31607364\r\n            ],\r\n            [\r\n              1917867.65125264,\r\n              8280156.9481358\r\n            ],\r\n            [\r\n              1915565.72355432,\r\n              8281051.77374375\r\n            ],\r\n            [\r\n              1913793.36739426,\r\n              8283294.57346023\r\n            ],\r\n            [\r\n              1913095.91512326,\r\n              8284181.74373175\r\n            ],\r\n            [\r\n              1913023.41343692,\r\n              8284265.50516216\r\n            ],\r\n            [\r\n              1912712.04587237,\r\n              8284667.01204431\r\n            ],\r\n            [\r\n              1911577.97100902,\r\n              8285273.26164848\r\n            ],\r\n            [\r\n              1910135.42979966,\r\n              8286034.8228487\r\n            ],\r\n            [\r\n              1909692.63812221,\r\n              8287052.75450839\r\n            ],\r\n            [\r\n              1909397.63790747,\r\n              8288547.72548744\r\n            ],\r\n            [\r\n              1909475.51034574,\r\n              8289300.19445522\r\n            ],\r\n            [\r\n              1909322.82410729,\r\n              8290316.92562423\r\n            ],\r\n            [\r\n              1909135.80578134,\r\n              8290965.64233894\r\n            ],\r\n            [\r\n              1908438.33156732,\r\n              8291910.39173402\r\n            ],\r\n            [\r\n              1908356.2386996,\r\n              8292205.72300097\r\n            ],\r\n            [\r\n              1908187.56056935,\r\n              8292812.4705515\r\n            ],\r\n            [\r\n              1906917.38611755,\r\n              8293589.10798177\r\n            ],\r\n            [\r\n              1905549.03159562,\r\n              8295014.59909122\r\n            ],\r\n            [\r\n              1903712.00868629,\r\n              8295480.58478846\r\n            ],\r\n            [\r\n              1901769.15076052,\r\n              8295988.41665519\r\n            ],\r\n            [\r\n              1901178.03056589,\r\n              8294376.55801319\r\n            ],\r\n            [\r\n              1899555.86119764,\r\n              8291662.68453788\r\n            ],\r\n            [\r\n              1898739.03965795,\r\n              8292086.88295448\r\n            ],\r\n            [\r\n              1897550.08023415,\r\n              8292822.04508165\r\n            ],\r\n            [\r\n              1897057.04817635,\r\n              8292418.23577585\r\n            ],\r\n            [\r\n              1896967.94250897,\r\n              8292278.21007935\r\n            ],\r\n            [\r\n              1896951.74522813,\r\n              8292245.51876443\r\n            ],\r\n            [\r\n              1896932.56516264,\r\n              8292197.89479173\r\n            ],\r\n            [\r\n              1896897.46749012,\r\n              8292107.6931327\r\n            ],\r\n            [\r\n              1896878.89291834,\r\n              8292052.17608182\r\n            ],\r\n            [\r\n              1896859.36528432,\r\n              8292006.53174072\r\n            ],\r\n            [\r\n              1896781.4487113,\r\n              8291910.47964465\r\n            ],\r\n            [\r\n              1896637.02459308,\r\n              8291732.4378226\r\n            ],\r\n            [\r\n              1895788.20543497,\r\n              8291092.59921673\r\n            ],\r\n            [\r\n              1894084.90550196,\r\n              8291501.12256937\r\n            ],\r\n            [\r\n              1893581.84887376,\r\n              8291642.62903418\r\n            ],\r\n            [\r\n              1892236.29293798,\r\n              8291979.00717161\r\n            ],\r\n            [\r\n              1892218.42767285,\r\n              8291983.53897515\r\n            ],\r\n            [\r\n              1892202.81729671,\r\n              8291987.49878799\r\n            ],\r\n            [\r\n              1891995.47928399,\r\n              8292040.10414005\r\n            ],\r\n            [\r\n              1891555.93073878,\r\n              8292483.6920037\r\n            ],\r\n            [\r\n              1891553.28605708,\r\n              8292486.36103923\r\n            ],\r\n            [\r\n              1891167.27396534,\r\n              8292839.79938972\r\n            ],\r\n            [\r\n              1890849.76144736,\r\n              8293130.48912138\r\n            ],\r\n            [\r\n              1888692.3654555,\r\n              8293700.30842503\r\n            ],\r\n            [\r\n              1888636.22448629,\r\n              8293715.12590327\r\n            ],\r\n            [\r\n              1888603.96230868,\r\n              8293756.59049111\r\n            ],\r\n            [\r\n              1887281.95358692,\r\n              8295452.46841156\r\n            ],\r\n            [\r\n              1887266.97738067,\r\n              8295471.67921267\r\n            ],\r\n            [\r\n              1884910.08174743,\r\n              8298517.0214475\r\n            ],\r\n            [\r\n              1884907.73187101,\r\n              8298520.05554146\r\n            ],\r\n            [\r\n              1884196.42101869,\r\n              8299429.01402999\r\n            ],\r\n            [\r\n              1883670.57866928,\r\n              8299704.04116809\r\n            ],\r\n            [\r\n              1883344.69013448,\r\n              8302523.1223886\r\n            ],\r\n            [\r\n              1883236.64342337,\r\n              8303457.56312373\r\n            ],\r\n            [\r\n              1882955.31513345,\r\n              8304103.90757729\r\n            ],\r\n            [\r\n              1882475.12117944,\r\n              8305202.57843952\r\n            ],\r\n            [\r\n              1882406.07636132,\r\n              8305524.52358116\r\n            ],\r\n            [\r\n              1882361.9478025,\r\n              8305730.2789646\r\n            ],\r\n            [\r\n              1882335.28844483,\r\n              8305854.58367586\r\n            ],\r\n            [\r\n              1882285.57471988,\r\n              8306065.9209952\r\n            ],\r\n            [\r\n              1882210.8269665,\r\n              8306260.60692724\r\n            ],\r\n            [\r\n              1882103.53465293,\r\n              8306467.93346643\r\n            ],\r\n            [\r\n              1881996.07125623,\r\n              8306631.07043092\r\n            ],\r\n            [\r\n              1881894.15874293,\r\n              8306797.29285016\r\n            ],\r\n            [\r\n              1881802.59289127,\r\n              8306972.07599509\r\n            ],\r\n            [\r\n              1881724.31983262,\r\n              8307138.80232233\r\n            ],\r\n            [\r\n              1881623.83763964,\r\n              8307357.11380811\r\n            ],\r\n            [\r\n              1881532.05520493,\r\n              8307609.65914697\r\n            ],\r\n            [\r\n              1881479.71264155,\r\n              8307740.15860358\r\n            ],\r\n            [\r\n              1881443.81339929,\r\n              8307956.10981695\r\n            ],\r\n            [\r\n              1881342.95437746,\r\n              8308302.73022836\r\n            ],\r\n            [\r\n              1881313.89447006,\r\n              8308404.92733228\r\n            ],\r\n            [\r\n              1881216.82789075,\r\n              8308646.51648262\r\n            ],\r\n            [\r\n              1881125.89748163,\r\n              8308875.01093542\r\n            ],\r\n            [\r\n              1881076.02363226,\r\n              8309014.97482512\r\n            ],\r\n            [\r\n              1881038.46508353,\r\n              8309163.07992889\r\n            ],\r\n            [\r\n              1881008.47923494,\r\n              8309317.80288128\r\n            ],\r\n            [\r\n              1880984.3168989,\r\n              8309402.58394094\r\n            ],\r\n            [\r\n              1880917.92502619,\r\n              8309577.49999934\r\n            ],\r\n            [\r\n              1880827.56309038,\r\n              8309790.22062835\r\n            ],\r\n            [\r\n              1880741.00656489,\r\n              8310023.82260016\r\n            ],\r\n            [\r\n              1880718.69965071,\r\n              8310090.67792491\r\n            ],\r\n            [\r\n              1880651.71446452,\r\n              8310291.42225854\r\n            ],\r\n            [\r\n              1880593.66655755,\r\n              8310472.96464569\r\n            ],\r\n            [\r\n              1880545.04663031,\r\n              8310652.03000847\r\n            ],\r\n            [\r\n              1880491.63970217,\r\n              8310826.81011885\r\n            ],\r\n            [\r\n              1880428.03824492,\r\n              8311006.06548354\r\n            ],\r\n            [\r\n              1880382.16729908,\r\n              8311089.94568455\r\n            ],\r\n            [\r\n              1880310.63145298,\r\n              8311297.33921237\r\n            ],\r\n            [\r\n              1880277.71174439,\r\n              8311376.71835148\r\n            ],\r\n            [\r\n              1880196.2016867,\r\n              8311511.57655902\r\n            ],\r\n            [\r\n              1880061.7625775,\r\n              8311734.36105974\r\n            ],\r\n            [\r\n              1879951.75853779,\r\n              8311861.27863075\r\n            ],\r\n            [\r\n              1879936.44253518,\r\n              8311961.77457272\r\n            ],\r\n            [\r\n              1879875.7261898,\r\n              8312121.27077109\r\n            ],\r\n            [\r\n              1879841.19521436,\r\n              8312230.29281455\r\n            ],\r\n            [\r\n              1879842.61524356,\r\n              8312313.21060628\r\n            ],\r\n            [\r\n              1879797.15372102,\r\n              8312450.77060074\r\n            ],\r\n            [\r\n              1879785.98229206,\r\n              8312484.55513474\r\n            ],\r\n            [\r\n              1879745.7666811,\r\n              8312589.87694058\r\n            ],\r\n            [\r\n              1879715.64855837,\r\n              8312674.59408719\r\n            ],\r\n            [\r\n              1879688.96728454,\r\n              8312744.67786481\r\n            ],\r\n            [\r\n              1879663.7020449,\r\n              8312845.5475474\r\n            ],\r\n            [\r\n              1879643.77298328,\r\n              8312921.89678564\r\n            ],\r\n            [\r\n              1879628.39134097,\r\n              8312974.13171505\r\n            ],\r\n            [\r\n              1879613.12261161,\r\n              8313040.96992528\r\n            ],\r\n            [\r\n              1879596.03239368,\r\n              8313127.96331106\r\n            ],\r\n            [\r\n              1879585.02443718,\r\n              8313237.02105939\r\n            ],\r\n            [\r\n              1879572.12213055,\r\n              8313408.48338156\r\n            ],\r\n            [\r\n              1879555.47041213,\r\n              8313501.40010109\r\n            ],\r\n            [\r\n              1879524.67302077,\r\n              8313653.6521479\r\n            ],\r\n            [\r\n              1879493.18678654,\r\n              8313767.2149817\r\n            ],\r\n            [\r\n              1879460.46713989,\r\n              8313874.07704802\r\n            ],\r\n            [\r\n              1879429.06121918,\r\n              8313998.30305473\r\n            ],\r\n            [\r\n              1879416.36381296,\r\n              8314040.65173787\r\n            ],\r\n            [\r\n              1879409.53698474,\r\n              8314077.03303618\r\n            ],\r\n            [\r\n              1879405.83967136,\r\n              8314109.8359181\r\n            ],\r\n            [\r\n              1879386.94473824,\r\n              8314167.23468818\r\n            ],\r\n            [\r\n              1879375.44138835,\r\n              8314211.15702517\r\n            ],\r\n            [\r\n              1879350.30351142,\r\n              8314277.68394061\r\n            ],\r\n            [\r\n              1879339.53012076,\r\n              8314313.70023405\r\n            ],\r\n            [\r\n              1879324.08783111,\r\n              8314358.43973553\r\n            ],\r\n            [\r\n              1879317.61241387,\r\n              8314388.89857078\r\n            ],\r\n            [\r\n              1879306.85428473,\r\n              8314479.41328287\r\n            ],\r\n            [\r\n              1879287.45618352,\r\n              8314575.12300168\r\n            ],\r\n            [\r\n              1879269.88783741,\r\n              8314651.87135642\r\n            ],\r\n            [\r\n              1879261.0492247,\r\n              8314735.66036545\r\n            ],\r\n            [\r\n              1879268.41161671,\r\n              8314822.88187829\r\n            ],\r\n            [\r\n              1879269.15011656,\r\n              8314868.68770636\r\n            ],\r\n            [\r\n              1879271.77210593,\r\n              8314902.63457011\r\n            ],\r\n            [\r\n              1879274.85368433,\r\n              8314945.26287468\r\n            ],\r\n            [\r\n              1879282.79476873,\r\n              8315004.44319916\r\n            ],\r\n            [\r\n              1879284.55506772,\r\n              8315028.5230654\r\n            ],\r\n            [\r\n              1879279.97395347,\r\n              8315049.09485157\r\n            ],\r\n            [\r\n              1879271.10624007,\r\n              8315076.41162178\r\n            ],\r\n            [\r\n              1879261.88062366,\r\n              8315108.47072342\r\n            ],\r\n            [\r\n              1879243.21515385,\r\n              8315144.5500516\r\n            ],\r\n            [\r\n              1879230.36275,\r\n              8315166.76138018\r\n            ],\r\n            [\r\n              1879206.88800684,\r\n              8315192.60826066\r\n            ],\r\n            [\r\n              1879195.94232,\r\n              8315206.11914019\r\n            ],\r\n            [\r\n              1879164.19404536,\r\n              8315233.60806415\r\n            ],\r\n            [\r\n              1879134.37804191,\r\n              8315255.94400121\r\n            ],\r\n            [\r\n              1879110.07739952,\r\n              8315277.06096185\r\n            ],\r\n            [\r\n              1879090.53478536,\r\n              8315301.29730511\r\n            ],\r\n            [\r\n              1879072.55633207,\r\n              8315323.94148887\r\n            ],\r\n            [\r\n              1879053.13279473,\r\n              8315364.37455239\r\n            ],\r\n            [\r\n              1879033.67150314,\r\n              8315399.66654368\r\n            ],\r\n            [\r\n              1879000.60536117,\r\n              8315461.92347059\r\n            ],\r\n            [\r\n              1878972.79688417,\r\n              8315541.91442925\r\n            ],\r\n            [\r\n              1878970.57071435,\r\n              8315552.52353376\r\n            ],\r\n            [\r\n              1878938.82328881,\r\n              8315703.89192199\r\n            ],\r\n            [\r\n              1878937.36924063,\r\n              8315710.82957162\r\n            ],\r\n            [\r\n              1878925.08209949,\r\n              8315742.90438283\r\n            ],\r\n            [\r\n              1878909.89149552,\r\n              8315782.5617265\r\n            ],\r\n            [\r\n              1878868.48442289,\r\n              8315890.66133252\r\n            ],\r\n            [\r\n              1878830.90128108,\r\n              8315982.57566967\r\n            ],\r\n            [\r\n              1878799.909794,\r\n              8316059.43130919\r\n            ],\r\n            [\r\n              1878778.62629065,\r\n              8316115.28578431\r\n            ],\r\n            [\r\n              1878760.85133393,\r\n              8316165.57947076\r\n            ],\r\n            [\r\n              1878737.48809245,\r\n              8316207.22794919\r\n            ],\r\n            [\r\n              1878724.20780672,\r\n              8316225.49830146\r\n            ],\r\n            [\r\n              1878704.27211965,\r\n              8316250.53137167\r\n            ],\r\n            [\r\n              1878672.27595618,\r\n              8316298.56205569\r\n            ],\r\n            [\r\n              1878641.07965036,\r\n              8316348.16910615\r\n            ],\r\n            [\r\n              1878618.49275651,\r\n              8316388.23217833\r\n            ],\r\n            [\r\n              1878603.28136851,\r\n              8316412.04544778\r\n            ],\r\n            [\r\n              1878585.43946638,\r\n              8316453.65103251\r\n            ],\r\n            [\r\n              1878574.69180768,\r\n              8316494.02189647\r\n            ],\r\n            [\r\n              1878552.60270327,\r\n              8316547.90954761\r\n            ],\r\n            [\r\n              1878537.05584032,\r\n              8316580.0203853\r\n            ],\r\n            [\r\n              1878523.39333657,\r\n              8316599.87053312\r\n            ],\r\n            [\r\n              1878514.4188935,\r\n              8316613.36885036\r\n            ],\r\n            [\r\n              1878510.245714,\r\n              8316635.91329128\r\n            ],\r\n            [\r\n              1878495.46721346,\r\n              8316718.18926365\r\n            ],\r\n            [\r\n              1878434.4417423,\r\n              8316843.07446162\r\n            ],\r\n            [\r\n              1878397.63942272,\r\n              8316935.38899871\r\n            ],\r\n            [\r\n              1878366.38070625,\r\n              8317030.42716603\r\n            ],\r\n            [\r\n              1878358.94663576,\r\n              8317092.50504825\r\n            ],\r\n            [\r\n              1878358.5682591,\r\n              8317153.1519817\r\n            ],\r\n            [\r\n              1878358.40597343,\r\n              8317179.42056076\r\n            ],\r\n            [\r\n              1878357.69403249,\r\n              8317243.02752528\r\n            ],\r\n            [\r\n              1878328.67527584,\r\n              8317308.44912896\r\n            ],\r\n            [\r\n              1878327.80674846,\r\n              8317310.40792567\r\n            ],\r\n            [\r\n              1878320.37533074,\r\n              8317319.5498144\r\n            ],\r\n            [\r\n              1878326.96430958,\r\n              8317356.63804398\r\n            ],\r\n            [\r\n              1878337.4688645,\r\n              8317389.743476\r\n            ],\r\n            [\r\n              1878338.66780239,\r\n              8317445.04256385\r\n            ],\r\n            [\r\n              1878332.35873631,\r\n              8317499.21625392\r\n            ],\r\n            [\r\n              1878319.86675476,\r\n              8317517.47996148\r\n            ],\r\n            [\r\n              1878294.75528312,\r\n              8317537.02430462\r\n            ],\r\n            [\r\n              1878264.18196384,\r\n              8317565.29515424\r\n            ],\r\n            [\r\n              1878251.7969944,\r\n              8317598.17965606\r\n            ],\r\n            [\r\n              1878248.01355972,\r\n              8317609.40342918\r\n            ],\r\n            [\r\n              1878230.17076959,\r\n              8317662.3382077\r\n            ],\r\n            [\r\n              1878217.41194544,\r\n              8317697.99227241\r\n            ],\r\n            [\r\n              1878216.20609022,\r\n              8317729.00739841\r\n            ],\r\n            [\r\n              1878216.0161743,\r\n              8317733.88810711\r\n            ],\r\n            [\r\n              1878215.75211419,\r\n              8317740.67224428\r\n            ],\r\n            [\r\n              1878218.10430524,\r\n              8317748.44329492\r\n            ],\r\n            [\r\n              1878225.05664846,\r\n              8317771.41927615\r\n            ],\r\n            [\r\n              1878264.51343133,\r\n              8317891.53360207\r\n            ],\r\n            [\r\n              1878269.46511535,\r\n              8317906.61018097\r\n            ],\r\n            [\r\n              1878284.80463452,\r\n              8317969.00888981\r\n            ],\r\n            [\r\n              1878294.28484375,\r\n              8318007.57141083\r\n            ],\r\n            [\r\n              1878305.26718753,\r\n              8318051.73972667\r\n            ],\r\n            [\r\n              1878302.44007974,\r\n              8318096.40937566\r\n            ],\r\n            [\r\n              1878291.65261474,\r\n              8318185.38685963\r\n            ],\r\n            [\r\n              1878271.51342339,\r\n              8318237.29087151\r\n            ],\r\n            [\r\n              1878232.0416611,\r\n              8318343.47394283\r\n            ],\r\n            [\r\n              1878183.70381747,\r\n              8318478.5598634\r\n            ],\r\n            [\r\n              1878149.19626294,\r\n              8318562.58204753\r\n            ],\r\n            [\r\n              1878113.55719556,\r\n              8318653.72163745\r\n            ],\r\n            [\r\n              1878069.39580799,\r\n              8318767.05374271\r\n            ],\r\n            [\r\n              1878023.54628491,\r\n              8318919.12257944\r\n            ],\r\n            [\r\n              1877964.62491855,\r\n              8319119.1024478\r\n            ],\r\n            [\r\n              1877909.67138799,\r\n              8319268.47937278\r\n            ],\r\n            [\r\n              1877875.55869238,\r\n              8319353.29334081\r\n            ],\r\n            [\r\n              1877828.38228653,\r\n              8319444.16561069\r\n            ],\r\n            [\r\n              1877803.03079094,\r\n              8319526.64837522\r\n            ],\r\n            [\r\n              1877795.3609721,\r\n              8319542.57527098\r\n            ],\r\n            [\r\n              1877785.9172558,\r\n              8319572.3557593\r\n            ],\r\n            [\r\n              1877734.44436604,\r\n              8319699.61328582\r\n            ],\r\n            [\r\n              1877715.44737622,\r\n              8319751.28096409\r\n            ],\r\n            [\r\n              1877696.75829305,\r\n              8319824.69196813\r\n            ],\r\n            [\r\n              1877674.26371766,\r\n              8319908.0463135\r\n            ],\r\n            [\r\n              1877661.35564195,\r\n              8319971.48682985\r\n            ],\r\n            [\r\n              1877638.63369415,\r\n              8320039.02464465\r\n            ],\r\n            [\r\n              1877613.79493753,\r\n              8320096.7119716\r\n            ],\r\n            [\r\n              1877586.92472119,\r\n              8320150.47138304\r\n            ],\r\n            [\r\n              1877524.88848413,\r\n              8320230.4374282\r\n            ],\r\n            [\r\n              1877482.18913087,\r\n              8320282.45788817\r\n            ],\r\n            [\r\n              1877447.71280929,\r\n              8320356.08013594\r\n            ],\r\n            [\r\n              1877438.06880497,\r\n              8320372.04439916\r\n            ],\r\n            [\r\n              1877413.35464587,\r\n              8320437.65013834\r\n            ],\r\n            [\r\n              1877381.39444376,\r\n              8320550.77864112\r\n            ],\r\n            [\r\n              1877343.52583197,\r\n              8320664.01105846\r\n            ],\r\n            [\r\n              1877317.38855519,\r\n              8320769.17041204\r\n            ],\r\n            [\r\n              1877295.08514658,\r\n              8320866.36806718\r\n            ],\r\n            [\r\n              1877282.1442312,\r\n              8320927.84535169\r\n            ],\r\n            [\r\n              1877257.35188269,\r\n              8320995.66313446\r\n            ],\r\n            [\r\n              1877232.74084547,\r\n              8321062.99418631\r\n            ],\r\n            [\r\n              1877211.81632801,\r\n              8321118.65389588\r\n            ],\r\n            [\r\n              1877179.63919426,\r\n              8321216.00144973\r\n            ],\r\n            [\r\n              1877153.10141138,\r\n              8321293.48608177\r\n            ],\r\n            [\r\n              1877134.20834414,\r\n              8321353.07148433\r\n            ],\r\n            [\r\n              1877119.38243086,\r\n              8321414.32536905\r\n            ],\r\n            [\r\n              1877102.51317176,\r\n              8321484.0240511\r\n            ],\r\n            [\r\n              1877095.74568149,\r\n              8321563.21300093\r\n            ],\r\n            [\r\n              1877085.48157655,\r\n              8321674.0873222\r\n            ],\r\n            [\r\n              1877081.08531404,\r\n              8321780.92697029\r\n            ],\r\n            [\r\n              1877080.52551304,\r\n              8321879.80624153\r\n            ],\r\n            [\r\n              1877089.81042216,\r\n              8321976.55993117\r\n            ],\r\n            [\r\n              1877100.5909717,\r\n              8322039.688656\r\n            ],\r\n            [\r\n              1877109.17242123,\r\n              8322087.02206131\r\n            ],\r\n            [\r\n              1877109.81781524,\r\n              8322132.49636517\r\n            ],\r\n            [\r\n              1877098.59642003,\r\n              8322176.16078117\r\n            ],\r\n            [\r\n              1877085.847903,\r\n              8322251.48670351\r\n            ],\r\n            [\r\n              1877069.12369027,\r\n              8322324.89331422\r\n            ],\r\n            [\r\n              1877060.76069247,\r\n              8322377.01363986\r\n            ],\r\n            [\r\n              1877048.27774524,\r\n              8322454.8161224\r\n            ],\r\n            [\r\n              1877041.11077242,\r\n              8322491.26352563\r\n            ],\r\n            [\r\n              1877040.69972143,\r\n              8322532.40043587\r\n            ],\r\n            [\r\n              1877031.58878665,\r\n              8322572.0360891\r\n            ],\r\n            [\r\n              1877022.30154413,\r\n              8322592.68986656\r\n            ],\r\n            [\r\n              1876984.5390844,\r\n              8322651.96847678\r\n            ],\r\n            [\r\n              1876929.69882836,\r\n              8322700.34519857\r\n            ],\r\n            [\r\n              1876907.09368554,\r\n              8322732.98663618\r\n            ],\r\n            [\r\n              1876869.07939347,\r\n              8322764.98364249\r\n            ],\r\n            [\r\n              1876833.51388316,\r\n              8322805.26284997\r\n            ],\r\n            [\r\n              1876820.700185,\r\n              8322829.11374461\r\n            ],\r\n            [\r\n              1876809.99768951,\r\n              8322867.58165062\r\n            ],\r\n            [\r\n              1876809.44356702,\r\n              8322893.29322047\r\n            ],\r\n            [\r\n              1876796.9519006,\r\n              8322951.9461765\r\n            ],\r\n            [\r\n              1876793.85411441,\r\n              8323001.80907335\r\n            ],\r\n            [\r\n              1876780.92909258,\r\n              8323099.2274281\r\n            ],\r\n            [\r\n              1876774.95982347,\r\n              8323180.3671432\r\n            ],\r\n            [\r\n              1876769.01448409,\r\n              8323220.76475885\r\n            ],\r\n            [\r\n              1876760.66858214,\r\n              8323301.1363205\r\n            ],\r\n            [\r\n              1876744.12573412,\r\n              8323434.9797495\r\n            ],\r\n            [\r\n              1876743.68458906,\r\n              8323472.95744105\r\n            ],\r\n            [\r\n              1876734.37880971,\r\n              8323534.75503603\r\n            ],\r\n            [\r\n              1876726.45985335,\r\n              8323618.67546129\r\n            ],\r\n            [\r\n              1876718.52113791,\r\n              8323700.23377419\r\n            ],\r\n            [\r\n              1876707.09622369,\r\n              8323789.34297228\r\n            ],\r\n            [\r\n              1876694.6759467,\r\n              8323856.30842342\r\n            ],\r\n            [\r\n              1876681.50975043,\r\n              8323928.02759892\r\n            ],\r\n            [\r\n              1876674.67363105,\r\n              8323957.75709959\r\n            ],\r\n            [\r\n              1876649.63613581,\r\n              8324026.43122536\r\n            ],\r\n            [\r\n              1876614.71479931,\r\n              8324094.7963935\r\n            ],\r\n            [\r\n              1876603.55843512,\r\n              8324127.73031582\r\n            ],\r\n            [\r\n              1876573.5006032,\r\n              8324166.38106663\r\n            ],\r\n            [\r\n              1876551.41486294,\r\n              8324213.27150944\r\n            ],\r\n            [\r\n              1876539.88874699,\r\n              8324248.58581989\r\n            ],\r\n            [\r\n              1876516.34625963,\r\n              8324308.93341669\r\n            ],\r\n            [\r\n              1876494.51099142,\r\n              8324383.11122241\r\n            ],\r\n            [\r\n              1876488.75665111,\r\n              8324444.8793809\r\n            ],\r\n            [\r\n              1876494.73230294,\r\n              8324493.47841571\r\n            ],\r\n            [\r\n              1876490.44632181,\r\n              8324542.96673553\r\n            ],\r\n            [\r\n              1876492.20179441,\r\n              8324562.33445196\r\n            ],\r\n            [\r\n              1876491.41840056,\r\n              8324606.25385322\r\n            ],\r\n            [\r\n              1876480.60153176,\r\n              8324632.86039011\r\n            ],\r\n            [\r\n              1876453.60046279,\r\n              8324703.53376981\r\n            ],\r\n            [\r\n              1876431.1605908,\r\n              8324755.17078967\r\n            ],\r\n            [\r\n              1876413.35119136,\r\n              8324794.89817358\r\n            ],\r\n            [\r\n              1876400.21821729,\r\n              8324827.46048041\r\n            ],\r\n            [\r\n              1876381.95871859,\r\n              8324860.86225908\r\n            ],\r\n            [\r\n              1876351.16154292,\r\n              8324905.85634697\r\n            ],\r\n            [\r\n              1876321.51807062,\r\n              8324947.27744783\r\n            ],\r\n            [\r\n              1876287.6056224,\r\n              8324997.04359135\r\n            ],\r\n            [\r\n              1876248.02980419,\r\n              8325075.74738964\r\n            ],\r\n            [\r\n              1876217.05629847,\r\n              8325144.8768221\r\n            ],\r\n            [\r\n              1876206.69084978,\r\n              8325220.9338377\r\n            ],\r\n            [\r\n              1876204.11929926,\r\n              8325285.83980327\r\n            ],\r\n            [\r\n              1876202.30698023,\r\n              8325346.78767294\r\n            ],\r\n            [\r\n              1876212.01965916,\r\n              8325415.53686836\r\n            ],\r\n            [\r\n              1876229.59278977,\r\n              8325459.16497088\r\n            ],\r\n            [\r\n              1876238.78641608,\r\n              8325484.7904663\r\n            ],\r\n            [\r\n              1876242.65426831,\r\n              8325495.57363581\r\n            ],\r\n            [\r\n              1876246.01655349,\r\n              8325513.65020415\r\n            ],\r\n            [\r\n              1876250.14479353,\r\n              8325533.95663595\r\n            ],\r\n            [\r\n              1876263.15113191,\r\n              8325600.34052705\r\n            ],\r\n            [\r\n              1876261.13731983,\r\n              8325630.20205796\r\n            ],\r\n            [\r\n              1876256.13787542,\r\n              8325704.34460455\r\n            ],\r\n            [\r\n              1876262.78195361,\r\n              8325739.88867676\r\n            ],\r\n            [\r\n              1876267.82913402,\r\n              8325773.4714993\r\n            ],\r\n            [\r\n              1876269.09924696,\r\n              8325826.08559593\r\n            ],\r\n            [\r\n              1876268.81744303,\r\n              8325881.87476299\r\n            ],\r\n            [\r\n              1876256.00825376,\r\n              8325950.04973385\r\n            ],\r\n            [\r\n              1876239.02400265,\r\n              8325993.73514363\r\n            ],\r\n            [\r\n              1876220.92612276,\r\n              8326045.73522755\r\n            ],\r\n            [\r\n              1876209.29425659,\r\n              8326069.98053547\r\n            ],\r\n            [\r\n              1876194.247772,\r\n              8326109.68630404\r\n            ],\r\n            [\r\n              1876176.34268194,\r\n              8326139.13615377\r\n            ],\r\n            [\r\n              1876135.77319081,\r\n              8326195.70279708\r\n            ],\r\n            [\r\n              1876108.79806703,\r\n              8326227.21348859\r\n            ],\r\n            [\r\n              1876075.83083884,\r\n              8326251.66207156\r\n            ],\r\n            [\r\n              1876019.29318345,\r\n              8326290.95896747\r\n            ],\r\n            [\r\n              1875952.8522919,\r\n              8326327.58688101\r\n            ],\r\n            [\r\n              1875920.75559513,\r\n              8326361.12134539\r\n            ],\r\n            [\r\n              1875879.68261089,\r\n              8326406.6109057\r\n            ],\r\n            [\r\n              1875851.75054212,\r\n              8326463.45395169\r\n            ],\r\n            [\r\n              1875834.72212769,\r\n              8326502.78859805\r\n            ],\r\n            [\r\n              1875817.3840866,\r\n              8326551.61705497\r\n            ],\r\n            [\r\n              1875777.64492082,\r\n              8326613.71905394\r\n            ],\r\n            [\r\n              1875755.49299674,\r\n              8326654.28445104\r\n            ],\r\n            [\r\n              1875724.74470813,\r\n              8326706.01262526\r\n            ],\r\n            [\r\n              1875681.81773306,\r\n              8326765.37312166\r\n            ],\r\n            [\r\n              1875647.81529058,\r\n              8326806.44434814\r\n            ],\r\n            [\r\n              1875622.02116796,\r\n              8326837.94316926\r\n            ],\r\n            [\r\n              1875606.39487593,\r\n              8326857.8773438\r\n            ],\r\n            [\r\n              1875569.80368563,\r\n              8326918.3677572\r\n            ],\r\n            [\r\n              1875534.42142801,\r\n              8326982.01149848\r\n            ],\r\n            [\r\n              1875520.9136489,\r\n              8327018.14661822\r\n            ],\r\n            [\r\n              1875505.01963384,\r\n              8327051.92818831\r\n            ],\r\n            [\r\n              1875488.5499923,\r\n              8327109.85905115\r\n            ],\r\n            [\r\n              1875473.80023026,\r\n              8327139.28115268\r\n            ],\r\n            [\r\n              1875465.02207306,\r\n              8327173.78681943\r\n            ],\r\n            [\r\n              1875454.36397132,\r\n              8327218.60113707\r\n            ],\r\n            [\r\n              1875440.98330238,\r\n              8327268.59019035\r\n            ],\r\n            [\r\n              1875418.21880117,\r\n              8327329.74440301\r\n            ],\r\n            [\r\n              1875401.29100825,\r\n              8327380.55808519\r\n            ],\r\n            [\r\n              1875383.60388445,\r\n              8327434.93796382\r\n            ],\r\n            [\r\n              1875356.08465756,\r\n              8327494.55279379\r\n            ],\r\n            [\r\n              1875338.15674944,\r\n              8327522.42301099\r\n            ],\r\n            [\r\n              1875312.49901367,\r\n              8327569.35497822\r\n            ],\r\n            [\r\n              1875282.5101894,\r\n              8327618.31281677\r\n            ],\r\n            [\r\n              1875245.7876866,\r\n              8327665.35134396\r\n            ],\r\n            [\r\n              1875225.56225555,\r\n              8327701.54845582\r\n            ],\r\n            [\r\n              1875195.09837304,\r\n              8327741.40795008\r\n            ],\r\n            [\r\n              1875176.49573337,\r\n              8327782.34354314\r\n            ],\r\n            [\r\n              1875163.67475041,\r\n              8327806.60192493\r\n            ],\r\n            [\r\n              1875139.52757737,\r\n              8327846.39799961\r\n            ],\r\n            [\r\n              1875122.29392246,\r\n              8327863.18273188\r\n            ],\r\n            [\r\n              1875069.39918092,\r\n              8327913.93290579\r\n            ],\r\n            [\r\n              1875030.90832131,\r\n              8327940.4078071\r\n            ],\r\n            [\r\n              1874994.30621546,\r\n              8327957.36485663\r\n            ],\r\n            [\r\n              1874951.02253904,\r\n              8327978.34354434\r\n            ],\r\n            [\r\n              1874914.88884279,\r\n              8328003.21198776\r\n            ],\r\n            [\r\n              1874886.63799007,\r\n              8328026.03304129\r\n            ],\r\n            [\r\n              1874854.9769969,\r\n              8328065.1054505\r\n            ],\r\n            [\r\n              1874839.53467592,\r\n              8328105.61910404\r\n            ],\r\n            [\r\n              1874832.45357515,\r\n              8328153.96863396\r\n            ],\r\n            [\r\n              1874831.79824419,\r\n              8328164.65698756\r\n            ],\r\n            [\r\n              1874830.72978252,\r\n              8328182.08537268\r\n            ],\r\n            [\r\n              1874821.88757629,\r\n              8328209.87260838\r\n            ],\r\n            [\r\n              1874825.9892169,\r\n              8328226.45613784\r\n            ],\r\n            [\r\n              1874823.28256597,\r\n              8328233.21016731\r\n            ],\r\n            [\r\n              1874823.36868157,\r\n              8328242.8027307\r\n            ],\r\n            [\r\n              1874823.39894831,\r\n              8328246.27221443\r\n            ],\r\n            [\r\n              1874822.60653503,\r\n              8328249.17228704\r\n            ],\r\n            [\r\n              1874816.89196611,\r\n              8328270.08004381\r\n            ],\r\n            [\r\n              1874807.66923636,\r\n              8328299.45246609\r\n            ],\r\n            [\r\n              1874799.95537244,\r\n              8328312.26235352\r\n            ],\r\n            [\r\n              1874792.94029187,\r\n              8328331.64663274\r\n            ],\r\n            [\r\n              1874778.17292596,\r\n              8328359.88313836\r\n            ],\r\n            [\r\n              1874767.38847526,\r\n              8328391.24576103\r\n            ],\r\n            [\r\n              1874762.81837027,\r\n              8328410.6832624\r\n            ],\r\n            [\r\n              1874760.57866966,\r\n              8328425.35364228\r\n            ],\r\n            [\r\n              1874755.31124843,\r\n              8328455.08486569\r\n            ],\r\n            [\r\n              1874753.56212628,\r\n              8328480.43397108\r\n            ],\r\n            [\r\n              1874752.82250412,\r\n              8328486.37558513\r\n            ],\r\n            [\r\n              1874750.18136821,\r\n              8328500.25083896\r\n            ],\r\n            [\r\n              1874743.27426406,\r\n              8328523.2700717\r\n            ],\r\n            [\r\n              1874733.15714107,\r\n              8328541.17700291\r\n            ],\r\n            [\r\n              1874718.39317607,\r\n              8328569.4162097\r\n            ],\r\n            [\r\n              1874683.95669286,\r\n              8328607.72877598\r\n            ],\r\n            [\r\n              1874645.31317518,\r\n              8328662.30611232\r\n            ],\r\n            [\r\n              1874607.09983326,\r\n              8328720.44639181\r\n            ],\r\n            [\r\n              1874586.10366152,\r\n              8328759.42423499\r\n            ],\r\n            [\r\n              1874575.6918458,\r\n              8328788.41746154\r\n            ],\r\n            [\r\n              1874573.51734913,\r\n              8328810.60356161\r\n            ],\r\n            [\r\n              1874562.60894505,\r\n              8328828.51509738\r\n            ],\r\n            [\r\n              1874566.75227223,\r\n              8328849.85286037\r\n            ],\r\n            [\r\n              1874576.40946447,\r\n              8328869.15811565\r\n            ],\r\n            [\r\n              1874594.75344677,\r\n              8328887.59628655\r\n            ],\r\n            [\r\n              1874613.53060476,\r\n              8328909.98576528\r\n            ],\r\n            [\r\n              1874632.66665289,\r\n              8328928.41307626\r\n            ],\r\n            [\r\n              1874652.99746445,\r\n              8328948.01973243\r\n            ],\r\n            [\r\n              1874674.13948583,\r\n              8328969.59649283\r\n            ],\r\n            [\r\n              1874684.15589919,\r\n              8328984.94178057\r\n            ],\r\n            [\r\n              1874687.81816075,\r\n              8328996.78364617\r\n            ],\r\n            [\r\n              1874693.54061055,\r\n              8329018.10670002\r\n            ],\r\n            [\r\n              1874695.30333373,\r\n              8329038.27799472\r\n            ],\r\n            [\r\n              1874689.65686185,\r\n              8329070.38855666\r\n            ],\r\n            [\r\n              1874681.50422767,\r\n              8329087.08843387\r\n            ],\r\n            [\r\n              1874666.68230765,\r\n              8329108.99191847\r\n            ],\r\n            [\r\n              1874621.70041439,\r\n              8329162.05256419\r\n            ],\r\n            [\r\n              1874594.686837,\r\n              8329191.1914811\r\n            ],\r\n            [\r\n              1874565.23448155,\r\n              8329212.83828234\r\n            ],\r\n            [\r\n              1874531.43534955,\r\n              8329234.52287649\r\n            ],\r\n            [\r\n              1874501.54716254,\r\n              8329251.81303783\r\n            ],\r\n            [\r\n              1874492.24306105,\r\n              8329254.97281073\r\n            ],\r\n            [\r\n              1874483.81978185,\r\n              8329257.83824065\r\n            ],\r\n            [\r\n              1874470.4168192,\r\n              8329262.38808048\r\n            ],\r\n            [\r\n              1874447.98221694,\r\n              8329273.28305655\r\n            ],\r\n            [\r\n              1874420.46707196,\r\n              8329290.15997704\r\n            ],\r\n            [\r\n              1874386.68977355,\r\n              8329314.61339045\r\n            ],\r\n            [\r\n              1874343.11701945,\r\n              8329348.65778016\r\n            ],\r\n            [\r\n              1874306.19900077,\r\n              8329375.51136403\r\n            ],\r\n            [\r\n              1874287.02834002,\r\n              8329397.45876679\r\n            ],\r\n            [\r\n              1874282.89572147,\r\n              8329422.04059301\r\n            ],\r\n            [\r\n              1874274.87204413,\r\n              8329453.38379343\r\n            ],\r\n            [\r\n              1874255.32255694,\r\n              8329477.31477937\r\n            ],\r\n            [\r\n              1874224.75608289,\r\n              8329507.28098539\r\n            ],\r\n            [\r\n              1874188.31566384,\r\n              8329543.63143963\r\n            ],\r\n            [\r\n              1874151.15378282,\r\n              8329586.72628788\r\n            ],\r\n            [\r\n              1874118.715414,\r\n              8329629.37534428\r\n            ],\r\n            [\r\n              1874089.04884728,\r\n              8329671.60540371\r\n            ],\r\n            [\r\n              1874065.52526382,\r\n              8329693.58883696\r\n            ],\r\n            [\r\n              1874037.98722912,\r\n              8329708.49009347\r\n            ],\r\n            [\r\n              1873995.48944425,\r\n              8329730.24882689\r\n            ],\r\n            [\r\n              1873976.5904565,\r\n              8329738.73464949\r\n            ],\r\n            [\r\n              1873909.2219779,\r\n              8329764.67640306\r\n            ],\r\n            [\r\n              1873836.68354003,\r\n              8329786.31835315\r\n            ],\r\n            [\r\n              1873806.35291831,\r\n              8329798.46665908\r\n            ],\r\n            [\r\n              1873761.03318366,\r\n              8329814.31366961\r\n            ],\r\n            [\r\n              1873718.22441206,\r\n              8329845.97499023\r\n            ],\r\n            [\r\n              1873670.33107887,\r\n              8329883.61626873\r\n            ],\r\n            [\r\n              1873630.8041012,\r\n              8329929.10267708\r\n            ],\r\n            [\r\n              1873626.94970478,\r\n              8329940.22362101\r\n            ],\r\n            [\r\n              1873609.25631374,\r\n              8329996.20164552\r\n            ],\r\n            [\r\n              1873576.04049308,\r\n              8330085.17794097\r\n            ],\r\n            [\r\n              1873539.36325359,\r\n              8330140.14187665\r\n            ],\r\n            [\r\n              1873491.73710613,\r\n              8330208.65872193\r\n            ],\r\n            [\r\n              1873456.07437347,\r\n              8330244.2167625\r\n            ],\r\n            [\r\n              1873439.23290651,\r\n              8330262.18074013\r\n            ],\r\n            [\r\n              1873423.13830699,\r\n              8330274.99331524\r\n            ],\r\n            [\r\n              1873351.14011552,\r\n              8330314.4365453\r\n            ],\r\n            [\r\n              1873309.19145706,\r\n              8330354.80161375\r\n            ],\r\n            [\r\n              1873277.2451406,\r\n              8330408.53311482\r\n            ],\r\n            [\r\n              1873245.60318791,\r\n              8330451.97094363\r\n            ],\r\n            [\r\n              1873207.95946033,\r\n              8330487.14794363\r\n            ],\r\n            [\r\n              1873177.40735636,\r\n              8330519.88511544\r\n            ],\r\n            [\r\n              1873132.23878539,\r\n              8330553.54646676\r\n            ],\r\n            [\r\n              1873077.5984904,\r\n              8330589.26583529\r\n            ],\r\n            [\r\n              1873028.83859564,\r\n              8330619.39658962\r\n            ],\r\n            [\r\n              1872971.54574346,\r\n              8330667.81434638\r\n            ],\r\n            [\r\n              1872890.28122563,\r\n              8330733.47206608\r\n            ],\r\n            [\r\n              1872851.03915194,\r\n              8330767.47080566\r\n            ],\r\n            [\r\n              1872836.70448934,\r\n              8330801.2517291\r\n            ],\r\n            [\r\n              1872821.9802624,\r\n              8330835.82893945\r\n            ],\r\n            [\r\n              1872816.50538986,\r\n              8330853.03180407\r\n            ],\r\n            [\r\n              1872812.48771732,\r\n              8330869.32938014\r\n            ],\r\n            [\r\n              1872810.86105335,\r\n              8330875.91643808\r\n            ],\r\n            [\r\n              1872803.92039936,\r\n              8330896.16952571\r\n            ],\r\n            [\r\n              1872787.72190498,\r\n              8330943.03383935\r\n            ],\r\n            [\r\n              1872769.34650977,\r\n              8331012.48630191\r\n            ],\r\n            [\r\n              1872732.80824922,\r\n              8331084.86914274\r\n            ],\r\n            [\r\n              1872690.94391913,\r\n              8331135.52410748\r\n            ],\r\n            [\r\n              1872653.84978346,\r\n              8331189.3099099\r\n            ],\r\n            [\r\n              1872635.16064684,\r\n              8331222.73535521\r\n            ],\r\n            [\r\n              1872616.93281322,\r\n              8331263.67576966\r\n            ],\r\n            [\r\n              1872599.28004075,\r\n              8331325.60320253\r\n            ],\r\n            [\r\n              1872591.6646042,\r\n              8331359.32678577\r\n            ],\r\n            [\r\n              1872583.51325877,\r\n              8331376.82157019\r\n            ],\r\n            [\r\n              1872569.86415538,\r\n              8331398.72127177\r\n            ],\r\n            [\r\n              1872533.7956745,\r\n              8331434.27862608\r\n            ],\r\n            [\r\n              1872503.56854118,\r\n              8331459.49400848\r\n            ],\r\n            [\r\n              1872468.97623921,\r\n              8331482.76372091\r\n            ],\r\n            [\r\n              1872432.03876809,\r\n              8331509.22190151\r\n            ],\r\n            [\r\n              1872416.75266176,\r\n              8331524.80032486\r\n            ],\r\n            [\r\n              1872404.6474138,\r\n              8331542.33168775\r\n            ],\r\n            [\r\n              1872401.63830773,\r\n              8331559.78343802\r\n            ],\r\n            [\r\n              1872391.97838173,\r\n              8331586.00467172\r\n            ],\r\n            [\r\n              1872419.40618705,\r\n              8331878.38750219\r\n            ],\r\n            [\r\n              1872436.78199931,\r\n              8331967.72734901\r\n            ],\r\n            [\r\n              1872480.58290816,\r\n              8332095.63973132\r\n            ],\r\n            [\r\n              1872574.70432881,\r\n              8332327.65805622\r\n            ],\r\n            [\r\n              1872592.49606397,\r\n              8332356.93867353\r\n            ],\r\n            [\r\n              1872623.75364516,\r\n              8332408.37814132\r\n            ],\r\n            [\r\n              1872639.8149391,\r\n              8332438.92307469\r\n            ],\r\n            [\r\n              1872657.88707791,\r\n              8332471.01006829\r\n            ],\r\n            [\r\n              1872677.14165705,\r\n              8332503.07082655\r\n            ],\r\n            [\r\n              1872697.7262324,\r\n              8332541.83699542\r\n            ],\r\n            [\r\n              1872700.97877007,\r\n              8332583.74093084\r\n            ],\r\n            [\r\n              1872700.32864624,\r\n              8332647.11215277\r\n            ],\r\n            [\r\n              1872698.40009365,\r\n              8332687.14325526\r\n            ],\r\n            [\r\n              1872692.48478843,\r\n              8332706.67055575\r\n            ],\r\n            [\r\n              1872694.7984587,\r\n              8332722.85725216\r\n            ],\r\n            [\r\n              1872679.44564584,\r\n              8332763.96821608\r\n            ],\r\n            [\r\n              1872657.99363055,\r\n              8332796.89255245\r\n            ],\r\n            [\r\n              1872641.7759712,\r\n              8332834.45833793\r\n            ],\r\n            [\r\n              1872614.39534139,\r\n              8332886.51476845\r\n            ],\r\n            [\r\n              1872597.25419281,\r\n              8332917.7609894\r\n            ],\r\n            [\r\n              1872577.71284973,\r\n              8332966.48511535\r\n            ],\r\n            [\r\n              1872573.85121258,\r\n              8332980.95302039\r\n            ],\r\n            [\r\n              1872563.04525307,\r\n              8333021.44275717\r\n            ],\r\n            [\r\n              1872543.95521832,\r\n              8333072.92961607\r\n            ],\r\n            [\r\n              1872538.33905165,\r\n              8333125.71506698\r\n            ],\r\n            [\r\n              1872525.12804785,\r\n              8333174.69982026\r\n            ],\r\n            [\r\n              1872512.01514474,\r\n              8333228.4369872\r\n            ],\r\n            [\r\n              1872495.42229167,\r\n              8333266.8024407\r\n            ],\r\n            [\r\n              1872486.75647072,\r\n              8333291.43608506\r\n            ],\r\n            [\r\n              1872480.11833934,\r\n              8333310.29847117\r\n            ],\r\n            [\r\n              1872470.24959838,\r\n              8333348.91772531\r\n            ],\r\n            [\r\n              1872468.86851995,\r\n              8333377.46051705\r\n            ],\r\n            [\r\n              1872473.21214876,\r\n              8333395.97843314\r\n            ],\r\n            [\r\n              1872486.19356125,\r\n              8333411.54784215\r\n            ],\r\n            [\r\n              1872509.03178045,\r\n              8333425.7185513\r\n            ],\r\n            [\r\n              1872529.79369527,\r\n              8333435.18112908\r\n            ],\r\n            [\r\n              1872547.90631361,\r\n              8333450.24352168\r\n            ],\r\n            [\r\n              1872559.72555985,\r\n              8333467.02334099\r\n            ],\r\n            [\r\n              1872558.00955403,\r\n              8333498.34423941\r\n            ],\r\n            [\r\n              1872556.42140363,\r\n              8333516.98936454\r\n            ],\r\n            [\r\n              1872553.07999628,\r\n              8333527.3583164\r\n            ],\r\n            [\r\n              1872555.94110911,\r\n              8333550.66308555\r\n            ],\r\n            [\r\n              1872562.52501477,\r\n              8333581.81006226\r\n            ],\r\n            [\r\n              1872564.96667964,\r\n              8333603.93489907\r\n            ],\r\n            [\r\n              1872549.67628883,\r\n              8333629.2077832\r\n            ],\r\n            [\r\n              1872526.41272347,\r\n              8333651.47880874\r\n            ],\r\n            [\r\n              1872469.86368347,\r\n              8333689.50414932\r\n            ],\r\n            [\r\n              1872436.19039853,\r\n              8333705.65737348\r\n            ],\r\n            [\r\n              1872400.83591306,\r\n              8333717.09910577\r\n            ],\r\n            [\r\n              1872376.77984294,\r\n              8333720.37685828\r\n            ],\r\n            [\r\n              1872357.47602182,\r\n              8333723.95242188\r\n            ],\r\n            [\r\n              1872340.6180931,\r\n              8333731.039593\r\n            ],\r\n            [\r\n              1872327.2371543,\r\n              8333753.1042933\r\n            ],\r\n            [\r\n              1872318.88775187,\r\n              8333769.9098788\r\n            ],\r\n            [\r\n              1872324.94341672,\r\n              8333794.73555601\r\n            ],\r\n            [\r\n              1872346.25150442,\r\n              8333811.31565366\r\n            ],\r\n            [\r\n              1872371.47320426,\r\n              8333825.83286793\r\n            ],\r\n            [\r\n              1872391.09162092,\r\n              8333837.3005978\r\n            ],\r\n            [\r\n              1872406.38550818,\r\n              8333850.04682199\r\n            ],\r\n            [\r\n              1872413.63510212,\r\n              8333875.24036519\r\n            ],\r\n            [\r\n              1872416.84470483,\r\n              8333915.17193979\r\n            ],\r\n            [\r\n              1872413.11649891,\r\n              8333944.95296069\r\n            ],\r\n            [\r\n              1872393.83453458,\r\n              8333987.34076958\r\n            ],\r\n            [\r\n              1872385.84003276,\r\n              8334021.17524428\r\n            ],\r\n            [\r\n              1872365.05492912,\r\n              8334048.54535973\r\n            ],\r\n            [\r\n              1872333.13565901,\r\n              8334072.98019508\r\n            ],\r\n            [\r\n              1872319.696214,\r\n              8334092.27346918\r\n            ],\r\n            [\r\n              1872316.80043921,\r\n              8334124.0171719\r\n            ],\r\n            [\r\n              1872316.36725923,\r\n              8334160.06777181\r\n            ],\r\n            [\r\n              1872321.62407769,\r\n              8334184.51297571\r\n            ],\r\n            [\r\n              1872332.58730305,\r\n              8334198.14413391\r\n            ],\r\n            [\r\n              1872341.33888265,\r\n              8334219.34628114\r\n            ],\r\n            [\r\n              1872343.77817633,\r\n              8334241.47327195\r\n            ],\r\n            [\r\n              1872340.2079268,\r\n              8334278.77873298\r\n            ],\r\n            [\r\n              1872333.13716878,\r\n              8334318.93113743\r\n            ],\r\n            [\r\n              1872316.01622672,\r\n              8334351.37304921\r\n            ],\r\n            [\r\n              1872294.01155519,\r\n              8334377.18215796\r\n            ],\r\n            [\r\n              1872269.28783823,\r\n              8334405.42985529\r\n            ],\r\n            [\r\n              1872249.24401064,\r\n              8334430.40730514\r\n            ],\r\n            [\r\n              1872240.11894572,\r\n              8334448.02836301\r\n            ],\r\n            [\r\n              1872229.10893633,\r\n              8334470.04446277\r\n            ],\r\n            [\r\n              1872221.96906235,\r\n              8334488.01616129\r\n            ],\r\n            [\r\n              1872222.72026792,\r\n              8334505.03101303\r\n            ],\r\n            [\r\n              1872215.80345174,\r\n              8334514.68491268\r\n            ],\r\n            [\r\n              1872209.40620783,\r\n              8334530.26619571\r\n            ],\r\n            [\r\n              1872194.75356922,\r\n              8334548.3958516\r\n            ],\r\n            [\r\n              1872184.19649638,\r\n              8334573.1728\r\n            ],\r\n            [\r\n              1872177.6253303,\r\n              8334599.45399143\r\n            ],\r\n            [\r\n              1872165.02620608,\r\n              8334640.12247809\r\n            ],\r\n            [\r\n              1872162.66439324,\r\n              8334659.58037282\r\n            ],\r\n            [\r\n              1872150.75830507,\r\n              8334714.48975913\r\n            ],\r\n            [\r\n              1872139.23321653,\r\n              8334768.59894472\r\n            ],\r\n            [\r\n              1872129.1199608,\r\n              8334814.75724113\r\n            ],\r\n            [\r\n              1872107.61816685,\r\n              8334883.73558741\r\n            ],\r\n            [\r\n              1872086.00968475,\r\n              8334947.56484191\r\n            ],\r\n            [\r\n              1872069.09394826,\r\n              8334989.90304101\r\n            ],\r\n            [\r\n              1872047.53820532,\r\n              8335018.48235331\r\n            ],\r\n            [\r\n              1872023.61453753,\r\n              8335066.12264577\r\n            ],\r\n            [\r\n              1871995.26662696,\r\n              8335129.30349839\r\n            ],\r\n            [\r\n              1871969.2492224,\r\n              8335209.46690246\r\n            ],\r\n            [\r\n              1871956.77967344,\r\n              8335256.46990554\r\n            ],\r\n            [\r\n              1871948.89155718,\r\n              8335314.46817866\r\n            ],\r\n            [\r\n              1871953.62270696,\r\n              8335351.60518373\r\n            ],\r\n            [\r\n              1871961.51590007,\r\n              8335388.67375191\r\n            ],\r\n            [\r\n              1871959.01367436,\r\n              8335439.42889828\r\n            ],\r\n            [\r\n              1871953.85111253,\r\n              8335476.37588677\r\n            ],\r\n            [\r\n              1871942.5591335,\r\n              8335503.94671579\r\n            ],\r\n            [\r\n              1871929.42991273,\r\n              8335519.27754873\r\n            ],\r\n            [\r\n              1871912.20586122,\r\n              8335527.95800563\r\n            ],\r\n            [\r\n              1871886.05065498,\r\n              8335525.73586847\r\n            ],\r\n            [\r\n              1871862.10238316,\r\n              8335515.54033136\r\n            ],\r\n            [\r\n              1871809.24091044,\r\n              8335503.57821862\r\n            ],\r\n            [\r\n              1871742.8927817,\r\n              8335489.92110429\r\n            ],\r\n            [\r\n              1871706.91143613,\r\n              8335490.67697163\r\n            ],\r\n            [\r\n              1871657.28715269,\r\n              8335501.22459904\r\n            ],\r\n            [\r\n              1871616.16666555,\r\n              8335521.10141484\r\n            ],\r\n            [\r\n              1871578.31277878,\r\n              8335546.05970971\r\n            ],\r\n            [\r\n              1871524.51204514,\r\n              8335584.02945926\r\n            ],\r\n            [\r\n              1871486.81686165,\r\n              8335616.51037697\r\n            ],\r\n            [\r\n              1871459.44094762,\r\n              8335650.75599169\r\n            ],\r\n            [\r\n              1871430.27894056,\r\n              8335694.15042569\r\n            ],\r\n            [\r\n              1871378.39386835,\r\n              8335786.34672035\r\n            ],\r\n            [\r\n              1871353.17725395,\r\n              8335829.26386165\r\n            ],\r\n            [\r\n              1871331.749236,\r\n              8335845.16233804\r\n            ],\r\n            [\r\n              1871307.5112426,\r\n              8335859.13740143\r\n            ],\r\n            [\r\n              1871279.37785574,\r\n              8335875.9709001\r\n            ],\r\n            [\r\n              1871261.08176777,\r\n              8335890.21847264\r\n            ],\r\n            [\r\n              1871246.70593257,\r\n              8335902.7998739\r\n            ],\r\n            [\r\n              1871196.96934909,\r\n              8335946.2255029\r\n            ],\r\n            [\r\n              1871156.47657979,\r\n              8335977.57947168\r\n            ],\r\n            [\r\n              1871141.35957919,\r\n              8335992.55151005\r\n            ],\r\n            [\r\n              1871140.64189417,\r\n              8335996.1306977\r\n            ],\r\n            [\r\n              1871111.86414777,\r\n              8336039.12236803\r\n            ],\r\n            [\r\n              1871100.73296724,\r\n              8336055.59518444\r\n            ],\r\n            [\r\n              1871103.1216581,\r\n              8336075.35539749\r\n            ],\r\n            [\r\n              1871103.04633139,\r\n              8336090.80888452\r\n            ],\r\n            [\r\n              1871098.79309479,\r\n              8336114.66822788\r\n            ],\r\n            [\r\n              1871102.3843175,\r\n              8336135.19250339\r\n            ],\r\n            [\r\n              1871107.47550053,\r\n              8336151.72149955\r\n            ],\r\n            [\r\n              1871089.82384185,\r\n              8336197.25579504\r\n            ],\r\n            [\r\n              1871070.11708768,\r\n              8336238.86690445\r\n            ],\r\n            [\r\n              1871022.11628872,\r\n              8336251.75449312\r\n            ],\r\n            [\r\n              1870906.92252545,\r\n              8336286.65261311\r\n            ],\r\n            [\r\n              1870876.6863789,\r\n              8336297.58130055\r\n            ],\r\n            [\r\n              1870745.97632943,\r\n              8336347.45366189\r\n            ],\r\n            [\r\n              1870579.34442855,\r\n              8336401.24123256\r\n            ],\r\n            [\r\n              1870505.36514153,\r\n              8336478.0551983\r\n            ],\r\n            [\r\n              1870463.51445669,\r\n              8336520.52603755\r\n            ],\r\n            [\r\n              1870197.1236274,\r\n              8336552.61658516\r\n            ],\r\n            [\r\n              1869916.77715176,\r\n              8336675.70867748\r\n            ],\r\n            [\r\n              1869883.65754831,\r\n              8336700.56538487\r\n            ],\r\n            [\r\n              1869765.71043024,\r\n              8336756.1010495\r\n            ],\r\n            [\r\n              1869742.6353182,\r\n              8336749.84311352\r\n            ],\r\n            [\r\n              1869719.59868019,\r\n              8336745.56668846\r\n            ],\r\n            [\r\n              1869704.84507358,\r\n              8336739.92755831\r\n            ],\r\n            [\r\n              1869693.86486955,\r\n              8336725.49773274\r\n            ],\r\n            [\r\n              1869682.01481447,\r\n              8336707.12303509\r\n            ],\r\n            [\r\n              1869671.53663133,\r\n              8336697.83152502\r\n            ],\r\n            [\r\n              1869662.77094036,\r\n              8336694.84574382\r\n            ],\r\n            [\r\n              1869644.68364302,\r\n              8336700.3667016\r\n            ],\r\n            [\r\n              1869624.21725839,\r\n              8336705.54718538\r\n            ],\r\n            [\r\n              1869608.48754197,\r\n              8336710.22835222\r\n            ],\r\n            [\r\n              1869579.49517687,\r\n              8336704.88871521\r\n            ],\r\n            [\r\n              1869521.77886731,\r\n              8336687.86083727\r\n            ],\r\n            [\r\n              1869486.00616007,\r\n              8336679.88332491\r\n            ],\r\n            [\r\n              1869474.10168201,\r\n              8336678.14887846\r\n            ],\r\n            [\r\n              1869450.05991938,\r\n              8336682.60910477\r\n            ],\r\n            [\r\n              1869414.00935684,\r\n              8336699.59731462\r\n            ],\r\n            [\r\n              1869388.39652418,\r\n              8336723.89817349\r\n            ],\r\n            [\r\n              1869357.31187635,\r\n              8336751.48084402\r\n            ],\r\n            [\r\n              1869341.77608926,\r\n              8336765.66836766\r\n            ],\r\n            [\r\n              1869305.97498461,\r\n              8336775.52079321\r\n            ],\r\n            [\r\n              1869258.86469329,\r\n              8336793.53095382\r\n            ],\r\n            [\r\n              1869224.29101415,\r\n              8336805.33797844\r\n            ],\r\n            [\r\n              1869192.71172544,\r\n              8336828.17572367\r\n            ],\r\n            [\r\n              1869152.57770923,\r\n              8336858.32200234\r\n            ],\r\n            [\r\n              1869136.6637057,\r\n              8336873.30945761\r\n            ],\r\n            [\r\n              1869123.29827506,\r\n              8336877.54654403\r\n            ],\r\n            [\r\n              1869075.30965088,\r\n              8336891.21521595\r\n            ],\r\n            [\r\n              1869047.38460083,\r\n              8336899.32110109\r\n            ],\r\n            [\r\n              1869025.06155705,\r\n              8336910.47763559\r\n            ],\r\n            [\r\n              1869008.99962631,\r\n              8336918.33763863\r\n            ],\r\n            [\r\n              1868978.43467113,\r\n              8336913.42117247\r\n            ],\r\n            [\r\n              1868916.97995339,\r\n              8336907.15756739\r\n            ],\r\n            [\r\n              1868886.02763486,\r\n              8336902.64672613\r\n            ],\r\n            [\r\n              1868861.98329191,\r\n              8336907.10519719\r\n            ],\r\n            [\r\n              1868835.07577031,\r\n              8336926.27789593\r\n            ],\r\n            [\r\n              1868817.14211524,\r\n              8336939.32891812\r\n            ],\r\n            [\r\n              1868801.1060518,\r\n              8336948.37517014\r\n            ],\r\n            [\r\n              1868777.0472301,\r\n              8336971.452829\r\n            ],\r\n            [\r\n              1868760.14892081,\r\n              8336996.36639514\r\n            ],\r\n            [\r\n              1868740.93213492,\r\n              8337024.0982791\r\n            ],\r\n            [\r\n              1868720.95605782,\r\n              8337034.01704057\r\n            ],\r\n            [\r\n              1868677.03251997,\r\n              8337053.14831748\r\n            ],\r\n            [\r\n              1868638.30698158,\r\n              8337074.94276003\r\n            ],\r\n            [\r\n              1868603.78760958,\r\n              8337089.5188323\r\n            ],\r\n            [\r\n              1868576.3608158,\r\n              8337102.7597375\r\n            ],\r\n            [\r\n              1868556.53044893,\r\n              8337119.80982935\r\n            ],\r\n            [\r\n              1868531.02730556,\r\n              8337149.65407448\r\n            ],\r\n            [\r\n              1868496.51410142,\r\n              8337184.04115702\r\n            ],\r\n            [\r\n              1868459.99988374,\r\n              8337217.28038095\r\n            ],\r\n            [\r\n              1868409.56181725,\r\n              8337266.25949679\r\n            ],\r\n            [\r\n              1868380.61874229,\r\n              8337302.1156406\r\n            ],\r\n            [\r\n              1868355.281631,\r\n              8337340.27468132\r\n            ],\r\n            [\r\n              1868335.5723213,\r\n              8337382.68127882\r\n            ],\r\n            [\r\n              1868323.14136312,\r\n              8337413.44257873\r\n            ],\r\n            [\r\n              1868310.71842349,\r\n              8337444.60459513\r\n            ],\r\n            [\r\n              1868305.56931164,\r\n              8337463.72915395\r\n            ],\r\n            [\r\n              1868315.81198742,\r\n              8337519.78258292\r\n            ],\r\n            [\r\n              1868321.4548276,\r\n              8337563.6489812\r\n            ],\r\n            [\r\n              1868324.04732707,\r\n              8337593.70799406\r\n            ],\r\n            [\r\n              1868315.05307606,\r\n              8337618.45813687\r\n            ],\r\n            [\r\n              1868301.75951427,\r\n              8337665.09000302\r\n            ],\r\n            [\r\n              1868289.51368754,\r\n              8337704.9661743\r\n            ],\r\n            [\r\n              1868273.62189257,\r\n              8337740.55591951\r\n            ],\r\n            [\r\n              1868255.99150288,\r\n              8337768.65500947\r\n            ],\r\n            [\r\n              1868248.29649783,\r\n              8337779.51159093\r\n            ],\r\n            [\r\n              1868229.64366369,\r\n              8337788.3897717\r\n            ],\r\n            [\r\n              1868195.4567271,\r\n              8337839.73992938\r\n            ],\r\n            [\r\n              1868181.75474155,\r\n              8337873.64326818\r\n            ],\r\n            [\r\n              1868176.2445893,\r\n              8337899.88682402\r\n            ],\r\n            [\r\n              1868146.59933249,\r\n              8337951.26746942\r\n            ],\r\n            [\r\n              1868118.40175507,\r\n              8338018.10776822\r\n            ],\r\n            [\r\n              1868091.57697725,\r\n              8338072.42822308\r\n            ],\r\n            [\r\n              1868083.8630604,\r\n              8338109.4037379\r\n            ],\r\n            [\r\n              1868076.05953792,\r\n              8338140.83469413\r\n            ],\r\n            [\r\n              1868076.09676757,\r\n              8338167.78288792\r\n            ],\r\n            [\r\n              1868077.76701717,\r\n              8338197.87156568\r\n            ],\r\n            [\r\n              1868081.47369798,\r\n              8338231.49828003\r\n            ],\r\n            [\r\n              1868084.72273754,\r\n              8338261.16975629\r\n            ],\r\n            [\r\n              1868086.83483203,\r\n              8338294.02971054\r\n            ],\r\n            [\r\n              1868083.40883585,\r\n              8338326.97656201\r\n            ],\r\n            [\r\n              1868080.05043682,\r\n              8338364.27940985\r\n            ],\r\n            [\r\n              1868068.39128437,\r\n              8338402.11704418\r\n            ],\r\n            [\r\n              1868061.97276795,\r\n              8338445.81135967\r\n            ],\r\n            [\r\n              1868051.95422357,\r\n              8338487.18669201\r\n            ],\r\n            [\r\n              1868046.31291747,\r\n              8338530.0759307\r\n            ],\r\n            [\r\n              1868060.83150865,\r\n              8338596.22293697\r\n            ],\r\n            [\r\n              1868145.57676646,\r\n              8338982.33882072\r\n            ],\r\n            [\r\n              1868146.07741677,\r\n              8338984.62497404\r\n            ],\r\n            [\r\n              1868169.40756024,\r\n              8339093.27826053\r\n            ],\r\n            [\r\n              1868182.26063912,\r\n              8339147.21457415\r\n            ],\r\n            [\r\n              1868254.13317129,\r\n              8339455.06341241\r\n            ],\r\n            [\r\n              1868303.3606115,\r\n              8339665.90904646\r\n            ],\r\n            [\r\n              1868436.13314949,\r\n              8340218.07966956\r\n            ],\r\n            [\r\n              1868309.02240653,\r\n              8340487.44771076\r\n            ],\r\n            [\r\n              1868270.51045242,\r\n              8340569.36442708\r\n            ],\r\n            [\r\n              1868169.29536001,\r\n              8340784.6450375\r\n            ],\r\n            [\r\n              1868085.09003755,\r\n              8340988.16631914\r\n            ],\r\n            [\r\n              1868108.47772923,\r\n              8341383.45227398\r\n            ],\r\n            [\r\n              1868143.31219397,\r\n              8341874.9224558\r\n            ],\r\n            [\r\n              1868050.10614635,\r\n              8342085.75221712\r\n            ],\r\n            [\r\n              1867980.11549423,\r\n              8342238.72539552\r\n            ],\r\n            [\r\n              1867948.26443685,\r\n              8342320.53420663\r\n            ],\r\n            [\r\n              1867784.59372519,\r\n              8342740.90554457\r\n            ],\r\n            [\r\n              1867709.20859985,\r\n              8342922.2156739\r\n            ],\r\n            [\r\n              1867621.08363668,\r\n              8343130.6509265\r\n            ],\r\n            [\r\n              1867560.35377636,\r\n              8343280.47361462\r\n            ],\r\n            [\r\n              1867470.00045533,\r\n              8343501.63343679\r\n            ],\r\n            [\r\n              1867394.86185991,\r\n              8343606.83033727\r\n            ],\r\n            [\r\n              1867089.33460001,\r\n              8344043.54300204\r\n            ],\r\n            [\r\n              1866915.33994988,\r\n              8344291.90515158\r\n            ],\r\n            [\r\n              1866515.05742389,\r\n              8344855.10320401\r\n            ],\r\n            [\r\n              1866309.51237856,\r\n              8345117.34227085\r\n            ],\r\n            [\r\n              1866254.62324697,\r\n              8345229.44656409\r\n            ],\r\n            [\r\n              1866184.49334626,\r\n              8345391.70802676\r\n            ],\r\n            [\r\n              1866171.45764919,\r\n              8345427.56064942\r\n            ],\r\n            [\r\n              1865794.4992889,\r\n              8345698.60569588\r\n            ],\r\n            [\r\n              1865511.80550255,\r\n              8345905.45324785\r\n            ],\r\n            [\r\n              1865476.47228512,\r\n              8345931.65602603\r\n            ],\r\n            [\r\n              1865340.11678787,\r\n              8346155.79677567\r\n            ],\r\n            [\r\n              1865203.92229779,\r\n              8346427.53422501\r\n            ],\r\n            [\r\n              1864993.44805002,\r\n              8346849.71306338\r\n            ],\r\n            [\r\n              1864903.19631963,\r\n              8347022.55004334\r\n            ],\r\n            [\r\n              1864897.76479704,\r\n              8347032.13447548\r\n            ],\r\n            [\r\n              1864427.42984918,\r\n              8347289.99582405\r\n            ],\r\n            [\r\n              1864347.58515579,\r\n              8347439.70257383\r\n            ],\r\n            [\r\n              1864233.27948983,\r\n              8347657.65469691\r\n            ],\r\n            [\r\n              1863990.37169403,\r\n              8348025.10407796\r\n            ],\r\n            [\r\n              1863956.11363047,\r\n              8348077.08088146\r\n            ],\r\n            [\r\n              1863864.66002283,\r\n              8348218.59941253\r\n            ],\r\n            [\r\n              1863381.05415308,\r\n              8348778.12490865\r\n            ],\r\n            [\r\n              1863180.08224447,\r\n              8349010.59815647\r\n            ],\r\n            [\r\n              1862887.57343278,\r\n              8349346.90091476\r\n            ],\r\n            [\r\n              1862440.18718987,\r\n              8349863.96424072\r\n            ],\r\n            [\r\n              1862290.82734511,\r\n              8349798.21984356\r\n            ],\r\n            [\r\n              1862185.66267785,\r\n              8349752.05261874\r\n            ],\r\n            [\r\n              1862157.80154229,\r\n              8349739.82199355\r\n            ],\r\n            [\r\n              1862136.14624797,\r\n              8349751.97791814\r\n            ],\r\n            [\r\n              1862113.76749381,\r\n              8349770.09145715\r\n            ],\r\n            [\r\n              1862099.71501228,\r\n              8349788.90426874\r\n            ],\r\n            [\r\n              1862095.25955263,\r\n              8349815.1430141\r\n            ],\r\n            [\r\n              1862108.34633502,\r\n              8349850.71165125\r\n            ],\r\n            [\r\n              1862127.68701526,\r\n              8349878.66577583\r\n            ],\r\n            [\r\n              1862144.13255774,\r\n              8349895.93668148\r\n            ],\r\n            [\r\n              1862159.70459189,\r\n              8349906.0759143\r\n            ],\r\n            [\r\n              1862176.89564659,\r\n              8349919.37288036\r\n            ],\r\n            [\r\n              1862191.08487202,\r\n              8349947.38511754\r\n            ],\r\n            [\r\n              1862202.8774239,\r\n              8349973.43918364\r\n            ],\r\n            [\r\n              1862222.21836565,\r\n              8350001.39561498\r\n            ],\r\n            [\r\n              1862227.43805124,\r\n              8350011.21664051\r\n            ],\r\n            [\r\n              1862233.50169463,\r\n              8350022.6278057\r\n            ],\r\n            [\r\n              1862240.03767679,\r\n              8350034.92358599\r\n            ],\r\n            [\r\n              1862251.03226694,\r\n              8350060.59476379\r\n            ],\r\n            [\r\n              1862256.86924024,\r\n              8350085.92425999\r\n            ],\r\n            [\r\n              1862260.43595098,\r\n              8350120.41001024\r\n            ],\r\n            [\r\n              1862263.22967442,\r\n              8350156.88912973\r\n            ],\r\n            [\r\n              1862259.40483775,\r\n              8350203.36292023\r\n            ],\r\n            [\r\n              1862271.49459528,\r\n              8350213.59611769\r\n            ],\r\n            [\r\n              1862638.78743817,\r\n              8350524.42812739\r\n            ],\r\n            [\r\n              1863059.36785385,\r\n              8350874.77859509\r\n            ],\r\n            [\r\n              1863144.72769022,\r\n              8351230.21042009\r\n            ],\r\n            [\r\n              1863310.6325558,\r\n              8351420.78915719\r\n            ],\r\n            [\r\n              1863491.455111,\r\n              8351632.63013846\r\n            ],\r\n            [\r\n              1863908.9341635,\r\n              8352116.00129901\r\n            ],\r\n            [\r\n              1864066.60484347,\r\n              8352298.475807\r\n            ],\r\n            [\r\n              1864695.30921628,\r\n              8353025.99960328\r\n            ],\r\n            [\r\n              1864991.34326903,\r\n              8353367.82353677\r\n            ],\r\n            [\r\n              1864915.23802632,\r\n              8353449.97128834\r\n            ],\r\n            [\r\n              1865011.46371203,\r\n              8353518.25174082\r\n            ],\r\n            [\r\n              1865134.72268731,\r\n              8353604.28745883\r\n            ],\r\n            [\r\n              1865275.16662342,\r\n              8353720.42828509\r\n            ],\r\n            [\r\n              1865355.47840094,\r\n              8353774.48120544\r\n            ],\r\n            [\r\n              1865444.59067102,\r\n              8353845.96989432\r\n            ],\r\n            [\r\n              1865480.77172919,\r\n              8353870.03158753\r\n            ],\r\n            [\r\n              1865630.6090561,\r\n              8354042.13040737\r\n            ],\r\n            [\r\n              1865909.65963025,\r\n              8354379.66889721\r\n            ],\r\n            [\r\n              1866006.89258865,\r\n              8354405.84209788\r\n            ],\r\n            [\r\n              1866144.86037484,\r\n              8354495.37694767\r\n            ],\r\n            [\r\n              1866164.85452415,\r\n              8354531.82769738\r\n            ],\r\n            [\r\n              1866198.028421,\r\n              8354589.27091388\r\n            ],\r\n            [\r\n              1866245.11498193,\r\n              8354654.19523478\r\n            ],\r\n            [\r\n              1866332.98051251,\r\n              8354873.05776903\r\n            ],\r\n            [\r\n              1866263.35394726,\r\n              8354901.5781572\r\n            ],\r\n            [\r\n              1866268.65702255,\r\n              8355342.09121573\r\n            ],\r\n            [\r\n              1866201.80062424,\r\n              8355616.50327339\r\n            ],\r\n            [\r\n              1866267.16119802,\r\n              8355770.34211993\r\n            ],\r\n            [\r\n              1866261.80125267,\r\n              8355809.69752119\r\n            ],\r\n            [\r\n              1866306.99329536,\r\n              8355891.32819286\r\n            ],\r\n            [\r\n              1866438.92341551,\r\n              8355874.03212268\r\n            ],\r\n            [\r\n              1866501.20581122,\r\n              8355964.32691725\r\n            ],\r\n            [\r\n              1866750.12122484,\r\n              8355821.72279504\r\n            ],\r\n            [\r\n              1866766.40239571,\r\n              8355843.89760684\r\n            ],\r\n            [\r\n              1866794.24195765,\r\n              8355889.43595748\r\n            ],\r\n            [\r\n              1866830.71345412,\r\n              8355953.30305496\r\n            ],\r\n            [\r\n              1866851.08465677,\r\n              8355989.9626946\r\n            ],\r\n            [\r\n              1866875.11467784,\r\n              8356045.82638358\r\n            ],\r\n            [\r\n              1866953.04624746,\r\n              8356230.95117968\r\n            ],\r\n            [\r\n              1866993.34842898,\r\n              8356237.75215433\r\n            ],\r\n            [\r\n              1867135.06564177,\r\n              8356354.69012445\r\n            ],\r\n            [\r\n              1867309.42806707,\r\n              8356497.69149952\r\n            ],\r\n            [\r\n              1867522.15748132,\r\n              8356541.97208565\r\n            ],\r\n            [\r\n              1867617.79493393,\r\n              8356559.7923046\r\n            ],\r\n            [\r\n              1867990.06727361,\r\n              8356634.68785213\r\n            ],\r\n            [\r\n              1868508.65854393,\r\n              8356747.37707379\r\n            ],\r\n            [\r\n              1868898.1742219,\r\n              8356832.04743622\r\n            ],\r\n            [\r\n              1868879.16027166,\r\n              8356950.86890634\r\n            ],\r\n            [\r\n              1868822.46898874,\r\n              8357054.28962761\r\n            ],\r\n            [\r\n              1868632.85936766,\r\n              8357008.67908718\r\n            ],\r\n            [\r\n              1868596.55248406,\r\n              8357202.19224998\r\n            ],\r\n            [\r\n              1868516.33686352,\r\n              8357268.19450331\r\n            ],\r\n            [\r\n              1868505.59182319,\r\n              8357272.29018498\r\n            ],\r\n            [\r\n              1868381.48996025,\r\n              8357505.74861953\r\n            ],\r\n            [\r\n              1868376.65301368,\r\n              8357644.84898747\r\n            ],\r\n            [\r\n              1868522.52333974,\r\n              8357687.78493439\r\n            ],\r\n            [\r\n              1868655.03841667,\r\n              8357726.69520344\r\n            ],\r\n            [\r\n              1868734.12999946,\r\n              8357752.59056596\r\n            ],\r\n            [\r\n              1868714.19269351,\r\n              8357780.12017067\r\n            ],\r\n            [\r\n              1868737.13248057,\r\n              8357838.73990386\r\n            ],\r\n            [\r\n              1868907.93498654,\r\n              8357950.79256754\r\n            ],\r\n            [\r\n              1869141.79610213,\r\n              8358353.69658493\r\n            ],\r\n            [\r\n              1869144.74731985,\r\n              8358360.46084285\r\n            ],\r\n            [\r\n              1869216.04485107,\r\n              8358523.85985955\r\n            ],\r\n            [\r\n              1869249.47479313,\r\n              8358575.54521448\r\n            ],\r\n            [\r\n              1869321.4798289,\r\n              8358686.870429\r\n            ],\r\n            [\r\n              1869337.88268029,\r\n              8358694.13819368\r\n            ],\r\n            [\r\n              1869551.20364785,\r\n              8358788.66395648\r\n            ],\r\n            [\r\n              1869842.75355435,\r\n              8358731.88084587\r\n            ],\r\n            [\r\n              1870025.78001775,\r\n              8358593.24209412\r\n            ],\r\n            [\r\n              1870077.93817115,\r\n              8358583.55197234\r\n            ],\r\n            [\r\n              1870086.84174472,\r\n              8358543.97820267\r\n            ],\r\n            [\r\n              1870189.93290835,\r\n              8358477.33981094\r\n            ],\r\n            [\r\n              1870216.43085968,\r\n              8358504.07733691\r\n            ],\r\n            [\r\n              1870299.07415213,\r\n              8358457.17877202\r\n            ],\r\n            [\r\n              1870556.84085794,\r\n              8358310.8929533\r\n            ],\r\n            [\r\n              1870587.43570654,\r\n              8358403.88843821\r\n            ],\r\n            [\r\n              1870886.87625883,\r\n              8358667.49497263\r\n            ],\r\n            [\r\n              1871062.81123473,\r\n              8358822.36630154\r\n            ],\r\n            [\r\n              1871297.9014759,\r\n              8359648.89962386\r\n            ],\r\n            [\r\n              1871415.45341057,\r\n              8359756.34539191\r\n            ],\r\n            [\r\n              1870680.64341607,\r\n              8359871.57754429\r\n            ],\r\n            [\r\n              1870578.4808788,\r\n              8360061.80180539\r\n            ],\r\n            [\r\n              1870157.7986299,\r\n              8360845.03659491\r\n            ],\r\n            [\r\n              1870454.91184481,\r\n              8361107.68803299\r\n            ],\r\n            [\r\n              1871263.84828741,\r\n              8361134.84329867\r\n            ],\r\n            [\r\n              1871263.13628136,\r\n              8361253.02879096\r\n            ],\r\n            [\r\n              1871262.73223588,\r\n              8361319.8044551\r\n            ],\r\n            [\r\n              1871262.22193677,\r\n              8361404.45136444\r\n            ],\r\n            [\r\n              1871261.74022042,\r\n              8361484.43485077\r\n            ],\r\n            [\r\n              1871261.11684957,\r\n              8361587.63002156\r\n            ],\r\n            [\r\n              1871260.41099977,\r\n              8361704.65358615\r\n            ],\r\n            [\r\n              1871260.06966796,\r\n              8361761.12926285\r\n            ],\r\n            [\r\n              1871259.51917288,\r\n              8361852.47465039\r\n            ],\r\n            [\r\n              1871258.6319431,\r\n              8361999.65625346\r\n            ],\r\n            [\r\n              1871258.17391387,\r\n              8362075.70650376\r\n            ],\r\n            [\r\n              1871258.77673026,\r\n              8362128.05088554\r\n            ],\r\n            [\r\n              1871259.68639963,\r\n              8362208.4228422\r\n            ],\r\n            [\r\n              1871259.14670853,\r\n              8362268.85388367\r\n            ],\r\n            [\r\n              1871256.76854574,\r\n              8362535.11679912\r\n            ],\r\n            [\r\n              1871255.74172193,\r\n              8362649.98687133\r\n            ],\r\n            [\r\n              1871475.59351339,\r\n              8362806.57493087\r\n            ],\r\n            [\r\n              1871945.34473149,\r\n              8363141.12628759\r\n            ],\r\n            [\r\n              1871989.20571692,\r\n              8363172.36045257\r\n            ],\r\n            [\r\n              1872180.72419847,\r\n              8363419.21607303\r\n            ],\r\n            [\r\n              1872127.67909376,\r\n              8363542.12519261\r\n            ],\r\n            [\r\n              1872038.29284938,\r\n              8363749.2368299\r\n            ],\r\n            [\r\n              1871981.72536724,\r\n              8363880.29985821\r\n            ],\r\n            [\r\n              1871848.85625964,\r\n              8363856.60003529\r\n            ],\r\n            [\r\n              1871676.9115243,\r\n              8363660.39851891\r\n            ],\r\n            [\r\n              1871086.50575823,\r\n              8364272.72842242\r\n            ],\r\n            [\r\n              1870854.05721225,\r\n              8364513.78373451\r\n            ],\r\n            [\r\n              1870279.32488655,\r\n              8365109.7228613\r\n            ],\r\n            [\r\n              1870489.59799023,\r\n              8365378.95731999\r\n            ],\r\n            [\r\n              1870508.74661388,\r\n              8365403.47441017\r\n            ],\r\n            [\r\n              1870584.65065046,\r\n              8365500.65850848\r\n            ],\r\n            [\r\n              1871114.49165513,\r\n              8366178.97277563\r\n            ],\r\n            [\r\n              1871230.40706574,\r\n              8366327.36015655\r\n            ],\r\n            [\r\n              1871234.82239494,\r\n              8366333.01072776\r\n            ],\r\n            [\r\n              1871605.96011917,\r\n              8366808.07416234\r\n            ],\r\n            [\r\n              1871671.98813332,\r\n              8366819.28696838\r\n            ],\r\n            [\r\n              1872076.61617917,\r\n              8366887.97147476\r\n            ],\r\n            [\r\n              1872143.17691571,\r\n              8366899.26989895\r\n            ],\r\n            [\r\n              1872212.4991793,\r\n              8366956.30689714\r\n            ],\r\n            [\r\n              1872891.23434545,\r\n              8367514.70888534\r\n            ],\r\n            [\r\n              1873248.61023534,\r\n              8367808.683932\r\n            ],\r\n            [\r\n              1874142.85208358,\r\n              8368544.15360274\r\n            ],\r\n            [\r\n              1874236.49491582,\r\n              8368621.15900945\r\n            ],\r\n            [\r\n              1874283.73782181,\r\n              8368882.39494181\r\n            ],\r\n            [\r\n              1874301.0369338,\r\n              8368978.05277999\r\n            ],\r\n            [\r\n              1874213.38878325,\r\n              8368994.08218306\r\n            ],\r\n            [\r\n              1873704.17146485,\r\n              8369242.8010576\r\n            ],\r\n            [\r\n              1873407.52041291,\r\n              8369299.56393566\r\n            ],\r\n            [\r\n              1873464.7421261,\r\n              8369567.06967047\r\n            ],\r\n            [\r\n              1873365.6864889,\r\n              8369595.54126576\r\n            ],\r\n            [\r\n              1873171.34442859,\r\n              8369632.54713482\r\n            ],\r\n            [\r\n              1872968.2789143,\r\n              8369671.62702592\r\n            ],\r\n            [\r\n              1872898.845041,\r\n              8369683.04008125\r\n            ],\r\n            [\r\n              1872738.5710145,\r\n              8369709.38318722\r\n            ],\r\n            [\r\n              1872654.59720569,\r\n              8369736.09697948\r\n            ],\r\n            [\r\n              1872114.06211615,\r\n              8370021.84153247\r\n            ],\r\n            [\r\n              1871859.17042733,\r\n              8370156.56796022\r\n            ],\r\n            [\r\n              1871785.96229335,\r\n              8370195.2592324\r\n            ],\r\n            [\r\n              1871781.4163182,\r\n              8370197.66139226\r\n            ],\r\n            [\r\n              1871208.71554795,\r\n              8370394.37832005\r\n            ],\r\n            [\r\n              1870696.69676846,\r\n              8370581.29633955\r\n            ],\r\n            [\r\n              1870223.9907379,\r\n              8370767.39763401\r\n            ],\r\n            [\r\n              1870236.71955963,\r\n              8370928.00481722\r\n            ],\r\n            [\r\n              1870232.38847732,\r\n              8371186.297756\r\n            ],\r\n            [\r\n              1870229.0569977,\r\n              8371385.06659736\r\n            ],\r\n            [\r\n              1870229.50456442,\r\n              8371456.04531537\r\n            ],\r\n            [\r\n              1870229.74175832,\r\n              8371534.07962176\r\n            ],\r\n            [\r\n              1870230.42534716,\r\n              8371758.76560691\r\n            ],\r\n            [\r\n              1870231.09522323,\r\n              8371797.76196337\r\n            ],\r\n            [\r\n              1870206.41360953,\r\n              8372092.39496385\r\n            ],\r\n            [\r\n              1870202.06190252,\r\n              8372144.3508628\r\n            ],\r\n            [\r\n              1870190.45277416,\r\n              8372282.94153839\r\n            ],\r\n            [\r\n              1870143.17780544,\r\n              8372847.26586692\r\n            ],\r\n            [\r\n              1870057.51746981,\r\n              8373402.42156298\r\n            ],\r\n            [\r\n              1870057.11484436,\r\n              8373402.65819795\r\n            ],\r\n            [\r\n              1868770.01268137,\r\n              8374447.25404618\r\n            ],\r\n            [\r\n              1868502.71669936,\r\n              8374663.2038157\r\n            ],\r\n            [\r\n              1867703.30456148,\r\n              8375299.58271206\r\n            ],\r\n            [\r\n              1867526.83860217,\r\n              8375602.97617236\r\n            ],\r\n            [\r\n              1867391.44070877,\r\n              8375839.18219038\r\n            ],\r\n            [\r\n              1867363.10655634,\r\n              8375894.08537432\r\n            ],\r\n            [\r\n              1867168.18675427,\r\n              8376236.1588729\r\n            ],\r\n            [\r\n              1867515.59387802,\r\n              8377907.61136768\r\n            ],\r\n            [\r\n              1867583.71558265,\r\n              8378227.4623836\r\n            ],\r\n            [\r\n              1867682.48719836,\r\n              8378703.91693337\r\n            ],\r\n            [\r\n              1867693.73345378,\r\n              8378774.69354174\r\n            ],\r\n            [\r\n              1867874.22240418,\r\n              8378763.32824628\r\n            ],\r\n            [\r\n              1868184.40465059,\r\n              8378751.1668603\r\n            ],\r\n            [\r\n              1868624.25980606,\r\n              8378736.59709\r\n            ],\r\n            [\r\n              1868733.56874169,\r\n              8378728.47757994\r\n            ],\r\n            [\r\n              1868833.95706503,\r\n              8378769.16717442\r\n            ],\r\n            [\r\n              1869022.95840929,\r\n              8378845.77006402\r\n            ],\r\n            [\r\n              1869451.91874564,\r\n              8379030.68294029\r\n            ],\r\n            [\r\n              1870403.92462603,\r\n              8379435.14021108\r\n            ],\r\n            [\r\n              1870396.13830648,\r\n              8379489.15264219\r\n            ],\r\n            [\r\n              1870326.01044802,\r\n              8379832.17412626\r\n            ],\r\n            [\r\n              1870291.47801578,\r\n              8379993.31248596\r\n            ],\r\n            [\r\n              1870257.90092007,\r\n              8380160.80789066\r\n            ],\r\n            [\r\n              1870251.29008956,\r\n              8380198.45111618\r\n            ],\r\n            [\r\n              1870220.00231568,\r\n              8380346.35675424\r\n            ],\r\n            [\r\n              1870174.88209275,\r\n              8380624.19338915\r\n            ],\r\n            [\r\n              1870155.3912836,\r\n              8380782.17586388\r\n            ],\r\n            [\r\n              1870153.79596046,\r\n              8380797.76600229\r\n            ],\r\n            [\r\n              1870137.92505716,\r\n              8380988.75436514\r\n            ],\r\n            [\r\n              1870121.03271732,\r\n              8381186.55065938\r\n            ],\r\n            [\r\n              1870108.14206246,\r\n              8381337.99861163\r\n            ],\r\n            [\r\n              1870101.56729124,\r\n              8381455.7933273\r\n            ],\r\n            [\r\n              1870094.97050038,\r\n              8381541.29247502\r\n            ],\r\n            [\r\n              1870089.60885794,\r\n              8381596.85407477\r\n            ],\r\n            [\r\n              1870081.53553292,\r\n              8381702.72971506\r\n            ],\r\n            [\r\n              1870080.1050623,\r\n              8381709.1468219\r\n            ],\r\n            [\r\n              1870038.08137425,\r\n              8381669.13618808\r\n            ],\r\n            [\r\n              1869990.64199556,\r\n              8381682.69927034\r\n            ],\r\n            [\r\n              1869991.39762043,\r\n              8381712.58606491\r\n            ],\r\n            [\r\n              1870007.53222474,\r\n              8381736.50255405\r\n            ],\r\n            [\r\n              1870063.31919939,\r\n              8381879.84285586\r\n            ],\r\n            [\r\n              1870083.78747421,\r\n              8381949.11035996\r\n            ],\r\n            [\r\n              1870083.8094232,\r\n              8382012.9139844\r\n            ],\r\n            [\r\n              1870076.9175787,\r\n              8382056.16910042\r\n            ],\r\n            [\r\n              1870068.50999595,\r\n              8382121.01746912\r\n            ],\r\n            [\r\n              1870055.86300032,\r\n              8382244.13151278\r\n            ],\r\n            [\r\n              1870051.2686499,\r\n              8382272.94455067\r\n            ],\r\n            [\r\n              1870039.57426284,\r\n              8382384.07392794\r\n            ],\r\n            [\r\n              1870026.34947459,\r\n              8382478.4818303\r\n            ],\r\n            [\r\n              1870017.55813391,\r\n              8382536.10188435\r\n            ],\r\n            [\r\n              1870004.7226199,\r\n              8382669.59766171\r\n            ],\r\n            [\r\n              1869990.18129525,\r\n              8382797.54641863\r\n            ],\r\n            [\r\n              1869981.59974312,\r\n              8382885.48329913\r\n            ],\r\n            [\r\n              1869975.9956467,\r\n              8382923.49412476\r\n            ],\r\n            [\r\n              1869945.10121049,\r\n              8383208.16341973\r\n            ],\r\n            [\r\n              1869925.58819707,\r\n              8383366.14056499\r\n            ],\r\n            [\r\n              1869898.52701854,\r\n              8383601.79241634\r\n            ],\r\n            [\r\n              1869883.92021108,\r\n              8383728.98707617\r\n            ],\r\n            [\r\n              1869878.83932407,\r\n              8383773.23019193\r\n            ],\r\n            [\r\n              1869850.98164442,\r\n              8384031.1372629\r\n            ],\r\n            [\r\n              1869818.34599876,\r\n              8384301.05883163\r\n            ],\r\n            [\r\n              1869817.47730343,\r\n              8384344.87295797\r\n            ],\r\n            [\r\n              1869801.68378433,\r\n              8384516.73701595\r\n            ],\r\n            [\r\n              1869790.68476793,\r\n              8384605.60392536\r\n            ],\r\n            [\r\n              1869782.91936814,\r\n              8384825.43567197\r\n            ],\r\n            [\r\n              1869768.13353414,\r\n              8384997.98372453\r\n            ],\r\n            [\r\n              1869754.73418522,\r\n              8385154.34138641\r\n            ],\r\n            [\r\n              1869741.35803625,\r\n              8385310.43650428\r\n            ],\r\n            [\r\n              1869718.98279047,\r\n              8385571.52786986\r\n            ],\r\n            [\r\n              1869707.36030506,\r\n              8385707.13204782\r\n            ],\r\n            [\r\n              1869685.42898471,\r\n              8385963.03629301\r\n            ],\r\n            [\r\n              1869675.23705069,\r\n              8386081.96158243\r\n            ],\r\n            [\r\n              1869645.57861542,\r\n              8386428.00485122\r\n            ],\r\n            [\r\n              1869622.90146828,\r\n              8386692.57099533\r\n            ],\r\n            [\r\n              1869597.8888141,\r\n              8386984.4015971\r\n            ],\r\n            [\r\n              1869581.53855981,\r\n              8387175.13047803\r\n            ],\r\n            [\r\n              1869565.43380981,\r\n              8387363.0231845\r\n            ],\r\n            [\r\n              1869549.04485838,\r\n              8387554.21306374\r\n            ],\r\n            [\r\n              1869546.99646221,\r\n              8387578.08425076\r\n            ],\r\n            [\r\n              1869544.1119646,\r\n              8387611.73832806\r\n            ],\r\n            [\r\n              1869536.80367817,\r\n              8387696.99943333\r\n            ],\r\n            [\r\n              1869467.44416753,\r\n              8387697.32186345\r\n            ],\r\n            [\r\n              1869477.44347284,\r\n              8387567.22068433\r\n            ],\r\n            [\r\n              1869485.45039175,\r\n              8387463.03860105\r\n            ],\r\n            [\r\n              1869435.48732603,\r\n              8387351.27103448\r\n            ],\r\n            [\r\n              1869389.12108762,\r\n              8387247.55115875\r\n            ],\r\n            [\r\n              1869345.56228055,\r\n              8387150.10702359\r\n            ],\r\n            [\r\n              1869289.48048299,\r\n              8387024.64859112\r\n            ],\r\n            [\r\n              1869231.51842575,\r\n              8386956.27346412\r\n            ],\r\n            [\r\n              1869133.10403176,\r\n              8386954.6120809\r\n            ],\r\n            [\r\n              1869154.45589172,\r\n              8387125.83796805\r\n            ],\r\n            [\r\n              1869178.63065908,\r\n              8387259.08674138\r\n            ],\r\n            [\r\n              1869180.35741312,\r\n              8387325.32074511\r\n            ],\r\n            [\r\n              1869204.30540135,\r\n              8387466.96196182\r\n            ],\r\n            [\r\n              1869204.62330652,\r\n              8387562.36441912\r\n            ],\r\n            [\r\n              1869206.51312594,\r\n              8387597.05848887\r\n            ],\r\n            [\r\n              1869216.19696451,\r\n              8387702.65826638\r\n            ],\r\n            [\r\n              1868755.92797809,\r\n              8388147.65462781\r\n            ],\r\n            [\r\n              1868735.66947402,\r\n              8388151.95799411\r\n            ],\r\n            [\r\n              1868725.56163435,\r\n              8388219.52510302\r\n            ],\r\n            [\r\n              1868728.90191601,\r\n              8388280.50386168\r\n            ],\r\n            [\r\n              1868682.87666746,\r\n              8388345.04127679\r\n            ],\r\n            [\r\n              1868640.03404864,\r\n              8388614.1643077\r\n            ],\r\n            [\r\n              1868556.93240927,\r\n              8388729.53999386\r\n            ],\r\n            [\r\n              1868541.91056133,\r\n              8388788.81371351\r\n            ],\r\n            [\r\n              1868540.45577458,\r\n              8388849.07215443\r\n            ],\r\n            [\r\n              1868538.57556948,\r\n              8388933.27392232\r\n            ],\r\n            [\r\n              1868521.28232807,\r\n              8388974.63068889\r\n            ],\r\n            [\r\n              1868501.62838643,\r\n              8389018.0201953\r\n            ],\r\n            [\r\n              1868447.54902043,\r\n              8389103.42996867\r\n            ],\r\n            [\r\n              1868395.95873947,\r\n              8389220.71769557\r\n            ],\r\n            [\r\n              1868367.99861845,\r\n              8389242.69419045\r\n            ],\r\n            [\r\n              1868274.58961902,\r\n              8389311.16126563\r\n            ],\r\n            [\r\n              1868188.83489855,\r\n              8389359.16290282\r\n            ],\r\n            [\r\n              1868052.30271739,\r\n              8389371.64352315\r\n            ],\r\n            [\r\n              1867895.14777112,\r\n              8389571.54823611\r\n            ],\r\n            [\r\n              1867735.20407451,\r\n              8389591.16766046\r\n            ],\r\n            [\r\n              1867592.70259129,\r\n              8389785.65915009\r\n            ],\r\n            [\r\n              1867565.28812796,\r\n              8389765.73279209\r\n            ],\r\n            [\r\n              1867528.48682197,\r\n              8389731.58874016\r\n            ],\r\n            [\r\n              1867496.36732361,\r\n              8389690.98942379\r\n            ],\r\n            [\r\n              1867471.39963483,\r\n              8389648.68269348\r\n            ],\r\n            [\r\n              1867459.27795992,\r\n              8389612.16490943\r\n            ],\r\n            [\r\n              1867445.70685166,\r\n              8389585.24410538\r\n            ],\r\n            [\r\n              1867448.89526998,\r\n              8389559.26281683\r\n            ],\r\n            [\r\n              1867441.56812167,\r\n              8389523.46936551\r\n            ],\r\n            [\r\n              1867430.61089774,\r\n              8389484.93944511\r\n            ],\r\n            [\r\n              1867402.01987238,\r\n              8389440.29588927\r\n            ],\r\n            [\r\n              1867368.53638839,\r\n              8389414.47982696\r\n            ],\r\n            [\r\n              1867339.64708095,\r\n              8389402.15679778\r\n            ],\r\n            [\r\n              1867312.06190441,\r\n              8389396.99507952\r\n            ],\r\n            [\r\n              1867285.20153681,\r\n              8389387.03304429\r\n            ],\r\n            [\r\n              1867245.61850786,\r\n              8389379.26282271\r\n            ],\r\n            [\r\n              1867051.24907588,\r\n              8389415.35027599\r\n            ],\r\n            [\r\n              1867049.34418424,\r\n              8389369.10252931\r\n            ],\r\n            [\r\n              1867035.17489435,\r\n              8389329.02508863\r\n            ],\r\n            [\r\n              1867032.77976888,\r\n              8389302.73144806\r\n            ],\r\n            [\r\n              1867012.48691344,\r\n              8389227.24364789\r\n            ],\r\n            [\r\n              1866988.98423704,\r\n              8389150.20878927\r\n            ],\r\n            [\r\n              1866738.9641659,\r\n              8389065.86288693\r\n            ],\r\n            [\r\n              1866675.8745984,\r\n              8389032.51789215\r\n            ],\r\n            [\r\n              1866694.56509376,\r\n              8388899.39145918\r\n            ],\r\n            [\r\n              1866768.84362359,\r\n              8388830.44098164\r\n            ],\r\n            [\r\n              1866731.14957618,\r\n              8388763.59983495\r\n            ],\r\n            [\r\n              1866762.27818002,\r\n              8388714.05898351\r\n            ],\r\n            [\r\n              1866767.11041342,\r\n              8388691.24792186\r\n            ],\r\n            [\r\n              1866761.33814497,\r\n              8388652.64164424\r\n            ],\r\n            [\r\n              1866730.53083049,\r\n              8388592.87755458\r\n            ],\r\n            [\r\n              1866630.44912726,\r\n              8388538.15728122\r\n            ],\r\n            [\r\n              1866582.72185121,\r\n              8388492.61172656\r\n            ],\r\n            [\r\n              1866508.02959633,\r\n              8388482.18068612\r\n            ],\r\n            [\r\n              1866444.72024625,\r\n              8388486.3356341\r\n            ],\r\n            [\r\n              1866403.70778222,\r\n              8388488.95481898\r\n            ],\r\n            [\r\n              1866374.47946103,\r\n              8388480.22451676\r\n            ],\r\n            [\r\n              1866322.19183601,\r\n              8388449.1067984\r\n            ],\r\n            [\r\n              1866274.23817289,\r\n              8388414.73331655\r\n            ],\r\n            [\r\n              1866176.02865732,\r\n              8388430.18538885\r\n            ],\r\n            [\r\n              1866055.3642969,\r\n              8388436.80253633\r\n            ],\r\n            [\r\n              1865941.5967775,\r\n              8388608.85724942\r\n            ],\r\n            [\r\n              1865920.61835592,\r\n              8388644.67793304\r\n            ],\r\n            [\r\n              1865908.10466121,\r\n              8388686.7518503\r\n            ],\r\n            [\r\n              1865865.84955179,\r\n              8388870.0933019\r\n            ],\r\n            [\r\n              1865877.26663777,\r\n              8389018.32125851\r\n            ],\r\n            [\r\n              1865862.72029518,\r\n              8389136.62424914\r\n            ],\r\n            [\r\n              1865870.47241556,\r\n              8389358.31676926\r\n            ],\r\n            [\r\n              1866029.41606502,\r\n              8389692.22611928\r\n            ],\r\n            [\r\n              1866085.03877224,\r\n              8389732.47734514\r\n            ],\r\n            [\r\n              1866194.7562746,\r\n              8389895.58556219\r\n            ],\r\n            [\r\n              1866286.33017142,\r\n              8390045.00996043\r\n            ],\r\n            [\r\n              1866726.78190193,\r\n              8390848.6795438\r\n            ],\r\n            [\r\n              1866755.99715171,\r\n              8390908.08808515\r\n            ],\r\n            [\r\n              1866777.17003163,\r\n              8390962.83155648\r\n            ],\r\n            [\r\n              1866834.76633862,\r\n              8391157.0825866\r\n            ],\r\n            [\r\n              1866858.06085728,\r\n              8391220.17481402\r\n            ],\r\n            [\r\n              1866903.28621874,\r\n              8391309.26880899\r\n            ],\r\n            [\r\n              1866895.04854592,\r\n              8391344.11292057\r\n            ],\r\n            [\r\n              1866919.55365198,\r\n              8391356.10764751\r\n            ],\r\n            [\r\n              1866930.89340295,\r\n              8391367.50814907\r\n            ],\r\n            [\r\n              1866942.51412906,\r\n              8391397.25963311\r\n            ],\r\n            [\r\n              1866953.27602178,\r\n              8391423.03328009\r\n            ],\r\n            [\r\n              1866971.48906538,\r\n              8391440.71328923\r\n            ],\r\n            [\r\n              1866984.77874163,\r\n              8391449.28863478\r\n            ],\r\n            [\r\n              1866980.25865162,\r\n              8391466.51822782\r\n            ],\r\n            [\r\n              1866978.16054427,\r\n              8391485.70487941\r\n            ],\r\n            [\r\n              1866992.09215298,\r\n              8391536.17210708\r\n            ],\r\n            [\r\n              1867004.4667965,\r\n              8391589.05778416\r\n            ],\r\n            [\r\n              1867010.68568807,\r\n              8391630.46532805\r\n            ],\r\n            [\r\n              1867011.52356935,\r\n              8391685.12531575\r\n            ],\r\n            [\r\n              1867019.06806683,\r\n              8391683.01482478\r\n            ],\r\n            [\r\n              1867069.22280933,\r\n              8391677.45870532\r\n            ],\r\n            [\r\n              1867162.29349943,\r\n              8391636.1277158\r\n            ],\r\n            [\r\n              1867190.36924557,\r\n              8391620.93255788\r\n            ],\r\n            [\r\n              1867301.43675794,\r\n              8391583.31335086\r\n            ],\r\n            [\r\n              1867357.82707412,\r\n              8391464.72609362\r\n            ],\r\n            [\r\n              1867426.2564523,\r\n              8391377.08077113\r\n            ],\r\n            [\r\n              1867493.77841495,\r\n              8391360.0823689\r\n            ],\r\n            [\r\n              1867561.98222517,\r\n              8391387.36726312\r\n            ],\r\n            [\r\n              1867556.35783999,\r\n              8391513.9549071\r\n            ],\r\n            [\r\n              1867563.75799044,\r\n              8391606.025507\r\n            ],\r\n            [\r\n              1867514.45322599,\r\n              8391718.52158028\r\n            ],\r\n            [\r\n              1867507.62770202,\r\n              8391767.31375626\r\n            ],\r\n            [\r\n              1867509.85175344,\r\n              8391833.92441225\r\n            ],\r\n            [\r\n              1867533.97458347,\r\n              8391898.6051289\r\n            ],\r\n            [\r\n              1867546.41613975,\r\n              8391955.48231382\r\n            ],\r\n            [\r\n              1867549.65592017,\r\n              8391984.56586163\r\n            ],\r\n            [\r\n              1867587.74295967,\r\n              8391997.94913432\r\n            ],\r\n            [\r\n              1867624.93767056,\r\n              8392004.96084884\r\n            ],\r\n            [\r\n              1867622.3510683,\r\n              8391940.74768439\r\n            ],\r\n            [\r\n              1867617.33291179,\r\n              8391925.65900948\r\n            ],\r\n            [\r\n              1867627.31445797,\r\n              8391874.82108099\r\n            ],\r\n            [\r\n              1867638.86550515,\r\n              8391796.42489776\r\n            ],\r\n            [\r\n              1867678.06182491,\r\n              8391778.26308591\r\n            ],\r\n            [\r\n              1867694.62432551,\r\n              8391714.55483075\r\n            ],\r\n            [\r\n              1867678.65749317,\r\n              8391687.66394228\r\n            ],\r\n            [\r\n              1867660.18984905,\r\n              8391679.56695244\r\n            ],\r\n            [\r\n              1867681.86064375,\r\n              8391507.63513083\r\n            ],\r\n            [\r\n              1868190.92420025,\r\n              8391345.75064628\r\n            ],\r\n            [\r\n              1868342.4251167,\r\n              8391293.52639197\r\n            ],\r\n            [\r\n              1868374.19134107,\r\n              8391285.05363087\r\n            ],\r\n            [\r\n              1869062.17147881,\r\n              8391995.81509314\r\n            ],\r\n            [\r\n              1868964.74259557,\r\n              8392117.70390799\r\n            ],\r\n            [\r\n              1868744.0262058,\r\n              8392375.07438403\r\n            ],\r\n            [\r\n              1868678.9534673,\r\n              8392447.20532423\r\n            ],\r\n            [\r\n              1868495.58900219,\r\n              8392653.49223053\r\n            ],\r\n            [\r\n              1868141.97379755,\r\n              8393064.33804971\r\n            ],\r\n            [\r\n              1867990.09046948,\r\n              8393231.18049759\r\n            ],\r\n            [\r\n              1867886.46643158,\r\n              8393358.4039482\r\n            ],\r\n            [\r\n              1867669.94642853,\r\n              8393604.16109367\r\n            ],\r\n            [\r\n              1867311.11878872,\r\n              8394021.46376203\r\n            ],\r\n            [\r\n              1866631.74647575,\r\n              8394095.37997897\r\n            ],\r\n            [\r\n              1865979.42653406,\r\n              8394162.9793197\r\n            ],\r\n            [\r\n              1865943.96365487,\r\n              8394166.92340136\r\n            ],\r\n            [\r\n              1865709.13520754,\r\n              8394261.49096558\r\n            ],\r\n            [\r\n              1865262.05677438,\r\n              8394436.00966294\r\n            ],\r\n            [\r\n              1865169.0375833,\r\n              8394469.66639371\r\n            ],\r\n            [\r\n              1864924.06038946,\r\n              8394547.94059125\r\n            ],\r\n            [\r\n              1864895.44902256,\r\n              8394559.40023021\r\n            ],\r\n            [\r\n              1864772.47603756,\r\n              8394589.75295236\r\n            ],\r\n            [\r\n              1864676.50983254,\r\n              8394607.86102139\r\n            ],\r\n            [\r\n              1864615.97942354,\r\n              8394618.83328088\r\n            ],\r\n            [\r\n              1864374.38511512,\r\n              8394802.08422692\r\n            ],\r\n            [\r\n              1864222.32667208,\r\n              8394920.96536599\r\n            ],\r\n            [\r\n              1864042.48275335,\r\n              8395055.29088209\r\n            ],\r\n            [\r\n              1863897.17583793,\r\n              8395172.10994356\r\n            ],\r\n            [\r\n              1863850.85904786,\r\n              8395210.09528853\r\n            ],\r\n            [\r\n              1863830.33346553,\r\n              8395233.45658133\r\n            ],\r\n            [\r\n              1863635.85722913,\r\n              8395425.42894482\r\n            ],\r\n            [\r\n              1863577.59013749,\r\n              8395508.26082696\r\n            ],\r\n            [\r\n              1863382.401764,\r\n              8395795.69433302\r\n            ],\r\n            [\r\n              1863295.06637646,\r\n              8395927.93316151\r\n            ],\r\n            [\r\n              1863241.59598725,\r\n              8396097.79352274\r\n            ],\r\n            [\r\n              1863198.24340205,\r\n              8396240.40049464\r\n            ],\r\n            [\r\n              1863147.80315867,\r\n              8396393.46104196\r\n            ],\r\n            [\r\n              1863117.73135633,\r\n              8396463.2500141\r\n            ],\r\n            [\r\n              1863117.2712286,\r\n              8396755.64467116\r\n            ],\r\n            [\r\n              1863117.87615391,\r\n              8396863.09042215\r\n            ],\r\n            [\r\n              1863035.1025717,\r\n              8397015.67363098\r\n            ],\r\n            [\r\n              1862851.92535191,\r\n              8397356.96867508\r\n            ],\r\n            [\r\n              1862921.5954554,\r\n              8397724.63678626\r\n            ],\r\n            [\r\n              1862957.35801751,\r\n              8398221.69017375\r\n            ],\r\n            [\r\n              1862820.57117399,\r\n              8399091.21670853\r\n            ],\r\n            [\r\n              1862798.85923572,\r\n              8399717.98087941\r\n            ],\r\n            [\r\n              1862786.82222793,\r\n              8400183.65513805\r\n            ],\r\n            [\r\n              1862579.05510502,\r\n              8400121.68239876\r\n            ],\r\n            [\r\n              1862260.42566062,\r\n              8400028.78202304\r\n            ],\r\n            [\r\n              1862018.21582486,\r\n              8399953.93376355\r\n            ],\r\n            [\r\n              1861854.67446738,\r\n              8399968.25459221\r\n            ],\r\n            [\r\n              1861667.85764848,\r\n              8400013.1625855\r\n            ],\r\n            [\r\n              1861602.04354101,\r\n              8400281.13031686\r\n            ],\r\n            [\r\n              1861586.17670395,\r\n              8400336.02822508\r\n            ],\r\n            [\r\n              1861576.86956574,\r\n              8400366.08876262\r\n            ],\r\n            [\r\n              1861561.58606207,\r\n              8400397.40349808\r\n            ],\r\n            [\r\n              1861544.24879334,\r\n              8400422.34302228\r\n            ],\r\n            [\r\n              1861534.39527724,\r\n              8400436.42235797\r\n            ],\r\n            [\r\n              1861540.60211215,\r\n              8400460.34392966\r\n            ],\r\n            [\r\n              1861559.48008903,\r\n              8400516.51787355\r\n            ],\r\n            [\r\n              1861567.78861063,\r\n              8400552.01088514\r\n            ],\r\n            [\r\n              1861571.25761588,\r\n              8400581.95088588\r\n            ],\r\n            [\r\n              1861571.12086853,\r\n              8400610.72773275\r\n            ],\r\n            [\r\n              1861567.26692905,\r\n              8400625.95130396\r\n            ],\r\n            [\r\n              1861566.56815058,\r\n              8400636.74785502\r\n            ],\r\n            [\r\n              1861572.44894641,\r\n              8400668.66545044\r\n            ],\r\n            [\r\n              1861574.71540423,\r\n              8400698.21890493\r\n            ],\r\n            [\r\n              1861568.70648663,\r\n              8400739.84150538\r\n            ],\r\n            [\r\n              1861561.85339641,\r\n              8400776.27344796\r\n            ],\r\n            [\r\n              1861547.37393882,\r\n              8400808.38224722\r\n            ],\r\n            [\r\n              1861535.60658005,\r\n              8400831.2731731\r\n            ],\r\n            [\r\n              1861514.25988283,\r\n              8400854.65289156\r\n            ],\r\n            [\r\n              1861386.61898844,\r\n              8400867.83114579\r\n            ],\r\n            [\r\n              1861270.90133352,\r\n              8400962.8316168\r\n            ],\r\n            [\r\n              1861270.69058999,\r\n              8401115.11114098\r\n            ],\r\n            [\r\n              1861205.49857323,\r\n              8401102.92667083\r\n            ],\r\n            [\r\n              1861153.59235794,\r\n              8401102.60887844\r\n            ],\r\n            [\r\n              1861165.01035031,\r\n              8401260.77928421\r\n            ],\r\n            [\r\n              1861179.94782756,\r\n              8401322.9913908\r\n            ],\r\n            [\r\n              1861192.1339537,\r\n              8401433.59557835\r\n            ],\r\n            [\r\n              1861134.12250862,\r\n              8401508.87710066\r\n            ],\r\n            [\r\n              1861123.59305485,\r\n              8401492.58744889\r\n            ],\r\n            [\r\n              1861065.35922111,\r\n              8401587.45773145\r\n            ],\r\n            [\r\n              1861050.45756054,\r\n              8401617.57390133\r\n            ],\r\n            [\r\n              1861045.49286396,\r\n              8401642.40164709\r\n            ],\r\n            [\r\n              1861033.41306023,\r\n              8401675.28949202\r\n            ],\r\n            [\r\n              1861025.79333278,\r\n              8401715.73014256\r\n            ],\r\n            [\r\n              1861020.35194813,\r\n              8401732.17080982\r\n            ],\r\n            [\r\n              1861015.27807851,\r\n              8401745.0084586\r\n            ],\r\n            [\r\n              1861002.30465666,\r\n              8401767.51220124\r\n            ],\r\n            [\r\n              1860979.52783647,\r\n              8401809.69366771\r\n            ],\r\n            [\r\n              1860958.70208903,\r\n              8401847.06076893\r\n            ],\r\n            [\r\n              1860946.93809353,\r\n              8401870.7534941\r\n            ],\r\n            [\r\n              1860916.43123161,\r\n              8401941.38630784\r\n            ],\r\n            [\r\n              1860898.17518203,\r\n              8401997.91740271\r\n            ],\r\n            [\r\n              1860878.24508385,\r\n              8402046.06983074\r\n            ],\r\n            [\r\n              1860860.35051921,\r\n              8402098.60030358\r\n            ],\r\n            [\r\n              1860844.70873312,\r\n              8402135.12137201\r\n            ],\r\n            [\r\n              1860838.04643859,\r\n              8402149.17280854\r\n            ],\r\n            [\r\n              1860837.76877667,\r\n              8402153.23087083\r\n            ],\r\n            [\r\n              1860836.25940634,\r\n              8402155.65864469\r\n            ],\r\n            [\r\n              1860838.40155533,\r\n              8402166.02247876\r\n            ],\r\n            [\r\n              1860839.45252156,\r\n              8402183.99593317\r\n            ],\r\n            [\r\n              1860839.68364596,\r\n              8402200.38144217\r\n            ],\r\n            [\r\n              1860847.59978984,\r\n              8402223.85399235\r\n            ],\r\n            [\r\n              1860853.81343759,\r\n              8402239.75530589\r\n            ],\r\n            [\r\n              1860859.2839969,\r\n              8402259.66513516\r\n            ],\r\n            [\r\n              1860976.58145298,\r\n              8402366.34331686\r\n            ],\r\n            [\r\n              1860992.42148693,\r\n              8402385.30869172\r\n            ],\r\n            [\r\n              1861079.69121631,\r\n              8402486.01194412\r\n            ],\r\n            [\r\n              1861053.52988147,\r\n              8402669.47145242\r\n            ],\r\n            [\r\n              1861406.44740439,\r\n              8403369.69545673\r\n            ],\r\n            [\r\n              1861543.22231374,\r\n              8404088.23303463\r\n            ],\r\n            [\r\n              1861491.34911128,\r\n              8404063.37929988\r\n            ],\r\n            [\r\n              1861439.19518058,\r\n              8404046.92381681\r\n            ],\r\n            [\r\n              1861396.31667217,\r\n              8404036.73626707\r\n            ],\r\n            [\r\n              1861343.10190552,\r\n              8404029.89295069\r\n            ],\r\n            [\r\n              1861303.56440204,\r\n              8404030.05237905\r\n            ],\r\n            [\r\n              1861193.0313466,\r\n              8404093.58976244\r\n            ],\r\n            [\r\n              1861190.83864532,\r\n              8404108.0134561\r\n            ],\r\n            [\r\n              1861192.31413183,\r\n              8404127.58542025\r\n            ],\r\n            [\r\n              1861199.75530203,\r\n              8404145.47327757\r\n            ],\r\n            [\r\n              1861210.4139137,\r\n              8404164.91443097\r\n            ],\r\n            [\r\n              1861230.32038961,\r\n              8404188.62476606\r\n            ],\r\n            [\r\n              1861246.16498415,\r\n              8404207.59242276\r\n            ],\r\n            [\r\n              1861261.55885747,\r\n              8404222.96870865\r\n            ],\r\n            [\r\n              1861278.98933568,\r\n              8404241.11639567\r\n            ],\r\n            [\r\n              1861296.67720995,\r\n              8404249.26214271\r\n            ],\r\n            [\r\n              1861314.41134237,\r\n              8404260.60680932\r\n            ],\r\n            [\r\n              1861336.53844989,\r\n              8404271.8904874\r\n            ],\r\n            [\r\n              1861362.00657832,\r\n              8404293.52015216\r\n            ],\r\n            [\r\n              1861398.30863475,\r\n              8404318.59721776\r\n            ],\r\n            [\r\n              1861423.31532997,\r\n              8404335.83786169\r\n            ],\r\n            [\r\n              1861461.31972917,\r\n              8404363.88878904\r\n            ],\r\n            [\r\n              1861488.26265481,\r\n              8404324.92096389\r\n            ],\r\n            [\r\n              1861566.07450366,\r\n              8404207.06240342\r\n            ],\r\n            [\r\n              1861629.99032783,\r\n              8404630.40261438\r\n            ],\r\n            [\r\n              1861677.92928044,\r\n              8405025.20427769\r\n            ],\r\n            [\r\n              1861663.87827792,\r\n              8405020.20380605\r\n            ],\r\n            [\r\n              1861650.65909566,\r\n              8405017.59173013\r\n            ],\r\n            [\r\n              1861633.83591344,\r\n              8405014.23156226\r\n            ],\r\n            [\r\n              1861620.2286869,\r\n              8405012.42659234\r\n            ],\r\n            [\r\n              1861599.45188634,\r\n              8405011.92113295\r\n            ],\r\n            [\r\n              1861579.07230269,\r\n              8405011.41023098\r\n            ],\r\n            [\r\n              1861567.08617327,\r\n              8405011.18060294\r\n            ],\r\n            [\r\n              1861553.0001638,\r\n              8405003.78252702\r\n            ],\r\n            [\r\n              1861538.46525038,\r\n              8404992.79097096\r\n            ],\r\n            [\r\n              1861521.86397473,\r\n              8404977.03035967\r\n            ],\r\n            [\r\n              1861509.64504354,\r\n              8404960.40901182\r\n            ],\r\n            [\r\n              1861489.72662756,\r\n              8404935.89911045\r\n            ],\r\n            [\r\n              1861468.43083716,\r\n              8404927.00301691\r\n            ],\r\n            [\r\n              1861449.54147247,\r\n              8404918.87357791\r\n            ],\r\n            [\r\n              1861431.43951311,\r\n              8404909.93129737\r\n            ],\r\n            [\r\n              1861419.35759523,\r\n              8404902.90519227\r\n            ],\r\n            [\r\n              1861217.76645203,\r\n              8404883.36424889\r\n            ],\r\n            [\r\n              1861279.47606869,\r\n              8405236.39451223\r\n            ],\r\n            [\r\n              1861311.565031,\r\n              8405243.92009319\r\n            ],\r\n            [\r\n              1861322.45717227,\r\n              8405253.38287277\r\n            ],\r\n            [\r\n              1861351.34883047,\r\n              8405262.57060263\r\n            ],\r\n            [\r\n              1861375.39588723,\r\n              8405268.22929999\r\n            ],\r\n            [\r\n              1861398.2895112,\r\n              8405277.10380991\r\n            ],\r\n            [\r\n              1861413.52935449,\r\n              8405281.28656142\r\n            ],\r\n            [\r\n              1861438.33437417,\r\n              8405284.1345791\r\n            ],\r\n            [\r\n              1861464.37988441,\r\n              8405289.76334737\r\n            ],\r\n            [\r\n              1861485.19810362,\r\n              8405293.06862694\r\n            ],\r\n            [\r\n              1861510.79256035,\r\n              8405295.10547839\r\n            ],\r\n            [\r\n              1861524.73683922,\r\n              8405292.50818574\r\n            ],\r\n            [\r\n              1861544.57561821,\r\n              8405283.02907042\r\n            ],\r\n            [\r\n              1861560.00966289,\r\n              8405272.81302473\r\n            ],\r\n            [\r\n              1861575.38086037,\r\n              8405258.19816915\r\n            ],\r\n            [\r\n              1861584.30488793,\r\n              8405239.67672692\r\n            ],\r\n            [\r\n              1861590.42321478,\r\n              8405220.39443293\r\n            ],\r\n            [\r\n              1861597.74468604,\r\n              8405201.49552806\r\n            ],\r\n            [\r\n              1861606.22501775,\r\n              8405179.78062728\r\n            ],\r\n            [\r\n              1861617.95115806,\r\n              8405161.61852516\r\n            ],\r\n            [\r\n              1861635.25167575,\r\n              8405142.17706672\r\n            ],\r\n            [\r\n              1861646.22878915,\r\n              8405127.62516829\r\n            ],\r\n            [\r\n              1861661.53114676,\r\n              8405108.21357333\r\n            ],\r\n            [\r\n              1861674.53065219,\r\n              8405095.23169282\r\n            ],\r\n            [\r\n              1861685.46692941,\r\n              8405077.88256865\r\n            ],\r\n            [\r\n              1861713.9871277,\r\n              8405314.21723999\r\n            ],\r\n            [\r\n              1861810.30714596,\r\n              8405823.94471766\r\n            ],\r\n            [\r\n              1861877.30894105,\r\n              8406181.34188207\r\n            ],\r\n            [\r\n              1862021.57145381,\r\n              8406914.03692942\r\n            ],\r\n            [\r\n              1862046.6183521,\r\n              8407270.07987328\r\n            ],\r\n            [\r\n              1862048.05315314,\r\n              8407370.86315215\r\n            ],\r\n            [\r\n              1862052.58857971,\r\n              8407436.80112621\r\n            ],\r\n            [\r\n              1862116.9050058,\r\n              8407828.31450683\r\n            ],\r\n            [\r\n              1862140.18506621,\r\n              8408508.48604239\r\n            ],\r\n            [\r\n              1862155.48792225,\r\n              8408881.14891484\r\n            ],\r\n            [\r\n              1862118.66566532,\r\n              8409183.78242214\r\n            ],\r\n            [\r\n              1862110.76882915,\r\n              8409248.68555696\r\n            ],\r\n            [\r\n              1862099.33872791,\r\n              8409350.98992563\r\n            ],\r\n            [\r\n              1862093.75526101,\r\n              8409400.97413714\r\n            ],\r\n            [\r\n              1862081.07613553,\r\n              8409408.75722352\r\n            ],\r\n            [\r\n              1861834.63698022,\r\n              8409814.0044824\r\n            ],\r\n            [\r\n              1861031.79337712,\r\n              8411128.77631685\r\n            ],\r\n            [\r\n              1860981.96863034,\r\n              8411209.49783546\r\n            ],\r\n            [\r\n              1860336.146705,\r\n              8412255.6888994\r\n            ],\r\n            [\r\n              1860536.07775548,\r\n              8412347.13249242\r\n            ],\r\n            [\r\n              1860601.33670847,\r\n              8412377.59245954\r\n            ],\r\n            [\r\n              1860604.47248008,\r\n              8412379.0566674\r\n            ],\r\n            [\r\n              1860966.9031357,\r\n              8412542.40384807\r\n            ],\r\n            [\r\n              1860841.95622642,\r\n              8412908.43408406\r\n            ],\r\n            [\r\n              1860768.61607999,\r\n              8413110.37514476\r\n            ],\r\n            [\r\n              1860614.79571863,\r\n              8413550.39902697\r\n            ],\r\n            [\r\n              1859862.24019395,\r\n              8415721.41197125\r\n            ],\r\n            [\r\n              1859575.15247096,\r\n              8416005.06255678\r\n            ],\r\n            [\r\n              1859480.04285973,\r\n              8416120.10530869\r\n            ],\r\n            [\r\n              1859456.57139175,\r\n              8416149.39021072\r\n            ],\r\n            [\r\n              1859425.67823717,\r\n              8416191.5585682\r\n            ],\r\n            [\r\n              1859383.88870174,\r\n              8416248.60636304\r\n            ],\r\n            [\r\n              1859237.58151488,\r\n              8416524.96093391\r\n            ],\r\n            [\r\n              1859143.95380202,\r\n              8416711.7977416\r\n            ],\r\n            [\r\n              1859135.10167426,\r\n              8416740.84766776\r\n            ],\r\n            [\r\n              1859121.74395467,\r\n              8416784.67921711\r\n            ],\r\n            [\r\n              1859115.34661625,\r\n              8416805.67179428\r\n            ],\r\n            [\r\n              1859103.08290586,\r\n              8416849.23242992\r\n            ],\r\n            [\r\n              1859097.19793034,\r\n              8416870.13405477\r\n            ],\r\n            [\r\n              1859094.37192413,\r\n              8416880.17578829\r\n            ],\r\n            [\r\n              1859078.06385874,\r\n              8416926.95251658\r\n            ],\r\n            [\r\n              1859070.84188013,\r\n              8416946.71874528\r\n            ],\r\n            [\r\n              1859056.35102722,\r\n              8416983.8461669\r\n            ],\r\n            [\r\n              1859031.99889487,\r\n              8417028.37283892\r\n            ],\r\n            [\r\n              1859002.7069319,\r\n              8417073.02651529\r\n            ],\r\n            [\r\n              1858993.02905751,\r\n              8417087.77837593\r\n            ],\r\n            [\r\n              1858987.29755578,\r\n              8417096.51753925\r\n            ],\r\n            [\r\n              1858927.5043904,\r\n              8417170.55781571\r\n            ],\r\n            [\r\n              1858893.21174796,\r\n              8417215.51189148\r\n            ],\r\n            [\r\n              1858882.23264302,\r\n              8417229.9029376\r\n            ],\r\n            [\r\n              1858811.91853744,\r\n              8417297.73376747\r\n            ],\r\n            [\r\n              1858765.14526212,\r\n              8417341.4823872\r\n            ],\r\n            [\r\n              1858726.34129527,\r\n              8417383.47884642\r\n            ],\r\n            [\r\n              1858649.87594899,\r\n              8417464.64598644\r\n            ],\r\n            [\r\n              1858545.37017465,\r\n              8417586.00388558\r\n            ],\r\n            [\r\n              1858487.56259562,\r\n              8417659.60789075\r\n            ],\r\n            [\r\n              1858405.80286651,\r\n              8417778.1325191\r\n            ],\r\n            [\r\n              1858345.5254121,\r\n              8417869.41188869\r\n            ],\r\n            [\r\n              1858298.4786535,\r\n              8417962.04478231\r\n            ],\r\n            [\r\n              1858262.73820379,\r\n              8418039.23868662\r\n            ],\r\n            [\r\n              1858205.75268755,\r\n              8418156.50036101\r\n            ],\r\n            [\r\n              1858173.12664463,\r\n              8418229.2304859\r\n            ],\r\n            [\r\n              1858141.15809845,\r\n              8418315.57182655\r\n            ],\r\n            [\r\n              1858157.23241304,\r\n              8418324.33954484\r\n            ],\r\n            [\r\n              1858290.74179623,\r\n              8418397.16963114\r\n            ],\r\n            [\r\n              1858618.6240744,\r\n              8418576.01424477\r\n            ],\r\n            [\r\n              1860612.414284,\r\n              8419662.3804609\r\n            ],\r\n            [\r\n              1860265.93013146,\r\n              8421218.68016483\r\n            ],\r\n            [\r\n              1859610.17439154,\r\n              8424036.26672874\r\n            ],\r\n            [\r\n              1861067.98013692,\r\n              8424302.83386972\r\n            ],\r\n            [\r\n              1861364.76807372,\r\n              8424349.83720462\r\n            ],\r\n            [\r\n              1862222.3442207,\r\n              8424502.94168916\r\n            ],\r\n            [\r\n              1862335.14003784,\r\n              8424596.64741043\r\n            ],\r\n            [\r\n              1862574.52380935,\r\n              8424798.73034187\r\n            ],\r\n            [\r\n              1862631.95340753,\r\n              8424847.77477408\r\n            ],\r\n            [\r\n              1862623.12091797,\r\n              8425238.72408611\r\n            ],\r\n            [\r\n              1862622.5231646,\r\n              8425265.11702148\r\n            ],\r\n            [\r\n              1862617.58366542,\r\n              8425483.75785146\r\n            ],\r\n            [\r\n              1862958.67640788,\r\n              8425994.99746379\r\n            ],\r\n            [\r\n              1862096.64846223,\r\n              8426696.99570592\r\n            ],\r\n            [\r\n              1864301.04144246,\r\n              8430066.78888528\r\n            ],\r\n            [\r\n              1863627.18059344,\r\n              8431856.46355589\r\n            ],\r\n            [\r\n              1863515.77096662,\r\n              8432163.25642287\r\n            ],\r\n            [\r\n              1863471.69463761,\r\n              8432284.6288101\r\n            ],\r\n            [\r\n              1863467.86092066,\r\n              8432320.41042233\r\n            ],\r\n            [\r\n              1863460.79836837,\r\n              8432357.14750391\r\n            ],\r\n            [\r\n              1863452.69204397,\r\n              8432399.31508011\r\n            ],\r\n            [\r\n              1863438.79131828,\r\n              8432457.32837693\r\n            ],\r\n            [\r\n              1863421.91994253,\r\n              8432505.3573738\r\n            ],\r\n            [\r\n              1863400.62723418,\r\n              8432540.01608305\r\n            ],\r\n            [\r\n              1863386.49480862,\r\n              8432563.01950951\r\n            ],\r\n            [\r\n              1863371.12203414,\r\n              8432588.03970523\r\n            ],\r\n            [\r\n              1863333.04194145,\r\n              8432638.8123052\r\n            ],\r\n            [\r\n              1863309.97640676,\r\n              8432675.7026125\r\n            ],\r\n            [\r\n              1863300.92840182,\r\n              8432711.5678181\r\n            ],\r\n            [\r\n              1863282.14284734,\r\n              8432765.24601438\r\n            ],\r\n            [\r\n              1863276.07663407,\r\n              8432787.01578699\r\n            ],\r\n            [\r\n              1863272.72610724,\r\n              8432803.12429623\r\n            ],\r\n            [\r\n              1863276.22439341,\r\n              8432821.13165762\r\n            ],\r\n            [\r\n              1863278.48249755,\r\n              8432836.74951876\r\n            ],\r\n            [\r\n              1863272.77892759,\r\n              8432856.10547281\r\n            ],\r\n            [\r\n              1863262.67097981,\r\n              8432875.93308552\r\n            ],\r\n            [\r\n              1863263.80822859,\r\n              8432896.78614983\r\n            ],\r\n            [\r\n              1863263.21903759,\r\n              8432934.9259876\r\n            ],\r\n            [\r\n              1863267.65520664,\r\n              8432986.23198781\r\n            ],\r\n            [\r\n              1863268.50198668,\r\n              8433013.91502523\r\n            ],\r\n            [\r\n              1863279.68838684,\r\n              8433036.21443548\r\n            ],\r\n            [\r\n              1863293.13306895,\r\n              8433074.13251459\r\n            ],\r\n            [\r\n              1863301.05284059,\r\n              8433092.87280352\r\n            ],\r\n            [\r\n              1863305.93009631,\r\n              8433121.69573342\r\n            ],\r\n            [\r\n              1863305.4247725,\r\n              8433140.16788753\r\n            ],\r\n            [\r\n              1863303.72489541,\r\n              8433159.0603653\r\n            ],\r\n            [\r\n              1863305.98107922,\r\n              8433174.67896087\r\n            ],\r\n            [\r\n              1863313.93524087,\r\n              8433195.42475277\r\n            ],\r\n            [\r\n              1863331.16952573,\r\n              8433219.63666986\r\n            ],\r\n            [\r\n              1863341.95576786,\r\n              8433241.944227\r\n            ],\r\n            [\r\n              1863353.6593219,\r\n              8433271.46231482\r\n            ],\r\n            [\r\n              1863360.72866816,\r\n              8433287.00497146\r\n            ],\r\n            [\r\n              1863361.58921202,\r\n              8433315.489992\r\n            ],\r\n            [\r\n              1863357.5720696,\r\n              8433340.03892731\r\n            ],\r\n            [\r\n              1863350.98326937,\r\n              8433354.19298979\r\n            ],\r\n            [\r\n              1863348.49312364,\r\n              8433373.90035453\r\n            ],\r\n            [\r\n              1863353.6593987,\r\n              8433395.89656753\r\n            ],\r\n            [\r\n              1863363.5988635,\r\n              8433415.40802834\r\n            ],\r\n            [\r\n              1863377.89165823,\r\n              8433431.23654932\r\n            ],\r\n            [\r\n              1863384.71442855,\r\n              8433456.41743187\r\n            ],\r\n            [\r\n              1863380.97088498,\r\n              8433472.93459086\r\n            ],\r\n            [\r\n              1863375.47198695,\r\n              8433505.13393698\r\n            ],\r\n            [\r\n              1863376.63647019,\r\n              8433527.59458884\r\n            ],\r\n            [\r\n              1863375.38298111,\r\n              8433549.2905152\r\n            ],\r\n            [\r\n              1863376.50738993,\r\n              8433569.34273383\r\n            ],\r\n            [\r\n              1863361.93156352,\r\n              8433585.63186488\r\n            ],\r\n            [\r\n              1863351.4675985,\r\n              8433608.2753618\r\n            ],\r\n            [\r\n              1863336.05567338,\r\n              8433647.45738461\r\n            ],\r\n            [\r\n              1863333.17032641,\r\n              8433692.46158405\r\n            ],\r\n            [\r\n              1863338.06704185,\r\n              8433722.49108763\r\n            ],\r\n            [\r\n              1863356.43278156,\r\n              8433742.27177154\r\n            ],\r\n            [\r\n              1863370.05397639,\r\n              8433766.1411672\r\n            ],\r\n            [\r\n              1863372.73024708,\r\n              8433782.95766437\r\n            ],\r\n            [\r\n              1863371.73509431,\r\n              8433795.81887746\r\n            ],\r\n            [\r\n              1863362.48656027,\r\n              8433819.24878191\r\n            ],\r\n            [\r\n              1863347.43695419,\r\n              8433831.12829263\r\n            ],\r\n            [\r\n              1863332.11588195,\r\n              8433851.04237698\r\n            ],\r\n            [\r\n              1863319.56765236,\r\n              8433868.90364845\r\n            ],\r\n            [\r\n              1863305.00987207,\r\n              8433911.28318395\r\n            ],\r\n            [\r\n              1863290.15341033,\r\n              8433960.09210782\r\n            ],\r\n            [\r\n              1863279.75326602,\r\n              8434011.64101058\r\n            ],\r\n            [\r\n              1863269.20471716,\r\n              8434053.95875459\r\n            ],\r\n            [\r\n              1863249.58761052,\r\n              8434106.05604704\r\n            ],\r\n            [\r\n              1863211.01203331,\r\n              8434176.51755901\r\n            ],\r\n            [\r\n              1863171.13668851,\r\n              8434240.97861276\r\n            ],\r\n            [\r\n              1863135.15508778,\r\n              8434298.15276333\r\n            ],\r\n            [\r\n              1863098.5002034,\r\n              8434363.36614618\r\n            ],\r\n            [\r\n              1863082.69786021,\r\n              8434403.36038643\r\n            ],\r\n            [\r\n              1863057.12657305,\r\n              8434459.56738879\r\n            ],\r\n            [\r\n              1863038.75781731,\r\n              8434489.56474204\r\n            ],\r\n            [\r\n              1863012.59750544,\r\n              8434534.13898641\r\n            ],\r\n            [\r\n              1862989.09133737,\r\n              8434569.03594001\r\n            ],\r\n            [\r\n              1862963.25576177,\r\n              8434608.78892072\r\n            ],\r\n            [\r\n              1862946.74834509,\r\n              8434629.92565903\r\n            ],\r\n            [\r\n              1862946.11957773,\r\n              8434665.66729653\r\n            ],\r\n            [\r\n              1862944.33173413,\r\n              8434704.23658489\r\n            ],\r\n            [\r\n              1862935.11156624,\r\n              8434754.56675244\r\n            ],\r\n            [\r\n              1862917.43761666,\r\n              8434803.02240064\r\n            ],\r\n            [\r\n              1862895.10465876,\r\n              8434861.18826707\r\n            ],\r\n            [\r\n              1862887.07116622,\r\n              8434885.40504435\r\n            ],\r\n            [\r\n              1862891.68719878,\r\n              8434923.07192454\r\n            ],\r\n            [\r\n              1862891.56813434,\r\n              8434940.73795206\r\n            ],\r\n            [\r\n              1862876.7539765,\r\n              8434967.47008571\r\n            ],\r\n            [\r\n              1862873.55695619,\r\n              8434993.21544971\r\n            ],\r\n            [\r\n              1862877.84971255,\r\n              8435035.70665826\r\n            ],\r\n            [\r\n              1862883.96705535,\r\n              8435066.92568871\r\n            ],\r\n            [\r\n              1862890.26408404,\r\n              8435109.3851795\r\n            ],\r\n            [\r\n              1862885.10059454,\r\n              8435137.5724212\r\n            ],\r\n            [\r\n              1862877.6528931,\r\n              8435173.42255912\r\n            ],\r\n            [\r\n              1862876.79651517,\r\n              8435195.11749577\r\n            ],\r\n            [\r\n              1862878.84667359,\r\n              8435222.78785335\r\n            ],\r\n            [\r\n              1862879.32933285,\r\n              8435252.89411124\r\n            ],\r\n            [\r\n              1862873.91929806,\r\n              8435390.6954165\r\n            ],\r\n            [\r\n              1862862.99264284,\r\n              8435484.82352744\r\n            ],\r\n            [\r\n              1862854.69973588,\r\n              8435568.06746887\r\n            ],\r\n            [\r\n              1862836.56210618,\r\n              8435637.81737123\r\n            ],\r\n            [\r\n              1862833.23028852,\r\n              8435655.13671307\r\n            ],\r\n            [\r\n              1862812.66949959,\r\n              8435673.9294745\r\n            ],\r\n            [\r\n              1862789.66569332,\r\n              8435690.35473268\r\n            ],\r\n            [\r\n              1862767.85112051,\r\n              8435705.95590465\r\n            ],\r\n            [\r\n              1862756.39907897,\r\n              8435717.37901665\r\n            ],\r\n            [\r\n              1862747.37321587,\r\n              8435729.9688199\r\n            ],\r\n            [\r\n              1862731.11974984,\r\n              8435767.16640142\r\n            ],\r\n            [\r\n              1862720.89669933,\r\n              8435805.07063812\r\n            ],\r\n            [\r\n              1862715.07006199,\r\n              8435842.10396103\r\n            ],\r\n            [\r\n              1862714.68379719,\r\n              8435893.10672199\r\n            ],\r\n            [\r\n              1862712.6576001,\r\n              8435991.91756015\r\n            ],\r\n            [\r\n              1862715.19846286,\r\n              8436075.39845027\r\n            ],\r\n            [\r\n              1862717.36289878,\r\n              8436185.38935189\r\n            ],\r\n            [\r\n              1862726.07454518,\r\n              8436278.41385186\r\n            ],\r\n            [\r\n              1862730.14173469,\r\n              8436381.95226589\r\n            ],\r\n            [\r\n              1862779.50561791,\r\n              8436708.05689368\r\n            ],\r\n            [\r\n              1862649.27315716,\r\n              8437044.23105946\r\n            ],\r\n            [\r\n              1862450.22900856,\r\n              8437569.86727179\r\n            ],\r\n            [\r\n              1862314.95415962,\r\n              8437858.73763612\r\n            ],\r\n            [\r\n              1862054.00372386,\r\n              8438415.94160589\r\n            ],\r\n            [\r\n              1861947.8710009,\r\n              8438354.14018852\r\n            ],\r\n            [\r\n              1861691.92540539,\r\n              8438992.00884342\r\n            ],\r\n            [\r\n              1861739.25832146,\r\n              8439040.67930594\r\n            ],\r\n            [\r\n              1861698.55722728,\r\n              8439207.22169495\r\n            ],\r\n            [\r\n              1861018.20077346,\r\n              8440704.32636623\r\n            ],\r\n            [\r\n              1860958.28490662,\r\n              8440830.13571209\r\n            ],\r\n            [\r\n              1860868.59700076,\r\n              8441018.45130952\r\n            ],\r\n            [\r\n              1860766.06915772,\r\n              8441085.13552517\r\n            ],\r\n            [\r\n              1860651.73232662,\r\n              8441166.8692918\r\n            ],\r\n            [\r\n              1860569.5556762,\r\n              8441226.00447984\r\n            ],\r\n            [\r\n              1860526.3168667,\r\n              8441259.2211011\r\n            ],\r\n            [\r\n              1860443.08137554,\r\n              8441327.6128128\r\n            ],\r\n            [\r\n              1860393.60752785,\r\n              8441372.98127223\r\n            ],\r\n            [\r\n              1860369.50845196,\r\n              8441397.86529642\r\n            ],\r\n            [\r\n              1860351.00942589,\r\n              8441421.45668535\r\n            ],\r\n            [\r\n              1860327.32769074,\r\n              8441447.54091128\r\n            ],\r\n            [\r\n              1860301.08395759,\r\n              8441489.33438971\r\n            ],\r\n            [\r\n              1860281.53284705,\r\n              8441522.58707635\r\n            ],\r\n            [\r\n              1860262.87096789,\r\n              8441561.45306555\r\n            ],\r\n            [\r\n              1860241.70040729,\r\n              8441619.64578521\r\n            ],\r\n            [\r\n              1860226.3757204,\r\n              8441666.49554296\r\n            ],\r\n            [\r\n              1860203.90683348,\r\n              8441744.39894037\r\n            ],\r\n            [\r\n              1860175.80439154,\r\n              8441846.90476922\r\n            ],\r\n            [\r\n              1860154.33009898,\r\n              8441962.5702502\r\n            ],\r\n            [\r\n              1860090.50601675,\r\n              8442117.47059984\r\n            ],\r\n            [\r\n              1860049.37545013,\r\n              8442183.20854086\r\n            ],\r\n            [\r\n              1860000.4234625,\r\n              8442255.22834143\r\n            ],\r\n            [\r\n              1859798.71981673,\r\n              8442750.15072561\r\n            ],\r\n            [\r\n              1859775.67110314,\r\n              8442920.17689161\r\n            ],\r\n            [\r\n              1859785.86154507,\r\n              8442996.37155211\r\n            ],\r\n            [\r\n              1859793.17857204,\r\n              8443046.48792104\r\n            ],\r\n            [\r\n              1859796.55425432,\r\n              8443078.18263361\r\n            ],\r\n            [\r\n              1859795.19330921,\r\n              8443091.87457237\r\n            ],\r\n            [\r\n              1859787.85067301,\r\n              8443107.67865396\r\n            ],\r\n            [\r\n              1859781.22286718,\r\n              8443118.64838697\r\n            ],\r\n            [\r\n              1859774.5391244,\r\n              8443126.40257163\r\n            ],\r\n            [\r\n              1859757.8195185,\r\n              8443134.92111007\r\n            ],\r\n            [\r\n              1859595.92645474,\r\n              8443217.40235485\r\n            ],\r\n            [\r\n              1859579.62269519,\r\n              8443268.662638\r\n            ],\r\n            [\r\n              1859568.80985612,\r\n              8443302.66465328\r\n            ],\r\n            [\r\n              1859592.19937649,\r\n              8443318.98670073\r\n            ],\r\n            [\r\n              1859761.9741132,\r\n              8443440.1370508\r\n            ],\r\n            [\r\n              1859824.69489372,\r\n              8443484.891996\r\n            ],\r\n            [\r\n              1859881.47289669,\r\n              8443517.66386716\r\n            ],\r\n            [\r\n              1859923.85525695,\r\n              8443530.9895728\r\n            ],\r\n            [\r\n              1859967.35782307,\r\n              8443539.4728806\r\n            ],\r\n            [\r\n              1860236.26200734,\r\n              8443538.37304794\r\n            ],\r\n            [\r\n              1860303.59176498,\r\n              8443532.76673159\r\n            ],\r\n            [\r\n              1860375.27961307,\r\n              8443523.86955584\r\n            ],\r\n            [\r\n              1860911.94647055,\r\n              8443257.94702623\r\n            ],\r\n            [\r\n              1861352.31393496,\r\n              8443372.75972685\r\n            ],\r\n            [\r\n              1861440.99328207,\r\n              8443393.69813553\r\n            ],\r\n            [\r\n              1861591.16679614,\r\n              8443419.17216974\r\n            ],\r\n            [\r\n              1861706.52766665,\r\n              8443428.37847746\r\n            ],\r\n            [\r\n              1861764.69375275,\r\n              8443426.54094009\r\n            ],\r\n            [\r\n              1861819.56240305,\r\n              8443419.93859924\r\n            ],\r\n            [\r\n              1861894.27373926,\r\n              8443400.92077764\r\n            ],\r\n            [\r\n              1862106.73415061,\r\n              8443342.0664716\r\n            ],\r\n            [\r\n              1862807.78488524,\r\n              8443312.25214623\r\n            ],\r\n            [\r\n              1864187.89614514,\r\n              8442196.71103197\r\n            ],\r\n            [\r\n              1864608.31156748,\r\n              8441856.80565189\r\n            ],\r\n            [\r\n              1867833.89737581,\r\n              8442143.81019726\r\n            ],\r\n            [\r\n              1868588.75435834,\r\n              8443460.83870997\r\n            ],\r\n            [\r\n              1869803.38932772,\r\n              8443880.24271434\r\n            ],\r\n            [\r\n              1870108.30730675,\r\n              8444307.7507998\r\n            ],\r\n            [\r\n              1869889.30276365,\r\n              8445254.20387861\r\n            ],\r\n            [\r\n              1870195.55561631,\r\n              8445754.42564461\r\n            ],\r\n            [\r\n              1870457.59520187,\r\n              8447117.75824429\r\n            ],\r\n            [\r\n              1872716.60781412,\r\n              8448161.19537314\r\n            ],\r\n            [\r\n              1874823.20484227,\r\n              8448173.9398001\r\n            ],\r\n            [\r\n              1874831.9939202,\r\n              8448189.50048327\r\n            ],\r\n            [\r\n              1874861.20550092,\r\n              8448239.17925749\r\n            ],\r\n            [\r\n              1875718.71992472,\r\n              8449763.57144542\r\n            ],\r\n            [\r\n              1875773.38078439,\r\n              8449860.72562298\r\n            ],\r\n            [\r\n              1875821.48387575,\r\n              8449867.6957664\r\n            ],\r\n            [\r\n              1876045.0541219,\r\n              8449900.08236389\r\n            ],\r\n            [\r\n              1876195.76564863,\r\n              8449921.91101703\r\n            ],\r\n            [\r\n              1876205.28559468,\r\n              8449951.20676889\r\n            ],\r\n            [\r\n              1876217.19380025,\r\n              8449978.46659832\r\n            ],\r\n            [\r\n              1876171.99893421,\r\n              8449999.42922181\r\n            ],\r\n            [\r\n              1876181.18140155,\r\n              8450035.57197303\r\n            ],\r\n            [\r\n              1876179.69215248,\r\n              8450048.0669907\r\n            ],\r\n            [\r\n              1876174.63140035,\r\n              8450065.02123136\r\n            ],\r\n            [\r\n              1876163.09712165,\r\n              8450077.20910881\r\n            ],\r\n            [\r\n              1876140.68362136,\r\n              8450085.87758155\r\n            ],\r\n            [\r\n              1876079.06682305,\r\n              8450112.22983406\r\n            ],\r\n            [\r\n              1876122.98566843,\r\n              8450168.17037641\r\n            ],\r\n            [\r\n              1876253.72106133,\r\n              8450221.91129496\r\n            ],\r\n            [\r\n              1876270.16657858,\r\n              8450176.13415184\r\n            ],\r\n            [\r\n              1876321.44227346,\r\n              8450033.3345221\r\n            ],\r\n            [\r\n              1876342.10457347,\r\n              8449953.32942282\r\n            ],\r\n            [\r\n              1876407.42443813,\r\n              8449958.93109472\r\n            ],\r\n            [\r\n              1876439.04563494,\r\n              8450031.08912162\r\n            ],\r\n            [\r\n              1876525.35132278,\r\n              8450066.086619\r\n            ],\r\n            [\r\n              1876537.6061492,\r\n              8450045.43776559\r\n            ],\r\n            [\r\n              1876474.59244476,\r\n              8450009.41127452\r\n            ],\r\n            [\r\n              1876479.05936332,\r\n              8449972.33297348\r\n            ],\r\n            [\r\n              1877350.45538025,\r\n              8450102.79335537\r\n            ],\r\n            [\r\n              1878056.78265883,\r\n              8450522.62288825\r\n            ],\r\n            [\r\n              1878607.17109212,\r\n              8451255.58520585\r\n            ],\r\n            [\r\n              1879532.86097027,\r\n              8451815.30337077\r\n            ],\r\n            [\r\n              1879875.36915594,\r\n              8452012.79953015\r\n            ],\r\n            [\r\n              1879972.92081525,\r\n              8452072.83516035\r\n            ],\r\n            [\r\n              1881844.90595761,\r\n              8453251.48189037\r\n            ],\r\n            [\r\n              1881261.58121479,\r\n              8453993.98255274\r\n            ],\r\n            [\r\n              1881092.93656648,\r\n              8454208.70715147\r\n            ],\r\n            [\r\n              1880927.36379768,\r\n              8454427.4681241\r\n            ],\r\n            [\r\n              1880419.8503454,\r\n              8454110.16046165\r\n            ],\r\n            [\r\n              1880010.42095236,\r\n              8454363.89777735\r\n            ],\r\n            [\r\n              1879987.21167755,\r\n              8454378.27353006\r\n            ],\r\n            [\r\n              1879915.37424586,\r\n              8454766.53375644\r\n            ],\r\n            [\r\n              1879912.83615921,\r\n              8454792.00522823\r\n            ],\r\n            [\r\n              1879849.63098801,\r\n              8455426.32716008\r\n            ],\r\n            [\r\n              1879825.73319562,\r\n              8455666.15198855\r\n            ],\r\n            [\r\n              1879810.59187836,\r\n              8455818.09234588\r\n            ],\r\n            [\r\n              1879807.15504398,\r\n              8455852.58543681\r\n            ],\r\n            [\r\n              1879766.24170827,\r\n              8456409.12529875\r\n            ],\r\n            [\r\n              1879790.20822966,\r\n              8456711.1995562\r\n            ],\r\n            [\r\n              1879870.99544611,\r\n              8456826.15757431\r\n            ],\r\n            [\r\n              1879910.56962406,\r\n              8456882.46824652\r\n            ],\r\n            [\r\n              1879976.76067149,\r\n              8457009.94498737\r\n            ],\r\n            [\r\n              1880056.2585286,\r\n              8457163.04555468\r\n            ],\r\n            [\r\n              1880101.89818424,\r\n              8457259.96128143\r\n            ],\r\n            [\r\n              1880301.14501996,\r\n              8457683.03910902\r\n            ],\r\n            [\r\n              1880349.14963563,\r\n              8457784.96680571\r\n            ],\r\n            [\r\n              1880489.7585867,\r\n              8457887.53619531\r\n            ],\r\n            [\r\n              1880639.86050166,\r\n              8457997.02811389\r\n            ],\r\n            [\r\n              1880863.30964729,\r\n              8458207.79606567\r\n            ],\r\n            [\r\n              1881627.76718396,\r\n              8459270.92370151\r\n            ],\r\n            [\r\n              1881547.17371903,\r\n              8459603.27679228\r\n            ],\r\n            [\r\n              1882074.26777952,\r\n              8459868.09862354\r\n            ],\r\n            [\r\n              1882143.28068641,\r\n              8459902.7689616\r\n            ],\r\n            [\r\n              1882224.87449209,\r\n              8459905.95564999\r\n            ],\r\n            [\r\n              1882483.05120054,\r\n              8459916.03146754\r\n            ],\r\n            [\r\n              1882509.32871159,\r\n              8459917.05773375\r\n            ],\r\n            [\r\n              1882618.17239169,\r\n              8459921.30221308\r\n            ],\r\n            [\r\n              1882653.2541954,\r\n              8459958.02631941\r\n            ],\r\n            [\r\n              1882761.207848,\r\n              8460290.51387691\r\n            ],\r\n            [\r\n              1882749.21242311,\r\n              8460427.67106586\r\n            ],\r\n            [\r\n              1882747.44652654,\r\n              8460447.8647396\r\n            ],\r\n            [\r\n              1882737.61694579,\r\n              8460560.26380607\r\n            ],\r\n            [\r\n              1882732.97368322,\r\n              8460573.10306654\r\n            ],\r\n            [\r\n              1882687.92096595,\r\n              8460697.67624154\r\n            ],\r\n            [\r\n              1882667.2822845,\r\n              8460754.74715286\r\n            ],\r\n            [\r\n              1882649.00430929,\r\n              8460805.28413025\r\n            ],\r\n            [\r\n              1882640.27574009,\r\n              8460817.69462204\r\n            ],\r\n            [\r\n              1882514.8247918,\r\n              8460996.06404049\r\n            ],\r\n            [\r\n              1882489.29825071,\r\n              8461147.21038745\r\n            ],\r\n            [\r\n              1882541.93921224,\r\n              8461351.4558915\r\n            ],\r\n            [\r\n              1882551.82370242,\r\n              8461465.86736326\r\n            ],\r\n            [\r\n              1882549.24607445,\r\n              8461528.89829954\r\n            ],\r\n            [\r\n              1882513.93664244,\r\n              8461622.55506973\r\n            ],\r\n            [\r\n              1882352.18249087,\r\n              8462042.01495281\r\n            ],\r\n            [\r\n              1882339.85038796,\r\n              8462361.89738839\r\n            ],\r\n            [\r\n              1882287.70613035,\r\n              8462678.76793725\r\n            ],\r\n            [\r\n              1882860.15938222,\r\n              8462988.01003755\r\n            ],\r\n            [\r\n              1882865.92792096,\r\n              8462991.31550299\r\n            ],\r\n            [\r\n              1883558.74409584,\r\n              8463388.16537378\r\n            ],\r\n            [\r\n              1883634.64314671,\r\n              8463431.63576679\r\n            ],\r\n            [\r\n              1883604.42223693,\r\n              8463450.3964524\r\n            ],\r\n            [\r\n              1883553.51460588,\r\n              8463481.67456719\r\n            ],\r\n            [\r\n              1883490.19340704,\r\n              8463516.43199125\r\n            ],\r\n            [\r\n              1883441.14347921,\r\n              8463540.00981786\r\n            ],\r\n            [\r\n              1883453.17121306,\r\n              8463557.50969862\r\n            ],\r\n            [\r\n              1883472.2432196,\r\n              8463584.94751995\r\n            ],\r\n            [\r\n              1883492.19545932,\r\n              8463605.49845133\r\n            ],\r\n            [\r\n              1883519.88775952,\r\n              8463591.64093372\r\n            ],\r\n            [\r\n              1883580.35063057,\r\n              8463554.92545993\r\n            ],\r\n            [\r\n              1883624.00541383,\r\n              8463523.39077326\r\n            ],\r\n            [\r\n              1883655.73345542,\r\n              8463499.35706875\r\n            ],\r\n            [\r\n              1883681.99955732,\r\n              8463481.28195765\r\n            ],\r\n            [\r\n              1883699.81446662,\r\n              8463469.02253228\r\n            ],\r\n            [\r\n              1883940.2986647,\r\n              8463605.96580395\r\n            ],\r\n            [\r\n              1884000.37377908,\r\n              8463640.17545354\r\n            ],\r\n            [\r\n              1884555.17149501,\r\n              8463956.05783454\r\n            ],\r\n            [\r\n              1884664.87496259,\r\n              8463956.96209916\r\n            ],\r\n            [\r\n              1884813.47406319,\r\n              8463958.18621615\r\n            ],\r\n            [\r\n              1885674.56268501,\r\n              8463965.21586483\r\n            ],\r\n            [\r\n              1885782.94187845,\r\n              8463966.09405599\r\n            ],\r\n            [\r\n              1886014.51531523,\r\n              8463967.9628909\r\n            ],\r\n            [\r\n              1886602.5577488,\r\n              8463972.67834222\r\n            ],\r\n            [\r\n              1886749.08964988,\r\n              8463973.84450387\r\n            ],\r\n            [\r\n              1886824.74106919,\r\n              8463974.44657416\r\n            ],\r\n            [\r\n              1886913.70786681,\r\n              8463975.15329526\r\n            ],\r\n            [\r\n              1886960.03520636,\r\n              8463975.52141241\r\n            ],\r\n            [\r\n              1887133.70142366,\r\n              8463976.89578726\r\n            ],\r\n            [\r\n              1887809.11745332,\r\n              8463982.20466658\r\n            ],\r\n            [\r\n              1887974.40199772,\r\n              8463983.49398341\r\n            ],\r\n            [\r\n              1888173.58968646,\r\n              8463983.96694163\r\n            ],\r\n            [\r\n              1888226.97751646,\r\n              8464052.66621345\r\n            ],\r\n            [\r\n              1888394.08824327,\r\n              8464267.69744663\r\n            ],\r\n            [\r\n              1888345.12750651,\r\n              8464489.33332017\r\n            ],\r\n            [\r\n              1888332.42175451,\r\n              8464633.98650523\r\n            ],\r\n            [\r\n              1888090.85097525,\r\n              8465122.64608767\r\n            ],\r\n            [\r\n              1888738.50572156,\r\n              8465281.68671886\r\n            ],\r\n            [\r\n              1888936.30705133,\r\n              8465768.42296216\r\n            ],\r\n            [\r\n              1890263.0247512,\r\n              8465773.15049889\r\n            ],\r\n            [\r\n              1891542.57423188,\r\n              8464029.79871446\r\n            ],\r\n            [\r\n              1891553.66449911,\r\n              8464014.68663957\r\n            ],\r\n            [\r\n              1891580.03193026,\r\n              8463941.55965479\r\n            ],\r\n            [\r\n              1891695.1142584,\r\n              8463622.39390992\r\n            ],\r\n            [\r\n              1891745.42553148,\r\n              8463482.84924436\r\n            ],\r\n            [\r\n              1892321.24869972,\r\n              8461868.96893306\r\n            ],\r\n            [\r\n              1892487.1654406,\r\n              8461394.31026524\r\n            ],\r\n            [\r\n              1892493.61381771,\r\n              8461375.84224193\r\n            ],\r\n            [\r\n              1892661.11538568,\r\n              8461210.80038473\r\n            ],\r\n            [\r\n              1894149.05899516,\r\n              8460809.85728189\r\n            ],\r\n            [\r\n              1894859.23494396,\r\n              8460814.53183241\r\n            ],\r\n            [\r\n              1897069.36218551,\r\n              8461303.62775692\r\n            ],\r\n            [\r\n              1898940.3054126,\r\n              8460763.13429342\r\n            ],\r\n            [\r\n              1900204.93305003,\r\n              8459298.04846573\r\n            ],\r\n            [\r\n              1901605.5285154,\r\n              8457674.75118952\r\n            ],\r\n            [\r\n              1901992.86388948,\r\n              8457934.04360502\r\n            ],\r\n            [\r\n              1902020.02720775,\r\n              8457952.22728893\r\n            ],\r\n            [\r\n              1904196.11608733,\r\n              8459408.38964751\r\n            ],\r\n            [\r\n              1904999.54257703,\r\n              8459945.77701132\r\n            ],\r\n            [\r\n              1904987.55469199,\r\n              8459955.87024389\r\n            ],\r\n            [\r\n              1904976.68310993,\r\n              8459965.02341809\r\n            ],\r\n            [\r\n              1904683.67572437,\r\n              8460211.71068577\r\n            ],\r\n            [\r\n              1904676.08338349,\r\n              8460218.10252636\r\n            ],\r\n            [\r\n              1904202.53757815,\r\n              8461643.43506605\r\n            ],\r\n            [\r\n              1903907.96647046,\r\n              8462529.84961005\r\n            ],\r\n            [\r\n              1903914.82544424,\r\n              8462533.48224245\r\n            ],\r\n            [\r\n              1904695.35010431,\r\n              8462946.79868815\r\n            ],\r\n            [\r\n              1904881.85687408,\r\n              8463043.55311012\r\n            ],\r\n            [\r\n              1905476.04247092,\r\n              8463439.99804878\r\n            ],\r\n            [\r\n              1905756.62115194,\r\n              8463627.1779538\r\n            ],\r\n            [\r\n              1906954.34723495,\r\n              8464122.32724069\r\n            ],\r\n            [\r\n              1909961.57340419,\r\n              8464009.78215136\r\n            ],\r\n            [\r\n              1909969.36192701,\r\n              8464009.49511533\r\n            ],\r\n            [\r\n              1910366.56888267,\r\n              8464397.41909238\r\n            ],\r\n            [\r\n              1911104.62161911,\r\n              8465118.09714836\r\n            ],\r\n            [\r\n              1912548.69809824,\r\n              8466527.75602732\r\n            ],\r\n            [\r\n              1912619.09926965,\r\n              8466551.38971188\r\n            ],\r\n            [\r\n              1912772.50564514,\r\n              8466602.88413998\r\n            ],\r\n            [\r\n              1914006.85253629,\r\n              8467017.08583556\r\n            ],\r\n            [\r\n              1914052.92460077,\r\n              8467149.7621052\r\n            ],\r\n            [\r\n              1914113.80974907,\r\n              8467325.05886569\r\n            ],\r\n            [\r\n              1914126.11541098,\r\n              8467360.50442185\r\n            ],\r\n            [\r\n              1914518.24149304,\r\n              8468489.44674485\r\n            ],\r\n            [\r\n              1914550.14649973,\r\n              8468581.33431184\r\n            ],\r\n            [\r\n              1914549.09984617,\r\n              8468624.65048341\r\n            ],\r\n            [\r\n              1914513.01201638,\r\n              8470104.66136435\r\n            ],\r\n            [\r\n              1914508.94609009,\r\n              8470271.20713652\r\n            ],\r\n            [\r\n              1914506.39926593,\r\n              8470372.6860667\r\n            ],\r\n            [\r\n              1914487.28083347,\r\n              8471132.33733526\r\n            ],\r\n            [\r\n              1914476.63134421,\r\n              8471517.26079278\r\n            ],\r\n            [\r\n              1914473.09735516,\r\n              8471645.27815965\r\n            ],\r\n            [\r\n              1914464.71464943,\r\n              8471972.26995191\r\n            ],\r\n            [\r\n              1914440.27575233,\r\n              8472924.7401224\r\n            ],\r\n            [\r\n              1914418.27008141,\r\n              8473782.55116794\r\n            ],\r\n            [\r\n              1914418.00437493,\r\n              8473792.79414019\r\n            ],\r\n            [\r\n              1914416.46533858,\r\n              8473853.11070972\r\n            ],\r\n            [\r\n              1914415.33168714,\r\n              8473897.16815118\r\n            ],\r\n            [\r\n              1914409.68155073,\r\n              8474117.35659452\r\n            ],\r\n            [\r\n              1914405.69122839,\r\n              8474272.84673853\r\n            ],\r\n            [\r\n              1914404.11093813,\r\n              8474334.36140384\r\n            ],\r\n            [\r\n              1914403.60592808,\r\n              8474354.60177934\r\n            ],\r\n            [\r\n              1914401.66640993,\r\n              8474429.77468939\r\n            ],\r\n            [\r\n              1914398.26173137,\r\n              8474562.58204877\r\n            ],\r\n            [\r\n              1914337.15242937,\r\n              8476944.23485268\r\n            ],\r\n            [\r\n              1914336.32950673,\r\n              8476976.28545601\r\n            ],\r\n            [\r\n              1914363.53874817,\r\n              8477042.70778778\r\n            ],\r\n            [\r\n              1916098.75085445,\r\n              8481278.36805278\r\n            ],\r\n            [\r\n              1916101.17251789,\r\n              8481284.27963763\r\n            ],\r\n            [\r\n              1916135.99193735,\r\n              8481336.9091635\r\n            ],\r\n            [\r\n              1916210.94360945,\r\n              8481450.26204558\r\n            ],\r\n            [\r\n              1914728.39269011,\r\n              8481609.49116542\r\n            ],\r\n            [\r\n              1914645.04784168,\r\n              8481621.86642158\r\n            ],\r\n            [\r\n              1914621.34936901,\r\n              8484502.9181757\r\n            ],\r\n            [\r\n              1913479.94220509,\r\n              8485341.87553659\r\n            ],\r\n            [\r\n              1913960.66163341,\r\n              8486838.24071676\r\n            ],\r\n            [\r\n              1914081.30047313,\r\n              8487213.71307986\r\n            ],\r\n            [\r\n              1913727.77395858,\r\n              8488080.35586856\r\n            ],\r\n            [\r\n              1914051.04823011,\r\n              8489553.62086678\r\n            ],\r\n            [\r\n              1913838.56777818,\r\n              8490266.28994124\r\n            ],\r\n            [\r\n              1914197.44871468,\r\n              8491699.87211788\r\n            ],\r\n            [\r\n              1914212.21987916,\r\n              8491731.84130964\r\n            ],\r\n            [\r\n              1914311.51181835,\r\n              8491933.87184419\r\n            ],\r\n            [\r\n              1914322.84803,\r\n              8491943.52833136\r\n            ],\r\n            [\r\n              1914601.02096475,\r\n              8492180.75650488\r\n            ],\r\n            [\r\n              1914603.94145285,\r\n              8492181.64582459\r\n            ],\r\n            [\r\n              1914749.11655191,\r\n              8492226.11295603\r\n            ],\r\n            [\r\n              1914913.9878212,\r\n              8492276.59488195\r\n            ],\r\n            [\r\n              1914952.46872803,\r\n              8492288.36851214\r\n            ],\r\n            [\r\n              1915220.86430771,\r\n              8492646.89305014\r\n            ],\r\n            [\r\n              1915226.1295387,\r\n              8492852.8989161\r\n            ],\r\n            [\r\n              1915478.52228795,\r\n              8493241.11564004\r\n            ],\r\n            [\r\n              1915616.00931332,\r\n              8493320.89659874\r\n            ],\r\n            [\r\n              1915638.72418893,\r\n              8493334.07711918\r\n            ],\r\n            [\r\n              1915740.65752994,\r\n              8493325.14037015\r\n            ],\r\n            [\r\n              1915743.40247528,\r\n              8493324.89991072\r\n            ],\r\n            [\r\n              1915762.18531115,\r\n              8493323.26423716\r\n            ],\r\n            [\r\n              1915821.77823153,\r\n              8493318.02480155\r\n            ],\r\n            [\r\n              1915956.06983766,\r\n              8493306.250954\r\n            ],\r\n            [\r\n              1916054.41009327,\r\n              8493347.71210453\r\n            ],\r\n            [\r\n              1916064.65457013,\r\n              8493361.52067802\r\n            ],\r\n            [\r\n              1916071.26346423,\r\n              8493370.44942531\r\n            ],\r\n            [\r\n              1916086.31262903,\r\n              8493390.75326375\r\n            ],\r\n            [\r\n              1916276.14412024,\r\n              8493646.94344208\r\n            ],\r\n            [\r\n              1916625.02367907,\r\n              8493759.47177288\r\n            ],\r\n            [\r\n              1916532.48954415,\r\n              8495321.40820196\r\n            ],\r\n            [\r\n              1916546.17988135,\r\n              8495502.48239023\r\n            ],\r\n            [\r\n              1916578.65314913,\r\n              8495931.84622798\r\n            ],\r\n            [\r\n              1916659.04757896,\r\n              8496994.68369959\r\n            ],\r\n            [\r\n              1916919.52170026,\r\n              8496927.43730876\r\n            ],\r\n            [\r\n              1916931.9276933,\r\n              8496924.2341764\r\n            ],\r\n            [\r\n              1917074.78489017,\r\n              8496887.35811063\r\n            ],\r\n            [\r\n              1917195.78078791,\r\n              8496856.11581302\r\n            ],\r\n            [\r\n              1917244.78568656,\r\n              8496843.45682129\r\n            ],\r\n            [\r\n              1917306.51187,\r\n              8496827.52379755\r\n            ],\r\n            [\r\n              1917722.99187292,\r\n              8496719.96723772\r\n            ],\r\n            [\r\n              1917871.52703472,\r\n              8496681.617247\r\n            ],\r\n            [\r\n              1917978.0402452,\r\n              8496654.09871173\r\n            ],\r\n            [\r\n              1918023.39605387,\r\n              8496642.38423523\r\n            ],\r\n            [\r\n              1918056.29564642,\r\n              8496633.89562904\r\n            ],\r\n            [\r\n              1918080.88822626,\r\n              8496627.52884383\r\n            ],\r\n            [\r\n              1918280.62825914,\r\n              8496575.93176154\r\n            ],\r\n            [\r\n              1918310.50977035,\r\n              8496568.21623906\r\n            ],\r\n            [\r\n              1918327.39144856,\r\n              8496563.84068676\r\n            ],\r\n            [\r\n              1918372.43008857,\r\n              8496552.20993523\r\n            ],\r\n            [\r\n              1918411.4653271,\r\n              8496542.12891985\r\n            ],\r\n            [\r\n              1918541.18891573,\r\n              8496508.61104802\r\n            ],\r\n            [\r\n              1918566.34543,\r\n              8496502.10926445\r\n            ],\r\n            [\r\n              1918640.51851803,\r\n              8496482.94072829\r\n            ],\r\n            [\r\n              1918738.63484462,\r\n              8496457.59886337\r\n            ],\r\n            [\r\n              1919165.71597478,\r\n              8496608.69117164\r\n            ],\r\n            [\r\n              1919268.07212294,\r\n              8496644.89744851\r\n            ],\r\n            [\r\n              1919273.75380453,\r\n              8496645.48203131\r\n            ],\r\n            [\r\n              1919289.04513688,\r\n              8496647.0317469\r\n            ],\r\n            [\r\n              1919310.93737309,\r\n              8496648.35649484\r\n            ],\r\n            [\r\n              1919339.50182146,\r\n              8496643.80395039\r\n            ],\r\n            [\r\n              1919360.07938865,\r\n              8496654.49469869\r\n            ],\r\n            [\r\n              1919371.67384703,\r\n              8496671.57577973\r\n            ],\r\n            [\r\n              1919383.51644553,\r\n              8496687.80325151\r\n            ],\r\n            [\r\n              1919579.25770145,\r\n              8496748.43550073\r\n            ],\r\n            [\r\n              1919663.69094657,\r\n              8496774.59765864\r\n            ],\r\n            [\r\n              1919867.27767048,\r\n              8496837.64356584\r\n            ],\r\n            [\r\n              1919925.70062008,\r\n              8496880.75795803\r\n            ],\r\n            [\r\n              1920030.46180161,\r\n              8496955.60486571\r\n            ],\r\n            [\r\n              1920134.30355981,\r\n              8497027.25107398\r\n            ],\r\n            [\r\n              1920164.37176483,\r\n              8497045.60911588\r\n            ],\r\n            [\r\n              1920190.36779522,\r\n              8497061.48305544\r\n            ],\r\n            [\r\n              1920278.42686365,\r\n              8497111.3396945\r\n            ],\r\n            [\r\n              1920355.08088143,\r\n              8497149.83757512\r\n            ],\r\n            [\r\n              1920359.36962294,\r\n              8497151.50719797\r\n            ],\r\n            [\r\n              1920418.97763027,\r\n              8497174.53990445\r\n            ],\r\n            [\r\n              1920467.14131653,\r\n              8497187.57845434\r\n            ],\r\n            [\r\n              1920496.41226557,\r\n              8497192.30181914\r\n            ],\r\n            [\r\n              1920528.941616,\r\n              8497197.34023085\r\n            ],\r\n            [\r\n              1920537.36419642,\r\n              8497220.18357936\r\n            ],\r\n            [\r\n              1920554.34683829,\r\n              8497244.76262772\r\n            ],\r\n            [\r\n              1920587.14684476,\r\n              8497271.26139301\r\n            ],\r\n            [\r\n              1920607.19726501,\r\n              8497278.31403693\r\n            ],\r\n            [\r\n              1920654.71200308,\r\n              8497283.6676169\r\n            ],\r\n            [\r\n              1920688.04937998,\r\n              8497276.10879753\r\n            ],\r\n            [\r\n              1920722.24762033,\r\n              8497257.59307507\r\n            ],\r\n            [\r\n              1920763.20747235,\r\n              8497223.43467865\r\n            ],\r\n            [\r\n              1920726.84599867,\r\n              8497111.52367444\r\n            ],\r\n            [\r\n              1920748.91121405,\r\n              8497093.39616514\r\n            ],\r\n            [\r\n              1920789.04296005,\r\n              8497058.04193943\r\n            ],\r\n            [\r\n              1920841.31435944,\r\n              8497010.57080444\r\n            ],\r\n            [\r\n              1920885.12976244,\r\n              8496951.59937724\r\n            ],\r\n            [\r\n              1920925.32774177,\r\n              8496893.55980038\r\n            ],\r\n            [\r\n              1920973.31856061,\r\n              8496813.80016805\r\n            ],\r\n            [\r\n              1921002.81827575,\r\n              8496762.19422191\r\n            ],\r\n            [\r\n              1921025.75475177,\r\n              8496695.78641192\r\n            ],\r\n            [\r\n              1921044.94613344,\r\n              8496638.83728506\r\n            ],\r\n            [\r\n              1921053.65566318,\r\n              8496570.0713972\r\n            ],\r\n            [\r\n              1921056.36686509,\r\n              8496478.79890686\r\n            ],\r\n            [\r\n              1921046.3669522,\r\n              8496406.98454141\r\n            ],\r\n            [\r\n              1921027.22423034,\r\n              8496327.37171433\r\n            ],\r\n            [\r\n              1921008.7171192,\r\n              8496255.42241568\r\n            ],\r\n            [\r\n              1920987.62333432,\r\n              8496190.86320671\r\n            ],\r\n            [\r\n              1920949.86564357,\r\n              8496085.90451163\r\n            ],\r\n            [\r\n              1920922.26333477,\r\n              8496020.33379613\r\n            ],\r\n            [\r\n              1920817.25413513,\r\n              8495535.41810954\r\n            ],\r\n            [\r\n              1920849.16168727,\r\n              8495408.38003306\r\n            ],\r\n            [\r\n              1920839.34077566,\r\n              8495367.36949455\r\n            ],\r\n            [\r\n              1920996.67523566,\r\n              8494852.96441346\r\n            ],\r\n            [\r\n              1921102.0877105,\r\n              8494995.42854068\r\n            ],\r\n            [\r\n              1921460.98012224,\r\n              8495516.38640484\r\n            ],\r\n            [\r\n              1921540.23239312,\r\n              8495631.51179845\r\n            ],\r\n            [\r\n              1924470.0429214,\r\n              8499885.63503978\r\n            ],\r\n            [\r\n              1924478.08898793,\r\n              8499965.67589804\r\n            ],\r\n            [\r\n              1924480.75648412,\r\n              8499992.22253185\r\n            ],\r\n            [\r\n              1924507.96902282,\r\n              8500262.93390375\r\n            ],\r\n            [\r\n              1924514.34614921,\r\n              8500326.36814773\r\n            ],\r\n            [\r\n              1924564.84188709,\r\n              8500828.65182726\r\n            ],\r\n            [\r\n              1924576.41242832,\r\n              8500943.75829409\r\n            ],\r\n            [\r\n              1924617.22960116,\r\n              8501349.74960516\r\n            ],\r\n            [\r\n              1924629.63510542,\r\n              8501473.13342617\r\n            ],\r\n            [\r\n              1924633.52587413,\r\n              8501511.83894833\r\n            ],\r\n            [\r\n              1924652.72769214,\r\n              8501535.61925645\r\n            ],\r\n            [\r\n              1924639.9187476,\r\n              8501555.38418972\r\n            ],\r\n            [\r\n              1924670.44412048,\r\n              8501628.63759227\r\n            ],\r\n            [\r\n              1924674.2482839,\r\n              8501637.76182617\r\n            ],\r\n            [\r\n              1924715.47081958,\r\n              8501743.83815503\r\n            ],\r\n            [\r\n              1924916.92501836,\r\n              8502262.24274091\r\n            ],\r\n            [\r\n              1924926.30013065,\r\n              8502286.36634398\r\n            ],\r\n            [\r\n              1924959.6359199,\r\n              8502372.13962485\r\n            ],\r\n            [\r\n              1924977.47861945,\r\n              8502418.05341402\r\n            ],\r\n            [\r\n              1925053.27663454,\r\n              8502613.08757021\r\n            ],\r\n            [\r\n              1925461.73262467,\r\n              8503663.96819179\r\n            ],\r\n            [\r\n              1925518.95989381,\r\n              8503811.18999349\r\n            ],\r\n            [\r\n              1925577.07438196,\r\n              8503960.69306216\r\n            ],\r\n            [\r\n              1925577.4560451,\r\n              8503966.82234094\r\n            ],\r\n            [\r\n              1925577.69397531,\r\n              8503970.63375925\r\n            ],\r\n            [\r\n              1925583.26099398,\r\n              8504059.83972292\r\n            ],\r\n            [\r\n              1925588.95277065,\r\n              8504151.05063626\r\n            ],\r\n            [\r\n              1925616.90414398,\r\n              8504598.91241689\r\n            ],\r\n            [\r\n              1925617.74839577,\r\n              8504612.43537079\r\n            ],\r\n            [\r\n              1925670.68646766,\r\n              8505460.58658003\r\n            ],\r\n            [\r\n              1925725.24746843,\r\n              8506334.62794118\r\n            ],\r\n            [\r\n              1925924.0704222,\r\n              8509518.65652149\r\n            ],\r\n            [\r\n              1925924.46330781,\r\n              8509547.3282492\r\n            ],\r\n            [\r\n              1925775.43478725,\r\n              8509594.61974596\r\n            ],\r\n            [\r\n              1925706.74762456,\r\n              8509616.41589573\r\n            ],\r\n            [\r\n              1925767.63452978,\r\n              8509822.44557536\r\n            ],\r\n            [\r\n              1925754.83150208,\r\n              8509940.55366014\r\n            ],\r\n            [\r\n              1925739.67290419,\r\n              8510005.39387249\r\n            ],\r\n            [\r\n              1925719.39394971,\r\n              8510092.13931557\r\n            ],\r\n            [\r\n              1925708.82025019,\r\n              8510137.36796285\r\n            ],\r\n            [\r\n              1925811.27152831,\r\n              8510312.39404324\r\n            ],\r\n            [\r\n              1925837.46711853,\r\n              8510357.14740527\r\n            ],\r\n            [\r\n              1925851.89441048,\r\n              8510647.10345831\r\n            ],\r\n            [\r\n              1925989.75150348,\r\n              8510642.64138529\r\n            ],\r\n            [\r\n              1926034.62474934,\r\n              8511736.86306828\r\n            ],\r\n            [\r\n              1926040.76519797,\r\n              8511886.51986506\r\n            ],\r\n            [\r\n              1926061.97095631,\r\n              8512369.84372935\r\n            ],\r\n            [\r\n              1926086.20700537,\r\n              8512922.21046545\r\n            ],\r\n            [\r\n              1926125.18382663,\r\n              8513810.40683113\r\n            ],\r\n            [\r\n              1926126.40636258,\r\n              8513838.27472492\r\n            ],\r\n            [\r\n              1926129.14806768,\r\n              8513900.75262603\r\n            ],\r\n            [\r\n              1926129.98314094,\r\n              8513919.78183271\r\n            ],\r\n            [\r\n              1926171.94694676,\r\n              8514046.10493856\r\n            ],\r\n            [\r\n              1926310.87074722,\r\n              8514464.30328045\r\n            ],\r\n            [\r\n              1927269.92621351,\r\n              8517350.5713527\r\n            ],\r\n            [\r\n              1928245.89182752,\r\n              8520286.40503074\r\n            ],\r\n            [\r\n              1928298.88333291,\r\n              8520476.52376085\r\n            ],\r\n            [\r\n              1928436.48998531,\r\n              8520958.28759065\r\n            ],\r\n            [\r\n              1928685.58808594,\r\n              8521830.30605689\r\n            ],\r\n            [\r\n              1928790.92341228,\r\n              8522238.05745984\r\n            ],\r\n            [\r\n              1929236.48949928,\r\n              8523962.59240169\r\n            ],\r\n            [\r\n              1929239.44052267,\r\n              8523974.01003064\r\n            ],\r\n            [\r\n              1930582.50873972,\r\n              8528859.85237915\r\n            ],\r\n            [\r\n              1930744.54392764,\r\n              8529449.05881342\r\n            ],\r\n            [\r\n              1931282.50809094,\r\n              8531404.89432058\r\n            ],\r\n            [\r\n              1931396.6572213,\r\n              8531941.67236837\r\n            ],\r\n            [\r\n              1931880.20804239,\r\n              8534353.42167584\r\n            ],\r\n            [\r\n              1932169.03908981,\r\n              8535793.57939121\r\n            ],\r\n            [\r\n              1932543.87661443,\r\n              8537606.6141981\r\n            ],\r\n            [\r\n              1932768.58566726,\r\n              8538671.09907702\r\n            ],\r\n            [\r\n              1932803.77262757,\r\n              8538840.70930142\r\n            ],\r\n            [\r\n              1932823.92948499,\r\n              8538937.8693738\r\n            ],\r\n            [\r\n              1932839.88014225,\r\n              8539014.75457826\r\n            ],\r\n            [\r\n              1932850.61432224,\r\n              8539089.89428847\r\n            ],\r\n            [\r\n              1932864.6888465,\r\n              8539157.94853239\r\n            ],\r\n            [\r\n              1932872.56881091,\r\n              8539196.05032006\r\n            ],\r\n            [\r\n              1932882.6236222,\r\n              8539244.66940803\r\n            ],\r\n            [\r\n              1933023.98449018,\r\n              8539928.11882624\r\n            ],\r\n            [\r\n              1933032.88363994,\r\n              8539971.13541716\r\n            ],\r\n            [\r\n              1933042.97726701,\r\n              8540019.93805674\r\n            ],\r\n            [\r\n              1933063.4490207,\r\n              8540118.90425918\r\n            ],\r\n            [\r\n              1933089.22741006,\r\n              8540243.53008758\r\n            ],\r\n            [\r\n              1933097.25258574,\r\n              8540282.3278048\r\n            ],\r\n            [\r\n              1933107.26863441,\r\n              8540330.74532228\r\n            ],\r\n            [\r\n              1933114.04566901,\r\n              8540372.41119703\r\n            ],\r\n            [\r\n              1933120.65819831,\r\n              8540413.06277608\r\n            ],\r\n            [\r\n              1933247.08219921,\r\n              8541379.49191616\r\n            ],\r\n            [\r\n              1933253.65336095,\r\n              8541429.72989773\r\n            ],\r\n            [\r\n              1933300.87108736,\r\n              8541790.64853244\r\n            ],\r\n            [\r\n              1933360.51806699,\r\n              8542141.49581574\r\n            ],\r\n            [\r\n              1933534.0933825,\r\n              8543162.34930927\r\n            ],\r\n            [\r\n              1933501.50188627,\r\n              8543152.37722782\r\n            ],\r\n            [\r\n              1933495.90405021,\r\n              8543155.35963259\r\n            ],\r\n            [\r\n              1933486.64710818,\r\n              8543287.82137541\r\n            ],\r\n            [\r\n              1933566.10147393,\r\n              8543359.54897949\r\n            ],\r\n            [\r\n              1933564.27515522,\r\n              8543362.2481052\r\n            ],\r\n            [\r\n              1933685.17819411,\r\n              8544093.03617441\r\n            ],\r\n            [\r\n              1933709.57114169,\r\n              8544237.15652771\r\n            ],\r\n            [\r\n              1933710.90978062,\r\n              8544244.77191982\r\n            ],\r\n            [\r\n              1933889.66040781,\r\n              8545260.62982812\r\n            ],\r\n            [\r\n              1933896.68541451,\r\n              8545300.55393962\r\n            ],\r\n            [\r\n              1933919.2321558,\r\n              8545427.6519832\r\n            ],\r\n            [\r\n              1933959.68285728,\r\n              8545655.68061126\r\n            ],\r\n            [\r\n              1933979.24928221,\r\n              8545766.25856867\r\n            ],\r\n            [\r\n              1933992.2075562,\r\n              8545836.76274555\r\n            ],\r\n            [\r\n              1934017.31142107,\r\n              8545973.34740007\r\n            ],\r\n            [\r\n              1934025.55407371,\r\n              8546018.77107528\r\n            ],\r\n            [\r\n              1933957.12773947,\r\n              8546123.85222968\r\n            ],\r\n            [\r\n              1933948.0130905,\r\n              8546135.83133962\r\n            ],\r\n            [\r\n              1933846.66781831,\r\n              8546269.01237563\r\n            ],\r\n            [\r\n              1933785.50425768,\r\n              8546343.53882787\r\n            ],\r\n            [\r\n              1933756.53733,\r\n              8546378.83701483\r\n            ],\r\n            [\r\n              1933541.21068034,\r\n              8546590.27836482\r\n            ],\r\n            [\r\n              1933601.38858299,\r\n              8546794.90448401\r\n            ],\r\n            [\r\n              1933625.18194263,\r\n              8546875.80824405\r\n            ],\r\n            [\r\n              1933707.73947931,\r\n              8547157.3485901\r\n            ],\r\n            [\r\n              1933744.41415213,\r\n              8547543.90167583\r\n            ],\r\n            [\r\n              1933945.56766839,\r\n              8547539.34586788\r\n            ],\r\n            [\r\n              1934250.37728656,\r\n              8547484.79936759\r\n            ],\r\n            [\r\n              1934525.57998426,\r\n              8548850.3897611\r\n            ],\r\n            [\r\n              1936320.99526238,\r\n              8548540.96469177\r\n            ],\r\n            [\r\n              1939087.50812981,\r\n              8548063.2945905\r\n            ],\r\n            [\r\n              1941014.93405212,\r\n              8546940.1312258\r\n            ],\r\n            [\r\n              1943854.37178813,\r\n              8545284.27903722\r\n            ],\r\n            [\r\n              1947689.18287065,\r\n              8548661.86870059\r\n            ],\r\n            [\r\n              1954390.96645948,\r\n              8549889.94931279\r\n            ],\r\n            [\r\n              1963184.08072046,\r\n              8546375.58068736\r\n            ]\r\n          ]\r\n        ],\r\n        \"crs\": null,\r\n        \"bbox\": null,\r\n        \"type\": \"Polygon\"\r\n      },\r\n      \"properties\": {\r\n        \"LAN_NAMN\": \"Uppsala l�n\",\r\n        \"ID\": \"03\"\r\n      },\r\n      \"crs\": null,\r\n      \"bbox\": null,\r\n      \"type\": \"Feature\"\r\n    }\r\n  ]\r\n}";
            var displayCoordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var gridCellFeatureStatistics =
                AnalysisManager.GetGridCellFeatureStatistics(Context, null, featureCollecationJson, gridSpecification, displayCoordinateSystem);
            Assert.IsTrue(gridCellFeatureStatistics.Count > 0);

        }

        [TestMethod]
        [Ignore]
        public void GetCoordinateSystemFromFeaturesUrlAsSrsNameTest()
        {
            string featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:MapOfSwedenCounties&srsName=EPSG:3857";
            Uri featuresUri = new Uri(featuresUrl);

            string srsName = AnalysisManager.GetCoordinateSystemFromFeaturesUrlAsSrsName(featuresUri);
            //      Assert.IsTrue(srsName.Equals("3857"));

            // Google mercator
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:MapOfSwedenCounties&srsName=EPSG:900913";
            featuresUri = new Uri(featuresUrl);
            srsName = AnalysisManager.GetCoordinateSystemFromFeaturesUrlAsSrsName(featuresUri);
            Assert.IsTrue(srsName.Equals("900913"));

            // extra space in the end
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:MapOfSwedenCounties&srsName=EPSG:900913    ";
            featuresUri = new Uri(featuresUrl);
            srsName = AnalysisManager.GetCoordinateSystemFromFeaturesUrlAsSrsName(featuresUri);
            Assert.IsTrue(srsName.Equals("900913"));

            // inside the url
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&srsName=EPSG:900913&version=1.1.0&typeName=SLW:MapOfSwedenCounties";
            featuresUri = new Uri(featuresUrl);
            srsName = AnalysisManager.GetCoordinateSystemFromFeaturesUrlAsSrsName(featuresUri);
            Assert.IsTrue(srsName.Equals("900913"));

            // inside the url
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&srsName=EPSG:3857&version=1.1.0&typeName=SLW:MapOfSwedenCounties";
            featuresUri = new Uri(featuresUrl);
            srsName = AnalysisManager.GetCoordinateSystemFromFeaturesUrlAsSrsName(featuresUri);
            Assert.IsTrue(srsName.Equals("3857"));
        }

        #endregion


        #region GetSpeciesObservationCountBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaFromDifferentMethodsTest()
        {
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            WebGridSpecification gridSpecification = new WebGridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago
            searchCriteria.TaxonIds = taxa;


            Int64 speciesObservationCount = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            // Use another method than default
            IList<WebGridCellSpeciesObservationCount> noOfGridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, gridSpecification, coordinateSystem);


            Assert.IsTrue(speciesObservationCount > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

            Int64 gridCellCount = 0;
            foreach (WebGridCellSpeciesObservationCount webGridCellSpeciesObservationCount in noOfGridCellObservations)
            {
                gridCellCount = gridCellCount + webGridCellSpeciesObservationCount.Count;
            }
            Assert.IsTrue(speciesObservationCount == gridCellCount);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetSpeciesObservationCountBySearchCriteriaFailedNoCriteriasSetTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = null;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetSpeciesObservationCountBySearchCriteriaFailedNoCoordinateSystemSetTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = null;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaAccurrancyTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 30;
            searchCriteria.IsAccuracySpecified = true;


            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

            // Increase Accurancy
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;


            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations2 > noOfObservations);


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaAccurracyElasticsearch()
        {
            Int64 speciesObservationCount1, speciesObservationCount2;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.IncludePositiveObservations = true;

            searchCriteria.Accuracy = 30;
            searchCriteria.IsAccuracySpecified = true;
            speciesObservationCount1 = AnalysisManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(speciesObservationCount1 > 0);

            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            speciesObservationCount2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(speciesObservationCount2 > speciesObservationCount1);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaAccurancyFailedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            searchCriteria.Accuracy = -3;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");
        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaIsAccurrancySpecifiedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Don't use accurancy, all positiv observations should be collected
            searchCriteria.IsAccuracySpecified = false;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);


            // Enable Accurancy
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IsAccuracySpecified = true;
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations2 < noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaAccurracyIsLessThanZeroTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = -1;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsAccuracySpecified = true;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaIsNaturalOccuranceTest()
        {
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(233790); // Större flamingo

            searchCriteria.TaxonIds = taxa;
            searchCriteria.IsNaturalOccurrence = false;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            taxa = new List<int>();
            taxa.Add(233790); // Större flamingo

            searchCriteria.TaxonIds = taxa;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_GoogleMercator_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_WGS84_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(89, 89);
            searchCriteria.BoundingBox.Min = new WebPoint(10, 10);


            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);



        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_SWEREF99_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBox
            searchCriteria.BoundingBox = new WebBoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            // SWEREF 99	6110000 – 7680000	260000 – 920000

            searchCriteria.BoundingBox.Max = new WebPoint(820000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(560000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_RT90_25_gon_v_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();

            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new WebPoint(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_RT90_Test()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();


            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new WebPoint(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new WebPoint(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxNoneTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.None;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(90, 90);
            searchCriteria.BoundingBox.Min = new WebPoint(0, 0);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxInvalidMaxMinValuesTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();
            //Ok boundig box values
            //searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            //searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);
            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                searchCriteria.BoundingBox.Min = new WebPoint(9993195, 1118890);
                AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            }
            catch (ArgumentException)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria = new WebSpeciesObservationSearchCriteria();
                    SetDefaultSearchCriteria(searchCriteria);
                    searchCriteria.BoundingBox = new WebBoundingBox();
                    searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = new WebPoint(1113195, 31118890);
                    AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

                }
                catch (ArgumentException)
                {

                    // Ok if we get here
                    return;
                }
                catch (Exception)
                {
                    Assert.Fail("No argument exception thrown that YMin value is larger that YMax value for bounding box.");
                }
                Assert.Fail("No argument exception thrown that YMin value is larger that YMax value for bounding box.");

            }
            catch (Exception)
            {
                Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");
            }
            Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxNullMaxMinValuesTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new WebBoundingBox();

            try
            {
                // Xmin > Xmax
                searchCriteria = new WebSpeciesObservationSearchCriteria();
                SetDefaultSearchCriteria(searchCriteria);
                searchCriteria.BoundingBox = new WebBoundingBox();
                searchCriteria.BoundingBox.Max = null;
                searchCriteria.BoundingBox.Min = new WebPoint(9993195, 1118890);
                AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            }
            catch (ArgumentException)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria = new WebSpeciesObservationSearchCriteria();
                    SetDefaultSearchCriteria(searchCriteria);
                    searchCriteria.BoundingBox = new WebBoundingBox();
                    searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = null;
                    AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

                }
                catch (ArgumentException)
                {

                    // Ok if we get here
                    return;
                }
                catch (Exception)
                {
                    Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");
                }
                Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");

            }
            catch (Exception)
            {
                Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");
            }
            Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaChangeDateTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 07, 25);
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 08, 01);

            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaChangePartOfYearTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            // Get complete years data
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 05, 01);
            interval2.End = new DateTime(2012, 05, 10);
            intervals2.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Adding one more time interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2012, 04, 30);
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 05, 01);
            interval2.End = new DateTime(2012, 05, 10);
            intervals.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 04, 01);
            interval3.End = new DateTime(2012, 05, 10);
            intervals3.Add(interval3);
            searchCriteria.ChangeDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
            Assert.IsTrue(noOfObservations > noOfObservations3);
            Assert.IsTrue(noOfObservations > noOfObservations4);
            Assert.IsTrue(noOfObservations >= noOfObservations5);
            Assert.IsTrue(noOfObservations > noOfObservations6);
            Assert.IsTrue(noOfObservations2 >= noOfObservations3);
            Assert.IsTrue(noOfObservations3 < noOfObservations5);
            Assert.IsTrue(noOfObservations4 < noOfObservations5);
            Assert.IsTrue(noOfObservations5 >= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaDataProvidersTest()
        {
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            searchCriteria.DataProviderGuids = guids as List<string>;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            searchCriteria.DataProviderGuids = guids as List<string>;

            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaDataProviderInvalidTest()
        {
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataInvalidProvider:1");
            searchCriteria.DataProviderGuids = guids as List<string>;

            AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaLocalityTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 100;
            searchCriteria.IsAccuracySpecified = true;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaLocalityAllConditionsTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IsAccuracySpecified = true;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            // Can only set one stringCompareOperator 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            localityString = new WebStringSearchCriteria();
            localityString.SearchString = "Solvik";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;
            Int64 noOfObservations6 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObserverSearchStringTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.TaxonIds = new List<int>();
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria operatorString = new WebStringSearchCriteria();
            // operatorString.SearchString = "Per Lundkvist";
            operatorString.SearchString = "";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            // Can only set one stringCompareOperator 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations6 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);


        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObserverSearchStringAllConditionsTest()
        {

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 2;
            searchCriteria.IsAccuracySpecified = true;
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebStringSearchCriteria operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            // Can only set one stringCompareOperator 
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            operatorString = new WebStringSearchCriteria();
            operatorString.SearchString = "Per Lundkvist";
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;
            Int64 noOfObservations6 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);


        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationTypeTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations6 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations7 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations8 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > noOfObservations);
            Assert.IsTrue(noOfObservations5 > noOfObservations4);
            // TODO should be 0
            Assert.IsTrue(noOfObservations6 == noOfObservations);
            Assert.IsTrue(noOfObservations7 > noOfObservations);
            Assert.IsTrue(noOfObservations8 > noOfObservations3);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 08, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
            Assert.IsTrue(noOfObservations3 >= noOfObservations2);


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateCompareOperatorFailedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Greater;
            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateInvalidDatesTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationPartOfYearTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            // Get complete years data
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2001, 06, 01);
            interval.End = new DateTime(2001, 09, 30);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2001, 06, 01);
            interval.End = new DateTime(2001, 06, 30);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2001, 09, 01);
            interval2.End = new DateTime(2002, 06, 01);
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Adding one more time interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2001, 07, 01);
            interval.End = new DateTime(2001, 07, 31);
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2001, 09, 01);
            interval2.End = new DateTime(2002, 06, 30);
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2001, 07, 01);
            interval3.End = new DateTime(2002, 06, 30);
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
            Assert.IsTrue(noOfObservations > noOfObservations3);
            Assert.IsTrue(noOfObservations > noOfObservations4);
            Assert.IsTrue(noOfObservations > noOfObservations5);
            Assert.IsTrue(noOfObservations > noOfObservations6);
            Assert.IsTrue(noOfObservations2 > noOfObservations3);
            Assert.IsTrue(noOfObservations3 < noOfObservations5);
            Assert.IsTrue(noOfObservations4 < noOfObservations5);
            Assert.IsTrue(noOfObservations5 <= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaPartOfYearIsDayOfYearSpecifiedTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 08, 01);
            interval.End = new DateTime(2000, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;


            // Get less amount of data since only two mounth within a year
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2000, 12, 31);
            interval2.End = new DateTime(2001, 07, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Adding one more time interval to the first one from nov to jan
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 08, 01);
            interval.End = new DateTime(2000, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2000, 12, 31);
            interval2.End = new DateTime(2001, 07, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval compare that on einterval and two interval is equal.
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2000, 08, 01);
            interval3.End = new DateTime(2001, 07, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Not using day of year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            intervals3 = new List<WebDateTimeInterval>();
            interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2000, 08, 01);
            interval3.End = new DateTime(2001, 07, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;
            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations4 >= noOfObservations3);

        }

        [TestMethod]
        public void GetGridSpeciesCounts_UseAllObservations_AllGridCellsAreSquares()
        {
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 10000;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            List<WebGridCellSpeciesCount> gridCellObservations = AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            foreach (WebGridCellSpeciesCount gridCellObservation in gridCellObservations)
            {
                Assert.IsFalse(Math.Abs(gridCellObservation.OrginalCentreCoordinate.X - 5045000) < 0.001 &&
                    Math.Abs(gridCellObservation.OrginalCentreCoordinate.Y - 8375000) < 0.001);
            }
        }

        [TestMethod]
        public void GetGridSpeciesObservationCounts_UseAllObservations_AllGridCellsAreSquares()
        {
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 10000;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<WebGridCellSpeciesObservationCount> gridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            foreach (WebGridCellSpeciesObservationCount gridCellObservation in gridCellObservations)
            {
                Assert.IsFalse(Math.Abs(gridCellObservation.OrginalCentreCoordinate.X - 5045000) < 0.001 &&
                    Math.Abs(gridCellObservation.OrginalCentreCoordinate.Y - 8375000) < 0.001);
            }
        }

        [TestMethod]
        public void GetGridSpeciesObservationCounts_GridCoordinateSystemAndDisplayCoordinateSystemAreEqual_OriginalCoordinatesAndDisplayCoordinatesAreEqual()
        {
            WebCoordinateSystem coordinateSystem;
            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IsNaturalOccurrence = true;
            WebGridSpecification webGridSpecification = new WebGridSpecification();
            webGridSpecification.IsGridCellSizeSpecified = true;
            webGridSpecification.GridCellSize = 10000;
            webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            webGridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<WebGridCellSpeciesObservationCount> gridCellObservations = AnalysisManager.GetGridSpeciesObservationCounts(Context, searchCriteria, webGridSpecification, coordinateSystem);
            foreach (WebGridCellSpeciesObservationCount gridCellObservation in gridCellObservations)
            {
                Assert.AreEqual(gridCellObservation.CentreCoordinate.X, gridCellObservation.OrginalCentreCoordinate.X, 0.01);
                Assert.AreEqual(gridCellObservation.CentreCoordinate.Y, gridCellObservation.OrginalCentreCoordinate.Y, 0.01);
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void GetSpeciesObservationCountBySearchCriteriaPartOfYearFailedTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IsAccuracySpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 04, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2008, 06, 01);
            interval.End = new DateTime(2008, 03, 01);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaPolygonsTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test search criteria Polygons.
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaPolygonsDifferentCoordinateSystemsTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            CoordinateConversionManager coordinateConversionManager = new CoordinateConversionManager();
            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Create polygon
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            searchCriteria.IncludePositiveObservations = true;
            // WGS84
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            //GoogleMercator
            WebCoordinateSystem coordinateSystemMercator;
            coordinateSystemMercator = new WebCoordinateSystem();
            coordinateSystemMercator.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebPolygon polygonMercator = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem, coordinateSystemMercator);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonMercator);
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystemMercator);
            //RT90
            WebCoordinateSystem coordinateSystemRT90;
            coordinateSystemRT90 = new WebCoordinateSystem();
            coordinateSystemRT90.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebPolygon polygonRT90 = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem, coordinateSystemRT90);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonRT90);
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystemRT90);

            //Rt90_25_gon_v
            WebCoordinateSystem coordinateSystemRT90_25_gon_v;

            coordinateSystemRT90_25_gon_v = new WebCoordinateSystem();
            coordinateSystemRT90_25_gon_v.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebPolygon polygonRT90_25_gon_v = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem, coordinateSystemRT90_25_gon_v);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonRT90_25_gon_v);
            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystemRT90_25_gon_v);

            //SWEREF99
            WebCoordinateSystem coordinateSystemSWEREF99;
            coordinateSystemSWEREF99 = new WebCoordinateSystem();
            coordinateSystemSWEREF99.Id = CoordinateSystemId.SWEREF99_TM;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            WebPolygon polygonSWEREF99 = coordinateConversionManager.GetConvertedPolygon(polygon, coordinateSystem, coordinateSystemSWEREF99);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Clear();
            searchCriteria.Polygons.Add(polygonSWEREF99);
            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystemSWEREF99);


            // Since conversion between coordinate systems are not excact we have a bit of
            // difference in number of observations in our db searches. If conversion of
            // coordinate systems were exact the number of observations should not differ.
            // Allowing 0.2 % difference in result
            double delta = noOfObservations * 0.002;
            Assert.IsTrue(noOfObservations > 0);
            Assert.AreEqual(noOfObservations, noOfObservations2, delta);
            Assert.AreEqual(noOfObservations, noOfObservations3, delta);
            Assert.AreEqual(noOfObservations, noOfObservations4, delta);
            Assert.AreEqual(noOfObservations, noOfObservations5, delta);


        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRegistrationDateTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2011, 01, 01);
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Enlarge the search area regarding time
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 01, 01);

            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRegistrationPartOfYearTest()
        {
            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;

            // Get complete years data
            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2012, 03, 31);
            intervals.Add(interval);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2012, 02, 28);
            intervals.Add(interval);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals2 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 04, 01);
            interval2.End = new DateTime(2012, 04, 15);
            intervals2.Add(interval2);
            searchCriteria.ReportedDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Adding one more time interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            intervals = new List<WebDateTimeInterval>();
            interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2010, 02, 28);
            intervals.Add(interval);
            interval2 = new WebDateTimeInterval();
            interval2.Begin = new DateTime(2010, 04, 01);
            interval2.End = new DateTime(2010, 04, 15);
            intervals.Add(interval2);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ReportedDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2008, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;
            List<WebDateTimeInterval> intervals3 = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval3 = new WebDateTimeInterval();
            interval3.Begin = new DateTime(2010, 02, 01);
            interval3.End = new DateTime(2010, 04, 15);
            intervals3.Add(interval3);
            searchCriteria.ReportedDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
            Assert.IsTrue(noOfObservations > noOfObservations3);
            Assert.IsTrue(noOfObservations > noOfObservations4);
            Assert.IsTrue(noOfObservations > noOfObservations5);
            Assert.IsTrue(noOfObservations > noOfObservations6);
            Assert.IsTrue(noOfObservations2 > noOfObservations3);
            Assert.IsTrue(noOfObservations3 < noOfObservations5);
            Assert.IsTrue(noOfObservations4 < noOfObservations5);
            // TODO something is wrong with timeintervals and its result..
            // Assert.IsTrue(noOfObservations5 <= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRedListdCategoriesTest()
        {

            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            // Test taxa list
            taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeRedlistedTaxa = true;
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaRedListTaxaTest()
        {

            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);


            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 10;
            searchCriteria.IsAccuracySpecified = true;
            // Test taxa list
            taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;
            List<RedListCategory> redListCategories = new List<RedListCategory>();
            RedListCategory redListCategory;
            redListCategory = RedListCategory.EN;
            redListCategories.Add(redListCategory);
            searchCriteria.IncludeRedListCategories = redListCategories;
            Int64 noOfObservations2 = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaTaxaTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(2001274); // Myggor
            taxa.Add(2002088);// Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Debug.Write("noOfObservations: " + noOfObservations);
            Assert.IsTrue(noOfObservations > 0);

        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationCountBySearchCriteriaUsedAllCriteriasTest()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();

            SetDefaultSearchCriteria(searchCriteria);

            List<int> taxa = new List<int>();
            taxa.Add((Int32)(TaxonId.DrumGrasshopper));
            taxa.Add((Int32)(TaxonId.Carnivore));


            searchCriteria.TaxonIds = taxa;

            // Test BoundingBox
            searchCriteria.BoundingBox = new WebBoundingBox();
            searchCriteria.BoundingBox.Max = new WebPoint(89, 89);
            searchCriteria.BoundingBox.Min = new WebPoint(10, 10);

            // Create polygon in WGS84
            WebLinearRing linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new WebPoint(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new WebPoint(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new WebPoint(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new WebPoint(17.703271, 59.869065));
            WebPolygon polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<WebPolygon>();
            searchCriteria.Polygons.Add(polygon);

            // Set Observation date and time interval.
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            List<WebDateTimeInterval> intervals = new List<WebDateTimeInterval>();
            WebDateTimeInterval interval = new WebDateTimeInterval();
            interval.Begin = new DateTime(2000, 03, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Set dataproviders
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            searchCriteria.DataProviderGuids = guids as List<string>;

            // Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.


            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
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

            Int64 noOfObservations = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Debug.Write("noOfObservations: " + noOfObservations);
            Assert.IsTrue(noOfObservations > 0);
        }

        #endregion

        #region GetSpeciesCountBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesCountBySearchCriteria()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            Int64 noOfObservations = AnalysisManager.GetSpeciesCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesCountBySearchCriteriaElasticsearch()
        {
            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            var count = AnalysisManager.GetSpeciesCountBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesCountBySearchCriteriaCompareDatabases()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(3000176); // Hopprätvingar.
            Int64 observationCountSqlServer = AnalysisManager.GetSpeciesCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(observationCountSqlServer > 0);

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(3000176); // Hopprätvingar.
            Int64 observationCountElasticsearch = AnalysisManager.GetSpeciesCountBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(observationCountElasticsearch > 0);
            Assert.AreEqual(observationCountSqlServer, observationCountElasticsearch);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesCountBySearchCriteriaElasticsearch_One_Specie()
        {
            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria { TaxonIds = new List<int> { Convert.ToInt32(TaxonId.DrumGrasshopper) } };

            var count = AnalysisManager.GetSpeciesCountBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(count == 1);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesCountBySearchCriteriaElasticsearch_Grasshoppers()
        {
            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria { TaxonIds = new List<int> { Convert.ToInt32(TaxonId.Grasshoppers) } }; //Hopprätvingar, inte gräshoppor

            var count = AnalysisManager.GetSpeciesCountBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);
            //Assert.IsTrue(count == 43); //42 + 1 subspecie, not always true - depends on how much data that's in the test database...
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesCountBySearchCriteria_UsesOwnerFieldSearchCriteria_ExpectedObservations()
        {
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            SetOwnerFieldSearchCriterias(searchCriteria);

            Int64 noOfObservations = AnalysisManager.GetSpeciesCountBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [Ignore]
        public void GetSpeciesCountBySearchCriteriaRegionAccessRightsTest()
        {
            Int64 speciesCount1, speciesCount2;
            WebCoordinateSystem coordinateSystem;
            WebClientInformation clientInformation;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            clientInformation = new WebClientInformation();
            clientInformation.Locale = LoginResponse.Locale;
            clientInformation.Token = LoginResponse.Token;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(3000181); // skalbaggar 

            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                speciesCount1 = AnalysisManager.GetSpeciesCountBySearchCriteria(context,
                                                                                searchCriteria,
                                                                                coordinateSystem);
            }
            Assert.IsTrue(speciesCount1 > 0);

            clientInformation.Role = LoginResponse.Roles[0];
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.RegionGuids = new List<String>();
            searchCriteria.RegionGuids.Add(ProvinceGuid.Uppland);
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(3000181); // skalbaggar 
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                speciesCount2 = AnalysisManager.GetSpeciesCountBySearchCriteria(context,
                                                                                searchCriteria,
                                                                                coordinateSystem);
            }
            Assert.IsTrue(speciesCount2 > 0);
            Assert.IsTrue(speciesCount1 > speciesCount2);
        }

        #endregion

        #region GetTaxaBySearchCriteria


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxaBySearchCriteriaTest()
        {

            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            IList<WebTaxon> taxonList = AnalysisManager.GetTaxaBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxaBySearchCriteriaElasticsearch()
        {
            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };

            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            var taxonList = AnalysisManager.GetTaxaBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);
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
            taxa = AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.HostIds = new List<Int32>();
            searchCriteria.HostIds.Add(102656); // Hedsidenbi.
            taxa = AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add(9); // Ungar (juveniler)
            taxa = AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            searchCriteria.IndividualCategoryIds.Add(10); // Vuxna (imago).
            taxa = AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.RedlistCategory));
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(4); // 2015
            taxa = AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            searchCriteria.PeriodIds.Add(5); // 2013 HELCOM
            taxa = AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.PeriodIds = new List<Int32>();
            searchCriteria.PeriodIds.Add(4); // 2015
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add(10); // Vuxna (imago).
            taxa = AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            taxa = AnalysisManager.GetTaxaBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxaWithSpeciesObservationCountsBySearchCriteriaTest()
        {

            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            IList<WebTaxonSpeciesObservationCount> taxonList = AnalysisManager.GetTaxaWithSpeciesObservationCountsBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxaWithSpeciesObservationCountsBySearchCriteriaElasticsearch()
        {
            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };

            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            var taxonList = AnalysisManager.GetTaxaWithSpeciesObservationCountsBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);
            Assert.IsTrue(taxonList[0].SpeciesObservationCount > 0);

        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxaBySearchCriteriaUsingCashTest()
        {

            // Test accurancy
            WebCoordinateSystem coordinateSystem;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            WebSpeciesObservationSearchCriteria searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            List<int> taxonIdList = new List<int>();
            List<int> taxonIdList2 = new List<int>();
            for (int i = 0; i < 500000; i++)
            {
                taxonIdList.Add(i);
                taxonIdList2.Add(i);
            }

            AnalysisManager.ClearCachedTaxa();
            DateTime start1 = DateTime.Now;
            List<WebTaxon> taxonList1 = AnalysisManager.GetCachedTaxa(Context, taxonIdList);
            DateTime stop1 = DateTime.Now;
            TimeSpan timeSpan1 = stop1.Subtract(start1);
            Assert.IsTrue(taxonList1.Count > 0);

            DateTime start2 = DateTime.Now;
            List<WebTaxon> taxonList2 = AnalysisManager.GetCachedTaxa(Context, taxonIdList2);
            DateTime stop2 = DateTime.Now;
            TimeSpan timeSpan2 = stop2.Subtract(start2);


            Assert.IsTrue(taxonList2.Count == taxonList1.Count);
            Assert.IsTrue(timeSpan1 > timeSpan2, "Cash1 time: " + timeSpan1.Milliseconds + " Cash2 time: " + timeSpan2.Milliseconds);
            foreach (WebTaxon webTaxon in taxonList2)
            {
                Assert.IsTrue(taxonList1.Contains(webTaxon));
            }
            Console.Write(String.Format("Cash1 time:  {0}  Cash2 time:  {1} ", timeSpan1.Milliseconds, timeSpan2.Milliseconds));

        }

        #endregion

        #region GetTimeSpeciesObservationCountsBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetYearlyTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<WebTimeStepSpeciesObservationCount> timeStepList;
            //Test time step type year
            Periodicity periodicity = Periodicity.Yearly;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2005, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 12, 31);
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteria(Context, searchCriteria, periodicity, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);

            long sum = 0;

            foreach (WebTimeStepSpeciesObservationCount item in timeStepList)
            {
                sum = sum + item.Count;
            }

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2005, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 12, 31);
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            long count = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.AreEqual(sum, count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetYearlyTimeSpeciesObservationCountsBySearchCriteriaTestElasticsearch()
        {
            //Test time step type year
            var periodicity = Periodicity.Yearly;

            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };

            var searchCriteria = new WebSpeciesObservationSearchCriteria();

            var timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(Context, searchCriteria, periodicity, coordinateSystem);

            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);

            //long sum = 0;

            //foreach (WebTimeStepSpeciesObservationCount item in timeStepList)
            //{
            //    sum = sum + item.Count;
            //}

            //searchCriteria = new WebSpeciesObservationSearchCriteria();
            //searchCriteria.IsNaturalOccurrence = true;
            //searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            //searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            //searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            //searchCriteria.ObservationDateTime.Begin = new DateTime(2005, 01, 01);
            //searchCriteria.ObservationDateTime.End = new DateTime(2010, 12, 31);
            //searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            //searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            //searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            //long count = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            //Assert.AreEqual(sum, count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetMonthOfTheYearTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<WebTimeStepSpeciesObservationCount> timeStepList;
            //Month of the year independent on actual year.
            Periodicity periodicity = Periodicity.MonthOfTheYear;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(1909, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(1909, 12, 31);
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteria(Context, searchCriteria, periodicity, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);
            Assert.IsTrue(timeStepList[0].Name.Length == 3);

            long sum = 0;

            foreach (WebTimeStepSpeciesObservationCount item in timeStepList)
            {
                sum = sum + item.Count;
            }

            long count = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.AreEqual(sum, count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetMonthOfTheYearTimeSpeciesObservationCountsBySearchCriteriaTestElasticsearch()
        {
            //Month of the year independent on actual year.
            var periodicity = Periodicity.MonthOfTheYear;

            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            var timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(Context, searchCriteria, periodicity, coordinateSystem);

            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);
            Assert.IsTrue(timeStepList[0].Name.Length == 3);

            var sum = timeStepList.Sum(timeStep => timeStep.Count);
            var count = AnalysisManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);

            Assert.AreEqual(sum, count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetDayOfYearTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<WebTimeStepSpeciesObservationCount> timeStepList;
            //Day of year
            Periodicity periodicity = Periodicity.DayOfTheYear;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)TaxonId.Grasshoppers);
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 12, 31);
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteria(Context, searchCriteria, periodicity, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);

            long sum = 0;

            foreach (WebTimeStepSpeciesObservationCount item in timeStepList)
            {
                sum = sum + item.Count;
            }

            long count = AnalysisManager.GetSpeciesObservationCountBySearchCriteria(Context, searchCriteria, coordinateSystem);

            Assert.AreEqual(sum, count);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetDayOfYearTimeSpeciesObservationCountsBySearchCriteriaTestElasticsearch()
        {
            //Day of year
            var periodicity = Periodicity.DayOfTheYear;

            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2015, 1, 1);
            searchCriteria.ObservationDateTime.End = new DateTime(2015, 12, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            var timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(Context, searchCriteria, periodicity, coordinateSystem);
            searchCriteria.DataFields = null;

            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[200].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);

            var sum = timeStepList.Sum(timeStep => timeStep.Count);
            var count = AnalysisManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);

            Assert.IsTrue(sum < count);
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
            hosts = AnalysisManager.GetHostsBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.HostIds = new List<Int32>();
            searchCriteria.HostIds.Add((Int32)(TaxonId.Insects));
            hosts = AnalysisManager.GetHostsBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());

            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = true;
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add(100011); // Kungsörn
            hosts = AnalysisManager.GetHostsBySpeciesFactSearchCriteria(Context, searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetMonthlyTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<WebTimeStepSpeciesObservationCount> timeStepList;
            //Test time step type monthly
            Periodicity periodicity = Periodicity.Monthly;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(1906, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(1909, 12, 31);
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteria(Context, searchCriteria, periodicity, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);
            Assert.IsTrue(timeStepList[0].Name.Length > 3);

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetMonthlyTimeSpeciesObservationCountsBySearchCriteriaTestElasticsearch()
        {
            //Test time step type monthly
            var periodicity = Periodicity.Monthly;

            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            var timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(Context, searchCriteria, periodicity, coordinateSystem);

            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);
            Assert.IsTrue(timeStepList[0].Name.Length > 3);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void
            GetDailyTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<WebTimeStepSpeciesObservationCount> timeStepList;
            //Test time step type daily
            Periodicity periodicidy = Periodicity.Daily;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2009, 11, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 01, 05);
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteria(Context, searchCriteria, periodicidy, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicidy);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetDailyTimeSpeciesObservationCountsBySearchCriteriaTestElasticsearch()
        {
            //Test time step type daily
            var periodicity = Periodicity.Daily;

            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria
            {
                ObservationDateTime = new WebDateTimeSearchCriteria
                {
                    Operator = CompareOperator.Excluding,
                    Begin = new DateTime(2009, 11, 01),
                    End = new DateTime(2010, 01, 05),
                    Accuracy = new WebTimeSpan
                    {
                        Days = 0,
                        IsDaysSpecified = true
                    }
                }
            };
            var timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(Context, searchCriteria, periodicity, coordinateSystem);

            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetWeeklyTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<WebTimeStepSpeciesObservationCount> timeStepList;
            //Test time step type weekly
            Periodicity periodicidy = Periodicity.Weekly;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 10, 04);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 05);
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteria(Context, searchCriteria, periodicidy, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicidy);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetWeeklyTimeSpeciesObservationCountsBySearchCriteriaTestElasticsearch()
        {
            //Test time step type weekly
            var periodicidy = Periodicity.Weekly;

            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            var timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(Context, searchCriteria, periodicidy, coordinateSystem);

            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicidy);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetWeekOfTheYearTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<WebTimeStepSpeciesObservationCount> timeStepList;
            //Test time step type week of the year
            Periodicity periodicidy = Periodicity.WeekOfTheYear;
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.ObservationDateTime = new WebDateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 10, 04);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 05);
            searchCriteria.ObservationDateTime.Accuracy = new WebTimeSpan();
            searchCriteria.ObservationDateTime.Accuracy.Days = 0;
            searchCriteria.ObservationDateTime.Accuracy.IsDaysSpecified = true;

            timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteria(Context, searchCriteria, periodicidy, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicidy);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetWeekOfTheYearTimeSpeciesObservationCountsBySearchCriteriaTestElasticsearch()
        {
            //Test time step type week of the year
            var periodicidy = Periodicity.WeekOfTheYear;

            var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
            var searchCriteria = new WebSpeciesObservationSearchCriteria();
            var timeStepList = AnalysisManager.GetTimeSpeciesObservationCountsBySearchCriteriaElasticsearch(Context, searchCriteria, periodicidy, coordinateSystem);

            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].Count > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicidy);
        }


        #endregion

        #region GetProvenancesBySearchCriteriaTest

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesNoTaxonIds_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<string>() { "urn:lsid:swedishlifewatch.se:DataProvider:6" }; // DataProvider:6 = NORS
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish
            searchCriteria.TaxonIds = null; // use no taxon ids

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesTaxonIds_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.DataProviderGuids = new List<string>() { "urn:lsid:swedishlifewatch.se:DataProvider:4" }; // DataProvider:4 = ArtPortalen1
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesTaxonIds_ExpectsListElasticsearch()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            //SetDefaultSearchCriteria(searchCriteria);
            //searchCriteria.DataProviderGuids = new List<string>() { "urn:lsid:swedishlifewatch.se:DataProvider:1" }; // DataProvider:1 = ArtPortalen
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteriaElasticsearch(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count == 4);
            Assert.IsTrue(speciesObservationProvenances[0].Values.Count == 20);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteriaTaxonIds()
        {
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria = new WebSpeciesObservationSearchCriteria();
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.Grasshoppers));
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_SearchCriteriaIsEmpty_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetProvenancesBySearchCriteria_SearchCriteriaIsNull_ExpectsArgumentNullException()
        {
            // Arrange
            // Search Criteria Is Null

            // Act
            AnalysisManager.GetProvenancesBySearchCriteria(Context, null, null);

            // Assert
            Assert.Fail("No argument null exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesProjectParameterFieldSearchCriteria_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish

            SetProjectParameterFieldSearchCriterias(searchCriteria);

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesOwnerFieldSearchCriteria_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish

            SetOwnerFieldSearchCriterias(searchCriteria);

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [Ignore]
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesLocationIdFieldSearchCriteria_ExpectsList()
        {
            // Test is ignored since NORS data does not currently have any location id values.
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish

            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(206198);

            searchCriteria.DataProviderGuids = new List<string>() { "urn:lsid:swedishlifewatch.se:DataProvider:6" }; // DataProvider:6 = Nors

            SetLocationIdFieldSearchCriterias(searchCriteria);

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesOrCombinedHabitatAndSubstrateFieldSearchCriterias_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish

            SetOrCombinedFieldSearchCriterias(searchCriteria);

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesIndividualCountFieldSearchCriteria_ExpectsList()
        {
            // Arrange
            WebCoordinateSystem coordinateSystem;
            WebSpeciesObservationSearchCriteria searchCriteria;
            List<WebSpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new WebCoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new WebSpeciesObservationSearchCriteria();
            Context.Locale = Context.GetDefaultLocale();
            Context.Locale.Id = 175; // Swedish

            searchCriteria.DataProviderGuids = new List<string>() { "urn:lsid:swedishlifewatch.se:DataProvider:1" };

            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(208249);

            SetIndividualCountFieldSearchCriteria(searchCriteria);

            // Act
            speciesObservationProvenances = AnalysisManager.GetProvenancesBySearchCriteria(Context, searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        #endregion

        [TestMethod]
        public void UnitOfWork_WhenStateUnderTest_ThenExpectedBehavior()
        {
            //Arrange
            var coordinateSystem = new WebCoordinateSystem {Id = CoordinateSystemId.GoogleMercator};
            
            var gridSpecification = new ArtDatabanken.WebService.Data.WebGridSpecification
            {
                BoundingBox = null,                
                GridCellGeometryType = GridCellGeometryType.Polygon,
                GridCellSize = 50000,
                GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v,                
                IsGridCellSizeSpecified = true
            };

            var searchCriteria = new ArtDatabanken.WebService.Data.WebSpeciesObservationSearchCriteria
            {
                Accuracy = 0,
                BirdNestActivityLimit = 0,
                BoundingBox = null,
                ChangeDateTime = null,
                DataFields = new System.Collections.Generic.List<ArtDatabanken.WebService.Data.WebDataField>
                {
                    new ArtDatabanken.WebService.Data.WebDataField
                    {
                        Information = null,
                        Name = "FieldLogicalOperator",
                        Type = WebDataType.String,
                        Unit = null,
                        Value = "And"
                    }
                },
                DataProviderGuids = new System.Collections.Generic.List<string>
                {
                    "urn:lsid:swedishlifewatch.se:DataProvider:1",
                    "urn:lsid:swedishlifewatch.se:DataProvider:2",
                    "urn:lsid:swedishlifewatch.se:DataProvider:3",
                    "urn:lsid:swedishlifewatch.se:DataProvider:4",
                    "urn:lsid:swedishlifewatch.se:DataProvider:5",
                    "urn:lsid:swedishlifewatch.se:DataProvider:6",
                    "urn:lsid:swedishlifewatch.se:DataProvider:7",
                    "urn:lsid:swedishlifewatch.se:DataProvider:8",
                    "urn:lsid:swedishlifewatch.se:DataProvider:9",
                    "urn:lsid:swedishlifewatch.se:DataProvider:10",
                    "urn:lsid:swedishlifewatch.se:DataProvider:11",
                    "urn:lsid:swedishlifewatch.se:DataProvider:12",
                    "urn:lsid:swedishlifewatch.se:DataProvider:13",
                    "urn:lsid:swedishlifewatch.se:DataProvider:14",
                    "urn:lsid:swedishlifewatch.se:DataProvider:15",
                    "urn:lsid:swedishlifewatch.se:DataProvider:16",
                    "urn:lsid:swedishlifewatch.se:DataProvider:17"
                },                
                IncludeNeverFoundObservations = false,
                IncludeNotRediscoveredObservations = false,
                IncludePositiveObservations = true,                
                IncludeRedlistedTaxa = false,
                IsAccuracyConsidered = false,
                IsAccuracySpecified = false,
                IsBirdNestActivityLimitSpecified = false,
                IsDataChecked = false,
                IsDisturbanceSensitivityConsidered = false,
                IsIsNaturalOccurrenceSpecified = true,
                IsMaxProtectionLevelSpecified = false,
                IsMinProtectionLevelSpecified = false,
                IsNaturalOccurrence = true,
                LocalityNameSearchString = null,
                MaxProtectionLevel = 0,
                MinProtectionLevel = 0,
                ObservationDateTime = null,                
                ObserverSearchString = null,                
                RegionLogicalOperator = LogicalOperator.And,                                
                TaxonIds = new System.Collections.Generic.List<int>
                {
                    100573
                },                
            };

            //Act
            AnalysisManager.GetGridSpeciesCounts(Context, searchCriteria, gridSpecification, coordinateSystem);            

            //Assert

        }


        //[TestMethod]
        //[Ignore]
        //public void testtt()
        //{
        //    var coordinateSystem = new WebCoordinateSystem { Id = CoordinateSystemId.GoogleMercator };
        //    var searchCriteria = new WebSpeciesObservationSearchCriteria { TaxonIds = new List<int> { Convert.ToInt32(TaxonId.Grasshoppers) } }; //Hopprätvingar, inte gräshoppor
        //    AnalysisManager.testt(Context, searchCriteria, coordinateSystem);
        //}
    }
}
