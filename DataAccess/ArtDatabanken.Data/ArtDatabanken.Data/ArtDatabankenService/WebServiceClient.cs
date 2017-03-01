#define IS_CLIENT_CONFIGURATION_IN_CODE
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using ArtDatabanken.Data.WebService;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Class used as client to the web service.
    /// </summary>
    sealed public class WebServiceClient : IDisposable
    {
        private const String ArtDatabankenSoaServiceAddress = @"https://ArtDatabanken.artdatabankensoa.se/ArtDatabankenService.svc/Fast";
        private const String ArtportalenTestServiceAddress = @"https://ArtDatabankenService-st.artdata.slu.se/ArtDatabankenService.svc/Fast";
        private const String LocalTestServiceAddress = @"http://localhost:2172/ArtDatabankenService/ArtDatabankenService.svc";
        private const String OldTestServerServiceAddress = @"https://moneses-dev.artdata.slu.se/ArtDatabankenService/ArtDatabankenService.svc/Fast";
        private const String SpeciesFactTestServiceAddress = @"https://artfakta-dev.artdata.slu.se/ArtDatabankenService/ArtDatabankenService.svc/Fast";
        private const String SystemTestServiceAddress = @"https://ArtDatabankenService-st.artdata.slu.se/ArtDatabankenService.svc/Fast";
        private const String TwoBlueberriesTestServiceAddress = @"https://Slw-dev.ardata.slu.se/ArtDatabankenService/ArtDatabankenService.svc/Fast";

        /// <summary>
        /// Enumeration of the different end points that
        /// are supported in this client.
        /// </summary>
        public enum EndpointId
        {
            /// <summary>
            /// Used for production clients.
            /// </summary>
            ArtDatabankenSoaEndpoint,

            /// <summary>
            /// Development test server for team Artportalen.
            /// </summary>
            ArtportalenTestEndpoint,

            /// <summary>
            /// Used for local test during development.
            /// Running both client and
            /// webservice on the same machine.
            /// </summary>
            LocalTestEndpoint,

            /// <summary>
            /// Used for remote test during development.
            /// </summary>
            OldTestServerEndpoint,

            /// <summary>
            /// Development test server for team Species Fact.
            /// </summary>
            SpeciesFactTestEndpoint,

            /// <summary>
            /// Test server used for system testing.
            /// </summary>
            SystemTestEndpoint,

            /// <summary>
            /// Development test server for team Two Blueberries.
            /// </summary>
            TwoBlueberriesTestEndpoint
        }

        private ArtDatabankenServiceClient _client;
        private static readonly Hashtable _clients;
        private static CustomBinding _fastBinding;
        private static EndpointId _endpoint;
        private static String _clientToken;
        private static String _webServiceAddress;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static WebServiceClient()
        {
            _clients = new Hashtable();
            switch (Configuration.InstallationType)
            {
                case InstallationType.ArtportalenTest:
                    Endpoint = EndpointId.ArtportalenTestEndpoint;
                    break;
                case InstallationType.LocalTest:
                    Endpoint = EndpointId.LocalTestEndpoint;
                    break;
                case InstallationType.Production:
                    Endpoint = EndpointId.ArtDatabankenSoaEndpoint;
                    break;
                case InstallationType.ServerTest:
                    Endpoint = EndpointId.OldTestServerEndpoint;
                    break;
                case InstallationType.SpeciesFactTest:
                    Endpoint = EndpointId.SpeciesFactTestEndpoint;
                    break;
                case InstallationType.SystemTest:
                    Endpoint = EndpointId.SystemTestEndpoint;
                    break;
                case InstallationType.TwoBlueberriesTest:
                    Endpoint = EndpointId.TwoBlueberriesTestEndpoint;
                    break;
                default:
                    throw new ApplicationException("Not handled installation type = " + Configuration.InstallationType);
            }

            WebServiceAddress = null;
        }

        /// <summary>
        /// Handle which Endpoint that are used.
        /// </summary>
        public static EndpointId Endpoint
        {
            get { return _endpoint; }
            set
            {
                Logout();
                _endpoint = value;
            }
        }

        /// <summary>
        /// A URI that specifies the address of the HTTP proxy.
        /// </summary>
        public static Uri HttpProxyAddress
        { get; set; }

        /// <summary>
        /// Address to web service including internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example https://SwedishSpeciesObservation.ArtDatabankenSOA.se/SwedishSpeciesObservationService.svc/Fast.
        /// </summary>
        public static String WebServiceAddress
        {
            get
            {
                return _webServiceAddress;
            }
            set
            {
                _webServiceAddress = value;
                ReleaseClients();
            }
        }

        /// <summary>
        /// Clear data cache in webb service.
        /// </summary>
        public static void ClearCache()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(1).ClearCache(_clientToken);
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        public static void CommitTransaction()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(1).CommitTransaction(_clientToken);
            }
        }

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="reference">New reference.</param>
        public static void CreateReference(WebReference reference)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(2).CreateReference(_clientToken, reference);
            }
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        public static void DeleteTrace()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(1).DeleteTrace(_clientToken);
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Recycle the client instance.
        /// </summary>
        public void Dispose()
        {
            if (_client.IsNotNull())
            {
                if ((_client.State != CommunicationState.Opened) ||
                    (!PushClient(_client)))
                {
                    // Client is not in state open or
                    // was not added to the client pool.
                    // Release resources.
                    _client.Close();
                }
                _client = null;
            }
        }

        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <returns>Information about bird nest activities.</returns>
        public static List<WebBirdNestActivity> GetBirdNestActivities()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetBirdNestActivities(_clientToken);
            }
        }

        /// <summary>
        /// Get information about cities that matches the search string.
        /// </summary>
        /// <param name="searchString">String that city name must match.</param>
        /// <returns>Information about cities.</returns>
        public static List<WebCity> GetCitiesBySearchString(String searchString)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetCitiesBySearchString(_clientToken, searchString);
            }
        }

        /// <summary>
        /// Get the real web service client.
        /// </summary>
        /// <returns>Returns the real web service client.</returns>
        private ArtDatabankenServiceClient GetClient()
        {
            ArtDatabankenServiceClient client;

#if IS_CLIENT_CONFIGURATION_IN_CODE
            BasicHttpBinding basicHttpBinding;
            BinaryMessageEncodingBindingElement messageEncoding;
            EndpointAddress endpointAddress;
            HttpsTransportBindingElement httpsTransport;
            OperationDescription operation;
            WSHttpBinding wsHttpBinding;

            // Get endpoint address.
            if (WebServiceAddress.IsEmpty())
            {
                if (Endpoint != EndpointId.LocalTestEndpoint)
                {
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.ArtDatabankenService);
                }

                if (WebServiceAddress.IsEmpty())
                {
                    switch (Endpoint)
                    {
                        case EndpointId.ArtDatabankenSoaEndpoint:
                            WebServiceAddress = ArtDatabankenSoaServiceAddress;
                            break;
                        case EndpointId.ArtportalenTestEndpoint:
                            WebServiceAddress = ArtportalenTestServiceAddress;
                            break;
                        case EndpointId.LocalTestEndpoint:
                            WebServiceAddress = LocalTestServiceAddress;
                            break;
                        case EndpointId.OldTestServerEndpoint:
                            WebServiceAddress = OldTestServerServiceAddress;
                            break;
                        case EndpointId.SpeciesFactTestEndpoint:
                            WebServiceAddress = SpeciesFactTestServiceAddress;
                            break;
                        case EndpointId.SystemTestEndpoint:
                            WebServiceAddress = SystemTestServiceAddress;
                            break;
                        case EndpointId.TwoBlueberriesTestEndpoint:
                            WebServiceAddress = TwoBlueberriesTestServiceAddress;
                            break;
                        default:
                            throw new Exception("Unknown EndPoint " + _endpoint);
                    }
                }
                else
                {
                    // Add internet protocol and
                    // binding protocol to web service address.
                    WebServiceAddress = @"https://" + WebServiceAddress + @"/Fast";
                }
            }

            endpointAddress = new EndpointAddress(WebServiceAddress);

            // Create client.
            if (WebServiceAddress.ToLower().EndsWith(@"/fast"))
            {
                // Binary binding.
                if (_fastBinding.IsNull())
                {
                    httpsTransport = new HttpsTransportBindingElement();
                    httpsTransport.ManualAddressing = false;
                    httpsTransport.MaxBufferPoolSize = 2000000000;
                    httpsTransport.MaxReceivedMessageSize = 2000000000;
                    httpsTransport.AllowCookies = false;
                    httpsTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous;
                    httpsTransport.BypassProxyOnLocal = false;
                    httpsTransport.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                    httpsTransport.KeepAliveEnabled = true;
                    httpsTransport.MaxBufferSize = 2000000000;
                    httpsTransport.ProxyAddress = GetHttpProxyAddress();
                    httpsTransport.ProxyAuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous;
                    httpsTransport.Realm = String.Empty;
                    httpsTransport.RequireClientCertificate = false;
                    httpsTransport.TransferMode = TransferMode.Buffered;
                    httpsTransport.UnsafeConnectionNtlmAuthentication = false;
                    httpsTransport.UseDefaultWebProxy = GetHttpProxyAddress().IsNull();

                    messageEncoding = new BinaryMessageEncodingBindingElement();
                    messageEncoding.MaxReadPoolSize = 2000000000;
                    messageEncoding.MaxWritePoolSize = 2000000000;
                    messageEncoding.MaxSessionSize = 2000000000;
                    messageEncoding.ReaderQuotas.MaxDepth = 2000000000;
                    messageEncoding.ReaderQuotas.MaxStringContentLength = 2000000000;
                    messageEncoding.ReaderQuotas.MaxArrayLength = 2000000000;
                    messageEncoding.ReaderQuotas.MaxBytesPerRead = 2000000000;
                    messageEncoding.ReaderQuotas.MaxNameTableCharCount = 2000000000;

                    _fastBinding = new CustomBinding(messageEncoding, httpsTransport);
                }
                client = new ArtDatabankenServiceClient(_fastBinding, endpointAddress);
            }
            else
            {
                if (WebServiceAddress.ToLower().EndsWith(@"/soap12"))
                {
                    // SOAP 1.2 binding.
                    basicHttpBinding = new BasicHttpBinding();
                    basicHttpBinding.BypassProxyOnLocal = false;
                    basicHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                    basicHttpBinding.MaxBufferPoolSize = 2000000000;
                    basicHttpBinding.MaxReceivedMessageSize = 2000000000;
                    basicHttpBinding.MessageEncoding = WSMessageEncoding.Text;
                    basicHttpBinding.OpenTimeout = new TimeSpan(0, 20, 0);
                    basicHttpBinding.ProxyAddress = GetHttpProxyAddress();
                    basicHttpBinding.ReaderQuotas.MaxArrayLength = 2000000000;
                    basicHttpBinding.ReaderQuotas.MaxBytesPerRead = 2000000000;
                    basicHttpBinding.ReaderQuotas.MaxDepth = 2000000000;
                    basicHttpBinding.ReaderQuotas.MaxNameTableCharCount = 2000000000;
                    basicHttpBinding.ReaderQuotas.MaxStringContentLength = 2000000000;
                    if ("https" == endpointAddress.Uri.OriginalString.Substring(0, 5))
                    {
                        basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                        basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                        basicHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                    }
                    else
                    {
                        basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
                    }
                    basicHttpBinding.TextEncoding = Encoding.UTF8;
                    basicHttpBinding.UseDefaultWebProxy = GetHttpProxyAddress().IsNull();
                    client = new ArtDatabankenServiceClient(basicHttpBinding, endpointAddress);
                }
                else
                {
                    // SOAP 1.1 binding.
                    wsHttpBinding = new WSHttpBinding();
                    wsHttpBinding.BypassProxyOnLocal = false;
                    wsHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                    wsHttpBinding.MaxBufferPoolSize = 2000000000;
                    wsHttpBinding.MaxReceivedMessageSize = 2000000000;
                    wsHttpBinding.MessageEncoding = WSMessageEncoding.Mtom;
                    wsHttpBinding.OpenTimeout = new TimeSpan(0, 20, 0);
                    wsHttpBinding.ProxyAddress = GetHttpProxyAddress();
                    wsHttpBinding.ReaderQuotas.MaxArrayLength = 2000000000;
                    wsHttpBinding.ReaderQuotas.MaxBytesPerRead = 2000000000;
                    wsHttpBinding.ReaderQuotas.MaxDepth = 2000000000;
                    wsHttpBinding.ReaderQuotas.MaxNameTableCharCount = 2000000000;
                    wsHttpBinding.ReaderQuotas.MaxStringContentLength = 2000000000;
                    if ("https" == endpointAddress.Uri.OriginalString.Substring(0, 5))
                    {
                        wsHttpBinding.Security.Mode = SecurityMode.Transport;
                        wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                        wsHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                    }
                    else
                    {
                        wsHttpBinding.Security.Mode = SecurityMode.None;
                    }
                    wsHttpBinding.TextEncoding = Encoding.UTF8;
                    wsHttpBinding.TransactionFlow = false;
                    wsHttpBinding.UseDefaultWebProxy = GetHttpProxyAddress().IsNull();
                    client = new ArtDatabankenServiceClient(wsHttpBinding, endpointAddress);
                }
            }

            // Increase data size for all methods that  
            // sends or receives a lot of data.
            operation = client.Endpoint.Contract.Operations.Find("GetCitiesBySearchString");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetFactors");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetFactorsBySearchCriteria");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetFactorTrees");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetFactorTreesBySearchCriteria");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetLog");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetReferences");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetSpeciesFactsById");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetSpeciesFactsByIdentifier");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetSpeciesFactsByUserParameterSelection");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetSpeciesObservationChange");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetSpeciesObservationsById");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetSpeciesObservationsBySearchCriteria");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetTaxaById");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetTaxaByQuery");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetTaxaByOrganismOrRedlist");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetTaxaBySearchCriteria");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetTaxaBySpeciesObservations");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetTaxonNamesBySearchCriteria");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("GetTaxonTreesBySearchCriteria");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
            operation = client.Endpoint.Contract.Operations.Find("UpdateSpeciesFacts");
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
#else
            switch (EndPoint)
            {
                case EndPointId.ColiasEndPoint:
                    client = new ArtDatabankenServiceClient("ColiasServiceEndpoint");
                    break;
                case EndPointId.ColiasFastEndPoint:
                    client = new ArtDatabankenServiceClient("ColiasServiceFastEndpoint");
                    break;
                case EndPointId.LocalEndPoint:
                    client = new ArtDatabankenServiceClient("LocalServiceEndpoint");
                    break;
                case EndPointId.SvalanEndPoint:
                    client = new ArtDatabankenServiceClient("SvalanServiceEndpoint");
                    break;
                case EndPointId.WebServiceEndPoint:
                    client = new ArtDatabankenServiceClient("WebServiceEndpoint");
                    break;
                default:
                    throw new Exception("Unknown EndPoint " + _endPoint);
            }
#endif
            return client;
        }

        /// <summary>
        /// Get the real web service client.
        /// </summary>
        /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
        /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
        /// <returns>Returns the real web service client.</returns>
        private ArtDatabankenServiceClient GetClient(Int32 operationTimeoutMinutes,
                                                     Int32 operationTimeoutSeconds = 0)
        {
            TimeSpan timeSpan;

            if (_client.IsNull())
            {
                _client = PopClient((operationTimeoutMinutes * 60) + operationTimeoutSeconds);
            }

            timeSpan = new TimeSpan(0, operationTimeoutMinutes, operationTimeoutSeconds);
            _client.Endpoint.Binding.CloseTimeout = timeSpan;
            _client.Endpoint.Binding.OpenTimeout = timeSpan;
            _client.Endpoint.Binding.ReceiveTimeout = timeSpan;
            _client.Endpoint.Binding.SendTimeout = timeSpan;
            return _client;
        }

        /// <summary>
        /// Get information about swedish counties.
        /// </summary>
        /// <returns>Information about swedish counties.</returns>
        public static List<WebCounty> GetCounties()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetCounties(_clientToken);
            }
        }

        /// <summary>
        /// Get information about databases.
        /// </summary>
        /// <returns>Information about databases.</returns>
        public static List<WebDatabase> GetDatabases()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetDatabases(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all factor data types.
        /// </summary>
        /// <returns>Factor data types.</returns>
        public static List<WebFactorDataType> GetFactorDataTypes()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetFactorDataTypes(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all factor field enums.
        /// </summary>
        /// <returns>Factor field enums.</returns>
        public static List<WebFactorFieldEnum> GetFactorFieldEnums()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetFactorFieldEnums(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all factor field types.
        /// </summary>
        /// <returns>Factor field types.</returns>
        public static List<WebFactorFieldType> GetFactorFieldTypes()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetFactorFieldTypes(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all factor origins.
        /// </summary>
        /// <returns>Factor origins.</returns>
        public static List<WebFactorOrigin> GetFactorOrigins()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetFactorOrigins(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all factors.
        /// </summary>
        /// <returns>Factors.</returns>
        public static List<WebFactor> GetFactors()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetFactors(_clientToken);
            }
        }

        /// <summary>
        /// Get information about factors that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Information about factors.</returns>
        /// <exception cref="ArgumentException">Thrown if factorSearchCriteria is null.</exception>
        public static List<WebFactor> GetFactorsBySearchCriteria(WebFactorSearchCriteria searchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetFactorsBySearchCriteria(_clientToken, searchCriteria);
            }
        }

        /// <summary>
        /// Get all top factor tree nodes.
        /// </summary>
        /// <returns>All top factor tree nodes.</returns>
        public static List<WebFactorTreeNode> GetFactorTrees()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(5).GetFactorTrees(_clientToken);
            }
        }

        /// <summary>
        /// Get information about factor trees that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The factor tree search criteria.</param>
        /// <returns>Factor tree information.</returns>
        public static List<WebFactorTreeNode> GetFactorTreesBySearchCriteria(WebFactorTreeSearchCriteria searchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(5).GetFactorTreesBySearchCriteria(_clientToken, searchCriteria);
            }
        }

        /// <summary>
        /// Get information about all factor update modes.
        /// </summary>
        /// <returns>Factor update modes.</returns>
        public static List<WebFactorUpdateMode> GetFactorUpdateModes()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetFactorUpdateModes(_clientToken);
            }
        }

        /// <summary>
        /// Get information about host taxa.
        /// </summary>
        /// <param name="factorId">Id for for factor to get host taxa information about.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>Host taxa information.</returns>
        public static List<WebTaxon> GetHostTaxa(Int32 factorId,
                                                 TaxonInformationType taxonInformationType)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetHostTaxa(_clientToken, factorId, taxonInformationType);
            }
        }

        /// <summary>
        /// Get all host taxa associated with a certain taxon.
        /// The method is restricted to faktors of type substrate.
        /// </summary>
        /// <param name="taxonId">Id of taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>List of host taxa.</returns>
        public static List<WebTaxon> GetHostTaxaByTaxonId(Int32 taxonId,
                                                          TaxonInformationType taxonInformationType)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(20).GetHostTaxaByTaxonId(_clientToken, taxonId, taxonInformationType);             
            }
        }

        /// <summary>
        /// Get http proxy address or null if no address has been set.
        /// </summary>
        /// <returns>Http proxy address or null if no address has been set.</returns>       
        private static Uri GetHttpProxyAddress()
        {
            Uri httpProxyAddress;

            httpProxyAddress = HttpProxyAddress;
            if (httpProxyAddress.IsNull())
            {
                httpProxyAddress = WebServiceProxy.Configuration.HttpProxyAddress;
            }
            return httpProxyAddress;
        }

        /// <summary>
        /// Get information about all individal categories.
        /// </summary>
        /// <returns>IndividualCategories.</returns>
        public static List<WebIndividualCategory> GetIndividualCategories()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetIndividualCategories(_clientToken);
            }
        }

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        public static List<WebLogRow> GetLog(LogType type,
                                             String userName,
                                             Int32 rowCount)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetLog(_clientToken, type, userName, rowCount);
            }
        }

        /// <summary>
        /// Get information about all periods.
        /// </summary>
        /// <returns>Periods.</returns>
        public static List<WebPeriod> GetPeriods()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetPeriods(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all period types.
        /// </summary>
        /// <returns>Period types.</returns>
        public static List<WebPeriodType> GetPeriodTypes()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetPeriodTypes(_clientToken);
            }
        }

        /// <summary>
        /// Get information about swedish provinces.
        /// </summary>
        /// <returns>Information about swedish provinces.</returns>
        public static List<WebProvince> GetProvinces()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetProvinces(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all references.
        /// </summary>
        /// <returns>References.</returns>
        public static List<WebReference> GetReferences()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetReferences(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all references that matches search string.
        /// </summary>
        /// <param name="searchString">Reference search string.</param>
        /// <returns>References.</returns>
        public static List<WebReference> GetReferencesBySearchString(String searchString)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetReferencesBySearchString(_clientToken, searchString);
            }
        }

        /// <summary>
        /// Get information about all categories of species fact qualty.
        /// </summary>
        /// <returns>Species fact qualities.</returns>
        public static List<WebSpeciesFactQuality> GetSpeciesFactQualities()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetSpeciesFactQualities(_clientToken);
            }
        }

        /// <summary>
        /// Get information about species facts.
        /// </summary>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species fact information.</returns>
        public static List<WebSpeciesFact> GetSpeciesFactsById(List<Int32> speciesFactIds)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetSpeciesFactsById(_clientToken, speciesFactIds);
            }
        }

        /// <summary>
        /// Get information about species facts.
        /// </summary>
        /// <param name="speciesFacts">Species facts to get information about.</param>
        /// <returns>Species facts information.</returns>
        public static List<WebSpeciesFact> GetSpeciesFactsByIdentifier(List<WebSpeciesFact> speciesFacts)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetSpeciesFactsByIdentifier(_clientToken, speciesFacts);
            }
        }

        /// <summary>
        /// Get information about species facts that correspond to the user parameter selection.
        /// </summary>
        /// <param name="userParameterSelection">The user parameter selection.</param>
        /// <returns>Information about species facts.</returns>
        /// <exception cref="ArgumentException">Thrown if userParameterSelection is null.</exception>
        public static List<WebSpeciesFact> GetSpeciesFactsByUserParameterSelection(WebUserParameterSelection userParameterSelection)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(4).GetSpeciesFactsByUserParameterSelection(_clientToken, userParameterSelection);
            }
        }

        /// <summary>
        /// Get information about species observations
        /// that has changed in the specified date range.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 100000 observations of each change type (new or updated)
        /// with information can be retrieved in one call.
        /// Max 1000000 observations of each change type (deleted, new
        /// or updated), with GUIDs or ids, can be retrieved in one call.
        /// Parameters changedFrom and changedTo may be the same date.
        /// Parameter changedTo must not be today or in the future.
        /// If parameter changedTo is yesterday the method call
        /// must be made after the nightly update of the 
        /// species observations have been performed. 
        /// Currently it is ok to call this method after 05:00
        /// if yesterdays species observations should be retrieved.
        /// Only date part of parameters changedFrom and changedTo
        /// are used. It does not matter what time of day that is set
        /// in parameters changedFrom and changedTo.
        /// </summary>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <returns>Information about changed species observations.</returns>
        public static WebSpeciesObservationChange GetSpeciesObservationChange(DateTime changedFrom,
                                                                              DateTime changedTo)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                try
                {

                return client.GetClient(20).GetSpeciesObservationChange(_clientToken,
                                                                        changedFrom,
                                                                        changedTo);


                }
                catch (Exception)
                {

                    return null;
                }
            }
        }

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        public static Int32 GetSpeciesObservationCount(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetSpeciesObservationCountBySearchCriteria(_clientToken,
                                                                                      searchCriteria);
            }
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Information about species observation.</returns>
        public static WebSpeciesObservationInformation GetSpeciesObservations(WebSpeciesObservationSearchCriteria searchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(20).GetSpeciesObservationsBySearchCriteria(_clientToken,
                                                                                   searchCriteria);
            }
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 10000 observations can be retrieved in one call.
        /// </summary>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="userRoleId">In which role is the user retrieving species observations.</param>
        /// <returns>Species observations.</returns>
        public static WebSpeciesObservationInformation GetSpeciesObservationsById(List<Int64> speciesObservationIds,
                                                                                  Int32 userRoleId)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(20).GetSpeciesObservationsById(_clientToken,
                                                                       speciesObservationIds,
                                                                       userRoleId);
            }
        }

        /// <summary>
        /// Get information about taxa.
        /// </summary>
        /// <param name="taxonIds">Ids for taxa to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxa information.</returns>
        public static List<WebTaxon> GetTaxaById(List<Int32> taxonIds,
                                                 TaxonInformationType taxonInformationType)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxaById(_clientToken, taxonIds, taxonInformationType);
            }
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <returns>Status for this web service.</returns>       
        public static List<WebResourceStatus> GetStatus()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(1).GetStatus(_clientToken);
            }
        }

        /// <summary>
        /// Get all taxa utelizing a certain host taxon and any of its child taxa.
        /// The method is restricted to factors of type substrate.
        /// </summary>
        /// <param name="hostTaxonId">Id of host taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>List of host taxa.</returns>
        public static List<WebTaxon> GetTaxaByHostTaxonId(Int32 hostTaxonId,
                                                          TaxonInformationType taxonInformationType)
        {
            
            using (WebServiceClient client = new WebServiceClient())
            {
               return client.GetClient(20).GetTaxaByHostTaxonId(_clientToken, hostTaxonId, taxonInformationType);
            }
            
        }

        /// <summary>
        /// Get information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="hasOrganismGroupId">Indicates if organism group id is set.</param>
        /// <param name="organismGroupId">Organism group id.</param>
        /// <param name="hasEndangeredListId">Indicates if endangered list id is set.</param>
        /// <param name="endangeredListId">Endangered list id.</param>
        /// <param name="hasRedlistCategoryId">Indicates if redlist category id is set.</param>
        /// <param name="redlistCategoryId">Redlist category id.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        public static List<WebTaxon> GetTaxaByOrganismOrRedlist(Boolean hasOrganismGroupId,
                                                                Int32 organismGroupId,
                                                                Boolean hasEndangeredListId,
                                                                Int32 endangeredListId,
                                                                Boolean hasRedlistCategoryId,
                                                                Int32 redlistCategoryId,
                                                                TaxonInformationType taxonInformationType)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(4).GetTaxaByOrganismOrRedlist(_clientToken,
                                                                      hasOrganismGroupId,
                                                                      organismGroupId,
                                                                      hasEndangeredListId,
                                                                      endangeredListId,
                                                                      hasRedlistCategoryId,
                                                                      redlistCategoryId,
                                                                      taxonInformationType);
            }
        }

        /// <summary>
        /// Get information about taxa that matches the query.
        /// </summary>
        /// <param name="dataQuery">Data query.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if query is null.</exception>
        public static List<WebTaxon> GetTaxaByQuery(WebDataQuery dataQuery,
                                                TaxonInformationType taxonInformationType)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxaByQuery(_clientToken, dataQuery, taxonInformationType);
            }
        }

        /// <summary>
        /// Get information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The taxon search criteria.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if taxonSearchCriteria is null.</exception>
        public static List<WebTaxon> GetTaxaBySearchCriteria(WebTaxonSearchCriteria searchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxaBySearchCriteria(_clientToken, searchCriteria);
            }
        }

        /// <summary>
        /// Get all taxa for the species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="speciesObservationSearchCriteria">The species observation search criteria.</param>
        /// <returns>Taxa information.</returns>
        public static List<WebTaxon> GetTaxaBySpeciesObservations(WebSpeciesObservationSearchCriteria speciesObservationSearchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(20).GetTaxaBySpeciesObservations(_clientToken,
                                                                         speciesObservationSearchCriteria);
            }
        }

        /// <summary>
        /// Get number of unique taxa for species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="speciesObservationSearchCriteria">The species observation search criteria.</param>
        /// <returns>Number of unique taxa for species observations that matches the search criteria.</returns>
        public static Int32 GetTaxaCountBySpeciesObservations(WebSpeciesObservationSearchCriteria speciesObservationSearchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(20).GetTaxaCountBySpeciesObservations(_clientToken,
                                                                              speciesObservationSearchCriteria);
            }
        }

        /// <summary>
        /// Get information about a taxon.
        /// </summary>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxon information.</returns>
        public static WebTaxon GetTaxon(Int32 taxonId,
                                        TaxonInformationType taxonInformationType)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxonById(_clientToken, taxonId, taxonInformationType);
            }
        }

        /// <summary>
        /// Get information about occurence in swedish
        /// counties for specified taxon.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Information about occurence in swedish counties for specified taxon.</returns>
        public static List<WebTaxonCountyOccurrence> GetTaxonCountyOccurence(Int32 taxonId)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxonCountyOccurence(_clientToken, taxonId);
            }
        }

        /// <summary>
        /// Get taxon names for specified taxon.
        /// </summary>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>Taxon names.</returns>
        public static List<WebTaxonName> GetTaxonNames(Int32 taxonId)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxonNames(_clientToken, taxonId);
            }
        }

        /// <summary>
        /// Get taxon names that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The taxon name search criteria.</param>
        /// <returns>Taxon names.</returns>
        public static List<WebTaxonName> GetTaxonNamesBySearchCriteria(WebTaxonNameSearchCriteria searchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxonNamesBySearchCriteria(_clientToken, searchCriteria);
            }
        }

        /// <summary>
        /// Get information about all taxon name types.
        /// </summary>
        /// <returns>Taxon name types.</returns>
        public static List<WebTaxonNameType> GetTaxonNameTypes()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxonNameTypes(_clientToken);
            }
        }

        /// <summary>
        /// Get information about all taxon name use types.
        /// </summary>
        /// <returns>Taxon name use types.</returns>
        public static List<WebTaxonNameUseType> GetTaxonNameUseTypes()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxonNameUseTypes(_clientToken);
            }
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// </summary>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        public static List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(WebTaxonTreeSearchCriteria searchCriteria)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(20).GetTaxonTreesBySearchCriteria(_clientToken, searchCriteria);
            }
        }

        /// <summary>
        /// Get information about all taxon types.
        /// </summary>
        /// <returns>Taxon types.</returns>
        public static List<WebTaxonType> GetTaxonTypes()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetTaxonTypes(_clientToken);
            }
        }

        /// <summary>
        /// Get information about current web service user.
        /// </summary>
        /// <returns>Returns user information.</returns>
        public static WebUser GetUser()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                return client.GetClient(2).GetUser(_clientToken);
            }
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>True if user was logged in.</returns>       
        public static Boolean Login(String userName,
                                    String password,
                                    String applicationIdentifier,
                                    Boolean isActivationRequired)
        {
            String clientToken;

            WebServiceProxy.UserService.LoadSoaWebServiceAddresses(userName,
                                                                   password,
                                                                   applicationIdentifier,
                                                                   isActivationRequired);

            using (WebServiceClient client = new WebServiceClient())
            {
                clientToken = client.GetClient(2).Login(userName, password, applicationIdentifier, isActivationRequired);
            }
            lock (_clients)
            {
                _clientToken = clientToken;
            }
            return clientToken.IsNotEmpty();
        }

        /// <summary>
        /// Logout user from web service and close connection.
        /// </summary>
        public static void Logout()
        {
            try
            {
                if (_clientToken.IsNotEmpty())
                {
                    using (WebServiceClient client = new WebServiceClient())
                    {
                        client.GetClient(1).Logout(_clientToken);
                    }
                }
            }
            catch
            {
                // No need to handle errors.
                // Logout is only used to relase
                // resources in the web service.
            }
            finally
            {
                ReleaseClients();
            }
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public static Boolean Ping()
        {
            try
            {
                using (WebServiceClient client = new WebServiceClient())
                {
                    return client.GetClient(0, 10).Ping();
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get a web service client from the client pool.
        /// </summary>
        /// <param name="operationTimeout">Operation timeout to set in the client. Unit is seconds.</param>
        /// <returns>A web service client.</returns>
        private ArtDatabankenServiceClient PopClient(Int32 operationTimeout)
        {
            ArtDatabankenServiceClient client;
            List<ArtDatabankenServiceClient> clients;

            client = null;
            lock (_clients)
            {
                if (_clients.ContainsKey(operationTimeout))
                {
                    clients = (List<ArtDatabankenServiceClient>)(_clients[operationTimeout]);
                    if (clients.IsNotEmpty())
                    {
                        // Take one client from the client pool.
                        client = clients[clients.Count - 1];
                        clients.RemoveAt(clients.Count - 1);
                    }
                }
            }
            if (client.IsNull())
            {
                // Create new client.
                client = GetClient();
            }
            return client;
        }

        /// <summary>
        /// Add a web service client to the client pool.
        /// </summary>
        /// <param name="client">Web service client.</param>
        /// <returns>True if client was added to the client pool.</returns>
        private Boolean PushClient(ArtDatabankenServiceClient client)
        {
            Boolean isClientAddedToPool;
            Int32 operationTimeout;
            List<ArtDatabankenServiceClient> clients;

            isClientAddedToPool = false;
            operationTimeout = (Int32)(client.Endpoint.Binding.CloseTimeout.TotalSeconds);
            lock (_clients)
            {
                if (_clients.ContainsKey(operationTimeout))
                {
                    clients = (List<ArtDatabankenServiceClient>)(_clients[operationTimeout]);
                }
                else
                {
                    clients = new List<ArtDatabankenServiceClient>();
                    _clients[operationTimeout] = clients;
                }
                if (clients.Count < 10)
                {
                    // Save client instance
                    clients.Add(client);
                    isClientAddedToPool = true;
                }
                // else: Don't save to many client instances.
            }
            return isClientAddedToPool;
        }

        /// <summary>
        /// Release all cached client instances.
        /// </summary>
        private static void ReleaseClients()
        {
            List<ArtDatabankenServiceClient> clients;

            lock (_clients)
            {
                foreach (Int32 key in _clients.Keys)
                {
                    clients = (List<ArtDatabankenServiceClient>)(_clients[key]);
                    foreach (ArtDatabankenServiceClient client in clients)
                    {
                        client.Close();
                    }
                    clients.Clear();
                }
                _clientToken = null;
            }
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        public static void RollbackTransaction()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(1).RollbackTransaction(_clientToken);
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="userName">User name.</param>
        public static void StartTrace(String userName)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(1).StartTrace(_clientToken, userName);
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public static void StartTransaction(Int32 timeout)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(1).StartTransaction(_clientToken, timeout);
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        [OperationContract]
        public static void StopTrace()
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(1).StopTrace(_clientToken);
            }
        }

        /// <summary>
        /// Update existing reference with specific id.
        /// </summary>
        /// <param name="reference">Existing reference with specific id.</param>
        public static void UpdateReference(WebReference reference)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(10).UpdateReference(_clientToken, reference);
            }
        }

        /// <summary>
        /// Update species facts.
        /// </summary>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        public static void UpdateSpeciesFacts(List<WebSpeciesFact> createSpeciesFacts,
                                              List<WebSpeciesFact> deleteSpeciesFacts,
                                              List<WebSpeciesFact> updateSpeciesFacts)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(10).UpdateSpeciesFacts(_clientToken, createSpeciesFacts, deleteSpeciesFacts, updateSpeciesFacts);
            }
        }

        /// <summary>
        /// Update species facts. This method should only be used by Dyntaxa web application.
        /// </summary>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        /// <param name="fullName">Full Name of editor.</param>
        public static void UpdateDyntaxaSpeciesFacts(List<WebSpeciesFact> createSpeciesFacts,
                                              List<WebSpeciesFact> deleteSpeciesFacts,
                                              List<WebSpeciesFact> updateSpeciesFacts,
                                              String fullName)
        {
            using (WebServiceClient client = new WebServiceClient())
            {
                client.GetClient(10).UpdateDyntaxaSpeciesFacts(_clientToken, createSpeciesFacts, deleteSpeciesFacts, updateSpeciesFacts, fullName);
            }
        }
    }
}

