using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class PhoneNumberTypeTest
    {
        PhoneNumberType _phoneNumberType;

        public PhoneNumberTypeTest()
        {
            _phoneNumberType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PhoneNumberType phoneNumberType;
            phoneNumberType = new PhoneNumberType(1, "Test", 1, DataContextTest.GetOneDataContext());
            Assert.IsNotNull(phoneNumberType);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetPhoneNumberType(true).DataContext);
        }

        private PhoneNumberType GetPhoneNumberType()
        {
            return GetPhoneNumberType(false);
        }

        private PhoneNumberType GetPhoneNumberType(Boolean refresh)
        {
            if (_phoneNumberType.IsNull() || refresh)
            {
                _phoneNumberType = new PhoneNumberType(1, "Test", 1, DataContextTest.GetOneDataContext());
            }
            return _phoneNumberType;
        }

        [TestMethod]
        public void Id()
        {
            Assert.AreNotEqual(GetPhoneNumberType(true).Id, Int32.MinValue);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetPhoneNumberType(true).Name.IsNotEmpty());
        }

        [TestMethod]
        public void NameStringId()
        {
            Assert.AreNotEqual(GetPhoneNumberType(true).NameStringId, Int32.MinValue);
        }
    }
}
