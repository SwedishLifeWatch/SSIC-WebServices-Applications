using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class UserManagerTest : TestBase
    {
        public UserManagerTest()
            : base(true, 50)
        {
        }

        [TestMethod]
        public void ActivateRoleMembership()
        {
            Boolean value;

            UseTransaction = true;
            value = UserService.Data.UserManager.ActivateRoleMembership(GetContext(), 25);
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void ActivateUserAccount()
        {
            WebUser user;

            UseTransaction = true;

            // Get existing user.
            user = UserService.Data.UserManager.GetUserById(GetContext(), Settings.Default.TestUserId);
            const String Password = "testwrRTUT1234";
            const String ActivationKey = "klfdlskfjalsdsdfsdfsdfsdfsdfsdfsdfsdfsfdfsdfsdfsdfsdfsdfsdf";
            user.ActivationKey = ActivationKey;

            // Make UserName unique
            user.UserName = user.UserName + "xx";
            user.Type = UserType.Person;

            // Make emailaddress unique
            user.EmailAddress = "first.last@unique.com";
            WebUser newUser;
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, Password);
            Assert.IsNotNull(newUser);
            Assert.IsTrue(UserService.Data.UserManager.ActivateUserAccount(GetContext(), newUser.UserName, ActivationKey));
        }

        [TestMethod]
        public void ActivateUserAccountActivationKeyFailure()
        {
            WebUser user;
            const String ActivationKey = "Abc123efG";

            // Get existing user.
            UseTransaction = true;
            user = UserService.Data.UserManager.GetUserById(GetContext(), Settings.Default.TestUserId);
            Assert.IsNotNull(user);
            Assert.IsFalse(UserService.Data.UserManager.ActivateUserAccount(GetContext(), user.UserName, ActivationKey));
        }

        [TestMethod]
        public void AddUserToRole()
        {
            UseTransaction = true;
            Int32 roleId = Settings.Default.TestRoleId;
            Int32 userId = Settings.Default.TestUserId;
            UserService.Data.UserManager.AddUserToRole(GetContext(), roleId, userId);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void AddUserToRoleNonExistingUser()
        {
            UseTransaction = true;
            Int32 roleId = Settings.Default.TestRoleId;
            const int UserId = -1;
            UserService.Data.UserManager.AddUserToRole(GetContext(), roleId, UserId);
        }

        [TestMethod]
        public void CheckStringIsUnique()
        {
            String value, objectName, propertyName;

            UseTransaction = true;
            value = "Test";
            objectName = "Application";
            propertyName = "Name";
            Assert.IsTrue(UserService.Data.UserManager.CheckStringIsUnique(GetContext(), value, objectName, propertyName));

            UseTransaction = false;
            value = "Test";
            objectName = "Application";
            propertyName = "Name";
            Assert.IsTrue(UserService.Data.UserManager.CheckStringIsUnique(GetContext(), value, objectName, propertyName));

            UseTransaction = false;
            value = "Test'";
            objectName = "Application'";
            propertyName = "Name'";
            Assert.IsTrue(UserService.Data.UserManager.CheckStringIsUnique(GetContext(), value, objectName, propertyName));
        }

        [TestMethod]
        public void CreateAuthenticationKey()
        {
            const Int32 FixedLength = 30;

            UseTransaction = true;
            String key1 = UserService.Data.UserManager.CreateAuthenticationKey(FixedLength, FixedLength);
            String key2 = UserService.Data.UserManager.CreateAuthenticationKey(FixedLength, FixedLength);
            Assert.AreEqual(key1.Length, FixedLength);
            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        public void CreateAuthority()
        {
            WebApplicationVersion applicationVersion;
            WebAuthority authority;
            const String Name = "TestNameUnique";

            // Get existing authority.
            UseTransaction = true;
            authority = UserService.Data.UserManager.GetAuthority(GetContext(), Settings.Default.TestAuthorityId);
            authority.Name = Name;
            List<String> GUIDs = new List<String>();
            GUIDs.Add("1");
            authority.ActionGUIDs = GUIDs;
            authority.ProjectGUIDs = GUIDs;
            authority.RegionGUIDs = GUIDs;
            authority.TaxonGUIDs = GUIDs;
            authority.LocalityGUIDs = GUIDs;

            WebAuthority newAuthority;
            newAuthority = UserService.Data.UserManager.CreateAuthority(GetContext(), authority);
            Assert.IsNotNull(newAuthority);
            Assert.IsTrue(authority.Identifier.Length > 5);
            Assert.IsTrue(newAuthority.Id > Settings.Default.TestAuthorityId);
            Assert.AreEqual(newAuthority.Name, Name);
            Assert.IsTrue(newAuthority.CreatePermission);
            Assert.IsTrue(newAuthority.ReadPermission);
            Assert.IsTrue(newAuthority.UpdatePermission);
            Assert.IsTrue(newAuthority.DeletePermission);
            Assert.IsTrue(newAuthority.MaxProtectionLevel > 0);
            Assert.IsNotNull(newAuthority.Obligation);
            Assert.IsTrue(newAuthority.ActionGUIDs.Count > 0);
            Assert.IsTrue(newAuthority.ProjectGUIDs.Count > 0);
            Assert.IsTrue(newAuthority.RegionGUIDs.Count > 0);
            Assert.IsTrue(newAuthority.TaxonGUIDs.Count > 0);
            Assert.IsTrue(newAuthority.LocalityGUIDs.Count > 0);
            Assert.AreEqual(newAuthority.AuthorityType.ToString(), AuthorityType.Application.ToString());

            // Create application version
            applicationVersion = UserService.Data.ApplicationManager.GetApplicationVersion(GetContext(), 1);
            WebApplicationVersion newApplicationVersion;
            newApplicationVersion = UserService.Data.ApplicationManager.CreateApplicationVersion(GetContext(), applicationVersion);
            Assert.IsNotNull(newApplicationVersion);
            Assert.AreEqual(newApplicationVersion.Version, applicationVersion.Version);
            Assert.IsNotNull(newApplicationVersion.Description);
            Assert.AreEqual(newApplicationVersion.IsRecommended, applicationVersion.IsRecommended);
            Assert.AreEqual(newApplicationVersion.IsValid, applicationVersion.IsValid);
        }

        [TestMethod]
        public void CreateAuthorityUsingAuthorityDataTypes()
        {
            WebAuthority authority;
            const String Name = "TestNameUnique";

            // Get existing authority.
            UseTransaction = true;
            authority = UserService.Data.UserManager.GetAuthority(GetContext(), Settings.Default.TestAuthorityId);
            authority.Name = Name;
            List<String> GUIDs = new List<String>();
            GUIDs.Add("1");
            authority.ActionGUIDs = GUIDs;
            authority.ProjectGUIDs = GUIDs;
            authority.RegionGUIDs = GUIDs;
            authority.TaxonGUIDs = GUIDs;
            authority.LocalityGUIDs = GUIDs;
            authority.AuthorityDataType = new WebAuthorityDataType();
            authority.AuthorityDataType.Id = 1;
            authority.AuthorityDataType.Identifier = "Test";

            WebAuthority newAuthority;
            newAuthority = UserService.Data.UserManager.CreateAuthority(GetContext(), authority);
            Assert.IsNotNull(newAuthority);
            Assert.IsTrue(authority.Identifier.Length > 5);
            Assert.IsTrue(newAuthority.Id > Settings.Default.TestAuthorityId);
            Assert.AreEqual(newAuthority.Name, Name);
            Assert.IsTrue(newAuthority.CreatePermission);
            Assert.IsTrue(newAuthority.ReadPermission);
            Assert.IsTrue(newAuthority.UpdatePermission);
            Assert.IsTrue(newAuthority.DeletePermission);
            Assert.IsTrue(newAuthority.MaxProtectionLevel > 0);
            Assert.IsNotNull(newAuthority.Obligation);
            Assert.IsTrue(newAuthority.ActionGUIDs.Count > 0);
            Assert.IsTrue(newAuthority.ProjectGUIDs.Count > 0);
            Assert.IsTrue(newAuthority.RegionGUIDs.Count > 0);
            Assert.IsTrue(newAuthority.TaxonGUIDs.Count > 0);
            Assert.IsTrue(newAuthority.LocalityGUIDs.Count > 0);
            Assert.AreEqual(AuthorityType.DataType, newAuthority.AuthorityType);
        }

        [TestMethod]
        public void CreatePerson()
        {
            WebPerson newPerson, person;

            UseTransaction = true;
            person = GetNewPersonInformation(GetContext());
            person.EmailAddress = "CreatePerson.slu.se";
            newPerson = UserService.Data.UserManager.CreatePerson(GetContext(), person);
            Assert.IsNotNull(newPerson);
            Assert.AreEqual(person.FirstName, newPerson.FirstName);
            Assert.AreEqual(person.EmailAddress, newPerson.EmailAddress);
            Assert.AreEqual(person.Gender.Id, newPerson.Gender.Id);
            Assert.AreEqual(person.HasSpeciesCollection, newPerson.HasSpeciesCollection);
            Assert.AreEqual(person.URL, newPerson.URL);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePersonNotUniqueEmailAddressError1()
        {
            WebPerson newPerson, person;

            // Create person.
            UseTransaction = true;
            person = GetNewPersonInformation(GetContext());
            person.EmailAddress = "CreatePersonError1.slu.se";
            newPerson = UserService.Data.UserManager.CreatePerson(GetContext(), person);
            Assert.IsNotNull(newPerson);

            // Create another person with the same email address.
            person.FirstName = "Kalle2";
            person.LastName = "Kula2";
            newPerson = UserService.Data.UserManager.CreatePerson(GetContext(), person);
            Assert.IsNotNull(newPerson);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePersonNotUniqueEmailAddressError2()
        {
            WebPerson newPerson, person;
            WebUser newUser, user;

            // Create user.
            UseTransaction = true;
            user = new WebUser();
            user.EmailAddress = @"CreatePersonError2.slu.se";
            user.IsPersonIdSpecified = false;
            user.Type = UserType.Person;
            user.UserName = "CreatePersonError2";
            user.ValidFromDate = DateTime.Today;
            user.ValidToDate = DateTime.Today + new TimeSpan(36500, 0, 0, 0);
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, "NoPassword");
            Assert.IsNotNull(newUser);

            // Create person with the same email address as user.
            person = GetNewPersonInformation(GetContext());
            person.EmailAddress = newUser.EmailAddress;
            newPerson = UserService.Data.UserManager.CreatePerson(GetContext(), person);
            Assert.IsNotNull(newPerson);
        }

        [TestMethod]
        public void CreateRole()
        {
            WebRole role;
            String roleName, roleShortName;

            UseTransaction = true;
            roleName = "UniqueRoleName";
            roleShortName = "UniqueRoleShortName";

            // Get existing role.
            role = UserService.Data.UserManager.GetRole(GetContext(), Settings.Default.TestRoleId);
            role.Name = roleName;
            role.ShortName = roleShortName;
            role.Identifier = roleName;
            role.IsUserAdministrationRoleIdSpecified = false;
            role.IsOrganizationIdSpecified = true;
            role.OrganizationId = 1;
            WebRole newRole;
            newRole = UserService.Data.UserManager.CreateRole(GetContext(), role);
            Assert.IsNotNull(newRole);
            Assert.AreEqual(roleName, newRole.Name);
            Assert.AreEqual(roleName, newRole.Identifier);
            Assert.IsTrue(newRole.Id > Settings.Default.TestRoleId);
            Assert.IsFalse(newRole.IsUserAdministrationRoleIdSpecified);
            Assert.IsTrue(newRole.IsOrganizationIdSpecified);
            Assert.AreEqual(newRole.OrganizationId, 1);
            Assert.AreEqual(newRole.MessageTypeId, 1);
            Assert.AreEqual(newRole.IsActivationRequired, false);
        }

        [TestMethod]
        public void CreateUser()
        {
            const String password = "tesT1234";
            WebUser newUser, user;

            UseTransaction = true;
            user = GetNewUserInformation();
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, password);
            Assert.IsNotNull(newUser);
            Assert.AreEqual(user.UserName, newUser.UserName);
            Assert.AreEqual(user.EmailAddress, newUser.EmailAddress);
            Assert.IsFalse(newUser.IsAdministrationRoleIdSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUserWithShortPassword()
        {
            const String password = "test";
            WebUser newUser, user;

            UseTransaction = true;
            user = GetNewUserInformation();
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, password);
            Assert.IsNotNull(newUser);
            Assert.AreEqual(user.UserName, newUser.UserName);
            Assert.AreEqual(user.EmailAddress, newUser.EmailAddress);
            Assert.IsFalse(newUser.IsAdministrationRoleIdSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUserWithShortUserName()
        {
            const String password = "tesT1234";
            WebUser newUser, user;

            UseTransaction = true;
            user = GetNewUserInformation();
            user.UserName = "aa";
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, password);
            Assert.IsNotNull(newUser);
            Assert.AreEqual(user.UserName, newUser.UserName);
            Assert.AreEqual(user.EmailAddress, newUser.EmailAddress);
            Assert.IsFalse(newUser.IsAdministrationRoleIdSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUserWithNotUniqueEmail1()
        {
            const String password = "tesT1234";
            WebUser newUser, user;

            UseTransaction = true;
            user = GetNewUserInformation();
            user.EmailAddress = @"CreateUserWithNotUniqueEmail1@slu.se";
            user.UserName = "CreateUserWithNotUniqueEmail11";
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, password);
            Assert.IsNotNull(newUser);

            // Create user with same email adress as existing user.
            user = GetNewUserInformation();
            user.EmailAddress = newUser.EmailAddress;
            user.UserName = "CreateUserWithNotUniqueEmail12";
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, password);
            Assert.IsNotNull(newUser);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUserWithNotUniqueEmail2()
        {
            const String password = "tesT1234";
            WebPerson newPerson, person;
            WebUser newUser, user;

            // Create person.
            UseTransaction = true;
            person = GetNewPersonInformation(GetContext());
            person.EmailAddress = "CreateUserWithNotUniqueEmail2.slu.se";
            newPerson = UserService.Data.UserManager.CreatePerson(GetContext(), person);
            Assert.IsNotNull(newPerson);

            // Create user with same email adress as existing person.
            user = GetNewUserInformation();
            user.EmailAddress = newPerson.EmailAddress;
            user.UserName = "CreateUserWithNotUniqueEmail22";
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, password);
            Assert.IsNotNull(newUser);
        }

        [TestMethod]
        public void DeleteAuthority()
        {
            WebAuthority authority;

            // Get existing authority.
            UseTransaction = true;
            authority = UserService.Data.UserManager.GetAuthority(GetContext(), Settings.Default.TestAuthorityId);
            UserService.Data.UserManager.DeleteAuthority(GetContext(), authority);
        }

        [TestMethod]
        public void DeletePerson()
        {
            WebPerson person;

            // Get existing person.
            UseTransaction = true;
            person = UserService.Data.UserManager.GetPerson(GetContext(), Settings.Default.TestPersonId);
            UserService.Data.UserManager.DeletePerson(GetContext(), person);
        }

        [TestMethod]
        public void DeleteRole()
        {
            WebRole role;

            UseTransaction = true;
            role = GetOneRole();
            UserService.Data.UserManager.DeleteRole(GetContext(), role);
        }

        [TestMethod]
        public void DeleteRoleMembers()
        {
            UseTransaction = false;
            UserService.Data.UserManager.DeleteRoleMembers(GetContext());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteUserAdminRoleNoRoleError()
        {
            Int32 roleId;
            WebRole role;

            // Get existing useradminrole.
            UseTransaction = true;
            roleId = 1010;
            role = UserService.Data.UserManager.GetRole(GetContext(), roleId);
            UserService.Data.UserManager.DeleteRole(GetContext(), role);
        }

        [TestMethod]
        public void DeleteUser()
        {
            const String password = "tesT1234";
            WebPerson person;
            WebUser user;

            // Create person.
            UseTransaction = true;
            person = GetNewPersonInformation(GetContext());
            person.EmailAddress = @"DeleteUser@slu.se";
            person = UserService.Data.UserManager.CreatePerson(GetContext(), person);
            Assert.IsNotNull(person);

            // Create user attached to created person.
            user = GetNewUserInformation();
            user.EmailAddress = person.EmailAddress;
            user.IsPersonIdSpecified = true;
            user.PersonId = person.Id;
            user = UserService.Data.UserManager.CreateUser(GetContext(), user, password);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.IsPersonIdSpecified);
            Assert.AreEqual(person.Id, user.PersonId);

            // Delete user.
            UserService.Data.UserManager.DeleteUser(GetContext(), user);

            // Check that persons email address is reset to null.
            person = UserService.Data.UserManager.GetPerson(GetContext(), person.Id);
            Assert.IsNotNull(person);
            Assert.IsTrue(person.EmailAddress.IsEmpty());
        }

        [TestMethod]
        [Ignore]
        public void GeneratePassword()
        {
            // This test should be moved to ArtDatabanken.Test.
            // This test fails and succeeds randomly.
            // PasswordGenerator should be modified.
            UseTransaction = false;
            PasswordGenerator pwdGenerator = new PasswordGenerator();
            String newPassword = pwdGenerator.Generate();
            Assert.IsNotNull(newPassword);
            Assert.IsTrue(Regex.IsMatch(newPassword, @"^.*(?=.{8,40})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$"));
        }

        [TestMethod]
        public void GetApplicationUsers()
        {
            List<WebUser> users;

            UseTransaction = true;
            users = UserService.Data.UserManager.GetApplicationUsers(GetContext());
            Assert.IsFalse(users.IsEmpty());

            UseTransaction = false;
            users = UserService.Data.UserManager.GetApplicationUsers(GetContext());
            Assert.IsFalse(users.IsEmpty());
        }

        [TestMethod]
        public void GetAuthoritiesBySearchCriteria()
        {
            List<WebAuthority> authorities;
            String authorityIdentifier, applicationIdentifier, authorityDataTypeIdentifier, authorityName;
            WebAuthoritySearchCriteria searchCriteria;

            // Test all serach criterias if exist in DB or if not.
            // Test Authority Identifier
            UseTransaction = true;
            authorityIdentifier = "U%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityIdentifier = "NotExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria(); 
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Application Identifier
            applicationIdentifier = "UserService%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            applicationIdentifier = "NoServiceExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test AuthorityDataType Idenetifier
            authorityDataTypeIdentifier = "Speci%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityDataTypeIdentifier = "NoObsExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

           // Test Authority Name.
            authorityName = "test%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityName = "noTestExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Finally test that if no critera is set(ie WebAuthoritySearchCriteria is created by no data is set to search for) will not generat a exception.
            searchCriteria = new WebAuthoritySearchCriteria();
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            // Test all serach criterias if exist in DB or if not.
            // Test Authority Identifier
            UseTransaction = false;
            authorityIdentifier = "U%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityIdentifier = "NotExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityIdentifier = authorityIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Application Identifier
            applicationIdentifier = "UserService%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            applicationIdentifier = "NoServiceExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = applicationIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test AuthorityDataType Idenetifier
            authorityDataTypeIdentifier = "Speci%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityDataTypeIdentifier = "NoObsExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityDataTypeIdentifier = authorityDataTypeIdentifier;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Test Authority Name.
            authorityName = "test%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            authorityName = "noTestExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.AuthorityName = authorityName;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());

            // Finally test that if no critera is set(ie WebAuthoritySearchCriteria is created by no data is set to search for) will not generat a exception.
            searchCriteria = new WebAuthoritySearchCriteria();
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(authorities.IsEmpty());

            // Test with character '
            authorityName = "noTest'ExistInDB%";
            searchCriteria = new WebAuthoritySearchCriteria();
            searchCriteria.ApplicationIdentifier = "No'ServiceExistInDB%";
            searchCriteria.AuthorityDataTypeIdentifier = "NoObs'ExistInDB%";
            searchCriteria.AuthorityIdentifier = "NotExist'InDB%";
            searchCriteria.AuthorityName = authorityName;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(authorities.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAuthoritiesBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebAuthority> authorities;

            UseTransaction = true;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), null);
            Assert.IsTrue(authorities.IsEmpty());

            UseTransaction = false;
            authorities = UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(GetContext(), null);
            Assert.IsTrue(authorities.IsEmpty());
        }
        
        [TestMethod]
        public void GetAuthority()
        {
            WebAuthority authority;

            UseTransaction = true;
            authority = UserService.Data.UserManager.GetAuthority(GetContext(), Settings.Default.TestAuthorityId);
            Assert.IsNotNull(authority);
            Assert.IsTrue(authority.Name.Length > 5);
            Assert.IsTrue(authority.ReadPermission);
            Assert.IsNotNull(authority.GUID);
            Assert.IsNotNull(authority.CreatedBy);
            Assert.IsNotNull(authority.CreatedDate);
            Assert.IsNotNull(authority.ModifiedBy);
            Assert.IsNotNull(authority.ModifiedDate);

            UseTransaction = false;
            authority = UserService.Data.UserManager.GetAuthority(GetContext(), Settings.Default.TestAuthorityId);
            Assert.IsNotNull(authority);
            Assert.IsTrue(authority.Name.Length > 5);
            Assert.IsTrue(authority.ReadPermission);
            Assert.IsNotNull(authority.GUID);
            Assert.IsNotNull(authority.CreatedBy);
            Assert.IsNotNull(authority.CreatedDate);
            Assert.IsNotNull(authority.ModifiedBy);
            Assert.IsNotNull(authority.ModifiedDate);
        }
        
        [TestMethod]
        public void GetAuthorityAttributeTypes()
        {
            List<WebAuthorityAttributeType> authorityAttributeTypes;

            UseTransaction = true;
            authorityAttributeTypes = UserService.Data.UserManager.GetAuthorityAttributeTypes(GetContext());
            Assert.IsTrue(authorityAttributeTypes.IsNotEmpty());

            UseTransaction = false;
            authorityAttributeTypes = UserService.Data.UserManager.GetAuthorityAttributeTypes(GetContext());
            Assert.IsTrue(authorityAttributeTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetAuthorityDataTypes()
        {
            List<WebAuthorityDataType> list;

            UseTransaction = true;
            list = UserService.Data.UserManager.GetAuthorityDataTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);

            UseTransaction = false;
            list = UserService.Data.UserManager.GetAuthorityDataTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void GetLockedUserInformation()
        {
            List<WebLockedUserInformation> lockedUsers;
            String userName;
            WebServiceContext context;
            WebStringSearchCriteria userNameSearchString;

            // Search with no locked user.
            UseTransaction = false;
            lockedUsers = UserService.Data.UserManager.GetLockedUserInformation(GetContext(), null);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userNameSearchString = new WebStringSearchCriteria();
            userNameSearchString.SearchString = "No user name";
            lockedUsers = UserService.Data.UserManager.GetLockedUserInformation(GetContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userName = "qwertyWebService";
            context = new WebServiceContext(userName, Settings.Default.TestApplicationIdentifier);
            UserService.Data.UserManager.Login(context,
                                               userName,
                                               "hej hopp i lingon skogen",
                                               Settings.Default.TestApplicationIdentifier,
                                               false);
            UserService.Data.UserManager.Login(context,
                                               userName,
                                               "hej hopp i lingon skogen",
                                               Settings.Default.TestApplicationIdentifier,
                                               false);
            lockedUsers = UserService.Data.UserManager.GetLockedUserInformation(GetContext(), null);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userNameSearchString = new WebStringSearchCriteria();
            userNameSearchString.SearchString = userName;
            lockedUsers = UserService.Data.UserManager.GetLockedUserInformation(GetContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());

            // Search with locked user.
            UserService.Data.UserManager.Login(context,
                                               userName,
                                               "hej hopp i lingon skogen",
                                               Settings.Default.TestApplicationIdentifier,
                                               false);
            UserService.Data.UserManager.Login(context,
                                               userName,
                                               "hej hopp i lingon skogen",
                                               Settings.Default.TestApplicationIdentifier,
                                               false);
            UserService.Data.UserManager.Login(context,
                                               userName,
                                               "hej hopp i lingon skogen",
                                               Settings.Default.TestApplicationIdentifier,
                                               false);
            lockedUsers = UserService.Data.UserManager.GetLockedUserInformation(GetContext(), null);
            Assert.IsTrue(lockedUsers.IsNotEmpty());
            Assert.AreEqual(1, lockedUsers.Count);
            Assert.AreEqual(5, lockedUsers[0].LoginAttemptCount);
            Assert.AreEqual(userName, lockedUsers[0].UserName);
            userNameSearchString = new WebStringSearchCriteria();
            userNameSearchString.SearchString = "No user name";
            lockedUsers = UserService.Data.UserManager.GetLockedUserInformation(GetContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsEmpty());
            userNameSearchString = new WebStringSearchCriteria();
            userNameSearchString.SearchString = userName;
            lockedUsers = UserService.Data.UserManager.GetLockedUserInformation(GetContext(), userNameSearchString);
            Assert.IsTrue(lockedUsers.IsNotEmpty());
            Assert.AreEqual(1, lockedUsers.Count);
            Assert.AreEqual(5, lockedUsers[0].LoginAttemptCount);
            Assert.AreEqual(userName, lockedUsers[0].UserName);
        }

        [TestMethod]
        public void GetMessageTypes()
        {
            List<WebMessageType> list;

            UseTransaction = true;
            list = UserService.Data.UserManager.GetMessageTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);

            UseTransaction = false;
            list = UserService.Data.UserManager.GetMessageTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);

            list = UserService.Data.UserManager.GetMessageTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);
        }

        /// <summary>
        /// Get information about a new person.
        /// The person has not been create in the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about a new person.</returns>
        private WebPerson GetNewPersonInformation(WebServiceContext context)
        {
            WebPerson person;

            person = new WebPerson();
            person.EmailAddress = "GetNewPersonInformation.slu.se";
            person.FirstName = "Kalle";
            person.Gender = UserService.Data.UserManager.GetPersonGenders(context)[0];
            person.HasSpeciesCollection = true;
            person.LastName = "Kula";
            person.Locale = LocaleManager.GetLocale(context, (Int32)(LocaleId.sv_SE));
            person.URL = "www.artfakta.se";
            return person;
        }

        private WebUser GetNewPersonUser()
        {
            WebUser user = new WebUser();

            // Get existing user.
            user = UserService.Data.UserManager.GetUserById(GetContext(), Settings.Default.TestUserId);
            String password = "test1234GHJJH";

            // Make UserName unique
            user.UserName = user.UserName + "x";
            user.EmailAddress = "first.last@unique.com";
            user.Type = UserType.Person;
            return UserService.Data.UserManager.CreateUser(GetContext(), user, password);
        }

        /// <summary>
        /// Get information about a new person.
        /// The person has not been create in the database.
        /// </summary>
        /// <returns>Information about a new person.</returns>
        private WebUser GetNewUserInformation()
        {
            WebUser user;

            user = new WebUser();
            user.ActivationKey = "klfdlskfjalsdsdfsdfsdfsdfsdfsdfsdfsdfsfdfsdfsdfsdfsdfsdfsdf";
            user.EmailAddress = @"GetNewUserInformation.slu.se";
            user.IsAdministrationRoleIdSpecified = false;
            user.IsPersonIdSpecified = false;
            user.Type = UserType.Person;
            user.UserName = "CreateUser";
            user.ValidFromDate = DateTime.Today;
            user.ValidToDate = DateTime.Today + new TimeSpan(36500, 0, 0, 0);
            return user;
        }

        private WebRole GetOneRole()
        {
            String roleName, roleShortName;
            WebRole newRole, role;

            roleName = "UniqueRoleName";
            roleShortName = "UniqueRoleShortName";

            // Get existing role.
            role = UserService.Data.UserManager.GetRole(GetContext(), Settings.Default.TestRoleId);
            role.Name = roleName;
            role.ShortName = roleShortName;
            role.Identifier = roleName;
            role.IsUserAdministrationRoleIdSpecified = false;
            role.IsOrganizationIdSpecified = true;
            role.OrganizationId = 1;
            newRole = UserService.Data.UserManager.CreateRole(GetContext(), role);
            return newRole;
        }

        [TestMethod]
        public void GetPerson()
        {
            WebPerson person;

            UseTransaction = true;
            person = UserService.Data.UserManager.GetPerson(GetContext(), Settings.Default.TestPersonId);
            Assert.IsNotNull(person);
            Assert.AreEqual(Settings.Default.TestPersonFirstName, person.FirstName);
            Assert.IsNotNull(person.URL);
            Assert.AreEqual(Settings.Default.TestPersonId, person.Id);
            Assert.AreEqual(Settings.Default.TestEmailAddress, person.EmailAddress);
            Assert.AreEqual(Settings.Default.TestUserId, person.UserId);
            Assert.IsTrue(person.IsUserIdSpecified);
            Assert.IsTrue(person.HasSpeciesCollection);
            Assert.IsNotNull(person.Gender);
            Assert.IsNotNull(person.GUID);
            Assert.IsNotNull(person.CreatedBy);
            Assert.IsNotNull(person.CreatedDate);
            Assert.IsNotNull(person.ModifiedBy);
            Assert.IsNotNull(person.ModifiedDate);
            for (int n = 0; n < person.Addresses.Count;  n++)
            {
                Assert.AreEqual(person.Addresses[n].TypeId, person.Addresses[n].Type.Id);
            }

            // Test one address record is read into the object Person
            Assert.AreEqual(1, person.Addresses.Count);

            // Check Addresses.WebCountry
            Assert.AreEqual(person.Addresses[0].Country.NativeName, "Sverige");
            foreach (WebPhoneNumber phoneNumber in person.PhoneNumbers)
            {
                Assert.AreEqual(phoneNumber.TypeId, phoneNumber.Type.Id);
            }

            // Test two phonenumber records are read into the object Person
            Assert.AreEqual(2, person.PhoneNumbers.Count);

            // Check PhoneNumbers.WebCountry
            Assert.AreEqual(person.PhoneNumbers[0].Country.PhoneNumberPrefix, 46);

            UseTransaction = false;
            person = UserService.Data.UserManager.GetPerson(GetContext(), Settings.Default.TestPersonId);
            Assert.IsNotNull(person);
            Assert.AreEqual(Settings.Default.TestPersonFirstName, person.FirstName);
            Assert.IsNotNull(person.URL);
            Assert.AreEqual(Settings.Default.TestPersonId, person.Id);
            Assert.AreEqual(Settings.Default.TestEmailAddress, person.EmailAddress);
            Assert.AreEqual(Settings.Default.TestUserId, person.UserId);
            Assert.IsTrue(person.IsUserIdSpecified);
            Assert.IsTrue(person.HasSpeciesCollection);
            Assert.IsNotNull(person.Gender);
            Assert.IsNotNull(person.GUID);
            Assert.IsNotNull(person.CreatedBy);
            Assert.IsNotNull(person.CreatedDate);
            Assert.IsNotNull(person.ModifiedBy);
            Assert.IsNotNull(person.ModifiedDate);
            foreach (WebAddress webAddress in person.Addresses)
            {
                Assert.AreEqual(webAddress.TypeId, webAddress.Type.Id);
            }

            // Test one address record is read into the object Person
            Assert.AreEqual(1, person.Addresses.Count);

            // Check Addresses.WebCountry
            Assert.AreEqual(person.Addresses[0].Country.NativeName, "Sverige");
            foreach (WebPhoneNumber phoneNumber in person.PhoneNumbers)
            {
                Assert.AreEqual(phoneNumber.TypeId, phoneNumber.Type.Id);
            }

            // Test two phonenumber records are read into the object Person
            Assert.AreEqual(2, person.PhoneNumbers.Count);

            // Check PhoneNumbers.WebCountry
            Assert.AreEqual(person.PhoneNumbers[0].Country.PhoneNumberPrefix, 46);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPersonNonExistingPerson()
        {
            WebPerson person;

            UseTransaction = false;
            Int32 personId = -1;

            // Try to get non-existing person.
            person = UserService.Data.UserManager.GetPerson(GetContext(), personId);
            Assert.IsNull(person);
        }

        [TestMethod]
        public void GetPersonsBySearchCriteria()
        {
            List<WebPerson> persons;
            String name;
            Boolean hasSpeciesCollection;
            WebPersonSearchCriteria searchCriteria;

            UseTransaction = true;

            // All parameters are empty
            searchCriteria = new WebPersonSearchCriteria();
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            // Test first name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test HasCollection
            hasSpeciesCollection = true;
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.HasSpeciesCollection = hasSpeciesCollection;
            searchCriteria.IsHasSpeciesCollectionSpecified = true;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            UseTransaction = false;

            // All parameters are empty
            searchCriteria = new WebPersonSearchCriteria();
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            // Test first name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FirstName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FullName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.LastName = name;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());

            // Test HasCollection
            hasSpeciesCollection = true;
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.HasSpeciesCollection = hasSpeciesCollection;
            searchCriteria.IsHasSpeciesCollectionSpecified = true;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(persons.IsEmpty());

            // Test with character '.
            searchCriteria = new WebPersonSearchCriteria();
            searchCriteria.FirstName = "Hej 'hopp i lingonskogen";
            searchCriteria.FullName = "Hej hopp' i lingonskogen";
            searchCriteria.LastName = "Hej hopp i lingon'skogen";
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(persons.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPersonsBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebPerson> persons;

            UseTransaction = false;
            persons = UserService.Data.UserManager.GetPersonsBySearchCriteria(GetContext(), null);
            Assert.IsTrue(persons.IsEmpty());
        }

        [TestMethod]
        public void GetUsersByRole()
        {
            List<WebUser> roleMembers;

            // Get all role members 
            UseTransaction = true;
            roleMembers = UserService.Data.UserManager.GetUsersByRole(GetContext(), Settings.Default.TestRoleId);
            Assert.IsNotNull(roleMembers);
            Assert.IsTrue(roleMembers.Count > 1);
            roleMembers[0].UserName = Settings.Default.TestUserName;
            roleMembers[0].EmailAddress = Settings.Default.TestEmailAddress;
            Assert.IsTrue(roleMembers[0].IsAccountActivated);
            Assert.IsTrue(roleMembers[1].IsAccountActivated);
            Assert.AreEqual(roleMembers[0].Type, UserType.Person);
            Assert.AreEqual(roleMembers[1].Type, UserType.Application);

            // Get all role members 
            UseTransaction = false;
            roleMembers = UserService.Data.UserManager.GetUsersByRole(GetContext(), Settings.Default.TestRoleId);
            Assert.IsNotNull(roleMembers);
            Assert.IsTrue(roleMembers.Count > 1);
            roleMembers[0].UserName = Settings.Default.TestUserName;
            roleMembers[0].EmailAddress = Settings.Default.TestEmailAddress;
            Assert.IsTrue(roleMembers[0].IsAccountActivated);
            Assert.IsTrue(roleMembers[1].IsAccountActivated);
            Assert.AreEqual(roleMembers[0].Type, UserType.Person);
            Assert.AreEqual(roleMembers[1].Type, UserType.Application);
        }

        [TestMethod]
        public void GetNonActivatedUsersByRole()
        {
            List<WebUser> roleMembers;

            UseTransaction = true;
            roleMembers = UserService.Data.UserManager.GetNonActivatedUsersByRole(GetContext(), Settings.Default.TestRoleId);
            Assert.IsNotNull(roleMembers);
            Assert.AreEqual(roleMembers.Count, 0);

            UseTransaction = false;
            roleMembers = UserService.Data.UserManager.GetNonActivatedUsersByRole(GetContext(), Settings.Default.TestRoleId);
            Assert.IsNotNull(roleMembers);
            Assert.AreEqual(roleMembers.Count, 0);
        }

        [TestMethod]
        public void GetRolesBySearchCriteria()
        {
            List<WebRole> roles;
            String name;
            Int32 organizationId;
            WebRoleSearchCriteria searchCriteria;

            UseTransaction = true;

            // Test role name.
            name = "A%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Name = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Name = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test short name.
            name = "A%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test identifier.
            name = "A%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Identifier = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Identifier = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test organizationId
            organizationId = 1;
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            organizationId = -1;
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            UseTransaction = false;

            // Test role name.
            name = "A%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Name = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Name = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test short name.
            name = "A%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.ShortName = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test identifier.
            name = "A%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Identifier = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Identifier = name;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test organizationId
            organizationId = 1;
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roles.IsEmpty());

            organizationId = -1;
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.OrganizationId = organizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());

            // Test with character '.
            searchCriteria = new WebRoleSearchCriteria();
            searchCriteria.Identifier = "Hej' hopp i lingonskogen";
            searchCriteria.Name = "Hej hopp' i lingonskogen";
            searchCriteria.ShortName = "Hej hopp i 'lingonskogen";
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(roles.IsEmpty());
        }

        [TestMethod]
        public void GetRoleMembersBySearchCriteria()
        {
            List<WebRoleMember> roleMembers;
            List<Int32> roleIds = new List<int>();
            List<Int32> userIds = new List<int>();
            WebRoleMemberSearchCriteria searchCriteria;

            UseTransaction = true;
            searchCriteria = new WebRoleMemberSearchCriteria();
            roleIds.Add(5);
            searchCriteria.RoleIds = roleIds;
            searchCriteria.UserIds = new List<int>();
            roleMembers = UserService.Data.UserManager.GetRoleMembersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.Role.Authorities.IsNull());
                Assert.IsFalse(member.User.IsNull());
            }

            userIds.Add(2);
            searchCriteria.RoleIds = new List<int>();
            searchCriteria.UserIds = userIds;
            roleMembers = UserService.Data.UserManager.GetRoleMembersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.User.IsNull());
            }

            searchCriteria.RoleIds = new List<int>();
            searchCriteria.UserIds = new List<int>();
            searchCriteria.IsActivated = false;
            searchCriteria.IsIsActivatedSpecified = true;
            roleMembers = UserService.Data.UserManager.GetRoleMembersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.User.IsNull());
                Assert.IsFalse(member.IsActivated);
            }

            UseTransaction = false;
            searchCriteria = new WebRoleMemberSearchCriteria();
            roleIds.Add(5);
            searchCriteria.RoleIds = roleIds;
            searchCriteria.UserIds = new List<int>();
            roleMembers = UserService.Data.UserManager.GetRoleMembersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.Role.Authorities.IsNull());
                Assert.IsFalse(member.User.IsNull());
            }

            userIds.Add(2);
            searchCriteria.RoleIds = new List<int>();
            searchCriteria.UserIds = userIds;
            roleMembers = UserService.Data.UserManager.GetRoleMembersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.User.IsNull());
            }

            searchCriteria.RoleIds = new List<int>();
            searchCriteria.UserIds = new List<int>();
            searchCriteria.IsActivated = false;
            searchCriteria.IsIsActivatedSpecified = true;
            roleMembers = UserService.Data.UserManager.GetRoleMembersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(roleMembers.IsEmpty());
            foreach (var member in roleMembers)
            {
                Assert.IsFalse(member.Role.IsNull());
                Assert.IsFalse(member.User.IsNull());
                Assert.IsFalse(member.IsActivated);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRolesBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebRole> roles;

            UseTransaction = false;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), null);
            Assert.IsTrue(roles.IsEmpty());
        }

        [TestMethod]
        public void GetRolesByUser()
        {
            List<WebRole> userRoles;
            Int32 userId = 4;
            String applicationIdentifier = "UserService";

            UseTransaction = true;
            userRoles = UserService.Data.UserManager.GetRolesByUser(GetContext(), userId, applicationIdentifier);
            Assert.IsNotNull(userRoles);
            Assert.IsTrue(userRoles.Count > 1);
            Assert.IsNotNull(userRoles[0].Description);
            Assert.IsNotNull(userRoles[1].Description);
            Assert.IsTrue(userRoles[0].Authorities.Count > 0);

            UseTransaction = false;
            userRoles = UserService.Data.UserManager.GetRolesByUser(GetContext(), userId, applicationIdentifier);
            Assert.IsNotNull(userRoles);
            Assert.IsTrue(userRoles.Count > 1);
            Assert.IsNotNull(userRoles[0].Description);
            Assert.IsNotNull(userRoles[1].Description);
            Assert.IsTrue(userRoles[0].Authorities.Count > 0);

            userRoles = UserService.Data.UserManager.GetRolesByUser(GetContext(), userId, "Hej ' hopp");
            Assert.IsTrue(userRoles.IsEmpty());
        }

        [TestMethod]
        public void GetRolesByUserGroupAdministrationRoleId()
        {
            List<WebRole> roles;
            WebRoleSearchCriteria searchCriteria = new WebRoleSearchCriteria();
            Int32 id = -1;

            UseTransaction = true;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            foreach (WebRole role in roles)
            {
                List<WebRole> administratedRoles = UserService.Data.UserManager.GetRolesByUserGroupAdministrationRoleId(GetContext(), role.Id);
                if (administratedRoles.IsNotEmpty())
                {
                    id = administratedRoles[0].Id;
                }
            }

            Assert.IsTrue(id > -1);

            UseTransaction = false;
            roles = UserService.Data.UserManager.GetRolesBySearchCriteria(GetContext(), searchCriteria);
            foreach (WebRole role in roles)
            {
                List<WebRole> administratedRoles = UserService.Data.UserManager.GetRolesByUserGroupAdministrationRoleId(GetContext(), role.Id);
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
            Int32 id = 1;
            List<WebRole> administratedRoles;

            UseTransaction = true;
            administratedRoles = UserService.Data.UserManager.GetRolesByUserGroupAdministratorUserId(GetContext(), id);
            Assert.IsTrue(administratedRoles.Count > 0);

            UseTransaction = false;
            administratedRoles = UserService.Data.UserManager.GetRolesByUserGroupAdministratorUserId(GetContext(), id);
            Assert.IsTrue(administratedRoles.Count > 0);
        }

        [TestMethod]
        public void GetUser()
        {
            WebUser user;

            UseTransaction = true;
            user = UserService.Data.UserManager.GetUser(GetContext());
            Assert.IsNotNull(user);
            Assert.AreEqual(user.UserName, GetContext().ClientToken.UserName);
            Assert.AreEqual(user.Type, UserType.Person);
            Assert.IsNotNull(user.ValidFromDate);
            Assert.IsNotNull(user.ValidToDate);

            UseTransaction = false;
            user = UserService.Data.UserManager.GetUser(GetContext());
            Assert.IsNotNull(user);
            Assert.AreEqual(user.UserName, GetContext().ClientToken.UserName);
            Assert.AreEqual(user.Type, UserType.Person);
            Assert.IsNotNull(user.ValidFromDate);
            Assert.IsNotNull(user.ValidToDate);
        }

        [TestMethod]
        public void GetUserByName()
        {
            WebUser user;

            UseTransaction = true;
            user = UserService.Data.UserManager.GetUserByName(GetContext(), Settings.Default.TestUserName);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.UserName, GetContext().ClientToken.UserName);
            Assert.AreEqual(user.Type, UserType.Person);
            Assert.IsNotNull(user.ValidFromDate);
            Assert.IsNotNull(user.ValidToDate);

            UseTransaction = false;
            user = UserService.Data.UserManager.GetUserByName(GetContext(), Settings.Default.TestUserName);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.UserName, GetContext().ClientToken.UserName);
            Assert.AreEqual(user.Type, UserType.Person);
            Assert.IsNotNull(user.ValidFromDate);
            Assert.IsNotNull(user.ValidToDate);

            user = UserService.Data.UserManager.GetUserByName(GetContext(), "Hej 'hopp");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserNonExistingUser()
        {
            WebUser user;

            // Get none existing user - should return NULL
            UseTransaction = true;
            using (WebServiceContext context = new WebServiceContext("None existing user", "No application name"))
            {
                user = UserService.Data.UserManager.GetUser(context);
                Assert.IsNull(user);
            }

            // Get none existing user - should return NULL
            UseTransaction = false;
            using (WebServiceContext context = new WebServiceContext("None existing user", "No application name"))
            {
                user = UserService.Data.UserManager.GetUser(context);
                Assert.IsNull(user);
            }
        }

        [TestMethod]
        public void GetUsersBySearchCriteria()
        {
            List<WebUser> users;
            String name;
            WebUserSearchCriteria searchCriteria;

            UseTransaction = true;

            // Test first name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            // Test email.
            name = "%Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.EmailAddress = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            // Test email.
            name = "%Uppsala%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.City = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FullName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FullName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.LastName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.LastName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test userType.
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.UserType = UserType.Application;
            searchCriteria.IsUserTypeSpecified = true;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            // Test organizationId
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.OrganizationId = Settings.Default.TestOrganizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            UseTransaction = false;

            // Test first name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            // Test email.
            name = "%Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.EmailAddress = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            // Test email.
            name = "%Uppsala%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.City = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FirstName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test full name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FullName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.FullName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test last name.
            name = "Test%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.LastName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            name = "Tilt%";
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.LastName = name;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());

            // Test userType.
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.UserType = UserType.Application;
            searchCriteria.IsUserTypeSpecified = true;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            // Test organizationId
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.OrganizationId = Settings.Default.TestOrganizationId;
            searchCriteria.IsOrganizationIdSpecified = true;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsFalse(users.IsEmpty());

            // Test with character '.
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.Address = "Hej' hopp i lingonskogen";
            searchCriteria.City = "Hej hopp' i lingonskogen";
            searchCriteria.EmailAddress = "Hej hopp i' lingonskogen";
            searchCriteria.FullName = "Hej hopp i lin'gonskogen";
            searchCriteria.LastName = "Hej hopp i lingons'kogen";
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(users.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetUsersBySearchCriteriaNullSearchCriteriaError()
        {
            List<WebUser> users;

            UseTransaction = false;
            users = UserService.Data.UserManager.GetUsersBySearchCriteria(GetContext(), null);
            Assert.IsTrue(users.IsEmpty());
        }

        [TestMethod]
        public void GetPersonGenders()
        {
            List<WebPersonGender> list;

            UseTransaction = true;
            list = UserService.Data.UserManager.GetPersonGenders(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);

            UseTransaction = false;
            list = UserService.Data.UserManager.GetPersonGenders(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);

            // Test if data is cached.
            list = UserService.Data.UserManager.GetPersonGenders(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);
        }

        [TestMethod]
        public void GetAddressTypes()
        {
            List<WebAddressType> list;

            UseTransaction = true;
            list = UserService.Data.UserManager.GetAddressTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);

            UseTransaction = false;
            list = UserService.Data.UserManager.GetAddressTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);

            list = UserService.Data.UserManager.GetAddressTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);
        }

        [TestMethod]
        public void GetPhoneNumberTypes()
        {
            List<WebPhoneNumberType> list;

            UseTransaction = true;
            list = UserService.Data.UserManager.GetPhoneNumberTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);

            UseTransaction = false;
            list = UserService.Data.UserManager.GetPhoneNumberTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);

            // Test if information is cached.
            list = UserService.Data.UserManager.GetPhoneNumberTypes(GetContext());
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 2);
        }

        [TestMethod]
        public void GetRole()
        {
            WebRole role;

            // Get existing role.
            UseTransaction = true;
            role = UserService.Data.UserManager.GetRole(GetContext(), Settings.Default.TestRoleId);
            Assert.IsNotNull(role);
            Assert.AreEqual(Settings.Default.TestRoleName, role.Name);
            Assert.AreEqual(Settings.Default.TestRoleId, role.Id);
            Assert.AreEqual(false, role.IsUserAdministrationRole);
            Assert.IsNotNull(role.Description);
            Assert.IsNotNull(role.GUID);
            Assert.IsNotNull(role.CreatedBy);
            Assert.IsNotNull(role.CreatedDate);
            Assert.IsNotNull(role.ModifiedBy);
            Assert.IsNotNull(role.ModifiedDate);
            Assert.IsNotNull(role.ValidFromDate);
            Assert.IsNotNull(role.ValidToDate);

            // Get existing role.
            UseTransaction = false;
            role = UserService.Data.UserManager.GetRole(GetContext(), Settings.Default.TestRoleId);
            Assert.IsNotNull(role);
            Assert.AreEqual(Settings.Default.TestRoleName, role.Name);
            Assert.AreEqual(Settings.Default.TestRoleId, role.Id);
            Assert.AreEqual(false, role.IsUserAdministrationRole);
            Assert.IsNotNull(role.Description);
            Assert.IsNotNull(role.GUID);
            Assert.IsNotNull(role.CreatedBy);
            Assert.IsNotNull(role.CreatedDate);
            Assert.IsNotNull(role.ModifiedBy);
            Assert.IsNotNull(role.ModifiedDate);
            Assert.IsNotNull(role.ValidFromDate);
            Assert.IsNotNull(role.ValidToDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetNonExistingRole()
        {
            WebRole role;

            // Set testdata
            Int32 roleId = -1;

            // Try to get non-existing role.
            UseTransaction = false;
            role = UserService.Data.UserManager.GetRole(GetContext(), roleId);
            Assert.IsNull(role);
        }

        [TestMethod]
        public void IsExistingPerson()
        {
            const String nonExistingEmail = "NonExisting@slu.se";
            WebPerson newPerson, person;

            // Get existing person.
            UseTransaction = true;
            person = GetNewPersonInformation(GetContext());
            person.EmailAddress = "IsExistingPerson.slu.se";
            newPerson = UserService.Data.UserManager.CreatePerson(GetContext(), person);
            Assert.IsNotNull(newPerson);
            Assert.IsTrue(UserService.Data.UserManager.IsExistingPerson(GetContext(), person.EmailAddress));

            // Get none existing person.
            Assert.IsFalse(UserService.Data.UserManager.IsExistingPerson(GetContext(), nonExistingEmail));
        }

        [TestMethod]
        public void IsExistingUser()
        {
            String nonExistingUserName;
            WebUser user;

            // Get existing user.
            UseTransaction = true;
            user = UserService.Data.UserManager.GetUser(GetContext());
            Assert.IsNotNull(user);
            Assert.IsTrue(UserService.Data.UserManager.IsExistingUser(GetContext(), user.UserName));
            nonExistingUserName = "NonExistinguserName";
            Assert.IsFalse(UserService.Data.UserManager.IsExistingUser(GetContext(), nonExistingUserName));

            // Get existing user.
            UseTransaction = false;
            user = UserService.Data.UserManager.GetUser(GetContext());
            Assert.IsNotNull(user);
            Assert.IsTrue(UserService.Data.UserManager.IsExistingUser(GetContext(), user.UserName));
            nonExistingUserName = "NonExistinguserName";
            Assert.IsFalse(UserService.Data.UserManager.IsExistingUser(GetContext(), nonExistingUserName));
        }

        [TestMethod]
        public void Login()
        {
            Int32 loginCount;
            WebLoginResponse loginResponse;

            UseTransaction = false;

            if (Configuration.IsAllTestsRun)
            {
                // UserLogin existing user.
                loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
                Assert.IsNotNull(loginResponse);

                // UserLogin none existing user.
                loginResponse = UserService.Data.UserManager.Login(GetContext(), "None existing user", "No password", Settings.Default.TestApplicationIdentifier, false);
                Assert.IsNull(loginResponse);

                // Test to fail a couple of times and finally succed.
                for (loginCount = 0; loginCount < (UserService.Settings.Default.MaxLoginAttempt - 1); loginCount++)
                {
                    loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, "No password", Settings.Default.TestApplicationIdentifier, false);
                    Assert.IsNull(loginResponse);
                }

                loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
                Assert.IsNotNull(loginResponse);
                for (loginCount = 0; loginCount < (UserService.Settings.Default.MaxLoginAttempt - 1); loginCount++)
                {
                    loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, "No password", Settings.Default.TestApplicationIdentifier, false);
                    Assert.IsNull(loginResponse);
                }

                loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
                Assert.IsNotNull(loginResponse);

                // Test to fail to many times.
                for (loginCount = 0; loginCount < UserService.Settings.Default.MaxLoginAttempt; loginCount++)
                {
                    loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, "No password", Settings.Default.TestApplicationIdentifier, false);
                    Assert.IsNull(loginResponse);
                }

                loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
                Assert.IsNull(loginResponse);

                // Wait for login attempt to be allowed again.
                Thread.Sleep(65000 * UserService.Settings.Default.MinFailedLoginWaitTime);
                loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
                Assert.IsNotNull(loginResponse);
            }
        }

        [TestMethod]
        public void LoginUserAdmin()
        {
            WebLoginResponse loginResponse;

            UseTransaction = false;
            loginResponse = UserService.Data.UserManager.Login(GetContext(), Settings.Default.TestUserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
            Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        public void Logout()
        {
            // Logout existing user.
            UserService.Data.UserManager.Logout(GetContext());

            // Should be ok to logout an already logged out user.
            UserService.Data.UserManager.Logout(GetContext());
        }

        [TestMethod]
        public void RemoveUserFromRole()
        {
            Int32 roleId;
            WebUser user;

            UseTransaction = true;
            roleId = Settings.Default.TestRoleId;
            user = GetNewPersonUser();

            // Add new user to Role
            UserService.Data.UserManager.AddUserToRole(GetContext(), roleId, user.Id);

            // Remove new user from Role
            UserService.Data.UserManager.RemoveUserFromRole(GetContext(), roleId, user.Id);
        }

        [TestMethod]
        public void ResetPassword()
        {
            String emailAddress = "artdata@slu.se";
            WebPasswordInformation passwordInformation;
            WebLoginResponse loginResponse = new WebLoginResponse();

            UseTransaction = true;
            passwordInformation = UserService.Data.UserManager.ResetPassword(GetContext(), emailAddress);
            Assert.IsNotNull(passwordInformation);

            // UserLogin w. new password
            loginResponse = UserService.Data.UserManager.Login(GetContext(), passwordInformation.UserName, passwordInformation.Password, Settings.Default.TestApplicationIdentifier, false);
            Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        public void UpdateAuthority()
        {
            WebAuthority authority;
            String name = "UpdateName";

            // Get existing authority.
            UseTransaction = true;
            authority = UserService.Data.UserManager.GetAuthority(GetContext(), Settings.Default.TestAuthorityId);
            authority.Identifier = "TestIdentityUpdate";
            authority.Name = name;
            authority.ModifiedBy = Settings.Default.TestUserId;
            authority.AdministrationRoleId = Settings.Default.TestUserId;
            authority.ReadPermission = false;
            authority.CreatePermission = false;
            authority.UpdatePermission = false;
            authority.DeletePermission = false;
            authority.Description = "Testdescription update";
            authority.Obligation = "Testobligation update";
            authority.ValidFromDate = DateTime.Now;
            authority.ValidToDate = DateTime.Today.AddYears(100);
            authority.ActionGUIDs.Add("1");
            authority.RegionGUIDs = new List<String>();
            authority.RegionGUIDs.Add("urn:lsid:artportalen.se:Area:DataSet1Feature1");

            WebAuthority updatedAuthority;
            updatedAuthority = UserService.Data.UserManager.UpdateAuthority(GetContext(), authority);
            Assert.IsNotNull(updatedAuthority);
            Assert.AreEqual(Settings.Default.TestAuthorityId, updatedAuthority.Id);
            Assert.AreEqual(updatedAuthority.Name, name);
            Assert.AreEqual(updatedAuthority.Description, authority.Description);
            Assert.AreEqual(updatedAuthority.Obligation, authority.Obligation);
            Assert.AreEqual(updatedAuthority.ModifiedBy, Settings.Default.TestUserId);
            Assert.AreEqual(updatedAuthority.Identifier, authority.Identifier);
            Assert.IsFalse(updatedAuthority.ReadPermission);
            Assert.IsFalse(updatedAuthority.CreatePermission);
            Assert.IsFalse(updatedAuthority.UpdatePermission);
            Assert.IsFalse(updatedAuthority.DeletePermission);
            Assert.IsNotNull(updatedAuthority.ValidFromDate);
            Assert.IsNotNull(updatedAuthority.ValidToDate);
            Assert.IsTrue(updatedAuthority.ActionGUIDs.Count > 1);
        }

        [TestMethod]
        public void UpdatePassword()
        {
            String newPassword, oldPassword;

            UseTransaction = true;
            oldPassword = Settings.Default.TestPassword;
            newPassword = Settings.Default.TestNewPassword;
            Assert.IsTrue(UserService.Data.UserManager.UpdatePassword(GetContext(), oldPassword, newPassword));
        }

        [TestMethod]
        public void UpdateUser()
        {
            WebUser user;

            // Get existing user.
            UseTransaction = true;
            user = UserService.Data.UserManager.GetUserById(GetContext(), Settings.Default.TestUserIdForUpdate);
            Assert.IsFalse(user.IsAdministrationRoleIdSpecified);
            user.ShowEmailAddress = true;
            user.EmailAddress = "update@Updated.com";
            user.IsAccountActivated = true;
            user.ValidFromDate = DateTime.Now;
            user.ValidToDate = DateTime.Today.AddYears(100);
            user = UserService.Data.UserManager.UpdateUser(GetContext(), user);
            Assert.IsNotNull(user);
            Assert.AreEqual(Settings.Default.TestUserNameForUpdate, user.UserName);
            Assert.AreNotEqual(Settings.Default.TestEmailAddress, user.EmailAddress);
            Assert.IsTrue(user.IsAccountActivated);
            Assert.IsTrue(user.ShowEmailAddress);
            Assert.IsNotNull(user.ValidFromDate);
            Assert.IsNotNull(user.ValidToDate);
            Assert.IsFalse(user.IsAdministrationRoleIdSpecified);
        }

        [TestMethod, ExpectedException(typeof(ApplicationException), "User account with Id: 3 is already activated")]
        public void SupportUpdatePersonUser_UserAccountIsAlreadyActivated_ExceptionIsThrown()
        {
            WebUser user;
            WebPerson person;

            // Activate account if needed.                        
            user = UserService.Data.UserManager.GetUserById(GetContext(), Settings.Default.TestUserIdForUpdate);
            if (!user.IsAccountActivated)
            {
                UseTransaction = true;
                user.IsAccountActivated = true;
                user = UserService.Data.UserManager.UpdateUser(GetContext(), user);
                Assert.IsNotNull(user);
                Assert.IsTrue(user.IsAccountActivated);
                GetContext().ClearCache();
            }

            // Update account using support authority.
            UseTransaction = true;
            user = UserService.Data.UserManager.GetUserById(GetContext(), Settings.Default.TestUserIdForUpdate);
            person = UserService.Data.UserManager.GetPerson(GetContext(), user.PersonId);
            user.EmailAddress = "update@Updated.com";
            person.EmailAddress = "update@Updated.com";
            user = UserService.Data.UserManager.SupportUpdatePersonUser(GetContext(), user, person);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void SupportUpdatePersonUser_SetUserAccountNotActivated_OnlyEmailAndActivationIsChanged()
        {
            WebUser user;
            WebPerson person;

            // Inactivate account if needed.                                    
            user = UserService.Data.UserManager.GetUserById(GetContext(), Settings.Default.TestUserIdForUpdate);
            if (user.IsAccountActivated)
            {
                UseTransaction = true;
                user.IsAccountActivated = false;
                user = UserService.Data.UserManager.UpdateUser(GetContext(), user);
                Assert.IsNotNull(user);
                Assert.IsFalse(user.IsAccountActivated);
            }

            // Update user and person by support authority.
            UseTransaction = true;
            user = UserService.Data.UserManager.GetUserById(GetContext(), Settings.Default.TestUserIdForUpdate);
            person = UserService.Data.UserManager.GetPerson(GetContext(), user.PersonId);
            Assert.IsFalse(user.IsAdministrationRoleIdSpecified);
            user.ShowEmailAddress = true;
            user.EmailAddress = "update@Updated.com";
            person.EmailAddress = "update@Updated.com";
            person.FirstName = "Glenn";
            user.IsAccountActivated = true;
            DateTime originalValidFromDate = user.ValidFromDate;
            DateTime originalValidToDate = user.ValidToDate;
            user.ValidFromDate = DateTime.Now;
            user.ValidToDate = DateTime.Today.AddYears(100);
            GetContext().ClearCache();
            user = UserService.Data.UserManager.SupportUpdatePersonUser(GetContext(), user, person);
            Assert.IsNotNull(user);
            Assert.AreEqual("update@Updated.com", user.EmailAddress);
            Assert.IsTrue(user.IsAccountActivated);
            Assert.AreEqual(originalValidFromDate, user.ValidFromDate);
            Assert.AreEqual(originalValidToDate, user.ValidToDate);
            Assert.IsFalse(user.IsAdministrationRoleIdSpecified);

            person = UserService.Data.UserManager.GetPerson(GetContext(), user.PersonId);
            Assert.IsNotNull(person);
            Assert.AreEqual("update@Updated.com", user.EmailAddress);
            Assert.AreNotEqual("Glenn", person.FirstName);
        }


        [TestMethod]
        public void UpdateUserPassword()
        {
            WebLoginResponse loginResponse;
            WebUser user;
            
            // Get existing user.
            UseTransaction = true;
            Int32 userId = Settings.Default.TestUserId;
            user = UserService.Data.UserManager.GetUserById(GetContext(), userId);
            
            // UserLogin w. old password
            loginResponse = UserService.Data.UserManager.Login(GetContext(), user.UserName, Settings.Default.TestPassword, Settings.Default.TestApplicationIdentifier, false);
            Assert.IsNotNull(loginResponse);
        }

        [TestMethod]
        public void UpdatePerson()
        {
            WebPerson person;

            // Get existing person.
            UseTransaction = true;
            person = UserService.Data.UserManager.GetPerson(GetContext(), Settings.Default.TestPersonId);
            person.ModifiedBy = Settings.Default.TestUserId;
            person.ShowEmailAddress = true;
            person.EmailAddress = "update@updated.com";
            person.Presentation = "Presentation update #3";
            person.ShowAddresses = true;
            person.ShowPhoneNumbers = true;
            person.TaxonNameTypeId = 10;

            // Try to change connected user record.
            person.IsUserIdSpecified = true;
            person.UserId = 23;
            person.ShowPresentation = true;

            WebPerson updatedPerson;
            updatedPerson = UserService.Data.UserManager.UpdatePerson(GetContext(), person);
            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(updatedPerson.EmailAddress, person.EmailAddress);
            Assert.AreEqual(updatedPerson.ModifiedBy, Settings.Default.TestUserId);
            Assert.IsTrue(updatedPerson.ShowEmailAddress);
            Assert.IsTrue(updatedPerson.ShowAddresses);
            Assert.IsTrue(updatedPerson.ShowPhoneNumbers);
            Assert.IsTrue(updatedPerson.ShowPresentation);
            Assert.IsNotNull(updatedPerson.Presentation);
            Assert.AreEqual(updatedPerson.TaxonNameTypeId, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdatePersonNotUniqueEmailAddressError1()
        {
            WebPerson newPerson1, newPerson2;

            // Create new person 1.
            UseTransaction = true;
            newPerson1 = GetNewPersonInformation(GetContext());
            newPerson1.EmailAddress = "UpdatePersonNotUniqueEmailAddressError11.slu.se";
            newPerson1 = UserService.Data.UserManager.CreatePerson(GetContext(), newPerson1);
            Assert.IsNotNull(newPerson1);

            // Create new person 2.
            newPerson2 = GetNewPersonInformation(GetContext());
            newPerson2.EmailAddress = "UpdatePersonNotUniqueEmailAddressError12.slu.se";
            newPerson2 = UserService.Data.UserManager.CreatePerson(GetContext(), newPerson2);
            Assert.IsNotNull(newPerson2);

            // Update new person 1 to new person 2 email adress.
            newPerson1.EmailAddress = newPerson2.EmailAddress;
            newPerson1 = UserService.Data.UserManager.UpdatePerson(GetContext(), newPerson1);
            Assert.IsNotNull(newPerson1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdatePersonNotUniqueEmailAddressError2()
        {
            WebPerson newPerson;
            WebUser newUser, user;

            // Create user.
            UseTransaction = true;
            user = GetNewUserInformation();
            user.EmailAddress = @"UpdatePersonNotUniqueEmailAddressError21.slu.se";
            newUser = UserService.Data.UserManager.CreateUser(GetContext(), user, "fakdffk2323fklH");
            Assert.IsNotNull(newUser);

            // Create new person.
            newPerson = GetNewPersonInformation(GetContext());
            newPerson.EmailAddress = "UpdatePersonNotUniqueEmailAddressError22.slu.se";
            newPerson = UserService.Data.UserManager.CreatePerson(GetContext(), newPerson);
            Assert.IsNotNull(newPerson);

            // Update new person to user email adress.
            newPerson.EmailAddress = user.EmailAddress;
            newPerson = UserService.Data.UserManager.UpdatePerson(GetContext(), newPerson);
            Assert.IsNotNull(newPerson);
        }

        [TestMethod]
        public void UpdatePersonNoAddressNoPhone()
        {
            WebPerson person;

            // Get existing person.
            UseTransaction = true;
            person = UserService.Data.UserManager.GetPerson(GetContext(), Settings.Default.TestPersonId);
            person.ModifiedBy = Settings.Default.TestUserId;
            person.ShowEmailAddress = true;
            person.EmailAddress = "update@updated.com";
            person.Presentation = "Presentation update #2";
            person.ShowAddresses = true;
            person.ShowPhoneNumbers = true;
            person.TaxonNameTypeId = 10;
          
            person.Addresses = null;
            person.PhoneNumbers = null;

            WebPerson updatedPerson;
            updatedPerson = UserService.Data.UserManager.UpdatePerson(GetContext(), person);
            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(Settings.Default.TestPersonId, updatedPerson.Id);
            Assert.AreEqual(updatedPerson.EmailAddress, person.EmailAddress);
            Assert.AreEqual(updatedPerson.ModifiedBy, Settings.Default.TestUserId);
            Assert.IsTrue(updatedPerson.ShowEmailAddress);
            Assert.IsTrue(updatedPerson.ShowAddresses);
            Assert.IsTrue(updatedPerson.ShowPhoneNumbers);
            Assert.IsNotNull(updatedPerson.Presentation);
            Assert.AreEqual(updatedPerson.TaxonNameTypeId, 10);

            // No address records
            Assert.AreEqual(0, updatedPerson.Addresses.Count);

            // No phonenumbers
            Assert.AreEqual(0, updatedPerson.PhoneNumbers.Count);
        }

        [TestMethod]
        public void UpdateRole()
        {
            WebRole role, updatedRole;

            // Get existing role.
            UseTransaction = true;
            role = UserService.Data.UserManager.GetRole(GetContext(), Settings.Default.TestRoleId);
            role.Name = "TestNameUpdated123";
            role.ShortName = "TestRoleShortName123";
            role.ModifiedBy = Settings.Default.TestUserId;
            role.IsUserAdministrationRoleIdSpecified = false;
            role.IsOrganizationIdSpecified = true;
            role.OrganizationId = Settings.Default.TestOrganizationId;
            role.Description = "Testdescription update123";
            role.ValidFromDate = DateTime.Now;
            role.ValidToDate = DateTime.Today.AddYears(100);
            role.IsActivationRequired = true;
            role.MessageTypeId = 2;

            updatedRole = UserService.Data.UserManager.UpdateRole(GetContext(), role);
            Assert.IsNotNull(updatedRole);
            Assert.AreEqual(Settings.Default.TestRoleId, updatedRole.Id);
            Assert.AreEqual(updatedRole.Description, role.Description);
            Assert.AreEqual(updatedRole.ModifiedBy, Settings.Default.TestUserId);
            Assert.AreEqual(updatedRole.Name, role.Name);
            Assert.AreEqual(updatedRole.ShortName, role.ShortName);
            Assert.IsTrue(updatedRole.IsOrganizationIdSpecified);
            Assert.AreEqual(updatedRole.OrganizationId, Settings.Default.TestOrganizationId);
            Assert.IsNotNull(updatedRole.ValidFromDate);
            Assert.IsNotNull(updatedRole.ValidToDate);
            Assert.AreEqual(updatedRole.IsActivationRequired, true);
            Assert.AreEqual(updatedRole.MessageTypeId, 2);
        }

        [TestMethod]
        public void UserAdminSetPassword()
        {
            Int32 userId;
            String newPassword;
            WebUser user;

            // Get existing user.
            UseTransaction = true;
            userId = Settings.Default.TestUserId;
            user = UserService.Data.UserManager.GetUserById(GetContext(), userId);

            // Update password and user
            newPassword = "TEST1234gsdgfsg";
            Assert.IsTrue(UserService.Data.UserManager.UserAdminSetPassword(GetContext(), user, newPassword));
        }
    }
}
