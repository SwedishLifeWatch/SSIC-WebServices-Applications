using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Summary description for WebRoleTest
    /// </summary>
    [TestClass]
    public class WebRoleTest
    {
        private WebRole _role;

        public WebRoleTest()
        {
            _role = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebRole role;

            role = new WebRole();
            Assert.IsNotNull(role);
        }

        private WebRole GetRole()
        {
            return GetRole(false);
        }

        private WebRole GetRole(Boolean refresh)
        {
            if (_role.IsNull() || refresh)
            {
                _role = new WebRole();
            }
            return _role;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id = 100;
            GetRole(true).Id = id;
            Assert.AreEqual(id, GetRole().Id);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsNull(GetRole().Name);
            String name = "TestName";
            GetRole(true).Name = name;
            Assert.AreEqual(name, GetRole().Name);
        }

        [TestMethod]
        public void ShortName()
        {
            Assert.IsNull(GetRole().ShortName);
            String shortName = "TestShortName";
            GetRole(true).ShortName = shortName;
            Assert.AreEqual(shortName, GetRole().ShortName);
        }

        [TestMethod]
        public void AdministrationRoleId()
        {
            Int32 administrationRoleId = 99;
            GetRole(true).AdministrationRoleId = administrationRoleId;
            Assert.AreEqual(administrationRoleId, GetRole().AdministrationRoleId);
        }

        [TestMethod]
        public void Description()
        {
            Assert.IsNull(GetRole().Description);
            String value = "DescriptionTest " +
            "DescriptionTest2";
            GetRole(true).Description = value;
            Assert.AreEqual(value, GetRole().Description);
        }

        [TestMethod]
        public void IsActivationRequired()
        {
            Assert.IsNotNull(GetRole().IsActivationRequired);
            Boolean value = true;
            GetRole(true).IsActivationRequired = value;
            Assert.AreEqual(value, GetRole().IsActivationRequired);
        }

        [TestMethod]
        public void MessageTypeId()
        {
            Assert.IsNotNull(GetRole().MessageTypeId);
            Int32 value = 2;
            GetRole(true).MessageTypeId = value;
            Assert.AreEqual(value, GetRole().MessageTypeId);
        }

        [TestMethod]
        public void Authorities()
        {
            Assert.IsNull(GetRole().Authorities);
            List<WebAuthority> authorities = new List<WebAuthority>();
            for (int i = 1; i <= 5; i++)
            {
                WebAuthority authority = new WebAuthority();
                authority.Id = i;
                authority.Identifier = "TestIdentity";
                authorities.Add(authority);
            }
            GetRole(true).Authorities = authorities;
            Assert.AreEqual(authorities, GetRole().Authorities);
        }

        [TestMethod]
        public void IsUserAdministrationRole()
        {
            Boolean isUserAdministrationRole = true;
            GetRole(true).IsUserAdministrationRole = isUserAdministrationRole;
            Assert.AreEqual(isUserAdministrationRole, GetRole().IsUserAdministrationRole);
        }


        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion


    }
}
