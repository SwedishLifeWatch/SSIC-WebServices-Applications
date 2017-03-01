using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.TaxonAttributeService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages taxon attribute service requests.
    /// </summary>
    public class TaxonAttributeServiceProxy : WebServiceProxyBase, ITransactionProxy, IWebService
    {
        /// <summary>
        /// Create a TaxonAttributeServiceProxy instance.
        /// </summary>
        public TaxonAttributeServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a TaxonAttributeServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example TaxonAttribute.ArtDatabankenSOA.se/TaxonAttributeService.svc.
        /// </param>
        public TaxonAttributeServiceProxy(String webServiceAddress)
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
        /// For example TaxonAttribute.ArtDatabankenSOA.se/TaxonAttributeService.svc.
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
                ((ClientBase<ITaxonAttributeService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<ITaxonAttributeService>)client).Abort();
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
            TaxonAttributeServiceClient client;

            client = new TaxonAttributeServiceClient(GetBinding(),
                                                     GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("CreateSpeciesFacts", client.Endpoint);
            IncreaseDataSize("DeleteSpeciesFacts", client.Endpoint);
            IncreaseDataSize("GetLog", client.Endpoint);
            IncreaseDataSize("GetSpeciesFactsByIdentifiers", client.Endpoint);
            IncreaseDataSize("GetSpeciesFactsByIds", client.Endpoint);
            IncreaseDataSize("GetSpeciesFactsBySearchCriteria", client.Endpoint);
            IncreaseDataSize("UpdateSpeciesFacts", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Create species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        public void CreateSpeciesFacts(WebClientInformation clientInformation,
                                       List<WebSpeciesFact> createSpeciesFacts)
        {
            using (ClientProxy client = new ClientProxy(this, 10))
            {
                client.Client.CreateSpeciesFacts(clientInformation, createSpeciesFacts);
            }
        }

        /// <summary>
        /// Delete species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        public void DeleteSpeciesFacts(WebClientInformation clientInformation,
                                       List<WebSpeciesFact> deleteSpeciesFacts)
        {
            using (ClientProxy client = new ClientProxy(this, 10))
            {
                client.Client.DeleteSpeciesFacts(clientInformation, deleteSpeciesFacts);
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
        /// Get all factor data types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor data types.</returns>
        public List<WebFactorDataType> GetFactorDataTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetFactorDataTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get information about all factor field enumerations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor field enumerations.</returns>
        public List<WebFactorFieldEnum> GetFactorFieldEnums(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetFactorFieldEnums(clientInformation);
            }
        }

        /// <summary>
        /// Get all factor field types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor field types.</returns>
        public List<WebFactorFieldType> GetFactorFieldTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetFactorFieldTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All factor origins.</returns>
        public List<WebFactorOrigin> GetFactorOrigins(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetFactorOrigins(clientInformation);
            }
        }

        /// <summary>
        /// Get all factors.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All factors.</returns>
        public List<WebFactor> GetFactors(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetFactors(clientInformation);
            }
        }

        /// <summary>
        /// Get factors that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Filtered factors.</returns>
        public List<WebFactor> GetFactorsBySearchCriteria(WebClientInformation clientInformation, WebFactorSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetFactorsBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get all factor trees.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor trees.</returns>
        public List<WebFactorTreeNode> GetFactorTrees(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetFactorTrees(clientInformation);
            }
        }

        /// <summary>
        /// Get all factor trees that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Factor trees.</returns>
        public List<WebFactorTreeNode> GetFactorTreesBySearchCriteria(WebClientInformation clientInformation, WebFactorTreeSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetFactorTreesBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get all factor update modes.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor update modes.</returns>
        public List<WebFactorUpdateMode> GetFactorUpdateModes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetFactorUpdateModes(clientInformation);
            }
        }

        /// <summary>
        /// Get all individual categories.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All individual categories.</returns>
        public List<WebIndividualCategory> GetIndividualCategories(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetIndividualCategories(clientInformation);
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
        /// Get all periods.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All periods.</returns>
        public List<WebPeriod> GetPeriods(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPeriods(clientInformation);
            }
        }

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Period types.</returns>
        public List<WebPeriodType> GetPeriodTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetPeriodTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get all species fact qualities.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All species fact qualities.</returns>
        public List<WebSpeciesFactQuality> GetSpeciesFactQualities(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetSpeciesFactQualities(clientInformation);
            }
        }

        /// <summary>
        /// Get species facts with specified identifiers.
        /// Only existing species facts are returned,
        /// e.g. species fact identifiers that does not
        /// match existing species fact does not affect
        /// the returned species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesFactIdentifiers">
        /// Species facts identifiers. E.g. WebSpeciesFacts
        /// instances where id for requested combination of
        /// factor, host, individual category, period and taxon
        /// has been set.
        /// Host id is only used together with taxonomic factors.
        /// Period id is only used together with periodic factors.
        /// </param>
        /// <returns>
        /// Existing species facts among the
        /// requested species facts.
        /// </returns>
        public List<WebSpeciesFact> GetSpeciesFactsByIdentifiers(WebClientInformation clientInformation,
                                                                 List<WebSpeciesFact> speciesFactIdentifiers)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetSpeciesFactsByIdentifiers(clientInformation, speciesFactIdentifiers);
            }
        }

        /// <summary>
        /// Get information about all species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species fact information.</returns>
        public List<WebSpeciesFact> GetSpeciesFactsByIds(WebClientInformation clientInformation, List<int> speciesFactIds)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetSpeciesFactsByIds(clientInformation, speciesFactIds);
            }
        }

        /// <summary>
        /// Get information about species facts that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        public List<WebSpeciesFact> GetSpeciesFactsBySearchCriteria(WebClientInformation clientInformation,
                                                                    WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetSpeciesFactsBySearchCriteria(clientInformation, searchCriteria);
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
        /// Get taxa count of taxa that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa count of taxa that matches search criteria.</returns>
        public Int32 GetTaxaCountBySearchCriteria(WebClientInformation clientInformation,
                                                                    WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetTaxaCountBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get information about taxa that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>List of Taxa ids that matches search criteria.</returns>
        public List<WebTaxon> GetTaxaBySearchCriteria(WebClientInformation clientInformation,
                                                                    WebSpeciesFactSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetTaxaBySearchCriteria(clientInformation, searchCriteria);
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
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.TaxonAttributeService);
                }

                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.ArtDatabankenSoa:
                            WebServiceAddress = Settings.Default.TaxonAttributeServiceArtDatabankenSoaAddress;
                            break;
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.TaxonAttributeServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.TaxonAttributeServiceMonesesAddress;
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
        /// Update species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        public void UpdateSpeciesFacts(WebClientInformation clientInformation,
                                       List<WebSpeciesFact> updateSpeciesFacts)
        {
            using (ClientProxy client = new ClientProxy(this, 10))
            {
                client.Client.UpdateSpeciesFacts(clientInformation,
                                                 updateSpeciesFacts);
            }
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private TaxonAttributeServiceClient _client;
            private readonly TaxonAttributeServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(TaxonAttributeServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (TaxonAttributeServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public TaxonAttributeServiceClient Client
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
