using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Observationsdatabasen;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Observationsdatabasen
{
    [TestClass]
    public class ObservationsdatabasenConnectorTest : TestBase
    {
        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            DateTime changedFrom = new DateTime(2009, 11, 03);
            DateTime changedTo = new DateTime(2009, 11, 03); 

            ObservationsdatabasenConnector obsdataConnector = new ObservationsdatabasenConnector();
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            WebSpeciesObservationDataProvider dataProvider = obsdataConnector.GetSpeciesObservationDataProvider(GetContext());

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            obsdataConnector.GetSpeciesObservationChange(changedFrom, changedTo, mappings, GetContext(), new ConnectorServer());

            //Assert.IsTrue(speciesObservationChange.CreatedSpeciesObservations.IsNotEmpty());

            //Assert.IsTrue(speciesObservationChange.UpdatedSpeciesObservations.IsNotEmpty());
            //Assert.IsTrue(speciesObservationChange.DeletedSpeciesObservationGuids.IsNotEmpty());
            //Assert.IsTrue(speciesObservationChange.CreatedSpeciesObservations[0].Fields[0].IsNotNull());
            //Assert.IsTrue(speciesObservationChange.UpdatedSpeciesObservations[0].Fields[0].IsNotNull());
            //Assert.IsTrue(speciesObservationChange.DeletedSpeciesObservationGuids[0].IsNotNull());
        }
   }
}
