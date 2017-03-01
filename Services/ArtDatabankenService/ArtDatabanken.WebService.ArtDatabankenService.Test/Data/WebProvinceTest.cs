using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebProvinceTest : TestBase
    {
        private WebProvince _province;

        public WebProvinceTest()
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
            WebProvince province;

            province = GetProvince(true);
            Assert.IsNotNull(province);
        }

        private WebProvince GetProvince()
        {
            return GetProvince(false);
        }

        private WebProvince GetProvince(Boolean refresh)
        {
            if (_province.IsNull() || refresh)
            {
                _province = GeographicManagerTest.GetOneProvince(GetContext());
            }
            return _province;
        }

        [TestMethod]
        public void Identifier()
        {
            String identifier;

            identifier = null;
            GetProvince(true).Identifier = identifier;
            Assert.IsNull(GetProvince().Identifier);
            identifier = "";
            GetProvince().Identifier = identifier;
            Assert.AreEqual(GetProvince().Identifier, identifier);
            identifier = "Test identifier";
            GetProvince().Identifier = identifier;
            Assert.AreEqual(GetProvince().Identifier, identifier);
        }

        [TestMethod]
        public void IsProvincePart()
        {
            Boolean isProvincePart;

            isProvincePart = GetProvince(true).IsProvincePart;

            // Test: Set to false;
            GetProvince().IsProvincePart = false;
            Assert.AreEqual(isProvincePart, GetProvince().IsProvincePart);

            // Test: Set to true;
            GetProvince().IsProvincePart = true;
            Assert.AreEqual(isProvincePart, GetProvince().IsProvincePart);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetProvince(true).Name = name;
            Assert.IsNull(GetProvince().Name);
            name = "";
            GetProvince().Name = name;
            Assert.AreEqual(GetProvince().Name, name);
            name = "Test name";
            GetProvince().Name = name;
            Assert.AreEqual(GetProvince().Name, name);
        }

        [TestMethod]
        public void PartOfProvinceId()
        {
            Int32 partOfProvinceId;

            partOfProvinceId = 423;
            GetProvince(true).PartOfProvinceId = partOfProvinceId;
            Assert.AreEqual(GetProvince().PartOfProvinceId, partOfProvinceId);
        }
    }
}
