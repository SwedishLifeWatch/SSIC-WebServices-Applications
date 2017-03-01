using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test
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
        public void CheckSqlInjection()
        {
            String text;

            text = null;
            Assert.IsNull(text.CheckSqlInjection());
            text = "";
            Assert.AreEqual(text, text.CheckSqlInjection());
            text = "Hej";
            Assert.AreEqual(text, text.CheckSqlInjection());
            text = "Hej'då";
            Assert.AreEqual("Hej''då", text.CheckSqlInjection());
        }

        [TestMethod]
        public void CheckLength()
        {
            Int32 maxLength;
            String text;

            text = null;
            maxLength = 10;
            text.CheckLength(maxLength);
            text = "";
            text.CheckLength(maxLength);
            text = "Hej";
            text.CheckLength(maxLength);
            text = "Hej'då";
            text.CheckLength(maxLength);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckLengthToLongError()
        {
            Int32 maxLength;
            String text;

            maxLength = 10;
            text = "Heeeeej'dååååååå";
            text.CheckLength(maxLength);
        }
    }
}
