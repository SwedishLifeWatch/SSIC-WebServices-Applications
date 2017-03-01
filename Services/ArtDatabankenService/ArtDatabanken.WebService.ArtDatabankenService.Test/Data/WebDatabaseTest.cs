using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebDatabaseTest : TestBase
    {
        private WebDatabase _database;

        public WebDatabaseTest()
        {
            _database = null;
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
            WebDatabase database;

            database = GetDatabase(true);
            Assert.IsNotNull(database);
        }

        private WebDatabase GetDatabase()
        {
            return GetDatabase(false);
        }

        private WebDatabase GetDatabase(Boolean refresh)
        {
            if (_database.IsNull() || refresh)
            {
                _database = DatabaseManagerTest.GetOneDatabase(GetContext());
            }
            return _database;
        }

        [TestMethod]
        public void LongName()
        {
            String name;

            name = null;
            GetDatabase(true).LongName = name;
            Assert.IsNull(GetDatabase().LongName);
            name = "";
            GetDatabase().LongName = name;
            Assert.AreEqual(GetDatabase().LongName, name);
            name = "Test database long name";
            GetDatabase().LongName = name;
            Assert.AreEqual(GetDatabase().LongName, name);
        }

        [TestMethod]
        public void ShortName()
        {
            String name;

            name = null;
            GetDatabase(true).ShortName = name;
            Assert.IsNull(GetDatabase().ShortName);
            name = "";
            GetDatabase().ShortName = name;
            Assert.AreEqual(GetDatabase().ShortName, name);
            name = "Test database short name";
            GetDatabase().ShortName = name;
            Assert.AreEqual(GetDatabase().ShortName, name);
        }
    }
}
