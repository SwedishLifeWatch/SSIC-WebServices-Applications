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
    public class TaxonListTest : TestBase
    {
        private ArtDatabanken.Data.ArtDatabankenService.TaxonList _taxa;

        public TaxonListTest()
        {
            _taxa = null;
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
        public void Get()
        {
            foreach (ArtDatabanken.Data.ArtDatabankenService.Taxon taxon in GetTaxa())
            {
                Assert.AreEqual(taxon, GetTaxa().Get(taxon.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 taxonId;

            taxonId = Int32.MinValue;
            GetTaxa().Get(taxonId);
        }

        [TestMethod]
        public void Exists()
        {
            ArtDatabanken.Data.ArtDatabankenService.TaxonList taxonList;
            taxonList = GetTaxa();

            Assert.IsTrue(taxonList.Exists(ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(BEAR_TAXON_ID, 0)));
        }

        private ArtDatabanken.Data.ArtDatabankenService.TaxonList GetTaxa()
        {
            if (_taxa.IsNull())
            {
                _taxa = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxa(TaxonManagerTest.GetTaxaIds(), TaxonInformationType.Basic);
            }
            return _taxa;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 taxonIndex;
            ArtDatabanken.Data.ArtDatabankenService.TaxonList newTaxonList, oldTaxonList;

            oldTaxonList = GetTaxa();
            newTaxonList = new ArtDatabanken.Data.ArtDatabankenService.TaxonList();
            for (taxonIndex = 0; taxonIndex < oldTaxonList.Count; taxonIndex++)
            {
                newTaxonList.Add(oldTaxonList[oldTaxonList.Count - taxonIndex - 1]);
            }
            for (taxonIndex = 0; taxonIndex < oldTaxonList.Count; taxonIndex++)
            {
                Assert.AreEqual(newTaxonList[taxonIndex], oldTaxonList[oldTaxonList.Count - taxonIndex - 1]);
            }
        }

        [TestMethod]
        public void GetTaxaBySearchString()
        {
            ArtDatabanken.Data.ArtDatabankenService.TaxonList taxa = GetTaxa();

            ArtDatabanken.Data.ArtDatabankenService.TaxonList subset = taxa.GetTaxaBySearchString("A", StringComparison.CurrentCultureIgnoreCase);
            Assert.IsNotNull(subset);
            String firstString = subset[0].ScientificName;
            ArtDatabanken.Data.ArtDatabankenService.TaxonList subset1 = taxa.GetTaxaBySearchString(firstString, StringComparison.CurrentCultureIgnoreCase);
            Assert.IsNotNull(subset1);
        }

    }
}
