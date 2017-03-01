using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebPersonSearchCriteriaTest
    {
        private WebPersonSearchCriteria _searchCriteria;

        public WebPersonSearchCriteriaTest()
        {
            _searchCriteria = null;
        }

        [TestMethod]
        public void Address()
        {
            String address;

            address = null;
            GetSearchCriteria(true).Address = address;
            Assert.IsNull(GetSearchCriteria().Address);
            address = String.Empty;
            GetSearchCriteria().Address = address;
            Assert.AreEqual(GetSearchCriteria().Address, address);
            address = "TestU";
            GetSearchCriteria().Address = address;
            Assert.AreEqual(GetSearchCriteria().Address, address);
        }

        [TestMethod]
        public void ApplicationActionId()
        {
            Int32 applicationActionId;

            applicationActionId = 10;
            GetSearchCriteria(true).ApplicationActionId = applicationActionId;
            Assert.AreEqual(applicationActionId, GetSearchCriteria().ApplicationActionId);
        }

        [TestMethod]
        public void ApplicationId()
        {
            Int32 applicationId;

            applicationId = 10;
            GetSearchCriteria(true).ApplicationId = applicationId;
            Assert.AreEqual(applicationId, GetSearchCriteria().ApplicationId);
        }

        [TestMethod]
        public void City()
        {
            String city;

            city = null;
            GetSearchCriteria(true).City = city;
            Assert.IsNull(GetSearchCriteria().City);
            city = String.Empty;
            GetSearchCriteria().City = city;
            Assert.AreEqual(GetSearchCriteria().City, city);
            city = "TestU";
            GetSearchCriteria().City = city;
            Assert.AreEqual(GetSearchCriteria().City, city);
        }

        [TestMethod]
        public void Constructor()
        {
            WebPersonSearchCriteria searchCriteria;

            searchCriteria = new WebPersonSearchCriteria();
            Assert.IsNotNull(searchCriteria);
        }

        [TestMethod]
        public void CountryISOCode()
        {
            String countryISOCode;

            countryISOCode = null;
            GetSearchCriteria(true).CountryISOCode = countryISOCode;
            Assert.IsNull(GetSearchCriteria().CountryISOCode);
            countryISOCode = String.Empty;
            GetSearchCriteria().CountryISOCode = countryISOCode;
            Assert.AreEqual(GetSearchCriteria().CountryISOCode, countryISOCode);
            countryISOCode = "TestU";
            GetSearchCriteria().CountryISOCode = countryISOCode;
            Assert.AreEqual(GetSearchCriteria().CountryISOCode, countryISOCode);
        }

        [TestMethod]
        public void EmailAddress()
        {
            String emailAddress;

            emailAddress = null;
            GetSearchCriteria(true).EmailAddress = emailAddress;
            Assert.IsNull(GetSearchCriteria().EmailAddress);
            emailAddress = String.Empty;
            GetSearchCriteria().EmailAddress = emailAddress;
            Assert.AreEqual(GetSearchCriteria().EmailAddress, emailAddress);
            emailAddress = "TestU";
            GetSearchCriteria().EmailAddress = emailAddress;
            Assert.AreEqual(GetSearchCriteria().EmailAddress, emailAddress);
        }

        [TestMethod]
        public void FirstName()
        {
            String firstName;

            firstName = null;
            GetSearchCriteria(true).FirstName = firstName;
            Assert.IsNull(GetSearchCriteria().FirstName);
            firstName = String.Empty;
            GetSearchCriteria().FirstName = firstName;
            Assert.AreEqual(GetSearchCriteria().FirstName, firstName);
            firstName = "TestU";
            GetSearchCriteria().FirstName = firstName;
            Assert.AreEqual(GetSearchCriteria().FirstName, firstName);
        }

        [TestMethod]
        public void FullName()
        {
            String fullName;

            fullName = null;
            GetSearchCriteria(true).FullName = fullName;
            Assert.IsNull(GetSearchCriteria().FullName);
            fullName = String.Empty;
            GetSearchCriteria().FullName = fullName;
            Assert.AreEqual(GetSearchCriteria().FullName, fullName);
            fullName = "TestU";
            GetSearchCriteria().FullName = fullName;
            Assert.AreEqual(GetSearchCriteria().FullName, fullName);
        }

        private WebPersonSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private WebPersonSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new WebPersonSearchCriteria();
            }
            return _searchCriteria;
        }

        [TestMethod]
        public void HasSpeciesCollection()
        {
            Boolean hasSpeciesCollection;

            hasSpeciesCollection = false;
            GetSearchCriteria(true).HasSpeciesCollection = hasSpeciesCollection;
            Assert.AreEqual(hasSpeciesCollection, GetSearchCriteria().HasSpeciesCollection);

            hasSpeciesCollection = true;
            GetSearchCriteria().HasSpeciesCollection = hasSpeciesCollection;
            Assert.AreEqual(hasSpeciesCollection, GetSearchCriteria().HasSpeciesCollection);
        }

        [TestMethod]
        public void HasUser()
        {
            Boolean hasUser;

            hasUser = false;
            GetSearchCriteria(true).HasUser = hasUser;
            Assert.AreEqual(hasUser, GetSearchCriteria().HasUser);

            hasUser = true;
            GetSearchCriteria().HasUser = hasUser;
            Assert.AreEqual(hasUser, GetSearchCriteria().HasUser);
        }

        [TestMethod]
        public void IsApplicationActionIdSpecified()
        {
            Boolean isApplicationActionIdSpecified;

            isApplicationActionIdSpecified = false;
            GetSearchCriteria(true).IsApplicationActionIdSpecified = isApplicationActionIdSpecified;
            Assert.AreEqual(isApplicationActionIdSpecified, GetSearchCriteria().IsApplicationActionIdSpecified);

            isApplicationActionIdSpecified = true;
            GetSearchCriteria().IsApplicationActionIdSpecified = isApplicationActionIdSpecified;
            Assert.AreEqual(isApplicationActionIdSpecified, GetSearchCriteria().IsApplicationActionIdSpecified);
        }

        [TestMethod]
        public void IsApplicationIdSpecified()
        {
            Boolean isApplicationIdSpecified;

            isApplicationIdSpecified = false;
            GetSearchCriteria(true).IsApplicationIdSpecified = isApplicationIdSpecified;
            Assert.AreEqual(isApplicationIdSpecified, GetSearchCriteria().IsApplicationIdSpecified);

            isApplicationIdSpecified = true;
            GetSearchCriteria().IsApplicationIdSpecified = isApplicationIdSpecified;
            Assert.AreEqual(isApplicationIdSpecified, GetSearchCriteria().IsApplicationIdSpecified);
        }

        [TestMethod]
        public void IsHasSpeciesCollectionSpecified()
        {
            Boolean isHasSpeciesCollectionSpecified;

            isHasSpeciesCollectionSpecified = false;
            GetSearchCriteria(true).IsHasSpeciesCollectionSpecified = isHasSpeciesCollectionSpecified;
            Assert.AreEqual(isHasSpeciesCollectionSpecified, GetSearchCriteria().IsHasSpeciesCollectionSpecified);

            isHasSpeciesCollectionSpecified = true;
            GetSearchCriteria().IsHasSpeciesCollectionSpecified = isHasSpeciesCollectionSpecified;
            Assert.AreEqual(isHasSpeciesCollectionSpecified, GetSearchCriteria().IsHasSpeciesCollectionSpecified);
        }

        [TestMethod]
        public void IsHasUserSpecified()
        {
            Boolean isHasUserSpecified;

            isHasUserSpecified = false;
            GetSearchCriteria(true).IsHasUserSpecified = isHasUserSpecified;
            Assert.AreEqual(isHasUserSpecified, GetSearchCriteria().IsHasUserSpecified);

            isHasUserSpecified = true;
            GetSearchCriteria().IsHasUserSpecified = isHasUserSpecified;
            Assert.AreEqual(isHasUserSpecified, GetSearchCriteria().IsHasUserSpecified);
        }

        [TestMethod]
        public void IsOrganizationCategoryIdSpecified()
        {
            Boolean isOrganizationCategoryIdSpecified;

            isOrganizationCategoryIdSpecified = false;
            GetSearchCriteria(true).IsOrganizationCategoryIdSpecified = isOrganizationCategoryIdSpecified;
            Assert.AreEqual(isOrganizationCategoryIdSpecified, GetSearchCriteria().IsOrganizationCategoryIdSpecified);

            isOrganizationCategoryIdSpecified = true;
            GetSearchCriteria().IsOrganizationCategoryIdSpecified = isOrganizationCategoryIdSpecified;
            Assert.AreEqual(isOrganizationCategoryIdSpecified, GetSearchCriteria().IsOrganizationCategoryIdSpecified);
        }

        [TestMethod]
        public void IsOrganizationIdSpecified()
        {
            Boolean isOrganizationIdSpecified;

            isOrganizationIdSpecified = false;
            GetSearchCriteria(true).IsOrganizationIdSpecified = isOrganizationIdSpecified;
            Assert.AreEqual(isOrganizationIdSpecified, GetSearchCriteria().IsOrganizationIdSpecified);

            isOrganizationIdSpecified = true;
            GetSearchCriteria().IsOrganizationIdSpecified = isOrganizationIdSpecified;
            Assert.AreEqual(isOrganizationIdSpecified, GetSearchCriteria().IsOrganizationIdSpecified);
        }

        [TestMethod]
        public void IsRoleIdSpecified()
        {
            Boolean isRoleIdSpecified;

            isRoleIdSpecified = false;
            GetSearchCriteria(true).IsRoleIdSpecified = isRoleIdSpecified;
            Assert.AreEqual(isRoleIdSpecified, GetSearchCriteria().IsRoleIdSpecified);

            isRoleIdSpecified = true;
            GetSearchCriteria().IsRoleIdSpecified = isRoleIdSpecified;
            Assert.AreEqual(isRoleIdSpecified, GetSearchCriteria().IsRoleIdSpecified);
        }

        [TestMethod]
        public void IsUserTypeSpecified()
        {
            Boolean isUserTypeSpecified;

            isUserTypeSpecified = false;
            GetSearchCriteria(true).IsUserTypeSpecified = isUserTypeSpecified;
            Assert.AreEqual(isUserTypeSpecified, GetSearchCriteria().IsUserTypeSpecified);

            isUserTypeSpecified = true;
            GetSearchCriteria().IsUserTypeSpecified = isUserTypeSpecified;
            Assert.AreEqual(isUserTypeSpecified, GetSearchCriteria().IsUserTypeSpecified);
        }

        [TestMethod]
        public void LastName()
        {
            String lastName;

            lastName = null;
            GetSearchCriteria(true).LastName = lastName;
            Assert.IsNull(GetSearchCriteria().LastName);
            lastName = String.Empty;
            GetSearchCriteria().LastName = lastName;
            Assert.AreEqual(GetSearchCriteria().LastName, lastName);
            lastName = "TestU";
            GetSearchCriteria().LastName = lastName;
            Assert.AreEqual(GetSearchCriteria().LastName, lastName);
        }

        [TestMethod]
        public void MiddleName()
        {
            String middleName;

            middleName = null;
            GetSearchCriteria(true).MiddleName = middleName;
            Assert.IsNull(GetSearchCriteria().MiddleName);
            middleName = String.Empty;
            GetSearchCriteria().MiddleName = middleName;
            Assert.AreEqual(GetSearchCriteria().MiddleName, middleName);
            middleName = "TestU";
            GetSearchCriteria().MiddleName = middleName;
            Assert.AreEqual(GetSearchCriteria().MiddleName, middleName);
        }

        [TestMethod]
        public void OrganizationCategoryId()
        {
            Int32 organizationCategoryId;

            organizationCategoryId = 10;
            GetSearchCriteria(true).OrganizationCategoryId = organizationCategoryId;
            Assert.AreEqual(organizationCategoryId, GetSearchCriteria().OrganizationCategoryId);
        }

        [TestMethod]
        public void OrganizationId()
        {
            Int32 organizationId;

            organizationId = 10;
            GetSearchCriteria(true).OrganizationId = organizationId;
            Assert.AreEqual(organizationId, GetSearchCriteria().OrganizationId);
        }

        [TestMethod]
        public void PhoneNumber()
        {
            String phoneNumber;

            phoneNumber = null;
            GetSearchCriteria(true).PhoneNumber = phoneNumber;
            Assert.IsNull(GetSearchCriteria().PhoneNumber);
            phoneNumber = String.Empty;
            GetSearchCriteria().PhoneNumber = phoneNumber;
            Assert.AreEqual(phoneNumber, GetSearchCriteria().PhoneNumber);
            phoneNumber = "TestU";
            GetSearchCriteria().PhoneNumber = phoneNumber;
            Assert.AreEqual(phoneNumber, GetSearchCriteria().PhoneNumber);
        }

        [TestMethod]
        public void RoleId()
        {
            Int32 roleId;

            roleId = 10;
            GetSearchCriteria(true).RoleId = roleId;
            Assert.AreEqual(roleId, GetSearchCriteria().RoleId);
        }

        [TestMethod]
        public void UserType()
        {
            GetSearchCriteria(true);
            foreach (UserType userType in Enum.GetValues(typeof(UserType)))
            {
                GetSearchCriteria().UserType = userType;
                Assert.AreEqual(userType, GetSearchCriteria().UserType);
            }
        }

        [TestMethod]
        public void ZipCode()
        {
            String zipCode;

            zipCode = null;
            GetSearchCriteria(true).ZipCode = zipCode;
            Assert.IsNull(GetSearchCriteria().ZipCode);
            zipCode = String.Empty;
            GetSearchCriteria().ZipCode = zipCode;
            Assert.AreEqual(zipCode, GetSearchCriteria().ZipCode);
            zipCode = "TestU";
            GetSearchCriteria().ZipCode = zipCode;
            Assert.AreEqual(zipCode, GetSearchCriteria().ZipCode);
        }
    }
}
