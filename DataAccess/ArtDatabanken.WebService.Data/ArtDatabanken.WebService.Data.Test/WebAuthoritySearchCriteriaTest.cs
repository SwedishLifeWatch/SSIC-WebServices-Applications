using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Data.Test
{
    [TestClass]
    public class WebAuthoritySearchCriteriaTest
    {
        private WebAuthoritySearchCriteria _searchCriteria;

        public WebAuthoritySearchCriteriaTest()
        {
            _searchCriteria = null;
        }

      

        [TestMethod]
        public void Constructor()
        {
            WebAuthoritySearchCriteria searchCriteria;

            searchCriteria = new WebAuthoritySearchCriteria();
            Assert.IsNotNull(searchCriteria);
        }


        private WebAuthoritySearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private WebAuthoritySearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new WebAuthoritySearchCriteria();
            }
            return _searchCriteria;
        }

        [TestMethod]
        public void AuthorityIdentifier()
        {
            String authorityIdentifier;

            authorityIdentifier = null;
            GetSearchCriteria(true).AuthorityIdentifier = authorityIdentifier;
            Assert.IsNull(GetSearchCriteria().AuthorityIdentifier);
            authorityIdentifier = String.Empty;
            GetSearchCriteria().AuthorityIdentifier = authorityIdentifier;
            Assert.AreEqual(GetSearchCriteria().AuthorityIdentifier, authorityIdentifier);
            authorityIdentifier = "U%";
            GetSearchCriteria().AuthorityIdentifier = authorityIdentifier;
            Assert.AreEqual(GetSearchCriteria().AuthorityIdentifier, authorityIdentifier);
        }

        [TestMethod]
        public void ApplicationIdentifier()
        {
            String applicationIdentifier;

            applicationIdentifier = null;
            GetSearchCriteria(true).ApplicationIdentifier = applicationIdentifier;
            Assert.IsNull(GetSearchCriteria().ApplicationIdentifier);
            applicationIdentifier = String.Empty;
            GetSearchCriteria().ApplicationIdentifier = applicationIdentifier;
            Assert.AreEqual(GetSearchCriteria().ApplicationIdentifier, applicationIdentifier);
            applicationIdentifier = "UserService%";
            GetSearchCriteria().ApplicationIdentifier = applicationIdentifier;
            Assert.AreEqual(GetSearchCriteria().ApplicationIdentifier, applicationIdentifier);
        }

        [TestMethod]
        public void AuthorityDataTypeIdentifier()
        {
            String authorityDataTypeIdentifier;

            authorityDataTypeIdentifier = null;
            GetSearchCriteria(true).AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            Assert.IsNull(GetSearchCriteria().AuthorityDataTypeIdentifier);
            authorityDataTypeIdentifier = String.Empty;
            GetSearchCriteria().AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            Assert.AreEqual(GetSearchCriteria().AuthorityDataTypeIdentifier, authorityDataTypeIdentifier);
            authorityDataTypeIdentifier = "Obs%";
            GetSearchCriteria().AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            Assert.AreEqual(GetSearchCriteria().AuthorityDataTypeIdentifier, authorityDataTypeIdentifier);
        }

        [TestMethod]
        public void AuthorityName()
        {
            String authorityName;

            authorityName = null;
            GetSearchCriteria(true).AuthorityName = authorityName;
            Assert.IsNull(GetSearchCriteria().AuthorityName);
            authorityName = String.Empty;
            GetSearchCriteria().AuthorityName = authorityName;
            Assert.AreEqual(GetSearchCriteria().AuthorityName, authorityName);
            authorityName = "test%";
            GetSearchCriteria().AuthorityName = authorityName;
            Assert.AreEqual(GetSearchCriteria().AuthorityName, authorityName);
        }
    }
}
