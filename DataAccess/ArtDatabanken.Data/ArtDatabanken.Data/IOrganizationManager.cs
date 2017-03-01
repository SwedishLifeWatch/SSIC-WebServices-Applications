using System;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of the OrganizationManager interface.
    /// </summary>
    public interface IOrganizationManager : IManager
    {
        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        IUserDataSource DataSource
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
        /// Delete a organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organization">Delete this organization.</param>
        void DeleteOrganization(IUserContext userContext, IOrganization organization);

        /// <summary>
        /// Get organization by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>Requested organization.</returns>       
        IOrganization GetOrganization(IUserContext userContext, Int32 organizationId);

        /// <summary>
        /// GetOrganizationCategories
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>
        /// Returns list of organization types or null if no organization types are found.
        /// </returns>
        OrganizationCategoryList GetOrganizationCategories(IUserContext userContext);

        /// <summary>
        /// Get organization category by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategoryId">OrganizationCategory id.</param>
        /// <returns>Requested organizationCategory.</returns>       
        IOrganizationCategory GetOrganizationCategory(IUserContext userContext, Int32 organizationCategoryId);

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
        /// Get organizations by organization category
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationCategory">Organization category object.</param>
        /// <returns>
        /// Returns list of organizations or null if no organizations are categorized as the
        /// specified category.
        /// </returns>
        OrganizationList GetOrganizationsByOrganizationCategory(IUserContext userContext,
                                                                OrganizationCategory organizationCategory);

        /// <summary>
        /// Get organizations that matches the search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Organizations that matches the search criteria</returns>
        OrganizationList GetOrganizationsBySearchCriteria(IUserContext userContext,
                                                          IOrganizationSearchCriteria searchCriteria);

        /// <summary>
        /// Get roles by organization.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="organizationId">Organization id.</param>
        /// <returns>
        /// Returns list of roles or 
        /// null if organizationid doesn't match or if organization has no roles.
        /// </returns>
        RoleList GetRolesByOrganization(IUserContext userContext, Int32 organizationId);

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

    }
}
