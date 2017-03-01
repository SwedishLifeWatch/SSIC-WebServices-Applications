using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about status of taxon names,
    /// (approved, tentatively, invalid, etc.).
    /// </summary>
    public interface ITaxonNameStatus : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Description of the taxon name status.
        /// </summary>
        String Description
        { get; set; }

        /// <summary>
        /// Name of the taxon name status.
        /// </summary>
        String Name
        { get; set; }
    }
}
