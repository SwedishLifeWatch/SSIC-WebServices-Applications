using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Class that handles application related information.
    /// </summary>
    public class ApplicationManager : ManagerBase, IApplicationManager 
    {
        /// <summary>
        /// Get application with specified id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="applicationId">Id for the requested application.</param>
        /// <returns>Requested application.</returns>
        public virtual WebApplication GetApplication(WebServiceContext context,
                                                     Int32 applicationId)
        {
            List<WebApplication> applications;
            String cacheKey;
            WebClientInformation clientInformation;

            // Get cached information.
            cacheKey = "Applications";
            applications = (List<WebApplication>)(context.GetCachedObject(cacheKey));

            if (applications.IsEmpty())
            {
                // Get information from user service.
                clientInformation = GetClientInformation(context, WebServiceId.UserService);
                applications = WebServiceProxy.UserService.GetApplications(clientInformation);
                if (applications.IsNotNull())
                {
                    // Add information to cache.
                    context.AddCachedObject(cacheKey, applications, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }

            if (applications.IsNotEmpty())
            {
                foreach (WebApplication application in applications)
                {
                    if (application.Id == applicationId)
                    {
                        return application;
                    }
                }
            }
            return null;
        }
    }
}
