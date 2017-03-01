using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class PeriodTypeTest : TestBase
    {
        private Data.ArtDatabankenService.PeriodType _periodType;

        public PeriodTypeTest()
        {
            _periodType = null;
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

        private Data.ArtDatabankenService.PeriodType GetPeriodType()
        {
            return GetPeriodType(false);
        }

        private Data.ArtDatabankenService.PeriodType GetPeriodType(Boolean refresh)
        {
            if (_periodType.IsNull() || refresh)
            {
                _periodType = PeriodManagerTest.GetOnePeriodType();
            }
            return _periodType;
        }

        [TestMethod]
        public void Description()
        {
            String description;

            description = GetPeriodType(true).Description;
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetPeriodType(true).Name.IsNotEmpty());
        }
    }
}
