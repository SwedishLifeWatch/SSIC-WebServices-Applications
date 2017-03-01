using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;

namespace UserService
{
    /// <summary>
    /// Interface to the user web service.
    /// </summary>
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface IUserService
    {
        /// <summary>
        /// Activates role membership.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Id of role.</param>
        /// <returns>
        /// Returns 'true' if users role membership is activated,
        /// 'false' if user doesn't exist or user is not associated to the role.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        Boolean ActivateRoleMembership(WebClientInformation clientInformation, Int32 roleId);

        /// <summary>
        /// Activate user account.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">UserName owner of account to activate.</param>
        /// <param name="activationKey">Activation key.</param>
        /// <returns>
        /// Returns 'true' if users account is activated
        /// 'false' if user doesn't exist or activation key doesn't match.
        ///</returns>
        [OperationContract]
        [OperationTelemetry]
        Boolean ActivateUserAccount(WebClientInformation clientInformation,
                                    String userName,
                                    String activationKey);

        /// <summary>
        /// Adds an authority data type to an application
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>void</returns>
        [OperationContract]
        [OperationTelemetry]
        void AddAuthorityDataTypeToApplication(WebClientInformation clientInformation,
                                               Int32 authorityDataTypeId, 
                                               Int32 applicationId);

        /// <summary>
        /// Adds a user to a role
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>void</returns>
        [OperationContract]
        [OperationTelemetry]
        void AddUserToRole(WebClientInformation clientInformation,
                           Int32 roleId,
                           Int32 userId);

        /// <summary>
        /// Check if a translation string is unique for this object/property and locale.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="value">String value to check.</param>
        /// <param name="objectName">Name of object this string belongs to</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>Boolean - 'true' if string value is unique
        ///                  - 'false' if string value already in database
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        Boolean CheckStringIsUnique(WebClientInformation clientInformation,
                                    String value,
                                    String objectName,
                                    String propertyName);

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void ClearCache(WebClientInformation clientInformation);

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void CommitTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Creates a new application
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="application">Information about the new application.</param>
        /// <returns>WebApplication object with the created application.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplication CreateApplication(WebClientInformation clientInformation,
                                         WebApplication application);

        /// <summary>
        /// Creates a new application action.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationAction">Information about the new application action.</param>
        /// <returns>WebApplicationAction object with the created application action.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplicationAction CreateApplicationAction(WebClientInformation clientInformation,
                                                     WebApplicationAction applicationAction);

        /// <summary>
        /// Creates a new applicationversion
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationVersion">Information about the new application version.</param>
        /// <returns>WebApplicationVersion object with the created application version.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplicationVersion CreateApplicationVersion(WebClientInformation clientInformation,
                                                       WebApplicationVersion applicationVersion);

        /// <summary>
        /// Creates a new authority.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authority">Object representing the authority.</param>
        /// <returns>WebAuthority object with the created authority.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebAuthority CreateAuthority(WebClientInformation clientInformation,
                                     WebAuthority authority);

        /// <summary>
        /// Creates a new organization.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organization">Information about the new organization.</param>
        /// <returns>WebOrganization object with the created organization.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebOrganization CreateOrganization(WebClientInformation clientInformation,
                                           WebOrganization organization);

        /// <summary>
        /// Creates new OrganizationCategory.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organizationCategory">Information about the new organization category.</param>
        /// <returns>WebOrganizationCategory object with the created organization category.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebOrganizationCategory CreateOrganizationCategory(WebClientInformation clientInformation,
                                                           WebOrganizationCategory organizationCategory);

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="person">Information about the new person.</param>
        /// <returns>WebPerson object with the created person.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebPerson CreatePerson(WebClientInformation clientInformation,
                               WebPerson person);

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="role">Object representing the role.</param>
        /// <returns>WebRole object with the created role.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebRole CreateRole(WebClientInformation clientInformation,
                           WebRole role);

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="password">Password.</param>
        /// <returns>WebUser object with the created user.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebUser CreateUser(WebClientInformation clientInformation,
                           WebUser user,
                           String password);

        /// <summary>
        /// Deletes an application.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="application">Object representing the application.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeleteApplication(WebClientInformation clientInformation,
                               WebApplication application);

        /// <summary>
        /// Deletes an authority.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authority">Object representing the authority.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeleteAuthority(WebClientInformation clientInformation,
                             WebAuthority authority);

        /// <summary>
        /// Deletes an organization.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organization">Object representing the organization.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeleteOrganization(WebClientInformation clientInformation,
                                WebOrganization organization);

        /// <summary>
        /// Deletes a person.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="person">Object representing the person.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeletePerson(WebClientInformation clientInformation,
                          WebPerson person);

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="role">Object representing the role.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeleteRole(WebClientInformation clientInformation,
                        WebRole role);

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void DeleteTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        /// <returns>void</returns>
        [OperationContract]
        [OperationTelemetry]
        void DeleteUser(WebClientInformation clientInformation,
                        WebUser user);

        /// <summary>
        /// Get all address types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all address types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebAddressType> GetAddressTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get application.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>
        /// WebApplication with information about an Application
        /// or NULL if the application is not found.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplication GetApplication(WebClientInformation clientInformation,
                                      Int32 applicationId);

        /// <summary>
        /// GetApplicationAction
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationActionId">ApplicationAction Id.</param>
        /// <returns>
        /// WebApplicationAction with information about an ApplicationAction
        /// or NULL if the applicationAction is not found.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplicationAction GetApplicationAction(WebClientInformation clientInformation,
                                                  Int32 applicationActionId);

        /// <summary>
        /// Get all actions for the specified application.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>
        /// All actions for the specified application.
        /// or NULL if there are no actions.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebApplicationAction> GetApplicationActions(WebClientInformation clientInformation,
                                                         Int32 applicationId);

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationActionGUIDs">List of application action GUIDs.</param>
        /// <returns>
        /// All application actions with id matching the list of id
        /// or NULL if there are no actions.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebApplicationAction> GetApplicationActionsByGUIDs(WebClientInformation clientInformation,
                                                                List<String> applicationActionGUIDs);

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationActionIds">List of application action id.</param>
        /// <returns>
        /// All application actions with id matching the list of id
        /// or NULL if there are no actions.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebApplicationAction> GetApplicationActionsByIds(WebClientInformation clientInformation,
                                                              List<Int32> applicationActionIds);

        /// <summary>
        /// Get all applications.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>
        /// List of all WebApplications
        /// or NULL if there are no applications.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebApplication> GetApplications(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about all web services that constitute the SOA.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>information about all applications web services that constitute the SOA.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebApplication> GetApplicationsInSoa(WebClientInformation clientInformation);

        /// <summary>
        /// Get all users of type Application.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>All users of type Application.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebUser> GetApplicationUsers(WebClientInformation clientInformation);

        /// <summary>
        /// Get application version.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationVersionId">ApplicationVersion Id.</param>
        /// <returns>
        /// WebApplicationVersion with information about an ApplicationVersion
        /// or NULL if the applicationVersion is not found.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplicationVersion GetApplicationVersion(WebClientInformation clientInformation,
                                                    Int32 applicationVersionId);

        /// <summary>
        /// Get all versions for the specified application.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>
        /// All versions for the specified application
        /// or NULL if there are no versions.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebApplicationVersion> GetApplicationVersions(WebClientInformation clientInformation,
                                                           Int32 applicationId);

        /// <summary>
        /// Get authorities that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Authorities that matches the search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebAuthority> GetAuthoritiesBySearchCriteria(WebClientInformation clientInformation,
                                                          WebAuthoritySearchCriteria searchCriteria);

        /// <summary>
        /// Get authority.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authorityId">Authority Id.</param>
        /// <returns>
        /// WebAuthority with information about an Authority
        /// or NULL if the authority is not found.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebAuthority GetAuthority(WebClientInformation clientInformation,
                                  Int32 authorityId);

        /// <summary>
        /// Get all authority data types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all authority data types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebAuthorityDataType> GetAuthorityDataTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get authority data types for specific application id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Get authority data types for this application.</param>
        /// <returns>A list of authority data types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebAuthorityDataType> GetAuthorityDataTypesByApplicationId(WebClientInformation clientInformation,
                                                                        Int32 applicationId);

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All countries.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebCountry> GetCountries(WebClientInformation clientInformation);

        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All active locales.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebLocale> GetLocales(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about users that are currently locked out
        /// from ArtDatabankenSOA.
        /// Users are locked out if the fail to login a couple of times.
        /// All currently locked out users are returned if parameter
        /// userSearchString is null.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userNameSearchString">
        /// String used to search among user names.
        /// Currently only string compare operator 'Like' is supported.
        /// </param>
        /// <returns>Information about users that are currently locked out from ArtDatabankenSOA.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebLockedUserInformation> GetLockedUserInformation(WebClientInformation clientInformation,
                                                                WebStringSearchCriteria userNameSearchString);

        /// <summary>
        /// Get entries from the web service log
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        [OperationContract]
        List<WebLogRow> GetLog(WebClientInformation clientInformation,
                               LogType type,
                               String userName,
                               Int32 rowCount);

        /// <summary>
        /// Get all Message types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of Message types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebMessageType> GetMessageTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get all users associated with a specified role that has not yet activated their role membership.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// Returns list of users or null if roleid doesn't match or if role has no members.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebUser> GetNonActivatedUsersByRole(WebClientInformation clientInformation, Int32 roleId);

        /// <summary>
        /// Get Organization
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organizationId">Organization Id.</param>
        /// <returns>
        /// WebOrganization with information about an Organization
        /// or NULL if the organization is not found.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebOrganization GetOrganization(WebClientInformation clientInformation,
                                        Int32 organizationId);

        /// <summary>
        /// GetOrganizationCategories
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>
        /// Returns list of organization types or null if no organization types are found.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebOrganizationCategory> GetOrganizationCategories(WebClientInformation clientInformation);

        /// <summary>
        /// Get a specific organization category by id
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organizationCategoryId">Organization category Id.</param>
        /// <returns>
        /// WebOrganizationCategory with information about an Organization category
        /// or NULL if no categories are found.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebOrganizationCategory GetOrganizationCategory(WebClientInformation clientInformation,
                                                        Int32 organizationCategoryId);

        /// <summary>
        /// Get all organizations.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>
        /// Returns list of all organizations or null if no organizations exists.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebOrganization> GetOrganizations(WebClientInformation clientInformation);

        /// <summary>
        /// Get organizations that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Organizations that matches the search criteria</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebOrganization> GetOrganizationsBySearchCriteria(WebClientInformation clientInformation,
                                                               WebOrganizationSearchCriteria searchCriteria);

        /// <summary>
        /// Get organizations by organization category.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="organizationCategoryId">Organization category id.</param>
        /// <returns>
        /// Returns list of organizations or null if no organizations are categorized as the
        /// specified category.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebOrganization> GetOrganizationsByOrganizationCategory(WebClientInformation clientInformation,
                                                                     Int32 organizationCategoryId);

        /// <summary>
        /// GetPerson
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="personId">Person Id.</param>
        /// <returns>Requested person.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebPerson GetPerson(WebClientInformation clientInformation,
                            Int32 personId);

        /// <summary>
        /// Get all person gender objects.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all person genders.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebPersonGender> GetPersonGenders(WebClientInformation clientInformation);

        /// <summary>
        /// Get persons that have been modfied after and before certain dates.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="modifiedFromDate">Date after which person object have been modified.</param>
        /// <param name="modifiedUntilDate">Date before which person object have been modified.</param>
        /// <returns>Persons that matches the search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebPerson> GetPersonsByModifiedDate(WebClientInformation clientInformation,
                                                 DateTime modifiedFromDate,
                                                 DateTime modifiedUntilDate);

        /// <summary>
        /// Get persons that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Persons that matches the search criteria</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebPerson> GetPersonsBySearchCriteria(WebClientInformation clientInformation,
                                                   WebPersonSearchCriteria searchCriteria);

        /// <summary>
        /// Get all phone number types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all phone number types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebPhoneNumberType> GetPhoneNumberTypes(WebClientInformation clientInformation);

        /// <summary>
        /// GetRole
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// WebRole with information about a Role
        /// or NULL if the role is not found.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebRole GetRole(WebClientInformation clientInformation,
                        Int32 roleId);

        /// <summary>
        /// Get roles related to specified organization.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>
        /// Returns list of roles or null if organizationid doesn't match or if organization has no roles.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebRole> GetRolesByOrganization(WebClientInformation clientInformation,
                                             Int32 organizationId);

        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebRole> GetRolesBySearchCriteria(WebClientInformation clientInformation,
                                               WebRoleSearchCriteria searchCriteria);

        /// <summary>
        /// Get roles members that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebRoleMember> GetRoleMembersBySearchCriteria(WebClientInformation clientInformation,
                                                            WebRoleMemberSearchCriteria searchCriteria);

        /// <summary>
        /// Get roles related to specified user.
        /// If application is specified only those roles
        /// that are related to the application are returned.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>Roles related to specified user.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebRole> GetRolesByUser(WebClientInformation clientInformation,
                                     Int32 userId,
                                     String applicationIdentifier);

        /// <summary>
        /// Get roles related to specified user group administration role.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="roleId">User group administration role id.</param>
        /// <returns>Roles related to specified user group administration role.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebRole> GetRolesByUserGroupAdministrationRoleId(WebClientInformation clientInformation,
                                                              Int32 roleId);

        /// <summary>
        /// Get roles related to specified user group administrator.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="userId">User group administrator user id.</param>
        /// <returns>Roles related to specified user group administrator.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebRole> GetRolesByUserGroupAdministratorUserId(WebClientInformation clientInformation,
                                                             Int32 userId);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>
        [OperationContract]
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Get currently logged in user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>
        /// WebUser object with information about
        /// the user who uses the web service.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebUser GetUser(WebClientInformation clientInformation);

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userId">User id.</param>
        /// <returns>User object.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebUser GetUserById(WebClientInformation clientInformation,
                            Int32 userId);

        /// <summary>
        /// Get user by name.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">User name.</param>
        /// <returns>User object.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebUser GetUserByName(WebClientInformation clientInformation,
                              String userName);

        /// <summary>
        /// Get users related to specified role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// Returns list of users or null if roleid doesn't match or if role has no members.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebUser> GetUsersByRole(WebClientInformation clientInformation,
                                     Int32 roleId);

        /// <summary>
        /// Get users that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Users that matches the search criteria</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebUser> GetUsersBySearchCriteria(WebClientInformation clientInformation,
                                               WebUserSearchCriteria searchCriteria);

        /// <summary>
        /// IsApplicationVersionValid
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <param name="version">Version to check if valid or not</param>
        /// <returns>Returns WebApplicationVersion object with information about
        ///          requested applicationversion.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplicationVersion IsApplicationVersionValid(WebClientInformation clientInformation,
                                                        String applicationIdentifier,
                                                        String version);

        /// <summary>
        /// Test if a person with specified email
        /// address already exists in the database.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="emailAddress">EmailAddress to check if person already exists or not.</param>
        /// <returns>
        /// Returns 'true' if person exists in database and
        /// 'false' if person not exists in database.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        Boolean IsExistingPerson(WebClientInformation clientInformation,
                                 String emailAddress);

        /// <summary>
        /// Test if username already exists in the database.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">UserName to check if already exists or not.</param>
        /// <returns>
        /// Returns 'true' if username exists in database and
        /// 'false' if username does not exists in database.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        Boolean IsExistingUser(WebClientInformation clientInformation,
                               String userName);

        /// <summary>
        /// UserLogin user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>
        /// Token and user authorities for the specified application
        /// or null if the login failed.
        /// </returns>
        [OperationContract]
        WebLoginResponse Login(String userName,
                               String password,
                               String applicationIdentifier,
                               Boolean isActivationRequired);

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void Logout(WebClientInformation clientInformation);

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>
        [OperationContract]
        Boolean Ping();

        /// <summary>
        /// Removes an authority data type from an application
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        [OperationContract]
        [OperationTelemetry]
        void RemoveAuthorityDataTypeFromApplication(WebClientInformation clientInformation,
                                                    Int32 authorityDataTypeId,
                                                    Int32 applicationId);

        /// <summary>
        /// Removes a user from a role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        [OperationContract]
        [OperationTelemetry]
        void RemoveUserFromRole(WebClientInformation clientInformation,
                                Int32 roleId,
                                Int32 userId);

        /// <summary>
        /// Reset user's password
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="emailAddress">Users emailaddress.</param>
        [OperationContract]
        WebPasswordInformation ResetPassword(WebClientInformation clientInformation,
                                             String emailAddress);

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void RollbackTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">User name.</param>
        [OperationContract]
        void StartTrace(WebClientInformation clientInformation,
                        String userName);

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        [OperationContract]
        void StartTransaction(WebClientInformation clientInformation,
                              Int32 timeout);

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void StopTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Updates an application.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="application">Object representing the Application.</param>
        /// <returns>WebApplication object with the updated application.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplication UpdateApplication(WebClientInformation clientInformation,
                                         WebApplication application);

        /// <summary>
        /// Updates an application action.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationAction">Object representing the ApplicationAction.</param>
        /// <returns>WebApplicationAction object with the updated applicationAction.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplicationAction UpdateApplicationAction(WebClientInformation clientInformation,
                                                     WebApplicationAction applicationAction);

        /// <summary>
        /// Updates an application version.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationVersion">Object representing the ApplicationVersion.</param>
        /// <returns>WebApplicationVersion object with the updated applicationVersion.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebApplicationVersion UpdateApplicationVersion(WebClientInformation clientInformation,
                                                       WebApplicationVersion applicationVersion);

        /// <summary>
        /// Updates an authority.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authority">Object representing the Authority.</param>
        /// <returns>WebAuthority object with the updated authority.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebAuthority UpdateAuthority(WebClientInformation clientInformation,
                                     WebAuthority authority);

        /// <summary>
        /// Updates an organization.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organization">Object representing the Organization.</param>
        /// <returns>WebOrganization object with the updated organization.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebOrganization UpdateOrganization(WebClientInformation clientInformation,
                                           WebOrganization organization);

        /// <summary>
        /// Updates an organization category.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organizationCategory">Object representing the OrganizationCategory.</param>
        /// <returns>WebOrganizationCategory object with the updated organization category.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebOrganizationCategory UpdateOrganizationCategory(WebClientInformation clientInformation,
                                                           WebOrganizationCategory organizationCategory);

        /// <summary>
        /// Update password for logged in user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="oldPassword">Current password.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>True, if password was updated.</returns>
        [OperationContract]
        Boolean UpdatePassword(WebClientInformation clientInformation,
                               String oldPassword,
                               String newPassword);

        /// <summary>
        /// Updates a person.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="person">Object representing the person.</param>
        /// <returns>WebPerson object with the updated person.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebPerson UpdatePerson(WebClientInformation clientInformation,
                               WebPerson person);

        /// <summary>
        /// Updates a role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="role">Object representing the role.</param>
        /// <returns>WebRole object with the updated role.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebRole UpdateRole(WebClientInformation clientInformation,
                           WebRole role);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        /// <returns>WebUser object with the updated user.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebUser UpdateUser(WebClientInformation clientInformation,
                           WebUser user);

        /// <summary>
        /// Updates a user and its associated person. The function can only be used by support users.
        /// Only Email and account activation can be changed when the account is inactivated.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="person">Object representing the Person.</param>        
        /// <returns>WebUser object with the updated user.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebUser SupportUpdatePersonUser(
            WebClientInformation clientInformation,
            WebUser user,
            WebPerson person);

        /// <summary>
        /// Updates a users password without sending the old password.
        /// Used by administrator.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="newPassword">New password</param>
        /// <returns>true - if users password is changed
        /// false - if password change failed
        /// </returns>
        [OperationContract]
        Boolean UserAdminSetPassword(WebClientInformation clientInformation,
                                     WebUser user,
                                     String newPassword);
    }
}
