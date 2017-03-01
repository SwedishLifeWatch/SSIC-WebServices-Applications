using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebApplicationExtensionTest : TestBase
    {
        private WebApplication _application;

        public WebApplicationExtensionTest()
        {
            _application = null;
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

        public WebApplication GetApplication()
        {
            return GetApplication(false);
        }

        public WebApplication GetApplication(Boolean refresh)
        {
            if (_application.IsNull() || refresh)
            {
                _application = ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationById(GetContext(), Settings.Default.TestApplicationId);
            }
            return _application;
        }

        [TestMethod]
        public void LoadData()
        {
            WebApplication application;
            using (DataReader dataReader = GetContext().GetUserDatabase().GetApplicationById(Settings.Default.TestApplicationId, Settings.Default.SwedenLocaleId))
            {
                application = new WebApplication();
                Assert.IsTrue(dataReader.Read());
                application.LoadData(dataReader);
                Assert.AreEqual(Settings.Default.TestApplicationId, application.Id);
                Assert.AreEqual(Settings.Default.TestApplicationIdentifier, application.Identifier);
                Assert.AreEqual("UserAdmin", application.ShortName);
                Assert.IsNotNull(application.Description);
            }
        }
    }
}
