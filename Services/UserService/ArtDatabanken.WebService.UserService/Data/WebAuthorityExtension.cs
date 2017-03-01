using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebAuthority class.
    /// </summary>
    public static class WebAuthorityExtension
    {

        /// <summary>
        /// Load data into the WebAuthority instance.
        /// </summary>
        /// <param name='authority'>The authority.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebAuthority authority,
                                    DataReader dataReader)
        {
            // Authority    
            authority.Id = dataReader.GetInt32(AuthorityData.ID);
            authority.GUID = dataReader.GetString(AuthorityData.GUID);
            authority.Name = dataReader.GetString(AuthorityData.NAME);
            authority.RoleId = dataReader.GetInt32(AuthorityData.ROLE_ID);
            if (dataReader.IsNotDbNull(AuthorityData.APPLICATION_ID))
            {
                authority.ApplicationId = dataReader.GetInt32(AuthorityData.APPLICATION_ID);
            }
            authority.Identifier = dataReader.GetString(AuthorityData.AUTHORITY_IDENTITY);
            //Set type of authority based on application or authority data type/dataType.
            if (dataReader.IsNotDbNull(AuthorityData.AUTHORITY_DATA_TYPE_ID))
            {
                authority.AuthorityDataType = new WebAuthorityDataType(); 
                authority.AuthorityDataType.Id = dataReader.GetInt32(AuthorityDataType.AUTHORITYDATATYPE_ID);
                authority.AuthorityDataType.Identifier = dataReader.GetString(AuthorityDataType.AUTHORITY_DATA_TYPE_IDENTITY);
                authority.AuthorityType = AuthorityType.DataType;
            }
            else
            {
                authority.AuthorityType = AuthorityType.Application;
            }
            authority.ShowNonPublicData = dataReader.GetBoolean(AuthorityData.SHOW_NON_PUBLIC_DATA);
            authority.MaxProtectionLevel = dataReader.GetInt32(AuthorityData.MAX_PROTECTION_LEVEL);
            authority.ReadPermission = dataReader.GetBoolean(AuthorityData.READ_PERMISSION);
            authority.CreatePermission = dataReader.GetBoolean(AuthorityData.CREATE_PERMISSION);
            authority.UpdatePermission = dataReader.GetBoolean(AuthorityData.UPDATE_PERMISSION);
            authority.DeletePermission = dataReader.GetBoolean(AuthorityData.DELETE_PERMISSION);
            authority.Description = dataReader.GetString(AuthorityData.DESCRIPTION);
            authority.Obligation = dataReader.GetString(AuthorityData.OBLIGATION);
            authority.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull(PersonData.ADMINISTRATION_ROLE_ID);
            if (authority.IsAdministrationRoleIdSpecified)
            {
                authority.AdministrationRoleId = dataReader.GetInt32(PersonData.ADMINISTRATION_ROLE_ID);
            }
            authority.CreatedDate = dataReader.GetDateTime(AuthorityData.CREATED_DATE);
            authority.CreatedBy = dataReader.GetInt32(AuthorityData.CREATED_BY, 0);
            authority.ModifiedDate = dataReader.GetDateTime(AuthorityData.MODIFIED_DATE);
            authority.ModifiedBy = dataReader.GetInt32(AuthorityData.MODIFIED_BY, 0);
            authority.ValidFromDate = dataReader.GetDateTime(AuthorityData.VALID_FROM_DATE);
            authority.ValidToDate = dataReader.GetDateTime(AuthorityData.VALID_TO_DATE);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='authority'>The authority.</param>
        public static void CheckData(this WebAuthority authority)
        {
            if (!authority.IsDataChecked)
            {
                authority.CheckStrings();
                authority.IsDataChecked = true;
            }
        }
    }
}
