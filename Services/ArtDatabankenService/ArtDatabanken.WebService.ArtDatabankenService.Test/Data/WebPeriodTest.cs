using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebPeriodTest : TestBase
    {
        private WebPeriod _period;

        public WebPeriodTest()
        {
            _period = null;
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

        public WebPeriod GetPeriod()
        {
            return GetPeriod(false);
        }

        public WebPeriod GetPeriod(Boolean refresh)
        {
            if (_period.IsNull() || refresh)
            {
                _period = PeriodManagerTest.GetOnePeriod(GetContext());
            }
            return _period;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;
            id = 2;

            GetPeriod(true).Id = id;
            Assert.AreEqual(GetPeriod().Id, id);
        }

        [TestMethod]
        public void Information()
        {
            String information;

            information = null;
            GetPeriod(true).Information = information;
            Assert.AreEqual(GetPeriod().Information, information);

            information = "";
            GetPeriod().Information = information;
            Assert.AreEqual(GetPeriod().Information, information);

            information = "Test period information";
            GetPeriod().Information = information;
            Assert.AreEqual(GetPeriod().Information, information);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetPeriod(true).Name = name;
            Assert.AreEqual(GetPeriod().Name, name);

            name = "";
            GetPeriod().Name = name;
            Assert.AreEqual(GetPeriod().Name, name);

            name = "Test period name";
            GetPeriod().Name = name;
            Assert.AreEqual(GetPeriod().Name, name);
        }

        [TestMethod]
        public void PeriodTypeId()
        {
            Int32 id;
            id = 2;

            GetPeriod(true).PeriodTypeId = id;
            Assert.AreEqual(GetPeriod().PeriodTypeId, id);
        }

        [TestMethod]
        public void StopUpdate()
        {
            Assert.IsTrue(GetPeriod(true).StopUpdate.IsNotNull());
        }

        [TestMethod]
        public void Year()
        {
            Int32 year;
            year = 2000;

            GetPeriod(true).Year = year;
            Assert.AreEqual(GetPeriod().Year, year);
        }
    }
}
