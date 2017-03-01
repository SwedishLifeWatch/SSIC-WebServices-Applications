using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class with information about a taxon revision event.
    /// </summary>
    [DataContract]
    public class WebTaxonRevisionEvent : WebData
    {
        /// <summary>
        /// Information about which taxa that are affected.
        /// </summary>
        [DataMember]
        public String AffectedTaxa { get; set; }

        /// <summary>
        /// Id for user who created this taxon revision event.
        /// Set by database when this taxon revision event was created.
        /// </summary>
        [DataMember]
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date when this taxon revision event was created.
        /// Set by database when this taxon revision event was created.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Id for the taxon revision event.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Information about the new value.
        /// </summary>
        [DataMember]
        public String NewValue { get; set; }

        /// <summary>
        /// Information about the old value.
        /// May be empty.
        /// </summary>
        [DataMember]
        public String OldValue { get; set; }

        /// <summary>
        /// Id of the revision that this
        /// taxon revision event belongs to.
        /// </summary>
        [DataMember]
        public Int32 RevisionId { get; set; }

        /// <summary>
        /// Id for the taxon revision event type.
        /// </summary>
        [DataMember]
        public Int32 TypeId { get; set; }
    }
}
