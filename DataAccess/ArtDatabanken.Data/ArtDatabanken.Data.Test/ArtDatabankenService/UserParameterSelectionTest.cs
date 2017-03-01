using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for UserParameterSelectionTest
    /// </summary>
    [TestClass]
    public class UserParameterSelectionTest : TestBase
    {
        public UserParameterSelectionTest()
        {

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

        public static UserParameterSelection GetUserParameterSelection()
        {
            UserParameterSelection userParameterSelection = new UserParameterSelection();
            return userParameterSelection;
        }


        [TestMethod]
        public void Taxa()
        {
            UserParameterSelection userParameterSelection = GetUserParameterSelection();
            Assert.AreEqual(userParameterSelection.Taxa.IsNotEmpty(), userParameterSelection.HasTaxa);

            userParameterSelection.Taxa.Merge(TaxonManagerTest.GetTaxaList());
            Assert.IsTrue(userParameterSelection.Taxa.IsNotEmpty());
            Assert.IsTrue(userParameterSelection.HasTaxa);
        }

        [TestMethod]
        public void Factors()
        {
            UserParameterSelection userParameterSelection = GetUserParameterSelection();
            Assert.AreEqual(userParameterSelection.Factors.IsNotEmpty(), userParameterSelection.HasFactors);

            userParameterSelection.Factors.Merge(ArtDatabanken.Data.ArtDatabankenService.FactorManager.GetFactors());
            Assert.IsTrue(userParameterSelection.Factors.IsNotEmpty());
            Assert.IsTrue(userParameterSelection.Factors.Count > 1500);
            Assert.IsTrue(userParameterSelection.HasFactors);
        }

        [TestMethod]
        public void IndividualCategories()
        {
            UserParameterSelection userParameterSelection = GetUserParameterSelection();
            Assert.AreEqual(userParameterSelection.IndividualCategories.IsNotEmpty(), userParameterSelection.HasIndividualCategories);

            userParameterSelection.IndividualCategories.Merge(IndividualCategoryManager.GetIndividualCategories());
            Assert.IsTrue(userParameterSelection.IndividualCategories.IsNotEmpty());
            Assert.IsTrue(userParameterSelection.IndividualCategories.Count > 15);
            Assert.IsTrue(userParameterSelection.HasIndividualCategories);
        }

        [TestMethod]
        public void Periods()
        {
            UserParameterSelection userParameterSelection = GetUserParameterSelection();
            Assert.AreEqual(userParameterSelection.Periods.IsNotEmpty(), userParameterSelection.HasPeriods);

            userParameterSelection.Periods.Merge(PeriodManager.GetPeriods());
            Assert.IsTrue(userParameterSelection.Periods.IsNotEmpty());
            Assert.IsTrue(userParameterSelection.Periods.Count > 2);
            Assert.IsTrue(userParameterSelection.HasPeriods);
        }

        [TestMethod]
        public void Hosts()
        {
            UserParameterSelection userParameterSelection = GetUserParameterSelection();
            Assert.AreEqual(userParameterSelection.Hosts.IsNotEmpty(), userParameterSelection.HasHosts);

            userParameterSelection.Hosts.Merge(TaxonManagerTest.GetTaxaList());
            Assert.IsTrue(userParameterSelection.Hosts.IsNotEmpty());
            Assert.IsTrue(userParameterSelection.HasHosts);
        }

        [TestMethod]
        public void References()
        {
            UserParameterSelection userParameterSelection = GetUserParameterSelection();
            Assert.AreEqual(userParameterSelection.References.IsNotEmpty(), userParameterSelection.HasReferences);

            userParameterSelection.References.Merge(ReferenceManagerTest.GetSomeReferences());
            Assert.IsTrue(userParameterSelection.References.IsNotEmpty());
            Assert.IsTrue(userParameterSelection.References.Count > 6);
            Assert.IsTrue(userParameterSelection.HasReferences);
        }

    }
}
