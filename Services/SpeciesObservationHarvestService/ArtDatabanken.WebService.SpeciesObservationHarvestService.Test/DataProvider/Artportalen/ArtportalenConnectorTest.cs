using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Artportalen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Artportalen
{
    [TestClass]
    public class ArtportalenConnectorTest : TestBase
    {
        [TestMethod]
        public void GetFullSpeciesObservationChange_20150323()
        {
            DateTime changedFrom = new DateTime(2015, 3, 23);
            DateTime changedTo = new DateTime(2015, 3, 24);

            var result = HarvestManager.UpdateSpeciesObservations(GetContext(), changedFrom, changedTo, new List<int> { (int)SpeciesObservationDataProviderId.SpeciesGateway }, false);
            Assert.IsTrue(result);

        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationChange()
        {
            DateTime changedFrom = new DateTime(2012, 10, 13);
            DateTime changedTo = new DateTime(2012, 10, 15);

            ArtportalenConnector artportalenConnector = new ArtportalenConnector();

            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            WebSpeciesObservationDataProvider dataProvider = artportalenConnector.GetSpeciesObservationDataProvider(GetContext());

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            artportalenConnector.GetSpeciesObservationChange(changedFrom, changedTo, mappings, GetContext(), new ConnectorServer());
        }



        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationSynchronization()
        {
            var sightingIds = GetContext().GetSpeciesObservationDatabase().GetAllSourceSpeciesObservationIdsForInsert((int)SpeciesObservationDataProviderId.SpeciesGateway);
            var totalCount = sightingIds.Count;
            var pageSize = 10000;
            var pageCount = totalCount / pageSize + 1;

            var stopwatchM = Stopwatch.StartNew();
            long stopwatchPrev = 0;

            var context = GetContext();
            var connector = new ArtportalenConnector();
            var webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(context, true);
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", 0, "webDarwinCoreFieldDescriptions.");

            // Get provider information
            var dataProvider = connector.GetSpeciesObservationDataProvider(context);
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", 0, "GetSpeciesObservationDataProvider.");
            var providerName = dataProvider.Name;
            var dataProviderId = dataProvider.Id;

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProviderId);

            //if (context.ClientToken.UserName != ApplicationIdentifier.SpeciesObservationHarvestService.ToString())
            //{
            //    WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.WebServiceAdministrator);
            //}

            for (var currentPage = 0; currentPage < pageCount; currentPage++)
            {
                var start = currentPage * pageSize + (currentPage == 0 ? 0 : 1);

                if ((start + pageSize) > totalCount)
                {
                    pageSize = totalCount - start;
                }

                //Har byggt hela skördningen här och lämnat HarvestManager orörd!!

                try
                {
                    // Make sure the temp tables in db are empty
                    context.GetSpeciesObservationDatabase().EmptyTempTables();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", 0, "Empty Temp tables before reading.");

                    // Ready with provider information
                    context.GetSpeciesObservationDatabase().Log(context, providerName, LogType.Information, string.Format("Start {0}, start:{1} pageSize:{2}", providerName, start, pageSize), String.Empty);

                    connector.GetSpeciesObservationChange(sightingIds.GetRange(start, pageSize), mappings, context,  new ConnectorServer());

                    //Add the deleted observations in the last loop
                    if ((currentPage + 1) == pageCount)
                    {
                        context.GetSpeciesObservationDatabase().AddTempDeleteSpeciesObservation(dataProviderId);
                        context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - DELETE", stopwatchM.ElapsedMilliseconds, " AddTempDeleteSpeciesObservation: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    }

                    // Log latest changed date (at data source), of modified information of harvested observations
                    context.GetSpeciesObservationDatabase().SetDataProviderLatestChangedDate(dataProvider.Id);

                    context.GetSpeciesObservationDatabase().UpdateTempObservationId();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Check", stopwatchM.ElapsedMilliseconds, " UpdateTempObservationId: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().CheckTaxonIdOnTemp();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Check", stopwatchM.ElapsedMilliseconds, " CheckTaxonIdOnTemp: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().CreatePointGoogleMercatorInTemp();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Calculate", stopwatchM.ElapsedMilliseconds, " CreatePointGoogleMercatorInTemp: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().UpdateAccuracyAndDisturbanceRadius();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Calculate", stopwatchM.ElapsedMilliseconds, " UpdateAccuracyAndDisturbanceRadius: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().MergeTempUpdateToPosition();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - POSITION", stopwatchM.ElapsedMilliseconds, " MergeTempUpdateToPosition: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().UpdateSpeciesObservationChange();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - CREATE & UPDATE", stopwatchM.ElapsedMilliseconds, " UpdateSpeciesObservationChange: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().MergeTempUpdateToDarwinCoreObservation();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - CREATE & UPDATE", stopwatchM.ElapsedMilliseconds, " MergeTempUpdateToDarwinCoreObservation: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().MergeTempUpdateToSpeciesObservationField();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - CREATE & UPDATE", stopwatchM.ElapsedMilliseconds, " MergeTempUpdateToSpeciesObservationField: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().CopyDeleteToSpeciesObservation();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - DELETE", stopwatchM.ElapsedMilliseconds, " CopyDeleteToSpeciesObservation: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().UpdateStatistics();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " UpdateStatistics:" + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().EmptyTempTables();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " EmptyTempTables: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().CleanLogUpdateError();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " CleanLogUpdateError: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    context.GetSpeciesObservationDatabase().CleanLogUpdateErrorDuplicates();
                    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " CleanLogUpdateErrorDuplicates: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    stopwatchPrev = stopwatchM.ElapsedMilliseconds;

                    //if (SpeciesObservationConfiguration.IsElasticsearchUsed)
                    //{
                    //  HarvestManager.UpdateSpeciesObservationsElasticsearchCurrentIndex(context);
                    //    context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " UpdateSpeciesObservationsElasticsearchCurrentIndex: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
                    //}

                    stopwatchM.Stop();
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                }
                finally
                {
                    try
                    {
                        // Clean up.
                        using (SpeciesObservationHarvestServer database = (SpeciesObservationHarvestServer)(WebServiceData.DatabaseManager.GetDatabase(context)))
                        {
                            database.EmptyTempTables();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            context.GetSpeciesObservationDatabase().CleanLogUpdateError();
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " CleanLogUpdateError: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().CleanLogUpdateErrorDuplicates();
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, " CleanLogUpdateErrorDuplicates: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev));
        }
    }
}
