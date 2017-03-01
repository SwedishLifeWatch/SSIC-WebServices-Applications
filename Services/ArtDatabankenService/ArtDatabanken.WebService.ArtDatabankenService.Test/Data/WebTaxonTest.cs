using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebTaxonTest : TestBase
    {
        private WebTaxon _taxon;

        public WebTaxonTest()
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
        public void CommonName()
        {
            String commonName;

            commonName = null;
            GetTaxon().CommonName = commonName;
            Assert.IsNull(GetTaxon().CommonName);
            commonName = "";
            GetTaxon().CommonName = commonName;
            Assert.AreEqual(GetTaxon().CommonName, commonName);
            commonName = "Test taxon common name";
            GetTaxon().CommonName = commonName;
            Assert.AreEqual(GetTaxon().CommonName, commonName);
        }

        public WebTaxon GetTaxon()
        {
            if (_taxon.IsNull())
            {
                _taxon = TaxonManagerTest.GetOneTaxon(GetContext());
            }
            return _taxon;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetTaxon().Id = id;
            Assert.AreEqual(GetTaxon().Id, id);
        }

        [TestMethod]
        public void ScientificName()
        {
            String scientificName;

            scientificName = null;
            GetTaxon().ScientificName = scientificName;
            Assert.IsNull(GetTaxon().ScientificName);
            scientificName = "";
            GetTaxon().ScientificName = scientificName;
            Assert.AreEqual(GetTaxon().ScientificName, scientificName);
            scientificName = "Test taxon scientific name";
            GetTaxon().ScientificName = scientificName;
            Assert.AreEqual(GetTaxon().ScientificName, scientificName);
        }

        [TestMethod]
        public void SortOrder()
        {
            Int32 sortOrder;

            sortOrder = 42;
            GetTaxon().SortOrder = sortOrder;
            Assert.AreEqual(GetTaxon().SortOrder, sortOrder);
        }

        [TestMethod]
        public void TaxonInformationType()
        {
            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                GetTaxon().TaxonInformationType = taxonInformationType;
                Assert.AreEqual(GetTaxon().TaxonInformationType, taxonInformationType);
            }
        }

        [TestMethod]
        public void TaxonTypeId()
        {
            Int32 taxonTypeId;

            taxonTypeId = 42;
            GetTaxon().TaxonTypeId = taxonTypeId;
            Assert.AreEqual(GetTaxon().TaxonTypeId, taxonTypeId);
        }
    }
}
