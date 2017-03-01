using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Class that handles user related information.
    /// </summary>
    public class UserManager : ManagerBase, IUserManager
    {
        /// <summary>
        /// Get person for current web service user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Person for current web service user.</returns>
        public virtual WebPerson GetPerson(WebServiceContext context)
        {
            String cacheKey;
            WebClientInformation clientInformation;
            WebPerson person;
            WebService.Data.WebUser user;

            user = GetUser(context);
            if (user.Type != ArtDatabanken.Data.UserType.Person)
            {
                return null;
            }

            // Get cached information.
            cacheKey = "PersonForUser:" + context.ClientToken.UserName +
                       ":WithLocale:" + context.Locale.ISOCode;
            person = (WebPerson)context.GetCachedObject(cacheKey);

            if (person.IsNull())
            {
                // Get information from user service.
                clientInformation = GetClientInformation(context, WebServiceId.UserService);
                person = WebServiceProxy.UserService.GetPerson(clientInformation,
                                                               user.PersonId);
                if (person.IsNotNull())
                {
                    // Add information to cache.
                    context.AddCachedObject(cacheKey, person, DateTime.Now + new TimeSpan(1, 0, 0), CacheItemPriority.AboveNormal);
                }
            }

            return person;
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
        /// Get information about current web service user.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Returns user information or null if 
        /// user information is not valid.
        /// It is only during login that the
        /// user information can be invalid.
        /// </returns>
        public virtual ArtDatabanken.WebService.Data.WebUser GetUser(WebServiceContext context)
        {
            String cacheKey;
            WebClientInformation clientInformation;
            ArtDatabanken.WebService.Data.WebUser user;

            // Get cached information.
            cacheKey = "User:" + context.ClientToken.UserName +
                       ":WithLocale:" + context.Locale.ISOCode;
            user = (ArtDatabanken.WebService.Data.WebUser)context.GetCachedObject(cacheKey);

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

