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
    public class TaxonPrintObsTest : TestBase
    {
        private TaxonPrintObs _taxon;

        public TaxonPrintObsTest()
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
        public void ClassName()
        {
            Assert.IsTrue(GetTaxon(true).ClassName.IsNotEmpty());
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(GetTaxon(true));
        }

        [TestMethod]
        public void FamilyName()
        {
            Assert.IsTrue(GetTaxon(true).FamilyName.IsNotEmpty());
        }

        private TaxonPrintObs GetTaxon()
        {
            return GetTaxon(false);
        }

        private TaxonPrintObs GetTaxon(Boolean refresh)
        {
            if (_taxon.IsNull() || refresh)
            {
                _taxon = (TaxonPrintObs)(ArtDatabanken.Data.ArtDatabankenService.TaxonManager.GetTaxon(BEAR_TAXON_ID, TaxonInformationType.PrintObs));
            }
            return _taxon;
        }

        [TestMethod]
        public void OrderName()
        {
            Assert.IsTrue(GetTaxon(true).OrderName.IsNotEmpty());
        }

        [TestMethod]
        public void PhylumName()
        {
            Assert.IsTrue(GetTaxon(true).PhylumName.IsNotEmpty());
        }
    }
}
