using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.UserService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages user service requests.
    /// </summary>
    public class UserServiceProxy : WebServiceProxyBase, ITransactionProxy, IWebService
    {
        /// <summary>
        /// Information about web services in ArtDatabankenSOA.
        /// </summary>
        private readonly Hashtable _soaApplications;

        /// <summary>
        /// Create a UserServiceProxy instance.
        /// </summary>
        public UserServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a UserServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example User.ArtDatabankenSOA.se/UserService.svc.
        /// </param>
        public UserServiceProxy(String webServiceAddress)
        {
            _soaApplications = new Hashtable();
            WebServiceAddress = webServiceAddress;
            switch (Configuration.InstallationType)
            {
                case InstallationType.ArtportalenTest:
                    WebServiceComputer = WebServiceComputer.ArtportalenTest;
                    break;

                case InstallationType.LocalTest:
                    WebServiceComputer = WebServiceComputer.LocalTest;
                    break;

                case InstallationType.Production:
#if OLD_WEB_SERVICE_ADDRESS
                    WebServiceComputer = WebServiceComputer.Lampetra2;
#else
                    WebServiceComputer = WebServiceComputer.ArtDatabankenSoa;
#endif
                    break;

                case InstallationType.ServerTest:
                    WebServiceComputer = WebServiceComputer.Moneses;
                    break;

                case InstallationType.SpeciesFactTest:
                    WebServiceComputer = WebServiceComputer.SpeciesFactTest;
                    break;

                case InstallationType.SystemTest:
                    WebServiceComputer = WebServiceComputer.SystemTest;
                    break;

                case InstallationType.TwoBlueberriesTest:
                    WebServiceComputer = WebServiceComputer.TwoBlueberriesTest;
                    break;

                default:
                    throw new ApplicationException("Not handled installation type " + Configuration.InstallationType);
            }
        }

        /// <summary>
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example User.ArtDatabankenSOA.se/UserService.svc.
        /// </summary>
        public String WebServiceAddress
        { get; set; }

        /// <summary>
        /// Activates the role membership of the user. In this case thes user is the user included in the client information.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Id of the role.</param>
        /// <returns>
        /// 'true' if role membership is activated.
        /// 'false' if if user is not associated with the role.</returns>
        public Boolean ActivateRoleMembership(WebClientInformation clientInformation,
                                      Int32 roleId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.ActivateRoleMembership(clientInformation, roleId);
            }
        }

        /// <summary>
        /// Activates user account.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">UserName.</param>
        /// <param name="activationKey">Activation key.</param>
        /// <returns>
        /// 'true' if account is activated
        /// 'false' if username doesn't exists in database or activation key doesn't match
        /// </returns>   
        public Boolean ActivateUserAccount(WebClientInformation clientInformation,
                                           String userName,
                                           String activationKey)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.ActivateUserAccount(clientInformation, userName, activationKey);
            }
        }

        /// <summary>
        /// Adds an authority data type to an application
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        
        public void AddAuthorityDataTypeToApplication(WebClientInformation clientInformation, 
                                                      Int32 authorityDataTypeId,
                                                      Int32 applicationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.AddAuthorityDataTypeToApplication(clientInformation, authorityDataTypeId, applicationId);
            }
        }

        /// <summary>
        /// Adds user to a role
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        
        public void AddUserToRole(WebClientInformation clientInformation, Int32 roleId, Int32 userId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.AddUserToRole(clientInformation, roleId, userId);
            }
        }

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
        public Boolean CheckStringIsUnique(WebClientInformation clientInformation,
                                           String value,
                                           String objectName,
                                           String propertyName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CheckStringIsUnique(clientInformation, value, objectName, propertyName);
            }
        }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void ClearCache(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.ClearCache(clientInformation);
            }
        }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<IUserService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<IUserService>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void CommitTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.CommitTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Create a new application.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="application">Information about the new application.</param>
        /// <returns>Object with updated application information.</returns>
        public WebApplication CreateApplication(WebClientInformation clientInformation,
                                                WebApplication application)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateApplication(clientInformation, application);
            }
        }

        /// <summary>
        /// Create a new application action.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="applicationAction">Information about the new application action.</param>
        /// <returns>Object with updated application action information.</returns>
        public WebApplicationAction CreateApplicationAction(WebClientInformation clientInformation,
                                                            WebApplicationAction applicationAction)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateApplicationAction(clientInformation, applicationAction);
            }
        }

        /// <summary>
        /// Create a new application version.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="applicationVersion">Information about the new application version.</param>
        /// <returns>Object with updated application version information.</returns>
        public WebApplicationVersion CreateApplicationVersion(WebClientInformation clientInformation,
                                                              WebApplicationVersion applicationVersion)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateApplicationVersion(clientInformation, applicationVersion);
            }
        }

        /// <summary>
        /// Create a new authority.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="authority">Information about the new authority.</param>
        /// <returns>Object with updated authority information.</returns>
        public WebAuthority CreateAuthority(WebClientInformation clientInformation,
                                            WebAuthority authority)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateAuthority(clientInformation, authority);
            }
        }

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected override Object CreateClient()
        {
            UserServiceClient client;

            client = new UserServiceClient(GetBinding(),
                                           GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetLog", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Create a new organization.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="organization">Information about the new organization.</param>
        /// <returns>Object with updated organization information.</returns>
        public WebOrganization CreateOrganization(WebClientInformation clientInformation,
                                                  WebOrganization organization)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateOrganization(clientInformation, organization);
            }
        }

        /// <summary>
        /// Create a new OrganizationCategory.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="organizationCategory">Information about the new organization category.</param>
        /// <returns>Object with updated OrganizationCategory information.</returns>
        public WebOrganizationCategory CreateOrganizationCategory(WebClientInformation clientInformation,
                                                                  WebOrganizationCategory organizationCategory)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateOrganizationCategory(clientInformation, organizationCategory);
            }
        }

        /// <summary>
        /// Create a new person.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="person">Information about the new person.</param>
        /// <returns>Object with updated person information.</returns>
        public WebPerson CreatePerson(WebClientInformation clientInformation,
                                      WebPerson person)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreatePerson(clientInformation, person);
            }
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="role">Information about the new role.</param>
        /// <returns>Object with updated role information.</returns>
        public WebRole CreateRole(WebClientInformation clientInformation,
                                  WebRole role)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateRole(clientInformation, role);
            }
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="user">Information about the new user.</param>
        /// <param name="password">Password.</param>
        /// <returns>Object with updated user information.</returns>
        public WebUser CreateUser(WebClientInformation clientInformation,
                                  WebUser user,
                                  String password)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateUser(clientInformation, user, password);
            }
        }

        /// <summary>
        /// Delete an application.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="application">Delete this application.</param>
        public void DeleteApplication(WebClientInformation clientInformation,
                                      WebApplication application)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteApplication(clientInformation, application);
            }
        }

        /// <summary>
        /// Delete an authority.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authority">Delete this authority.</param>
        public void DeleteAuthority(WebClientInformation clientInformation,
                                    WebAuthority authority)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteAuthority(clientInformation, authority);
            }
        }

        /// <summary>
        /// Delete an organization.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organization">Delete this organization.</param>
        public void DeleteOrganization(WebClientInformation clientInformation,
                                       WebOrganization organization)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteOrganization(clientInformation, organization);
            }
        }

        /// <summary>
        /// Delete a person.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="person">Delete this person.</param>
        public void DeletePerson(WebClientInformation clientInformation,
                                 WebPerson person)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeletePerson(clientInformation, person);
            }
        }

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="role">Delete this role.</param>
        public void DeleteRole(WebClientInformation clientInformation,
                               WebRole role)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteRole(clientInformation, role);
            }
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void DeleteTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteTrace(clientInformation);
            }
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="user">Delete this user.</param>
        public void DeleteUser(WebClientInformation clientInformation,
                               WebUser user)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteUser(clientInformation, user);
            }
        }

        /// <summary>
        /// Get all address types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all address types.</returns>
        public List<WebAddressType> GetAddressTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetAddressTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All countries.</returns>
        public List<WebCountry> GetCountries(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetCountries(clientInformation);
            }
        }

        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All active locales.</returns>
        public List<WebLocale> GetLocales(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetLocales(clientInformation);
            }
        }

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
        public List<WebLockedUserInformation> GetLockedUserInformation(WebClientInformation clientInformation,
                                                                       WebStringSearchCriteria userNameSearchString)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetLockedUserInformation(clientInformation, userNameSearchString);
            }
        }

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        public List<WebLogRow> GetLog(WebClientInformation clientInformation,
                                      LogType type,
                                      String userName,
                                      Int32 rowCount)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetLog(clientInformation, type, userName, rowCount);
            }
        }

        /// <summary>
        /// Get application by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Requested application.</returns>       
        public WebApplication GetApplication(WebClientInformation clientInformation,
                                             Int32 applicationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplication(clientInformation, applicationId);
            }
        }

        /// <summary>
        /// Get ApplicationAction by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="applicationActionId">ApplicationAction id.</param>
        /// <returns>Requested ApplicationAction.</returns>       
        public WebApplicationAction GetApplicationAction(WebClientInformation clientInformation,
                                                         Int32 applicationActionId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplicationAction(clientInformation, applicationActionId);
            }
        }

        /// <summary>
        /// GetApplicationActions
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application actions or 
        /// null if applicationid doesn't match or if application has no actions.
        /// </returns>
        public List<WebApplicationAction> GetApplicationActions(WebClientInformation clientInformation,
                                                                Int32 applicationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplicationActions(clientInformation, applicationId);
            }
        }

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="applicationActionGUIDs">List of application action GUIDs</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        public List<WebApplicationAction> GetApplicationActionsByGUIDs(WebClientInformation clientInformation,
                                                                       List<String> applicationActionGUIDs)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplicationActionsByGUIDs(clientInformation, applicationActionGUIDs);
            }
        }

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="applicationActionIds">List of application action id</param>
        /// <returns>
        /// Returns list of application actions or null if no application actions are found.
        /// </returns>
        public List<WebApplicationAction> GetApplicationActionsByIds(WebClientInformation clientInformation,
                                                                     List<Int32> applicationActionIds)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplicationActionsByIds(clientInformation, applicationActionIds);
            }
        }

        /// <summary>
        /// GetApplications
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>
        /// Returns list of applications or 
        /// null if no applications exists.
        /// </returns>
        public List<WebApplication> GetApplications(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplications(clientInformation);
            }
        }

        /// <summary>
        /// Get information about all web services that constitute the SOA.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>information about all applications web services that constitute the SOA.</returns>
        public List<WebApplication> GetApplicationsInSoa(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplicationsInSoa(clientInformation);
            }
        }

        /// <summary>
        /// Get all users of type Application
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>List of all ApplicationUsers</returns>
        public List<WebUser> GetApplicationUsers(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplicationUsers(clientInformation);
            }
        }

        /// <summary>
        /// Get ApplicationVersion by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="applicationVersionId">ApplicationVersion id.</param>
        /// <returns>Requested ApplicationVersion.</returns>       
        public WebApplicationVersion GetApplicationVersion(WebClientInformation clientInformation,
                                                           Int32 applicationVersionId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplicationVersion(clientInformation, applicationVersionId);
            }
        }

        /// <summary>
        /// GetApplicationVersionList
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>
        /// Returns list of application versions or 
        /// null if applicationid doesn't match or if application has no versions.
        /// </returns>
        public List<WebApplicationVersion> GetApplicationVersions(WebClientInformation clientInformation,
                                                                  Int32 applicationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetApplicationVersions(clientInformation, applicationId);
            }
        }

        /// <summary>
        /// Get authorities that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Authorities that matches the search criteria</returns>
        public List<WebAuthority> GetAuthoritiesBySearchCriteria(WebClientInformation clientInformation,
                                                                 WebAuthoritySearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetAuthoritiesBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get authority by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="authorityId">Authority id.</param>
        /// <returns>Requested authority.</returns>       
        public WebAuthority GetAuthority(WebClientInformation clientInformation,
                                         Int32 authorityId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetAuthority(clientInformation, authorityId);
            }
        }

        /// <summary>
        /// Get all authority data types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all authority data types.</returns>
        public List<WebAuthorityDataType> GetAuthorityDataTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetAuthorityDataTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get authority data types for spectific application id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Application id to get AuthorityDataTypes for.</param>
        /// <returns>A list of authority data types.</returns>
        public List<WebAuthorityDataType> GetAuthorityDataTypesByApplicationId(WebClientInformation clientInformation, Int32 applicationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetAuthorityDataTypesByApplicationId(clientInformation, applicationId);
            }
        }

        /// <summary>
        /// Get all message types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all message types.</returns>
        public List<WebMessageType> GetMessageTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetMessageTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get all users that have been associated with a role that has not activated their role membership yet.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>Users that matches the search criteria</returns>
        public List<WebUser> GetNonActivatedUsersByRole(WebClientInformation clientInformation,
                                                        Int32 roleId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetNonActivatedUsersByRole(clientInformation, roleId);
            }
        }

        /// <summary>
        /// Get organization by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Requested organization.</returns>       
        public WebOrganization GetOrganization(WebClientInformation clientInformation,
                                               Int32 organizationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetOrganization(clientInformation, organizationId);
            }
        }

        /// <summary>
        /// GetOrganizationCategories
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>
        /// Returns list of organization types or null if no organization types are found.
        /// </returns>
        public List<WebOrganizationCategory> GetOrganizationCategories(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetOrganizationCategories(clientInformation);
            }
        }

        /// <summary>
        /// Get a specific organization category by id
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organizationCategoryId">Organization category Id.</param>
        /// <returns>
        /// WebOrganizationCategory with information about an Organization category
        /// or NULL if no categories are found.
        /// </returns> 
        public WebOrganizationCategory GetOrganizationCategory(WebClientInformation clientInformation,
                                                               Int32 organizationCategoryId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetOrganizationCategory(clientInformation, organizationCategoryId);
            }
        }

        /// <summary>
        /// GetOrganizationRoles
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>
        /// Returns list of roles or 
        /// null if organizationid doesn't match or if organization has no roles.
        /// </returns>
        public List<WebRole> GetOrganizationRoles(WebClientInformation clientInformation,
                                                  Int32 organizationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRolesByOrganization(clientInformation, organizationId);
            }
        }

        /// <summary>
        /// Get all organizations 
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>
        /// Returns list of all organizations or null if no organizations exists.
        /// </returns>
        public List<WebOrganization> GetOrganizations(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetOrganizations(clientInformation);
            }
        }

        /// <summary>
        /// Get organizations by organization category
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="organizationCategoryId">Organization category id.</param>
        /// <returns>
        /// Returns list of organizations or null if no organizations are categorized as the
        /// specified category.
        /// </returns>
        public List<WebOrganization> GetOrganizationsByOrganizationCategory(WebClientInformation clientInformation,
                                                                                   Int32 organizationCategoryId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetOrganizationsByOrganizationCategory(clientInformation, organizationCategoryId);
            }
        }

        /// <summary>
        /// Get organizations that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Organizations that matches the search criteria</returns>
        public List<WebOrganization> GetOrganizationsBySearchCriteria(WebClientInformation clientInformation,
                                                                      WebOrganizationSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetOrganizationsBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get person by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="personId">Person id.</param>
        /// <returns>Requested person.</returns>       
        public WebPerson GetPerson(WebClientInformation clientInformation,
                                   Int32 personId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetPerson(clientInformation, personId);
            }
        }

        /// <summary>
        /// Get all person genders.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all person genders.</returns>
        public List<WebPersonGender> GetPersonGenders(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetPersonGenders(clientInformation);
            }
        }

        /// <summary>
        /// Get persons that have been modified or created between certain dates.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="modifiedFromDate">Date after which person object have been modified.</param>
        /// <param name="modifiedUntilDate">Date before which person object have been modified.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public List<WebPerson> GetPersonsByModifiedDate(WebClientInformation clientInformation,
                                                        DateTime modifiedFromDate,
                                                        DateTime modifiedUntilDate)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetPersonsByModifiedDate(clientInformation, modifiedFromDate, modifiedUntilDate);
            }
        }

        /// <summary>
        /// Get persons that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public List<WebPerson> GetPersonsBySearchCriteria(WebClientInformation clientInformation,
                                                          WebPersonSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetPersonsBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get all phone number types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of all phone number types.</returns>
        public List<WebPhoneNumberType> GetPhoneNumberTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetPhoneNumberTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get role by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Requested role.</returns>       
        public WebRole GetRole(WebClientInformation clientInformation,
                               Int32 roleId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRole(clientInformation, roleId);
            }
        }

        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        public List<WebRole> GetRolesBySearchCriteria(WebClientInformation clientInformation,
                                                      WebRoleSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRolesBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get roles members that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Rolemembers that matches the search criteria</returns>
        public List<WebRoleMember> GetRoleMembersBySearchCriteria(WebClientInformation clientInformation,
                                                                  WebRoleMemberSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRoleMembersBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get roles related to specified user.
        /// If application is specified only those roles
        /// that are related to the application are returned.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>Roles related to specified user.</returns>
        public List<WebRole> GetRolesByUser(WebClientInformation clientInformation,
                                            Int32 userId,
                                            String applicationIdentifier)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRolesByUser(clientInformation, userId, applicationIdentifier);
            }
        }

        /// <summary>
        /// Get all roles where its usergroup is admnistrated by a certain role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Id of administration role</param>
        /// <returns>Roles that matches the search criteria</returns>
        public List<WebRole> GetRolesByUserGroupAdministrationRoleId(WebClientInformation clientInformation,
                                                                     Int32 roleId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRolesByUserGroupAdministrationRoleId(clientInformation, roleId);
            }
        }

        /// <summary>
        /// Get all roles where its usergroup is admnistrated by a certain user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userId">Id of administrating user</param>
        /// <returns>Roles that matches the search criteria</returns>
        public List<WebRole> GetRolesByUserGroupAdministratorUserId(WebClientInformation clientInformation,
                                                                    Int32 userId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRolesByUserGroupAdministratorUserId(clientInformation, userId);
            }
        }

        /// <summary>
        /// Get address to web service, in ArtDatabanken SOA,
        /// with specified application identifier.
        /// </summary>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>Address to web service.</returns>
        public String GetSoaWebServiceAddress(ApplicationIdentifier applicationIdentifier)
        {
            String webServiceAddress;

            lock (_soaApplications)
            {
                webServiceAddress = (String)(_soaApplications[applicationIdentifier.ToString()]);
            }

            return webServiceAddress;
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        public List<WebResourceStatus> GetStatus(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetStatus(clientInformation);
            }
        }

        /// <summary>
        /// Get currently loged in user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>
        /// WebUser object with information about
        /// the user who uses the web service.
        /// </returns>       
        public WebUser GetUser(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetUser(clientInformation);
            }
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userId">User id.</param>
        /// <returns>User object.</returns>
        public WebUser GetUser(WebClientInformation clientInformation,
                               Int32 userId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetUserById(clientInformation, userId);
            }
        }

        /// <summary>
        /// Get user by name.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">User name.</param>
        /// <returns>User object.</returns>
        public WebUser GetUser(WebClientInformation clientInformation,
                               String userName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetUserByName(clientInformation, userName);
            }
        }

        /// <summary>
        /// Get list of WebRoles for userid and applicationIdentifier
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentity">A string that identifies the application.</param>
        /// <returns>List of WebRole.</returns>
        public List<WebRole> GetUserRoles(WebClientInformation clientInformation,
                                          Int32 userId,
                                          String applicationIdentity)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRolesByUser(clientInformation, userId, applicationIdentity);
            }
        }

        /// <summary>
        /// Get all users that have a certain role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>Users that matches the search criteria</returns>
        public List<WebUser> GetUsersByRole(WebClientInformation clientInformation,
                                            Int32 roleId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetUsersByRole(clientInformation, roleId);
            }
        }

        /// <summary>
        /// Get users that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Users that matches the search criteria</returns>
        public List<WebUser> GetUsersBySearchCriteria(WebClientInformation clientInformation,
                                                      WebUserSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetUsersBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get address of currently used web service.
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                switch (WebServiceComputer)
                {
                    case WebServiceComputer.ArtDatabankenSoa:
                        WebServiceAddress = Settings.Default.UserServiceArtDatabankenSoaAddress;
                        break;
                    case WebServiceComputer.ArtportalenTest:
                        WebServiceAddress = Settings.Default.UserServiceArtportalenTestAddress;
                        break;
                    case WebServiceComputer.LocalTest:
                        WebServiceAddress = Settings.Default.UserServiceLocalAddress;
                        break;
                    case WebServiceComputer.Moneses:
                        WebServiceAddress = Settings.Default.UserServiceMonesesAddress;
                        break;
                    case WebServiceComputer.SpeciesFactTest:
                        WebServiceAddress = Settings.Default.UserServiceSpeciesFactTestAddress;
                        break;
                    case WebServiceComputer.SystemTest:
                        WebServiceAddress = Settings.Default.UserServiceSystemTestAddress;
                        break;
                    case WebServiceComputer.TwoBlueberriesTest:
                        WebServiceAddress = Settings.Default.UserServiceTwoBlueberriesTestAddress;
                        break;
                    default:
                        throw new Exception("Not handled computer in web service " + GetWebServiceName() + " " +
                                            WebServiceComputer);
                }
            }

            return WebServiceAddress;
        }

        /// <summary>
        /// Test if application version is valid.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationIdentifier">Application identity.</param>
        /// <param name="version">Version to check if valid or not</param>
        /// <returns>Returns WebApplicationVersion object with information about
        ///          requested applicationversion.
        /// </returns>     
        public WebApplicationVersion IsApplicationVersionValid(WebClientInformation clientInformation,
                                                               String applicationIdentifier,
                                                               String version)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.IsApplicationVersionValid(clientInformation, applicationIdentifier, version);
            }
        }

        /// <summary>
        /// Test if a person already exists.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="emailAddress">Email address to check if person already exists or not.</param>
        /// <returns>
        /// Returns 'true' if person exists and
        /// 'false' if person does not exists.
        /// </returns>   
        public Boolean IsExistingPerson(WebClientInformation clientInformation,
                                        String emailAddress)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.IsExistingPerson(clientInformation, emailAddress);
            }
        }

        /// <summary>
        /// Test if username already exists in the database.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">UserName to check if already exists or not.</param>
        /// <returns>
        /// Returns 'true' if username exists in database and
        /// 'false' if username does not exists in database.
        /// </returns>   
        public Boolean IsExistingUser(WebClientInformation clientInformation,
                                      String userName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.IsExistingUser(clientInformation, userName);
            }
        }

        /// <summary>
        /// Load address to web services, in ArtDatabanken SOA, into cache.
        /// This method is used by web service ArtDatabankenService.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succed.
        /// </param>
        public void LoadSoaWebServiceAddresses(String userName,
                                               String password,
                                               String applicationIdentifier,
                                               Boolean isActivationRequired)
        {
            Boolean isSoaWebServiceAddressesLoaded;

            lock (_soaApplications)
            {
                isSoaWebServiceAddressesLoaded = _soaApplications.IsNotEmpty();
            }

            if (!isSoaWebServiceAddressesLoaded)
            {
                Login(userName,
                      password, 
                      applicationIdentifier,
                      isActivationRequired);
            }
        }

        /// <summary>
        /// Load address to web services, in ArtDatabanken SOA, into cache.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        private void LoadSoaWebServiceAddresses(WebClientInformation clientInformation)
        {
            List<WebApplication> webServices;

            lock (_soaApplications)
            {
                if (_soaApplications.IsEmpty())
                {
                    // Load address to all web services in ArtDatabanken SOA.
                    webServices = GetApplicationsInSoa(clientInformation);
                    foreach (WebApplication webService in webServices)
                    {
                        if (webService.URL.ToLower().StartsWith(@"http://"))
                        {
                            webService.URL = webService.URL.Substring(7);
                        }

                        if (webService.URL.ToLower().StartsWith(@"https://"))
                        {
                            webService.URL = webService.URL.Substring(8);
                        }

                        _soaApplications[webService.Identifier] = webService.URL;
                    }
                }
            }
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">The password.</param>
        /// <param name="applicationIdentifier"> Application 
        /// identifier. User authorities for this 
        /// application is included in the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succeed.
        /// </param>
        /// <returns>Web login response or null if login failed.</returns>
        public WebLoginResponse Login(String userName,
                                      String password,
                                      String applicationIdentifier,
                                      Boolean isActivationRequired)
        {
            WebClientInformation clientInformation;
            WebLoginResponse loginResponse;

            using (ClientProxy client = new ClientProxy(this, 1))
            {
                loginResponse = client.Client.Login(userName, password, applicationIdentifier, isActivationRequired);
            }

            if (loginResponse.IsNotNull())
            {
                // Cache addresses to the other SOA web services.
                clientInformation = new WebClientInformation();
                clientInformation.Token = loginResponse.Token;
                clientInformation.Locale = loginResponse.Locale;
                LoadSoaWebServiceAddresses(clientInformation);
            }

            return loginResponse;
        }

        /// <summary>
        /// Logout user from web service.
        /// </summary>
        /// <param name="clientInformation">
        /// The clientInformation.
        /// </param>
        public void Logout(WebClientInformation clientInformation)
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 1))
                {
                    client.Client.Logout(clientInformation);
                }
            }
            catch
            {
                // No need to handle errors.
                // Logout is only used to relase
                // resources in the web service.
            }
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public Boolean Ping()
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 0, 10))
                {
                    return client.Client.Ping();
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reset user's password.
        /// </summary>
        /// <param name="clientInformation">
        /// Client information.
        /// </param>
        /// <param name="emailAddress">
        /// Users email address.
        /// </param>
        /// <returns>
        /// Information about user and new password.
        /// </returns>
        public WebPasswordInformation ResetPassword(WebClientInformation clientInformation,
                                                    String emailAddress)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.ResetPassword(clientInformation, emailAddress);
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="clientInformation">
        /// Client information.
        /// </param>
        public void RefreshCache(WebClientInformation clientInformation)
        {
            lock (_soaApplications)
            {
                _soaApplications.Clear();
                LoadSoaWebServiceAddresses(clientInformation);
            }
        }

        /// <summary>
        /// Removes an authority data type from an application.
        /// </summary>
        /// <param name="clientInformation">
        /// Client information.
        /// </param>
        /// <param name="authorityDataTypeId">
        /// AuthorityDataType Id.
        /// </param>
        /// <param name="applicationId">
        /// Application Id.
        /// </param>
        public void RemoveAuthorityDataTypeFromApplication(WebClientInformation clientInformation,
                                                           Int32 authorityDataTypeId,
                                                           Int32 applicationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.RemoveAuthorityDataTypeFromApplication(clientInformation, authorityDataTypeId, applicationId);
            }
        }

        /// <summary>
        /// Removes user from a role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        public void RemoveUserFromRole(WebClientInformation clientInformation, Int32 roleId, Int32 userId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.RemoveUserFromRole(clientInformation, roleId, userId);
            }
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientInformation">
        /// The clientInformation.
        /// </param>
        public void RollbackTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.RollbackTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">
        /// ClientInformation.
        /// </param>
        /// <param name="userName">
        /// User name.
        /// </param>
        public void StartTrace(WebClientInformation clientInformation,
                               String userName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTrace(clientInformation, userName);
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public void StartTransaction(WebClientInformation clientInformation,
                                     Int32 timeout)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTransaction(clientInformation,
                                               timeout);
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void StopTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StopTrace(clientInformation);
            }
        }

        /// <summary>
        /// Update existing application.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="application">Information about the updated application.</param>
        /// <returns>Object with updated application information.</returns>
        public WebApplication UpdateApplication(WebClientInformation clientInformation,
                                                WebApplication application)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateApplication(clientInformation, application);
            }
        }

        /// <summary>
        /// Update existing ApplicationAction.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="applicationAction">Information about the updated applicationaction.</param>
        /// <returns>Object with updated ApplicationAction information.</returns>
        public WebApplicationAction UpdateApplicationAction(WebClientInformation clientInformation,
                                                            WebApplicationAction applicationAction)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateApplicationAction(clientInformation, applicationAction);
            }
        }

        /// <summary>
        /// Update existing ApplicationVersion.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="applicationVersion">Information about the updated applicationversion.</param>
        /// <returns>Object with updated ApplicationVersion information.</returns>
        public WebApplicationVersion UpdateApplicationVersion(WebClientInformation clientInformation,
                                                              WebApplicationVersion applicationVersion)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateApplicationVersion(clientInformation, applicationVersion);
            }
        }

        /// <summary>
        /// Updates an authority
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authority">Object representing the Authority.</param>
        /// <returns>WebAuthority object with the updated authority.</returns>
        public WebAuthority UpdateAuthority(WebClientInformation clientInformation,
                                            WebAuthority authority)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateAuthority(clientInformation, authority);
            }
        }

        /// <summary>
        /// Update existing organization.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="organization">Information about the updated organization.</param>
        /// <returns>Object with updated organization information.</returns>
        public WebOrganization UpdateOrganization(WebClientInformation clientInformation,
                                                  WebOrganization organization)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateOrganization(clientInformation, organization);
            }
        }

        /// <summary>
        /// Update existing organization category.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="organizationCategory">Information about the updated organization category.</param>
        /// <returns>Object with updated organization category information.</returns>
        public WebOrganizationCategory UpdateOrganizationCategory(WebClientInformation clientInformation,
                                                                  WebOrganizationCategory organizationCategory)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateOrganizationCategory(clientInformation, organizationCategory);
            }
        }

        /// <summary>
        /// Update password for logged in user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="oldPassword">Current password.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>True, if password was updated.</returns>
        public Boolean UpdatePassword(WebClientInformation clientInformation,
                                      String oldPassword,
                                      String newPassword)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdatePassword(clientInformation, oldPassword, newPassword);
            }
        }

        /// <summary>
        /// Update existing person.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="person">Information about the updated person.</param>
        /// <returns>Object with updated person information.</returns>
        public WebPerson UpdatePerson(WebClientInformation clientInformation,
                                      WebPerson person)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdatePerson(clientInformation, person);
            }
        }

        /// <summary>
        /// Update existing role.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="role">Information about the updated role.</param>
        /// <returns>Object with updated role information.</returns>
        public WebRole UpdateRole(WebClientInformation clientInformation,
                                  WebRole role)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateRole(clientInformation, role);
            }
        }

        /// <summary>
        /// Update existing user.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="user">Updated information about user.</param>
        /// <returns>Object with updated user information.</returns>
        public WebUser UpdateUser(WebClientInformation clientInformation,
                                  WebUser user)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateUser(clientInformation, user);
            }
        }

        /// <summary>
        /// Update existing user.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="user">Updated information about user.</param>
        /// <param name="person">Updated information about person.</param>
        /// <returns>Object with updated user information.</returns>
        public WebUser SupportUpdatePersonUser(
            WebClientInformation clientInformation,
            WebUser user,
            WebPerson person)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.SupportUpdatePersonUser(clientInformation, user, person);
            }
        }

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
        public Boolean UserAdminSetPassword(WebClientInformation clientInformation,
                                            WebUser user,
                                            String newPassword)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UserAdminSetPassword(clientInformation, user, newPassword);
            }
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private UserServiceClient _client;
            private readonly UserServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(UserServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (UserServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public UserServiceClient Client
            {
                get { return _client; }
            }

            /// <summary>
            /// Implementation of the IDisposable interface.
            /// Recycle the client instance.
            /// </summary>
            public void Dispose()
            {
                if ((_client.State != CommunicationState.Opened) ||
                    (!_webService.PushClient(_client, _operationTimeout)))
                {
                    // Client is not in state open or
                    // was not added to the client pool.
                    // Release resources.
                    _client.Close();
                }

                _client = null;
            }
        }
    }
}
