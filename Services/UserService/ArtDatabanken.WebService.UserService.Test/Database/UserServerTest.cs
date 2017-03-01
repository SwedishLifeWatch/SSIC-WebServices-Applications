using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.UserService.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.UserService.Test.Database
{
    [TestClass]
    public class UserServerTest 
    {
        private UserServer _database;

        public UserServerTest()
        {
            _database = null;
        }

        private Int32 LocaleId
        {
            get
            {
                switch (Configuration.CountryId)
                {
                    case CountryId.Norway:
                        return Settings.Default.NorwayLocaleId;
                    case CountryId.Sweden:
                        return Settings.Default.SwedenLocaleId;
                    default:
                        throw new Exception("Not handled country " + Configuration.CountryId);
                }
            }
        }

        [TestMethod()]
        public void AddAuthorityDataTypeToApplication()
        {
            int authorityDataTypeId = 1;
            int applicationId = 1;
            GetDatabase(true).AddAuthorityDataTypeToApplication(authorityDataTypeId, applicationId);
        }

        [TestMethod()]
        public void AddUserToRole()
        {
            int roleId = 1; 
            int userId = 1; 
            GetDatabase().AddUserToRole(roleId, userId);
        }

        [TestMethod]
        public void CheckStringIsUnique()
        {
            String value, objectName, propertyName;
            value = "Test";
            objectName = "Application";
            propertyName = "Name";
            Assert.IsTrue(GetDatabase(true).CheckStringIsUnique(value, LocaleId, objectName, propertyName));
        }

        [TestMethod]
        public void Constructor()
        {
            using (WebServiceDataServer database = new UserServer())
            {
                Assert.IsNotNull(database);
            }
        }

        [TestMethod()]
        public void CreateApplication()
        {
            string applicationIdentity = "AppIdentity";
            string name = "AppName" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string shortName = "AppShortName" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string URL = "http://testurl.artdata.se";
            string description = "description";
            Nullable<int> contactPersonId = new Nullable<int>(1); 
            Nullable<int> administrationRoleId = new Nullable<int>(1);
            int createdBy = 1;

            DateTime validFromDate = DateTime.Now;
            DateTime validToDate = DateTime.Today.AddYears(100);

            int applicationId;
            applicationId = GetDatabase().CreateApplication(applicationIdentity, name, shortName, URL, description, LocaleId, contactPersonId, administrationRoleId, createdBy, validFromDate, validToDate);
            Assert.IsTrue(applicationId > 0);
        }

        [TestMethod()]
        public void CreateApplicationAction()
        {
            Int32 applicationId = 1;
            String name = "AppActionName";
            String actionIdentity = "ActionIdentity";
            String description = "description";
            Int32? administrationRoleId = 1;
            Int32 createdBy = 1;

            DateTime validFromDate = DateTime.Now;
            DateTime validToDate = DateTime.Today.AddYears(100);

            int id = GetDatabase().CreateApplicationAction(applicationId, name, actionIdentity, description, LocaleId, administrationRoleId, createdBy, validFromDate, validToDate);
            Assert.IsTrue(id > 0);
        }

        [TestMethod()]
        public void CreateApplicationVersion()
        {
            Int32 applicationId = 1;
            String version = "1.1";
            Boolean isRecommended = true;
            Boolean isValid = true;
            String description = "description";
            Int32 createdBy = 1;

            DateTime validFromDate = DateTime.Now;
            DateTime validToDate = DateTime.Today.AddYears(100);

            int id = GetDatabase(true).CreateApplicationVersion(applicationId, version, isRecommended, isValid, description, LocaleId, createdBy, validFromDate, validToDate);
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void CreateAuthority()
        {
            Int32 roleId, applicationId, maxProtectionLevel, createdBy, authorityId, authorityDataTypeId;
            Int32? administrationRoleId;
            String authorityIdentity, name, description, obligation;
            Boolean showNonPublicData, readPermission, createPermission, updatePermission, deletePermission;
            DateTime validFromDate, validToDate;

            roleId = 1;
            applicationId = 1;
            authorityDataTypeId = 1;
            maxProtectionLevel = 1;
            createdBy = 1;
            administrationRoleId = 1;

            authorityIdentity = "Test";
            name = "Test";
            description = "Test description";
            obligation = "Test obligation";

            showNonPublicData = false;
            readPermission = true;
            createPermission = false;
            updatePermission = false;
            deletePermission = false;

            validFromDate = DateTime.Now;
            validToDate = DateTime.Today.AddYears(100);

            authorityId = GetDatabase(true).CreateAuthority(roleId, applicationId, authorityIdentity, authorityDataTypeId, name, showNonPublicData, maxProtectionLevel,
                readPermission, createPermission, updatePermission, deletePermission, administrationRoleId, description, obligation, LocaleId, 
                createdBy, validFromDate, validToDate);
            Assert.IsTrue(authorityId > 0);
        }

        [TestMethod]
        public void CreateAuthorityAttribute()
        {            
            Int32 authorityId = 1;
            Int32 authorityAttributeTypeId = 1;
            String attributeValue = "1";
            GetDatabase().CreateAuthorityAttribute(authorityId, authorityAttributeTypeId, attributeValue);
        }

        [TestMethod]
        public void CreateOrganization()
        {
            String name, shortName, description;
            Int32 ? administrationRoleId;
            Int32 createdBy, organizationCategoryId, organizationId;
            Boolean hasCollection;
            DateTime validFromDate, validToDate;
            name = "OrganizationUniqueName5";
            shortName = "OrganizationUniqueShortName5";
            description = "Description";

            administrationRoleId = 1;
            organizationCategoryId = 1;
            createdBy = 1;
            hasCollection = false;

            validFromDate = DateTime.Now;
            validToDate = DateTime.Today.AddYears(100);

            organizationId = GetDatabase(true).CreateOrganization(name, shortName, description, administrationRoleId, hasCollection, organizationCategoryId, createdBy, LocaleId, validFromDate, validToDate);
            Assert.IsNotNull(organizationId);
        }

        [TestMethod]
        public void CreateOrganizationCategory()
        {
            String name, description;
            Int32? administrationRoleId;
            Int32 createdBy, organizationCategoryId;
            name = "OrganizationCategoryUniqueName";

            description = "Description";

            administrationRoleId = 1;
            createdBy = 1;

            organizationCategoryId = GetDatabase(true).CreateOrganizationCategory(name, description, administrationRoleId, createdBy, LocaleId);
            Assert.IsNotNull(organizationCategoryId);
        }

        [TestMethod]
        public void DeleteRole()
        {
            Int32 roleId, modifiedBy;
            //roleId = 1009;
            roleId = 1010;
            modifiedBy = 1;
            GetDatabase(true).DeleteRole(roleId, modifiedBy);
        }

        [TestMethod]
        public void DeleteRoleMembers()
        {
            Int32 deletedCount;

            deletedCount = GetDatabase(true).DeleteRoleMembers();
            Assert.IsTrue(0 <= deletedCount);
        }

        private UserServer GetDatabase(Boolean refresh = false)
        {
            if (_database.IsNull() || refresh)
            {
                if (_database.IsNotNull())
                {
                    _database.Dispose();
                }
                _database = new UserServer();
                _database.BeginTransaction();
            }
            return _database;
        }

        [TestMethod]
        public void GetAddress()
        {
            String address;

            address = UserServer.GetAddress();
            Assert.IsTrue(address.IsNotEmpty());
        }

        [TestMethod]
        public void GetAddressTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetAddressTypes(LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetApplicationById()
        {
            Int32 applicationId;

            applicationId = 1;
            using (DataReader dataReader = GetDatabase(true).GetApplicationById(applicationId, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetApplicationByIdentifier(ApplicationIdentifier.UserService.ToString(), LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetApplicationByIdentifier()
        {
            using (DataReader dataReader = GetDatabase(true).GetApplicationByIdentifier(ApplicationIdentifier.UserService.ToString(), LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetApplicationActionsByIds()
        {
            List<Int32> applicationActions = new List<Int32>();
            applicationActions = new List<Int32>();
            applicationActions.Add(3);
            applicationActions.Add(4);
            using (DataReader dataReader = GetDatabase(true).GetApplicationActionsByIds(applicationActions, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetAuthoritiesBySearchCriteria()
        {
            using (DataReader dataReader = GetDatabase(true).GetAuthoritiesBySearchCriteria("U%", null, null, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetAuthoritiesBySearchCriteria(null, "UserService%", null, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetAuthoritiesBySearchCriteria(null, null, "Speci%", null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetAuthoritiesBySearchCriteria(null, null, null, "test%", LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetAuthorityDataTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetAuthorityDataTypes())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetAuthorityDataTypesByApplicationId()
        {
            using (DataReader dataReader = GetDatabase(true).GetAuthorityDataTypesByApplicationId(Settings.Default.TestApplicationId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetCountries()
        {
            using (DataReader dataReader = GetDatabase(true).GetCountries())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetMessageTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetMessageTypes(LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetOrganizations()
        {
            using (DataReader dataReader = GetDatabase(true).GetOrganizations(Settings.Default.TestOrganizationCategoryId, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetOrganizationsBySearchCriteria()
        {
            using (DataReader dataReader = GetDatabase(true).GetOrganizationsBySearchCriteria("L%", null, null, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetOrganizationsBySearchCriteria(null, "L%", null, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetOrganizationsBySearchCriteria(null, null, 3, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetOrganizationsBySearchCriteria(null, null, null, false, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetOrganizationCategory()
        {
            using (DataReader dataReader = GetDatabase(true).GetOrganizationCategory(Settings.Default.TestOrganizationCategoryId, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }

        }

        [TestMethod]
        public void GetLocales()
        {
            using (DataReader dataReader = GetDatabase(true).GetLocales())
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetLocaleById()
        {
            using (DataReader dataReader = GetDatabase(true).GetLocaleById(LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetPhoneNumberTypes()
        {
            using (DataReader dataReader = GetDatabase(true).GetPhoneNumberTypes(LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetPerson()
        {
            using (DataReader dataReader = GetDatabase(true).GetPerson(Settings.Default.TestPersonId, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }

            Int32 personIdFail = -1;
            using (DataReader dataReader = GetDatabase(true).GetPerson(personIdFail, LocaleId))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetPersonGenders()
        {
            using (DataReader dataReader = GetDatabase(true).GetPersonGenders(LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetPersonsBySearchCriteria()
        {
            using (DataReader dataReader = GetDatabase(true).GetPersonsBySearchCriteria("Test%", null, null, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase(true).GetPersonsBySearchCriteria(null, "Test%", null, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase(true).GetPersonsBySearchCriteria(null, null, "Test%", null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetPersonsBySearchCriteria(null, null, null, true, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }

        }

        [TestMethod]
        public void GetRolesBySearchCriteria()
        {
            using (DataReader dataReader = GetDatabase(true).GetRolesBySearchCriteria("A%", null, null, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase(true).GetRolesBySearchCriteria(null, "A%", null, null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            // test Identifier
            using (DataReader dataReader = GetDatabase(true).GetRolesBySearchCriteria(null, null, "A%", null, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetRolesBySearchCriteria(null, null, null, 1, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetRolesByUserGroupAdministrationRoleId()
        {
            using (DataReader dataReader = GetDatabase(true).GetRolesByUserGroupAdministrationRoleId(27, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }

            
        }

        [TestMethod]
        public void GetRolesByUserGroupAdministratorUserId()
        {
            using (DataReader dataReader = GetDatabase(true).GetRolesByUserGroupAdministratorUserId(1, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetUser()
        {
            using (DataReader dataReader = GetDatabase(true).GetUser(Settings.Default.TestUserName))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase(true).GetUser("testuserfail"))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetUserRoles()
        {
            using (DataReader dataReader = GetDatabase(true).GetRolesByUser(Settings.Default.TestUserId, Settings.Default.TestApplicationIdentifier, LocaleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetUsersByRole()
        {
            using (DataReader dataReader = GetDatabase(true).GetUsersByRole(Settings.Default.TestRoleId))
            {
                Assert.IsTrue(dataReader.Read());
            }
        }

        [TestMethod]
        public void GetUsersBySearchCriteria()
        {
            using (DataReader dataReader = GetDatabase(true).GetUsersBySearchCriteria(null, null, null, null, null, null, null, null, null, null))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetUsersBySearchCriteria("Test%", null, null, null , null, null, null, null, null, null))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase(true).GetUsersBySearchCriteria(null, "Test%", null, null, null, null, null, null, null, null))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase(true).GetUsersBySearchCriteria(null, null, "Test%", null, null, null, null, null, null, null))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetUsersBySearchCriteria(null, null, null, null, null, "Application", null, null, null, null))
            {
                Assert.IsTrue(dataReader.Read());
            }
            using (DataReader dataReader = GetDatabase(true).GetUsersBySearchCriteria(null, null, null, null, null, "Person", null, null, null, 3))
            {
                Assert.IsTrue(dataReader.Read());
            }

        }

        [TestMethod]
        public void IsEmailAddressUnique()
        {
            Boolean isEmailAddressUnique;

            // Test method IsEmailAddressUnique.
            isEmailAddressUnique = GetDatabase(true).IsEmailAddressUnique("NoEmailAddress", null, null);
            Assert.IsTrue(isEmailAddressUnique);
            isEmailAddressUnique = GetDatabase().IsEmailAddressUnique("Bjorn.Karlsson@slu.se", null, null);
            Assert.IsFalse(isEmailAddressUnique);
        }

        [TestMethod]
        public void Login()
        {
            using (DataReader dataReader = GetDatabase(true).Login(Settings.Default.TestUserName, Settings.Default.TestPasswordHash, false))
            {
                Assert.IsTrue(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase(true).Login(Settings.Default.TestUserName, "Hhejhiopp123", false))
            {
                Assert.IsFalse(dataReader.Read());
            }

            using (DataReader dataReader = GetDatabase(true).Login("testuserfail", Settings.Default.TestPasswordHash, false))
            {
                Assert.IsFalse(dataReader.Read());
            }
        }

        [TestMethod]
        public void Ping()
        {
            using (WebServiceDataServer database = new UserServer())
            {
                Assert.IsTrue(database.Ping());
            }
        }

        [TestMethod()]
        public void RemoveAuthorityDataTypeFromApplication()
        {
            int authorityDataTypeId = 1;
            int applicationId = 1;
            GetDatabase().RemoveAuthorityDataTypeFromApplication(authorityDataTypeId, applicationId);
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            if (_database.IsNotNull())
            {
                _database.RollbackTransaction();
                _database.Dispose();
                _database = null;
            }
        }
        

        [TestMethod]
        public void UpdateOrganization()
        {
            String name, shortName, description;
            Int32? administrationRoleId;
            Int32 modifiedBy, organizationCategoryId, organizationId;
            Boolean hasCollection;
            DateTime validFromDate, validToDate;
            name = "OrganizationUniqueName6";
            shortName = "OrganizationUniqueShortName6";
            description = "Description";

            organizationId = 1;
            administrationRoleId = 1;
            organizationCategoryId = 1;
            modifiedBy = 1;
            hasCollection = false;

            validFromDate = DateTime.Now;
            validToDate = DateTime.Today.AddYears(100);

            GetDatabase(true).UpdateOrganization(organizationId, name, shortName, organizationCategoryId, administrationRoleId, hasCollection, description, LocaleId, modifiedBy, validFromDate, validToDate);
        }

        [TestMethod]
        public void UpdatePerson()
        {
            Int32 Id = 1;
            String firstName = "test";
            String middleName = "test";
            String lastName = "test";
            Int32 genderId = 1;
            String emailAddress = "test@slu.se";
            Boolean showEmail = true;
            Boolean showAddresses = true;
            Boolean showPhoneNumbers = true;
            DateTime? birthYear  = DateTime.Today.AddYears(-30);
            DateTime? deathYear  = DateTime.Today.AddYears(50);
            Int32? administrationRoleId = 1;
            Boolean hasCollection = true;
            Int32 taxonNameTypeId = 1;
            String URL = "http://artdata.slu.se";
            String presentation = "Presentation";
            Boolean showPresentation  = true;
            Boolean showPersonalInformation  = true;
            Int32? userId = 1;
            Int32 modifiedBy = 1;

            GetDatabase(true).UpdatePerson(Id, firstName, middleName, lastName, genderId, emailAddress, showEmail, showAddresses,
                                    showPhoneNumbers, birthYear, deathYear, administrationRoleId, hasCollection, LocaleId, taxonNameTypeId,
                                    URL, presentation, showPresentation, showPersonalInformation, userId, modifiedBy);
            }

        [TestMethod]
        public void UserAdminSetPassword()
        {
            String newPassword = @"MyNewPassword";
            Assert.IsTrue(GetDatabase(true).UserAdminSetPassword(Settings.Default.TestUserId, newPassword));
        }
    }
}
