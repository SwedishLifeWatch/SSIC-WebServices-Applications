using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeciesObservationManager = ArtDatabanken.WebService.Data.SpeciesObservationManager;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class SpeciesObservationManagerTest : TestBase
    {
        private SpeciesObservationManager _speciesObservationManager;

        public SpeciesObservationManagerTest()
        {
            _speciesObservationManager = null;
        }

        [TestMethod]
        public void GetSpeciesObservationDataProvider()
        {
            WebSpeciesObservationDataProvider dataProvider;

            GetSpeciesObservationManager(true);
            foreach (SpeciesObservationDataProviderId speciesObservationDataProviderId in Enum.GetValues(typeof(SpeciesObservationDataProviderId)))
            {
                dataProvider = GetSpeciesObservationManager().GetSpeciesObservationDataProvider(GetContext(),
                                                                                            speciesObservationDataProviderId);
                Assert.IsNotNull(dataProvider);
                Assert.AreEqual((Int32)speciesObservationDataProviderId, dataProvider.Id);
            }

            foreach (SpeciesObservationDataProviderId speciesObservationDataProviderId in Enum.GetValues(typeof(SpeciesObservationDataProviderId)))
            {
                dataProvider = GetSpeciesObservationManager().GetSpeciesObservationDataProvider(GetContext(),
                                                                                            (Int32)speciesObservationDataProviderId);
                Assert.IsNotNull(dataProvider);
                Assert.AreEqual((Int32)speciesObservationDataProviderId, dataProvider.Id);
            }
        }

        [TestMethod]
        public void GetSpeciesObservationDataProviders()
        {
            List<WebSpeciesObservationDataProvider> dataProviders = GetSpeciesObservationManager(true).GetSpeciesObservationDataProviders(GetContext());
            Assert.IsTrue(dataProviders.IsNotEmpty());
        }

        [TestMethod]
        public void GetSpeciesObservationElasticsearch()
        {
            SpeciesObservationElasticsearch speciesObservationElasticsearch;

            speciesObservationElasticsearch = WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationElasticsearch(GetContext());
            Assert.IsNotNull(speciesObservationElasticsearch);
            Assert.IsTrue(speciesObservationElasticsearch.CurrentIndexChangeId > 0);
            Assert.IsTrue(speciesObservationElasticsearch.CurrentIndexCount > 0);
            Assert.IsTrue(speciesObservationElasticsearch.CurrentIndexName.IsNotEmpty());
        }

        private SpeciesObservationManager GetSpeciesObservationManager(Boolean refresh = false)
        {
            if (_speciesObservationManager.IsNull() || refresh)
            {
                _speciesObservationManager = new SpeciesObservationManager();
            }

            return _speciesObservationManager;
        }
    }
}
