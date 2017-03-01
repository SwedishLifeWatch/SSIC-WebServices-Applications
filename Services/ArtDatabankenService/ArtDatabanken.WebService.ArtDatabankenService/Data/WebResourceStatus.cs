using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class contains status information about some kind
    /// of resource that an application is dependent on.
    /// </summary>
    [DataContract]
    public class WebResourceStatus : WebData
    {
        /// <summary>
        /// Type of access that the application needs to the resource.
        /// For example read, write or execute.
        /// </summary>
        [DataMember]
        public String AccessType
        { get; set; }

        /// <summary>
        /// Address of resource.
        /// </summary>
        [DataMember]
        public String Address
        { get; set; }

        /// <summary>
        /// Further information about the status or resource.
        /// </summary>
        [DataMember]
        public String Information
        { get; set; }

        /// <summary>
        /// Name of resource.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Type of resource.
        /// </summary>
        [DataMember]
        public WebResourceType ResourceType
        { get; set; }

        /// <summary>
        /// Status: True = OK and False = Some kind of problem.
        /// </summary>
        [DataMember]
        public Boolean Status
        { get; set; }

        /// <summary>
        /// Date and time when this status information was created.
        /// </summary>
        [DataMember]
        public DateTime Time
        { get; set; }
    }
}
