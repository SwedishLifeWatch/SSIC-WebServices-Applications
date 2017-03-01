using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebApplicationAction class.
    /// </summary>
    public static class WebApplicationActionExtension
    {

        /// <summary>
        /// Load data into the WebApplicationAction instance.
        /// </summary>
        /// <param name='applicationAction'>The ApplicationAction object.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebApplicationAction applicationAction,
                                    DataReader dataReader)
        {
            applicationAction.Id = dataReader.GetInt32(ApplicationActionData.ID);
            applicationAction.ApplicationId = dataReader.GetInt32(ApplicationActionData.APPLICATION_ID);
            applicationAction.Identifier = dataReader.GetString(ApplicationActionData.ACTION_IDENTITY);
            applicationAction.Name = dataReader.GetString(ApplicationActionData.NAME);
            applicationAction.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull(ApplicationActionData.ADMINISTRATION_ROLE_ID);
            if (applicationAction.IsAdministrationRoleIdSpecified)
            {
                applicationAction.AdministrationRoleId = dataReader.GetInt32(ApplicationActionData.ADMINISTRATION_ROLE_ID);
            }
            applicationAction.Description = dataReader.GetString(ApplicationActionData.DESCRIPTION);
            applicationAction.GUID = dataReader.GetString(ApplicationActionData.GUID);
            applicationAction.CreatedDate = dataReader.GetDateTime(ApplicationActionData.CREATED_DATE);
            applicationAction.CreatedBy = dataReader.GetInt32(ApplicationActionData.CREATED_BY);
            applicationAction.ModifiedDate = dataReader.GetDateTime(ApplicationActionData.MODIFIED_DATE);
            applicationAction.ModifiedBy = dataReader.GetInt32(ApplicationActionData.MODIFIED_BY);
            applicationAction.ValidFromDate = dataReader.GetDateTime(ApplicationActionData.VALID_FROM_DATE);
            applicationAction.ValidToDate = dataReader.GetDateTime(ApplicationActionData.VALID_TO_DATE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='applicationAction'>The application action.</param>
        public static void CheckData(this WebApplicationAction applicationAction)
        {
            if (!applicationAction.IsDataChecked)
            {
                applicationAction.CheckStrings();
                applicationAction.IsDataChecked = true;
            }
        }

    }
}
