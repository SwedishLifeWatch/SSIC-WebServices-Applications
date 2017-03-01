using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.UserService
{
    /// <summary>
    /// Delegate for event handling after user
    /// has logged in to web service.
    /// </summary>
    /// <param name="userContext">User context.</param>
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
    public delegate void UserSOALoggedInEventHandler(IUserContext userContext,
                                                     String userName,
                                                     String password,
                                                     String applicationIdentifier,
                                                     Boolean isActivationRequired);

    /// <summary>
    /// Delegate for event handling after user
    /// has logged out from web service.
    /// </summary>
    /// <param name="userContext">User context.</param>
    public delegate void UserSOALoggedOutEventHandler(IUserContext userContext);

    /// <summary>
    /// This class is used to retrieve or update
    /// user related information.
    /// </summary>
    public class UserDataSource : UserDataSourceBase, IUserDataSource 
    {
        /// <summary>             
        /// Event handling after user has logged in.
        /// </summary>
        public event UserSOALoggedInEventHandler UserLoggedInEvent = null;

        /// <summary>
        /// Event handling after user has logged out.
        /// </summary>
        public event UserSOALoggedOutEventHandler UserLoggedOutEvent = null;

        /// <summary>
        /// Create a UserDataSource instance.
        /// </summary>
        public UserDataSource()
        {
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Activates the role membership of the user. In this case thes user is the user included in the client information.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Id of the role.</param>
        /// <returns>
        /// 'true' if role membership is activated.
        /// 'false' if if user is not associated with the role.</returns>
        public Boolean ActivateRoleMembership(IUserContext userContext,
                                              Int32 roleId)
        {
            CheckTransaction(userContext);
            return WebServiceProxy.UserService.ActivateRoleMembership(GetClientInformation(userContext),
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
        ///</returns>   
        public Boolean ActivateUserAccount(IUserContext userContext,
                                           String userName,
                                           String activationKey)
        {
            CheckTransaction(userContext);
            return WebServiceProxy.UserService.ActivateUserAccount(GetClientInformation(userContext),
                                                          userName,
                                                          activationKey);
        }

        /// <summary>
        /// Adds user to a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>void</returns>
        public void AddUserToRole(IUserContext userContext, Int32 roleId, Int32 userId)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.AddUserToRole(GetClientInformation(userContext),
                                                      roleId,
                                                      userId);
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
            Boolean bExists;
            RoleList currentRoles;
            bExists = false;
            currentRoles = userContext.CurrentRoles;

            if (currentRoles.IsNotEmpty())
            {
                foreach (Role role in currentRoles)
                {
                    if (ApplicationActionCheckExists(userContext, role, applicationActionIdentifier))
                    {
                        bExists = true;
                        break;
                    }
                }
            }
            return bExists;
        }

        /// <summary>
        /// Check if application action identifier exists in the authorities for a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role.</param>
        /// <param name="applicationActionIdentifier">The application action identifier to search for.</param>
        /// <returns>Boolean - true if applicationActionIdentifier exists
        ///                  - false if it doesn't
        /// </returns>
        public Boolean ApplicationActionExists(IUserContext userContext, IRole role, String applicationActionIdentifier)
        {
            return ApplicationActionCheckExists(userContext, role, applicationActionIdentifier);
        }

        /// <summary>
        /// Check if application action identifier exists in the authorities for a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role.</param>
        /// <param name="applicationActionIdentifier">The application action identifier to search for.</param>
        /// <returns>Boolean - true if applicationActionIdentifier exists
        ///                  - false if it doesn't
        /// </returns>
        private Boolean ApplicationActionCheckExists(IUserContext userContext, IRole role, String applicationActionIdentifier)
        {
            AuthorityList authorityList;
            ApplicationActionList applicationActionList;
            List<Int32> actionIds;

            authorityList = role.Authorities;
            if (authorityList.IsNotEmpty())
            {
                foreach (Authority authority in authorityList)
                {
                    actionIds = new List<Int32>();
                    foreach (String actionId in authority.ActionGUIDs)
                    {
                        actionIds.Add(Int32.Parse(actionId));
                    }
                    applicationActionList = authority.GetApplicationActionsByIdList(userContext, actionIds);
                    if (applicationActionList.IsNotEmpty())
                    {
                        foreach (ApplicationAction applicationAction in applicationActionList)
                        {
                            if (applicationAction.Identifier.ToLower().Equals(applicationActionIdentifier.ToLower()))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
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
        public Boolean CheckStringIsUnique(IUserContext userContext,
                                           String value,
                                           String objectName,
                                           String propertyName)
        {
            CheckTransaction(userContext);
            return WebServiceProxy.UserService.CheckStringIsUnique(GetClientInformation(userContext),
                                                          value,
                                                          objectName,
                                                          propertyName);
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
        public void CreateAuthority(IUserContext userContext,
                                    IAuthority authority)
        {
            WebAuthority webAuthority;

            CheckTransaction(userContext);
            webAuthority = WebServiceProxy.UserService.CreateAuthority(GetClientInformation(userContext),
                                                              GetAuthority(userContext, authority));
            UpdateAuthority(userContext, authority, webAuthority);
        }

        /// <summary>
        /// Create new organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">
        /// Information about the new organization.
        /// This object is updated with information 
        /// about the created organization.
        /// </param>
        public void CreateOrganization(IUserContext userContext,
                                       IOrganization organization)
        {
            WebOrganization webOrganization;

            CheckTransaction(userContext);
            webOrganization = WebServiceProxy.UserService.CreateOrganization(GetClientInformation(userContext),
                                                                    GetOrganization(userContext, organization));
            UpdateOrganization(userContext, organization, webOrganization);
        }

        /// <summary>
        /// Create new organization category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategory">
        /// Information about the new organization category.
        /// This object is updated with information 
        /// about the created organization category.
        /// </param>
        public void CreateOrganizationCategory(IUserContext userContext,
                                               IOrganizationCategory organizationCategory)
        {
            WebOrganizationCategory webOrganizationCategory;

            CheckTransaction(userContext);
            webOrganizationCategory = WebServiceProxy.UserService.CreateOrganizationCategory(GetClientInformation(userContext),
                                                                                    GetOrganizationCategory(userContext, organizationCategory));
            UpdateOrganizationCategory(userContext, organizationCategory, webOrganizationCategory);
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
        public void CreatePerson(IUserContext userContext,
                                 IPerson person)
        {
            WebPerson webPerson;

            CheckTransaction(userContext);
            webPerson = WebServiceProxy.UserService.CreatePerson(GetClientInformation(userContext),
                                                        GetPerson(userContext, person));
            UpdatePerson(userContext, person, webPerson);
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
        public void CreateRole(IUserContext userContext,
                               IRole role)
        {
            WebRole webRole;

            CheckTransaction(userContext);
            webRole = WebServiceProxy.UserService.CreateRole(GetClientInformation(userContext),
                                                    GetRole(userContext, role));
            UpdateRole(userContext, role, webRole);
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
        public void CreateUser(IUserContext userContext,
                               IUser user,
                               String password)
        {
            WebUser webUser;

            CheckTransaction(userContext);
            webUser = WebServiceProxy.UserService.CreateUser(GetClientInformation(userContext),
                                                    GetUser(userContext, user),
                                                    password);
            UpdateUser(userContext, user, webUser);
        }

        /// <summary>
        /// Delete an authority.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">Delete this authority.</param>
        public void DeleteAuthority(IUserContext userContext,
                                    IAuthority authority)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.DeleteAuthority(GetClientInformation(userContext),
                                                        GetAuthority(userContext, authority));
        }

        /// <summary>
        /// Delete an organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">Delete this organization.</param>
        public void DeleteOrganization(IUserContext userContext,
                                       IOrganization organization)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.DeleteOrganization(GetClientInformation(userContext),
                                                  GetOrganization(userContext, organization));
        }

        /// <summary>
        /// Delete a person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">Delete this person.</param>
        public void DeletePerson(IUserContext userContext,
                                 IPerson person)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.DeletePerson(GetClientInformation(userContext),
                                            GetPerson(userContext, person));
        }

        /// <summary>
        /// Delete a role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Delete this role.</param>
        public void DeleteRole(IUserContext userContext,
                               IRole role)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.DeleteRole(GetClientInformation(userContext),
                                                   GetRole(userContext, role));
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">Delete this user.</param>
        public void DeleteUser(IUserContext userContext, IUser user)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.DeleteUser(GetClientInformation(userContext),
                                          GetUser(userContext, user));
        }

        /// <summary>
        /// Fire user logged in event.
        /// </summary>
        /// <param name="userContext">User context.</param>
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
        private void FireUserLoggedInEvent(IUserContext userContext,
                                           String userName,
                                           String password,
                                           String applicationIdentifier,
                                           Boolean isActivationRequired)
        {
            if (UserLoggedInEvent.IsNotNull())
            {
                UserLoggedInEvent(userContext,
                                  userName,
                                  password,
                                  applicationIdentifier,
                                  isActivationRequired);
            }
        }

        /// <summary>
        /// Fire user logged out event.
        /// </summary>
        /// <param name="userContext">User context.</param>
        private void FireUserLoggedOutEvent(IUserContext userContext)
        {
            if (UserLoggedOutEvent.IsNotNull())
            {
                UserLoggedOutEvent(userContext);
            }
        }

        /// <summary>
        /// Get address from web address.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webAddress">Web address.</param>
        /// <returns>Address.</returns>
        private IAddress GetAddress(IUserContext userContext,
                                    WebAddress webAddress)
        {
            IAddress address;

            webAddress.Country.CheckNotNull("Country in WebAddress");

            address = new Address(userContext);
            address.City = webAddress.City;
            address.Country = GetCountry(userContext, webAddress.Country);
            address.DataContext = GetDataContext(userContext);
            address.Id = webAddress.Id;
            address.PostalAddress1 = webAddress.PostalAddress1;
            address.PostalAddress2 = webAddress.PostalAddress2;
            address.Type = GetAddressType(userContext,
                                          webAddress.Type);
            address.ZipCode = webAddress.ZipCode;
            return address;
        }

        /// <summary>
        /// Get web address from address.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="address">Address.</param>
        /// <returns>Web address.</returns>
        private WebAddress GetAddress(IUserContext userContext,
                                      IAddress address)
        {
            WebAddress webAddress;

            address.Country.CheckNotNull("Country in IAddress");

            webAddress = new WebAddress();
            webAddress.City = address.City;
            webAddress.Country = GetCountry(userContext, address.Country);
            webAddress.Id = address.Id;
            webAddress.PostalAddress1 = address.PostalAddress1;
            webAddress.PostalAddress2 = address.PostalAddress2;
            webAddress.Type = GetAddressType(userContext,
                                             address.Type);
            webAddress.ZipCode = address.ZipCode;
            return webAddress;
        }

        /// <summary>
        /// Get addresses from web addresses.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webAddresses">Web addresses.</param>
        /// <returns>Addresses.</returns>
        private AddressList GetAddresses(IUserContext userContext,
                                         List<WebAddress> webAddresses)
        {
            AddressList addresses;

            addresses = new AddressList();
            if (webAddresses.IsNotEmpty())
            {
                foreach (WebAddress webAddress in webAddresses)
                {
                    addresses.Add(GetAddress(userContext,
                                             webAddress));
                }
            }
            return addresses;
        }

        /// <summary>
        /// Get web addresses from addresses.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="addresses">Addresses.</param>
        /// <returns>Web addresses.</returns>
        private List<WebAddress> GetAddresses(IUserContext userContext,
                                              AddressList addresses)
        {
            List<WebAddress> webAddresses;

            webAddresses = null;
            if (addresses.IsNotEmpty())
            {
                webAddresses = new List<WebAddress>();
                foreach (IAddress address in addresses)
                {
                    webAddresses.Add(GetAddress(userContext, address));
                }
            }
            return webAddresses;
        }

        /// <summary>
        /// Get address type from web address type.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webAddressType">Web address type.</param>
        /// <returns>Address type.</returns>
        private IAddressType GetAddressType(IUserContext userContext,
                                            WebAddressType webAddressType)
        {
            return new AddressType(webAddressType.Id,
                                   webAddressType.Name,
                                   webAddressType.NameStringId,
                                   GetDataContext(userContext));
        }

        /// <summary>
        /// Get web address type from address type.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="addressType">Address type.</param>
        /// <returns>Web address type.</returns>
        private WebAddressType GetAddressType(IUserContext userContext,
                                              IAddressType addressType)
        {
            WebAddressType webAddressType;

            webAddressType = new WebAddressType();
            webAddressType.Id = addressType.Id;
            webAddressType.Name = addressType.Name;
            webAddressType.NameStringId = addressType.NameStringId;
            return webAddressType;
        }

        /// <summary>
        /// Get address types from web address types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webAddressTypes">Web address types.</param>
        /// <returns>Address types.</returns>
        private AddressTypeList GetAddressTypes(IUserContext userContext,
                                                List<WebAddressType> webAddressTypes)
        {
            AddressTypeList addressTypes;

            addressTypes = new AddressTypeList();
            if (webAddressTypes.IsNotEmpty())
            {
                foreach (WebAddressType webAddressType in webAddressTypes)
                {
                    addressTypes.Add(GetAddressType(userContext,
                                                    webAddressType));
                }
            }
            return addressTypes;
        }

        /// <summary>
        /// Get all address types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All address types.</returns>
        public AddressTypeList GetAddressTypes(IUserContext userContext)
        {
            List<WebAddressType> webAddressTypes;

            CheckTransaction(userContext);
            webAddressTypes = WebServiceProxy.UserService.GetAddressTypes(GetClientInformation(userContext));
            return GetAddressTypes(userContext, webAddressTypes);
        }

        /// <summary>
        /// Get all users of type Application
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>List of all ApplicationUsers</returns>
        public UserList GetApplicationUsers(IUserContext userContext)
        {
            List<WebUser> webUsers;
            CheckTransaction(userContext);
            webUsers = WebServiceProxy.UserService.GetApplicationUsers(GetClientInformation(userContext));
            return GetUsers(userContext, webUsers);
        }

        /// <summary>
        /// Get authority by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityId">Authority id.</param>
        /// <returns>Requested authority.</returns>       
        public IAuthority GetAuthority(IUserContext userContext,
                                       Int32 authorityId)
        {
            WebAuthority webAuthority;

            CheckTransaction(userContext);
            webAuthority = WebServiceProxy.UserService.GetAuthority(GetClientInformation(userContext),
                                                           authorityId);
            return GetAuthority(userContext, webAuthority);
        }

        /// <summary>
        /// Get Authority from WebAuthority.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webAuthority">A web authority.</param>
        /// <returns>Authority.</returns>
        public IAuthority GetAuthority(IUserContext userContext,
                                       WebAuthority webAuthority)
        {
            IAuthority authority;

            authority = new Authority(userContext);
            UpdateAuthority(userContext, authority, webAuthority);
            return authority;
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
            AuthorityList authorityList;
            Int32 applicationId;
            authorityList = new AuthorityList();
            applicationId = application.Id;

            if (role.Authorities.IsNotEmpty())
            {
                // Get authority data types for selected application and check if tere are any datatypes for this application (and role).
                // If so add authority to the list of authorities.
                AuthorityDataTypeList authorityDataTypeList = GetAuthorityDataTypesByApplicationId(userContext, applicationId);
                foreach (Authority authority in role.Authorities)
                {
                    if (authority.ApplicationId.Equals(applicationId))
                    {
                        authorityList.Add(authority);
                    }
                    else if((authorityDataTypeList.Count > 0) && (authority.AuthorityType.Equals(AuthorityType.DataType)) && (authority.AuthorityDataType.IsNotNull()) )
                    {
                        //Get data type
                        foreach (AuthorityDataType dataType in authorityDataTypeList)
                        {
                            if(authority.AuthorityDataType.Id == dataType.Id)
                            {
                                // Found authority data type for this application and role.
                                authorityList.Add(authority);
                                break;
                            }
                        }
                    }
                }
            }
            return authorityList;
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
            AuthorityList authorityList;
            authorityList = new AuthorityList();

            if (role.Authorities.IsNotEmpty())
            {
                // Get authority data types for selected application and check if tere are any datatypes for this application (and role).
                // If so add authority to the list of authorities.
                AuthorityDataTypeList authorityDataTypeList = GetAuthorityDataTypesByApplicationId(userContext, applicationId);
                foreach (Authority authority in role.Authorities)
                {
                    if (authority.ApplicationId.Equals(applicationId) &&
                        authority.Identifier.Equals(authorityIdentifier))
                    {
                        authorityList.Add(authority);
                    }
                    else if ((authorityDataTypeList.Count > 0) && (authority.AuthorityType.Equals(AuthorityType.DataType)) && (authority.AuthorityDataType.IsNotNull()))
                    {
                        //Get data type
                        foreach (AuthorityDataType dataType in authorityDataTypeList)
                        {
                            if (authority.AuthorityDataType.Id == dataType.Id && authority.Identifier.Equals(authorityIdentifier))
                            {
                                // Found authority data type for this application, authorityUdentifier and role.
                                authorityList.Add(authority);
                                break;
                            }
                        }
                    }
                }
            }
            return authorityList;
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
            AuthorityList authorityList;
            RoleList roleList;

            authorityList = new AuthorityList();
            roleList = GetUserRoles(userContext, userId, null);
            if (roleList.IsNotEmpty())
            {
                foreach (Role role in roleList)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        // Get authority data types for selected application and check if tere are any datatypes for this application (and role).
                        // If so add authority to the list of authorities.
                        AuthorityDataTypeList authorityDataTypeList = GetAuthorityDataTypesByApplicationId(userContext, applicationId);
                        foreach (Authority authority in role.Authorities)
                        {
                            if (authority.ApplicationId.Equals(applicationId))
                            {
                                authorityList.Add(authority);
                            }
                            else if ((authorityDataTypeList.Count > 0) && (authority.AuthorityType.Equals(AuthorityType.DataType)) && (authority.AuthorityDataType.IsNotNull()))
                            {
                                //Get data type
                                foreach (AuthorityDataType dataType in authorityDataTypeList)
                                {
                                    if (authority.AuthorityDataType.Id == dataType.Id)
                                    {
                                        // Found authority data type for this application and role.
                                        authorityList.Add(authority);
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return authorityList;
        }           
        
        /// <summary>
        /// Get all authorities from a list of web authorities
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webAuthorities">A list of web authorities</param>
        /// <returns>List of requested authorities.</returns>
        private AuthorityList GetAuthorities(IUserContext userContext,
                                             List<WebAuthority> webAuthorities)
        {
            AuthorityList authorities;

            authorities = new AuthorityList();
            if (webAuthorities.IsNotEmpty())
            {
                foreach (WebAuthority webAuthority in webAuthorities)
                {
                    authorities.Add(GetAuthority(userContext, webAuthority));
                }
            }
            return authorities;
        }


        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        public AuthorityList GetAuthoritiesBySearchCriteria(IUserContext userContext,
                                                            IAuthoritySearchCriteria searchCriteria)
        {
            List<WebAuthority> webAuthorities;

            CheckTransaction(userContext);
            webAuthorities = WebServiceProxy.UserService.GetAuthoritiesBySearchCriteria(GetClientInformation(userContext),
                                                                                        GetSearchCriteria(userContext, searchCriteria));
            return GetAuthorities(userContext, webAuthorities);
        }


        /// <summary>
        /// Get AuthorityList from list of WebAuthority
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webAuthorityList">List of WebAuthority.</param>
        /// <returns>AuthorityList.</returns>
        private AuthorityList GetAuthorityList(IUserContext userContext,
                                               List<WebAuthority> webAuthorityList)
        {
            AuthorityList authorityList;

            authorityList = new AuthorityList();
            if (webAuthorityList.IsNotEmpty())
            {
                foreach (WebAuthority webAuthority in webAuthorityList)
                {
                    authorityList.Add(GetAuthority(userContext, webAuthority));
                }
            }
            return authorityList;
        }

        /// <summary>
        /// Get authority data type intreface from WebAuthorityDataType.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webAuthorityDataType">WebAuthorityDataType.</param>
        /// <returns>IAuthorityDataType.</returns>
        private IAuthorityDataType GetAuthorityDataType(IUserContext userContext,
                                            WebAuthorityDataType webAuthorityDataType)
        {
            return new AuthorityDataType(webAuthorityDataType.Id,
                                   webAuthorityDataType.Identifier,
                                   GetDataContext(userContext));
        }

        /// <summary>
        /// Get web authority data type from authority data type interface.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="AuthorityDataType">AuthorityDataType.</param>
        /// <returns>WebAuthorityDataType.</returns>
        private WebAuthorityDataType GetAuthorityDataType(IUserContext userContext,
                                              IAuthorityDataType AuthorityDataType)
        {
            WebAuthorityDataType webAuthorityDataType;

            webAuthorityDataType = new WebAuthorityDataType();
            webAuthorityDataType.Id = AuthorityDataType.Id;
            webAuthorityDataType.Identifier = AuthorityDataType.Identifier;
            return webAuthorityDataType;
        }

        /// <summary>
        /// Get authority data type list from a list of web authority data type.
        /// </summary>
        /// <param name="userContext">User context.></param>
        /// <param name="webAuthorityDataTypes">List of WebAuthorityDataType></param>
        /// <returns>AuthorityDataTypeList</returns>
        private AuthorityDataTypeList GetAuthorityDataTypes(IUserContext userContext,
                                                List<WebAuthorityDataType> webAuthorityDataTypes)
        {
            AuthorityDataTypeList AuthorityDataTypes;

            AuthorityDataTypes = new AuthorityDataTypeList();
            if (webAuthorityDataTypes.IsNotEmpty())
            {
                foreach (WebAuthorityDataType webAuthorityDataType in webAuthorityDataTypes)
                {
                    AuthorityDataTypes.Add(GetAuthorityDataType(userContext,
                                                    webAuthorityDataType));
                }
            }
            return AuthorityDataTypes;
        }

        /// <summary>
        /// Get authority data types list.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>AuthorityDataTypeList</returns>
        public AuthorityDataTypeList GetAuthorityDataTypes(IUserContext userContext)
        {
            List<WebAuthorityDataType> webAuthorityDataTypes;

            CheckTransaction(userContext);
            webAuthorityDataTypes = WebServiceProxy.UserService.GetAuthorityDataTypes(GetClientInformation(userContext));
            return GetAuthorityDataTypes(userContext, webAuthorityDataTypes);
        } 
        
        /// <summary>
        /// Get authority data types list for specific application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>AuthorityDataTypeList</returns>
        public AuthorityDataTypeList GetAuthorityDataTypesByApplicationId(IUserContext userContext, Int32 applicationId)
        {
            List<WebAuthorityDataType> webAuthorityDataTypes;

            CheckTransaction(userContext);
            webAuthorityDataTypes = WebServiceProxy.UserService.GetAuthorityDataTypesByApplicationId(GetClientInformation(userContext), applicationId);
            return GetAuthorityDataTypes(userContext, webAuthorityDataTypes);
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
            List<WebLockedUserInformation> lockedUserInformation;

            CheckTransaction(userContext);
            lockedUserInformation = WebServiceProxy.UserService.GetLockedUserInformation(GetClientInformation(userContext),
                                                                                         GetStringSearchCriteria(userNameSearchString));
            return GetLockedUserInformation(userContext, lockedUserInformation);
        }

        /// <summary>
        /// Get locked user information from web locked user information.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webLockedUserInformation">Web locked user information.</param>
        /// <returns>Locked user information.</returns>
        private ILockedUserInformation GetLockedUserInformation(IUserContext userContext,
                                                                WebLockedUserInformation webLockedUserInformation)
        {
            return new LockedUserInformation(webLockedUserInformation.LockedFrom,
                                             webLockedUserInformation.LockedTo,
                                             webLockedUserInformation.LoginAttemptCount,
                                             webLockedUserInformation.UserName,
                                             GetDataContext(userContext));
        }

        /// <summary>
        /// Get locked user information from web locked user information.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webLockedUsersInformation">Web locked user information.</param>
        /// <returns>Locked user information.</returns>
        private LockedUserInformationList GetLockedUserInformation(IUserContext userContext,
                                                                   List<WebLockedUserInformation> webLockedUsersInformation)
        {
            LockedUserInformationList lockedUserInformation;

            lockedUserInformation = new LockedUserInformationList();
            if (webLockedUsersInformation.IsNotEmpty())
            {
                foreach (WebLockedUserInformation webLockedUserInformation in webLockedUsersInformation)
                {
                    lockedUserInformation.Add(GetLockedUserInformation(userContext,
                                                                       webLockedUserInformation));
                }
            }
            return lockedUserInformation;
        }

        /// <summary>
        /// Get Message type from web Message type.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webMessageType">Web Message type.</param>
        /// <returns>Message type.</returns>
        private IMessageType GetMessageType(IUserContext userContext,
                                            WebMessageType webMessageType)
        {
            return new MessageType(webMessageType.Id,
                                   webMessageType.Name,
                                   webMessageType.NameStringId,
                                   GetDataContext(userContext));
        }

        /// <summary>
        /// Get web Message type from Message type.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="MessageType">Message type.</param>
        /// <returns>Web Message type.</returns>
        private WebMessageType GetMessageType(IUserContext userContext,
                                              IMessageType MessageType)
        {
            WebMessageType webMessageType;

            webMessageType = new WebMessageType();
            webMessageType.Id = MessageType.Id;
            webMessageType.Name = MessageType.Name;
            webMessageType.NameStringId = MessageType.NameStringId;
            return webMessageType;
        }

        /// <summary>
        /// Get Message types from web Message types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webMessageTypes">Web Message types.</param>
        /// <returns>Message types.</returns>
        private MessageTypeList GetMessageTypes(IUserContext userContext,
                                                List<WebMessageType> webMessageTypes)
        {
            MessageTypeList MessageTypes;

            MessageTypes = new MessageTypeList();
            if (webMessageTypes.IsNotEmpty())
            {
                foreach (WebMessageType webMessageType in webMessageTypes)
                {
                    MessageTypes.Add(GetMessageType(userContext,
                                                    webMessageType));
                }
            }
            return MessageTypes;
        }

        /// <summary>
        /// Get all Message types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All Message types.</returns>
        public MessageTypeList GetMessageTypes(IUserContext userContext)
        {
            List<WebMessageType> webMessageTypes;

            CheckTransaction(userContext);
            webMessageTypes = WebServiceProxy.UserService.GetMessageTypes(GetClientInformation(userContext));
            return GetMessageTypes(userContext, webMessageTypes);
        }

        /// <summary>
        /// Get organization by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Requested organization.</returns>       
        public IOrganization GetOrganization(IUserContext userContext,
                                             Int32 organizationId)
        {
            WebOrganization webOrganization;

            CheckTransaction(userContext);
            webOrganization = WebServiceProxy.UserService.GetOrganization(GetClientInformation(userContext),
                                                 organizationId);
            return GetOrganization(userContext, webOrganization);
        }

        /// <summary>
        /// Get organization from web organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webOrganization">A web organization.</param>
        /// <returns>A organization.</returns>
        public IOrganization GetOrganization(IUserContext userContext,
                                             WebOrganization webOrganization)
        {
            IOrganization organization;

            organization = new Organization(userContext);
            UpdateOrganization(userContext, organization, webOrganization);
            return organization;
        }

        /// <summary>
        /// Get WebOrganization from Organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">Organization.</param>
        /// <returns>WebOrganization.</returns>
        public WebOrganization GetOrganization(IUserContext userContext,
                                               IOrganization organization)
        {
            WebOrganization webOrganization;
            webOrganization = new WebOrganization();

            webOrganization.Addresses = GetAddresses(userContext, organization.Addresses);
            if (organization.AdministrationRoleId.HasValue)
            {
                webOrganization.AdministrationRoleId = organization.AdministrationRoleId.Value;
            }
            webOrganization.CreatedBy = organization.UpdateInformation.CreatedBy;
            webOrganization.CreatedDate = organization.UpdateInformation.CreatedDate;
            webOrganization.Description = organization.Description;
            webOrganization.GUID = organization.GUID;
            webOrganization.HasSpeciesCollection = organization.HasSpeciesCollection;
            webOrganization.Id = organization.Id;
            webOrganization.IsAdministrationRoleIdSpecified = organization.AdministrationRoleId.HasValue;
            webOrganization.ModifiedBy = organization.UpdateInformation.ModifiedBy;
            webOrganization.ModifiedDate = organization.UpdateInformation.ModifiedDate;
            webOrganization.Name = organization.Name;
            webOrganization.Category = GetOrganizationCategory(userContext, organization.Category);
            webOrganization.PhoneNumbers = GetPhoneNumbers(userContext, organization.PhoneNumbers);
            webOrganization.ShortName = organization.ShortName;

            return webOrganization;
        }

        /// <summary>
        /// GetOrganizationRoles
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>
        /// Returns list of roles or 
        /// null if organizationid doesn't match or if organization has no roles.
        /// </returns>
        public RoleList GetOrganizationRoles(IUserContext userContext, Int32 organizationId)
        {
            RoleList roles;
            List<WebRole> webRoles;

            CheckTransaction(userContext);
            webRoles = WebServiceProxy.UserService.GetOrganizationRoles(GetClientInformation(userContext), organizationId);
            roles = new RoleList();
            foreach (WebRole webRole in webRoles)
            {
                roles.Add(GetRole(userContext, webRole));
            }
            return roles;
        }

        /// <summary>
        /// Get all organizations 
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of all organizations or null if no organizations exists.
        /// </returns>
        public OrganizationList GetOrganizations(IUserContext userContext)
        {

            List<WebOrganization> webOrganizationList;

            CheckTransaction(userContext);
            webOrganizationList = WebServiceProxy.UserService.GetOrganizations(GetClientInformation(userContext));
            return GetOrganizations(userContext, webOrganizationList);

        }

        /// <summary>
        /// Get organizations from web organizations.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webOrganizations">Web organizations.</param>
        /// <returns>Organizations.</returns>
        private OrganizationList GetOrganizations(IUserContext userContext,
                                                  List<WebOrganization> webOrganizations)
        {
            OrganizationList organizations;

            organizations = new OrganizationList();
            if (webOrganizations.IsNotEmpty())
            {
                foreach (WebOrganization webOrganization in webOrganizations)
                {
                    organizations.Add(GetOrganization(userContext,
                                                      webOrganization));
                }
            }
            return organizations;
        }

        /// <summary>
        /// Get organizations by organization category
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategoryId">Organization category id.</param>
        /// <returns>
        /// Returns list of organizations or null if no organizations are categorized as the
        /// specified category.
        /// </returns>
        public OrganizationList GetOrganizationsByOrganizationCategory(IUserContext userContext,
                                                                       Int32 organizationCategoryId)
        {
            List<WebOrganization> webOrganizationList;

            CheckTransaction(userContext);
            webOrganizationList = WebServiceProxy.UserService.GetOrganizationsByOrganizationCategory(GetClientInformation(userContext), organizationCategoryId);
            return GetOrganizations(userContext, webOrganizationList);
        }

        /// <summary>
        /// Get organizations that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Organizations that matches the search criteria</returns>
        public OrganizationList GetOrganizationsBySearchCriteria(IUserContext userContext,
                                                                 IOrganizationSearchCriteria searchCriteria)
        {
            List<WebOrganization> webOrganizations;

            CheckTransaction(userContext);
            webOrganizations = WebServiceProxy.UserService.GetOrganizationsBySearchCriteria(GetClientInformation(userContext),
                                                                                   GetSearchCriteria(userContext, searchCriteria));
            return GetOrganizations(userContext, webOrganizations);
        }

        /// <summary>
        /// Get WebOrganizationCategory from OrganizationCategory.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategory">OrganizationCategory.</param>
        /// <returns>WebOrganizationCategory.</returns>
        public WebOrganizationCategory GetOrganizationCategory(IUserContext userContext,
                                                               IOrganizationCategory organizationCategory)
        {
            WebOrganizationCategory webOrganizationCategory;
            webOrganizationCategory = new WebOrganizationCategory();

            if (organizationCategory.AdministrationRoleId.HasValue)
            {
                webOrganizationCategory.AdministrationRoleId = organizationCategory.AdministrationRoleId.Value;
            }
            webOrganizationCategory.CreatedBy = organizationCategory.UpdateInformation.CreatedBy;
            webOrganizationCategory.CreatedDate = organizationCategory.UpdateInformation.CreatedDate;
            webOrganizationCategory.Id = organizationCategory.Id;
            webOrganizationCategory.Name = organizationCategory.Name;
            webOrganizationCategory.Description = organizationCategory.Description;
            webOrganizationCategory.DescriptionStringId = organizationCategory.DescriptionStringId;
            webOrganizationCategory.IsAdministrationRoleIdSpecified = organizationCategory.AdministrationRoleId.HasValue;
            webOrganizationCategory.ModifiedBy = organizationCategory.UpdateInformation.ModifiedBy;
            webOrganizationCategory.ModifiedDate = organizationCategory.UpdateInformation.ModifiedDate;

            return webOrganizationCategory;
        }

        /// <summary>
        /// GetOrganizationCategories
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of organization types or null if no organization types are found.
        /// </returns>
        public OrganizationCategoryList GetOrganizationCategories(IUserContext userContext)
        {
            OrganizationCategoryList organizationCategories;
            List<WebOrganizationCategory> webOrganizationCategories;

            CheckTransaction(userContext);
            webOrganizationCategories = WebServiceProxy.UserService.GetOrganizationCategories(GetClientInformation(userContext));
            organizationCategories = new OrganizationCategoryList();
            foreach (WebOrganizationCategory webOrganizationCategory in webOrganizationCategories)
            {
                organizationCategories.Add(GetOrganizationCategory(userContext, webOrganizationCategory));
            }
            return organizationCategories;
        }

        /// <summary>
        /// Get organization category by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategoryId">Organization category id.</param>
        /// <returns>Requested organization category.</returns>       
        public IOrganizationCategory GetOrganizationCategory(IUserContext userContext, Int32 organizationCategoryId)
        {
            WebOrganizationCategory webOrganizationCategory;

            CheckTransaction(userContext);
            webOrganizationCategory = WebServiceProxy.UserService.GetOrganizationCategory(GetClientInformation(userContext),
                                                                                 organizationCategoryId);
            return GetOrganizationCategory(userContext, webOrganizationCategory);
        }

        /// <summary>
        /// Get OrganizationCategory from WebOrganizationCategory.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webOrganizationCategory">WebOrganizationCategory.</param>
        /// <returns>OrganizationCategory.</returns>
        public IOrganizationCategory GetOrganizationCategory(IUserContext userContext,
                                                             WebOrganizationCategory webOrganizationCategory)
        {
            UpdateInformation updateInformation;
            updateInformation = new UpdateInformation();
            updateInformation.ModifiedBy = webOrganizationCategory.ModifiedBy;
            updateInformation.ModifiedDate = webOrganizationCategory.ModifiedDate;
            updateInformation.CreatedBy = webOrganizationCategory.CreatedBy;
            updateInformation.CreatedDate = webOrganizationCategory.CreatedDate;
            return new OrganizationCategory(webOrganizationCategory.Id,
                                        webOrganizationCategory.Name,
                                        webOrganizationCategory.Description,
                                        webOrganizationCategory.DescriptionStringId,
                                        webOrganizationCategory.AdministrationRoleId,
                                        updateInformation,
                                        GetDataContext(userContext));
        }

        /// <summary>
        /// Get password information from web password information.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webPasswordInformation">A web password information.</param>
        /// <returns>Password information.</returns>
        private IPasswordInformation GetPasswordInformation(IUserContext userContext,
                                                            WebPasswordInformation webPasswordInformation)
        {
            return new PasswordInformation(webPasswordInformation.UserName,
                                           webPasswordInformation.EmailAddress,
                                           webPasswordInformation.Password,
                                           GetDataContext(userContext));
        }

        /// <summary>
        /// Get person by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="personId">Person id.</param>
        /// <returns>Requested person.</returns>       
        public IPerson GetPerson(IUserContext userContext,
                                 Int32 personId)
        {
            WebPerson webPerson;

            CheckTransaction(userContext);
            webPerson = WebServiceProxy.UserService.GetPerson(GetClientInformation(userContext),
                                                     personId);
            return GetPerson(userContext, webPerson);
        }

        /// <summary>
        /// Get person from web person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webPerson">A web person.</param>
        /// <returns>A person.</returns>
        public IPerson GetPerson(IUserContext userContext,
                                 WebPerson webPerson)
        {
            IPerson person;

            person = new Person(userContext);
            UpdatePerson(userContext, person, webPerson); 
            return person;
        }

        /// <summary>
        /// Get web person from person.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">Person.</param>
        /// <returns>Web person.</returns>
        public WebPerson GetPerson(IUserContext userContext,
                                   IPerson person)
        {
            WebPerson webPerson;

            webPerson = new WebPerson();
            webPerson.Addresses = GetAddresses(userContext, person.Addresses);
            if (person.AdministrationRoleId.HasValue)
            {
                webPerson.AdministrationRoleId = person.AdministrationRoleId.Value;
            }
            if (person.BirthYear.HasValue)
            {
                webPerson.BirthYear = person.BirthYear.Value;
            }
            webPerson.CreatedBy = person.UpdateInformation.CreatedBy;
            webPerson.CreatedDate = person.UpdateInformation.CreatedDate;
            if (person.DeathYear.HasValue)
            {
                webPerson.DeathYear = person.DeathYear.Value;
            }
            webPerson.EmailAddress = person.EmailAddress;
            webPerson.FirstName = person.FirstName;
            webPerson.Gender = GetPersonGender(userContext, person.Gender);
            webPerson.GUID = person.GUID;
            webPerson.HasSpeciesCollection = person.HasSpeciesCollection;
            webPerson.Id = person.Id;
            webPerson.IsAdministrationRoleIdSpecified = person.AdministrationRoleId.HasValue;
            webPerson.IsBirthYearSpecified = person.BirthYear.HasValue;
            webPerson.IsDeathYearSpecified = person.DeathYear.HasValue;
            webPerson.IsUserIdSpecified = person.UserId.HasValue;
            webPerson.LastName = person.LastName;
            webPerson.Locale = GetLocale(person.Locale);
            webPerson.MiddleName = person.MiddleName;
            webPerson.ModifiedBy = person.UpdateInformation.ModifiedBy;
            webPerson.ModifiedDate = person.UpdateInformation.ModifiedDate;
            webPerson.PhoneNumbers = GetPhoneNumbers(userContext, person.PhoneNumbers);
            webPerson.Presentation = person.Presentation;
            webPerson.ShowAddresses = person.ShowAddresses;
            webPerson.ShowEmailAddress = person.ShowEmailAddress;
            webPerson.ShowPersonalInformation = person.ShowPersonalInformation;
            webPerson.ShowPhoneNumbers = person.ShowPhoneNumbers;
            webPerson.ShowPresentation = person.ShowPresentation;
            webPerson.TaxonNameTypeId = person.TaxonNameTypeId;
            webPerson.URL = person.URL;
            if (person.UserId.HasValue)
            {
                webPerson.UserId = person.UserId.Value;
            }
            return webPerson;
        }

        /// <summary>
        /// Get web person gender from person gender.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="personGender">A person gender.</param>
        /// <returns>A web person gender.</returns>
        public WebPersonGender GetPersonGender(IUserContext userContext,
                                               IPersonGender personGender)
        {
            WebPersonGender webPersonGender;

            webPersonGender = new WebPersonGender();
            webPersonGender.Id = personGender.Id;
            webPersonGender.Name = personGender.Name;
            webPersonGender.NameStringId = personGender.NameStringId;
            return webPersonGender;
        }

        /// <summary>
        /// Get person gender from web person gender.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webPersonGender">A web person gender.</param>
        /// <returns>A person gender.</returns>
        public IPersonGender GetPersonGender(IUserContext userContext,
                                             WebPersonGender webPersonGender)
        {
            return new PersonGender(webPersonGender.Id,
                                    webPersonGender.Name,
                                    webPersonGender.NameStringId,
                                    GetDataContext(userContext));
        }

        /// <summary>
        /// Get all person genders.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A list of all person genders.</returns>
        public PersonGenderList GetPersonGenders(IUserContext userContext)
        {
            PersonGenderList personGenders;
            List<WebPersonGender> webPersonGenders;

            CheckTransaction(userContext);
            webPersonGenders = WebServiceProxy.UserService.GetPersonGenders(GetClientInformation(userContext));
            personGenders = new PersonGenderList();
            foreach (WebPersonGender webPersonGender in webPersonGenders)
            {
                personGenders.Add(GetPersonGender(userContext,
                                                  webPersonGender));
            }
            return personGenders;
        }

        /// <summary>
        /// Get persons from web users.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webPersons">Web persons.</param>
        /// <returns>Users.</returns>
        private PersonList GetPersons(IUserContext userContext,
                                      List<WebPerson> webPersons)
        {
            PersonList persons;

            persons = new PersonList();
            if (webPersons.IsNotEmpty())
            {
                foreach (WebPerson webPerson in webPersons)
                {
                    persons.Add(GetPerson(userContext, webPerson));
                }
            }
            return persons;
        }

        /// <summary>
        /// Get persons that have been modified or created between certain dates.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="modifiedFromDate">Date after which person object have been modified.</param>
        /// <param name="modifiedUntilDate">Date before which person object have been modified.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public PersonList GetPersonsByModifiedDate(IUserContext userContext,
                                            DateTime modifiedFromDate, 
                                            DateTime modifiedUntilDate)
        {
            List<WebPerson> webPersons;

            CheckTransaction(userContext);
            webPersons = WebServiceProxy.UserService.GetPersonsByModifiedDate(GetClientInformation(userContext),
                                                                    modifiedFromDate, 
                                                                    modifiedUntilDate);
            return GetPersons(userContext, webPersons);
        }

        /// <summary>
        /// Get persons that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public PersonList GetPersonsBySearchCriteria(IUserContext userContext,
                                                     IPersonSearchCriteria searchCriteria)
        {
            List<WebPerson> webPersons;

            CheckTransaction(userContext);
            webPersons = WebServiceProxy.UserService.GetPersonsBySearchCriteria(GetClientInformation(userContext),
                                                                       GetSearchCriteria(userContext, searchCriteria));
            return GetPersons(userContext, webPersons);
        }

        /// <summary>
        /// Get web phone number from phone number.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="phoneNumber">Phone number.</param>
        /// <returns>Web phone number.</returns>
        private WebPhoneNumber GetPhoneNumber(IUserContext userContext,
                                              IPhoneNumber phoneNumber)
        {
            WebPhoneNumber webPhoneNumber;

            webPhoneNumber = new WebPhoneNumber();
            webPhoneNumber.Country = GetCountry(userContext,
                                                phoneNumber.Country);
            webPhoneNumber.Id = phoneNumber.Id;
            webPhoneNumber.Number = phoneNumber.Number;
            webPhoneNumber.Type = GetPhoneNumberType(userContext,
                                                     phoneNumber.Type);
            return webPhoneNumber;
        }

        /// <summary>
        /// Get phone number from web phone number.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webPhoneNumber">Web phone number.</param>
        /// <returns>Phone number.</returns>
        private IPhoneNumber GetPhoneNumber(IUserContext userContext,
                                            WebPhoneNumber webPhoneNumber)
        {
            IPhoneNumber phoneNumber;

            phoneNumber = new PhoneNumber(userContext);
            phoneNumber.Country = GetCountry(userContext, webPhoneNumber.Country);
            phoneNumber.DataContext = GetDataContext(userContext);
            phoneNumber.Id = webPhoneNumber.Id;
            phoneNumber.Number = webPhoneNumber.Number;
            phoneNumber.Type = GetPhoneNumberType(userContext,
                                                  webPhoneNumber.Type);
            return phoneNumber;
        }

        /// <summary>
        /// Get web phone numbers from phone numbers.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="phoneNumbers">Phone numbers.</param>
        /// <returns>Web phone numbers.</returns>
        private List<WebPhoneNumber> GetPhoneNumbers(IUserContext userContext,
                                                     PhoneNumberList phoneNumbers)
        {
            List<WebPhoneNumber> webPhoneNumbers;

            webPhoneNumbers = null;
            if (phoneNumbers.IsNotEmpty())
            {
                webPhoneNumbers = new List<WebPhoneNumber>();
                foreach (IPhoneNumber phoneNumber in phoneNumbers)
                {
                    webPhoneNumbers.Add(GetPhoneNumber(userContext,
                                                       phoneNumber));
                }
            }
            return webPhoneNumbers;
        }

        /// <summary>
        /// Get phone numbers from web phone numbers.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webPhoneNumbers">Web phone numbers.</param>
        /// <returns>Phone numbers.</returns>
        private PhoneNumberList GetPhoneNumbers(IUserContext userContext,
                                                List<WebPhoneNumber> webPhoneNumbers)
        {
            PhoneNumberList phoneNumbers;

            phoneNumbers = new PhoneNumberList();
            if (webPhoneNumbers.IsNotEmpty())
            {
                foreach (WebPhoneNumber webPhoneNumber in webPhoneNumbers)
                {
                    phoneNumbers.Add(GetPhoneNumber(userContext,
                                                    webPhoneNumber));
                }
            }
            return phoneNumbers;
        }

        /// <summary>
        /// Get web phone number type from  phone number type.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="phoneNumberType">Phone number type.</param>
        /// <returns>A person gender.</returns>
        private WebPhoneNumberType GetPhoneNumberType(IUserContext userContext,
                                                      IPhoneNumberType phoneNumberType)
        {
            WebPhoneNumberType webPhoneNumberType;

            webPhoneNumberType = new WebPhoneNumberType();
            webPhoneNumberType.Id = phoneNumberType.Id;
            webPhoneNumberType.Name = phoneNumberType.Name;
            webPhoneNumberType.NameStringId = phoneNumberType.NameStringId;
            return webPhoneNumberType;
        }

        /// <summary>
        /// Get phone number type from web phone number type.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webPhoneNumberType">Web phone number type.</param>
        /// <returns>Phone number type.</returns>
        private IPhoneNumberType GetPhoneNumberType(IUserContext userContext,
                                                    WebPhoneNumberType webPhoneNumberType)
        {
            return new PhoneNumberType(webPhoneNumberType.Id,
                                       webPhoneNumberType.Name,
                                       webPhoneNumberType.NameStringId,
                                       GetDataContext(userContext));
        }

        /// <summary>
        /// Get all phone number types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>A list of all phone number types.</returns>
        public PhoneNumberTypeList GetPhoneNumberTypes(IUserContext userContext)
        {
            List<WebPhoneNumberType> webPhoneNumberTypes;

            CheckTransaction(userContext);
            webPhoneNumberTypes = WebServiceProxy.UserService.GetPhoneNumberTypes(GetClientInformation(userContext));
            return GetPhoneNumberTypes(userContext, webPhoneNumberTypes);
        }

        /// <summary>
        /// Get phone number types from web phone number types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webPhoneNumberTypes">Web phone number types.</param>
        /// <returns>Phone number types.</returns>
        private PhoneNumberTypeList GetPhoneNumberTypes(IUserContext userContext,
                                                        List<WebPhoneNumberType> webPhoneNumberTypes)
        {
            PhoneNumberTypeList phoneNumberTypes;

            phoneNumberTypes = new PhoneNumberTypeList();
            if (webPhoneNumberTypes.IsNotEmpty())
            {
                foreach (WebPhoneNumberType webPhoneNumberType in webPhoneNumberTypes)
                {
                    phoneNumberTypes.Add(GetPhoneNumberType(userContext,
                                                            webPhoneNumberType));
                }
            }
            return phoneNumberTypes;
        }

        /// <summary>
        /// Get role by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Requested role.</returns>       
        public IRole GetRole(IUserContext userContext,
                             Int32 roleId)
        {
            WebRole webRole;

            CheckTransaction(userContext);
            webRole = WebServiceProxy.UserService.GetRole(GetClientInformation(userContext), roleId);
            return GetRole(userContext, webRole);
        }

        /// <summary>
        /// Get role from web role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRole">A web role.</param>
        /// <returns>A role.</returns>
        public IRole GetRole(IUserContext userContext,
                             WebRole webRole)
        {
            IRole role;

            role = new Role(userContext);
            UpdateRole(userContext, role, webRole);
            return role;
        }

        /// <summary>
        /// Get Roles from list of WebRoles
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRoles">List of WebRole.</param>
        /// <returns>RoleList.</returns>
        private RoleList GetRoles(IUserContext userContext,
                                  List<WebRole> webRoles)
        {
            RoleList roles;

            roles = new RoleList();
            if (webRoles.IsNotEmpty())
            {
                foreach (WebRole webRole in webRoles)
                {
                    roles.Add(GetRole(userContext, webRole));
                }
            }
            return roles;
        }

        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        public RoleList GetRolesBySearchCriteria(IUserContext userContext,
                                                 IRoleSearchCriteria searchCriteria)
        {
            List<WebRole> webRoles;

            CheckTransaction(userContext);
            webRoles = WebServiceProxy.UserService.GetRolesBySearchCriteria(GetClientInformation(userContext),
                                                                   GetSearchCriteria(userContext, searchCriteria));
            return GetRoles(userContext, webRoles);
        }

        /// <summary>
        /// Get all roles where its usergroup is admnistrated by a certain role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Id of the administration role.</param>
        /// <returns>List of roles.</returns>
        public RoleList GetRolesByUserGroupAdministrationRoleId(IUserContext userContext,
                                                                Int32 roleId)
        {
            List<WebRole> webRoles;

            CheckTransaction(userContext);
            webRoles = WebServiceProxy.UserService.GetRolesByUserGroupAdministrationRoleId(GetClientInformation(userContext),
                                                                                  roleId);

            return GetRoles(userContext, webRoles);
        }

        /// <summary>
        /// Get all roles where its usergroup is admnistrated by a certain user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">Id of the administrating user.</param>
        /// <returns>List of roles.</returns>
        public RoleList GetRolesByUserGroupAdministratorUserId(IUserContext userContext,
                                                                Int32 userId)
        {
            List<WebRole> webRoles;

            CheckTransaction(userContext);
            webRoles = WebServiceProxy.UserService.GetRolesByUserGroupAdministratorUserId(GetClientInformation(userContext),
                                                                                  userId);

            return GetRoles(userContext, webRoles);
        }

        /// <summary>
        /// Get rolemembers that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria</returns>
        public List<RoleMember> GetRoleMembersBySearchCriteria(IUserContext userContext,
                                                               IRoleMemberSearchCriteria searchCriteria)
        {
            List<WebRoleMember> webRoleMembers;

            CheckTransaction(userContext);
            webRoleMembers = WebServiceProxy.UserService.GetRoleMembersBySearchCriteria(GetClientInformation(userContext),
                                                                                        GetSearchCriteria(userContext, searchCriteria));
            return GetRoleMembers(userContext, webRoleMembers);
        }

        /// <summary>
        /// List of WebRoleMembers -> List of RoleMembers
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRoleMembers">List of WebRoleMember.</param>
        /// <returns>List of RoleMembers.</returns>   
        private List<RoleMember> GetRoleMembers(IUserContext userContext, List<WebRoleMember> webRoleMembers)
        {
            List<RoleMember> roleMembers = new List<RoleMember>();
            RoleMember roleMember;

            foreach (WebRoleMember _roleMember in webRoleMembers)
            {
                roleMember = new RoleMember();
                roleMember.Role = GetRole(userContext, _roleMember.Role);
                roleMember.User = GetUser(userContext, _roleMember.User);
                roleMember.IsActivated = _roleMember.IsActivated;
                roleMembers.Add(roleMember);
            }
            return roleMembers;
        }

        /// <summary>
        /// Get web search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebAuthoritySearchCriteria GetSearchCriteria(IUserContext userContext,
                                                             IAuthoritySearchCriteria searchCriteria)
        {
            WebAuthoritySearchCriteria webSearchCriteria;

            webSearchCriteria = new WebAuthoritySearchCriteria();
            webSearchCriteria.AuthorityIdentifier = searchCriteria.AuthorityIdentifier;
            webSearchCriteria.ApplicationIdentifier = searchCriteria.ApplicationIdentifier;
            webSearchCriteria.AuthorityDataTypeIdentifier = searchCriteria.AuthorityDataTypeIdentifier;
            webSearchCriteria.AuthorityName = searchCriteria.AuthorityName;
            
            return webSearchCriteria;
        }

        /// <summary>
        /// Get web search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebPersonSearchCriteria GetSearchCriteria(IUserContext userContext,
                                                          IPersonSearchCriteria searchCriteria)
        {
            WebPersonSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebPersonSearchCriteria();
            webSearchCriteria.FirstName = searchCriteria.FirstName;
            webSearchCriteria.FullName = searchCriteria.FullName;
            webSearchCriteria.LastName = searchCriteria.LastName;
            webSearchCriteria.IsHasSpeciesCollectionSpecified = searchCriteria.HasSpiecesCollection.IsNotNull();
            if (searchCriteria.HasSpiecesCollection.HasValue)
            {
                webSearchCriteria.HasSpeciesCollection = searchCriteria.HasSpiecesCollection.Value;
            }
            return webSearchCriteria;
        }

        /// <summary>
        /// Get web search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebUserSearchCriteria GetSearchCriteria(IUserContext userContext,
                                                        IPersonUserSearchCriteria searchCriteria)
        {
            WebUserSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebUserSearchCriteria();
            webSearchCriteria.FirstName = searchCriteria.FirstName;
            webSearchCriteria.FullName = searchCriteria.FullName;
            webSearchCriteria.LastName = searchCriteria.LastName;
            webSearchCriteria.EmailAddress = searchCriteria.EmailAddress;
            webSearchCriteria.City = searchCriteria.City;
            webSearchCriteria.UserType = UserType.Person;
            webSearchCriteria.IsUserTypeSpecified = true;
            if (searchCriteria.OrganizationId.HasValue)
            {
                webSearchCriteria.OrganizationId = searchCriteria.OrganizationId.Value;
            }
            webSearchCriteria.IsOrganizationIdSpecified = searchCriteria.OrganizationId.HasValue;
            if (searchCriteria.OrganizationCategoryId.HasValue)
            {
                webSearchCriteria.OrganizationCategoryId = searchCriteria.OrganizationCategoryId.Value;
            }
            webSearchCriteria.IsOrganizationCategoryIdSpecified = searchCriteria.OrganizationCategoryId.HasValue;
            if (searchCriteria.ApplicationId.HasValue)
            {
                webSearchCriteria.ApplicationId = searchCriteria.ApplicationId.Value;
            }
            webSearchCriteria.IsApplicationIdSpecified = searchCriteria.ApplicationId.HasValue;
            if (searchCriteria.ApplicationActionId.HasValue)
            {
                webSearchCriteria.ApplicationActionId = searchCriteria.ApplicationActionId.Value;
            }
            webSearchCriteria.IsApplicationActionIdSpecified = searchCriteria.ApplicationActionId.HasValue;

            return webSearchCriteria;
        }

        /// <summary>
        /// Get web search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebRoleSearchCriteria GetSearchCriteria(IUserContext userContext,
                                                        IRoleSearchCriteria searchCriteria)
        {
            WebRoleSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebRoleSearchCriteria();
            webSearchCriteria.Name = searchCriteria.Name;
            webSearchCriteria.ShortName = searchCriteria.ShortName;
            webSearchCriteria.Identifier = searchCriteria.Identifier;
            webSearchCriteria.IsOrganizationIdSpecified = searchCriteria.OrganizationId.HasValue;
            if (searchCriteria.OrganizationId.HasValue)
            {
                webSearchCriteria.OrganizationId = searchCriteria.OrganizationId.Value;
            }
            return webSearchCriteria;
        }

        /// <summary>
        /// Get web search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebRoleMemberSearchCriteria GetSearchCriteria(IUserContext userContext,
                                                              IRoleMemberSearchCriteria searchCriteria)
        {
            WebRoleMemberSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebRoleMemberSearchCriteria();
            // RoleIdList
            if (searchCriteria.RoleIdList.IsNotNull() && searchCriteria.RoleIdList.IsNotEmpty())
            {
                webSearchCriteria.RoleIds = new List<Int32>();
                webSearchCriteria.RoleIds = searchCriteria.RoleIdList;
            }
            // UserIdList
            if (searchCriteria.UserIdList.IsNotNull() && searchCriteria.UserIdList.IsNotEmpty())
            {
                webSearchCriteria.UserIds = new List<Int32>();
                webSearchCriteria.UserIds = searchCriteria.UserIdList;
            }
            // IsActivated
            webSearchCriteria.IsIsActivatedSpecified = false;
            if (searchCriteria.IsActivated.HasValue)
            {
                webSearchCriteria.IsActivated = searchCriteria.IsActivated.Value;
                webSearchCriteria.IsIsActivatedSpecified = true;
            }
            return webSearchCriteria;
        }

        /// <summary>
        /// Get web search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebOrganizationSearchCriteria GetSearchCriteria(IUserContext userContext,
                                                        IOrganizationSearchCriteria searchCriteria)
        {
            WebOrganizationSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebOrganizationSearchCriteria();
            webSearchCriteria.Name = searchCriteria.Name;
            webSearchCriteria.ShortName = searchCriteria.ShortName;
            webSearchCriteria.IsOrganizationCategoryIdSpecified = searchCriteria.OrganizationCategoryId.HasValue;
            if (searchCriteria.OrganizationCategoryId.HasValue)
            {
                webSearchCriteria.OrganizationCategoryId = searchCriteria.OrganizationCategoryId.Value;
            }
            webSearchCriteria.IsHasSpeciesCollectionSpecified = searchCriteria.HasSpiecesCollection.HasValue;
            if (searchCriteria.HasSpiecesCollection.HasValue)
            {
                webSearchCriteria.HasSpeciesCollection = searchCriteria.HasSpiecesCollection.Value;
            }

            return webSearchCriteria;
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <returns>User.</returns>
        public IUser GetUser(IUserContext userContext, Int32 userId)
        {
            IUser user;
            WebUser webUser;

            CheckTransaction(userContext);
            webUser = WebServiceProxy.UserService.GetUser(GetClientInformation(userContext),
                                                 userId);
            user = new User(userContext);
            UpdateUser(userContext, user, webUser);
            return user;
        }

        /// <summary>
        /// Get web user object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">User.</param>
        /// <returns>Web user object.</returns>
        private WebUser GetUser(IUserContext userContext, IUser user)
        {
            WebUser webUser;

            webUser = new WebUser();
            webUser.IsAccountActivated = user.IsAccountActivated;
            webUser.IsApplicationIdSpecified = user.ApplicationId.HasValue;
            if (user.ApplicationId.HasValue)
            {
                webUser.ApplicationId = user.ApplicationId.Value;
            }
            webUser.ActivationKey = user.ActivationKey;
            webUser.IsAdministrationRoleIdSpecified = user.AdministrationRoleId.HasValue;
            if (user.AdministrationRoleId.HasValue)
            {
                webUser.AdministrationRoleId = user.AdministrationRoleId.Value;
            }
            webUser.AuthenticationType = user.AuthenticationType;
            if (user.UpdateInformation.IsNotNull())
            {
                webUser.CreatedBy = user.UpdateInformation.CreatedBy;
                webUser.CreatedDate = user.UpdateInformation.CreatedDate;
                webUser.ModifiedBy = user.UpdateInformation.ModifiedBy;
                webUser.ModifiedDate = user.UpdateInformation.ModifiedDate;
            }
            webUser.EmailAddress = user.EmailAddress;
            webUser.GUID = user.GUID;
            webUser.Id = user.Id;
            webUser.IsPersonIdSpecified = user.PersonId.HasValue;
            if (user.PersonId.HasValue)
            {
                webUser.PersonId = user.PersonId.Value;
            }
            webUser.ShowEmailAddress = user.ShowEmailAddress;
            webUser.Type = user.Type;
            webUser.UserName = user.UserName;
            webUser.ValidFromDate = user.ValidFromDate;
            webUser.ValidToDate = user.ValidToDate;
            return webUser;
        }

        /// <summary>
        /// Get user by username.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userName">Users username.</param>
        /// <returns>User.</returns>
        public IUser GetUser(IUserContext userContext, String userName)
        {
            IUser user = null;
            WebUser webUser;

            CheckTransaction(userContext);
            webUser = WebServiceProxy.UserService.GetUser(GetClientInformation(userContext), userName);
            if (webUser.IsNotNull())
            {
                user = new User(userContext);
                UpdateUser(userContext, user, webUser);
            }
            return user;
        }

        /// <summary>
        /// Get user from web user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webUser">Web user.</param>
        /// <returns>User.</returns>
        private IUser GetUser(IUserContext userContext,
                              WebUser webUser)
        {
            IUser user;

            user = new User(userContext);
            UpdateUser(userContext, user, webUser);
            return user;
        }

        /// <summary>
        /// Get user context object.
        /// </summary>
        /// <param name="loginResponse">Login response.</param>
        /// <returns>User context object or null if login response is null.</returns>
        private IUserContext GetUserContext(WebLoginResponse loginResponse)
        {
            IUserContext userContext = null;

            if (loginResponse.IsNotNull())
            {
                userContext = new UserContext();
                userContext.Locale = GetLocale(loginResponse.Locale);
                SetToken(userContext, loginResponse.Token);
                userContext.User = GetUser(userContext,
                                           loginResponse.User);
                userContext.CurrentRoles = GetRoles(userContext, loginResponse.Roles);
            }
            return userContext;
        }

        /// <summary>
        /// Get list of user roles
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentifier">String that identifies the application.</param>
        /// <returns>List of roles.</returns>   
        public RoleList GetUserRoles(IUserContext userContext, Int32 userId, String applicationIdentifier)
        {
            RoleList roles;
            List<WebRole> webRoles;

            CheckTransaction(userContext);
            webRoles = WebServiceProxy.UserService.GetUserRoles(GetClientInformation(userContext), userId, applicationIdentifier);
            roles = new RoleList();
            foreach (WebRole webRole in webRoles)
            {
                roles.Add(GetRole(userContext, webRole));
            }
            return roles;
        }

        /// <summary>
        /// Get users from web users.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webUsers">Web users.</param>
        /// <returns>Users.</returns>
        private UserList GetUsers(IUserContext userContext,
                                  List<WebUser> webUsers)
        {
            UserList users;

            users = new UserList();
            if (webUsers.IsNotEmpty())
            {
                foreach (WebUser webUser in webUsers)
                {
                    users.Add(GetUser(userContext, webUser));
                }
            }
            return users;
        }

        /// <summary>
        /// Get all users that have specified role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>Users that matches the search criteria</returns>
        public UserList GetUsersByRole(IUserContext userContext,
                                       Int32 roleId)
        {
            List<WebUser> webUsers;

            CheckTransaction(userContext);
            webUsers = WebServiceProxy.UserService.GetUsersByRole(GetClientInformation(userContext),
                                                         roleId);
            return GetUsers(userContext, webUsers);
        }

        /// <summary>
        /// Get all users that have been associated with a role, that have not yet activated their role membership.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <returns>Users that matches the search criteria</returns>
        public UserList GetNonActivatedUsersByRole(IUserContext userContext,
                                       Int32 roleId)
        {
            List<WebUser> webUsers;

            CheckTransaction(userContext);
            webUsers = WebServiceProxy.UserService.GetNonActivatedUsersByRole(GetClientInformation(userContext),
                                                         roleId);
            return GetUsers(userContext, webUsers);
        }

        /// <summary>
        /// Get users that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Users that matches the search criteria</returns>
        public UserList GetUsersBySearchCriteria(IUserContext userContext,
                                                 IPersonUserSearchCriteria searchCriteria)
        {
            List<WebUser> webUsers;

            CheckTransaction(userContext);
            webUsers = WebServiceProxy.UserService.GetUsersBySearchCriteria(GetClientInformation(userContext),
                                                                   GetSearchCriteria(userContext, searchCriteria));
            return GetUsers(userContext, webUsers);
        }


        /// <summary>
        /// Test if a person already exists.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="emailAddress">Email address to check if person already exists or not.</param>
        /// <returns>
        /// Returns 'true' if person exists and
        /// 'false' if person does not exists.
        ///</returns>   
        public Boolean IsExistingPerson(IUserContext userContext,
                                        String emailAddress)
        {
            CheckTransaction(userContext);
            return WebServiceProxy.UserService.IsExistingPerson(GetClientInformation(userContext),
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
        ///</returns>   
        public Boolean IsExistingUser(IUserContext userContext,
                                      String userName)
        {
            CheckTransaction(userContext);
            return WebServiceProxy.UserService.IsExistingUser(GetClientInformation(userContext),
                                                     userName);
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
        public IUserContext Login(String userName,
                                  String password,
                                  String applicationIdentifier,
                                  Boolean isActivationRequired)
        {
            IUserContext userContext;
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.UserService.Login(userName,
                                                              password,
                                                              applicationIdentifier,
                                                              isActivationRequired);
            userContext = GetUserContext(loginResponse);
            if (userContext.IsNotNull())
            {
                FireUserLoggedInEvent(userContext,
                                      userName,
                                      password,
                                      applicationIdentifier,
                                      isActivationRequired);
            }
            return userContext;
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public void Logout(IUserContext userContext)
        {
            WebServiceProxy.UserService.Logout(GetClientInformation(userContext));
            FireUserLoggedOutEvent(userContext);
            SetToken(userContext, null);
        }

        /// <summary>
        /// Reset user's password
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="emailAddress">Users email address.</param>
        /// <returns>Information about user and new password.</returns>
        public IPasswordInformation ResetPassword(IUserContext userContext,
                                                  String emailAddress)
        {
            WebPasswordInformation webPasswordInformation;

            CheckTransaction(userContext);
            webPasswordInformation = WebServiceProxy.UserService.ResetPassword(GetClientInformation(userContext), emailAddress);
            return GetPasswordInformation(userContext, webPasswordInformation);
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        private void RefreshCache(IUserContext userContext)
        {
            WebServiceProxy.UserService.RefreshCache(GetClientInformation(userContext));
        }

        /// <summary>
        /// Removes user from a role
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>void</returns>
        public void RemoveUserFromRole(IUserContext userContext, Int32 roleId, Int32 userId)
        {
            CheckTransaction(userContext);
            WebServiceProxy.UserService.RemoveUserFromRole(GetClientInformation(userContext),
                                                  roleId,
                                                  userId);
        }

        /// <summary>
        /// Set UserService as data source in the onion data model.
        /// This method must me called before SetDataSource()
        /// methods in other data sources are called.
        /// </summary>
        public static void SetDataSource()
        {
            CoreData.CountryManager.DataSource = new CountryDataSource();
            CoreData.LocaleManager.DataSource = new LocaleDataSource();
            CoreData.UserManager.DataSource = new UserDataSource();
            CoreData.OrganizationManager.DataSource = new UserDataSource();
            CoreData.ApplicationManager.DataSource = new ApplicationDataSource();
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
        public void UpdateAuthority(IUserContext userContext, IAuthority authority)
        {
            WebAuthority webAuthority;

            CheckTransaction(userContext);
            webAuthority = WebServiceProxy.UserService.UpdateAuthority(GetClientInformation(userContext),
                                                              GetAuthority(userContext, authority));
            UpdateAuthority(userContext, authority, webAuthority);
        }

        /// <summary>
        /// Update authority object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">Authority.</param>
        /// <param name="webAuthority">Web authority.</param>
        private void UpdateAuthority(IUserContext userContext,
                                     IAuthority authority,
                                     WebAuthority webAuthority)
        {
            authority.ActionGUIDs.Clear();
            if (webAuthority.ActionGUIDs.IsNotNull())
            {
                authority.ActionGUIDs.AddRange(webAuthority.ActionGUIDs);
            }
            authority.FactorGUIDs.Clear();
            if (webAuthority.FactorGUIDs.IsNotNull())
            {
                authority.FactorGUIDs.AddRange(webAuthority.FactorGUIDs);
            }
            authority.LocalityGUIDs.Clear();
            if (webAuthority.LocalityGUIDs.IsNotNull())
            {
                authority.LocalityGUIDs.AddRange(webAuthority.LocalityGUIDs);
            }
            authority.ProjectGUIDs.Clear();
            if (webAuthority.ProjectGUIDs.IsNotNull())
            {
                authority.ProjectGUIDs.AddRange(webAuthority.ProjectGUIDs);
            }
            authority.RegionGUIDs.Clear();
            if (webAuthority.RegionGUIDs.IsNotNull())
            {
                authority.RegionGUIDs.AddRange(webAuthority.RegionGUIDs);
            }
            authority.TaxonGUIDs.Clear();
            if (webAuthority.TaxonGUIDs.IsNotNull())
            {
                authority.TaxonGUIDs.AddRange(webAuthority.TaxonGUIDs);
            }
            authority.ReadPermission = webAuthority.ReadPermission;
            authority.CreatePermission = webAuthority.CreatePermission;
            authority.UpdatePermission = webAuthority.UpdatePermission;
            authority.DeletePermission = webAuthority.DeletePermission;

            authority.MaxProtectionLevel = webAuthority.MaxProtectionLevel;
            authority.ReadNonPublicPermission = webAuthority.ShowNonPublicData;

            authority.RoleId = webAuthority.RoleId;
            authority.ApplicationId = webAuthority.ApplicationId;

            if (webAuthority.IsAdministrationRoleIdSpecified)
            {
                authority.AdministrationRoleId = webAuthority.AdministrationRoleId;
            }
            else
            {
                authority.AdministrationRoleId = null;
            }
            authority.Identifier = webAuthority.Identifier;
            authority.UpdateInformation.CreatedBy = webAuthority.CreatedBy;
            authority.UpdateInformation.CreatedDate = webAuthority.CreatedDate;
            authority.DataContext = GetDataContext(userContext);
            authority.Description = webAuthority.Description;
            authority.GUID = webAuthority.GUID;
            authority.Id = webAuthority.Id;
            authority.Name = webAuthority.Name;
            authority.Obligation = webAuthority.Obligation;
            authority.UpdateInformation.ModifiedBy = webAuthority.ModifiedBy;
            authority.UpdateInformation.ModifiedDate = webAuthority.ModifiedDate;
            authority.ValidFromDate = webAuthority.ValidFromDate;
            authority.ValidToDate = webAuthority.ValidToDate;
            if (webAuthority.AuthorityType.IsNotNull() && webAuthority.AuthorityType.Equals(AuthorityType.DataType))
            {
                // Update authority with correct autority data type.
                int authorityDataTypeId = webAuthority.AuthorityDataType.Id;
                String authorityDataTypeIdentifier = webAuthority.AuthorityDataType.Identifier;
                IDataContext dataContext = GetDataContext(userContext);
                AuthorityDataType authorityDataType = new AuthorityDataType(authorityDataTypeId, authorityDataTypeIdentifier, dataContext);
                authority.AuthorityDataType = authorityDataType;
            }
            authority.AuthorityType = webAuthority.AuthorityType;
        }

        /// <summary>
        /// Update organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">
        /// Information about the updated organization.
        /// This object is updated with information 
        /// about the updated organization.
        /// </param>
        public void UpdateOrganization(IUserContext userContext, IOrganization organization)
        {
            WebOrganization webOrganization;

            CheckTransaction(userContext);
            webOrganization = WebServiceProxy.UserService.UpdateOrganization(GetClientInformation(userContext),
                                                                    GetOrganization(userContext, organization));
            UpdateOrganization(userContext, organization, webOrganization);
        }

        /// <summary>
        /// Update organization object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">Organization.</param>
        /// <param name="webOrganization">Web organization.</param>
        private void UpdateOrganization(IUserContext userContext,
                                        IOrganization organization,
                                        WebOrganization webOrganization)
        {
            organization.Addresses.Clear();
            organization.Addresses.AddRange(GetAddresses(userContext, webOrganization.Addresses));
            if (webOrganization.IsAdministrationRoleIdSpecified)
            {
                organization.AdministrationRoleId = webOrganization.AdministrationRoleId;
            }
            else
            {
                organization.AdministrationRoleId = null;
            }
            organization.UpdateInformation.CreatedBy = webOrganization.CreatedBy;
            organization.UpdateInformation.CreatedDate = webOrganization.CreatedDate;
            organization.DataContext = GetDataContext(userContext);
            organization.Description = webOrganization.Description;
            organization.GUID = webOrganization.GUID;
            organization.HasSpeciesCollection = webOrganization.HasSpeciesCollection;
            organization.Id = webOrganization.Id;
            organization.UpdateInformation.ModifiedBy = webOrganization.ModifiedBy;
            organization.UpdateInformation.ModifiedDate = webOrganization.ModifiedDate;
            organization.Name = webOrganization.Name;
            organization.Category = GetOrganizationCategory(userContext, webOrganization.Category);
            organization.PhoneNumbers.Clear();
            organization.PhoneNumbers.AddRange(GetPhoneNumbers(userContext, webOrganization.PhoneNumbers));
            organization.ShortName = webOrganization.ShortName;
        }

        /// <summary>
        /// Update organization category object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategory">OrganizationCategory.</param>
        /// <param name="webOrganizationCategory">WebOrganizationCategory object.</param>
        private void UpdateOrganizationCategory(IUserContext userContext,
                                                IOrganizationCategory organizationCategory,
                                                WebOrganizationCategory webOrganizationCategory)
        {
            if (webOrganizationCategory.IsAdministrationRoleIdSpecified)
            {
                organizationCategory.AdministrationRoleId = webOrganizationCategory.AdministrationRoleId;
            }
            else
            {
                organizationCategory.AdministrationRoleId = null;
            }
            organizationCategory.UpdateInformation.CreatedBy = webOrganizationCategory.CreatedBy;
            organizationCategory.UpdateInformation.CreatedDate = webOrganizationCategory.CreatedDate;
            organizationCategory.DataContext = GetDataContext(userContext);
            organizationCategory.Description = webOrganizationCategory.Description;
            organizationCategory.Id = webOrganizationCategory.Id;
            organizationCategory.UpdateInformation.ModifiedBy = webOrganizationCategory.ModifiedBy;
            organizationCategory.UpdateInformation.ModifiedDate = webOrganizationCategory.ModifiedDate;
            organizationCategory.Name = webOrganizationCategory.Name;
        }

        /// <summary>
        /// Update existing organization category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategory">
        /// Information about the updated organization category.
        /// This object is updated with information 
        /// about the updated organization category.
        /// </param>
        public void UpdateOrganizationCategory(IUserContext userContext, IOrganizationCategory organizationCategory)
        {
            WebOrganizationCategory webOrganizationCategory;

            CheckTransaction(userContext);
            webOrganizationCategory = WebServiceProxy.UserService.UpdateOrganizationCategory(GetClientInformation(userContext),
                                                                                    GetOrganizationCategory(userContext, organizationCategory));
            UpdateOrganizationCategory(userContext, organizationCategory, webOrganizationCategory);
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
            CheckTransaction(userContext);
            return WebServiceProxy.UserService.UpdatePassword(GetClientInformation(userContext),
                                                              oldPassword,
                                                              newPassword);
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
        public void UpdatePerson(IUserContext userContext, IPerson person)
        {
            WebPerson webPerson;

            CheckTransaction(userContext);
            webPerson = WebServiceProxy.UserService.UpdatePerson(GetClientInformation(userContext),
                                                        GetPerson(userContext, person));
            UpdatePerson(userContext, person, webPerson);
        }

        /// <summary>
        /// Update person object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="person">Person.</param>
        /// <param name="webPerson">Web person.</param>
        private void UpdatePerson(IUserContext userContext,
                                  IPerson person,
                                  WebPerson webPerson)
        {
            person.Addresses.Clear();
            person.Addresses.AddRange(GetAddresses(userContext, webPerson.Addresses));
            if (webPerson.IsAdministrationRoleIdSpecified)
            {
                person.AdministrationRoleId = webPerson.AdministrationRoleId;
            }
            else
            {
                person.AdministrationRoleId = null;
            }
            if (webPerson.IsBirthYearSpecified)
            {
                person.BirthYear = webPerson.BirthYear;
            }
            else
            {
                person.BirthYear = null;
            }
            person.UpdateInformation.CreatedBy = webPerson.CreatedBy;
            person.UpdateInformation.CreatedDate = webPerson.CreatedDate;
            person.DataContext = GetDataContext(userContext);
            if (webPerson.IsDeathYearSpecified)
            {
                person.DeathYear = webPerson.DeathYear;
            }
            else
            {
                person.DeathYear = null;
            }
            person.EmailAddress = webPerson.EmailAddress;
            person.FirstName = webPerson.FirstName;
            person.Gender = GetPersonGender(userContext,
                                            webPerson.Gender);
            person.GUID = webPerson.GUID;
            person.HasSpeciesCollection = webPerson.HasSpeciesCollection;
            person.Id = webPerson.Id;
            person.LastName = webPerson.LastName;
            person.Locale = GetLocale(webPerson.Locale);
            person.MiddleName = webPerson.MiddleName;
            person.UpdateInformation.ModifiedBy = webPerson.ModifiedBy;
            person.UpdateInformation.ModifiedDate = webPerson.ModifiedDate;
            person.PhoneNumbers.Clear();
            person.PhoneNumbers.AddRange(GetPhoneNumbers(userContext,
                                                         webPerson.PhoneNumbers));
            person.Presentation = webPerson.Presentation;
            person.ShowAddresses = webPerson.ShowAddresses;
            person.ShowEmailAddress = webPerson.ShowEmailAddress;
            person.ShowPersonalInformation = webPerson.ShowPersonalInformation;
            person.ShowPhoneNumbers = webPerson.ShowPhoneNumbers;
            person.ShowPresentation = webPerson.ShowPresentation;
            person.TaxonNameTypeId = webPerson.TaxonNameTypeId;
            if (webPerson.IsUserIdSpecified)
            {
                person.UserId = webPerson.UserId;
            }
            else
            {
                person.UserId = null;
            }
            person.URL = webPerson.URL;
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
        public void UpdateRole(IUserContext userContext, IRole role)
        {
            WebRole webRole;

            CheckTransaction(userContext);
            webRole = WebServiceProxy.UserService.UpdateRole(GetClientInformation(userContext),
                                                    GetRole(userContext, role));
            UpdateRole(userContext, role, webRole);
        }

        /// <summary>
        /// Update role object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role.</param>
        /// <param name="webRole">Web role.</param>
        private void UpdateRole(IUserContext userContext,
                                IRole role,
                                WebRole webRole)
        {
            if (webRole.IsAdministrationRoleIdSpecified)
            {
                role.AdministrationRoleId = webRole.AdministrationRoleId;
            }
            else
            {
                role.AdministrationRoleId = null;
            }
            role.Authorities.Clear();
            role.Authorities.AddRange(GetAuthorityList(userContext, webRole.Authorities));
            role.UpdateInformation.CreatedBy = webRole.CreatedBy;
            role.UpdateInformation.CreatedDate = webRole.CreatedDate;
            role.DataContext = GetDataContext(userContext);
            role.Description = webRole.Description;
            role.GUID = webRole.GUID;
            role.Id = webRole.Id;
            role.Identifier = webRole.Identifier;
            role.IsActivationRequired = webRole.IsActivationRequired;
            role.IsUserAdministrationRole = webRole.IsUserAdministrationRole;
            role.MessageType = CoreData.UserManager.GetMessageType(userContext, webRole.MessageTypeId);
            role.UpdateInformation.ModifiedBy = webRole.ModifiedBy;
            role.UpdateInformation.ModifiedDate = webRole.ModifiedDate;
            role.ValidFromDate = webRole.ValidFromDate;
            role.ValidToDate = webRole.ValidToDate;
            if (webRole.IsOrganizationIdSpecified)
            {
                role.OrganizationId = webRole.OrganizationId;
            }
            else
            {
                role.OrganizationId = null;
            }
            if (webRole.IsUserAdministrationRoleIdSpecified)
            {
                role.UserAdministrationRoleId = webRole.UserAdministrationRoleId;
            }
            else
            {
                role.UserAdministrationRoleId = null;
            }
            role.Name = webRole.Name;
            role.ShortName = webRole.ShortName;
        }

        /// <summary>
        /// Update user object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">User.</param>
        /// <param name="webUser">Web user.</param>
        private void UpdateUser(IUserContext userContext, 
                                IUser user,
                                WebUser webUser)
        {
            user.IsAccountActivated = webUser.IsAccountActivated;
            user.ApplicationId = null;
            if (webUser.IsApplicationIdSpecified)
            {
                user.ApplicationId = webUser.ApplicationId;
            }
            if (webUser.IsAdministrationRoleIdSpecified)
            {
                user.AdministrationRoleId = webUser.AdministrationRoleId;
            }
            user.ActivationKey = webUser.ActivationKey;
            user.AuthenticationType = webUser.AuthenticationType;
            user.DataContext = GetDataContext(userContext);
            user.EmailAddress = webUser.EmailAddress;
            user.GUID = webUser.GUID;
            user.Id = webUser.Id;
            if (webUser.IsPersonIdSpecified)
            {
                user.PersonId = webUser.PersonId;
            }
            else
            {
                user.PersonId = null;
            }
            user.ShowEmailAddress = webUser.ShowEmailAddress;
            user.Type = webUser.Type;
            user.UpdateInformation = new UpdateInformation();
            user.UpdateInformation.CreatedBy = webUser.CreatedBy;
            user.UpdateInformation.CreatedDate = webUser.CreatedDate;
            user.UpdateInformation.ModifiedBy = webUser.ModifiedBy;
            user.UpdateInformation.ModifiedDate = webUser.ModifiedDate;
            user.UserName = webUser.UserName;
            user.ValidFromDate = webUser.ValidFromDate;
            user.ValidToDate = webUser.ValidToDate;
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
        public void UpdateUser(IUserContext userContext, IUser user)
        {
            WebUser webUser;

            CheckTransaction(userContext);
            webUser = WebServiceProxy.UserService.UpdateUser(GetClientInformation(userContext),
                                                    GetUser(userContext, user));
            UpdateUser(userContext, user, webUser);
        }

        /// <summary>
        /// Updates a user and its associated person. The function only be used by support users.
        /// Only Email and account activation can be changed when the account is inactivated.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="user">Information about the updated user.</param>
        /// <param name="person">Information about the updated person.</param>
        public void SupportUpdatePersonUser(IUserContext userContext, IUser user, IPerson person)
        {
            WebUser webUser;
            WebPerson webPerson;

            CheckTransaction(userContext);
            webUser = WebServiceProxy.UserService.SupportUpdatePersonUser(GetClientInformation(userContext),
                                                    GetUser(userContext, user), GetPerson(userContext, person));
            UpdateUser(userContext, user, webUser);

            webPerson = WebServiceProxy.UserService.GetPerson(GetClientInformation(userContext), webUser.PersonId);
            UpdatePerson(userContext, person, webPerson);
        }

        /// <summary>
        /// Updates a users password without sending the old password.
        /// Used by administrator.
        /// </summary>
        /// <param name="userContext">Web service request context.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="newPassword">New password</param>
        /// <returns>true - if users password is changed
        /// false - if password change failed
        /// </returns>    
        public Boolean UserAdminSetPassword(IUserContext userContext,
                                            IUser user,
                                            String newPassword)
        {
            CheckTransaction(userContext);
            return WebServiceProxy.UserService.UserAdminSetPassword(GetClientInformation(userContext),
                                                           GetUser(userContext, user),
                                                           newPassword);
        }
    }
}
