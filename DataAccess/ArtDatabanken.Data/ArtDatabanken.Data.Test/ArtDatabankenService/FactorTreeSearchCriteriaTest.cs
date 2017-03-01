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
    /// Summary description for FactorTreeSearchCriteriaTest
    /// </summary>
    [TestClass]
    public class FactorTreeSearchCriteriaTest : TestBase
    {
        private Data.ArtDatabankenService.FactorTreeSearchCriteria _searchCriteria;

        public FactorTreeSearchCriteriaTest()
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
            GetSearchCriteria(true).CheckData();
        }

        private Data.ArtDatabankenService.FactorTreeSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private Data.ArtDatabankenService.FactorTreeSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new Data.ArtDatabankenService.FactorTreeSearchCriteria();
            }
            return _searchCriteria;
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
