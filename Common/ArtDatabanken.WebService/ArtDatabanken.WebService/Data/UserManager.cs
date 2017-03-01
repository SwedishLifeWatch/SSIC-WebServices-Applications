using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class that handles user related information.
    /// </summary>
    public class UserManager : ManagerBase, IUserManager
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
            WebClientInformation clientInformation;
            WebUser user;

            user = GetUser(context);
            if (user.Type != UserType.Person)
            {
                throw new Exception("Can't get person for application user :" + user.UserName);
            }

            clientInformation = GetClientInformation(context, WebServiceId.UserService);
            return WebServiceProxy.UserService.GetPerson(clientInformation,
                                                         user.PersonId);
        }

        /// <summary>
        /// Get roles for current web service user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Roles for current web service user. </returns>
        public virtual List<WebRole> GetRoles(WebServiceContext context)
        {
            String cacheKey;
            WebClientInformation clientInformation;
            List<WebRole> roles;

            // Get cached information.
            cacheKey = "RolesForUser:" +
                       context.ClientToken.UserName +
                       ":WhenUsingApplication:" +
                       context.ClientToken.ApplicationIdentifier +
                       ":WithLocale:" +
                       context.Locale.ISOCode;
            roles = (List<WebRole>)context.GetCachedObject(cacheKey);

            if (roles.IsNull())
            {
                // Get information from user service.
                clientInformation = GetClientInformation(context, WebServiceId.UserService);
                roles = WebServiceProxy.UserService.GetRolesByUser(clientInformation,
                                                                   context.GetUser().Id,
                                                                   context.ClientToken.ApplicationIdentifier);
                if (roles.IsNotNull())
                {
                    // Add information to cache.
                    context.AddCachedObject(cacheKey, roles, DateTime.Now + new TimeSpan(1, 0, 0), CacheItemPriority.AboveNormal);
                }
            }

            return roles;
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
            WebClientInformation clientInformation;

            // Get information from user service.
            clientInformation = GetClientInformation(WebServiceId.UserService);
            return WebServiceProxy.UserService.GetUser(clientInformation, userName);
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
        public virtual WebUser GetUser(WebServiceContext context)
        {
            String cacheKey;
            WebClientInformation clientInformation;
            WebUser user;

            // Get cached information.
            cacheKey = "User:" + context.ClientToken.UserName +
                       ":WithLocale:" + context.Locale.ISOCode;
            user = (WebUser)context.GetCachedObject(cacheKey);

            if (user.IsNull())
            {
                // Get information from user service.
                clientInformation = GetClientInformation(context, WebServiceId.UserService);
                user = WebServiceProxy.UserService.GetUser(clientInformation, context.ClientToken.UserName);
                if (user.IsNotNull())
                {
                    // Add information to cache.
                    context.AddCachedObject(cacheKey, user, DateTime.Now + new TimeSpan(1, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return user;
        }

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
        public virtual WebLoginResponse Login(WebServiceContext context, 
                                              String userName,
                                              String password,
                                              String applicationIdentifier,
                                              Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.UserService.Login(userName,
                                                              password,
                                                              applicationIdentifier,
                                                              isActivationRequired);
            if (loginResponse.IsNotNull())
            {
                context.Locale = loginResponse.Locale;
                loginResponse.Token = context.ClientToken.Token;
            }
            return loginResponse;
        }

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public virtual void Logout(WebServiceContext context)
        {
            // TODO: Release resources.
        }
    }
}
