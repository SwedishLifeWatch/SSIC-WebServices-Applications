using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Dina;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Dina
{
    [TestClass]
    public class DinaConnectorTest : TestBase
    {
        [TestMethod]
        public void GetSpeciesObservationChange()
        {
            //DateTime changedFrom = new DateTime(2009, 11, 22);
            //DateTime changedTo = new DateTime(2009, 11, 24);

            DateTime changedFrom = new DateTime(2012, 05, 26);
            DateTime changedTo = new DateTime(2012, 05, 27); 

            DinaConnector dinaConnector = new DinaConnector();
            List<WebSpeciesObservationFieldDescriptionExtended> webDarwinCoreFieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext(), true);

            WebSpeciesObservationDataProvider dataProvider = dinaConnector.GetSpeciesObservationDataProvider(GetContext());

            var mappings = HarvestManager.CreateMappingList(webDarwinCoreFieldDescriptions, dataProvider.Id);

            dinaConnector.GetSpeciesObservationChange(changedFrom, changedTo,  mappings, GetContext(), new ConnectorServer());

            //Assert.IsTrue(speciesObservationChange.CreatedSpeciesObservations.IsNotEmpty());
            //Assert.IsTrue(speciesObservationChange.UpdatedSpeciesObservations.IsNotEmpty());
            //Assert.IsTrue(speciesObservationChange.DeletedSpeciesObservationGuids.IsNotEmpty());
            //Assert.IsTrue(speciesObservationChange.CreatedSpeciesObservations[0].Fields[0].IsNotNull());
            //Assert.IsTrue(speciesObservationChange.UpdatedSpeciesObservations[0].Fields[0].IsNotNull());
            //Assert.IsTrue(speciesObservationChange.DeletedSpeciesObservationGuids[0].IsNotNull());
        }
   }
}
