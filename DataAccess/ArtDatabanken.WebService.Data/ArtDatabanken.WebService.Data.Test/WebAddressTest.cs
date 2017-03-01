using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebAddress
    /// </summary>
    [TestClass]
    public class WebAddressTest
    {
        private WebAddress _address;

        public WebAddressTest()
        {
            _address = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebAddress address;

            address = new WebAddress();
            Assert.IsNotNull(address);
        }

        private WebAddress GetAddress()
        {
            return GetAddress(false);
        }

        private WebAddress GetAddress(Boolean refresh)
        {
            if (_address.IsNull() || refresh)
            {
                _address = new WebAddress();
            }
            return _address;
        }


        [TestMethod]
        public void PostalAddress1()
        {
            Assert.IsNull(GetAddress().PostalAddress1);
            String value = "TestPostAddress1";
            GetAddress(true).PostalAddress1 = value;
            Assert.AreEqual(value, GetAddress().PostalAddress1);
        }

        [TestMethod]
        public void PostalAddress2()
        {
            Assert.IsNull(GetAddress().PostalAddress2);
            String value = "TestPostAddress2";
            GetAddress(true).PostalAddress2 = value;
            Assert.AreEqual(value, GetAddress().PostalAddress2);
        }

        [TestMethod]
        public void CountryId()
        {
            Int32 value = 199;
            GetAddress(true).CountryId = value;
            Assert.AreEqual(value, GetAddress().CountryId);
        }

        [TestMethod]
        public void City()
        {
            Assert.IsNull(GetAddress().City);
            String value = "City";
            GetAddress(true).City = value;
            Assert.AreEqual(value, GetAddress().City);
        }

        [TestMethod]
        public void AddressId()
        {
            Int32 addressId = 0;
            GetAddress().Id = addressId;
            Assert.AreEqual(GetAddress().Id, addressId);
            addressId = 42;
            GetAddress().Id = addressId;
            Assert.AreEqual(GetAddress().Id, addressId);
        }

        [TestMethod]
        public void ZipCode()
        {
            Assert.IsNull(GetAddress().ZipCode);
            String value = "111 22";
            GetAddress(true).ZipCode = value;
            Assert.AreEqual(value, GetAddress().ZipCode);
        }

        [TestMethod]
        public void AddressType()
        {
            Assert.IsNull(GetAddress().Type);
            String value = "Arbete";
            WebAddress address = GetAddress(true);
            address.Type = new WebAddressType();
            address.Type.Name = value;
            Assert.AreEqual(value, address.Type.Name);
        }

        [TestMethod]
        public void Country()
        {
            Assert.IsNull(GetAddress().Country);
            String value = "Sverige";
            WebAddress address = GetAddress(true);
            address.Country = new WebCountry();
            address.Country.NativeName = value;
            Assert.AreEqual(value, address.Country.NativeName);
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


    }
}
