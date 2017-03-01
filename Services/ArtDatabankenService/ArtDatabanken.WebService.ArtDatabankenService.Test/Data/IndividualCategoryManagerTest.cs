using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
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
        public void AddUserSelectedIndividualCategories()
        {
            foreach (UserSelectedIndividualCategoryUsage individualCategoryUsage in Enum.GetValues(typeof(UserSelectedIndividualCategoryUsage)))
            {
                IndividualCategoryManager.AddUserSelectedIndividualCategories(GetContext(), GetSomeIndividualCategoryIds(GetContext()), individualCategoryUsage);
                IndividualCategoryManager.DeleteUserSelectedIndividualCategories(GetContext());
            }
        }

        [TestMethod]
        public void DeleteUserSelectedIndividualCategories()
        {
            foreach (UserSelectedIndividualCategoryUsage individualCategoryUsage in Enum.GetValues(typeof(UserSelectedIndividualCategoryUsage)))
            {
                IndividualCategoryManager.AddUserSelectedIndividualCategories(GetContext(), GetSomeIndividualCategoryIds(GetContext()), individualCategoryUsage);
                IndividualCategoryManager.DeleteUserSelectedIndividualCategories(GetContext());
            }
        }

        public static WebIndividualCategory GetIndividualCategory(WebServiceContext context)
        {
            return IndividualCategoryManager.GetIndividualCategories(context)[0];
        }

        [TestMethod]
        public void GetIndividualCategories()
        {
            List<WebIndividualCategory> individualCategories;

            individualCategories = IndividualCategoryManager.GetIndividualCategories(GetContext());
            Assert.IsNotNull(individualCategories);
            Assert.IsTrue(individualCategories.IsNotEmpty());
        }

        public static List<Int32> GetSomeIndividualCategoryIds(WebServiceContext context)
        {
            return GetSomeIndividualCategoryIds(context, 2);
        }

        public static List<Int32> GetSomeIndividualCategoryIds(WebServiceContext context,
                                                               Int32 individualCategoryIdCount)
        {
            List<Int32> individualCategoryIds;

            individualCategoryIds = new List<Int32>();
            foreach (WebIndividualCategory individualCategory in IndividualCategoryManager.GetIndividualCategories(context))
            {
                if (individualCategoryIds.Count >= individualCategoryIdCount)
                {
                    break;
                }
                individualCategoryIds.Add(individualCategory.Id);
            }
            return individualCategoryIds;
        }

        public static DataTable GetUserSelectedIndividualCategories(WebServiceContext context)
        {
            return GetUserSelectedIndividualCategories(context, GetSomeIndividualCategoryIds(context, 2));
        }

        public static DataTable GetUserSelectedIndividualCategories(WebServiceContext context,
                                                                    List<Int32> individualCategoryIds)
        {
            DataColumn column;
            DataRow row;
            DataTable individualCategoryTable;

            individualCategoryTable = new DataTable(UserSelectedIndividualCategoryData.TABLE_NAME);
            column = new DataColumn(UserSelectedIndividualCategoryData.REQUEST_ID, typeof(Int32));
            individualCategoryTable.Columns.Add(column);
            column = new DataColumn(UserSelectedIndividualCategoryData.INDIVIDUAL_CATEGORY_ID, typeof(Int32));
            individualCategoryTable.Columns.Add(column);
            column = new DataColumn(UserSelectedIndividualCategoryData.INDIVIDUAL_CATEGORY_USAGE, typeof(String));
            individualCategoryTable.Columns.Add(column);
            foreach (Int32 individualCategoryId in individualCategoryIds)
            {
                row = individualCategoryTable.NewRow();
                row[0] = context.RequestId;
                row[1] = individualCategoryId;
                row[2] = UserSelectedIndividualCategoryUsage.Output.ToString();
                individualCategoryTable.Rows.Add(row);
            }
            return individualCategoryTable;
        }
    }
}
