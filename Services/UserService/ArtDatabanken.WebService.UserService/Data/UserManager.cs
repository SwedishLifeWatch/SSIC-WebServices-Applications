using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Manager of user information.
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// Indicates if any authority related
        /// information has been changed.
        /// </summary>
        private static Boolean _isAuthorityInformationUpdated;

        /// <summary>
        /// Indicates if role related information has been changed.
        /// </summary>
        private static Boolean _isRoleUpdated;

        /// <summary>
        /// Information about users that has failed to login.
        /// </summary>
        private static readonly ConcurrentDictionary<String, LoginInformation> _loginFailedTable;

        /// <summary>
        /// Information about users that has failed to login.
        /// </summary>
        private static readonly ConcurrentDictionary<String, Int32> _userNameToIdMapping;

        /// <summary>
        /// Users that has changed.
        /// </summary>
        private static List<Int32> _changedUserIds;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static UserManager()
        {
            _changedUserIds = new List<Int32>();
            _isAuthorityInformationUpdated = false;
            _isRoleUpdated = false;
            _loginFailedTable = new ConcurrentDictionary<String, LoginInformation>();
            _userNameToIdMapping = new ConcurrentDictionary<String, Int32>();
            ApplicationManager.ApplicationInformationChangeEvent += UpdateIsAuthorityInformationUpdated;
            WebServiceContext.CommitTransactionEvent += RemoveCachedObjects;
        }

        /// <summary>
        /// Activates role membership for an user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleId">Id of role.</param>
        /// <returns>
        /// Returns 'true' if users role membership is activated,
        /// 'false' if user doesn't exist or user is not associated to the role.
        /// </returns>   
        public static Boolean ActivateRoleMembership(WebServiceContext context,
                                                     Int32 roleId)
        {
            Boolean isMembershipActivated;
            Int32 userId;

            context.CheckTransaction();
            userId = GetUserId(context);
            isMembershipActivated = context.GetUserDatabase().ActivateRoleMembership(userId, roleId);
            IsUserChanged(userId);
            return isMembershipActivated;
        }

        /// <summary>
        /// Activates user account.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userName">UserName owner of account to activate.</param>
        /// <param name="activationKey">Activation key.</param>
        /// <returns>
        /// Returns 'true' if users account is activated
        /// 'false' if user doesn't exist or activation key doesn't match.
        /// </returns>   
        public static Boolean ActivateUserAccount(WebServiceContext context,
                                                  String userName,
                                                  String activationKey)
        {
            Boolean isUserActivated;

            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check arguments.
            context.CheckTransaction();
            userName.CheckNotEmpty("userName");
            activationKey.CheckNotEmpty("activationKey");
            activationKey = activationKey.CheckInjection();
            userName = userName.CheckInjection();

            // Activate user.
            isUserActivated = context.GetUserDatabase().ActivateUserAccount(userName, activationKey);

            // Remove cached user information.
            IsUserChanged(userName);

            return isUserActivated;
        }

        /// <summary>
        /// Add user to cache.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="user">User to cache.</param>
        private static void AddUserToCache(WebServiceContext context,
                                           WebUser user)
        {
#if !OLD_WEB_SERVICE_ADDRESS
            ConcurrentDictionary<Int32, Hashtable> userLocaleCache;
            Hashtable userCache;
            String cacheKey;

            if (!context.IsInTransaction())
            {
                // Save user name to user id relation.
                _userNameToIdMapping[user.UserName] = user.Id;

                // Get user cache for all locales.
                cacheKey = GetUserCacheKey(user.Id);
                userLocaleCache = (ConcurrentDictionary<Int32, Hashtable>)context.GetCachedObject(cacheKey);
                if (userLocaleCache.IsNull())
                {
                    userLocaleCache = new ConcurrentDictionary<Int32, Hashtable>();
                    context.AddCachedObject(cacheKey,
                                            userLocaleCache,
                                            DateTime.Now + new TimeSpan(12, 0, 0),
                                            CacheItemPriority.Normal);
                }

                // Get user cache for specified locale.
                if (userLocaleCache.ContainsKey(context.Locale.Id))
                {
                    userCache = userLocaleCache[context.Locale.Id];
                }
                else
                {
                    userCache = Hashtable.Synchronized(new Hashtable());
                    userLocaleCache[context.Locale.Id] = userCache;
                }

                // Save user in cache.
                userCache[user.Id] = user;
            }
#endif
        }

        /// <summary>
        /// Adds a user to a role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        public static void AddUserToRole(WebServiceContext context, Int32 roleId, Int32 userId)
        {
            // Check whether or not the user has the user group administrator role. 
            // Beside super administrators, only user group administrators are currently authorized to run this method.
            if (!AuthorizationManager.IsUserAuthorized(context, Settings.Default.RoleIdForSuperAdministrator, Settings.Default.AuthorityIdentifierForUserGroupAdministration, null, null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotUserGroupAdministrator);
            }

            if (roleId == Settings.Default.RoleIdForSuperAdministrator)
            {
                AuthorizationManager.CheckSuperAdministrator(context);
            }

            context.CheckTransaction();
            context.GetUserDatabase().AddUserToRole(roleId, userId);
            IsUserChanged(userId);
        }

        /// <summary>
        /// Check if a translation string is unique for this object/property and locale.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="value">String value to check.</param>
        /// <param name="objectName">Name of object this string belongs to.</param>
        /// <param name="propertyName">Name of property.</param>
        /// <returns>Boolean - 'true' if string value is unique
        ///                  - 'false' if string value already in database.
        /// </returns>
        public static Boolean CheckStringIsUnique(WebServiceContext context,
                                                  String value,
                                                  String objectName,
                                                  String propertyName)
        {
            // Check data.
            objectName.CheckNotEmpty("objectName");
            objectName = objectName.CheckInjection();
            propertyName.CheckNotEmpty("propertyName");
            propertyName = propertyName.CheckInjection();
            value = value.CheckInjection();

            return context.GetUserDatabase().CheckStringIsUnique(value, context.Locale.Id, objectName, propertyName);
        }

        /// <summary>
        /// Creates a new address.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="address">Object representing the Address.</param>
        public static void CreateAddress(WebServiceContext context, WebAddress address)
        {
            context.CheckTransaction();
            address.CheckData();
            context.GetUserDatabase().CreateAddress(address.PersonId,
                                                    address.OrganizationId,
                                                    address.PostalAddress1,
                                                    address.PostalAddress2,
                                                    address.ZipCode,
                                                    address.City,
                                                    address.Country.Id,
                                                    address.Type.Id);
        }

        /// <summary>
        /// A method that generates an authentication key.
        /// </summary>
        /// <param name="minLength">Minimum length of authentication key.</param>
        /// <param name="maxLength">Maximum length of authentication key.</param>
        /// <returns>An authentication key.</returns>
        public static String CreateAuthenticationKey(Int32 minLength,
                                                     Int32 maxLength)
        {
            PasswordGenerator keyGenerator = new PasswordGenerator();
            keyGenerator.Minimum = minLength;
            keyGenerator.Maximum = maxLength;
            String key = keyGenerator.Generate();

            return key;
        }

        /// <summary>
        /// Creates a new authority. 
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Object representing the authority.</param>
        /// <returns>WebAuthority object with the created authority.</returns>
        public static WebAuthority CreateAuthority(WebServiceContext context,
                                                   WebAuthority authority)
        {
            Int32 authorityId;
            Int32? administrationRoleId, authorityDataTypeId, applicationId;

            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check arguments.
            context.CheckTransaction();
            authority.CheckNotNull("authority");
            authority.CheckData();

            applicationId = null;
            authorityDataTypeId = null;
            if (authority.AuthorityDataType.IsNotNull())
            {
                authorityDataTypeId = authority.AuthorityDataType.Id;
            }
            else
            {
                applicationId = authority.ApplicationId;
            }

            // Verify that authority is valid for either authority data type or application,
            // if not through exception.
            if (authorityDataTypeId.IsNull() && applicationId.IsNull())
            {
                throw new ArgumentException("CreateAuthority: Authority is not set to application or authority data type.");
            }

            administrationRoleId = null;
            if (authority.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = authority.AdministrationRoleId;
            }

            authorityId = context.GetUserDatabase().CreateAuthority(authority.RoleId,
                                                                    applicationId,
                                                                    authority.Identifier,
                                                                    authorityDataTypeId,
                                                                    authority.Name,
                                                                    authority.ShowNonPublicData,
                                                                    authority.MaxProtectionLevel,
                                                                    authority.ReadPermission,
                                                                    authority.CreatePermission,
                                                                    authority.UpdatePermission,
                                                                    authority.DeletePermission,
                                                                    administrationRoleId,
                                                                    authority.Description,
                                                                    authority.Obligation,
                                                                    context.Locale.Id,
                                                                    GetUserId(context),
                                                                    authority.ValidFromDate,
                                                                    authority.ValidToDate);

            authority.Id = authorityId;
            CreateAuthorityAttributes(context, authority);
            _isAuthorityInformationUpdated = true;
            return GetAuthority(context, authorityId);
        }

        /// <summary>
        /// Add attributes to a authority.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Authority with attributes.</param>
        private static void CreateAuthorityAttributes(WebServiceContext context,
                                                      WebAuthority authority)
        {
            if (authority.ActionGUIDs.IsNotEmpty())
            {
                foreach (String attributeGuid in authority.ActionGUIDs)
                {
                    context.GetUserDatabase().CreateAuthorityAttribute(authority.Id,
                                                                       (Int32)AuthorityAttributeTypeId.Actions,
                                                                       attributeGuid);
                }
            }

            if (authority.FactorGUIDs.IsNotEmpty())
            {
                foreach (String attributeGuid in authority.FactorGUIDs)
                {
                    context.GetUserDatabase().CreateAuthorityAttribute(authority.Id,
                                                                       (Int32)AuthorityAttributeTypeId.Factors,
                                                                       attributeGuid);
                }
            }

            if (authority.LocalityGUIDs.IsNotEmpty())
            {
                foreach (String attributeGuid in authority.LocalityGUIDs)
                {
                    context.GetUserDatabase().CreateAuthorityAttribute(authority.Id,
                                                                       (Int32)AuthorityAttributeTypeId.Localities,
                                                                       attributeGuid);
                }
            }

            if (authority.ProjectGUIDs.IsNotEmpty())
            {
                foreach (String attributeGuid in authority.ProjectGUIDs)
                {
                    context.GetUserDatabase().CreateAuthorityAttribute(authority.Id,
                                                                       (Int32)AuthorityAttributeTypeId.Projects,
                                                                       attributeGuid);
                }
            }

            if (authority.RegionGUIDs.IsNotEmpty())
            {
                foreach (String attributeGuid in authority.RegionGUIDs)
                {
                    context.GetUserDatabase().CreateAuthorityAttribute(authority.Id,
                                                                       (Int32)AuthorityAttributeTypeId.Regions,
                                                                       attributeGuid);
                }
            }

            if (authority.TaxonGUIDs.IsNotEmpty())
            {
                foreach (String attributeGuid in authority.TaxonGUIDs)
                {
                    context.GetUserDatabase().CreateAuthorityAttribute(authority.Id,
                                                                       (Int32)AuthorityAttributeTypeId.Taxa,
                                                                       attributeGuid);
                }
            }
        }

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="person">Object representing the Person.</param>
        /// <returns>WebPerson object with the created person.</returns>
        public static WebPerson CreatePerson(WebServiceContext context, WebPerson person)
        {
            // Check whether or not the user has the super administrator role. 
            // Only super administrators are currently authorized to run this method.
            if (!AuthorizationManager.IsUserAuthorized(context, Settings.Default.RoleIdForSuperAdministrator, null, null, null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotSuperAdministrator);
            }

            DateTime? birthYear, deathYear;
            Int32? userId, administrationRoleId;

            // Check arguments.
            context.CheckTransaction();
            person.CheckData();

            birthYear = null;
            if (person.IsBirthYearSpecified)
            {
                birthYear = person.BirthYear;
            }

            deathYear = null;
            if (person.IsDeathYearSpecified)
            {
                deathYear = person.DeathYear;
            }

            userId = null;
            if (person.IsUserIdSpecified)
            {
                userId = person.UserId;
            }

            if (person.EmailAddress.IsNotEmpty() &&
                !IsEmailAddressUnique(context, person.EmailAddress, userId))
            {
                throw new ArgumentException("CreatePerson: Email address already exists. " + person.EmailAddress);
            }

            administrationRoleId = null;
            if (person.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = person.AdministrationRoleId;
            }

            Int32 personId;
            personId = context.GetUserDatabase().CreatePerson(userId,
                                                              person.FirstName,
                                                              person.MiddleName,
                                                              person.LastName,
                                                              person.Gender.Id,
                                                              person.EmailAddress,
                                                              person.ShowEmailAddress,
                                                              person.ShowAddresses,
                                                              person.ShowPhoneNumbers,
                                                              birthYear,
                                                              deathYear,
                                                              administrationRoleId,
                                                              person.HasSpeciesCollection,
                                                              person.Locale.Id,
                                                              person.TaxonNameTypeId,
                                                              person.URL,
                                                              person.Presentation,
                                                              person.ShowPresentation,
                                                              person.ShowPersonalInformation,
                                                              GetUserId(context));

            if (person.Addresses.IsNotEmpty())
            {
                List<WebAddress> addresses = person.Addresses;
                foreach (WebAddress webAddress in addresses)
                {
                    webAddress.PersonId = personId;
                    CreateAddress(context, webAddress);
                }
            }

            if (person.PhoneNumbers.IsNotEmpty())
            {
                List<WebPhoneNumber> phoneNumbers = person.PhoneNumbers;
                foreach (WebPhoneNumber webPhoneNumber in phoneNumbers)
                {
                    webPhoneNumber.PersonId = personId;
                    CreatePhoneNumber(context, webPhoneNumber);
                }
            }

            if (person.IsUserIdSpecified)
            {
                IsUserChanged(person.UserId);
            }

            return GetPerson(context, personId);
        }

        /// <summary>
        /// Creates a new phone number.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="phoneNumber">Object representing the PhoneNumber.</param>
        public static void CreatePhoneNumber(WebServiceContext context, WebPhoneNumber phoneNumber)
        {
            context.CheckTransaction();
            phoneNumber.CheckData();
            context.GetUserDatabase().CreatePhoneNumber(
                phoneNumber.PersonId, phoneNumber.OrganizationId, phoneNumber.Number, phoneNumber.Country.Id, phoneNumber.Type.Id);
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="role">Object representing the Role.</param>
        /// <returns>The new role.</returns>
        public static WebRole CreateRole(WebServiceContext context, WebRole role)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            Int32 roleId;
            Int32? administrationRoleId, userAdministrationRoleId, organizationId;

            // Check arguments.
            context.CheckTransaction();
            role.CheckNotNull("role");
            role.CheckData();

            administrationRoleId = null;
            if (role.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = role.AdministrationRoleId;
            }

            userAdministrationRoleId = null;
            if (role.IsUserAdministrationRoleIdSpecified)
            {
                userAdministrationRoleId = role.UserAdministrationRoleId;
            }

            organizationId = null;
            if (role.IsOrganizationIdSpecified)
            {
                organizationId = role.OrganizationId;
            }

            roleId = context.GetUserDatabase().CreateRole(role.Name,
                                                          role.ShortName,
                                                          role.Description,
                                                          context.Locale.Id,
                                                          administrationRoleId,
                                                          userAdministrationRoleId,
                                                          organizationId,
                                                          GetUserId(context),
                                                          role.ValidFromDate,
                                                          role.ValidToDate,
                                                          role.Identifier,
                                                          role.IsActivationRequired,
                                                          role.MessageTypeId);
            _isAuthorityInformationUpdated = true;
            return GetRole(context, roleId);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="password">User password.</param>
        /// <returns>The new user.</returns>
        public static WebUser CreateUser(WebServiceContext context,
                                         WebUser user,
                                         String password)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            Int32 userId;
            Int32? applicationId, personId, administrationRoleId;

            // Check arguments.
            context.CheckTransaction();
            user.CheckNotNull("user");
            password.CheckNotNull("password");
            password = password.CheckInjection();
            user.CheckData();

            // Check that email address is unique for UserType = Person.
            personId = null;
            if (user.IsPersonIdSpecified)
            {
                personId = user.PersonId;
            }

            if (user.Type.Equals(UserType.Person) &&
                user.EmailAddress.IsNotEmpty() &&
                !IsEmailAddressUnique(context, user.EmailAddress, null, personId))
            {
                throw new ArgumentException("CreateUser: Email address already exists. " + user.EmailAddress);
            }

            // Check password and username   
            // Old password regexp = @"^[a-zåäöA-ZÅÄÖ0-9_]{5,40}$" 
            if (!(Regex.IsMatch(password, Settings.Default.PasswordRegularExpression) &&
                 Regex.IsMatch(user.UserName, Settings.Default.UserNameRegularExpression) &&
                 (password.Length <= WebUserExtension.GetPasswordMaxLength(context)) &&
                 (user.UserName.Length <= WebUserExtension.GetUserNameMaxLength(context))))
            {
                    throw new ArgumentException("CreateUser: userName/password does not meet requirements.");
            }

            applicationId = null;
            if (user.IsApplicationIdSpecified)
            {
                applicationId = user.ApplicationId;
            }

            administrationRoleId = null;
            if (user.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = user.AdministrationRoleId;
            }

            if (user.ActivationKey.IsNull())
            {
                const Int32 FixedLength = 30;
                user.ActivationKey = CreateAuthenticationKey(FixedLength, FixedLength);
            }

            password = CryptationHandler.GetSHA1Hash(password);
            userId = context.GetUserDatabase().CreateUser(user.UserName,
                                                          password,
                                                          personId,
                                                          applicationId,
                                                          user.Type.ToString(),
                                                          user.GUID,
                                                          user.EmailAddress,
                                                          user.ShowEmailAddress,
                                                          user.AuthenticationType,
                                                          user.IsAccountActivated,
                                                          user.ActivationKey,
                                                          administrationRoleId,
                                                          GetUserId(context),
                                                          user.ValidFromDate,
                                                          user.ValidToDate);
            return GetUserById(context, userId);
        }

        /// <summary>
        /// Deletes address.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="personId">Id for person.</param>
        /// <param name="organizationId">Id for organization.</param>
        public static void DeleteAddress(WebServiceContext context, Int32 personId, Int32 organizationId)
        {
            context.CheckTransaction();
            context.GetUserDatabase().DeleteAddress(personId, organizationId);
        }

        /// <summary>
        /// Delete authority.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Object representing the authority.</param>
        public static void DeleteAuthority(WebServiceContext context, WebAuthority authority)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check arguments.
            context.CheckTransaction();
            authority.CheckNotNull("authority");
            authority.CheckData();

            context.GetUserDatabase().DeleteAuthority(authority.Id);
            _isAuthorityInformationUpdated = true;
        }

        /// <summary>
        /// Delete attributes when updating a authority.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Delete attributes for this authority.</param>
        private static void DeleteAuthorityAttributes(WebServiceContext context, WebAuthority authority)
        {
            context.GetUserDatabase().DeleteAttributeValues(authority.Id);
            _isAuthorityInformationUpdated = true;
        }

        /// <summary>
        /// Delete person.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="person">Delete this person.</param>
        public static void DeletePerson(WebServiceContext context, WebPerson person)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            context.CheckTransaction();
            context.GetUserDatabase().DeletePerson(person.Id, GetUserId(context));

            if (person.IsUserIdSpecified)
            {
                IsUserChanged(person.UserId);
            }
        }

        /// <summary>
        /// Delete phone number.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="personId">Id for person.</param>
        /// <param name="organizationId">Id for organization.</param>
        public static void DeletePhoneNumber(WebServiceContext context, Int32 personId, Int32 organizationId)
        {
            context.CheckTransaction();
            context.GetUserDatabase().DeletePhoneNumber(personId, organizationId);
        }

        /// <summary>
        /// Delete role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="role">Delete this role.</param>
        public static void DeleteRole(WebServiceContext context, WebRole role)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);
            
            // try to delete the UserAdminRole for the role to be deleted
            context.CheckTransaction();
            try
            {
                context.GetUserDatabase().DeleteRole(role.UserAdministrationRoleId, GetUserId(context));
            }
            catch (SqlException)
            {
                // failed to delete the UserAdminRole 
                // no need to communicate failure to client.
            }

            // delete role
            context.GetUserDatabase().DeleteRole(role.Id, GetUserId(context));
            _isAuthorityInformationUpdated = true;
            _isRoleUpdated = true;
        }

        /// <summary>
        /// Deleted role members for roles that are not valid.
        /// This method should only be used internally
        /// in the web service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteRoleMembers(WebServiceContext context)
        {
            Int32 deletedCount;

            context.StartTransaction(30);
            deletedCount = context.GetUserDatabase().DeleteRoleMembers();
            _isRoleUpdated = _isRoleUpdated || (0 < deletedCount);
            context.CommitTransaction();
            if (0 < deletedCount)
            {
                WebServiceData.LogManager.Log(context, "Number of members deleted from roles = " + deletedCount, LogType.Information, null);
            }
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="user">Object representing the User.</param>
        public static void DeleteUser(WebServiceContext context, WebUser user)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            context.CheckTransaction();
            context.GetUserDatabase().DeleteUser(user.Id, GetUserId(context));
            IsUserChanged(user.Id);
        }

        /// <summary>
        /// Get addresses for a person.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="personId">Person id.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Addresses for a person.</returns>
        public static List<WebAddress> GetAddresses(WebServiceContext context, Int32 personId, Int32 organizationId)
        {
            List<WebAddress> addresses;
            WebAddress address;

            // Get data from database.
            using (DataReader dataReader = context.GetUserDatabase().GetAddresses(personId, organizationId, context.Locale.Id))
            {
                addresses = new List<WebAddress>();
                while (dataReader.Read())
                {
                    address = new WebAddress();
                    address.LoadData(dataReader);
                    addresses.Add(address);
                }
            }

            foreach (WebAddress webAddress in addresses)
            {
                webAddress.Type = GetAddressType(context, webAddress.TypeId);
                webAddress.Country = CountryManager.GetCountry(context, webAddress.CountryId);
            }

            return addresses;
        }

        /// <summary>
        /// Get cached WebAddressType object.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="addressTypeId">Requested AddressTypeId.</param>
        /// <returns>Returns WebAddressType.</returns>
        private static WebAddressType GetAddressType(WebServiceContext context, Int32 addressTypeId)
        {
            Hashtable addressTypesTable;
            WebAddressType addressType;

            addressTypesTable = GetAddressTypesFromCache(context);
            addressType = (WebAddressType)(addressTypesTable[addressTypeId]);
            return addressType;
        }

        /// <summary>
        /// Get key used when handling the AddressType cache.
        /// </summary>
        /// <param name="localeId">Id representing language.</param>
        /// <returns>The AddressType cache key.</returns>       
        private static String GetAddressTypeCacheKey(Int32 localeId)
        {
            return "WebAddressType:" + localeId;
        }

        /// <summary>
        /// Get list of all Address Types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>A list of Web Address Types.</returns>
        public static List<WebAddressType> GetAddressTypes(WebServiceContext context)
        {
            Hashtable addressTypesCached;
            List<WebAddressType> addressTypes;

            addressTypesCached = GetAddressTypesFromCache(context);
            addressTypes = new List<WebAddressType>();
            if (addressTypesCached.Values.IsNotEmpty())
            {
                foreach (WebAddressType addressType in addressTypesCached.Values)
                {
                    addressTypes.Add(addressType);
                }
            }

            return addressTypes;
        }

        /// <summary>
        /// Get cached WebAddressTypes.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Returns cached WebAddressTypes.</returns>
        private static Hashtable GetAddressTypesFromCache(WebServiceContext context)
        {
            String cacheKey;
            Hashtable addressTypes;
            WebAddressType addressType;

            // Get cached information.
            cacheKey = GetAddressTypeCacheKey(context.Locale.Id);
            addressTypes = (Hashtable)context.GetCachedObject(cacheKey);

            // Data not in cache - store it in the cache
            if (addressTypes.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetAddressTypes(context.Locale.Id))
                {
                    addressTypes = new Hashtable();
                    while (dataReader.Read())
                    {
                        addressType = new WebAddressType();
                        addressType.LoadData(dataReader);

                        // Add object to Hashtable
                        addressTypes.Add(addressType.Id, addressType);
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, addressTypes, DateTime.Now + new TimeSpan(1, 0, 0, 0), CacheItemPriority.High);
                }
            }

            return addressTypes;
        }

        /// <summary>
        /// Get all users of type application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>List of all ApplicationUsers.</returns>
        public static List<WebUser> GetApplicationUsers(WebServiceContext context)
        {
            WebUserSearchCriteria searchCriteria;
            searchCriteria = new WebUserSearchCriteria();
            searchCriteria.IsUserTypeSpecified = true;
            searchCriteria.UserType = UserType.Application;
            return GetUsersBySearchCriteria(context, searchCriteria);
        }

        /// <summary>
        /// Get authorities related to specified application.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="allAuthorities">All authorities for one role.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>Authorities related to specified application.</returns>
        private static List<WebAuthority> GetAuthorities(WebServiceContext context,
                                                         List<WebAuthority> allAuthorities,
                                                         String applicationIdentifier)
        {
            List<WebAuthority> authorities;
            List<WebAuthorityDataType> authorityDataTypes;
            WebApplication application;

            if (allAuthorities.IsEmpty() ||
                applicationIdentifier.IsEmpty())
            {
                return allAuthorities;
            }

            authorities = new List<WebAuthority>();
            application = ApplicationManager.GetApplicationByIdentifier(context, applicationIdentifier);
            if (application.IsNotNull())
            {
                authorityDataTypes = ApplicationManager.GetAuthorityDataTypesByApplicationId(context, application.Id);
                if (allAuthorities.IsNotEmpty())
                {
                    foreach (WebAuthority authority in allAuthorities)
                    {
                        switch (authority.AuthorityType)
                        {
                            case AuthorityType.Application:
                                if (authority.ApplicationId == application.Id)
                                {
                                    authorities.Add(authority);
                                }

                                break;
                            case AuthorityType.DataType:
                                if (authorityDataTypes.IsNotEmpty())
                                {
                                    foreach (WebAuthorityDataType authorityDataType in authorityDataTypes)
                                    {
                                        if (authority.AuthorityDataType.Id == authorityDataType.Id)
                                        {
                                            authorities.Add(authority);
                                        }
                                    }
                                }

                                break;
                            default:
                                throw new Exception("Not handled authority type = " + authority.AuthorityType);
                        }
                    }
                }
            }

            return authorities;
        }

        /// <summary>
        /// Get authorities for specified locale.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Authorities for specified locale.</returns>
        private static ConcurrentDictionary<Int32, List<WebAuthority>> GetAuthorities(WebServiceContext context)
        {
            ConcurrentDictionary<Int32, ConcurrentDictionary<Int32, List<WebAuthority>>> allCachedAuthorities;
            List<WebAuthority> authorities;
            ConcurrentDictionary<Int32, List<WebAuthority>> allAuthorities;
            String cacheKey;
            WebAuthority authority;

#if OLD_WEB_SERVICE_ADDRESS
            allAuthorities = null;
#else
            if (context.IsInTransaction())
            {
                // Do not use cached information.
                // User must get information from database.
                allAuthorities = null;
            }
            else
            {
                // Get cached information.
                cacheKey = Settings.Default.AuthorityCacheKey;
                allCachedAuthorities = (ConcurrentDictionary<Int32, ConcurrentDictionary<Int32, List<WebAuthority>>>)context.GetCachedObject(cacheKey);

                if (allCachedAuthorities.IsNull())
                {
                    // Add information to ASP.NET cache.
                    allCachedAuthorities = new ConcurrentDictionary<Int32, ConcurrentDictionary<Int32, List<WebAuthority>>>();
                    context.AddCachedObject(cacheKey,
                                            allCachedAuthorities,
                                            DateTime.Now + new TimeSpan(24, 0, 0),
                                            CacheItemPriority.Normal);
                }

                lock (allCachedAuthorities)
                {
                    if (allCachedAuthorities.ContainsKey(context.Locale.Id))
                    {
                        allAuthorities = allCachedAuthorities[context.Locale.Id];
                    }
                    else
                    {
                        // Get information from database.
                        allAuthorities = new ConcurrentDictionary<Int32, List<WebAuthority>>();
                        using (DataReader dataReader = context.GetUserDatabase().GetAuthoritiesByRole(null, null, context.Locale.Id))
                        {
                            while (dataReader.Read())
                            {
                                authority = new WebAuthority();
                                authority.LoadData(dataReader);

                                if (allAuthorities.ContainsKey(authority.RoleId))
                                {
                                    authorities = allAuthorities[authority.RoleId];
                                }
                                else
                                {
                                    authorities = new List<WebAuthority>();
                                    allAuthorities[authority.RoleId] = authorities;
                                }

                                authorities.Add(authority);
                            }
                        }

                        foreach (List<WebAuthority> tempAuthorities in allAuthorities.Values)
                        {
                            GetAuthorityAttributes(context, tempAuthorities);
                        }

                        allCachedAuthorities[context.Locale.Id] = allAuthorities;
                    }
                }
            }
#endif
            return allAuthorities;
        }

        /// <summary>
        /// Get authorities for role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="role">Get authorities for this role.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        /// <returns>Authorities for role.</returns>
        private static List<WebAuthority> GetAuthoritiesByRole(WebServiceContext context,
                                                               WebRole role,
                                                               String applicationIdentifier)
        {
            ConcurrentDictionary<Int32, List<WebAuthority>> cachedAuthorities;
            List<WebAuthority> authorities;
            WebAuthority authority;

            // Get cached information.
            cachedAuthorities = GetAuthorities(context);

            if (cachedAuthorities.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetAuthoritiesByRole(role.Id,
                                                                                              applicationIdentifier,
                                                                                              context.Locale.Id))
                {
                    authorities = new List<WebAuthority>();
                    while (dataReader.Read())
                    {
                        authority = new WebAuthority();
                        authority.LoadData(dataReader);
                        authorities.Add(authority);
                    }
                }

                GetAuthorityAttributes(context, authorities);
            }
            else
            {
                if (cachedAuthorities.ContainsKey(role.Id))
                {
                    authorities = cachedAuthorities[role.Id];
                    authorities = GetAuthorities(context,
                                                 authorities,
                                                 applicationIdentifier);
                }
                else
                {
                    // Role with no authorities.
                    authorities = new List<WebAuthority>();
                }
            }

            return authorities;
        }

        /// <summary>
        /// Get authorities for specified roles.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roles">Get authorities for these roles.</param>
        /// <param name="applicationIdentifier">Application identifier.</param>
        private static void GetAuthoritiesByRoles(WebServiceContext context, List<WebRole> roles, String applicationIdentifier)
        {
            if (roles.IsNotEmpty())
            {
                foreach (WebRole webRole in roles)
                {
                    webRole.Authorities = GetAuthoritiesByRole(context,
                                                               webRole,
                                                               applicationIdentifier);
                }
            }
        }

        /// <summary>
        /// Get authorities that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Authorities that matches the search criteria.</returns>
        public static List<WebAuthority> GetAuthoritiesBySearchCriteria(WebServiceContext context,
                                                                        WebAuthoritySearchCriteria searchCriteria)
        {
            List<WebAuthority> authorities;
            WebAuthority authority;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetAuthoritiesBySearchCriteria(searchCriteria.AuthorityIdentifier,
                                                                                                    searchCriteria.ApplicationIdentifier,
                                                                                                    searchCriteria.AuthorityDataTypeIdentifier,
                                                                                                    searchCriteria.AuthorityName,
                                                                                                    context.Locale.Id))
            {
                authorities = new List<WebAuthority>();
                while (dataReader.Read())
                {
                    authority = new WebAuthority();
                    authority.LoadData(dataReader);
                    authorities.Add(authority);
                }
            }

            GetAuthorityAttributes(context, authorities);
            return authorities;
        }

        /// <summary>
        /// Get specified authority.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorityId">Authority id.</param>
        /// <returns>Specified authority.</returns>
        public static WebAuthority GetAuthority(WebServiceContext context, Int32 authorityId)
        {
            WebAuthority authority;

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetAuthority(authorityId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    authority = new WebAuthority();
                    authority.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Authority not found. AuthorityId = " + authorityId);
                }
            }

            GetAuthorityAttributes(context, authority);
            return authority;
        }

        /// <summary>
        /// Get authority attributes for specified authority.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Authority that should be filled with attributes.</param>
        private static void GetAuthorityAttributes(WebServiceContext context,
                                                   WebAuthority authority)
        {
            List<WebAuthorityAttribute> authorityAttributes;
            WebAuthorityAttribute authorityAttribute;

            // Get authority attributes from cache.
            authorityAttributes = GetAuthorityAttributesFromCache(context, authority);

            if (authorityAttributes.IsNull())
            {
                // Get authority attributes from database.
                authorityAttributes = new List<WebAuthorityAttribute>();
                using (DataReader dataReader = context.GetUserDatabase().GetAuthorityAttributes(authority.Id, context.Locale.Id))
                {
                    while (dataReader.Read())
                    {
                        authorityAttribute = new WebAuthorityAttribute();
                        authorityAttribute.LoadData(dataReader);
                        authorityAttributes.Add(authorityAttribute);
                    }
                }
            }

            // Insert authority attributes into authority.
            if (authorityAttributes.IsNotEmpty())
            {
                foreach (WebAuthorityAttribute webAuthorityAttribute in authorityAttributes)
                {
                    switch (webAuthorityAttribute.TypeId)
                    {
                        case (Int32)AuthorityAttributeTypeId.Projects:
                            if (authority.ProjectGUIDs.IsNull())
                            {
                                authority.ProjectGUIDs = new List<String>();
                            }

                            authority.ProjectGUIDs.Add(webAuthorityAttribute.Guid);
                            break;
                        case (Int32)AuthorityAttributeTypeId.Factors:
                            if (authority.FactorGUIDs.IsNull())
                            {
                                authority.FactorGUIDs = new List<String>();
                            }

                            authority.FactorGUIDs.Add(webAuthorityAttribute.Guid);
                            break;
                        case (Int32)AuthorityAttributeTypeId.Regions:
                            if (authority.RegionGUIDs.IsNull())
                            {
                                authority.RegionGUIDs = new List<String>();
                            }

                            authority.RegionGUIDs.Add(webAuthorityAttribute.Guid);
                            break;
                        case (Int32)AuthorityAttributeTypeId.Taxa:
                            if (authority.TaxonGUIDs.IsNull())
                            {
                                authority.TaxonGUIDs = new List<String>();
                            }

                            authority.TaxonGUIDs.Add(webAuthorityAttribute.Guid);
                            break;
                        case (Int32)AuthorityAttributeTypeId.Actions:
                            if (authority.ActionGUIDs.IsNull())
                            {
                                authority.ActionGUIDs = new List<String>();
                            }

                            authority.ActionGUIDs.Add(webAuthorityAttribute.Guid);
                            break;
                        case (Int32)AuthorityAttributeTypeId.Localities:
                            if (authority.LocalityGUIDs.IsNull())
                            {
                                authority.LocalityGUIDs = new List<String>();
                            }

                            authority.LocalityGUIDs.Add(webAuthorityAttribute.Guid);
                            break;
                        default:
                            throw new ApplicationException("Not handled authority type. Type = " + webAuthorityAttribute.TypeId);
                    }
                }
            }
        }

        /// <summary>
        /// Get authority attributes for specified authority.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Authority that should be filled with attributes.</param>
        /// <returns>Authority attributes for specified authority.</returns>
        private static List<WebAuthorityAttribute> GetAuthorityAttributesFromCache(WebServiceContext context,
                                                                                   WebAuthority authority)
        {
            ConcurrentDictionary<Int32, List<WebAuthorityAttribute>> allAuthorityAttributes;
            List<WebAuthorityAttribute> authorityAttributes;
            String cacheKey;
            WebAuthorityAttribute authorityAttribute;

#if OLD_WEB_SERVICE_ADDRESS
            authorityAttributes = null;
#else
            if (context.IsInTransaction())
            {
                // Do not use cached information.
                // User must get information from database.
                authorityAttributes = null;
            }
            else
            {
                // Get cached information.
                cacheKey = Settings.Default.AuthorityAttributeCacheKey;
                allAuthorityAttributes = (ConcurrentDictionary<Int32, List<WebAuthorityAttribute>>)context.GetCachedObject(cacheKey);

                if (allAuthorityAttributes.IsNull())
                {
                    // Get information from database.
                    allAuthorityAttributes = new ConcurrentDictionary<Int32, List<WebAuthorityAttribute>>();
                    using (DataReader dataReader = context.GetUserDatabase().GetAuthorityAttributes(null, context.Locale.Id))
                    {
                        while (dataReader.Read())
                        {
                            authorityAttribute = new WebAuthorityAttribute();
                            authorityAttribute.LoadData(dataReader);

                            if (allAuthorityAttributes.ContainsKey(authorityAttribute.AuthorityId))
                            {
                                authorityAttributes = allAuthorityAttributes[authorityAttribute.AuthorityId];
                            }
                            else
                            {
                                authorityAttributes = new List<WebAuthorityAttribute>();
                                allAuthorityAttributes[authorityAttribute.AuthorityId] = authorityAttributes;
                            }

                            authorityAttributes.Add(authorityAttribute);
                        }
                    }

                    // Add information to ASP.NET cache.
                    context.AddCachedObject(cacheKey,
                                            allAuthorityAttributes,
                                            DateTime.Now + new TimeSpan(24, 0, 0),
                                            CacheItemPriority.Normal);
                }

                if (allAuthorityAttributes.ContainsKey(authority.Id))
                {
                    authorityAttributes = allAuthorityAttributes[authority.Id];
                }
                else
                {
                    authorityAttributes = new List<WebAuthorityAttribute>();
                }
            }
#endif

            return authorityAttributes;
        }

        /// <summary>
        /// Get authority attributes for specified authorities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorities">Authorities that should be filled with attributes.</param>
        private static void GetAuthorityAttributes(WebServiceContext context,
                                                   List<WebAuthority> authorities)
        {
            if (authorities.IsNotEmpty())
            {
                foreach (WebAuthority authority in authorities)
                {
                    GetAuthorityAttributes(context, authority);
                }
            }
        }

        /// <summary>
        /// Get all authority attribute types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All authority attribute types.</returns>
        public static List<WebAuthorityAttributeType> GetAuthorityAttributeTypes(WebServiceContext context)
        {
            List<WebAuthorityAttributeType> authorityAttributeTypes;
            WebAuthorityAttributeType authorityAttributeType;

            if (context.IsInTransaction())
            {
                authorityAttributeTypes = null;
            }
            else
            {
                // Get cached information.
                authorityAttributeTypes = (List<WebAuthorityAttributeType>)context.GetCachedObject(Settings.Default.AuthorityAttributeTypeCacheKey);
            }

            if (authorityAttributeTypes.IsNull())
            {
                // Data not in cache. Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetAuthorityAttributeTypeList())
                {
                    authorityAttributeTypes = new List<WebAuthorityAttributeType>();
                    while (dataReader.Read())
                    {
                        authorityAttributeType = new WebAuthorityAttributeType();
                        authorityAttributeType.LoadData(dataReader);

                        authorityAttributeTypes.Add(authorityAttributeType);
                    }

                    // Add information to cache.
                    context.AddCachedObject(Settings.Default.AuthorityAttributeTypeCacheKey,
                                            authorityAttributeTypes,
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.High);
                }
            }

            return authorityAttributeTypes;
        }

        /// <summary>
        /// Get all authority data types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All authority data types.</returns>
        public static List<WebAuthorityDataType> GetAuthorityDataTypes(WebServiceContext context)
        {
            List<WebAuthorityDataType> authorityDataTypes;
            String cacheKey;
            WebAuthorityDataType authorityDataType;

            // Get cached information.
            cacheKey = Settings.Default.AuthorityDataTypeCacheKey;
            authorityDataTypes = (List<WebAuthorityDataType>)context.GetCachedObject(cacheKey);

            if (authorityDataTypes.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetAuthorityDataTypes())
                {
                    authorityDataTypes = new List<WebAuthorityDataType>();
                    while (dataReader.Read())
                    {
                        authorityDataType = new WebAuthorityDataType();
                        authorityDataType.LoadData(dataReader);
                        authorityDataTypes.Add(authorityDataType);
                    }
                }

                // Add information to ASP.NET cache.
                context.AddCachedObject(cacheKey,
                                        authorityDataTypes,
                                        DateTime.Now + new TimeSpan(24, 0, 0),
                                        CacheItemPriority.AboveNormal);
            }

            return authorityDataTypes;
        }

        /// <summary>
        /// Get information about users that are currently locked out
        /// from ArtDatabankenSOA.
        /// Users are locked out if the fail to login a couple of times.
        /// All currently locked out users are returned if parameter
        /// userNameSearchString is null.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userNameSearchString">
        /// String used to search among user names.
        /// Currently only string compare operator 'Like' is supported.
        /// </param>
        /// <returns>Information about users that are currently locked out from ArtDatabankenSOA.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static List<WebLockedUserInformation> GetLockedUserInformation(WebServiceContext context,
                                                                              WebStringSearchCriteria userNameSearchString)
        {
            List<WebLockedUserInformation> lockedUsers;
            TimeSpan timeSpan;
            WebLockedUserInformation lockedUser;

            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check data.
            if (userNameSearchString.IsNotNull())
            {
                userNameSearchString.SearchString = userNameSearchString.SearchString.CheckInjection();
            }

            lockedUsers = new List<WebLockedUserInformation>();
            if ((_loginFailedTable.Values != null) && (_loginFailedTable.Values.Count >= 1))
            {
                foreach (LoginInformation loginInformation in _loginFailedTable.Values)
                {
                    if (userNameSearchString.IsNotNull() &&
                        userNameSearchString.SearchString.IsNotEmpty() &&
                        (!loginInformation.UserName.Contains(userNameSearchString.SearchString)))
                    {
                        // No match on user name search string.
                        continue;
                    }

                    if (loginInformation.LoginAttemptCount >= Settings.Default.MaxLoginAttempt)
                    {
                        timeSpan = DateTime.Now - loginInformation.LastLogin;
                        if (timeSpan.TotalMinutes <= Settings.Default.MinFailedLoginWaitTime)
                        {
                            // User is locked.
                            lockedUser = new WebLockedUserInformation();
                            lockedUser.LockedFrom = loginInformation.LastLogin;
                            lockedUser.LockedTo = loginInformation.LastLogin + new TimeSpan(0, Settings.Default.MinFailedLoginWaitTime, 0);
                            lockedUser.LoginAttemptCount = loginInformation.LoginAttemptCount;
                            lockedUser.UserName = loginInformation.UserName;
                            lockedUsers.Add(lockedUser);
                        }
                    }
                }
            }

            return lockedUsers;
        }

        /// <summary>
        /// Get key used when handling the MessageType cache.
        /// </summary>
        /// <param name="localeId">Id representing language.</param>
        /// <returns>The MessageType cache key.</returns>       
        private static String GetMessageTypeCacheKey(Int32 localeId)
        {
            return "WebMessageType:" + localeId;
        }

        /// <summary>
        /// Get list of all Message Types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>A list of Web Message Types.</returns>
        public static List<WebMessageType> GetMessageTypes(WebServiceContext context)
        {
            String cacheKey;
            List<WebMessageType> messageTypes;
            WebMessageType messageType;

            // Get cached information.
            cacheKey = GetMessageTypeCacheKey(context.Locale.Id);
            messageTypes = (List<WebMessageType>)context.GetCachedObject(cacheKey);

            // Data not in cache - store it in the cache
            if (messageTypes.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetMessageTypes(context.Locale.Id))
                {
                    messageTypes = new List<WebMessageType>();
                    while (dataReader.Read())
                    {
                        messageType = new WebMessageType();
                        messageType.LoadData(dataReader);
                        messageTypes.Add(messageType);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey, messageTypes, DateTime.Now + new TimeSpan(1, 0, 0, 0), CacheItemPriority.High);
            }

            return messageTypes;
        }

        /// <summary>
        /// Get list of Users associated with a Role that has not activated their role membership.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Returns list of users or 
        /// null if roleid doesn't match or if role has no members.
        /// </returns>
        public static List<WebUser> GetNonActivatedUsersByRole(WebServiceContext context, Int32 roleId)
        {
            List<WebUser> roleMembers;
            WebUser user;

            // Get data from database.
            using (DataReader dataReader = context.GetUserDatabase().GetNonActivatedUsersByRole(roleId))
            {
                roleMembers = new List<WebUser>();
                while (dataReader.Read())
                {
                    user = new WebUser();
                    user.LoadData(dataReader);
                    roleMembers.Add(user);
                }
            }
            return roleMembers;
        }

        /// <summary>
        /// Get information about a person
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="personId">Person id.</param>
        /// <returns>Returns person information or null if 
        ///          personid doesn't match.</returns>
        public static WebPerson GetPerson(WebServiceContext context, Int32 personId)
        {
            WebPerson person;

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetPerson(personId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    person = new WebPerson();
                    person.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Person not found. PersonId = " + personId);
                }
            }

            person.Gender = GetPersonGender(context, person.GenderId);
            person.Addresses = GetAddresses(context, person.Id, 0);
            person.PhoneNumbers = GetPhoneNumbers(context, person.Id, 0);
            person.Locale = LocaleManager.GetLocale(context, person.LocaleISOCode);
            return person;
        }

        /// <summary>
        /// Get cached WebPersonGender object.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="personGenderId">Requested PersonGenderId</param>
        /// <returns>Returns WebPersonGender</returns>
        private static WebPersonGender GetPersonGender(WebServiceContext context, Int32 personGenderId)
        {
            Hashtable personGendersTable;
            WebPersonGender personGender;

            personGendersTable = GetPersonGendersFromCache(context);
            personGender = (WebPersonGender)(personGendersTable[personGenderId]);
            return personGender;
        }

        /// <summary>
        /// Get key used when handling the Person Gender cache.
        /// </summary>
        /// <param name="localeId">Id representing language.</param>
        /// <returns>The Person Gender cache key.</returns>       
        private static String GetPersonGenderCacheKey(Int32 localeId)
        {
            return "WebPersonGender:" + localeId;
        }

        /// <summary>
        /// Get list of all Person Genders.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>A list of Person Gender.</returns>
        public static List<WebPersonGender> GetPersonGenders(WebServiceContext context)
        {
            Hashtable personGendersCached;
            List<WebPersonGender> personGenders;

            personGendersCached = GetPersonGendersFromCache(context);
            personGenders = new List<WebPersonGender>();
            if (personGendersCached.Values.IsNotEmpty())
            {
                foreach (WebPersonGender personGender in personGendersCached.Values)
                {
                    personGenders.Add(personGender);
                }
            }

            return personGenders;
        }

        /// <summary>
        /// Get cached WebPersonGender object.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Returns WebPersonGender</returns>
        private static Hashtable GetPersonGendersFromCache(WebServiceContext context)
        {
            String cacheKey;
            Hashtable personGenders;
            WebPersonGender personGender;

            // Get cached information.
            cacheKey = GetPersonGenderCacheKey(context.Locale.Id);
            personGenders = (Hashtable)context.GetCachedObject(cacheKey);

            // Data not in cache - store it in the cache
            if (personGenders.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetPersonGenders(context.Locale.Id))
                {
                    personGenders = new Hashtable();
                    while (dataReader.Read())
                    {
                        personGender = new WebPersonGender();
                        personGender.LoadData(dataReader);

                        // Add object to Hashtable
                        personGenders.Add(personGender.Id, personGender);
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, personGenders, DateTime.Now + new TimeSpan(1, 0, 0, 0), CacheItemPriority.High);
                }
            }

            return personGenders;
        }

        /// <summary>
        /// Get persons that have been modfied after and before certain dates.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="modifiedFromDate">Date after which person object have been modified.</param>
        /// <param name="modifiedUntilDate">Date before which person object have been modified.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public static List<WebPerson> GetPersonsByModifiedDate(WebServiceContext context,
                                                               DateTime modifiedFromDate, DateTime modifiedUntilDate)
        {
            List<WebPerson> persons;
            WebPerson person;

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetPersonsByModifiedDate(modifiedFromDate,
                                                                                              modifiedUntilDate,
                                                                                              context.Locale.Id))
            {
                persons = new List<WebPerson>();
                while (dataReader.Read())
                {
                    person = new WebPerson();
                    person.LoadData(dataReader);
                    persons.Add(person);
                }
            }

            foreach (WebPerson tempPerson in persons)
            {
                tempPerson.Gender = GetPersonGender(context, tempPerson.GenderId);
                tempPerson.Addresses = GetAddresses(context, tempPerson.Id, 0);
                tempPerson.PhoneNumbers = GetPhoneNumbers(context, tempPerson.Id, 0);
                tempPerson.Locale = LocaleManager.GetLocale(context, tempPerson.LocaleISOCode);
            }

            return persons;
        }

        /// <summary>
        /// Get persons that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Persons that matches the search criteria</returns>
        public static List<WebPerson> GetPersonsBySearchCriteria(WebServiceContext context,
                                                                 WebPersonSearchCriteria searchCriteria)
        {
            List<WebPerson> persons;
            WebPerson person;
            Boolean? hasSpiecesCollection;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();

            hasSpiecesCollection = null;
            if (searchCriteria.IsHasSpeciesCollectionSpecified)
            {
                hasSpiecesCollection = searchCriteria.HasSpeciesCollection;
            }

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetPersonsBySearchCriteria(searchCriteria.FullName,
                                                                                                searchCriteria.FirstName,
                                                                                                searchCriteria.LastName,
                                                                                                hasSpiecesCollection,
                                                                                                context.Locale.Id))
            {
                persons = new List<WebPerson>();
                while (dataReader.Read())
                {
                    person = new WebPerson();
                    person.LoadData(dataReader);
                    persons.Add(person);
                }
            }

            foreach (WebPerson tempPerson in persons)
            {
                tempPerson.Gender = GetPersonGender(context, tempPerson.GenderId);
                tempPerson.Addresses = GetAddresses(context, tempPerson.Id, 0);
                tempPerson.PhoneNumbers = GetPhoneNumbers(context, tempPerson.Id, 0);
                tempPerson.Locale = LocaleManager.GetLocale(context, tempPerson.LocaleISOCode);
            }

            return persons;
        }

        /// <summary>
        /// Get list of PhoneNumbers for a person
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="personId">Person id.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Returns list of phonenumbers or null if 
        ///          personid doesn't match.</returns>
        public static List<WebPhoneNumber> GetPhoneNumbers(WebServiceContext context, Int32 personId, Int32 organizationId)
        {
            List<WebPhoneNumber> phoneNumbers;
            WebPhoneNumber phoneNumber;

            // Get data from database.
            using (DataReader dataReader = context.GetUserDatabase().GetPhoneNumbers(personId, organizationId))
            {
                phoneNumbers = new List<WebPhoneNumber>();
                while (dataReader.Read())
                {
                    phoneNumber = new WebPhoneNumber();
                    phoneNumber.LoadData(dataReader);
                    phoneNumbers.Add(phoneNumber);
                }
            }

            foreach (WebPhoneNumber webPhoneNumber in phoneNumbers)
            {
                webPhoneNumber.Type = GetPhoneNumberType(context, webPhoneNumber.TypeId);
                webPhoneNumber.Country = CountryManager.GetCountry(context, webPhoneNumber.CountryId);
            }
            return phoneNumbers;
        }

        /// <summary>
        /// Get cached WebPhoneNumberType object.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="phoneNumberTypeId">Requested PhoneNumberTypeId</param>
        /// <param name="localeId">Id representing language.</param>
        /// <returns>Returns WebPhoneNumberType</returns>
        private static WebPhoneNumberType GetPhoneNumberType(WebServiceContext context, Int32 phoneNumberTypeId)
        {
            Hashtable phoneNumberTypesTable;
            WebPhoneNumberType phoneNumberType;

            phoneNumberTypesTable = GetPhoneNumberTypesFromCache(context);
            phoneNumberType = (WebPhoneNumberType)(phoneNumberTypesTable[phoneNumberTypeId]);
            return phoneNumberType;
        }

        /// <summary>
        /// Get key used when handling the PhoneNumberType cache.
        /// </summary>
        /// <param name="localeId">Id representing language.</param>
        /// <returns>The AddressType cache key.</returns>       
        private static String GetPhoneNumberTypeCacheKey(Int32 localeId)
        {
            return "WebPhoneNumberType:" + localeId;
        }

        /// <summary>
        /// Get list of all Phone Number Types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>A list of Web Phone Number Types.</returns>
        public static List<WebPhoneNumberType> GetPhoneNumberTypes(WebServiceContext context)
        {
            Hashtable phoneNumberTypesCached;
            List<WebPhoneNumberType> phoneNumberTypes;

            phoneNumberTypesCached = GetPhoneNumberTypesFromCache(context);
            phoneNumberTypes = new List<WebPhoneNumberType>();
            if (phoneNumberTypesCached.Values.IsNotEmpty())
            {
                foreach (WebPhoneNumberType phoneNumberType in phoneNumberTypesCached.Values)
                {
                    phoneNumberTypes.Add(phoneNumberType);
                }
            }

            return phoneNumberTypes;
        }

        /// <summary>
        /// Get cached WebPhoneNumberTypes object.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Returns WebPhoneNumberTypes</returns>
        private static Hashtable GetPhoneNumberTypesFromCache(WebServiceContext context)
        {
            String cacheKey;
            Hashtable phoneNumberTypesTable;
            WebPhoneNumberType phoneNumberType;

            // Get cached information.
            cacheKey = GetPhoneNumberTypeCacheKey(context.Locale.Id);
            phoneNumberTypesTable = (Hashtable)context.GetCachedObject(cacheKey);

            // Data not in cache - store it in the cache
            if (phoneNumberTypesTable.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetAddressTypes(context.Locale.Id))
                {
                    phoneNumberTypesTable = new Hashtable();
                    while (dataReader.Read())
                    {
                        phoneNumberType = new WebPhoneNumberType();
                        phoneNumberType.LoadData(dataReader);

                        // Add object to Hashtable
                        phoneNumberTypesTable.Add(phoneNumberType.Id, phoneNumberType);
                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, phoneNumberTypesTable, DateTime.Now + new TimeSpan(1, 0, 0, 0), CacheItemPriority.High);
                }
            }

            return phoneNumberTypesTable;
        }

        /// <summary>
        /// Get specified role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Specified role.</returns>
        public static WebRole GetRole(WebServiceContext context,
                                      Int32 roleId)
        {
            WebRole role;

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetRole(roleId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    role = new WebRole();
                    role.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Role not found. RoleId = " + roleId);
                }
            }

            // Get all authorities for this role
            role.Authorities = GetAuthoritiesByRole(context, role, null);
            return role;
        }

        /// <summary>
        /// Get roles that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Roles that matches the search criteria.</returns>
        public static List<WebRole> GetRolesBySearchCriteria(WebServiceContext context,
                                                             WebRoleSearchCriteria searchCriteria)
        {
            Int32? organizationId;
            List<WebRole> roles;
            WebRole role;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();

            organizationId = null;
            if (searchCriteria.IsOrganizationIdSpecified)
            {
                organizationId = searchCriteria.OrganizationId;
            }

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetRolesBySearchCriteria(searchCriteria.Name,
                                                                                              searchCriteria.ShortName,
                                                                                              searchCriteria.Identifier,
                                                                                              organizationId,
                                                                                              context.Locale.Id))
            {
                roles = new List<WebRole>();
                while (dataReader.Read())
                {
                    role = new WebRole();
                    role.LoadData(dataReader);
                    roles.Add(role);
                }
            }

            GetAuthoritiesByRoles(context, roles, null);

            return roles;
        }

        /// <summary>
        /// Get role members that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Role members that matches the search criteria.</returns>
        public static List<WebRoleMember> GetRoleMembersBySearchCriteria(WebServiceContext context,
                                                                         WebRoleMemberSearchCriteria searchCriteria)
        {
            List<WebRoleMember> roleMembers;
            WebRole role;
            WebUser user;
            WebRoleMember roleMember;
            Boolean? isActivated;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();

            isActivated = null;
            if (searchCriteria.IsIsActivatedSpecified)
            {
                isActivated = searchCriteria.IsActivated;
            }

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetRoleMembersBySearchCriteria(searchCriteria.RoleIds,
                                                                                                     searchCriteria.UserIds,
                                                                                                     isActivated,
                                                                                                     context.Locale.Id))
            {
                roleMembers = new List<WebRoleMember>();
                while (dataReader.Read())
                {
                    roleMember = new WebRoleMember();
                    role = new WebRole();
                    role.LoadData(dataReader);
                    roleMember.Role = role;

                    user = new WebUser();
                    user.LoadRoleMemberUserData(dataReader);
                    roleMember.User = user;

                    roleMember.IsActivated = dataReader.GetBoolean("IsActivated");
                    roleMembers.Add(roleMember);
                }
            }

            // Add the authorities for the roles. Store the authorities in local cache to prevent to many db roundtrips.
            Hashtable localAuthorityCache = new Hashtable();
            foreach (WebRoleMember webRoleMember in roleMembers)
            {
                if (!localAuthorityCache.ContainsKey(webRoleMember.Role.Id))
                {
                    localAuthorityCache[webRoleMember.Role.Id] = GetAuthoritiesByRole(context, webRoleMember.Role, null);
                }

                webRoleMember.Role.Authorities = (List<WebAuthority>)localAuthorityCache[webRoleMember.Role.Id];
            }

            return roleMembers;
        }

        /// <summary>
        /// Get roles with authorities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="allRoles">All roles without authorities.</param>
        /// <param name="applicationIdentifier">
        /// Restrict returned roles to those that are related
        /// to the application with this identifier.
        /// All roles are returned if application
        /// identifier is not specified.
        /// </param>
        /// <returns>Roles for a user.</returns>
        private static List<WebRole> GetRolesWithAuthorities(WebServiceContext context,
                                                             List<WebRole> allRoles,
                                                             String applicationIdentifier)
        {
            List<WebRole> roles;
            WebRole role;

            if (allRoles.IsEmpty())
            {
                return allRoles;
            }
            else
            {
                roles = new List<WebRole>();
                foreach (WebRole tempRole in allRoles)
                {
                    role = tempRole.Clone();
                    role.Authorities = GetAuthoritiesByRole(context,
                                                            role,
                                                            applicationIdentifier);
                    if (applicationIdentifier.IsEmpty() ||
                        role.Authorities.IsNotEmpty())
                    {
                        roles.Add(role);
                    }
                }

                return roles;
            }
        }

        /// <summary>
        /// Get cached roles for a user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Roles for a user.</returns>
        private static List<WebRole> GetRoles(WebServiceContext context,
                                              Int32 userId)
        {
            ConcurrentDictionary<Int32, Hashtable> userLocaleCache;
            Hashtable userCache;
            List<WebRole> userRoles;
            String cacheKey;
            WebRole role;

#if OLD_WEB_SERVICE_ADDRESS
            userRoles = null;
#else
            if (context.IsInTransaction())
            {
                // Do not use cached information.
                // User must get information from database.
                userRoles = null;
            }
            else
            {
                // Get user cache for all locales.
                cacheKey = GetUserCacheKey(userId);
                userLocaleCache = (ConcurrentDictionary<Int32, Hashtable>)context.GetCachedObject(cacheKey);
                if (userLocaleCache.IsNull())
                {
                    userLocaleCache = new ConcurrentDictionary<Int32, Hashtable>();
                    context.AddCachedObject(cacheKey,
                                            userLocaleCache,
                                            DateTime.Now + new TimeSpan(12, 0, 0),
                                            CacheItemPriority.Normal);
                }

                // Get user cache for specified locale.
                if (userLocaleCache.ContainsKey(context.Locale.Id))
                {
                    userCache = userLocaleCache[context.Locale.Id];
                }
                else
                {
                    userCache = Hashtable.Synchronized(new Hashtable());
                    userLocaleCache[context.Locale.Id] = userCache;
                }

                cacheKey = Settings.Default.RoleCacheKey;
                if (userCache.Contains(cacheKey))
                {
                    userRoles = (List<WebRole>)(userCache[cacheKey]);
                }
                else
                {
                    // Get data from database.
                    using (DataReader dataReader = context.GetUserDatabase().GetRolesByUser(userId,
                                                                                            null,
                                                                                            context.Locale.Id))
                    {
                        userRoles = new List<WebRole>();
                        while (dataReader.Read())
                        {
                            role = new WebRole();
                            role.LoadData(dataReader);
                            userRoles.Add(role);
                        }
                    }

                    // Save roles in cache.
                    userCache[cacheKey] = userRoles;
                }
            }
#endif
            return userRoles;
        }

        /// <summary>
        /// Get roles for a User.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userId">User id.</param>
        /// <param name="applicationIdentifier">
        /// Restrict returned roles to those that are related
        /// to the application with this identifier.
        /// All roles for user are returned if application
        /// identifier is not specified.
        /// </param>
        /// <returns>Roles for a user.</returns>
        public static List<WebRole> GetRolesByUser(WebServiceContext context,
                                                   Int32 userId,
                                                   String applicationIdentifier)
        {
            List<WebRole> roles;
            WebRole role;

            // Check data.
            applicationIdentifier = applicationIdentifier.CheckInjection();

            // Get cached information.
            roles = GetRoles(context, userId);

            if (roles.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetRolesByUser(userId, applicationIdentifier, context.Locale.Id))
                {
                    roles = new List<WebRole>();
                    while (dataReader.Read())
                    {
                        role = new WebRole();
                        role.LoadData(dataReader);
                        roles.Add(role);
                    }
                }

                GetAuthoritiesByRoles(context,
                                      roles,
                                      applicationIdentifier);
            }
            else
            {
                roles = GetRolesWithAuthorities(context,
                                                roles,
                                                applicationIdentifier);
            }

            return roles;
        }

        /// <summary>
        /// Get list of roles associated with a user group admnistration role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleId">User Group Administration Role Id.</param>
        public static List<WebRole> GetRolesByUserGroupAdministrationRoleId(WebServiceContext context, Int32 roleId)
        {
            List<WebRole> roles;
            WebRole role;

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetRolesByUserGroupAdministrationRoleId(roleId, context.Locale.Id))
            {
                roles = new List<WebRole>();
                while (dataReader.Read())
                {
                    role = new WebRole();
                    role.LoadData(dataReader);
                    roles.Add(role);
                }
            }

            GetAuthoritiesByRoles(context, roles, null);

            return roles;
        }

        /// <summary>
        /// Get roles associated with a user group administrator.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userId">User id for user group administrator.</param>
        /// <returns>Roles associated with a user group administrator.</returns>
        public static List<WebRole> GetRolesByUserGroupAdministratorUserId(WebServiceContext context, Int32 userId)
        {
            List<WebRole> roles;
            WebRole role;

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetRolesByUserGroupAdministratorUserId(userId, context.Locale.Id))
            {
                roles = new List<WebRole>();
                while (dataReader.Read())
                {
                    role = new WebRole();
                    role.LoadData(dataReader);
                    roles.Add(role);
                }
            }

            GetAuthoritiesByRoles(context, roles, null);

            return roles;
        }

        /// <summary>
        /// Get information about current web service user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about current web service user.</returns>
        public static WebUser GetUser(WebServiceContext context)
        {
            return GetUserByName(context, context.ClientToken.UserName);
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userId">User id.</param>
        /// <returns>
        /// Returns user information or exception if user id doesn't match.
        /// </returns>
        public static WebUser GetUserById(WebServiceContext context,
                                          Int32 userId)
        {
            WebUser user;

            // Get cached information.
            user = GetUserFromCache(context, userId);
            if (user.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetUser(userId))
                {
                    if (dataReader.Read())
                    {
                        user = new WebUser();
                        user.LoadData(dataReader);

                        // Add information to cache.
                        AddUserToCache(context, user);
                    }
                    else
                    {
                        throw new ArgumentException("User not found. UserId = " + userId);
                    }
                }
            }

            return user;
        }

        /// <summary>
        /// Get information about user with specified name.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userName">User name.</param>
        /// <returns>Information about user with specified name.
        /// or NULL if no user with matching userName is found.
        /// </returns>
        public static WebUser GetUserByName(WebServiceContext context,
                                            String userName)
        {
            WebUser user;

            // Check data.
            userName.CheckNotEmpty("userName");
            userName = userName.CheckInjection();

            // Get cached information.
            user = GetUserFromCache(context, userName);
            if (user.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetUserDatabase().GetUser(userName))
                {
                    if (dataReader.Read())
                    {
                        user = new WebUser();
                        user.LoadData(dataReader);

                        // Add information to cache.
                        AddUserToCache(context, user);
                    }
                }
            }

            return user;
        }

        /// <summary>
        /// Get key used when handling the cached user object.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>The user cache key.</returns>       
        private static String GetUserCacheKey(Int32 userId)
        {
            return "WebUser:" + userId;
        }

        /// <summary>
        /// Get cached user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userId">User id.</param>
        /// <returns>The user object.</returns>       
        private static WebUser GetUserFromCache(WebServiceContext context,
                                                Int32 userId)
        {
            ConcurrentDictionary<Int32, Hashtable> userLocaleCache;
            Hashtable userCache;
            String cacheKey;
            WebUser user;

            user = null;
            cacheKey = GetUserCacheKey(userId);
            userLocaleCache = (ConcurrentDictionary<Int32, Hashtable>)context.GetCachedObject(cacheKey);
            if (userLocaleCache.IsNotNull())
            {
                if (userLocaleCache.ContainsKey(context.Locale.Id))
                {
                    userCache = userLocaleCache[context.Locale.Id];
                    user = (WebUser)(userCache[userId]);
                }
            }

            return user;
        }

        /// <summary>
        /// Get cached user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userName">User name.</param>
        /// <returns>The user object.</returns>       
        private static WebUser GetUserFromCache(WebServiceContext context,
                                                String userName)
        {
            if (_userNameToIdMapping.ContainsKey(userName))
            {
                return GetUserFromCache(context, _userNameToIdMapping[userName]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get id for current user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Id for current user.</returns>       
        public static Int32 GetUserId(WebServiceContext context)
        {
            String userName;

            userName = context.ClientToken.UserName;
            if (_userNameToIdMapping.ContainsKey(userName))
            {
                return _userNameToIdMapping[userName];
            }
            else
            {
                return GetUserByName(context, userName).Id;
            }
        }

        /// <summary>
        /// Get list of Users with a Role
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleId">Role id.</param>
        /// <returns>Returns list of users or 
        /// null if roleid doesn't match or if role has no members.
        /// </returns>
        public static List<WebUser> GetUsersByRole(WebServiceContext context, Int32 roleId)
        {
            List<WebUser> roleMembers;
            WebUser user;

            // Get data from database.
            using (DataReader dataReader = context.GetUserDatabase().GetUsersByRole(roleId))
            {
                roleMembers = new List<WebUser>();
                while (dataReader.Read())
                {
                    user = new WebUser();
                    user.LoadData(dataReader);
                    roleMembers.Add(user);
                }
            }

            return roleMembers;
        }

        /// <summary>
        /// Get users that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Users that matches the search criteria.</returns>
        public static List<WebUser> GetUsersBySearchCriteria(WebServiceContext context,
                                                             WebUserSearchCriteria searchCriteria)
        {
            List<WebUser> users;
            WebUser user;
            String userType;
            Int32? organizationId, organizationCategoryId, applicationId, applicationActionId;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();

            if (searchCriteria.IsUserTypeSpecified)
            {
                userType = searchCriteria.UserType.ToString();
            }
            else
            {
                userType = null;
            }

            organizationId = null;
            if (searchCriteria.IsOrganizationIdSpecified)
            {
                organizationId = searchCriteria.OrganizationId;
            }

            organizationCategoryId = null;
            if (searchCriteria.IsOrganizationCategoryIdSpecified)
            {
                organizationCategoryId = searchCriteria.OrganizationCategoryId;
            }

            applicationId = null;
            if (searchCriteria.IsApplicationIdSpecified)
            {
                applicationId = searchCriteria.ApplicationId;
            }

            applicationActionId = null;
            if (searchCriteria.IsApplicationActionIdSpecified)
            {
                applicationActionId = searchCriteria.ApplicationActionId;
            }

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetUsersBySearchCriteria(searchCriteria.FullName,
                                                                                              searchCriteria.FirstName,
                                                                                              searchCriteria.LastName,
                                                                                              searchCriteria.EmailAddress,
                                                                                              searchCriteria.City,
                                                                                              userType,
                                                                                              organizationId,
                                                                                              organizationCategoryId,
                                                                                              applicationId,
                                                                                              applicationActionId))
            {
                users = new List<WebUser>();
                while (dataReader.Read())
                {
                    user = new WebUser();
                    user.LoadData(dataReader);
                    users.Add(user);
                }
            }

            return users;
        }

        /// <summary>
        /// Test if an email address is unique.
        /// Do not include specified person or user if 
        /// parameter userId or personId has a value.
        /// It is assumed that max one of the parameters userId
        /// and personId has a value in one request.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="userId">Id of user to exclude when searching for duplicates.</param>
        /// <param name="personId">Id of person to exclude when searching for duplicates.</param>
        /// <returns>True if an email address is unique.</returns>
        private static Boolean IsEmailAddressUnique(WebServiceContext context,
                                                    String emailAddress,
                                                    Int32? userId = null,
                                                    Int32? personId = null)
        {
            return context.GetUserDatabase().IsEmailAddressUnique(emailAddress, userId, personId);
        }

        /// <summary>
        /// Check if person already exists in database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="emailAddress">Email address.</param>
        /// <returns>
        /// Returns 'true' if person exists in database
        /// 'false' if person not exists in database.
        /// </returns>
        public static Boolean IsExistingPerson(WebServiceContext context,
                                               String emailAddress)
        {
            // Check data.
            emailAddress.CheckNotEmpty("emailAddress");
            emailAddress = emailAddress.CheckInjection();

            return emailAddress.IsNotEmpty() &&
                   !IsEmailAddressUnique(context, emailAddress);
        }

        /// <summary>
        /// Check if user name already exists in database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userName">User name.</param>
        /// <returns>
        /// Returns 'true' if username exists in database
        /// 'false' if username not exists in database.
        /// </returns>
        public static Boolean IsExistingUser(WebServiceContext context,
                                             String userName)
        {
            // Check data.
            userName.CheckNotEmpty("userName");
            userName = userName.CheckInjection();

            // Get information from database.
            return context.GetUserDatabase().IsExistingUser(userName);
        }

        /// <summary>
        /// Mark user as changed.
        /// </summary>
        /// <param name="userName">User name.</param>
        private static void IsUserChanged(String userName)
        {
            if (_userNameToIdMapping.ContainsKey(userName))
            {
                IsUserChanged(_userNameToIdMapping[userName]);
            }
        }

        /// <summary>
        /// Mark user as changed.
        /// </summary>
        /// <param name="userId">User id.</param>
        private static void IsUserChanged(Int32 userId)
        {
            lock (_changedUserIds)
            {
                if (!_changedUserIds.Contains(userId))
                {
                    _changedUserIds.Add(userId);
                }
            }
        }

        /// <summary>
        /// Test if it is ok for this user to login.
        /// If the user has failed to login (wrong password)
        /// three times in one hour then
        /// it is not ok for the user to login.
        /// This blocking of a user are automatically
        /// removed if the user makes no attempt to login
        /// for one hour.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>True if it is ok for the user to login.</returns>       
        private static Boolean IsLoginOK(String userName)
        {
            LoginInformation loginInformation;
            TimeSpan timeSpan;

            // User name is already checked for injection.
            // Do not check again!
            if (_loginFailedTable.ContainsKey(userName))
            {
                loginInformation = _loginFailedTable[userName];
                if (loginInformation.LoginAttemptCount < Settings.Default.MaxLoginAttempt)
                {
                    // Not to many failed loggins.
                    return true;
                }

                timeSpan = DateTime.Now - loginInformation.LastLogin;
                if (timeSpan.TotalMinutes > Settings.Default.MinFailedLoginWaitTime)
                {
                    // Have waited long enough to allow some more login attempts.
                    _loginFailedTable.TryRemove(userName, out loginInformation);
                    return true;
                }

                // Not ok to allow login attempt.
                return false;
            }
            else
            {
                // No failed logins.
                return true;
            }
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">Users password.</param>
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
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation")]
        public static WebLoginResponse Login(WebServiceContext context,
                                             String userName,
                                             String password,
                                             String applicationIdentifier,
                                             Boolean isActivationRequired)
        {
            WebClientToken okClientToken = null;
            WebLoginResponse loginResponse = null;
            Int32 localeId = 0;

            // Check arguments.
            applicationIdentifier.CheckNotEmpty("applicationIdentifier");
            password.CheckNotEmpty("password");
            userName.CheckNotEmpty("userName");
            applicationIdentifier = applicationIdentifier.CheckInjection();
            password = password.CheckInjection();
            userName = userName.CheckInjection();

            try
            {
                if (Regex.IsMatch(applicationIdentifier, @"^[a-zåäöA-ZÅÄÖ0-9_]{3,40}$") &&
                    Regex.IsMatch(userName, Settings.Default.UserNameRegularExpression) &&
                    (password.Length <= WebUserExtension.GetPasswordMaxLength(context)) &&
                    (userName.Length <= WebUserExtension.GetUserNameMaxLength(context)) &&
                    IsLoginOK(userName))
                {
                    password = CryptationHandler.GetSHA1Hash(password);
                    using (DataReader dataReader = context.GetUserDatabase().Login(userName,
                                                                                   password,
                                                                                   isActivationRequired))
                    {
                        if (dataReader.Read())
                        {
                            okClientToken = context.ClientToken;
                            ResetFailedLoggins(userName);
                            localeId = dataReader.GetInt32(UserData.LOCALE_ID, 0);
                        }
                    }
                }
            }
            finally
            {
                if (okClientToken.IsNull())
                {
                    LoginFailed(context, userName);
                }
            }

            // Set Locale object in context
            if (localeId > 0)
            {
                context.Locale = LocaleManager.GetLocale(context, localeId);
            }
            else
            {
                context.Locale = LocaleManager.GetLocale(context, Settings.Default.DefaultLocale);
            }

            if (okClientToken.IsNotNull())
            {
                // Create login response object.
                loginResponse = new WebLoginResponse();
                loginResponse.Locale = context.Locale;
// ReSharper disable PossibleNullReferenceException
                loginResponse.Token = okClientToken.Token;
// ReSharper restore PossibleNullReferenceException
                loginResponse.User = GetUser(context);
                loginResponse.Roles = GetRolesByUser(context, loginResponse.User.Id, context.ClientToken.ApplicationIdentifier);
            }

            return loginResponse;
        }

        /// <summary>
        /// UserLogin failed.
        /// Store information about failed login attempt.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userName">User name.</param>
        private static void LoginFailed(WebServiceContext context,
                                        String userName)
        {
            Boolean logFailedLogin;
            LoginInformation loginInformation;

            if (_loginFailedTable.ContainsKey(userName))
            {
                // Update old entry.
                loginInformation = _loginFailedTable[userName];
                loginInformation.LoginAttemptCount += 1;
                loginInformation.LastLogin = DateTime.Now;
            }
            else
            {
                // Add new entry.
                loginInformation = new LoginInformation(userName);
                _loginFailedTable[userName] = loginInformation;
            }

            // Log information about failed login.
            logFailedLogin = (loginInformation.LoginAttemptCount == Settings.Default.MaxLoginAttempt);
            switch (loginInformation.LoginAttemptCount)
            {
                case 10:
                case 100:
                case 1000:
                case 10000:
                case 100000:
                case 1000000:
                case 10000000:
                case 100000000:
                case 1000000000:
                    logFailedLogin = true;
                    break;
            }

            if (logFailedLogin)
            {
                WebServiceData.LogManager.Log(context,
                                              "User " + userName +
                                              " has failed to login " +
                                              loginInformation.LoginAttemptCount +
                                              " times.",
                                              LogType.Security,
                                              null);
            }
        }

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void Logout(WebServiceContext context)
        {
            // Nothing to release yet.
        }

        /// <summary>
        /// Remove information objects from cache.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void RemoveCachedObjects(WebServiceContext context)
        {
            List<Int32> changedUserIds;
            String cacheKey;

            if (_isRoleUpdated)
            {
                context.ClearCache(false);
                _isAuthorityInformationUpdated = false;
                _isRoleUpdated = false;
                lock (_changedUserIds)
                {
                    _changedUserIds = new List<Int32>();
                }
            }
            else
            {
                if (_isAuthorityInformationUpdated)
                {
                    _isAuthorityInformationUpdated = false;
                    cacheKey = Settings.Default.AuthorityAttributeTypeCacheKey;
                    context.RemoveCachedObject(cacheKey);
                    cacheKey = Settings.Default.AuthorityAttributeCacheKey;
                    context.RemoveCachedObject(cacheKey);
                    cacheKey = Settings.Default.AuthorityCacheKey;
                    context.RemoveCachedObject(cacheKey);
                }

                lock (_changedUserIds)
                {
                    changedUserIds = _changedUserIds;
                    _changedUserIds = new List<Int32>();
                }

                if (changedUserIds.IsNotEmpty())
                {
                    foreach (Int32 changedUserId in changedUserIds)
                    {
                        cacheKey = GetUserCacheKey(changedUserId);
                        context.RemoveCachedObject(cacheKey);
                    }
                }
            }
        }

        /// <summary>
        /// Removes a user from a role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleId">Role Id.</param>
        /// <param name="userId">User Id.</param>
        public static void RemoveUserFromRole(WebServiceContext context,
                                              Int32 roleId,
                                              Int32 userId)
        {
            // Check whether or not the user has the user group administrator role. 
            // Beside super administrators, only user group administrators are currently authorized to run this method.
            if (!AuthorizationManager.IsUserAuthorized(context, Settings.Default.RoleIdForSuperAdministrator, Settings.Default.AuthorityIdentifierForUserGroupAdministration, null, null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotUserGroupAdministrator);
            }

            if (roleId == Settings.Default.RoleIdForSuperAdministrator)
            {
                AuthorizationManager.CheckSuperAdministrator(context);
            }

            context.CheckTransaction();
            context.GetUserDatabase().RemoveUserFromRole(roleId, userId);
            IsUserChanged(userId);
        }

        /// <summary>
        /// Remove information about failed login attempt.
        /// </summary>
        /// <param name="userName">User name.</param>
        private static void ResetFailedLoggins(String userName)
        {
            LoginInformation loginInformation;

            if (_loginFailedTable.ContainsKey(userName))
            {
                _loginFailedTable.TryRemove(userName, out loginInformation);
            }
        }
        
        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="emailAddress">User email address that needs password reset.</param>
        /// <returns>PasswordInformation object with username and new password.</returns>
        public static WebPasswordInformation ResetPassword(WebServiceContext context, String emailAddress)
        {
            const Int32 FixedLength = 12;
            String userName;
            WebPasswordInformation webPasswordInformation = new WebPasswordInformation();

            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check param
            context.CheckTransaction();
            emailAddress.CheckNotEmpty("emailAddress");
            emailAddress = emailAddress.CheckInjection();
            if (!emailAddress.IsValidEmail())
            {
                 throw new ArgumentException("Reset password failed. Emailaddress not valid " + emailAddress);
            }

            String newPassword = CreateAuthenticationKey(FixedLength, FixedLength);
            String newPasswordHashed = CryptationHandler.GetSHA1Hash(newPassword);
            using (DataReader dataReader = context.GetUserDatabase().ResetPassword(emailAddress, newPasswordHashed))
            {
                if (dataReader.Read())
                {
                    userName = dataReader.GetString(UserData.USER_NAME);
                    if (String.IsNullOrEmpty(userName))
                    {
                        // No username found for param emailAddress
                        throw new ArgumentException("Reset password failed. Username not found for emailaddress " + emailAddress);
                    }
                }
                else
                {
                    // Password reset failed
                    throw new ArgumentException("Reset password failed. Error updating database for emailaddress " + emailAddress);
                }
            }

            webPasswordInformation.EmailAddress = emailAddress;
            webPasswordInformation.UserName = userName;
            webPasswordInformation.Password = newPassword;

            ResetFailedLoggins(userName);

            return webPasswordInformation;
        }

        /// <summary>
        /// Updates an authority
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authority">Object representing the Authority.</param>
        /// <returns>WebAuthority object with the updated authority.</returns>
        public static WebAuthority UpdateAuthority(WebServiceContext context, WebAuthority authority)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check parameters
            context.CheckTransaction();
            authority.CheckData();

            int? administrationRoleId = null;
            if (authority.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = authority.AdministrationRoleId;
            }

            context.GetUserDatabase().UpdateAuthority(
                    authority.Id, authority.Identifier, authority.Name, authority.ShowNonPublicData, authority.MaxProtectionLevel,
                    authority.ReadPermission, authority.CreatePermission, authority.UpdatePermission, authority.DeletePermission,
                    administrationRoleId, authority.Description, authority.Obligation,
                    context.Locale.Id, GetUserId(context), authority.ValidFromDate, authority.ValidToDate);

            DeleteAuthorityAttributes(context, authority);
            CreateAuthorityAttributes(context, authority);
            _isAuthorityInformationUpdated = true;
            return GetAuthority(context, authority.Id);
        }

        /// <summary>
        /// Mark authority information as updated.
        /// </summary>
        private static void UpdateIsAuthorityInformationUpdated()
        {
            _isAuthorityInformationUpdated = true;
        }

        /// <summary>
        /// Update password for logged in user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="oldPassword">Current password.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>True, if password was updated.</returns>
        public static Boolean UpdatePassword(WebServiceContext context,
                                             String oldPassword,
                                             String newPassword)
        {
            Boolean isOldPasswordOk, isPasswordUpdated = false;

            // Check data.
            context.CheckTransaction();
            oldPassword.CheckNotEmpty("oldPassword");
            oldPassword = oldPassword.CheckInjection();
            newPassword.CheckNotEmpty("newPassword");
            newPassword = newPassword.CheckInjection();
            newPassword.CheckLength(WebUserExtension.GetPasswordMaxLength(context));
            newPassword.CheckRegularExpression(Settings.Default.PasswordRegularExpression);

            // Test that old password is correct.
            oldPassword = CryptationHandler.GetSHA1Hash(oldPassword);
            using (DataReader dataReader = context.GetUserDatabase().Login(GetUser(context).UserName, oldPassword, false))
            {
                isOldPasswordOk = dataReader.Read();
            }

            if (isOldPasswordOk)
            {
                // Update password.
                newPassword = CryptationHandler.GetSHA1Hash(newPassword);
                isPasswordUpdated = context.GetUserDatabase().UpdateUserPassword(GetUser(context).UserName, newPassword);

                // Remove cached information.
                IsUserChanged(context.ClientToken.UserName);
            }

            return isPasswordUpdated;
        }      

        /// <summary>
        /// Updates a person
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="person">Object representing the Person.</param>
        /// <returns>WebPerson object with the updated person.</returns>
        public static WebPerson UpdatePerson(WebServiceContext context, WebPerson person)
        {
            // Check wheter or not it is the user who attempts to edit its own person object.
            // If not only super administrators are allowed to perform this method.
            bool self = false;
            if (person.IsUserIdSpecified)
            {
                if (context.GetUser().Id == person.UserId)
                {
                    self = true;
                }
            }

            if (!self)
            {
                // Check whether or not the user has the super administrator role.
                AuthorizationManager.CheckSuperAdministrator(context);
            }

            return UpdatePersonData(context, person);
        }

        /// <summary>
        /// Updates a person
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="person">Object representing the Person.</param>
        /// <returns>WebPerson object with the updated person.</returns>
        private static WebPerson UpdatePersonData(WebServiceContext context, WebPerson person)
        {
            DateTime? birthYear, deathYear;
            Int32? userId, administrationRoleId;

            // Check parameters
            context.CheckTransaction();
            person.CheckData();

            // Check unique email address.
            if (person.EmailAddress.IsNotEmpty() &&
                !IsEmailAddressUnique(context, person.EmailAddress, null, person.Id))
            {
                throw new ArgumentException("Update person: Email address already exists. Person id = " + person.Id + " Email address = " + person.EmailAddress);
            }

            birthYear = null;
            if (person.IsBirthYearSpecified)
            {
                birthYear = person.BirthYear;
            }
            deathYear = null;
            if (person.IsDeathYearSpecified)
            {
                deathYear = person.DeathYear;
            }
            userId = null;
            if (person.IsUserIdSpecified)
            {
                userId = person.UserId;
            }
            administrationRoleId = null;
            if (person.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = person.AdministrationRoleId;
            }

            context.GetUserDatabase().UpdatePerson(
                person.Id, person.FirstName, person.MiddleName, person.LastName,
                person.Gender.Id, person.EmailAddress, person.ShowEmailAddress, person.ShowAddresses,
                person.ShowPhoneNumbers, birthYear, deathYear, administrationRoleId, person.HasSpeciesCollection,
                person.Locale.Id, person.TaxonNameTypeId, person.URL, person.Presentation,
                person.ShowPresentation, person.ShowPersonalInformation, userId, GetUserId(context));
            DeleteAddress(context, person.Id, Int32.MinValue);
            if (person.Addresses.IsNotEmpty())
            {
                List<WebAddress> addresses = person.Addresses;
                foreach (WebAddress webAddress in addresses)
                {
                    webAddress.PersonId = person.Id;
                    CreateAddress(context, webAddress);
                }
            }
            DeletePhoneNumber(context, person.Id, Int32.MinValue);
            if (person.PhoneNumbers.IsNotEmpty())
            {
                List<WebPhoneNumber> phoneNumbers = person.PhoneNumbers;
                foreach (WebPhoneNumber webPhoneNumber in phoneNumbers)
                {
                    webPhoneNumber.PersonId = person.Id;
                    CreatePhoneNumber(context, webPhoneNumber);
                }
            }

            if (person.IsUserIdSpecified)
            {
                IsUserChanged(person.UserId);
            }

            return GetPerson(context, person.Id);
        }

        /// <summary>
        /// Updates an role
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="role">Object representing the Role.</param>
        /// <returns>WebRole object with the updated role.</returns>
        public static WebRole UpdateRole(WebServiceContext context, WebRole role)
        {
            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check parameters.
            context.CheckTransaction();
            role.CheckData();

            Int32? administrationRoleId, userAdministrationRoleId, organizationId;
            administrationRoleId = null;
            if (role.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = role.AdministrationRoleId;
            }

            userAdministrationRoleId = null;
            if (role.IsUserAdministrationRoleIdSpecified)
            {
                userAdministrationRoleId = role.UserAdministrationRoleId;
            }

            organizationId = null;
            if (role.IsOrganizationIdSpecified)
            {
                organizationId = role.OrganizationId;
            }

            context.GetUserDatabase().UpdateRole(
                    role.Id, role.Name, role.ShortName, role.Description, context.Locale.Id,
                    administrationRoleId, userAdministrationRoleId, organizationId,
                    GetUserId(context), role.ValidFromDate, role.ValidToDate,
                    role.Identifier, role.IsActivationRequired, role.MessageTypeId);

            // Update authorities.
            if (role.Authorities.IsNotEmpty())
            {
                List<WebAuthority> authorityList = role.Authorities;
                foreach (WebAuthority webAuthority in authorityList)
                {
                    UpdateAuthority(context, webAuthority);
                }
            }

            _isAuthorityInformationUpdated = true;
            _isRoleUpdated = true;
            return GetRole(context, role.Id);
        }

        /// <summary>
        /// Updates a user and its associated person. The function only be used by support users.
        /// Only Email and account activation can be changed when the account is inactivated.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userData">Object representing the User.</param>
        /// <param name="personData">Object representing the Person.</param>        
        /// <returns>WebUser object with the updated user.</returns>
        public static WebUser SupportUpdatePersonUser(WebServiceContext context, WebUser userData, WebPerson personData)
        {
            // Check whether or not the user has the support edit permission.            
            AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.SupportEdit);

            WebUser user = GetUserById(context, personData.UserId);

            // Support users can only change data on account that is not activated.
            if (user.IsAccountActivated)
            {
                throw new ApplicationException(string.Format("User account with Id: {0} is already activated", personData.UserId));
            }

            // Update person
            WebPerson person = GetPerson(context, personData.Id);
            person.EmailAddress = personData.EmailAddress;
            UpdatePersonData(context, person);

            // Update user
            user.EmailAddress = userData.EmailAddress;
            user.IsAccountActivated = userData.IsAccountActivated;
            return UpdateUserData(context, user);          
        }        


        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="user">Object representing the User.</param>
        /// <returns>WebUser object with the updated user.</returns>
        public static WebUser UpdateUser(WebServiceContext context, WebUser user)
        {
            // Check access rights.            
            AuthorizationManager.CheckSuperAdministrator(context);

            return UpdateUserData(context, user);           
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="user">Object representing the User.</param>
        /// <returns>WebUser object with the updated user.</returns>
        private static WebUser UpdateUserData(WebServiceContext context, WebUser user)
        {            
            // Check param.
            context.CheckTransaction();
            user.CheckData();

            // Check email address is unique for usertype = Person
            if (user.Type.Equals(UserType.Person) &&
                user.EmailAddress.IsNotEmpty() &&
                !IsEmailAddressUnique(context, user.EmailAddress, user.Id))
            {
                throw new ArgumentException("UpdateUser: Email address already exists. " + user.EmailAddress);
            }

            Int32? applicationId, personId, administrationRoleId;
            Int32 userId;

            applicationId = null;
            if (user.IsApplicationIdSpecified)
            {
                applicationId = user.ApplicationId;
            }

            personId = null;
            if (user.IsPersonIdSpecified)
            {
                personId = user.PersonId;
            }

            administrationRoleId = null;
            if (user.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = user.AdministrationRoleId;
            }

            userId = context.GetUserDatabase().UpdateUser(user.UserName,
                                                          personId,
                                                          applicationId,
                                                          user.GUID,
                                                          user.EmailAddress,
                                                          user.ShowEmailAddress,
                                                          user.AuthenticationType,
                                                          user.IsAccountActivated,
                                                          administrationRoleId,
                                                          GetUserId(context),
                                                          user.ValidFromDate,
                                                          user.ValidToDate);

            IsUserChanged(user.Id);

            return GetUserById(context, userId);
        }
        

        /// <summary>
        /// Updates a users password without sending the old password.
        /// Used by administrator.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="user">Object representing the User.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>true - if users password is changed
        /// false - if password change failed.
        /// </returns>
        public static Boolean UserAdminSetPassword(WebServiceContext context, WebUser user, String newPassword)
        {
            Boolean isPasswordChanged = false;

            // Check access rights.
            AuthorizationManager.CheckSuperAdministrator(context);

            // Check data.
            context.CheckTransaction();
            user.CheckNotNull("user");
            newPassword.CheckNotEmpty("newPassword");
            newPassword = newPassword.CheckInjection();
            if (Regex.IsMatch(newPassword, Settings.Default.PasswordRegularExpression) &&
               (newPassword.Length <= WebUserExtension.GetPasswordMaxLength(context)))
            {
                // Update password.
                newPassword = CryptationHandler.GetSHA1Hash(newPassword);
                isPasswordChanged = context.GetUserDatabase().UserAdminSetPassword(user.Id, newPassword); 
            }

            return isPasswordChanged;
        }

        /// <summary>
        /// This class holds information about failed logins.
        /// </summary>
        private class LoginInformation
        {
            /// <summary>
            /// Name of user that tries to login.
            /// </summary>
            private readonly String _userName;

            /// <summary>
            /// Create a LoginInformation instance.
            /// </summary>
            /// <param name="userName">User name.</param>
            public LoginInformation(String userName)
            {
                LastLogin = DateTime.Now;
                LoginAttemptCount = 1;
                _userName = userName;
            }

            /// <summary>
            /// Date and time for last login attempt.
            /// </summary>
            public DateTime LastLogin { get; set; }

            /// <summary>
            /// Number of times that the user has tried to login.
            /// </summary>
            public Int64 LoginAttemptCount { get; set; }

            /// <summary>
            /// Get user name.
            /// </summary>
            public String UserName
            {
                get { return _userName; }
            }
        }
    }
}
