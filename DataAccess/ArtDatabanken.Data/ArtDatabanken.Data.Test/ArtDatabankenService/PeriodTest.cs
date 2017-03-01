﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class PeriodTest : TestBase
    {
        private Data.ArtDatabankenService.Period _period;

        public PeriodTest()
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

        [TestMethod]
        public void AllowUpdate()
        {
            if (DateTime.Today <= GetPeriod(true).StopUpdate)
            {
                Assert.IsTrue(GetPeriod().AllowUpdate);
            }
            else
            {
                Assert.IsFalse(GetPeriod().AllowUpdate);
            }
        }

        private Data.ArtDatabankenService.Period GetPeriod()
        {
            return GetPeriod(false);
        }

        private Data.ArtDatabankenService.Period GetPeriod(Boolean refresh)
        {
            if (_period.IsNull() || refresh)
            {
                _period = PeriodManagerTest.GetOnePeriod();
            }
            return _period;
        }

        [TestMethod]
        public void Information()
        {
            String information;

            information = GetPeriod(true).Information;
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetPeriod(true).Name.IsNotEmpty());
        }

        [TestMethod]
        public void PeriodType()
        {
            Assert.IsTrue(GetPeriod(true).PeriodType.IsNotNull());
        }

        [TestMethod]
        public void StopUpdate()
        {
            Assert.IsTrue(GetPeriod(true).StopUpdate.IsNotNull());
        }

        [TestMethod]
        public void Year()
        {
            Assert.IsTrue(GetPeriod(true).Year.IsNotNull());
        }
    }
}