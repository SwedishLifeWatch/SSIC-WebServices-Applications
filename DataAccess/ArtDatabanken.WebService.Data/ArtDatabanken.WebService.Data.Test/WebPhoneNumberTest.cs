using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebPhoneNumber
    /// </summary>
    [TestClass]
    public class WebPhoneNumberTest
    {
        private WebPhoneNumber _phoneNumber;

        public WebPhoneNumberTest()
        {
            _phoneNumber = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebPhoneNumber phoneNumber;

            phoneNumber = new WebPhoneNumber();
            Assert.IsNotNull(phoneNumber);
        }

        private WebPhoneNumber GetPhoneNumber()
        {
            return GetPhoneNumber(false);
        }

        private WebPhoneNumber GetPhoneNumber(Boolean refresh)
        {
            if (_phoneNumber.IsNull() || refresh)
            {
                _phoneNumber = new WebPhoneNumber();
            }
            return _phoneNumber;
        }


        [TestMethod]
        public void PhoneNumber()
        {
            Assert.IsNull(GetPhoneNumber().Number);
            String value = "018-123456";
            GetPhoneNumber(true).Number = value;
            Assert.AreEqual(value, GetPhoneNumber().Number);
        }

        [TestMethod]
        public void PhoneNumberId()
        {
            Int32 phoneNumberId = 0;
            GetPhoneNumber().Id = phoneNumberId;
            Assert.AreEqual(GetPhoneNumber().Id, phoneNumberId);
            phoneNumberId = 42;
            GetPhoneNumber().Id = phoneNumberId;
            Assert.AreEqual(GetPhoneNumber().Id, phoneNumberId);
        }

        [TestMethod]
        public void PhoneNumberType()
        {
            Assert.IsNull(GetPhoneNumber().Type);
            String value = "Arbete";
            WebPhoneNumber phoneNumber = GetPhoneNumber(true);
            phoneNumber.Type = new WebPhoneNumberType();
            phoneNumber.Type.Name = value;
            Assert.AreEqual(value, phoneNumber.Type.Name);
        }

        [TestMethod]
        public void CountryId()
        {
            Int32 value = 199;
            GetPhoneNumber(true).CountryId = value;
            Assert.AreEqual(value, GetPhoneNumber().CountryId);
        }

        [TestMethod]
        public void Country()
        {
            Assert.IsNull(GetPhoneNumber().Country);
            Int32 value = 46;
            WebPhoneNumber phoneNumber = GetPhoneNumber(true);
            phoneNumber.Country = new WebCountry();
            phoneNumber.Country.PhoneNumberPrefix = value;
            Assert.AreEqual(value, phoneNumber.Country.PhoneNumberPrefix);
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

        #endregion


    }
}
