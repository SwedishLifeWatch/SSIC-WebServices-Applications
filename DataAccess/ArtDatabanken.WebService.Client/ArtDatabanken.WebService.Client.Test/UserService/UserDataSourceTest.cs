using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.UserService
{
    [TestClass]
    public class UserDataSourceTest : TestBase
    {
        private UserDataSource _userDataSource;
        private ApplicationDataSource _applicationDataSource;

        public UserDataSourceTest()
        {
            _userDataSource = null;
        }

        [TestMethod]
        public void Constructor()
        {
            UserDataSource userDataSource;

            userDataSource = new UserDataSource();
            Assert.IsNotNull(userDataSource);
        }

        [TestMethod]
        public void ActivateRoleMembership()
        {
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                Assert.IsTrue(GetUserDataSource().ActivateRoleMembership(GetUserContext(), 1));
                Assert.IsFalse(GetUserDataSource().ActivateRoleMembership(GetUserContext(), 100));
            }
        }

        [TestMethod]
        public void ActivateUserAccount()
        {
            const String activationKey = "6DFR8QI7IqrViqBQ1PhP4RKbCfla6n";

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                Assert.IsTrue(GetUserDataSource().ActivateUserAccount(GetUserContext(), Settings.Default.TestUserName, activationKey));
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
                GetUserDataSource(true).AddUserToRole(GetUserContext(), roleId, userId);
            }
        }

        [TestMethod]
        public void ApplicationActionExists()
        {
            String applicationActionIdentifier;
            applicationActionIdentifier = Settings.Default.TestApplicationActionIdentifier;
            Assert.IsTrue(GetUserDataSource(true).ApplicationActionExists(GetUserContext(), applicationActionIdentifier));

            // Test non existing action identifier
            applicationActionIdentifier = "NonExistingIdentifier";
            Assert.IsFalse(GetUserDataSource(true).ApplicationActionExists(GetUserContext(), applicationActionIdentifier));
        }

        [TestMethod]
        public void ApplicationActionExistsInRole()
        {
            String applicationActionIdentifier;
            Role role;
            role = (Role) GetUserDataSource().GetRole(GetUserContext(), Settings.Default.TestRoleId);

            applicationActionIdentifier = Settings.Default.TestApplicationActionIdentifier;
            Assert.IsTrue(GetUserDataSource(true).ApplicationActionExists(GetUserContext(), role, applicationActionIdentifier));

            // Test non existing action identifier
            applicationActionIdentifier = "NonExistingIdentifier";
            Assert.IsFalse(GetUserDataSource(true).ApplicationActionExists(GetUserContext(), role, applicationActionIdentifier));
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
            isUnique = GetUserDataSource(true).CheckStringIsUnique(GetUserContext(), value, objectName, propertyName);
            Assert.IsTrue(isUnique);

            // Check not unique value
            value = "Artportalen";
            isUnique = GetUserDataSource(true).CheckStringIsUnique(GetUserContext(), value, objectName, propertyName);
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
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                GetUserDataSource(true).CreateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
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
                authority.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(administrationRoleId, authority.AdministrationRoleId.Value);
            }


            // Test authorityIdentity
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authorityIdentity = @"Test AuthorityIdentity XX";
                authority = GetNewAuthority();
                authority.Identifier = authorityIdentity;
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(authorityIdentity, authority.Identifier);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                authority = GetNewAuthority();
                authority.Description = description;
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(description, authority.Description);
            }

            // Test name
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"MyAuthorityName";
                authority = GetNewAuthority();
                authority.Name = name;
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(name, authority.Name);
            }

            // Test obligation
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                obligation = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                authority = GetNewAuthority();
                authority.Obligation = obligation;
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(maxProtectionLevel, authority.MaxProtectionLevel);
            }

            // Test showNonPublicData
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showNonPublicData = true;
                authority = GetNewAuthority();
                authority.ReadNonPublicPermission = showNonPublicData;
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ReadNonPublicPermission);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                authority = GetNewAuthority();
                authority.ValidFromDate = validFromDate;
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(validFromDate, authority.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                authority = GetNewAuthority();
                authority.ValidToDate = validToDate;
                GetUserDataSource().CreateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(validToDate, authority.ValidToDate);
            }

        }

        [TestMethod]
        public void CreateOrganization()
        {
            Boolean hasCollection;
            DateTime validFromDate, validToDate;
            IAddress address;
            ICountry country;
            Int32 administrationRoleId;
            IOrganization organization;
            IOrganizationCategory organizationCategory;
            IPhoneNumber phoneNumber;
            String city, name, shortName, description, number,
                   postalAddress1, postalAddress2, zipCode;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetNewOrganization();
                GetUserDataSource(true).CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, organization.UpdateInformation.CreatedBy);
                Assert.AreEqual(organization.UpdateInformation.ModifiedBy, organization.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - organization.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test DataContext.
                Assert.IsNotNull(organization.DataContext);

                // Test GUID.
                Assert.IsTrue(organization.GUID.IsNotEmpty());

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, organization.Id);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, organization.UpdateInformation.ModifiedBy);
                Assert.AreEqual(organization.UpdateInformation.ModifiedBy, organization.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - organization.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }

            // Test addresses.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetNewOrganization();
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
                organization.Addresses.Add(address);
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.IsTrue(organization.Addresses.IsNotEmpty());
                Assert.AreEqual(1, organization.Addresses.Count);
                Assert.AreEqual(city, organization.Addresses[0].City);
                Assert.AreEqual(country.Id, organization.Addresses[0].Country.Id);
                Assert.AreEqual(postalAddress1, organization.Addresses[0].PostalAddress1);
                Assert.AreEqual(postalAddress2, organization.Addresses[0].PostalAddress2);
                Assert.AreEqual(zipCode, organization.Addresses[0].ZipCode);
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetNewOrganization();
                administrationRoleId = 42;
                organization.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(administrationRoleId, organization.AdministrationRoleId.Value);
            }


            // Test shortName
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                shortName = @"testshortname räksmörgås RÄKSMÖRGÅS";
                organization = GetNewOrganization();
                organization.ShortName = shortName;
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(shortName, organization.ShortName);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"testname räksmörgås RÄKSMÖRGÅS";
                organization = GetNewOrganization();
                organization.Name = name;
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(name, organization.Name);
            }

            // Test phone numbers.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetNewOrganization();
                phoneNumber = new PhoneNumber(GetUserContext());
                country = CoreData.CountryManager.GetCountry(GetUserContext(), CountryId.Sweden);
                phoneNumber.Country = country;
                number = "018-67 10 00";
                phoneNumber.Number = number;
                organization.PhoneNumbers.Add(phoneNumber);
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.IsTrue(organization.PhoneNumbers.IsNotEmpty());
                Assert.AreEqual(1, organization.PhoneNumbers.Count);
                Assert.AreEqual(country.Id, organization.PhoneNumbers[0].Country.Id);
                Assert.AreEqual(number, organization.PhoneNumbers[0].Number);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                organization = GetNewOrganization();
                organization.Description = description;
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(description, organization.Description);
            }

            // Test HasCollection
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                hasCollection = true;
                organization = GetNewOrganization();
                organization.HasSpeciesCollection = hasCollection;
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(hasCollection, organization.HasSpeciesCollection);
            }


            // Test organization type 
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetNewOrganization();
                organizationCategory = new OrganizationCategory(GetUserContext());
                organizationCategory.Id = 2;
                organizationCategory.Name = @"Valideringsorganisation för fåglar";
                organizationCategory.Description = @"Validering och granskning av observationsrapporter för fåglar utförs av olika kommittéer.";
                organization.Category = organizationCategory;
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(organizationCategory.Description, organization.Category.Description);
                Assert.AreEqual(organizationCategory.Name, organization.Category.Name);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                organization = GetNewOrganization();
                organization.ValidFromDate = validFromDate;
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(validFromDate, organization.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                organization = GetNewOrganization();
                organization.ValidToDate = validToDate;
                GetUserDataSource().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(validToDate, organization.ValidToDate);
            }

        }

        [TestMethod]
        public void CreateOrganizationCategory()
        {
            Int32 administrationRoleId;
            IOrganizationCategory organizationCategory;
            String name, description;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organizationCategory = GetNewOrganizationCategory();
                GetUserDataSource(true).CreateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, organizationCategory.UpdateInformation.CreatedBy);
                Assert.AreEqual(organizationCategory.UpdateInformation.ModifiedBy, organizationCategory.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - organizationCategory.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test DataContext.
                Assert.IsNotNull(organizationCategory.DataContext);

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, organizationCategory.Id);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, organizationCategory.UpdateInformation.ModifiedBy);
                Assert.AreEqual(organizationCategory.UpdateInformation.ModifiedBy, organizationCategory.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - organizationCategory.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }


            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organizationCategory = GetNewOrganizationCategory();
                administrationRoleId = 42;
                organizationCategory.AdministrationRoleId = administrationRoleId; 
                GetUserDataSource().CreateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(administrationRoleId, organizationCategory.AdministrationRoleId.Value);
            }


            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"testname räksmörgås RÄKSMÖRGÅS";
                organizationCategory = GetNewOrganizationCategory();
                organizationCategory.Name = name;
                GetUserDataSource().CreateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(name, organizationCategory.Name);
            }


            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                organizationCategory = GetNewOrganizationCategory();
                organizationCategory.Description = description;
                GetUserDataSource().CreateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(description, organizationCategory.Description);
            }

        }

        [TestMethod]
        public void CreatePerson()
        {
            Boolean hasSpeciesCollection, showAddress, showEmailAddress, showPersonalInformation,
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
                GetUserDataSource(true).CreatePerson(GetUserContext(), person);
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
                GetUserDataSource().CreatePerson(GetUserContext(), person);
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
                person.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(administrationRoleId, person.AdministrationRoleId.Value);
            }

            // Test birt year.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                birthYear = null;
                person.BirthYear = birthYear;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsFalse(person.BirthYear.HasValue);

                person = GetNewPerson();
                person.EmailAddress += "t";
                birthYear = DateTime.Now;
                person.BirthYear = birthYear;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.BirthYear.HasValue);
                Assert.IsTrue((birthYear.Value - person.BirthYear.Value) < new TimeSpan(0, 0, 1));
            }

            // Test death year.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                deathYear = null;
                person.DeathYear = deathYear;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsFalse(person.DeathYear.HasValue);

                person = GetNewPerson();
                person.EmailAddress += "t";
                deathYear = DateTime.Now;
                person.DeathYear = deathYear;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
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
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(emailAddress, person.EmailAddress);
            }

            // Test first name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                firstName = @"Maria";
                person = GetNewPerson();
                person.FirstName = firstName;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(firstName, person.FirstName);
            }

            // Test gender.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                gender = CoreData.UserManager.GetPersonGender(GetUserContext(), PersonGenderId.Woman);
                person = GetNewPerson();
                person.Gender = gender;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(gender.Id, person.Gender.Id);
            }

            // Test has species collection.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                hasSpeciesCollection = false;
                person = GetNewPerson();
                person.HasSpeciesCollection = hasSpeciesCollection;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(hasSpeciesCollection, person.HasSpeciesCollection);

                hasSpeciesCollection = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.HasSpeciesCollection = hasSpeciesCollection;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(hasSpeciesCollection, person.HasSpeciesCollection);
            }

            // Test last name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                lastName = @"Ripa";
                person = GetNewPerson();
                person.LastName = lastName;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(lastName, person.LastName);
            }

            // Test locale.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                locale = CoreData.LocaleManager.GetLocales(GetUserContext())[0];
                person = GetNewPerson();
                person.Locale = locale;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(locale.Id, person.Locale.Id);
            }

            // Test middle name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                middleName = @"Barret";
                person = GetNewPerson();
                person.MiddleName = middleName;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
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
                GetUserDataSource().CreatePerson(GetUserContext(), person);
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
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(presentation, person.Presentation);
            }

            // Test show address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showAddress = false;
                person = GetNewPerson();
                person.ShowAddresses = showAddress;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showAddress, person.ShowAddresses);

                showAddress = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowAddresses = showAddress;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showAddress, person.ShowAddresses);
            }

            // Test show email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = false;
                person = GetNewPerson();
                person.ShowEmailAddress = showEmailAddress;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showEmailAddress, person.ShowEmailAddress);

                showEmailAddress = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowEmailAddress = showEmailAddress;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showEmailAddress, person.ShowEmailAddress);
            }

            // Test show personal information.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPersonalInformation = false;
                person = GetNewPerson();
                person.ShowPersonalInformation = showPersonalInformation;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPersonalInformation, person.ShowPersonalInformation);

                showPersonalInformation = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowPersonalInformation = showPersonalInformation;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPersonalInformation, person.ShowPersonalInformation);
            }

            // Test show phone numbers.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPhoneNumbers = false;
                person = GetNewPerson();
                person.ShowPhoneNumbers = showPhoneNumbers;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPhoneNumbers, person.ShowPhoneNumbers);

                showPhoneNumbers = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowPhoneNumbers = showPhoneNumbers;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPhoneNumbers, person.ShowPhoneNumbers);
            }

            // Test show presentation.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPresentation = false;
                person = GetNewPerson();
                person.ShowPresentation = showPresentation;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPresentation, person.ShowPresentation);

                showPresentation = true;
                person = GetNewPerson();
                person.EmailAddress += "2";
                person.ShowPresentation = showPresentation;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPresentation, person.ShowPresentation);
            }

            // Test taxon name type id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                taxonNameTypeId = (Int32)(TaxonNameCategoryId.ScientificName);
                person = GetNewPerson();
                person.TaxonNameTypeId = taxonNameTypeId;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(taxonNameTypeId, person.TaxonNameTypeId);
            }

            // Test url.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                url = @"http://artdata.slu.se";
                person = GetNewPerson();
                person.URL = url;
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(url, person.URL);
            }

            // Test user.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = null;
                person = GetNewPerson();
                person.SetUser(GetUserContext(), user);
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(user, person.GetUser(GetUserContext()));

                user = CoreData.UserManager.GetUser(GetUserContext());
                person = GetNewPerson();
                person.EmailAddress += "t";
                person.SetUser(GetUserContext(), user);
                GetUserDataSource().CreatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(user.Id, person.GetUser(GetUserContext()).Id);
            }
        }

        [TestMethod]
        public void CreateRole()
        {
            DateTime validFromDate, validToDate;
            AuthorityList authorityList;
            IAuthority authority;
            Int32 administrationRoleId, userAdministrationRoleId, authorityId, organizationId;
            IRole role;
            String roleName, description, shortName;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                GetUserDataSource(true).CreateRole(GetUserContext(), role);
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
                role.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(administrationRoleId, role.AdministrationRoleId.Value);
            }

            // Test authorityList.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                GetUserDataSource().CreateRole(GetUserContext(), role);
                authorityId = 2;
                authorityList = new AuthorityList();
                authority = GetUserDataSource().GetAuthority(GetUserContext(), authorityId);
                Assert.IsNotNull(authority);
                authorityList.Add(authority);
                Assert.IsNotNull(role);
                role.Authorities = authorityList;
                Assert.AreEqual("UserManager.UpdateUser", role.Authorities[0].Identifier);
            }

            // Test roleName
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                roleName = @"Test RoleName XX";
                role = GetNewRole();
                role.Name = roleName;
                GetUserDataSource().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(roleName, role.Name);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                role = GetNewRole();
                role.Description = description;
                GetUserDataSource().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(description, role.Description);
            }

            // Test organization id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                organizationId = Settings.Default.TestOrganizationId;
                role.OrganizationId = organizationId;
                GetUserDataSource().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(organizationId, role.OrganizationId.Value);
            }

            // Test shortName
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                shortName = @"RoleShortName";
                role = GetNewRole();
                role.ShortName = shortName;
                GetUserDataSource().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(shortName, role.ShortName);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                role = GetNewRole();
                role.ValidFromDate = validFromDate;
                GetUserDataSource().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(validFromDate, role.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                role = GetNewRole();
                role.ValidToDate = validToDate;
                GetUserDataSource().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(validToDate, role.ValidToDate);
            }

            // Test user administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetNewRole();
                userAdministrationRoleId = 42;
                role.UserAdministrationRoleId = userAdministrationRoleId;
                GetUserDataSource().CreateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(userAdministrationRoleId, role.UserAdministrationRoleId.Value);
            }


        }

        [TestMethod]
        public void CreateUser()
        {
            Boolean showEmailAddress;
            DateTime validFromDate, validToDate;
            String emailAddress, userName;
            IUser user;
            Int32? administrationRoleId;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetNewUser();
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
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

                // Test person.
                Assert.IsNull(user.GetPerson(GetUserContext()));
            }

            Thread.Sleep(1000);

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetNewUser();
                administrationRoleId = 42;
                user.AdministrationRoleId = administrationRoleId;
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(administrationRoleId, user.AdministrationRoleId.Value);
            }

            Thread.Sleep(1000);

            // Test NULL administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetNewUser();
                user.AdministrationRoleId = null;
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.IsNull(user.AdministrationRoleId);
            }

            Thread.Sleep(1000);

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                emailAddress = @"fdskfd.sdff@lksfdf.ldfk";
                user = GetNewUser();
                user.EmailAddress = emailAddress;
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(emailAddress, user.EmailAddress);
            }

            Thread.Sleep(1000);

            // Test show email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = false;
                user = GetNewUser();
                user.ShowEmailAddress = showEmailAddress;
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(showEmailAddress, user.ShowEmailAddress);
            }

            Thread.Sleep(1000);

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = true;
                user = GetNewUser();
                user.ShowEmailAddress = showEmailAddress;
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(showEmailAddress, user.ShowEmailAddress);
            }

            Thread.Sleep(1000);

            // Test user name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                userName = "hshshshsggsghg";
                user = GetNewUser();
                user.UserName = userName;
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(userName, user.UserName);
            }

            Thread.Sleep(1000);

            // Test user type.
            foreach (UserType userType in Enum.GetValues(typeof(UserType)))
            {
                using (ITransaction transaction = GetUserContext().StartTransaction())
                {
                    user = GetNewUser();
                    user.Type = userType;
                    GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                    Assert.IsNotNull(user);
                    Assert.AreEqual(userType, user.Type);
                }
            }

            Thread.Sleep(1000);

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                user = GetNewUser();
                user.ValidFromDate = validFromDate;
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(validFromDate, user.ValidFromDate);
            }

            Thread.Sleep(1000);

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                user = GetNewUser();
                user.ValidToDate = validToDate;
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsNotNull(user);
                Assert.AreEqual(validToDate, user.ValidToDate);
            }
        }

        [TestMethod]
        public void CreateUserReal()
        {
/*            IUser user;

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = new User(GetUserContext());
                user.EmailAddress = "Johan.Nilsson@artdata.slu.se";
                user.UserName = "Artportalen"; ;
                user.Type = UserType.Application;
                user.ValidFromDate = DateTime.Now;
                user.ValidToDate = user.ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
                GetUserDataSource(true).CreateUser(GetUserContext(), user, "");
                Assert.IsNotNull(user);
                transaction.Commit();
            }*/
        }

        [TestMethod]
        public void DeleteAuthority()
        {
            IAuthority authority;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetNewAuthority();
                GetUserDataSource(true).CreateAuthority(GetUserContext(), authority);
                GetUserDataSource().DeleteAuthority(GetUserContext(), authority);
            }
        }

        [TestMethod]
        public void DeleteOrganization()
        {
            IOrganization organization;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetNewOrganization();
                GetUserDataSource(true).CreateOrganization(GetUserContext(), organization);
                GetUserDataSource().DeleteOrganization(GetUserContext(), organization);
            }
        }

        [TestMethod]
        public void DeletePerson()
        {
            IPerson person;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetNewPerson();
                GetUserDataSource(true).CreatePerson(GetUserContext(), person);
                GetUserDataSource().DeletePerson(GetUserContext(), person);
            }
        }

        [TestMethod]
        public void DeleteRole()
        {
            IRole Role;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                Role = GetNewRole();
                GetUserDataSource(true).CreateRole(GetUserContext(), Role);
                GetUserDataSource().DeleteRole(GetUserContext(), Role);
            }
        }

        [TestMethod]
        public void DeleteUser()
        {
            IUser user;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetNewUser();
                GetUserDataSource(true).CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                GetUserDataSource().DeleteUser(GetUserContext(), user);
            }
        }

        [TestMethod]
        public void GetAddressTypes()
        {
            AddressTypeList addressTypes;

            addressTypes = GetUserDataSource(true).GetAddressTypes(GetUserContext());
            Assert.IsTrue(addressTypes.IsNotEmpty());
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
                GetUserDataSource(true).CreateAuthority(GetUserContext(), authority1);
                authority2 = GetUserDataSource().GetAuthority(GetUserContext(), authority1.Id);
                Assert.AreEqual(authority1.Id, authority2.Id);
                Assert.AreEqual(authority1.Name, authority2.Name);
                Assert.AreEqual(authority1.ActionGUIDs[0], authority2.ActionGUIDs[0]);
                Assert.AreEqual("200", authority2.ActionGUIDs[1]);
                Assert.AreEqual(authority1.AuthorityType.ToString(), authority2.AuthorityType.ToString());
                Assert.AreEqual(authority2.AuthorityType.ToString(), AuthorityType.Application.ToString());
                Assert.AreEqual(authority2.AuthorityDataType, null);
                
            }
        }

        [TestMethod]
        public void GetAuthorityUsingAuthorityDataType()
        {
            IAuthority authority1, authority2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority1 = GetNewAuthorityUsingAutorityDataType();
                authority1.ActionGUIDs.Add("100");
                authority1.ActionGUIDs.Add("200");
                GetUserDataSource(true).CreateAuthority(GetUserContext(), authority1);
                authority2 = GetUserDataSource().GetAuthority(GetUserContext(), authority1.Id);
                Assert.AreEqual(authority1.Id, authority2.Id);
                Assert.AreEqual(authority1.Name, authority2.Name);
                Assert.AreEqual(authority1.ActionGUIDs[0], authority2.ActionGUIDs[0]);
                Assert.AreEqual("200", authority2.ActionGUIDs[1]);
                Assert.AreEqual(authority1.AuthorityType.ToString(), authority2.AuthorityType.ToString());
                Assert.AreEqual(authority2.AuthorityType.ToString(), AuthorityType.DataType.ToString());
                Assert.AreEqual(authority1.AuthorityDataType.Id, authority2.AuthorityDataType.Id);
                Assert.AreEqual(authority1.AuthorityDataType.Identifier, authority2.AuthorityDataType.Identifier);
                Assert.AreEqual(authority2.ApplicationId, 0);
            }
        }

        [TestMethod]
        public void GetAuthorities()
        {
            AuthorityList authorityList;
            Role role;
            Application application;
            role = (Role)GetUserDataSource().GetRole(GetUserContext(), Test.Settings.Default.TestRoleId);
            application = (Application)GetApplicationDataSource().GetApplication(GetUserContext(), Test.Settings.Default.TestApplicationId);

            authorityList = GetUserDataSource().GetAuthorities(GetUserContext(), role, application);
            Assert.IsTrue(authorityList.IsNotEmpty());
            Assert.IsInstanceOfType(authorityList[0], typeof(Authority));

            // Check the authorities connected to application and to datatype is correct.
            // Note! this test can be evalutated after checking the database...  Test OK 2011-09-09

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
            //role = (Role)GetUserDataSource().GetRole(GetUserContext(), 618);
            //application = (Application)GetApplicationDataSource().GetApplication(GetUserContext(), 528);

            //authorityList = GetUserDataSource().GetAuthorities(GetUserContext(), role, application);
            //Assert.IsTrue(authorityList.IsNotEmpty());
            //Assert.IsInstanceOfType(authorityList[0], typeof(Authority));
            //Assert.AreEqual(authorityList.Count, 1);
            //Assert.AreEqual(authorityList[0].AuthorityType, AuthorityType.Application);

            //// Third test is returning one datatype authority
            //role = (Role)GetUserDataSource().GetRole(GetUserContext(), 618);
            //application = (Application)GetApplicationDataSource().GetApplication(GetUserContext(), 1230);

            //authorityList = GetUserDataSource().GetAuthorities(GetUserContext(), role, application);
            //Assert.IsTrue(authorityList.IsNotEmpty());
            //Assert.IsInstanceOfType(authorityList[0], typeof(Authority));
            //Assert.AreEqual(authorityList.Count, 1);
            //Assert.AreEqual(authorityList[0].AuthorityType, AuthorityType.DataType);
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
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityIdentifier = "NotExistInDB%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Application Identifier
            applicationIdentifier = "UserService%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            applicationIdentifier = "NoServiceExistInDB%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test AuthorityDataType Idenetifier
            authorityDataTypeIdentifier = "Species%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityDataTypeIdentifier = "NoObsExistInDB%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Authority Name.
            authorityName = "test%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityName = "noTestExistInDB%";
            searchCriteria = new AuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Finally test that if no critera is set(ie WebAuthoritySearchCriteria is created by no data is set to search for) will not generat a exception.
            searchCriteria = new AuthoritySearchCriteria();
            authorities = GetUserDataSource().GetAuthoritiesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());
        }

        [TestMethod]
        public void GetAuthoritiesWithIdentifier()
        {
            AuthorityList authorityList;
            Role role;
            Int32 applicationId = Test.Settings.Default.TestApplicationId;
            const String authorityIdentifier = "UserAdministration";
            role = (Role)GetUserDataSource().GetRole(GetUserContext(), Test.Settings.Default.TestRoleId);
            authorityList = GetUserDataSource().GetAuthorities(GetUserContext(), role, applicationId, authorityIdentifier);
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
            //role = (Role)GetUserDataSource().GetRole(GetUserContext(), 2);
            //authorityList = GetUserDataSource().GetAuthorities(GetUserContext(), role, applicationId, authorityIdentifier);
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
            //role = (Role)GetUserDataSource().GetRole(GetUserContext(), 618);
            //authorityList = GetUserDataSource().GetAuthorities(GetUserContext(), role, 1230, authorityIdentifier);
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
            authorityList = GetUserDataSource().GetAuthorities(GetUserContext(), userId, applicationId);
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
        public void GetLockedUserInformation()
        {
            LockedUserInformationList lockedUsers;
            String userName;
            StringSearchCriteria userNameSearchString;

            // Search with no locked user.
            GetUserDataSource(true).GetLockedUserInformation(GetUserContext(), null);

            userNameSearchString = new StringSearchCriteria();
            userNameSearchString.SearchString = "No user name";
            lockedUsers = GetUserDataSource().GetLockedUserInformation(GetUserContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userName = "qwertyClient" + DateTime.Now.ToString(GetUserContext().Locale.CultureInfo);
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            userNameSearchString = new StringSearchCriteria();
            userNameSearchString.SearchString = userName;
            lockedUsers = GetUserDataSource().GetLockedUserInformation(GetUserContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());

            // Search with locked user.
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            CoreData.UserManager.Login(userName,
                                       "hej hopp i lingon skogen",
                                       Settings.Default.DyntaxaApplicationIdentifier);
            lockedUsers = GetUserDataSource().GetLockedUserInformation(GetUserContext(), null);
            Assert.IsTrue(lockedUsers.IsNotEmpty());
            userNameSearchString = new StringSearchCriteria();
            userNameSearchString.SearchString = "No user name";
            lockedUsers = GetUserDataSource().GetLockedUserInformation(GetUserContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userNameSearchString = new StringSearchCriteria();
            userNameSearchString.SearchString = userName;
            lockedUsers = GetUserDataSource().GetLockedUserInformation(GetUserContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsNotEmpty());
            Assert.AreEqual(1, lockedUsers.Count);
            Assert.AreEqual(5, lockedUsers[0].LoginAttemptCount);
            Assert.AreEqual(userName, lockedUsers[0].UserName);
        }

        private IAuthority GetNewAuthority()
        {
            IAuthority newAuthority;

            newAuthority = new Authority(GetUserContext());
            newAuthority.Identifier = @"AuthorityIdentity";
            newAuthority.Name = @"MyAuthorityName";
            newAuthority.Obligation = @"testObligation";
            newAuthority.Description = @"testdescription";
            newAuthority.ApplicationId = Settings.Default.TestApplicationId;
            newAuthority.RoleId = Settings.Default.TestRoleId;
            newAuthority.ActionGUIDs = new List<String>();
            newAuthority.FactorGUIDs = new List<String>();
            newAuthority.LocalityGUIDs = new List<String>();
            newAuthority.ProjectGUIDs = new List<String>();
            newAuthority.RegionGUIDs = new List<String>();
            newAuthority.TaxonGUIDs = new List<String>();
            newAuthority.AuthorityType = AuthorityType.Application;
            return newAuthority;
        }

        private IAuthority GetNewAuthorityUsingAutorityDataType()
        {
            IAuthority newAuthority;

            newAuthority = new Authority(GetUserContext());
            newAuthority.Identifier = @"AuthorityIdentityUsingAuthorityDataType";
            newAuthority.Name = @"MyAuthorityUsingAuthorityDataTypeName";
            newAuthority.Obligation = @"testAuthorityDataTypeObligation";
            newAuthority.Description = @"testAuthorityDataTypedescription";
            newAuthority.RoleId = Settings.Default.TestRoleId;
            newAuthority.ActionGUIDs = new List<String>();
            newAuthority.FactorGUIDs = new List<String>();
            newAuthority.LocalityGUIDs = new List<String>();
            newAuthority.ProjectGUIDs = new List<String>();
            newAuthority.RegionGUIDs = new List<String>();
            newAuthority.TaxonGUIDs = new List<String>();
            DataContext dataContext = new DataContext(GetUserContext());
            AuthorityDataType authorityDataType = new AuthorityDataType(2, "Observationer", dataContext);
            newAuthority.AuthorityDataType = authorityDataType;
            //newAuthority.AuthorityType = AuthorityType.DataType;
            return newAuthority;
        }

        [TestMethod]
        public void GetAuthorityDataTypes()
        {
            AuthorityDataTypeList authorityDataTypes;

            authorityDataTypes = GetUserDataSource(true).GetAuthorityDataTypes(GetUserContext());
            Assert.IsTrue(authorityDataTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetAuthorityDataTypesByApplicationId()
        {
            AuthorityDataTypeList authorityDataTypes, authorityDataTypesTest;

            authorityDataTypes = GetUserDataSource(true).GetAuthorityDataTypesByApplicationId(GetUserContext(), Settings.Default.TestApplicationId);
            authorityDataTypesTest = GetAuthorityDataTypesForOneApplication();
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
        /// Reutrns authority data type list for Settings.Default.TestApplicationId.
        /// </summary>
        /// <returns></returns>
        private AuthorityDataTypeList GetAuthorityDataTypesForOneApplication()
        {
            AuthorityDataTypeList authorityDataTypeList = new AuthorityDataTypeList();
            IDataContext dataContext = new DataContext(GetUserContext());
            AuthorityDataType authorityDataType1 = new AuthorityDataType(1, "Test", dataContext);
            AuthorityDataType authorityDataType2 = new AuthorityDataType(2, "SpeciesObservation", dataContext);
            authorityDataTypeList.Add(authorityDataType1);
            authorityDataTypeList.Add(authorityDataType2);
            return authorityDataTypeList;
        }

        private IOrganization GetNewOrganization()
        {
            IOrganization newOrganization;

            newOrganization = new Organization(GetUserContext());
            newOrganization.Name = @"Fågelskådarna NameX";
            newOrganization.ShortName = @"Fågelskådarna ShortNameX";
            newOrganization.Description = @"testdescription";
            newOrganization.Category = new OrganizationCategory(1, "Universitetsinstitution", "Universitetsinstitution eller annan organisation ansluten till universitet eller högskola.", 123, 1, new UpdateInformation(), new DataContext(GetUserContext()));
            return newOrganization;
        }

        private IOrganizationCategory GetNewOrganizationCategory()
        {
            IOrganizationCategory newOrganizationCategory;

            newOrganizationCategory = new OrganizationCategory(GetUserContext());
            newOrganizationCategory.Name = @"Fågelskådarna Name";
            newOrganizationCategory.Description = @"testdescription";
            return newOrganizationCategory;
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
            newRole.ShortName = @"NewRoleShortName";
            newRole.Name = @"NewRole";
            newRole.Description = @"testdescription";
            return newRole;
        }

        private IUser GetNewUser(String emailAddress = @"MyEmail@Address")
        {
            IUser newUser;

            newUser = new User(GetUserContext());
            newUser.EmailAddress = emailAddress;
            newUser.UserName = Settings.Default.TestUserName + 42;
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
            GetUserDataSource(true).CreateAuthority(GetUserContext(), authority);
            return authority;
        }

        private IOrganization GetOneOrganization()
        {
            IOrganization organization;

            // It is assumed that this method is called
            // inside a transaction.
            organization = GetNewOrganization();
            GetUserDataSource(true).CreateOrganization(GetUserContext(), organization);
            return organization;
        }

        private IOrganizationCategory GetOneOrganizationCategory()
        {
            IOrganizationCategory organizationCategory;

            // It is assumed that this method is called
            // inside a transaction.
            organizationCategory = GetNewOrganizationCategory();
            GetUserDataSource(true).CreateOrganizationCategory(GetUserContext(), organizationCategory);
            return organizationCategory;
        }

        private IPerson GetOnePerson(String emailAddress = @"GetOnePerson@slu.se")
        {
            IPerson person;

            // It is assumed that this method is called
            // inside a transaction.
            person = GetNewPerson();
            person.EmailAddress = emailAddress;
            GetUserDataSource(true).CreatePerson(GetUserContext(), person);
            return person;
        }

        private IRole GetOneRole()
        {
            IRole role;

            // It is assumed that this method is called
            // inside a transaction.
            role = GetNewRole();
            GetUserDataSource(true).CreateRole(GetUserContext(), role);
            return role;
        }

        private IUser GetOneUser()
        {
            IUser user;
            String password;
            // It is assumed that this method is called
            // inside a transaction.
            user = GetNewUser();
            password = "newPASSwOrd12";
            GetUserDataSource(true).CreateUser(GetUserContext(), user, password);
            return user;
        }

        [TestMethod]
        public void GetOrganization()
        {
            IOrganization organization1, organization2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization1 = GetNewOrganization();
                GetUserDataSource(true).CreateOrganization(GetUserContext(), organization1);
                organization2 = GetUserDataSource().GetOrganization(GetUserContext(), organization1.Id);
                Assert.AreEqual(organization1.Id, organization2.Id);
            }
        }

        [TestMethod]
        public void GetOrganizationCategories()
        {
            OrganizationCategoryList OrganizationCategories;

            OrganizationCategories = GetUserDataSource(true).GetOrganizationCategories(GetUserContext());
            Assert.IsTrue(OrganizationCategories.IsNotEmpty());
            Assert.IsInstanceOfType(OrganizationCategories[0], typeof(OrganizationCategory));
        }

        [TestMethod]
        public void GetOrganizationCategory()
        {
            IOrganizationCategory organizationCategory1, organizationCategory2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organizationCategory1 = GetNewOrganizationCategory();
                GetUserDataSource(true).CreateOrganizationCategory(GetUserContext(), organizationCategory1);
                organizationCategory2 = GetUserDataSource().GetOrganizationCategory(GetUserContext(), organizationCategory1.Id);
                Assert.AreEqual(organizationCategory1.Id, organizationCategory2.Id);
            }
        }

        [TestMethod]
        public void GetOrganizations()
        {
            OrganizationList organizationList;

            organizationList = GetUserDataSource(true).GetOrganizations(GetUserContext());
            Assert.IsTrue(organizationList.IsNotEmpty());
            Assert.IsInstanceOfType(organizationList[0], typeof(Organization));
        }

        [TestMethod]
        public void GetOrganizationsByOrganizationCategory()
        {
            OrganizationList organizationList;

            organizationList = GetUserDataSource(true).GetOrganizationsByOrganizationCategory(GetUserContext(), Settings.Default.TestOrganizationCategoryId);
            Assert.IsTrue(organizationList.IsNotEmpty());
            Assert.IsInstanceOfType(organizationList[0], typeof(Organization));
        }

        [TestMethod]
        public void GetOrganizationsBySearchCriteria()
        {
            Int32 organizationCategoryId;
            OrganizationList organizations;
            String name;
            Boolean hasSpiecesCollection;
            OrganizationSearchCriteria searchCriteria;

            // Test organization name.
            name = "M%";
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.Name = name;
            organizations = GetUserDataSource(true).GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            name = "Tilt%";
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.Name = name;
            organizations = GetUserDataSource().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test short name.
            name = "Art%";
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.ShortName = name;
            organizations = GetUserDataSource().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            name = "Tilt%";
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.ShortName = name;
            organizations = GetUserDataSource().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test organization category Id.
            organizationCategoryId = 3;
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.OrganizationCategoryId = organizationCategoryId;
            organizations = GetUserDataSource().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            organizationCategoryId = -1;
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.OrganizationCategoryId = organizationCategoryId;
            organizations = GetUserDataSource().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test hasSpiecesCollection
            hasSpiecesCollection = true;
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.HasSpiecesCollection = hasSpiecesCollection;
            organizations = GetUserDataSource().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());
        }

        [TestMethod]
        public void GetPerson()
        {
            IPerson person1, person2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person1 = GetNewPerson();
                GetUserDataSource(true).CreatePerson(GetUserContext(), person1);
                person2 = GetUserDataSource().GetPerson(GetUserContext(), person1.Id);
                Assert.AreEqual(person1.Id, person2.Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetPersonIdError()
        {
            IPerson person;

            person = GetUserDataSource(true).GetPerson(GetUserContext(), Int32.MinValue);
            Assert.IsNull(person);
        }

        [TestMethod]
        public void GetPersonGenders()
        {
            PersonGenderList personGenders;

            personGenders = GetUserDataSource(true).GetPersonGenders(GetUserContext());
            Assert.IsTrue(personGenders.IsNotEmpty());
        }

        [TestMethod]
        public void GetPersonsBySearchCriteria()
        {
            PersonList persons;
            String name;
            Boolean hasSpiecesCollection;
            PersonSearchCriteria searchCriteria;

            // Test first name.
            name = "Test%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = GetUserDataSource(true).GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = GetUserDataSource().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = GetUserDataSource().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = GetUserDataSource().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = GetUserDataSource().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = GetUserDataSource().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test hasSpiecesCollection
            hasSpiecesCollection = true;
            searchCriteria = new PersonSearchCriteria();
            searchCriteria.HasSpiecesCollection = hasSpiecesCollection;
            persons = GetUserDataSource().GetPersonsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());
        }

        [TestMethod]
        public void GetPhoneNumberTypes()
        {
            PhoneNumberTypeList phoneNumberTypes;

            phoneNumberTypes = GetUserDataSource(true).GetPhoneNumberTypes(GetUserContext());
            Assert.IsTrue(phoneNumberTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetRole()
        {
            IRole role1, role2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role1 = GetNewRole();
                role1.ValidFromDate = new DateTime(2011,9,1);
                GetUserDataSource(true).CreateRole(GetUserContext(), role1);
                role2 = GetUserDataSource().GetRole(GetUserContext(), role1.Id);
                Assert.AreEqual(role1.Id, role2.Id);
                Assert.AreEqual(role1.ValidFromDate, role2.ValidFromDate);
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
            roles = GetUserDataSource(true).GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt2%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.Name = name;
            roles = GetUserDataSource().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test short name.
            name = "A%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = GetUserDataSource().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt2%";
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = GetUserDataSource().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test organization Id.
            organizationId = Settings.Default.TestOrganizationId;
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            roles = GetUserDataSource().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            organizationId = -1;
            searchCriteria = new RoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            roles = GetUserDataSource().GetRolesBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());
        }

        [TestMethod]
        public void GetUser()
        {
            IUser user1, user2;

            user1 = CoreData.UserManager.GetUser(GetUserContext());
            Assert.IsNotNull(user1);

            user2 = GetUserDataSource().GetUser(GetUserContext(), user1.Id);
            Assert.IsNotNull(user2);
            Assert.AreEqual(user1.UserName, user2.UserName);
            Assert.AreEqual(user1.Id, user2.Id);
        }

        [TestMethod]
        public void GetUserRoles()
        {
            RoleList roleList;

            roleList = GetUserDataSource(true).GetUserRoles(GetUserContext(), Settings.Default.TestUserId, Settings.Default.TestApplicationIdentifier);
            Assert.IsTrue(roleList.IsNotEmpty());
            Assert.IsInstanceOfType(roleList[0], typeof(Role));
        }

        private ApplicationDataSource GetApplicationDataSource()
        {
            return GetApplicationDataSource(false);
        }

        private ApplicationDataSource GetApplicationDataSource(Boolean refresh)
        {
            if (_applicationDataSource.IsNull() || refresh)
            {
                _applicationDataSource = new ApplicationDataSource();
            }
            return _applicationDataSource;
        }

        [TestMethod]
        public void GetApplicationUsers()
        {
            UserList users;
            users = GetUserDataSource(true).GetApplicationUsers(GetUserContext());
            Assert.IsFalse(users.IsEmpty());
        }

        private UserDataSource GetUserDataSource()
        {
            return GetUserDataSource(false);
        }

        private UserDataSource GetUserDataSource(Boolean refresh)
        {
            if (_userDataSource.IsNull() || refresh)
            {
                _userDataSource = new UserDataSource();
            }
            return _userDataSource;
        }

        [TestMethod]
        public void GetUsersByRole()
        {
            UserList users;
            Int32 roleId = Settings.Default.TestRoleId;
            users = GetUserDataSource(true).GetUsersByRole(GetUserContext(), roleId);
            Assert.IsFalse(users.IsEmpty());
        }

        [TestMethod]
        public void GetNonactivatedUsersByRole()
        {
            UserList users;
            Int32 roleId = Settings.Default.TestRoleId;
            users = GetUserDataSource(true).GetNonActivatedUsersByRole(GetUserContext(), roleId);
            Assert.IsTrue(users.IsEmpty());
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
            users = GetUserDataSource(true).GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = GetUserDataSource().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.FullName = name;
            users = GetUserDataSource().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.FullName = name;
            users = GetUserDataSource().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.LastName = name;
            users = GetUserDataSource().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.LastName = name;
            users = GetUserDataSource().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test organizationId
            Int32 organizationId = Settings.Default.TestOrganizationId;
            searchCriteria = new PersonUserSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            users = GetUserDataSource().GetUsersBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());
        }



        [TestMethod]
        public void IsExistingPerson()
        {
            const String emailAddress = @"artdata@slu.se";

            Assert.IsFalse(GetUserDataSource().IsExistingPerson(GetUserContext(), emailAddress + 42));
            Assert.IsTrue(GetUserDataSource().IsExistingPerson(GetUserContext(), emailAddress));
        }

        [TestMethod]
        public void IsExistingUser()
        {
            Assert.IsFalse(GetUserDataSource().IsExistingUser(GetUserContext(), Settings.Default.TestUserName + 42));
            Assert.IsTrue(GetUserDataSource().IsExistingUser(GetUserContext(), Settings.Default.TestUserName));
        }

        [TestMethod]
        public void Login()
        {
            IUserContext userContext;

            userContext = GetUserDataSource(true).Login(Settings.Default.TestUserName,
                                                        Settings.Default.TestPassword,
                                                        Settings.Default.TestApplicationIdentifier,
                                                        false);
            Assert.IsNotNull(userContext);
            Assert.IsNotNull(userContext.Locale);
            Assert.IsNotNull(userContext.CurrentRoles);
        }

        [TestMethod]
        public void Logout()
        {
            IUserContext userContext;

            userContext = GetUserDataSource(true).Login(Settings.Default.TestUserName,
                                                        Settings.Default.TestPassword,
                                                        Settings.Default.TestApplicationIdentifier,
                                                        false);
            GetUserDataSource().Logout(userContext);
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
                GetUserDataSource(true).CreateUser(GetUserContext(), user, password);
                passwordInformation = GetUserDataSource().ResetPassword(GetUserContext(), emailAddress);
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
                GetUserDataSource(true).AddUserToRole(GetUserContext(), roleId, userId);
                // Remove user from role
                GetUserDataSource(true).RemoveUserFromRole(GetUserContext(), roleId, userId);
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

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
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
                authority = GetOneAuthority();
                actions = new List<String>();
                actions.Add("1");
                actions.Add("2");
                actions.Add("3");
                authority.ActionGUIDs = actions;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
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
                localities.Add("20");
                localities.Add("21");
                localities.Add("22");
                authority.LocalityGUIDs = localities;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.LocalityGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.LocalityGUIDs.Count);
                Assert.AreEqual("20", authority.LocalityGUIDs[0]);
                Assert.AreEqual("21", authority.LocalityGUIDs[1]);
            }

            // Test regions.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authority = GetOneAuthority();
                regions = new List<String>();
                regions.Add("7");
                regions.Add("8");
                regions.Add("9");
                authority.RegionGUIDs = regions;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.RegionGUIDs.IsNotEmpty());
                Assert.AreEqual(3, authority.RegionGUIDs.Count);
                Assert.AreEqual("7", authority.RegionGUIDs[0]);
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
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
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
                authority.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(administrationRoleId, authority.AdministrationRoleId.Value);
            }


            // Test authorityIdentity
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                authorityIdentity = @"Test AuthorityIdentity XX";
                authority = GetOneAuthority();
                authority.Identifier = authorityIdentity;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(authorityIdentity, authority.Identifier);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                authority = GetOneAuthority();
                authority.Description = description;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(description, authority.Description);
            }

            // Test name
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"AuthorityNameUpdate";
                authority = GetOneAuthority();
                authority.Name = name;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(name, authority.Name);
            }

            // Test obligation
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                obligation = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                authority = GetOneAuthority();
                authority.Obligation = obligation;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
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
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(maxProtectionLevel, authority.MaxProtectionLevel);
            }

            // Test showNonPublicData
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showNonPublicData = true;
                authority = GetOneAuthority();
                authority.ReadNonPublicPermission = showNonPublicData;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.IsTrue(authority.ReadNonPublicPermission);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                authority = GetOneAuthority();
                authority.ValidFromDate = validFromDate;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(validFromDate, authority.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                authority = GetOneAuthority();
                authority.ValidToDate = validToDate;
                GetUserDataSource().UpdateAuthority(GetUserContext(), authority);
                Assert.IsNotNull(authority);
                Assert.AreEqual(validToDate, authority.ValidToDate);
            }
        }

        [TestMethod]
        public void UpdateOrganization()
        {
            Boolean hasCollection;
            DateTime validFromDate, validToDate;
            IAddress address;
            ICountry country;
            Int32 administrationRoleId;
            IOrganization organization;
            IOrganizationCategory organizationCategory;
            IPhoneNumber phoneNumber;
            String city, name, shortName, number,
                   organizationCategoryDescription, organizationCategoryName,
                   postalAddress1, postalAddress2, description, zipCode;

            // Test addresses.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetOneOrganization();
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
                organization.Addresses.Add(address);
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.IsTrue(organization.Addresses.IsNotEmpty());
                Assert.AreEqual(1, organization.Addresses.Count);
                Assert.AreEqual(city, organization.Addresses[0].City);
                Assert.AreEqual(country.Id, organization.Addresses[0].Country.Id);
                Assert.AreEqual(postalAddress1, organization.Addresses[0].PostalAddress1);
                Assert.AreEqual(postalAddress2, organization.Addresses[0].PostalAddress2);
                Assert.AreEqual(zipCode, organization.Addresses[0].ZipCode);
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetOneOrganization();
                administrationRoleId = 42;
                organization.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(administrationRoleId, organization.AdministrationRoleId.Value);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"Maria";
                organization = GetOneOrganization();
                organization.Name = name;
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(name, organization.Name);
            }


            // Test shortName.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                shortName = @"Uppdaterat shortname";
                organization = GetOneOrganization();
                organization.ShortName = shortName;
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(shortName, organization.ShortName);
            }

            // Test phone numbers.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetOneOrganization();
                phoneNumber = new PhoneNumber(GetUserContext());
                country = CoreData.CountryManager.GetCountry(GetUserContext(), CountryId.Sweden);
                phoneNumber.Country = country;
                number = "018-67 10 00";
                phoneNumber.Number = number;
                organization.PhoneNumbers.Add(phoneNumber);
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.IsTrue(organization.PhoneNumbers.IsNotEmpty());
                Assert.AreEqual(1, organization.PhoneNumbers.Count);
                Assert.AreEqual(country.Id, organization.PhoneNumbers[0].Country.Id);
                Assert.AreEqual(number, organization.PhoneNumbers[0].Number);
            }

            // Test description.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen";
                organization = GetOneOrganization();
                organization.Description = description;
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(description, organization.Description);
            }

            // Test HasCollection
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                hasCollection = true;
                organization = GetOneOrganization();
                organization.HasSpeciesCollection = hasCollection;
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(hasCollection, organization.HasSpeciesCollection);
            }

            // Test organization category 
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organizationCategoryDescription = @"De svenska länsstyrelserna";
                organizationCategoryName = @"Länsstyrelse";
                organizationCategory = new OrganizationCategory(GetUserContext());
                organizationCategory.Id = 3;
                organizationCategory.Name = organizationCategoryName;
                organizationCategory.Description = organizationCategoryDescription;
                GetUserDataSource().UpdateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(organizationCategoryDescription, organizationCategory.Description);
                Assert.AreEqual(organizationCategoryName, organizationCategory.Name);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                organization = GetOneOrganization();
                organization.ValidFromDate = validFromDate;
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(validFromDate, organization.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                organization = GetOneOrganization();
                organization.ValidToDate = validToDate;
                GetUserDataSource().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(validToDate, organization.ValidToDate);
            }
        }

        [TestMethod]
        public void UpdateOrganizationCategory()
        {
            Int32 administrationRoleId;
            IOrganizationCategory organizationCategory;
            String name, description;

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organizationCategory = GetOneOrganizationCategory();
                administrationRoleId = 42;
                organizationCategory.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().UpdateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(administrationRoleId, organizationCategory.AdministrationRoleId.Value);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"OrganizationCateroryName";
                organizationCategory = GetOneOrganizationCategory();
                organizationCategory.Name = name;
                GetUserDataSource().UpdateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(name, organizationCategory.Name);
            }

            // Test description.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen";
                organizationCategory = GetOneOrganizationCategory();
                organizationCategory.Description = description;
                GetUserDataSource().UpdateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(description, organizationCategory.Description);
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
                Assert.IsTrue(GetUserDataSource().UpdatePassword(GetUserContext(), oldPassword, newPassword));
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
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
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
                person.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(administrationRoleId, person.AdministrationRoleId.Value);
            }

            // Test birt year.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                person = GetOnePerson(@"UpdatePerson3@slu.se");
                birthYear = null;
                person.BirthYear = birthYear;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsFalse(person.BirthYear.HasValue);

                person = GetOnePerson(@"UpdatePerson4@slu.se");
                person.EmailAddress += "t";
                birthYear = DateTime.Now;
                person.BirthYear = birthYear;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
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
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsFalse(person.DeathYear.HasValue);

                person = GetOnePerson(@"UpdatePerson6@slu.se");
                person.EmailAddress += "t";
                deathYear = DateTime.Now;
                person.DeathYear = deathYear;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.IsTrue(person.DeathYear.HasValue);
                Assert.IsTrue((deathYear.Value - person.DeathYear.Value) < new TimeSpan(0, 0, 1));
            }

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                emailAddress = @"My.Email@Address.se2";
                person = GetOnePerson(@"UpdatePerson7@slu.se");
                person.EmailAddress = emailAddress;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(emailAddress, person.EmailAddress);
            }

            // Test first name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                firstName = @"Maria";
                person = GetOnePerson(@"UpdatePerson8@slu.se");
                person.FirstName = firstName;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(firstName, person.FirstName);
            }

            // Test gender.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                gender = CoreData.UserManager.GetPersonGender(GetUserContext(), PersonGenderId.Woman);
                person = GetOnePerson(@"UpdatePerson9@slu.se");
                person.Gender = gender;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(gender.Id, person.Gender.Id);
            }

            // Test last name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                lastName = @"Ripa";
                person = GetOnePerson(@"UpdatePerson10@slu.se");
                person.LastName = lastName;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(lastName, person.LastName);
            }

            // Test locale.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                locale = CoreData.LocaleManager.GetLocales(GetUserContext())[0];
                person = GetOnePerson(@"UpdatePerson11@slu.se");
                person.Locale = locale;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(locale.Id, person.Locale.Id);
            }

            // Test middle name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                middleName = @"Barret";
                person = GetOnePerson(@"UpdatePerson12@slu.se");
                person.MiddleName = middleName;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
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
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
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
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(presentation, person.Presentation);
            }

            // Test show address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showAddress = false;
                person = GetOnePerson(@"UpdatePerson15@slu.se");
                person.ShowAddresses = showAddress;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showAddress, person.ShowAddresses);

                showAddress = true;
                person = GetOnePerson(@"UpdatePerson16@slu.se");
                person.EmailAddress += "2";
                person.ShowAddresses = showAddress;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showAddress, person.ShowAddresses);
            }

            // Test show email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = false;
                person = GetOnePerson(@"UpdatePerson17@slu.se");
                person.ShowEmailAddress = showEmailAddress;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showEmailAddress, person.ShowEmailAddress);

                showEmailAddress = true;
                person = GetOnePerson(@"UpdatePerson18@slu.se");
                person.EmailAddress += "2";
                person.ShowEmailAddress = showEmailAddress;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showEmailAddress, person.ShowEmailAddress);
            }

            // Test show personal information.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPersonalInformation = false;
                person = GetOnePerson(@"UpdatePerson19@slu.se");
                person.ShowPersonalInformation = showPersonalInformation;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPersonalInformation, person.ShowPersonalInformation);

                showPersonalInformation = true;
                person = GetOnePerson(@"UpdatePerson20@slu.se");
                person.EmailAddress += "2";
                person.ShowPersonalInformation = showPersonalInformation;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPersonalInformation, person.ShowPersonalInformation);
            }

            // Test show phone numbers.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPhoneNumbers = false;
                person = GetOnePerson(@"UpdatePerson21@slu.se");
                person.ShowPhoneNumbers = showPhoneNumbers;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPhoneNumbers, person.ShowPhoneNumbers);

                showPhoneNumbers = true;
                person = GetOnePerson(@"UpdatePerson22@slu.se");
                person.EmailAddress += "2";
                person.ShowPhoneNumbers = showPhoneNumbers;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPhoneNumbers, person.ShowPhoneNumbers);
            }

            // Test show presentation.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showPresentation = false;
                person = GetOnePerson(@"UpdatePerson23@slu.se");
                person.ShowPresentation = showPresentation;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPresentation, person.ShowPresentation);

                showPresentation = true;
                person = GetOnePerson(@"UpdatePerson24@slu.se");
                person.EmailAddress += "2";
                person.ShowPresentation = showPresentation;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(showPresentation, person.ShowPresentation);
            }

            // Test taxon name type id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                taxonNameTypeId = (Int32)(TaxonNameCategoryId.ScientificName);
                person = GetOnePerson(@"UpdatePerson25@slu.se");
                person.TaxonNameTypeId = taxonNameTypeId;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(taxonNameTypeId, person.TaxonNameTypeId);
            }

            // Test url.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                url = @"http://artdata.slu.se";
                person = GetOnePerson(@"UpdatePerson26@slu.se");
                person.URL = url;
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(url, person.URL);
            }

            // Test user.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = null;
                person = GetOnePerson(@"UpdatePerson27@slu.se");
                person.SetUser(GetUserContext(), user);
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(user, person.GetUser(GetUserContext()));

                user = CoreData.UserManager.GetUser(GetUserContext());
                person = GetOnePerson();
                person.EmailAddress += "t";
                person.SetUser(GetUserContext(), user);
                GetUserDataSource().UpdatePerson(GetUserContext(), person);
                Assert.IsNotNull(person);
                Assert.AreEqual(user.Id, person.GetUser(GetUserContext()).Id);
            }
        }

        [TestMethod]
        public void UpdateRole()
        {
            DateTime validFromDate, validToDate;
            Int32 administrationRoleId, userAdministrationRoleId, testRoleId;
            IRole role;
            String roleName, description, shortName;

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetOneRole();
                administrationRoleId = 42;
                role.AdministrationRoleId = administrationRoleId;
                GetUserDataSource().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(administrationRoleId, role.AdministrationRoleId.Value);
            }

            // Test authorities
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                testRoleId = 5;
                role = GetUserDataSource().GetRole(GetUserContext(), testRoleId);
                role.Authorities[0].Obligation = @"UPDATE OBLIGATION STRING";
                role.Authorities[0].ActionGUIDs.Add("400");
                GetUserDataSource().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(@"UPDATE OBLIGATION STRING", role.Authorities[0].Obligation);
                Assert.IsTrue(role.Authorities[0].ActionGUIDs.Count > 1);
            }

            // Test shortName
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                role = GetOneRole();
                shortName = @"MyShortName";
                role.ShortName = shortName;
                GetUserDataSource().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(shortName, role.ShortName);
            }

            // Test roleName.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                roleName = @"RoleName";
                role = GetOneRole();
                role.Name = roleName;
                GetUserDataSource().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(roleName, role.Name);
            }

            // Test description.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen";
                role = GetOneRole();
                role.Description = description;
                GetUserDataSource().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(description, role.Description);
            }

            // Test userAdministrationRoleId
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                userAdministrationRoleId = 1;
                role = GetOneRole();
                role.UserAdministrationRoleId = userAdministrationRoleId;
                GetUserDataSource().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(userAdministrationRoleId, role.UserAdministrationRoleId);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                role = GetOneRole();
                role.ValidFromDate = validFromDate;
                GetUserDataSource().UpdateRole(GetUserContext(), role);
                Assert.IsNotNull(role);
                Assert.AreEqual(validFromDate, role.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                role = GetOneRole();
                role.ValidToDate = validToDate;
                GetUserDataSource().UpdateRole(GetUserContext(), role);
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

            GetUserDataSource(true);

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                emailAddress = @"fdskfd.sdff@lkf.ld";
                user = GetNewUser();
                GetUserDataSource().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.EmailAddress = emailAddress;
                GetUserDataSource().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.EmailAddress, user.EmailAddress);
            }

            // Test show email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = false;
                user = GetNewUser();
                GetUserDataSource().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.ShowEmailAddress = showEmailAddress;
                GetUserDataSource().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.ShowEmailAddress, user.ShowEmailAddress);
            }
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                showEmailAddress = true;
                user = GetNewUser();
                GetUserDataSource().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.ShowEmailAddress = showEmailAddress;
                GetUserDataSource().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.ShowEmailAddress, user.ShowEmailAddress);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                user = GetNewUser();
                GetUserDataSource().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.ValidFromDate = validFromDate;
                GetUserDataSource().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.ValidFromDate, user.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                user = GetNewUser();
                GetUserDataSource().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                user.ValidToDate = validToDate;
                GetUserDataSource().UpdateUser(GetUserContext(), user);
                Assert.AreEqual(user.ValidToDate, user.ValidToDate);
            }
        }

        [TestMethod]
        public void UserAdminSetPassword()
        {
            const String newPassword = "TEst1243qwe";
            IUser user;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                user = GetNewUser();
                GetUserDataSource().CreateUser(GetUserContext(), user, Settings.Default.TestPassword);
                Assert.IsTrue(GetUserDataSource().UserAdminSetPassword(GetUserContext(), user, newPassword));
            }
        }

    }
}
