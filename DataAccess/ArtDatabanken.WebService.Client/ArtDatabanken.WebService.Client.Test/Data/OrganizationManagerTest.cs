using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class OrganizationManagerTest : TestBase
    {
        private OrganizationManager _organizationManager;

        public OrganizationManagerTest()
        {
            _organizationManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            OrganizationManager organizationManager;

            organizationManager = new OrganizationManager();
            Assert.IsNotNull(organizationManager);
        }


        private IRole GetOneRole()
        {
            IRole role;

            // It is assumed that this method is called
            // inside a transaction.
            role = GetNewRole();
            CoreData.UserManager.CreateRole(GetUserContext(), role);
            return role;
        }

        private IRole GetNewRole()
        {
            IRole newRole;

            newRole = new Role(GetUserContext());
            newRole.Name = @"RoleName";
            newRole.ShortName = @"RoleShortName";
            newRole.Description = @"testdescription";
            newRole.Id = Settings.Default.TestRoleId;
            newRole.UserAdministrationRoleId = 1;
            return newRole;
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
            IPhoneNumber phoneNumber;
            String city, name, shortName, description, number,
                   postalAddress1, postalAddress2, zipCode;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetNewOrganization();
                GetOrganizationManager(true).CreateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(administrationRoleId, organization.AdministrationRoleId.Value);
            }

            // Test shortName
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                shortName = @"testshortname räksmörgås RÄKSMÖRGÅS";
                organization = GetNewOrganization();
                organization.ShortName = shortName;
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(shortName, organization.ShortName);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"testname räksmörgås RÄKSMÖRGÅS";
                organization = GetNewOrganization();
                organization.Name = name;
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(description, organization.Description);
            }

            // Test HasCollection
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                hasCollection = true;
                organization = GetNewOrganization();
                organization.HasSpeciesCollection = hasCollection;
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(hasCollection, organization.HasSpeciesCollection);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                organization = GetNewOrganization();
                organization.ValidFromDate = validFromDate;
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(validFromDate, organization.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                organization = GetNewOrganization();
                organization.ValidToDate = validToDate;
                GetOrganizationManager().CreateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager(true).CreateOrganizationCategory(GetUserContext(), organizationCategory);
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
                GetOrganizationManager().CreateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(administrationRoleId, organizationCategory.AdministrationRoleId.Value);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"testname räksmörgås RÄKSMÖRGÅS";
                organizationCategory = GetNewOrganizationCategory();
                organizationCategory.Name = name;
                GetOrganizationManager().CreateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(name, organizationCategory.Name);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                organizationCategory = GetNewOrganizationCategory();
                organizationCategory.Description = description;
                GetOrganizationManager().CreateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(description, organizationCategory.Description);
            }

        }


        [TestMethod]
        public void DataSource()
        {
            IUserDataSource dataSource;

            dataSource = null;
            GetOrganizationManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetOrganizationManager().DataSource);

            dataSource = new UserDataSource();
            GetOrganizationManager().DataSource = dataSource;
            Assert.AreEqual(dataSource, GetOrganizationManager().DataSource);
        }

        
        [TestMethod]
        public void DeleteOrganization()
        {
            IOrganization organization;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization = GetNewOrganization();
                GetOrganizationManager(true).CreateOrganization(GetUserContext(), organization);
                GetOrganizationManager().DeleteOrganization(GetUserContext(), organization);
            }
        }
         

        [TestMethod]
        public void GetDataSourceInformation()
        {
            Assert.IsNotNull(GetOrganizationManager(true).GetDataSourceInformation());
        }

        /*
        private IOrganization GetNewOrganization()
        {
            IOrganization newOrganization;

            newOrganization = new Organization(GetUserContext());
            newOrganization.Name = @"Fågelskådarna Name";
            newOrganization.ShortName = @"Fågelskådarna ShortName";
            newOrganization.Description = @"testdescription";
            newOrganization.Category = new OrganizationCategory(2, "Länsstyrelse", "Länsstyrelse", 0, 0, new UpdateInformation(), new DataContext(GetUserContext()));
            return newOrganization;
        }
         */

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
            newOrganizationCategory.Name = @"NewOrgCategory";
            newOrganizationCategory.Description = @"2 steg från Paradise";
            return newOrganizationCategory;
        }

        private IOrganization GetOneOrganization()
        {
            IOrganization organization;

            // It is assumed that this method is called
            // inside a transaction.
            organization = GetNewOrganization();
            GetOrganizationManager(true).CreateOrganization(GetUserContext(), organization);
            return organization;
        }

        private IOrganizationCategory GetOneOrganizationCategory()
        {
            IOrganizationCategory organizationCategory;

            // It is assumed that this method is called
            // inside a transaction.
            organizationCategory = GetNewOrganizationCategory();
            GetOrganizationManager(true).CreateOrganizationCategory(GetUserContext(), organizationCategory);
            return organizationCategory;
        }


        [TestMethod]
        public void GetOrganization()
        {
            IOrganization organization1, organization2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organization1 = GetNewOrganization();
                GetOrganizationManager(true).CreateOrganization(GetUserContext(), organization1);
                organization2 = GetOrganizationManager().GetOrganization(GetUserContext(), organization1.Id);
                Assert.AreEqual(organization1.Id, organization2.Id);
            }
        }

        [TestMethod]
        public void GetOrganizationCategory()
        {
            IOrganizationCategory organizationCategory1, organizationCategory2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organizationCategory1 = GetNewOrganizationCategory();
                GetOrganizationManager(true).CreateOrganizationCategory(GetUserContext(), organizationCategory1);
                organizationCategory2 = GetOrganizationManager().GetOrganizationCategory(GetUserContext(), organizationCategory1.Id);
                Assert.AreEqual(organizationCategory1.Id, organizationCategory2.Id);
            }
        }

        [TestMethod]
        public void GetOrganizationCategories()
        {
            List<WebOrganizationCategory> organizationCategories;
            Int32 organizationId = Settings.Default.TestOrganizationId;
            organizationCategories = WebServiceProxy.UserService.GetOrganizationCategories(GetClientInformation());
            Assert.IsTrue(organizationCategories.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetOrganizationIdError()
        {
            IOrganization organization;
            organization = GetOrganizationManager(true).GetOrganization(GetUserContext(), Int32.MinValue);
            Assert.IsNull(organization);
        }

        private OrganizationManager GetOrganizationManager()
        {
            return GetOrganizationManager(false);
        }

        private OrganizationManager GetOrganizationManager(Boolean refresh)
        {
            if (_organizationManager.IsNull() || refresh)
            {
                _organizationManager = new OrganizationManager();
                _organizationManager.DataSource = new UserDataSource();
            }
            return _organizationManager;
        }

        [TestMethod]
        public void GetOrganizationRoles()
        {
            RoleList roles;
            Int32 organizationId = Settings.Default.TestOrganizationId;
            roles = GetOrganizationManager(true).GetRolesByOrganization(GetUserContext(), organizationId);
            Assert.IsTrue(roles.IsNotEmpty());
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
            organizations = GetOrganizationManager(true).GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            name = "Tilt%";
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.Name = name;
            organizations = GetOrganizationManager().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test short name.
            name = "Art%";
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.ShortName = name;
            organizations = GetOrganizationManager().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            name = "Tilt%";
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.ShortName = name;
            organizations = GetOrganizationManager().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test organization category Id.
            organizationCategoryId = 3;
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.OrganizationCategoryId = organizationCategoryId;
            organizations = GetOrganizationManager().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            organizationCategoryId = -1;
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.OrganizationCategoryId = organizationCategoryId;
            organizations = GetOrganizationManager().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test hasSpiecesCollection
            hasSpiecesCollection = true;
            searchCriteria = new OrganizationSearchCriteria();
            searchCriteria.HasSpiecesCollection = hasSpiecesCollection;
            organizations = GetOrganizationManager().GetOrganizationsBySearchCriteria(GetUserContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());
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
            String city, name, shortName,  number,
                   organizationCategoryName, organizationCategoryDescription,
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
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(administrationRoleId, organization.AdministrationRoleId.Value);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"Maria";
                organization = GetOneOrganization();
                organization.Name = name;
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(name, organization.Name);
            }


            // Test shortName.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                shortName = @"Uppdaterat shortname";
                organization = GetOneOrganization();
                organization.ShortName = shortName;
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(description, organization.Description);
            }

            // Test HasCollection
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                hasCollection = true;
                organization = GetOneOrganization();
                organization.HasSpeciesCollection = hasCollection;
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(hasCollection, organization.HasSpeciesCollection);
            }

            // Test organization category.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                organizationCategoryDescription = @"De svenska länsstyrelserna";
                organizationCategoryName = @"Länsstyrelse";
                organizationCategory = new OrganizationCategory(GetUserContext());
                organizationCategory.Id = 3;
                organizationCategory.Name = organizationCategoryName;
                organizationCategory.Description = organizationCategoryDescription;
                GetOrganizationManager().UpdateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organization);
                Assert.AreEqual(organizationCategoryDescription, organizationCategory.Description);
                Assert.AreEqual(organizationCategoryName, organizationCategory.Name);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                organization = GetOneOrganization();
                organization.ValidFromDate = validFromDate;
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
                Assert.IsNotNull(organization);
                Assert.AreEqual(validFromDate, organization.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                organization = GetOneOrganization();
                organization.ValidToDate = validToDate;
                GetOrganizationManager().UpdateOrganization(GetUserContext(), organization);
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
                GetOrganizationManager().UpdateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(administrationRoleId, organizationCategory.AdministrationRoleId.Value);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"OrganizationCateroryName";
                organizationCategory = GetOneOrganizationCategory();
                organizationCategory.Name = name;
                GetOrganizationManager().UpdateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(name, organizationCategory.Name);
            }

            // Test description.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen";
                organizationCategory = GetOneOrganizationCategory();
                organizationCategory.Description = description;
                GetOrganizationManager().UpdateOrganizationCategory(GetUserContext(), organizationCategory);
                Assert.IsNotNull(organizationCategory);
                Assert.AreEqual(description, organizationCategory.Description);
            }

        }

    }
}
