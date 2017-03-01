using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken;

namespace ArtDatabanken.Test
{
    [TestClass]
    public class BooleanExtensionTest
    {
        public BooleanExtensionTest()
        {
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
        public void WebToString()
        {
            Boolean value;
            String stringValue;

            value = false;
            stringValue = value.WebToString();
            Assert.AreEqual(value, stringValue.WebParseBoolean());
            value = true;
            stringValue = value.WebToString();
            Assert.AreEqual(value, stringValue.WebParseBoolean());
        }

        [TestMethod]
        public void ToStringBooleanStringRepresentation()
        {
            Boolean value;
            String stringValue;

            value = false;
            stringValue = value.ToString("Sant", "Falskt");
            Assert.AreEqual("Falskt", stringValue);

            value = true;
            stringValue = value.ToString("Sant", "Falskt");
            Assert.AreEqual("Sant", stringValue);
        }

        [TestMethod]
        public void ToStringWithDefaultValue()
        {
            Boolean? value;
            String stringValue;

            value = false;
            stringValue = value.ToString("Sant", "Falskt", "N/A");
            Assert.AreEqual("Falskt", stringValue);

            value = true;
            stringValue = value.ToString("Sant", "Falskt", "N/A");
            Assert.AreEqual("Sant", stringValue);

            value = null;
            stringValue = value.ToString("Sant", "Falskt", "N/A");
            Assert.AreEqual("N/A", stringValue);
        }
    }
}
