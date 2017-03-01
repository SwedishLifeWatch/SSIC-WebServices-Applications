using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebTaxonNameSearchCriteriaTest : TestBase
    {
        private WebTaxonNameSearchCriteria _taxonNameSearchCriteria;

        public WebTaxonNameSearchCriteriaTest()
        {
            _taxonNameSearchCriteria = null;
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
            String nameSearchString;

            nameSearchString = "björn";
            GetTaxonNameSearchCriteria(true).NameSearchString = nameSearchString;
            GetTaxonNameSearchCriteria().CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataNameSearchStringEmptyError()
        {
            String nameSearchString;

            nameSearchString = "";
            GetTaxonNameSearchCriteria(true).NameSearchString = nameSearchString;
            GetTaxonNameSearchCriteria().CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataNameSearchStringNullError()
        {
            String nameSearchString;

            nameSearchString = null;
            GetTaxonNameSearchCriteria(true).NameSearchString = nameSearchString;
            GetTaxonNameSearchCriteria().CheckData();
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(GetTaxonNameSearchCriteria(true));
        }

        public WebTaxonNameSearchCriteria GetTaxonNameSearchCriteria()
        {
            return GetTaxonNameSearchCriteria(false);
        }

        public WebTaxonNameSearchCriteria GetTaxonNameSearchCriteria(Boolean refresh)
        {
            if (_taxonNameSearchCriteria.IsNull() || refresh)
            {
                _taxonNameSearchCriteria = new WebTaxonNameSearchCriteria();
            }
            return _taxonNameSearchCriteria;
        }

        [TestMethod]
        public void NameSearchMethod()
        {
            GetTaxonNameSearchCriteria(true);
            foreach (SearchStringComparisonMethod nameSearchMethod in Enum.GetValues(typeof(SearchStringComparisonMethod)))
            {
                GetTaxonNameSearchCriteria().NameSearchMethod = nameSearchMethod;
                Assert.AreEqual(nameSearchMethod, GetTaxonNameSearchCriteria().NameSearchMethod);
            }
        }

        [TestMethod]
        public void NameSearchString()
        {
            String nameSearchString;

            nameSearchString = null;
            GetTaxonNameSearchCriteria(true).NameSearchString = nameSearchString;
            Assert.IsNull(GetTaxonNameSearchCriteria().NameSearchString);

            nameSearchString = "";
            GetTaxonNameSearchCriteria().NameSearchString = nameSearchString;
            Assert.AreEqual(GetTaxonNameSearchCriteria().NameSearchString, nameSearchString);

            nameSearchString = "björn";
            GetTaxonNameSearchCriteria().NameSearchString = nameSearchString;
            Assert.AreEqual(GetTaxonNameSearchCriteria().NameSearchString, nameSearchString);
        }
    }
}
