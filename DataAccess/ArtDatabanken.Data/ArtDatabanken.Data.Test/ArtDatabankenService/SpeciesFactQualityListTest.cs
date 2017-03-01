using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    /// <summary>
    /// Summary description for SpeciesFactQualityListTest
    /// </summary>
    [TestClass]
    public class SpeciesFactQualityListTest : TestBase
    {
        private Data.ArtDatabankenService.SpeciesFactQualityList _speciesFactQualities;

        public SpeciesFactQualityListTest()
        {
            _speciesFactQualities = null;
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
        public void Get()
        {
            foreach (SpeciesFactQuality speciesFactQuality in GetSpeciesFactQualities())
            {
                Assert.AreEqual(speciesFactQuality, GetSpeciesFactQualities().Get(speciesFactQuality.Id));
            }
        }

        [TestMethod]
        public void GetSortOrder()
        {
            foreach (Data.ArtDatabankenService.SpeciesFactQuality speciesFactQuality in GetSpeciesFactQualities())
            {
                Assert.AreEqual(speciesFactQuality.Id, GetSpeciesFactQualities().Get(speciesFactQuality.Id).SortOrder);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 speciesFactQualityId;

            speciesFactQualityId = Int32.MinValue;
            GetSpeciesFactQualities().Get(speciesFactQualityId);
        }

        private Data.ArtDatabankenService.SpeciesFactQualityList GetSpeciesFactQualities()
        {
            if (_speciesFactQualities.IsNull())
            {
                _speciesFactQualities = Data.ArtDatabankenService.SpeciesFactManager.GetSpeciesFactQualities();
            }
            return _speciesFactQualities;
        }
    }
}
