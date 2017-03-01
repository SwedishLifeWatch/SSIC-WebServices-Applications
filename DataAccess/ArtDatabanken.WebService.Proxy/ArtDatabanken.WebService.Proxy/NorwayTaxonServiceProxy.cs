using System;
using System.ServiceModel;
using ArtDatabanken.WebService.Proxy.NorwayTaxonService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages taxon service requests to Norway.
    /// </summary>
    public class NorwayTaxonServiceProxy : WebServiceProxyBase
    {
        /// <summary>
        /// Create a NorwayTaxonServiceProxy instance.
        /// </summary>
        public NorwayTaxonServiceProxy()
        {
            WebServiceAddress = null;
            WebServiceProtocol = WebServiceProtocol.SOAP11;
            InternetProtocol = InternetProtocol.Http;
        }

        /// <summary>
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example User.ArtDatabankenSOA.se/UserService.svc.
        /// </summary>
        public String WebServiceAddress
        { get; set; }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<ArtsnavnebaseSoap>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<ArtsnavnebaseSoap>)client).Abort();
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
            ArtsnavnebaseSoapClient client;

            client = new ArtsnavnebaseSoapClient(GetBinding(),
                                                 GetEndpointAddress());

            return client;
        }

        /// <summary>
        /// Get taxon by id.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Requested taxon.</returns>         
        public ArtsnavnType GetTaxonById(Int32 taxonId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.TaksonFraID(taxonId);
            }
        }

        /// <summary>
        ///  Get taxon names that matches the search criteria.
        /// </summary>
        /// <param name="nameSearchString">Taxon name search string.</param>
        /// <returns>Taxon names that matches the search criteria.</returns>
        public ArtsnavnType GetTaxonNamesBySearchCriteria(String nameSearchString)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {

                return client.Client.Artssok(nameSearchString);
            }
        }

        /// <summary>
        /// Get address to web service.
        /// </summary>
        /// <returns>Address to web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                WebServiceAddress = Settings.Default.NorwayTaxonServiceAddress;
            }
            return WebServiceAddress;
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private ArtsnavnebaseSoapClient _client;
            private readonly NorwayTaxonServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(NorwayTaxonServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (ArtsnavnebaseSoapClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public ArtsnavnebaseSoapClient Client
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
