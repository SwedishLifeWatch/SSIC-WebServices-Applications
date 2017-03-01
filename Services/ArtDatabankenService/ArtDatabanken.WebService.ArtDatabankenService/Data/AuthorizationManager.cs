using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Manager that handle authorization.
    /// </summary>
    public class AuthorizationManager : IAuthorizationManager
    {
        /// <summary>
        /// Check that user has the specified access rights.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorityIdentifier">Identifier of authority to verify.</param>
        /// <exception cref="ApplicationException">Thrown if user does not have the specified access rights.</exception>
        public virtual void CheckAuthorization(WebServiceContext context,
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

        /// <summary>
        /// Check that user has the specified access rights.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationIdentifier">Identifier of application to verify.</param>
        /// <param name="authorityIdentifier">Identifier of authority to verify.</param>
        /// <exception cref="ApplicationException">Thrown if user does not have the specified access rights.</exception>
        public virtual void CheckAuthorization(WebServiceContext context,
                                               ApplicationIdentifier applicationIdentifier,
                                               AuthorityIdentifier authorityIdentifier)
        {
            if (applicationIdentifier.ToString() == context.ClientToken.ApplicationIdentifier)
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
