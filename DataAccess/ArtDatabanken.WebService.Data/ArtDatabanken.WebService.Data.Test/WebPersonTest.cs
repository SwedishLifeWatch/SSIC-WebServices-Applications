using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Summary description for WebPersonTest
    /// </summary>
    [TestClass]
    public class WebPersonTest
    {
        private WebPerson _person;

        public WebPersonTest()
        {
            _person = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebPerson person;

            person = new WebPerson();
            Assert.IsNotNull(person);
        }

        private WebPerson GetPerson()
        {
            return GetPerson(false);
        }

        private WebPerson GetPerson(Boolean refresh)
        {
            if (_person.IsNull() || refresh)
            {
                _person = new WebPerson();
            }
            return _person;
        }


        [TestMethod]
        public void FirstName()
        {
            Assert.IsNull(GetPerson().FirstName);
            String firstName = "TestName";
            GetPerson(true).FirstName = firstName;
            Assert.AreEqual(firstName, GetPerson().FirstName);
        }

        [TestMethod]
        public void LastName()
        {
            Assert.IsNull(GetPerson().LastName);
            String lastName = "TestlastName";
            GetPerson(true).LastName = lastName;
            Assert.AreEqual(lastName, GetPerson().LastName);
        }

        [TestMethod]
        public void MiddleName()
        {
            Assert.IsNull(GetPerson().MiddleName);
            String value = "TesMiddleName";
            GetPerson(true).MiddleName = value;
            Assert.AreEqual(value, GetPerson().MiddleName);
        }

        [TestMethod]
        public void PersonId()
        {
            Int32 personId = 0;
            GetPerson().Id = personId;
            Assert.AreEqual(GetPerson().Id, personId);
            personId = 42;
            GetPerson().Id = personId;
            Assert.AreEqual(GetPerson().Id, personId);
        }

        [TestMethod]
        public void Presentation()
        {
            Assert.IsNull(GetPerson().Presentation);
            String value = "PresentationTest "+
            "PresentationTest2";
            GetPerson(true).Presentation = value;
            Assert.AreEqual(value, GetPerson().Presentation);
        }

        [TestMethod]
        public void ShowPresentation()
        {
            Assert.IsFalse(GetPerson().ShowPresentation);
            Boolean value = true;
            GetPerson(true).ShowPresentation = value;
            Assert.AreEqual(value, GetPerson().ShowPresentation);
            Assert.IsTrue(GetPerson().ShowPresentation);
        }

        [TestMethod]
        public void ShowAddresses()
        {
            Assert.IsFalse(GetPerson().ShowAddresses);
            Boolean value = true;
            GetPerson(true).ShowAddresses = value;
            Assert.AreEqual(value, GetPerson().ShowAddresses);
            Assert.IsTrue(GetPerson().ShowAddresses);
        }

        [TestMethod]
        public void ShowPhoneNumbers()
        {
            Assert.IsFalse(GetPerson().ShowPhoneNumbers);
            Boolean value = true;
            GetPerson(true).ShowPhoneNumbers = value;
            Assert.AreEqual(value, GetPerson().ShowPhoneNumbers);
            Assert.IsTrue(GetPerson().ShowPhoneNumbers);
        }

        [TestMethod]
        public void TaxonNameTypeId()
        {
            Int32 taxonNameLanguageId = 1;
            GetPerson().TaxonNameTypeId = taxonNameLanguageId;
            Assert.AreEqual(GetPerson().TaxonNameTypeId, taxonNameLanguageId);
            taxonNameLanguageId = 42;
            GetPerson().TaxonNameTypeId = taxonNameLanguageId;
            Assert.AreEqual(GetPerson().TaxonNameTypeId, taxonNameLanguageId);
        }

        [TestMethod]
        public void DeathYear()
        {
            DateTime deathYear;

            deathYear = DateTime.Now;
            GetPerson().DeathYear = deathYear;
            Assert.AreEqual(deathYear, GetPerson().DeathYear);
        }

        [TestMethod]
        public void Addresses()
        {
            Assert.IsNull(GetPerson().Addresses);
            List<WebAddress> addresses = new List<WebAddress>();
            for (int i = 1; i <= 5; i++)
            {
                WebAddress address = new WebAddress();
                address.PostalAddress1 = "Agatan1" + i;
                addresses.Add(address);
            }
            GetPerson(true).Addresses = addresses;
            Assert.AreEqual(addresses, GetPerson().Addresses);
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
