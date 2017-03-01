using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class SpeciesObservationAreaListTest : TestBase
    {
        private Data.ArtDatabankenService.SpeciesObservationAreaList _speciesObservationAreas;

        public SpeciesObservationAreaListTest()
        {
            _speciesObservationAreas = null;
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

        private Data.ArtDatabankenService.SpeciesObservationAreaList GetSpeciesObservationAreas()
        {
            return GetSpeciesObservationAreas(false);
        }

        private Data.ArtDatabankenService.SpeciesObservationAreaList GetSpeciesObservationAreas(Boolean refresh)
        {
            if (_speciesObservationAreas.IsNull() || refresh)
            {
                _speciesObservationAreas = new Data.ArtDatabankenService.SpeciesObservationAreaList();
                foreach (ArtDatabanken.Data.ArtDatabankenService.Taxon taxon in TaxonManagerTest.GetTaxaList())
                {
                    _speciesObservationAreas.Add(new Data.ArtDatabankenService.SpeciesObservationArea(taxon,
                                                                            543,
                                                                            2000,
                                                                            50000,
                                                                            53450000000,
                                                                            5435340000000000));
                }
            }
            return _speciesObservationAreas;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 speciesObservationAreaIndex;
            Data.ArtDatabankenService.SpeciesObservationAreaList newSpeciesObservationAreaList, oldSpeciesObservationAreaList;

            oldSpeciesObservationAreaList = GetSpeciesObservationAreas(true);
            newSpeciesObservationAreaList = new Data.ArtDatabankenService.SpeciesObservationAreaList();
            for (speciesObservationAreaIndex = 0; speciesObservationAreaIndex < oldSpeciesObservationAreaList.Count; speciesObservationAreaIndex++)
            {
                newSpeciesObservationAreaList.Add(oldSpeciesObservationAreaList[oldSpeciesObservationAreaList.Count - speciesObservationAreaIndex - 1]);
            }
            for (speciesObservationAreaIndex = 0; speciesObservationAreaIndex < oldSpeciesObservationAreaList.Count; speciesObservationAreaIndex++)
            {
                Assert.AreEqual(newSpeciesObservationAreaList[speciesObservationAreaIndex], oldSpeciesObservationAreaList[oldSpeciesObservationAreaList.Count - speciesObservationAreaIndex - 1]);
            }
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.IsTrue(GetSpeciesObservationAreas(true).ToString().IsNotEmpty());
        }
    }
}
