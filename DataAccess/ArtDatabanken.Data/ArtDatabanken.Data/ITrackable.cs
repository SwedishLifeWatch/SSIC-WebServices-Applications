using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a trackable object.
    /// </summary>
    public interface ITrackable
    {
        /// <summary>
        /// Test if object has changed.
        /// </summary>
        Boolean IsDirty { get; }

        /// <summary>
        /// Mark object as changed.
        /// </summary>
        void HasChanged();
    }
}
