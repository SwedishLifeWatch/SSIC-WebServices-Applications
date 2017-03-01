using System;


namespace ArtDatabanken.Data
{

    /// <summary>
    /// RevisionEvent
    /// </summary>
    public class TaxonRevisionEvent : ITaxonRevisionEvent
    {
        /// <summary>
        /// Id for the taxon revision event.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Id of the revision that this
        /// taxon revision event belongs to.
        /// </summary>
        public Int32 RevisionId { get; set; }

        /// <summary>
        /// Type of taxon revision event.
        /// </summary>
        public ITaxonRevisionEventType Type { get; set; }

        /// <summary>
        /// Date when this taxon revision event was created.
        /// Set by database when this taxon revision event was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Id of user who created this taxon revision event.
        /// Set by database when this taxon revision event was created.
        /// </summary>
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about affected taxa.
        /// </summary>
        public String AffectedTaxa { get; set; }

        /// <summary>
        /// New value.
        /// </summary>
        public String NewValue { get; set; }

        /// <summary>
        /// Old value.
        /// </summary>
        public String OldValue { get; set; }
    }
}
