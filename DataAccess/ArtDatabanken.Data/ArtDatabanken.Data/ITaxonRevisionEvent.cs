using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a revision event
    /// </summary>
    public interface ITaxonRevisionEvent : IDataId32
    {
        /// <summary>
        /// Information about affected taxa.
        /// </summary>
        String AffectedTaxa { get; set; }

        /// <summary>
        /// Id of user who created this taxon revision event.
        /// Set by database when this taxon revision event was created.
        /// </summary>
        Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date when this taxon revision event was created.
        /// Set by database when this taxon revision event was created.
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the new value.
        /// </summary>
        String NewValue { get; set; }

        /// <summary>
        /// Information about the old value.
        /// May be empty.
        /// </summary>
        String OldValue { get; set; }

        /// <summary>
        /// Id of the revision that this
        /// taxon revision event belongs to.
        /// </summary>
        Int32 RevisionId { get; set; }

        /// <summary>
        /// Type of taxon revision event.
        /// </summary>
        ITaxonRevisionEventType Type { get; set; }
    }
}
