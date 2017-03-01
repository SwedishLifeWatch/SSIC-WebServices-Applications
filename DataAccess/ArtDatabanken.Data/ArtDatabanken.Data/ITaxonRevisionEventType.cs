using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about
    /// a taxon revision event type.
    /// </summary>
    public interface ITaxonRevisionEventType : IDataId32
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Name of the taxon revision event type.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier of the taxon revision event type.
        /// </summary>
        String Identifier { get; set; }
    }
}
