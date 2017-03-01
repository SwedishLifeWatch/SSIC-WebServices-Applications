using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnalysisPortal.Tests;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;

namespace Dyntaxa.Tests.TestModels
{
    public class UserDataSourceTestRepository:IUserDataSource
    {
        private IDataSourceInformation _dataSourceInformation = null;
        private const string serviceName = "UserServiceTestName";
        private const string serviceEndPoint = "UserServiceTestNameAdress";



        public IDataSourceInformation GetDataSourceInformation()
        {
            _dataSourceInformation = new DataSourceInformation( serviceName,
                                                                serviceEndPoint,
                                                                DataSourceType.WebService);

            //_dataSourceInformation.Address = Assembly.GetExecutingAssembly().GetApplicationName() + " " +
            //          Assembly.GetExecutingAssembly().GetApplicationVersion();
            //_dataSourceInformation.Name = Settings.Default.OnionDataSourceName;
            //_dataSourceInformation.Type = DataSourceType.Onion;
            return _dataSourceInformation;
        }

        public bool ActivateRoleMembership(IUserContext userContext, int roleId)
        {
            throw new NotImplementedException();
        }

        public bool ActivateUserAccount(IUserContext userContext, string userName, string activationKey)
        {
            throw new NotImplementedException();
        }

        public void AddUserToRole(IUserContext userContext, int roleId, int userId)
        {
            return;
        }

        public bool ApplicationActionExists(IUserContext userContext, string applicationActionIdentifier)
        {
            throw new NotImplementedException();
        }

        public bool ApplicationActionExists(IUserContext userContext, IRole role, string applicationActionIdentifier)
        {
            throw new NotImplementedException();
        }

        public bool CheckStringIsUnique(IUserContext userContext, string value, string objectName, string propertyName)
        {
            throw new NotImplementedException();
        }

        public void CreateAuthority(IUserContext userContext, IAuthority authority)
        {
            return;
        }

        public void CreateOrganization(IUserContext userContext, IOrganization organization)
        {
            throw new NotImplementedException();
        }

        public void CreateOrganizationCategory(IUserContext userContext, IOrganizationCategory organizationCategory)
        {
            throw new NotImplementedException();
        }

        public void CreatePerson(IUserContext userContext, IPerson person)
        {
            throw new NotImplementedException();
        }

        public void CreateRole(IUserContext userContext, IRole role)
        {
            return;
        }

        public void CreateUser(IUserContext userContext, IUser user, string password)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthority(IUserContext userContext, IAuthority authority)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrganization(IUserContext userContext, IOrganization organization)
        {
            throw new NotImplementedException();
        }

        public void DeletePerson(IUserContext userContext, IPerson person)
        {
            throw new NotImplementedException();
        }

        public void DeleteRole(IUserContext userContext, IRole role)
        {
            return;
        }

        public void DeleteUser(IUserContext userContext, IUser user)
        {
            throw new NotImplementedException();
        }

        public AddressTypeList GetAddressTypes(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public UserList GetApplicationUsers(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public IAuthority GetAuthority(IUserContext userContext, int authorityId)
        {
            throw new NotImplementedException();
        }

        public AuthorityList GetAuthorities(IUserContext userContext, IRole role, IApplication application)
        {
            throw new NotImplementedException();
        }

        public AuthorityList GetAuthorities(IUserContext userContext, IRole role, int applicationId, string authorityIdentifier)
        {
            throw new NotImplementedException();
        }

        public AuthorityList GetAuthorities(IUserContext userContext, int userId, int applicationId)
        {
            throw new NotImplementedException();
        }

        public AuthorityList GetAuthoritiesBySearchCriteria(IUserContext userContext, IAuthoritySearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public AuthorityDataTypeList GetAuthorityDataTypes(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public AuthorityDataTypeList GetAuthorityDataTypesByApplicationId(IUserContext userContext, int applicationId)
        {
            throw new NotImplementedException();
        }

        public LockedUserInformationList GetLockedUserInformation(IUserContext userContext, StringSearchCriteria userNameSearchString)
        {
            throw new NotImplementedException();
        }

        public MessageTypeList GetMessageTypes(IUserContext userContext)
        {
            MessageTypeList messageTypeList = new MessageTypeList();
            MessageType messageType = new MessageType(1,
                                   "TestMessageType",
                                   1,
                                   GetDataContext(userContext));
            messageTypeList.Add(messageType);
            return messageTypeList;
        }

        public IOrganization GetOrganization(IUserContext userContext, int organizationId)
        {
            throw new NotImplementedException();
        }

        public IOrganizationCategory GetOrganizationCategory(IUserContext userContext, int organizationCategoryId)
        {
            throw new NotImplementedException();
        }

        public OrganizationCategoryList GetOrganizationCategories(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public RoleList GetOrganizationRoles(IUserContext userContext, int organizationId)
        {
            throw new NotImplementedException();
        }

        public OrganizationList GetOrganizations(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public OrganizationList GetOrganizationsByOrganizationCategory(IUserContext userContext, int organizationCategoryId)
        {
            throw new NotImplementedException();
        }

        public OrganizationList GetOrganizationsBySearchCriteria(IUserContext userContext, IOrganizationSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public IPerson GetPerson(IUserContext userContext, int personId)
        {
            IPerson person = new Person(userContext);
            person.FirstName = "TestUserFirstName";
            person.LastName = "TestUserLastName";
            // Set person to user
            userContext.User.SetPerson(userContext, person);
            return person;
        }

        public PersonGenderList GetPersonGenders(IUserContext userContext)
        {
            IPersonGender gender = new PersonGender(1, "gender", 1, new DataContext(userContext));
            PersonGenderList personGenderList = new PersonGenderList();
            personGenderList.Add(gender);
            IPersonGender gender2 = new PersonGender(3, "gender2", 3, new DataContext(userContext));
            personGenderList.Add(gender2);
            return personGenderList;
        }

        public PersonList GetPersonsByModifiedDate(IUserContext userContext, DateTime modifiedFromDate, DateTime modifiedUntilDate)
        {
            throw new NotImplementedException();
        }

        public PersonList GetPersonsBySearchCriteria(IUserContext userContext, IPersonSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public PhoneNumberTypeList GetPhoneNumberTypes(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public IRole GetRole(IUserContext userContext, int roleId)
        {
            throw new NotImplementedException();
        }

        public List<RoleMember> GetRoleMembersBySearchCriteria(IUserContext userContext, IRoleMemberSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public RoleList GetRolesBySearchCriteria(IUserContext userContext, IRoleSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public IUser GetUser(IUserContext userContext, int userId)
        {
           return  GetApplicationUser(userContext);
        }

        public IUser GetUser(IUserContext userContext, string userName)
        {
            throw new NotImplementedException();
        }

        public RoleList GetUserRoles(IUserContext userContext, int userId, string applicationIdentifier)
        {
            throw new NotImplementedException();
        }

        public RoleList GetRolesByUserGroupAdministrationRoleId(IUserContext userContext, int roleId)
        {
            throw new NotImplementedException();
        }

        public RoleList GetRolesByUserGroupAdministratorUserId(IUserContext userContext, int userId)
        {
            throw new NotImplementedException();
        }

        public UserList GetUsersByRole(IUserContext userContext, int roleId)
        {
            UserList userList = new UserList();
            userList.Add(GetUser(userContext));
            if (roleId == 2)
            {
                userList.Add(GetUser(userContext, true));
            }
            return userList;
            
        }

        public UserList GetNonActivatedUsersByRole(IUserContext userContext, int roleId)
        {
            throw new NotImplementedException();
        }

        public UserList GetUsersBySearchCriteria(IUserContext userContext, IPersonUserSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public bool IsExistingPerson(IUserContext userContext, string emailAddress)
        {
            throw new NotImplementedException();
        }

        public bool IsExistingUser(IUserContext userContext, string userName)
        {
            throw new NotImplementedException();
        }

        public IUserContext Login(string userName, string password, string applicationIdentifier, bool isActivationRequired)
        {
            IUserContext userContext;
            userContext = new UserContext();
            userContext.Locale = new Locale(AnalysisPortalTestSettings.Default.SwedishLocaleId, AnalysisPortalTestSettings.Default.SwedishLocale, AnalysisPortalTestSettings.Default.SwedishNameString, AnalysisPortalTestSettings.Default.SvenskNameString, new DataContext(userContext));
            if (isActivationRequired)
            {
                userContext.User = GetApplicationUser(userContext);
               
            }
            else
            {
                userContext.User = GetUser(userContext);
            }
            return userContext;
        }

        public void Logout(IUserContext userContext)
        {
           return;
        }

        public IPasswordInformation ResetPassword(IUserContext userContext, string emailAddress)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromRole(IUserContext userContext, int roleId, int userId)
        {
            return;
        }

        public void UpdateAuthority(IUserContext userContext, IAuthority authority)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrganization(IUserContext userContext, IOrganization organization)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrganizationCategory(IUserContext userContext, IOrganizationCategory organizationCategory)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePassword(IUserContext userContext, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(IUserContext userContext, IPerson person)
        {
            throw new NotImplementedException();
        }

        public void SupportUpdatePersonUser(IUserContext userContext, IUser user, IPerson person)
        {
            throw new NotImplementedException();
        }

        public void UpdateRole(IUserContext userContext, IRole role)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(IUserContext userContext, IUser user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(IUserContext userContext, IUser user, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool UserAdminSetPassword(IUserContext userContext, IUser user, string newPassword)
        {
            throw new NotImplementedException();
        }


        public IUserContext GetUserContext()
        {
            IUserContext testUserContext = new UserContextTestReository();

            testUserContext.Locale = new Locale(AnalysisPortalTestSettings.Default.SwedishLocaleId, AnalysisPortalTestSettings.Default.SwedishLocale, AnalysisPortalTestSettings.Default.SwedishNameString, AnalysisPortalTestSettings.Default.SvenskNameString, new DataContext(testUserContext));
            // SetToken(testUser, loginResponse.Token);
            testUserContext.User = GetUser(testUserContext);
            RoleList roleList = new RoleList();
            testUserContext.CurrentRoles = roleList;
            return testUserContext;
        }
        private IUser GetNewUser()
        {
            IUser newUser;

            newUser = new User(GetUserContext());
            newUser.EmailAddress = "MyEmail@Address";
            newUser.UserName = "TestUser";//Settings.Default. + 42; 
            newUser.Type = UserType.Person;
            newUser.ValidFromDate = DateTime.Now;
            newUser.ValidToDate = newUser.ValidFromDate + new TimeSpan(Settings.Default.ValidToDateYearIncrement * 365, 0, 0, 0);
            return newUser;
        }



        public DataContext GetDataContext(IUserContext testUserContext)
        {
            // Set data
            ILocale locale = new Locale(AnalysisPortalTestSettings.Default.SwedishLocaleId, AnalysisPortalTestSettings.Default.SwedishLocale, AnalysisPortalTestSettings.Default.SwedishNameString, AnalysisPortalTestSettings.Default.SvenskNameString, new DataContext(testUserContext));

            IDataSourceInformation dataSource = new DataSourceInformation();
            DataContext dataContext = new DataContext(dataSource, locale);

            return dataContext;
        }

        public IUserContext GetUserContextForTaxonAdminstator(IUserContext userContext)
        {
            IUserContext testUser = new UserContext();
            testUser.User.Id = AnalysisPortalTestSettings.Default.TestUserId;
            testUser.CurrentRole = GetTaxonAdministratorRole("TaxonAdministatorRole", 3, userContext);
            return testUser;
        }

        public IRole GetNewRole(string userName, int roleId)
        {
            IRole newRole;

            newRole = new Role(GetUserContext());
            newRole.Name = userName;
            newRole.ShortName = userName;
            newRole.Description = @"testdescription";
            newRole.Id = roleId;
            newRole.UserAdministrationRoleId = 1;
            return newRole;
        }

        public IRole GetTaxonAdministratorRole(string userName, int roleId, IUserContext userContext)
        {
            IRole newRole;

            newRole = new Role(userContext);
            newRole.Name = userName;
            newRole.ShortName = userName;
            newRole.Description = @"testdescription";
            newRole.Id = roleId;
            newRole.UserAdministrationRoleId = 1;
            newRole.Identifier = Resources.AppSettings.Default.ApplicationIdentifier;
            return newRole;
        }

      

        public IRole GetSpeciesFactRole(string userName, int roleId, IUserContext userContext)
        {
            IRole newRole;

            newRole = new Role(userContext);
            newRole.Name = userName;
            newRole.ShortName = userName;
            newRole.Description = @"testSpeciesDescription";
            newRole.Id = roleId;
            newRole.UserAdministrationRoleId = 1;
            newRole.Identifier = Resources.AppSettings.Default.ApplicationIdentifier;
            AuthorityList autorities = new AuthorityList();
            Authority authority = new Authority(userContext);
            authority.UpdatePermission = true;
            authority.Identifier = "SpeciesFact";
            autorities.Add(authority);
            newRole.Authorities = autorities;
            return newRole;
        }

        public IUser GetUser(IUserContext testUserContext, bool multipleUsers = false)
        {
            IUser testUser;

            testUser = new User(testUserContext);
            testUser.IsAccountActivated = true;
            testUser.ApplicationId = AnalysisPortalTestSettings.Default.TestnAnalysisPortalApplcationId;
            testUser.DataContext = GetDataContext(testUserContext);
            testUser.EmailAddress = AnalysisPortalTestSettings.Default.TestUserEmail;
            testUser.GUID = AnalysisPortalTestSettings.Default.TestUserGuid;
            int id = AnalysisPortalTestSettings.Default.TestUserId;
            if(multipleUsers)
                id = id+1;
            testUser.Id = id;
            testUser.ShowEmailAddress = true;
            testUser.Type = UserType.Person;
            testUser.UpdateInformation = new UpdateInformation();
            testUser.UpdateInformation.CreatedBy = AnalysisPortalTestSettings.Default.TestUserId;
            testUser.UpdateInformation.CreatedDate = DateTime.Now;
            testUser.UpdateInformation.ModifiedBy = AnalysisPortalTestSettings.Default.TestUserId;
            testUser.UpdateInformation.ModifiedDate = DateTime.Now;
            testUser.UserName = AnalysisPortalTestSettings.Default.TestUserName;
            testUser.ValidFromDate = DateTime.Now;
            testUser.ValidToDate = new DateTime(2144, 12, 31);
            
           
            return testUser;
        }

        public IUser GetApplicationUser(IUserContext appUserContext)
        {
            IUser appUser;

            appUser = new User(appUserContext);
            appUser.IsAccountActivated = true;
            appUser.ApplicationId = AnalysisPortalTestSettings.Default.TestnAnalysisPortalApplcationId;
            appUser.DataContext = GetDataContext(appUserContext);
            appUser.EmailAddress = AnalysisPortalTestSettings.Default.TestUserEmail + "Appuser";
            appUser.GUID = AnalysisPortalTestSettings.Default.TestUserGuid + "Appuser";
            appUser.Id = AnalysisPortalTestSettings.Default.TestUserId + 1;
            appUser.ShowEmailAddress = true;
            appUser.Type = UserType.Person;
            appUser.UpdateInformation = new UpdateInformation();
            appUser.UpdateInformation.CreatedBy = AnalysisPortalTestSettings.Default.TestUserId + 1;
            appUser.UpdateInformation.CreatedDate = DateTime.Now;
            appUser.UpdateInformation.ModifiedBy = AnalysisPortalTestSettings.Default.TestUserId + 1;
            appUser.UpdateInformation.ModifiedDate = DateTime.Now;
            appUser.UserName = AnalysisPortalTestSettings.Default.TestUserName + "Appuser";
            appUser.ValidFromDate = DateTime.Now;
            appUser.ValidToDate = new DateTime(2144, 12, 31);
            
           

            
            
            return appUser;
        }
    }
}
