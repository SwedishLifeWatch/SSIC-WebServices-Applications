using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class TaxonNameSearchCriteriaTest : TestBase
    {
        private ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria _searchCriteria;

        public TaxonNameSearchCriteriaTest()
        {
            _searchCriteria = null;
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

        [TestMethod]
        public void CheckData()
        {
            String nameSearchString;

            nameSearchString = "björn";
            GetSearchCriteria(true).NameSearchString = nameSearchString;
            GetSearchCriteria().CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataNameSearchStringEmptyError()
        {
            String nameSearchString;

            nameSearchString = String.Empty;
            GetSearchCriteria(true).NameSearchString = nameSearchString;
            GetSearchCriteria().CheckData();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckDataNameSearchStringNullError()
        {
            String nameSearchString;

            nameSearchString = null;
            GetSearchCriteria(true).NameSearchString = nameSearchString;
            GetSearchCriteria().CheckData();
        }

        private ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new ArtDatabanken.Data.ArtDatabankenService.TaxonNameSearchCriteria();
            }
            return _searchCriteria;
        }

        [TestMethod]
        public void NameSearchString()
        {
            String nameSearchString;

            nameSearchString = null;
            GetSearchCriteria(true).NameSearchString = nameSearchString;
            Assert.IsNull(GetSearchCriteria().NameSearchString);

            nameSearchString = String.Empty;
            GetSearchCriteria().NameSearchString = nameSearchString;
            Assert.AreEqual(GetSearchCriteria().NameSearchString, nameSearchString);

            nameSearchString = "björn";
            GetSearchCriteria().NameSearchString = nameSearchString;
            Assert.AreEqual(GetSearchCriteria().NameSearchString, nameSearchString);
        }
    }
}
