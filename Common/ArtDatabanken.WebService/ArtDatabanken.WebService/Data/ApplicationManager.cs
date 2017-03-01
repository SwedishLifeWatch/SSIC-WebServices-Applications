using System;
using System.Web.Caching;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class that handles application related information.
    /// </summary>
    public class ApplicationManager : ManagerBase, IApplicationManager
    {
        /// <summary>
        /// Get application by id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationId">Application id.</param>
        /// <returns>Requested application.</returns>       
        public virtual WebApplication GetApplication(WebServiceContext context,
                                                     Int32 applicationId)
        {
            WebClientInformation clientInformation;

            clientInformation = GetClientInformation(context, WebServiceId.UserService);
            return WebServiceProxy.UserService.GetApplication(clientInformation, applicationId);
        }
    }
}
