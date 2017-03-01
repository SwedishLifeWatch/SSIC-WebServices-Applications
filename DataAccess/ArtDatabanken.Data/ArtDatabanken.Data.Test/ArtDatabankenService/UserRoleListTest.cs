using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class UserRoleListTest : TestBase
    {
        private UserRoleList _userRoles;

        public UserRoleListTest()
        {
            _userRoles = null;
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
        public void Get()
        {
            foreach (UserRole userRole in GetUserRoles(true))
            {
                Assert.AreEqual(userRole, GetUserRoles().Get(userRole.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 userRoleId;

            userRoleId = Int32.MinValue;
            GetUserRoles().Get(userRoleId);
        }

        private UserRoleList GetUserRoles()
        {
            return GetUserRoles(false);
        }

        private UserRoleList GetUserRoles(Boolean refresh)
        {
            if (_userRoles.IsNull() || refresh)
            {
                _userRoles = UserManagerTest.GetUserRoles();
            }
            return _userRoles;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            Int32 userRoleIndex;
            UserRoleList newUserRoleList, oldUserRoleList;

            oldUserRoleList = GetUserRoles(true);
            newUserRoleList = new UserRoleList();
            for (userRoleIndex = 0; userRoleIndex < oldUserRoleList.Count; userRoleIndex++)
            {
                newUserRoleList.Add(oldUserRoleList[oldUserRoleList.Count - userRoleIndex - 1]);
            }
            for (userRoleIndex = 0; userRoleIndex < oldUserRoleList.Count; userRoleIndex++)
            {
                Assert.AreEqual(newUserRoleList[userRoleIndex], oldUserRoleList[oldUserRoleList.Count - userRoleIndex - 1]);
            }
        }
    }
}
