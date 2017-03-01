using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for FactorSearchCriteriaTest
    /// </summary>
    [TestClass]
    public class FactorSearchCriteriaTest : TestBase
    {
        private Data.ArtDatabankenService.FactorSearchCriteria _searchCriteria;

        public FactorSearchCriteriaTest()
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

            factorNameSearchString = "Landskapstyp%";
            GetSearchCriteria(true).FactorNameSearchString = factorNameSearchString;
            GetSearchCriteria().CheckData();
        }

        private Data.ArtDatabankenService.FactorSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private Data.ArtDatabankenService.FactorSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new Data.ArtDatabankenService.FactorSearchCriteria();
            }
            return _searchCriteria;
        }

        [TestMethod]
        public void RestrictReturnToScope()
        {
            foreach (WebService.FactorSearchScope factorReturnScope in Enum.GetValues(typeof(WebService.FactorSearchScope)))
            {
                GetSearchCriteria(true).RestrictReturnToScope = factorReturnScope;
                Assert.AreEqual(GetSearchCriteria().RestrictReturnToScope, factorReturnScope);
            }
        }

        [TestMethod]
        public void RestrictSearchToFactorIds()
        {
            Int32 factorIdIndex;
            List<Int32> factorIds;

            factorIds = null;
            GetSearchCriteria(true).RestrictSearchToFactorIds = factorIds;
            Assert.IsNull(GetSearchCriteria().RestrictSearchToFactorIds);

            factorIds = new List<Int32>();
            GetSearchCriteria().RestrictSearchToFactorIds = factorIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToFactorIds, factorIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToFactorIds.Count, factorIds.Count);

            factorIds.Add(42);
            GetSearchCriteria().RestrictSearchToFactorIds = factorIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToFactorIds, factorIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToFactorIds.Count, factorIds.Count);
            for (factorIdIndex = 0; factorIdIndex < factorIds.Count; factorIdIndex++)
            {
                Assert.AreEqual(GetSearchCriteria().RestrictSearchToFactorIds[factorIdIndex], factorIds[factorIdIndex]);
            }

            factorIds.Add(11);
            GetSearchCriteria().RestrictSearchToFactorIds = factorIds;
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToFactorIds, factorIds);
            Assert.AreEqual(GetSearchCriteria().RestrictSearchToFactorIds.Count, factorIds.Count);
            for (factorIdIndex = 0; factorIdIndex < factorIds.Count; factorIdIndex++)
            {
                Assert.AreEqual(GetSearchCriteria().RestrictSearchToFactorIds[factorIdIndex], factorIds[factorIdIndex]);
            }
        }

    }
}
