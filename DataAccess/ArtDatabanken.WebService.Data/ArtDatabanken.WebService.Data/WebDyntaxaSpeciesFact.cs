using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a species fact in Dyntaxa that is used 
    /// during Revision to save state until the revision is checked in.
    /// </summary>
    [DataContract]
    public class WebDyntaxaRevisionSpeciesFact : WebData
    {
        /// <summary>
        /// Unique id for this dyntaxa revision species fact.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Factor Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 FactorId { get; set; }

        /// <summary>
        /// Taxon id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 TaxonId { get; set; }

        /// <summary>
        /// Revision id.
        /// </summary>
        [DataMember]
        public Int32 RevisionId { get; set; }

        /// <summary>
        /// StatusId for this species fact.        
        /// </summary>
        [DataMember]
        public Int32? StatusId { get; set; }        

        /// <summary>
        /// Quality id for this species fact.
        /// </summary>
        [DataMember]
        public Int32? QualityId { get; set; }

        /// <summary>
        /// Reference id for this species fact.
        /// </summary>
        [DataMember]
        public Int32? ReferenceId { get; set; }

        /// <summary>
        /// Description for this species fact.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Indicates whether the species fact exists in Artfakta database or not.
        /// </summary>
        [DataMember]
        public Boolean SpeciesFactExists { get; set; }

        /// <summary>
        /// Original StatusId for this species fact.
        /// Will be null if SpeciesFactExists==false.
        /// </summary>
        [DataMember]
        public Int32? OriginalStatusId { get; set; }

        /// <summary>
        /// Original Quality id for this species fact.
        /// Will be null if SpeciesFactExists==false.
        /// </summary>
        [DataMember]
        public Int32? OriginalQualityId { get; set; }

        /// <summary>
        /// Original Reference id for this species fact.
        /// Will be null if SpeciesFactExists==false.
        /// </summary>
        [DataMember]
        public Int32? OriginalReferenceId { get; set; }

        /// <summary>
        /// Original Description for this species fact.
        /// Will be null if SpeciesFactExists==false.
        /// </summary>
        [DataMember]
        public String OriginalDescription { get; set; }

        /// <summary>
        /// Last modified by the user with this id.        
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Last modified at this date.        
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Id of user that created the item.        
        /// </summary> 
        [DataMember]
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the item was created.        
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }        

        /// <summary>
        /// Revision event id.
        /// </summary>
        [DataMember]
        public Int32 RevisionEventId { get; set; }

        /// <summary>
        /// Specifies if property RevisionEventId contains a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsRevisionEventIdSpecified { get; set; }

        /// <summary>
        /// This species fact are part of a revision if a
        /// revision event id is specified.        
        /// </summary>
        [DataMember]
        public Int32 ChangedInRevisionEventId { get; set; }

        /// <summary>
        /// Specifies if property ChangedInRevisionEventId contains a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsChangedInRevisionEventIdSpecified { get; set; }

        /// <summary>
        /// Indicates if this species fact has been published.
        /// </summary>
        [DataMember]
        public Boolean IsPublished { get; set; }
    }
}