using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.GeoReferenceService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages geo reference service requests.
    /// </summary>
    public class GeoReferenceServiceProxy : WebServiceProxyBase, IWebService
    {
        /// <summary>
        /// Create a GeoReferenceServiceProxy instance.
        /// </summary>
        public GeoReferenceServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a GeoReferenceServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example GeoReference.ArtDatabankenSOA.se/GeoReferenceService.svc
        /// </param>
        public GeoReferenceServiceProxy(String webServiceAddress)
        {
            WebServiceAddress = webServiceAddress;
            if (Configuration.CountryId == CountryId.Norway)
            {
                WebServiceComputer = WebServiceComputer.Silurus2;
            }
            else
            {
                switch (Configuration.InstallationType)
                {
                    case InstallationType.ArtportalenTest:
                        WebServiceComputer = WebServiceComputer.ArtportalenTest;
                        break;

                    case InstallationType.ServerTest:
                        WebServiceComputer = WebServiceComputer.Moneses;
                        break;

                    case InstallationType.LocalTest:
                        WebServiceComputer = WebServiceComputer.LocalTest;
                        break;

                    case InstallationType.Production:
#if OLD_WEB_SERVICE_ADDRESS
                        WebServiceComputer = WebServiceComputer.Silurus2;
#else
                        WebServiceComputer = WebServiceComputer.ArtDatabankenSoa;
#endif
                        break;

                    case InstallationType.SpeciesFactTest:
                        WebServiceComputer = WebServiceComputer.SpeciesFactTest;
                        break;

                    case InstallationType.SystemTest:
                        WebServiceComputer = WebServiceComputer.SystemTest;
                        break;

                    case InstallationType.TwoBlueberriesTest:
                        WebServiceComputer = WebServiceComputer.TwoBlueberriesTest;
                        break;

                    default:
                        throw new ApplicationException("Not handled installation type " + Configuration.InstallationType);
                }
            }
        }

        /// <summary>
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example GeoReference.ArtDatabankenSOA.se/GeoReferenceService.svc.
        /// </summary>
        public String WebServiceAddress
        { get; set; }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void ClearCache(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.ClearCache(clientInformation);
            }
        }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<IGeoReferenceService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<IGeoReferenceService>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected override Object CreateClient()
        {
            GeoReferenceServiceClient client;

            client = new GeoReferenceServiceClient(GetBinding(),
                                                   GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetRegionsByCategories", client.Endpoint);
            IncreaseDataSize("GetRegionsByGUIDs", client.Endpoint);
            IncreaseDataSize("GetRegionsByIds", client.Endpoint);
            IncreaseDataSize("GetRegionsBySearchCriteria", client.Endpoint);
            IncreaseDataSize("GetRegionsGeographyByGUIDs", client.Endpoint);
            IncreaseDataSize("GetRegionsGeographyByIds", client.Endpoint);
            IncreaseDataSize("GetLog", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void DeleteTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteTrace(clientInformation);
            }
        }

        /// <summary>
        /// Get cities that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">City search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Regions that matches the search criterias.</returns>       
        public List<WebCityInformation> GetCitiesByNameSearchString(WebClientInformation clientInformation,
                                                          WebStringSearchCriteria searchCriteria,
                                                          WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetCitiesByNameSearchString(clientInformation, searchCriteria, coordinateSystem);
            }
        }

        /// <summary>
        /// Geographic coordinate conversion of points.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="points">Points that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Converted points.</returns>       
        public List<WebPoint> GetConvertedPoints(WebClientInformation clientInformation,
                                                 List<WebPoint> points,
                                                 WebCoordinateSystem fromCoordinateSystem,
                                                 WebCoordinateSystem toCoordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetConvertedPoints(clientInformation, points, fromCoordinateSystem, toCoordinateSystem);
            }
        }

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        public List<WebLogRow> GetLog(WebClientInformation clientInformation,
                                      LogType type,
                                      String userName,
                                      Int32 rowCount)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetLog(clientInformation, type, userName, rowCount);
            }
        }

        /// <summary>
        /// Get region categories.
        /// All region categories are returned if parameter countryIsoCode is not specified.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="isCountryIsoCodeSpecified">Indicates if parameter countryIsoCode is specified.</param>
        /// <param name="countryIsoCode">
        /// Get region categories related to this country.
        /// Country iso codes are specified in standard ISO-3166.
        /// </param>
        /// <returns>Region categories.</returns>       
        public List<WebRegionCategory> GetRegionCategories(WebClientInformation clientInformation,
                                                           Boolean isCountryIsoCodeSpecified,
                                                           Int32 countryIsoCode)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRegionCategories(clientInformation, isCountryIsoCodeSpecified, countryIsoCode);
            }
        }

        /// <summary>
        /// Get regions related to specified region categories.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionCategories">Get regions related to specified region categories.</param>
        /// <returns>Regions related to specified region categories.</returns>       
        public List<WebRegion> GetRegionsByCategories(WebClientInformation clientInformation,
                                                      List<WebRegionCategory> regionCategories)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRegionsByCategories(clientInformation, regionCategories);
            }
        }

        /// <summary>
        /// Get regions by GUIDs.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <returns>Regions matching provided GUIDs.</returns>       
        public List<WebRegion> GetRegionsByGUIDs(WebClientInformation clientInformation,
                                                 List<String> GUIDs)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRegionsByGUIDs(clientInformation, GUIDs);
            }
        }

        /// <summary>
        /// Gets the regions by list of regionId values.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionIds">List of region id values.</param>
        /// <returns>Return a list of regions matching the list of id values.</returns>
        public List<WebRegion> GetRegionsByIds(WebClientInformation clientInformation,
                                               List<Int32> regionIds)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRegionsByIds(clientInformation, regionIds);
            }
        }

        /// <summary>
        /// Get regions that matches the search criterias.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Region search criterias.</param>
        /// <returns>Regions that matches the search criterias.</returns>       
        public List<WebRegion> GetRegionsBySearchCriteria(WebClientInformation clientInformation,
                                                          WebRegionSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRegionsBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="GUIDs">Region GUIDs.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        public List<WebRegionGeography> GetRegionsGeographyByGUIDs(WebClientInformation clientInformation,
                                                                   List<String> GUIDs,
                                                                   WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetRegionsGeographyByGUIDs(clientInformation, GUIDs, coordinateSystem);
            }
        }

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        public List<WebRegionGeography> GetRegionsGeographyByIds(WebClientInformation clientInformation,
                                                                 List<Int32> regionIds,
                                                                 WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetRegionsGeographyByIds(clientInformation, regionIds, coordinateSystem);
            }
        }

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All region types.</returns>       
        public List<WebRegionType> GetRegionTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetRegionTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        public List<WebResourceStatus> GetStatus(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetStatus(clientInformation);
            }
        }

        /// <summary>
        /// Get address of currently used web service.
        /// This address does not contain information about
        /// protocol or end point.
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                if (Configuration.InstallationType != InstallationType.LocalTest)
                {
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.GeoReferenceService);
                }

                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.ArtDatabankenSoa:
                            WebServiceAddress = Settings.Default.GeoReferenceServiceArtDatabankenSoaAddress;
                            break;
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.GeoReferenceServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.GeoReferenceServiceMonesesAddress;
                            break;
                        case WebServiceComputer.Silurus2:
                            WebServiceAddress = Settings.Default.GeoReferenceServiceSilurus2Address;
                            break;
                        default:
                            throw new Exception("Not handled computer in web service " + GetWebServiceName() + " " + WebServiceComputer);
                    }
                }
            }

            return WebServiceAddress;
        }

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
        public WebLoginResponse Login(String userName,
                                      String password,
                                      String applicationIdentifier,
                                      Boolean isActivationRequired)
        {
            WebServiceProxy.UserService.LoadSoaWebServiceAddresses(userName,
                                                                   password,
                                                                   applicationIdentifier,
                                                                   isActivationRequired);

            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.Login(userName, password, applicationIdentifier, isActivationRequired);
            }
        }

        /// <summary>
        /// Logout user from web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void Logout(WebClientInformation clientInformation)
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 1))
                {
                    client.Client.Logout(clientInformation);
                }
            }
            catch
            {
                // No need to handle errors.
                // Logout is only used to release
                // resources in the web service.
            }
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public Boolean Ping()
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 0, 10))
                {
                    return client.Client.Ping();
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="userName">User name.</param>
        public void StartTrace(WebClientInformation clientInformation,
                               String userName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTrace(clientInformation, userName);
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void StopTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StopTrace(clientInformation);
            }
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private GeoReferenceServiceClient _client;
            private readonly GeoReferenceServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(GeoReferenceServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (GeoReferenceServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public GeoReferenceServiceClient Client
            {
                get { return _client; }
            }

            /// <summary>
            /// Implementation of the IDisposable interface.
            /// Recycle the client instance.
            /// </summary>
            public void Dispose()
            {
                if ((_client.State != CommunicationState.Opened) ||
                    (!_webService.PushClient(_client, _operationTimeout)))
                {
                    // Client is not in state open or
                    // was not added to the client pool.
                    // Release resources.
                    _client.Close();
                }
                _client = null;
            }
        }
    }
}
