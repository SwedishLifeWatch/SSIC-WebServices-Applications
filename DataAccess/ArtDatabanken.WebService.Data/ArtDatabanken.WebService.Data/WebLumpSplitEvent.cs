using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Information about a taxonomic change of an existing taxon.
    /// </summary>
    [DataContract]
    public class WebLumpSplitEvent : WebData
    {
        /// <summary>
        /// This lump split event was created
        /// or updated in this revision event.
        /// </summary>
        [DataMember]
        public Int32 ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Id of user that created the lump split event.
        /// Set by database when created.
        /// Mandatory ie always required.
        /// </summary> 
        [DataMember]
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// The lump split event was created at this date.
        /// Set by database when created.
        /// Mandatory ie always required.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Information about the change.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the
        /// record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// Mandatory ie always required.
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// Id of the lump split event.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if property ChangedInRevisionEventId has been set.
        /// </summary>
        [DataMember]
        public Boolean IsChangedInTaxonRevisionEventIdSpecified { get; set; }

        /// <summary>
        /// Indicates if this change has been published or not.
        /// </summary>
        [DataMember]
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Indicates if property ReplacedInRevisionEvent has been set.
        /// </summary>
        [DataMember]
        public Boolean IsReplacedInTaxonRevisionEventIdSpecified { get; set; }

        /// <summary>
        /// This lump split event was replaced in this revision event.
        /// </summary>
        [DataMember]
        public Int32 ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Changed taxon (specified in TaxonIdBefore) was replaced
        /// by taxon with this id (TaxonIdAfter).
        /// </summary>
        [DataMember]
        public Int32 TaxonIdAfter { get; set; }

        /// <summary>
        /// Id of taxon that was changed.
        /// </summary>
        [DataMember]
        public Int32 TaxonIdBefore { get; set; }

        /// <summary>
        /// Id of the lump split event type.
        /// </summary>
        [DataMember]
        public Int32 TypeId { get; set; }
    }
}
