using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Nors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Nors
{
    [TestClass]
    public class NorsConnectorTest : TestBase
    {
        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            Int64 maxReturnedChanges = 100;

            DateTime changedFrom = new DateTime(2012, 3, 1);
            DateTime changedTo = new DateTime(2014, 1, 1);

            NorsConnector norsConnector = new NorsConnector();
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            WebSpeciesObservationDataProvider dataProvider = norsConnector.GetSpeciesObservationDataProvider(GetContext());

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            while (changedFrom < changedTo)
            {
                norsConnector.GetSpeciesObservationChange(changedFrom, true, changedFrom.AddDays(1), true, 0, false, maxReturnedChanges, mappings, GetContext(), new ConnectorServer());
                changedFrom = changedFrom.AddDays(1);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationChange_ChangeId()
        {
            //DateTime changedFrom = new DateTime(2009, 11, 22);
            //DateTime changedTo = new DateTime(2009, 11, 24);

            DateTime changedFrom = new DateTime(2010, 1, 1);
            DateTime changedTo = new DateTime(2011, 1, 1);

            Int64 changeId = 0;
            Int64 maxReturnedChanges = 100;

            NorsConnector norsConnector = new NorsConnector();
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            WebSpeciesObservationDataProvider dataProvider = norsConnector.GetSpeciesObservationDataProvider(GetContext());

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            norsConnector.GetSpeciesObservationChange(changedFrom, false, changedTo, false, changeId, true, maxReturnedChanges, mappings, GetContext(), new ConnectorServer());
        }
        
        [TestMethod]
        public void GetSpeciesObservationChange_StandardSignature()
        {
            DateTime changedFrom = new DateTime(2010, 01, 1);
            DateTime changedTo = new DateTime(2010, 1, 2);

            NorsConnector norsConnector = new NorsConnector();
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            WebSpeciesObservationDataProvider dataProvider = norsConnector.GetSpeciesObservationDataProvider(GetContext());

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            //norsConnector.GetSpeciesObservationChange(changedFrom, true, changedTo, true, 0, true, maxReturnedChanges, mappings, GetContext(), new ConnectorServer());
            norsConnector.GetSpeciesObservationChange(changedFrom, changedTo, mappings, GetContext(), new ConnectorServer());
        }
   }
}
