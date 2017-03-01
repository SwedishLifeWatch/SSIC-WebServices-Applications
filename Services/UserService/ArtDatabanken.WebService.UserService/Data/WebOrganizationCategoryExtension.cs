using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebOrganizationCategory class.
    /// </summary>
    public static class WebOrganizationCategoryExtension
    {

        /// <summary>
        /// Load data into the WebOrganizationCategory instance.
        /// </summary>
        /// <param name="organizationCategory">This authority attribute type.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebOrganizationCategory organizationCategory,
                                    DataReader dataReader)
        {
            organizationCategory.Id = dataReader.GetInt32(OrganizationCategoryData.ID);
            organizationCategory.Name = dataReader.GetString(OrganizationCategoryData.ORGANIZATION_CATEGORY_NAME);
            organizationCategory.Description = dataReader.GetString(OrganizationCategoryData.ORGANIZATION_CATEGORY_DESCRIPTION);
            organizationCategory.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull(OrganizationCategoryData.ADMINISTRATION_ROLE_ID);
            if (organizationCategory.IsAdministrationRoleIdSpecified)
            {
                organizationCategory.AdministrationRoleId = dataReader.GetInt32(OrganizationCategoryData.ADMINISTRATION_ROLE_ID);
            }
            organizationCategory.CreatedDate = dataReader.GetDateTime(OrganizationCategoryData.CREATED_DATE);
            organizationCategory.CreatedBy = dataReader.GetInt32(OrganizationCategoryData.CREATED_BY, 0);
            organizationCategory.ModifiedDate = dataReader.GetDateTime(OrganizationCategoryData.MODIFIED_DATE);
            organizationCategory.ModifiedBy = dataReader.GetInt32(OrganizationCategoryData.MODIFIED_BY, 0);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='organizationCategory'>The OrganizationCategory.</param>
        public static void CheckData(this WebOrganizationCategory organizationCategory)
        {
            if (!organizationCategory.IsDataChecked)
            {
                organizationCategory.CheckStrings();
                organizationCategory.IsDataChecked = true;
            }
        }
    }
}
