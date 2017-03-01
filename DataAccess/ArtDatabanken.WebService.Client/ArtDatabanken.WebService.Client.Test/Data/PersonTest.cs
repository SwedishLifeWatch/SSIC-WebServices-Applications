using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class PersonTest : TestBase
    {
        private Person _person;

        public PersonTest()
        {
            _person = null;
        }

        [TestMethod]
        public void Addresses()
        {
            Assert.IsTrue(GetPerson(true).Addresses.IsEmpty());
        }

        [TestMethod]
        public void AdministrationRoleId()
        {
            Assert.IsFalse(GetPerson(true).AdministrationRoleId.HasValue);
        }

        [TestMethod]
        public void BirthYear()
        {
            DateTime birthYear;

            Assert.IsFalse(GetPerson(true).BirthYear.HasValue);
            birthYear = new DateTime(2000, 1, 1);
            GetPerson().BirthYear = birthYear;
            Assert.IsTrue(GetPerson().BirthYear.HasValue);
            Assert.AreEqual(birthYear, GetPerson().BirthYear.Value);
        }

        [TestMethod]
        public void Constructor()
        {
            Person person;

            person = new Person(GetUserContext());
            Assert.IsNotNull(person);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetPerson(true).DataContext);
        }

        [TestMethod]
        public void DeathYear()
        {
            DateTime deathYear;

            Assert.IsFalse(GetPerson(true).DeathYear.HasValue);
            deathYear = new DateTime(2000, 1, 1);
            GetPerson().DeathYear = deathYear;
            Assert.IsTrue(GetPerson().DeathYear.HasValue);
            Assert.AreEqual(deathYear, GetPerson().DeathYear.Value);
        }

        [TestMethod]
        public void EmailAddress()
        {
            String emailAddress;

            emailAddress = null;
            GetPerson(true).EmailAddress = emailAddress;
            Assert.IsNull(GetPerson().EmailAddress);

            emailAddress = "";
            GetPerson().EmailAddress = emailAddress;
            Assert.AreEqual(GetPerson().EmailAddress, emailAddress);

            emailAddress = Settings.Default.TestUserName;
            GetPerson().EmailAddress = emailAddress;
            Assert.AreEqual(GetPerson().EmailAddress, emailAddress);
        }

        [TestMethod]
        public void FirstName()
        {
            String firstName;

            firstName = null;
            GetPerson(true).FirstName = firstName;
            Assert.IsNull(GetPerson().FirstName);

            firstName = "";
            GetPerson().FirstName = firstName;
            Assert.AreEqual(GetPerson().FirstName, firstName);

            firstName = Settings.Default.TestUserName;
            GetPerson().FirstName = firstName;
            Assert.AreEqual(GetPerson().FirstName, firstName);
        }

        [TestMethod]
        public void FullName()
        {
            String firstName;

            Assert.IsTrue(GetPerson(true).FullName.IsEmpty());

            firstName = Settings.Default.TestUserName;
            GetPerson().FirstName = firstName;
            Assert.IsFalse(GetPerson().FullName.IsEmpty());
            Assert.AreEqual(GetPerson().FullName, firstName);
        }

        [TestMethod]
        public void Gender()
        {
            Assert.IsNotNull(GetPerson(true).Gender);
        }

        private Person GetPerson()
        {
            return GetPerson(false);
        }

        private Person GetPerson(Boolean refresh)
        {
            if (_person.IsNull() || refresh)
            {
                _person = new Person(GetUserContext());
            }
            return _person;
        }

        public static Person GetOnePerson(IUserContext userContext)
        {
            return new Person(userContext);
        }

        [TestMethod]
        public void GUID()
        {
            Assert.IsNotNull(GetPerson(true).GUID);
        }

        [TestMethod]
        public void HasSpeciesCollection()
        {
            Boolean hasSpeciesCollection;

            hasSpeciesCollection = false;
            GetPerson(true).HasSpeciesCollection = hasSpeciesCollection;
            Assert.AreEqual(hasSpeciesCollection, GetPerson().HasSpeciesCollection);

            hasSpeciesCollection = true;
            GetPerson(true).HasSpeciesCollection = hasSpeciesCollection;
            Assert.AreEqual(hasSpeciesCollection, GetPerson().HasSpeciesCollection);
        }

        [TestMethod]
        public void Id()
        {
            Int32 id;

            id = GetPerson(true).Id;
            Assert.AreEqual(id, GetPerson().Id);
        }

        [TestMethod]
        public void LastName()
        {
            String lastName;

            lastName = null;
            GetPerson(true).LastName = lastName;
            Assert.IsNull(GetPerson().LastName);

            lastName = "";
            GetPerson().LastName = lastName;
            Assert.AreEqual(GetPerson().LastName, lastName);

            lastName = Settings.Default.TestUserName;
            GetPerson().LastName = lastName;
            Assert.AreEqual(GetPerson().LastName, lastName);
        }

        [TestMethod]
        public void Locale()
        {
            Assert.IsNotNull(GetPerson(true).Locale);
        }

        [TestMethod]
        public void MiddleName()
        {
            String middleName;

            middleName = null;
            GetPerson(true).MiddleName = middleName;
            Assert.IsNull(GetPerson().MiddleName);

            middleName = "";
            GetPerson().MiddleName = middleName;
            Assert.AreEqual(GetPerson().MiddleName, middleName);

            middleName = Settings.Default.TestUserName;
            GetPerson().MiddleName = middleName;
            Assert.AreEqual(GetPerson().MiddleName, middleName);
        }

        [TestMethod]
        public void PhoneNumbers()
        {
            Assert.IsTrue(GetPerson(true).PhoneNumbers.IsEmpty());
        }

        [TestMethod]
        public void Presentation()
        {
            String presentation;

            presentation = null;
            GetPerson(true).Presentation = presentation;
            Assert.IsNull(GetPerson().Presentation);

            presentation = "";
            GetPerson().Presentation = presentation;
            Assert.AreEqual(GetPerson().Presentation, presentation);

            presentation = Settings.Default.TestUserName;
            GetPerson().Presentation = presentation;
            Assert.AreEqual(GetPerson().Presentation, presentation);
        }

        [TestMethod]
        public void ShowAddresses()
        {
            Boolean showAddresses;

            showAddresses = false;
            GetPerson(true).ShowAddresses = showAddresses;
            Assert.AreEqual(showAddresses, GetPerson().ShowAddresses);

            showAddresses = true;
            GetPerson(true).ShowAddresses = showAddresses;
            Assert.AreEqual(showAddresses, GetPerson().ShowAddresses);
        }

        [TestMethod]
        public void ShowEmailAddress()
        {
            Boolean showEmailAddress;

            showEmailAddress = false;
            GetPerson(true).ShowEmailAddress = showEmailAddress;
            Assert.AreEqual(showEmailAddress, GetPerson().ShowEmailAddress);

            showEmailAddress = true;
            GetPerson(true).ShowEmailAddress = showEmailAddress;
            Assert.AreEqual(showEmailAddress, GetPerson().ShowEmailAddress);
        }

        [TestMethod]
        public void ShowPersonalInformation()
        {
            Boolean showPersonalInformation;

            showPersonalInformation = false;
            GetPerson(true).ShowPersonalInformation = showPersonalInformation;
            Assert.AreEqual(showPersonalInformation, GetPerson().ShowPersonalInformation);

            showPersonalInformation = true;
            GetPerson(true).ShowPersonalInformation = showPersonalInformation;
            Assert.AreEqual(showPersonalInformation, GetPerson().ShowPersonalInformation);
        }

        [TestMethod]
        public void ShowPhoneNumbers()
        {
            Boolean showPhoneNumbers;

            showPhoneNumbers = false;
            GetPerson(true).ShowPhoneNumbers = showPhoneNumbers;
            Assert.AreEqual(showPhoneNumbers, GetPerson().ShowPhoneNumbers);

            showPhoneNumbers = true;
            GetPerson(true).ShowPhoneNumbers = showPhoneNumbers;
            Assert.AreEqual(showPhoneNumbers, GetPerson().ShowPhoneNumbers);
        }

        [TestMethod]
        public void ShowPresentation()
        {
            Boolean showPresentation;

            showPresentation = false;
            GetPerson(true).ShowPresentation = showPresentation;
            Assert.AreEqual(showPresentation, GetPerson().ShowPresentation);

            showPresentation = true;
            GetPerson(true).ShowPresentation = showPresentation;
            Assert.AreEqual(showPresentation, GetPerson().ShowPresentation);
        }

        [TestMethod]
        public void TaxonNameTypeId()
        {
            Int32 taxonNameTypeId;

            taxonNameTypeId = GetPerson(true).TaxonNameTypeId;
            Assert.AreEqual(taxonNameTypeId, GetPerson().TaxonNameTypeId);
        }

        [TestMethod]
        public void UpdateInformation()
        {
            Assert.IsNotNull(GetPerson(true).UpdateInformation);
        }

        [TestMethod]
        public void URL()
        {
            String url;

            url = null;
            GetPerson(true).URL = url;
            Assert.IsNull(GetPerson().URL);

            url = "";
            GetPerson().URL = url;
            Assert.AreEqual(GetPerson().URL, url);

            url = Settings.Default.TestUserName;
            GetPerson().URL = url;
            Assert.AreEqual(GetPerson().URL, url);
        }
    }
}
