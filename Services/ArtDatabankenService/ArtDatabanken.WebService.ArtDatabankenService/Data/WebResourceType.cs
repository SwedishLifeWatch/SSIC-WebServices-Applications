using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Definition of possible types of resources.
    /// </summary>
    [DataContract]
    public class WebResourceType : WebData
    {
        /// <summary>
        /// Id for the resource type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Identifier for the resource type.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }

        /// <summary>
        /// Name of the resource type.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}
