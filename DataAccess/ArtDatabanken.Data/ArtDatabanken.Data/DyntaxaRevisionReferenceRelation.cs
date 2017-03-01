using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enum for revision reference edit action.
    /// </summary>
    public enum ReferenceRelationEditAction
    {
        /// <summary>
        /// Unknown action.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Add new reference relation action.
        /// </summary>
        Add = 1,

        /// <summary>
        /// Delete existing reference relation action.
        /// </summary>
        Delete = 2,

        /// <summary>
        /// Modify existing reference relation action.
        /// I.e. change reference relation type.
        /// </summary>
        Modify = 3
    }

    /// <summary>
    /// This interface represents a Dyntaxa revision reference relation.
    /// </summary>
    public interface IDyntaxaRevisionReferenceRelation
    {
        /// <summary>
        /// Unique id for this dyntaxa revision reference relation.
        /// </summary>
        Int32 Id { get; set; }

        /// <summary>
        /// Revision id.
        /// </summary>
        Int32 RevisionId { get; set; }

        /// <summary>
        /// Action.
        /// </summary>
        ReferenceRelationEditAction Action { get; set; }

        /// <summary>
        /// Related object unique identifier.
        /// </summary>        
        String RelatedObjectGUID { get; set; }

        /// <summary>
        /// The Reference DB reference identifier.
        /// </summary>
        Int32 ReferenceId { get; set; }

        /// <summary>
        /// Reference type.
        /// </summary>        
        Int32 ReferenceType { get; set; }

        /// <summary>
        /// Old reference type.
        /// </summary>        
        Int32? OldReferenceType { get; set; }

        /// <summary>
        /// The Reference DB Reference relation identifier.
        /// </summary>
        Int32? ReferenceRelationId { get; set; }

        /// <summary>
        /// Last modified by the user with this id.        
        /// </summary>
        Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Last modified at this date.        
        /// </summary>
        DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Id of user that created the item.        
        /// </summary> 
        Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the item was created.        
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Revision event id.
        /// </summary>
        Int32? RevisionEventId { get; set; }

        /// <summary>
        /// This species fact are part of a revision if a
        /// revision event id is specified.        
        /// </summary>
        Int32? ChangedInRevisionEventId { get; set; }

        /// <summary>
        /// Indicates if this species fact has been published.
        /// </summary>        
        Boolean IsPublished { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }
    }

    /// <summary>
    /// This class represents a Dyntaxa revision reference relation.
    /// </summary>
    public class DyntaxaRevisionReferenceRelation : IDyntaxaRevisionReferenceRelation
    {
        /// <summary>
        /// Unique id for this dyntaxa revision reference relation.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Revision id.
        /// </summary>
        public Int32 RevisionId { get; set; }

        /// <summary>
        /// Action.
        /// </summary>
        public ReferenceRelationEditAction Action { get; set; }

        /// <summary>
        /// Related object unique identifier.
        /// </summary>        
        public String RelatedObjectGUID { get; set; }

        /// <summary>
        /// The Reference DB reference identifier.
        /// </summary>
        public Int32 ReferenceId { get; set; }

        /// <summary>
        /// Reference type.
        /// </summary>        
        public Int32 ReferenceType { get; set; }

        /// <summary>
        /// Old reference type.
        /// </summary>        
        public Int32? OldReferenceType { get; set; }

        /// <summary>
        /// The Reference DB Reference relation identifier.
        /// </summary>
        public Int32? ReferenceRelationId { get; set; }

        /// <summary>
        /// Last modified by the user with this id.        
        /// </summary>
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Last modified at this date.        
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Id of user that created the item.        
        /// </summary> 
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the item was created.        
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Revision event id.
        /// </summary>
        public Int32? RevisionEventId { get; set; }

        /// <summary>
        /// This species fact are part of a revision if a
        /// revision event id is specified.        
        /// </summary>
        public Int32? ChangedInRevisionEventId { get; set; }

        /// <summary>
        /// Indicates if this species fact has been published.
        /// </summary>        
        public Boolean IsPublished { get; set; }        

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }
    }
}