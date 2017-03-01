using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy.Test
{
    [TestClass]
    public class UserServiceProxyTest
    {
        private WebClientInformation _clientInformation;

        public UserServiceProxyTest()
        {
            _clientInformation = null;
        }

        [TestMethod]
        public void ActivateUserAccount()
        {
            WebUser user;
            String activationKey = "6DFR8QI7IqrViqBQ1PhP4RKbCfla6n";

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                user = GetExistingUser();
                Assert.IsTrue(WebServiceProxy.UserService.ActivateUserAccount(GetClientInformation(), user.UserName, activationKey));
            }
        }

        [TestMethod]
        public void ActivateRoleMembership()
        {
            Boolean isActivated1 = false;
            Boolean isActivated2 = false;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                isActivated1 = WebServiceProxy.UserService.ActivateRoleMembership(GetClientInformation(), 1);
                isActivated2 = WebServiceProxy.UserService.ActivateRoleMembership(GetClientInformation(), 100);
            }

            Assert.IsTrue(isActivated1);
            Assert.IsFalse(isActivated2);
        }

        [TestMethod]
        public void ActivateUserAccountActivationKeyFailure()
        {
            WebUser user;
            String activationKey = "Abc123efG";

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                user = GetExistingUser();
                Assert.IsFalse(WebServiceProxy.UserService.ActivateUserAccount(GetClientInformation(), user.UserName, activationKey));
            }
        }

        [TestMethod]
        public void AddAuthorityDataTypeToApplication()
        {
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                Int32 applicationId = 1;
                Int32 authorityDataTypeId = 2;
                WebServiceProxy.UserService.AddAuthorityDataTypeToApplication(GetClientInformation(), authorityDataTypeId, applicationId);
            }
        }

        [TestMethod]
        public void AddUserToRole()
        {
            WebUser webUser;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                webUser = GetOneUser(@"AddUserToRole@slu.se");
                Int32 userId = webUser.Id;
                Int32 roleId = Settings.Default.TestRoleId;
                WebServiceProxy.UserService.AddUserToRole(GetClientInformation(), roleId, userId);
            }
        }

        [TestMethod]
        public void CheckStringIsUnique()
        {
            String value, objectName, propertyName;
            Boolean isUnique;
            objectName = "Application";
            propertyName = "Name";
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                // Check unique value
                value = "Test";
                isUnique = WebServiceProxy.UserService.CheckStringIsUnique(GetClientInformation(), value, objectName, propertyName);
                Assert.IsTrue(isUnique);

                // Check not unique value
                value = "Artportalen";
                isUnique = WebServiceProxy.UserService.CheckStringIsUnique(GetClientInformation(), value, objectName, propertyName);
                Assert.IsFalse(isUnique);
            }
        }

        [TestMethod]
        public void ClearCache()
        {
            WebServiceProxy.UserService.ClearCache(GetClientInformation());
        }

        [TestMethod]
        public void CommitTransaction()
        {
            WebServiceProxy.UserService.StartTransaction(GetClientInformation(), 1);
            WebServiceProxy.UserService.CommitTransaction(GetClientInformation());
        }

        [TestMethod]
        public void CreateApplication()
        {
            String applicationIdentity, description, name;
            WebApplication newApplication, createdApplication;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService, 60))
            {
                name = "ApplicationName";
                applicationIdentity = "ApplicationIdentity";
                description = @"ApplicationDescription";
                newApplication = new WebApplication();
                newApplication.Name = name;
                newApplication.Identifier = applicationIdentity;
                newApplication.Description = description;
                newApplication.IsAdministrationRoleIdSpecified = false;
                createdApplication = WebServiceProxy.UserService.CreateApplication(GetClientInformation(), newApplication);
                Assert.IsNotNull(createdApplication);
                Assert.AreEqual(newApplication.Description, createdApplication.Description);
                Assert.AreEqual(newApplication.Name, createdApplication.Name);
                Assert.AreEqual(newApplication.Identifier, createdApplication.Identifier);
            }
        }

        [TestMethod]
        public void CreateApplicationAction()
        {
            String actionIdentity, description, name;
            Int32 applicationId;
            WebApplicationAction newApplicationAction, createdApplicationAction;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                name = @"TestApplicationAction";
                actionIdentity = @"TestActionIdentity";
                applicationId = 3;
                description = @"ApplicationActionDescription";
                newApplicationAction = new WebApplicationAction();
                newApplicationAction.Identifier = actionIdentity;
                newApplicationAction.ApplicationId = applicationId;
                newApplicationAction.Description = description;
                newApplicationAction.Name = name;
                newApplicationAction.IsAdministrationRoleIdSpecified = false;
                createdApplicationAction = WebServiceProxy.UserService.CreateApplicationAction(GetClientInformation(), newApplicationAction);
                Assert.IsNotNull(createdApplicationAction);
                Assert.AreEqual(newApplicationAction.Description, createdApplicationAction.Description);
                Assert.AreEqual(newApplicationAction.Identifier, createdApplicationAction.Identifier);
                Assert.AreEqual(newApplicationAction.ApplicationId, createdApplicationAction.ApplicationId);
                Assert.AreEqual(newApplicationAction.Name, createdApplicationAction.Name);
                Assert.IsFalse(createdApplicationAction.IsAdministrationRoleIdSpecified);
            }
        }

        [TestMethod]
        public void CreateApplicationVersion()
        {
            String version, description;
            Int32 applicationId;
            WebApplicationVersion newApplicationVersion, createdApplicationVersion;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                version = "3.0";
                applicationId = 3;
                description = @"ApplicationVersionDescription";
                newApplicationVersion = new WebApplicationVersion();
                newApplicationVersion.Version = version;
                newApplicationVersion.ApplicationId = applicationId;
                newApplicationVersion.Description = description;
                newApplicationVersion.IsValid = true;
                newApplicationVersion.IsRecommended = true;
                createdApplicationVersion = WebServiceProxy.UserService.CreateApplicationVersion(GetClientInformation(), newApplicationVersion);
                Assert.IsNotNull(createdApplicationVersion);
                Assert.AreEqual(newApplicationVersion.Description, createdApplicationVersion.Description);
                Assert.AreEqual(newApplicationVersion.Version, createdApplicationVersion.Version);
                Assert.AreEqual(newApplicationVersion.ApplicationId, createdApplicationVersion.ApplicationId);
                Assert.IsTrue(createdApplicationVersion.IsValid);
                Assert.IsTrue(createdApplicationVersion.IsRecommended);
            }
        }

        [TestMethod]
        public void CreateAuthority()
        {
            String authorityIdentity, description;
            Int32 roleId, applicationId;
            WebAuthority newAuthority, createdAuthority;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                // set var values
                authorityIdentity = "AuthorityName";
                description = "Authority description";
                roleId = Settings.Default.TestRoleId;
                applicationId = Settings.Default.TestApplicationId;
                // create object instance
                newAuthority = new WebAuthority();

                // assign var to object
                newAuthority.Name = "AuthorityName";
                newAuthority.Identifier = authorityIdentity;
                newAuthority.Description = description;
                newAuthority.RoleId = roleId;
                newAuthority.ApplicationId = applicationId;
                newAuthority.ReadPermission = true;
                newAuthority.CreatePermission = true;
                newAuthority.UpdatePermission = true;
                newAuthority.DeletePermission = true;
                newAuthority.AuthorityType = AuthorityType.Application;

                createdAuthority = WebServiceProxy.UserService.CreateAuthority(GetClientInformation(), newAuthority);
                Assert.IsNotNull(createdAuthority);
                Assert.AreEqual(newAuthority.Description, createdAuthority.Description);
                Assert.AreEqual(newAuthority.Identifier, createdAuthority.Identifier);
                Assert.AreEqual(newAuthority.RoleId, createdAuthority.RoleId);
                Assert.AreEqual(newAuthority.ApplicationId, createdAuthority.ApplicationId);
                Assert.AreEqual(newAuthority.ReadPermission, createdAuthority.ReadPermission);
                Assert.AreEqual(newAuthority.AuthorityType.ToString(), createdAuthority.AuthorityType.ToString());
                Assert.AreEqual(newAuthority.AuthorityDataType, null);
            }
        }

        [TestMethod]
        public void CreateAuthorityUsingAuthorityDataTypes()
        {
            String authorityIdentity, description;
            Int32 roleId;
            WebAuthority newAuthority, createdAuthority;
            WebAuthorityDataType newAuthorityDataType;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                // set var values
                authorityIdentity = "AuthorityUsingAuthorityDataTypeName";
                description = "Authority using data type description";
                roleId = Settings.Default.TestRoleId;
                // Don't set application id
                //applicationId = Settings.Default.TestApplicationId;
                // create object instance
                newAuthority = new WebAuthority();
                // Create authority data type
                newAuthorityDataType = new WebAuthorityDataType();
                newAuthorityDataType.Id = 1;
                newAuthorityDataType.Identifier = "Test";
                newAuthority.ApplicationId = 0;
                newAuthority.AuthorityType = AuthorityType.DataType;

                // assign var to object
                newAuthority.Name = "AuthorityUsingAuthorityDataTypeName";
                newAuthority.Identifier = authorityIdentity;
                newAuthority.Description = description;
                newAuthority.RoleId = roleId;
               // newAuthority.ApplicationId = applicationId;
                newAuthority.ReadPermission = true;
                newAuthority.CreatePermission = true;
                newAuthority.UpdatePermission = true;
                newAuthority.DeletePermission = true;
                newAuthority.AuthorityDataType = newAuthorityDataType;
                newAuthority.AuthorityType = AuthorityType.DataType;

                createdAuthority = WebServiceProxy.UserService.CreateAuthority(GetClientInformation(), newAuthority);
                Assert.IsNotNull(createdAuthority);
                Assert.AreEqual(newAuthority.Description, createdAuthority.Description);
                Assert.AreEqual(newAuthority.Identifier, createdAuthority.Identifier);
                Assert.AreEqual(newAuthority.RoleId, createdAuthority.RoleId);
                Assert.AreEqual(newAuthority.ApplicationId, createdAuthority.ApplicationId);
                Assert.AreEqual(newAuthority.ReadPermission, createdAuthority.ReadPermission);
                Assert.AreEqual(newAuthority.AuthorityType.ToString(), createdAuthority.AuthorityType.ToString());
                Assert.AreEqual(newAuthority.AuthorityDataType.Id, createdAuthority.AuthorityDataType.Id);
                Assert.AreEqual(newAuthority.AuthorityDataType.Identifier, createdAuthority.AuthorityDataType.Identifier);
            }
        }

        [TestMethod]
        public void CreateOrganization()
        {
            String name, shortName;
            WebOrganization newOrganization, createdOrganization;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                name = "Organisations namn7";
                shortName = "Organisation shortname7";
                newOrganization = new WebOrganization();
                newOrganization.Name = name;
                newOrganization.ShortName = shortName;
                newOrganization.IsAdministrationRoleIdSpecified = false;
                newOrganization.Description = @"Test description ABC";
                newOrganization.Category = new WebOrganizationCategory();
                newOrganization.Category.Id = 2;
                createdOrganization = WebServiceProxy.UserService.CreateOrganization(GetClientInformation(), newOrganization);
                Assert.IsNotNull(createdOrganization);
                Assert.AreEqual(newOrganization.Description, createdOrganization.Description);
                Assert.AreEqual(newOrganization.Name, createdOrganization.Name);
                Assert.AreEqual(newOrganization.ShortName, createdOrganization.ShortName);
            }
        }

        [TestMethod]
        public void CreateOrganizationCategory()
        {
            String name, description;
            WebOrganizationCategory newOrganizationCategory, createdOrganizationCategory;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                name = "OrgCategoryName";
                description = "OrgCatergoy description";
                newOrganizationCategory = new WebOrganizationCategory();
                newOrganizationCategory.Name = name;
                newOrganizationCategory.Description = description;
                newOrganizationCategory.IsAdministrationRoleIdSpecified = false;
                createdOrganizationCategory = WebServiceProxy.UserService.CreateOrganizationCategory(GetClientInformation(), newOrganizationCategory);
                Assert.IsNotNull(createdOrganizationCategory);
                Assert.AreEqual(newOrganizationCategory.Description, createdOrganizationCategory.Description);
                Assert.AreEqual(newOrganizationCategory.Name, createdOrganizationCategory.Name);
            }
        }

        [TestMethod]
        public void CreateRole()
        {
            String name, shortName, description;
            WebRole newRole, createdRole;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                name = "MyRole";
                shortName = "MyShortName";
                description = "description";
                newRole = new WebRole();
                newRole.Name = name;
                newRole.ShortName = shortName;
                newRole.Description = description;
                newRole.IsOrganizationIdSpecified = true;
                newRole.OrganizationId = Settings.Default.TestOrganizationId;
                newRole.IsUserAdministrationRoleIdSpecified = true;
                newRole.UserAdministrationRoleId = 1;
                createdRole = WebServiceProxy.UserService.CreateRole(GetClientInformation(), newRole);
                Assert.IsNotNull(createdRole);
                Assert.AreEqual(newRole.Name, createdRole.Name);
                Assert.AreEqual(newRole.ShortName, createdRole.ShortName);
                Assert.AreEqual(newRole.OrganizationId, createdRole.OrganizationId);
                Assert.IsTrue(newRole.IsOrganizationIdSpecified);
            }
        }

        [TestMethod]
        public void CreatePerson()
        {
            String firstName, lastName;
            WebPerson newPerson, createdPerson;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                firstName = "Björn";
                lastName = "Karlsson";
                newPerson = new WebPerson();
                newPerson.EmailAddress = "My.Email@Address.se";
                newPerson.FirstName = firstName;
                newPerson.LastName = lastName;
                newPerson.IsBirthYearSpecified = false;
                newPerson.IsDeathYearSpecified = false;
                newPerson.IsUserIdSpecified = false;
                newPerson.Gender = new WebPersonGender();
                newPerson.Gender.Id = Settings.Default.PersonGenderManId;
                newPerson.Locale = new WebLocale();
                newPerson.Locale.Id = Settings.Default.SwedishLocaleId;
                createdPerson = WebServiceProxy.UserService.CreatePerson(GetClientInformation(), newPerson);
                Assert.IsNotNull(createdPerson);
                Assert.AreEqual(newPerson.EmailAddress, createdPerson.EmailAddress);
                Assert.AreEqual(newPerson.FirstName, createdPerson.FirstName);
                Assert.AreEqual(newPerson.LastName, createdPerson.LastName);
            }
        }

        [TestMethod]
        public void CreateUser()
        {
            Boolean showEmailAddress;
            DateTime now, validFromDate, validToDate;
            String emailAddress, userName;
            WebUser newUser, createdUser;

            // Test data that is not set in the client.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                newUser = GetNewUser();
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                Assert.IsNotNull(createdUser);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, createdUser.CreatedBy);
                Assert.AreEqual(createdUser.ModifiedBy, createdUser.CreatedBy);

                // Test created date.
                now = DateTime.Now;
                Assert.IsTrue((now - createdUser.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test GUID.
                Assert.IsTrue(createdUser.GUID.IsNotEmpty());

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, createdUser.Id);

                // Test is account activated.
                Assert.IsFalse(createdUser.IsAccountActivated);

                // Test is application id specified.
                Assert.IsFalse(createdUser.IsApplicationIdSpecified);

                // Test is person id specified.
                Assert.IsFalse(createdUser.IsPersonIdSpecified);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, createdUser.ModifiedBy);
                Assert.AreEqual(createdUser.ModifiedBy, createdUser.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - createdUser.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }

            // Test email address.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                emailAddress = @"fdskfd.sdff@lksfdf.ldfk";
                newUser = GetNewUser();
                newUser.EmailAddress = emailAddress;
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                Assert.IsNotNull(createdUser);
                Assert.AreEqual(newUser.EmailAddress, createdUser.EmailAddress);
            }

            // Test show email address.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                showEmailAddress = false;
                newUser = GetNewUser();
                newUser.ShowEmailAddress = showEmailAddress;
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                Assert.IsNotNull(createdUser);
                Assert.AreEqual(newUser.ShowEmailAddress, createdUser.ShowEmailAddress);
            }
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                showEmailAddress = true;
                newUser = GetNewUser();
                newUser.ShowEmailAddress = showEmailAddress;
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                Assert.IsNotNull(createdUser);
                Assert.AreEqual(newUser.ShowEmailAddress, createdUser.ShowEmailAddress);
            }

            // Test user name.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                userName = "hshshshsggsghg";
                newUser = GetNewUser();
                newUser.UserName = userName;
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                Assert.IsNotNull(createdUser);
                Assert.AreEqual(newUser.UserName, createdUser.UserName);
            }

            // Test user type.
            foreach (UserType userType in Enum.GetValues(typeof(UserType)))
            {
                using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
                {
                    newUser = GetNewUser();
                    newUser.Type = userType;
                    createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                    Assert.IsNotNull(createdUser);
                    Assert.AreEqual(newUser.Type, createdUser.Type);
                }
            }

            // Test valid from date.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                validFromDate = new DateTime(2000, 6, 5);
                newUser = GetNewUser();
                newUser.ValidFromDate = validFromDate;
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                Assert.IsNotNull(createdUser);
                Assert.AreEqual(newUser.ValidFromDate, createdUser.ValidFromDate);
            }

            // Test valid to date.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                validToDate = new DateTime(2000, 6, 5);
                newUser = GetNewUser();
                newUser.ValidToDate = validToDate;
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                Assert.IsNotNull(createdUser);
                Assert.AreEqual(newUser.ValidToDate, createdUser.ValidToDate);
            }
        }

        [TestMethod]
        public void DeleteApplication()
        {
            WebApplication application;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                application = GetOneApplication();
                WebServiceProxy.UserService.DeleteApplication(GetClientInformation(), application);
            }
        }

        [TestMethod]
        public void DeleteAuthority()
        {
            WebAuthority authority;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                authority = GetOneAuthority();
                WebServiceProxy.UserService.DeleteAuthority(GetClientInformation(), authority);
            }
        }

        [TestMethod]
        public void Ping()
        {
            Boolean ping;

            ping = WebServiceProxy.UserService.Ping();
            Assert.IsTrue(ping);
        }

        [TestMethod]
        public void RemoveAuthorityDataTypeFromApplication()
        {
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                Int32 applicationId = 1;
                Int32 authorityDataTypeId = 2;
                // Must first add datatype to application and then remove
                WebServiceProxy.UserService.AddAuthorityDataTypeToApplication(GetClientInformation(), authorityDataTypeId, applicationId);
                WebServiceProxy.UserService.RemoveAuthorityDataTypeFromApplication(GetClientInformation(), authorityDataTypeId, applicationId);
            }
        }

        [TestMethod]
        public void DeleteOrganization()
        {
            WebOrganization organization;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                organization = GetOneOrganization();
                WebServiceProxy.UserService.DeleteOrganization(GetClientInformation(), organization);
            }
        }

        [TestMethod]
        public void DeletePerson()
        {
            WebPerson person;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                person = GetOnePerson();
                WebServiceProxy.UserService.DeletePerson(GetClientInformation(), person);
            }
        }

        [TestMethod]
        public void DeleteTrace()
        {
            // Create some trace information.
            WebServiceProxy.UserService.StartTrace(GetClientInformation(), Settings.Default.TestUserName);
            WebServiceProxy.UserService.GetAddressTypes(GetClientInformation());
            WebServiceProxy.UserService.StopTrace(GetClientInformation());

            // Delete trace information.
            WebServiceProxy.UserService.DeleteTrace(GetClientInformation());
        }

        [TestMethod]
        public void DeleteUser()
        {
            WebUser newUser, createdUser;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                newUser = GetNewUser();
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
                WebServiceProxy.UserService.DeleteUser(GetClientInformation(), createdUser);
            }
        }

        public static WebApplication GetApplication(WebClientInformation clientInformation,
                                                    String applicationIdentifier)
        {
            List<WebApplication> applications;

            applications = WebServiceProxy.UserService.GetApplications(clientInformation);
            foreach (WebApplication application in applications)
            {
                if (applicationIdentifier == application.Identifier)
                {
                    return application;
                }
            }
            throw new Exception("Application with identifier " + applicationIdentifier + " was not found");
        }

        [TestMethod]
        public void GetApplicationsInSoa()
        {
            List<WebApplication> applications;

            applications = WebServiceProxy.UserService.GetApplicationsInSoa(GetClientInformation());
            Assert.IsTrue(applications.IsNotEmpty());
            Assert.AreEqual(11, applications.Count);
            foreach (WebApplication application in applications)
            {
                Assert.IsTrue(((application.Identifier == ApplicationIdentifier.AnalysisService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.ArtDatabankenService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.GeoReferenceService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.PictureService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.ReferenceService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SpeciesObservationHarvestService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SwedishSpeciesObservationService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SwedishSpeciesObservationSOAPService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.TaxonService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.TaxonAttributeService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.UserService.ToString())));
            }
        }

        [TestMethod]
        public void GetApplicationUsers()
        {
            List<WebUser> webUsers;
            webUsers = WebServiceProxy.UserService.GetApplicationUsers(GetClientInformation());
            Assert.IsNotNull(webUsers);
            Assert.IsTrue(webUsers.Count >= 1);
        }


        [TestMethod]
        public void GetAddressTypes()
        {
            List<WebAddressType> addressTypes;

            addressTypes = WebServiceProxy.UserService.GetAddressTypes(GetClientInformation());
            Assert.IsTrue(addressTypes.IsNotEmpty());
        }

        protected WebClientInformation GetClientInformation()
        {
            return _clientInformation;
        }

        [TestMethod]
        public void GetCountries()
        {
            List<WebCountry> countries;

            countries = WebServiceProxy.UserService.GetCountries(GetClientInformation());
            Assert.IsTrue(countries.IsNotEmpty());
        }

        [TestMethod]
        public void GetLocales()
        {
            List<WebLocale> locales;

            locales = WebServiceProxy.UserService.GetLocales(GetClientInformation());
            Assert.IsTrue(locales.IsNotEmpty());
        }

        [TestMethod]
        public void GetLockedUserInformation()
        {
            List<WebLockedUserInformation> lockedUsers;
            String userName;
            WebStringSearchCriteria userNameSearchString;

            // Search with no specific user.
            WebServiceProxy.UserService.GetLockedUserInformation(GetClientInformation(), null);

            // Search with specific user that is not locked.
            userNameSearchString = new WebStringSearchCriteria();
            userNameSearchString.SearchString = "No user name";
            lockedUsers = WebServiceProxy.UserService.GetLockedUserInformation(GetClientInformation(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());


            userName = "qwertyProxy" + DateTime.Now.ToString();
            WebServiceProxy.UserService.Login(userName,
                                              "hej hopp i lingon skogen",
                                              Settings.Default.DyntaxaApplicationIdentifier,
                                              false);
            WebServiceProxy.UserService.Login(userName,
                                              "hej hopp i lingon skogen",
                                              Settings.Default.DyntaxaApplicationIdentifier,
                                              false);

            userNameSearchString = new WebStringSearchCriteria();
            userNameSearchString.SearchString = userName;
            lockedUsers = WebServiceProxy.UserService.GetLockedUserInformation(GetClientInformation(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());

            // Search with specific user that is locked.
            WebServiceProxy.UserService.Login(userName,
                                              "hej hopp i lingon skogen",
                                              Settings.Default.DyntaxaApplicationIdentifier,
                                              false);
            WebServiceProxy.UserService.Login(userName,
                                              "hej hopp i lingon skogen",
                                              Settings.Default.DyntaxaApplicationIdentifier,
                                              false);
            WebServiceProxy.UserService.Login(userName,
                                              "hej hopp i lingon skogen",
                                              Settings.Default.DyntaxaApplicationIdentifier,
                                              false);
            lockedUsers = WebServiceProxy.UserService.GetLockedUserInformation(GetClientInformation(), null);
            Assert.IsTrue(lockedUsers.IsNotEmpty());
            userNameSearchString = new WebStringSearchCriteria();
            userNameSearchString.SearchString = "No user name";
            lockedUsers = WebServiceProxy.UserService.GetLockedUserInformation(GetClientInformation(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userNameSearchString = new WebStringSearchCriteria();
            userNameSearchString.SearchString = userName;
            lockedUsers = WebServiceProxy.UserService.GetLockedUserInformation(GetClientInformation(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsNotEmpty());
            Assert.AreEqual(1, lockedUsers.Count);
            Assert.AreEqual(5, lockedUsers[0].LoginAttemptCount);
            Assert.AreEqual(userName, lockedUsers[0].UserName);
        }

        [TestMethod]
        public void GetLog()
        {
            List<WebLogRow> logRows;

            logRows = WebServiceProxy.UserService.GetLog(GetClientInformation(), LogType.None, null, 100);
            Assert.IsTrue(logRows.IsNotEmpty());
        }

        public WebUser GetNewUser()
        {
            WebUser newUser;

            newUser = new WebUser();
            newUser.EmailAddress = "MyEmail@Address";
            newUser.UserName = Settings.Default.TestUserName + 42; ;
            newUser.Type = UserType.Person;
            newUser.ValidFromDate = DateTime.Now;
            newUser.ValidToDate = newUser.ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
            return newUser;
        }

        public WebApplication GetOneApplication()
        {
            String name, applicationIdentity;
            WebApplication newApplication;
            // It is assumed that this method is called
            // inside a transaction.
            name = "Applikation namn";
            applicationIdentity = "Applikation Identity";
            newApplication = new WebApplication();
            newApplication.Name = name;
            newApplication.Identifier = applicationIdentity;
            newApplication.IsAdministrationRoleIdSpecified = false;
            newApplication.Description = @"Test description applikation";
            return WebServiceProxy.UserService.CreateApplication(GetClientInformation(), newApplication);
        }

        /// <summary>
        /// It is assumed that this method is called
        /// inside a transaction.
        /// </summary>
        public WebApplicationAction GetOneApplicationAction()
        {
            WebApplication application;
            List<WebApplicationAction> applicationActions;

            application = GetApplication(GetClientInformation(), Settings.Default.ArtportalenApplicationIdentifier);
            applicationActions = WebServiceProxy.UserService.GetApplicationActions(GetClientInformation(),
                                                                                   application.Id);
            return applicationActions[0];
        }

        public WebApplicationVersion GetOneApplicationVersion()
        {
            String version;
            Int32 applicationId;
            WebApplicationVersion newApplicationVersion;
            // It is assumed that this method is called
            // inside a transaction.
            version = "3.0";
            applicationId = 3;
            newApplicationVersion = new WebApplicationVersion();
            newApplicationVersion.Version = version;
            newApplicationVersion.ApplicationId = applicationId;
            newApplicationVersion.IsValid = true;
            newApplicationVersion.IsRecommended = true;
            newApplicationVersion.Description = @"Test description application version";
            return WebServiceProxy.UserService.CreateApplicationVersion(GetClientInformation(), newApplicationVersion);
        }

        public WebAuthority GetOneAuthority()
        {
            String authorityIdentity, description;
            Int32 roleId, applicationId;

            WebAuthority newAuthority;
            // It is assumed that this method is called
            // inside a transaction.
            authorityIdentity = "AuthorityName";
            description = "Authority description";
            roleId = Settings.Default.TestRoleId;
            applicationId = Settings.Default.TestApplicationId;
            newAuthority = new WebAuthority();
            newAuthority.Name = "AuthorityName";
            newAuthority.RoleId = roleId;
            newAuthority.ApplicationId = applicationId;
            newAuthority.Identifier = authorityIdentity;
            newAuthority.Description = description;
            newAuthority.ReadPermission = true;
            newAuthority.CreatePermission = true;
            newAuthority.UpdatePermission = true;
            newAuthority.DeletePermission = true;
            return WebServiceProxy.UserService.CreateAuthority(GetClientInformation(), newAuthority);
        }

        /// <summary>
        /// Reutrns a list of web authority data types for Settings.Default.TestApplicationId.
        /// </summary>
        /// <returns></returns>
        public List<WebAuthorityDataType> GetAuthorityDataTypesForOneApplication()
        {
            List<WebAuthorityDataType> authorityDataTypeList = new List<WebAuthorityDataType>();
            WebAuthorityDataType authorityDataType1 = new WebAuthorityDataType();
            authorityDataType1.Id = 1;
            authorityDataType1.Identifier = "Test";
            authorityDataTypeList.Add(authorityDataType1);
            WebAuthorityDataType authorityDataType2 = new WebAuthorityDataType();
            authorityDataType2.Id = 2;
            authorityDataType2.Identifier = "SpeciesObservation";
            authorityDataTypeList.Add(authorityDataType2);
            return authorityDataTypeList;
        }

        public WebOrganization GetOneOrganization()
        {
            String name, shortName;
            WebOrganization newOrganization;
            // It is assumed that this method is called
            // inside a transaction.
            name = "Organisations namn9";
            shortName = "Organisation shortname9";
            newOrganization = new WebOrganization();
            newOrganization.Name = name;
            newOrganization.ShortName = shortName;
            newOrganization.IsAdministrationRoleIdSpecified = false;
            newOrganization.Description = @"Test description ABC";
            newOrganization.Category = new WebOrganizationCategory();
            newOrganization.Category.Id = 2;
            return WebServiceProxy.UserService.CreateOrganization(GetClientInformation(), newOrganization);
        }

        public WebOrganizationCategory GetOneOrganizationCategory()
        {
            String name, description;
            WebOrganizationCategory newOrganizationCategory;
            // It is assumed that this method is called
            // inside a transaction.
            name = "OrganisationCategoryName";
            description = @"Test description ABC";
            newOrganizationCategory = new WebOrganizationCategory();
            newOrganizationCategory.Name = name;
            newOrganizationCategory.IsAdministrationRoleIdSpecified = false;
            newOrganizationCategory.Description = description;
            return WebServiceProxy.UserService.CreateOrganizationCategory(GetClientInformation(), newOrganizationCategory);
        }

        public WebPerson GetOnePerson()
        {
            String firstName, lastName;
            WebPerson newPerson;

            // It is assumed that this method is called
            // inside a transaction.
            firstName = "Björn";
            lastName = "Karlsson";
            newPerson = new WebPerson();
            newPerson.EmailAddress = Settings.Default.TestEmailAddress;
            newPerson.FirstName = firstName;
            newPerson.LastName = lastName;
            newPerson.IsBirthYearSpecified = false;
            newPerson.IsDeathYearSpecified = false;
            newPerson.IsUserIdSpecified = false;
            newPerson.Gender = new WebPersonGender();
            newPerson.Gender.Id = Settings.Default.PersonGenderManId;
            newPerson.Locale = new WebLocale();
            newPerson.Locale.Id = Settings.Default.SwedishLocaleId;
            return WebServiceProxy.UserService.CreatePerson(GetClientInformation(), newPerson);
        }

        public WebRole GetOneRole()
        {
            String roleName, shortName;
            WebRole newRole;
            // It is assumed that this method is called
            // inside a transaction.
            roleName = "TheSuperRoleName2";
            shortName = "SuperShortName2";
            newRole = new WebRole();
            newRole.Name = roleName;
            newRole.ShortName = shortName;
            newRole.IsAdministrationRoleIdSpecified = false;
            newRole.Description = @"Test description role";
            newRole.IsOrganizationIdSpecified = true;
            newRole.OrganizationId = Settings.Default.TestOrganizationId;
            return WebServiceProxy.UserService.CreateRole(GetClientInformation(), newRole);
        }

        public WebUser GetOneUser(String emailAddress = @"My.Email@Adress.se")
        {
            String userName;
            WebUser newUser, createdUser;

            userName = Settings.Default.TestUserName + 42;
            newUser = new WebUser();
            newUser.EmailAddress = emailAddress;
            newUser.UserName = userName;
            newUser.Type = UserType.Person;
            newUser.ValidFromDate = DateTime.Now;
            newUser.ValidToDate = newUser.ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
            createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, Settings.Default.TestPassword);
            return createdUser;
        }

        [TestMethod]
        public void GetApplication()
        {
            WebApplication application1, application2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                application1 = GetOneApplication();
                application2 = WebServiceProxy.UserService.GetApplication(GetClientInformation(), application1.Id);
                Assert.IsNotNull(application2);
                Assert.AreEqual(application1.Description, application2.Description);
                Assert.AreEqual(application1.Name, application2.Name);
                Assert.AreEqual(application1.Identifier, application2.Identifier);
            }
        }

        [TestMethod]
        public void GetApplicationAction()
        {
            WebApplicationAction applicationAction1, applicationAction2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                applicationAction1 = GetOneApplicationAction();
                applicationAction2 = WebServiceProxy.UserService.GetApplicationAction(GetClientInformation(), applicationAction1.Id);
                Assert.IsNotNull(applicationAction2);
                Assert.AreEqual(applicationAction1.Description, applicationAction2.Description);
                Assert.AreEqual(applicationAction1.Name, applicationAction2.Name);
                Assert.AreEqual(applicationAction1.ApplicationId, applicationAction2.ApplicationId);
            }
        }

        [TestMethod]
        public void GetApplicationActions()
        {
            List<WebApplicationAction> applicationActionList;
            Int32 applicationId;
            applicationId = Settings.Default.TestApplicationId;
            applicationActionList = WebServiceProxy.UserService.GetApplicationActions(GetClientInformation(), applicationId);
            Assert.IsTrue(applicationActionList.IsNotEmpty());
        }

        [TestMethod]
        public void GetApplicationActionsByGUIDs()
        {
            List<WebApplicationAction> applicationActionList;
            List<String> applicationActionGUIDList = new List<String>();
            applicationActionGUIDList.Add("3");
            applicationActionGUIDList.Add("4");
            applicationActionList = WebServiceProxy.UserService.GetApplicationActionsByGUIDs(GetClientInformation(), applicationActionGUIDList);
            Assert.IsTrue(applicationActionList.IsNotEmpty());
        }

        [TestMethod]
        public void GetApplicationActionsByIds()
        {
            List<WebApplicationAction> applicationActionList;
            List<Int32> applicationActionIdList = new List<Int32>();
            applicationActionIdList.Add(3);
            applicationActionIdList.Add(4);
            applicationActionList = WebServiceProxy.UserService.GetApplicationActionsByIds(GetClientInformation(), applicationActionIdList);
            Assert.IsTrue(applicationActionList.IsNotEmpty());
        }

        [TestMethod]
        public void GetApplicationVersion()
        {
            WebApplicationVersion applicationVersion1, applicationVersion2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                applicationVersion1 = GetOneApplicationVersion();
                applicationVersion2 = WebServiceProxy.UserService.GetApplicationVersion(GetClientInformation(), applicationVersion1.Id);
                Assert.IsNotNull(applicationVersion2);
                Assert.AreEqual(applicationVersion1.Description, applicationVersion2.Description);
                Assert.AreEqual(applicationVersion1.Version, applicationVersion2.Version);
                Assert.AreEqual(applicationVersion1.ApplicationId, applicationVersion2.ApplicationId);
            }
        }

        [TestMethod]
        public void GetApplicationVersionList()
        {
            List<WebApplicationVersion> applicationVersionList;
            Int32 applicationId;
            applicationId = Settings.Default.TestApplicationId;
            applicationVersionList = WebServiceProxy.UserService.GetApplicationVersions(GetClientInformation(), applicationId);
            Assert.IsTrue(applicationVersionList.IsNotEmpty());
        }

        [TestMethod]
        public void GetAuthoritiesBySearchCriteria()
        {
            List<WebAuthority> authorities;
            String authorityIdentifier, applicationIdentifier, authorityDataTypeIdentifier, authorityName;
            WebAuthoritySearchCriteria searchCriteria;

            // Test all serach criterias if exist in DB or if not.
            // Test Authority Identifier
            authorityIdentifier = "U%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityIdentifier = "NotExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Application Identifier
            applicationIdentifier = "UserService%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            applicationIdentifier = "NoServiceExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test AuthorityDataType Identifier
            authorityDataTypeIdentifier = "Species%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityDataTypeIdentifier = "NoObsExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Authority Name.
            authorityName = "test%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityName = "noTestExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Finally test that if no critera is set(ie WebAuthoritySearchCriteria is created by no data is set to search for) will not generat a exception.
            searchCriteria = new WebAuthoritySearchCriteria();
            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetAuthoritiesBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebAuthority> authorities;

            authorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(), null);
            Assert.IsTrue(authorities.IsEmpty());
        }

        [TestMethod]
        public void GetAuthority()
        {
            WebAuthority authority1, authority2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                authority1 = GetOneAuthority();
                authority2 = WebServiceProxy.UserService.GetAuthority(GetClientInformation(), authority1.Id);
                Assert.IsNotNull(authority2);
                Assert.AreEqual(authority1.Description, authority2.Description);
                Assert.AreEqual(authority1.Identifier, authority2.Identifier);
                Assert.AreEqual(authority1.ReadPermission, authority2.ReadPermission);
            }
        }

        [TestMethod]
        public void GetAuthorityDataTypes()
        {
            List<WebAuthorityDataType> authorityDataTypes;

            authorityDataTypes = WebServiceProxy.UserService.GetAuthorityDataTypes(GetClientInformation());
            Assert.IsTrue(authorityDataTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetAuthorityDataTypesByApplicationId()
        {
            List<WebAuthorityDataType> authorityDataTypes;
            List<WebAuthorityDataType> authorityDataTypesTest;

            authorityDataTypes = WebServiceProxy.UserService.GetAuthorityDataTypesByApplicationId(GetClientInformation(), Settings.Default.TestApplicationId);
            authorityDataTypesTest = GetAuthorityDataTypesForOneApplication();
            Assert.IsTrue(authorityDataTypes.IsNotEmpty());
            int noOfEntries = 0;
            foreach (WebAuthorityDataType authorityDataTypeTest in authorityDataTypesTest)
            {
                foreach (WebAuthorityDataType authorityDataType in authorityDataTypes)
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

        [TestMethod]
        public void GetOrganization()
        {
            WebOrganization organization1, organization2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                organization1 = GetOneOrganization();
                organization2 = WebServiceProxy.UserService.GetOrganization(GetClientInformation(), organization1.Id);
                Assert.IsNotNull(organization2);
                Assert.AreEqual(organization1.Description, organization2.Description);
                Assert.AreEqual(organization1.Name, organization2.Name);
                Assert.AreEqual(organization1.ShortName, organization2.ShortName);
            }
        }

        [TestMethod]
        public void GetOrganizationCategory()
        {
            WebOrganizationCategory organizationCategory1, organizationCategory2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                organizationCategory1 = GetOneOrganizationCategory();
                organizationCategory2 = WebServiceProxy.UserService.GetOrganizationCategory(GetClientInformation(), organizationCategory1.Id);
                Assert.IsNotNull(organizationCategory2);
                Assert.AreEqual(organizationCategory1.Description, organizationCategory2.Description);
                Assert.AreEqual(organizationCategory1.Name, organizationCategory2.Name);
                Assert.AreEqual(organizationCategory1.IsAdministrationRoleIdSpecified, organizationCategory2.IsAdministrationRoleIdSpecified);
            }
        }

        [TestMethod]
        public void GetOrganizations()
        {
            List<WebOrganization> organizationList;
            organizationList = WebServiceProxy.UserService.GetOrganizations(GetClientInformation());
            Assert.IsTrue(organizationList.IsNotEmpty());
            Assert.IsTrue(organizationList.Count > 3);
        }

        [TestMethod]
        public void GetOrganizationsByOrganizationCategory()
        {
            List<WebOrganization> organizationList;
            organizationList = WebServiceProxy.UserService.GetOrganizationsByOrganizationCategory(GetClientInformation(), Settings.Default.TestOrganizationCategoryId);
            Assert.IsTrue(organizationList.IsNotEmpty());
            Assert.IsTrue(organizationList.Count > 3);
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
            organizations = WebServiceProxy.UserService.GetOrganizationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.Name = name;
            organizations = WebServiceProxy.UserService.GetOrganizationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test short name.
            name = "Art%";
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.ShortName = name;
            organizations = WebServiceProxy.UserService.GetOrganizationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.ShortName = name;
            organizations = WebServiceProxy.UserService.GetOrganizationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test OrganizationCategoryId
            organizationCategoryId = 3;
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.OrganizationCategoryId = organizationCategoryId;
            searchCriteria.IsOrganizationCategoryIdSpecified = true;
            organizations = WebServiceProxy.UserService.GetOrganizationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());

            organizationCategoryId = -1;
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.OrganizationCategoryId = organizationCategoryId;
            searchCriteria.IsOrganizationCategoryIdSpecified = true;
            organizations = WebServiceProxy.UserService.GetOrganizationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(organizations.IsEmpty());

            // Test hasSpiecesCollection
            hasSpiecesCollection = true;
            searchCriteria = new WebOrganizationSearchCriteria();
            searchCriteria.HasSpeciesCollection = hasSpiecesCollection;
            searchCriteria.IsHasSpeciesCollectionSpecified = true;
            organizations = WebServiceProxy.UserService.GetOrganizationsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(organizations.IsEmpty());
        }

        [TestMethod]
        public void GetPerson()
        {
            WebPerson person1, person2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                person1 = GetOnePerson();
                person2 = WebServiceProxy.UserService.GetPerson(GetClientInformation(), person1.Id);
                Assert.IsNotNull(person2);
                Assert.AreEqual(person1.EmailAddress, person2.EmailAddress);
                Assert.AreEqual(person1.FirstName, person2.FirstName);
                Assert.AreEqual(person1.LastName, person2.LastName);
            }
        }

        [TestMethod]
        public void GetPersonGenders()
        {
            List<WebPersonGender> personGenders;

            personGenders = WebServiceProxy.UserService.GetPersonGenders(GetClientInformation());
            Assert.IsTrue(personGenders.IsNotEmpty());
        }

        [TestMethod]
        public void GetPersonsBySearchCriteria()
        {
            List<WebPerson> persons;
            String name;
            WebPersonSearchCriteria searchCriteria;

            // Test first name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = WebServiceProxy.UserService.GetPersonsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = WebServiceProxy.UserService.GetPersonsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = WebServiceProxy.UserService.GetPersonsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = WebServiceProxy.UserService.GetPersonsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = WebServiceProxy.UserService.GetPersonsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = WebServiceProxy.UserService.GetPersonsBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetPersonsBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebPerson> persons;

            persons = WebServiceProxy.UserService.GetPersonsBySearchCriteria(GetClientInformation(), null);
            Assert.IsTrue(persons.IsEmpty());
        }

        [TestMethod]
        public void GetPhoneNumberTypes()
        {
            List<WebPhoneNumberType> phoneNumberTypes;

            phoneNumberTypes = WebServiceProxy.UserService.GetPhoneNumberTypes(GetClientInformation());
            Assert.IsTrue(phoneNumberTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetRole()
        {
            WebRole role1, role2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                role1 = GetOneRole();
                role2 = WebServiceProxy.UserService.GetRole(GetClientInformation(), role1.Id);
                Assert.IsNotNull(role2);
                Assert.AreEqual(role1.Description, role2.Description);
                Assert.AreEqual(role1.Name, role2.Name);
                Assert.AreEqual(role1.ShortName, role2.ShortName);
            }
        }

        [TestMethod]
        public void GetRolesByUser()
        {
            List<WebRole> roles;
            WebUser user;

            user = WebServiceProxy.UserService.GetUser(GetClientInformation());
            roles = WebServiceProxy.UserService.GetRolesByUser(GetClientInformation(), user.Id, Settings.Default.DyntaxaApplicationIdentifier);
            Assert.IsNotNull(roles.IsNotEmpty());
        }

        [TestMethod]
        public void GetRolesBySearchCriteria()
        {
            List<WebRole> roles;
            String name;
            Int32 organizationId;
            WebRoleSearchCriteria searchCriteria;

            // Test role name.
            name = "A%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Name = name;
            roles = WebServiceProxy.UserService.GetRolesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt2%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Name = name;
            roles = WebServiceProxy.UserService.GetRolesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test short name.
            name = "A%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = WebServiceProxy.UserService.GetRolesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt2%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = WebServiceProxy.UserService.GetRolesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test OrganizationId
            organizationId = Settings.Default.TestOrganizationId;
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            roles = WebServiceProxy.UserService.GetRolesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            organizationId = 1000;
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            roles = WebServiceProxy.UserService.GetRolesBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetRolesBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebRole> roles;

            roles = WebServiceProxy.UserService.GetRolesBySearchCriteria(GetClientInformation(), null);
            Assert.IsTrue(roles.IsEmpty());
        }

        [TestMethod]
        public void GetUserRoles()
        {
            List<WebRole> webRoles;

            webRoles = WebServiceProxy.UserService.GetUserRoles(GetClientInformation(), Settings.Default.TestUserId, Settings.Default.DyntaxaApplicationIdentifier);
            Assert.IsNotNull(webRoles);
            Assert.IsTrue(webRoles.Count > 1);
        }

        [TestMethod]
        public void GetSoaWebServiceAddress()
        {
            String webServiceAddress;

            webServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.ArtDatabankenService);
            Assert.IsTrue(webServiceAddress.IsNotEmpty());
            webServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.GeoReferenceService);
            Assert.IsTrue(webServiceAddress.IsNotEmpty());
            webServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.SwedishSpeciesObservationSOAPService);
            Assert.IsTrue(webServiceAddress.IsNotEmpty());
            webServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.TaxonService);
            Assert.IsTrue(webServiceAddress.IsNotEmpty());
            webServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.UserService);
            Assert.IsTrue(webServiceAddress.IsNotEmpty());
        }

        [TestMethod]
        public void GetStatus()
        {
            List<WebResourceStatus> status;

            status = WebServiceProxy.UserService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
            status = WebServiceProxy.UserService.GetStatus(GetClientInformation());
            Assert.IsTrue(status.IsNotEmpty());
        }

        [TestMethod]
        public void GetUser()
        {
            WebUser user1, user2;

            user1 = WebServiceProxy.UserService.GetUser(GetClientInformation());
            Assert.IsNotNull(user1);

            user2 = WebServiceProxy.UserService.GetUser(GetClientInformation(), user1.Id);
            Assert.IsNotNull(user2);
            Assert.AreEqual(user1.UserName, user2.UserName);
            Assert.AreEqual(user1.Id, user2.Id);

            user2 = WebServiceProxy.UserService.GetUser(GetClientInformation(), user1.UserName);
            Assert.IsNotNull(user2);
            Assert.AreEqual(user1.UserName, user2.UserName);
            Assert.AreEqual(user1.Id, user2.Id);
        }

        private WebUser GetExistingUser()
        {
            return WebServiceProxy.UserService.GetUser(GetClientInformation(), Settings.Default.TestUserId);
        }

        [TestMethod]
        public void GetUsersByRole()
        {
            List<WebUser> webUsers;
            Int32 roleId = Settings.Default.TestRoleId;
            webUsers = WebServiceProxy.UserService.GetUsersByRole(GetClientInformation(), roleId);
            Assert.IsNotNull(webUsers);
            Assert.IsTrue(webUsers.Count >= 1);
        }


        [TestMethod]
        public void GetNonActivatedUsersByRole()
        {
            List<WebUser> webUsers;
            Int32 roleId = Settings.Default.TestRoleId;
            webUsers = WebServiceProxy.UserService.GetNonActivatedUsersByRole(GetClientInformation(), roleId);
            Assert.IsNotNull(webUsers);
            Assert.IsTrue(webUsers.Count == 0);
        }

        [TestMethod]
        public void GetUsersBySearchCriteria()
        {
            List<WebUser> users;
            String name;
            WebUserSearchCriteria searchCriteria;

            // Test first name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FullName = name;
            users = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FullName = name;
            users = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.LastName = name;
            users = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.LastName = name;
            users = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test organizationId
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.UserType = UserType.Person;
            searchCriteria.OrganizationId = Settings.Default.TestOrganizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            users = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetUsersBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebUser> users;

            users = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(), null);
            Assert.IsTrue(users.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(EndpointNotFoundException))]
        public void HttpProxyAddressError1()
        {
            List<WebLocale> locales;
            UserServiceProxy userService;

            // This is not the real test.
            // The developer must step into the code and verify
            // that the http proxy address is set.
            userService = new UserServiceProxy();
            userService.HttpProxyAddress = new Uri(@"http://noproxyserver:1234");
            locales = userService.GetLocales(GetClientInformation());
            Assert.IsTrue(locales.IsNotEmpty());
        }

        [TestMethod]
        public void IsApplicationVersionValid()
        {
            String applicationIdentity, version;
            WebApplicationVersion applicationVersion;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                // Check valid version
                applicationIdentity = Settings.Default.UserAdminApplicationIdentifier;
                version = "1.0";
                applicationVersion = WebServiceProxy.UserService.IsApplicationVersionValid(GetClientInformation(), applicationIdentity, version);
                Assert.IsNotNull(applicationVersion);
                Assert.IsTrue(applicationVersion.IsValid);
                Assert.IsTrue(applicationVersion.IsRecommended);

                // Check not valid version 
                version = "Version 0.8";
                applicationVersion = WebServiceProxy.UserService.IsApplicationVersionValid(GetClientInformation(), applicationIdentity, version);
                Assert.IsNotNull(applicationVersion);
                Assert.IsFalse(applicationVersion.IsValid);
                Assert.IsFalse(applicationVersion.IsRecommended);
            }
        }

        [TestMethod]
        public void IsExistingPerson()
        {
            WebPerson person;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                // Get existing person.
                person = WebServiceProxy.UserService.GetPerson(GetClientInformation(), Settings.Default.TestPersonId);
                Assert.IsNotNull(person);
                Assert.IsTrue(WebServiceProxy.UserService.IsExistingPerson(GetClientInformation(), person.EmailAddress));
                String nonExistingEmail = "NonExisting@slu.se";
                Assert.IsFalse(WebServiceProxy.UserService.IsExistingPerson(GetClientInformation(), nonExistingEmail));
            }
        }

        [TestMethod]
        public void IsExistingUser()
        {
            WebUser user;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                Assert.IsFalse(WebServiceProxy.UserService.IsExistingUser(GetClientInformation(), Settings.Default.TestUserName + 42));
                user = GetOneUser(@"IsExistingUser@slu.se");
                Assert.IsTrue(WebServiceProxy.UserService.IsExistingUser(GetClientInformation(), Settings.Default.TestUserName + 42));
            }
        }

        [TestMethod]
        public void LoadSoaWebServiceAddresses()
        {
            WebServiceProxy.UserService.LoadSoaWebServiceAddresses(Settings.Default.TestUserName,
                                                                   Settings.Default.TestPassword,
                                                                   Settings.Default.DyntaxaApplicationIdentifier,
                                                                   false);
        }

        [TestMethod]
        public void Login()
        {
            Int32 loginAttempt;
            String emailAddress, password, userName;
            WebLoginResponse loginResponse;
            WebPasswordInformation passwordInformation;
            WebUser createdUser = null, newUser;

            loginResponse = WebServiceProxy.UserService.Login(Settings.Default.TestUserName,
                                                              Settings.Default.TestPassword,
                                                              Settings.Default.DyntaxaApplicationIdentifier,
                                                              false);
            Assert.IsNotNull(loginResponse);

            // Test login after user is locked out from web service
            // followed by ResetPassword.
            try
            {
                using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
                {
                    emailAddress = @"Login4@Adress.se";
                    password = "fsdlkjKJ994";
                    userName = "LoginTest2";
                    newUser = GetNewUser();
                    newUser.EmailAddress = emailAddress;
                    newUser.UserName = userName;
                    createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, password);
                    Assert.IsNotNull(createdUser);
                    transaction.Commit();
                }

                // Login is ok.
                loginResponse = WebServiceProxy.UserService.Login(userName,
                                                                  password,
                                                                  Settings.Default.UserAdminApplicationIdentifier,
                                                                  false);
                Assert.IsNotNull(loginResponse);

                // Fail to login a couple of times.
                for (loginAttempt = 0; loginAttempt < 5; loginAttempt++)
                {
                    loginResponse = WebServiceProxy.UserService.Login(userName,
                                                                      "No password",
                                                                      Settings.Default.UserAdminApplicationIdentifier,
                                                                      false);
                    Assert.IsNull(loginResponse);
                }

                // Reset password.
                using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
                {
                    passwordInformation = WebServiceProxy.UserService.ResetPassword(GetClientInformation(), emailAddress);
                    Assert.IsNotNull(passwordInformation);
                    transaction.Commit();
                }

                // Login should be ok now.
                loginResponse = WebServiceProxy.UserService.Login(userName,
                                                                  passwordInformation.Password,
                                                                  Settings.Default.UserAdminApplicationIdentifier,
                                                                  false);
                Assert.IsNotNull(loginResponse);
            }
            finally 
            {
                using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
                {
                    if (createdUser.IsNotNull())
                    {
                        WebServiceProxy.UserService.DeleteUser(GetClientInformation(), createdUser);
                    }

                    transaction.Commit();
                }
            }
        }

        [TestMethod]
        public void Logout()
        {
            WebClientInformation clientInformation;
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.UserService.Login(Settings.Default.TestUserName,
                                                   Settings.Default.TestPassword,
                                                   Settings.Default.DyntaxaApplicationIdentifier,
                                                   false);
            Assert.IsNotNull(loginResponse);
            clientInformation = new WebClientInformation();
            clientInformation.Token = loginResponse.Token;
            WebServiceProxy.UserService.Logout(clientInformation);
        }

        [TestMethod]
        public void RemoveUserFromRole()
        {
            WebUser webUser;
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                webUser = GetOneUser(@"RemoveUserFromRole@slu.se");
                Int32 userId = webUser.Id;
                Int32 roleId = Settings.Default.TestRoleId;
                // Add user to role
                WebServiceProxy.UserService.AddUserToRole(GetClientInformation(), roleId, userId);
                // Remove user from role
                WebServiceProxy.UserService.RemoveUserFromRole(GetClientInformation(), roleId, userId);
            }
        }

        [TestMethod]
        public void ResetPassword()
        {
            String emailAddress, password, userName;
            // WebLoginResponse loginResponse;
            WebPasswordInformation passwordInformation;
            WebUser createdUser, newUser;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                emailAddress = @"ResetPassword@Adress.se";
                password = "fsdlkjKJ994";
                userName = "ResetPasswordTest";
                newUser = GetNewUser();
                newUser.EmailAddress = emailAddress;
                newUser.UserName = userName;
                createdUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), newUser, password);
                passwordInformation = WebServiceProxy.UserService.ResetPassword(GetClientInformation(), emailAddress);
                Assert.IsNotNull(passwordInformation);
                Assert.AreEqual(emailAddress, passwordInformation.EmailAddress);
                Assert.AreNotEqual(password, passwordInformation.Password);
                Assert.AreEqual(userName, passwordInformation.UserName);
                // transaction.Commit();
            }

            // Login with the new password.
            // loginResponse = UserServiceManager.Login(passwordInformation.UserName,
            //                                             passwordInformation.Password,
            //                                            Settings.Default.TestApplicationIdentifier,
            //                                            false);
            //Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void ResetPasswordEmailAddressError()
        {
            WebPasswordInformation passwordInformation;

            passwordInformation = WebServiceProxy.UserService.ResetPassword(GetClientInformation(), Settings.Default.TestEmailAddress + 42);
            Assert.IsNull(passwordInformation);
        }

        [TestMethod]
        public void RollbackTransaction()
        {
            // Should be ok to rollback an unexisting transaction.
            WebServiceProxy.UserService.RollbackTransaction(GetClientInformation());

            // Normal rollback.
            WebServiceProxy.UserService.StartTransaction(GetClientInformation(), 1);
            WebServiceProxy.UserService.RollbackTransaction(GetClientInformation());
            Thread.Sleep(2000);

            // Should be ok to rollback twice.
            WebServiceProxy.UserService.StartTransaction(GetClientInformation(), 1);
            WebServiceProxy.UserService.RollbackTransaction(GetClientInformation());
            WebServiceProxy.UserService.RollbackTransaction(GetClientInformation());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void StartTransactionAlreadyStartedError()
        {
            WebServiceProxy.UserService.StartTransaction(GetClientInformation(), 1);
            WebServiceProxy.UserService.StartTransaction(GetClientInformation(), 1);
        }

        [TestMethod]
        public void StartTrace()
        {
            WebServiceProxy.UserService.StartTrace(GetClientInformation(), "kalle kula");
            WebServiceProxy.UserService.StopTrace(GetClientInformation());
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            try
            {
                WebServiceProxy.UserService.Logout(_clientInformation);
                _clientInformation = null;
            }
            catch
            {
                // Test is done.
                // We are not interested in problems that
                // occures due to test of error handling.
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestInitialize()
        {
            WebLoginResponse loginResponse;

            Configuration.InstallationType = InstallationType.ServerTest;
            // Configuration.InstallationType = InstallationType.Production;
            // WebServiceProxy.UserService.WebServiceAddress = @"lampetra2-2.artdata.slu.se/UserService/UserService.svc";
            // WebServiceProxy.UserService.WebServiceAddress = @"user.artdatabankensoa.se/UserService.svc";
            loginResponse = WebServiceProxy.UserService.Login(Settings.Default.TestUserName,
                                                              Settings.Default.TestPassword,
                                                              Settings.Default.UserAdminApplicationIdentifier,
                                                              false);
            _clientInformation = new WebClientInformation();
            _clientInformation.Locale = loginResponse.Locale;
            _clientInformation.Role = loginResponse.Roles[0];
            _clientInformation.Token = loginResponse.Token;
        }

        [TestMethod]
        public void UpdateApplication()
        {
            String name;
            WebApplication application1, application2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                application1 = GetOneApplication();
                name = @"MyApplicationName";
                application1.Name = name;
                application2 = WebServiceProxy.UserService.UpdateApplication(GetClientInformation(), application1);
                Assert.IsNotNull(application2);
                Assert.AreEqual(application1.Id, application2.Id);
                Assert.AreEqual(application1.Name, application2.Name);
            }
        }

        [TestMethod]
        public void UpdateApplicationAction()
        {
            String name, actionIdentity;
            WebApplicationAction applicationAction1, applicationAction2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                applicationAction1 = GetOneApplicationAction();
                name = "@TestApplicationAction";
                actionIdentity = "@TestApplicationActionIdentity";
                applicationAction1.Name = name;
                applicationAction1.Identifier = actionIdentity;
                applicationAction2 = WebServiceProxy.UserService.UpdateApplicationAction(GetClientInformation(), applicationAction1);
                Assert.IsNotNull(applicationAction2);
                Assert.AreEqual(applicationAction1.Id, applicationAction2.Id);
                Assert.AreEqual(applicationAction1.Name, applicationAction2.Name);
            }
        }

        [TestMethod]
        public void UpdateApplicationVersion()
        {
            String version;
            WebApplicationVersion applicationVersion1, applicationVersion2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                applicationVersion1 = GetOneApplicationVersion();
                version = "3.1";
                applicationVersion1.Version = version;
                applicationVersion2 = WebServiceProxy.UserService.UpdateApplicationVersion(GetClientInformation(), applicationVersion1);
                Assert.IsNotNull(applicationVersion2);
                Assert.AreEqual(applicationVersion1.Id, applicationVersion2.Id);
                Assert.AreEqual(applicationVersion1.Version, applicationVersion2.Version);
            }
        }

        [TestMethod]
        public void UpdateAuthority()
        {
            String authorityIdentifier;
            WebAuthority authority1, authority2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                authority1 = GetOneAuthority();
                authorityIdentifier = @"MyAuthorityIdentifier";
                authority1.Identifier = authorityIdentifier;
                authority2 = WebServiceProxy.UserService.UpdateAuthority(GetClientInformation(), authority1);
                Assert.IsNotNull(authority2);
                Assert.AreEqual(authority1.Id, authority2.Id);
                Assert.AreEqual(authority1.Identifier, authority2.Identifier);
            }
        }

        [TestMethod]
        public void UpdateOrganization()
        {
            String name;
            WebOrganization organization1, organization2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                organization1 = GetOneOrganization();
                name = @"MyOrganizationName11";
                organization1.Name = name;
                organization2 = WebServiceProxy.UserService.UpdateOrganization(GetClientInformation(), organization1);
                Assert.IsNotNull(organization2);
                Assert.AreEqual(organization1.Id, organization2.Id);
                Assert.AreEqual(organization1.Name, organization2.Name);
            }
        }

        [TestMethod]
        public void UpdateOrganizationCategory()
        {
            String name;
            WebOrganizationCategory organizationCategory1, organizationCategory2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                organizationCategory1 = GetOneOrganizationCategory();
                name = @"MyOrganizationCategoryName12";
                organizationCategory1.Name = name;
                organizationCategory2 = WebServiceProxy.UserService.UpdateOrganizationCategory(GetClientInformation(), organizationCategory1);
                Assert.IsNotNull(organizationCategory2);
                Assert.AreEqual(organizationCategory1.Id, organizationCategory2.Id);
                Assert.AreEqual(organizationCategory1.Name, organizationCategory2.Name);
            }
        }

        [TestMethod]
        public void UpdatePassword()
        {
            String newPassword, oldPassword;

            oldPassword = Settings.Default.TestPassword;
            newPassword = "NotUsedPassword0-";
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                Assert.IsTrue(WebServiceProxy.UserService.UpdatePassword(GetClientInformation(), oldPassword, newPassword));
            }
        }

        [TestMethod]
        public void UpdatePerson()
        {
            String emailAddress;
            WebPerson person1, person2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                person1 = GetOnePerson();
                emailAddress = @"My.Email@address.se";
                person1.EmailAddress = emailAddress;
                person2 = WebServiceProxy.UserService.UpdatePerson(GetClientInformation(), person1);
                Assert.IsNotNull(person2);
                Assert.AreEqual(person1.Id, person2.Id);
                Assert.AreEqual(person1.EmailAddress, person2.EmailAddress);
            }
        }

        [TestMethod]
        public void UpdateRole()
        {
            String roleName;
            WebRole role1, role2;

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                role1 = GetOneRole();
                roleName = @"MyRoleName13";
                role1.Name = roleName;
                role1.IsOrganizationIdSpecified = true;
                //role1.OrganizationId = Settings.Default.TestOrganizationId;
                role1.OrganizationId = 100;
                role2 = WebServiceProxy.UserService.UpdateRole(GetClientInformation(), role1);
                Assert.IsNotNull(role2);
                Assert.AreEqual(role1.Id, role2.Id);
                Assert.AreEqual(role1.Name, role2.Name);
                Assert.AreEqual(role1.OrganizationId, role2.OrganizationId);
            }
        }

        [TestMethod]
        public void UpdateUser()
        {
            Boolean showEmailAddress;
            DateTime validFromDate, validToDate;
            String emailAddress;
            WebUser user, updatedUser;

            // Test email address.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                emailAddress = @"fdskfd.sdff@lkf.ld";
                user = GetOneUser(@"UpdateUser1@slu.se");
                user.EmailAddress = emailAddress;
                updatedUser = WebServiceProxy.UserService.UpdateUser(GetClientInformation(), user);
                Assert.IsNotNull(updatedUser);
                Assert.AreEqual(user.EmailAddress, updatedUser.EmailAddress);
            }

            // Test show email address.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                showEmailAddress = false;
                user = GetOneUser(@"UpdateUser2@slu.se");
                user.ShowEmailAddress = showEmailAddress;
                updatedUser = WebServiceProxy.UserService.UpdateUser(GetClientInformation(), user);
                Assert.IsNotNull(updatedUser);
                Assert.AreEqual(user.ShowEmailAddress, updatedUser.ShowEmailAddress);
            }

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                showEmailAddress = true;
                user = GetOneUser(@"UpdateUser3@slu.se");
                user.ShowEmailAddress = showEmailAddress;
                updatedUser = WebServiceProxy.UserService.UpdateUser(GetClientInformation(), user);
                Assert.IsNotNull(updatedUser);
                Assert.AreEqual(user.ShowEmailAddress, updatedUser.ShowEmailAddress);
            }

            // Test valid from date.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                validFromDate = new DateTime(2010, 6, 5);
                user = GetOneUser(@"UpdateUser4@slu.se");
                user.ValidFromDate = validFromDate;
                updatedUser = WebServiceProxy.UserService.UpdateUser(GetClientInformation(), user);
                Assert.IsNotNull(updatedUser);
                Assert.AreEqual(user.ValidFromDate, updatedUser.ValidFromDate);
            }

            // Test valid to date.
            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                validToDate = new DateTime(2010, 6, 5);
                user = GetOneUser(@"UpdateUser5@slu.se");
                user.ValidToDate = validToDate;
                updatedUser = WebServiceProxy.UserService.UpdateUser(GetClientInformation(), user);
                Assert.IsNotNull(updatedUser);
                Assert.AreEqual(user.ValidToDate, updatedUser.ValidToDate);
            }
        }

        [TestMethod]
        public void UserAdminSetPassword()
        {
            WebUser user, newUser;
            String newPassword = "TEst1243qwe";

            using (IWebServiceTransaction transaction = new WebServiceTransaction(GetClientInformation(), WebServiceProxy.UserService))
            {
                user = GetNewUser();
                newUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(), user, Settings.Default.TestPassword);
                Assert.IsTrue(WebServiceProxy.UserService.UserAdminSetPassword(GetClientInformation(), newUser, newPassword));
            }
        }
    }
}
