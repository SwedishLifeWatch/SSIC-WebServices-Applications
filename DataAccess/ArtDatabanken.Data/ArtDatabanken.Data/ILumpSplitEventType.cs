using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface that handles information about a lump split event type.
    /// </summary>
    public interface ILumpSplitEventType : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the lump split event type.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier for the lump split event type.
        /// </summary>
        String Identifier { get; set; }
    }
}
