using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class with information about a taxon name category type.
    /// </summary>
    [DataContract]
    public class WebTaxonNameCategoryType : WebData
    {
        /// <summary>
        /// Information about the taxon name category type.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id for the taxon name category type.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for the taxon name category type.
        /// </summary>
        [DataMember]
        public String Identifier { get; set; }
    }
}
