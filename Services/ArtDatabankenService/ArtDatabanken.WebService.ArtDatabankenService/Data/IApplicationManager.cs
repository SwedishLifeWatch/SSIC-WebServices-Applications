using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Enumeration of application identifiers.
    /// </summary>
    public enum ApplicationIdentifier
    {
        /// <summary>
        /// ArtDatabankenSOA.
        /// </summary>
        ArtDatabankenSOA,
        /// <summary>
        /// Dyntaxa.
        /// </summary>
        Dyntaxa,
        /// <summary>
        /// EVA.
        /// </summary>
        EVA,
        /// <summary>
        /// PrintObs.
        /// </summary>
        PrintObs,
        /// <summary>
        /// Web administration.
        /// </summary>
        WebAdministration
    }

    /// <summary>
    /// Definition of the ApplicationManager interface.
    /// </summary>
    public interface IApplicationManager
    {
        /// <summary>
        /// Get application with specified id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationId">Id for the requested application.</param>
        /// <returns>Requested application.</returns>
        WebApplication GetApplication(WebServiceContext context,
                                      Int32 applicationId);
    }
}
