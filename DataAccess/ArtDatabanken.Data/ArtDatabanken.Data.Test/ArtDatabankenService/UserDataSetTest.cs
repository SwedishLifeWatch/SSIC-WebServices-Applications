using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for UserDataSetTest
    /// </summary>
    [TestClass]
    public class UserDataSetTest : TestBase
    {
        private UserDataSet _userDataSet;

        public UserDataSetTest()
        {
            _userDataSet = null;
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

        private UserDataSet GetUserDataSet()
        {
            if (_userDataSet.IsNull())
            {
                _userDataSet = new UserDataSet();
                UserParameterSelection userParameterSelection = UserParameterSelectionTest.GetUserParameterSelection();
                userParameterSelection.Taxa.Merge(TaxonManagerTest.GetTaxaList());
                Data.ArtDatabankenService.FactorList factors = new Data.ArtDatabankenService.FactorList();
                factors.Add(ArtDatabanken.Data.ArtDatabankenService.FactorManager.GetFactor(LANDSCAPE_FACTOR_ID));
                userParameterSelection.Factors.Merge(factors);
                _userDataSet = Data.ArtDatabankenService.SpeciesFactManager.GetUserDataSetByParameterSelection(userParameterSelection);
            }

            return _userDataSet;
        }

        [TestMethod]
        public void Taxa()
        {
            UserDataSet userDataSet = GetUserDataSet();
            Assert.IsTrue(userDataSet.HasTaxa);
        }

        [TestMethod]
        public void Factors()
        {
            UserDataSet userDataSet = GetUserDataSet();
            Assert.IsTrue(userDataSet.HasFactors);
        }
    }
}
