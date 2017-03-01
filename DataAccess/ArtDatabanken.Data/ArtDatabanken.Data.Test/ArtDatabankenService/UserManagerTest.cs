using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class UserManagerTest : TestBase
    {
        private Boolean _userLoggedInEventHappened;
        private Boolean _userLoggedOutEventHappened;

        public UserManagerTest()
        {
            _userLoggedInEventHappened = false;
            _userLoggedOutEventHappened = false;
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

        public static Data.ArtDatabankenService.User GetCurrentUser()
        {
            return Data.ArtDatabankenService.UserManager.GetUser();
        }

        [TestMethod]
        public void GetUser()
        {
            Data.ArtDatabankenService.User user;

            Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, TEST_PASSWORD, "EVA", false);
            user = Data.ArtDatabankenService.UserManager.GetUser();
            Assert.IsNotNull(user);
            Assert.AreEqual(TEST_USER_NAME, user.UserName);

            Data.ArtDatabankenService.UserManager.Logout();
            user = Data.ArtDatabankenService.UserManager.GetUser();
            Assert.IsNull(user);
        }

        public static UserRole GetUserRole()
        {
            return GetCurrentUser().Roles[0];
        }

        public static UserRoleList GetUserRoles()
        {
            return GetCurrentUser().Roles;
        }

        [TestMethod]
        public void IsUserLoggedIn()
        {
            Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, TEST_PASSWORD, "EVA", false);
            Assert.IsTrue(Data.ArtDatabankenService.UserManager.IsUserLoggedIn());

            Data.ArtDatabankenService.UserManager.Logout();
            Assert.IsFalse(Data.ArtDatabankenService.UserManager.IsUserLoggedIn());
        }

        [TestMethod]
        public void Login()
        {
            String errorMessage;

            Assert.IsTrue(Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, TEST_PASSWORD, "EVA", false));
            Assert.IsFalse(Data.ArtDatabankenService.UserManager.Login("None existing user name", "None existing password", "EVA", false));

            Assert.IsTrue(Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, TEST_PASSWORD, "EVA", false, out errorMessage));
            Assert.IsTrue(errorMessage.IsEmpty());
            Assert.IsFalse(Data.ArtDatabankenService.UserManager.Login("None existing user name", "None existing password", "EVA", false, out errorMessage));
            Assert.IsTrue(errorMessage.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoginEmptyPasswordError()
        {
            Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, "", "EVA", false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoginEmptyUserNameError()
        {
            Data.ArtDatabankenService.UserManager.Login(" ", TEST_PASSWORD, "EVA", false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoginNullPasswordError()
        {
            Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, null, "EVA", false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoginNullUserNameError()
        {
            Data.ArtDatabankenService.UserManager.Login(null, TEST_PASSWORD, "EVA", false);
        }

        [TestMethod]
        public void Logout()
        {
            // Logout user.
            Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, TEST_PASSWORD, "EVA", false);
            Data.ArtDatabankenService.UserManager.Logout();

            // Should be ok to logout an already logged out user.
            Data.ArtDatabankenService.UserManager.Logout();
        }

        private void UserLoggedIn()
        {
            _userLoggedInEventHappened = true;
        }

        [TestMethod]
        public void UserLoggedInEvent()
        {
            Data.ArtDatabankenService.UserLoggedInEventHandler userLoggedIn;

            userLoggedIn = new Data.ArtDatabankenService.UserLoggedInEventHandler(UserLoggedIn);
            _userLoggedInEventHappened = false;
            try
            {
                Data.ArtDatabankenService.UserManager.UserLoggedInEvent += userLoggedIn;
                Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, TEST_PASSWORD, "EVA", false);
            }
            finally
            {
                Data.ArtDatabankenService.UserManager.UserLoggedInEvent -= userLoggedIn;
            }
            Assert.IsTrue(_userLoggedInEventHappened);
        }

        private void UserLoggedOut()
        {
            _userLoggedOutEventHappened = true;
        }

        [TestMethod]
        public void UserLoggedOutEvent()
        {
            Data.ArtDatabankenService.UserLoggedOutEventHandler userLoggedOut;

            userLoggedOut = new Data.ArtDatabankenService.UserLoggedOutEventHandler(UserLoggedOut);
            _userLoggedOutEventHappened = false;
            try
            {
                Data.ArtDatabankenService.UserManager.UserLoggedOutEvent += userLoggedOut;
                Data.ArtDatabankenService.UserManager.Login(TEST_USER_NAME, TEST_PASSWORD, "EVA", false);
                Data.ArtDatabankenService.UserManager.Logout();
            }
            finally
            {
                Data.ArtDatabankenService.UserManager.UserLoggedOutEvent -= userLoggedOut;
            }
            Assert.IsTrue(_userLoggedOutEventHappened);
        }
    }
}
