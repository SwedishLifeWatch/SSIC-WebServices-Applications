using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles user related information.
    /// </summary>
    public class UserManager : IUserManager
    {
        /// <summary>             
        /// Event handling after user has logged in.
        /// </summary>
        public event UserLoggedInEventHandler UserLoggedInEvent = null;

        /// <summary>
        /// Event handling after user has logged out.
        /// </summary>
        public event UserLoggedOutEventHandler UserLoggedOutEvent = null;

        /// <summary>
        /// This property is used to retrieve or update information.
        /// </summary>
        public IUserDataSource DataSource
        { get; set; }

        /// <summary>
        /// Activates role membership for an user. 
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Id of the role.</param>
        /// <returns>
        /// Returns 'true' if users role membership is activated,
        /// 'false' if user doesn't exist or user is not associated to the role. 
        /// </returns>
        public virtual Boolean ActivateRoleMembership(IUserContext userContext,
                                                      Int32 roleId)
        {
            return DataSource.ActivateRoleMembership(userContext,
                                                     roleId);
        }

        /// <summary>
        /// Activates user account.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">UserName.</param>
        /// <param name="activationKey">Activation key.</param>
        /// <returns>
        /// 'true' if account is activated
        /// 'false' if username doesn't exists in database or activation key doesn't match
        /// </returns>   
        public virtual Boolean ActivateUserAccount(IUserContext userContext,
                                                   String userName,
                                                   String activationKey)
        {
            return DataSource.ActivateUserAccount(userContext,
                                                  userName,
                                                  activationKey);
        }

        /// <summary>
        /// Adds user to a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">Organization Id.</param>
        
        public virtual void AddUserToRole(IUserContext userContext,
                                          Int32 roleId,
                                          Int32 userId)
        {
            DataSource.AddUserToRole(userContext, roleId, userId);
        }

        /// <summary>
        /// Check if application action identifier exists in users
        /// current authorities
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionIdentifier">The application action identifier to search for.</param>
        /// <returns>Boolean - true if applicationActionIdentifier exists
        ///                  - false if it doesn't
        /// </returns>
        public Boolean ApplicationActionExists(IUserContext userContext, String applicationActionIdentifier)
        {
            return DataSource.ApplicationActionExists(userContext, applicationActionIdentifier);
        }

        /// <summary>
        /// Check if application action identifier exists in the authorities for a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role with the authorities that are checked.</param>
        /// <param name="applicationActionIdentifier">The application action identifier to search for.</param>
        /// <returns>Boolean - true if applicationActionIdentifier exists
        ///                  - false if it doesn't
        /// </returns>
        public Boolean ApplicationActionExists(IUserContext userContext,
                                               IRole role,
                                               String applicationActionIdentifier)
        {
             return DataSource.ApplicationActionExists(userContext, role, applicationActionIdentifier);
        }

        /// <summary>
        /// Check if a translation string is unique for this object/property and locale.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="value">String value to check.</param>
        /// <param name="objectName">Name of object this string belongs to</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Boolean - 'true' if string value is unique
        ///                  - 'false' if string value already in database
        /// </returns>
        public virtual Boolean CheckStringIsUnique(IUserContext userContext,
                                                   String value,
                                                   String objectName,
                                                   String propertyName)
        {
            return DataSource.CheckStringIsUnique(userContext, value, objectName, propertyName);
        }

        /// <summary>
        /// Create new authority.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">
        /// Information about the new authority.
        /// This object is updated with information 
        /// about the created authority.
        /// </param>
        public virtual void CreateAuthority(IUserContext userContext,
                                            IAuthority authority)
        {
            DataSource.CreateAuthority(userContext, authority);
        }


        /// <summary>
        /// Create new person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">
        /// Information about the new person.
        /// This object is updated with information 
        /// about the created person.
        /// </param>
        public virtual void CreatePerson(IUserContext userContext,
                                         IPerson person)
        {
            DataSource.CreatePerson(userContext, person);
        }

        /// <summary>
        /// Create new role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">
        /// Information about the new role.
        /// This object is updated with information 
        /// about the created role.
        /// </param>
        public virtual void CreateRole(IUserContext userContext,
                                       IRole role)
        {
            DataSource.CreateRole(userContext, role);
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">
        /// Information about the new user.
        /// This object is updated with information 
        /// about the created user.
        /// </param>
        /// <param name="password">Password.</param>
        public virtual void CreateUser(IUserContext userContext,
                                       IUser user,
                                       String password)
        {
            DataSource.CreateUser(userContext, user, password);
        }

        /// <summary>
        /// Delete an authority
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">Delete this authority.</param>
        public virtual void DeleteAuthority(IUserContext userContext,
                                            IAuthority authority)
        {
            DataSource.DeleteAuthority(userContext, authority);
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Delete this role.</param>
        public virtual void DeleteRole(IUserContext userContext,
                                       IRole role)
        {
            DataSource.DeleteRole(userContext, role);
        }

        /// <summary>
        /// Delete a person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">Delete this person.</param>
        public virtual void DeletePerson(IUserContext userContext,
                                         IPerson person)
        {
            DataSource.DeletePerson(userContext, person);
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">Delete this user.</param>
        public virtual void DeleteUser(IUserContext userContext,
                                       IUser user)
        {
            DataSource.DeleteUser(userContext, user);
        }

        /// <summary>
        /// Fire user logged in event.
        /// </summary>
        /// <param name="userContext">Information about the user that has logged in.</param>
        private void FireUserLoggedInEvent(IUserContext userContext)
        {
            if (UserLoggedInEvent.IsNotNull())
            {
                UserLoggedInEvent(userContext);
            }
        }

        /// <summary>
        /// Fire user logged out event.
        /// </summary>
        /// <param name="userContext">Information about the user that has logged out.</param>
        private void FireUserLoggedOutEvent(IUserContext userContext)
        {
            if (UserLoggedOutEvent.IsNotNull())
            {
                UserLoggedOutEvent(userContext);
            }
        }

        /// <summary>
        /// Get address type with the given id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="addressTypeId">Address type id.</param>
        /// <returns>Address type with the given id.</returns>
        public virtual IAddressType GetAddressType(IUserContext userContext,
                                                   AddressTypeId addressTypeId)
        {
            return GetAddressTypes(userContext).Get(addressTypeId);
        }

        /// <summary>
        /// Get address type with the given id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="addressTypeId">Address type id.</param>
        /// <returns>Address type with the given id.</returns>
        /// <exception cref="ArgumentException">Thrown if no address type has the requested id.</exception>
        public virtual IAddressType GetAddressType(IUserContext userContext,
                                                   Int32 addressTypeId)
        {
            return GetAddressTypes(userContext).Get(addressTypeId);
        }

        /// <summary>
        /// Get all address types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All address types.</returns>
        public virtual AddressTypeList GetAddressTypes(IUserContext userContext)
        {
            return DataSource.GetAddressTypes(userContext);
        }

        /// <summary>
        /// Get all users of type Application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>List of all ApplicationUsers</returns>
        public virtual UserList GetApplicationUsers(IUserContext userContext)
        {
            return DataSource.GetApplicationUsers(userContext);
        }

        /// <summary>
        /// Get authority by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityId">Authority id.</param>
        /// <returns>Requested authority.</returns>       
        public virtual IAuthority GetAuthority(IUserContext userContext,
                                               Int32 authorityId)
        {
            return DataSource.GetAuthority(userContext, authorityId);
        }

        /// <summary>
        /// Get all authorities within a role that is connected to specified application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role.</param>
        /// <param name="application">Application object.</param>
        /// <returns>List of requested authorities.</returns>       
        public AuthorityList GetAuthorities(IUserContext userContext, IRole role, IApplication application)
        {
            return DataSource.GetAuthorities(userContext, role, application);
        }

        /// <summary>
        /// Get all authorities within a role that is connected to specified application
        /// and having specified authority identifier.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <param name="authorityIdentifier">Authority identifier.</param>
        /// <returns>List of requested authorities.</returns>       
        public AuthorityList GetAuthorities(IUserContext userContext, IRole role, Int32 applicationId, String authorityIdentifier)
        {
            return DataSource.GetAuthorities(userContext, role, applicationId, authorityIdentifier);
        }

        /// <summary>
        /// Get all authorities for a user and authority is connected to specified application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">UserId.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>List of requested authorities.</returns>       
        public AuthorityList GetAuthorities(IUserContext userContext, Int32 userId, Int32 applicationId)
        {
            return DataSource.GetAuthorities(userContext, userId, applicationId);
        }

        /// <summary>
        /// Get authorities that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Role search criteria.</param>
        /// <returns>Authoritiesthat matches the search criteria</returns>
        public virtual AuthorityList GetAuthoritiesBySearchCriteria(IUserContext userContext,
                                                                    IAuthoritySearchCriteria searchCriteria)
        {
            return DataSource.GetAuthoritiesBySearchCriteria(userContext,
                                                             searchCriteria);
        }

        /// <summary>
        /// Get all authority data types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All authority data types.</returns>
        public virtual AuthorityDataTypeList GetAuthorityDataTypes(IUserContext userContext)
        {
            return DataSource.GetAuthorityDataTypes(userContext);
        }

        /// <summary>
        /// Get authority data types for specific application id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>All authority data types.</returns>
        public virtual AuthorityDataTypeList GetAuthorityDataTypesByApplicationId(IUserContext userContext, Int32 applicationId)
        {
            return DataSource.GetAuthorityDataTypesByApplicationId(userContext, applicationId);
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return DataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Get information about users that are currently locked out
        /// from ArtDatabankenSOA.
        /// Users are locked out if the fail to login a couple of times.
        /// All currently locked out users are returned if parameter
        /// userSearchString is null.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userNameSearchString">
        /// String used to search among user names.
        /// Currently only string compare operator 'Like' is supported.
        /// </param>
        /// <returns>Information about users that are currently locked out from ArtDatabankenSOA.</returns>
        public LockedUserInformationList GetLockedUserInformation(IUserContext userContext,
                                                                  StringSearchCriteria userNameSearchString)
        {
            return DataSource.GetLockedUserInformation(userContext, userNameSearchString);
        }

        /// <summary>
        /// Get message type with the given id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="messageTypeId">Message type id.</param>
        /// <returns>Message type with the given id.</returns>
        public virtual IMessageType GetMessageType(IUserContext userContext,
                                                   MessageTypeId messageTypeId)
        {
            return GetMessageTypes(userContext).Get(messageTypeId);
        }

        /// <summary>
        /// Get message type with the given id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="messageTypeId">Message type id.</param>
        /// <returns>Message type with the given id.</returns>
        /// <exception cref="ArgumentException">Thrown if no address type has the requested id.</exception>
        public virtual IMessageType GetMessageType(IUserContext userContext,
                                                   Int32 messageTypeId)
        {
            return GetMessageTypes(userContext).Get(messageTypeId);
        }

        /// <summary>
        /// Get all message types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All message types.</returns>
        public virtual MessageTypeList GetMessageTypes(IUserContext userContext)
        {
            return DataSource.GetMessageTypes(userContext);
        }

        /// <summary>
        /// Get all users that have been associated with a role but have not activated their role membership yet.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// Returns list of users or null if roleid doesn't match or if role has no members.
        /// </returns>
        public virtual UserList GetNonActivatedUsersByRole(IUserContext userContext,
                                                           Int32 roleId)
        {
            return DataSource.GetNonActivatedUsersByRole(userContext,
                                                         roleId);
        }

        /// <summary>
        /// Get person by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="personId">Person id.</param>
        /// <returns>Requested person.</returns>       
        public virtual IPerson GetPerson(IUserContext userContext,
                                         Int32 personId)
        {
            return DataSource.GetPerson(userContext, personId);
        }

        /// <summary>
        /// Get person gender with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='personGenderId'>Id of person gender.</param>
        /// <returns>Requested person gender.</returns>
        public virtual IPersonGender GetPersonGender(IUserContext userContext,
                                                     PersonGenderId personGenderId)
        {
            return GetPersonGenders(userContext).Get(personGenderId);
        }

        /// <summary>
        /// Get person gender with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='personGenderId'>Id of person gender.</param>
        /// <returns>Requested person gender.</returns>
        public virtual IPersonGender GetPersonGender(IUserContext userContext,
                                                     Int32 personGenderId)
        {
            return GetPersonGenders(userContext).Get(personGenderId);
        }

        /// <summary>
        /// Get all person genders.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A list with all person genders.</returns>
        public virtual PersonGenderList GetPersonGenders(IUserContext userContext)
        {
            return DataSource.GetPersonGenders(userContext);
        }

        /// <summary>
        /// Get persons that have been modified or created between certain dates.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="modifiedFromDate">Date after which person object have been modified.</param>
        /// <param name="modifiedUntilDate">Date before which person object have been modified.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public virtual PersonList GetPersonsByModifiedDate(IUserContext userContext,
                                                           DateTime modifiedFromDate, 
                                                           DateTime modifiedUntilDate)
        {
            return DataSource.GetPersonsByModifiedDate(userContext,
                                                       modifiedFromDate,
                                                       modifiedUntilDate);
        }

        /// <summary>
        /// Get persons that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public virtual PersonList GetPersonsBySearchCriteria(IUserContext userContext,
                                                             IPersonSearchCriteria searchCriteria)
        {
            return DataSource.GetPersonsBySearchCriteria(userContext,
                                                         searchCriteria);
        }

        /// <summary>
        /// Get phone number type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='phoneNumberTypeId'>Id of phone number type.</param>
        /// <returns>Requested phone number type.</returns>
        public virtual IPhoneNumberType GetPhoneNumberType(IUserContext userContext,
                                                           PhoneNumberTypeId phoneNumberTypeId)
        {
            return GetPhoneNumberTypes(userContext).Get(phoneNumberTypeId);
        }

        /// <summary>
        /// Get all phone number types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A list with all phone number types.</returns>
        public virtual PhoneNumberTypeList GetPhoneNumberTypes(IUserContext userContext)
        {
            return DataSource.GetPhoneNumberTypes(userContext);
        }

        /// <summary>
        /// Get role by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Requested role.</returns>       
        public virtual IRole GetRole(IUserContext userContext,
                                     Int32 roleId)
        {
            return DataSource.GetRole(userContext, roleId);
        }

        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Role search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        public virtual RoleList GetRolesBySearchCriteria(IUserContext userContext,
                                                         IRoleSearchCriteria searchCriteria)
        {
            return DataSource.GetRolesBySearchCriteria(userContext,
                                                       searchCriteria);
        }

        /// <summary>
        /// Get rolemembers that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Rolemember search criteria.</param>
        /// <returns>Rolemembers that matches the search criteria</returns>
        public virtual List<RoleMember> GetRoleMembersBySearchCriteria(IUserContext userContext,
                                                                       IRoleMemberSearchCriteria searchCriteria)
        {
            return DataSource.GetRoleMembersBySearchCriteria(userContext, searchCriteria);
        }

        /// <summary>
        /// Get currently active user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Currently active user.</returns>
        public virtual IUser GetUser(IUserContext userContext)
        {
            return userContext.User;
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <returns>User.</returns>
        public virtual IUser GetUser(IUserContext userContext, Int32 userId)
        {
            return DataSource.GetUser(userContext, userId);
        }

        /// <summary>
        /// Get user by username.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">Users username.</param>
        /// <returns>User.</returns>
        public virtual IUser GetUser(IUserContext userContext, String userName)
        {
            return DataSource.GetUser(userContext, userName);
        }

        /// <summary>
        /// GetUserRoles
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentifier">String that identifies an application.</param>
        /// <returns>
        /// Returns list of roles or 
        /// null if no roles match the search criteria.
        /// </returns>
        public virtual RoleList GetRolesByUser(IUserContext userContext, Int32 userId, String applicationIdentifier)
        {
            return DataSource.GetUserRoles(userContext, userId, applicationIdentifier);
        }

        /// <summary>
        /// GetRolesByUserGroupAdministrationRoleId
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Returns list of roles.</returns>
        public virtual RoleList GetRolesByUserGroupAdministrationRoleId(IUserContext userContext, Int32 roleId)
        {
            return DataSource.GetRolesByUserGroupAdministrationRoleId(userContext, roleId);
        }

        /// <summary>
        /// GetRolesByUserGroupAdministratorUserId
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Returns list of roles.</returns>
        public virtual RoleList GetRolesByUserGroupAdministratorUserId(IUserContext userContext, Int32 userId)
        {
            return DataSource.GetRolesByUserGroupAdministrationRoleId(userContext, userId);
        }

        /// <summary>
        /// Get all users that have specified role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// Returns list of users or null if roleid doesn't match or if role has no members.
        /// </returns>
        public virtual UserList GetUsersByRole(IUserContext userContext,
                                               Int32 roleId)
        {
            return DataSource.GetUsersByRole(userContext,
                                             roleId);
        }

        /// <summary>
        /// Get users that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Users that matches the search criteria</returns>
        public virtual UserList GetUsersBySearchCriteria(IUserContext userContext,
                                                         IPersonUserSearchCriteria searchCriteria)
        {
            return DataSource.GetUsersBySearchCriteria(userContext,
                                                       searchCriteria);
        }

        /// <summary>
        /// Test if a person already exists.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="emailAddress">Email address to check if person already exists or not.</param>
        /// <returns>
        /// Returns 'true' if person exists and
        /// 'false' if person does not exists.
        /// </returns>   
        public virtual Boolean IsExistingPerson(IUserContext userContext,
                                                String emailAddress)
        {
            return DataSource.IsExistingPerson(userContext,
                                               emailAddress);
        }

        /// <summary>
        /// Test if username already exists in the database.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">UserName to check if already exists or not.</param>
        /// <returns>
        /// Returns 'true' if username exists in database and
        /// 'false' if username does not exists in database.
        /// </returns>   
        public virtual Boolean IsExistingUser(IUserContext userContext,
                                              String userName)
        {
            return DataSource.IsExistingUser(userContext, userName);
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <returns>User context or null if login failed.</returns>
        public virtual IUserContext Login(String userName,
                                          String password,
                                          String applicationIdentifier)
        {
            return Login(userName, password, applicationIdentifier, false);
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succed.
        /// </param>
        /// <returns>User context or null if login failed.</returns>
        public virtual IUserContext Login(String userName,
                                          String password,
                                          String applicationIdentifier,
                                          Boolean isActivationRequired)
        {
            IUserContext userContext;

            // Loggin user.
            userContext = DataSource.Login(userName,
                                           password,
                                           applicationIdentifier,
                                           isActivationRequired);

            if (userContext.IsNotNull())
            {
                // Check that user selected locale is supported by this application.
                if (!CoreData.LocaleManager.GetUsedLocales(userContext).Contains(userContext.Locale))
                {
                    // Use default locale instead.
                    userContext.Locale = CoreData.LocaleManager.GetDefaultLocale(userContext);
                }

                FireUserLoggedInEvent(userContext);
            }

            return userContext;
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public virtual void Logout(IUserContext userContext)
        {
            DataSource.Logout(userContext);
            FireUserLoggedOutEvent(userContext);
        }

        /// <summary>
        /// Reset user's password
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="emailAddress">Users email address.</param>
        /// <returns>Information about user and new password.</returns>
        public virtual IPasswordInformation ResetPassword(IUserContext userContext,
                                                          String emailAddress)
        {
            return DataSource.ResetPassword(userContext, emailAddress);
        }

        /// <summary>
        /// Removes user from a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        public virtual void RemoveUserFromRole(IUserContext userContext,
                                               Int32 roleId,
                                               Int32 userId)
        {
            DataSource.RemoveUserFromRole(userContext, roleId, userId);
        }

        /// <summary>
        /// Update authority.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">
        /// Information about the updated authority.
        /// This object is updated with information 
        /// about the updated authority.
        /// </param>
        public virtual void UpdateAuthority(IUserContext userContext,
                                            IAuthority authority)
        {
            DataSource.UpdateAuthority(userContext, authority);
        }

        /// <summary>
        /// Update password for logged in user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="oldPassword">Current password.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>True, if password was updated.</returns>
        public Boolean UpdatePassword(IUserContext userContext,
                                      String oldPassword,
                                      String newPassword)
        {
            return DataSource.UpdatePassword(userContext, oldPassword, newPassword);
        }

        /// <summary>
        /// Update person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">
        /// Information about the updated person.
        /// This object is updated with information 
        /// about the updated person.
        /// </param>
        public virtual void UpdatePerson(IUserContext userContext,
                                         IPerson person)
        {
            DataSource.UpdatePerson(userContext, person);
        }

        /// <summary>
        /// Updates a user and its associated person. The function only be used by support users.
        /// Only Email and account activation can be changed when the account is inactivated.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">Information about the updated user.</param>
        /// <param name="person">Information about the updated person.</param>
        public virtual void SupportUpdatePersonUser(IUserContext userContext, IUser user, IPerson person)
        {
            DataSource.SupportUpdatePersonUser(userContext, user, person);
        }

        /// <summary>
        /// Update role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">
        /// Information about the updated role.
        /// This object is updated with information 
        /// about the updated role.
        /// </param>
        public virtual void UpdateRole(IUserContext userContext,
                                       IRole role)
        {
            DataSource.UpdateRole(userContext, role);
        }

        /// <summary>
        /// Update existing user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">
        /// Information about the updated user.
        /// This object is updated with information 
        /// about the updated user.
        /// </param>
        public virtual void UpdateUser(IUserContext userContext,
                                       IUser user)
        {
            DataSource.UpdateUser(userContext, user);
        }

        /// <summary>
        /// Updates a users password without sending the old password.
        /// Used by administrator.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="newPassword">New password</param>
        /// <returns>true - if users password is changed
        /// false - if password change failed
        /// </returns>    
        public virtual Boolean UserAdminSetPassword(IUserContext userContext,
                                                    IUser user,
                                                    String newPassword)
        {
            return DataSource.UserAdminSetPassword(userContext,
                                                   user,
                                                   newPassword);
        }
    }
}
