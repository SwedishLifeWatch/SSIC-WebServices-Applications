using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesObservationDataProviderListTest : TestBase
    {
        private SpeciesObservationDataProviderList _speciesObservationDataProviders;

        public SpeciesObservationDataProviderListTest()
        {
            _speciesObservationDataProviders = null;
        }

        [TestMethod]
        public void GetGuids()
        {
            List<String> guids;
            SpeciesObservationDataProviderList dataProviders;

            dataProviders = new SpeciesObservationDataProviderList();
            guids = dataProviders.GetGuids();
            Assert.IsTrue(guids.IsEmpty());

            guids = GetSpeciesObservationDataProviders(true).GetGuids();
            Assert.IsTrue(guids.IsNotEmpty());
            Assert.AreEqual(GetSpeciesObservationDataProviders().Count, guids.Count);
            foreach (String guid in guids)
            {
                Assert.IsTrue(guid.IsNotEmpty());
            }
        }

        private SpeciesObservationDataProviderList GetSpeciesObservationDataProviders(Boolean refresh = false)
        {
            if (_speciesObservationDataProviders.IsNull() || refresh)
            {
                _speciesObservationDataProviders = CoreData.SpeciesObservationManager.GetSpeciesObservationDataProviders(GetUserContext());
            }

            return _speciesObservationDataProviders;
        }
    }
}
