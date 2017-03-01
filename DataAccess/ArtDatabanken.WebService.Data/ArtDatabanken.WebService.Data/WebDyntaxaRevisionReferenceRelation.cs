using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a reference relation in Dyntaxa that is used 
    /// during Revision to save state until the revision is checked in.
    /// </summary>
    [DataContract]
    public class WebDyntaxaRevisionReferenceRelation : WebData
    {                
        /// <summary>
        /// Unique id for this dyntaxa revision reference relation.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }
        
        /// <summary>
        /// Revision id.
        /// </summary>
        [DataMember]
        public Int32 RevisionId { get; set; }

        /// <summary>
        /// Action.
        /// </summary>
        [DataMember]
        public String Action { get; set; }

        /// <summary>
        /// Related object unique identifier.
        /// </summary>        
        [DataMember]
        public String RelatedObjectGUID { get; set; }        

        /// <summary>
        /// The Reference DB reference identifier.
        /// </summary>
        [DataMember]
        public Int32 ReferenceId { get; set; }
        
        /// <summary>
        /// Reference type.
        /// </summary>        
        [DataMember]
        public Int32 ReferenceType { get; set; }

        /// <summary>
        /// Old reference type.
        /// </summary>        
        [DataMember]
        public Int32? OldReferenceType { get; set; }

        /// <summary>
        /// The Reference DB Reference relation identifier.
        /// </summary>
        [DataMember]
        public Int32? ReferenceRelationId { get; set; }

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