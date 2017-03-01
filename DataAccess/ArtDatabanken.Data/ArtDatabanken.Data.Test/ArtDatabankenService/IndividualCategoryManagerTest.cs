using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class IndividualCategoryManagerTest : TestBase
    {
        public IndividualCategoryManagerTest()
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
        public void GetIndividualCategory()
        {
            Data.ArtDatabankenService.IndividualCategory individualCategory;

            // Get factor update mode type by Int32 id.
            {
                Int32 individualCategoryId;

                individualCategoryId = 0;
                individualCategory = IndividualCategoryManager.GetIndividualCategory(individualCategoryId);
                Assert.IsNotNull(individualCategory);
                Assert.AreEqual(individualCategoryId, individualCategory.Id);


            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIndividualCategoryIdError()
        {
            Int32 individualCategoryId;

            individualCategoryId = Int32.MinValue;
            IndividualCategoryManager.GetIndividualCategory(individualCategoryId);
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            Data.ArtDatabankenService.IndividualCategoryList individualCategories;

            individualCategories = IndividualCategoryManager.GetIndividualCategories();
            Assert.IsNotNull(individualCategories);
            Assert.IsTrue(individualCategories.IsNotEmpty());
        }

        public static Data.ArtDatabankenService.IndividualCategory GetOneIndividualCategory()
        {
            return IndividualCategoryManager.GetIndividualCategories()[0];
        }

        public static Data.ArtDatabankenService.IndividualCategoryList GetSomeIndividualCategories()
        {
            Data.ArtDatabankenService.IndividualCategoryList individualCategories;

            individualCategories = new Data.ArtDatabankenService.IndividualCategoryList();
            individualCategories.AddRange(IndividualCategoryManager.GetIndividualCategories().GetRange(2, 8));
            return individualCategories;
        }
    }
}
