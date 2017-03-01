using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebTaxonNameTest : TestBase
    {
        private WebTaxonName _taxonName;

        public WebTaxonNameTest()
        {
            _taxonName = null;
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

        public WebTaxonName GetTaxonName()
        {
            if (_taxonName.IsNull())
            {
                _taxonName = TaxonManagerTest.GetTaxonName(GetContext());
            }
            return _taxonName;
        }

        [TestMethod]
        public void Author()
        {
            String author;

            author = null;
            GetTaxonName().Author = author;
            Assert.IsNull(GetTaxonName().Author);
            author = "";
            GetTaxonName().Author = author;
            Assert.AreEqual(GetTaxonName().Author, author);
            author = "Test taxon name author";
            GetTaxonName().Author = author;
            Assert.AreEqual(GetTaxonName().Author, author);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetTaxonName().Id = id;
            Assert.AreEqual(GetTaxonName().Id, id);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetTaxonName().Name = name;
            Assert.IsNull(GetTaxonName().Name);
            name = "";
            GetTaxonName().Name = name;
            Assert.AreEqual(GetTaxonName().Name, name);
            name = "Test taxon name";
            GetTaxonName().Name = name;
            Assert.AreEqual(GetTaxonName().Name, name);
        }

        [TestMethod]
        public void Taxon()
        {
            WebTaxon taxon;

            taxon = GetTaxonName().Taxon;
            Assert.IsNotNull(taxon);
        }

        [TestMethod]
        public void TaxonNameTypeId()
        {
            Int32 taxonNameTypeId;

            taxonNameTypeId = 42;
            GetTaxonName().TaxonNameTypeId = taxonNameTypeId;
            Assert.AreEqual(GetTaxonName().TaxonNameTypeId, taxonNameTypeId);
        }

        [TestMethod]
        public void TaxonNameUseTypeId()
        {
            Int32 taxonNameUseTypeId;

            taxonNameUseTypeId = 42;
            GetTaxonName().TaxonNameUseTypeId = taxonNameUseTypeId;
            Assert.AreEqual(GetTaxonName().TaxonNameUseTypeId, taxonNameUseTypeId);
        }
    }
}
