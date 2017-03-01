using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
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
            String authorityIdentifierString;

            authorityIdentifierString = authorityIdentifier.ToString();
            foreach (WebRole role in context.GetRoles())
            {
                if (role.Authorities.IsNotEmpty())
                {
                    foreach (WebAuthority authority in role.Authorities)
                    {
                        if (authority.Identifier == authorityIdentifierString)
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
                                           authorityIdentifier +
                                           " in application:" +
                                           context.ClientToken.ApplicationIdentifier);
        }

        /// <summary>
        /// Check if users current roles identifier matches the parameter roleIdentifier
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleIdentifier">Identifier to check.</param>
        /// <returns>
        /// true if roleIdentifier matches current roles identifier
        /// false in any other case
        /// </returns>
        public virtual Boolean IsIdentiferInCurrentRole(WebServiceContext context,
                                                        String roleIdentifier)
        {
            Int32 n = -1;
            if (context.CurrentRole.IsNotNull())
            {
                n = context.CurrentRole.Identifier.IndexOf(roleIdentifier);
            }
            return (n > -1);
        }

        /// <summary>
        /// Check if user has access rights to none public data.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorityIdentifiers">Identifiers of authoritys to verify.</param>
        /// <returns>
        /// True, if user has access rights to none public data.
        /// </returns>
        public virtual Boolean IsNonePublicDataAuthorized(WebServiceContext context,
                                                          List<AuthorityIdentifier> authorityIdentifiers)
        {
            foreach (WebRole role in context.CurrentRoles)
            {
                if (role.Authorities.IsNotEmpty())
                {
                    foreach (WebAuthority authority in role.Authorities)
                    {
                        if (authorityIdentifiers.IsNotEmpty())
                        {
                            foreach (AuthorityIdentifier authorityIdentifier in authorityIdentifiers)
                            {
                                if ((authority.Identifier == authorityIdentifier.ToString()) &&
                                    authority.ShowNonPublicData)
                                {
                                    // User has the specified access rights.
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
