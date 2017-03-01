using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class PhoneNumberTypeListTest : TestBase
    {
        private PhoneNumberTypeList _phoneNumberTypes;

        public PhoneNumberTypeListTest()
        {
            _phoneNumberTypes = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PhoneNumberTypeList phoneNumberTypes;

            phoneNumberTypes = new PhoneNumberTypeList();
            Assert.IsNotNull(phoneNumberTypes);
        }

        [TestMethod]
        public void Get()
        {
            IPhoneNumberType phoneNumberType;

            GetPhoneNumberTypes(true);
            foreach (PhoneNumberTypeId phoneNumberTypeId in Enum.GetValues(typeof(PhoneNumberTypeId)))
            {
                phoneNumberType = GetPhoneNumberTypes().Get(phoneNumberTypeId);
                Assert.IsNotNull(phoneNumberType);
            }

            foreach (IPhoneNumberType tempPhoneNumberType in GetPhoneNumberTypes())
            {
                Assert.AreEqual(tempPhoneNumberType, GetPhoneNumberTypes().Get(tempPhoneNumberType.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 phoneNumberTypeId;

            phoneNumberTypeId = Int32.MinValue;
            GetPhoneNumberTypes(true).Get(phoneNumberTypeId);
        }

        private PhoneNumberTypeList GetPhoneNumberTypes()
        {
            return GetPhoneNumberTypes(false);
        }

        private PhoneNumberTypeList GetPhoneNumberTypes(Boolean refresh)
        {
            if (_phoneNumberTypes.IsNull() || refresh)
            {
                _phoneNumberTypes = CoreData.UserManager.GetPhoneNumberTypes(GetUserContext());
            }
            return _phoneNumberTypes;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            PhoneNumberTypeList newPhoneNumberTypeList, oldPhoneNumberTypeList;
            Int32 phoneNumberTypeIndex;

            oldPhoneNumberTypeList = GetPhoneNumberTypes(true);
            newPhoneNumberTypeList = new PhoneNumberTypeList();
            for (phoneNumberTypeIndex = 0; phoneNumberTypeIndex < oldPhoneNumberTypeList.Count; phoneNumberTypeIndex++)
            {
                newPhoneNumberTypeList.Add(oldPhoneNumberTypeList[oldPhoneNumberTypeList.Count - phoneNumberTypeIndex - 1]);
            }
            for (phoneNumberTypeIndex = 0; phoneNumberTypeIndex < oldPhoneNumberTypeList.Count; phoneNumberTypeIndex++)
            {
                Assert.AreEqual(newPhoneNumberTypeList[phoneNumberTypeIndex], oldPhoneNumberTypeList[oldPhoneNumberTypeList.Count - phoneNumberTypeIndex - 1]);
            }
        }
    }
}
