using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    /// <summary>
    /// Summary description for WebSpeciesFactQualityTest
    /// </summary>
    [TestClass]
    public class WebSpeciesFactQualityTest : TestBase
    {
        private WebSpeciesFactQuality _speciesFactQuality;

        public WebSpeciesFactQualityTest()
        {
            _speciesFactQuality = null;
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

        public WebSpeciesFactQuality GetSpeciesFactQuality()
        {
            if (_speciesFactQuality.IsNull())
            {
                _speciesFactQuality = SpeciesFactManagerTest.GetOneSpeciesFactQuality(GetContext());

            }
            return _speciesFactQuality;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetSpeciesFactQuality().Id = id;
            Assert.AreEqual(GetSpeciesFactQuality().Id, id);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetSpeciesFactQuality().Name = name;
            Assert.IsNull(GetSpeciesFactQuality().Name);
            name = "";
            GetSpeciesFactQuality().Name = name;
            Assert.AreEqual(GetSpeciesFactQuality().Name, name);
            name = "Test Name of the species fact quality";
            GetSpeciesFactQuality().Name = name;
            Assert.AreEqual(GetSpeciesFactQuality().Name, name);
        }

        [TestMethod]
        public void Definition()
        {
            String definition;

            definition = null;
            GetSpeciesFactQuality().Definition = definition;
            Assert.IsNull(GetSpeciesFactQuality().Definition);
            definition = "";
            GetSpeciesFactQuality().Definition = definition;
            Assert.AreEqual(GetSpeciesFactQuality().Definition, definition);
            definition = "Test definition of the species fact qualit";
            GetSpeciesFactQuality().Definition = definition;
            Assert.AreEqual(GetSpeciesFactQuality().Definition, definition);
        }

    }
}
