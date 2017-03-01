using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class AuthorityTest : TestBase
    {
        Authority _authority;

        public AuthorityTest()
        {
            _authority = null;
        }

        [TestMethod]
        public void Constructor()
        {
            Authority authority;

            authority = new Authority(GetUserContext());
            Assert.IsNotNull(authority);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetAuthority(true).DataContext);
        }

        public static Authority GetOneAuthority(IUserContext userContext)
        {
            return new Authority(userContext);
        }

        private Authority GetAuthority()
        {
            return GetAuthority(false);
        }

        private Authority GetAuthority(Boolean refresh)
        {
            if (_authority.IsNull() || refresh)
            {
                _authority = new Authority(GetUserContext());
            }
            return _authority;
        }

        [TestMethod]
        public void AuthorityIdentity()
        {
            String authorityIdentity;

            authorityIdentity = null;
            GetAuthority(true).Identifier = authorityIdentity;
            Assert.IsNull(GetAuthority().Identifier);

            authorityIdentity = "";
            GetAuthority().Identifier = authorityIdentity;
            Assert.AreEqual(GetAuthority().Identifier, authorityIdentity);

            authorityIdentity = @"AuthorityIdentity";
            GetAuthority().Identifier = authorityIdentity;
            Assert.AreEqual(GetAuthority().Identifier, authorityIdentity);
        }

        [TestMethod]
        public void GetApplicationActions()
        {
            ApplicationActionList applicationActionList;
            List<Int32> applicationActionIdList = new List<Int32>();

            applicationActionList = new ApplicationActionList();
            applicationActionIdList.Add(3);
            applicationActionIdList.Add(4);

            applicationActionList = GetAuthority(true).GetApplicationActionsByIdList(GetUserContext(), applicationActionIdList);
            Assert.IsTrue(applicationActionList.IsNotEmpty());
            Assert.IsInstanceOfType(applicationActionList[0], typeof(ApplicationAction));
            Assert.IsTrue(applicationActionList.Count > 1);

        }
    }
}
