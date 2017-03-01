using System;

namespace ArtDatabanken.WebService.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Manager that handle authorization.
    /// </summary>
    public interface IAuthorizationManager
    {
        /// <summary>
        /// Check that user has the specified access rights.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorityIdentifier">Identifier of authority to verify.</param>
        /// <exception cref="ApplicationException">Thrown if user does not have the specified access rights.</exception>
        void CheckAuthorization(WebServiceContext context,
                                AuthorityIdentifier authorityIdentifier);

        /// <summary>
        /// Check if users current roles identifier matches the parameter roleIdentifier
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="roleIdentifier">Identifier to check.</param>
        /// <returns>
        /// true if roleIdentifier matches current roles identifier
        /// false in any other case
        /// </returns>
        Boolean IsIdentiferInCurrentRole(WebServiceContext context,
                                         String roleIdentifier);

        /// <summary>
        /// Check if user has access rights to none public data.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="authorityIdentifiers">Identifiers of authoritys to verify.</param>
        /// <returns>
        /// True, if user has access rights to none public data.
        /// </returns>
        Boolean IsNonePublicDataAuthorized(WebServiceContext context,
                                           List<AuthorityIdentifier> authorityIdentifiers);
    }
}
