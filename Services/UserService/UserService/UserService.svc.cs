using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Data;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;

namespace UserService
{
    /// <summary>
    /// Implementation of the user web service.
    /// </summary>
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class UserService : WebServiceBase, IUserService
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static UserService()
        {
            WebServiceData.AuthorizationManager = new ArtDatabanken.WebService.Data.AuthorizationManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.UserManager = new UserManagerAdapter();
            WebServiceData.WebServiceManager = new WebServiceManager();
        }

        /// <summary>
        /// Activates role membership.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Id of role.</param>
        /// <returns>
        /// Returns 'true' if users role membership is activated,
        /// 'false' if user doesn't exist or user is not associated to the role.  
        ///</returns> 
        public Boolean ActivateRoleMembership(WebClientInformation clientInformation,
                                           Int32 roleId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.ActivateRoleMembership(context, roleId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public Boolean ActivateUserAccount(WebClientInformation clientInformation,
                                           String userName,
                                           String activationKey)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.ActivateUserAccount(context, userName, activationKey);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }


        /// <summary>
        /// Adds an authority data type to an application
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>void</returns>
        public void AddAuthorityDataTypeToApplication(WebClientInformation clientInformation,
                                                      Int32 authorityDataTypeId, 
                                                      Int32 applicationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.ApplicationManager.AddAuthorityDataTypeToApplication(context, authorityDataTypeId, applicationId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds a user to a role
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>void</returns>
        public void AddUserToRole(WebClientInformation clientInformation,
                                      Int32 roleId, Int32 userId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.UserManager.AddUserToRole(context, roleId, userId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
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
        public Boolean CheckStringIsUnique(WebClientInformation clientInformation, String value, String objectName, String propertyName)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.CheckStringIsUnique(context, value, objectName, propertyName);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new application
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="application">Information about the new application.</param>
        /// <returns>WebApplication object with the created application.</returns>
        public WebApplication CreateApplication(WebClientInformation clientInformation,
                                                WebApplication application)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.CreateApplication(context, application);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new application action.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationAction">Information about the new application action.</param>
        /// <returns>WebApplicationAction object with the created application action.</returns>
        public WebApplicationAction CreateApplicationAction(WebClientInformation clientInformation,
                                                            WebApplicationAction applicationAction)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.CreateApplicationAction(context, applicationAction);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new applicationversion
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationVersion">Information about the new application version.</param>
        /// <returns>WebApplicationVersion object with the created application version.</returns>
        public WebApplicationVersion CreateApplicationVersion(WebClientInformation clientInformation,
                                                              WebApplicationVersion applicationVersion)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.CreateApplicationVersion(context, applicationVersion);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new authority
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authority">Object representing the authority.</param>
        /// <returns>WebAuthority object with the created authority.</returns>
        public WebAuthority CreateAuthority(WebClientInformation clientInformation, WebAuthority authority)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.CreateAuthority(context, authority);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new organization.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organization">Information about the new organization.</param>
        /// <returns>WebOrganization object with the created organization.</returns>
        public WebOrganization CreateOrganization(WebClientInformation clientInformation,
                                                  WebOrganization organization)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.CreateOrganization(context, organization);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates new OrganizationCategory.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organizationCategory">Information about the new organization category.</param>
        /// <returns>WebOrganizationCategory object with the created organization category.</returns>
        public WebOrganizationCategory CreateOrganizationCategory(WebClientInformation clientInformation,
                                                                  WebOrganizationCategory organizationCategory)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.CreateOrganizationCategory(context, organizationCategory);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }


        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="person">Information about the new person.</param>
        /// <returns>WebPerson object with the created person.</returns>
        public WebPerson CreatePerson(WebClientInformation clientInformation,
                                      WebPerson person)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.CreatePerson(context, person);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="role">Object representing the role.</param>
        /// <returns>WebRole object with the created role.</returns>
        public WebRole CreateRole(WebClientInformation clientInformation,
                                  WebRole role)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.CreateRole(context, role);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="password">Password.</param>
        /// <returns>WebUser object with the created user.</returns>
        public WebUser CreateUser(WebClientInformation clientInformation,
                                  WebUser user,
                                  String password)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.CreateUser(context, user, password);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes an authority
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authority">Object representing the Authority.</param>
        /// <returns>void</returns>
        public void DeleteAuthority(WebClientInformation clientInformation,
                                    WebAuthority authority)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.UserManager.DeleteAuthority(context, authority);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes an application
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="application">Object representing the Application.</param>
        /// <returns>void</returns>
        public void DeleteApplication(WebClientInformation clientInformation,
                                       WebApplication application)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.ApplicationManager.DeleteApplication(context, application);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes an organization
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organization">Object representing the Organization.</param>
        /// <returns>void</returns>
        public void DeleteOrganization(WebClientInformation clientInformation,
                                       WebOrganization organization)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    OrganizationManager.DeleteOrganization(context, organization);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes a person
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="person">Object representing the Person.</param>
        /// <returns>void</returns>
        public void DeletePerson(WebClientInformation clientInformation,
                                 WebPerson person)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.UserManager.DeletePerson(context, person);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="role">Object representing the Role.</param>
        /// <returns>void</returns>
        public void DeleteRole(WebClientInformation clientInformation,
                               WebRole role)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.UserManager.DeleteRole(context, role);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        public void DeleteUser(WebClientInformation clientInformation,
                                 WebUser user)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.UserManager.DeleteUser(context, user);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all Address types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of Address types.</returns>
        public List<WebAddressType> GetAddressTypes(WebClientInformation clientInformation)
        {
            List<WebAddressType> addressTypes;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    addressTypes = ArtDatabanken.WebService.UserService.Data.UserManager.GetAddressTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return addressTypes;
        }

        /// <summary>
        /// GetApplication
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>
        /// WebApplication with information about an Application
        /// or NULL if the application is not found.
        /// </returns>    
        public WebApplication GetApplication(WebClientInformation clientInformation, Int32 applicationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationById(context, applicationId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// GetApplicationAction
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationActionId">ApplicationAction Id.</param>
        /// <returns>
        /// WebApplicationAction with information about an ApplicationAction
        /// or NULL if the applicationAction is not found.
        /// </returns>    
        public WebApplicationAction GetApplicationAction(WebClientInformation clientInformation, Int32 applicationActionId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationAction(context, applicationActionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// GetApplicationActionList
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>
        /// List of WebApplicationActions for an Application
        /// or NULL if there are no versions.
        /// </returns>    
        public List<WebApplicationAction> GetApplicationActions(WebClientInformation clientInformation, Int32 applicationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationActionsByApplicationId(context, applicationId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationActionGUIDs">List of application action GUIDs.</param>
        /// <returns>
        /// All application actions with id matching the list of id
        /// or NULL if there are no actions.
        /// </returns>        
        public List<WebApplicationAction> GetApplicationActionsByGUIDs(WebClientInformation clientInformation,
                                                                       List<String> applicationActionGUIDs)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationActionsByGuids(context, applicationActionGUIDs);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get list of application action objects.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationActionIds">List of application action id.</param>
        /// <returns>
        /// All application actions with id matching the list of id
        /// or NULL if there are no actions.
        /// </returns>        
        public List<WebApplicationAction> GetApplicationActionsByIds(WebClientInformation clientInformation,
                                                                     List<Int32> applicationActionIds)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationActionsByIds(context, applicationActionIds);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all applications
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>
        /// List of all WebApplications
        /// or NULL if there are no applications.
        /// </returns>        
        public List<WebApplication> GetApplications(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplications(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about all web services that constitute the SOA.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>information about all applications web services that constitute the SOA.</returns>
        public List<WebApplication> GetApplicationsInSoa(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationsInSoa(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all users of type Application.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>All users of type Application.</returns>
        public List<WebUser> GetApplicationUsers(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetApplicationUsers(context);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// GetApplicationVersion
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationVersionId">ApplicationVersion Id.</param>
        /// <returns>
        /// WebApplicationVersion with information about an ApplicationVersion
        /// or NULL if the applicationVersion is not found.
        /// </returns>    
        public WebApplicationVersion GetApplicationVersion(WebClientInformation clientInformation, Int32 applicationVersionId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationVersion(context, applicationVersionId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// GetApplicationVersionList
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>
        /// List of WebApplicationVersions for an Application
        /// or NULL if there are no versions.
        /// </returns>    
        public List<WebApplicationVersion> GetApplicationVersions(WebClientInformation clientInformation, Int32 applicationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetApplicationVersionsByApplicationId(context, applicationId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get authorities that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Authorities that matches the search criteria</returns>
        public List<WebAuthority> GetAuthoritiesBySearchCriteria(WebClientInformation clientInformation,
                                                                 WebAuthoritySearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetAuthoritiesBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get authority with specified id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authorityId">Authority Id.</param>
        /// <returns>
        /// Authority with specified id or NULL if the
        /// authority is not found.
        /// </returns>    
        public WebAuthority GetAuthority(WebClientInformation clientInformation, Int32 authorityId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetAuthority(context, authorityId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all Authority data types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of Authority data types.</returns>
        public List<WebAuthorityDataType> GetAuthorityDataTypes(WebClientInformation clientInformation)
        {
            List<WebAuthorityDataType> authorityDataTypes;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    authorityDataTypes = ArtDatabanken.WebService.UserService.Data.UserManager.GetAuthorityDataTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return authorityDataTypes;
        }

        /// <summary>
        /// Get Authority data types for specific application id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationId">Application id to get AuthorityDataTypes for.</param>
        /// <returns>A list of Authority data types.</returns>
        public List<WebAuthorityDataType> GetAuthorityDataTypesByApplicationId(WebClientInformation clientInformation, Int32 applicationId)
        {
            List<WebAuthorityDataType> authorityDataTypesByApplicationId;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    authorityDataTypesByApplicationId = ArtDatabanken.WebService.UserService.Data.ApplicationManager.GetAuthorityDataTypesByApplicationId(context, applicationId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return authorityDataTypesByApplicationId;
        }

        /// <summary>
        /// Get all Countries.
        /// </summary>
        /// <param name="clientInformation">Client information</param>
        /// <returns>All countries</returns>
        public List<WebCountry> GetCountries(WebClientInformation clientInformation)
        {
            List<WebCountry> countries;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    countries = CountryManager.GetCountries(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return countries;
        }

        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All active locales.</returns>
        public List<WebLocale> GetLocales(WebClientInformation clientInformation)
        {
            List<WebLocale> locales;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    locales = LocaleManager.GetLocales(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return locales;
        }

        /// <summary>
        /// GetOrganization
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organizationId">Organization Id.</param>
        /// <returns>
        /// WebOrganization with information about an Organization
        /// or NULL if the organization is not found.
        /// </returns>    
        public WebOrganization GetOrganization(WebClientInformation clientInformation,
                                               Int32 organizationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.GetOrganization(context, organizationId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
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
            List<WebLockedUserInformation> lockedUser;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    lockedUser = ArtDatabanken.WebService.UserService.Data.UserManager.GetLockedUserInformation(context, userNameSearchString);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return lockedUser;
        }

        /// <summary>
        /// Get all Message types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>A list of Message types.</returns>
        public List<WebMessageType> GetMessageTypes(WebClientInformation clientInformation)
        {
            List<WebMessageType> messageTypes;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    messageTypes = ArtDatabanken.WebService.UserService.Data.UserManager.GetMessageTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return messageTypes;
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
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.GetOrganizationCategory(context, organizationCategoryId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
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
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.GetOrganizationCategories(context);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all organizations. 
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>
        /// Returns list of all organizations or null if no organizations exists.
        /// </returns>
        public List<WebOrganization> GetOrganizations(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.GetOrganizations(context);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
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
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.GetOrganizationsByOrganizationCategory(context, organizationCategoryId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get organizations that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Organizations that matches the search criteria</returns>
        public List<WebOrganization> GetOrganizationsBySearchCriteria(WebClientInformation clientInformation,
                                                                      WebOrganizationSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.GetOrganizationsBySearchCriteria(context, searchCriteria);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// GetPerson
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="personId">Person Id.</param>
        /// <returns>
        /// WebPerson with information about a Person
        /// or NULL if the person is not found.
        /// </returns>    
        public WebPerson GetPerson(WebClientInformation clientInformation,
                                    Int32 personId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetPerson(context, personId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all person Genders
        /// </summary>
        /// <param name="clientInformation">Client information</param>
        /// <returns>A list of Person Genders.</returns>
        public List<WebPersonGender> GetPersonGenders(WebClientInformation clientInformation)
        {
            List<WebPersonGender> personGenders;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    personGenders = ArtDatabanken.WebService.UserService.Data.UserManager.GetPersonGenders(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return personGenders;
        }

        /// <summary>
        /// Get persons that have been modfied after and before certain dates.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="modifiedFromDate">Date after which person object have been modified.</param>
        /// <param name="modifiedUntilDate">Date before which person object have been modified.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public List<WebPerson> GetPersonsByModifiedDate(WebClientInformation clientInformation,
                                                        DateTime modifiedFromDate,
                                                        DateTime modifiedUntilDate)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetPersonsByModifiedDate(context, modifiedFromDate, modifiedUntilDate);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get persons that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public List<WebPerson> GetPersonsBySearchCriteria(WebClientInformation clientInformation,
                                                          WebPersonSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetPersonsBySearchCriteria(context, searchCriteria);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all Phone Number types
        /// </summary>
        /// <param name="clientInformation">Client information</param>
        /// <returns>A list of phone number types</returns>
        public List<WebPhoneNumberType> GetPhoneNumberTypes(WebClientInformation clientInformation)
        {
            List<WebPhoneNumberType> phoneNumberTypes;

            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    phoneNumberTypes = ArtDatabanken.WebService.UserService.Data.UserManager.GetPhoneNumberTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }

            return phoneNumberTypes;
        }

        /// <summary>
        /// GetRole
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// WebRole with information about an Role
        /// or NULL if the Role is not found.
        /// </returns>    
        public WebRole GetRole(WebClientInformation clientInformation,
                               Int32 roleId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetRole(context, roleId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get roles related to specified organization.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>
        /// Returns list of roles or null if organizationid doesn't match or if organization has no roles.
        /// </returns>
        public List<WebRole> GetRolesByOrganization(WebClientInformation clientInformation, Int32 organizationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.GetOrganizationRoles(context, organizationId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// GetUser
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <returns>
        /// WebUser object with information about
        /// the user who uses the web service.
        /// </returns>     
        public WebUser GetUser(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetUser(context);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get user by name.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">User name.</param>
        /// <returns>User object or NULL if no user with matching userName is found.</returns>       
        public WebUser GetUserByName(WebClientInformation clientInformation,
                                     String userName)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetUserByName(context, userName);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userId">User id.</param>
        /// <returns>User object.</returns>       
        public WebUser GetUserById(WebClientInformation clientInformation,
                                   Int32 userId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetUserById(context, userId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get users related to specified role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// Returns list of users or null if roleid doesn't match or if role has no members.
        /// </returns>    
        public List<WebUser> GetUsersByRole(WebClientInformation clientInformation, Int32 roleId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetUsersByRole(context, roleId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all users associated with a specified role that has not yet activated their role membership.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>
        /// Returns list of users or null if roleid doesn't match or if role has no members.
        /// </returns>    
        public List<WebUser> GetNonActivatedUsersByRole(WebClientInformation clientInformation, Int32 roleId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetNonActivatedUsersByRole(context, roleId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        public List<WebRole> GetRolesBySearchCriteria(WebClientInformation clientInformation,
                                                      WebRoleSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetRolesBySearchCriteria(context, searchCriteria);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get rolesmembers that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        public List<WebRoleMember> GetRoleMembersBySearchCriteria(WebClientInformation clientInformation,
                                                                  WebRoleMemberSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetRoleMembersBySearchCriteria(context, searchCriteria);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }


        /// <summary>
        /// GetUserRoles
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>
        /// Returns list of roles or null if userid doesn't match or if user has no roles.
        /// </returns>
        public List<WebRole> GetRolesByUser(WebClientInformation clientInformation,
                                          Int32 userId,
                                          String applicationIdentifier)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetRolesByUser(context, userId, applicationIdentifier);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get roles related to specified user group administration role.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="roleId">User group administration role id.</param>
        /// <returns>Roles related to specified user group administration role.</returns>
        public List<WebRole> GetRolesByUserGroupAdministrationRoleId(WebClientInformation clientInformation,
                                                              Int32 roleId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetRolesByUserGroupAdministrationRoleId(context, roleId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get roles related to specified user group administrator.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="userId">User group administrator user id.</param>
        /// <returns>Roles related to specified user group administrator.</returns>
        public List<WebRole> GetRolesByUserGroupAdministratorUserId(WebClientInformation clientInformation,
                                                             Int32 userId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetRolesByUserGroupAdministratorUserId(context, userId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get users that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Users that matches the search criteria</returns>
        public List<WebUser> GetUsersBySearchCriteria(WebClientInformation clientInformation,
                                                      WebUserSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.GetUsersBySearchCriteria(context, searchCriteria);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get web service context.
        /// This method is used to add Application Insights telemetry data from the request.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Web service context.</returns>
        private WebServiceContext GetWebServiceContext(WebClientInformation clientInformation)
        {
            RequestTelemetry telemetry;
            WebServiceContext context;
            WebUser user;

            try
            {
                context = new WebServiceContext(clientInformation);
                try
                {
                    if (context.IsNotNull() && (Configuration.InstallationType == InstallationType.Production))
                    {
                        telemetry = OperationContext.Current.GetRequestTelemetry();
                        if (telemetry.IsNotNull())
                        {
                            if (context.ClientToken.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = context.ClientToken.ApplicationIdentifier;
                                telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = context.ClientToken.ClientIpAddress;
                                telemetry.Properties[TelemetryProperty.LoginDateTime.ToString()] = context.ClientToken.CreatedDate.WebToString();
                            }

                            user = context.GetUser();
                            if (user.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.UserId.ToString()] = user.Id.WebToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Do nothing. We don't want calls to fail because of logging problems.
                }
            }
            catch (ApplicationException)
            {
                LogClientToken(clientInformation);
                throw;
            }
            catch (ArgumentException)
            {
                LogClientToken(clientInformation);
                throw;
            }

            return context;
        }

        /// <summary>
        /// Add information about client to Application Insights.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        private void LogClientToken(WebClientInformation clientInformation)
        {
            RequestTelemetry telemetry;
            WebClientToken clientToken;
            WebUser user;

            try
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    clientToken = new WebClientToken(clientInformation.Token, WebServiceData.WebServiceManager.Key);
                    if (clientToken.IsNotNull())
                    {
                        telemetry = OperationContext.Current.GetRequestTelemetry();
                        if (telemetry.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = clientToken.ApplicationIdentifier;
                            telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = clientToken.ClientIpAddress;
                            telemetry.Properties[TelemetryProperty.LoginDateTime.ToString()] = clientToken.CreatedDate.WebToString();

                            user = WebServiceData.UserManager.GetUser(clientToken.UserName);
                            if (user.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.UserId.ToString()] = user.Id.WebToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing. We don't want calls to fail because of logging problems.
            }
        }

        /// <summary>
        /// IsApplicationVersionValid
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <param name="version">Version to check if valid or not</param>
        /// <returns>Returns WebApplicationVersion object with information about
        ///          requested applicationversion.
        /// </returns>
        public WebApplicationVersion IsApplicationVersionValid(WebClientInformation clientInformation,
                                                               String applicationIdentifier,
                                                               String version)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.IsApplicationVersionValid(context, applicationIdentifier, version);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// IsExistingPerson
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="emailAddress">EmailAddress to check if person already exists or not.</param>
        /// <returns>Returns 'true' if person exists in database
        ///                  'false' if person not exists in database
        ///</returns>   
        public Boolean IsExistingPerson(WebClientInformation clientInformation, String emailAddress)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.IsExistingPerson(context, emailAddress);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// IsExistingUser
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="userName">UserName to check if already exists or not.</param>
        /// <returns>Returns 'true' if username exists in database
        ///                  'false' if username not exists in database
        ///</returns>   
        public Boolean IsExistingUser(WebClientInformation clientInformation, String userName)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.IsExistingUser(context, userName);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Removes an authority data type from an application
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authorityDataTypeId">AuthorityDataType Id.</param>
        /// <param name="applicationId">Application Id.</param>
        /// <returns>void</returns>
        public void RemoveAuthorityDataTypeFromApplication(WebClientInformation clientInformation,
                                                           Int32 authorityDataTypeId,
                                                           Int32 applicationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.ApplicationManager.RemoveAuthorityDataTypeFromApplication(context, authorityDataTypeId, applicationId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Removes a user from a role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        public void RemoveUserFromRole(WebClientInformation clientInformation,
                                       Int32 roleId,
                                       Int32 userId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ArtDatabanken.WebService.UserService.Data.UserManager.RemoveUserFromRole(context, roleId, userId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Reset user's password
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="emailAddress">Users emailaddress.</param>
        /// <returns>WebPasswordInformation object with user and password information.</returns>
        public WebPasswordInformation ResetPassword(WebClientInformation clientInformation,
                                                     String emailAddress)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.ResetPassword(context, emailAddress);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates an application.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="application">Object representing the Application.</param>
        /// <returns>WebApplication object with the updated application.</returns>
        public WebApplication UpdateApplication(WebClientInformation clientInformation, WebApplication application)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.UpdateApplication(context, application);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates an application action.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationAction">Object representing the ApplicationAction.</param>
        /// <returns>WebApplicationAction object with the updated applicationAction.</returns>
        public WebApplicationAction UpdateApplicationAction(WebClientInformation clientInformation, WebApplicationAction applicationAction)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.UpdateApplicationAction(context, applicationAction);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates an application version.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="applicationVersion">Object representing the ApplicationVersion.</param>
        /// <returns>WebApplicationVersion object with the updated applicationVersion.</returns>
        public WebApplicationVersion UpdateApplicationVersion(WebClientInformation clientInformation, WebApplicationVersion applicationVersion)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.ApplicationManager.UpdateApplicationVersion(context, applicationVersion);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates an authority.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="authority">Object representing the Authority.</param>
        /// <returns>WebAuthority object with the updated authority.</returns>
        public WebAuthority UpdateAuthority(WebClientInformation clientInformation, WebAuthority authority)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.UpdateAuthority(context, authority);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates an organization.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organization">Object representing the Organization.</param>
        /// <returns>WebOrganization object with the updated organization.</returns>
        public WebOrganization UpdateOrganization(WebClientInformation clientInformation, WebOrganization organization)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.UpdateOrganization(context, organization);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates an organization category.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="organizationCategory">Object representing the OrganizationCategory.</param>
        /// <returns>WebOrganizationCategory object with the updated organization category.</returns>
        public WebOrganizationCategory UpdateOrganizationCategory(WebClientInformation clientInformation,
                                                                  WebOrganizationCategory organizationCategory)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return OrganizationManager.UpdateOrganizationCategory(context, organizationCategory);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        /// <returns>WebUser object with the updated user.</returns>
        public WebUser UpdateUser(WebClientInformation clientInformation,
                                  WebUser user)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.UpdateUser(context, user);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }
        
        /// <summary>
        /// Updates a user and its associated person. The function can only be used by support users.
        /// Only Email and account activation can be changed when the account is inactivated.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="person">Object representing the Person.</param>        
        /// <returns>WebUser object with the updated user.</returns>
        public WebUser SupportUpdatePersonUser(
            WebClientInformation clientInformation,
            WebUser user, 
            WebPerson person)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.SupportUpdatePersonUser(context, user, person);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
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
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.UpdatePassword(context, oldPassword, newPassword);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates a person.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="person">Object representing the Person.</param>
        /// <returns>WebUser object with the updated user.</returns>
        public WebPerson UpdatePerson(WebClientInformation clientInformation,
                                      WebPerson person)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.UpdatePerson(context, person);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates a role.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="role">Object representing the role.</param>
        /// <returns>WebRole object with the updated role.</returns>
        public WebRole UpdateRole(WebClientInformation clientInformation, WebRole role)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.UpdateRole(context, role);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
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
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return ArtDatabanken.WebService.UserService.Data.UserManager.UserAdminSetPassword(context, user, newPassword);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }
    }
}
