using System;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebRole class.
    /// </summary>
    public static class WebRoleExtension
    {
        /// <summary>
        /// Get a copy of the role object.
        /// </summary>
        /// <param name='role'>The role.</param>
        /// <returns>A copy of the role object.</returns>
        public static WebRole Clone(this WebRole role)
        {
            WebRole clone;

            clone = new WebRole();
            clone.AdministrationRoleId = role.AdministrationRoleId;
            clone.Authorities = role.Authorities;
            clone.CreatedBy = role.CreatedBy;
            clone.CreatedDate = role.CreatedDate;
            clone.DataFields = role.DataFields;
            clone.Description = role.Description;
            clone.GUID = role.GUID;
            clone.Id = role.Id;
            clone.Identifier = role.Identifier;
            clone.IsActivationRequired = role.IsActivationRequired;
            clone.IsAdministrationRoleIdSpecified = role.IsAdministrationRoleIdSpecified;
            clone.IsOrganizationIdSpecified = role.IsOrganizationIdSpecified;
            clone.IsUserAdministrationRole = role.IsUserAdministrationRole;
            clone.IsUserAdministrationRoleIdSpecified = role.IsUserAdministrationRoleIdSpecified;
            clone.MessageTypeId = role.MessageTypeId;
            clone.ModifiedBy = role.ModifiedBy;
            clone.ModifiedDate = role.ModifiedDate;
            clone.Name = role.Name;
            clone.OrganizationId = role.OrganizationId;
            clone.ShortName = role.ShortName;
            clone.UserAdministrationRoleId = clone.UserAdministrationRoleId;
            clone.ValidFromDate = role.ValidFromDate;
            clone.ValidToDate = role.ValidToDate;
            return clone;
        }

        /// <summary>
        /// Load data into the WebRole instance.
        /// </summary>
        /// <param name='role'>The role.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebRole role,
                                    DataReader dataReader)
        {
            Int32 isUserAdministrationRole;

            // Role
            role.Id = dataReader.GetInt32(RoleData.ID);
            role.GUID = dataReader.GetString(RoleData.GUID);
            role.Name = dataReader.GetString(RoleData.ROLE_NAME);
            role.ShortName = dataReader.GetString(RoleData.SHORT_NAME);
            role.Description = dataReader.GetString(RoleData.DESCRIPTION);
            role.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull(RoleData.ADMINISTRATION_ROLE_ID);
            role.AdministrationRoleId = dataReader.GetInt32(RoleData.ADMINISTRATION_ROLE_ID, 0);
            role.IsActivationRequired = dataReader.GetBoolean(RoleData.IS_ACTIVATION_REQUIRED);
            role.MessageTypeId = dataReader.GetInt32(RoleData.MESSAGE_TYPE_ID);
            role.IsUserAdministrationRoleIdSpecified = dataReader.IsNotDbNull(RoleData.USER_ADMINISTRATION_ROLE_ID);
            role.UserAdministrationRoleId = dataReader.GetInt32(RoleData.USER_ADMINISTRATION_ROLE_ID, 0);
            role.CreatedDate = dataReader.GetDateTime(RoleData.CREATED_DATE);
            role.CreatedBy = dataReader.GetInt32(RoleData.CREATED_BY);
            role.ModifiedDate = dataReader.GetDateTime(RoleData.MODIFIED_DATE);
            role.ModifiedBy = dataReader.GetInt32(RoleData.MODIFIED_BY);
            role.ValidFromDate = dataReader.GetDateTime(RoleData.VALID_FROM_DATE);
            role.ValidToDate = dataReader.GetDateTime(RoleData.VALID_TO_DATE);
            role.IsOrganizationIdSpecified = dataReader.IsNotDbNull(RoleData.ORGANIZATION_ID);
            role.OrganizationId = dataReader.GetInt32(RoleData.ORGANIZATION_ID, 0);
            role.Identifier = dataReader.GetString(RoleData.IDENTIFIER);
            isUserAdministrationRole = dataReader.GetInt32(RoleData.IS_USER_ADMINISTRATION_ROLE);
            role.IsUserAdministrationRole = (isUserAdministrationRole != 0);
        }

        /// <summary>
        /// Check the data in current object.
        /// </summary>
        /// <param name='role'>The role.</param>
        public static void CheckData(this WebRole role)
        {
            if (!role.IsDataChecked)
            {
                role.CheckStrings();
                role.IsDataChecked = true;
            }
        }
    }
}
