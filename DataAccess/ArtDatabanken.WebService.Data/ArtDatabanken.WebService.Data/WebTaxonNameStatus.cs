using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Information about status of taxon names,
    /// (approved, tentatively, invalid, etc.).
    /// </summary>
    [DataContract]
    public class WebTaxonNameStatus : WebData
    {
        /// <summary>
        /// Description of the taxon name status.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id of taxon name status.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name of the taxon name status.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}
