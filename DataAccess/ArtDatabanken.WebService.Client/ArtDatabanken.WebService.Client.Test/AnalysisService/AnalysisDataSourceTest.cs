using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalysisDataSource = ArtDatabanken.WebService.Client.AnalysisService.AnalysisDataSource;

namespace ArtDatabanken.WebService.Client.Test.AnalysisService
{
    [TestClass]
    public class AnalysisDataSourceTest : TestBase
    {
        private AnalysisDataSource _analysisDataSource;

        public AnalysisDataSourceTest()
        {
            _analysisDataSource = null;
        }
        
        private AnalysisDataSource GetAnalysisDataSource(Boolean refresh = false)
        {
            if (_analysisDataSource.IsNull() || refresh)
            {
                _analysisDataSource = new AnalysisDataSource();
            }

            return _analysisDataSource;
        }

        #region Other Tests

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Constructor()
        {
            AnalysisDataSource analysisDataSource;

            analysisDataSource = new AnalysisDataSource();
            Assert.IsNotNull(analysisDataSource);
        }

        #endregion
       
        #region GetGridCellSpeciesCounts

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesCountsTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
          
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            // IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 5000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<IGridCellSpeciesCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            // Use another setting than default
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            IList<IGridCellSpeciesCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellCentreCoordinate.X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellCentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellSize == 5000);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.Rt90_25_gon_v.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.LinearRings[0].Points[0].X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.LinearRings[0].Points[2].X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.LinearRings[0].Points[0].Y > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.LinearRings[0].Points[2].Y > 0);
            Assert.IsTrue(noOfGridCellObservations[0].ObservationCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].ObservationCount >= noOfGridCellObservations[0].SpeciesCount);

            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellCentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellSize == 5000);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.SWEREF99_TM.ToString()));
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[0].X > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[2].X > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[0].Y > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[2].Y > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].ObservationCount > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].SpeciesCount > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].ObservationCount >= noOfGridCellObservations2[0].SpeciesCount);

        }


        #endregion

        #region GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts

        [TestMethod]
        [Ignore]
        public void 
            GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_Sweref99CoordinateSystem_ReturnListSuccessfully()
        {
            ICoordinateSystem coordinateSystem;
            IGridSpecification gridSpecification;
            ISpeciesObservationSearchCriteria searchCriteria;
            FeatureStatisticsSummary featureStatisticsSummary;
            string featuresUrl;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator; // Return the result converted to Google Mercator coordinates.
            featureStatisticsSummary = null;
            gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; // Grid specified in SWEREF99. Do calculations in this coordinate system.
            gridSpecification.GridCellSize = 100000; // Each square is 100 * 100 km.
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan";

            searchCriteria = new SpeciesObservationSearchCriteria();

            IList<IGridCellCombinedStatistics> result = GetAnalysisDataSource(true).GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(GetUserContext(), gridSpecification, searchCriteria, featureStatisticsSummary, featuresUrl, null, coordinateSystem);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 80);
        }
        #endregion

        #region GetGridCellSpeciesObservationCounts


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            //searchCriteria.Accuracy = 1;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 5000;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            // Use another setting than default
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellCentreCoordinate.X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellCentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellSize == 10000);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.Rt90_25_gon_v.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.LinearRings[0].Points[0].X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.LinearRings[0].Points[2].X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.LinearRings[0].Points[0].Y > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox.LinearRings[0].Points[2].Y > 0);

            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellCentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellSize == 5000);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.SWEREF99_TM.ToString()));
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[0].X > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[2].X > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[0].Y > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[2].Y > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetGridCellSpeciesObservationCountsFailedNoCriteriasSetTest()
            {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;

            ISpeciesObservationSearchCriteria searchCriteria = null;
            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

            }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsFailedNoCoordinateSystemSetTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = null;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsAccurrancyTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 50;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

            // Increase Accurancy
            searchCriteria.Accuracy = 1200;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsAccurancyFailedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            searchCriteria.Accuracy = -3;
            searchCriteria.IncludePositiveObservations = true;
            IGridSpecification gridSpecification = null;
           IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

           Assert.Fail("No  exception occur" +
                        "" +
                        "ed.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsIsAccurrancySpecifiedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            // Don't use accurancy, all positiv observations should be collected
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = null;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);


            // Enable Accurancy
            searchCriteria.Accuracy = 50;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count < noOfGridCellObservations.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsAccurracyIsLessThanZeroTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = -1;
            searchCriteria.IncludePositiveObservations = true;

            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsGridSpecificationBoundingBoxTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Test BoundingBbox
            IBoundingBox testBox = new BoundingBox();
            testBox.Max = new Point(75, 75);
            testBox.Min = new Point(0, 0);
            IBoundingBox testBox2 = new BoundingBox();
            testBox2.Max = new Point(820000, 6781000);
            testBox2.Min = new Point(560000, 6122000);


            searchCriteria.BoundingBox = testBox;
            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            searchCriteria.BoundingBox = null;
            gridSpecification.BoundingBox = testBox2;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsGridSpecificationBoundingBoxFailedTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            // Test BoundingBbox
            IBoundingBox testBox = new BoundingBox();
            testBox.Max = new Point(30, 30);
            testBox.Min = new Point(0, 0);
            IBoundingBox testBox2 = new BoundingBox();
            testBox.Max = new Point(90, 90);
            testBox.Min = new Point(0, 0);
            searchCriteria.BoundingBox = testBox;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.BoundingBox = testBox;
            gridSpecification.BoundingBox = testBox2;
            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_GoogleMercator_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;

            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
            searchCriteria.BoundingBox.Min = new Point(1113195, 1118890);

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_WGS84_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;

            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(89, 89);
            searchCriteria.BoundingBox.Min = new Point(10, 10);

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);



        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_SWEREF99_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            // Test BoundingBox
            searchCriteria.BoundingBox = new BoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            // SWEREF 99	6110000 – 7680000	260000 – 920000

            searchCriteria.BoundingBox.Max = new Point(820000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(560000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_RT90_25_gon_v_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;


            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new Point(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsBoundingBox_RT90_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();


            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new Point(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsBoundingBoxNoneTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.None;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;

            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(90, 90);
            searchCriteria.BoundingBox.Min = new Point(0, 0);

            searchCriteria.IncludePositiveObservations = true;

            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsBoundingBoxInvalidMaxMinValuesTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();
            //Ok boundig box values
            //searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            //searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);
            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                searchCriteria.BoundingBox.Min = new Point(9993195, 1118890);
                GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);


            }
            catch (Exception)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = new Point(1113195, 31118890);
                    GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

                }
                catch (Exception)
                {

                    // Ok if we get here
                    return;
                }
               
                Assert.Fail("No argument exception thrown that YMin value is larger that YMax value for bounding box.");

            }
           
            Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsBoundingBoxNullMaxMinValuesTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();

            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = null;
                searchCriteria.BoundingBox.Min = new Point(9993195, 1118890);
                GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            }
            catch (Exception)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = null;
                    GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

                }
                catch (Exception)
                {

                    // Ok if we get here
                    return;
                }
                
                Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");

            }
           
            Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsChangeDateTest()
        {
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations, noOfGridCellObservations2;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM; 
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;

            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 05, 01);
            searchCriteria.IncludePositiveObservations = true;
            noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Enlarge the search area regarding time

            searchCriteria.ChangeDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 01, 01);

            noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsChangePartOfYearTest1()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;

            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            // Get complete years data
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 06, 01);
            interval.End = new DateTime(2012, 07, 31);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 07, 01);
            interval.End = new DateTime(2012, 07, 31);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

                  
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations2.Count);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations3.Count);
             Assert.IsTrue(noOfGridCellObservations2.Count >= noOfGridCellObservations3.Count);
             
        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsChangePartOfYearTest2()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.ObservationDateTime = null;

            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
           
            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 06, 01);
            interval.End = new DateTime(2012, 06, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            DateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 07, 01);
            interval2.End = new DateTime(2012, 07, 10);
            intervals2.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals2;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations4 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Adding one more time interval
            intervals.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations5 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            DateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 06, 01);
            interval3.End = new DateTime(2012, 07, 10);
            intervals3.Add(interval3);
            searchCriteria.ChangeDateTime.PartOfYear = intervals3;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations6 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations6.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > noOfGridCellObservations4.Count);
            Assert.IsTrue(noOfGridCellObservations3.Count < noOfGridCellObservations5.Count);
            Assert.IsTrue(noOfGridCellObservations4.Count < noOfGridCellObservations5.Count);
            Assert.IsTrue(noOfGridCellObservations5.Count >= noOfGridCellObservations6.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsDataProvidersTest()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            //searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));

            ICoordinateSystem coordinateSystem;

            IGridSpecification gridSpecifications = new GridSpecification();
            //IGridSpecifications.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecifications.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecifications.GridCellSize = 10000;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            searchCriteria.DataSourceGuids = guids as List<string>;

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecifications, coordinateSystem);
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecifications, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);
            //Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsDataProviderInvalidTest()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;

            ICoordinateSystem coordinateSystem;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataInvalidProvider:1");
            searchCriteria.DataSourceGuids = guids as List<string>;

            searchCriteria.IncludePositiveObservations = true;

            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsDifferentGridCoordinateSystemsTestToWGS84()
        {
            // Test taxon ids.
            List<Int32> taxonIds;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservationsRT90;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservationsSWEREF99;
            ICoordinateSystem coordinateSystem;
            ISpeciesObservationSearchCriteria searchCriteria;
            IGridSpecification gridSpecification;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            taxonIds = new List<Int32>();
            taxonIds.Add(3000176); // Hopprätvingar
            searchCriteria.TaxonIds = taxonIds;

            gridSpecification = new GridSpecification();
            //gridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            noOfGridCellObservationsRT90 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservationsRT90.Count > 0);

            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;

            noOfGridCellObservationsSWEREF99 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservationsSWEREF99.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsDifferentGridCoordinateSystemsTestToGoogleMercatorTest()
        {

            // Giltiga värden för RT90 och SWEREF99 (Sverge värden)
            // System	    N-värde	             E-värde
            // RT90	        6110000 – 7680000	1200000 – 1900000
            // SWEREF 99	6110000 – 7680000	260000 – 920000

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservationsRT90;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservationsSWEREF99;
            ICoordinateSystem coordinateSystem;
            ISpeciesObservationSearchCriteria searchCriteria;
            IGridSpecification gridSpecification;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);


            gridSpecification = new GridSpecification();
            //gridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            noOfGridCellObservationsRT90 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservationsRT90.Count > 0);


            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;

            noOfGridCellObservationsSWEREF99 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservationsSWEREF99.Count > 0);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsIsNaturalOccurrenceTest()
        {
            ICoordinateSystem coordinateSystem;
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IGridSpecification gridSpecification = new GridSpecification();
            //gridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 500;
            searchCriteria.IncludePositiveObservations = true;
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(233790); // Större flamingo

            searchCriteria.TaxonIds = taxa;
            searchCriteria.IsNaturalOccurrence = false;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            searchCriteria.IsNaturalOccurrence = true;
            //searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);


            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations2.Count);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsLocalityTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 100;

            ICoordinateSystem coordinateSystem;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria localityString = new StringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
           // Can only set one stringCompareOperator 
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
           
        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsLocalityAllConditionsTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 100;

            ICoordinateSystem coordinateSystem;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria localityString = new StringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            // Can only set one stringCompareOperator 
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations4 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations5 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations6 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations6.Count > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsCriteriaObservationTypeTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations4 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations5 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations6 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations7 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations8 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

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
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsObservationDateTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 07, 25);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);


            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 10, 01);

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Enlarge the search area regarding time
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);
            Assert.IsTrue(noOfGridCellObservations3.Count >= noOfGridCellObservations2.Count);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsObservationDateCompareOperatorFailedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.NotEqual;
            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsObservationDateInvalidDatesTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 12, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;
            // Get complete years data
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            IDateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 06, 01);
            interval.End = new DateTime(2010, 07, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 08, 01);
            interval.End = new DateTime(2010, 11, 30);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get small part of a year data only one month but another interval
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            DateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 12, 01);
            interval2.End = new DateTime(2011, 07, 31);
            intervals2.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations4 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Adding one more time interval
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations5 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            DateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 08, 01);
            interval3.End = new DateTime(2011, 07, 31);
            intervals3.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations6 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

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
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearIsDayOfYearSpecifiedTest1()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 01, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2010, 07, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;


            // Get less amount of data since only two mounth within a year
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);


            // Get small part of a year data only one month but interval next year
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            DateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 08, 01);
            interval2.End = new DateTime(2011, 01, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

          
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
           

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearIsDayOfYearSpecifiedTest2()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 08, 01);
            interval.End = new DateTime(2010, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;



            // Get small part of a year data only one month but interval next year
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            DateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2011, 01, 01);
            interval2.End = new DateTime(2011, 05, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Adding one more time interval to the first one from aug to jan
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get the last two intervals but as one interval compare that on einterval and two interval is equal.
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            DateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 08, 01);
            interval3.End = new DateTime(2011, 05, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations4 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Not using day of year
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations5 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count >= noOfGridCellObservations3.Count);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetGridCellSpeciesObservationCountsObservationPartOfYearFailedTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 04, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2008, 03, 01);
            interval.End = new DateTime(2000, 06, 01);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsObserverSearchStringTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.Accuracy = 100;
            ICoordinateSystem coordinateSystem;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria operatorString = new StringSearchCriteria();
            operatorString.SearchString = "";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsObserverSearchStringAllConditionsTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 5;
            ICoordinateSystem coordinateSystem;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria operatorString = new StringSearchCriteria();
            operatorString.SearchString = "";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            // Can only set one stringCompareOperator 
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations4 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations5 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations6 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations3.Count > 0);
            Assert.IsTrue(noOfGridCellObservations4.Count > 0);
            Assert.IsTrue(noOfGridCellObservations5.Count > 0);
            Assert.IsTrue(noOfGridCellObservations6.Count > 0);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsPolygonsTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));

            // Test search criteria Polygons.
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > noOfGridCellObservations2.Count);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsPolygonsDifferentCoordinateSystemsTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
           IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Create polygon
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            searchCriteria.IncludePositiveObservations = true;
            // WGS84
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            //GoogleMercator
            ICoordinateSystem coordinateSystemMercator;
            coordinateSystemMercator = new CoordinateSystem();
            coordinateSystemMercator.Id = CoordinateSystemId.GoogleMercator;
            
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1970719.113095327, 8370644.1704083839)); //Uppsala E-N
            linearRing.Points.Add(new Point(1444869.9949174046, 8667823.4407080747));  //Tandådalen
            linearRing.Points.Add(new Point(1689906.68069054, 8241460.585692808));   //Örebro
            linearRing.Points.Add(new Point(2041443.6138615266, 7896601.1644738344));   //Visby
            linearRing.Points.Add(new Point(1970719.113095327, 8370644.1704083839));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystemMercator);

           
            //Rt90_25_gon_v
            ICoordinateSystem coordinateSystemRT90_25_gon_v;
            coordinateSystemRT90_25_gon_v = new CoordinateSystem();
            coordinateSystemRT90_25_gon_v.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883)); //Uppsala E-N
            linearRing.Points.Add(new Point(1348023.3768757628, 6788474.8526085708));  //Tandådalen
            linearRing.Points.Add(new Point(1464402.0051632109, 6573554.02424856));   //Örebro
            linearRing.Points.Add(new Point(1651196.4277014462, 6395804.2455542833));   //Visby
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations4 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystemRT90_25_gon_v);

            //SWEREF99
            ICoordinateSystem coordinateSystemSWEREF99;
            coordinateSystemSWEREF99 = new CoordinateSystem();
            coordinateSystemSWEREF99.Id = CoordinateSystemId.SWEREF99_TM;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(651349.75577459659, 6639918.25285019)); //Uppsala E-N
            linearRing.Points.Add(new Point(391358.12244400941, 6784781.6853031125));  //Tandådalen
            linearRing.Points.Add(new Point(510296.21811902762, 6571401.8266052864));   //Örebro
            linearRing.Points.Add(new Point(699151.04601517064, 6395960.6072938712));   //Visby
            linearRing.Points.Add(new Point(651349.75577459659, 6639918.25285019));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations5 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystemSWEREF99);


            // Since conversion between coordinate systems are not excact we have a bit of
            // difference in number of observations in our db searches. If conversion of
            // coordinate systems were exact the number of observations should not differ.
            // Allowing 3 % difference in result
            double delta = noOfGridCellObservations.Count * 0.03;
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations.Count == noOfGridCellObservations2.Count);
            Assert.AreEqual(noOfGridCellObservations.Count, noOfGridCellObservations4.Count, delta);
            Assert.AreEqual(noOfGridCellObservations.Count, noOfGridCellObservations5.Count, delta);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRegionTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.
            // Taxa list
            List<int> taxa = new List<int>();
            taxa.Add(3000176); // Hopprätvingar
            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsRegionTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            //searchCriteria.Accuracy = 10;
            // Test Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.

            // Taxa list
            List<int> taxa = new List<int>();
            //taxa.Add(3000176); // Hopprätvingar
            //taxa.Add(101509); //Appollofjäril Redlisted NE-category
            //taxa.Add(2002088);//Duvor
            //taxa.Add(2002118);//Kråkfåglar
            //taxa.Add(1005916);//Tussilago
            searchCriteria.TaxonIds = taxa;


            searchCriteria.IncludePositiveObservations = true;
            
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsRegistationDateTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            //searchCriteria.Accuracy = 10);
            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 05, 01);
            searchCriteria.IncludePositiveObservations = true;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Enlarge the search area regarding time

            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2011, 01, 01);

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > noOfGridCellObservations.Count);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsRegistationPartOfYearTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            // Get complete years data
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2010, 03, 31);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            // Get less amount of data only two mounth
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 04, 01);
            interval.End = new DateTime(2010, 04, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations3 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get small part of a year data 
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            DateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 05, 01);
            interval2.End = new DateTime(2010, 05, 10);
            intervals2.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals2;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations4 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Adding one more time interval
            intervals.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations5 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            DateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 04, 01);
            interval3.End = new DateTime(2010, 05, 10);
            intervals3.Add(interval3);
            searchCriteria.ChangeDateTime.PartOfYear = intervals3;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations6 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

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
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsRedListTaxaTest()
        {

            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            //searchCriteria.Accuracy = 10;
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            searchCriteria.IncludeRedlistedTaxa = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsRedListCategoriesTest()
        {

            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            //searchCriteria.Accuracy = 10;
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            List<RedListCategory> redListCategories = new List<RedListCategory>();
            RedListCategory redListCategory;
            redListCategory = RedListCategory.EN;
            redListCategories.Add(redListCategory);
            searchCriteria.IncludeRedListCategories = redListCategories;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsTaxaTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(2001274); // Myggor
            taxa.Add(2002088);// Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsUsedAllCriteriasTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;


            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            List<int> taxa = new List<int>();
            taxa.Add((Int32)(TaxonId.Carnivore));


            searchCriteria.TaxonIds = taxa;

            // Test BoundingBox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(89, 89);
            searchCriteria.BoundingBox.Min = new Point(10, 10);

            // Create polygon in WGS84
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            // Set Observation date and time interval.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            IDateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2000, 03, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Set dataproviders
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            searchCriteria.DataSourceGuids = guids as List<string>;

            // Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.


            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);
        }


        //[TestMethod]
        //[Ignore]
        //[TestCategory("NightlyTestApp")]
        //public void GetGridCellSpeciesObservationCountsPublicAutorityTest()
        //{
        //    ICoordinateSystem coordinateSystem;

        //    coordinateSystem = new ICoordinateSystem();
        //    coordinateSystem.Id = CoordinateSystemId.WGS84;

        //    ISpeciesObservationSearchCriteria searchCriteria = new ISpeciesObservationSearchCriteria();
        //    searchCriteria.Accuracy = 1;
        //    searchCriteria.IsAccuracySpecified = true;
        //    searchCriteria.IncludePositiveObservations = true;
        //    searchCriteria.MaxProtectionLevel = 1;
        //    searchCriteria.MinProtectionLevel = 1;
        //    IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridCellSpeciesObservationCounts(GetUserContext(), searchCriteria, null, coordinateSystem);

        //    Assert.IsTrue(noOfGridCellObservations.Count > 0);

        //}

        //[TestMethod]
        //[Ignore]
        //[TestCategory("NightlyTestApp")]
        //public void GetGridCellSpeciesObservationCountsHigherAutorityTest()
        //{
        //    ICoordinateSystem coordinateSystem;

        //    coordinateSystem = new ICoordinateSystem();
        //    coordinateSystem.Id = CoordinateSystemId.WGS84;

        //    ISpeciesObservationSearchCriteria searchCriteria = new ISpeciesObservationSearchCriteria();
        //    searchCriteria.Accuracy = 1;
        //    searchCriteria.IsAccuracySpecified = true;
        //    searchCriteria.IncludePositiveObservations = true;
        //    searchCriteria.MaxProtectionLevel = 5;
        //    //Todo Should be able to run from level 2 and higher
        //    searchCriteria.MinProtectionLevel = 1;
        //    IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridCellSpeciesObservationCounts(GetUserContext(), searchCriteria, null, coordinateSystem);

        //    Assert.IsTrue(noOfGridCellObservations.Count > 0);

        //}

        //[TestMethod]
        //[Ignore]
        //[TestCategory("NightlyTestApp")]
        //public void GetGridCellSpeciesObservationCountsTaxaAutorityTest()
        //{
        //    ICoordinateSystem coordinateSystem;

        //    coordinateSystem = new ICoordinateSystem();
        //    coordinateSystem.Id = CoordinateSystemId.WGS84;

        //    ISpeciesObservationSearchCriteria searchCriteria = new ISpeciesObservationSearchCriteria();
        //    searchCriteria.MaxProtectionLevel = 1;
        //    searchCriteria.MinProtectionLevel = 1;

        //    List<int> taxa = new List<int>();
        //    taxa.Add(Convert.ToInt32(TaxonId.GreenhouseMoths));
        //    taxa.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));

        //    List<int> authorityTaxa = new List<int>();
        //    taxa.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));

        //    searchCriteria.TaxonIds = taxa;
        //    // TODO make this autority work for taxa
        //    WebServiceGetUserContext() testGetUserContext() = GetUserContext();
        //    foreach (WebAuthority autority in testGetUserContext().CurrentRole.Authorities)
        //    {
        //        autority.TaxonGUIDs.Add("urn:lsid:dyntaxa.se:Taxon:101656");
        //    }

        //    IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridCellSpeciesObservationCounts(GetUserContext(), searchCriteria, null, coordinateSystem);

        //    Assert.IsTrue(noOfGridCellObservations.Count > 0);

        //}

        //[TestMethod]
        //[Ignore]
        //[TestCategory("NightlyTestApp")]
        //public void GetSpeciesObservationCountBySearchCriteriaAuthorityRegionsTest()
        //{
        //    ICoordinateSystem coordinateSystem;

        //    coordinateSystem = new ICoordinateSystem();
        //    coordinateSystem.Id = CoordinateSystemId.WGS84;

        //    ISpeciesObservationSearchCriteria searchCriteria = new ISpeciesObservationSearchCriteria();
        //    // Test bounding box
        //    List<int> regionIdTable = new List<int>();
        //    regionIdTable.Add(13); //Uppland

        //    searchCriteria.RegionGuids = new List<string>();
        //    // searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
        //    searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.

        //    WebServiceGetUserContext() testGetUserContext() = GetUserContext();
        //    WebRole role = testGetUserContext().CurrentRoles[0];
        //    WebAuthority authority = role.Authorities[0];
        //    authority.RegionGUIDs = new List<string>();
        //    authority.RegionGUIDs.Add("URN:LSID:artportalen.se:area:DataSet21Feature4");
        //    searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.

        //    // TODO RegionID is missing - failed - Dont work for this
        //    // IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisDataSource(true).GetGridCellSpeciesObservationCounts(testGetUserContext(), searchCriteria, null, coordinateSystem);
        //    Int64 noOfGridCellObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(testGetUserContext(), searchCriteria, coordinateSystem);


        //    Assert.IsTrue(noOfGridCellObservations > 0);

        //}


        #endregion


        #region  GetGridCellFeatureStatistics
        [Ignore]
        [TestMethod]
        public void GetGridCellFeatureStatistics_Rt90CoordinateSystemWithBoundingBoxInNorthDalarna_ReturnsGridStatisticsListSuccessfully()
        {
            String featuresUrl;
            CoordinateSystem coordinateSystem;
            GridSpecification gridSpecification;

            gridSpecification = new GridSpecification();
            coordinateSystem = new CoordinateSystem();
            
            gridSpecification.BoundingBox = new BoundingBox { Max = new Point(), Min = new Point() };
            
            gridSpecification.BoundingBox.Max.X = 1489104;
            gridSpecification.BoundingBox.Max.Y = 6858363;
            gridSpecification.BoundingBox.Min.X = 1400000;
            gridSpecification.BoundingBox.Min.Y = 6800000;

            gridSpecification.GridCellSize = 10000;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:MapOfSwedishCounties";


            
            List<IGridCellFeatureStatistics> gridCellFeatureStatistics = GetAnalysisDataSource(true).GetGridCellFeatureStatistics(GetUserContext(), null, featuresUrl, null,
                                                                                                      gridSpecification, coordinateSystem);
            Assert.IsTrue(gridCellFeatureStatistics.Count > 50);
            Assert.IsTrue(gridCellFeatureStatistics.Count < 100);
        }
       


        #endregion


        #region GetSpeciesObservationCountBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteria()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Set smaller interval than default
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 07, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 08, 01);

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GetSpeciesObservationCountBySearchCriteriaFailedNoCriteriasSetTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = null;
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaFailedNoCoordinateSystemSetTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = null;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaAccurrancyTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.Accuracy = 50;
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

            // Increase Accurancy
            searchCriteria.Accuracy = 1200;
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations2 > noOfObservations);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaAccurancyFailedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            searchCriteria.Accuracy = -3;
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument null exception occured.");
        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaIsAccurrancySpecifiedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Don't use accurancy, all positiv observations should be collected
            searchCriteria.Accuracy = null;
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);


            // Enable Accurancy
            searchCriteria.Accuracy = 50;
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations2 < noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaAccurracyIsLessThanZeroTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = -1;
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaIsNaturalOccurrenceTest()
        {
            ICoordinateSystem coordinateSystem;
            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;

            searchCriteria.IncludePositiveObservations = true;
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(233790); // Större flamingo

            searchCriteria.TaxonIds = taxa;
            searchCriteria.IsNaturalOccurrence = false;
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            searchCriteria.IsNaturalOccurrence = true;
           // searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);


            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_GoogleMercator_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
            searchCriteria.BoundingBox.Min = new Point(1113195, 1118890);

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_WGS84_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(89, 89);
            searchCriteria.BoundingBox.Min = new Point(10, 10);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);



        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_SWEREF99_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test BoundingBox
            searchCriteria.BoundingBox = new BoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.SWEREF99_TM;
            // SWEREF 99	6110000 – 7680000	260000 – 920000

            searchCriteria.BoundingBox.Max = new Point(820000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(560000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_RT90_25_gon_v_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();

            searchCriteria.IncludePositiveObservations = true;

            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new Point(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBox_RT90_Test()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();


            coordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
            // RT90	        6110000 – 7680000	1200000 – 1900000 ; Sverige

            searchCriteria.BoundingBox.Max = new Point(1300000, 6781000);
            searchCriteria.BoundingBox.Min = new Point(1250000, 6122000);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxNoneTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.None;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 2;
            // Test BoundingBbox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(90, 90);
            searchCriteria.BoundingBox.Min = new Point(0, 0);

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxInvalidMaxMinValuesTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();
            //Ok boundig box values
            //searchCriteria.BoundingBox.Max = new WebPoint(9907435, 30240972);
            //searchCriteria.BoundingBox.Min = new WebPoint(1113195, 1118890);
            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                searchCriteria.BoundingBox.Min = new Point(9993195, 1118890);
                GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            }
            catch (Exception)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = new Point(1113195, 31118890);
                    GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

                }
                catch (Exception)
                {

                    // Ok if we get here
                    return;
                }
               
                Assert.Fail("No argument exception thrown that YMin value is larger that YMax value for bounding box.");

            }
            
            Assert.Fail("No argument exception thrown that XMin value is larger that XMax value for bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaBoundingBoxNullMaxMinValuesTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();

            searchCriteria.IncludePositiveObservations = true;

            // Wgs84  coordinates max 89,89 min 10,10 giving the following mercator coordinates
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.BoundingBox = new BoundingBox();

            try
            {
                // Xmin > Xmax
                searchCriteria.BoundingBox.Max = null;
                searchCriteria.BoundingBox.Min = new Point(9993195, 1118890);
                GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            }
            catch (Exception)
            {
                try
                {
                    // Ymin > Ymax
                    searchCriteria.BoundingBox.Max = new Point(9907435, 30240972);
                    searchCriteria.BoundingBox.Min = null;
                    GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

                }
                catch (Exception)
                {

                    // Ok if we get here
                    return;
                }
                
                Assert.Fail("No argument exception thrown for Min values that is null in bounding box.");

            }
            
            Assert.Fail("No argument exception thrown for Max values that is null in bounding box.");

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaChangeDateTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2010, 07, 25);
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Enlarge the search area regarding time

            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2011, 01, 01);

            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaChangePartOfYearTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.ChangeDateTime = new DateTimeSearchCriteria();
            searchCriteria.ChangeDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ChangeDateTime.End = new DateTime(2011, 01, 01);
            searchCriteria.ChangeDateTime.Operator = CompareOperator.Excluding;

            // Get complete years data
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            IDateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 07, 01);
            interval.End = new DateTime(2010, 09, 30);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 07, 26);
            interval.End = new DateTime(2012, 08, 31);
            intervals.Add(interval);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data 
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            IDateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 07, 01);
            interval2.End = new DateTime(2012, 07, 25);
            intervals2.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Adding one more time interval
            // There is a bug here!!!! This is not working
            intervals.Add(interval2);
            searchCriteria.ChangeDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            IDateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 07, 01);
            interval3.End = new DateTime(2012, 08, 31);
            intervals3.Add(interval3);
            searchCriteria.ChangeDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);
            Assert.IsTrue(noOfObservations > noOfObservations3);
            Assert.IsTrue(noOfObservations > noOfObservations4);
            // Bug Bug Assert.IsTrue(noOfObservations >= noOfObservations5);
            Assert.IsTrue(noOfObservations > noOfObservations6);
            Assert.IsTrue(noOfObservations2 > noOfObservations3);
            Assert.IsTrue(noOfObservations3 < noOfObservations5);
            Assert.IsTrue(noOfObservations4 < noOfObservations5);
            Assert.IsTrue(noOfObservations5 >= noOfObservations6);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaDataProvidersTest()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            //searchCriteria.Accuracy = 1;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            searchCriteria.DataSourceGuids = guids as List<string>;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaDataProviderInvalidTest()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataInvalidProvider:1");
            searchCriteria.DataSourceGuids = guids as List<string>;

            searchCriteria.IncludePositiveObservations = true;

            GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaLocalityTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds = null;
            searchCriteria.Accuracy = 100;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria localityString = new StringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
          
            Assert.IsTrue(noOfObservations > 0);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaLocalityAllConditionsTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 100;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria localityString = new StringSearchCriteria();
            localityString.SearchString = "Solvik";
            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            localityString.CompareOperators = stringOperators;
            searchCriteria.LocalityNameSearchString = localityString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            // Can only set one stringCompareOperator 
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            Int64 noOfObservations4 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            Int64 noOfObservations5 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            Int64 noOfObservations6 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaObserverSearchStringTest()
        {

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            IStringSearchCriteria operatorString = new StringSearchCriteria();
            operatorString.SearchString = "";

            List<StringCompareOperator> stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Equal);
            operatorString.CompareOperators = stringOperators;
            searchCriteria.ObserverSearchString = operatorString;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            // Can only set one stringCompareOperator 
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Like);
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.BeginsWith);
            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.Contains);
            Int64 noOfObservations4 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.EndsWith);
            Int64 noOfObservations5 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            stringOperators = new List<StringCompareOperator>();
            stringOperators.Add(StringCompareOperator.NotEqual);
            Int64 noOfObservations6 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations6 > 0);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationTypeTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations4 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations5 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations6 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = false;

            Int64 noOfObservations7 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            searchCriteria.IncludePositiveObservations = false;
            searchCriteria.IncludeNeverFoundObservations = true;
            searchCriteria.IncludeNotRediscoveredObservations = true;

            Int64 noOfObservations8 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

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
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);


            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2013, 01, 01);

            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            // Enlarge the search area regarding time
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
            Assert.IsTrue(noOfObservations3 >= noOfObservations2);


        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateCompareOperatorFailedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Greater;
            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaObservationDateInvalidDatesTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2000, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            // No ObservationDateTime.Operator is set then dafult value is set - then we send exception
            GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaObservationPartOfYearTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 09, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;
            // Get obe and a half year data
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            IDateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 07, 01);
            interval.End = new DateTime(2010, 09, 30);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 12, 01);
            interval.End = new DateTime(2010, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data only one month but interval next year
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            IDateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2011, 01, 01);
            interval2.End = new DateTime(2011, 01, 31);
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Adding one more time interval
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            IDateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 12, 01);
            interval3.End = new DateTime(2011, 01, 31);
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

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
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaPartOfYearIsDayOfYearSpecifiedTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2011, 03, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.IncludePositiveObservations = true;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            IDateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 11, 01);
            interval.End = new DateTime(2010, 12, 31);
            interval.IsDayOfYearSpecified = true;
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;


            // Get less amount of data since only two mounth within a year
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);


            // Get small part of a year data only one month but interval next year
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            IDateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 12, 31);
            interval2.End = new DateTime(2011, 01, 31);
            interval2.IsDayOfYearSpecified = true;
            intervals2.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals2;
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Adding one more time interval to the first one from nov to jan
            intervals.Add(interval2);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval compare that on einterval and two interval is equal.
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            IDateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 11, 01);
            interval3.End = new DateTime(2011, 01, 31);
            interval3.IsDayOfYearSpecified = true;
            intervals3.Add(interval3);
            searchCriteria.ObservationDateTime.PartOfYear = intervals3;
            Int64 noOfObservations4 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Not using day of year
            searchCriteria.ObservationDateTime.PartOfYear[0].IsDayOfYearSpecified = false;
            Int64 noOfObservations5 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations3 > 0);
            Assert.IsTrue(noOfObservations4 > 0);
            Assert.IsTrue(noOfObservations5 > 0);
            Assert.IsTrue(noOfObservations4 >= noOfObservations3);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        [ExpectedException(typeof(System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>))]
        public void GetSpeciesObservationCountBySearchCriteriaPartOfYearFailedTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 04, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2003, 01, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            searchCriteria.IncludePositiveObservations = true;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2008, 06, 01);
            interval.End = new DateTime(2008, 03, 01);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.Fail("No Argument exception occured.");
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaPolygonsTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));

            // Test search criteria Polygons.
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations > noOfObservations2);

        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaPolygonsDifferentCoordinateSystemsTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            // Create polygon
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            searchCriteria.IncludePositiveObservations = true;
            // WGS84
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            //GoogleMercator
            ICoordinateSystem coordinateSystemMercator;
            coordinateSystemMercator = new CoordinateSystem();
            coordinateSystemMercator.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1970719.113095327, 8370644.1704083839)); //Uppsala E-N
            linearRing.Points.Add(new Point(1444869.9949174046, 8667823.4407080747));  //Tandådalen
            linearRing.Points.Add(new Point(1689906.68069054, 8241460.585692808));   //Örebro
            linearRing.Points.Add(new Point(2041443.6138615266, 7896601.1644738344));   //Visby
            linearRing.Points.Add(new Point(1970719.113095327, 8370644.1704083839));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystemMercator);
            //RT90
            ICoordinateSystem coordinateSystemRT90;
            coordinateSystemRT90 = new CoordinateSystem();
            coordinateSystemRT90.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883)); //Uppsala E-N
            linearRing.Points.Add(new Point(1348023.3768757628, 6788474.8526085708));  //Tandådalen
            linearRing.Points.Add(new Point(1464402.0051632109, 6573554.02424856));   //Örebro
            linearRing.Points.Add(new Point(1651196.4277014462, 6395804.2455542833));   //Visby
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystemRT90);

            //Rt90_25_gon_v
            ICoordinateSystem coordinateSystemRT90_25_gon_v;

            coordinateSystemRT90_25_gon_v = new CoordinateSystem();
            coordinateSystemRT90_25_gon_v.Id = CoordinateSystemId.Rt90_25_gon_v;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883)); //Uppsala E-N
            linearRing.Points.Add(new Point(1348023.3768757628, 6788474.8526085708));  //Tandådalen
            linearRing.Points.Add(new Point(1464402.0051632109, 6573554.02424856));   //Örebro
            linearRing.Points.Add(new Point(1651196.4277014462, 6395804.2455542833));   //Visby
            linearRing.Points.Add(new Point(1606325.0867813849, 6640376.3398303883));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            Int64 noOfObservations4 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystemRT90_25_gon_v);

            //SWEREF99
            ICoordinateSystem coordinateSystemSWEREF99;
            coordinateSystemSWEREF99 = new CoordinateSystem();
            coordinateSystemSWEREF99.Id = CoordinateSystemId.SWEREF99_TM;
            searchCriteria.Polygons.Clear();
            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(651349.75577459659, 6639918.25285019)); //Uppsala E-N
            linearRing.Points.Add(new Point(391358.12244400941, 6784781.6853031125));  //Tandådalen
            linearRing.Points.Add(new Point(510296.21811902762, 6571401.8266052864));   //Örebro
            linearRing.Points.Add(new Point(699151.04601517064, 6395960.6072938712));   //Visby
            linearRing.Points.Add(new Point(651349.75577459659, 6639918.25285019));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);
            Int64 noOfObservations5 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystemSWEREF99);


            // Since conversion between coordinate systems are not excact we have a bit of
            // difference in number of observations in our db searches. If conversion of
            // coordinate systems were exact the number of observations should not differ.
            // Allowing 0.2 % difference in result
            double delta = noOfObservations * 0.002;
            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations == noOfObservations2);
            Assert.AreEqual(noOfObservations, noOfObservations3, delta);
            Assert.AreEqual(noOfObservations, noOfObservations4, delta);
            Assert.AreEqual(noOfObservations, noOfObservations5, delta);


        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRegistrationDateTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.ObservationDateTime = null;
            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2011, 01, 01);
            searchCriteria.IncludePositiveObservations = true;
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Enlarge the search area regarding time

            searchCriteria.ReportedDateTime.Begin = new DateTime(2003, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 01, 01);

            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);

        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRegistrationPartOfYearTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            searchCriteria.ReportedDateTime = new DateTimeSearchCriteria();
            searchCriteria.ReportedDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ReportedDateTime.End = new DateTime(2013, 04, 15);
            searchCriteria.ReportedDateTime.Operator = CompareOperator.Excluding;

            // Get complete years data
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2010, 03, 31);
            intervals.Add(interval);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            // Get less amount of data since only two mounth
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data only one month
            intervals = new List<IDateTimeInterval>();
            interval = new DateTimeInterval();
            interval.Begin = new DateTime(2010, 02, 01);
            interval.End = new DateTime(2010, 02, 28);
            intervals.Add(interval);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            Int64 noOfObservations3 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get small part of a year data 
            List<IDateTimeInterval> intervals2 = new List<IDateTimeInterval>();
            DateTimeInterval interval2 = new DateTimeInterval();
            interval2.Begin = new DateTime(2010, 04, 01);
            interval2.End = new DateTime(2010, 04, 15);
            intervals2.Add(interval2);
            searchCriteria.ReportedDateTime.PartOfYear = intervals2;
            Int64 noOfObservations4 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Adding one more time interval
            intervals.Add(interval2);
            searchCriteria.ReportedDateTime.PartOfYear = intervals;
            Int64 noOfObservations5 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Get the last two intervals but as one interval
            List<IDateTimeInterval> intervals3 = new List<IDateTimeInterval>();
            DateTimeInterval interval3 = new DateTimeInterval();
            interval3.Begin = new DateTime(2010, 02, 01);
            interval3.End = new DateTime(2010, 04, 15);
            intervals3.Add(interval3);
            searchCriteria.ReportedDateTime.PartOfYear = intervals3;
            Int64 noOfObservations6 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

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
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRedListCategoriesTest()
        {

            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Appollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

          

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            searchCriteria.IncludeRedlistedTaxa = true;
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaRedListTaxaTest()
        {

            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

            searchCriteria.IncludePositiveObservations = true;

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            List<RedListCategory> redListCategories = new List<RedListCategory>();
            RedListCategory redListCategory;
            redListCategory = RedListCategory.EN;
            redListCategories.Add(redListCategory);
            searchCriteria.IncludeRedListCategories = redListCategories;
            Int64 noOfObservations2 = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(noOfObservations > 0);
            Assert.IsTrue(noOfObservations2 > 0);
            Assert.IsTrue(noOfObservations2 > noOfObservations);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaTaxaTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            // Test taxa list
            List<int> taxa = new List<int>();
            taxa.Add(2001274); // Myggor
            taxa.Add(2002088);// Duvor
            taxa.Add(2002118); //Kråkfåglar
            taxa.Add(1005916);//Tussilago

            searchCriteria.TaxonIds = taxa;

           

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);

        }


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaUsedAllCriteriasTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Butterflies));

            // Test BoundingBox
            searchCriteria.BoundingBox = new BoundingBox();
            searchCriteria.BoundingBox.Max = new Point(89, 89);
            searchCriteria.BoundingBox.Min = new Point(10, 10);

            // Create polygon in WGS84
            ILinearRing linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(17.703271, 59.869065)); //Uppsala E-N
            linearRing.Points.Add(new Point(12.979488, 61.18239));  //Tandådalen
            linearRing.Points.Add(new Point(15.18069, 59.28141));   //Örebro
            linearRing.Points.Add(new Point(18.33860, 57.66178));   //Visby
            linearRing.Points.Add(new Point(17.703271, 59.869065));
            IPolygon polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            searchCriteria.Polygons = new List<IPolygon>();
            searchCriteria.Polygons.Add(polygon);

            // Set Observation date and time interval.
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 12, 31);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;

            List<IDateTimeInterval> intervals = new List<IDateTimeInterval>();
            DateTimeInterval interval = new DateTimeInterval();
            interval.Begin = new DateTime(2000, 03, 01);
            interval.End = new DateTime(2000, 12, 31);
            intervals.Add(interval);
            searchCriteria.ObservationDateTime.PartOfYear = intervals;

            // Set dataproviders
            IList<string> guids = new List<string>();
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:1");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:2");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:3");
            guids.Add("urn:lsid:swedishlifewatch.se:DataProvider:4");
            searchCriteria.DataSourceGuids = guids as List<string>;

            // Regions
            searchCriteria.RegionGuids = new List<string>();
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature4"); // Södermanlands län.
            searchCriteria.RegionGuids.Add("URN:LSID:artportalen.se:area:DataSet21Feature3"); // Uppsala län.

            
            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        #endregion


        #region GetSpeciesCountBySearchCriteria

        [TestMethod]
        public void GetHostsBySpeciesFactSearchCriteria()
        {
            TaxonList hosts;
            ISpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 1222)); // Värddjur
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            hosts = GetAnalysisDataSource(true).GetHosts(GetUserContext(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Hosts = new TaxonList();
            searchCriteria.Hosts.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)(TaxonId.Insects)));
            hosts = GetAnalysisDataSource().GetHosts(GetUserContext(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), 100011)); // Kungsörn
            hosts = GetAnalysisDataSource().GetHosts(GetUserContext(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesCountBySearchCriteria()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            Int64 noOfObservations = GetAnalysisDataSource(true).GetSpeciesCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        #endregion


        #region GetTaxaBySearchCriteria


        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetTaxaBySearchCriteriaTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.Accuracy = 50;

            TaxonList taxonList = GetAnalysisDataSource(true).GetTaxaBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);

            // Increase Accurancy
            searchCriteria.Accuracy = 1200;
            TaxonList taxonList2 = GetAnalysisDataSource(true).GetTaxaBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(taxonList2.Count > taxonList.Count);
        }

        [TestMethod]
        public void GetTaxaBySpeciesFactSearchCriteria()
        {
            TaxonList taxa;
            ISpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 4)); // 2015
            taxa = GetAnalysisDataSource(true).GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Hosts = new TaxonList();
            searchCriteria.Hosts.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), 102656)); // Hedsidenbi.
            taxa = GetAnalysisDataSource().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 9)); // Ungar (juveniler)
            taxa = GetAnalysisDataSource().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 10)); // Vuxna (imago).
            taxa = GetAnalysisDataSource().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 4)); // 2015
            taxa = GetAnalysisDataSource().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 5)); // 2013 HELCOM
            taxa = GetAnalysisDataSource().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 4)); // 2015
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 10)); // Vuxna (imago).
            taxa = GetAnalysisDataSource().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper));
            taxa = GetAnalysisDataSource().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetTaxaWithSpeciesObservationCountsBySearchCriteriaTest()
        {
            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.Accuracy = 50;

            TaxonSpeciesObservationCountList taxonList = GetAnalysisDataSource(true).GetTaxaWithSpeciesObservationCountsBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);

            // Increase Accurancy
            searchCriteria.Accuracy = 1200;
            TaxonSpeciesObservationCountList taxonList2 = GetAnalysisDataSource(true).GetTaxaWithSpeciesObservationCountsBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(taxonList2.Count > taxonList.Count);


        }
        #endregion


        #region GetTimeSpeciesObservationCountsBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetYearlyTimeSpeciesObservationCountsTest()
        {
            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.IncludePositiveObservations = true;
            ICoordinateSystem coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;
            Periodicity type = Periodicity.Yearly;
            TimeStepSpeciesObservationCountList testList = GetAnalysisDataSource(true).GetTimeSpeciesObservationCounts(GetUserContext(), searchCriteria, type, coordinateSystem);
            Assert.IsTrue(testList.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetYearlyTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            TimeStepSpeciesObservationCountList timeStepList;
            //Test time step type year
            Periodicity periodicity = Periodicity.Yearly;
            ICoordinateSystem coordinateSystem;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.Grasshoppers));
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 12, 31);
            searchCriteria.ObservationDateTime.Accuracy = new TimeSpan();
            int days = searchCriteria.ObservationDateTime.Accuracy.Value.Days;
          

            timeStepList = GetAnalysisDataSource(true).GetTimeSpeciesObservationCounts(GetUserContext(), searchCriteria, periodicity, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].ObservationCount > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);

            long sum = 0;

            foreach (ITimeStepSpeciesObservationCount item in timeStepList)
            {
                sum = sum + item.ObservationCount;
            }

            long count = GetAnalysisDataSource(true).GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            // Should be 2915792 as in Analysis Service 2013-05-03
            Assert.AreEqual(sum, count);
        }

        #endregion

        #region GetSpeciesObservationProvenancesBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationProvenancesBySearchCriteria_Test()
        {
            // Assign
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);

            // Act
            var speciesObservationProvenances = GetAnalysisDataSource(true).GetSpeciesObservationProvenancesBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            
            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationProvenancesBySearchCriteria_FieldSearchCriteria_Test()
        {
            // Assign
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            SetDefaultSearchCriteria(searchCriteria);
            SetOwnerFieldSearchCriterias(searchCriteria);

            // Act
            var speciesObservationProvenances = GetAnalysisDataSource(true).GetSpeciesObservationProvenancesBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        #endregion

        #region Helper methods

        private static void SetDefaultSearchCriteria(ISpeciesObservationSearchCriteria searchCriteria)
        {
            searchCriteria.TaxonIds = new List<int>();
            searchCriteria.TaxonIds.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Begin = new DateTime(2010, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 08, 01);
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Including;
            searchCriteria.IncludePositiveObservations = true;
        }

        private static void SetOwnerFieldSearchCriterias(ISpeciesObservationSearchCriteria searchCriteria)
        {
            SpeciesObservationFieldSearchCriteriaList fieldSearchCriterias;
            fieldSearchCriterias = new SpeciesObservationFieldSearchCriteriaList();

            SetOwnerFieldSearchCriteria(fieldSearchCriterias);

            searchCriteria.FieldSearchCriteria = fieldSearchCriterias;
        }

        private static void SetOwnerFieldSearchCriteria(SpeciesObservationFieldSearchCriteriaList fieldSearchCriterias)
        {
            SpeciesObservationFieldSearchCriteria fieldSearchCriteria = new SpeciesObservationFieldSearchCriteria();

            fieldSearchCriteria.Class = new SpeciesObservationClass(SpeciesObservationClassId.DarwinCore);
            //fieldSearchCriteria.Operator = CompareOperator.Equal;
            fieldSearchCriteria.Operator = CompareOperator.Like;
            fieldSearchCriteria.Property = new SpeciesObservationProperty(SpeciesObservationPropertyId.Owner);
            fieldSearchCriteria.Type = DataType.String;
            //fieldSearchCriteria.Value = "Länsstyrelsen Östergötland";
            //fieldSearchCriteria.Value = "Per Flodin";
            fieldSearchCriteria.Value = "%Flodin";

            fieldSearchCriterias.Add(fieldSearchCriteria);
        }

        #endregion
    }
}
