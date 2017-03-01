using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for definitions of taxon name category types.
    /// </summary>
    public interface ITaxonNameCategoryType : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the taxon name category type.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier for this name category type.
        /// </summary>
        String Identifier { get; set; }
    }
}
