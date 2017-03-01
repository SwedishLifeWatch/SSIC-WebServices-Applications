using System;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Definition of the ApplicationManager interface.
    /// </summary>
    public interface IApplicationManager
    {
        /// <summary>
        /// Get application by id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Requested application.</returns>       
        WebApplication GetApplication(WebServiceContext context,
                                      Int32 applicationId);
    }
}
