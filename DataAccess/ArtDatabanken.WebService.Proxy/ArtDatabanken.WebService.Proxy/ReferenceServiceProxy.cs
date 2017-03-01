using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.ReferenceService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages reference service requests.
    /// </summary>
    public class ReferenceServiceProxy : WebServiceProxyBase, ITransactionProxy, IWebService
    {
        /// <summary>
        /// Create a ReferenceServiceProxy instance.
        /// </summary>
        public ReferenceServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a ReferenceServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example Reference.ArtDatabankenSOA.se/ReferenceService.svc.
        /// </param>
        public ReferenceServiceProxy(String webServiceAddress)
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
        /// For example Reference.ArtDatabankenSOA.se/ReferenceService.svc.
        /// </summary>
        public String WebServiceAddress { get; set; }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
                ((ClientBase<IReferenceService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<IReferenceService>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void CommitTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.CommitTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected override Object CreateClient()
        {
            ReferenceServiceClient client;

            client = new ReferenceServiceClient(GetBinding(),
                                                GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetLog", client.Endpoint);
            IncreaseDataSize("GetReferences", client.Endpoint);
            IncreaseDataSize("GetReferencesByIds", client.Endpoint);
            IncreaseDataSize("GetReferencesBySearchCriteria", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="reference">New reference to create.</param>
        public void CreateReference(WebClientInformation clientInformation,
                                    WebReference reference)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.CreateReference(clientInformation, reference);
            }
        }

        /// <summary>
        /// Create a reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelation">Information about the new reference relation.</param>
        /// <returns>The new reference relation.</returns>
        public WebReferenceRelation CreateReferenceRelation(WebClientInformation clientInformation,
                                                            WebReferenceRelation referenceRelation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateReferenceRelation(clientInformation, referenceRelation);
            }
        }

        /// <summary>
        /// Delete specified reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelationId">The reference relation id.</param>
        public void DeleteReferenceRelation(WebClientInformation clientInformation,
                                            Int32 referenceRelationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteReferenceRelation(clientInformation, referenceRelationId);
            }
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
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
        /// Get information about a reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelationId">Id for reference relation.</param>
        /// <returns>A WebReferenceRelation object.</returns>
        public WebReferenceRelation GetReferenceRelationById(WebClientInformation clientInformation,
                                                             Int32 referenceRelationId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetReferenceRelationById(clientInformation, referenceRelationId);
            }
        }

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        public List<WebReferenceRelation> GetReferenceRelationsByRelatedObjectGuid(WebClientInformation clientInformation,
                                                                                   String relatedObjectGuid)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetReferenceRelationsByRelatedObjectGuid(clientInformation, relatedObjectGuid);
            }
        }

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All reference relation types.</returns>
        public List<WebReferenceRelationType> GetReferenceRelationTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetReferenceRelationTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All references.</returns>
        public List<WebReference> GetReferences(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetReferences(clientInformation);
            }
        }

        /// <summary>
        /// Get specified references.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns>Specified references.</returns>
        public List<WebReference> GetReferencesByIds(WebClientInformation clientInformation,
                                                     List<Int32> referenceIds)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetReferencesByIds(clientInformation, referenceIds);
            }
        }

        /// <summary>
        /// Get references that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Reference search criteria.</param>
        /// <returns>References that matches search criteria.</returns>
        public List<WebReference> GetReferencesBySearchCriteria(WebClientInformation clientInformation,
                                                                WebReferenceSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetReferencesBySearchCriteria(clientInformation, searchCriteria);
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
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.ReferenceService);
                }

                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.ArtDatabankenSoa:
                            WebServiceAddress = Settings.Default.ReferenceServiceArtDatabankenSoaAddress;
                            break;
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.ReferenceServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.ReferenceServiceMonesesAddress;
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
        /// <param name="password">The password.</param>
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
            WebLoginResponse loginResponse;

            WebServiceProxy.UserService.LoadSoaWebServiceAddresses(userName,
                                                                   password,
                                                                   applicationIdentifier,
                                                                   isActivationRequired);
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                loginResponse = client.Client.Login(userName, password, applicationIdentifier, isActivationRequired);
            }

            return loginResponse;
        }

        /// <summary>
        /// Logout user from web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void Logout(WebClientInformation clientInformation)
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 1))
                {
                    client.Client.Logout(clientInformation);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
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
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void RollbackTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.RollbackTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negative impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
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
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public void StartTransaction(WebClientInformation clientInformation,
                                     Int32 timeout)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTransaction(clientInformation,
                                               timeout);
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        public void StopTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StopTrace(clientInformation);
            }
        }

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="reference">Existing reference to update.</param>
        public void UpdateReference(WebClientInformation clientInformation,
                                    WebReference reference)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.UpdateReference(clientInformation, reference);
            }
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private ReferenceServiceClient _client;
            private readonly ReferenceServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(ReferenceServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (ReferenceServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public ReferenceServiceClient Client
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
