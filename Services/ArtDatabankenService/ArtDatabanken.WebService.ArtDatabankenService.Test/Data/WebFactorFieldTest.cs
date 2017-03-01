using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebFactorFieldTest : TestBase
    {
        private WebFactorField _factorField;

        public WebFactorFieldTest()
        {
            _factorField = null;
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

        public WebFactorField GetFactorField()
        {
            if (_factorField.IsNull())
            {
                _factorField = FactorManagerTest.GetOneFactorField(GetContext());

            }
            return _factorField;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetFactorField().Id = id;
            Assert.AreEqual(GetFactorField().Id, id);
        }

        [TestMethod]
        public void DatabaseFieldName()
        {
            String testString;

            testString = null;
            GetFactorField().DatabaseFieldName = testString;
            Assert.IsNull(GetFactorField().DatabaseFieldName);
            testString = "";
            GetFactorField().DatabaseFieldName = testString;
            Assert.AreEqual(GetFactorField().DatabaseFieldName, testString);
            testString = "Test DatabaseFieldName of Factor field";
            GetFactorField().DatabaseFieldName = testString;
            Assert.AreEqual(GetFactorField().DatabaseFieldName, testString);
        }

        [TestMethod]
        public void Label()
        {
            String testString;

            testString = null;
            GetFactorField().Label = testString;
            Assert.IsNull(GetFactorField().Label);
            testString = "";
            GetFactorField().Label = testString;
            Assert.AreEqual(GetFactorField().Label, testString);
            testString = "Test Label of Factor field";
            GetFactorField().Label = testString;
            Assert.AreEqual(GetFactorField().Label, testString);
        }

        [TestMethod]
        public void Information()
        {
            String testString;

            testString = null;
            GetFactorField().Information = testString;
            Assert.IsNull(GetFactorField().Information);
            testString = "";
            GetFactorField().Information = testString;
            Assert.AreEqual(GetFactorField().Information, testString);
            testString = "Test Information of Factor field";
            GetFactorField().Information = testString;
            Assert.AreEqual(GetFactorField().Information, testString);
        }

        [TestMethod]
        public void UnitLabel()
        {
            String testString;

            testString = null;
            GetFactorField().UnitLabel = testString;
            Assert.IsNull(GetFactorField().UnitLabel);
            testString = "";
            GetFactorField().UnitLabel = testString;
            Assert.AreEqual(GetFactorField().UnitLabel, testString);
            testString = "Test UnitLabel of Factor field";
            GetFactorField().UnitLabel = testString;
            Assert.AreEqual(GetFactorField().UnitLabel, testString);
        }

        [TestMethod]
        public void IsMain()
        {
            Boolean testValue;

            testValue = false;
            GetFactorField().IsMain = testValue;
            Assert.AreEqual(GetFactorField().IsMain, testValue);
            testValue = true;
            Assert.AreNotEqual(GetFactorField().IsMain, testValue);
        }

        [TestMethod]
        public void IsSubstantial()
        {
            Boolean testValue;

            testValue = false;
            GetFactorField().IsSubstantial = testValue;
            Assert.AreEqual(GetFactorField().IsSubstantial, testValue);
            testValue = true;
            Assert.AreNotEqual(GetFactorField().IsSubstantial, testValue);
        }

        [TestMethod]
        public void TypeId()
        {
            Int32 id;

            id = 42;
            GetFactorField().TypeId = id;
            Assert.AreEqual(GetFactorField().TypeId, id);
        }

        [TestMethod]
        public void FactorfieldEnumId()
        {
            Int32 id;

            id = 42;
            GetFactorField().FactorFieldEnumId = id;
            Assert.AreEqual(GetFactorField().FactorFieldEnumId, id);
        }

    }
}
