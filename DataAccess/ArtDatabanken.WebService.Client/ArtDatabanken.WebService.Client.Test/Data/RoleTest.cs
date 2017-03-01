using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RoleTest : TestBase
    {
        Role _role;

        public RoleTest()
        {
            _role = null;
        }

        [TestMethod]
        public void Constructor()
        {
            Role role;

            role = new Role(GetUserContext());
            Assert.IsNotNull(role);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetRole(true).DataContext);
        }

        public static Role GetOneRole(IUserContext userContext)
        {
            return new Role(userContext);
        }

        private Role GetRole()
        {
            return GetRole(false);
        }

        private Role GetRole(Boolean refresh)
        {
            if (_role.IsNull() || refresh)
            {
                _role = new Role(GetUserContext());
            }
            return _role;
        }

        [TestMethod]
        public void RoleName()
        {
            String roleName;

            roleName = null;
            GetRole(true).Name = roleName;
            Assert.IsNull(GetRole().Name);

            roleName = "";
            GetRole().Name = roleName;
            Assert.AreEqual(GetRole().Name, roleName);

            roleName = Settings.Default.TestRoleName;
            GetRole().Name = roleName;
            Assert.AreEqual(GetRole().Name, roleName);
        }
    }
}
