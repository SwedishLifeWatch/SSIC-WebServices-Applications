using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about a revision state.
    /// </summary>
    public interface ITaxonRevisionState : IDataId32
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the taxon revision state.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier for the taxon revision state.
        /// </summary>
        String Identifier { get; set; }
    }
}
