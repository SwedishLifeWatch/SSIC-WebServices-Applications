using System;
using System.Collections.Generic;
using System.Net;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Delegate for event handling after user has logged in.
    /// </summary>
    public delegate void UserLoggedInEventHandler();

    /// <summary>
    /// Delegate for event handling after user has logged out.
    /// </summary>
    public delegate void UserLoggedOutEventHandler();

    /// <summary>
    /// This class contains handling of user information.
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// Event handling after user has logged in.
        /// </summary>
        public static event UserLoggedInEventHandler UserLoggedInEvent = null;

        /// <summary>
        /// Event handling after user has logged out.
        /// </summary>
        public static event UserLoggedOutEventHandler UserLoggedOutEvent = null;

        private static User _user = null;

        /// <summary>
        /// Get user.
        /// </summary>
        /// <returns>Logged in user or null if user has not logged in.</returns>       
        public static User GetUser()
        {
            return _user;
        }

        /// <summary>
        /// Convert a WebUser instance into a User instance.
        /// </summary>
        /// <param name="webUser">The web user instance to convert.</param>
        /// <returns>A user instance with information from the WebUser instance.</returns>       
        private static User GetUser(WebUser webUser)
        {
            User user;

            user = new User(webUser.Id,
                            webUser.FirstName,
                            webUser.LastName,
                            webUser.UserName,
                            GetUserRoles(webUser.Roles));
            return user;
        }

        /// <summary>
        /// Convert a WebUserRole array into a UserRoleList.
        /// </summary>
        /// <param name="webUserRoles">The web user role array to convert.</param>
        /// <returns>A user role list instance.</returns>       
        private static UserRoleList GetUserRoles(List<WebUserRole> webUserRoles)
        {
            UserRoleList userRoles;

            userRoles = new UserRoleList();
            foreach (WebUserRole webUserRole in webUserRoles)
            {
                userRoles.Add(new UserRole(webUserRole.Id,
                                           webUserRole.Name,
                                           webUserRole.Description));
            }
            return userRoles;
        }

        /// <summary>
        /// Test if user has logged in.
        /// </summary>
        /// <returns>True if user has logged in.</returns>       
        public static Boolean IsUserLoggedIn()
        {
            return _user.IsNotNull();
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>True if user was logged in.</returns>       
        public static Boolean   Login(String userName,
                                    String password,
                                    String applicationIdentifier,
                                    Boolean isActivationRequired)
        {
            String errorMessage;

            return Login(userName, password, applicationIdentifier, isActivationRequired, out errorMessage);
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <param name="errorMessage">Possible error message if login failed.</param>
        /// <returns>True if user was logged in.</returns>       
        public static Boolean Login(String userName,
                                    String password,
                                    String applicationIdentifier,
                                    Boolean isActivationRequired,
                                    out String errorMessage)
        {
            Boolean isUserLoggedIn = false;

            // Check arguments.
            userName.CheckNotEmpty("userName");
            password.CheckNotEmpty("password");

            // Make sure the user is not already logged in.
            Logout();

            // Loggin.
            try
            {
                errorMessage = null;
                isUserLoggedIn = WebServiceClient.Login(userName, password, applicationIdentifier, isActivationRequired);
            }
            catch (WebException)
            {
                // No contact with web service.
                errorMessage = "Validering av ditt konto misslyckades!\nIngen kontakt mot servern gick att erhålla.";
            }
            catch (Exception exception)
            {
                errorMessage = "Validering av ditt konto misslyckades!\nServern returnerade ett fel.\n" + exception.Message;
            }
            if (isUserLoggedIn)
            {
                // Get user information.
                _user = GetUser(WebServiceClient.GetUser());

                // Fire user logged in event.
                if (UserLoggedInEvent.IsNotNull())
                {
                    UserLoggedInEvent();
                }
            }

            return isUserLoggedIn;
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        public static void Logout()
        {
            _user = null;
            WebServiceClient.Logout();

            // Fire user logged out event.
            if (UserLoggedOutEvent.IsNotNull())
            {
                UserLoggedOutEvent();
            }
        }
    }
}

