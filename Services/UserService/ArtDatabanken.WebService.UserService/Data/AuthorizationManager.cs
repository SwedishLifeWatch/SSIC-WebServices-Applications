using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.UserService.Data
{
    /// <summary>
    /// Manager that handle authorization in the user service.
    /// </summary>
    public class AuthorizationManager
    {
        /// <summary>
        /// Check that user has the super administrator role.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <exception cref="Exception">Thrown if user does not have super administrator role.</exception>
        public static void CheckSuperAdministrator(WebServiceContext context)
        {
            if (!IsUserAuthorized(context,
                                  Settings.Default.RoleIdForSuperAdministrator,
                                  null,
                                  null,
                                  null))
            {
                throw new Exception(Settings.Default.ErrorMessageIsNotSuperAdministrator);
            }
        }

        /// <summary>
        /// Method that checks whether or not an user is authorized to perform a certain action.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleId">Id of current role</param>
        /// <param name="authorityIdentfier">Code identifier for the authority.</param>
        /// <param name="applicationIdentfier">Code identifier for the application.</param>
        /// <param name="applicationActionIdentfier">Code identifier for the application action</param>
        /// <returns></returns>
        public static Boolean IsUserAuthorized(WebServiceContext context, 
                                               Int32 ?roleId, 
                                               String authorityIdentfier, 
                                               String applicationIdentfier,
                                               String applicationActionIdentfier)
        {
            if (roleId.IsNotNull() && (authorityIdentfier.IsNotEmpty() || applicationIdentfier.IsNotEmpty() || applicationActionIdentfier.IsNotEmpty()))
            {
                if (context.GetUserDatabase().IsUserAuthorized(context.GetUser().Id, roleId, null, null, null))
                {
                    return true;
                }
                else
                {
                    return context.GetUserDatabase().IsUserAuthorized(context.GetUser().Id, null, authorityIdentfier, applicationIdentfier, applicationActionIdentfier);
                }

            }
            return context.GetUserDatabase().IsUserAuthorized(context.GetUser().Id, roleId, authorityIdentfier, applicationIdentfier, applicationActionIdentfier);
        }

        ///// <summary>
        ///// Check that user has the specified access rights with update permission.
        ///// </summary>
        ///// <param name="context">Web service request context.</param>
        ///// <param name="authorityIdentifier">Identifier of authority to verify.</param>
        ///// <exception cref="ApplicationException">Thrown if user does not have the specified access rights.</exception>
        //public static void CheckAuthorizationWithUpdatePermission(
        //    WebServiceContext context,
        //    AuthorityIdentifier authorityIdentifier)
        //{
        //    foreach (WebRole role in context.GetRoles())
        //    {
        //        if (role.Authorities.IsNotEmpty())
        //        {
        //            foreach (WebAuthority authority in role.Authorities)
        //            {
        //                if (authority.Identifier == authorityIdentifier.ToString())
        //                {
        //                    if (authority.UpdatePermission)
        //                    {
        //                        // User has the specified access rights.
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    throw new ApplicationException("User:" +
        //                                   context.GetUser().UserName +
        //                                   " does not have authority with identifier:" +
        //                                   authorityIdentifier.ToString() +
        //                                   " in application:" +
        //                                   context.ClientToken.ApplicationIdentifier);
        //}

        /// <summary>
        /// Check that user has the specified access rights.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorityIdentifier">Identifier of authority to verify.</param>
        /// <exception cref="ApplicationException">Thrown if user does not have the specified access rights.</exception>
        public static void CheckAuthorization(WebServiceContext context,
                                               AuthorityIdentifier authorityIdentifier)
        {
            foreach (WebRole role in context.GetRoles())
            {
                if (role.Authorities.IsNotEmpty())
                {
                    foreach (WebAuthority authority in role.Authorities)
                    {
                        if (authority.Identifier == authorityIdentifier.ToString())
                        {
                            
                            // User has the specified access rights.
                            return;
                        }
                    }
                }
            }

            throw new ApplicationException("User:" +
                                           context.GetUser().UserName +
                                           " does not have authority with identifier:" +
                                           authorityIdentifier.ToString() +
                                           " in application:" +
                                           context.ClientToken.ApplicationIdentifier);
        }
    }
}
