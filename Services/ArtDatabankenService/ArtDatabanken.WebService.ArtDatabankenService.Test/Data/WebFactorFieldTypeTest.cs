using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebFactorFieldTypeTest : TestBase
    {
        private WebFactorFieldType _factorFieldType;

        public WebFactorFieldTypeTest()
        {
            _factorFieldType = null;
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

        public WebFactorFieldType GetFactorFieldType()
        {
            if (_factorFieldType.IsNull())
            {
                _factorFieldType = FactorManagerTest.GetOneFactorFieldType(GetContext());

            }
            return _factorFieldType;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetFactorFieldType().Id = id;
            Assert.AreEqual(GetFactorFieldType().Id, id);
        }

        [TestMethod]
        public void Name()
        {
            String testString;

            testString = null;
            GetFactorFieldType().Name = testString;
            Assert.IsNull(GetFactorFieldType().Name);
            testString = "";
            GetFactorFieldType().Name = testString;
            Assert.AreEqual(GetFactorFieldType().Name, testString);
            testString = "Test Name of Factor Field Type";
            GetFactorFieldType().Name = testString;
            Assert.AreEqual(GetFactorFieldType().Name, testString);
        }

        [TestMethod]
        public void Definition()
        {
            String testString;

            testString = null;
            GetFactorFieldType().Definition = testString;
            Assert.IsNull(GetFactorFieldType().Definition);
            testString = "";
            GetFactorFieldType().Definition = testString;
            Assert.AreEqual(GetFactorFieldType().Definition, testString);
            testString = "Test Definition of Factor Field Type";
            GetFactorFieldType().Definition = testString;
            Assert.AreEqual(GetFactorFieldType().Definition, testString);
        }
    }
}
