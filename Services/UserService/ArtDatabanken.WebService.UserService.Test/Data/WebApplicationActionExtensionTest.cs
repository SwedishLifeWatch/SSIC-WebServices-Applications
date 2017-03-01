using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebApplicationActionExtensionTest : TestBase
    {
        private WebApplicationAction _applicationAction;

        public WebApplicationActionExtensionTest()
        {
            _applicationAction = null;
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

        public WebApplicationAction GetApplicationAction()
        {
            return GetApplicationAction(false);
        }

        public WebApplicationAction GetApplicationAction(Boolean refresh)
        {
            if (_applicationAction.IsNull() || refresh)
            {
                _applicationAction = ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationAction(GetContext(), 3);
            }
            return _applicationAction;
        }

        [TestMethod]
        public void LoadData()
        {
            WebApplicationAction applicationAction;
            using (DataReader dataReader = GetContext().GetUserDatabase().GetApplicationAction(1, Settings.Default.SwedenLocaleId))
            {
                applicationAction = new WebApplicationAction();
                Assert.IsTrue(dataReader.Read());
                applicationAction.LoadData(dataReader);
                Assert.AreEqual(1, applicationAction.Id);
                Assert.IsTrue(applicationAction.Identifier.Length > 5);
                Assert.IsNotNull(applicationAction.GUID);
            }
        }
    }
}
