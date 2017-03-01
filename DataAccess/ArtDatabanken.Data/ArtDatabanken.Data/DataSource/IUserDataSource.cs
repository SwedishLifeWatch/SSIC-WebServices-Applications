using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// Definition of the UserDataSource interface.
    /// This interface is used to retrieve user related information.
    /// </summary>
    public interface IUserDataSource : IDataSource
    {
        /// <summary>
        /// Activates the role membership of the user. In this case thes user is the user included in the client information.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Id of the role.</param>
        /// <returns>
        /// 'true' if role membership is activated.
        /// 'false' if if user is not associated with the role.</returns>
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
        /// Check if application action identifier exists in users current authorities.
        /// Current authorities are stored in user context.
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
        /// <param name="role">Role that has the authorities that are checked.</param>
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
        /// Create new organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">
        /// Information about the new organization.
        /// This object is updated with information 
        /// about the created organization.
        /// </param>
        void CreateOrganization(IUserContext userContext,
                                IOrganization organization);

        /// <summary>
        /// Create new organization category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategory">
        /// Information about the new organization category.
        /// This object is updated with information 
        /// about the created organization category.
        /// </param>
        void CreateOrganizationCategory(IUserContext userContext,
                                        IOrganizationCategory organizationCategory);
        
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
        /// Delete an organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">Delete this organization.</param>
        void DeleteOrganization(IUserContext userContext, IOrganization organization);

        /// <summary>
        /// Delete a person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">Delete this person.</param>
        void DeletePerson(IUserContext userContext, IPerson person);

        /// <summary>
        /// Delete a role
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
        /// <param name="userId">User Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>List of requested authorities.</returns>       
        AuthorityList GetAuthorities(IUserContext userContext, Int32 userId, Int32 applicationId);

        /// <summary>
        /// Get authorities that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Authorities that matches the search criteria</returns>
        AuthorityList GetAuthoritiesBySearchCriteria(IUserContext userContext,
                                                     IAuthoritySearchCriteria searchCriteria);

        /// <summary>
        /// Get all authority data types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All authority data types.</returns>
        AuthorityDataTypeList GetAuthorityDataTypes(IUserContext userContext);

        /// <summary>
        /// Get authority data types for a specific application.
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
        /// Get all message types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All message types.</returns>
        MessageTypeList GetMessageTypes(IUserContext userContext);

        /// <summary>
        /// Get organization by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Requested organization.</returns>       
        IOrganization GetOrganization(IUserContext userContext, Int32 organizationId);

        /// <summary>
        /// Get organization category by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategoryId">Organization category id.</param>
        /// <returns>Requested organization category.</returns>       
        IOrganizationCategory GetOrganizationCategory(IUserContext userContext, Int32 organizationCategoryId);

        /// <summary>
        /// Get all OrganizationCategories
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of organization categories or null if no organization categories are found.
        /// </returns>
        OrganizationCategoryList GetOrganizationCategories(IUserContext userContext);

        /// <summary>
        /// GetOrganizationRoles
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>
        /// Returns list of roles or 
        /// null if organizationid doesn't match or if organization has no roles.
        /// </returns>
        RoleList GetOrganizationRoles(IUserContext userContext, Int32 organizationId);

        /// <summary>
        /// Get all organizations 
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of all organizations or null if no organizations exists.
        /// </returns>
        OrganizationList GetOrganizations(IUserContext userContext);

        /// <summary>
        /// Get organizations by organization category
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategoryId">Organization category id.</param>
        /// <returns>
        /// Returns list of organizations or null if no organizations are categorized as the
        /// specified category.
        /// </returns>
        OrganizationList GetOrganizationsByOrganizationCategory(IUserContext userContext,
                                                                Int32 organizationCategoryId);

        /// <summary>
        /// Get organizations that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Organizations that matches the search criteria</returns>
        OrganizationList GetOrganizationsBySearchCriteria(IUserContext userContext,
                                                          IOrganizationSearchCriteria searchCriteria);

        /// <summary>
        /// Get person by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="personId">Person id.</param>
        /// <returns>Requested person.</returns>       
        IPerson GetPerson(IUserContext userContext, Int32 personId);

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
        IRole GetRole(IUserContext userContext,
                      Int32 roleId);

        /// <summary>
        /// Get rolemembers that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        List<RoleMember> GetRoleMembersBySearchCriteria(IUserContext userContext, 
                                                        IRoleMemberSearchCriteria searchCriteria);

        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        RoleList GetRolesBySearchCriteria(IUserContext userContext,
                                          IRoleSearchCriteria searchCriteria);

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
        /// Get list of user roles
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentifier">String that identifies the application.</param>
        /// <returns>List of roles.</returns>       
        RoleList GetUserRoles(IUserContext userContext,
                           Int32 userId,
                           String applicationIdentifier);

        /// <summary>
        /// Get all roles where its usergroup is admnistrated by a certain role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Id of the administration role.</param>
        /// <returns>List of roles.</returns>
        RoleList GetRolesByUserGroupAdministrationRoleId(IUserContext userContext,
                                                         Int32 roleId);
        
        /// <summary>
        /// Get all roles where its usergroup is admnistrated by a certain user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">Id of the administrating user.</param>
        /// <returns>List of roles.</returns>
        RoleList GetRolesByUserGroupAdministratorUserId(IUserContext userContext,
                                                        Int32 userId);

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
        /// Get all users that have been associated with a role, that have not yet activated their role membership.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>Users that matches the search criteria</returns>
        UserList GetNonActivatedUsersByRole(IUserContext userContext,
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
        /// Removes user from a role
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
        /// Update organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">
        /// Information about the updated organization.
        /// This object is updated with information 
        /// about the updated organization.
        /// </param>
        void UpdateOrganization(IUserContext userContext, 
                                IOrganization organization);

        /// <summary>
        /// Update existing organization category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategory">
        /// Information about the updated organization category.
        /// This object is updated with information 
        /// about the updated organization category.
        /// </param>
        void UpdateOrganizationCategory(IUserContext userContext,
                                        IOrganizationCategory organizationCategory);

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
        void UpdatePerson(IUserContext userContext, 
                          IPerson person);

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
        void UpdateRole(IUserContext userContext,
                        IRole role);

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
