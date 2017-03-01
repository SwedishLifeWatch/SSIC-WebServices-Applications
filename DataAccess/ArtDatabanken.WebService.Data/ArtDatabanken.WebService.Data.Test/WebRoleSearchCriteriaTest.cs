using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebRoleSearchCriteriaTest
    {
        private WebRoleSearchCriteria _searchCriteria;

        public WebRoleSearchCriteriaTest()
        {
            _searchCriteria = null;
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
        public void Constructor()
        {
            WebRoleSearchCriteria searchCriteria;

            searchCriteria = new WebRoleSearchCriteria();
            Assert.IsNotNull(searchCriteria);
        }


        private WebRoleSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private WebRoleSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new WebRoleSearchCriteria();
            }
            return _searchCriteria;
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
        public void IsIsValidSpecified()
        {
            Boolean isIsValidSpecified;

            isIsValidSpecified = false;
            GetSearchCriteria(true).IsIsValidSpecified = isIsValidSpecified;
            Assert.AreEqual(isIsValidSpecified, GetSearchCriteria().IsIsValidSpecified);

            isIsValidSpecified = true;
            GetSearchCriteria().IsIsValidSpecified = isIsValidSpecified;
            Assert.AreEqual(isIsValidSpecified, GetSearchCriteria().IsIsValidSpecified);
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
        public void IsValid()
        {
            Boolean isValid;

            isValid = false;
            GetSearchCriteria(true).IsValid = isValid;
            Assert.AreEqual(isValid, GetSearchCriteria().IsValid);

            isValid = true;
            GetSearchCriteria().IsValid = isValid;
            Assert.AreEqual(isValid, GetSearchCriteria().IsValid);
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
        public void RoleName()
        {
            String roleName;

            roleName = null;
            GetSearchCriteria(true).Name = roleName;
            Assert.IsNull(GetSearchCriteria().Name);
            roleName = String.Empty;
            GetSearchCriteria().Name = roleName;
            Assert.AreEqual(GetSearchCriteria().Name, roleName);
            roleName = "TestU";
            GetSearchCriteria().Name = roleName;
            Assert.AreEqual(GetSearchCriteria().Name, roleName);
        }

        [TestMethod]
        public void ShortName()
        {
            String shortName;

            shortName = null;
            GetSearchCriteria(true).ShortName = shortName;
            Assert.IsNull(GetSearchCriteria().ShortName);
            shortName = String.Empty;
            GetSearchCriteria().ShortName = shortName;
            Assert.AreEqual(GetSearchCriteria().ShortName, shortName);
            shortName = "TestU";
            GetSearchCriteria().ShortName = shortName;
            Assert.AreEqual(GetSearchCriteria().ShortName, shortName);
        }

    }
}
