using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Proxy.WramService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages taxon service requests to Wram.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class WramServiceProxy : WebServiceProxyBase
    {
        /// <summary>
        /// The _Wram service endpoint address.
        /// </summary>
        private EndpointAddress _wramServiceEndpointAddress;

        /// <summary>
        /// Create a WramServiceProxy instance.
        /// </summary>
        public WramServiceProxy()
        {
            WebServiceAddress = null;
            WebServiceProtocol = WebServiceProtocol.SOAP11;
            InternetProtocol = InternetProtocol.Http;
            _wramServiceEndpointAddress = null;
        }

        /// <summary>
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example User.ArtDatabankenSOA.se/UserService.svc.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public String WebServiceAddress { get; set; }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<IWramSpeciesDataChangeService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<IWramSpeciesDataChangeService>)client).Abort();
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
            WramSpeciesDataChangeServiceClient client;

            client = new WramSpeciesDataChangeServiceClient(GetBinding(), GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetSpeciesObservationChangeAsSpecies", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Checks for server response.
        /// </summary>
        /// <returns>If server are there.</returns>
        public Boolean AreYouThere()
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.AreYouThere();
            }
        }

        /// <summary>
        /// Gets service version.
        /// </summary>
        /// <returns>The service version.</returns>
        public String GetServiceVersion()
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetServiceVersion();
            }
        }

        /// <summary>
        /// Get Species Observation Change As Species.
        /// </summary>
        /// <param name="changedFrom">
        /// Changed from date.
        /// </param>
        /// <param name="isChangedFromSpecified">
        /// Is changed from specified.
        /// </param>
        /// <param name="changedTo">
        /// Changed to date.
        /// </param>
        /// <param name="isChangedToSpecified">
        /// Is changed to specified.
        /// </param>
        /// <param name="changeId">
        /// From witch change id.
        /// </param>
        /// <param name="isChangeIdSpecified">
        /// The is Change Id Specified.
        /// </param>
        /// <param name="maxReturnedChanges">
        /// Max number of observations returned.
        /// </param>
        /// <returns>
        /// Requested taxon.
        /// </returns>
        public WebSpeciesObservationChange GetSpeciesObservationChangeAsSpecies(DateTime changedFrom, Boolean isChangedFromSpecified, DateTime changedTo, Boolean isChangedToSpecified, Int64 changeId, Boolean isChangeIdSpecified, Int64 maxReturnedChanges)
        {
            using (ClientProxy client = new ClientProxy(this, 10))
            {
                
                return client.Client.GetSpeciesObservationChangeAsSpecies(GetDecryptedToken(), 
                                                                          changedFrom, 
                                                                          isChangedFromSpecified, 
                                                                          changedTo, 
                                                                          isChangedToSpecified, 
                                                                          changeId, 
                                                                          isChangeIdSpecified, 
                                                                          maxReturnedChanges);
            }
        }

        /// <summary>
        /// Token to identify the client.       
        /// </summary>
        /// <returns>
        /// Decrypted token.
        /// </returns>
        private static string GetDecryptedToken()
        {
            CipherString cipherString;
            String token;

            cipherString = new CipherString();
            switch (Environment.MachineName)
            {
                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    token = null;
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    token = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHLKGBDFBDBPKCEEOKLLJKALLMDFDELLAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAADCALMELDBAEHDBEMLPBOCEDNHILCPOOHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAGLIMMHMBDHPAICHMJOHKMIEGCMLALGCPBIAAAAAAIEDBAJEDJDLCJEKGJFICCEMHKBHDCIJNBFOPDNAJCJOJFKJIBEAAAAAAFDJKIKLNMLKJBCGLHBKAIAPKHDKNDJKDJOEICPKE";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    token = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJGEOCNEDAFICHHEFLFLMCEBIEGIHNGNCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJFFAJFGLBCMPFHKKOJJIKIGIGOMAAMCHAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAPHMOAONACAHEKJCJJOBOIIIJOCMPEFCCBIAAAAAABCIANDGFILIFGLHENNHGGMPKDFMMCBGIAJHMGCLAOJBIDDNCBEAAAAAAMHMLGILGEGLDKMNGPCAACBEFANNIFGONAOLPLEFJ";
                    break;

                case "MONESES-ST": // System test server.
                    token = null;
                    break;

                case "TFSBUILD": // Build Server
                    token = null;
                    break;

                case "SLU011837": // 
                    token = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFDGKFHMFDIIJOBEBLFPNAIHICEBDJLBPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALEGBHAPJAIIAHNAGOMCNIPPCKJANMFNPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAMCBMOMPABKCOENLIPGFKBEIGBMMONLNLBIAAAAAAKHDBHJNELBLKAFIIICHBEENFNDGNMKFJBFIICMLKIGCGKPCDBEAAAAAAIHGLIOMFIPHPMAIDOIICFAOAOBBDEAAMMCCEEDMA";
                    break;

                case "SLU004994": // 
                    token = null;
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    token = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHDCGMMMLDMCPKPEMKILLNDOBEOLFBAJJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAACGDPODCLMOKKDFEINGKIDBNLJDOHLLEPAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHLIGKPGMMADGLMDHPFKPOLEAGKCGEIEJBIAAAAAAELNCIMLNJBEPLLHKDCIPCMOPEIHPPHOFOGFICOLDEKIAJBDNBEAAAAAAGHJPPHCFEKMGAPENGDLCPABJFONBKNBJNOMKGNKJ";
                    break;

                default:
                    throw new ApplicationException("Unknown web server " + Environment.MachineName);
            }

            return cipherString.DecryptText(token);
        }

        /// <summary>
        /// Get address of currently used endpoint.
        /// </summary>
        /// <returns>Address of currently used endpoint.</returns>
        public override EndpointAddress GetEndpointAddress()
        {
            String webAddress;

            if (_wramServiceEndpointAddress.IsNull())
            {
                switch (InternetProtocol)
                {
                    case InternetProtocol.Http:
                        webAddress = @"http://";
                        break;

                    case InternetProtocol.Https:
                        webAddress = @"https://";
                        break;

                    default:
                        throw new ApplicationException("Not supported Internet protocol: " + InternetProtocol);
                }

                webAddress += GetWebServiceAddress();
          
                webAddress += @"/soap";

                _wramServiceEndpointAddress = new EndpointAddress(webAddress);
            }

            return _wramServiceEndpointAddress;
        }

        /// <summary>
        /// Get address to web service.
        /// </summary>
        /// <returns>Address to web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                WebServiceAddress = Settings.Default.WramServiceAddress;
            }

            return WebServiceAddress;
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
            /// The _client.
            /// </summary>
            private WramSpeciesDataChangeServiceClient _client;

            /// <summary>
            /// The _web service.
            /// </summary>
            private readonly WramServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(WramServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (WramSpeciesDataChangeServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public WramSpeciesDataChangeServiceClient Client
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
