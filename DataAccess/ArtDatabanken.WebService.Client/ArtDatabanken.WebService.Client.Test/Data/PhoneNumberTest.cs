using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class PhoneNumberTest : TestBase
    {
        PhoneNumber _phoneNumber;

        public PhoneNumberTest()
        {
            _phoneNumber = null;
        }

        [TestMethod]
        public void Country()
        {
            ICountry country;

            country = null;
            GetPhoneNumber(true).Country = country;
            Assert.IsNotNull(GetPhoneNumber().Country);
            country = CoreData.CountryManager.GetCountries(GetUserContext())[0];
            GetPhoneNumber().Country = country;
            Assert.AreEqual(country, GetPhoneNumber().Country);
        }

        [TestMethod]
        public void Constructor()
        {
            PhoneNumber phoneNumber;

            phoneNumber = new PhoneNumber(GetUserContext());
            Assert.IsNotNull(phoneNumber);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetPhoneNumber(true).DataContext);
        }

        private PhoneNumber GetPhoneNumber()
        {
            return GetPhoneNumber(false);
        }

        private PhoneNumber GetPhoneNumber(Boolean refresh)
        {
            if (_phoneNumber.IsNull() || refresh)
            {
                _phoneNumber = new PhoneNumber(GetUserContext());
            }
            return _phoneNumber;
        }

        public static PhoneNumber GetOnePhoneNumber(IUserContext userContext)
        {
            return new PhoneNumber(userContext);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = 42;
            GetPhoneNumber(true).Id = id;
            Assert.AreEqual(id, GetPhoneNumber().Id);
        }

        [TestMethod]
        public void Number()
        {
            String number;

            number = null;
            GetPhoneNumber(true).Number = number;
            Assert.AreEqual(number, GetPhoneNumber().Number);
            number = "  ";
            GetPhoneNumber().Number = number;
            Assert.AreEqual(number, GetPhoneNumber().Number);
            number = "018 - 752 52";
            GetPhoneNumber().Number = number;
            Assert.AreEqual(number, GetPhoneNumber().Number);
        }

        [TestMethod]
        public void Prefix()
        {
            Assert.AreNotEqual(Int32.MinValue, GetPhoneNumber(true).Prefix);
        }

        [TestMethod]
        public void Type()
        {
            IPhoneNumberType type;

            type = null;
            GetPhoneNumber(true).Type = type;
            Assert.IsNotNull(GetPhoneNumber().Type);
            type = CoreData.UserManager.GetPhoneNumberTypes(GetUserContext())[0];
            GetPhoneNumber().Type = type;
            Assert.AreEqual(type, GetPhoneNumber().Type);
        }
    }
}
