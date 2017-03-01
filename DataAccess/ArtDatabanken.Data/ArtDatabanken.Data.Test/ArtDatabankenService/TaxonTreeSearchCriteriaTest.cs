using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for TaxonTreeSearchCriteriaTest
    /// </summary>
    [TestClass]
    public class TaxonTreeSearchCriteriaTest : TestBase
    {
        private Data.ArtDatabankenService.TaxonTreeSearchCriteria _searchCriteria;

        public TaxonTreeSearchCriteriaTest()
        {
            _searchCriteria = null;
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

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CheckData()
        {
            GetSearchCriteria(true).CheckData();
        }

        private Data.ArtDatabankenService.TaxonTreeSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private Data.ArtDatabankenService.TaxonTreeSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new Data.ArtDatabankenService.TaxonTreeSearchCriteria();
            }
            return _searchCriteria;
        }

        [TestMethod]
        public void RestrictSearchToTaxonIds()
        {
            Int32 taxonIdIndex;
            List<Int32> taxonIds;

            taxonIds = null;
            GetSearchCriteria(true).RestrictSearchToTaxonIds = taxonIds;
            Assert.IsNull(GetSearchCriteria().RestrictSearchToTaxonIds);

            taxonIds = new List<Int32>();
            GetSearchCriteria().RestrictSearchToTaxonIds = taxonIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonIds, taxonIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonIds.Count, taxonIds.Count);

            taxonIds.Add(42);
            GetSearchCriteria().RestrictSearchToTaxonIds = taxonIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonIds, taxonIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonIds.Count, taxonIds.Count);
            for (taxonIdIndex = 0; taxonIdIndex < taxonIds.Count; taxonIdIndex++)
            {
                Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonIds[taxonIdIndex], taxonIds[taxonIdIndex]);
            }

            taxonIds.Add(11);
            GetSearchCriteria().RestrictSearchToTaxonIds = taxonIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonIds, taxonIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonIds.Count, taxonIds.Count);
            for (taxonIdIndex = 0; taxonIdIndex < taxonIds.Count; taxonIdIndex++)
            {
                Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonIds[taxonIdIndex], taxonIds[taxonIdIndex]);
            }
        }

        [TestMethod]
        public void RestrictSearchToTaxonTypeIds()
        {
            Int32 taxonTypeIdIndex;
            List<Int32> taxonTypeIds;

            taxonTypeIds = null;
            GetSearchCriteria(true).RestrictSearchToTaxonTypeIds = taxonTypeIds;
            Assert.IsNull(GetSearchCriteria().RestrictSearchToTaxonTypeIds);

            taxonTypeIds = new List<Int32>();
            GetSearchCriteria().RestrictSearchToTaxonTypeIds = taxonTypeIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonTypeIds, taxonTypeIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonTypeIds.Count, taxonTypeIds.Count);

            taxonTypeIds.Add(42);
            GetSearchCriteria().RestrictSearchToTaxonTypeIds = taxonTypeIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonTypeIds, taxonTypeIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonTypeIds.Count, taxonTypeIds.Count);
            for (taxonTypeIdIndex = 0; taxonTypeIdIndex < taxonTypeIds.Count; taxonTypeIdIndex++)
            {
                Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonTypeIds[taxonTypeIdIndex], taxonTypeIds[taxonTypeIdIndex]);
            }

            taxonTypeIds.Add(11);
            GetSearchCriteria().RestrictSearchToTaxonTypeIds = taxonTypeIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonTypeIds, taxonTypeIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonTypeIds.Count, taxonTypeIds.Count);
            for (taxonTypeIdIndex = 0; taxonTypeIdIndex < taxonTypeIds.Count; taxonTypeIdIndex++)
            {
                Assert.AreEqual(GetSearchCriteria().RestrictSearchToTaxonTypeIds[taxonTypeIdIndex], taxonTypeIds[taxonTypeIdIndex]);
            }
        }

        [TestMethod]
        public void TaxonInformationType()
        {
            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                GetSearchCriteria(true).TaxonInformationType = taxonInformationType;
                Assert.AreEqual(GetSearchCriteria().TaxonInformationType, taxonInformationType);
            }
        }
    }
}
