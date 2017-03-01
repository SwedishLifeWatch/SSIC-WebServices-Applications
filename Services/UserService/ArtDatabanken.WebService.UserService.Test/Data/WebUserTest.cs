﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebUserTest : TestBase
    {
        private WebUser _user;

        public WebUserTest()
        {
            _user = null;
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
        public void GetPasswordMaxLength()
        {
            Int32 passwordMaxLength;

            passwordMaxLength = UserService.Data.WebUserExtension.GetPasswordMaxLength(GetContext());
            Assert.IsTrue(0 < passwordMaxLength);
        }

        public WebUser GetUser()
        {
            return GetUser(false);
        }

        public WebUser GetUser(Boolean refresh)
        {
            if (_user.IsNull() || refresh)
            {
                _user = ArtDatabanken.WebService.UserService.Data.UserManager.GetUser(GetContext());
            }
            return _user;
        }

        [TestMethod]
        public void GetUserNameMaxLength()
        {
            Int32 userNameMaxLength;

            userNameMaxLength = UserService.Data.WebUserExtension.GetUserNameMaxLength(GetContext());
            Assert.IsTrue(0 < userNameMaxLength);
        }


    }
}
