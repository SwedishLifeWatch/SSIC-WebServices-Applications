using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.SwedishSpeciesObservationSOAPService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages species observation related requests.
    /// </summary>
    public class SwedishSpeciesObservationSOAPServiceProxy : WebServiceProxyBase, IWebService
    {
        /// <summary>
        /// Create a SwedishSpeciesObservationSOAPServiceProxy instance.
        /// </summary>
        public SwedishSpeciesObservationSOAPServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a SwedishSpeciesObservationSOAPServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example SwedishSpeciesObservation.ArtDatabankenSOA.se/SwedishSpeciesObservationService.svc.
        /// </param>
        public SwedishSpeciesObservationSOAPServiceProxy(String webServiceAddress)
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
                    WebServiceComputer = WebServiceComputer.Silurus2;
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
        /// For example SwedishSpeciesObservation.ArtDatabankenSOA.se/SwedishSpeciesObservationService.svc.
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
                ((ClientBase<ISwedishSpeciesObservationSOAPService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<ISwedishSpeciesObservationSOAPService>)client).Abort();
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
            SwedishSpeciesObservationSOAPServiceClient client;

            client = new SwedishSpeciesObservationSOAPServiceClient(GetBinding(),
                                                                    GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetDarwinCoreByIds", client.Endpoint);
            IncreaseDataSize("GetDarwinCoreBySearchCriteria", client.Endpoint);
            IncreaseDataSize("GetDarwinCoreChange", client.Endpoint);
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
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Information about bird nest activities.</returns>
        public List<WebBirdNestActivity> GetBirdNestActivities(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetBirdNestActivities(clientInformation);
            }
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 25000 observations can be retrieved in one call.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <returns>Species observations.</returns>
        public WebDarwinCoreInformation GetDarwinCoreByIds(WebClientInformation clientInformation,
                                                           List<Int64> speciesObservationIds,
                                                           WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetDarwinCoreByIds(clientInformation, speciesObservationIds, coordinateSystem);
            }
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criterias
        /// and returned species observations.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public WebDarwinCoreInformation GetDarwinCoreBySearchCriteria(WebClientInformation clientInformation,
                                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                                      WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetDarwinCoreBySearchCriteria(clientInformation, searchCriteria, coordinateSystem);
            }
        }

        /// <summary>
        /// Get information about species observations
        /// that has changed in the specified date range.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// Max 25000 observations of each change type (new or updated)
        /// with information can be retrieved in one call.
        /// Max 1000000 observations of each change type (deleted, new
        /// or updated), with GUIDs or ids, can be retrieved in one call.
        /// Parameters changedFrom and changedTo may be the same date.
        /// Parameter changedTo must not be today or in the future.
        /// Only date part of parameters changedFrom and changedTo
        /// are used. It does not matter what time of day that is set
        /// in parameters changedFrom and changedTo.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>Information about changed species observations.</returns>
        public WebDarwinCoreChange GetDarwinCoreChange(WebClientInformation clientInformation,
                                                       DateTime changedFrom,
                                                       DateTime changedTo,
                                                       WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 20))
            {
                return client.Client.GetDarwinCoreChange(clientInformation, changedFrom, changedTo, coordinateSystem);
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
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                if (Configuration.InstallationType != InstallationType.LocalTest)
                {
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.SwedishSpeciesObservationSOAPService);
                }
                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.SwedishSpeciesObservationSOAPServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.SwedishSpeciesObservationSOAPServiceMonesesAddress;
                            break;
                        case WebServiceComputer.Silurus2:
                            WebServiceAddress = Settings.Default.SwedishSpeciesObservationSOAPServiceSilurus2Address;
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
        /// Test if specified geometries contains any conservation relevant
        /// species observations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">
        /// Species observation search criteria including at least one
        /// polygon, bounding box or region.
        /// </param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>True if specified geometries contains conservation relevant species observations.</returns>
        public Boolean HasConservationSpeciesObservation(WebClientInformation clientInformation,
                                                         WebSpeciesObservationSearchCriteria searchCriteria,
                                                         WebCoordinateSystem coordinateSystem)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.HasConservationSpeciesObservation(clientInformation, searchCriteria, coordinateSystem);
            }
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
                // Logout is only used to relase
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
            private readonly SwedishSpeciesObservationSOAPServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(SwedishSpeciesObservationSOAPServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                Client = (SwedishSpeciesObservationSOAPServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public SwedishSpeciesObservationSOAPServiceClient Client
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
