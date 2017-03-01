using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for definitions of reference relation types.
    /// </summary>
    public interface IReferenceRelationType : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the reference relation type.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier for this reference relation type.
        /// </summary>
        String Identifier { get; set; }
    }
}
