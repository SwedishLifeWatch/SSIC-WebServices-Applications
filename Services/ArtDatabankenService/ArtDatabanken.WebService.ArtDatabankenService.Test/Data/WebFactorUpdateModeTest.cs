using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebFactorUpdateModeTest : TestBase
    {
        private WebFactorUpdateMode _factorUpdateMode;

        public WebFactorUpdateModeTest()
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
        public void AllowUpdate()
        {
            Boolean allowUpdate;

            allowUpdate = true;
            GetFactorUpdateMode(true).IsUpdateAllowed = allowUpdate;
            Assert.AreEqual(GetFactorUpdateMode().IsUpdateAllowed, allowUpdate);

            allowUpdate = false;
            GetFactorUpdateMode().IsUpdateAllowed = allowUpdate;
            Assert.AreEqual(GetFactorUpdateMode().IsUpdateAllowed, allowUpdate);
        }

        [TestMethod]
        public void Definition()
        {
            String definition;

            definition = null;
            GetFactorUpdateMode(true).Definition = definition;
            Assert.IsNull(GetFactorUpdateMode().Definition);
            definition = "";
            GetFactorUpdateMode().Definition = definition;
            Assert.AreEqual(GetFactorUpdateMode().Definition, definition);
            definition = "Test definition of factor update mode";
            GetFactorUpdateMode().Definition = definition;
            Assert.AreEqual(GetFactorUpdateMode().Definition, definition);
        }

        private WebFactorUpdateMode GetFactorUpdateMode()
        {
            return GetFactorUpdateMode(false);
        }

        private WebFactorUpdateMode GetFactorUpdateMode(Boolean refresh)
        {
            if (_factorUpdateMode.IsNull() || refresh)
            {
                _factorUpdateMode = FactorManagerTest.GetOneFactorUpdateMode(GetContext());

            }
            return _factorUpdateMode;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetFactorUpdateMode(true).Id = id;
            Assert.AreEqual(GetFactorUpdateMode().Id, id);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetFactorUpdateMode(true).Name = name;
            Assert.IsNull(GetFactorUpdateMode().Name);
            name = "";
            GetFactorUpdateMode().Name = name;
            Assert.AreEqual(GetFactorUpdateMode().Name, name);
            name = "Test Name of factor update mode";
            GetFactorUpdateMode().Name = name;
            Assert.AreEqual(GetFactorUpdateMode().Name, name);
        }

        [TestMethod]
        public void Type()
        {
            GetFactorUpdateMode(true);
            foreach (FactorUpdateModeType type in Enum.GetValues(typeof(FactorUpdateModeType)))
            {
                GetFactorUpdateMode().Type = type;
                Assert.AreEqual(GetFactorUpdateMode().Type, type);
            }
        }
    }
}
