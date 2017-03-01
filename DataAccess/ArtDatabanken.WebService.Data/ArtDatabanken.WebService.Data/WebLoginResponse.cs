using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class contains the response to a
    ///  login call to a web service.
    /// </summary>
    [DataContract]
    public class WebLoginResponse : WebData
    {
        /// <summary>
        /// Locale used when user authority was retrieved.
        /// </summary>
        [DataMember]
        public WebLocale Locale
        { get; set; }

        /// <summary>
        /// User authority for the application that was
        /// specified in the login.
        /// </summary>
        [DataMember]
        public List<WebRole> Roles
        { get; set; }

        /// <summary>
        /// Token, that must be used in all requestes to  
        /// the same web service as the login was made to.
        /// </summary>
        [DataMember]
        public String Token
        { get; set; }

        /// <summary>
        /// Information about the user that has logged in.
        /// </summary>
        [DataMember]
        public WebUser User
        { get; set; }
    }
}
