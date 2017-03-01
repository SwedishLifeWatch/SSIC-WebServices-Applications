using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Interface to data that can be displayed in lists.
    /// </summary>
    public interface IListableItem
    {
        /// <summary>
        /// A string usable as a display name.
        /// </summary>
        String Label { get; }
        
        /// <summary>
        /// An integer usable as a key in a list.
        /// </summary>
        Int32 Id { get; }
    }
}
