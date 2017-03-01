using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Enumeration of web services in ArtDatabankenSOA.
    /// </summary>
    public enum WebServiceId
    {
        /// <summary>
        /// ArtDatabankenService.
        /// </summary>
        ArtDatabankenService,

        /// <summary>
        /// GeoReferenceService.
        /// </summary>
        GeoReferenceService,

        /// <summary>
        /// SwedishSpeciesObservationSOAPService
        /// </summary>
        SwedishSpeciesObservationSOAPService,

        /// <summary>
        /// TaxonService
        /// </summary>
        TaxonService,

        /// <summary>
        /// UserService
        /// </summary>
        UserService
    }

    /// <summary>
    /// Interface that handles information related to 
    /// the web service that this project is included into.
    /// </summary>
    public interface IWebServiceManager
    {
        /// <summary>
        /// Encryption key that is used in production.
        /// </summary>
        String Key
        { get; }

        /// <summary>
        /// Web service user name in UserService.
        /// </summary>
        String Name
        { get; }

        /// <summary>
        /// Web service password in UserService.
        /// </summary>
        String Password
        { get; }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Status for this web service.</returns>       
        List<WebResourceStatus> GetStatus(WebServiceContext context);

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        Boolean Ping(WebServiceContext context);
    }
}
