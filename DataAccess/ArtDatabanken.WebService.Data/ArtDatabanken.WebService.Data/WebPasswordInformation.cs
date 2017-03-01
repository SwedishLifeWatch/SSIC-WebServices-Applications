using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information that is returned
    /// in response to a ResetPassword method call. 
    /// </summary>
    [DataContract]
    public class WebPasswordInformation : WebData
    {
        /// <summary>
        /// Email address.
        /// Password was changed for the user
        /// who has this email address.
        /// </summary>
        [DataMember]
        public String EmailAddress
        { get; set; }

        /// <summary>
        /// New password.
        /// </summary>
        [DataMember]
        public String Password
        { get; set; }

        /// <summary>
        /// User name for the user who has the specified email address.
        /// </summary>
        [DataMember]
        public String UserName
        { get; set; }
    }
}
