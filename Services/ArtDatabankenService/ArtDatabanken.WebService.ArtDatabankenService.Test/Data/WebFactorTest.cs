using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    /// <summary>
    /// Summary description for WebFactorTest
    /// </summary>
    [TestClass]
    public class WebFactorTest : TestBase
    {
        private WebFactor _factor;

        public WebFactorTest()
        {
            _factor = null;
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
        public void FactorDataTypeId()
        {
            Int32 id;

            id = 42;
            GetFactor().FactorDataTypeId = id;
            Assert.AreEqual(GetFactor().FactorDataTypeId, id);
        }

        [TestMethod]
        public void FactorOriginId()
        {
            Int32 id;

            id = 42;
            GetFactor(true).FactorOriginId = id;
            Assert.AreEqual(GetFactor().FactorOriginId, id);
        }

        [TestMethod]
        public void FactorUpdateModeId()
        {
            Int32 id;

            id = 42;
            GetFactor(true).FactorUpdateModeId = id;
            Assert.AreEqual(GetFactor().FactorUpdateModeId, id);
        }

        public WebFactor GetFactor()
        {
            return GetFactor(false);
        }

        public WebFactor GetFactor(Boolean refresh)
        {
            if (_factor.IsNull() || refresh)
            {
                _factor = FactorManagerTest.GetOneFactor(GetContext());
            }
            return _factor;
        }

        [TestMethod]
        public void HostLabel()
        {
            String testString;

            testString = null;
            GetFactor(true).HostLabel = testString;
            Assert.IsNull(GetFactor().HostLabel);
            testString = "";
            GetFactor().HostLabel = testString;
            Assert.AreEqual(GetFactor().HostLabel, testString);
            testString = "Test HostLabel of Factor";
            GetFactor().HostLabel = testString;
            Assert.AreEqual(GetFactor().HostLabel, testString);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetFactor(true).Id = id;
            Assert.AreEqual(GetFactor().Id, id);
        }

        [TestMethod]
        public void Information()
        {
            String testString;

            testString = null;
            GetFactor(true).Information = testString;
            Assert.IsNull(GetFactor().Information);
            testString = "";
            GetFactor().Information = testString;
            Assert.AreEqual(GetFactor().Information, testString);
            testString = "Test Information of Factor";
            GetFactor().Information = testString;
            Assert.AreEqual(GetFactor().Information, testString);
        }

        [TestMethod]
        public void IsLeaf()
        {
            Boolean isLeaf;

            isLeaf = GetFactor(true).IsLeaf;
            Assert.IsTrue(isLeaf || !isLeaf);

            isLeaf = false;
            GetFactor().IsLeaf = isLeaf;
            Assert.AreEqual(GetFactor().IsLeaf, isLeaf);

            isLeaf = true;
            GetFactor().IsLeaf = isLeaf;
            Assert.AreEqual(GetFactor().IsLeaf, isLeaf);
        }

        [TestMethod]
        public void IsPeriodic()
        {
            Boolean isPeriodic;

            isPeriodic = true;
            GetFactor(true).IsPeriodic = isPeriodic;
            Assert.AreEqual(GetFactor().IsPeriodic, isPeriodic);

            isPeriodic = false;
            GetFactor().IsPeriodic = isPeriodic;
            Assert.AreEqual(GetFactor().IsPeriodic, isPeriodic);
        }

        [TestMethod]
        public void IsPublic()
        {
            Boolean testValue;

            testValue = false;
            GetFactor(true).IsPublic = testValue;
            Assert.AreEqual(GetFactor().IsPublic, testValue);
            testValue = true;
            Assert.AreNotEqual(GetFactor().IsPublic, testValue);
        }

        [TestMethod]
        public void IsTaxonomic()
        {
            Boolean testValue;

            testValue = false;
            GetFactor(true).IsTaxonomic = testValue;
            Assert.AreEqual(GetFactor().IsTaxonomic, testValue);
            testValue = true;
            Assert.AreNotEqual(GetFactor().IsTaxonomic, testValue);
        }

        [TestMethod]
        public void Label()
        {
            String testString;

            testString = null;
            GetFactor(true).Label = testString;
            Assert.IsNull(GetFactor().Label);
            testString = "";
            GetFactor().Label = testString;
            Assert.AreEqual(GetFactor().Label, testString);
            testString = "Test Label of Factor";
            GetFactor().Label = testString;
            Assert.AreEqual(GetFactor().Label, testString);
        }

        [TestMethod]
        public void Name()
        {
            String testString;

            testString = null;
            GetFactor(true).Name = testString;
            Assert.IsNull(GetFactor().Name);
            testString = "";
            GetFactor().Name = testString;
            Assert.AreEqual(GetFactor().Name, testString);
            testString = "Test Name of Factor";
            GetFactor().Name = testString;
            Assert.AreEqual(GetFactor().Name, testString);
        }
    }
}
