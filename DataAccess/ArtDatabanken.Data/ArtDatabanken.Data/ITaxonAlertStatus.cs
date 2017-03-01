using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for definitions of taxon alert statuses.
    /// </summary>
    public interface ITaxonAlertStatus : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the taxon alert status.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier for this taxon alert status.
        /// </summary>
        String Identifier { get; set; }
    }
}
