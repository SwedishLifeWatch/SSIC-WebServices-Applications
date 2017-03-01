using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    /// <summary>
    /// Summary description for WebFactorSearchCriteriaTest
    /// </summary>
    [TestClass]
    public class WebFactorSearchCriteriaTest : TestBase
    {
        private WebFactorSearchCriteria _searchCriteria;

        public WebFactorSearchCriteriaTest()
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
            String factorNameSearchString;

            factorNameSearchString = "Rödl%";
            GetSearchCriteria(true).NameSearchString = factorNameSearchString;
            GetSearchCriteria().CheckData();
        }

        private WebFactorSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private WebFactorSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new WebFactorSearchCriteria();
            }
            return _searchCriteria;
        }

        [TestMethod]
        public void RestrictReturnToScope()
        {
            foreach (FactorSearchScope factorReturnScope in Enum.GetValues(typeof(FactorSearchScope)))
            {
                GetSearchCriteria(true).RestrictReturnToScope = factorReturnScope;
                Assert.AreEqual(GetSearchCriteria().RestrictReturnToScope, factorReturnScope);
            }
        }

        [TestMethod]
        public void FactorNameSearchString()
        {
            String factorNameSearchString;

            factorNameSearchString = null;
            GetSearchCriteria(true).NameSearchString = factorNameSearchString;
            Assert.IsNull(GetSearchCriteria().NameSearchString);

            factorNameSearchString = "";
            GetSearchCriteria().NameSearchString = factorNameSearchString;
            Assert.AreEqual(GetSearchCriteria().NameSearchString, factorNameSearchString);

            factorNameSearchString = "Kriterium";
            GetSearchCriteria().NameSearchString = factorNameSearchString;
            Assert.AreEqual(GetSearchCriteria().NameSearchString, factorNameSearchString);
        }
    }
}
