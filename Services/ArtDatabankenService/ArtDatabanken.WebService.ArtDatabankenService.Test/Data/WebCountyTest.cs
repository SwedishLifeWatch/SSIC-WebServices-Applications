using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebCountyTest : TestBase
    {
        private WebCounty _county;

        public WebCountyTest()
        {
            _county = null;
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
            WebCounty county;

            county = GetCounty(true);
            Assert.IsNotNull(county);
        }

        private WebCounty GetCounty()
        {
            return GetCounty(false);
        }

        private WebCounty GetCounty(Boolean refresh)
        {
            if (_county.IsNull() || refresh)
            {
                _county = GeographicManagerTest.GetOneCounty(GetContext());
            }
            return _county;
        }

        [TestMethod]
        public void HasNumber()
        {
            // Test: Set to false;
            GetCounty().IsNumberSpecified = false;
            Assert.IsFalse(GetCounty().IsNumberSpecified);

            // Test: Set to true;
            GetCounty().IsNumberSpecified = true;
            Assert.IsTrue(GetCounty().IsNumberSpecified);
        }

        [TestMethod]
        public void Identifier()
        {
            String identifier;

            identifier = null;
            GetCounty(true).Identifier = identifier;
            Assert.IsNull(GetCounty().Identifier);
            identifier = "";
            GetCounty().Identifier = identifier;
            Assert.AreEqual(GetCounty().Identifier, identifier);
            identifier = "Test identifier";
            GetCounty().Identifier = identifier;
            Assert.AreEqual(GetCounty().Identifier, identifier);
        }

        [TestMethod]
        public void IsCountyPart()
        {
            Boolean isCountyPart;

            isCountyPart = GetCounty(true).IsCountyPart;

            // Test: Set to false;
            GetCounty().IsCountyPart = false;
            Assert.AreEqual(isCountyPart, GetCounty().IsCountyPart);

            // Test: Set to true;
            GetCounty().IsCountyPart = true;
            Assert.AreEqual(isCountyPart, GetCounty().IsCountyPart);
        }

        [TestMethod]
        public void Name()
        {
            String name;

            name = null;
            GetCounty(true).Name = name;
            Assert.IsNull(GetCounty().Name);
            name = "";
            GetCounty().Name = name;
            Assert.AreEqual(GetCounty().Name, name);
            name = "Test name";
            GetCounty().Name = name;
            Assert.AreEqual(GetCounty().Name, name);
        }

        [TestMethod]
        public void Number()
        {
            Int32 number;

            number = 423;
            GetCounty(true).Number = number;
            Assert.AreEqual(GetCounty().Number, number);
        }

        [TestMethod]
        public void PartOfCountyId()
        {
            Int32 partOfCountyId;

            partOfCountyId = 423;
            GetCounty(true).PartOfCountyId = partOfCountyId;
            Assert.AreEqual(GetCounty().PartOfCountyId, partOfCountyId);
        }
    }
}
