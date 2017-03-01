using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Test
{
    /// <summary>
    /// Class that handles information related to 
    /// the web service that this project is included into.
    /// </summary>
    public class WebServiceManager : IWebServiceManager
    {
        /// <summary>
        /// Encryption key that is used in production.
        /// </summary>
        public String Key
        {
            get
            {
                return "SecretKey";
            }
        }

        /// <summary>
        /// Web service user name in test.
        /// </summary>
        public String Name
        {
            get
            {
                return "TestService";
            }
        }

        /// <summary>
        /// Web service password in test.
        /// </summary>
        public String Password
        {
            get
            {
                return "No password";
            }
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <returns>Status for this web service.</returns>       
        public Dictionary<Int32, List<WebResourceStatus>> GetStatus()
        {
            Dictionary<Int32, List<WebResourceStatus>> status;

            status = new Dictionary<Int32, List<WebResourceStatus>>();
            status[(Int32)(LocaleId.en_GB)] = new List<WebResourceStatus>();
            status[(Int32)(LocaleId.sv_SE)] = new List<WebResourceStatus>();
            return status;
        }
    }
}
