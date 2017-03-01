using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class PhoneNumberListTest : TestBase
    {
        private PhoneNumberList _phoneNumbers;

        public PhoneNumberListTest()
        {
            _phoneNumbers = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PhoneNumberList phoneNumbers;

            phoneNumbers = new PhoneNumberList();
            Assert.IsNotNull(phoneNumbers);
        }

        [TestMethod]
        public void Get()
        {
            GetPhoneNumbers(true);
            foreach (IPhoneNumber phoneNumber in GetPhoneNumbers())
            {
                Assert.AreEqual(phoneNumber, GetPhoneNumbers().Get(phoneNumber.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 phoneNumberId;

            phoneNumberId = Int32.MaxValue;
            GetPhoneNumbers(true).Get(phoneNumberId);
        }

        private PhoneNumberList GetPhoneNumbers()
        {
            return GetPhoneNumbers(false);
        }

        private PhoneNumberList GetPhoneNumbers(Boolean refresh)
        {
            if (_phoneNumbers.IsNull() || refresh)
            {
                _phoneNumbers = new PhoneNumberList();
                _phoneNumbers.Add(PhoneNumberTest.GetOnePhoneNumber(GetUserContext()));
            }
            return _phoneNumbers;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            PhoneNumberList newPhoneNumberList, oldPhoneNumberList;
            Int32 phoneNumberIndex;

            oldPhoneNumberList = GetPhoneNumbers(true);
            newPhoneNumberList = new PhoneNumberList();
            for (phoneNumberIndex = 0; phoneNumberIndex < oldPhoneNumberList.Count; phoneNumberIndex++)
            {
                newPhoneNumberList.Add(oldPhoneNumberList[oldPhoneNumberList.Count - phoneNumberIndex - 1]);
            }
            for (phoneNumberIndex = 0; phoneNumberIndex < oldPhoneNumberList.Count; phoneNumberIndex++)
            {
                Assert.AreEqual(newPhoneNumberList[phoneNumberIndex], oldPhoneNumberList[oldPhoneNumberList.Count - phoneNumberIndex - 1]);
            }
        }
    }
}
