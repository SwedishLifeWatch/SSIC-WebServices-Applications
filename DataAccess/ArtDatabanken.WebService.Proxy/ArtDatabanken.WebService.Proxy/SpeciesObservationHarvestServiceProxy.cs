using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.SpeciesObservationHarvestService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages harvest of species observations.
    /// </summary>
    public class SpeciesObservationHarvestServiceProxy : WebServiceProxyBase, IWebService
    {
        /// <summary>
        /// Create a SpeciesObservationHarvestServiceProxy instance.
        /// </summary>
        public SpeciesObservationHarvestServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a SpeciesObservationHarvestServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example SwedishSpeciesObservation.ArtDatabankenSOA.se/SwedishSpeciesObservationService.svc.
        /// </param>
        public SpeciesObservationHarvestServiceProxy(String webServiceAddress)
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
        /// For example SpeciesObservationHarvest.ArtDatabankenSOA.se/SpeciesObservationHarvestService.svc.
        /// </summary>
        public String WebServiceAddress { get; set; }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">The clientInformation.</param>
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
                ((ClientBase<ISpeciesObservationHarvestService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<ISpeciesObservationHarvestService>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Let harvest job thread continue by current harvest setup.
        /// Used as synchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public void ContinueSpeciesObservationUpdate(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.ContinueSpeciesObservationUpdate(clientInformation);
            }
        }

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected override Object CreateClient()
        {
            SpeciesObservationHarvestServiceClient client;

            client = new SpeciesObservationHarvestServiceClient(GetBinding(),
                                                                GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetLog", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">The clientInformation.</param>
        public void DeleteTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteTrace(clientInformation);
            }
        }

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">The clientInformation.</param>
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
        /// Get progress status about the current observation update process.
        /// Used as synchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Status object.</returns>
        public WebSpeciesObservationHarvestStatus GetSpeciesObservationUpdateStatus(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetSpeciesObservationUpdateStatus(clientInformation);
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
                switch (WebServiceComputer)
                {
                    case WebServiceComputer.LocalTest:
                        WebServiceAddress = Settings.Default.SpeciesObservationHarvestServiceLocalAddress;
                        break;
                    case WebServiceComputer.Moneses:
                        WebServiceAddress = Settings.Default.SpeciesObservationHarvestServiceMonesesAddress;
                        break;
                    case WebServiceComputer.Silurus2:
                        WebServiceAddress = Settings.Default.SpeciesObservationHarvestServiceSilurus2Address;
                        break;
                    default:
                        throw new Exception("Not handled computer in web service " + GetWebServiceName() + " " +
                                            WebServiceComputer);
                }
            }

            return WebServiceAddress;
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">The User name.</param>
        /// <param name="password">The Password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succeed.
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
        /// <param name="clientInformation">The clientInformation.</param>
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
        /// Let harvest job thread stop and save current harvest setup.
        /// Used as asynchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public void PauseSpeciesObservationUpdate(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.PauseSpeciesObservationUpdate(clientInformation);
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
        /// Update database with changes in species observation.
        /// Loop over each connector and read all changes. 
        /// Add data to create, update or delete in data tables.
        /// Write Data tables to temp tables in database.
        /// Update ID, TaxonId and convert Points to wgs85 if needed.
        /// Call DB procedures to Copy/Update/Delete from temp tables to production tables.
        /// Used as asynchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <param name="dataProviderIds">Update species observations for these data providers.</param>
        /// <param name="isChangedDatesSpecified">Notification if start and end dates are specified or not.</param>
        public void StartSpeciesObservationUpdate(
            WebClientInformation clientInformation,
            DateTime changedFrom,
            DateTime changedTo,
            List<int> dataProviderIds,
            bool isChangedDatesSpecified)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartSpeciesObservationUpdate(clientInformation, changedFrom, changedTo, dataProviderIds, isChangedDatesSpecified);
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negative impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">The clientInformation.</param>
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
        /// Let harvest job thread stop.
        /// Used as asynchronous service operation.
        /// This method can only be used by web service administrators.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        public void StopSpeciesObservationUpdate(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StopSpeciesObservationUpdate(clientInformation);
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">The clientInformation.</param>
        public void StopTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StopTrace(clientInformation);
            }
        }

        /// <summary>
        /// Update database with changes in species observation.
        /// Loop over each connector and read all changes 
        /// Add data to create, update or delete data tables
        /// Write Data tables to Temp tables in database
        /// Update ID, TaxonId and convert Points to wgs85 if needed
        /// Call DB procedures to Copy/Update/Delete from temp tables to production tables.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <param name="dataProviderIds">Update species observations for these data providers.</param>
        public void UpdateSpeciesObservations(WebClientInformation clientInformation,
                                              DateTime changedFrom,
                                              DateTime changedTo,
                                              List<Int32> dataProviderIds)
        {
            using (ClientProxy client = new ClientProxy(this, 60))
            {
                client.Client.UpdateSpeciesObservations(clientInformation, changedFrom, changedTo, dataProviderIds);
            }
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            /// <summary>
            /// The _operation timeout.
            /// </summary>
            private readonly Int32 _operationTimeout;

            /// <summary>
            /// The _web service.
            /// </summary>
            private readonly SpeciesObservationHarvestServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(SpeciesObservationHarvestServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                Client = (SpeciesObservationHarvestServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public SpeciesObservationHarvestServiceClient Client { get; private set; }

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
