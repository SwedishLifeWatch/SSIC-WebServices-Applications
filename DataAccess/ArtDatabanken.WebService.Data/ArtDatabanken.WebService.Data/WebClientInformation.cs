using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a client of a web service.
    /// </summary>
    [DataContract]
    public class WebClientInformation : WebData
    {
        /// <summary>
        /// Current locale specified by user.
        /// This locale is used when information that is 
        /// language handled are retrieved from a web service.
        /// </summary>
        [DataMember]
        public WebLocale Locale { get; set; }

        /// <summary>
        /// Property Role is used to inform web service in which
        /// role the user is making the web service call.
        /// The sum of user rights are used if no role is specified.
        /// </summary>
        [DataMember]
        public WebRole Role { get; set; }

        /// <summary>
        /// Security token that was returned in WebLoginResponse
        /// when the user logged in to the web service.
        /// The web service call will fail if this token is not provided.
        /// </summary>
        [DataMember]
        public String Token { get; set; }
    }
}
