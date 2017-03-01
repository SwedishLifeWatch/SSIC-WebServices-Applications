using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.DyntaxaInternalService;
using ArtDatabanken.WebService.Proxy.TaxonService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages DyntaxaInternal service requests.
    /// </summary>
    public class DyntaxaInternalServiceProxy : WebServiceProxyBase, ITransactionProxy, IWebService
    {
        /// <summary>
        /// Create a DyntaxaInternalServiceProxy instance.
        /// </summary>
        public DyntaxaInternalServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a DyntaxaInternalServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example Taxon.ArtDatabankenSOA.se/DyntaxaInternalService.svc.
        /// </param>
        public DyntaxaInternalServiceProxy(String webServiceAddress)
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
        /// For example Taxon.ArtDatabankenSOA.se/DyntaxaInternalService.svc.
        /// </summary>
        public String WebServiceAddress
        { get; set; }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void ClearCache(WebClientInformation clientInformation)
        {
            throw new NotImplementedException("Clear cache is not implemented in DyntaxaInternalService");
        }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<IDyntaxaInternalService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<IDyntaxaInternalService>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Gets the dyntaxa revision species fact.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="factorId">The factor identifier.</param>
        /// <param name="taxonId">The taxon identifier.</param>
        /// <param name="taxonRevisionId">The taxon revision identifier.</param>
        /// <returns></returns>
        public WebDyntaxaRevisionSpeciesFact GetDyntaxaRevisionSpeciesFact(WebClientInformation clientInformation, Int32 factorId, Int32 taxonId, Int32 taxonRevisionId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetDyntaxaRevisionSpeciesFact(clientInformation, factorId, taxonId, taxonRevisionId);
            }
        }

        /// <summary>
        ///  Gets all dyntaxa revision species facts.
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <param name="taxonRevisionId"></param>
        /// <returns></returns>
        public List<WebDyntaxaRevisionSpeciesFact> GetAllDyntaxaRevisionSpeciesFacts(WebClientInformation clientInformation, Int32 taxonRevisionId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetAllDyntaxaRevisionSpeciesFacts(clientInformation, taxonRevisionId);
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
        /// Creates the dyntaxa revision species fact.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="dyntaxaRevisionSpeciesFact">The dyntaxa revision species fact.</param>
        /// <returns></returns>
        public WebDyntaxaRevisionSpeciesFact CreateDyntaxaRevisionSpeciesFact(WebClientInformation clientInformation, WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateDyntaxaRevisionSpeciesFact(clientInformation, dyntaxaRevisionSpeciesFact);                
            }
        }

        /// <summary>
        /// Creates the complete revision event.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="revisionEvent">The revision event.</param>
        /// <returns></returns>
        public WebTaxonRevisionEvent CreateCompleteRevisionEvent(WebClientInformation clientInformation, WebTaxonRevisionEvent revisionEvent)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateCompleteRevisionEvent(clientInformation, revisionEvent);
            }
        }

        /// <summary>
        /// Commit an transaction
        /// </summary>
        /// <param name="clientInformation"></param>
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
            DyntaxaInternalServiceClient client;

            client = new DyntaxaInternalServiceClient(
                GetBinding(),
                GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data. 
            //IncreaseDataSize("GetLog", client.Endpoint);
            //IncreaseDataSize("GetTaxaByIds", client.Endpoint);
            //IncreaseDataSize("GetTaxaBySearchCriteria", client.Endpoint);
            //IncreaseDataSize("GetTaxonNamesBySearchCriteria", client.Endpoint);
            //IncreaseDataSize("GetTaxonNamesByTaxonIds", client.Endpoint);
            //IncreaseDataSize("GetTaxonRelationsBySearchCriteria", client.Endpoint);
            //IncreaseDataSize("GetTaxonRevisionEventsByTaxonRevisionId", client.Endpoint);
            //IncreaseDataSize("GetTaxonTreesBySearchCriteria", client.Endpoint);

            return client;
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
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.TaxonService);
                    WebServiceAddress = WebServiceAddress.Replace(
                        Settings.Default.TaxonServiceImplementation,
                        Settings.Default.DyntaxaInternalServiceImplementation);
                }

                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.ArtDatabankenSoa:
                            WebServiceAddress = Settings.Default.DyntaxaInternalServiceArtDatabankenSoaAddress;
                            break;
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.DyntaxaInternalServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.DyntaxaInternalServiceMonesesAddress;
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
        public WebLoginResponse Login(
            String userName,
            String password,
            String applicationIdentifier,
            Boolean isActivationRequired)
        {
            WebServiceProxy.UserService.LoadSoaWebServiceAddresses(
                userName,
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
        /// Rollback a transaction
        /// </summary>
        /// <param name="clientInformation"></param>
        public void RollbackTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.RollbackTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public void StartTransaction(
            WebClientInformation clientInformation,
            Int32 timeout)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTransaction(clientInformation, timeout);
            }
        }

        /// <summary>
        /// Set revision species fact published flag to true
        /// </summary>revisionId
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="revisionId"></param>
        public bool SetRevisionSpeciesFactPublished(WebClientInformation clientInformation, int revisionId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.SetRevisionSpeciesFactPublished(clientInformation, revisionId);
            }
        }

        /// <summary>
        /// Get dyntaxa revision reference relation item(s).
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="relatedObjectGUID">The related object unique identifier.</param>
        /// <returns>
        /// A List of WebDyntaxaRevisionReferenceRelation if any revision steps have been
        /// made for specified (revisionId,relatedObjectGUID); otherwise null.
        /// </returns>
        public List<WebDyntaxaRevisionReferenceRelation> GetDyntaxaRevisionReferenceRelation(
            WebClientInformation clientInformation, 
            int revisionId, 
            string relatedObjectGUID)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetDyntaxaRevisionReferenceRelation(clientInformation, revisionId, relatedObjectGUID);
            }
        }

        /// <summary>
        /// Get all Dyntaxa Revision Reference relation items.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="revisionId">Revision id.</param>
        /// <returns></returns>
        public List<WebDyntaxaRevisionReferenceRelation> GetAllDyntaxaRevisionReferenceRelations(
            WebClientInformation clientInformation, 
            int revisionId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetAllDyntaxaRevisionReferenceRelations(clientInformation, revisionId);
            }
        }

        /// <summary>
        /// Gets the dyntaxa revision reference relation by identifier.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public WebDyntaxaRevisionReferenceRelation GetDyntaxaRevisionReferenceRelationById(
            WebClientInformation clientInformation, 
            int id)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetDyntaxaRevisionReferenceRelationById(clientInformation, id);
            }            
        }

        /// <summary>
        /// Set revision reference relation published flag to true
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        public bool SetRevisionReferenceRelationPublished(
            WebClientInformation clientInformation, 
            int revisionId)
        {            
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.SetRevisionReferenceRelationPublished(clientInformation, revisionId);
            }
        }

        /// <summary>
        /// Creates a dyntaxa revision reference relation..
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="dyntaxaRevisionReferenceRelation">The dyntaxa revision reference relation.</param>
        /// <returns></returns>
        public WebDyntaxaRevisionReferenceRelation CreateDyntaxaRevisionReferenceRelation(
            WebClientInformation clientInformation, 
            WebDyntaxaRevisionReferenceRelation dyntaxaRevisionReferenceRelation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateDyntaxaRevisionReferenceRelation(clientInformation, dyntaxaRevisionReferenceRelation);
            }            
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private DyntaxaInternalServiceClient _client;
            private readonly DyntaxaInternalServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(
                DyntaxaInternalServiceProxy webService,
                Int32 operationTimeoutMinutes,
                Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (DyntaxaInternalServiceClient)_webService.PopClient(_operationTimeout);
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public DyntaxaInternalServiceClient Client
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