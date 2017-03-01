using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class DataSortOrderTest : TestBase
    {
        public DataSortOrderTest()
        {
        }

        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        [TestMethod]
        public void CompareTo()
        {
            DataSortOrder data1, data2;

            data1 = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(BEAR_TAXON_ID, TaxonInformationType.Basic);
            data2 = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(GOLDEN_EAGLE_TAXON_ID, TaxonInformationType.Basic);
            if (data1.SortOrder < data2.SortOrder)
            {
                // Swap places.
                DataSortOrder tempData;

                tempData = data1;
                data1 = data2;
                data2 = tempData;
            }
            Assert.AreEqual(data1.CompareTo(data1), 0);
            Assert.IsTrue(data1.CompareTo(data2) > 0);
            Assert.IsTrue(data2.CompareTo(data1) < 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareToNotSortOrderError()
        {
            DataSortOrder data1, data2;

            data1 = GetDataSortOrder();
            data2 = null;
            Assert.AreEqual(data1.CompareTo(data2), 0);
        }

        [TestMethod]
        public void DataSortOrder()
        {
            Assert.IsTrue(GetDataSortOrder().SortOrder >= 0);
        }

        private DataSortOrder GetDataSortOrder()
        {
            return ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(BEAR_TAXON_ID, TaxonInformationType.Basic);
        }
    }
}
