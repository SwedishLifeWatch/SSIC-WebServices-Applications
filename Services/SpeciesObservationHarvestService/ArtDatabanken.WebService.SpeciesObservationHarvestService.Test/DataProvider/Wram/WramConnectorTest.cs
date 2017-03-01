using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Wram;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Wram
{
    [TestClass]
    public class WramConnectorTest : TestBase
    {
        private WramConnector _wramConnector;

        public WramConnectorTest()
        {
            _wramConnector = null;
        }

        private WramConnector GetWramConnector(Boolean refresh = false)
        {
            if (_wramConnector.IsNull() || refresh)
            {
                _wramConnector = new WramConnector();
            }

            return _wramConnector;
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationChange()
        {
            Boolean areMoreSpeciesObservationsAvailable;
            ConnectorServer connectorServer = new ConnectorServer();
            DateTime changedFrom = new DateTime(2000, 1, 1);
            DateTime changedTo = changedFrom;
            List<HarvestMapping> mappings;
            List<WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = GetWramConnector(true).GetSpeciesObservationDataProvider(GetContext());
            fieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext());
            mappings = HarvestManager.CreateMappingList(fieldDescriptions, dataProvider.Id);
            GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, -1);
            GetContext().GetSpeciesObservationDatabase().EmptyTempTables();
            areMoreSpeciesObservationsAvailable = GetWramConnector().GetSpeciesObservationChange(changedFrom,
                                                                                                 changedTo,
                                                                                                 mappings,
                                                                                                 GetContext(),
                                                                                                 connectorServer);
            Assert.IsFalse(areMoreSpeciesObservationsAvailable);
        }

        [TestMethod]
        public void GetSpeciesObservationDataProvider()
        {
            WebSpeciesObservationDataProvider dataProvider;

            dataProvider = GetWramConnector(true).GetSpeciesObservationDataProvider(GetContext());
            Assert.IsNotNull(dataProvider);
            Assert.AreEqual(dataProvider.Id, (Int32)(SpeciesObservationDataProviderId.Wram));
        }
    }
}
