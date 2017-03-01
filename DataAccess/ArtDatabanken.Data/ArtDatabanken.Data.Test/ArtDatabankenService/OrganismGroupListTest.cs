using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    using OrganismGroupList = Data.ArtDatabankenService.OrganismGroupList;

    [TestClass]
    public class OrganismGroupListTest : TestBase
    {
        private OrganismGroupList _organismGroups;

        public OrganismGroupListTest()
        {
            _organismGroups = null;
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
        public void Get()
        {
            foreach (OrganismGroup organismGroup in GetOrganismGroups(true))
            {
                Assert.AreEqual(organismGroup, GetOrganismGroups().Get(organismGroup.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 organismGroupId;

            organismGroupId = Int32.MinValue;
            GetOrganismGroups().Get(organismGroupId);
        }

        private OrganismGroupList GetOrganismGroups()
        {
            return GetOrganismGroups(false);
        }

        private OrganismGroupList GetOrganismGroups(Boolean refresh)
        {
            if (_organismGroups.IsNull() || refresh)
            {
                _organismGroups = SpeciesFactManagerTest.GetSomeOrganismGroups();
            }
            return _organismGroups;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 organismGroupIndex;
            OrganismGroupList organismGroups;

            GetOrganismGroups(true);
            organismGroups = new OrganismGroupList();
            for (organismGroupIndex = 0; organismGroupIndex < GetOrganismGroups().Count; organismGroupIndex++)
            {
                organismGroups.Add(GetOrganismGroups()[GetOrganismGroups().Count - organismGroupIndex - 1]);
            }
            for (organismGroupIndex = 0; organismGroupIndex < GetOrganismGroups().Count; organismGroupIndex++)
            {
                Assert.AreEqual(organismGroups[organismGroupIndex], GetOrganismGroups()[GetOrganismGroups().Count - organismGroupIndex - 1]);
            }
        }
    }
}
