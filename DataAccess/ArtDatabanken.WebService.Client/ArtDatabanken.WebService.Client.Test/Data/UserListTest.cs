using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class UserListTest : TestBase
    {
        private UserList _users;

        public UserListTest()
        {
            _users = null;
        }

        [TestMethod]
        public void Constructor()
        {
            UserList users;

            users = new UserList();
            Assert.IsNotNull(users);
            users = new UserList(true);
            Assert.IsNotNull(users);
            users = new UserList(false);
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void Get()
        {
            foreach (IUser user in GetUsers(true))
            {
                Assert.AreEqual(user, GetUsers().Get(user.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 userId;

            userId = Int32.MinValue;
            GetUsers(true).Get(userId);
        }

        private UserList GetUsers()
        {
            return GetUsers(false);
        }

        private UserList GetUsers(Boolean refresh)
        {
            if (_users.IsNull() || refresh)
            {
                _users = new UserList();
                _users.Add(CoreData.UserManager.GetUser(GetUserContext()));
            }
            return _users;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            UserList newUserList, oldUserList;
            Int32 userIndex;

            oldUserList = GetUsers(true);
            newUserList = new UserList();
            for (userIndex = 0; userIndex < oldUserList.Count; userIndex++)
            {
                newUserList.Add(oldUserList[oldUserList.Count - userIndex - 1]);
            }
            for (userIndex = 0; userIndex < oldUserList.Count; userIndex++)
            {
                Assert.AreEqual(newUserList[userIndex], oldUserList[oldUserList.Count - userIndex - 1]);
            }
        }
    }
}
