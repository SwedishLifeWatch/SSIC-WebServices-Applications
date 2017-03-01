using System;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Contains extension to the WebUser class.
    /// </summary>
    public static class WebUserExtension
    {
        private static readonly DateTime _dateTimeDefault = DateTime.MinValue;

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="context">Web service request context.</param>
        public static void CheckData(this WebUser user,
                                     WebServiceContext context)
        {
            WebData data;

            data = user;
            data.CheckData();
            user.UserName.CheckNotEmpty("UserName");
            user.UserName.CheckLength(GetUserNameMaxLength(context));
            user.UserName.CheckRegularExpression(Settings.Default.UserNameRegularExpression);
        }

        /// <summary>
        /// Get max string length for password.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for password.</returns>
        public static Int32 GetPasswordMaxLength(WebServiceContext context)
        {
            return context.GetDatabase().GetColumnLength(UserData.TABLE_NAME, UserData.PASSWORD);
        }

        /// <summary>
        /// Get max string length for user name.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Max string length for user name.</returns>
        public static Int32 GetUserNameMaxLength(WebServiceContext context)
        {
            return context.GetDatabase().GetColumnLength(UserData.TABLE_NAME, UserData.USER_NAME);
        }

        /// <summary>
        /// Load data into the WebUser instance.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadData(this WebUser user,
                                    DataReader dataReader)
        {
            user.Id = dataReader.GetInt32(UserData.ID);
            user.IsApplicationIdSpecified = dataReader.IsNotDbNull(UserData.APPLICATION_ID);
            if (user.IsApplicationIdSpecified)
            {
                user.ApplicationId = dataReader.GetInt32(UserData.APPLICATION_ID);
            }
            user.IsPersonIdSpecified = dataReader.IsNotDbNull(UserData.PERSON_ID);
            if (user.IsPersonIdSpecified)
            {
                user.PersonId = dataReader.GetInt32(UserData.PERSON_ID);
            }
            user.Type = (UserType)Enum.Parse(typeof(UserType), dataReader.GetString(UserData.USER_TYPE));
            user.UserName = dataReader.GetString(UserData.USER_NAME);
            user.GUID = dataReader.GetString(UserData.GUID);
            user.IsAccountActivated = dataReader.GetBoolean(UserData.ACCOUNT_ACTIVATED);
            user.EmailAddress = dataReader.GetString(EmailData.EMAIL_ADDRESS);
            user.ShowEmailAddress = dataReader.GetBoolean(EmailData.SHOW_EMAIL);
            user.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull(UserData.ADMINISTRATION_ROLE_ID);
            if (user.IsAdministrationRoleIdSpecified)
            {
                user.AdministrationRoleId = dataReader.GetInt32(UserData.ADMINISTRATION_ROLE_ID);
            }
            user.AuthenticationType = dataReader.GetInt32(UserData.AUTHENTICATION_TYPE);
            user.CreatedDate = dataReader.GetDateTime(UserData.CREATED_DATE, _dateTimeDefault);
            user.CreatedBy = dataReader.GetInt32(UserData.CREATED_BY);
            user.ModifiedDate = dataReader.GetDateTime(UserData.MODIFIED_DATE, _dateTimeDefault);
            user.ModifiedBy = dataReader.GetInt32(UserData.MODIFIED_BY);
            user.ValidFromDate = dataReader.GetDateTime(UserData.VALID_FROM_DATE, _dateTimeDefault);
            user.ValidToDate = dataReader.GetDateTime(UserData.VALID_TO_DATE, _dateTimeDefault);
        }

        /// <summary>
        /// Load data into the WebUser instance. Used for RoleMember data.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name='dataReader'>An open data reader.</param>
        public static void LoadRoleMemberUserData(this WebUser user,
                                                   DataReader dataReader)
        {
            user.Id = dataReader.GetInt32(UserData.USER_ID);
            user.IsApplicationIdSpecified = dataReader.IsNotDbNull(UserData.APPLICATION_ID);
            if (user.IsApplicationIdSpecified)
            {
                user.ApplicationId = dataReader.GetInt32(UserData.APPLICATION_ID);
            }
            user.IsPersonIdSpecified = dataReader.IsNotDbNull(UserData.PERSON_ID);
            if (user.IsPersonIdSpecified)
            {
                user.PersonId = dataReader.GetInt32(UserData.PERSON_ID);
            }
            user.Type = (UserType)Enum.Parse(typeof(UserType), dataReader.GetString(UserData.USER_TYPE));
            user.UserName = dataReader.GetString(UserData.USER_NAME);
            user.GUID = dataReader.GetString("UserGUID");
            user.IsAccountActivated = dataReader.GetBoolean(UserData.ACCOUNT_ACTIVATED);
            user.EmailAddress = dataReader.GetString(EmailData.EMAIL_ADDRESS);
            user.ShowEmailAddress = dataReader.GetBoolean(EmailData.SHOW_EMAIL);
            user.IsAdministrationRoleIdSpecified = dataReader.IsNotDbNull("User_AdministrationRoleId");
            if (user.IsAdministrationRoleIdSpecified)
            {
                user.AdministrationRoleId = dataReader.GetInt32("User_AdministrationRoleId");
            }
            user.AuthenticationType = dataReader.GetInt32(UserData.AUTHENTICATION_TYPE);
            user.CreatedDate = dataReader.GetDateTime("UserCreatedDate", _dateTimeDefault);
            user.CreatedBy = dataReader.GetInt32("UserCreatedBy");
            user.ModifiedDate = dataReader.GetDateTime("UserModifiedDate", _dateTimeDefault);
            user.ModifiedBy = dataReader.GetInt32("UserModifiedBy");
            user.ValidFromDate = dataReader.GetDateTime("UserValidFromDate", _dateTimeDefault);
            user.ValidToDate = dataReader.GetDateTime("UserValidToDate", _dateTimeDefault);
        }

        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='user'>The user.</param>
        public static void CheckData(this WebUser user)
        {
            if (!user.IsDataChecked)
            {
                user.CheckStrings();
                user.IsDataChecked = true;
            }
        }
    }
}
