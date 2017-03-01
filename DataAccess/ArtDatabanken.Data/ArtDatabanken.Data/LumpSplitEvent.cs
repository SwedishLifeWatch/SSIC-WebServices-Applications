using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a taxonomic change of an existing taxon.
    /// </summary>
    public class LumpSplitEvent : ILumpSplitEvent
    {
        /// <summary>
        /// This lump split event was created
        /// or updated in this revision event.
        /// </summary>
        public Int32? ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Id of user that created the lump split event.
        /// </summary>
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// The lump split event was created at this date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Information about the change.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id of the lump split event.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// This lump split event was replaced in this revision event.
        /// Is null if this revision event has not been replaced.
        /// </summary>
        public Int32? ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Changed taxon (specified in TaxonBefore) was replaced
        /// by this taxon.
        /// </summary>
        public ITaxon TaxonAfter { get; set; }

        /// <summary>
        /// Gets or sets TaxonBefore.
        /// </summary>
        public ITaxon TaxonBefore { get; set; }

        /// <summary>
        /// Type of lump split event.
        /// </summary>
        public ILumpSplitEventType Type { get; set; }
    }
}
