using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
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
        /// AnalysisService.
        /// </summary>
        AnalysisService,

        /// <summary>
        /// GeoReferenceService.
        /// </summary>
        GeoReferenceService,

        /// <summary>
        /// Reference service.
        /// </summary>
        ReferenceService,

        /// <summary>
        /// SwedishSpeciesObservationService
        /// </summary>
        SwedishSpeciesObservationService,

        /// <summary>
        /// SwedishSpeciesObservationSOAPService
        /// </summary>
        SwedishSpeciesObservationSOAPService,

        /// <summary>
        /// Taxon attribute service.
        /// </summary>
        TaxonAttributeService,

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
        /// <returns>
        /// Status for this web service divided into supported locales.
        /// </returns>
        Dictionary<Int32, List<WebResourceStatus>> GetStatus();
    }
}
