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
    public class WebAuthorityDataTypeExtensionTest : TestBase
    {
        private WebAuthorityDataType _authorityDataType;

        public WebAuthorityDataTypeExtensionTest()
        {
            _authorityDataType = null;
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

        public WebAuthorityDataType GetAuthorityType()
        {
            return GetAuthorityDataType(false);
        }

        public WebAuthorityDataType GetAuthorityDataType(Boolean refresh)
        {
            if (_authorityDataType.IsNull() || refresh)
            {
                _authorityDataType = ArtDatabanken.WebService.UserService.Data.UserManager.GetAuthorityDataTypes(GetContext())[0];
            }
            return _authorityDataType;
        }

        [TestMethod]
        public void LoadData()
        {
            Assert.IsTrue(GetAuthorityDataType(false).Identifier.Length > 2);
            Assert.IsTrue(GetAuthorityDataType(false).Id > Int32.MinValue);
        }


    }
}
