using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class with information about a taxon alert status.
    /// </summary>
    [DataContract]
    public class WebTaxonAlertStatus : WebData
    {
        /// <summary>
        /// Information about the taxon alert status.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for the taxon alert status.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for the taxon alert status.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }
    }
}
