using System;
using System.Collections.Generic;
using ArtDatabanken.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class WebAddressExtensionTest : TestBase
    {
        private WebAddress _address;
        private List<WebAddress> _addresses;

        public WebAddressExtensionTest()
        {
            _address = null;
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
        public void CheckData()
        {
            _address = new WebAddress();
            _address.City = "abcd<Uppsala>";
            _address.CheckData();
            Assert.AreEqual("abcdUppsala", _address.City);
            Assert.IsTrue(_address.IsDataChecked);           
 
            // Test IsDataChecked
            // Try a second time -- CheckData removes "<" and ">" one time only
            _address.City = "abcd<Uppsala>";
            _address.CheckData();
            Assert.AreEqual("abcd<Uppsala>", _address.City);

            _address.IsDataChecked = false;
            _address.City = "abcd<Uppsala";
            _address.CheckData();
            Assert.AreEqual("abcd<Uppsala", _address.City);

        }

        public List<WebAddress> GetAddresses()
        {
            return GetAddresses(false);
        }

        public List<WebAddress> GetAddresses(Boolean refresh)
        {
            if (_address.IsNull() || refresh)
            {
                _addresses = ArtDatabanken.WebService.UserService.Data.UserManager.GetAddresses(GetContext(), Settings.Default.TestPersonId, Settings.Default.TestOrganizationId);
            }
            return _addresses;
        }

        [TestMethod]
        public void LoadData()
        {
            WebAddress address;

            using (DataReader dataReader = GetContext().GetUserDatabase().GetAddresses( Int32.MinValue, 
                                                                                        Settings.Default.TestOrganizationId, 
                                                                                        Settings.Default.SwedenLocaleId ))
            {
                address = new WebAddress();
                Assert.IsTrue(dataReader.Read());
                address.LoadData(dataReader);
                Assert.AreEqual(Settings.Default.TestOrganizationId, address.OrganizationId);
            }
            using (DataReader dataReader = GetContext().GetUserDatabase().GetAddresses( Settings.Default.TestPersonId,
                                                                                        Int32.MinValue,
                                                                                        Settings.Default.SwedenLocaleId ))
            {
                address = new WebAddress();
                Assert.IsTrue(dataReader.Read());
                address.LoadData(dataReader);
                Assert.AreEqual(Settings.Default.TestPersonId, address.PersonId);
            }
        }
    }
}
