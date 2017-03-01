using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Shark;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Shark
{
    [TestClass]
    public class SharkConnectorTest : TestBase
    {
        [TestMethod]
        [Ignore]
        public void GetDatasetInformation()
        {
            List<String> datasets;
            SharkConnector connector = new SharkConnector();

            datasets = connector.GetDatasetInformation();
            Assert.IsTrue(datasets.IsNotEmpty());
            foreach (String dataset in datasets)
            {
                Debug.WriteLine(dataset);
            }
        }

        [TestMethod]
        [Ignore]
        public void GetPageCount()
        {
            SharkConnector connector = new SharkConnector();
            Int64 pageCount, speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.IsTrue(0 < speciesObservationCount);
            Debug.WriteLine("Shark, total number of species observations is " + speciesObservationCount);
            pageCount = connector.GetPageCount(speciesObservationCount);
            Assert.IsTrue(0 < pageCount);
            Debug.WriteLine("Shark, total number of pages is " + pageCount);
        }

        [TestMethod]
        [Ignore]
        public void GetSpeciesObservationCount()
        {
            SharkConnector connector = new SharkConnector();
            Int64 speciesObservationCount;

            speciesObservationCount = connector.GetSpeciesObservationCount();
            Assert.IsTrue(0 < speciesObservationCount);
            Debug.WriteLine("Shark, total number of species observations are " + speciesObservationCount);
        }
    }
}
