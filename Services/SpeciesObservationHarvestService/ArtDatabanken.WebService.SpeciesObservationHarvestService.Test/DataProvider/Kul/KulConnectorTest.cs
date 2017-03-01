using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Kul;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Kul
{
    /// <summary>
    /// The kul connector test.
    /// </summary>
    [TestClass]
    public class KulConnectorTest : TestBase
    {
        /// <summary>
        /// The get species observation change.
        /// </summary>
        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            Int64 maxReturnedChanges = 100000;
            var changedFrom = new DateTime(2014, 7, 1);  
           //// var changedFrom = new DateTime(1987, 7, 1);          
            ////var changedTo = new DateTime(2015, 10, 29);
             var changedTo = new DateTime(2014, 9, 29);
            
            var kulConnector = new KulConnector();
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            WebServiceContext context = GetContext();
            
            // Read meta data for current job
            var harvestJobManager = new HarvestJobManager(context);
            var harvestJob = harvestJobManager.GetHarvestJob();

            WebSpeciesObservationDataProvider dataProvider = kulConnector.GetSpeciesObservationDataProvider(context);
            var dataProviderId = new List<int>() { dataProvider.Id };

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);


            // Log data provider harvest status
            /*
            context.GetSpeciesObservationDatabase().EmptyTempTables();
            context.GetSpeciesObservationDatabase().CleanLogUpdateError();
            context.GetSpeciesObservationDatabase().CleanLogUpdateErrorDuplicates();
            */

            context.GetSpeciesObservationDatabase().SetHarvestJobStatistics(dataProviderId, harvestJob.JobStatus, changedFrom);

            //// Stopwatch stopwatch1 = Stopwatch.StartNew();
            while (changedFrom < changedTo)
            {
                //// stopwatch1.Reset();
                //// stopwatch1 = Stopwatch.StartNew();
                kulConnector.GetSpeciesObservationChange(changedFrom, true, changedFrom.AddMonths(1), true, 0, false, maxReturnedChanges, mappings, GetContext(), new ConnectorServer());
                changedFrom = changedFrom.AddMonths(1);
                //// Debug.WriteLine("Harvest and fix: " + DateTime.Now.ToLongTimeString() + " - Time: " + stopwatch1.ElapsedMilliseconds);
            }
         

            // Log latest changed date (at data source), of modified information of harvested observations
            context.GetSpeciesObservationDatabase().SetDataProviderLatestChangedDate(dataProvider.Id);

            Stopwatch stopwatchM = Stopwatch.StartNew();
            string logMessage = String.Empty;
            
            long stopwatchPrev = 0;

            context.GetSpeciesObservationDatabase().UpdateTempObservationId();
            logMessage += " ID: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().CheckTaxonIdOnTemp();
            logMessage += ", TAXON: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Check", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().CreatePointGoogleMercatorInTemp();
            logMessage += " POINT: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Calculate", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().UpdateAccuracyAndDisturbanceRadius();
            logMessage += " ACCURACY_AND_DISTURBANCE_RADIUS: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Calculate", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().MergeTempUpdateToPosition();
            logMessage += " POSITION: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - POSITION", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().UpdateSpeciesObservationChange();
            logMessage += " SOC: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().MergeTempUpdateToDarwinCoreObservation();
            logMessage += " DCO: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().MergeTempUpdateToSpeciesObservationField();
            logMessage += ", SOF: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - CREATE & UPDATE", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().CopyDeleteToSpeciesObservation();
            logMessage += " SO: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - DELETE", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().UpdateStatistics();
            logMessage += ", DPS:" + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().EmptyTempTables();
            logMessage += ", Empty: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);

            context.GetSpeciesObservationDatabase().CleanLogUpdateError();
            context.GetSpeciesObservationDatabase().CleanLogUpdateErrorDuplicates();
            logMessage += ", LUE: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);

            stopwatchM.Stop();
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, logMessage);
           
            // Log data provider harvest status
            context.GetSpeciesObservationDatabase().SetHarvestJobStatistics(dataProviderId, HarvestStatusEnum.Done, changedFrom);                    
        }

        /// <summary>
        /// The get species observation change by change id.
        /// </summary>
        [TestMethod]
        public void GetSpeciesObservationChangeByChangeId()
        {
            DateTime changedFrom = new DateTime(2010, 1, 6);
            DateTime changedTo = new DateTime(2010, 1, 7);

            Int64 changeId = 8000; // changed from 0 - Calle
            Int64 maxReturnedChanges = 100;

            WebServiceContext context = GetContext();
            KulConnector kulConnector = new KulConnector();
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            // Read meta data for current job
            var harvestJobManager = new HarvestJobManager(context);
            var harvestJob = harvestJobManager.GetHarvestJob();

            WebSpeciesObservationDataProvider dataProvider = kulConnector.GetSpeciesObservationDataProvider(context);
            var dataProviderId = new List<int>() { dataProvider.Id };
            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            // Log data provider harvest status
            context.GetSpeciesObservationDatabase().SetHarvestJobStatistics(dataProviderId, harvestJob.JobStatus, changedFrom);

            kulConnector.GetSpeciesObservationChange(changedFrom, false, changedTo, false, changeId, true, maxReturnedChanges, mappings, context, new ConnectorServer());

            // Log latest changed date (at data source), of modified information of harvested observations
            context.GetSpeciesObservationDatabase().SetDataProviderLatestChangedDate(dataProvider.Id);

            Stopwatch stopwatchM = Stopwatch.StartNew();
            string logMessage = String.Empty;

            long stopwatchPrev = 0;

            context.GetSpeciesObservationDatabase().UpdateTempObservationId();
            logMessage += " ID: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().CheckTaxonIdOnTemp();
            logMessage += ", TAXON: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Check", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().CreatePointGoogleMercatorInTemp();
            logMessage += " POINT: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Calculate", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().UpdateAccuracyAndDisturbanceRadius();
            logMessage += " ACCURACY_AND_DISTURBANCE_RADIUS: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - Calculate", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().MergeTempUpdateToPosition();
            logMessage += " POSITION: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - POSITION", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().UpdateSpeciesObservationChange();
            logMessage += " SOC: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().MergeTempUpdateToDarwinCoreObservation();
            logMessage += " DCO: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().MergeTempUpdateToSpeciesObservationField();
            logMessage += ", SOF: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - CREATE & UPDATE", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().CopyDeleteToSpeciesObservation();
            logMessage += " SO: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest SP - DELETE", stopwatchM.ElapsedMilliseconds, logMessage);
            logMessage = String.Empty;

            context.GetSpeciesObservationDatabase().UpdateStatistics();
            logMessage += ", DPS:" + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);
            stopwatchPrev = stopwatchM.ElapsedMilliseconds;

            context.GetSpeciesObservationDatabase().EmptyTempTables();
            logMessage += ", Empty: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);

            context.GetSpeciesObservationDatabase().CleanLogUpdateError();
            context.GetSpeciesObservationDatabase().CleanLogUpdateErrorDuplicates();
            logMessage += ", LUE: " + (stopwatchM.ElapsedMilliseconds - stopwatchPrev);

            stopwatchM.Stop();
            context.GetSpeciesObservationDatabase().LogHarvestMove(context, "Harvest DB - OPERATIONS", stopwatchM.ElapsedMilliseconds, logMessage);

            // Log data provider harvest status
            context.GetSpeciesObservationDatabase().SetHarvestJobStatistics(dataProviderId, HarvestStatusEnum.Done, changedFrom);           
        }

        /// <summary>
        /// The get species observation change with standard signature.
        /// </summary>
        [TestMethod]
        public void GetSpeciesObservationChangeWithStandardSignature()
        {
            DateTime changedFrom = new DateTime(2010, 1, 6);
            DateTime changedTo = new DateTime(2010, 1, 7);

            KulConnector kulConnector = new KulConnector();
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            WebSpeciesObservationDataProvider dataProvider = kulConnector.GetSpeciesObservationDataProvider(GetContext());

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            // KulConnector.GetSpeciesObservationChange(changedFrom, true, changedTo, true, 0, true, maxReturnedChanges, mappings, GetContext(), new ConnectorServer());
            kulConnector.GetSpeciesObservationChange(changedFrom, changedTo, mappings, GetContext(), new ConnectorServer());
        }
    }
}
