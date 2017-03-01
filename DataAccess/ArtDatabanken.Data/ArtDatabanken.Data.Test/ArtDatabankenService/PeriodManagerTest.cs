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
        public void GetCurrentPublicPeriod()
        {
            Data.ArtDatabankenService.Period currentPublicPeriod;

            currentPublicPeriod = PeriodManager.GetCurrentPublicPeriod();
            Assert.IsNotNull(currentPublicPeriod);
            Assert.IsFalse(currentPublicPeriod.AllowUpdate);
            Assert.AreEqual((Int32)Data.ArtDatabankenService.PeriodTypeId.SwedishRedlist, currentPublicPeriod.PeriodType.Id);
        }

        public static Data.ArtDatabankenService.Period GetOnePeriod()
        {
            return PeriodManager.GetPeriods()[0];
        }

        public static Data.ArtDatabankenService.PeriodType GetOnePeriodType()
        {
            return PeriodManager.GetPeriodTypes()[0];
        }

        [TestMethod]
        public void GetPeriod()
        {
            foreach (Period period in GetSomePeriods())
            {
                Assert.AreEqual(period, PeriodManager.GetPeriod(period.Id));
            }
        }

        [TestMethod]
        public void GetPeriods()
        {
            Data.ArtDatabankenService.PeriodList allPeriods;
            Data.ArtDatabankenService.PeriodList redlistPeriods;

            allPeriods = PeriodManager.GetPeriods();
            Assert.IsTrue(allPeriods.IsNotEmpty());
            redlistPeriods = PeriodManager.GetPeriods(Data.ArtDatabankenService.PeriodTypeId.SwedishRedlist);
            Assert.IsTrue(redlistPeriods.IsNotEmpty());
            Assert.IsTrue(redlistPeriods.Count < allPeriods.Count);
        }

        public static Data.ArtDatabankenService.PeriodList GetSomePeriods()
        {
            Data.ArtDatabankenService.PeriodList periods;

            periods = new Data.ArtDatabankenService.PeriodList();
            periods.AddRange(PeriodManager.GetPeriods().GetRange(0, 2));
            return periods;
        }

        [TestMethod]
        public void GetPeriodTypes()
        {
            Data.ArtDatabankenService.PeriodTypeList periodTypeList;

            periodTypeList = PeriodManager.GetPeriodTypes();
            Assert.IsTrue(periodTypeList.IsNotEmpty());
        }

        [TestMethod]
        public void GetPeriodType()
        {
            Data.ArtDatabankenService.PeriodType redlistPeriodType = PeriodManager.GetPeriodType(Data.ArtDatabankenService.PeriodTypeId.SwedishRedlist);
            Assert.AreEqual(1, redlistPeriodType.Id);
            Data.ArtDatabankenService.PeriodType helcomRedlistPeriodType = PeriodManager.GetPeriodType(2);
            Assert.AreEqual((Int32)PeriodTypeId.HelcomRedList, helcomRedlistPeriodType.Id);
        }


    }
}
