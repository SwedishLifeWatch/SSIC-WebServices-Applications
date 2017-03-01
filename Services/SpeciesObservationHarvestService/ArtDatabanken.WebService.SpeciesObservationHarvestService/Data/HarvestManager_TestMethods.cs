using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using SpeciesObservationChange = ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.SpeciesObservationChange;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Manager of species observation harvest services.
    /// Test Methods, not used in production.
    /// These methods are used for testing purpose only and should not be removed or used for anything else.
    /// </summary>
    public partial class HarvestManager
    {
#if DEBUG

        /// <summary>
        /// Will get all changes from the first connection and return it to HarvestManagerTest.
        /// If you want to test more connectors, code must be rewritten.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="changedFrom">Changed from date.</param>
        /// <param name="changedTo">Changed to date.</param>
        /// <param name="dataProvider">Data provider.</param>
        public static void TestGetSpeciesObservationChange(WebServiceContext context,
                                                           DateTime changedFrom,
                                                           DateTime changedTo,
                                                           out WebSpeciesObservationDataProvider dataProvider)
        {
            IDataProviderConnector connector = Connectors[0];

            dataProvider = connector.GetSpeciesObservationDataProvider(context);
            
            // Read metadata from SpeciesObservationDatabase
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(context, true);

            var mappings = CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);
            ConnectorServer connectorServer = new ConnectorServer();
            connector.GetSpeciesObservationChange(changedFrom, changedTo, mappings, context, connectorServer);
        }

        /// <summary>
        /// Used to bridge to the private method CreateSpeciesObservations for the HarvestManagerTest.
        /// </summary>
        /// <param name="context">Web service context.</param>
        /// <param name="speciesObservations">Species observations.</param>
        /// <param name="dataProvider">Data provider.</param>
        /// <param name="changedFrom">Changed from date.</param>
        /// <param name="changedTo">Changed to date.</param>
        public static void TestCreateSpeciesObservations(WebServiceContext context,
                                                         SpeciesObservationChange speciesObservations,
                                                         WebSpeciesObservationDataProvider dataProvider,
                                                         DateTime changedFrom, 
                                                         DateTime changedTo)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int noOfUpdated, noOfUpdatedErrors, noOfDeleted, noOfDeletedErrors;

            ////ConnectorServer.CreateSpeciesObservations(context, speciesObservations.CreatedSpeciesObservations, dataProvider, out noOfCreated, out noOfCreatedErrors);
            ConnectorServer.UpdateSpeciesObservations(context, speciesObservations.UpdatedSpeciesObservations, dataProvider, out noOfUpdated, out noOfUpdatedErrors);
            ConnectorServer.DeleteSpeciesObservations(context, speciesObservations.DeletedSpeciesObservationGuids, dataProvider, out noOfDeleted, out noOfDeletedErrors);
            stopwatch.Stop();
            context.GetSpeciesObservationDatabase().LogHarvestRead(context, 
                                                                   dataProvider, 
                                                                   changedFrom, 
                                                                   changedTo,
                                                                   stopwatch.ElapsedMilliseconds, 
                                                                   0, 
                                                                   0,
                                                                   noOfUpdated, 
                                                                   noOfUpdatedErrors, 
                                                                   noOfDeleted, 
                                                                   noOfDeletedErrors);
        }

#endif
    }
}
