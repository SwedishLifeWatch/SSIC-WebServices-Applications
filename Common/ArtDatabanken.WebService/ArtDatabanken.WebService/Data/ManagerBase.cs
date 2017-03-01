using System;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class adds functionality that are useful
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
        protected WebClientInformation GetClientInformation(WebServiceId webService)
        {
            WebClientInformation clientInformation;

            clientInformation = new WebClientInformation();
            clientInformation.Locale = new WebLocale();
            clientInformation.Locale.Id = (Int32)(LocaleId.en_GB);
            clientInformation.Role = null;
            clientInformation.Token = GetToken(webService);
            return clientInformation;
        }

        /// <summary>
        /// Get client information that this web service can use when
        /// communicating with the specified web service.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webService">Web service to communicate with.</param>
        /// <returns>Client information.</returns>
        protected WebClientInformation GetClientInformation(WebServiceContext context,
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
        /// This method should only be used for logging purpose
        /// when client token is not accepted.
        /// </summary>
        /// <param name="webService">Web service to communicate with.</param>
        /// <returns>Requested token.</returns>
        private String GetToken(WebServiceId webService)
        {
            String token;
            WebLoginResponse loginResponse;

            // Log in to web service.
            token = null;
            switch (webService)
            {
                case WebServiceId.AnalysisService:
                    loginResponse = WebServiceProxy.AnalysisService.Login(WebServiceData.WebServiceManager.Name,
                                                                            WebServiceData.WebServiceManager.Password,
                                                                            ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                            false);
                    break;

                case WebServiceId.GeoReferenceService:
                    loginResponse = WebServiceProxy.GeoReferenceService.Login(WebServiceData.WebServiceManager.Name,
                                                                                WebServiceData.WebServiceManager.Password,
                                                                                ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                                false);
                    break;

                case WebServiceId.ReferenceService:
                    loginResponse = WebServiceProxy.ReferenceService.Login(WebServiceData.WebServiceManager.Name,
                                                                            WebServiceData.WebServiceManager.Password,
                                                                            ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                            false);
                    break;

                case WebServiceId.SwedishSpeciesObservationService:
                    loginResponse = WebServiceProxy.SwedishSpeciesObservationService.Login(WebServiceData.WebServiceManager.Name,
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

                case WebServiceId.TaxonAttributeService:
                    loginResponse = WebServiceProxy.TaxonAttributeService.Login(WebServiceData.WebServiceManager.Name,
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
            }

            return token;
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
                       webService;
            token = (String)context.GetCachedObject(cacheKey);

            if (token.IsNull())
            {
                // Log in to web service.
                switch (webService)
                {
                    case WebServiceId.AnalysisService:
                        loginResponse = WebServiceProxy.AnalysisService.Login(WebServiceData.WebServiceManager.Name,
                                                                              WebServiceData.WebServiceManager.Password,
                                                                              ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                              false);
                        break;

                    case WebServiceId.GeoReferenceService:
                        loginResponse = WebServiceProxy.GeoReferenceService.Login(WebServiceData.WebServiceManager.Name,
                                                                                  WebServiceData.WebServiceManager.Password,
                                                                                  ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                                  false);
                        break;

                    case WebServiceId.ReferenceService:
                        loginResponse = WebServiceProxy.ReferenceService.Login(WebServiceData.WebServiceManager.Name,
                                                                               WebServiceData.WebServiceManager.Password,
                                                                               ApplicationIdentifier.ArtDatabankenSOA.ToString(),
                                                                               false);
                        break;

                    case WebServiceId.SwedishSpeciesObservationService:
                        loginResponse = WebServiceProxy.SwedishSpeciesObservationService.Login(WebServiceData.WebServiceManager.Name,
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

                    case WebServiceId.TaxonAttributeService:
                        loginResponse = WebServiceProxy.TaxonAttributeService.Login(WebServiceData.WebServiceManager.Name,
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
    }
}
