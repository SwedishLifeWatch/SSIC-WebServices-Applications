using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken;

namespace ArtDatabanken.Test
{
    [TestClass]
    public class StringExtensionTest
    {
        public StringExtensionTest()
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
        public void CheckEmpty()
        {
            String text;

            text = null;
            text.CheckEmpty("text");
            text = String.Empty;
            text.CheckEmpty("text");
            text = " ";
            text.CheckEmpty("text");
            text = "    ";
            text.CheckEmpty("text");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckEmptyError()
        {
            String text;

            text = "Hej";
            text.CheckEmpty("text");
        }

        [TestMethod]
        public void CheckNotEmpty()
        {
            String text;

            text = "Hej";
            text.CheckNotEmpty("text");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNotEmptyEmptyError()
        {
            String text;

            text = String.Empty;
            text.CheckNotEmpty("text");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNotEmptyNullError()
        {
            String text;

            text = null;
            text.CheckNotEmpty("text");
        }

        [TestMethod]
        public void IsEmpty()
        {
            String text;

            text = null;
            Assert.IsTrue(text.IsEmpty());
            text = String.Empty;
            Assert.IsTrue(text.IsEmpty());
            text = "Hej";
            Assert.IsFalse(text.IsEmpty());
        }

        [TestMethod]
        public void IsNotEmpty()
        {
            String text;

            text = null;
            Assert.IsFalse(text.IsNotEmpty());
            text = String.Empty;
            Assert.IsFalse(text.IsNotEmpty());
            text = "Hej";
            Assert.IsTrue(text.IsNotEmpty());
        }

        [TestMethod]
        public void IsValidEmail()
        {
            String emailAddress;

            emailAddress = String.Empty;
            Assert.IsFalse(emailAddress.IsValidEmail());
            emailAddress = "test@slu.se";
            Assert.IsTrue(emailAddress.IsValidEmail());
            emailAddress = "test@slu.se.com";
            Assert.IsTrue(emailAddress.IsValidEmail());
            emailAddress = ".test@slu.se";
            Assert.IsFalse(emailAddress.IsValidEmail());
            emailAddress = "test.@slu.se";
            Assert.IsFalse(emailAddress.IsValidEmail());
            emailAddress = "test.ABCD.abc@slu.se";
            Assert.IsTrue(emailAddress.IsValidEmail());
            emailAddress = "test_ABCD3@slu.se";
            Assert.IsTrue(emailAddress.IsValidEmail());
            emailAddress = "test%@slu.se";
            Assert.IsFalse(emailAddress.IsValidEmail());
        }

        [TestMethod]
        public void ToStringWithDefaultValue()
        {
            String str;
            String stringValue;

            str = null;
            stringValue = str.ToString("String is empty");
            Assert.AreEqual("String is empty", stringValue);
            str = "  ";
            stringValue = str.ToString("String is empty", false);
            Assert.AreEqual("  ", stringValue);
            str = "  ";
            stringValue = str.ToString("String is empty", true);
            Assert.AreEqual("String is empty", stringValue);
            str = "Hello!";
            stringValue = str.ToString("String is empty", false);
            Assert.AreEqual("Hello!", stringValue);
            str = "Hello!";
            stringValue = str.ToString("String is empty", true);
            Assert.AreEqual("Hello!", stringValue);
        }


        [TestMethod]
        public void WebParseBoolean()
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
        public void WebParseDateTime()
        {
            DateTime value;
            String stringValue;

            value = DateTime.Now;
            stringValue = value.WebToString();
            Assert.AreEqual(value, stringValue.WebParseDateTime());
        }

        [TestMethod]
        public void WebParseDouble()
        {
            Double value;
            String stringValue;

            for (value = 2; value < 1000; value *= Math.PI)
            {
                stringValue = value.WebToString();
                Assert.IsTrue(Math.Abs(value - stringValue.WebParseDouble()) < (value / 1000000));
            }
        }

        [TestMethod]
        public void WebParseInt32()
        {
            Int32 value;
            String stringValue;

            for (value = 2; value < 1000; value *= 3)
            {
                stringValue = value.WebToString();
                Assert.AreEqual(value, stringValue.WebParseInt32());
            }
        }

        [TestMethod]
        public void WebParseInt64()
        {
            Int64 value;
            String stringValue;

            for (value = 2; value < 100000000000; value *= 300000000)
            {
                stringValue = value.WebToString();
                Assert.AreEqual(value, stringValue.WebParseInt64());
            }
        }
    }
}
