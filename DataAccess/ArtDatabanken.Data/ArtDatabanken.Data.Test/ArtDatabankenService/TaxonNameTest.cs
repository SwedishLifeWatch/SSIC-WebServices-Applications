using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonNameTest : TestBase
    {
        private TaxonName _taxoName;

        public TaxonNameTest()
        {
            _taxoName = null;
        }

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

        #region Additional test attributes
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

        private TaxonName GetTaxonName()
        {
            return GetTaxonName(false);
        }

        private TaxonName GetTaxonName(Boolean refresh)
        {
            if (_taxoName.IsNull() || refresh)
            {
                _taxoName = TaxonManagerTest.GetTaxonName();
            }
            return _taxoName;
        }

        [TestMethod]
        public void Author()
        {
            String author;

            author = GetTaxonName().Author;
        }

        [TestMethod]
        public void IsDummyTaxonName()
        {
            TaxonPrintObs taxon;

            taxon = (TaxonPrintObs)TaxonManager.GetTaxon(BEAR_TAXON_ID, ArtDatabanken.Data.WebService.TaxonInformationType.PrintObs);
            Assert.IsFalse(TaxonName.IsDummyTaxonName(taxon.CommonName));
            taxon = (TaxonPrintObs)TaxonManager.GetTaxon(LEPTOCHITON_ALVEOLUS_TAXON_ID, ArtDatabanken.Data.WebService.TaxonInformationType.PrintObs);
            Assert.IsTrue(TaxonName.IsDummyTaxonName(taxon.OrderName));
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetTaxonName().Name.IsNotEmpty());
        }

        [TestMethod]
        public void Taxon()
        {
            Assert.IsNotNull(GetTaxonName().Taxon);
            Assert.AreEqual(GetTaxonName().Taxon.Id, GetTaxonName().TaxonId);
        }

        [TestMethod]
        public void TaxonId()
        {
            Assert.IsTrue(GetTaxonName().TaxonId >= 0);
        }

        [TestMethod]
        public void TaxonNameType()
        {
            Assert.IsNotNull(GetTaxonName().TaxonNameType);
        }

        [TestMethod]
        public void TaxonNameUseType()
        {
            Assert.IsNotNull(GetTaxonName().TaxonNameUseType);
        }
    }
}