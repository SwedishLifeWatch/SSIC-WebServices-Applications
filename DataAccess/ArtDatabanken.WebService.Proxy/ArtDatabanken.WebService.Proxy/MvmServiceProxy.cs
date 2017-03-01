using System;
using System.ServiceModel;
using ArtDatabanken.Security;
using ArtDatabanken.WebService.Proxy.MvmService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages taxon service requests to Mvm.
    /// </summary>
    public class MvmServiceProxy : WebServiceProxyBase
    {
        /// <summary>
        /// The _nors service endpoint address.
        /// </summary>
        private EndpointAddress _mvmServiceEndpointAddress;

        /// <summary>
        /// Create a MvmServiceProxy instance.
        /// </summary>
        public MvmServiceProxy()
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
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<ISpeciesObservationChangeService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<ISpeciesObservationChangeService>)client).Abort();
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
            SpeciesObservationChangeServiceClient client;

            client = new SpeciesObservationChangeServiceClient(GetBinding(), GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            IncreaseDataSize("GetSpeciesObservationChangeAsSpecies", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Checks if the Service is Alive.
        /// </summary>
        /// <returns>
        /// True if the service is alive.
        /// </returns>
        public Boolean AreYouThere()
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.AreYouThere();
            }
        }

        /// <summary>
        /// Returns the Service Version.
        /// </summary>
        /// <returns>
        /// A string containing the service version.
        /// </returns>
        public String GetServiceVersion()
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetServiceVersion();
            }
        }

        /// <summary>
        /// Use as template.
        /// </summary>
        /// <param name="changedFrom">
        /// ChangedFrom observation date.
        /// </param>
        /// <param name="isChangedFromSpecified">
        /// Is fromdate specific.
        /// </param>
        /// <param name="changedTo">
        /// Changedto observation date.
        /// </param>
        /// <param name="isChangedToSpecified">
        /// Is todate specific.
        /// </param>
        /// <param name="changeId">
        /// Changed from ID.
        /// </param>
        /// <param name="isChangeIdSpecified">
        /// Is id specific.
        /// </param>
        /// <param name="maxReturnedChanges">
        /// The max returned observations.
        /// </param>
        /// <returns>
        /// Requested taxon.
        /// </returns>
        public WebSpeciesObservationChange GetSpeciesObservationChangeAsSpecies(DateTime changedFrom, Boolean isChangedFromSpecified, DateTime changedTo, Boolean isChangedToSpecified, Int64 changeId, Boolean isChangeIdSpecified, Int64 maxReturnedChanges)
        {

            using (ClientProxy client = new ClientProxy(this, 1))
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
        /// Returns the detcrypted token.
        /// </summary>
        /// <returns>
        /// A string containing the Token.
        /// </returns>
        private static string GetDecryptedToken()
        {
            CipherString cipherString;
            String token;

            // Valid to 2017-10-31. Label = PublicTicket04.
            // Steps to make when next token should be collected.
            // 1 Log in to https://miljodata.slu.se/mvm/LogOn with your UserService account.
            // 2 Creat a new token with button "Aktivera och visa publika tickets".
            // 3 Encrypt token on servers and your own computer and update encrypted token in this method.
            // 4 Update valid to date in this method.
            cipherString = new CipherString();
            switch (Environment.MachineName)
            {
                case "ARTFAKTA-DEV": // Team Species Fact test server.
                    token = null;
                    break;

                case "ARTSERVICE2-1": // New production web service server.
                    token = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHLKGBDFBDBPKCEEOKLLJKALLMDFDELLAAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAALIDFJIDFKFIIDCACAKKABPLPBENILFPOAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAHKBCGGOJDAODLEHICGHNNNNPENCGKIGEBIAAAAAACFJEEBIKIKNJJAPAAFGBDMJEIKCINCHOMMMCGCCIIEPNLHIMBEAAAAAAKIJEHEABGDEJDKFKEFGOLHMCCDDMBJCKNJKDAHFJ";
                    break;

                case "ARTSERVICE2-2": // New production web service server.
                    token = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAJGEOCNEDAFICHHEFLFLMCEBIEGIHNGNCAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAEIFDHKJAHPFCBHAIIGLIMBICIGCINBOCAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAALMHPCECEPANLPFIBLIJHBCDLKGPABAPDBIAAAAAALIBPBDHGCJOPHMHFKMFDCCLFCPOILHBPECPAKIDMPKLFEPLEBEAAAAAAJBMJHBOJEIGFBOOMBMAOPBICOHDMJGOIMLGBDHHA";
                    break;

                case "MONESES-ST": // System test server.
                    token = null;
                    break;

                case "TFSBUILD": // Build Server
                    token = null;
                    break;

                case "SLU011837": // 
                    token = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAFDGKFHMFDIIJOBEBLFPNAIHICEBDJLBPAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAJJNIIHIACKMLFGFDGFPBFJNBNFFKNIDBAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAKGAOGIFCDIDBIMAKGBBAKLOPGMNKHCCEBIAAAAAAEHOJPBOJFGFHGLAJAEDHODEHHNBOMCOAJBMHLLFJLKHJINKDBEAAAAAABOOLIECCMBJBKOGMNCNHCELGKKEABAICKIMABMBF";
                    break;

                case "SLU004994": // 
                    token = null;
                    break;

                case "SLW-DEV": // Team Two Blueberries test server.
                    token = "ABAAAAAANAIMJNNPABBFNBBBIMHKAAMAEPMCJHOLABAAAAAAHDCGMMMLDMCPKPEMKILLNDOBEOLFBAJJAEAAAAAAACAAAAAAAAAAADGGAAAAMAAAAAAABAAAAAAAAIJKOMFGLDNCEGINNHHGFCBMJBPFONDNAAAAAAAAAEIAAAAAKAAAAAAABAAAAAAAJBDBIHHKDAEJJPFENMFGGEOMGNBBCLELBIAAAAAADICKGNBMEBONAPDPJKCIEMEAJBDCLKGGOGNIAFOBHPPDBPEABEAAAAAAMMLJOHMLMGOCNCIMBGPOMLIPOLAEDEKACJNIHMMO";
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
            if (_mvmServiceEndpointAddress.IsNull())
            {
                String webAddress;
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

                switch (WebServiceProtocol)
                {
                    case WebServiceProtocol.Binary:
                        webAddress += @"/Fast";
                        break;
                    case WebServiceProtocol.SOAP11:
                        webAddress += @"/SOAP11";
                        break;
                    case WebServiceProtocol.SOAP12:
                        webAddress += @"/SOAP12";
                        break;
                    default:
                        throw new Exception("Unknown WebServiceProtocol: " + WebServiceProtocol);
                }

                _mvmServiceEndpointAddress = new EndpointAddress(webAddress);
            }

            return _mvmServiceEndpointAddress;
        }

        /// <summary>
        /// Get address to web service.
        /// </summary>
        /// <returns>Address to web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                //if (Configuration.Debug)
                //{
                //    WebServiceAddress = Settings.Default.MvmServiceAddressTest;
                //}
                //else
                {
                    WebServiceAddress = Settings.Default.MvmServiceAddress;
                }
            }

            return WebServiceAddress;
        }

        /// <summary>
        /// Test if service is ready to use.
        /// </summary>
        /// <returns>
        /// True, if service is ready to use.
        /// </returns>
        public Boolean IsReadyToUse()
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 1))
                {
                    return client.Client.IsReadyToUse();
                }
            }
            catch (Exception)
            {
                // The service is probably not ready to use.
                return false;
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
            /// The _client.
            /// </summary>
            private SpeciesObservationChangeServiceClient _client;

            /// <summary>
            /// The _web service.
            /// </summary>
            private readonly MvmServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(MvmServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (SpeciesObservationChangeServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public SpeciesObservationChangeServiceClient Client
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
