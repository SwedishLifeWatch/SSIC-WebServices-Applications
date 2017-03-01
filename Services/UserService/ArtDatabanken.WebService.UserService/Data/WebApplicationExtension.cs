using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebApplication class.
    /// </summary>
    public static class WebApplicationExtension
    {

        /// <summary>
        /// Load data into the WebApplication instance.
        /// </summary>
        /// <param name='application'>The application.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebApplication application,
                                    DataReader dataReader)
        {
            // Application
            application.Id = dataReader.GetInt32(ApplicationData.ID);
            application.GUID = dataReader.GetString(ApplicationData.GUID);
            application.Identifier = dataReader.GetString(ApplicationData.APPLICATION_IDENTITY);
            application.Name = dataReader.GetString(ApplicationData.NAME);
            application.ShortName = dataReader.GetString(ApplicationData.SHORT_NAME);
            application.URL = dataReader.GetString(ApplicationData.URL);
            application.Description = dataReader.GetString(ApplicationData.DESCRIPTION);
            application.IsContactPersonIdSpecified = dataReader.IsNotDbNull(ApplicationData.CONTACT_PERSON_ID);
            if (application.IsContactPersonIdSpecified)
            {
                application.ContactPersonId = dataReader.GetInt32(ApplicationData.CONTACT_PERSON_ID);
            }
            application.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull(ApplicationData.ADMINISTRATION_ROLE_ID);
            if (application.IsAdministrationRoleIdSpecified)
            {
                application.AdministrationRoleId = dataReader.GetInt32(ApplicationData.ADMINISTRATION_ROLE_ID);
            }
            application.CreatedDate = dataReader.GetDateTime(ApplicationData.CREATED_DATE);
            application.CreatedBy = dataReader.GetInt32(ApplicationData.CREATED_BY, 0);
            application.ModifiedDate = dataReader.GetDateTime(ApplicationData.MODIFIED_DATE);
            application.ModifiedBy = dataReader.GetInt32(ApplicationData.MODIFIED_BY, 0);
            application.ValidFromDate = dataReader.GetDateTime(ApplicationData.VALID_FROM_DATE);
            application.ValidToDate = dataReader.GetDateTime(ApplicationData.VALID_TO_DATE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='application'>The application.</param>
        public static void CheckData(this WebApplication application)
        {
            if (!application.IsDataChecked)
            {
                application.CheckStrings();
                application.IsDataChecked = true;
            }
        }
    }
}
