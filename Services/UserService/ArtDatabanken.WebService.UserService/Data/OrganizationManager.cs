using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.UserService.Data
{

    /// <summary>
    /// Manager of organization information.
    /// </summary>
    public class OrganizationManager
    {

        /// <summary>
        /// Creates a new organization
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organization">Object representing the Organization.</param>
        /// <returns>WebOrganization object with the created organization.</returns>
        public static WebOrganization CreateOrganization (WebServiceContext context, WebOrganization organization)
        {
            //Check whether or not the user has the super administrator role. 
            //Only super administrators are currently authorized to run this method.
            if (!AuthorizationManager.IsUserAuthorized(context, Settings.Default.RoleIdForSuperAdministrator, null, null, null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotSuperAdministrator);
            }

            Int32 organizationId = Int32.MinValue;

            // Check arguments.
            context.CheckTransaction();
            organization.CheckNotNull("organization");
            organization.CheckData();

            Int32? administrationRoleId;
            administrationRoleId = null;
            if (organization.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = organization.AdministrationRoleId;
            }
            organizationId = context.GetUserDatabase().CreateOrganization(
                organization.Name, organization.ShortName, organization.Description, administrationRoleId,
                organization.HasSpeciesCollection, organization.Category.Id, context.GetUser().Id, 
                context.Locale.Id, organization.ValidFromDate, organization.ValidToDate);
            if (organization.Addresses.IsNotEmpty())
            {
                List<WebAddress> addresses = organization.Addresses;
                foreach (WebAddress webAddress in addresses)
                {
                    webAddress.OrganizationId = organizationId;
                    UserManager.CreateAddress(context, webAddress);
                }
            }

            if (organization.PhoneNumbers.IsNotEmpty())
            {
                List<WebPhoneNumber> phoneNumbers = organization.PhoneNumbers;
                foreach (WebPhoneNumber webPhoneNumber in phoneNumbers)
                {
                    webPhoneNumber.OrganizationId = organizationId;
                    UserManager.CreatePhoneNumber(context, webPhoneNumber);
                }
            }
            return GetOrganization(context, organizationId);
        }

        /// <summary>
        /// Creates a new OrganizationCategory
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organizationCategory">Object representing the OrganizationCategory.</param>
        /// <returns>WebOrganizationCategory object with the created organizationCategory.</returns>
        public static WebOrganizationCategory CreateOrganizationCategory(WebServiceContext context, WebOrganizationCategory organizationCategory)
        {
            //Check whether or not the user has the super administrator role. 
            //Only super administrators are currently authorized to run this method.
            if (!AuthorizationManager.IsUserAuthorized(context, Settings.Default.RoleIdForSuperAdministrator, null, null, null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotSuperAdministrator);
            }

            Int32 organizationCategoryId = Int32.MinValue;

            // Check arguments.
            context.CheckTransaction();
            organizationCategory.CheckNotNull("organizationCategory");
            organizationCategory.CheckData();

            Int32? administrationRoleId;
            administrationRoleId = null;
            if (organizationCategory.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = organizationCategory.AdministrationRoleId;
            }
            organizationCategoryId = context.GetUserDatabase().CreateOrganizationCategory(
                organizationCategory.Name, organizationCategory.Description, administrationRoleId,
                context.GetUser().Id, context.Locale.Id );
            return GetOrganizationCategory(context, organizationCategoryId);
        }

        /// <summary>
        /// Delete Organization 
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organization">Object representing the Organization.</param>
        /// <returns>void</returns>
        public static void DeleteOrganization(WebServiceContext context, WebOrganization organization)
        {
            //Check whether or not the user has the super administrator role. 
            //Only super administrators are currently authorized to run this method.
            if (!AuthorizationManager.IsUserAuthorized(context, Settings.Default.RoleIdForSuperAdministrator, null, null, null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotSuperAdministrator);
            }

            context.CheckTransaction();
            context.GetUserDatabase().DeleteOrganization(organization.Id, context.GetUser().Id);
        }

        /// <summary>
        /// Get information about a organization
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Returns organization information or null if 
        ///          organizationid doesn't match.</returns>
        public static WebOrganization GetOrganization(WebServiceContext context, Int32 organizationId)
        {
            WebOrganization organization;

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetOrganization(organizationId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    organization = new WebOrganization();
                    organization.Category = new WebOrganizationCategory();
                    organization.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("Organization not found. OrganizationId = " + organizationId);
                }
            }
            // Get organizationCategory for this organization
            organization.Category = GetOrganizationCategory(context, organization.CategoryId);
            // Set WebAddress + WebPhone
            organization.Addresses = UserManager.GetAddresses(context, 0, organization.Id);
            organization.PhoneNumbers = UserManager.GetPhoneNumbers(context, 0, organization.Id);
            return organization;
        }

        /// <summary>
        /// Get list of all organizations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns list of organizations or null if no organizations exists.
        /// </returns>
        public static List<WebOrganization> GetOrganizations(WebServiceContext context)
        {
            return GetOrganizations(context, null);
        }

        /// <summary>
        /// Get information about organizations.
        /// If organizationCategoryId is specified all organizations of that category are returned.
        /// If no organizationCategoryId is specified all organizations are returned.
        /// <param name="context">Web service request context.</param>
        /// <param name="organizationCategoryId">Organization category id.</param>
        /// </summary>
        /// <returns>
        /// Returns list of organizations or null if specified category
        /// has no organizations.
        /// </returns>
        private static List<WebOrganization> GetOrganizations(WebServiceContext context, Int32? organizationCategoryId)
        {
            WebOrganization organization;
            List<WebOrganization> organizationList;

            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetOrganizations(organizationCategoryId, context.Locale.Id))
            {
                organizationList = new List<WebOrganization>();
                while (dataReader.Read())
                {
                    organization = new WebOrganization();
                    organization.Category = new WebOrganizationCategory();
                    organization.LoadData(dataReader);
                    organizationList.Add(organization);
                }
            }
            if (organizationList.IsNotEmpty())
            {
                foreach (WebOrganization tempOrganization in organizationList)
                {
                    // Get organizationCategory for this organization
                    tempOrganization.Category = GetOrganizationCategory(context, tempOrganization.CategoryId);
                    // Set WebAddress + WebPhone
                    tempOrganization.Addresses = UserManager.GetAddresses(context, 0, tempOrganization.Id);
                    tempOrganization.PhoneNumbers = UserManager.GetPhoneNumbers(context, 0, tempOrganization.Id);
                }
            }
            return organizationList;
        }


        /// <summary>
        /// Get organizations by organization category
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organizationCategoryId">Organization category id.</param>
        /// <returns>
        /// Returns list of organizations or null if no organizations are categorized as the
        /// specified category.
        /// </returns>
        public static List<WebOrganization> GetOrganizationsByOrganizationCategory(WebServiceContext context,
                                                                                   Int32 organizationCategoryId)
        {
            return GetOrganizations(context, organizationCategoryId);
        }

        /// <summary>
        /// Get organizations that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Organizations that matches the search criteria</returns>
        public static List<WebOrganization> GetOrganizationsBySearchCriteria(WebServiceContext context,
                                                                             WebOrganizationSearchCriteria searchCriteria)
        {
            Int32? organizationCategoryId;
            Boolean? hasSpiecesCollection;
            List<WebOrganization> organizations;
            WebOrganization organization;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();

            organizationCategoryId = null;
            if (searchCriteria.IsOrganizationCategoryIdSpecified)
            {
                organizationCategoryId = searchCriteria.OrganizationCategoryId;
            }
            hasSpiecesCollection = null;
            if (searchCriteria.IsHasSpeciesCollectionSpecified)
            {
                hasSpiecesCollection = searchCriteria.HasSpeciesCollection;
            }
            // Get information from database.
            using (DataReader dataReader = context.GetUserDatabase().GetOrganizationsBySearchCriteria(searchCriteria.Name,
                                                                                                      searchCriteria.ShortName,
                                                                                                      organizationCategoryId,
                                                                                                      hasSpiecesCollection,
                                                                                                      context.Locale.Id))
            {
                organizations = new List<WebOrganization>();
                while (dataReader.Read())
                {
                    organization = new WebOrganization();
                    organization.LoadData(dataReader);
                    organizations.Add(organization);
                }
            }

            if (organizations.IsNotEmpty())
            {
                foreach (WebOrganization tempOrganization in organizations)
                {
                    // Get organizationCategory for this organization
                    tempOrganization.Category = GetOrganizationCategory(context, tempOrganization.CategoryId);
                    // Set WebAddress + WebPhone
                    tempOrganization.Addresses = UserManager.GetAddresses(context, 0, tempOrganization.Id);
                    tempOrganization.PhoneNumbers = UserManager.GetPhoneNumbers(context, 0, tempOrganization.Id);
                }
            }
            return organizations;
        }

        /// <summary>
        /// Get list of Roles for a Organization.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Returns list of roles or 
        /// null if organizationId doesn't match or if organization has no roles.
        /// </returns>
        public static List<WebRole> GetOrganizationRoles(WebServiceContext context, Int32 organizationId)
        {
            List<WebRole> userRoles;
            WebRole role;

            // Get data from database.
            using (DataReader dataReader = context.GetUserDatabase().GetOrganizationRoles(organizationId, context.Locale.Id))
            {
                userRoles = new List<WebRole>();
                while (dataReader.Read())
                {
                    role = new WebRole();
                    role.LoadData(dataReader);
                    userRoles.Add(role);
                }
            }
            return userRoles;
        }

        /// <summary>
        /// Get OrganizationCategory.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organizationCategoryId">Id for OrganizationCategory</param>
        /// <returns>A list of OrganizationCategories.</returns>
        public static WebOrganizationCategory GetOrganizationCategory(WebServiceContext context, Int32 organizationCategoryId)
        {
            WebOrganizationCategory organizationCategory;

            //GetData from Database.
            using (DataReader dataReader = context.GetUserDatabase().GetOrganizationCategory(organizationCategoryId, context.Locale.Id))
            {
                if (dataReader.Read())
                {
                    organizationCategory = new WebOrganizationCategory();
                    organizationCategory.LoadData(dataReader);
                }
                else
                {
                    throw new ArgumentException("OrganizationCategory not found. OrganizationCategoryId = " + organizationCategoryId);
                }
            }
            return organizationCategory;
        }

        /// <summary>
        /// Get key used when handling the OrganizationCategories cache.
        /// </summary>
        /// <param name="localeId">Id representing language.</param>
        /// <returns>The OrganizationCategories cache key.</returns>       
        private static String GetOrganizationCategoriesCacheKey(Int32 localeId)
        {
            return "WebOrganizationCategories:" + localeId;
        }

        /// <summary>
        /// Get list of all OrganizationCategories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>A list of OrganizationCategories.</returns>
        public static List<WebOrganizationCategory> GetOrganizationCategories(WebServiceContext context)
        {
            String cacheKey;
            List<WebOrganizationCategory> organizationCategories;
            WebOrganizationCategory organizationCategory;

            // Get cached information.
            cacheKey = GetOrganizationCategoriesCacheKey(context.Locale.Id);
            organizationCategories = (List<WebOrganizationCategory>)context.GetCachedObject(cacheKey);

            // Data not in cache - store it in the cache
            if (organizationCategories.IsNull())
            {
                // Get data from database.
                using (DataReader dataReader = context.GetUserDatabase().GetOrganizationCategories(context.Locale.Id))
                {
                    organizationCategories = new List<WebOrganizationCategory>();
                    while (dataReader.Read())
                    {
                        organizationCategory = new WebOrganizationCategory();
                        organizationCategory.LoadData(dataReader);
                        organizationCategories.Add(organizationCategory);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey, organizationCategories, DateTime.Now + new TimeSpan(1, 0, 0, 0), CacheItemPriority.High);
            }

            return organizationCategories;
        }

        /// <summary>
        /// Updates an organization
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organization">Object representing the Organization.</param>
        /// <returns>WebOrganization object with the updated organization.</returns>
        public static WebOrganization UpdateOrganization(WebServiceContext context, WebOrganization organization)
        {
            //Check whether or not the user has the super administrator role. 
            //Only super administrators are currently authorized to run this method.
            if (!AuthorizationManager.IsUserAuthorized(context, Settings.Default.RoleIdForSuperAdministrator, null, null, null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotSuperAdministrator);
            }

            Int32? administrationRoleId;
            administrationRoleId = null;

            //Check arguments
            context.CheckTransaction();
            organization.CheckData();

            if (organization.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = organization.AdministrationRoleId;
            }
            context.GetUserDatabase().UpdateOrganization(
                    organization.Id, organization.Name, organization.ShortName, organization.Category.Id,
                    administrationRoleId, organization.HasSpeciesCollection, organization.Description, context.Locale.Id,
                    context.GetUser().Id, organization.ValidFromDate, organization.ValidToDate);
            UserManager.DeleteAddress(context, Int32.MinValue, organization.Id);
            if (organization.Addresses.IsNotEmpty())
            {
                List<WebAddress> addresses = organization.Addresses;
                foreach (WebAddress webAddress in addresses)
                {
                    webAddress.OrganizationId = organization.Id;
                    UserManager.CreateAddress(context, webAddress);
                }
            }
            UserManager.DeletePhoneNumber(context, Int32.MinValue, organization.Id);
            if (organization.PhoneNumbers.IsNotEmpty())
            {
                List<WebPhoneNumber> phoneNumbers = organization.PhoneNumbers;
                foreach (WebPhoneNumber webPhoneNumber in phoneNumbers)
                {
                    webPhoneNumber.OrganizationId = organization.Id;
                    UserManager.CreatePhoneNumber(context, webPhoneNumber);
                }
            }
            return GetOrganization(context, organization.Id);
        }

        /// <summary>
        /// Updates an organization category
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="organizationCategory">Object representing the Organization category.</param>
        /// <returns>WebOrganizationCategory object with the updated organization category.</returns>
        public static WebOrganizationCategory UpdateOrganizationCategory(WebServiceContext context, 
                                                                         WebOrganizationCategory organizationCategory)
        {
            //Check whether or not the user has the super administrator role. 
            //Only super administrators are currently authorized to run this method.
            if (!AuthorizationManager.IsUserAuthorized(context, Settings.Default.RoleIdForSuperAdministrator, null, null, null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotSuperAdministrator);
            }

            Int32? administrationRoleId;
            administrationRoleId = null;

            // Check arguments
            context.CheckTransaction();
            organizationCategory.CheckData();

            if (organizationCategory.IsAdministrationRoleIdSpecified)
            {
                administrationRoleId = organizationCategory.AdministrationRoleId;
            }
            context.GetUserDatabase().UpdateOrganizationCategory(
                    organizationCategory.Id, organizationCategory.Name, organizationCategory.Description,
                    administrationRoleId, context.GetUser().Id, context.Locale.Id);
            return GetOrganizationCategory(context, organizationCategory.Id);
        }
    }
    
}
