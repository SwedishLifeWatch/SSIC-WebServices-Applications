using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a taxon name.
    /// 
    /// The following dynamic properties exists: 
    /// NameUsageId (Int32), Version (Int32), ModifiedByPerson (String)
    /// 
    /// </summary>
    [DataContract]
    public class WebTaxonName : WebData
    {
        /// <summary>
        /// Author of this taxon name. May be empty.
        /// Normally used together with scientific name.
        /// </summary>
        [DataMember]
        public String Author { get; set; }

        /// <summary>
        /// Taxon name category id.
        /// </summary>
        [DataMember]
        public Int32 CategoryId { get; set; }

        /// <summary>
        /// This taxon name values are part of a revision if a
        /// revision event id is specified.
        /// This means that this taxon name is working material.
        /// </summary>
        [DataMember]
        public Int32 ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Id of user that created the taxon name.
        /// Set by web service when taxon name is created.
        /// </summary> 
        [DataMember]
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the taxon name was created.
        /// Set by web service when taxon name is created.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Description of this taxon name.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// GUID for this taxon name.
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// Unique id for this taxon name.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Specifies if property RevisionEventId contains a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsChangedInTaxonRevisionEventIdSpecified 
        { get; set; }

        /// <summary>
        /// Indicates whether this taxon name is appropriate to
        /// use when reporting species observations.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsOkForReportingSpeciesObservations { get; set; }

        /// <summary>
        /// Indicates if it this taxon name may be used in 
        /// species observation systems.
        /// </summary>
        [DataMember]
        public Boolean IsOkForSpeciesObservation { get; set; }

        /// <summary>
        /// Indicates if this is the original taxon name.
        /// </summary>
        [DataMember]
        public Boolean IsOriginalName { get; set; }

        /// <summary>
        /// Indicates if this taxon name has been published.
        /// </summary>
        [DataMember]
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Indicates if this name is the recommended name in 
        /// the specified taxon name category.
        /// </summary>
        [DataMember]
        public Boolean IsRecommended { get; set; }

        /// <summary>
        /// Specifies if property ReplacedInTaxonRevisionEventId
        /// contains a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsReplacedInTaxonRevisionEventIdSpecified 
        { get; set; }

        /// <summary>
        /// Indicates if this taxon name is unique.
        /// </summary>
        [DataMember]
        public Boolean IsUnique { get; set; }

        /// <summary>
        /// Last modified by the user with this id.
        /// Set by web service when taxon name is modified.
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Last modified at this date.
        /// Set by web service when taxon name is modified.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Name of this taxon name.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// This taxon name has been checked out into a revision if
        /// ChangedInRevisionEventId has been specified.
        /// The property ChangedInRevisionEventId works as a flag that
        /// the taxon name may change in the future but the taxon name
        /// is used as normally in the mean time.
        /// Property IsChangedInRevisionEventIdSpecified indicates if
        /// property ChangedInRevisionEventId has a value or not.
        /// </summary>
        [DataMember]
        public Int32 ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Taxon name status id.
        /// </summary>
        [DataMember]
        public Int32 StatusId { get; set; }

        /// <summary>
        /// Taxon that this name belongs to.
        /// </summary>
        [DataMember]
        public WebTaxon Taxon { get; set; }

        /// <summary>
        /// Date taxon name is valid from.
        /// Is set to today when taxon name is created if
        /// ValidFromDate has the value 0001-01-01 00:00:00.0000000.
        /// </summary>
        [DataMember]
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Date taxon name is valid to. 
        /// Is set to date 2444-08-01 when taxon name is created if
        /// ValidToDate has the value 0001-01-01 00:00:00.0000000.
        /// </summary>
        [DataMember]
        public DateTime ValidToDate { get; set; }
    }
}
