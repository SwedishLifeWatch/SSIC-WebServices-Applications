using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class FactorUpdateModeTest : TestBase
    {
        private Data.ArtDatabankenService.FactorUpdateMode _factorUpdateMode;

        public FactorUpdateModeTest()
        {
            _factorUpdateMode = null;
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
        public void AllowAutomaticUpdate()
        {
            Boolean allowUpdate;

            allowUpdate = GetFactorUpdateMode(true).AllowAutomaticUpdate;
        }

        [TestMethod]
        public void AllowManualUpdate()
        {
            Boolean allowUpdate;

            allowUpdate = GetFactorUpdateMode(true).AllowManualUpdate;
        }

        [TestMethod]
        public void AllowUpdate()
        {
            Boolean allowUpdate;

            allowUpdate = GetFactorUpdateMode(true).AllowUpdate;
        }

        [TestMethod]
        public void Definition()
        {
            Assert.IsTrue(GetFactorUpdateMode(true).Definition.IsNotEmpty());
        }

        private Data.ArtDatabankenService.FactorUpdateMode GetFactorUpdateMode()
        {
            return GetFactorUpdateMode(false);
        }

        private Data.ArtDatabankenService.FactorUpdateMode GetFactorUpdateMode(Boolean refresh)
        {
            if (_factorUpdateMode.IsNull() || refresh)
            {
                _factorUpdateMode = FactorManagerTest.GetHeaderFactorUpdateMode();
            }
            return _factorUpdateMode;
        }

        [TestMethod]
        public void IsHeader()
        {
            Boolean isHeader;

            isHeader = GetFactorUpdateMode(true).IsHeader;
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetFactorUpdateMode(true).Name.IsNotEmpty());
        }

        [TestMethod]
        public void SortOrder()
        {
            Assert.AreEqual(GetFactorUpdateMode(true).SortOrder, GetFactorUpdateMode().Id);
        }
    }
}