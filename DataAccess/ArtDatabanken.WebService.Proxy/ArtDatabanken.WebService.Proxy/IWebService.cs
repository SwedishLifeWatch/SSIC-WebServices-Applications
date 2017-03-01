using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Interface that defines common methods and properties for
    /// ArtDatabankens SOAP web services.
    /// </summary>
    public interface IWebService
    {
        /// <summary>
        /// Clear cache in web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        void ClearCache(WebClientInformation clientInformation);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Get web address of currently used endpoint.
        /// </summary>
        /// <returns>Web address of currently used endpoint.</returns>
        String GetWebAddress();

        /// <summary>
        /// Login user.
        /// </summary>
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
        /// <returns>Web login response or null if login failed.</returns>
        WebLoginResponse Login(String userName,
                               String password,
                               String applicationIdentifier,
                               Boolean isActivationRequired);

        /// <summary>
        /// Logout user from web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        void Logout(WebClientInformation clientInformation);

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        Boolean Ping();
    }
}
