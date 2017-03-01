using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class AuthoritySearchCriteriaTest
    {
        private AuthoritySearchCriteria _searchCriteria;

        public AuthoritySearchCriteriaTest()
        {
            _searchCriteria = null;
        }

        [TestMethod]
        public void Constructor()
        {
            AuthoritySearchCriteria searchCriteria;

            searchCriteria = new AuthoritySearchCriteria();
            Assert.IsNotNull(searchCriteria);
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
            authorityIdentifier = "U";
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

            applicationIdentifier = "UserService";
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
            authorityDataTypeIdentifier = "Obs";
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
            authorityName = "test";
            GetSearchCriteria().AuthorityName = authorityName;
            Assert.AreEqual(GetSearchCriteria().AuthorityName, authorityName);
            authorityName = "test%";
            GetSearchCriteria().AuthorityName = authorityName;
            Assert.AreEqual(GetSearchCriteria().AuthorityName, authorityName);
        }

        private AuthoritySearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private AuthoritySearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new AuthoritySearchCriteria();
            }
            return _searchCriteria;
        }
    }
}
