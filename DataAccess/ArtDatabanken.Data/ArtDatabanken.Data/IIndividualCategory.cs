using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents an individual category.
    /// </summary>
    public interface IIndividualCategory : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this individual category.
        /// </summary>
        String Definition { get; set; }

        /// <summary>
        /// Name for this individual category.
        /// </summary>
        String Name { get; set; }
    }
}