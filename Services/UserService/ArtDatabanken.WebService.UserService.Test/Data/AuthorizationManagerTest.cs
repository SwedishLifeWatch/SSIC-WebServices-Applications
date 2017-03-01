using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class AuthorizationManagerTest : TestBase
    {
        public AuthorizationManagerTest()
            : base(useTransaction, 50)
        {
        }

        #region Additional test attributes
        private TestContext testContextInstance;
        private static Boolean useTransaction = true;

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
        public void IsUserAuthorized()
        {
            WebServiceContext context = GetContext();
            bool isAuthorized = ArtDatabanken.WebService.UserService.Data.AuthorizationManager.IsUserAuthorized(context, null, "UserAdministration", "UserService", null);
            Assert.IsTrue(isAuthorized);

            isAuthorized = ArtDatabanken.WebService.UserService.Data.AuthorizationManager.IsUserAuthorized(context, 1, "UserAdministration", "UserService", null);
            Assert.IsTrue(isAuthorized);

            isAuthorized = ArtDatabanken.WebService.UserService.Data.AuthorizationManager.IsUserAuthorized(context, 2, "Translation", "UserService", null);
            Assert.IsFalse(isAuthorized);
        }

        [TestMethod]
        public void GetApplication()
        {
            WebServiceContext context = GetContext();
            bool isAuthorized = ArtDatabanken.WebService.UserService.Data.AuthorizationManager.IsUserAuthorized(context, 1, "UserAdministration", "UserService", null);
            Assert.IsTrue(isAuthorized);

            WebApplication application = ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationById(context, 1);
            Assert.IsNotNull(application);
        }
    }
}
