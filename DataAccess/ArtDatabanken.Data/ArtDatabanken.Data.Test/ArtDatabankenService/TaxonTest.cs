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
    public class TaxonTest : TestBase
    {
        private ArtDatabanken.Data.ArtDatabankenService.Taxon _taxon;

        public TaxonTest()
        {
            _taxon = null;
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
        public void Author()
        {
            String author;

            author = GetTaxon().Author;
        }

        [TestMethod]
        public void CommonName()
        {
            String commonName;

            commonName = GetTaxon().CommonName;
        }

        private ArtDatabanken.Data.ArtDatabankenService.Taxon GetTaxon()
        {
            if (_taxon.IsNull())
            {
                _taxon = TaxonManagerTest.GetOneTaxon();
            }
            return _taxon;
        }

        [TestMethod]
        public void ScientificName()
        {
            String scientificName;

            scientificName = GetTaxon().ScientificName;
        }

        [TestMethod]
        public void ScientificNameAndAuthor()
        {
            String scientificNameAndAuthor;

            scientificNameAndAuthor = GetTaxon().ScientificNameAndAuthor;
        }

        [TestMethod]
        public void TaxonNames()
        {
            Assert.IsTrue(GetTaxon().TaxonNames.IsNotEmpty());
        }

        [TestMethod]
        public void TaxonType()
        {
            Assert.IsNotNull(GetTaxon().TaxonType);
        }

        [TestMethod]
        public void TaxonInformationType()
        {
            Int32 taxonId;
            ArtDatabanken.Data.ArtDatabankenService.Taxon taxon;

            taxonId = BEAR_TAXON_ID;
            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxon = ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(taxonId, taxonInformationType);
                Assert.AreEqual(taxon.TaxonInformationType, taxonInformationType);
            }
        }
    }
}
