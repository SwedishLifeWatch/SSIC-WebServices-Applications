using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Summary description for WebOrganizationTest
    /// </summary>
    [TestClass]
    public class WebOrganizationTest
    {
        private WebOrganization _organization;

        public WebOrganizationTest()
        {
            _organization = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebOrganization organization;

            organization = new WebOrganization();
            Assert.IsNotNull(organization);
        }

        private WebOrganization GetOrganization()
        {
            return GetOrganization(false);
        }

        private WebOrganization GetOrganization(Boolean refresh)
        {
            if (_organization.IsNull() || refresh)
            {
                _organization = new WebOrganization();
            }
            return _organization;
        }

        [TestMethod]
        public void Id()
        {
            Int32 id = 100;
            GetOrganization(true).Id = id;
            Assert.AreEqual(id, GetOrganization().Id);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsNull(GetOrganization().Name);
            String name = "TestName";
            GetOrganization(true).Name = name;
            Assert.AreEqual(name, GetOrganization().Name);
        }

        [TestMethod]
        public void ShortName()
        {
            Assert.IsNull(GetOrganization().ShortName);
            String shortName = "TestShortName";
            GetOrganization(true).ShortName = shortName;
            Assert.AreEqual(shortName, GetOrganization().ShortName);
        }

        [TestMethod]
        public void AdministrationRoleId()
        {
            Int32 administrationRoleId = 99;
            GetOrganization(true).AdministrationRoleId = administrationRoleId;
            Assert.AreEqual(administrationRoleId, GetOrganization().AdministrationRoleId);
        }

        [TestMethod]
        public void Description()
        {
            Assert.IsNull(GetOrganization().Description);
            String value = "DescriptionTest " +
            "DescriptionTest2";
            GetOrganization(true).Description = value;
            Assert.AreEqual(value, GetOrganization().Description);
        }

        [TestMethod]
        public void Addresses()
        {
            Assert.IsNull(GetOrganization().Addresses);
            List<WebAddress> addresses = new List<WebAddress>();
            for (int i = 1; i <= 5; i++)
            {
                WebAddress address = new WebAddress();
                address.PostalAddress1 = "Agatan1" + i;
                addresses.Add(address);
            }
            GetOrganization(true).Addresses = addresses;
            Assert.AreEqual(addresses, GetOrganization().Addresses);
        }

        [TestMethod]
        public void PhoneNumbers()
        {
            Assert.IsNull(GetOrganization().PhoneNumbers);
            List<WebPhoneNumber> phoneNumbers = new List<WebPhoneNumber>();
            for (int i = 1; i <= 5; i++)
            {
                WebPhoneNumber phoneNumber = new WebPhoneNumber();
                phoneNumber.Number = "018123456" + i;
                phoneNumbers.Add(phoneNumber);
            }
            GetOrganization(true).PhoneNumbers = phoneNumbers;
            Assert.AreEqual(phoneNumbers, GetOrganization().PhoneNumbers);
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
