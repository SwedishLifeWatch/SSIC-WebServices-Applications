using System;
using System.Collections.Generic;
using System.Data;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonAttributeService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeciesFactManager = ArtDatabanken.WebService.TaxonAttributeService.Data.SpeciesFactManager;

namespace ArtDatabanken.WebService.TaxonAttributeService.Test.Database
{
    [TestClass]
    public class TaxonAttributeServerTest : TestBase
    {
        private TaxonAttributeServer _database;

        public TaxonAttributeServerTest()
        {
            _database = null;
        }

        [TestMethod]
        public void GetAddress()
        {
            String address;

            address = TaxonAttributeServer.GetAddress();
            Assert.IsTrue(address.IsNotEmpty());
        }

        private TaxonAttributeServer GetDatabase(Boolean refresh = false)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.Dispose();
                }

                _database = new TaxonAttributeServer();
                _database.BeginTransaction();
            }

            return _database;
        }

        [TestMethod]
        public void GetFactorOrigins()
        {
            using (DataReader dataReader = GetDatabase(true).GetFactorOrigins())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorUpdateModes()
        {
            using (DataReader dataReader = GetDatabase(true).GetFactorUpdateModes())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetFactorFieldTypes())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetPeriodTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetPeriodTypes())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetPeriods()
        {
            using (DataReader dataReader = GetDatabase(true).GetPeriods())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            using (DataReader dataReader = GetDatabase(true).GetIndividualCategories())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesFactQualities()
        {
            using (DataReader dataReader = GetDatabase(true).GetSpeciesFactQualities())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            using (DataReader dataReader = GetDatabase(true).GetFactorFieldEnums())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorFields()
        {
            using (DataReader dataReader = GetDatabase(true).GetFactorFields())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactors()
        {
            using (DataReader dataReader = GetDatabase(true).GetFactors())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            string factorNameSearchString = string.Empty;

            using (DataReader dataReader = GetDatabase(true).GetFactorsBySearchCriteria(null, factorNameSearchString, "Like", FactorSearchScope.NoScope.ToString(), FactorSearchScope.NoScope.ToString()))
            {
                Assert.IsFalse(dataReader.Read());
            }

            factorNameSearchString = "Rödli%";
            using (DataReader dataReader = GetDatabase().GetFactorsBySearchCriteria(null, factorNameSearchString, "Like", FactorSearchScope.NoScope.ToString(), FactorSearchScope.AllChildFactors.ToString()))
            {
                Assert.IsTrue(dataReader.Read());
            }

            factorNameSearchString = "Rödli'stan";
            using (DataReader dataReader = GetDatabase().GetFactorsBySearchCriteria(null, factorNameSearchString, "Like", FactorSearchScope.NoScope.ToString(), FactorSearchScope.AllChildFactors.ToString()))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorTrees()
        {
            using (DataReader dataReader = GetDatabase(true).GetFactorTrees())
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.NextResultSet());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.NextResultSet());
            }
        }

        [TestMethod]
        public void GetFactorTreesByIdsAndSearchCriteria()
        {
            List<int> factorIds = new List<int> { 1, 2, 3, 4, 5 };

            using (DataReader dataReader = GetDatabase(true).GetFactorTreesBySearchCriteria(factorIds))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.NextResultSet());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.NextResultSet());
            }

            using (DataReader dataReader = GetDatabase().GetFactorTreesBySearchCriteria(factorIds))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.NextResultSet());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.NextResultSet());
            }
        }

        [TestMethod]
        public void GetSpeciesFactsByIds()
        {
            List<int> speciesFactIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            using (DataReader dataReader = GetDatabase(true).GetSpeciesFactsByIds(speciesFactIds))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesFactsBySpeciesFactIdentifiers()
        {
            DataTable speciesFactIdentifiers = SpeciesFactManager.GetSpeciesFactIdentifiersTable();

            speciesFactIdentifiers.Rows.Add(1, 14, 0);
            speciesFactIdentifiers.Rows.Add(2, 25, 0);
            speciesFactIdentifiers.Rows.Add(3, 60, 0);

            using (DataReader dataReader = GetDatabase(true).GetSpeciesFactsByIdentifiers(speciesFactIdentifiers))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesFactsBySearchCriteria()
        {
            List<Int32> factorIds, hostIds, taxonIds;
            String query;

            query = @"SELECT * FROM af_data " +
                    @"INNER JOIN #FactorIds AS InputFactors ON InputFactors.FactorId = af_data.faktor";
            factorIds = new List<Int32>();
            factorIds.Add((Int32)(FactorId.RedlistCategory));
            using (DataReader dataReader = GetDatabase(true).GetSpeciesFactsBySearchCriteria(query, null, factorIds, null, null))
            {
                Assert.IsTrue(dataReader.Read());
            }

            query = @"SELECT * FROM af_data " +
                    @"INNER JOIN #HostIds AS InputHosts ON InputHosts.HostId = af_data.host";
            hostIds = new List<Int32>();
            hostIds.Add(102656); // Hedsidenbi.
            using (DataReader dataReader = GetDatabase().GetSpeciesFactsBySearchCriteria(query, null, null, hostIds, null))
            {
                Assert.IsTrue(dataReader.Read());
            }

            query = @"SELECT * FROM af_data " +
                    @"INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = af_data.taxon";
            taxonIds = new List<Int32>();
            taxonIds.Add((Int32)(TaxonId.DrumGrasshopper));
            using (DataReader dataReader = GetDatabase().GetSpeciesFactsBySearchCriteria(query, null, null, null, taxonIds))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesFactsBySearchCriteriaAndFieldSearchCriteria()
        {
            List<Int32> factorIds;
            String query;

            query = @"SELECT * FROM af_data " +
                    @"INNER JOIN #FactorIds AS InputFactors ON InputFactors.FactorId = af_data.faktor ";
            query += @"AND af_data.tal1 IN (0,1,2)";
               
            factorIds = new List<Int32>();
            factorIds.Add(1001);
            using (DataReader dataReader = GetDatabase(true).GetSpeciesFactsBySearchCriteria(query, null, factorIds, null, null))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetMaxSpeciesFactId()
        {
            Assert.IsTrue(GetDatabase(true).GetMaxSpeciesFactId() > 0);
        }

        [TestMethod]
        public void CreateSpeciesFacts()
        {
            List<int> speciesFactIds = new List<int> { 1 };
            WebSpeciesFact speciesFact = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds)[0], newSpeciesFact;
            DataTable speciesFactsTable;
            int newSpeciesFactId;

            speciesFact.FactorId = 10;
            speciesFact.FieldValue1 = 10;
            speciesFact.IsFieldValue1Specified = true;
            speciesFact.IndividualCategoryId = 2;
            speciesFact.TaxonId = 209210;
            speciesFactsTable = SpeciesFactManager.GetSpeciesFactCreateTable(GetContext(),
                                                                             new List<WebSpeciesFact> { speciesFact },
                                                                             DateTime.Now,
                                                                             string.Empty);
            newSpeciesFactId = (int)speciesFactsTable.Rows[0]["idnr"];
            GetDatabase(true).CreateSpeciesFacts(speciesFactsTable);
            newSpeciesFact = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), new List<int> { newSpeciesFactId })[0];
            Assert.AreEqual(newSpeciesFact.Id, newSpeciesFactId);
        }

        [TestMethod]
        public void UpdateSpeciesFacts()
        {
            List<int> speciesFactIds = new List<int> { 1 };
            WebSpeciesFact speciesFact = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds)[0], updatedSpeciesFact;
            DataTable speciesFactTable = SpeciesFactManager.GetSpeciesFactUpdateTable(GetContext(),
                                                                                      new List<WebSpeciesFact> { speciesFact });
            GetDatabase(true).UpdateSpeciesFacts(speciesFactTable);
            updatedSpeciesFact = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds)[0];
            Assert.AreEqual(updatedSpeciesFact.ModifiedBy, "TestFirstName TestLastName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteSpeciesFacts()
        {
            List<int> speciesFactIds = new List<int>
                                           {
                                               GetDatabase(true).GetMaxSpeciesFactId(),
                                               GetDatabase().GetMaxSpeciesFactId() - 1
                                           };
            GetDatabase().DeleteSpeciesFactsByIds(speciesFactIds);

            List<WebSpeciesFact> deletedSpeciesFact = SpeciesFactManager.GetSpeciesFactsByIds(GetContext(), speciesFactIds);
            
            Assert.IsTrue(deletedSpeciesFact.IsEmpty());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public new void TestCleanup()
        {
            if (_database.IsNotNull())
            {
                _database.RollbackTransaction();
                _database.Dispose();
                _database = null;
            }
        }
    }
}
