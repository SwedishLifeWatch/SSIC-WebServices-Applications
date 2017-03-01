using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.AnalysisService.Database;
using ArtDatabanken.WebService.Database;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.AnalysisService.Test.Database
{
    [TestClass]
    public class AnalysisServerTest : TestBase
    {
        #region Test setup

        private AnalysisServer _database;

        public AnalysisServerTest()
        {
            _database = null;
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void Constructor()
        {
            using (AnalysisServer database = new AnalysisServer())
            {
                Assert.IsNotNull(database);
            }
        }

        private AnalysisServer GetDatabase(Boolean refresh = false)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    TestCleanup();
                    TestInitialize();
                }
                _database = Context.GetAnalysisDatabase();
            }
            return _database;
        }

        #endregion

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetAddress()
        {
            String address;

            address = AnalysisServer.GetAddress();
            Assert.IsTrue(address.IsNotEmpty());
        }



        #region GetGridCellSpeciesCounts

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesCountsTest()
        {
            List<Int32> regionIds, taxonIds;
            String whereCondition;
            List<Int64> speciesObservationIds;

            regionIds = new List<Int32>();
            speciesObservationIds = new List<Int64>();
            taxonIds = new List<Int32>();
            int accuracy = 3;
            int gridCellSize = 20000;
            whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " (O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ") ";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesCounts(null, regionIds, taxonIds, null, whereCondition, null, gridCellSize, speciesObservationIds))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        #endregion


        #region GetGridCellSpeciesObservationCounts

        [TestMethod]
        [TestCategory("NightlyTest")]
         public void GetGridCellSpeciesObservationCountsTest()
        {
            List<Int32> regionIds, taxonIds;
            String whereCondition;

            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            int accuracy = 3;
            whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " (O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ") "; ;
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, null, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsAccuracyTest()
        {
            List<Int32> regionIds, taxonIds;
            String whereCondition;
            Int32 accuracy;            

            regionIds = new List<Int32>();
            accuracy = 100;
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            string coordinateSystem = "RT90";
            int gridCellSize = 5000;
            whereCondition = "(O.protectionLevel = 1)" + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + accuracy + ")";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, null, whereCondition, coordinateSystem, gridCellSize, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsTaxaTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            string coordinateSystem = GridCoordinateSystem.SWEREF99_TM.ToString();
            coordinateSystem = "SWEREF99";
            int gridCellSize = 50000;
            whereCondition = "O.protectionLevel = 1";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, coordinateSystem, gridCellSize, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsRedListedTaxaTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(101509)); // Apollofjäril, Redlisted NE-category
            string coordinateSystem = "SWEREF99";
            int gridCellSize = 50000;
            whereCondition = "O.protectionLevel = 1";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, coordinateSystem, gridCellSize, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBoxSWEREF99Test()
        {
            List<Int32> regionIds, taxonIds;
            String whereCondition;

            // Test bounding box
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            // SWEREF 99	6110000 – 7680000	260000 – 920000
            double? boundingBoxMaxX = 820000;
            double? boundingBoxMinX = 560000;
            double? boundingBoxMaxY = 6781000;
            double? boundingBoxMinY = 6122000;
            int accuracy = 5;


            whereCondition = "O.protectionLevel = 1" + " AND " +
                                      "(O.coordinateX_SWEREF99 <= " + (Int32)(boundingBoxMaxX) + " AND " +
                                      " O.coordinateX_SWEREF99 >= " + (Int32)(boundingBoxMinX) + " AND " +
                                      " O.coordinateY_SWEREF99 <= " + (Int32)(boundingBoxMaxY) + " AND " +
                                      " O.coordinateY_SWEREF99 >= " + (Int32)(boundingBoxMinY) + ") " + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")"; 

            string coordinateSystem = "SWEREF99";
            int gridCellSize = 50000;

            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, null, whereCondition, coordinateSystem, gridCellSize, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }


        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsBoundingBoxRT90Test()
        {
            List<Int32> regionIds, taxonIds;
            String whereCondition;

            // Test bounding box
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();

            // RT90	        6110000 – 7680000	1200000 – 1900000
            double? boundingBoxMaxX = 1300000;
            double? boundingBoxMinX = 1250000;
            double? boundingBoxMaxY = 6781000;
            double? boundingBoxMinY = 6122000;

            int accuracy = 7;


            whereCondition = "O.protectionLevel = 1" + " AND " +
                                      "(O.coordinateX_RT90 <= " + (Int32)(boundingBoxMaxX) + " AND " +
                                      " O.coordinateX_RT90 >= " + (Int32)(boundingBoxMinX) + " AND " +
                                      " O.coordinateY_RT90 <= " + (Int32)(boundingBoxMaxY) + " AND " +
                                      " O.coordinateY_RT90 >= " + (Int32)(boundingBoxMinY) + ") " + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")"; 

            string coordinateSystem = "RT90";
            int gridCellSize = 10000;

            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, null, whereCondition, coordinateSystem, gridCellSize, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsDataProvidersTest()
        {
            List<Int32> regionIds, taxonIds;
            String whereCondition;
            string dataProviderId = "1";
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            int accuracy = 3;

            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " (O.dataProviderId = " + dataProviderId + " ) " + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")"; ;
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, null, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
       public void GetGridCellSpeciesObservationCountsObservationTypeTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " O.isNeverFoundObservation = 1 " + " OR " +
                             " O.isNotRediscoveredObservation = 1 " + " OR " +
                             " O.isPositiveObservation = 1 ";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObservationDateTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            DateTime begin = new DateTime(2003, 01, 01);
            DateTime end = new DateTime(2012, 01, 03);

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " (" + "O.[start]" + " >= '" + begin + "') AND " +
                             " (" + "O.[end]" + " <= '" + end + "') ";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsChangeDateTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            DateTime begin = new DateTime(2003, 01, 01);
            DateTime end = new DateTime(2012, 01, 03);

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " (" + "O.[modified]" + " >= '" + begin + "') AND " +
                             " (" + "O.[modified]" + " <= '" + end + "') ";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }

        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsReportedDateTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Carnivore));
            DateTime begin = new DateTime(2003, 01, 01);
            DateTime end = new DateTime(2012, 01, 03);

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " (" + "O.[reportedDate]" + " >= '" + begin + "') AND " +
                             " (" + "O.[reportedDate]" + " <= '" + end + "') ";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }

        }


        [TestMethod]
        [TestCategory("NightlyTest")]
         public void GetGridCellSpeciesObservationCountsLocalityTest()
        {

            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            //taxonIds.Add((Int32)(TaxonId.Carnivore));
            int accuracy = 100;

            joinCondition = null; // " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

            whereCondition = "O.protectionLevel = 1" + " AND " +
                         " (" + "O.[locality]" + " LIKE '" + "Djupvik" + "')" + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }


        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsRecordedByTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.Carnivore));


            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

            whereCondition = "O.protectionLevel = 1" + " AND " +
                            " (" + "O.[recordedBy]" + "... ";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }

        }


        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsIsNaturalOccuranceTest()
        {
            List<Int32> regionIds, taxonIds;
            String whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            int accuracy = 5;

            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " (O.isNaturalOccurrence = 1) " + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, null, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
         public void GetGridCellSpeciesObservationCountsIsNotNaturalOccuranceTest()
        {
            List<Int32> regionIds, taxonIds;
            String whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            int accuracy = 100;

            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " (O.isNaturalOccurrence = 0) " + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, null, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsObserverSearchStringTest()
        {

            List<Int32> regionIds, taxonIds;
            String whereCondition;
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            int accuracy = 100;

            whereCondition = "O.protectionLevel = 1" + " AND " +
                            " (" + "O.[recordedBy]" + " = '" + "" + "')" + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, null, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsPolygonTest()
        {

            List<Int32> regionIds, taxonIds;
            String whereCondition;
            List<SqlGeometry> polygons;

            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            polygons = new List<SqlGeometry>();


            //Uppsala, Tandådalen, Örebro, Visby
            StringBuilder polygonData = new StringBuilder("POLYGON ((17 59, 12 61, 15 59, 18 57, 17 59))");
            SqlGeometry polygon = SqlGeometry.Parse(new SqlString(polygonData.ToString()));
            polygons.Add(polygon);
            int accuracy = 50;

            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " O.isPositiveObservation = 1 " + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")"; ;

            string coordinateSystem = "RT90";
            int gridCellSize = 10000;

            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(polygons, regionIds, taxonIds, null, whereCondition, coordinateSystem, gridCellSize, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }


        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsRegionsTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;

            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            regionIds.Add(13);  //Uppland
            int accuracy = 50;
            whereCondition = "O.protectionLevel = 1" + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")"; ;
            joinCondition = " INNER JOIN #Regions AS R ON O.Point_GoogleMercator.STIntersects(R.Polygon) = 1 ";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(null, regionIds, taxonIds, joinCondition, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsUsingAllCriteriasTest()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;
            List<SqlGeometry> polygons;

            DateTime begin = new DateTime(2003, 01, 01);
            DateTime end = new DateTime(2012, 01, 03);
            string dataProviderId = "1";
            Int32 accuracy = 3;
       
            taxonIds = new List<Int32>();
            taxonIds.Add(3000176);
            taxonIds.Add((int)TaxonId.DrumGrasshopper);
            taxonIds.Add((int)TaxonId.Mammals);
            regionIds = new List<Int32>();
            regionIds.Add(13);  //Uppland
            polygons = new List<SqlGeometry>();

             // SWEREF 99	6110000 – 7680000	260000 – 920000
            double? boundingBoxMaxX = 820000;
            double? boundingBoxMinX = 560000;
            double? boundingBoxMaxY = 6781000;
            double? boundingBoxMinY = 6122000;

            //Uppsala, Tandådalen, Örebro, Visby
            StringBuilder polygonData = new StringBuilder("POLYGON ((17 59, 12 61, 15 59, 18 57, 17 59))");
            SqlGeometry polygon = SqlGeometry.Parse(new SqlString(polygonData.ToString()));
            polygons.Add(polygon);
            whereCondition = "O.protectionLevel = 1" + " AND " +
                             " (O.isNaturalOccurrence = 1) " + " AND " +
                             " (" + "O.[start]" + " >= '" + begin + "') AND " +
                             " (" + "O.[end]" + " <= '" + end + "') " + " AND " +
                             " O.isNeverFoundObservation = 1 " + " OR " +
                             " O.isNotRediscoveredObservation = 1 " + " OR " +
                             " O.isPositiveObservation = 1 " + " AND " +
                             " (O.dataProviderId = " + dataProviderId + " ) "+ " AND " +
                             " (O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ") " + " AND " +
                             " (O.coordinateX_SWEREF99 <= " + (Int32)(boundingBoxMaxX) + " AND " +
                             " O.coordinateX_SWEREF99 >= " + (Int32)(boundingBoxMinX) + " AND " +
                             " O.coordinateY_SWEREF99 <= " + (Int32)(boundingBoxMaxY) + " AND " +
                             " O.coordinateY_SWEREF99 >= " + (Int32)(boundingBoxMinY) + ") " + " AND " +
                             " (" + "O.[modified]" + " >= '" + begin + "') AND " +
                             " (" + "O.[modified]" + " <= '" + end + "') " + " AND " +
                             " (" + "O.[reportedDate]" + " >= '" + begin + "') AND " +
                             " (" + "O.[reportedDate]" + " <= '" + end + "') ";
            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
                     
            // TODO add jouin for both Region and taxonId" INNER JOIN #Regions AS Regions ON Observation.Point_GoogleMercator.STIntersects(Regions.Polygon) = 1 ";
            //" INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId  INNER JOIN #Regions AS Regions ON O.Point_GoogleMercator.STIntersects(Regions.Polygon) = 1 ";
            using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(polygons, regionIds, taxonIds, joinCondition, whereCondition, null, null, new List<Int64>()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

      
#endregion


        #region GetSpeciesCountBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesCountBySearchCriteriaTest()
        {
            Int64 speciesObservationCount;
            List<Int32> regionIds, taxonIds;
            String whereCondition;
            List<Int64> speciesObservationIds;

            regionIds = new List<Int32>();
            speciesObservationIds = new List<Int64>();
            taxonIds = new List<Int32>();
            taxonIds.Add(3000176);
            whereCondition = "O.protectionLevel = 1";
            speciesObservationCount = GetDatabase(true).GetSpeciesCountBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, speciesObservationIds);
            Assert.IsTrue(0 < speciesObservationCount);
        }

        #endregion


        #region GetSpeciesObservationCountBySearchCriteria

        [TestMethod]
                [TestCategory("NightlyTest")]
                public void GetSpeciesObservationCountBySearchCriteriaTest()
                {
                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String whereCondition;

                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                    whereCondition = "O.protectionLevel = 1";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);
                }

                [TestMethod]
                [TestCategory("NightlyTest")] 
                public void GetSpeciesObservationCountBySearchCriteriaAccuracyTest()
                {
                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String whereCondition;
                    Int32 accuracy;

                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                    taxonIds.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
                    accuracy = 3;
                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " (O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ") ";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);
                }

                [TestMethod]
                [TestCategory("NightlyTest")] 
                public void GetSpeciesObservationCountBySearchCriteriaDataProvidersTest()
                {
                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String whereCondition;
                    string dataProviderId = "1";
                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                    taxonIds.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " (O.dataProviderId = " + dataProviderId + " ) ";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);
                }

                [TestMethod]
                [TestCategory("NightlyTest")] 
                public void GetSpeciesObservationCountBySearchCriteriaObservationTypeTest()
                {
            
                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String joinCondition, whereCondition;
                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                    taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

                    joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " O.isNeverFoundObservation = 1 " + " OR " +
                                     " O.isNotRediscoveredObservation = 1 "+ " OR " +
                                     " O.isPositiveObservation = 1 ";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);

                }

     

                 [TestMethod]
                 [TestCategory("NightlyTest")] 
                 public void GetSpeciesObservationCountBySearchCriteriaChangeDateTest()
                 {

                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String joinCondition, whereCondition;
                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                    taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
                    DateTime begin = new DateTime(2003,01,01);
                    DateTime end = new DateTime(2012,01,03);

                    joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " (" + "O.[modified]" + " >= '" + begin + "') AND " +
                                     " (" + "O.[modified]" + " <= '" + end + "') ";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);

                 }

                 [TestMethod]
                 [TestCategory("NightlyTest")]
                  public void GetSpeciesObservationCountBySearchCriteriaObservationDateTest()
                 {

                     Int64 speciesObservationCount;
                     List<Int32> regionIds, taxonIds;
                     String joinCondition, whereCondition;
                     regionIds = new List<Int32>();
                     taxonIds = new List<Int32>();
                     taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
                     DateTime begin = new DateTime(2003, 01, 01);
                     DateTime end = new DateTime(2012, 01, 03);

                     joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

                     whereCondition = "O.protectionLevel = 1" + " AND " +
                                      " (" + "O.[start]" + " >= '" + begin + "') AND " +
                                      " (" + "O.[end]" + " <= '" + end + "') ";
                     speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);
                     Assert.IsTrue(0 < speciesObservationCount);

                 }

                 [TestMethod]
                 [TestCategory("NightlyTest")] 
                 public void GetSpeciesObservationCountBySearchCriteriaRegistrationDateTest()
                 {

                     Int64 speciesObservationCount;
                     List<Int32> regionIds, taxonIds;
                     String joinCondition, whereCondition;
                     regionIds = new List<Int32>();
                     taxonIds = new List<Int32>();
                     taxonIds.Add((Int32)(TaxonId.Carnivore));
                     DateTime begin = new DateTime(2003, 01, 01);
                     DateTime end = new DateTime(2012, 01, 03);

                     joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";

                     whereCondition = "O.protectionLevel = 1" + " AND " +
                                      " (" + "O.[reportedDate]" + " >= '" + begin + "') AND " +
                                      " (" + "O.[reportedDate]" + " <= '" + end + "') ";
                     speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);
                     Assert.IsTrue(0 < speciesObservationCount);

                 }
     
         
                [TestMethod]
                [TestCategory("NightlyTest")] 
                public void GetSpeciesObservationCountBySearchCriteriaLocalityTest()
                {

                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String whereCondition;
                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                    int accuracy = 5000;
            
                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                 " (" + "O.[locality]" + " LIKE '" + "Älvhyttan" + "')" + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")"; ;
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);

                 }
        

                [TestMethod]
                [TestCategory("NightlyTest")] 
                public void GetSpeciesObservationCountBySearchCriteriaObserverSearchStringTest()
                {

                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String  whereCondition;
                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                    int accuracy = 100;

                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                    " (" + "O.[recordedBy]" + " = '" + "" + "')" + " AND " +
                             "(O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ")";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);

                 }

     
                [TestMethod]
                [TestCategory("NightlyTest")]
                public void GetSpeciesObservationCountBySearchCriteriaIsNaturalOccuranceTest()
                {
                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String whereCondition;
                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
           
                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " (O.isNaturalOccurrence = 1) ";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);
                }

                [TestMethod]
                [TestCategory("NightlyTest")]
                public void GetSpeciesObservationCountBySearchCriteriaIsNotNaturalOccuranceTest()
                {
                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String whereCondition;
                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();

                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " (O.isNaturalOccurrence = 0) ";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);
                }

                [TestMethod]
                [TestCategory("NightlyTest")]
                public void GetSpeciesObservationCountBySearchCriteriaPolygonTest()
                {

                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String whereCondition;
                    List<SqlGeometry> polygons;

                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                    polygons = new List<SqlGeometry>();

                   //Uppsala, Tandådalen, Örebro, Visby
                    StringBuilder  polygonData = new StringBuilder("POLYGON ((17 59, 12 61, 15 59, 18 57, 17 59))");
                    SqlGeometry polygon = SqlGeometry.Parse(new SqlString(polygonData.ToString()));
                    polygons.Add(polygon);

                    whereCondition = "O.protectionLevel = 1" + " AND " +
                                     " O.isPositiveObservation = 1 ";
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(polygons, regionIds, taxonIds, null, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);
                }

      

                 [TestMethod]
                 [Ignore]
                 [TestCategory("NightlyTest")]
                 [TestCategory("NightlyTest")] 
                 public void GetSpeciesObservationCountBySearchCriteriaRegionsTest()
                 {
                    Int64 speciesObservationCount;
                    List<Int32> regionIds, taxonIds;
                    String joinCondition, whereCondition;
                    regionIds = new List<Int32>();
                    taxonIds = new List<Int32>();
                 
                    regionIds.Add(13);  //Uppland
                    whereCondition = "O.protectionLevel = 1";
                    joinCondition = " INNER JOIN #Regions AS R ON O.Point_GoogleMercator.STIntersects(R.Polygon) = 1 ";
                    // joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId  INNER JOIN #Regions AS Regions ON O.Point_GoogleMercator.STIntersects(Regions.Polygon) = 1 ";
                    // taxonIds.Add(3000176);
                    speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);
                    Assert.IsTrue(0 < speciesObservationCount);
                 }

                 [TestMethod]
                 [TestCategory("NightlyTest")]
                 public void GetSpeciesObservationCountBySearchCriteriaRedlistedTaxaTest()
                 {
                     List<Int32> regionIds, taxonIds;
                     String joinCondition, whereCondition;

                     joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
                     regionIds = new List<Int32>();
                     taxonIds = new List<Int32>();
                     taxonIds.Add(101509); // Apollofjäril, Redlisted NE-category
                     whereCondition = "O.protectionLevel = 1";
                     long result = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);

                     Assert.IsTrue(result > 0);
                 }

                 [TestMethod]
                 [TestCategory("NightlyTest")]
                 public void GetSpeciesObservationCountBySearchCriteriaTaxaTest()
                 {
                     Int64 speciesObservationCount;
                     List<Int32> regionIds, taxonIds;
                     String joinCondition, whereCondition;

                     joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
                     regionIds = new List<Int32>();
                     taxonIds = new List<Int32>();
                     taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
                     whereCondition = "O.protectionLevel = 1";
                     speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);
                     Assert.IsTrue(0 < speciesObservationCount);
                 }


                 [TestMethod]
                 [TestCategory("NightlyTest")] 
                 public void GetSpeciesObservationCountBySearchCriteriaUsingAllCriteriasTest()
                 {
                     Int64 speciesObservationCount;
                     List<Int32> regionIds, taxonIds;
                     String joinCondition, whereCondition;
                     List<SqlGeography> polygons;

                     DateTime begin = new DateTime(2003, 01, 01);
                     DateTime end = new DateTime(2012, 01, 03);
                     string dataProviderId = "1";
                     Int32 accuracy = 3;

                     taxonIds = new List<Int32>();
                     taxonIds.Add((Int32)(TaxonId.Carnivore));
                     regionIds = new List<Int32>();
                     regionIds.Add(13);  //Uppland
                     polygons = new List<SqlGeography>();

                     // Google Mercator.
                     double? boundingBoxMaxX = 2100000;
                     double? boundingBoxMinX = 1900000;
                     double? boundingBoxMaxY = 8500000;
                     double? boundingBoxMinY = 8300000;

                     //Uppsala, Tandådalen, Örebro, Visby
                     StringBuilder polygonData = new StringBuilder("POLYGON ((17 59, 12 61, 15 59, 18 57, 17 59))");
                     SqlGeography polygon = SqlGeography.Parse(new SqlString(polygonData.ToString()));
                     polygons.Add(polygon);
                     whereCondition = "O.protectionLevel = 1" + " AND " +
                                      " (O.isNaturalOccurrence = 1) " + " AND " +
                                      " (" + "O.[start]" + " >= '" + begin + "') AND " +
                                      " (" + "O.[end]" + " <= '" + end + "') " + " AND " +
                                      " O.isNeverFoundObservation = 1 " + " OR " +
                                      " O.isNotRediscoveredObservation = 1 " + " OR " +
                                      " O.isPositiveObservation = 1 " + " AND " +
                                      " (O.dataProviderId = " + dataProviderId + " ) " + " AND " +
                                      " (O.coordinateUncertaintyInMeters <= " + (Int32)(accuracy) + ") " + " AND " +
                                      " (O.coordinateX <= " + (Int32)(boundingBoxMaxX) + " AND " +
                                      " O.coordinateX >= " + (Int32)(boundingBoxMinX) + " AND " +
                                      " O.coordinateY <= " + (Int32)(boundingBoxMaxY) + " AND " +
                                      " O.coordinateY >= " + (Int32)(boundingBoxMinY) + ") " + " AND " +
                                      " (" + "O.[modified]" + " >= '" + begin + "') AND " +
                                      " (" + "O.[modified]" + " <= '" + end + "') " + " AND " +
                                      " (" + "O.[reportedDate]" + " >= '" + begin + "') AND " +
                                      " (" + "O.[reportedDate]" + " <= '" + end + "') ";
                     joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
                     // TODO add jouin for both Region and taxonId" INNER JOIN #Regions AS Regions ON Observation.Point_GoogleMercator.STIntersects(Regions.Polygon) = 1 ";

                     speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);
                     Assert.IsTrue(0 < speciesObservationCount);
                 }
        #endregion

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesObservationsAccessRights()
        {
           // StubAnalysisServer stubserver = new StubAnalysisServer();
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            whereCondition = "O.protectionLevel = 1";
            using (DataReader dataReader = GetDatabase(true).GetSpeciesObservationsAccessRights(null, regionIds, taxonIds, joinCondition, whereCondition, null))
            {
                Assert.IsNotNull(dataReader.Read());
                Assert.IsTrue(dataReader.Read());
            }
        }


        #region Tests to be used later when authority is implemented..

         //[TestMethod]
         //[TestCategory("NightlyTest")]
         //public void GetGridCellSpeciesObservationCountsPublicAutorityTest()
         //{
         //    AnalysisServiceSearchCriteria serviceSearchCriteria = new AnalysisServiceSearchCriteria();
         //    int accuracy = 3;
         //    serviceSearchCriteria.Accuracy = accuracy;
         //    serviceSearchCriteria.MaxProtectionLevel = 1;
         //    serviceSearchCriteria.MinProtectionLevel = 1;
         //    using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(serviceSearchCriteria, null, null))
         //    {
         //        Assert.IsTrue(dataReader.Read());
         //    }

         //}

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTest")]
        public void GetGridCellSpeciesObservationCountsHigherProtectionLevelTest()
        {
            Int64 speciesObservationCount;
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));

            whereCondition = "O.protectionLevel = 1"+ " AND " +
                             " (O.protectionLevel >= " + 2 + ") ";
            speciesObservationCount = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null);
            Assert.IsTrue(0 < speciesObservationCount);

        }

         //[TestMethod]
         //[TestCategory("NightlyTest")]
         //public void GetGridCellSpeciesObservationCountsTaxaAutorityTest()
         //{
         //    AnalysisServiceSearchCriteria serviceSearchCriteria = new AnalysisServiceSearchCriteria();
         //    serviceSearchCriteria.MaxProtectionLevel = 1;
         //    serviceSearchCriteria.MinProtectionLevel = 1;

         //    List<int> taxa = new List<int>();
         //    taxa.Add(Convert.ToInt32(TaxonId.GreenhouseMoths));
         //    taxa.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
        
         //    List<int> authorityTaxa = new List<int>();
         //    taxa.Add(Convert.ToInt32(TaxonId.DrumGrasshopper));
             
         //    serviceSearchCriteria.TaxonIdTable = new Collection<Int32>(taxa);
         //    // TODO make this autority work for taxa
         //   // serviceSearchCriteria.AuthorityTaxonIdTable = new Collection<int>(authorityTaxa);
         //    using (DataReader dataReader = GetDatabase(true).GetGridCellSpeciesObservationCounts(serviceSearchCriteria, null, null))
         //    {
         //        Assert.IsTrue(dataReader.Read());
         //    }

         //}

         //[TestMethod]
         //[TestCategory("NightlyTest")]
         //public void GetSpeciesObservationCountBySearchCriteriaAuthorityRegionsTest()
         //{
         //    AnalysisServiceSearchCriteria serviceSearchCriteria = new AnalysisServiceSearchCriteria();
         //    // Test bounding box
         //    List<int> regionIdTable = new List<int>();
         //    regionIdTable.Add(13); //Uppland

         //     List<int> authorityRegionIdTable = new List<int>();
         //    authorityRegionIdTable.Add(13); //Uppland
         //    authorityRegionIdTable.Add(14);

         //    serviceSearchCriteria.RegionIdTable = new Collection<Int32>(regionIdTable);
         //    serviceSearchCriteria.AuthorityRegionIdTable = new Collection<int>(authorityRegionIdTable);

         //    using (DataReader dataReader = GetDatabase(true).GetSpeciesObservationCountBySearchCriteria(serviceSearchCriteria))
         //    {
         //        Assert.IsTrue(dataReader.Read());
         //    }
        //}
#endregion


        #region GetTaxaBySearchCriteria
        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxonIdsBySearchCriteriaTest()
        {
            List<Int64> speciesObservationIds;
            String whereCondition;

            speciesObservationIds = new List<Int64>();
            whereCondition = "O.protectionLevel = 1";
            using (DataReader dataReader = GetDatabase(true).GetTaxonIdsBySearchCriteria(null, null, null, null, whereCondition, speciesObservationIds))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxonIdsWithSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<Int64> speciesObservationIds;
            String whereCondition;

            speciesObservationIds = new List<Int64>();
            whereCondition = "O.protectionLevel = 1";
            using (DataReader dataReader = GetDatabase(true).GetTaxonIdsWithSpeciesObservationCountsBySearchCriteria(null, null, null, null, whereCondition, speciesObservationIds))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }
        #endregion

        #region GetTimeSpeciesObservationCountsBySearchCriteria
        [TestMethod]
        [TestCategory("NightlyTest")]
       public void GetTimeSpeciesObservationCountsBySearchCriteriaTest()
        {
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;
            String whereCondition;
            String timeStepType = Periodicity.Yearly.ToString();

            regionIds = new List<Int32>();
            speciesObservationIds = new List<Int64>();
            taxonIds = new List<Int32>();
            whereCondition = "O.protectionLevel = 1";
            using (DataReader dataReader = GetDatabase(true).GetTimeSpeciesObservationCountsBySearchCriteria(null, regionIds, taxonIds, null, whereCondition, timeStepType, speciesObservationIds))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }
        #endregion

        [TestMethod]
        [TestCategory("NightlyTest")]
       public void Ping()
        {
            using (WebServiceDataServer database = new AnalysisServer())
            {
                Assert.IsTrue(database.Ping());
            }
        }

        #region GetProvenanceBySearchCriteria

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenanceDarwinCoreObservationsBySearchCriteria_DataProviderId6_ExpectsResult()
        {
            // Arrange
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;
            String joinCondition, whereCondition;

            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            whereCondition = "O.protectionLevel = 1 AND O.dataProviderId = 6 "; // data provider: NORS
            joinCondition = String.Empty;
            speciesObservationIds = new List<Int64>();

            // Act
            using (DataReader dataReader = GetDatabase(true).GetProvenanceDarwinCoreObservationsBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, speciesObservationIds))
            {
                // Assert
                Assert.IsNotNull(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetProvenanceDataProvidersBySearchCriteria_DataProviderId6AndSwedishLocale_ExpectsResultAndInSwedish()
        {
            // Arrange
            List<Int32> regionIds, taxonIds;
            List<Int64> speciesObservationIds;
            String whereCondition;
            String joinCondition;
            Int32 localeId;

            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            localeId = 175; // Swedish
            whereCondition = "O.protectionLevel = 1 AND O.[dataProviderId] = 6 "; // data provider: NORS
            joinCondition = String.Empty;
            speciesObservationIds = new List<Int64>();

            // Act
            using (DataReader dataReader = GetDatabase(true).GetProvenanceDataProvidersBySearchCriteria(null, regionIds, taxonIds, whereCondition, joinCondition, localeId, speciesObservationIds))
            {
                // Assert
                Assert.IsNotNull(dataReader.Read());
            }
        }

        #endregion
    }
}
