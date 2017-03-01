using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class OrganizationManagerTest : TestBase
    {
        public OrganizationManagerTest()
            //: base(true, 50)
            : base(useTransaction, 50)
        {
        }

        #region Additional test attributes
        private TestContext testContextInstance;
        private static Boolean useTransaction = true;

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

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateOrganization()
        {
            WebOrganization organization;
            String sUniqueName;

            sUniqueName = "OrganizationUniqueName2";
            // Get existing organization.
            organization = OrganizationManager.GetOrganization(GetContext(), Settings.Default.TestOrganizationId);
            organization.Name = sUniqueName;
            organization.ShortName = "OrganizationUniqueShortName2";
            organization.Description = null;
            WebOrganization newOrganization;
            newOrganization = OrganizationManager.CreateOrganization(GetContext(), organization);
            Assert.IsNotNull(newOrganization);
            Assert.AreEqual(sUniqueName, newOrganization.Name);
            Assert.IsTrue(newOrganization.Id > Settings.Default.TestOrganizationId);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void CreateOrganizationWithNotUniqueName()
        {
            String errorMsg;
            WebOrganization organization;
            // Get existing organization.
            organization = OrganizationManager.GetOrganization(GetContext(), Settings.Default.TestOrganizationId);
            WebOrganization newOrganization;
            // Use same Name which should generate a SqlExeption
            try
            {
                newOrganization = OrganizationManager.CreateOrganization(GetContext(), organization);
            }
            catch (SqlException sqlEx)
            {
                errorMsg = sqlEx.Message;
                throw sqlEx;
            }
        }

        [TestMethod]
        public void CreateOrganizationCategory()
        {
            WebOrganizationCategory organizationCategory;
            Int32 organizationCategoryId;
            organizationCategoryId = 2;
            // Get existing organizationCategory.
            organizationCategory = OrganizationManager.GetOrganizationCategory(GetContext(), organizationCategoryId);
            // create unique name
            organizationCategory.Name = organizationCategory.Name + "123";
            organizationCategory.Description = null;
            organizationCategory.AdministrationRoleId = 42;
            WebOrganizationCategory newOrganizationCategory;
            newOrganizationCategory = OrganizationManager.CreateOrganizationCategory(GetContext(), organizationCategory);
            Assert.IsNotNull(newOrganizationCategory);
            Assert.AreEqual(organizationCategory.Name, newOrganizationCategory.Name);
            Assert.IsTrue(newOrganizationCategory.Id > organizationCategoryId);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteOrganization()
        {
            WebOrganization organization;
            // Get existing organization.
            organization = OrganizationManager.GetOrganization(GetContext(), Settings.Default.TestOrganizationId);
            OrganizationManager.DeleteOrganization(GetContext(), organization);
            organization = OrganizationManager.GetOrganization(GetContext(), Settings.Default.TestOrganizationId);
            Assert.IsNull(organization);
        }

        /// <summary>
        /// Gets the organization.
        /// </summary>
        [TestMethod]
        public void GetOrganization()
        {
            WebOrganization organization;
            // Get existing organization.
            organization = OrganizationManager.GetOrganization(GetContext(), Settings.Default.TestOrganizationId);
            Assert.IsNotNull(organization);
            Assert.AreEqual(Settings.Default.TestOrganizationName, organization.Name);
            Assert.AreEqual(Settings.Default.TestOrganizationId, organization.Id);
            Assert.IsNotNull(organization.Description);
            Assert.IsNotNull(organization.GUID);
            Assert.IsTrue(organization.HasSpeciesCollection);
            Assert.IsNotNull(organization.CreatedBy);
            Assert.IsNotNull(organization.CreatedDate);
            Assert.IsNotNull(organization.ModifiedBy);
            Assert.IsNotNull(organization.ModifiedDate);
            Assert.IsNotNull(organization.ValidFromDate);
            Assert.IsNotNull(organization.ValidToDate);
            Assert.AreEqual(organization.Category.Name, Settings.Default.TestOrganizationCategoryName);
        }

        [TestMethod]
        public void GetOrganizations()
        {
            
            List<WebOrganization> organizationList;
            // Get all organizations  
            organizationList = OrganizationManager.GetOrganizations(GetContext());
            Assert.IsNotNull(organizationList);
            Assert.IsTrue(organizationList.Count > 1);
            Assert.AreEqual(organizationList[0].Name, "ArtDatabanken");
            Assert.IsTrue(organizationList[0].HasSpeciesCollection);
        }

        [TestMethod]
        public void GetOrganizationsByOrganizationCategory()
        {
            List<WebOrganization> organizationList;
            // Get organizations by organization category
            organizationList = new List<WebOrganization>();
            organizationList = OrganizationManager.GetOrganizationsByOrganizationCategory(GetContext(), Settings.Default.TestOrganizationCategoryId);
            Assert.IsNotNull(organizationList);
            Assert.IsTrue(organizationList.Count > 0);
            
            Assert.IsTrue(organizationList[0].HasSpeciesCollection);
        }

        [TestMethod]
        public void GetOrganizationsBySearchCriteria()
        {
            List<WebOrganization> organizations;
            String name;
            Boolean hasSpiecesCollection;
            Int32 organizationCategoryId;
            WebOrganizationSearchCriteria searchCriteria;

            // Test organization name.
            name = "M%";
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.Name = name;
            organizations = OrganizationManager.GetOrganizationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.Name = name;
            organizations = OrganizationManager.GetOrganizationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test short name.
            name = "A%";
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.ShortName = name;
            organizations = OrganizationManager.GetOrganizationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.ShortName = name;
            organizations = OrganizationManager.GetOrganizationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test organizationCategoryId
            organizationCategoryId = 3;
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.OrganizationCategoryId = organizationCategoryId;
            searchCriteria.IsOrganizationCategoryIdSpecified = true;
            organizations = OrganizationManager.GetOrganizationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            organizationCategoryId = -1;
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.OrganizationCategoryId = organizationCategoryId;
            searchCriteria.IsOrganizationCategoryIdSpecified = true; organizations = OrganizationManager.GetOrganizationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test hasSpiecesCollection
            hasSpiecesCollection = true;
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.HasSpeciesCollection = hasSpiecesCollection;
            searchCriteria.IsHasSpeciesCollectionSpecified = true;
            organizations = OrganizationManager.GetOrganizationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            // Test with character '.
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.Name = "And'ers något";
            searchCriteria.ShortName = "And'ers";
            organizations = OrganizationManager.GetOrganizationsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());
        }

        [TestMethod]
        public void GetOrganizationRoles()
        {
            List<WebRole> organizationRoles;
            // Get all roles for an organization  
            organizationRoles = OrganizationManager.GetOrganizationRoles(GetContext(), Settings.Default.TestOrganizationId);
            Assert.IsNotNull(organizationRoles);
            Assert.IsTrue(organizationRoles.Count > 0);
        }

        [TestMethod]
        public void GetOrganizationCategory()
        {
            WebOrganizationCategory organizationCategory;
            // Get organization type
            organizationCategory = OrganizationManager.GetOrganizationCategory(GetContext(), 1);
            Assert.IsNotNull(organizationCategory);
            Assert.AreEqual(1,organizationCategory.Id);
            Assert.IsTrue(organizationCategory.Name.Length > 5);
            Assert.IsTrue(organizationCategory.Description.Length > 5);
        }

        [TestMethod]
        public void GetOrganizationCategories()
        {
            List<WebOrganizationCategory> organizationCategories;

            // Get all organization categories.
            organizationCategories = OrganizationManager.GetOrganizationCategories(GetContext());
            Assert.IsNotNull(organizationCategories);
            Assert.IsTrue(organizationCategories.Count > 1);
            Assert.IsTrue(organizationCategories[0].Id > 0);
            Assert.IsTrue(organizationCategories[1].Id > 0);
            Assert.IsNotNull(organizationCategories[0].Description);
            Assert.IsNotNull(organizationCategories[1].Description);

            // Check if information is cahced.
            organizationCategories = OrganizationManager.GetOrganizationCategories(GetContext());
            Assert.IsNotNull(organizationCategories);
        }

        private WebOrganization GetNewWebOrganization()
        {
            WebOrganization newOrganization;
            WebOrganizationCategory organizationCategory;
            List<WebAddress> addresses;

            newOrganization = new WebOrganization();
            newOrganization.Name = @"Fågelskådarna Name3";
            newOrganization.ShortName = @"Fågelskådarna ShortName2";
            newOrganization.Description = @"testdescription";
            newOrganization.CategoryId = 2;
            organizationCategory = new WebOrganizationCategory();
            newOrganization.Category = organizationCategory;
            newOrganization.Category.Id = 2;
            addresses = new List<WebAddress>();
            newOrganization.Addresses = addresses;
            return newOrganization;
        }

        private WebOrganization GetOneOrganization()
        {
            WebOrganization organization;

            // It is assumed that this method is called
            // inside a transaction.
            organization = GetNewWebOrganization();
            organization = OrganizationManager.CreateOrganization(GetContext(), organization);
            return organization;
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetNonExistingOrganization()
        {
            WebOrganization organization;
            // Set testdata
            Int32 organizationId = -1;
            // Try to get non-existing organization.
            organization = OrganizationManager.GetOrganization(GetContext(), organizationId);
            Assert.IsNull(organization);
        }

        [TestMethod]
        public void UpdateOrganization()
        {
            // Assert.IsFalse(useTransaction);
            WebOrganization organization;
            // Get existing organization.
            organization = OrganizationManager.GetOrganization(GetContext(), Settings.Default.TestOrganizationId);
            organization.Name = "TestNameUpdate3";
            organization.ShortName = "TestOrgShortName3";
            organization.ModifiedBy = Settings.Default.TestUserId;
            organization.AdministrationRoleId = Settings.Default.TestUserId;
            organization.Category.Id = 2;
            organization.Description = "Testdescription update";
            organization.ValidFromDate = DateTime.Now;
            organization.ValidToDate = DateTime.Today.AddYears(100);

            //organization.Addresses[0].PostalAddress1 = "PA1 update1";
            //organization.PhoneNumbers[0].Number = "090-101010";

            WebOrganization updatedOrganization;
            updatedOrganization = OrganizationManager.UpdateOrganization(GetContext(), organization);
            Assert.IsNotNull(updatedOrganization);
            Assert.AreEqual(Settings.Default.TestOrganizationId, updatedOrganization.Id);
            Assert.AreEqual(updatedOrganization.Description, organization.Description);
            Assert.AreEqual(updatedOrganization.ModifiedBy, Settings.Default.TestUserId);
            Assert.AreEqual(updatedOrganization.Name, organization.Name);
            Assert.AreEqual(updatedOrganization.ShortName, organization.ShortName);
            Assert.IsNotNull(updatedOrganization.ValidFromDate);
            Assert.IsNotNull(updatedOrganization.ValidToDate);
            // Number of address records
           // Assert.AreEqual(1, updatedOrganization.Addresses.Count);
            // Number of phone records
          //  Assert.AreEqual(1, updatedOrganization.PhoneNumbers.Count);

            // Update description for organization from blank to not blank
            organization = new WebOrganization();
            organization.Name = "OrganizationUniqueName4";
            organization.ShortName = "OrganizationUniqueShortName4";
            organization.Category = new WebOrganizationCategory();
            organization.Category.Id = 2;
            organization.Description = null;
            WebOrganization newOrganization = new WebOrganization();
            newOrganization = OrganizationManager.CreateOrganization(GetContext(), organization);
            newOrganization.Description = "Description update";
            updatedOrganization = OrganizationManager.UpdateOrganization(GetContext(), newOrganization);
            Assert.IsNotNull(updatedOrganization.Description);
        }

        [TestMethod]
        public void UpdateOrganizationAddress()
        {
            String city, postalAddress1, postalAddress2, zipCode;

            WebOrganization organization, updatedOrganization;
            WebAddress address;
            WebCountry country;
            WebAddressType addressType;
            organization = new WebOrganization();
            organization = GetOneOrganization();
            address = new WebAddress();
            city = "Uppsala";
            address.City = city;
            country = new WebCountry();
            country = CountryManager.GetCountry(GetContext(), Settings.Default.TestCountryId);
            address.Country = country;
            postalAddress1 = "";
            address.PostalAddress1 = postalAddress1;
            postalAddress2 = "ArtDatabanken, SLU";
            address.PostalAddress2 = postalAddress2;
            zipCode = "752 52";
            address.ZipCode = zipCode;
            addressType = new WebAddressType();
            addressType.Id = 1;
            address.Type = addressType;
            organization.Addresses.Add(address);

            updatedOrganization = new WebOrganization();
            updatedOrganization = OrganizationManager.UpdateOrganization(GetContext(), organization);

            Assert.IsNotNull(updatedOrganization);
            Assert.IsTrue(updatedOrganization.Addresses.IsNotEmpty());
            Assert.AreEqual(1, updatedOrganization.Addresses.Count);
            Assert.AreEqual(city, updatedOrganization.Addresses[0].City);
            Assert.AreEqual(country.Id, updatedOrganization.Addresses[0].Country.Id);
            Assert.AreEqual(postalAddress1, updatedOrganization.Addresses[0].PostalAddress1);
            Assert.AreEqual(postalAddress2, updatedOrganization.Addresses[0].PostalAddress2);
            Assert.AreEqual(zipCode, updatedOrganization.Addresses[0].ZipCode);
        }

        [TestMethod]
        public void UpdateOrganizationCategory()
        {
            WebOrganizationCategory organizationCategory;
            // Get existing organization category.
            organizationCategory = OrganizationManager.GetOrganizationCategory(GetContext(), Settings.Default.TestOrganizationCategoryId);
            organizationCategory.Name = "TestNameUpdate";
            organizationCategory.ModifiedBy = Settings.Default.TestUserId;
            organizationCategory.AdministrationRoleId = Settings.Default.TestUserId;
            organizationCategory.Description = "Testdescription update";

            WebOrganizationCategory updatedOrganizationCategory;
            updatedOrganizationCategory = OrganizationManager.UpdateOrganizationCategory(GetContext(), organizationCategory);
            Assert.IsNotNull(updatedOrganizationCategory);
            Assert.AreEqual(Settings.Default.TestOrganizationCategoryId, updatedOrganizationCategory.Id);
            Assert.AreEqual(updatedOrganizationCategory.ModifiedBy, Settings.Default.TestUserId);
            Assert.AreEqual(updatedOrganizationCategory.Name, organizationCategory.Name);
            Assert.AreEqual(updatedOrganizationCategory.Description, organizationCategory.Description);

            // Update description for organization category from blank to not blank
            organizationCategory = new WebOrganizationCategory();
            organizationCategory.Name = "UniqueName";
            organizationCategory.Description = null;
            WebOrganizationCategory newOrganizationCategory = new WebOrganizationCategory();
            newOrganizationCategory = OrganizationManager.CreateOrganizationCategory(GetContext(), organizationCategory);
            newOrganizationCategory.Description = "Description update";
            updatedOrganizationCategory = OrganizationManager.UpdateOrganizationCategory(GetContext(), newOrganizationCategory);
            Assert.IsNotNull(updatedOrganizationCategory.Description);
        }

    }
}
