using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebPhoneNumberTypeExtensionTest : TestBase
    {
        private WebPhoneNumberType _phoneNumberType;

        public WebPhoneNumberTypeExtensionTest()
        {
            _phoneNumberType = null;
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

        public WebPhoneNumberType GetPhoneNumberType()
        {
            return GetPhoneNumberType(false);
        }

        public WebPhoneNumberType GetPhoneNumberType(Boolean refresh)
        {
            if (_phoneNumberType.IsNull() || refresh)
            {
                _phoneNumberType = ArtDatabanken.WebService.UserService.Data.UserManager.GetPhoneNumberTypes(GetContext())[0];
            }
            return _phoneNumberType;
        }

        [TestMethod]
        public void LoadData()
        {
            Assert.IsTrue(GetPhoneNumberType(false).Name.Length > 2);
            Assert.IsTrue(GetPhoneNumberType(false).NameStringId > 0);
            Assert.IsTrue(GetPhoneNumberType(false).Id > Int32.MinValue);
        }


    }
}

