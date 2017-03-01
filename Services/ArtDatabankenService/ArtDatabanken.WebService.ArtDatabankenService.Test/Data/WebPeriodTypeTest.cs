using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebPeriodTypeTest : TestBase
    {
        private WebPeriodType _periodType;

        public WebPeriodTypeTest()
        {
            _periodType = null;
        }

        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///Description about and functionality for the current test run.
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

        public WebPeriodType GetPeriodType()
        {
            return GetPeriodType(false);
        }

        public WebPeriodType GetPeriodType(Boolean refresh)
        {
            if (_periodType.IsNull() || refresh)
            {
                _periodType = PeriodManagerTest.GetOnePeriodType(GetContext());
            }
            return _periodType;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;
            id = 2;

            GetPeriodType(true).Id = id;
            Assert.AreEqual(GetPeriodType().Id, id);
        }

        [TestMethod]
        public void Description()
        {
            String Description;

            Description = null;
            GetPeriodType(true).Description = Description;
            Assert.AreEqual(GetPeriodType().Description, Description);

            Description = "";
            GetPeriodType().Description = Description;
            Assert.AreEqual(GetPeriodType().Description, Description);

            Description = "Test periodType Description";
            GetPeriodType().Description = Description;
            Assert.AreEqual(GetPeriodType().Description, Description);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetPeriodType(true).Name = name;
            Assert.AreEqual(GetPeriodType().Name, name);

            name = "";
            GetPeriodType().Name = name;
            Assert.AreEqual(GetPeriodType().Name, name);

            name = "Test periodType name";
            GetPeriodType().Name = name;
            Assert.AreEqual(GetPeriodType().Name, name);
        }
    }
}
