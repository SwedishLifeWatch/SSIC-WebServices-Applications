using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Each taxon relation connects a parent taxon with a child taxon.
    /// Current taxon tree is defined by all currently
    /// valid taxon relations.
    /// </summary>
    [DataContract]
    public class WebTaxonRelation : WebData
    {
        /// <summary>
        /// This taxon relation was created
        /// or updated in this revision event.
        /// </summary>
        [DataMember]
        public Int32 ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Id for the child taxon in this taxon relation.
        /// </summary>
        [DataMember]
        public Int32 ChildTaxonId { get; set; }

        /// <summary>
        /// Id of user that created the taxon relation.
        /// Set by database when created.
        /// </summary>
        [DataMember]
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the taxon relation was created.
        /// Set by web service when taxon relation is created.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// GUID for this taxon relation.
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// Id for this taxon relation.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if property ChangedInRevisionEventId has been set.
        /// </summary>
        [DataMember]
        public Boolean IsChangedInTaxonRevisionEventIdSpecified { get; set; }

        /// <summary>
        /// Indicates if this taxon relation is a main relation or not.
        /// Only main relations are used to form the standard taxon tree. 
        /// </summary>
        [DataMember]
        public Boolean IsMainRelation { get; set; }

        /// <summary>
        /// Indicates if this taxon relation has been published or not.
        /// </summary>
        [DataMember]
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Indicates if property ReplacedInRevisionEvent has been set.
        /// </summary>
        [DataMember]
        public Boolean IsReplacedInTaxonRevisionEventIdSpecified { get; set; }

        /// <summary>
        /// Last modified by the user with this id.
        /// Set by web service when taxon relation is modified.
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Last modified at this date.
        /// Set by web service when taxon relation is modified.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Id for the parent taxon in this taxon relation.
        /// </summary>
        [DataMember]
        public Int32 ParentTaxonId { get; set; }

        /// <summary>
        /// This taxon relation was replaced in this revision event.
        /// </summary>
        [DataMember]
        public Int32 ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Sort order for this taxon relation amongs other
        /// taxon relations related to the same parent taxon.
        /// </summary>
        [DataMember]
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// This taxon relation is valid from this date.
        /// </summary>
        [DataMember]
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// This taxon relation is valid to this date.
        /// </summary>
        [DataMember]
        public DateTime ValidToDate { get; set; }

        /// <summary>
        /// True if relation is valid
        /// </summary>
        public bool IsValid { get { return DateTime.Now > ValidFromDate && DateTime.Now < ValidToDate; } } 
        
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("[{0}] => [{1}] TaxonTreeId: {2}, MainRelation: {3}, Valid: {4}", ParentTaxonId, ChildTaxonId, Id, IsMainRelation ? "Yes" : "No", IsValid ? "Yes" : "No");            
        }
    }
}
