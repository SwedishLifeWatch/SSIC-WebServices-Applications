using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class represents a taxon name type.
    /// In TaxonService this information has the name "taxon name category".
    /// </summary>
    [DataContract]
    public class WebTaxonNameType : WebData
    {
        /// <summary>
        /// Id for this taxon name type.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this taxon name type.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Sort order for this taxon name type.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }
    }
}
