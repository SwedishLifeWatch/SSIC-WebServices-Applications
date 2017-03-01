using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebApplicationVersion class.
    /// </summary>
    public static class WebApplicationVersionExtension
    {
        /// <summary>
        /// Load data into the WebApplicationVersion instance.
        /// </summary>
        /// <param name='applicationVersion'>The ApplicationVersion object.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebApplicationVersion applicationVersion,
                                    DataReader dataReader)
        {
            applicationVersion.Id = dataReader.GetInt32(ApplicationVersionData.ID);
            applicationVersion.ApplicationId = dataReader.GetInt32(ApplicationVersionData.APPLICATION_ID);
            applicationVersion.Version = dataReader.GetString(ApplicationVersionData.VERSION);
            applicationVersion.IsRecommended = dataReader.GetBoolean(ApplicationVersionData.IS_RECOMMENDED);
            applicationVersion.IsValid = dataReader.GetBoolean(ApplicationVersionData.IS_VALID);
            applicationVersion.Description = dataReader.GetString(ApplicationVersionData.DESCRIPTION);
            applicationVersion.CreatedDate = dataReader.GetDateTime(ApplicationVersionData.CREATED_DATE);
            applicationVersion.CreatedBy = dataReader.GetInt32(ApplicationVersionData.CREATED_BY);
            applicationVersion.ModifiedDate = dataReader.GetDateTime(ApplicationVersionData.MODIFIED_DATE);
            applicationVersion.ModifiedBy = dataReader.GetInt32(ApplicationVersionData.MODIFIED_BY);
            applicationVersion.ValidFromDate = dataReader.GetDateTime(ApplicationVersionData.VALID_FROM_DATE);
            applicationVersion.ValidToDate = dataReader.GetDateTime(ApplicationVersionData.VALID_TO_DATE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='applicationVersion'>The app version.</param>
        public static void CheckData(this WebApplicationVersion applicationVersion)
        {
            if (!applicationVersion.IsDataChecked)
            {
                applicationVersion.CheckStrings();
                applicationVersion.IsDataChecked = true;
            }
        }
    }
}
