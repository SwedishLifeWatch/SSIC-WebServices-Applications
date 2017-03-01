using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class PersonSearchCriteriaTest
    {
        private PersonSearchCriteria _searchCriteria;

        public PersonSearchCriteriaTest()
        {
            _searchCriteria = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PersonSearchCriteria searchCriteria;

            searchCriteria = new PersonSearchCriteria();
            Assert.IsNotNull(searchCriteria);
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

        private PersonSearchCriteria GetSearchCriteria()
        {
            return GetSearchCriteria(false);
        }

        private PersonSearchCriteria GetSearchCriteria(Boolean refresh)
        {
            if (_searchCriteria.IsNull() || refresh)
            {
                _searchCriteria = new PersonSearchCriteria();
            }
            return _searchCriteria;
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
    }
}
