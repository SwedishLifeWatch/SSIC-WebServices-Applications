using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about changes made regarding taxa.
    /// Current version includes changes regarding:
    /// - new taxon
    /// - new/edited taxon name (scientific + common)
    /// - lump/split events
    /// - taxon category changes
    /// </summary>
    public interface ITaxonChange
    {
        /// <summary>
        /// Category that this taxon belongs to.
        /// </summary>
        ITaxonCategory Category { get; set; }

        /// <summary>
        /// Id of the type of Event that changed the taxon.
        /// </summary>
        Int32 EventTypeId { get; set; }

        /// <summary>
        /// TaxonId unique identification of a taxon.
        /// </summary>
        Int32 TaxonId { get; set; }

        /// <summary>
        /// TaxonId for taxon involved in lump or split.
        /// Value is the id taxon got AFTER the lump/split event occurred.
        /// </summary>
        Int32 TaxonIdAfter { get; set; }

        /// <summary>
        /// Recommended scientific name.
        /// </summary>
        String ScientificName { get; set; }
    }
}
