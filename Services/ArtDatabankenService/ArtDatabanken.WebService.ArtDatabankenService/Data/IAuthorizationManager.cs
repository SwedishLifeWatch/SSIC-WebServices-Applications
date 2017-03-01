using System;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Enumeration of authority identifiers.
    /// </summary>
    public enum AuthorityIdentifier
    {
        /// <summary>
        /// Species fact updater.
        /// </summary>
        EditSpeciesFacts,
        /// <summary>
        /// Species observations.
        /// </summary>
        Sighting,
        /// <summary>
        /// Web service administrator.
        /// </summary>
        WebServiceAdministrator
    }

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
        /// Check that user has the specified access rights.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationIdentifier">Identifier of application to verify.</param>
        /// <param name="authorityIdentifier">Identifier of authority to verify.</param>
        /// <exception cref="ApplicationException">Thrown if user does not have the specified access rights.</exception>
        void CheckAuthorization(WebServiceContext context,
                                ApplicationIdentifier applicationIdentifier,
                                AuthorityIdentifier authorityIdentifier);
    }
}
