using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class AuthorityDataTypeListTest : TestBase
    {
        private AuthorityDataTypeList _authorityDataTypes;

        public AuthorityDataTypeListTest()
        {
            _authorityDataTypes = null;
        }

        [TestMethod]
        public void Constructor()
        {
            AuthorityDataTypeList addressTypes;

            addressTypes = new AuthorityDataTypeList();
            Assert.IsNotNull(addressTypes);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 addressTypeId;

            addressTypeId = Int32.MinValue;
            GetAuthorityDataTypes(true).Get(addressTypeId);
        }

        private AuthorityDataTypeList GetAuthorityDataTypes()
        {
            return GetAuthorityDataTypes(false);
        }

        private AuthorityDataTypeList GetAuthorityDataTypes(Boolean refresh)
        {
            if (_authorityDataTypes.IsNull() || refresh)
            {
                _authorityDataTypes = CoreData.UserManager.GetAuthorityDataTypes(GetUserContext());
            }
            return _authorityDataTypes;
        }

        private AuthorityDataTypeList GetAuthorityDataTypesByApplicationId()
        {
            return GetAuthorityDataTypesByApplicationId(false);
        }

        private AuthorityDataTypeList GetAuthorityDataTypesByApplicationId(Boolean refresh)
        {
            if (_authorityDataTypes.IsNull() || refresh)
            {
                _authorityDataTypes = CoreData.UserManager.GetAuthorityDataTypesByApplicationId(GetUserContext(), Settings.Default.TestApplicationId);
            }
            return _authorityDataTypes;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            AuthorityDataTypeList newAuthorityDataTypeList, oldAuthorityDataTypeList;
            Int32 authorityDataTypeIndex;

            // Test all authorityDataTypes
            oldAuthorityDataTypeList = GetAuthorityDataTypes(true);
            newAuthorityDataTypeList = new AuthorityDataTypeList();
            for (authorityDataTypeIndex = 0; authorityDataTypeIndex < oldAuthorityDataTypeList.Count; authorityDataTypeIndex++)
            {
                newAuthorityDataTypeList.Add(oldAuthorityDataTypeList[oldAuthorityDataTypeList.Count - authorityDataTypeIndex - 1]);
            }
            for (authorityDataTypeIndex = 0; authorityDataTypeIndex < oldAuthorityDataTypeList.Count; authorityDataTypeIndex++)
            {
                Assert.AreEqual(newAuthorityDataTypeList[authorityDataTypeIndex], oldAuthorityDataTypeList[oldAuthorityDataTypeList.Count - authorityDataTypeIndex - 1]);
            }

            // Test all selectecd by application id authorityDataTypes
            oldAuthorityDataTypeList = GetAuthorityDataTypesByApplicationId(true);
            newAuthorityDataTypeList = new AuthorityDataTypeList();
            for (authorityDataTypeIndex = 0; authorityDataTypeIndex < oldAuthorityDataTypeList.Count; authorityDataTypeIndex++)
            {
                newAuthorityDataTypeList.Add(oldAuthorityDataTypeList[oldAuthorityDataTypeList.Count - authorityDataTypeIndex - 1]);
            }
            for (authorityDataTypeIndex = 0; authorityDataTypeIndex < oldAuthorityDataTypeList.Count; authorityDataTypeIndex++)
            {
                Assert.AreEqual(newAuthorityDataTypeList[authorityDataTypeIndex], oldAuthorityDataTypeList[oldAuthorityDataTypeList.Count - authorityDataTypeIndex - 1]);
            }
        }
    }
}
