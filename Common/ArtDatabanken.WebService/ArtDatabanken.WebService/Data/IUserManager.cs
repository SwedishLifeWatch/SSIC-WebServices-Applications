using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Definition of the UserManager interface.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Get information about current web service user.
        /// It is assumed that current web service user
        /// is a person user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Returns person information.</returns>
        WebPerson GetPerson(WebServiceContext context);

        /// <summary>
        /// Get roles for current web service user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Roles for current web service user. </returns>
        List<WebRole> GetRoles(WebServiceContext context);

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
        WebUser GetUser(String userName);

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
        WebUser GetUser(WebServiceContext context);


        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succed.
        /// </param>
        /// <returns>User context or null if login failed.</returns>
        WebLoginResponse Login(WebServiceContext context,
                               String userName,
                               String password,
                               String applicationIdentifier,
                               Boolean isActivationRequired);

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        void Logout(WebServiceContext context);
    }
}
