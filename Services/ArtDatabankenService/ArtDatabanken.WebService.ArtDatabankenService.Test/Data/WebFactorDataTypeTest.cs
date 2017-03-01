using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebFactorDataTypeTest : TestBase
    {
        private WebFactorDataType _factorDataType;

        public WebFactorDataTypeTest()
        {
            _factorDataType = null;
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

        public WebFactorDataType GetFactorDataType()
        {
            if (_factorDataType.IsNull())
            {
                _factorDataType = FactorManagerTest.GetOneFactorDataType(GetContext());

            }
            return _factorDataType;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetFactorDataType().Id = id;
            Assert.AreEqual(GetFactorDataType().Id, id);
        }

        [TestMethod]
        public void Name()
        {
            String testString;

            testString = null;
            GetFactorDataType().Name = testString;
            Assert.IsNull(GetFactorDataType().Name);
            testString = "";
            GetFactorDataType().Name = testString;
            Assert.AreEqual(GetFactorDataType().Name, testString);
            testString = "Test Name of Factor Data Type";
            GetFactorDataType().Name = testString;
            Assert.AreEqual(GetFactorDataType().Name, testString);
        }

        [TestMethod]
        public void Definition()
        {
            String testString;

            testString = null;
            GetFactorDataType().Definition = testString;
            Assert.IsNull(GetFactorDataType().Definition);
            testString = "";
            GetFactorDataType().Definition = testString;
            Assert.AreEqual(GetFactorDataType().Definition, testString);
            testString = "Test Definition of Factor Data Type";
            GetFactorDataType().Definition = testString;
            Assert.AreEqual(GetFactorDataType().Definition, testString);
        }
    }
}
