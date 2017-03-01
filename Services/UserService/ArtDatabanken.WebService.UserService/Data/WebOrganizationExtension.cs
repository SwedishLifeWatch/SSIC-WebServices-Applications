using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebOrganization class.
    /// </summary>
    public static class WebOrganizationExtension
    {

        /// <summary>
        /// Load data into the WebOrganization instance.
        /// </summary>
        /// <param name='organization'>The organization.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebOrganization organization,
                                    DataReader dataReader)
        {
            // Organization
            organization.Id = dataReader.GetInt32(OrganizationData.ID);
            organization.GUID = dataReader.GetString(OrganizationData.GUID);
            organization.Name = dataReader.GetString(OrganizationData.NAME);
            organization.ShortName = dataReader.GetString(OrganizationData.SHORT_NAME);
            organization.CategoryId = dataReader.GetInt32(OrganizationData.ORGANIZATION_CATEGORY_ID);
            organization.Description = dataReader.GetString(OrganizationData.DESCRIPTION);
            organization.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull(OrganizationData.ADMINISTRATION_ROLE_ID);
            if (organization.IsAdministrationRoleIdSpecified)
            {
                organization.AdministrationRoleId = dataReader.GetInt32(OrganizationData.ADMINISTRATION_ROLE_ID);
            }
            organization.HasSpeciesCollection = dataReader.GetBoolean(OrganizationData.HAS_COLLECTION);
            organization.CategoryId = dataReader.GetInt32(OrganizationData.ORGANIZATION_CATEGORY_ID);
            organization.CreatedDate = dataReader.GetDateTime(OrganizationData.CREATED_DATE);
            organization.CreatedBy = dataReader.GetInt32(OrganizationData.CREATED_BY, 0);
            organization.ModifiedDate = dataReader.GetDateTime(OrganizationData.MODIFIED_DATE);
            organization.ModifiedBy = dataReader.GetInt32(OrganizationData.MODIFIED_BY, 0);
            organization.ValidFromDate = dataReader.GetDateTime(OrganizationData.VALID_FROM_DATE);
            organization.ValidToDate = dataReader.GetDateTime(OrganizationData.VALID_TO_DATE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='organization'>The Organization.</param>
        public static void CheckData(this WebOrganization organization)
        {
            if (!organization.IsDataChecked)
            {
                organization.CheckStrings();
                organization.IsDataChecked = true;
            }
        }
    }
}
