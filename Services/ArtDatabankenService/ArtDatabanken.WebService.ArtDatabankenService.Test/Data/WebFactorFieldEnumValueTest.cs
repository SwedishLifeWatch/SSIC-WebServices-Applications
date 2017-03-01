using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebFactorFieldEnumValueTest : TestBase
    {
        private WebFactorFieldEnumValue _factorFieldEnumValue;

        public WebFactorFieldEnumValueTest()
        {
            _factorFieldEnumValue = null;
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

        public WebFactorFieldEnumValue GetFactorFieldEnumValue()
        {
            if (_factorFieldEnumValue.IsNull())
            {
                _factorFieldEnumValue = FactorManagerTest.GetOneFactorFieldEnumValue(GetContext());

            }
            return _factorFieldEnumValue;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetFactorFieldEnumValue().Id = id;
            Assert.AreEqual(GetFactorFieldEnumValue().Id, id);
        }

        [TestMethod]
        public void FactorFieldEnumId()
        {
            Int32 id;

            id = 42;
            GetFactorFieldEnumValue().FactorFieldEnumId = id;
            Assert.AreEqual(GetFactorFieldEnumValue().FactorFieldEnumId, id);
        }

        [TestMethod]
        public void KeyText()
        {
            String keyText;

            keyText = null;
            GetFactorFieldEnumValue().KeyText = keyText;
            Assert.IsNull(GetFactorFieldEnumValue().KeyText);
            keyText = "";
            GetFactorFieldEnumValue().KeyText = keyText;
            Assert.AreEqual(GetFactorFieldEnumValue().KeyText, keyText);
            keyText = "Test KeyText of factor field enum value";
            GetFactorFieldEnumValue().KeyText = keyText;
            Assert.AreEqual(GetFactorFieldEnumValue().KeyText, keyText);
        }

        [TestMethod]
        public void KeyInt()
        {
            Int32 keyInt;

            keyInt = 42;
            GetFactorFieldEnumValue().KeyInteger = keyInt;
            Assert.AreEqual(GetFactorFieldEnumValue().KeyInteger, keyInt);
        }

        [TestMethod]
        public void SortOrder()
        {
            Int32 sortOrder;

            sortOrder = 42;
            GetFactorFieldEnumValue().SortOrder = sortOrder;
            Assert.AreEqual(GetFactorFieldEnumValue().SortOrder, sortOrder);
        }

        [TestMethod]
        public void Label()
        {
            String label;

            label = null;
            GetFactorFieldEnumValue().Label = label;
            Assert.IsNull(GetFactorFieldEnumValue().Label);
            label = "";
            GetFactorFieldEnumValue().Label = label;
            Assert.AreEqual(GetFactorFieldEnumValue().Label, label);
            label = "Test information of factor field enum value";
            GetFactorFieldEnumValue().Label = label;
            Assert.AreEqual(GetFactorFieldEnumValue().Label, label);
        }

        [TestMethod]
        public void Information()
        {
            String information;

            information = null;
            GetFactorFieldEnumValue().Information = information;
            Assert.IsNull(GetFactorFieldEnumValue().Information);
            information = "";
            GetFactorFieldEnumValue().Information = information;
            Assert.AreEqual(GetFactorFieldEnumValue().Information, information);
            information = "Test information of factor field enum value";
            GetFactorFieldEnumValue().Information = information;
            Assert.AreEqual(GetFactorFieldEnumValue().Information, information);
        }


    }
}