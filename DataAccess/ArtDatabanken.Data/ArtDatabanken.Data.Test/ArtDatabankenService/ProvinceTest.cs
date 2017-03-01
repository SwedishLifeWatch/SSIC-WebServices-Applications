using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class ProvinceTest : TestBase
    {
        private Province _province;

        public ProvinceTest()
        {
            _province = null;
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
        public void Constructor()
        {
            Province province;

            province = GetProvince(true);
            Assert.IsNotNull(province);
        }

        private Province GetProvince()
        {
            return GetProvince(false);
        }

        private Province GetProvince(Boolean refresh)
        {
            if (_province.IsNull() || refresh)
            {
                _province = GeographicManagerTest.GetProvince();
            }
            return _province;
        }

        [TestMethod]
        public void Identifier()
        {
            Assert.IsTrue(GetProvince(true).Identifier.IsNotEmpty());
        }

        [TestMethod]
        public void IsProvincePart()
        {
            Boolean isProvincePart;

            isProvincePart = GetProvince(true).IsProvincePart;
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetProvince(true).Name.IsNotEmpty());
        }

        [TestMethod]
        public void PartOfProvinceId()
        {
            Int32 partOfProvinceId;

            partOfProvinceId = GetProvince(true).PartOfProvinceId;
        }
    }
}
