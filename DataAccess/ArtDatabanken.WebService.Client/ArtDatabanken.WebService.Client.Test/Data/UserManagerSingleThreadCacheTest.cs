using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class UserManagerSingleThreadCacheTest : TestBase
    {
        private UserManagerSingleThreadCache _userManager;

        public UserManagerSingleThreadCacheTest()
        {
            _userManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            UserManagerSingleThreadCache userManager;

            userManager = new UserManagerSingleThreadCache();
            Assert.IsNotNull(userManager);
        }

        [TestMethod]
        public void GetAddressTypes()
        {
            AddressTypeList addressTypes;

            addressTypes = GetUserManager(true).GetAddressTypes(GetUserContext());
            Assert.IsTrue(addressTypes.IsNotEmpty());
        }

        private UserManagerSingleThreadCache GetUserManager()
        {
            return GetUserManager(false);
        }

        private UserManagerSingleThreadCache GetUserManager(Boolean refresh)
        {
            if (_userManager.IsNull() || refresh)
            {
                _userManager = new UserManagerSingleThreadCache();
                _userManager.DataSource = new UserDataSource();
            }
            return _userManager;
        }
    }
}
