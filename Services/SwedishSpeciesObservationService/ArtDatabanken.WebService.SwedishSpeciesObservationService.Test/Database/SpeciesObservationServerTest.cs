using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Test.Database
{
    [TestClass]
    public class SpeciesObservationServerTest : TestBase
    {
        private SpeciesObservationServer _database;

        [TestMethod]
        public void Constructor()
        {
            using (SpeciesObservationServer database = new SpeciesObservationServer())
            {
                Assert.IsNotNull(database);
            }
        }

        [TestMethod]
        public void GetAddress()
        {
            String address;

            address = SpeciesObservationServer.GetAddress();
            Assert.IsTrue(address.IsNotEmpty());
        }

        [TestMethod]
        public void GetCountyRegions()
        {
            using (DataReader dataReader = GetDatabase(true).GetCountyRegions())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetDarwinCoreByIds()
        {
            List<Int64> ids = new List<Int64>();
            ids.Add(5747105);
            ids.Add(5747106);
            ids.Add(5747107);
            ids.Add(5747108);
            ids.Add(5747109);
            ids.Add(5747110);

            using (DataReader dataReader = GetDatabase(true).GetDarwinCoreByIds(ids, 1))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteria()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition;

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            whereCondition = "O.protectionLevel = 1";
            using (DataReader dataReader = GetDatabase(true).GetDarwinCoreBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null))
            {
                Assert.IsNotNull(dataReader.Read());
                while (dataReader.Read())
                {
                    dataReader.GetInt64(0);
                }
            }
        }

        [TestMethod]
        public void GetDarwinCoreBySearchCriteriaSort()
        {
            List<Int32> regionIds, taxonIds;
            String joinCondition, whereCondition, sortOrder;

            joinCondition = " INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
            regionIds = new List<Int32>();
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            whereCondition = "O.protectionLevel = 1";
            sortOrder = " O.[End] desc ";
            using (DataReader dataReader = GetDatabase(true).GetDarwinCoreBySearchCriteria(null, regionIds, taxonIds, joinCondition, whereCondition, null, sortOrder))
            {
                Assert.IsNotNull(dataReader.Read());
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetDarwinCoreFieldDescriptions()
        {
            //Test with the locale id representing Swedish
            using (DataReader dataReader = GetDatabase(true).GetSpeciesObservationFieldDescriptions(175))
            {

                Assert.IsTrue(dataReader.Read());
            }
            //Test with the locale set to Swedish
            using (DataReader dataReader = GetDatabase(true).GetSpeciesObservationFieldDescriptions(1))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetDarwinCoreFieldMappings()
        {
            using (DataReader dataReader = GetDatabase(true).GetSpeciesObservationFieldMappings())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        private SpeciesObservationServer GetDatabase(Boolean refresh = false)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.Dispose();
                }

                _database = new SpeciesObservationServer();
            }

            return _database;
        }

        [TestMethod]
        public void GetProvinceRegions()
        {
            using (DataReader dataReader = GetDatabase(true).GetProvinceRegions())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesActivities()
        {
            using (DataReader dataReader = GetDatabase(true).GetSpeciesActivities((Int32)LocaleId.en_GB))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase().GetSpeciesActivities((Int32)LocaleId.sv_SE))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetSpeciesActivityCategories()
        {
            using (DataReader dataReader = GetDatabase(true).GetSpeciesActivityCategories((Int32)LocaleId.en_GB))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase().GetSpeciesActivityCategories((Int32)LocaleId.sv_SE))
            {
                Assert.IsTrue(dataReader.Read());
            }
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
        public void GetSpeciesObservationDataProviders()
        {
            using (DataReader dataReader = GetDatabase(true).GetSpeciesObservationDataProviders(175))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationProjectParametersByIds()
        {
            List<Int64> speciesObservationIds;

            speciesObservationIds = new List<Int64>();
            speciesObservationIds.Add(12827433);
            speciesObservationIds.Add(12827443);
            using (DataReader dataReader = GetDatabase(true).GetSpeciesObservationProjectParametersByIds(speciesObservationIds))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

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

        [TestMethod]
        [TestCategory("NightlyTest")]
        public void GetTaxonInformation()
        {
            using (DataReader dataReader = GetDatabase(true).GetTaxonInformation())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void IsDatabaseUpdated()
        {
            Boolean isDatabaseUpdated;

            isDatabaseUpdated = GetDatabase(true).IsDatabaseUpdated();
            Assert.IsFalse(isDatabaseUpdated);
        }

        [TestMethod]
        public void Ping()
        {
            using (WebServiceDataServer database = GetDatabase(true))
            {
                Assert.IsTrue(database.Ping());
            }
        }
    }
}
