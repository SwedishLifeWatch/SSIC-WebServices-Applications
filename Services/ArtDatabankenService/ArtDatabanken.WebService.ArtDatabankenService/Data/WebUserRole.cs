using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    ///  This class represents a user role.
    /// </summary>
    [DataContract]
    public class WebUserRole : WebData
    {
        /// <summary>
        /// Description of the user role.
        /// </summary>
        [DataMember]
        public String Description
        { get; set; }

        /// <summary>
        /// User role id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// User role name.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
