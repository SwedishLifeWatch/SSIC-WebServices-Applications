using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a taxonomic change of an existing taxon.
    /// </summary>
    public interface ILumpSplitEvent : IDataId32
    {
        /// <summary>
        /// This lump split event was created
        /// or updated in this revision event.
        /// </summary>
        Int32? ChangedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Id of user that created the lump split event.
        /// </summary>
        Int32 CreatedBy { get; set; }

        /// <summary>
        /// The lump split event was created at this date.
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the change.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// This lump split event was replaced in this revision event.
        /// Is null if this revision event has not been replaced.
        /// </summary>
        Int32? ReplacedInTaxonRevisionEventId { get; set; }

        /// <summary>
        /// Changed taxon (specified in TaxonBefore) was replaced
        /// by this taxon.
        /// </summary>
        ITaxon TaxonAfter { get; set; }

        /// <summary>
        /// Gets or sets TaxonBefore.
        /// </summary>
        ITaxon TaxonBefore { get; set; }

        /// <summary>
        /// Type of lump split event.
        /// </summary>
        ILumpSplitEventType Type { get; set; }
    }
}
