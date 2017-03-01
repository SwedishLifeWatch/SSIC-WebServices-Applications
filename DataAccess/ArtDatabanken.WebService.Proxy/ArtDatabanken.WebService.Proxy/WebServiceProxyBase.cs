using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Base class for all web service proxies.
    /// </summary>
    public abstract class WebServiceProxyBase
    {
        /// <summary>
        /// The _clients.
        /// </summary>
        private readonly Hashtable _clients;

        /// <summary>
        /// The _endpoint address.
        /// </summary>
        private EndpointAddress _endpointAddress;

        /// <summary>
        /// The _web service name.
        /// </summary>
        private String _webServiceName;

        /// <summary>
        /// Create a WebServiceProxyBase instance.
        /// </summary>
        protected WebServiceProxyBase()
        {            
            _clients = new Hashtable();
            _endpointAddress = null;
            HttpProxyAddress = null;
            _webServiceName = null;
            MaxBufferPoolSize = 2000000000;

            switch (Configuration.InstallationType)
            {
                case InstallationType.LocalTest:
                    InternetProtocol = InternetProtocol.Http;
                    WebServiceProtocol = WebServiceProtocol.SOAP11;
                    break;

                case InstallationType.ArtportalenTest:
                case InstallationType.Production:
                case InstallationType.ServerTest:
                case InstallationType.SpeciesFactTest:
                case InstallationType.SystemTest:
                case InstallationType.TwoBlueberriesTest:
                    InternetProtocol = InternetProtocol.Https;
                    WebServiceProtocol = WebServiceProtocol.Binary;
                    break;

                default:
                    throw new ApplicationException("Not handled installation type " + Configuration.InstallationType);
            }
        }

        /// <summary>
        /// A URI that specifies the address of the HTTP proxy.
        /// </summary>
        public Uri HttpProxyAddress { get; set; }

        /// <summary>
        /// Information about which Internet protocol that are used.
        /// </summary>
        public InternetProtocol InternetProtocol { get; set; }

        /// <summary>
        /// Information about which computer that hosts the web service.
        /// </summary>
        public WebServiceComputer WebServiceComputer { get; set; }

        /// <summary>
        /// Information about which web service protocol that are used.
        /// </summary>
        public WebServiceProtocol WebServiceProtocol { get; set; }

        /// <summary>
        /// Size of MaxBufferPoolSize.
        /// If 0 a GCBufferManager gets created and later garbage collection; 
        /// otherwise you get a PooledBufferManager when TransferMode=Buffered.
        /// http://obsessivelycurious.blogspot.ru/2008/04/wcf-memory-buffer-management.html
        /// </summary>
        public Int64 MaxBufferPoolSize { get; set; }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected abstract void CloseClient(Object client);

        /// <summary>
        /// Close all clients (connections) to web service.
        /// </summary>
        public void CloseClients()
        {
            List<Object> clients;

            lock (_clients)
            {
                foreach (Int32 key in _clients.Keys)
                {
                    clients = (List<Object>)(_clients[key]);
                    if (clients.IsNotEmpty())
                    {
                        foreach (object client in clients)
                        {
                            CloseClient(client);
                        }
                    }
                }

                _clients.Clear();
            }
        }

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected abstract Object CreateClient();

        /// <summary>
        /// Get binding.
        /// </summary>
        /// <returns>The Binding.</returns>       
        protected Binding GetBinding()
        {
            Binding binding;

            switch (WebServiceProtocol)
            {
                case WebServiceProtocol.Binary:
                    binding = GetBinaryBinding();
                    break;
                case WebServiceProtocol.SOAP11:
                    binding = GetSoap11Binding();
                    break;
                case WebServiceProtocol.SOAP12:
                    binding = GetSoap12Binding();
                    break;
                default:
                    throw new Exception("Unknown WebServiceProtocol: " + WebServiceProtocol);
            }

            return binding;
        }

        /// <summary>
        /// Get fast binding.
        /// </summary>
        /// <returns>Fast binding.</returns>       
        private Binding GetBinaryBinding()
        {
            BinaryMessageEncodingBindingElement messageEncoding;
            HttpsTransportBindingElement httpsTransport;
            HttpTransportBindingElement httpTransport;

            messageEncoding = new BinaryMessageEncodingBindingElement();
            messageEncoding.MaxReadPoolSize = 2000000000;
            messageEncoding.MaxWritePoolSize = 2000000000;
            messageEncoding.ReaderQuotas.MaxDepth = 2000000000;
            messageEncoding.ReaderQuotas.MaxStringContentLength = 2000000000;
            messageEncoding.ReaderQuotas.MaxArrayLength = 2000000000;
            messageEncoding.ReaderQuotas.MaxBytesPerRead = 2000000000;
            messageEncoding.ReaderQuotas.MaxNameTableCharCount = 2000000000;

            switch (InternetProtocol)
            {
                case InternetProtocol.Http:
                    httpTransport = new HttpTransportBindingElement();
                    break;

                case InternetProtocol.Https:
                    httpsTransport = new HttpsTransportBindingElement();
                    httpsTransport.RequireClientCertificate = false;
                    httpTransport = httpsTransport;
                    break;

                default:
                    throw new ApplicationException("Not supported Internet protocol: " + InternetProtocol);
            }

            httpTransport.ManualAddressing = false;            
            httpTransport.MaxBufferPoolSize = MaxBufferPoolSize;            
            httpTransport.MaxReceivedMessageSize = 2000000000;
            httpTransport.AllowCookies = false;
            httpTransport.AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous;
            httpTransport.BypassProxyOnLocal = false;
            httpTransport.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            httpTransport.KeepAliveEnabled = true;
            httpTransport.MaxBufferSize = 2000000000;
            httpTransport.ProxyAddress = GetHttpProxyAddress();
            httpTransport.ProxyAuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous;
            httpTransport.Realm = String.Empty;
            httpTransport.TransferMode = TransferMode.Buffered;
            httpTransport.UnsafeConnectionNtlmAuthentication = false;
            httpTransport.UseDefaultWebProxy = GetHttpProxyAddress().IsNull();

            return new CustomBinding(messageEncoding, httpTransport);
        }

        /// <summary>
        /// Get address of currently used endpoint.
        /// </summary>
        /// <returns>Address of currently used endpoint.</returns>
        public virtual EndpointAddress GetEndpointAddress()
        {
            if (_endpointAddress.IsNull())
            {
                _endpointAddress = new EndpointAddress(GetWebAddress());
            }

            return _endpointAddress;
        }

        /// <summary>
        /// Get http proxy address or null if no address has been set.
        /// </summary>
        /// <returns>Http proxy address or null if no address has been set.</returns>       
        private Uri GetHttpProxyAddress()
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
        /// Get binding.
        /// </summary>
        /// <returns>The Binding.</returns>       
        private Binding GetSoap11Binding()
        {
            BasicHttpBinding basicHttpBinding;

            basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.BypassProxyOnLocal = false;
            basicHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            basicHttpBinding.MaxBufferPoolSize = MaxBufferPoolSize;
            basicHttpBinding.MaxReceivedMessageSize = 2000000000;
            basicHttpBinding.MessageEncoding = WSMessageEncoding.Text;
            basicHttpBinding.OpenTimeout = new TimeSpan(0, 20, 0);
            basicHttpBinding.ProxyAddress = GetHttpProxyAddress();
            basicHttpBinding.ReaderQuotas.MaxArrayLength = 2000000000;
            basicHttpBinding.ReaderQuotas.MaxBytesPerRead = 2000000000;
            basicHttpBinding.ReaderQuotas.MaxDepth = 2000000000;
            basicHttpBinding.ReaderQuotas.MaxNameTableCharCount = 2000000000;
            basicHttpBinding.ReaderQuotas.MaxStringContentLength = 2000000000;
            basicHttpBinding.TextEncoding = Encoding.UTF8;
            basicHttpBinding.UseDefaultWebProxy = GetHttpProxyAddress().IsNull();

            switch (InternetProtocol)
            {
                case InternetProtocol.Http:
                    basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
                    break;

                case InternetProtocol.Https:
                    basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                    basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    basicHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                    break;

                default:
                    throw new ApplicationException("Not supported Internet protocol: " + InternetProtocol);
            }

            return basicHttpBinding;
        }

        /// <summary>
        /// Get binding.
        /// </summary>
        /// <returns>The Binding.</returns>       
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private Binding GetSoap12Binding()
        {
            WSHttpBinding wsHttpBinding;

            wsHttpBinding = new WSHttpBinding();
            wsHttpBinding.BypassProxyOnLocal = false;
            wsHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            wsHttpBinding.MaxBufferPoolSize = MaxBufferPoolSize;
            wsHttpBinding.MaxReceivedMessageSize = 2000000000;
            wsHttpBinding.MessageEncoding = WSMessageEncoding.Mtom;
            wsHttpBinding.OpenTimeout = new TimeSpan(0, 20, 0);
            wsHttpBinding.ProxyAddress = GetHttpProxyAddress();
            wsHttpBinding.ReaderQuotas.MaxArrayLength = 2000000000;
            wsHttpBinding.ReaderQuotas.MaxBytesPerRead = 2000000000;
            wsHttpBinding.ReaderQuotas.MaxDepth = 2000000000;
            wsHttpBinding.ReaderQuotas.MaxNameTableCharCount = 2000000000;
            wsHttpBinding.ReaderQuotas.MaxStringContentLength = 2000000000;
            wsHttpBinding.TextEncoding = Encoding.UTF8;
            wsHttpBinding.TransactionFlow = false;
            wsHttpBinding.UseDefaultWebProxy = GetHttpProxyAddress().IsNull();
            switch (InternetProtocol)
            {
                case InternetProtocol.Http:
                    wsHttpBinding.Security.Mode = SecurityMode.None;
                    break;

                case InternetProtocol.Https:
                    wsHttpBinding.Security.Mode = SecurityMode.Transport;
                    wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    wsHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                    break;

                default:
                    throw new ApplicationException("Not supported Internet protocol: " + InternetProtocol);
            }

            return wsHttpBinding;
        }

        /// <summary>
        /// Get web address of currently used endpoint.
        /// </summary>
        /// <returns>Web address of currently used endpoint.</returns>
        public virtual String GetWebAddress()
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
                    break;
                case WebServiceProtocol.SOAP12:
                    webAddress += @"/SOAP12";
                    break;
                default:
                    throw new Exception("Unknown WebServiceProtocol: " + WebServiceProtocol);
            }

            return webAddress;
        }

        /// <summary>
        /// Get address of currently used web service.
        /// This address does not contain information about
        /// protocol or end point.
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected abstract String GetWebServiceAddress();

        /// <summary>
        /// Get name of currently used web service.
        /// </summary>
        /// <returns>Name of currently used web service.</returns>
        public String GetWebServiceName()
        {
            if (_webServiceName.IsNull())
            {
                _webServiceName = GetEndpointAddress().ToString();
                if (_webServiceName.Contains(".asmx"))
                {
                    _webServiceName = _webServiceName.Substring(0, _webServiceName.IndexOf(".asmx", StringComparison.Ordinal));
                }

                if (_webServiceName.Contains(".php"))
                {
                    _webServiceName = _webServiceName.Substring(0, _webServiceName.IndexOf(".php", StringComparison.Ordinal));
                }

                if (_webServiceName.Contains(".svc"))
                {
                    _webServiceName = _webServiceName.Substring(0, _webServiceName.IndexOf(".svc", StringComparison.Ordinal));
                }

                _webServiceName = _webServiceName.Substring(_webServiceName.LastIndexOf(@"/", StringComparison.Ordinal) + 1);
            }

            return _webServiceName;
        }

        /// <summary>
        /// Increase data size for method that
        /// sends or receives a lot of data.
        /// </summary>
        /// <param name="method">Method name.</param>
        /// <param name="endpoint">The Endpoint.</param>
        protected void IncreaseDataSize(String method,
                                        ServiceEndpoint endpoint)
        {
            OperationDescription operation;

            operation = endpoint.Contract.Operations.Find(method);
            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = 2000000000;
        }

        /// <summary>
        /// Get a web service client from the client pool.
        /// </summary>
        /// <param name="operationTimeout">Operation timeout to set in the client. Unit is seconds.</param>
        /// <returns>A web service client.</returns>
        protected Object PopClient(Int32 operationTimeout)
        {
            Object client;
            List<Object> clients;
            
            client = null;
            lock (_clients)
            {
                if (_clients.ContainsKey(operationTimeout))
                {
                    clients = (List<Object>)(_clients[operationTimeout]);
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
                client = CreateClient();
            }

            return client;
        }

        /// <summary>
        /// Add a web service client to the client pool.
        /// </summary>
        /// <param name="client">Web service client.</param>
        /// <param name="operationTimeout">Operation timeout to set in the client. Unit is seconds.</param>
        /// <returns>True if client was added to the client pool.</returns>
        protected Boolean PushClient(Object client,
                                     Int32 operationTimeout)
        {
            Boolean isClientAddedToPool;
            List<Object> clients;

            isClientAddedToPool = false;
            lock (_clients)
            {
                if (_clients.ContainsKey(operationTimeout))
                {
                    clients = (List<Object>)(_clients[operationTimeout]);
                }
                else
                {
                    clients = new List<Object>();
                    _clients[operationTimeout] = clients;
                }

                if (clients.Count < Settings.Default.MaxOpenWebServiceConnections)
                {
                    // Save client instance
                    clients.Add(client);
                    isClientAddedToPool = true;
                }
                else
                {
                    // Don't save to many client instances.
                    CloseClient(client);
                }
            }

            return isClientAddedToPool;
        }

        /// <summary>
        /// Set timeout value in web service connection.
        /// </summary>
        /// <param name="binding">Binding in web service connection.</param>
        /// <param name="operationTimeout">Operation timeout to set in the client. Unit is seconds.</param>
        public void SetTimeout(Binding binding,
                               Int32 operationTimeout)
        {
            TimeSpan timeSpan;

            timeSpan = new TimeSpan(0, operationTimeout / 60, operationTimeout % 60);
            binding.CloseTimeout = timeSpan;
            binding.OpenTimeout = timeSpan;
            binding.ReceiveTimeout = timeSpan;
            binding.SendTimeout = timeSpan;
        }
    }
}
