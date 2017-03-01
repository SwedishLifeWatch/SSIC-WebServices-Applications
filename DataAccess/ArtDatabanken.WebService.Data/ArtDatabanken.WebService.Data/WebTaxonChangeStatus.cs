using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class with information about a taxon change status.
    /// </summary>
    [DataContract]
    public class WebTaxonChangeStatus : WebData
    {
        /// <summary>
        /// Information about the taxon change status.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for the taxon change status.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for the taxon change status.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }
    }
}
