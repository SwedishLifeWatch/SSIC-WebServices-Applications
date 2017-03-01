using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    using OrganismGroup = Data.ArtDatabankenService.OrganismGroup;
    using OrganismGroupType = Data.ArtDatabankenService.OrganismGroupType;

    [TestClass]
    public class OrganismGroupTest : TestBase
    {
        private OrganismGroup _organismGroup;

        public OrganismGroupTest()
        {
            _organismGroup = null;
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
        public void Definition()
        {
            Assert.IsTrue(GetOrganismGroup(true).Definition.IsNotEmpty());
        }

        private OrganismGroup GetOrganismGroup()
        {
            return GetOrganismGroup(false);
        }

        private OrganismGroup GetOrganismGroup(Boolean refresh)
        {
            if (_organismGroup.IsNull() || refresh)
            {
                _organismGroup = SpeciesFactManagerTest.GetOneOrganismGroup();
            }
            return _organismGroup;
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetOrganismGroup(true).Name.IsNotEmpty());
        }

        [TestMethod]
        public void Type()
        {
            Boolean typeFound;

            typeFound = false;
            foreach (OrganismGroupType type in Enum.GetValues(typeof(OrganismGroupType)))
            {
                if (GetOrganismGroup().Type == type)
                {
                    typeFound = true;
                }
            }
            Assert.IsTrue(typeFound);
        }
    }
}
