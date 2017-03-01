using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebIndividualCategoryTest : TestBase
    {
        private WebIndividualCategory _individualcategory;
        public WebIndividualCategoryTest()
        {
            _individualcategory = null;
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


        public WebIndividualCategory GetIndividualCategory()
        {
            if (_individualcategory.IsNull())
            {
                _individualcategory = IndividualCategoryManagerTest.GetIndividualCategory(GetContext());
            }
            return _individualcategory;
        }

        [TestMethod]
         public void Id()
        {
            Int32 id;
            id = 2;

           GetIndividualCategory().Id = id;
           Assert.AreEqual(GetIndividualCategory().Id, id);

        }
        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetIndividualCategory().Name = name;
            Assert.AreEqual(GetIndividualCategory().Name, name);

            name = "";
            GetIndividualCategory().Name = name;
            Assert.AreEqual(GetIndividualCategory().Name, name);

            name = "Test individual category name";
            GetIndividualCategory().Name = name;
            Assert.AreEqual(GetIndividualCategory().Name, name);
        }
        [TestMethod]
        public void Definition()
        {
            String definition;

            definition = null;
            GetIndividualCategory().Definition = definition;
            Assert.AreEqual(GetIndividualCategory().Definition, definition);

            definition = "";
            GetIndividualCategory().Definition = definition;
            Assert.AreEqual(GetIndividualCategory().Definition, definition);

            definition = "Test IndiVidual Category definition";
            GetIndividualCategory().Definition = definition;
            Assert.AreEqual(GetIndividualCategory().Definition, definition);
        }
        [TestMethod]
        public void SortOrder()
        {
            Int32 sortorder;
            sortorder = 1;

            GetIndividualCategory().SortOrder = sortorder;
            Assert.AreEqual(GetIndividualCategory().SortOrder, sortorder);

        }
    }
}
