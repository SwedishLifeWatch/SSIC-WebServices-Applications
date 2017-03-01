using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken;

namespace ArtDatabanken.Test
{
    [TestClass]
    public class ObjectExtensionTest
    {
        public ObjectExtensionTest()
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
        public void CheckNotNull()
        {
            ArrayList list;

            list = new ArrayList();
            list.CheckNotNull("list");
            list.Add("Hej");
            list.CheckNotNull("list");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckNotNullError()
        {
            ArrayList list;

            list = null;
            list.CheckNotNull("list");
        }

        [TestMethod]
        public void CheckNull()
        {
            ArrayList list;

            list = null;
            list.CheckNull("list");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNullError()
        {
            ArrayList list;

            list = new ArrayList();
            list.CheckNull("list");
        }

        [TestMethod]
        public void IsNotNull()
        {
            String text;

            text = null;
            Assert.IsFalse(text.IsNotNull());
            text = "Hej";
            Assert.IsTrue(text.IsNotNull());
        }

        [TestMethod]
        public void IsNull()
        {
            String text;

            text = null;
            Assert.IsTrue(text.IsNull());
            text = "Hej";
            Assert.IsFalse(text.IsNull());
        }
    }
}
