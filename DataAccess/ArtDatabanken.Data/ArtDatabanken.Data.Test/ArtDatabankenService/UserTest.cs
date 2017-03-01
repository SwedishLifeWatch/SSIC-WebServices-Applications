using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;

namespace ArtDatabanken.Data.Test.ArtDatabankenService
{
    [TestClass]
    public class UserTest : TestBase
    {
        private Data.ArtDatabankenService.User _user;

        public UserTest()
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
        public void Constructor()
        {
            Data.ArtDatabankenService.User user;

            user = GetUser(true);
            Assert.IsNotNull(user);
        }

        private Data.ArtDatabankenService.User GetUser()
        {
            return GetUser(false);
        }

        private Data.ArtDatabankenService.User GetUser(Boolean refresh)
        {
            if (_user.IsNull() || refresh)
            {
                _user = UserManagerTest.GetCurrentUser();
            }
            return _user;
        }

        [TestMethod]
        public void FirstName()
        {
            Assert.IsTrue(GetUser(true).FirstName.IsNotEmpty());
        }

        [TestMethod]
        public void FullName()
        {
            Assert.IsTrue(GetUser(true).FullName.IsNotEmpty());
        }

        [TestMethod]
        public void LastName()
        {
            Assert.IsTrue(GetUser(true).LastName.IsNotEmpty());
        }

        [TestMethod]
        public void Roles()
        {
            Assert.IsTrue(GetUser(true).Roles.IsNotEmpty());
        }

        [TestMethod]
        public void UserName()
        {
            Assert.IsTrue(GetUser(true).UserName.IsNotEmpty());
        }
    }
}
