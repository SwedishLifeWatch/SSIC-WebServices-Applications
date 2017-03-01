using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class PeriodManagerTest : TestBase
    {
        public PeriodManagerTest()
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
        public void AddUserSelectedPeriods()
        {
            foreach (UserSelectedPeriodUsage periodUsage in Enum.GetValues(typeof(UserSelectedPeriodUsage)))
            {
                PeriodManager.AddUserSelectedPeriods(GetContext(), PeriodManagerTest.GetSomePeriodIds(GetContext()), periodUsage);
                PeriodManager.DeleteUserSelectedPeriods(GetContext());
            }
        }

        [TestMethod]
        public void DeleteUserSelectedPeriods()
        {
            foreach (UserSelectedPeriodUsage periodUsage in Enum.GetValues(typeof(UserSelectedPeriodUsage)))
            {
                PeriodManager.AddUserSelectedPeriods(GetContext(), PeriodManagerTest.GetSomePeriodIds(GetContext()), periodUsage);
                PeriodManager.DeleteUserSelectedPeriods(GetContext());
            }
        }

        public static WebPeriod GetOnePeriod(WebServiceContext context)
        {
            return PeriodManager.GetPeriods(context)[0];
        }

        public static WebPeriodType GetOnePeriodType(WebServiceContext context)
        {
            return PeriodManager.GetPeriodTypes(context)[0];
        }

        [TestMethod]
        public void GetPeriods()
        {
            List<WebPeriod> periods;

            periods = PeriodManager.GetPeriods(GetContext());
            Assert.IsTrue(periods.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriodTypesTest()
        {
            List<WebPeriodType> periodTypes;

            periodTypes = PeriodManager.GetPeriodTypes(GetContext());
            Assert.IsTrue(periodTypes.IsNotEmpty());
        }

        public static List<WebPeriod> GetSomePeriods(WebServiceContext context)
        {
            return PeriodManager.GetPeriods(context);
        }

        public static List<Int32> GetSomePeriodIds(WebServiceContext context)
        {
            List<Int32> periodIds;

            periodIds = new List<Int32>();
            foreach (WebPeriod period in GetSomePeriods(context))
            {
                periodIds.Add(period.Id);
            }
            return periodIds;
        }

        public static DataTable GetUserSelectedPeriods(WebServiceContext context)
        {
            return GetUserSelectedPeriods(context, GetSomePeriods(context));
        }

        public static DataTable GetUserSelectedPeriods(WebServiceContext context,
                                                       List<WebPeriod> periodIds)
        {
            DataColumn column;
            DataRow row;
            DataTable periodTable;

            periodTable = new DataTable(UserSelectedPeriodData.TABLE_NAME);
            column = new DataColumn(UserSelectedPeriodData.REQUEST_ID, typeof(Int32));
            periodTable.Columns.Add(column);
            column = new DataColumn(UserSelectedPeriodData.PERIOD_ID, typeof(Int32));
            periodTable.Columns.Add(column);
            column = new DataColumn(UserSelectedPeriodData.PERIOD_USAGE, typeof(String));
            periodTable.Columns.Add(column);
            foreach (WebPeriod period in periodIds)
            {
                row = periodTable.NewRow();
                row[0] = context.RequestId;
                row[1] = period.Id;
                row[2] = UserSelectedPeriodUsage.Output.ToString();
                periodTable.Rows.Add(row);
            }
            return periodTable;
        }
    }
}
