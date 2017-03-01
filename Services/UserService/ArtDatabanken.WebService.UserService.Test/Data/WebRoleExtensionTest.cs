using System;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebRoleExtensionTest : TestBase
    {
        private WebRole _role;

        public WebRoleExtensionTest()
        {
            _role = null;
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

        public WebRole GetRole()
        {
            return GetRole(false);
        }

        public WebRole GetRole(Boolean refresh)
        {
            if (_role.IsNull() || refresh)
            {
                _role = ArtDatabanken.WebService.UserService.Data.UserManager.GetRole(GetContext(), Settings.Default.TestRoleId);
            }
            return _role;
        }

        [TestMethod]
        public void LoadData()
        {
            WebRole role;
            using (DataReader dataReader = GetContext().GetUserDatabase().GetRole(Settings.Default.TestRoleId, Settings.Default.SwedenLocaleId))
            {
                role = new WebRole();
                Assert.IsTrue(dataReader.Read());
                role.LoadData(dataReader);
                Assert.AreEqual(Settings.Default.TestRoleId, role.Id);
                Assert.AreEqual(Settings.Default.TestRoleName, role.Name);
                Assert.IsNotNull(role.Description);
                Assert.IsNotNull(role.ValidFromDate);
                Assert.IsNotNull(role.ValidToDate);
                Assert.IsNotNull(role.OrganizationId);
            }
        }
    }
}
