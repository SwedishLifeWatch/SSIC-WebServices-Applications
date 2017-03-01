using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class HarvestManagerTest : TestBase
    {
        /// <summary>
        /// This testcase has some assumptions:
        /// * The class TestBase is modifed for use with production.
        /// * Ids for species observations that should be checked are inserted into table
        ///   ElasticsearchSpeciesObservationAdd in database SwedishSpeciesObservation.
        /// * Stored procedure GetSpeciesObservationDifference is modified to only
        ///   returned result and not calculated any differences.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void CheckDifference()
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            DocumentFilterResponse documentFilterResponse;
            ElasticsearchSpeciesObservationProxy elasticsearch;
            Int32 index;
            List<Int64> speciesObservationAdd, speciesObservationDelete,
                        speciesObservationIds, speciesObservationDifferenceIds;
            List<WebSpeciesObservation> speciesObservations;
            SpeciesObservationElasticsearch speciesObservationElasticsearch;
            StringBuilder filter;

            speciesObservationAdd = new List<Int64>();
            speciesObservationDelete = new List<Int64>();
            HarvestManager.GetSpeciesObservationDifference(GetContext(), speciesObservationAdd, speciesObservationDelete);

            if (speciesObservationAdd.IsNotEmpty())
            {
                speciesObservationElasticsearch = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(GetContext());
                elasticsearch = new ElasticsearchSpeciesObservationProxy(speciesObservationElasticsearch.CurrentIndexName);
                mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(GetContext(), elasticsearch);

                filter = new StringBuilder();
                filter.Append("{");
                filter.Append(" \"size\": " + speciesObservationAdd.Count);
                filter.Append(", \"_source\" : {\"include\": [\"Occurrence_OccurrenceID\", \"DarwinCore_Id\"]}");
                filter.Append(", \"filter\": { \"terms\": { \"DarwinCore_Id\":[");
                filter.Append(speciesObservationAdd[0].WebToString());
                for (index = 1; index < speciesObservationAdd.Count; index++)
                {
                    filter.Append(", " + speciesObservationAdd[index].WebToString());
                }

                filter.Append("]}}}");
                documentFilterResponse = elasticsearch.GetSpeciesObservations(filter.ToString());
                if (documentFilterResponse.TimedOut)
                {
                    throw new Exception("Method UpdateSpeciesObservationsElasticsearch() timed out!");
                }

                speciesObservations = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservations(documentFilterResponse.DocumentsJson,
                                                                                                                        mapping);
                if (speciesObservations.IsNotEmpty())
                {
                    speciesObservationIds = new List<Int64>();
                    foreach (WebSpeciesObservation speciesObservationDeleteTemp in speciesObservations)
                    {
                        speciesObservationIds.Add(speciesObservationDeleteTemp.Fields.GetField(SpeciesObservationClassId.DarwinCore, SpeciesObservationPropertyId.Id).Value.WebParseInt64());
                    }

                    speciesObservationDifferenceIds = new List<Int64>();
                    foreach (Int64 speciesObservationId in speciesObservationAdd)
                    {
                        if (!(speciesObservationIds.Contains(speciesObservationId)))
                        {
                            speciesObservationDifferenceIds.Add(speciesObservationId);
                            Debug.WriteLine("Missing species observation id = " + speciesObservationId);
                        }
                    }
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void CheckSpeciesObservations()
        {
            HarvestManager.CheckSpeciesObservations(GetContext());
        }

        /// <summary>
        /// This testcase has some assumptions:
        /// * The class TestBase is modifed for use with production.
        /// * Ids for species observations that should be deleted are inserted into table
        ///   ElasticsearchSpeciesObservationDelete in database SwedishSpeciesObservation.
        /// * Stored procedure GetSpeciesObservationDifference is modified to only
        ///   returned result and not calculated any differences.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void DeleteDifference()
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            DocumentFilterResponse documentFilterResponse;
            ElasticsearchSpeciesObservationProxy elasticsearch;
            Int32 index;
            List<Int64> speciesObservationAdd, speciesObservationDelete, speciesObservationIds;
            List<WebSpeciesObservation> speciesObservations;
            SpeciesObservationElasticsearch speciesObservationElasticsearch;
            StringBuilder filter;

            speciesObservationAdd = new List<Int64>();
            speciesObservationDelete = new List<Int64>();
            HarvestManager.GetSpeciesObservationDifference(GetContext(), speciesObservationAdd, speciesObservationDelete);
            speciesObservationElasticsearch = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(GetContext());
            elasticsearch = new ElasticsearchSpeciesObservationProxy(speciesObservationElasticsearch.CurrentIndexName);
            mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(GetContext(), elasticsearch);

            while (speciesObservationDelete.IsNotEmpty())
            {
                speciesObservationIds = new List<Int64>();
                while (speciesObservationDelete.IsNotEmpty())
                {
                    speciesObservationIds.Add(speciesObservationDelete[0]);
                    speciesObservationDelete.RemoveAt(0);
                    if (speciesObservationIds.Count >= 10000)
                    {
                        break;
                    }
                }

                filter = new StringBuilder();
                filter.Append("{");
                filter.Append(" \"size\": " + speciesObservationIds.Count);
                filter.Append(", \"_source\" : {\"include\": [\"Occurrence_OccurrenceID\", \"DarwinCore_Id\", \"DarwinCore_DataProviderId\", \"Occurrence_CatalogNumber\"]}");
                filter.Append(", \"filter\": { \"terms\": { \"DarwinCore_Id\":[");
                filter.Append(speciesObservationIds[0].WebToString());
                for (index = 1; index < speciesObservationIds.Count; index++)
                {
                    filter.Append(", " + speciesObservationIds[index].WebToString());
                }

                filter.Append("]}}}");
                documentFilterResponse = elasticsearch.GetSpeciesObservations(filter.ToString());
                if (documentFilterResponse.TimedOut)
                {
                    throw new Exception("Method UpdateSpeciesObservationsElasticsearch() timed out!");
                }

                speciesObservations = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservations(documentFilterResponse.DocumentsJson,
                                                                                                                        mapping);
                if (speciesObservations.IsNotEmpty())
                {
                    DataTable deletedSpeciesObservationTable = new DataTable();
                    deletedSpeciesObservationTable.TableName = "DeletedSpeciesObservation";
                    deletedSpeciesObservationTable.Columns.Add("id", typeof(Int64));
                    deletedSpeciesObservationTable.Columns.Add("dataProviderId", typeof(Int32));
                    deletedSpeciesObservationTable.Columns.Add("catalogNumber", typeof(String));
                    deletedSpeciesObservationTable.Columns.Add("occuranceId", typeof(String));

                    foreach (WebSpeciesObservation speciesObservation in speciesObservations)
                    {
                        DataRow speciesObservationRow = deletedSpeciesObservationTable.NewRow();
                        speciesObservationRow[0] = speciesObservation.Fields.GetField(SpeciesObservationClassId.DarwinCore, SpeciesObservationPropertyId.Id).Value.WebParseInt64();
                        speciesObservationRow[1] = speciesObservation.Fields.GetField("DarwinCore", "DataProviderId").Value.WebParseInt32();
                        speciesObservationRow[2] = speciesObservation.Fields.GetField(SpeciesObservationClassId.Occurrence, SpeciesObservationPropertyId.CatalogNumber).Value;
                        speciesObservationRow[3] = speciesObservation.Fields.GetField(SpeciesObservationClassId.Occurrence, SpeciesObservationPropertyId.OccurrenceID).Value;
                        deletedSpeciesObservationTable.Rows.Add(speciesObservationRow);
                    }

                    GetContext().GetSpeciesObservationDatabase().AddTableData(GetContext(), deletedSpeciesObservationTable);
                }
            }
        }

        [TestMethod]
        public void DeleteUnnecessaryChanges()
        {
            HarvestManager.DeleteUnnecessaryChanges(GetContext());
        }

        [TestMethod]
        public void GetSpeciesObservationUpdateStatus()
        {
            WebSpeciesObservationHarvestStatus harvestStatus;

            harvestStatus = HarvestManager.GetSpeciesObservationUpdateStatus(GetContext());
            Assert.IsNotNull(harvestStatus);
        }

        [TestMethod]
        [Ignore]
        public void UpdateAllSpeciesObservationsMvm()
        {
            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            Int64 maxChangeId = -2;
            List<Int32> dataProviderIds;
            SpeciesObservationHarvestService.Data.SpeciesObservationManager speciesObservationManager;
            WebSpeciesObservationDataProvider dataProvider;

            SpeciesObservationConfiguration.IsElasticsearchUsed = false;
            speciesObservationManager = new SpeciesObservationHarvestService.Data.SpeciesObservationManager();
            dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.Mvm);
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(dataProvider.Id);

            // Remove comment from next two lines if a complete reharvest is needed.
            // GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, -1);
            // dataProvider.MaxChangeId = -1;

            while (maxChangeId < dataProvider.MaxChangeId)
            {
                maxChangeId = dataProvider.MaxChangeId;
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
                dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.Mvm);

                while (5 <= DateTime.Now.Hour)
                {
                    // Do not harvest to new index at the same time as
                    // automatic update is done to currently used index.
                    Thread.Sleep(600000);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateAllSpeciesObservationsEntomologicalCollections()
        {
            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            Int64 maxChangeId = -1;
            List<Int32> dataProviderIds;
            SpeciesObservationHarvestService.Data.SpeciesObservationManager speciesObservationManager;
            WebSpeciesObservationDataProvider dataProvider;

            SpeciesObservationConfiguration.IsElasticsearchUsed = false;
            speciesObservationManager = new SpeciesObservationHarvestService.Data.SpeciesObservationManager();
            dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.EntomologicalCollections);
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(dataProvider.Id);

            // Remove comment from next two lines if a complete reharvest is needed.
            // GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, 0);
            // dataProvider.MaxChangeId = 0;

            while (maxChangeId < dataProvider.MaxChangeId)
            {
                maxChangeId = dataProvider.MaxChangeId;
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
                dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.EntomologicalCollections);

                while ((DateTime.Now.Hour == 4) || (DateTime.Now.Hour == 5))
                {
                    // Do not harvest to new index at the same time as
                    // automatic update is done to currently used index.
                    Thread.Sleep(600000);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateAllSpeciesObservationsHerbariumOfOskarshamn()
        {
            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            Int64 maxChangeId = -1;
            List<Int32> dataProviderIds;
            SpeciesObservationHarvestService.Data.SpeciesObservationManager speciesObservationManager;
            WebSpeciesObservationDataProvider dataProvider;

            SpeciesObservationConfiguration.IsElasticsearchUsed = false;
            speciesObservationManager = new SpeciesObservationHarvestService.Data.SpeciesObservationManager();
            dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.HerbariumOfOskarshamn);
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(dataProvider.Id);

            // Remove comment from next two lines if a complete reharvest is needed.
            // GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, 0);
            // dataProvider.MaxChangeId = 0;

            while (maxChangeId < dataProvider.MaxChangeId)
            {
                maxChangeId = dataProvider.MaxChangeId;
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
                dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.HerbariumOfOskarshamn);

                while ((DateTime.Now.Hour == 4) || (DateTime.Now.Hour == 5))
                {
                    // Do not harvest to new index at the same time as
                    // automatic update is done to currently used index.
                    Thread.Sleep(600000);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateAllSpeciesObservationsShark()
        {
            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            Int64 maxChangeId = -1;
            List<Int32> dataProviderIds;
            SpeciesObservationHarvestService.Data.SpeciesObservationManager speciesObservationManager;
            WebSpeciesObservationDataProvider dataProvider;

            SpeciesObservationConfiguration.IsElasticsearchUsed = false;
            speciesObservationManager = new SpeciesObservationHarvestService.Data.SpeciesObservationManager();
            dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.Shark);
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(dataProvider.Id);

            // Remove comment from next two lines if a complete reharvest is needed.
            // GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, 0);
            // dataProvider.MaxChangeId = 0;

            while (maxChangeId < dataProvider.MaxChangeId)
            {
                maxChangeId = dataProvider.MaxChangeId;
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
                dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.Shark);

                if (Configuration.InstallationType == InstallationType.Production)
                {
                    while (6 <= DateTime.Now.Hour)
                    {
                        // Do not harvest to new index at the same time as
                        // automatic update is done to currently used index.
                        Thread.Sleep(600000);
                    }
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateAllSpeciesObservationsSwedishMalaiseTrapProject()
        {
            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            Int64 maxChangeId = -1;
            List<Int32> dataProviderIds;
            SpeciesObservationHarvestService.Data.SpeciesObservationManager speciesObservationManager;
            WebSpeciesObservationDataProvider dataProvider;

            SpeciesObservationConfiguration.IsElasticsearchUsed = false;
            speciesObservationManager = new SpeciesObservationHarvestService.Data.SpeciesObservationManager();
            dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.SwedishMalaiseTrapProject);
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(dataProvider.Id);

            // Remove comment from next two lines if a complete reharvest is needed.
            // GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, 0);
            // dataProvider.MaxChangeId = 0;

            while (maxChangeId < dataProvider.MaxChangeId)
            {
                maxChangeId = dataProvider.MaxChangeId;
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
                dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.SwedishMalaiseTrapProject);

                while ((DateTime.Now.Hour == 4) || (DateTime.Now.Hour == 5))
                {
                    // Do not harvest to new index at the same time as
                    // automatic update is done to currently used index.
                    Thread.Sleep(600000);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateAllSpeciesObservationsUme()
        {
            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            Int64 maxChangeId = -1;
            List<Int32> dataProviderIds;
            SpeciesObservationHarvestService.Data.SpeciesObservationManager speciesObservationManager;
            WebSpeciesObservationDataProvider dataProvider;

            SpeciesObservationConfiguration.IsElasticsearchUsed = false;
            speciesObservationManager = new SpeciesObservationHarvestService.Data.SpeciesObservationManager();
            dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.HerbariumOfUmeaUniversity);
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(dataProvider.Id);

            // Remove comment from next two lines if a complete reharvest is needed.
            // GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, 0);
            // dataProvider.MaxChangeId = 0;

            while (maxChangeId < dataProvider.MaxChangeId)
            {
                maxChangeId = dataProvider.MaxChangeId;
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
                dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.HerbariumOfUmeaUniversity);

                while ((DateTime.Now.Hour == 4) || (DateTime.Now.Hour == 5))
                {
                    // Do not harvest to new index at the same time as
                    // automatic update is done to currently used index.
                    Thread.Sleep(600000);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateAllSpeciesObservationsWram()
        {
            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            Int64 maxChangeId = -2;
            List<Int32> dataProviderIds;
            SpeciesObservationHarvestService.Data.SpeciesObservationManager speciesObservationManager;
            WebSpeciesObservationDataProvider dataProvider;

            SpeciesObservationConfiguration.IsElasticsearchUsed = false;
            speciesObservationManager = new SpeciesObservationHarvestService.Data.SpeciesObservationManager();
            dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.Wram);
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add(dataProvider.Id);

            // Remove comment from next two lines if a complete reharvest is needed.
            // GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, -1);
            // dataProvider.MaxChangeId = -1;

            while (maxChangeId < dataProvider.MaxChangeId)
            {
                maxChangeId = dataProvider.MaxChangeId;
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
                dataProvider = speciesObservationManager.GetSpeciesObservationDataProvider(GetContext(), SpeciesObservationDataProviderId.Wram);

                while ((DateTime.Now.Hour == 4) || (DateTime.Now.Hour == 5))
                {
                    // Do not harvest to new index at the same time as
                    // automatic update is done to currently used index.
                    Thread.Sleep(600000);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateSpeciesObservations()
        {
            DateTime changedFrom, changedTo;
            List<Int32> dataProviderIds;

            //SpeciesObservationConfiguration.IsElasticsearchUsed = true;

            // Set changedFrom and changedTo to yesterday.
            //changedFrom = new DateTime(DateTime.Now.Year,
            //    DateTime.Now.Month,
            //    DateTime.Now.Day) - new TimeSpan(1, 0, 0, 0);
            changedFrom = new DateTime(2015, 3, 23);
            changedTo = changedFrom;
            dataProviderIds = new List<Int32>();
            dataProviderIds.Add((int)(SpeciesObservationDataProviderId.SpeciesGateway)); // Artportalen 2
            //dataProviderIds.Add((int)(SpeciesObservationDataProviderId.Observationsdatabasen));
            //dataProviderIds.Add((int)(SpeciesObservationDataProviderId.Wram));
            HarvestManager.UpdateSpeciesObservations(GetContext(),
                                                     changedFrom,
                                                     changedTo,
                                                     dataProviderIds, false);
        }

        [TestMethod]
        [Ignore]
        public void UpdateSpeciesObservationsElasticsearchNewIndex()
        {
            DateTime lastPause = DateTime.Now;

            Configuration.InstallationType = InstallationType.LocalTest;
            while (HarvestManager.UpdateSpeciesObservationsElasticsearchNewIndex(GetContext()))
            {
                //SpeciesObservationElasticsearch speciesObservationElasticsearch = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(GetContext());
                //if (speciesObservationElasticsearch.NextIndexCount.HasValue &&
                //    speciesObservationElasticsearch.NextIndexCount.Value > 76000000)
                //{
                //    break;
                //}

                if (Configuration.InstallationType == InstallationType.Production)
                {
                    while ((DateTime.Now.Hour == 4) || (DateTime.Now.Hour == 5))
                    {
                        // Do not harvest to new index at the same time as
                        // automatic update is done to currently used index.
                        Thread.Sleep(600000);
                    }
                }

                Thread.Sleep(5000);

                if ((DateTime.Now - lastPause).TotalMinutes > 60)
                {
                    // Make a 5 minute break each hour.
                    // This will allow Elasticsearch to save transactions to disk.
                    Thread.Sleep(300000);
                    lastPause = DateTime.Now;
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateSpeciesObservationsElasticsearchToNewIndex()
        {
            SpeciesObservationElasticsearch speciesObservationElasticsearchAfter,
                                            speciesObservationElasticsearchBefore;

            speciesObservationElasticsearchBefore = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(GetContext());
            HarvestManager.UpdateSpeciesObservationsElasticsearchToNewIndex(GetContext());
            speciesObservationElasticsearchAfter =WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(GetContext());
            Assert.AreEqual(speciesObservationElasticsearchAfter.CurrentIndexChangeId, speciesObservationElasticsearchBefore.NextIndexChangeId.Value);
            Assert.AreEqual(speciesObservationElasticsearchAfter.CurrentIndexCount, speciesObservationElasticsearchBefore.NextIndexCount.Value);
            Assert.AreEqual(speciesObservationElasticsearchAfter.CurrentIndexName, speciesObservationElasticsearchBefore.NextIndexName);
            Assert.AreEqual(speciesObservationElasticsearchAfter.NextIndexChangeId.Value, speciesObservationElasticsearchBefore.CurrentIndexChangeId);
            Assert.AreEqual(speciesObservationElasticsearchAfter.NextIndexCount.Value, speciesObservationElasticsearchBefore.CurrentIndexCount);
            Assert.AreEqual(speciesObservationElasticsearchAfter.NextIndexName, speciesObservationElasticsearchBefore.CurrentIndexName);
        }

        [TestMethod]
        [Ignore]
        public void UpdateSpeciesObservationsMvm()
        {
            // Select number of times that MVM service should be called.
            Int32 numberOfRepetitions = 1;
            List<Int32> dataProviderIds;

            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;

            dataProviderIds = new List<Int32>();
            dataProviderIds.Add((Int32)(SpeciesObservationDataProviderId.Mvm));
            for (int index = 0; index < numberOfRepetitions; index++)
            {
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateSpeciesObservationsUme()
        {
            // Select number of times that Ume service should be called.
            Int32 numberOfRepetitions = 2;
            List<Int32> dataProviderIds;

            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;

            dataProviderIds = new List<Int32>();
            dataProviderIds.Add((Int32)(SpeciesObservationDataProviderId.HerbariumOfUmeaUniversity));
            for (int index = 0; index < numberOfRepetitions; index++)
            {
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateSpeciesObservationsWram()
        {
            // Select number of times that WRAM service should be called.
            Int32 numberOfRepetitions = 2;
            List<Int32> dataProviderIds;

            Boolean result;
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;

            dataProviderIds = new List<Int32>();
            dataProviderIds.Add((Int32)(SpeciesObservationDataProviderId.Wram));
            for (int index = 0; index < numberOfRepetitions; index++)
            {
                result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, dataProviderIds, false);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateProjectParameterInformation()
        {
            HarvestManager.UpdateProjectParameterInformation(new ArtportalenServer(), GetContext());
        }

        /// <summary>
        /// Update the new PropertyIdentifier column with data on the existing rows
        /// </summary>
        [TestMethod]
        [Ignore]
        public void UpdateSpeciesObservationFieldMappings()
        {
            foreach (var description in WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true).Where(item => !item.IsClass && item.IsSearchable && item.SortOrder != 9999))
            {
                SpeciesObservationPropertyId propertyIdentifier;
                if (!Enum.TryParse(description.Name, true, out propertyIdentifier))
                {
                    throw new NotImplementedException(description.Name);
                }

                foreach (var mapping in description.Mappings)
                {
                    Console.WriteLine("UPDATE SpeciesObservationFieldMapping SET PropertyIdentifier = '{0}' WHERE Id = {1}", propertyIdentifier, mapping.Id);
                }
            }
        }

        [TestMethod]
        [Ignore]
        public void UpdateTaxonInformation()
        {
            HarvestManager.UpdateTaxonInformation(GetContext());
        }
    }
}
