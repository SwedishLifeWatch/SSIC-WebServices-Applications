using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.ArtDatabankenService.Test.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Database
{
    [TestClass]
    public class DataServerTest : TestBase
    {
        public DataServerTest()
            : base(true, 60)
        {
        }

        [TestMethod]
        public void AddUserSelectedFactors()
        {
            DataServer.AddUserSelectedFactors(GetContext(), FactorManager.GetUserSelectedFactorsTable(GetContext(), FactorManagerTest.GetSomeFactorIds(), UserSelectedFactorUsage.Input));
            DataServer.DeleteUserSelectedFactors(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedHosts()
        {
            DataServer.AddUserSelectedHosts(GetContext(), SpeciesFactManagerTest.GetUserSelectedHosts(GetContext()));
            DataServer.DeleteUserSelectedHosts(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedIndividualCategories()
        {
            DataServer.AddUserSelectedIndividualCategories(GetContext(), IndividualCategoryManagerTest.GetUserSelectedIndividualCategories(GetContext()));
            DataServer.DeleteUserSelectedIndividualCategories(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedParameters()
        {
            DataServer.AddUserSelectedParameters(GetContext(), SpeciesFactManagerTest.GetUserSelectedParameterTable(GetContext()));
            DataServer.DeleteUserSelectedParameters(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedPeriods()
        {
            DataServer.AddUserSelectedPeriods(GetContext(), PeriodManagerTest.GetUserSelectedPeriods(GetContext()));
            DataServer.DeleteUserSelectedPeriods(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedReferences()
        {
            DataServer.AddUserSelectedReferences(GetContext(), ReferenceManagerTest.GetUserSelectedReferences(GetContext()));
            DataServer.DeleteUserSelectedReferences(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedSpeciesFacts()
        {
            DataServer.AddUserSelectedSpeciesFacts(GetContext(), SpeciesFactManagerTest.GetUserSelectedSpeciesFacts(GetContext()));
            DataServer.DeleteUserSelectedSpeciesFacts(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedSpeciesObservations()
        {
            DataServer.AddUserSelectedSpeciesObservations(GetContext(), SpeciesObservationManagerTest.GetUserSelectedSpeciesObservationTable(GetContext()));
            DataServer.DeleteUserSelectedSpeciesObservations(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedTaxa()
        {
            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext()));
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void AddUserSelectedTaxonTypes()
        {
            DataServer.AddUserSelectedTaxonTypes(GetContext(), TaxonManagerTest.GetUserSelectedTaxonTypes(GetContext()));
            DataServer.DeleteUserSelectedTaxonTypes(GetContext());
        }

        [TestMethod]
        public void BeginTransaction()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    database.BeginTransaction();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void BeginTransactionAlreadyStartedError()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    database.BeginTransaction();
                    database.BeginTransaction();
                }
            }
        }

        [TestMethod]
        public void CommandTimeout()
        {
            Int32 commandTimeout;

            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    commandTimeout = -43234;
                    database.CommandTimeout = commandTimeout;
                    Assert.AreEqual(commandTimeout, database.CommandTimeout);
                    commandTimeout = 0;
                    database.CommandTimeout = commandTimeout;
                    Assert.AreEqual(commandTimeout, database.CommandTimeout);
                    commandTimeout = 5345345;
                    database.CommandTimeout = commandTimeout;
                    Assert.AreEqual(commandTimeout, database.CommandTimeout);
                }
            }
        }

        [TestMethod]
        public void CommitTransaction()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    database.BeginTransaction();
                    database.CommitTransaction();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void CommitTransactionNoTransactionError()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    database.CommitTransaction();
                }
            }
        }

        [TestMethod]
        public void Constructor()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    Assert.IsNotNull(database);
                }
            }
        }

        [TestMethod]
        public void CreateReference()
        {
            DataServer.CreateReference((GetContext()), "AutoInsertTestName", 1966, "AutoInsertTestText", "AutoTestPerson");
        }

        [TestMethod]
        public void CreateSpeciesFacts()
        {
            DataServer.CreateSpeciesFacts(GetContext(), SpeciesFactManagerTest.GetNewSpeciesFactTable(GetContext()));
        }

        [TestMethod]
        public void CreateTaxonTables()
        {
            DataServer.CreateTaxonTables(GetContext());
        }

        [TestMethod]
        public void DeleteSpeciesFacts()
        {
            DataServer.DeleteSpeciesFacts(GetContext());
        }

        [TestMethod]
        public void DeleteTrace()
        {
            DataServer.DeleteTrace(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedFactors()
        {
            DataServer.AddUserSelectedFactors(GetContext(), FactorManager.GetUserSelectedFactorsTable(GetContext(), FactorManagerTest.GetSomeFactorIds(), UserSelectedFactorUsage.Input));
            DataServer.DeleteUserSelectedFactors(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedHosts()
        {
            DataServer.AddUserSelectedHosts(GetContext(), SpeciesFactManagerTest.GetUserSelectedHosts(GetContext()));
            DataServer.DeleteUserSelectedHosts(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedIndividualCategories()
        {
            DataServer.AddUserSelectedIndividualCategories(GetContext(), IndividualCategoryManagerTest.GetUserSelectedIndividualCategories(GetContext()));
            DataServer.DeleteUserSelectedIndividualCategories(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedParameters()
        {
            DataServer.AddUserSelectedParameters(GetContext(), SpeciesFactManagerTest.GetUserSelectedParameterTable(GetContext()));
            DataServer.DeleteUserSelectedParameters(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedPeriods()
        {
            DataServer.AddUserSelectedPeriods(GetContext(), PeriodManagerTest.GetUserSelectedPeriods(GetContext()));
            DataServer.DeleteUserSelectedPeriods(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedReferences()
        {
            DataServer.AddUserSelectedReferences(GetContext(), ReferenceManagerTest.GetUserSelectedReferences(GetContext()));
            DataServer.DeleteUserSelectedReferences(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedSpeciesFacts()
        {
            DataServer.AddUserSelectedSpeciesFacts(GetContext(), SpeciesFactManagerTest.GetUserSelectedSpeciesFacts(GetContext()));
            DataServer.DeleteUserSelectedSpeciesFacts(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedSpeciesObservations()
        {
            DataServer.AddUserSelectedSpeciesObservations(GetContext(), SpeciesObservationManagerTest.GetUserSelectedSpeciesObservationTable(GetContext()));
            DataServer.DeleteUserSelectedSpeciesObservations(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedTaxa()
        {
            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext()));
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void DeleteUserSelectedTaxonTypes()
        {
            DataServer.AddUserSelectedTaxonTypes(GetContext(), TaxonManagerTest.GetUserSelectedTaxonTypes(GetContext()));
            DataServer.DeleteUserSelectedTaxonTypes(GetContext());
        }

        [TestMethod]
        public void Disconnect()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    database.Disconnect();

                    // Should be ok to disconnect an already disconnected database.
                    database.Disconnect();
                }
            }
        }

        [TestMethod]
        public void Dispose()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    database.Dispose();

                    // Should be ok to dispose an already disposed database.
                    database.Dispose();
                }
            }
        }

        [TestMethod]
        public void GetBirdNestActivities()
        {
            using (DataReader dataReader = DataServer.GetBirdNestActivities(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetCitiesBySearchString()
        {
            String citySearchString;

            citySearchString = "Uppsala";
            using (DataReader dataReader = DataServer.GetCitiesBySearchString(GetContext(), citySearchString))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetColumnLength()
        {
            Int32 columnLength;
            String columnName, tableName;

            tableName = "County";
            columnName = "CountyName";
            columnLength = DataServer.GetColumnLength(GetContext(), DataServer.DatabaseId.SpeciesFact, tableName, columnName);
            Assert.IsTrue(0 < columnLength);

            tableName = "Users";
            columnName = "Username";
            columnLength = DataServer.GetColumnLength(GetContext(), DataServer.DatabaseId.User, tableName, columnName);
            Assert.IsTrue(0 < columnLength);
        }

        [TestMethod]
        public void GetCounties()
        {
            using (DataReader dataReader = DataServer.GetCounties(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetDatabaseId()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    Assert.AreEqual(databaseId, database.GetDatabaseId());
                }
            }
        }

        [TestMethod]
        public void GetDatabases()
        {
            using (DataReader dataReader = DataServer.GetDatabases(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetDatabaseUpdate()
        {
            using (DataReader dataReader = DataServer.GetDatabaseUpdate(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetEndangeredLists()
        {
            using (DataReader dataReader = DataServer.GetEndangeredLists(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorFieldEnums()
        {
            using (DataReader dataReader = DataServer.GetFactorFieldEnums(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorFields()
        {
            using (DataReader dataReader = DataServer.GetFactorFields(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorFieldTypes()
        {
            using (DataReader dataReader = DataServer.GetFactorFieldTypes(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorOrigins()
        {
            using (DataReader dataReader = DataServer.GetFactorOrigins(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactors()
        {
            using (DataReader dataReader = DataServer.GetFactors(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorsBySearchCriteria()
        {
            String factorNameSearchString;

            factorNameSearchString = null;
            DataServer.DeleteUserSelectedFactors(GetContext());
            using (DataReader dataReader = DataServer.GetFactorsBySearchCriteria(
                GetContext(),
                factorNameSearchString,
                SearchStringComparisonMethod.Like.ToString(),
                false,
                FactorSearchScope.NoScope.ToString(),
                FactorSearchScope.NoScope.ToString()))
            {
                Assert.IsFalse(dataReader.Read());
            }

            factorNameSearchString = "Rödli%";
            DataServer.DeleteUserSelectedFactors(GetContext());
            using (DataReader dataReader = DataServer.GetFactorsBySearchCriteria(
                GetContext(),
                factorNameSearchString,
                SearchStringComparisonMethod.Like.ToString(),
                false,
                FactorSearchScope.NoScope.ToString(),
                FactorSearchScope.AllChildFactors.ToString()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetFactorTrees()
        {
            using (DataReader dataReader = DataServer.GetFactorTrees(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.NextResultSet());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.NextResultSet());
            }
        }

        [TestMethod]
        public void GetFactorTreesBySearchCriteria()
        {
            DataServer.AddUserSelectedFactors(GetContext(), FactorManager.GetUserSelectedFactorsTable(GetContext(), FactorManagerTest.GetSomeFactorIds(), UserSelectedFactorUsage.Input));
            using (DataReader dataReader = DataServer.GetFactorTreesBySearchCriteria(GetContext(), true))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.NextResultSet());
                Assert.IsTrue(dataReader.Read());
                Assert.IsFalse(dataReader.NextResultSet());
            }
            DataServer.DeleteUserSelectedFactors(GetContext());
        }

        [TestMethod]
        public void GetFactorUpdateModes()
        {
            using (DataReader dataReader = DataServer.GetFactorUpdateModes(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetHostTaxa()
        {
            Int32 factorId;
            String taxonInformationType;

            factorId = Int32.MinValue;
            taxonInformationType = TaxonInformationType.Basic.ToString();
            using (DataReader dataReader = DataServer.GetHostTaxa(GetContext(), factorId, taxonInformationType))
            {
                Assert.IsFalse(dataReader.Read());
            }

            factorId = SPECIES_FACT_DATABASE_FACTOR_ID;
            taxonInformationType = TaxonInformationType.Basic.ToString();
            using (DataReader dataReader = DataServer.GetHostTaxa(GetContext(), factorId, taxonInformationType))
            {
                Assert.IsTrue(dataReader.Read());
            }

            factorId = SPECIES_FACT_DATABASE_FACTOR_ID;
            taxonInformationType = TaxonInformationType.PrintObs.ToString();
            using (DataReader dataReader = DataServer.GetHostTaxa(GetContext(), factorId, taxonInformationType))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetHostTaxaByTaxonId()
        {
            Int32 taxonId;
            String taxonInformationType;

            taxonId = Int32.MinValue;
            taxonInformationType = TaxonInformationType.Basic.ToString();
            using (DataReader dataReader = DataServer.GetHostTaxaByTaxonId(GetContext(), taxonId, taxonInformationType))
            {
                Assert.IsFalse(dataReader.Read());
            }

            taxonId = 101246; // Ekoxe
            taxonInformationType = TaxonInformationType.Basic.ToString();
            using (DataReader dataReader = DataServer.GetHostTaxaByTaxonId(GetContext(), taxonId, taxonInformationType))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            using (DataReader dataReader = DataServer.GetPeriods(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetLog()
        {
            using (DataReader dataReader = DataServer.GetLog(GetContext(), null, null, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = DataServer.GetLog(GetContext(), LogType.Error.ToString(), null, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = DataServer.GetLog(GetContext(), null, TEST_USER_NAME, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = DataServer.GetLog(GetContext(), LogType.Error.ToString(), TEST_USER_NAME, 10))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetMaxSpeciesFactId()
        {
            Assert.IsTrue(0 < DataServer.GetMaxSpeciesFactId(GetContext()));
        }

        [TestMethod]
        public void GetPeriods()
        {
            using (DataReader dataReader = DataServer.GetPeriods(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetProvinces()
        {
            using (DataReader dataReader = DataServer.GetProvinces(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetReferences()
        {
            using (DataReader dataReader = DataServer.GetReferences(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetReferencesBySearchString()
        {
            const String searchString = "1995";

            using (DataReader dataReader = DataServer.GetReferencesBySearchString(GetContext(), searchString))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesFactQualities()
        {
            using (DataReader dataReader = DataServer.GetSpeciesFactQualities(GetContext()))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesFactsById()
        {
            using (DataReader dataReader = DataServer.GetSpeciesFactsById(GetContext()))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesFactsByIdentifier()
        {
            using (DataReader dataReader = DataServer.GetSpeciesFactsByIdentifier(GetContext()))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesFactsByUserParametersSelection()
        {
            DataServer.DeleteUserSelectedParameters(GetContext());
            DataServer.AddUserSelectedParameters(GetContext(), SpeciesFactManagerTest.GetUserSelectedParameterTable(GetContext()));

            using (DataReader dataReader = DataServer.GetSpeciesFactsByUserParameterSelection(
                GetContext(),
                true,
                true,
                false,
                false,
                false,
                false))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            DateTime changedFrom, changedTo;
            Int32 maxProtectionLevel;

            changedFrom = new DateTime(2011, 2, 1, 0, 0, 0);
            changedTo = new DateTime(2011, 2, 2, 0, 0, 0);
            maxProtectionLevel = 2;
            using (DataReader dataReader = DataServer.GetSpeciesObservationChange(GetContext(), maxProtectionLevel, changedFrom, changedTo))
            {
                Assert.IsTrue(dataReader.Read());
                while (dataReader.Read())
                {}
                Assert.IsTrue(dataReader.NextResultSet());
                Assert.IsTrue(dataReader.Read());
                while (dataReader.Read())
                { }
                Assert.IsTrue(dataReader.NextResultSet());
                Assert.IsTrue(dataReader.Read());
                while (dataReader.Read())
                { }
            }
        }

        [TestMethod]
        public void GetSpeciesObservationCountBySearchCriteria()
        {
            Int32 speciesObservationCount;

            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext()));
            DataServer.UpdateUserSelecedTaxa(GetContext());
            speciesObservationCount = DataServer.GetSpeciesObservationCountBySearchCriteria(GetContext(),
                                                                            0,
                                                                            5,
                                                                            false,
                                                                            0,
                                                                            false,
                                                                            0,
                                                                            0,
                                                                            false,
                                                                            true,
                                                                            0,
                                                                            false,
                                                                            DateTime.Now,
                                                                            DateTime.Now,
                                                                            0,
                                                                            false,
                                                                            DateTime.Now,
                                                                            DateTime.Now,
                                                                            false,
                                                                            Int32.MinValue,
                                                                            Int32.MinValue,
                                                                            Int32.MinValue,
                                                                            Int32.MinValue,
                                                                            false,
                                                                            Int32.MinValue,
                                                                            null,
                                                                            true,
                                                                            false,
                                                                            false,
                                                                            false,
                                                                            -1,
                                                                            false,
                                                                            -1,
                                                                            false,
                                                                            -1,
                                                                            false,
                                                                            -1,
                                                                            false,
                                                                            -1,
                                                                            false,
                                                                            -1,
                                                                            null,
                                                                            true);
            Assert.IsTrue(speciesObservationCount > 0);
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void GetSpeciesObservations()
        {
            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext()));
            DataServer.UpdateUserSelecedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetSpeciesObservations(GetContext(),
                                                                             0,
                                                                             5,
                                                                             false,
                                                                             0,
                                                                             false,
                                                                             0,
                                                                             0,
                                                                             false,
                                                                             true,
                                                                             0,
                                                                             false,
                                                                             DateTime.Now,
                                                                             DateTime.Now,
                                                                             0,
                                                                             false,
                                                                             DateTime.Now,
                                                                             DateTime.Now,
                                                                             false,
                                                                             Int32.MinValue,
                                                                             Int32.MinValue,
                                                                             Int32.MinValue,
                                                                             Int32.MinValue,
                                                                             false,
                                                                             Int32.MinValue,
                                                                             null,
                                                                             true,
                                                                             false,
                                                                             false,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             false,
                                                                             -1,
                                                                             "All",
                                                                             null,
                                                                             true))
            { 
                Assert.IsTrue(dataReader.Read());
            }
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void GetSpeciesObservationsById()
        {
            DataServer.AddUserSelectedSpeciesObservations(GetContext(), SpeciesObservationManagerTest.GetUserSelectedSpeciesObservationTable(GetContext()));
            using (DataReader dataReader = DataServer.GetSpeciesObservationsById(GetContext(),
                                                                                 0,
                                                                                 5,
                                                                                 false,
                                                                                 0,
                                                                                 false,
                                                                                 0,
                                                                                 0,
                                                                                 false,
                                                                                 "All"))
            {
                Assert.IsTrue(dataReader.Read());
            }
            DataServer.DeleteUserSelectedSpeciesObservations(GetContext());
        }

        /// <summary>
        /// Get DataReader with species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns an open DataReader. Remember to close the
        /// DataReader after reading has been finished.
        /// </returns>
        public static DataReader GetSpeciesObservationsDataReader(WebServiceContext context)
        {
            return DataServer.GetSpeciesObservations(context,
                                                     0,
                                                     2,
                                                     false,
                                                     0,
                                                     false,
                                                     0,
                                                     0,
                                                     false,
                                                     false,
                                                     1,
                                                     true,
                                                     new DateTime(2000, 1, 20), 
                                                     new DateTime(2000, 1, 20),
                                                     0,
                                                     false,
                                                     DateTime.Now,
                                                     DateTime.Now,
                                                     false,
                                                     Int32.MinValue,
                                                     Int32.MinValue,
                                                     Int32.MinValue,
                                                     Int32.MinValue,
                                                     false,
                                                     Int32.MinValue,
                                                     null,
                                                     false,
                                                     false,
                                                     false,
                                                     false,
                                                     -1,
                                                     false,
                                                     -1,
                                                     false,
                                                     -1,
                                                     false,
                                                     -1,
                                                     false,
                                                     -1,
                                                     false,
                                                     -1,
                                                     "All",
                                                     null,
                                                     true);
        }

        [TestMethod]
        public void GetTaxa()
        {
            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext()));
            DataServer.UpdateUserSelecedTaxa(GetContext());
            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                using (DataReader dataReader = DataServer.GetTaxa(GetContext(), taxonInformationType.ToString()))
                {
                    Assert.IsTrue(dataReader.Read());
                }
            }
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void GetTaxaByHostTaxonId()
        {
            Int32 hostTaxonId;
            String taxonInformationType;

            hostTaxonId = Int32.MinValue;
            taxonInformationType = TaxonInformationType.Basic.ToString();
            using (DataReader dataReader = DataServer.GetTaxaByHostTaxonId(GetContext(), hostTaxonId, taxonInformationType))
            {
                Assert.IsFalse(dataReader.Read());
            }

            hostTaxonId = 1006592; //Salix
            taxonInformationType = TaxonInformationType.Basic.ToString();
            using (DataReader dataReader = DataServer.GetTaxaByHostTaxonId(GetContext(), hostTaxonId, taxonInformationType))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxaByOrganismOrRedlist()
        {
            Int32 organismGroupId, endangeredListId, redlistCategoryId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                organismGroupId = 5;
                endangeredListId = 1;
                redlistCategoryId = 0;
                using (DataReader dataReader = DataServer.GetTaxaByOrganismOrRedlist(GetContext(), true, organismGroupId, true, endangeredListId, true, redlistCategoryId, taxonInformationType.ToString()))
                {
                    Assert.IsTrue(dataReader.Read());
                }
            }
        }

        [TestMethod]
        public void GetTaxaByQuery()
        {
            String query;
            String taxonInformationType;

            // Get all spiders by organism group.
            query = "SELECT " +
                        GetContext().RequestId + " AS RequestId," +
			            " Data1.taxon AS TaxonId, " +
			            " ''Output'' AS TaxonUsage" +
		            " from af_data AS Data1" +
		            " where" +
			            " (Data1.faktor = 656 and" + 
			            " Data1.IndividualCategoryId = 0 and" +
			            " Data1.host = 0 AND" +
			            " Data1.tal1 in (18))";
            taxonInformationType = "Basic";
            DataServer.DeleteUserSelectedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetTaxaByQuery(GetContext(), query, taxonInformationType))
            {
                Assert.IsTrue(dataReader.Read());
            }
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void GetTaxaBySearchCriteria()
        {
            String taxonNameSearchString;
            String taxonInformationType;

            taxonNameSearchString = null;
            taxonInformationType = "Basic";
            DataServer.DeleteUserSelectedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetTaxaBySearchCriteria(GetContext(), taxonNameSearchString, taxonInformationType, false, false, false, TaxonSearchScope.AllChildTaxa.ToString(),false , false))
            {
                Assert.IsFalse(dataReader.Read());
            }

            taxonNameSearchString = "björn";
            taxonInformationType = "Basic";
            DataServer.DeleteUserSelectedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetTaxaBySearchCriteria(GetContext(), taxonNameSearchString, taxonInformationType, false, false, false, TaxonSearchScope.NoScope.ToString(), false, false))
            {
                Assert.IsTrue(dataReader.Read());
            }

            taxonNameSearchString = "björn%";
            taxonInformationType = "PrintObs";
            DataServer.DeleteUserSelectedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetTaxaBySearchCriteria(GetContext(), taxonNameSearchString, taxonInformationType, false, false, false, TaxonSearchScope.AllChildTaxa.ToString(), false, false))
            {
                Assert.IsTrue(dataReader.Read());
            }

            taxonNameSearchString = null;
            taxonInformationType = "Basic";
            DataServer.DeleteUserSelectedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetTaxaBySearchCriteria(GetContext(), taxonNameSearchString, taxonInformationType, false, false, true, TaxonSearchScope.NoScope.ToString(), false, false))
            {
                Assert.IsFalse(dataReader.Read());
            }

            taxonNameSearchString = "björn";
            taxonInformationType = "Basic";
            DataServer.DeleteUserSelectedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetTaxaBySearchCriteria(GetContext(), taxonNameSearchString, taxonInformationType, false, false, true, TaxonSearchScope.AllChildTaxa.ToString(), false, false))
            {
                Assert.IsTrue(dataReader.Read());
            }
            
            taxonNameSearchString = "björn%";
            taxonInformationType = "PrintObs";
            DataServer.DeleteUserSelectedTaxa(GetContext());
            using (DataReader dataReader = DataServer.GetTaxaBySearchCriteria(GetContext(), taxonNameSearchString, taxonInformationType, false, false, true, TaxonSearchScope.NoScope.ToString(), false, false))
            {
                Assert.IsTrue(dataReader.Read());
            }

            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void GetTaxaBySpeciesObservations()
        {
            using (DataReader dataReader = DataServer.GetTaxaBySpeciesObservations(GetContext(),
                                                                                   0,
                                                                                   5,
                                                                                   false,
                                                                                   0,
                                                                                   false,
                                                                                   0,
                                                                                   0,
                                                                                   false,
                                                                                   false,
                                                                                   1,
                                                                                   true,
                                                                                   new DateTime(2004, 1, 1),
                                                                                   new DateTime(2004, 2, 1),
                                                                                   0,
                                                                                   false,
                                                                                   DateTime.Now,
                                                                                   DateTime.Now,
                                                                                   false,
                                                                                   Int32.MinValue,
                                                                                   Int32.MinValue,
                                                                                   Int32.MinValue,
                                                                                   Int32.MinValue,
                                                                                   false,
                                                                                   Int32.MinValue,
                                                                                   null,
                                                                                   true,
                                                                                   true,
                                                                                   true,
                                                                                   false,
                                                                                   -1,
                                                                                   false,
                                                                                   -1,
                                                                                   false,
                                                                                   -1,
                                                                                   false,
                                                                                   -1,
                                                                                   false,
                                                                                   -1,
                                                                                   false,
                                                                                   -1,
                                                                                   null,
                                                                                   true))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxaCountBySpeciesObservations()
        {
            Int32 taxaCount;

            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext()));
            DataServer.UpdateUserSelecedTaxa(GetContext());
            taxaCount = DataServer.GetTaxaCountBySpeciesObservations(GetContext(),
                                                                     0,
                                                                     5,
                                                                     false,
                                                                     0,
                                                                     false,
                                                                     0,
                                                                     0,
                                                                     false,
                                                                     true,
                                                                     0,
                                                                     false,
                                                                     DateTime.Now,
                                                                     DateTime.Now,
                                                                     0,
                                                                     false,
                                                                     DateTime.Now,
                                                                     DateTime.Now,
                                                                     false,
                                                                     Int32.MinValue,
                                                                     Int32.MinValue,
                                                                     Int32.MinValue,
                                                                     Int32.MinValue,
                                                                     false,
                                                                     Int32.MinValue,
                                                                     null,
                                                                     true,
                                                                     false,
                                                                     false,
                                                                     false,
                                                                     -1,
                                                                     false,
                                                                     -1,
                                                                     false,
                                                                     -1,
                                                                     false,
                                                                     -1,
                                                                     false,
                                                                     -1,
                                                                     false,
                                                                     -1,
                                                                     null,
                                                                     true);
            Assert.IsTrue(taxaCount > 0); 
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void GetTaxon()
        {
            Int32 taxonId;

            taxonId = TaxonManagerTest.GetOneTaxonId();
            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                using (DataReader dataReader = DataServer.GetTaxon(GetContext(), taxonId, taxonInformationType.ToString()))
                {
                    Assert.IsTrue(dataReader.Read());
                }
            }
        }

        [TestMethod]
        public void GetTaxonCountyOccurrence()
        {
            Int32 taxonId;

            taxonId = BEAR_TAXON_ID;
            using (DataReader dataReader = DataServer.GetTaxonCountyOccurrence(GetContext(), taxonId))
            {
                Assert.IsTrue(dataReader.Read());
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonNames()
        {
            Int32 taxonId;

            taxonId = TaxonManagerTest.GetOneTaxonId();
            using (DataReader dataReader = DataServer.GetTaxonNames(GetContext(), taxonId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            String nameSearchString;

            nameSearchString = TaxonManagerTest.GetOneTaxon(GetContext()).CommonName;
            foreach (SearchStringComparisonMethod nameSearchMethod in Enum.GetValues(typeof(SearchStringComparisonMethod)))
            {
                using (DataReader dataReader = DataServer.GetTaxonNamesBySearchCriteria(GetContext(), nameSearchString, nameSearchMethod.ToString()))
                {
                    Assert.IsTrue(dataReader.Read());
                }
            }
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria()
        {
            List<Int32> taxonIds;

            taxonIds = new List<Int32>();
            taxonIds.Add(MAMMAL_TAXON_ID);
            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext(), taxonIds));
            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                using (DataReader dataReader = DataServer.GetTaxonTreesBySearchCriteria(GetContext(), taxonInformationType.ToString(), true))
                {
                    Assert.IsTrue(dataReader.Read());
                    Assert.IsTrue(dataReader.NextResultSet());
                    Assert.IsTrue(dataReader.Read());
                    Assert.IsFalse(dataReader.NextResultSet());
                }
            }
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }

        [TestMethod]
        public void HasPendingTransaction()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    Assert.IsFalse(database.HasPendingTransaction());
                    database.BeginTransaction();
                    Assert.IsTrue(database.HasPendingTransaction());
                }
            }
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                ping = DataServer.Ping(GetContext(), databaseId);
                Assert.IsTrue(ping);
            }
        }

        [TestMethod]
        public void RollbackTransaction()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    database.BeginTransaction();
                    database.RollbackTransaction();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void RollbackTransactionNoTransactionError()
        {
            foreach (DataServer.DatabaseId databaseId in Enum.GetValues(typeof(DataServer.DatabaseId)))
            {
                using (DataServer database = new DataServer(databaseId))
                {
                    database.RollbackTransaction();
                }
            }
        }

        [TestMethod]
        public void UpdateLog()
        {
            DataServer.UpdateLog(GetContext(), "Have a nice day!", "AutomaticTest", null);
        }

        [TestMethod]
        public void UpdateReference()
        {
            // There is a rollback on the data so we cannot se the update if we look in the database.

            DataServer.UpdateReference((GetContext()), JAN_EDELSJO_REFERENCE_ID, "AutotestName", 1966, "AutotestText", "AutoTestPerson");

            // Here we could add a test where we select the data from the updated row and compare with 
            // the data we sent.
        }

        [TestMethod]
        public void UpdateSpeciesFact()
        {
            DateTime now = DateTime.Now;
            List<Int32> speciesFactIds;
            WebSpeciesFact speciesFact;
            WebSpeciesFactQuality speciesFactQuality = null;

            // Get species fact.
            speciesFact = SpeciesFactManagerTest.GetOneSpeciesFact(GetContext());

            // Get new quality value.
            foreach (WebSpeciesFactQuality speciesFactQualityTemp in SpeciesFactManager.GetSpeciesFactQualities(GetContext()))
            {
                if (speciesFact.QualityId != speciesFactQualityTemp.Id)
                {
                    speciesFactQuality = speciesFactQualityTemp;
                    break;
                }
            }

            // Update species fact.
            DataServer.UpdateSpeciesFact(GetContext(),
                                         speciesFact.Id,
                                         speciesFact.ReferenceId,
                                         now,
                                         WebServiceData.UserManager.GetPerson(GetContext()).GetFullName(),
                                         speciesFact.IsFieldValue1Specified,
                                         speciesFact.FieldValue1,
                                         speciesFact.IsFieldValue2Specified,
                                         speciesFact.FieldValue2,
                                         speciesFact.IsFieldValue3Specified,
                                         speciesFact.FieldValue3,
                                         speciesFact.FieldValue4,
                                         speciesFact.FieldValue5,
                                         speciesFactQuality.Id);

            // Get updated species fact.
            speciesFactIds = new List<Int32>();
            speciesFactIds.Add(speciesFact.Id);
            speciesFact = SpeciesFactManager.GetSpeciesFactsById(GetContext(), speciesFactIds)[0];

            // Check updated species fact.
            Assert.IsNotNull(speciesFact);
            Assert.AreEqual(speciesFactQuality.Id, speciesFact.QualityId);
            Assert.AreEqual(now.Date, speciesFact.UpdateDate.Date);
//            Assert.AreEqual(UserManager.GetUser(GetContext()).FullName, speciesFact.UpdateUserFullName);
        }

        [TestMethod]
        public void UpdateUserSelecedTaxa()
        {
            DataServer.AddUserSelectedTaxa(GetContext(), TaxonManagerTest.GetUserSelectedTaxa(GetContext()));
            DataServer.UpdateUserSelecedTaxa(GetContext());
            DataServer.DeleteUserSelectedTaxa(GetContext());
        }
    }
}
