using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Each taxon relation connects a parent taxon with a child taxon.
    /// Current taxon tree is defined by all currently
    /// valid taxon relations.
    /// </summary>
    public interface ITaxonRelation : IDataId32
    {
        /// <summary>
        /// This taxon relation was created
        /// or updated in this revision event.
        /// </summary>
        Int32? ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Child taxon in this taxon relation.
        /// </summary>
        ITaxon ChildTaxon { get; set; }

        /// <summary>
        /// Id of user that created the taxon relation.
        /// Set by database when created.
        /// </summary>
        Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date and time when the taxon relation was created.
        /// Set by web service when taxon relation is created.
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// GUID for this taxon relation.
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// Indicates if this taxon relation is a main relation or not.
        /// Only main relations are used to form the standard taxon tree. 
        /// </summary>
        Boolean IsMainRelation { get; set; }

        /// <summary>
        /// Indicates if this taxon relation has been published or not.
        /// </summary>
        Boolean IsPublished { get; set; }

        /// <summary>
        /// Last modified by the user with this id.
        /// Set by web service when taxon relation is modified.
        /// </summary>
        Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Last modified by a person with this name.
        /// </summary>
        String ModifiedByPerson { get; set; }

        /// <summary>
        /// Last modified at this date.
        /// Set by web service when taxon relation is modified.
        /// </summary>
        DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Parent taxon in this taxon relation.
        /// </summary>
        ITaxon ParentTaxon { get; set; }

        /// <summary>
        /// This taxon relation was replaced in this revision event.
        /// </summary>
        Int32? ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Sort order for this taxon relation amongs other
        /// taxon relations related to the same parent taxon.
        /// </summary>
        Int32 SortOrder { get; set; }

        /// <summary>
        /// This taxon relation is valid from this date.
        /// </summary>
        DateTime ValidFromDate { get; set; }

        /// <summary>
        /// This taxon relation is valid to this date.
        /// </summary>
        DateTime ValidToDate { get; set; }
    }
}
