using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.AnalysisService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages analysis related requests.
    /// </summary>
    public class AnalysisServiceProxy : WebServiceProxyBase, IWebService
    {
        /// <summary>
        /// Create a AnalysisServiceProxy instance.
        /// </summary>
        public AnalysisServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a AnalysisServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example Analysis.ArtDatabankenSOA.se/AnalysisService.svc.
        /// </param>
        public AnalysisServiceProxy(String webServiceAddress)
        {
            WebServiceAddress = webServiceAddress;
            switch (Configuration.InstallationType)
            {
                case InstallationType.ArtportalenTest:
                case InstallationType.ServerTest:
                    WebServiceComputer = WebServiceComputer.Moneses;
                    break;

                case InstallationType.LocalTest:
                    WebServiceComputer = WebServiceComputer.LocalTest;
                    break;

                case InstallationType.Production:
                    WebServiceComputer = WebServiceComputer.ArtDatabankenSoa;
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

        /// <summary>
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example Analysis.ArtDatabankenSOA.se/AnalysisService.svc.
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
                ((ClientBase<IAnalysisService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<IAnalysisService>)client).Abort();
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
            AnalysisServiceClient client;

            client = new AnalysisServiceClient(GetBinding(),GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetLog", client.Endpoint);
            IncreaseDataSize("GetHostsBySpeciesFactSearchCriteria", client.Endpoint);
            IncreaseDataSize("GetTaxaBySpeciesFactSearchCriteria", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void DeleteTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
              client.Client.DeleteTrace(clientInformation);
            }
        }

        /// <summary>
        /// Gets number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.
        /// </param>
        /// <returns>No of species observations that matches the search criteria.</returns>
        public Int64 GetSpeciesObservationCountBySearchCriteria(WebClientInformation clientInformation,
                                                                                   WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                   WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationCountBySpeciesObservationSearchCriteria(clientInformation, searchCriteria, coordinateSystem);
            }
        }

        /// <summary>
        /// Gets number of species 
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.
        /// </param>
        /// <returns>No of species  that matches the search criteria.</returns>
        public Int64 GetSpeciesCountBySearchCriteria(WebClientInformation clientInformation,
                                                     WebSpeciesObservationSearchCriteria searchCriteria,
                                                     WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesCountBySpeciesObservationSearchCriteria(clientInformation, searchCriteria, coordinateSystem);
            }
        }
        
        /// <summary>
        /// Gets no of species observations
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria, GridCellSize and GridCoordinateSystem are the
        /// only properties that is used in this method.</param>
        /// <param name="gridSpecification">Search properties for grid search.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Information about changed species observations.</returns>
        public List<WebGridCellSpeciesObservationCount> GetGridSpeciesObservationCounts(WebClientInformation clientInformation,
                                                                                            WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                            WebGridSpecification gridSpecification,
                                                                                            WebCoordinateSystem coordinateSystem)
          {
              using (ClientProxy client = new ClientProxy(this, 10))
              {
                  return client.Client.GetGridSpeciesObservationCountsBySpeciesObservationSearchCriteria(clientInformation, searchCriteria, gridSpecification, coordinateSystem); 
              }
          }

        /// <summary>
        /// Gets no of species observations
        /// that matches the search criteria and grid specifications.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria, GridCellSize and GridCoordinateSystem are the
        /// only properties that is used in this method.</param>
        /// <param name="gridSpecification">Search properties for grid search.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Information about changed species observations.</returns>
        public List<WebGridCellSpeciesCount> GetGridSpeciesCounts(WebClientInformation clientInformation,
                                                                  WebSpeciesObservationSearchCriteria searchCriteria,
                                                                  WebGridSpecification gridSpecification,
                                                                  WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 10))
            {
                return client.Client.GetGridSpeciesCountsBySpeciesObservationSearchCriteria(clientInformation,
                                                                                            searchCriteria,
                                                                                            gridSpecification,
                                                                                            coordinateSystem);
                
            }
        }

        /// <summary>
        /// Gets the grid cell feature statistics combined with species observation counts.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="gridSpecification">The grid specification: bounding box, grid cell size, etc.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="featureStatisticsSpecification">Information about requested information from a web feature service.</param>
        /// <param name="featuresUrl">Address to data in a web feature service.</param>
        /// <param name="featureCollectionJson">Feature collection json.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned grid.</param>
        /// <returns>A list with combined result from GetGridSpeciesCounts() and GetGridCellFeatureStatistics().</returns>
        public List<WebGridCellCombinedStatistics> GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
            WebClientInformation clientInformation,
            WebGridSpecification gridSpecification,
            WebSpeciesObservationSearchCriteria searchCriteria,
            WebFeatureStatisticsSpecification featureStatisticsSpecification,
            String featuresUrl,
            String featureCollectionJson,
            WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 20))
            {
                return client.Client.GetGridCellFeatureStatisticsCombinedWithSpeciesObservationCounts(
                    clientInformation, gridSpecification, searchCriteria, featureStatisticsSpecification, featuresUrl, featureCollectionJson, coordinateSystem);
            }
        }

        /// <summary>
        /// Get information about spatial features in a grid representation inside a user supplied bounding box.
        /// </summary>
        /// /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="featureStatisticsSpecification">Information about what statistics are requested from a web feature 
        /// service and wich spatial feature type that is to be measured</param>
        /// <param name="featuresUrl">Resource address.</param>
        /// <param name="featureCollectionJson">Feature collection as json string.</param>
        /// <param name="gridSpecification">Specifications of requested grid cell size, requested coordinate system 
        /// and user supplied bounding box.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>Statistical measurements on spatial features in grid format.</returns>
        public List<WebGridCellFeatureStatistics> GetGridCellFeatureStatistics(WebClientInformation clientInformation,
                                                                                WebFeatureStatisticsSpecification
                                                                                    featureStatisticsSpecification,
                                                                                String featuresUrl,
                                                                                String featureCollectionJson,
                                                                                //WfsTypeName typeName,
                                                                                WebGridSpecification gridSpecification,
                                                                                WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 20))
            {
                return client.Client.GetGridFeatureStatistics(clientInformation, featureStatisticsSpecification,
                                                              featuresUrl,
                                                              featureCollectionJson,
                                                              gridSpecification,
                                                              coordinateSystem);
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
        /// Get no of species that matches that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public List<WebTaxon> GetTaxaBySearchCriteria(WebClientInformation clientInformation,
                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                      WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTaxaBySpeciesObservationSearchCriteria(clientInformation,
                                                                               searchCriteria,
                                                                               coordinateSystem);
            }
        }

        /// <summary>
        /// Get unique taxa for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa for all species facts that matches search criteria.</returns>
        public List<WebTaxon> GetTaxaBySpeciesFactSearchCriteria(WebClientInformation clientInformation,
                                                                 WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTaxaBySpeciesFactSearchCriteria(clientInformation,
                                                                        searchCriteria);
            }
        }

        /// <summary>
        /// Get no of species, with related number of observed species, that matches that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>No of species that matches search criteria.</returns>
        public List<WebTaxonSpeciesObservationCount> GetTaxaWithSpeciesObservationCountsBySearchCriteria(WebClientInformation clientInformation,
                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                      WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTaxaWithSpeciesObservationCountsBySpeciesObservationSearchCriteria(clientInformation,
                                                                               searchCriteria,
                                                                               coordinateSystem);
            }
        }

        /// <summary>
        /// Get number of observation for each time step determinined by search criteria and time step type.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="periodicity">Time step type.</param>
        /// <param name="coordinateSystem">Coordinate system used for polygons in species observation search criteria.</param>
        /// <returns></returns>
        public List<WebTimeStepSpeciesObservationCount> GetTimeSpeciesObservationCounts(WebClientInformation clientInformation,
                                                          WebSpeciesObservationSearchCriteria searchCriteria,
                                                          Periodicity periodicity,
                                                          WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTimeSpeciesObservationCountsBySpeciesObservationSearchCriteria(clientInformation, searchCriteria, periodicity, coordinateSystem);
            }
        }

        /// <summary>
        /// Get EOO as geojson, EOO and AOO area as attributes
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="gridCells">Grid cells used to calculate result</param>
        /// <param name = "alphaValue" > If greater than 0 a concave hull will be calculated with this alpha value</param>
        /// <param name="useCenterPoint">Used when concave hull is calculated. Grid corner coordinates used when false</param>
        /// <returns>A JSON FeatureCollection with one feature showing EOO. EOO AND AOO Areas stored in feature attributes</returns>
        public string GetSpeciesObservationAOOEOOAsGeoJson(WebClientInformation clientInformation, List<WebGridCellSpeciesObservationCount> gridCells, int alphaValue = 0, bool useCenterPoint = true)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationAOOEOOAsGeoJson(clientInformation, gridCells, alphaValue, useCenterPoint);
            }
        }

        /// <summary>
        /// Get unique hosts for all species facts
        /// that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Hosts for all species facts that matches search criteria.</returns>
        public List<WebTaxon> GetHostsBySpeciesFactSearchCriteria(WebClientInformation clientInformation,
                                                                  WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetHostsBySpeciesFactSearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get species observation provenances 
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species observation search criteria - defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Number of species that matches the search criteria.</returns>
        public List<WebSpeciesObservationProvenance> GetProvenancesBySearchCriteria(WebClientInformation clientInformation,
                                                                                    WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                    WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetSpeciesObservationProvenancesBySearchCriteria(clientInformation, searchCriteria, coordinateSystem);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetLog(clientInformation, type, userName, rowCount);
            }
        }

        /// <summary>
        /// Get address of currently used web service.
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                if (Configuration.InstallationType != InstallationType.LocalTest)
                {
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.AnalysisService);
                }
                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.ArtDatabankenSoa:
                            WebServiceAddress = Settings.Default.AnalysisServiceArtDatabankenSoaAddress;
                            break;
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.AnalysisServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.AnalysisServiceMonesesAddress;
                            break;
                        default:
                            throw new Exception("Not handled computer in web service " + GetWebServiceName() + " " +
                                                WebServiceComputer);
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
            private readonly AnalysisServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(AnalysisServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                Client = (AnalysisServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public AnalysisServiceClient Client
            { get; private set; }

            /// <summary>
            /// Implementation of the IDisposable interface.
            /// Recycle the client instance.
            /// </summary>
            public void Dispose()
            {
                if ((Client.State != CommunicationState.Opened) ||
                    (!_webService.PushClient(Client, _operationTimeout)))
                {
                    // Client is not in state open or
                    // was not added to the client pool.
                    // Release resources.
                    Client.Close();
                }
                Client = null;
            }
        }
    }
}
