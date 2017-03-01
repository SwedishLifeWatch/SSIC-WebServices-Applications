using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class SpeciesObservationAreaTest : TestBase
    {
        private Data.ArtDatabankenService.SpeciesObservationArea _speciesObservationArea;

        public SpeciesObservationAreaTest()
        {
            _speciesObservationArea = null;
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
        public void AreaOfOccupancy()
        {
            Assert.IsTrue(0 <= GetSpeciesObservationArea(true).AreaOfOccupancy);
        }

        [TestMethod]
        public void Constructor()
        {
            Data.ArtDatabankenService.SpeciesObservationArea speciesObservationArea;

            speciesObservationArea = GetSpeciesObservationArea(true);
            Assert.IsNotNull(speciesObservationArea);
        }

        [TestMethod]
        public void ExtentOfOccurrence()
        {
            Assert.IsTrue(0 <= GetSpeciesObservationArea(true).ExtentOfOccurrence);
        }

        [TestMethod]
        public void GridSquareMaxDistance()
        {
            Assert.IsTrue(0 <= GetSpeciesObservationArea(true).GridSquareMaxDistance);
        }

        [TestMethod]
        public void GridSquareSize()
        {
            Assert.IsTrue(0 <= GetSpeciesObservationArea(true).GridSquareSize);
        }

        private Data.ArtDatabankenService.SpeciesObservationArea GetSpeciesObservationArea()
        {
            return GetSpeciesObservationArea(false);
        }

        private Data.ArtDatabankenService.SpeciesObservationArea GetSpeciesObservationArea(Boolean refresh)
        {
            if (_speciesObservationArea.IsNull() || refresh)
            {
                _speciesObservationArea = new Data.ArtDatabankenService.SpeciesObservationArea(TaxonManagerTest.GetOneTaxon(),
                                                                     543,
                                                                     2000,
                                                                     50000,
                                                                     53450000000,
                                                                     5435340000000000);
            }
            return _speciesObservationArea;
        }

        [TestMethod]
        public void SpeciesObservationCount()
        {
            Assert.IsTrue(0 <= GetSpeciesObservationArea(true).SpeciesObservationCount);
        }

        [TestMethod]
        public void Taxon()
        {
            Assert.IsNotNull(GetSpeciesObservationArea(true).Taxon);
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.IsTrue(GetSpeciesObservationArea(true).ToString().IsNotEmpty());
        }
    }
}
