using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Mvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Mvm
{
    [TestClass]
    public class MvmConnectorTest : TestBase
    {
        private MvmConnector _mvmConnector;

        public MvmConnectorTest()
        {
            _mvmConnector = null;
        }

        private MvmConnector GetMvmConnector(Boolean refresh = false)
        {
            if (_mvmConnector.IsNull() || refresh)
            {
                _mvmConnector = new MvmConnector();
            }

            return _mvmConnector;
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

            dataProvider = GetMvmConnector(true).GetSpeciesObservationDataProvider(GetContext());
            fieldDescriptions = WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(GetContext());
            mappings = HarvestManager.CreateMappingList(fieldDescriptions, dataProvider.Id);
            GetContext().GetSpeciesObservationDatabase().SetMaxChangeId(dataProvider.Id, 100000);
            GetContext().GetSpeciesObservationDatabase().EmptyTempTables();
            areMoreSpeciesObservationsAvailable = GetMvmConnector().GetSpeciesObservationChange(changedFrom,
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

            dataProvider = GetMvmConnector(true).GetSpeciesObservationDataProvider(GetContext());
            Assert.IsNotNull(dataProvider);
            Assert.AreEqual(dataProvider.Id, (Int32)(SpeciesObservationDataProviderId.Mvm));
        }
    }
}
