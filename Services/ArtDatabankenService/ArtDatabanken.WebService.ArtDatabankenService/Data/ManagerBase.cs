using System;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class adds functionality that are usefull
    /// in many manager related implementations.
    /// </summary>
    public class ManagerBase
    {
        /// <summary>
        /// Get client information that this web service can use when
        /// communicating with the specified web service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webService">Web service to communicate with.</param>
        /// <returns>Client information.</returns>
        public WebClientInformation GetClientInformation(WebServiceContext context,
                                                         WebServiceId webService)
        {
            WebClientInformation clientInformation;

            clientInformation = new WebClientInformation();
            clientInformation.Locale = context.Locale;
            clientInformation.Role = null;
            clientInformation.Token = GetToken(context, webService);
            return clientInformation;
        }

        /// <summary>
        /// Get token that this web service can use when
        /// communicating with the specified web service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webService">Web service to communicate with.</param>
        /// <returns>Requested token.</returns>
        private String GetToken(WebServiceContext context,
                                WebServiceId webService)
        {
            String token;
            String cacheKey;
            WebLoginResponse loginResponse;

            // Get cached information.
            cacheKey = "TokenForWebService:" +
                       WebServiceData.WebServiceManager.Name +
                       ":WhenUsingWebService:" +
                       webService.ToString();
            token = (String)context.GetCachedObject(cacheKey);

            if (token.IsNull())
            {
                // Log in to web service.
                switch (webService)
                {
                    case WebServiceId.GeoReferenceService:
                        loginResponse = WebServiceProxy.GeoReferenceService.Login(WebServiceData.WebServiceManager.Name,
                                                                                  WebServiceData.WebServiceManager.Password,
                                                                                  ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                                  false);
                        break;
                    case WebServiceId.SwedishSpeciesObservationSOAPService:
                        loginResponse = WebServiceProxy.SwedishSpeciesObservationSOAPService.Login(WebServiceData.WebServiceManager.Name,
                                                                                                   WebServiceData.WebServiceManager.Password,
                                                                                                   ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                                                   false);
                        break;
                    case WebServiceId.TaxonService:
                        loginResponse = WebServiceProxy.TaxonService.Login(WebServiceData.WebServiceManager.Name,
                                                                           WebServiceData.WebServiceManager.Password,
                                                                           ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                           false);
                        break;
                    case WebServiceId.UserService:
                        loginResponse = WebServiceProxy.UserService.Login(WebServiceData.WebServiceManager.Name,
                                                                          WebServiceData.WebServiceManager.Password,
                                                                          ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                          false);
                        break;
                    default:
                        throw new ApplicationException();
                }
                if (loginResponse.IsNotNull())
                {
                    token = loginResponse.Token;

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, token, DateTime.Now + new TimeSpan(24, 0, 0), CacheItemPriority.AboveNormal);
                }
            }

            return token;
        }

        /// <summary>
        /// Get user context for this web service.
        /// </summary>
        /// <param name="webServiceContext">Web service request context.</param>
        /// <returns>Requested token.</returns>
        public static IUserContext GetUserContext(WebServiceContext webServiceContext)
        {
            IUserContext userContext;
            String cacheKey;

            // Get cached information.
            cacheKey = "UserContext:" + WebServiceData.WebServiceManager.Name;
            userContext = (IUserContext)webServiceContext.GetCachedObject(cacheKey);

            if (userContext.IsNull())
            {
                // Log in to onion.
                userContext = CoreData.UserManager.Login(WebServiceData.WebServiceManager.Name,
                                                         WebServiceData.WebServiceManager.Password,
                                                         ApplicationIdentifier.ArtDatabankenSOA.ToString());
                if (userContext.IsNotNull())
                {
                    // Set locale to swedish.
                    // ArtDatabankenService only handles swedish.
                    userContext.Locale = CoreData.LocaleManager.GetLocale(userContext, LocaleId.sv_SE);

                    // Add information to cache.
                    webServiceContext.AddCachedObject(cacheKey,
                                                      userContext,
                                                      DateTime.Now + new TimeSpan(24, 0, 0),
                                                      CacheItemPriority.AboveNormal);
                }
            }

            return userContext;
        }
    }
}
