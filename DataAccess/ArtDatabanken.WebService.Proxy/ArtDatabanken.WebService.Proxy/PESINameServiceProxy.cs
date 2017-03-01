using System;
using System.Net.Mail;
using System.ServiceModel;
using ArtDatabanken.WebService.Proxy.PESINameService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that retrieves taxon information from PESI
    /// (Pan-European Species directories Infrastructure).
    /// </summary>
    public class PESINameServiceProxy : WebServiceProxyBase
    {
        /// <summary>
        /// Create a PESINameServiceProxy instance.
        /// </summary>
        public PESINameServiceProxy()
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
                ((ClientBase<PESINameServicePortType>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<PESINameServicePortType>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Get PESI GUID by vernacular name.
        /// </summary>
        /// <param name="vernacularName">Vernacular name.</param>
        /// <returns>PESI GUID by vernacular name.</returns>       
        public String GetPesiGuidByVernacularName(String vernacularName)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                PESIRecord[] pesiRecords = client.Client.getPESIRecordsByVernacular(vernacularName);
                if (pesiRecords.IsEmpty())
                {
                    return String.Empty;
                }
                else
                {
                    return pesiRecords[0].GUID;
                }
            }
        }

        /// <summary>
        /// Get PESI GUID by scientific name.
        /// </summary>
        /// <param name="scientificName">Information about the client that makes this web service call.</param>
        /// <returns>PESI GUID by scientific name.</returns>       
        public String GetPesiGuidByScientificName(String scientificName)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                const Boolean searchLike = false;
                try
                {
                    PESIRecord[] pesiRecords = client.Client.getPESIRecords(scientificName, searchLike);
                    if (pesiRecords.IsNotNull() && pesiRecords.Length > 0)
                    {
                        return pesiRecords[0].GUID; 
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                catch (Exception ex)
                {
                    // Send email alarm
                    sendEmail(scientificName, ex);
                    // PESI call failed - return empty string.
                    return String.Empty;
                }

            }
        }        

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected override object CreateClient()
        {
            var client = new PESINameServicePortTypeClient(GetBinding(),
                                                GetEndpointAddress());
            return client;
        }

        /// <summary>
        /// Get address of currently used web service.
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected override string GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                WebServiceAddress = @"www.eu-nomen.eu/portal/soap.php";
            }
            return WebServiceAddress;
        }

        /// <summary>
        /// Send alarm email about PESI WS not working
        /// </summary>
        private void sendEmail(string scientificName, Exception exception)
        {
            string body = "Failed to get name from PESI.";
            body += Environment.NewLine + "Scientific name input: " + (scientificName ?? "[null]");
            body += Environment.NewLine + "Exception message: " + exception.Message;
            body += Environment.NewLine + "Stack trace: " + Environment.NewLine;
            body += exception.StackTrace;

            /* create the email message */
            MailMessage message = new MailMessage(
                Settings.Default.PesiNameServiceEmailSender, 
                Settings.Default.PesiNameServiceEmailRecipient,
                "PESI Web Service not working", 
                body);
            message.IsBodyHtml = false;
            SmtpClient smtpClient = new SmtpClient("smtp.slu.se");
            smtpClient.Send(message);
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private PESINameServicePortTypeClient _client;
            private readonly PESINameServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(PESINameServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (PESINameServicePortTypeClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>
            public PESINameServicePortTypeClient Client
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
