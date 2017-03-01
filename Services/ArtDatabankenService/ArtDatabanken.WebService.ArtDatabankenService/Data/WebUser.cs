using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a system user.
    /// </summary>
    [DataContract]
    public class WebUser : WebData
    {
        private String _fullName; // Not used dummy value.

        /// <summary>
        /// First name of the user.
        /// </summary>
        [DataMember]
        public String FirstName
        { get; set; }

        /// <summary>
        /// Full name of the user.
        /// </summary>
        [DataMember]
        public String FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { _fullName = value; }
        }

        /// <summary>
        /// Id for this user.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        [DataMember]
        public String LastName
        { get; set; }

        /// <summary>
        /// All user roles that this user belongs to.
        /// </summary>
        [DataMember]
        public List<WebUserRole> Roles
        { get; set; }

        /// <summary>
        /// Name used when accessing the web service.
        /// </summary>
        [DataMember]
        public String UserName
        { get; set; }
    }
}
