using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;
using TaxonSearchScope = ArtDatabanken.Data.WebService.TaxonSearchScope;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for TaxonSearchCriteriaTest
    /// </summary>
    [TestClass]
    public class TaxonSearchCriteriaTest : TestBase
    {
        private ArtDatabanken.Data.ArtDatabankenService.TaxonSearchCriteria _searchCriteria;

        public TaxonSearchCriteriaTest()
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
            String taxonNameSearchString;

            taxonNameSearchString = "björn";
            GetSearchCriteria(true).TaxonNameSearchString = taxonNameSearchString;
            GetSearchCriteria().CheckData();
        }

        private ArtDatabanken.Data.ArtDatabankenService.TaxonSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private ArtDatabanken.Data.ArtDatabankenService.TaxonSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonSearchCriteria();
            }
            return _searchCriteria;
        }

        [TestMethod]
        public void RestrictReturnToScope()
        {
            foreach (WebService.TaxonSearchScope taxonReturnScope in Enum.GetValues(typeof(WebService.TaxonSearchScope)))
            {
                GetSearchCriteria(true).RestrictReturnToScope = taxonReturnScope;
                Assert.AreEqual(GetSearchCriteria().RestrictReturnToScope, taxonReturnScope);
            }
        }

        [TestMethod]
        public void RestrictReturnToSwedishSpecies()
        {
            Boolean restrictReturnToSwedishSpecies;

            restrictReturnToSwedishSpecies = false;
            GetSearchCriteria(true).RestrictReturnToSwedishSpecies = restrictReturnToSwedishSpecies;
            Assert.IsFalse(GetSearchCriteria().RestrictReturnToSwedishSpecies);

            restrictReturnToSwedishSpecies = true;
            GetSearchCriteria().RestrictReturnToSwedishSpecies = restrictReturnToSwedishSpecies;
            Assert.IsTrue(GetSearchCriteria().RestrictReturnToSwedishSpecies);
        }

        [TestMethod]
        public void RestrictReturnToTaxonTypeIds()
        {
            Int32 taxonTypeIdIndex;
            List<Int32> taxonTypeIds;

            taxonTypeIds = null;
            GetSearchCriteria(true).RestrictReturnToTaxonTypeIds = taxonTypeIds;
            Assert.IsNull(GetSearchCriteria().RestrictReturnToTaxonTypeIds);

            taxonTypeIds = new List<Int32>();
            GetSearchCriteria().RestrictReturnToTaxonTypeIds = taxonTypeIds;
            Assert.AreEqual(GetSearchCriteria().RestrictReturnToTaxonTypeIds, taxonTypeIds);
            Assert.AreEqual(GetSearchCriteria().RestrictReturnToTaxonTypeIds.Count, taxonTypeIds.Count);

            taxonTypeIds.Add(42);
            GetSearchCriteria().RestrictReturnToTaxonTypeIds = taxonTypeIds;
            Assert.AreEqual(GetSearchCriteria().RestrictReturnToTaxonTypeIds, taxonTypeIds);
            Assert.AreEqual(GetSearchCriteria().RestrictReturnToTaxonTypeIds.Count, taxonTypeIds.Count);
            for (taxonTypeIdIndex = 0; taxonTypeIdIndex < taxonTypeIds.Count; taxonTypeIdIndex++)
            {
                Assert.AreEqual(GetSearchCriteria().RestrictReturnToTaxonTypeIds[taxonTypeIdIndex], taxonTypeIds[taxonTypeIdIndex]);
            }

            taxonTypeIds.Add(11);
            GetSearchCriteria().RestrictReturnToTaxonTypeIds = taxonTypeIds;
            Assert.AreEqual(GetSearchCriteria().RestrictReturnToTaxonTypeIds, taxonTypeIds);
            Assert.AreEqual(GetSearchCriteria().RestrictReturnToTaxonTypeIds.Count, taxonTypeIds.Count);
            for (taxonTypeIdIndex = 0; taxonTypeIdIndex < taxonTypeIds.Count; taxonTypeIdIndex++)
            {
                Assert.AreEqual(GetSearchCriteria().RestrictReturnToTaxonTypeIds[taxonTypeIdIndex], taxonTypeIds[taxonTypeIdIndex]);
            }
        }

        [TestMethod]
        public void RestrictSearchToSwedishSpecies()
        {
            Boolean restrictSearchToSwedishSpecies;

            restrictSearchToSwedishSpecies = false;
            GetSearchCriteria(true).RestrictSearchToSwedishSpecies = restrictSearchToSwedishSpecies;
            Assert.IsFalse(GetSearchCriteria().RestrictSearchToSwedishSpecies);

            restrictSearchToSwedishSpecies = true;
            GetSearchCriteria().RestrictSearchToSwedishSpecies = restrictSearchToSwedishSpecies;
            Assert.IsTrue(GetSearchCriteria().RestrictSearchToSwedishSpecies);
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

        [TestMethod]
        public void TaxonNameSearchString()
        {
            String taxonNameSearchString;

            taxonNameSearchString = null;
            GetSearchCriteria(true).TaxonNameSearchString = taxonNameSearchString;
            Assert.IsNull(GetSearchCriteria().TaxonNameSearchString);

            taxonNameSearchString = String.Empty;
            GetSearchCriteria().TaxonNameSearchString = taxonNameSearchString;
            Assert.AreEqual(GetSearchCriteria().TaxonNameSearchString, taxonNameSearchString);

            taxonNameSearchString = "björn";
            GetSearchCriteria().TaxonNameSearchString = taxonNameSearchString;
            Assert.AreEqual(GetSearchCriteria().TaxonNameSearchString, taxonNameSearchString);
        }
    }
}
