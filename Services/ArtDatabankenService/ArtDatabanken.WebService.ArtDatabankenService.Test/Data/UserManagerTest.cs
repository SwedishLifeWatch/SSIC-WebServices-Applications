using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class UserManagerTest : TestBase
    {
        private UserManager _userManager;

        public UserManagerTest()
            : base(true, 50)
        {
            _userManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            UserManager userManager;

            userManager = new UserManager();
            Assert.IsNotNull(userManager);
        }

        [TestMethod]
        public void GetPerson()
        {
            WebPerson person;

            person = GetUserManager(true).GetPerson(GetContext());
            Assert.IsNotNull(person);
        }

        [TestMethod]
        public void GetRoles()
        {
            List<WebRole> roles;

            roles = GetUserManager(true).GetRoles(GetContext());
            Assert.IsTrue(roles.IsNotEmpty());
        }

        [TestMethod]
        public void GetUser()
        {
            ArtDatabanken.WebService.Data.WebUser user;

            // Get existing user.
            user = GetUserManager(true).GetUser(GetContext());
            Assert.IsNotNull(user);
            Assert.AreEqual(user.UserName, GetContext().ClientToken.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetUserUnknownUserError()
        {
            WebClientToken clientToken;
            ArtDatabanken.WebService.Data.WebUser user;

            // Get none existing user.
            clientToken = new WebClientToken("None existing user", "No application identifier", WebServiceData.WebServiceManager.Key);
            using (WebServiceContext context = new WebServiceContextCached(clientToken, false))
            {
                user = GetUserManager().GetUser(context);
                Assert.IsNull(user);
            }
        }

        private UserManager GetUserManager()
        {
            return GetUserManager(false);
        }

        private UserManager GetUserManager(Boolean refresh)
        {
            if (_userManager.IsNull() || refresh)
            {
                _userManager = new UserManager();
            }
            return _userManager;
        }

        [TestMethod]
        public void Login()
        {
            Int32 loginCount;
            WebLoginResponse loginResponse;

            // Login existing user.
            loginResponse = GetUserManager(true).Login(GetContext(), TEST_USER_NAME, TEST_PASSWORD, TEST_APPLICATION_IDENTIFIER, false);
            Assert.IsNotNull(loginResponse);

            // Login none existing user.
            loginResponse = GetUserManager().Login(GetContext(), "None existing user", "No password", "No application identifier", false);
            Assert.IsNull(loginResponse);

            // Test to fail a couple of times and finally succed.
            for (loginCount = 0; loginCount < (Settings.Default.LoginAttemptLimit - 1); loginCount++)
            {
                loginResponse = GetUserManager().Login(GetContext(), TEST_USER_NAME, "No password", TEST_APPLICATION_IDENTIFIER, false);
                Assert.IsNull(loginResponse);
            }
            loginResponse = GetUserManager().Login(GetContext(), TEST_USER_NAME, TEST_PASSWORD, TEST_APPLICATION_IDENTIFIER, false);
            Assert.IsNotNull(loginResponse);
            for (loginCount = 0; loginCount < (Settings.Default.LoginAttemptLimit - 1); loginCount++)
            {
                loginResponse = GetUserManager().Login(GetContext(), TEST_USER_NAME, "No password", TEST_APPLICATION_IDENTIFIER, false);
                Assert.IsNull(loginResponse);
            }
            loginResponse = GetUserManager().Login(GetContext(), TEST_USER_NAME, TEST_PASSWORD, TEST_APPLICATION_IDENTIFIER, false);
            Assert.IsNotNull(loginResponse);

            // Test to fail to many times.
            for (loginCount = 0; loginCount < Settings.Default.LoginAttemptLimit; loginCount++)
            {
                loginResponse = GetUserManager().Login(GetContext(), TEST_USER_NAME, "No password", TEST_APPLICATION_IDENTIFIER, false);
                Assert.IsNull(loginResponse);
            }
            loginResponse = GetUserManager().Login(GetContext(), TEST_USER_NAME, TEST_PASSWORD, TEST_APPLICATION_IDENTIFIER, false);
            Assert.IsNull(loginResponse);

            // Wait for login attempt to be allowed again.
            Thread.Sleep(1200 * Settings.Default.LoginTimeLimit);
            loginResponse = GetUserManager().Login(GetContext(), TEST_USER_NAME, TEST_PASSWORD, TEST_APPLICATION_IDENTIFIER, false);
            Assert.IsNotNull(loginResponse); 
        }

        [TestMethod]
        public void Logout()
        {
            // Logout existing user.
            GetUserManager(true).Logout(GetContext());

            // Should be ok to logout an already logged out user.
            GetUserManager().Logout(GetContext());
        }
    }
}
