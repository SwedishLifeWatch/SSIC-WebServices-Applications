using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class with information about a lump split event type.
    /// </summary>
    [DataContract]
    public class WebLumpSplitEventType : WebData
    {
        /// <summary>
        /// Information about the lump split event type.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for the lump split event type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for the lump split event type.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }
    }
}
