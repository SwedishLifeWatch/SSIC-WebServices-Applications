using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.Data
{
    [TestClass]
    public class ListWebSpeciesObservationDataProviderExtensionTest : TestBase
    {
        private List<WebSpeciesObservationDataProvider> _dataProviders;

        public ListWebSpeciesObservationDataProviderExtensionTest()
        {
            _dataProviders = null;
        }

        private List<WebSpeciesObservationDataProvider> GetDataProviders(Boolean refresh = false)
        {
            if (refresh || _dataProviders.IsNull())
            {
                _dataProviders = HarvestManager.GetSpeciesObservationDataProviders(GetContext());
            }

            return _dataProviders;
        }

        [TestMethod]
        public void GetDataProvidersThatUseChangeId()
        {
            List<WebSpeciesObservationDataProvider> dataProvidersThatUseChangeId;

            dataProvidersThatUseChangeId = GetDataProviders().GetDataProvidersThatUseChangeId();
            Assert.IsTrue(dataProvidersThatUseChangeId.IsNotEmpty());
            foreach (WebSpeciesObservationDataProvider dataProviderThatUseChangeId in dataProvidersThatUseChangeId)
            {
                Assert.IsTrue(dataProviderThatUseChangeId.IsMaxChangeIdSpecified);
            }
        }

        [TestMethod]
        public void GetDataProvidersThatUseDate()
        {
            List<WebSpeciesObservationDataProvider> dataProvidersThatUseDate;

            dataProvidersThatUseDate = GetDataProviders().GetDataProvidersThatUseDate();
            Assert.IsTrue(dataProvidersThatUseDate.IsNotEmpty());
            foreach (WebSpeciesObservationDataProvider dataProviderThatUseChangeId in dataProvidersThatUseDate)
            {
                Assert.IsFalse(dataProviderThatUseChangeId.IsMaxChangeIdSpecified);
            }
        }
    }
}
