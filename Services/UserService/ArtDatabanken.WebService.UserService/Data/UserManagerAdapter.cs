using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.UserService.Database;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Class that handles user related information.
    /// </summary>
    public class UserManagerAdapter : IUserManager
    {
        /// <summary>
        /// Get information about current web service user.
        /// It is assumed that current web service user
        /// is a person user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Returns person information.</returns>
        public virtual WebPerson GetPerson(WebServiceContext context)
        {
            WebUser user;

            user = GetUser(context);
            if (user.Type != UserType.Person)
            {
                throw new Exception("Can't get person for application user :" + user.UserName);
            }

            return UserManager.GetPerson(context, user.PersonId);
        }

        /// <summary>
        /// Get roles for current web service user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Roles for current web service user. </returns>
        public virtual List<WebRole> GetRoles(WebServiceContext context)
        {
            return UserManager.GetRolesByUser(context,
                                              UserManager.GetUserId(context),
                                              context.ClientToken.ApplicationIdentifier);
        }

        /// <summary>
        /// Get information about a web service user.
        /// This method should only be used for logging purpose
        /// when client token is not accepted.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>
        /// Returns user information or null if
        /// user information is not valid.
        /// It is only during login that the
        /// user information can be invalid.
        /// </returns>
        public virtual WebUser GetUser(String userName)
        {
            WebUser user;

            // Check data.
            userName.CheckNotEmpty("userName");
            userName = userName.CheckInjection();

            // Get information from database.
            user = null;
            using (UserServer database = new UserServer())
            {
                using (DataReader dataReader = database.GetUser(userName))
                {
                    if (dataReader.Read())
                    {
                        user = new WebUser();
                        user.LoadData(dataReader);
                    }
                }
            }

            return user;
        }

        /// <summary>
        /// Get information about current web service user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns user information or null if 
        /// user information is not valid.
        /// It is only during login that the
        /// user information can be invalid.
        /// </returns>
        public WebUser GetUser(WebServiceContext context)
        {
            return UserManager.GetUser(context);
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">User password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succeed.
        /// </param>
        /// <returns>User context or null if login failed.</returns>
        public WebLoginResponse Login(WebServiceContext context,
                                      String userName,
                                      String password,
                                      String applicationIdentifier,
                                      Boolean isActivationRequired)
        {
            return UserManager.Login(context,
                                     userName,
                                     password,
                                     applicationIdentifier,
                                     isActivationRequired);
        }

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public void Logout(WebServiceContext context)
        {
            UserManager.Logout(context);
        }
    }
}
