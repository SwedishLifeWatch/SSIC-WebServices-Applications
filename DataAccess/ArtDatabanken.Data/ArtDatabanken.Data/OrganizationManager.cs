using System;
using System.Reflection;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles organization related information.
    /// </summary>
    public class OrganizationManager : IOrganizationManager
    {
        /// <summary>
        /// This property is used to retrieve or update information.
        /// </summary>
        public IUserDataSource DataSource
        { get; set; }

        /// <summary>
        /// Create new organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">
        /// Information about the new organization.
        /// This object is updated with information 
        /// about the created organization.
        /// </param>
        public virtual void CreateOrganization(IUserContext userContext,
                                               IOrganization organization)
        {
            DataSource.CreateOrganization(userContext, organization);
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
        public virtual void CreateOrganizationCategory(IUserContext userContext,
                                                       IOrganizationCategory organizationCategory)
        {
            DataSource.CreateOrganizationCategory(userContext, organizationCategory);
        }

        /// <summary>
        /// Delete a organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">Delete this organization.</param>
        public virtual void DeleteOrganization(IUserContext userContext,
                                               IOrganization organization)
        {
            DataSource.DeleteOrganization(userContext, organization);
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
        /// Get organization by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Requested organization.</returns>       
        public virtual IOrganization GetOrganization(IUserContext userContext,
                                                    Int32 organizationId)
        {
            return DataSource.GetOrganization(userContext, organizationId);
        }

        /// <summary>
        /// Get organization category by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategoryId">OrganizationCategory id.</param>
        /// <returns>Requested organization category.</returns>       
        public virtual IOrganizationCategory GetOrganizationCategory(IUserContext userContext,
                                                                     Int32 organizationCategoryId)
        {
            return DataSource.GetOrganizationCategory(userContext, organizationCategoryId);
        }


        /// <summary>
        /// GetOrganizationCategories
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of organization types or null if no organization types are found.
        /// </returns>
        public virtual OrganizationCategoryList GetOrganizationCategories(IUserContext userContext)
        {
            return DataSource.GetOrganizationCategories(userContext);
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
        public virtual RoleList GetRolesByOrganization(IUserContext userContext, Int32 organizationId)
        {
            return DataSource.GetOrganizationRoles(userContext, organizationId);
        }

        /// <summary>
        /// Get all organizations 
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of all organizations or null if no organizations exists.
        /// </returns>
        public virtual OrganizationList GetOrganizations(IUserContext userContext)
        {
            return DataSource.GetOrganizations(userContext);
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
        public virtual OrganizationList GetOrganizationsByOrganizationCategory(IUserContext userContext,
                                                                               Int32 organizationCategoryId)
        {
            return DataSource.GetOrganizationsByOrganizationCategory(userContext, organizationCategoryId);
        }

        /// <summary>
        /// Get organizations by organization category
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategory">Organization category object.</param>
        /// <returns>
        /// Returns list of organizations or null if no organizations are categorized as the
        /// specified category.
        /// </returns>
        public virtual OrganizationList GetOrganizationsByOrganizationCategory(IUserContext userContext,
                                                                               OrganizationCategory organizationCategory)
        {
            return GetOrganizationsByOrganizationCategory(userContext, organizationCategory.Id);
        }

        /// <summary>
        /// Get organizations that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Organizations that matches the search criteria</returns>
        public virtual OrganizationList GetOrganizationsBySearchCriteria(IUserContext userContext,
                                                                 IOrganizationSearchCriteria searchCriteria)
        {
            return DataSource.GetOrganizationsBySearchCriteria(userContext, searchCriteria);
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
        public virtual void UpdateOrganization(IUserContext userContext,
                                               IOrganization organization)
        {
            DataSource.UpdateOrganization(userContext, organization);
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
        public virtual void UpdateOrganizationCategory(IUserContext userContext,
                                                       IOrganizationCategory organizationCategory)
        {
            DataSource.UpdateOrganizationCategory(userContext, organizationCategory);
        }

    }
}
