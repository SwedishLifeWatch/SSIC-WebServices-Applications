using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebService.Client.SpeciesObservationService;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using AnalysisDataSource = ArtDatabanken.WebService.Client.AnalysisService.AnalysisDataSource;
//using ArtDatabanken.GIS.WFS;

namespace ArtDatabanken.WebService.Client.Test.Data
{
   

    [TestClass]
    public class AnalysisManagerTest: TestBase
    {
        #region Test setup

        private AnalysisManager _analysisManager;

        private void CheckSpeciesObservationExist(SpeciesObservationList speciesObservations)
        {
            Assert.IsNotNull(speciesObservations);
            Assert.IsTrue(speciesObservations.Count > 0);
        }


        public AnalysisManagerTest()
        {
            _analysisManager = null;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetActionPlanTaxa()
        {
            IFactor factor;
            ISpeciesFactSearchCriteria speciesFactSearchCriteria;
            ISpeciesFactFieldSearchCriteria speciesFactFieldSearchCriteria;
            TaxonList taxa;

            speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
            factor = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.ActionPlan);
            speciesFactSearchCriteria.Factors = new FactorList();
            speciesFactSearchCriteria.Factors.Add(factor);
            speciesFactSearchCriteria.IndividualCategories = new IndividualCategoryList();
            speciesFactSearchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext()));
            speciesFactSearchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            speciesFactFieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
            speciesFactFieldSearchCriteria.FactorField = factor.DataType.Field1;
            speciesFactFieldSearchCriteria.Operator = CompareOperator.Equal;
            speciesFactSearchCriteria.FieldSearchCriteria.Add(speciesFactFieldSearchCriteria);
            foreach (IFactorFieldEnumValue enumValue in factor.DataType.Field1.Enum.Values)
            {
                // ReSharper disable once PossibleInvalidOperationException
                speciesFactFieldSearchCriteria.AddValue(enumValue.KeyInt.Value);
            }

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new TaxonList();
            taxa.Merge(CoreData.AnalysisManager.GetTaxa(GetUserContext(), speciesFactSearchCriteria));
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        private AnalysisManager GetAnalysisManager()
        {
            return GetAnalysisManager(false);
        }

        private AnalysisManager GetAnalysisManager(Boolean refresh)
        {
            if (_analysisManager.IsNull() || refresh)
            {
                _analysisManager = new AnalysisManager();
                _analysisManager.DataSource = new AnalysisDataSource();
            }
            return _analysisManager;
        }

        #endregion


        #region Additional tests

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void Constructor()
        {
            AnalysisManager analysisManager;

            analysisManager = new AnalysisManager();
            Assert.IsNotNull(analysisManager);
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
            searchCriteria.Accuracy = 5;

            IGridSpecification gridSpecification = new GridSpecification();
            // webGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<IGridCellSpeciesCount> noOfGridCellObservations = GetAnalysisManager().GetGridSpeciesCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            // Use another setting than default
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            IList<IGridCellSpeciesCount> noOfGridCellObservations2 = GetAnalysisManager().GetGridSpeciesCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

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
            Assert.IsTrue(noOfGridCellObservations[0].ObservationCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].SpeciesCount > 0);
            Assert.IsTrue(noOfGridCellObservations[0].ObservationCount >= noOfGridCellObservations[0].SpeciesCount);

            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellCentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellSize == 50000);
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
        [TestCategory("IntegrationTest")]
        public void GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_Sweref99CoordinateSystem_ReturnListSuccessfully()
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

            IList<IGridCellCombinedStatistics> result = GetAnalysisManager().GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(GetUserContext(), gridSpecification, searchCriteria, featureStatisticsSummary, featuresUrl, null, coordinateSystem);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 70);
            Assert.IsTrue(result.Count < 100);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public void GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts_WfsFilteringDalarnaPolygon_ReturnListSuccessfully()
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
            gridSpecification.GridCellSize = 10000; // Each square is 10 * 10 km.
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;
            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs?service=wfs&version=1.1.0&request=GetFeature&typeName=SLW:Sverigekarta_med_lan&filter=<Filter><PropertyIsEqualTo><PropertyName>LänSKOD</PropertyName><Literal>20</Literal></PropertyIsEqualTo></Filter>";

            searchCriteria = new SpeciesObservationSearchCriteria();

            IList<IGridCellCombinedStatistics> result = GetAnalysisManager().GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(GetUserContext(), gridSpecification, searchCriteria, featureStatisticsSummary, featuresUrl, null, coordinateSystem);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 4000);
            Assert.IsTrue(result.Count < 5000);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.IsNotNull(result[i].GridCellCentreCoordinate);
                Assert.IsNotNull(result[i].OrginalGridCellCentreCoordinate);                
            }
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
            searchCriteria.Accuracy = 2;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.Polygon;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisManager().GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            // Use another setting than default
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisManager().GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

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
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellSize == 50000);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.SWEREF99_TM.ToString()));
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[0].X > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[2].X > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[0].Y > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox.LinearRings[0].Points[2].Y > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountsGeometryTypeCenterPointTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 2;
            IGridSpecification gridSpecification = new GridSpecification();
            //IGridSpecification.GridCoordinateSystem = GridCoordinateSystem.RT90;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.GridCellGeometryType = GridCellGeometryType.CentrePoint;

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisManager().GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            // Use another setting than default
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.SWEREF99_TM;
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations2 = GetAnalysisManager().GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellCentreCoordinate.X > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellCentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations[0].GridCellSize == 10000);
            //Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.RT90.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.Rt90_25_gon_v.ToString()));
            Assert.IsTrue(noOfGridCellObservations[0].GridCellBoundingBox == null);
           
            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellCentreCoordinate.Y > 0);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellSize == 50000);
            Assert.IsTrue(noOfGridCellObservations2[0].GridCoordinateSystem.ToString().Equals(GridCoordinateSystem.SWEREF99_TM.ToString()));
            Assert.IsTrue(noOfGridCellObservations2[0].GridCellBoundingBox == null);
            
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetGridCellSpeciesObservationCountFromDifferentMethodsTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago
            searchCriteria.TaxonIds = taxa;

            

            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisManager().GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);
            // Use another method than default
            IList<IGridCellSpeciesCount> noOfGridCellObservations2 = GetAnalysisManager().GetGridSpeciesCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);

            Assert.IsTrue(noOfGridCellObservations.Count > 0);
            Assert.IsTrue(noOfGridCellObservations[0].ObservationCount > 0);

            Assert.IsTrue(noOfGridCellObservations2.Count > 0);
            List<IGridCellSpeciesObservationCount> noOfGridCellObservationsSortedList = noOfGridCellObservations.OrderBy(o => o.ObservationCount).ThenBy(o => o.GridCellCentreCoordinate.X).ToList();
            List<IGridCellSpeciesCount> noOfGridCellObservations2SortedList = noOfGridCellObservations2.OrderBy(o => o.ObservationCount).ThenBy(o => o.GridCellCentreCoordinate.X).ToList();

            Assert.IsTrue(noOfGridCellObservations.Count >= noOfGridCellObservations2.Count);
            if (noOfGridCellObservations.Count == noOfGridCellObservations2.Count)
            {
                for (int i = 0; i < noOfGridCellObservations2SortedList.Count; i++)
                {
                    Assert.IsTrue(noOfGridCellObservationsSortedList[i].ObservationCount >= noOfGridCellObservations2SortedList[i].ObservationCount);
                }
            }
           
        }
        #endregion


        #region GetGridFeatureStatistics


        [TestMethod]
        [Ignore]
        [TestCategory("IntegrationTest")]
        public void GetGridFeatureStatisticsWithCompleteUrl()
        {
            ICoordinateSystem coordinateSystem;
            String featuresUrl;
            FeatureStatisticsSummary featureStatisticsSpecification;
            featureStatisticsSpecification = new FeatureStatisticsSummary();
            coordinateSystem = new CoordinateSystem();
            IGridSpecification gridSpecification = new GridSpecification();
          
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
//            WFSVersion version = WFSVersion.Ver110;
            
            gridSpecification.GridCellSize = 10000;
            gridSpecification.IsGridCellSizeSpecified = true;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;
            
            featureStatisticsSpecification.BoundingBox = new BoundingBox();
            featureStatisticsSpecification.BoundingBox.Max = new Point();
            featureStatisticsSpecification.BoundingBox.Min = new Point();

            gridSpecification.BoundingBox = new BoundingBox();
            gridSpecification.BoundingBox.Max = new Point();
            gridSpecification.BoundingBox.Min = new Point();

            featureStatisticsSpecification.BoundingBox.Max.X = 1521024; //= RT90 Y
            featureStatisticsSpecification.BoundingBox.Max.Y = 6937341; //= RT90 X
            featureStatisticsSpecification.BoundingBox.Min.X = 1457184;
            featureStatisticsSpecification.BoundingBox.Min.Y = 6875163;

            gridSpecification.BoundingBox.Max.X = 1489104;
            gridSpecification.BoundingBox.Max.Y = 6858363;
            gridSpecification.BoundingBox.Min.X = 1456064;
            gridSpecification.BoundingBox.Min.Y = 6842683;

            gridSpecification.GridCellSize = 10000;
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.GoogleMercator;
            gridSpecification.IsGridCellSizeSpecified = true;
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            featuresUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&service=wfs&request=GetFeature&version=1.1.0&typeName=SLW:MapOfSwedishCounties&srs=3857";


            IList<IGridCellFeatureStatistics> gridCellFeatureStatistics =
                GetAnalysisManager().GetGridFeatureStatistics(GetUserContext(),
                                                              featureStatisticsSpecification,
                                                              featuresUrl,
                                                              null, //typeName, 
                                                              gridSpecification,
                                                              coordinateSystem);

            Assert.IsTrue(gridCellFeatureStatistics.Count > 0);
            Assert.IsTrue(gridCellFeatureStatistics.Count.Equals(70));
            Assert.IsTrue(gridCellFeatureStatistics[0].GridCellBoundingBox.LinearRings[0].Points[0].X.Equals(1456064));
            Assert.IsTrue(gridCellFeatureStatistics[69].GridCellBoundingBox.LinearRings[0].Points[2].Y.Equals(6942683));
        }

        #endregion

        [TestMethod]
        public void GetHostsBySpeciesFactSearchCriteria()
        {
            TaxonList hosts;
            ISpeciesFactSearchCriteria searchCriteria;

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 1222)); // Värddjur
            hosts = GetAnalysisManager(true).GetHosts(GetUserContext(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Hosts = new TaxonList();
            searchCriteria.Hosts.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), (Int32)(TaxonId.Insects)));
            hosts = GetAnalysisManager().GetHosts(GetUserContext(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), 100011)); // Kungsörn
            hosts = GetAnalysisManager().GetHosts(GetUserContext(), searchCriteria);
            Assert.IsTrue(hosts.IsNotEmpty());
        }

        #region GetSpeciesObservationCountBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteria()
        {
            ICoordinateSystem coordinateSystem;
            Int64 noOfObservations;
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            noOfObservations = GetAnalysisManager().GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesObservationCountBySearchCriteriaFromDifferentMethodsTest()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 50;
            IGridSpecification gridSpecification = new GridSpecification();
            gridSpecification.GridCoordinateSystem = GridCoordinateSystem.Rt90_25_gon_v;
            gridSpecification.GridCellSize = 50000;
            gridSpecification.IsGridCellSizeSpecified = true;
            List<int> taxa = new List<int>();
            taxa.Add(101509); //Apollofjäril Redlisted NE-category
            taxa.Add(2002088);//Duvor
            taxa.Add(2002118);//Kråkfåglar
            taxa.Add(1005916);//Tussilago
            searchCriteria.TaxonIds = taxa;


            Int64 speciesObservationCount = GetAnalysisManager().GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            // Use another method than default
            IList<IGridCellSpeciesObservationCount> noOfGridCellObservations = GetAnalysisManager().GetGridSpeciesObservationCounts(GetUserContext(), searchCriteria, gridSpecification, coordinateSystem);


            Assert.IsTrue(speciesObservationCount > 0);
            Assert.IsTrue(noOfGridCellObservations.Count > 0);

            Int64 gridCellCount = 0;
            foreach (IGridCellSpeciesObservationCount webGridCellSpeciesObservationCount in noOfGridCellObservations)
            {
                gridCellCount = gridCellCount + webGridCellSpeciesObservationCount.ObservationCount;
            }
            Assert.IsTrue(speciesObservationCount == gridCellCount);

        }

        #endregion


        #region GetSpeciesCountBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetRedlistedTaxa()
        {
            IFactor factor;
            RedListCategory redListCategory;
            ISpeciesFactSearchCriteria speciesFactSearchCriteria;
            ISpeciesFactFieldSearchCriteria speciesFactFieldSearchCriteria;
            TaxonList taxa;

            speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
            factor = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            speciesFactSearchCriteria.Factors = new FactorList();
            speciesFactSearchCriteria.Factors.Add(factor);
            speciesFactSearchCriteria.IndividualCategories = new IndividualCategoryList();
            speciesFactSearchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext()));
            speciesFactSearchCriteria.Periods = new PeriodList();
            speciesFactSearchCriteria.Periods.Add(CoreData.FactorManager.GetCurrentPublicPeriod(GetUserContext()));

            speciesFactFieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
            speciesFactFieldSearchCriteria.FactorField = factor.DataType.Field1;
            speciesFactFieldSearchCriteria.Operator = CompareOperator.Equal;
            speciesFactSearchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            speciesFactSearchCriteria.FieldSearchCriteria.Add(speciesFactFieldSearchCriteria);
            for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
            {
                speciesFactFieldSearchCriteria.AddValue((Int32)redListCategory);
            }

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new TaxonList();
            taxa.Merge(CoreData.AnalysisManager.GetTaxa(GetUserContext(), speciesFactSearchCriteria));
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        private TaxonList GetRedlistedTaxaByRedListCategory(RedListCategory redListCategory)
        {
            IFactor factor;
            ISpeciesFactSearchCriteria speciesFactSearchCriteria;
            ISpeciesFactFieldSearchCriteria speciesFactFieldSearchCriteria;
            TaxonList taxa;

            speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
            factor = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.RedlistCategory);
            speciesFactSearchCriteria.Factors = new FactorList();
            speciesFactSearchCriteria.Factors.Add(factor);
            speciesFactSearchCriteria.IndividualCategories = new IndividualCategoryList();
            speciesFactSearchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext()));
            speciesFactSearchCriteria.Periods = new PeriodList();
            speciesFactSearchCriteria.Periods.Add(CoreData.FactorManager.GetCurrentPublicPeriod(GetUserContext()));

            speciesFactFieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
            speciesFactFieldSearchCriteria.FactorField = factor.DataType.Field1;
            speciesFactFieldSearchCriteria.Operator = CompareOperator.Equal;
            speciesFactSearchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            speciesFactSearchCriteria.FieldSearchCriteria.Add(speciesFactFieldSearchCriteria);
            speciesFactFieldSearchCriteria.AddValue((Int32)redListCategory);

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new TaxonList();
            taxa.Merge(CoreData.AnalysisManager.GetTaxa(GetUserContext(), speciesFactSearchCriteria));
            return taxa;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetRedlistedTaxaByRedListCategory()
        {
            RedListCategory redListCategory;
            TaxonList taxa;

            for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
            {
                taxa = GetRedlistedTaxaByRedListCategory(redListCategory);
                Assert.IsTrue(taxa.IsNotEmpty());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetSpeciesCountBySearchCriteria()
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;

            Int64 noOfObservations = GetAnalysisManager().GetSpeciesCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(noOfObservations > 0);
        }

        #endregion


        #region GetTaxaBySearchCriteria


        [TestMethod]
        public void GetTaxaByOrganismGroup()
        {
            IOrganismGroup organismGroup;
            TaxonList taxa;

            organismGroup = CoreData.FactorManager.GetDefaultOrganismGroups(GetUserContext())[5];
            taxa = GetAnalysisManager(true).GetTaxa(GetUserContext(), organismGroup);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaByOrganismGroups()
        {
            OrganismGroupList organismGroups;
            TaxonList taxa1, taxa2;

            organismGroups = new OrganismGroupList();
            organismGroups.Add(CoreData.FactorManager.GetDefaultOrganismGroups(GetUserContext())[5]); // Däggdjur
            taxa1 = GetAnalysisManager(true).GetTaxa(GetUserContext(), organismGroups);
            Assert.IsTrue(taxa1.IsNotEmpty());

            organismGroups.Add(CoreData.FactorManager.GetDefaultOrganismGroups(GetUserContext())[7]); // Grod- och kräldjur
            taxa2 = GetAnalysisManager(true).GetTaxa(GetUserContext(), organismGroups);
            Assert.IsTrue(taxa2.IsNotEmpty());
            Assert.IsTrue(taxa1.Count < taxa2.Count);
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
            taxa = GetAnalysisManager(true).GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Hosts = new TaxonList();
            searchCriteria.Hosts.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), 102656)); // Hedsidenbi.
            taxa = GetAnalysisManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 9)); // Ungar (juveniler)
            taxa = GetAnalysisManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 10)); // Vuxna (imago).
            taxa = GetAnalysisManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), (Int32)(FactorId.RedlistCategory)));
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 4)); // 2015
            taxa = GetAnalysisManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 5)); // 2013 HELCOM
            taxa = GetAnalysisManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetPeriod(GetUserContext(), 4)); // 2015
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(GetUserContext(), 10)); // Vuxna (imago).
            taxa = GetAnalysisManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsEmpty());

            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Taxa = new TaxonList();
            searchCriteria.Taxa.Add(CoreData.TaxonManager.GetTaxon(GetUserContext(), TaxonId.DrumGrasshopper));
            taxa = GetAnalysisManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [Ignore]
        public void GetTaxaBySpeciesFactSearchCriteriaForestAgency()
        {
            TaxonList taxa;
            ISpeciesFactSearchCriteria searchCriteria;

            // This test case only works in production.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 2648));
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext()));
            taxa = CoreData.AnalysisManager.GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [Ignore]
        public void GetTaxaBySpeciesFactSearchCriteriaForestAgencyIndicatorSpecies()
        {
            TaxonList taxa;
            ISpeciesFactSearchCriteria searchCriteria;

            // This test case only works in production.
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), 2023));
            searchCriteria.IncludeNotValidTaxa = true;
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext()));
            taxa = CoreData.AnalysisManager.GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaBySearchCriteriaFactFieldSearchCriteria()
        {
            TaxonList taxa;
            ISpeciesFactSearchCriteria searchCriteria;
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IncludeNotValidHosts = false;
            searchCriteria.IncludeNotValidTaxa = false;
            searchCriteria.Factors = new FactorList();

            IFactor factor = CoreData.FactorManager.GetFactor(GetUserContext(), 1001);
            searchCriteria.Factors.Add(factor);
            
            searchCriteria.FieldLogicalOperator = LogicalOperator.Or;
            searchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();

            // Test Double species fact field condition.
            searchCriteria.FieldSearchCriteria.Add(new SpeciesFactFieldSearchCriteria());
            searchCriteria.FieldSearchCriteria[0].FactorField = GetFactorField(52, "tal2");
            searchCriteria.FieldSearchCriteria[0].Operator = CompareOperator.GreaterOrEqual;
            searchCriteria.FieldSearchCriteria[0].AddValue("11");

            taxa = GetAnalysisManager().GetTaxa(GetUserContext(), searchCriteria);
            Assert.IsTrue(taxa.Count > 0);
        }

        private IFactorField GetFactorField(Int32 factorDataTypeId, String databaseFieldName)
        {
            FactorDataTypeList factorDataTypes;

            factorDataTypes = CoreData.FactorManager.GetFactorDataTypes(GetUserContext());
            foreach (IFactorDataType factorDataType in factorDataTypes)
            {
                if (factorDataType.Id == factorDataTypeId)
                {
                    foreach (IFactorField factorField in factorDataType.Fields)
                    {
                        if (factorField.DatabaseFieldName == databaseFieldName)
                        {
                            return factorField;
                        }
                    }
                }
            }

            return null;
        }

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetTaxaBySearchCriteriaTest()
        {

            // Test accurancy
            ICoordinateSystem coordinateSystem;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.WGS84;

            ISpeciesObservationSearchCriteria searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.Accuracy = 1;
            searchCriteria.IncludePositiveObservations = true;

            TaxonList taxonList = GetAnalysisManager().GetTaxaBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);

            // Increase Accurancy
            searchCriteria.Accuracy = 10;
            TaxonList taxonList2 = GetAnalysisManager().GetTaxaBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(taxonList2.Count > taxonList.Count);


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
            searchCriteria.Accuracy = 1;
            searchCriteria.IncludePositiveObservations = true;

            TaxonSpeciesObservationCountList taxonList = GetAnalysisManager().GetTaxaWithSpeciesObservationCountsBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            Assert.IsTrue(taxonList.Count > 0);

            // Increase Accurancy
            searchCriteria.Accuracy = 10;
            TaxonSpeciesObservationCountList taxonList2 = GetAnalysisManager().GetTaxaWithSpeciesObservationCountsBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            Assert.IsTrue(taxonList2.Count > taxonList.Count);


        }
        #endregion

        #region TimeSeries

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void GetYearlyTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            TimeStepSpeciesObservationCountList timeStepList;
            //Test time step type year
            Periodicity periodicity = Periodicity.Yearly;
            CoordinateSystem coordinateSystem;
            SpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.ObservationDateTime = new DateTimeSearchCriteria();
            searchCriteria.ObservationDateTime.Operator = CompareOperator.Excluding;
            searchCriteria.ObservationDateTime.Begin = new DateTime(2000, 01, 01);
            searchCriteria.ObservationDateTime.End = new DateTime(2010, 12, 31);
            searchCriteria.ObservationDateTime.Accuracy = new TimeSpan(0, 0, 0, 0);

            timeStepList = GetAnalysisManager().GetTimeSpeciesObservationCounts(GetUserContext(), searchCriteria, periodicity, coordinateSystem);
            Assert.IsTrue(timeStepList.Count > 0);
            Assert.IsTrue(timeStepList[0].ObservationCount > 0);
            Assert.AreEqual(timeStepList[0].Periodicity, periodicity);
            
            long sum = 0;

            foreach (TimeStepSpeciesObservationCount item in timeStepList)
            {
                sum = sum + item.ObservationCount;
            }

            long count = GetAnalysisManager().GetSpeciesObservationCountBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

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
            ISpeciesObservationSearchCriteria searchCriteria;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            
            // Act
            var speciesObservationProvenances = GetAnalysisManager().GetSpeciesObservationProvenancesBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);
            
            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetNatura2000Taxa()
        {
            IFactor factor;
            ISpeciesFactSearchCriteria speciesFactSearchCriteria;
            ISpeciesFactFieldSearchCriteria speciesFactFieldSearchCriteria;
            TaxonList taxa;

            speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
            factor = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Natura2000BirdsDirective);
            speciesFactSearchCriteria.Factors = new FactorList();
            speciesFactSearchCriteria.Factors.Add(factor);
            speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Natura2000HabitatsDirectiveArticle2));
            speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Natura2000HabitatsDirectiveArticle4));
            speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.Natura2000HabitatsDirectiveArticle5));
            speciesFactSearchCriteria.IndividualCategories = new IndividualCategoryList();
            speciesFactSearchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext()));
            speciesFactFieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
            speciesFactFieldSearchCriteria.AddValue(Boolean.TrueString);
            speciesFactFieldSearchCriteria.FactorField = factor.DataType.Field1;
            speciesFactFieldSearchCriteria.Operator = CompareOperator.Equal;
            speciesFactSearchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            speciesFactSearchCriteria.FieldSearchCriteria.Add(speciesFactFieldSearchCriteria);

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new TaxonList();
            taxa.Merge(CoreData.AnalysisManager.GetTaxa(GetUserContext(), speciesFactSearchCriteria));
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProtectedByLawTaxa()
        {
            IFactor factor;
            ISpeciesFactSearchCriteria searchCriteria;
            TaxonList taxa;

            searchCriteria = new SpeciesFactSearchCriteria();
            factor = CoreData.FactorManager.GetFactor(GetUserContext(), FactorId.ProtectedByLaw);
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.Add(factor);
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(GetUserContext()));

            // The merge is necessary if there are problems in Dyntaxa.
            taxa = new TaxonList();
            taxa.Merge(CoreData.AnalysisManager.GetTaxa(GetUserContext(), searchCriteria));
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenancesBySearchCriteria_UsesOwnerFieldSearchCriteria_ExpectsList()
        {
            // Arrange
            CoordinateSystem coordinateSystem;
            SpeciesObservationSearchCriteria searchCriteria;
            List<SpeciesObservationProvenance> speciesObservationProvenances;

            coordinateSystem = new CoordinateSystem();
            coordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            searchCriteria = new SpeciesObservationSearchCriteria();
            searchCriteria.TaxonIds = new List<Int32>();
            searchCriteria.TaxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            SetOwnerFieldSearchCriterias(searchCriteria);

            // Act
            speciesObservationProvenances = GetAnalysisManager().GetSpeciesObservationProvenancesBySearchCriteria(GetUserContext(), searchCriteria, coordinateSystem);

            // Assert
            Assert.IsTrue(speciesObservationProvenances.Count > 0);
        }

        #endregion

        private static void SetOwnerFieldSearchCriterias(SpeciesObservationSearchCriteria searchCriteria)
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

    }
}
