using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebReferenceTest : TestBase
    {
        private WebReference _reference;

        public WebReferenceTest()
        {
            _reference = null;
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


        [TestMethod]
        public void GetNameMaxLength()
        {
            Int32 nameMaxLength;

            nameMaxLength = WebReference.GetNameMaxLength(GetContext());
            Assert.IsTrue(0 < nameMaxLength);
        }

        public WebReference GetReference()
        {
            if (_reference.IsNull())
            {
                _reference = ReferenceManagerTest.GetOneReference(GetContext());
            }
            return _reference;
        }

        [TestMethod]
        public void GetTextMaxLength()
        {
            Int32 textMaxLength;

            textMaxLength = WebReference.GetTextMaxLength(GetContext());
            Assert.IsTrue(0 < textMaxLength);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;
            id = 2;

            GetReference().Id = id;
            Assert.AreEqual(GetReference().Id, id);

        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetReference().Name = name;
            Assert.AreEqual(GetReference().Name, name);
 
            name = "";
            GetReference().Name = name; 
            Assert.AreEqual(GetReference().Name, name);

            name = "Test reference name";
            GetReference().Name = name;
            Assert.AreEqual(GetReference().Name, name);
        }

        [TestMethod]
        public void Year()
        {
            Int32 year;

            year = 1;
            GetReference().Year = year;
            Assert.AreEqual(GetReference().Year, year);

        }

        [TestMethod]
        public void Text()
        {
            String text;

            text = null;
            GetReference().Text = text;
            Assert.AreEqual(GetReference().Text, text);

            text = "";
            GetReference().Text = text;
            Assert.AreEqual(GetReference().Text, text);

            text = "Test reference text";
            GetReference().Text = text;
            Assert.AreEqual(GetReference().Text, text);
        }
    }
}
