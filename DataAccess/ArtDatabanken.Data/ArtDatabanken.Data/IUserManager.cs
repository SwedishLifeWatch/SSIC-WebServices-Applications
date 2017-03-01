using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Delegate for event handling after user has logged in.
    /// </summary>
    /// <param name="userContext">Information about the user that has logged in.</param>
    public delegate void UserLoggedInEventHandler(IUserContext userContext);

    /// <summary>
    /// Delegate for event handling after user has logged out.
    /// </summary>
    /// <param name="userContext">Information about the user that has logged out.</param>
    public delegate void UserLoggedOutEventHandler(IUserContext userContext);

    /// <summary>
    /// Definition of the UserManager interface.
    /// </summary>
    public interface IUserManager : IManager
    {
        /// <summary>             
        /// Event handling after user has logged in.
        /// </summary>
        event UserLoggedInEventHandler UserLoggedInEvent;

        /// <summary>
        /// Event handling after user has logged out.
        /// </summary>
        event UserLoggedOutEventHandler UserLoggedOutEvent;

        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        IUserDataSource DataSource
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
        Boolean ActivateRoleMembership(IUserContext userContext,
                                       Int32 roleId);

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
        Boolean ActivateUserAccount(IUserContext userContext,
                                    String userName,
                                    String activationKey);

        /// <summary>
        /// Adds user to a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        void AddUserToRole(IUserContext userContext, 
                           Int32 roleId, 
                           Int32 userId);

        /// <summary>
        /// Check if application action identifier exists in users
        /// current authorities
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationActionIdentifier">The application action identifier to search for.</param>
        /// <returns>Boolean - true if applicationActionIdentifier exists
        ///                  - false if it doesn't
        /// </returns>
        Boolean ApplicationActionExists(IUserContext userContext, 
                                        String applicationActionIdentifier);

        /// <summary>
        /// Check if application action identifier exists in the authorities for a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role with the authorities that are checked.</param>
        /// <param name="applicationActionIdentifier">The application action identifier to search for.</param>
        /// <returns>Boolean - true if applicationActionIdentifier exists
        ///                  - false if it doesn't
        /// </returns>
        Boolean ApplicationActionExists(IUserContext userContext,
                                        IRole role,
                                        String applicationActionIdentifier);

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
        Boolean CheckStringIsUnique(IUserContext userContext,
                                    String value,
                                    String objectName,
                                    String propertyName);

        /// <summary>
        /// Create new authority.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">
        /// Information about the new authority.
        /// This object is updated with information 
        /// about the created authority.
        /// </param>
        void CreateAuthority(IUserContext userContext,
                             IAuthority authority);

        /// <summary>
        /// Create new person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">
        /// Information about the new person.
        /// This object is updated with information 
        /// about the created person.
        /// </param>
        void CreatePerson(IUserContext userContext,
                          IPerson person);

        /// <summary>
        /// Create new role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">
        /// Information about the new role.
        /// This object is updated with information 
        /// about the created role.
        /// </param>
        void CreateRole(IUserContext userContext,
                        IRole role);


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
        void CreateUser(IUserContext userContext,
                        IUser user,
                        String password);

        /// <summary>
        /// Delete an authority.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">Delete this authority.</param>
        void DeleteAuthority(IUserContext userContext, IAuthority authority);

        /// <summary>
        /// Delete a person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">Delete this person.</param>
        void DeletePerson(IUserContext userContext, IPerson person);

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Delete this role.</param>
        void DeleteRole(IUserContext userContext, IRole role);

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">Delete this user.</param>
        void DeleteUser(IUserContext userContext, IUser user);

        /// <summary>
        /// Get address type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='addressTypeId'>Id of address type.</param>
        /// <returns>Requested address type.</returns>
        IAddressType GetAddressType(IUserContext userContext,
                                    AddressTypeId addressTypeId);

        /// <summary>
        /// Get address type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='addressTypeId'>Id of address type.</param>
        /// <returns>Requested address type.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        IAddressType GetAddressType(IUserContext userContext,
                                    Int32 addressTypeId);

        /// <summary>
        /// Get all address types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All address types.</returns>
        AddressTypeList GetAddressTypes(IUserContext userContext);

        /// <summary>
        /// Get all users of type Application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>List of all ApplicationUsers</returns>
        UserList GetApplicationUsers(IUserContext userContext);

        /// <summary>
        /// Get authority by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityId">Authority id.</param>
        /// <returns>Requested authority.</returns>       
        IAuthority GetAuthority(IUserContext userContext, Int32 authorityId);

        /// <summary>
        /// Get all authorities within a role that is connected to specified application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role.</param>
        /// <param name="application">Application object.</param>
        /// <returns>List of requested authorities.</returns>       
        AuthorityList GetAuthorities(IUserContext userContext, IRole role, IApplication application);

        /// <summary>
        /// Get all authorities within a role that is connected to specified application
        /// and having specified authority identifier.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <param name="authorityIdentifier">Authority identifier.</param>
        /// <returns>List of requested authorities.</returns>       
        AuthorityList GetAuthorities(IUserContext userContext, IRole role, Int32 applicationId, String authorityIdentifier);

        /// <summary>
        /// Get all authorities for a user and authority is connected to specified application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">UserId.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>List of requested authorities.</returns>       
        AuthorityList GetAuthorities(IUserContext userContext, Int32 userId, Int32 applicationId);

        /// <summary>
        /// Get all authority data types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All authority data types.</returns>
        AuthorityDataTypeList GetAuthorityDataTypes(IUserContext userContext);

        /// <summary>
        /// Get authority data types for specific application Id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>All authority data types.</returns>
        AuthorityDataTypeList GetAuthorityDataTypesByApplicationId(IUserContext userContext, Int32 applicationId);

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
        LockedUserInformationList GetLockedUserInformation(IUserContext userContext,
                                                           StringSearchCriteria userNameSearchString);

        /// <summary>
        /// Get Message type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='messageTypeId'>Id of Message type.</param>
        /// <returns>Requested Message type.</returns>
        IMessageType GetMessageType(IUserContext userContext,
                                    MessageTypeId messageTypeId);

        /// <summary>
        /// Get Message type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='messageTypeId'>Id of Message type.</param>
        /// <returns>Requested Message type.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        IMessageType GetMessageType(IUserContext userContext,
                                    Int32 messageTypeId);

        /// <summary>
        /// Get all Message types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All Message types.</returns>
        MessageTypeList GetMessageTypes(IUserContext userContext);

        /// <summary>
        /// Get all users that have been associated with a role but have not activated their role membership yet.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// Returns list of users or null if roleid doesn't match or if role has no members.
        /// </returns>
        UserList GetNonActivatedUsersByRole(IUserContext userContext,
                                Int32 roleId);

        /// <summary>
        /// Get person by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="personId">Person id.</param>
        /// <returns>Requested person.</returns>       
        IPerson GetPerson(IUserContext userContext, Int32 personId);

        /// <summary>
        /// Get person gender with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='personGenderId'>Id of person gender.</param>
        /// <returns>Requested person gender.</returns>
        IPersonGender GetPersonGender(IUserContext userContext,
                                      PersonGenderId personGenderId);

        /// <summary>
        /// Get person gender with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='personGenderId'>Id of person gender.</param>
        /// <returns>Requested person gender.</returns>
        IPersonGender GetPersonGender(IUserContext userContext,
                                      Int32 personGenderId);

        /// <summary>
        /// Get all person genders.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A list of with all person genders.</returns>
        PersonGenderList GetPersonGenders(IUserContext userContext);

        /// <summary>
        /// Get persons that have been modified or created between certain dates.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="modifiedFromDate">Date after which person object have been modified.</param>
        /// <param name="modifiedUntilDate">Date before which person object have been modified.</param>
        /// <returns>Persons that matches the search criteria</returns>
        PersonList GetPersonsByModifiedDate(IUserContext userContext,
                                            DateTime modifiedFromDate, 
                                            DateTime modifiedUntilDate);

        /// <summary>
        /// Get persons that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Persons that matches the search criteria</returns>
        PersonList GetPersonsBySearchCriteria(IUserContext userContext,
                                              IPersonSearchCriteria searchCriteria);

        /// <summary>
        /// Get phone number type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name='phoneNumberTypeId'>Id of phone number type.</param>
        /// <returns>Requested phone number type.</returns>
        IPhoneNumberType GetPhoneNumberType(IUserContext userContext,
                                            PhoneNumberTypeId phoneNumberTypeId);

        /// <summary>
        /// Get all phone number types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All phone number types.</returns>
        PhoneNumberTypeList GetPhoneNumberTypes(IUserContext userContext);

        /// <summary>
        /// Get role by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Requested role.</returns>       
        IRole GetRole(IUserContext userContext, Int32 roleId);

        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Role search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        RoleList GetRolesBySearchCriteria(IUserContext userContext,
                                          IRoleSearchCriteria searchCriteria);

        /// <summary>
        /// Get rolemembers that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Rolemember search criteria.</param>
        /// <returns>Rolemembers that matches the search criteria</returns>
        List<RoleMember> GetRoleMembersBySearchCriteria(IUserContext userContext, 
                                                        IRoleMemberSearchCriteria searchCriteria);

        /// <summary>
        /// Get roles related to specified user.
        /// If application is specified only those roles
        /// that are related to the application are returned.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentifier">String that identifies an application.</param>
        /// <returns>Roles related to specified user.</returns>
        RoleList GetRolesByUser(IUserContext userContext, Int32 userId, String applicationIdentifier);

        /// <summary>
        /// GetRolesByUserGroupAdministrationRoleId
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Returns list of roles.</returns>
        RoleList GetRolesByUserGroupAdministrationRoleId(IUserContext userContext, Int32 roleId);

        /// <summary>
        /// GetRolesByUserGroupAdministratorUserId
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Returns list of roles.</returns>
        RoleList GetRolesByUserGroupAdministratorUserId(IUserContext userContext, Int32 userId);

        /// <summary>
        /// Get user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>User.</returns>
        IUser GetUser(IUserContext userContext);

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <returns>User.</returns>
        IUser GetUser(IUserContext userContext, Int32 userId);

        /// <summary>
        /// Get user by username.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">Users username.</param>
        /// <returns>User.</returns>
        IUser GetUser(IUserContext userContext, String userName);

        /// <summary>
        /// Get all users that have specified role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// Returns list of users or null if roleid doesn't match or if role has no members.
        /// </returns>
        UserList GetUsersByRole(IUserContext userContext,
                                Int32 roleId);

        /// <summary>
        /// Get users that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Users that matches the search criteria</returns>
        UserList GetUsersBySearchCriteria(IUserContext userContext,
                                          IPersonUserSearchCriteria searchCriteria);

        /// <summary>
        /// Test if a person already exists.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="emailAddress">Email address to check if person already exists or not.</param>
        /// <returns>
        /// Returns 'true' if person exists and
        /// 'false' if person does not exists.
        /// </returns>   
        Boolean IsExistingPerson(IUserContext userContext,
                                 String emailAddress);

        /// <summary>
        /// Test if username already exists in the database.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">UserName to check if already exists or not.</param>
        /// <returns>
        /// Returns 'true' if username exists in database and
        /// 'false' if username does not exists in database.
        /// </returns>   
        Boolean IsExistingUser(IUserContext userContext,
                               String userName);

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
        IUserContext Login(String userName,
                           String password,
                           String applicationIdentifier);

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
        IUserContext Login(String userName,
                           String password,
                           String applicationIdentifier,
                           Boolean isActivationRequired);

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        void Logout(IUserContext userContext);

        /// <summary>
        /// Reset user's password
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="emailAddress">Users email address.</param>
        /// <returns>Information about user and new password.</returns>
        IPasswordInformation ResetPassword(IUserContext userContext,
                                           String emailAddress);

        /// <summary>
        /// Remove user from a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        void RemoveUserFromRole(IUserContext userContext,
                                Int32 roleId,
                                Int32 userId);

        /// <summary>
        /// Update authority.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">
        /// Information about the updated authority.
        /// This object is updated with information 
        /// about the updated authority.
        /// </param>
        void UpdateAuthority(IUserContext userContext, 
                             IAuthority authority);

        /// <summary>
        /// Update password for logged in user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="oldPassword">Current password.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>True, if password was updated.</returns>
        Boolean UpdatePassword(IUserContext userContext,
                               String oldPassword,
                               String newPassword);

        /// <summary>
        /// Update person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">
        /// Information about the updated person.
        /// This object is updated with information 
        /// about the updated person.
        /// </param>
        void UpdatePerson(IUserContext userContext, IPerson person);

        /// <summary>
        /// Updates a user and its associated person. The function only be used by support users.
        /// Only Email and account activation can be changed when the account is inactivated.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">Information about the updated user.</param>
        /// <param name="person">Information about the updated person.</param>
        void SupportUpdatePersonUser(IUserContext userContext, IUser user, IPerson person);

        /// <summary>
        /// Update role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">
        /// Information about the updated role.
        /// This object is updated with information 
        /// about the updated role.
        /// </param>
        void UpdateRole(IUserContext userContext, IRole role);

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">
        /// Information about the updated user.
        /// This object is updated with information 
        /// about the updated user.
        /// </param>
        void UpdateUser(IUserContext userContext, IUser user);

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
        Boolean UserAdminSetPassword(IUserContext userContext,
                                     IUser user,
                                     String newPassword);
    }
}
