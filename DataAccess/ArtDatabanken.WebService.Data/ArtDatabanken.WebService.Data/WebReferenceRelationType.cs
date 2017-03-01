using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains definitions of reference relation types.
    /// </summary>
    [DataContract]
    public class WebReferenceRelationType : WebData
    {
        /// <summary>
        /// Information about the reference relation type.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id of the reference relation type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this data type.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }
    }
}
