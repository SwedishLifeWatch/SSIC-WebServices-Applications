using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken;

namespace ArtDatabanken.Test
{
    [TestClass]
    public class ICollectionExtensionTest
    {
        public ICollectionExtensionTest()
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
            ArrayList collection;

            collection = null;
            collection.CheckEmpty("collection");
            collection = new ArrayList();
            collection.CheckEmpty("collection");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckEmptyError()
        {
            ArrayList collection;

            collection = new ArrayList();
            collection.Add("Hej");
            collection.CheckEmpty("collection");
        }

        [TestMethod]
        public void CheckNotEmpty()
        {
            ArrayList collection;

            collection = new ArrayList();
            collection.Add("Hej");
            collection.CheckNotEmpty("collection");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNotEmptyEmptyError()
        {
            ArrayList collection;

            collection = new ArrayList();
            collection.CheckNotEmpty("collection");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNotEmptyNullError()
        {
            ArrayList collection;

            collection = null;
            collection.CheckNotEmpty("collection");
        }

        [TestMethod]
        public void IsEmpty()
        {
            ArrayList list;

            list = null;
            Assert.IsTrue(list.IsEmpty());
            list = new ArrayList();
            Assert.IsTrue(list.IsEmpty());
            list.Add("Hej");
            Assert.IsFalse(list.IsEmpty());
        }

        [TestMethod]
        public void IsNotEmpty()
        {
            ArrayList list;

            list = null;
            Assert.IsFalse(list.IsNotEmpty());
            list = new ArrayList();
            Assert.IsFalse(list.IsNotEmpty());
            list.Add("Hej");
            Assert.IsTrue(list.IsNotEmpty());
        }
    }
}
