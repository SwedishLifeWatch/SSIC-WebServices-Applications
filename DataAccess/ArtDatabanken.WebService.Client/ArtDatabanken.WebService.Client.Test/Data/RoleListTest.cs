using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class RoleListTest : TestBase
    {
        private RoleList _roles;

        public RoleListTest()
        {
            _roles = null;
        }

        [TestMethod]
        public void Constructor()
        {
            RoleList roles;

            roles = new RoleList();
            Assert.IsNotNull(roles);
        }

        [TestMethod]
        public void Get()
        {
            GetRoles(true);
            foreach (IRole role in GetRoles())
            {
                Assert.AreEqual(role, GetRoles().Get(role.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 roleId;

            roleId = Int32.MaxValue;
            GetRoles(true).Get(roleId);
        }

        private RoleList GetRoles()
        {
            return GetRoles(false);
        }

        private RoleList GetRoles(Boolean refresh)
        {
            if (_roles.IsNull() || refresh)
            {
                _roles = new RoleList();
                _roles.Add(RoleTest.GetOneRole(GetUserContext()));
            }
            return _roles;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            RoleList newRoleList, oldRoleList;
            Int32 roleIndex;

            oldRoleList = GetRoles(true);
            newRoleList = new RoleList();
            for (roleIndex = 0; roleIndex < oldRoleList.Count; roleIndex++)
            {
                newRoleList.Add(oldRoleList[oldRoleList.Count - roleIndex - 1]);
            }
            for (roleIndex = 0; roleIndex < oldRoleList.Count; roleIndex++)
            {
                Assert.AreEqual(newRoleList[roleIndex], oldRoleList[oldRoleList.Count - roleIndex - 1]);
            }
        }
    }
}
