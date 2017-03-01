using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Information about a species activity category.
    /// For example "Ensured reproduction"
    /// </summary>
    [DataContract]
    public class WebSpeciesActivityCategory : WebData
    {
        /// <summary>
        /// GUID for this species activity.
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// Id for this species activity category.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier for this species activity.
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }

        /// <summary>
        /// Name for this species activity category.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
