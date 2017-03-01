using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for SpeciesFactQualityTest
    /// </summary>
    [TestClass]
    public class SpeciesFactQualityTest : TestBase
    {
        public SpeciesFactQualityTest()
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

        private Data.ArtDatabankenService.SpeciesFactQuality GetSpeciesFactQuality()
        {
            return SpeciesFactManagerTest.GetFirstSpeciesFactQuality();
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetSpeciesFactQuality().Name.IsNotEmpty());
        }

        [TestMethod]
        public void Definition()
        {
            Assert.IsTrue(GetSpeciesFactQuality().Definition.IsNotEmpty());
        }

        [TestMethod]
        public void SortOrder()
        {
            Assert.AreEqual(GetSpeciesFactQuality().SortOrder, GetSpeciesFactQuality().Id);
        }

    }
}
