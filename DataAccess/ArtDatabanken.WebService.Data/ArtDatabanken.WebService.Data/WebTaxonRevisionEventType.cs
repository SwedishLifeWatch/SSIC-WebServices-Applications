using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class with information about a taxon revision event type.
    /// </summary>
    [DataContract]
    public class WebTaxonRevisionEventType : WebData
    {
        /// <summary>
        /// Information about the taxon revision event type.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for the taxon revision event type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for the taxon revision event type.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }
    }
}
