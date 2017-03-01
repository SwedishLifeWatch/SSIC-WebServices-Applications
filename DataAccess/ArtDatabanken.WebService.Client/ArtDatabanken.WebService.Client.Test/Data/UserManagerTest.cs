using System;
using System.ServiceModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class UserManagerTest : TestBase
    {
        private UserManager _userManager;

        public UserManagerTest()
        {
            _userManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            UserManager userManager;

            userManager = new UserManager();
            Assert.IsNotNull(userManager);
        }

        [TestMethod]
        public void ActivateRoleMembership()
        {
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                Assert.IsTrue(GetUserManager(true).ActivateRoleMembership(GetUserContext(), 1));
                Assert.IsFalse(GetUserManager(true).ActivateRoleMembership(GetUserContext(), 100));
            }
        }

        [TestMethod]
        public void ActivateUserAccount()
        {
            String activationKey = "6DFR8QI7IqrViqBQ1PhP4RKbCfla6n";
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                Assert.IsTrue(GetUserManager(true).ActivateUserAccount(GetUserContext(), Settings.Default.TestUserName, activationKey));
            }
        }

        [TestMethod]
        public void AddUserToRole()
        {
            Int32 userId;
            Int32 roleId = Settings.Default.TestRoleId;
            IUser user;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetOneUser();
                userId = user.Id;
                GetUserManager(true).AddUserToRole(GetUserContext(), roleId, userId);
            }
        }


        [TestMethod]
        public void ApplicationActionExists()
        {
            String applicationActionIdentifier;
            applicationActionIdentifier = Settings.Default.TestApplicationActionIdentifier;
            Assert.IsTrue(GetUserManager(true).ApplicationActionExists(GetUserContext(), applicationActionIdentifier));

            // Test non existing action identifier
            applicationActionIdentifier = "NonExistingIdentifier";
            Assert.IsFalse(GetUserManager(true).ApplicationActionExists(GetUserContext(), applicationActionIdentifier));
        }

        [TestMethod]
        public void ApplicationActionExistsInRole()
        {
            String applicationActionIdentifier;
            Role role;
            role = (Role)GetUserManager().GetRole(GetUserContext(), Test.Settings.Default.TestRoleId);

            applicationActionIdentifier = Settings.Default.TestApplicationActionIdentifier;
            Assert.IsTrue(GetUserManager(true).ApplicationActionExists(GetUserContext(), role, applicationActionIdentifier));

            // Test non existing action identifier
            applicationActionIdentifier = "NonExistingIdentifier";
            Assert.IsFalse(GetUserManager(true).ApplicationActionExists(GetUserContext(), role, applicationActionIdentifier));
        }

        [TestMethod]
        public void CheckStringIsUnique()
        {
            String value, objectName, propertyName;
            Boolean isUnique;
            objectName = "Application";
            propertyName = "Name";
            // Check unique value
            value = "Test";
            isUnique = GetUserManager(true).CheckStringIsUnique(GetUserContext(), value, objectName, propertyName);
            Assert.IsTrue(isUnique);

            // Check not unique value
            value = "Artportalen";
            isUnique = GetUserManager(true).CheckStringIsUnique(GetUserContext(), value, objectName, propertyName);
            Assert.IsFalse(isUnique);

        }


        [TestMethod]
        public void CreateAuthority()
        {
            Boolean createPermission, readPermission, deletePermission, updatePermission,
                    showNonPublicData;
            DateTime validFromDate, validToDate;
            List<String> actions, regions, factors, taxa, projects, localities;
            Int32 administrationRoleId, maxProtectionLevel;
            IAuthority authority;
            String authorityIdentity, description, name, obligation;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction(1000))
            {
                authority = GetNewAuthority();
                GetUserManager(true).CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, authority.UpdateInformation.CreatedBy);
                Assert.AreEqual(authority.UpdateInformation.ModifiedBy, authority.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - authority.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test DataContext.
                Assert.IsNotNull(authority.DataContext);

                // Test GUID.
                Assert.IsTrue(authority.GUID.IsNotEmpty());

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, authority.Id);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, authority.UpdateInformation.ModifiedBy);
                Assert.AreEqual(authority.UpdateInformation.ModifiedBy, authority.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - authority.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }

            // Test actions.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                actions = new List<String>();
                actions.Add("1");
                actions.Add("2");
                actions.Add("3");
                authority.ActionGUIDs = actions;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ActionGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.ActionGUIDs.Count);
                Assert.AreEqual("1", authority.ActionGUIDs[0]);
            }

            // Test factors
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                factors = new List<String>();
                factors.Add("4");
                factors.Add("5");
                factors.Add("6");
                authority.FactorGUIDs = factors;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.FactorGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.FactorGUIDs.Count);
                Assert.AreEqual("4", authority.FactorGUIDs[0]);
            }

            // Test localities
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                localities = new List<String>();
                localities.Add("20");
                localities.Add("21");
                localities.Add("22");
                authority.LocalityGUIDs = localities;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.LocalityGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.LocalityGUIDs.Count);
                Assert.AreEqual("20", authority.LocalityGUIDs[0]);
            }

            // Test regions.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                regions = new List<String>();
                regions.Add("7");
                regions.Add("8");
                regions.Add("9");
                authority.RegionGUIDs = regions;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.RegionGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.RegionGUIDs.Count);
                Assert.AreEqual("7", authority.RegionGUIDs[0]);
            }

            // Test taxa.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                taxa = new List<String>();
                taxa.Add("10");
                taxa.Add("11");
                taxa.Add("12");
                authority.TaxonGUIDs = taxa;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.TaxonGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.TaxonGUIDs.Count);
                Assert.AreEqual("10", authority.TaxonGUIDs[0]);
            }

            // Test projects.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                projects = new List<String>();
                projects.Add("13");
                projects.Add("14");
                projects.Add("15");
                authority.ProjectGUIDs = projects;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ProjectGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.ProjectGUIDs.Count);
                Assert.AreEqual("13", authority.ProjectGUIDs[0]);
                Assert.AreEqual("14", authority.ProjectGUIDs[1]);
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                administrationRoleId = 42;
                authority.AdministrationRoleId = administrationRoleId; ;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(administrationRoleId, authority.AdministrationRoleId.Value);
            }


            // Test authorityIdentity
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authorityIdentity = @"Test AuthorityIdentity XX";
                authority = GetNewAuthority();
                authority.Identifier = authorityIdentity;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(authorityIdentity, authority.Identifier);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                authority = GetNewAuthority();
                authority.Description = description;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(description, authority.Description);
            }

            // Test name
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"NameOfAuthority";
                authority = GetNewAuthority();
                authority.Name = name;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(name, authority.Name);
            }

            // Test obligation
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                obligation = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                authority = GetNewAuthority();
                authority.Obligation = obligation;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(obligation, authority.Obligation);
            }

            // Test permissions
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                readPermission = true;
                updatePermission = true;
                deletePermission = true;
                createPermission = true;
                authority = GetNewAuthority();
                authority.ReadPermission = readPermission;
                authority.UpdatePermission = updatePermission;
                authority.DeletePermission = deletePermission;
                authority.CreatePermission = createPermission;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ReadPermission);
                Assert.IsTrue(authority.UpdatePermission);
                Assert.IsTrue(authority.DeletePermission);
                Assert.IsTrue(authority.CreatePermission);
            }


            // Test maxProtectionLevel
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                maxProtectionLevel = 3;
                authority = GetNewAuthority();
                authority.MaxProtectionLevel = maxProtectionLevel;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(maxProtectionLevel, authority.MaxProtectionLevel);
            }

            // Test showNonPublicData
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showNonPublicData = true;
                authority = GetNewAuthority();
                authority.ReadNonPublicPermission = showNonPublicData;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ReadNonPublicPermission);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                authority = GetNewAuthority();
                authority.ValidFromDate = validFromDate;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(validFromDate, authority.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                authority = GetNewAuthority();
                authority.ValidToDate = validToDate;
                GetUserManager().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(validToDate, authority.ValidToDate);
            }

        }

        [TestMethod]
        public void CreatePerson()
        {
            Boolean showAddress, showEmailAddress, showPersonalInformation,
                    showPhoneNumbers, showPresentation;
            DateTime? birthYear, deathYear;
            IAddress address;
            ICountry country;
            ILocale locale;
            Int32 administrationRoleId, taxonNameTypeId;
            IPerson person;
            IPersonGender gender;
            IPhoneNumber phoneNumber;
            IUser user;
            String city, emailAddress, firstName, lastName, middleName,
                   number, postalAddress1, postalAddress2,
                   presentation, url, zipCode;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                GetUserManager(true).CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, person.UpdateInformation.CreatedBy);
                Assert.AreEqual(person.UpdateInformation.ModifiedBy, person.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - person.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test DataContext.
                Assert.IsNotNull(person.DataContext);

                // Test GUID.
                Assert.IsTrue(person.GUID.IsNotEmpty());

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, person.Id);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, person.UpdateInformation.ModifiedBy);
                Assert.AreEqual(person.UpdateInformation.ModifiedBy, person.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - person.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }

            // Test addresses.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                address = new Address(GetUserContext());
                city = "Uppsala";
                address.City = city;
                country = CoreData.CountryManager.GetCountry(GetUserContext(), CountryId.Sweden);
                address.Country = country;
                postalAddress1 = "";
                address.PostalAddress1 = postalAddress1;
                postalAddress2 = "ArtDatabanken, SLU";
                address.PostalAddress2 = postalAddress2;
                zipCode = "752 52";
                address.ZipCode = zipCode;
                person.Addresses.Add(address);
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.Addresses.IsNotEmpty());
                Assert.AreEqual(1, person.Addresses.Count);
                Assert.AreEqual(city, person.Addresses[0].City);
                Assert.AreEqual(country.Id, person.Addresses[0].Country.Id);
                Assert.AreEqual(postalAddress1, person.Addresses[0].PostalAddress1);
                Assert.AreEqual(postalAddress2, person.Addresses[0].PostalAddress2);
                Assert.AreEqual(zipCode, person.Addresses[0].ZipCode);
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                administrationRoleId = 42;
                person.AdministrationRoleId = administrationRoleId; ;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(administrationRoleId, person.AdministrationRoleId.Value);
            }

            // Test birt year.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                birthYear = null;
                person.BirthYear = birthYear;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsFalse(person.BirthYear.HasValue);

                person = GetNewPerson();
                person.EmailAddress += "t";
                birthYear = DateTime.Now;
                person.BirthYear = birthYear;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.BirthYear.HasValue);
                Assert.IsTrue((birthYear.Value - person.BirthYear.Value) < new TimeSpan(0, 0, 1));
            }

            // Test birt year.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                deathYear = null;
                person.DeathYear = deathYear;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsFalse(person.DeathYear.HasValue);

                person = GetNewPerson();
                person.EmailAddress += "t";
                deathYear = DateTime.Now;
                person.DeathYear = deathYear;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.DeathYear.HasValue);
                Assert.IsTrue((deathYear.Value - person.DeathYear.Value) < new TimeSpan(0, 0, 1));
            }

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                emailAddress = @"My.Email@Address.se2";
                person = GetNewPerson();
                person.EmailAddress = emailAddress;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(emailAddress, person.EmailAddress);
            }

            // Test first name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                firstName = @"Maria";
                person = GetNewPerson();
                person.FirstName = firstName;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(firstName, person.FirstName);
            }

            // Test gender.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                gender = CoreData.UserManager.GetPersonGender(GetUserContext(), PersonGenderId.Woman);
                person = GetNewPerson();
                person.Gender = gender;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(gender.Id, person.Gender.Id);
            }

            // Test last name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                lastName = @"Ripa";
                person = GetNewPerson();
                person.LastName = lastName;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(lastName, person.LastName);
            }

            // Test locale.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                locale = CoreData.LocaleManager.GetLocales(GetUserContext())[0];
                person = GetNewPerson();
                person.Locale = locale;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(locale.Id, person.Locale.Id);
            }

            // Test middle name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                middleName = @"Barret";
                person = GetNewPerson();
                person.MiddleName = middleName;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(middleName, person.MiddleName);
            }

            // Test phone numbers.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                phoneNumber = new PhoneNumber(GetUserContext());
                country = CoreData.CountryManager.GetCountry(GetUserContext(), CountryId.Sweden);
                phoneNumber.Country = country;
                number = "018-67 10 00";
                phoneNumber.Number = number;
                person.PhoneNumbers.Add(phoneNumber);
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.PhoneNumbers.IsNotEmpty());
                Assert.AreEqual(1, person.PhoneNumbers.Count);
                Assert.AreEqual(country.Id, person.PhoneNumbers[0].Country.Id);
                Assert.AreEqual(number, person.PhoneNumbers[0].Number);
            }

            // Test presentation.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                presentation = @"Hej hopp i lingonskogen";
                person = GetNewPerson();
                person.Presentation = presentation;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(presentation, person.Presentation);
            }

            // Test show address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showAddress = false;
                person = GetNewPerson();
                person.ShowAddresses = showAddress;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showAddress, person.ShowAddresses);

                showAddress = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowAddresses = showAddress;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showAddress, person.ShowAddresses);
            }

            // Test show email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = false;
                person = GetNewPerson();
                person.ShowEmailAddress = showEmailAddress;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showEmailAddress, person.ShowEmailAddress);

                showEmailAddress = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowEmailAddress = showEmailAddress;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showEmailAddress, person.ShowEmailAddress);
            }

            // Test show personal information.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPersonalInformation = false;
                person = GetNewPerson();
                person.ShowPersonalInformation = showPersonalInformation;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPersonalInformation, person.ShowPersonalInformation);

                showPersonalInformation = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowPersonalInformation = showPersonalInformation;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPersonalInformation, person.ShowPersonalInformation);
            }

            // Test show phone numbers.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPhoneNumbers = false;
                person = GetNewPerson();
                person.ShowPhoneNumbers = showPhoneNumbers;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPhoneNumbers, person.ShowPhoneNumbers);

                showPhoneNumbers = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowPhoneNumbers = showPhoneNumbers;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPhoneNumbers, person.ShowPhoneNumbers);
            }

            // Test show presentation.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPresentation = false;
                person = GetNewPerson();
                person.ShowPresentation = showPresentation;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPresentation, person.ShowPresentation);

                showPresentation = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowPresentation = showPresentation;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPresentation, person.ShowPresentation);
            }

            // Test taxon name type id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                taxonNameTypeId = (Int32)(TaxonNameCategoryId.ScientificName);
                person = GetNewPerson();
                person.TaxonNameTypeId = taxonNameTypeId;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(taxonNameTypeId, person.TaxonNameTypeId);
            }

            // Test url.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                url = @"http://artdata.slu.se";
                person = GetNewPerson();
                person.URL = url;
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(url, person.URL);
            }

            // Test user.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = null;
                person = GetNewPerson();
                person.SetUser(GetUserContext(), user);
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(user, person.GetUser(GetUserContext()));

                user = CoreData.UserManager.GetUser(GetUserContext());
                person = GetNewPerson();
                person.EmailAddress += "t";
                person.SetUser(GetUserContext(), user);
                GetUserManager().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(user.Id, person.GetUser(GetUserContext()).Id);
            }
        }

        [TestMethod]
        public void CreateUser()
        {
            Boolean showEmailAddress;
            DateTime validFromDate, validToDate;
            String emailAddress, userName;
            IUser user;

            GetUserManager(true);

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetNewUser();
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);

                // Test application id.
                Assert.IsNull(user.ApplicationId);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, user.UpdateInformation.CreatedBy);
                Assert.AreEqual(user.UpdateInformation.ModifiedBy, user.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - user.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test GUID.
                Assert.IsTrue(user.GUID.IsNotEmpty());

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, user.Id);

                // Test is account activated.
                Assert.IsFalse(user.IsAccountActivated);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, user.UpdateInformation.ModifiedBy);
                Assert.AreEqual(user.UpdateInformation.ModifiedBy, user.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - user.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test person id.
                Assert.IsNull(user.GetPerson(GetUserContext()));
            }

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                emailAddress = @"fdskfd.sdff@lksfdf.ldfk";
                user = GetNewUser();
                user.EmailAddress = emailAddress;
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(emailAddress, user.EmailAddress);
            }

            // Test show email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = false;
                user = GetNewUser();
                user.ShowEmailAddress = showEmailAddress;
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(showEmailAddress, user.ShowEmailAddress);
            }
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = true;
                user = GetNewUser();
                user.ShowEmailAddress = showEmailAddress;
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(showEmailAddress, user.ShowEmailAddress);
            }

            // Test user name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                userName = "hshshshsggsghg";
                user = GetNewUser();
                user.UserName = userName;
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(userName, user.UserName);
            }

            // Test user type.
            foreach (UserType userType in Enum.GetValues(typeof(UserType)))
            {
                using (ITransaction transaction = GetUserContext().StartTransaction())
                {
                    user = GetNewUser();
                    user.Type = userType;
                    GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                    Assert.IsNotNull(user);
                    Assert.AreEqual(userType, user.Type);
                }
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                user = GetNewUser();
                user.ValidFromDate = validFromDate;
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(validFromDate, user.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                user = GetNewUser();
                user.ValidToDate = validToDate;
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(validToDate, user.ValidToDate);
            }
        }

        [TestMethod]
        public void CreateRole()
        {
            DateTime validFromDate, validToDate;
            AuthorityList authorityList;
            IAuthority authority;
            Int32 administrationRoleId, userAdministrationRoleId, authorityId;
            IRole role;
            String roleName, description;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                GetUserManager(true).CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, role.UpdateInformation.CreatedBy);
                Assert.AreEqual(role.UpdateInformation.ModifiedBy, role.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - role.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test DataContext.
                Assert.IsNotNull(role.DataContext);

                // Test GUID.
                Assert.IsTrue(role.GUID.IsNotEmpty());

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, role.Id);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, role.UpdateInformation.ModifiedBy);
                Assert.AreEqual(role.UpdateInformation.ModifiedBy, role.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - role.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                administrationRoleId = 42;
                role.AdministrationRoleId = administrationRoleId; ;
                GetUserManager().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(administrationRoleId, role.AdministrationRoleId.Value);
            }

            // Test authorityList.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                GetUserManager().CreateRole(GetUserContext(), role);
                authorityId = 1;
                authorityList = new AuthorityList();
                authority = GetUserManager().GetAuthority(GetUserContext(), authorityId);
                Assert.IsNotNull(authority);
                authorityList.Add(authority);
                Assert.IsNotNull(role);
                role.Authorities = authorityList;
                Assert.AreEqual("UserAdmin.AccountController", role.Authorities[0].Identifier);
            }

            


            // Test roleName
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                roleName = @"Test RoleName XX";
                role = GetNewRole();
                role.Name = roleName;
                GetUserManager().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(roleName, role.Name);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                role = GetNewRole();
                role.Description = description;
                GetUserManager().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(description, role.Description);
            }

            // Test shortName
           /*
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                shortName = @"RoleShortNameUnique";
                role = GetNewRole();
                role.ShortName = shortName;
                GetUserManager().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(shortName, role.ShortName);
            }
            */

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                role = GetNewRole();
                role.ValidFromDate = validFromDate;
                GetUserManager().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(validFromDate, role.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                role = GetNewRole();
                role.ValidToDate = validToDate;
                GetUserManager().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(validToDate, role.ValidToDate);
            }

            // Test user administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                userAdministrationRoleId = 42;
                role.UserAdministrationRoleId = userAdministrationRoleId; 
                GetUserManager().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(userAdministrationRoleId, role.UserAdministrationRoleId.Value);
            }


        }

        [TestMethod]
        public void DataSource()
        {
            IUserDataSource dataSource;

            dataSource = null;
            GetUserManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetUserManager().DataSource);

            dataSource = new UserDataSource();
            GetUserManager().DataSource = dataSource;
            Assert.AreEqual(dataSource, GetUserManager().DataSource);
        }

        [TestMethod]
        public void DeleteAuthority()
        {
            IAuthority authority;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                GetUserManager(true).CreateAuthority(GetUserContext(), authority);
                GetUserManager().DeleteAuthority(GetUserContext(), authority);
            }
        }

        [TestMethod]
        public void DeleteRole()
        {
            IRole role;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                GetUserManager(true).CreateRole(GetUserContext(), role);
                GetUserManager().DeleteRole(GetUserContext(), role);
            }
        }

        [TestMethod]
        public void DeletePerson()
        {
            IPerson person;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                GetUserManager(true).CreatePerson(GetUserContext(), person);
                GetUserManager().DeletePerson(GetUserContext(), person);
            }
        }

        [TestMethod]
        public void DeleteUser()
        {
            IUser user;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetNewUser();
                GetUserManager(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                GetUserManager().DeleteUser(GetUserContext(), user);
            }
        }

        [TestMethod]
        public void GetAddressType()
        {
            IAddressType addressType;

            GetUserManager(true);
            foreach (AddressTypeId addressTypeId in Enum.GetValues(typeof(AddressTypeId)))
            {
                addressType = GetUserManager().GetAddressType(GetUserContext(), addressTypeId);
                Assert.IsNotNull(addressType);
            }

            IUserContext context = GetUserContext();
            String name = "";
            foreach (IAddressType tempAddressType in GetUserManager().GetAddressTypes(context))
            {
                Assert.AreEqual(tempAddressType.Id, GetUserManager().GetAddressType(context, tempAddressType.Id).Id);
                Assert.IsTrue(name != tempAddressType.Name);
                name = tempAddressType.Name;
            }


        }

        [TestMethod]
        public void GetAddressTypes()
        {
            AddressTypeList addressTypes;

            addressTypes = GetUserManager(true).GetAddressTypes(GetUserContext());
            Assert.IsTrue(addressTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetApplicationUsers()
        {
            UserList users;
            users = GetUserManager(true).GetApplicationUsers(GetUserContext());
            Assert.IsFalse(users.IsEmpty());
        }

        [TestMethod]
        public void GetAuthority()
        {
            IAuthority authority1, authority2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority1 = GetNewAuthority();
                authority1.ActionGUIDs.Add("100");
                authority1.ActionGUIDs.Add("200");
                GetUserManager(true).CreateAuthority(GetUserContext(), authority1);
                authority2 = GetUserManager().GetAuthority(GetUserContext(), authority1.Id);
                Assert.AreEqual(authority1.Id, authority2.Id);
                Assert.AreEqual(authority1.Name, authority2.Name);
                Assert.AreEqual(authority1.ActionGUIDs[0], authority2.ActionGUIDs[0]);
                Assert.AreEqual("200", authority2.ActionGUIDs[1]);
            }
        }

        [TestMethod]
        public void GetAuthoritiesBySearchCriteria()
        {
            AuthorityList authorities;
            String authorityIdentifier, applicationIdentifier, authorityDataTypeIdentifier, authorityName;
            AuthoritySearchCriteria searchCriteria;

            // Test all serach criterias if exist in DB or if not.
            // Test Authority Identifier
            authorityIdentifier = "U%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityIdentifier = "NotExistInDB%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Application Identifier
            applicationIdentifier = "UserService%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            applicationIdentifier = "NoServiceExistInDB%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test AuthorityDataType Idenetifier
            authorityDataTypeIdentifier = "Speci%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityDataTypeIdentifier = "NoObsExistInDB%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Authority Name.
            authorityName = "test%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityName = "noTestExistInDB%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Finally test that if no critera is set(ie WebAuthoritySearchCriteria is created by no data is set to search for) will not generat a exception.
            searchCriteria = new AuthoritySearchCriteria();
            authorities = GetUserManager(true).GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());
        }

        [TestMethod]
        public void GetAuthorities()
        {
            AuthorityList authorityList;
            IRole role;
            IApplication application;
            role = (Role)GetUserManager().GetRole(GetUserContext(), Test.Settings.Default.TestRoleId);
            application = CoreData.ApplicationManager.GetApplication(GetUserContext(), Test.Settings.Default.TestApplicationId);
            authorityList = GetUserManager().GetAuthorities(GetUserContext(), role, application);
            Assert.IsTrue(authorityList.IsNotEmpty());
            Assert.IsInstanceOfType(authorityList[0], typeof(Authority));

            //// Check the authorities connected to application and to datatype is correct.
            //// Note! this test can be evalutated after checking the database...  Test OK 2011-09-09

            //// First test should have both application autorites and data type authorities 
            //List<IAuthority> authorites = new List<IAuthority>();
            //foreach (IAuthority authority in authorityList)
            //{
            //    authorites.Add(authority);
            //}
            //IEnumerable<IAuthority> appAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.Application);
            //IEnumerable<IAuthority> dataAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.DataType);
            //Assert.AreEqual(appAuthorities.Count(), 2);
            //Assert.AreEqual(dataAuthorities.Count(), 6);
            //// Second test is returning one application autority
            //role = (Role)GetUserManager().GetRole(GetUserContext(), 618);
            //application = CoreData.ApplicationManager.GetApplication(GetUserContext(), 528);

            //authorityList = GetUserManager().GetAuthorities(GetUserContext(), role, application);
            //Assert.IsTrue(authorityList.IsNotEmpty());
            //Assert.IsInstanceOfType(authorityList[0], typeof(Authority));
            //Assert.AreEqual(authorityList.Count, 1);
            //Assert.AreEqual(authorityList[0].AuthorityType, AuthorityType.Application);

            //// Third test is returning one datatype authority
            //role = (Role)GetUserManager().GetRole(GetUserContext(), 618);
            //application = CoreData.ApplicationManager.GetApplication(GetUserContext(), 1230);

            //authorityList = GetUserManager().GetAuthorities(GetUserContext(), role, application);
            //Assert.IsTrue(authorityList.IsNotEmpty());
            //Assert.IsInstanceOfType(authorityList[0], typeof(Authority));
            //Assert.AreEqual(authorityList.Count, 1);
            //Assert.AreEqual(authorityList[0].AuthorityType, AuthorityType.DataType);

        }

        [TestMethod]
        public void GetAuthoritiesWithIdentifier()
        {
            AuthorityList authorityList;
            Role role;
            Int32 applicationId = Test.Settings.Default.TestApplicationId;
            String authorityIdentifier = "UserAdministration";
            role = (Role)GetUserManager().GetRole(GetUserContext(), Test.Settings.Default.TestRoleId);
            authorityList = GetUserManager().GetAuthorities(GetUserContext(), role, applicationId, authorityIdentifier);
            Assert.IsTrue(authorityList.IsNotEmpty());
            Assert.IsInstanceOfType(authorityList[0], typeof(Authority));

            //// Check the authorities connected to application and to datatype is correct
            //// Note! this test can be evalutated after checking the database...  Test OK 2011-09-09

            //// First test should only have application autorites.
            //List<IAuthority> authorites = new List<IAuthority>();
            //foreach (IAuthority authority in authorityList)
            //{
            //    authorites.Add(authority);
            //}
            //IEnumerable<IAuthority> appAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.Application);
            //IEnumerable<IAuthority> dataAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.DataType);
            //Assert.AreEqual(appAuthorities.Count(), 1);
            //Assert.AreEqual(dataAuthorities.Count(), 0);

            //// Second test is checking application 2 and role 2 which have both applications and datatype authorities.
            //authorityIdentifier = "UserAdmin.AccountController";
            //role = (Role)GetUserManager().GetRole(GetUserContext(), 2);
            //authorityList = GetUserManager().GetAuthorities(GetUserContext(), role, applicationId, authorityIdentifier);
            //authorites = new List<IAuthority>();
            //foreach (IAuthority authority in authorityList)
            //{
            //    authorites.Add(authority);
            //}
            //appAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.Application);
            //dataAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.DataType);
            //Assert.IsTrue(authorityList.IsNotEmpty());
            //Assert.IsInstanceOfType(authorityList[0], typeof(Authority));
            //Assert.AreEqual(appAuthorities.Count(), 2);
            //Assert.AreEqual(dataAuthorities.Count(), 3);

            //// Third test should only have data type autorites.
            //authorityIdentifier = "GunnarTest";
            //role = (Role)GetUserManager().GetRole(GetUserContext(), 618);
            //authorityList = GetUserManager().GetAuthorities(GetUserContext(), role, 1230, authorityIdentifier);
            //authorites = new List<IAuthority>();
            //foreach (IAuthority authority in authorityList)
            //{
            //    authorites.Add(authority);
            //}
            //appAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.Application);
            //dataAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.DataType);
            //Assert.IsTrue(authorityList.IsNotEmpty());
            //Assert.IsInstanceOfType(authorityList[0], typeof(Authority));
            //Assert.AreEqual(appAuthorities.Count(), 0);
            //Assert.AreEqual(dataAuthorities.Count(), 1);
        }

        [TestMethod]
        public void GetAuthoritiesUserApplication()
        {
            AuthorityList authorityList;
            Int32 userId = Settings.Default.TestUserId;
            Int32 applicationId = Test.Settings.Default.TestApplicationId;
            authorityList = GetUserManager().GetAuthorities(GetUserContext(), userId, applicationId);
            Assert.IsTrue(authorityList.IsNotEmpty());
            Assert.IsInstanceOfType(authorityList[0], typeof(Authority));

            //// Check the authorities connected to application and to datatype is correct
            //// Note! this test can be evalutated after checking the database...  Test OK 2011-09-09

            //// First test should only have application autorites.
            //List<IAuthority> authorites = new List<IAuthority>();
            //foreach (IAuthority authority in authorityList)
            //{
            //    authorites.Add(authority);
            //}
            //IEnumerable<IAuthority> appAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.Application);
            //IEnumerable<IAuthority> dataAuthorities = authorites.Where(s => s.AuthorityType == AuthorityType.DataType);
            //Assert.AreEqual(appAuthorities.Count(), 4);
            //Assert.AreEqual(dataAuthorities.Count(), 9);

        }

        [TestMethod]
        public void GetAuthorityDataTypes()
        {
            AuthorityDataTypeList authorityDataTypes;

            authorityDataTypes = GetUserManager(true).GetAuthorityDataTypes(GetUserContext());
            Assert.IsTrue(authorityDataTypes.IsNotEmpty());
        }

        /// <summary>
        /// Test that correct list of authorityDataTypes is retuned for TestApplication (Settings.Default.TestApplicationId)
        /// </summary>
        [TestMethod]
        public void GetAuthorityDataTypesByApplicationId()
        {
            AuthorityDataTypeList authorityDataTypes, authorityDataTypesTest;
            Int32 applicationId = Settings.Default.TestApplicationId;
            authorityDataTypes = GetUserManager(true).GetAuthorityDataTypesByApplicationId(GetUserContext(), applicationId);
            authorityDataTypesTest = GetAuthorityDataTypesForTestApplication();
            Assert.IsTrue(authorityDataTypes.IsNotEmpty());
            Assert.IsTrue(authorityDataTypesTest.IsNotEmpty());
            int noOfEntries = 0;
            foreach (AuthorityDataType authorityDataTypeTest in authorityDataTypesTest)
            {
                foreach (AuthorityDataType authorityDataType in authorityDataTypes)
                {
                    if (authorityDataType.Id == authorityDataTypeTest.Id)
                    {
                        Assert.AreEqual(authorityDataTypeTest.Identifier, authorityDataType.Identifier);
                        noOfEntries++;
                    }
                }

            }
            Assert.AreEqual(authorityDataTypesTest.Count, noOfEntries);
        }

        /// <summary>
        /// Reuturns authority data type list for Settings.Default.TestApplicationId, created as
        /// test data.
        /// </summary>
        /// <returns></returns>
        private AuthorityDataTypeList GetAuthorityDataTypesForTestApplication()
        {
            AuthorityDataTypeList authorityDataTypeList = new AuthorityDataTypeList();
            IDataContext dataContext = new DataContext(GetUserContext());
            AuthorityDataType authorityDataType1 = new AuthorityDataType(1, "Test", dataContext);
            AuthorityDataType authorityDataType2 = new AuthorityDataType(2, "SpeciesObservation", dataContext);
            authorityDataTypeList.Add(authorityDataType1);
            authorityDataTypeList.Add(authorityDataType2);
            return authorityDataTypeList;
        }

        [TestMethod]
        public void GetDataSourceInformation()
        {
            Assert.IsNotNull(GetUserManager(true).GetDataSourceInformation());
        }

        [TestMethod]
        public void GetLockedUserInformation()
        {
            LockedUserInformationList lockedUsers;
            String userName;
            StringSearchCriteria userNameSearchString;

            // Search with no specific user.
            GetUserManager(true).GetLockedUserInformation(GetUserContext(), null);

            // Search with specific user that is not locked.
            userNameSearchString = new StringSearchCriteria();
            userNameSearchString.SearchString = "No user name";
            lockedUsers = GetUserManager().GetLockedUserInformation(GetUserContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userName = "qwertyOnion" + DateTime.Now.ToString(GetUserContext().Locale.CultureInfo);
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            userNameSearchString = new StringSearchCriteria();
            userNameSearchString.SearchString = userName;
            lockedUsers = GetUserManager().GetLockedUserInformation(GetUserContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());

            // Search with specific user that is locked.
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            lockedUsers = GetUserManager().GetLockedUserInformation(GetUserContext(), null);
            Assert.IsTrue(lockedUsers.IsNotEmpty());
            userNameSearchString = new StringSearchCriteria();
            userNameSearchString.SearchString = "No user name";
            lockedUsers = GetUserManager().GetLockedUserInformation(GetUserContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userNameSearchString = new StringSearchCriteria();
            userNameSearchString.SearchString = userName;
            lockedUsers = GetUserManager().GetLockedUserInformation(GetUserContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsNotEmpty());
            Assert.AreEqual(1, lockedUsers.Count);
            Assert.AreEqual(5, lockedUsers[0].LoginAttemptCount);
            Assert.AreEqual(userName, lockedUsers[0].UserName);
        }

        [TestMethod]
        public void GetMessageType()
        {
            IMessageType messageType;

            GetUserManager(true);
            foreach (MessageTypeId messageTypeId in Enum.GetValues(typeof(MessageTypeId)))
            {
                messageType = GetUserManager().GetMessageType(GetUserContext(), messageTypeId);
                Assert.IsNotNull(messageType);
            }

            IUserContext context = GetUserContext();
            String name = "";
            foreach (IMessageType tempMessageType in GetUserManager().GetMessageTypes(context))
            {
                Assert.AreEqual(tempMessageType.Id, GetUserManager().GetMessageType(context, tempMessageType.Id).Id);
                Assert.IsTrue(name != tempMessageType.Name);
                name = tempMessageType.Name;
            }
        }

        [TestMethod]
        public void GetMessageTypes()
        {
            MessageTypeList messageTypes;

            messageTypes = GetUserManager(true).GetMessageTypes(GetUserContext());
            Assert.IsTrue(messageTypes.IsNotEmpty());
            Assert.IsTrue(messageTypes.Count > 2);
        }

        private IAuthority GetNewAuthority()
        {
            IAuthority newAuthority;

            newAuthority = new Authority(GetUserContext());
            newAuthority.Identifier = @"AuthorityIdentity";
            newAuthority.Obligation = @"testObligation";
            newAuthority.Name = @"NameOfAtuhority";
            newAuthority.Description = @"testdescription";
            newAuthority.ApplicationId = Settings.Default.TestApplicationId;
           // newAuthority.AuthorityType = AuthorityType.Application;
            newAuthority.RoleId = Settings.Default.TestRoleId;
            newAuthority.ActionGUIDs = new List<String>();
            newAuthority.FactorGUIDs = new List<String>();
            newAuthority.LocalityGUIDs = new List<String>();
            newAuthority.ProjectGUIDs = new List<String>();
            newAuthority.RegionGUIDs = new List<String>();
            newAuthority.TaxonGUIDs = new List<String>();
            return newAuthority;
        }

        private IPerson GetNewPerson()
        {
            IPerson newPerson;

            newPerson = new Person(GetUserContext());
            newPerson.BirthYear = null;
            newPerson.DeathYear = null;
            newPerson.EmailAddress = "atestmail@slu.se";
            newPerson.FirstName = "Björn";
            newPerson.LastName = "Karlsson";
            newPerson.SetUser(GetUserContext(), null);
            newPerson.Gender = CoreData.UserManager.GetPersonGender(GetUserContext(), PersonGenderId.Man);
            newPerson.Locale = CoreData.LocaleManager.GetLocale(GetUserContext(), Settings.Default.SwedishLocale);
            return newPerson;
        }

        private IRole GetNewRole()
        {
            IRole newRole;

            newRole = new Role(GetUserContext());
            newRole.Name = @"RoleNameUnique2";
            newRole.ShortName = @"RoleShortNameUnique2";
            newRole.Description = @"testdescription";
            newRole.Id = Settings.Default.TestRoleId;
            newRole.UserAdministrationRoleId = 1;
            return newRole;
        }

        private IUser GetNewUser(String emailAddress = @"MyEmail@Address")
        {
            IUser newUser;

            newUser = new User(GetUserContext());
            newUser.EmailAddress = emailAddress;
            newUser.UserName = Settings.Default.TestUserName + 42; ;
            newUser.Type = UserType.Person;
            newUser.ValidFromDate = DateTime.Now;
            newUser.ValidToDate = newUser.ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
            return newUser;
        }

        private IAuthority GetOneAuthority()
        {
            IAuthority authority;

            // It is assumed that this method is called
            // inside a transaction.
            authority = GetNewAuthority();
            GetUserManager(true).CreateAuthority(GetUserContext(), authority);
            return authority;
        }

        private IPerson GetOnePerson(String emailAddress = @"GetOnePerson@slu.se")
        {
            IPerson person;

            // It is assumed that this method is called
            // inside a transaction.
            person = GetNewPerson();
            person.EmailAddress = emailAddress;
            GetUserManager(true).CreatePerson(GetUserContext(), person);
            return person;
        }

        private IPerson GetOnePerson(int personId)
        {
            return GetUserManager(true).GetPerson(GetUserContext(), personId);
        }

        private IRole GetOneRole()
        {
            IRole role;

            // It is assumed that this method is called
            // inside a transaction.
            role = GetNewRole();
            GetUserManager(true).CreateRole(GetUserContext(), role);
            return role;
        }

        private IUser GetOneUser()
        {
            IUser user;
            String password;
            // It is assumed that this method is called
            // inside a transaction.
            user = GetNewUser();
            password = "newPASSword1";
            GetUserManager(true).CreateUser(GetUserContext(), user, password);
            return user;
        }

        [TestMethod]
        public void GetPerson()
        {
            IPerson person1, person2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person1 = GetNewPerson();
                GetUserManager(true).CreatePerson(GetUserContext(), person1);
                person2 = GetUserManager().GetPerson(GetUserContext(), person1.Id);
                Assert.AreEqual(person1.Id, person2.Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetPersonIdError()
        {
            IPerson person;

            person = GetUserManager(true).GetPerson(GetUserContext(), Int32.MinValue);
            Assert.IsNull(person);
        }

        [TestMethod]
        public void GetPersonGender()
        {
            IPersonGender personGender;

            GetUserManager(true);
            foreach (PersonGenderId personGenderId in Enum.GetValues(typeof(PersonGenderId)))
            {
                personGender = GetUserManager().GetPersonGender(GetUserContext(), personGenderId);
                Assert.IsNotNull(personGender);
            }
        }

        [TestMethod]
        public void GetPersonGenders()
        {
            PersonGenderList personGenders;

            personGenders = GetUserManager(true).GetPersonGenders(GetUserContext());
            Assert.IsTrue(personGenders.IsNotEmpty());
            string name = "";
            //Checks that names are unique
            foreach (IPersonGender gender in personGenders)
            {
                Assert.IsTrue(name != gender.Name);
                name = gender.Name;
            }
        }

        [TestMethod]
        public void GetPersonsByModifiedDate()
        {
            PersonList persons;
            DateTime start, end;
            start = DateTime.Parse("2011-02-25");
            end = DateTime.Now;
            persons = GetUserManager(true).GetPersonsByModifiedDate(GetUserContext(), start, end);
            Assert.IsNotNull(persons);
            int count1 = persons.Count;
            start = DateTime.Now;
            persons = GetUserManager(true).GetPersonsByModifiedDate(GetUserContext(), start, end);
            Assert.IsTrue(count1 > persons.Count);
        }

        [TestMethod]
        public void GetPersonsBySearchCriteria()
        {
            PersonList persons;
            String name;
            Boolean hasSpiecesCollection;
            PersonSearchCriteria searchCriteria;

            searchCriteria = new PersonSearchCriteria();
            persons = GetUserManager(true).GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            // Test first name.
            name = "Test%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = GetUserManager(true).GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = GetUserManager().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = GetUserManager().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = GetUserManager().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = GetUserManager().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = GetUserManager().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test hasSpiecesCollection
            hasSpiecesCollection = true;
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.HasSpiecesCollection = hasSpiecesCollection;
            persons = GetUserManager().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());
        }

        [TestMethod]
        public void GetPhoneNumberType()
        {
            IPhoneNumberType phoneNumberType;

            GetUserManager(true);
            foreach (PhoneNumberTypeId phoneNumberTypeId in Enum.GetValues(typeof(PhoneNumberTypeId)))
            {
                phoneNumberType = GetUserManager().GetPhoneNumberType(GetUserContext(), phoneNumberTypeId);
                Assert.IsNotNull(phoneNumberType);
            }
        }

        [TestMethod]
        public void GetPhoneNumberTypes()
        {
            PhoneNumberTypeList phoneNumberTypes;

            phoneNumberTypes = GetUserManager(true).GetPhoneNumberTypes(GetUserContext());
            Assert.IsTrue(phoneNumberTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetRole()
        {
            IRole role1, role2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role1 = GetNewRole();
                GetUserManager(true).CreateRole(GetUserContext(), role1);
                role2 = GetUserManager().GetRole(GetUserContext(), role1.Id);
                Assert.AreEqual(role1.Id, role2.Id);
                Assert.AreEqual(role1.Name, role2.Name);
                Assert.AreEqual(role1.ShortName, role2.ShortName);
                Assert.IsFalse(role1.IsUserAdministrationRole);
            }
        }

        [TestMethod]
        public void GetRolesBySearchCriteria()
        {
            Int32 organizationId;
            RoleList roles;
            String name;
            RoleSearchCriteria searchCriteria;

            // Test role name.
            name = "A%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.Name = name;
            roles = GetUserManager(true).GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt23%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.Name = name;
            roles = GetUserManager().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test short name.
            name = "Test%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = GetUserManager().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt23%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = GetUserManager().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test identifier.
            name = "A%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.Identifier = name;
            roles = GetUserManager().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt23%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.Identifier = name;
            roles = GetUserManager().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test organization Id.
            organizationId = Settings.Default.TestOrganizationId;
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            roles = GetUserManager().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            organizationId = -1;
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            roles = GetUserManager().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());
        }

        [TestMethod]
        public void GetRoleMembersBySearchCriteria()
        {
            List<RoleMember> roleMembers;
            List<Int32> roleIds = new List<int>();
            List<Int32> userIds = new List<int>();
            RoleMemberSearchCriteria searchCriteria;

            searchCriteria = new RoleMemberSearchCriteria();

            roleIds.Add(5);
            searchCriteria.RoleIdList = roleIds;
            searchCriteria.UserIdList = new List<int>();
            roleMembers = GetUserManager().GetRoleMembersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.Role.Authorities.IsNull());
                Assert.IsFalse(member.User.IsNull());
            }

            userIds.Add(2);
            searchCriteria.RoleIdList = new List<int>();
            searchCriteria.UserIdList = userIds;
            roleMembers = GetUserManager().GetRoleMembersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.User.IsNull());
            }

            searchCriteria.RoleIdList = new List<int>();
            searchCriteria.UserIdList = new List<int>();
            searchCriteria.IsActivated = false;
            roleMembers = GetUserManager().GetRoleMembersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.User.IsNull());
                Assert.IsFalse(member.IsActivated);
            }

        }

        [TestMethod]
        public void GetRolesByUserGroupAdministrationRoleId()
        {
            RoleSearchCriteria searchCriteria = new RoleSearchCriteria();
            Int32 id = -1;
            RoleList roles = GetUserManager(true).GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            foreach (IRole role in roles)
            {
                RoleList administratedRoles = GetUserManager().GetRolesByUserGroupAdministrationRoleId(GetUserContext(), role.Id);
                if (administratedRoles.IsNotEmpty())
                {
                    id = administratedRoles[0].Id;
                }
            }
            Assert.IsTrue(id > -1);
        }

        [TestMethod]
        public void GetRolesByUserGroupAdministratorUserId()
        {
            RoleSearchCriteria searchCriteria = new RoleSearchCriteria();
            Int32 id = 23;

            RoleList administratedRoles = GetUserManager(true).GetRolesByUserGroupAdministratorUserId(GetUserContext(), id);

            Assert.IsTrue(administratedRoles.Count > 0);
        }

        [TestMethod]
        public void GetUser()
        {
            IUser user1, user2;

            user1 = GetUserManager().GetUser(GetUserContext());
            Assert.IsNotNull(user1);

            user2 = GetUserManager().GetUser(GetUserContext(), user1.Id);
            Assert.IsNotNull(user2);
            Assert.AreEqual(user1.UserName, user2.UserName);
            Assert.AreEqual(user1.Id, user2.Id);
        }


        [TestMethod]
        public void GetUserByUserName()
        {
            IUser user1, user2;

            user1 = GetUserManager().GetUser(GetUserContext());
            Assert.IsNotNull(user1);

            user2 = GetUserManager().GetUser(GetUserContext(), user1.UserName);
            Assert.IsNotNull(user2);
            Assert.AreEqual(user1.UserName, user2.UserName);
            Assert.AreEqual(user1.Id, user2.Id);
        }

        [TestMethod]
        public void GetUserByUserNameNonExistingUserName()
        {
            IUser user;
            // Non existing username should return NULL.
            user = GetUserManager().GetUser(GetUserContext(), "NonExistingName");
            Assert.IsNull(user);
        }


        private UserManager GetUserManager()
        {
            return GetUserManager(false);
        }

        private UserManager GetUserManager(Boolean refresh)
        {
            if (_userManager.IsNull() || refresh)
            {
                _userManager = new UserManager();
                _userManager.DataSource = new UserDataSource();
            }
            return _userManager;
        }


        [TestMethod]
        public void GetUserRoles()
        {
            RoleList roles;
            Int32 userId = Settings.Default.TestUserId;
            String applicationIdentifier = Settings.Default.TestApplicationIdentifier;
            roles = GetUserManager().GetRolesByUser(GetUserContext(), userId, applicationIdentifier);
            Assert.IsTrue(roles.IsNotEmpty());
        }

        [TestMethod]
        public void GetUsersByRole()
        {
            UserList users;
            Int32 roleId = Settings.Default.TestRoleId;
            users = GetUserManager(true).GetUsersByRole(GetUserContext(), roleId);
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count >= 1);
        }

        [TestMethod]
        public void GetNonActivatedUsersByRole()
        {
            UserList users;
            Int32 roleId = Settings.Default.TestRoleId;
            users = GetUserManager(true).GetNonActivatedUsersByRole(GetUserContext(), roleId);
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count == 0);
        }

        [TestMethod]
        public void GetUsersBySearchCriteria()
        {
            UserList users;
            String name;
            PersonUserSearchCriteria searchCriteria;

            // Test first name.
            name = "Test%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = GetUserManager(true).GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = GetUserManager().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.FullName = name;
            users = GetUserManager().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.FullName = name;
            users = GetUserManager().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.LastName = name;
            users = GetUserManager().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.LastName = name;
            users = GetUserManager().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());
        }

        [TestMethod]
        public void IsExistingPerson()
        {
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                Assert.IsFalse(GetUserManager(true).IsExistingPerson(GetUserContext(), Settings.Default.TestEmailAddress + 42));
                Assert.IsTrue(GetUserManager().IsExistingPerson(GetUserContext(), "artdata@slu.se"));
            }
        }

        [TestMethod]
        public void IsExistingUser()
        {
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                Assert.IsFalse(GetUserManager(true).IsExistingUser(GetUserContext(), Settings.Default.TestUserName + 42));
                Assert.IsTrue(GetUserManager().IsExistingUser(GetUserContext(), Settings.Default.TestUserName));
            }
        }

        [TestMethod]
        public void Login()
        {
            IUserContext userContext;

            userContext = GetUserManager(true).Login(Settings.Default.TestUserName,
                                                     Settings.Default.TestPassword,
                                                     Settings.Default.TestApplicationIdentifier);
            Assert.IsNotNull(userContext);

            userContext = GetUserManager().Login(Settings.Default.TestUserName,
                                                 Settings.Default.TestPassword,
                                                 Settings.Default.TestApplicationIdentifier,
                                                 false);
            Assert.IsNotNull(userContext);
        }

        [TestMethod]
        public void Logout()
        {
            IUserContext userContext;

            userContext = GetUserManager(true).Login(Settings.Default.TestUserName,
                                                     Settings.Default.TestPassword,
                                                     Settings.Default.TestApplicationIdentifier);
            GetUserManager().Logout(userContext);
        }

        [TestMethod]
        public void ResetPassword()
        {
            String emailAddress, password, userName;
            IPasswordInformation passwordInformation;
            IUser user;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                emailAddress = @"ResetPassword1@slu.se";
                password = "fsdlkjKJ994";
                userName = "skdfhja";
                user = GetNewUser(@"ResetPassword2@slu.se");
                user.EmailAddress = emailAddress;
                user.UserName = userName;
                GetUserManager(true).CreateUser(GetUserContext(), user, password);
                passwordInformation = GetUserManager().ResetPassword(GetUserContext(), emailAddress);
                Assert.IsNotNull(passwordInformation);
                Assert.AreEqual(emailAddress, passwordInformation.EmailAddress);
                Assert.AreNotEqual(password, passwordInformation.Password);
                Assert.AreEqual(userName, passwordInformation.UserName);
            }
        }

        [TestMethod]
        public void RemoveUserFromRole()
        {
            Int32 userId;
            Int32 roleId = Settings.Default.TestRoleId;
            IUser user;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetOneUser();
                userId = user.Id;
                // Add user to role
                GetUserManager(true).AddUserToRole(GetUserContext(), roleId, userId);
                // Remove user from role
                GetUserManager(true).RemoveUserFromRole(GetUserContext(), roleId, userId);
            }
        }

        [TestMethod]
        public void UpdateAuthority()
        {
            Boolean createPermission, readPermission, deletePermission, updatePermission,
                    showNonPublicData;
            DateTime validFromDate, validToDate;
            List<String> actions, regions, factors, taxa, projects, localities;
            Int32 administrationRoleId, maxProtectionLevel;
            IAuthority authority;
            String authorityIdentity, description, name, obligation;

            // Test actions.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
                actions = new List<String>();
                actions.Add("1");
                actions.Add("2");
                actions.Add("3");
                authority.ActionGUIDs = actions;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ActionGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.ActionGUIDs.Count);
                Assert.AreEqual("1", authority.ActionGUIDs[0]);
            }

            // Test factors
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
                factors = new List<String>();
                factors.Add("4");
                factors.Add("5");
                factors.Add("6");
                authority.FactorGUIDs = factors;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.FactorGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.FactorGUIDs.Count);
                Assert.AreEqual("4", authority.FactorGUIDs[0]);
            }

            // Test localities
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
                localities = new List<String>();
                localities.Add("21");
                localities.Add("22");
                localities.Add("23");
                authority.LocalityGUIDs = localities;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.LocalityGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.LocalityGUIDs.Count);
                Assert.AreEqual("21", authority.LocalityGUIDs[0]);
            }


            // Test regions.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
                regions = new List<String>();
                regions.Add("urn:lsid:artportalen.se:Area:DataSet1Feature1");
                regions.Add("urn:lsid:artportalen.se:Area:DataSet1Feature3");
                regions.Add("urn:lsid:artportalen.se:Area:DataSet1Feature2");
                authority.RegionGUIDs = regions;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.RegionGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.RegionGUIDs.Count);
                Assert.AreEqual("urn:lsid:artportalen.se:Area:DataSet1Feature1", authority.RegionGUIDs[0]);
            }

            // Test taxa.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
                taxa = new List<String>();
                taxa.Add("10");
                taxa.Add("11");
                taxa.Add("12");
                authority.TaxonGUIDs = taxa;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.TaxonGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.TaxonGUIDs.Count);
                Assert.AreEqual("10", authority.TaxonGUIDs[0]);
            }

            // Test projects.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
                projects = new List<String>();
                projects.Add("13");
                projects.Add("14");
                projects.Add("15");
                authority.ProjectGUIDs = projects;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ProjectGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.ProjectGUIDs.Count);
                Assert.AreEqual("13", authority.ProjectGUIDs[0]);
                Assert.AreEqual("14", authority.ProjectGUIDs[1]);
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
                administrationRoleId = 42;
                authority.AdministrationRoleId = administrationRoleId; ;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(administrationRoleId, authority.AdministrationRoleId.Value);
            }


            // Test authorityIdentity
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authorityIdentity = @"Test AuthorityIdentity XX";
                authority = GetOneAuthority();
                authority.Identifier = authorityIdentity;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(authorityIdentity, authority.Identifier);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                authority = GetOneAuthority();
                authority.Description = description;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(description, authority.Description);
            }

            // Test name
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"NameOfAuthority";
                authority = GetOneAuthority();
                authority.Name = name;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(name, authority.Name);
            }

            // Test obligation
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                obligation = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                authority = GetOneAuthority();
                authority.Obligation = obligation;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(obligation, authority.Obligation);
            }

            // Test permissions
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                readPermission = true;
                updatePermission = true;
                deletePermission = true;
                createPermission = true;
                authority = GetOneAuthority();
                authority.ReadPermission = readPermission;
                authority.UpdatePermission = updatePermission;
                authority.DeletePermission = deletePermission;
                authority.CreatePermission = createPermission;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ReadPermission);
                Assert.IsTrue(authority.UpdatePermission);
                Assert.IsTrue(authority.DeletePermission);
                Assert.IsTrue(authority.CreatePermission);
            }

            // Test maxProtectionLevel
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                maxProtectionLevel = 3;
                authority = GetOneAuthority();
                authority.MaxProtectionLevel = maxProtectionLevel;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(maxProtectionLevel, authority.MaxProtectionLevel);
            }

            // Test showNonPublicData
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showNonPublicData = true;
                authority = GetOneAuthority();
                authority.ReadNonPublicPermission = showNonPublicData;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ReadNonPublicPermission);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                authority = GetOneAuthority();
                authority.ValidFromDate = validFromDate;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(validFromDate, authority.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                authority = GetOneAuthority();
                authority.ValidToDate = validToDate;
                GetUserManager().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(validToDate, authority.ValidToDate);
            }

        }

        [TestMethod]
        public void UpdatePassword()
        {
            String newPassword, oldPassword;

            oldPassword = Settings.Default.TestPassword;
            newPassword = "NotUsedPassword0-";
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                Assert.IsTrue(GetUserManager().UpdatePassword(GetUserContext(), oldPassword, newPassword));
            }
        }

        [TestMethod]
        public void UpdatePerson()
        {
            Boolean showAddress, showEmailAddress, showPersonalInformation,
                    showPhoneNumbers, showPresentation;
            DateTime? birthYear, deathYear;
            IAddress address;
            ICountry country;
            ILocale locale;
            Int32 administrationRoleId, taxonNameTypeId;
            IPerson person;
            IPersonGender gender;
            IPhoneNumber phoneNumber;
            IUser user;
            String city, emailAddress, firstName, lastName, middleName,
                   number, postalAddress1, postalAddress2,
                   presentation, url, zipCode;

            // Test addresses.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetOnePerson(@"UpdatePerson1@slu.se");
                address = new Address(GetUserContext());
                city = "Uppsala";
                address.City = city;
                country = CoreData.CountryManager.GetCountry(GetUserContext(), CountryId.Sweden);
                address.Country = country;
                postalAddress1 = "";
                address.PostalAddress1 = postalAddress1;
                postalAddress2 = "ArtDatabanken, SLU";
                address.PostalAddress2 = postalAddress2;
                zipCode = "752 52";
                address.ZipCode = zipCode;
                person.Addresses.Add(address);
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.Addresses.IsNotEmpty());
                Assert.AreEqual(1, person.Addresses.Count);
                Assert.AreEqual(city, person.Addresses[0].City);
                Assert.AreEqual(country.Id, person.Addresses[0].Country.Id);
                Assert.AreEqual(postalAddress1, person.Addresses[0].PostalAddress1);
                Assert.AreEqual(postalAddress2, person.Addresses[0].PostalAddress2);
                Assert.AreEqual(zipCode, person.Addresses[0].ZipCode);
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetOnePerson(@"UpdatePerson2@slu.se");
                administrationRoleId = 42;
                person.AdministrationRoleId = administrationRoleId; ;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(administrationRoleId, person.AdministrationRoleId.Value);
            }

            // Test birt year.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetOnePerson(@"UpdatePerson3@slu.se");
                birthYear = null;
                person.BirthYear = birthYear;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsFalse(person.BirthYear.HasValue);

                person = GetOnePerson(@"UpdatePerson4@slu.se");
                person.EmailAddress += "t";
                birthYear = DateTime.Now;
                person.BirthYear = birthYear;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.BirthYear.HasValue);
                Assert.IsTrue((birthYear.Value - person.BirthYear.Value) < new TimeSpan(0, 0, 1));
            }

            // Test birt year.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetOnePerson(@"UpdatePerson5@slu.se");
                deathYear = null;
                person.DeathYear = deathYear;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsFalse(person.DeathYear.HasValue);

                person = GetOnePerson(@"UpdatePerson6@slu.se");
                person.EmailAddress += "t";
                deathYear = DateTime.Now;
                person.DeathYear = deathYear;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.DeathYear.HasValue);
                Assert.IsTrue((deathYear.Value - person.DeathYear.Value) < new TimeSpan(0, 0, 1));
            }

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                emailAddress = @"UpdatePerson7@slu.se";
                person = GetOnePerson(@"UpdatePerson7@slu.se");
                person.EmailAddress = emailAddress;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(emailAddress, person.EmailAddress);
            }

            // Test first name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                firstName = @"Maria";
                person = GetOnePerson(@"UpdatePerson8@slu.se");
                person.FirstName = firstName;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(firstName, person.FirstName);
            }

            // Test gender.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                gender = CoreData.UserManager.GetPersonGender(GetUserContext(), PersonGenderId.Woman);
                person = GetOnePerson(@"UpdatePerson9@slu.se");
                person.Gender = gender;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(gender.Id, person.Gender.Id);
            }

            // Test last name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                lastName = @"Ripa";
                person = GetOnePerson(@"UpdatePerson10@slu.se");
                person.LastName = lastName;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(lastName, person.LastName);
            }

            // Test locale.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                locale = CoreData.LocaleManager.GetLocales(GetUserContext())[0];
                person = GetOnePerson(@"UpdatePerson11@slu.se");
                person.Locale = locale;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(locale.Id, person.Locale.Id);
            }

            // Test middle name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                middleName = @"Barret";
                person = GetOnePerson(@"UpdatePerson12@slu.se");
                person.MiddleName = middleName;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(middleName, person.MiddleName);
            }

            // Test phone numbers.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetOnePerson(@"UpdatePerson13@slu.se");
                phoneNumber = new PhoneNumber(GetUserContext());
                country = CoreData.CountryManager.GetCountry(GetUserContext(), CountryId.Sweden);
                phoneNumber.Country = country;
                number = "018-67 10 00";
                phoneNumber.Number = number;
                person.PhoneNumbers.Add(phoneNumber);
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.PhoneNumbers.IsNotEmpty());
                Assert.AreEqual(1, person.PhoneNumbers.Count);
                Assert.AreEqual(country.Id, person.PhoneNumbers[0].Country.Id);
                Assert.AreEqual(number, person.PhoneNumbers[0].Number);
            }

            // Test presentation.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                presentation = @"Hej hopp i lingonskogen";
                person = GetOnePerson(@"UpdatePerson14@slu.se");
                person.Presentation = presentation;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(presentation, person.Presentation);
            }

            // Test show address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showAddress = false;
                person = GetOnePerson(@"UpdatePerson15@slu.se");
                person.ShowAddresses = showAddress;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showAddress, person.ShowAddresses);

                showAddress = true;
                person = GetOnePerson(@"UpdatePerson16@slu.se");
                person.EmailAddress += "2";
                person.ShowAddresses = showAddress;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showAddress, person.ShowAddresses);
            }

            // Test show email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = false;
                person = GetOnePerson(@"UpdatePerson17@slu.se");
                person.ShowEmailAddress = showEmailAddress;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showEmailAddress, person.ShowEmailAddress);

                showEmailAddress = true;
                person = GetOnePerson(@"UpdatePerson18@slu.se");
                person.EmailAddress += "2";
                person.ShowEmailAddress = showEmailAddress;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showEmailAddress, person.ShowEmailAddress);
            }

            // Test show personal information.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPersonalInformation = false;
                person = GetOnePerson(@"UpdatePerson19@slu.se");
                person.ShowPersonalInformation = showPersonalInformation;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPersonalInformation, person.ShowPersonalInformation);

                showPersonalInformation = true;
                person = GetOnePerson(@"UpdatePerson20@slu.se");
                person.EmailAddress += "2";
                person.ShowPersonalInformation = showPersonalInformation;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPersonalInformation, person.ShowPersonalInformation);
            }

            // Test show phone numbers.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPhoneNumbers = false;
                person = GetOnePerson(@"UpdatePerson21@slu.se");
                person.ShowPhoneNumbers = showPhoneNumbers;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPhoneNumbers, person.ShowPhoneNumbers);

                showPhoneNumbers = true;
                person = GetOnePerson(@"UpdatePerson22@slu.se");
                person.EmailAddress += "2";
                person.ShowPhoneNumbers = showPhoneNumbers;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPhoneNumbers, person.ShowPhoneNumbers);
            }

            // Test show presentation.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPresentation = false;
                person = GetOnePerson(@"UpdatePerson23@slu.se");
                person.ShowPresentation = showPresentation;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPresentation, person.ShowPresentation);

                showPresentation = true;
                person = GetOnePerson(@"UpdatePerson24@slu.se");
                person.EmailAddress += "2";
                person.ShowPresentation = showPresentation;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPresentation, person.ShowPresentation);
            }

            // Test taxon name type id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                taxonNameTypeId = (Int32)(TaxonNameCategoryId.ScientificName);
                person = GetOnePerson(@"UpdatePerson25@slu.se");
                person.TaxonNameTypeId = taxonNameTypeId;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(taxonNameTypeId, person.TaxonNameTypeId);
            }

            // Test url.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                url = @"http://artdata.slu.se";
                person = GetOnePerson(@"UpdatePerson26@slu.se");
                person.URL = url;
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(url, person.URL);
            }

            // Test user.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = null;
                person = GetOnePerson(@"UpdatePerson27@slu.se");
                person.SetUser(GetUserContext(), user);
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(user, person.GetUser(GetUserContext()));

                user = CoreData.UserManager.GetUser(GetUserContext());
                person = GetOnePerson(@"UpdatePerson28@slu.se");
                person.EmailAddress += "t";
                person.SetUser(GetUserContext(), user);
                GetUserManager().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(user.Id, person.GetUser(GetUserContext()).Id);
            }
        }

        [TestMethod]
        public void UpdateRole()
        {
            DateTime validFromDate, validToDate;
            Int32 administrationRoleId, userAdministrationRoleId, testRoleId, organizationId;
            IRole role;
            String description, shortName;

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetOneRole();
                administrationRoleId = 42;
                role.AdministrationRoleId = administrationRoleId; ;
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(administrationRoleId, role.AdministrationRoleId.Value);
            }

            // Test authorities
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                testRoleId = 5;
                role = GetUserManager().GetRole(GetUserContext(), testRoleId);
                role.Authorities[0].Obligation = @"UPDATE OBLIGATION STRING";
                role.Authorities[0].ActionGUIDs.Add("400");
                role.Authorities[0].ActionGUIDs.Add("401");
                role.Authorities[0].ActionGUIDs.Add("402");
                role.Authorities[0].ActionGUIDs.Add("403");
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(@"UPDATE OBLIGATION STRING", role.Authorities[0].Obligation);
                Assert.IsTrue(role.Authorities[0].ActionGUIDs.Count > 3);
            }

            // Test shortName
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetOneRole();
                shortName = @"MyShortNameUnique";
                role.ShortName = shortName;
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(shortName, role.ShortName);
            }

            // Test roleName.
            /*
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                roleName = @"RoleNameUnique";
                role = GetOneRole();
                role.Name = roleName;
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(roleName, role.Name);
            }
             * */

            // Test description.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen";
                role = GetOneRole();
                role.Description = description;
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(description, role.Description);
            }

            // Test userAdministrationRoleId
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                userAdministrationRoleId = 1;
                role = GetOneRole();
                role.UserAdministrationRoleId = userAdministrationRoleId;
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(userAdministrationRoleId, role.UserAdministrationRoleId);
            }

            // Test OrganizationId
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organizationId = Settings.Default.TestOrganizationId;
                role = GetOneRole();
                role.OrganizationId = organizationId;
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(organizationId, role.OrganizationId);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                role = GetOneRole();
                role.ValidFromDate = validFromDate;
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(validFromDate, role.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                role = GetOneRole();
                role.ValidToDate = validToDate;
                GetUserManager().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(validToDate, role.ValidToDate);
            }
        }

        [TestMethod]
        public void UpdateUser()
        {
            Boolean showEmailAddress;
            DateTime validFromDate, validToDate;
            String emailAddress;
            IUser user;

            GetUserManager(true);

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                emailAddress = @"fdskfd.sdff@lkf.ld";
                user = GetNewUser();
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.EmailAddress = emailAddress;
                GetUserManager().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.EmailAddress, user.EmailAddress);
            }

            // Test show email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = false;
                user = GetNewUser();
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.ShowEmailAddress = showEmailAddress;
                GetUserManager().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.ShowEmailAddress, user.ShowEmailAddress);
            }
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = true;
                user = GetNewUser();
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.ShowEmailAddress = showEmailAddress;
                GetUserManager().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.ShowEmailAddress, user.ShowEmailAddress);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                user = GetNewUser();
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.ValidFromDate = validFromDate;
                GetUserManager().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.ValidFromDate, user.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                user = GetNewUser();
                GetUserManager().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.ValidToDate = validToDate;
                GetUserManager().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.ValidToDate, user.ValidToDate);
            }
        }

        [TestMethod]
        public void UserAdminSetPassword()
        {
            String newPassword = "TEst1243qwe";
            IUser user;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetNewUser();
                GetUserManager(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsTrue(GetUserManager(true).UserAdminSetPassword(GetUserContext(), user, newPassword));
            }
        }
    }
}
