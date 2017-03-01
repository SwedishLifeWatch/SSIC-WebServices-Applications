using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class WebDatabaseUpdateTest : TestBase
    {
        private WebDatabaseUpdate _databaseUpdate;

        public WebDatabaseUpdateTest()
        {
            _databaseUpdate = null;
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
            WebDatabaseUpdate databaseUpdate;

            databaseUpdate = GetDatabaseUpdate(true);
            Assert.IsNotNull(databaseUpdate);
        }

        private WebDatabaseUpdate GetDatabaseUpdate()
        {
            return GetDatabaseUpdate(false);
        }

        private WebDatabaseUpdate GetDatabaseUpdate(Boolean refresh)
        {
            if (_databaseUpdate.IsNull() || refresh)
            {
                _databaseUpdate = DatabaseManagerTest.GetOneDatabaseUpdate(GetContext());
            }
            return _databaseUpdate;
        }

        [TestMethod]
        public void UpdateEnd()
        {
            DateTime now;

            now = DateTime.Now;
            GetDatabaseUpdate(true).UpdateEnd = now;
            Assert.AreEqual(GetDatabaseUpdate().UpdateEnd, now);
        }

        [TestMethod]
        public void UpdateStart()
        {
            DateTime now;

            now = DateTime.Now;
            GetDatabaseUpdate(true).UpdateStart = now;
            Assert.AreEqual(GetDatabaseUpdate().UpdateStart, now);
        }
    }
}
