using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebFactorOriginTest : TestBase
    {
        private WebFactorOrigin _factorOrigin;

        public WebFactorOriginTest()
        {
            _factorOrigin = null;
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


        public WebFactorOrigin GetFactorOrigin()
        {
            if (_factorOrigin.IsNull())
            {
                _factorOrigin = FactorManagerTest.GetOneFactorOrigin(GetContext());
            }
            return _factorOrigin;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;
            id = 2;

            GetFactorOrigin().Id = id;
            Assert.AreEqual(GetFactorOrigin().Id, id);

        }
        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetFactorOrigin().Name = name;
            Assert.AreEqual(GetFactorOrigin().Name, name);

            name = "";
            GetFactorOrigin().Name = name;
            Assert.AreEqual(GetFactorOrigin().Name, name);

            name = "Test factor origin name";
            GetFactorOrigin().Name = name;
            Assert.AreEqual(GetFactorOrigin().Name, name);
        }
        [TestMethod]
        public void Definition()
        {
            String definition;

            definition = null;
            GetFactorOrigin().Definition = definition;
            Assert.AreEqual(GetFactorOrigin().Definition, definition);

            definition = "";
            GetFactorOrigin().Definition = definition;
            Assert.AreEqual(GetFactorOrigin().Definition, definition);

            definition = "Test factor origin definition";
            GetFactorOrigin().Definition = definition;
            Assert.AreEqual(GetFactorOrigin().Definition, definition);
        }
        [TestMethod]
        public void SortOrder()
        {
            Int32 sortOrder;

            sortOrder = 11;
            GetFactorOrigin().SortOrder = sortOrder;
            Assert.AreEqual(GetFactorOrigin().SortOrder, sortOrder);

        }
    }
}

