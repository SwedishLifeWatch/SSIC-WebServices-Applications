using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebTaxonTypeTest : TestBase
    {
        private WebTaxonType _taxonType;

        public WebTaxonTypeTest()
        {
            _taxonType = null;
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

        public WebTaxonType GetTaxonType()
        {
            if (_taxonType.IsNull())
            {
                _taxonType = TaxonManagerTest.GetTaxonType(GetContext());
            }
            return _taxonType;
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetTaxonType().Name = name;
            Assert.IsNull(GetTaxonType().Name);
            name = "";
            GetTaxonType().Name = name;
            Assert.AreEqual(GetTaxonType().Name, name);
            name = "Test taxon type";
            GetTaxonType().Name = name;
            Assert.AreEqual(GetTaxonType().Name, name);
        }

        [TestMethod]
        public void SortOrder()
        {
            Int32 sortOrder;

            sortOrder = 42;
            GetTaxonType().SortOrder = sortOrder;
            Assert.AreEqual(GetTaxonType().SortOrder, sortOrder);
        }
    }
}
