using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a Role member
    /// </summary>
    [DataContract]
    public class WebRoleMember : WebData
    {
        /// <summary>
        /// Role 
        /// </summary>
        [DataMember]
        public WebRole Role
        { get; set; }

        /// <summary>
        /// User connected with this role
        /// </summary>
        [DataMember]
        public WebUser User
        { get; set; }

        /// <summary>
        /// Flag indicating if user has activated the role.
        /// </summary>
        [DataMember]
        public Boolean IsActivated
        { get; set; }
    }
}
