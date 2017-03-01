using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for definitions of taxon change statuses.
    /// </summary>
    public interface ITaxonChangeStatus : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the taxon change status.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier for this taxon change status.
        /// </summary>
        String Identifier { get; set; }
    }
}
