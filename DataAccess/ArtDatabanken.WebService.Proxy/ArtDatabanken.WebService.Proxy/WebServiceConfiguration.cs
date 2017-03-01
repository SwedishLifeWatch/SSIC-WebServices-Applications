using System;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class used to hold configuration
    /// information related to web services.
    /// This information may be used by all web services.
    /// </summary>
    public class WebServiceConfiguration
    {
        /// <summary>
        /// A URI that specifies the address of the HTTP proxy.
        /// </summary>
        public Uri HttpProxyAddress
        { get; set; }
    }
}
