using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains information about changes regarding taxon.
    /// </summary>
    [DataContract]
    public class WebTaxonChange : WebData
    {
        /// <summary>
        /// Recommended scientific name.
        /// </summary>
        [DataMember]
        public String ScientificName { get; set; }

        /// <summary>
        /// Category id for the taxon.
        /// </summary>
        [DataMember]
        public Int32 TaxonCategoryId { get; set; }

        /// <summary>
        /// TaxonId unique identification of a taxon.
        /// </summary>
        [DataMember]
        public Int32 TaxonId { get; set; }

        /// <summary>
        /// TaxonId for taxon involved in lump or split.
        /// Value is the id taxon got AFTER the lump/split event occurred.
        /// </summary>
        [DataMember]
        public Int32 TaxonIdAfter { get; set; }

        /// <summary>
        /// Id of the taxon revision event type that changed the taxon.
        /// </summary>
        [DataMember]
        public Int32 TaxonRevisionEventTypeId { get; set; }
    }
}
