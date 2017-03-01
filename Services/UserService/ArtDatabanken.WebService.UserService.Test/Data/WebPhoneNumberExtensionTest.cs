using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebPhoneNumberExtensionTest : TestBase
    {
        private WebPhoneNumber _phoneNumber;
        private List<WebPhoneNumber> _phoneNumbers;

        public WebPhoneNumberExtensionTest()
        {
            _phoneNumber = null;
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

        public List<WebPhoneNumber> GetPhoneNumbers()
        {
            return GetPhoneNumbers(false);
        }

        public List<WebPhoneNumber> GetPhoneNumbers(Boolean refresh)
        {
            if (_phoneNumber.IsNull() || refresh)
            {
                _phoneNumbers = ArtDatabanken.WebService.UserService.Data.UserManager.GetPhoneNumbers(GetContext(), Settings.Default.TestPersonId, Settings.Default.TestOrganizationId);
            }
            return _phoneNumbers;
        }

        [TestMethod]
        public void LoadData()
        {
            WebPhoneNumber phoneNumber;
            using (DataReader dataReader = GetContext().GetUserDatabase().GetPhoneNumbers( Settings.Default.TestPersonId, 
                                                                                        Settings.Default.TestOrganizationId))
            {
                phoneNumber = new WebPhoneNumber();
                Assert.IsTrue(dataReader.Read());
                phoneNumber.LoadData(dataReader);
                Assert.AreEqual(Settings.Default.TestPersonId, phoneNumber.PersonId);
            }
        }
    }
}
