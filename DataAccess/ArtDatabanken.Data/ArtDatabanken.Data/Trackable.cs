using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about a trackable object.
    /// </summary>
    public class Trackable : ITrackable
    {
        private Boolean _isDirty = false;

        /// <summary>
        /// Test if object has changed.
        /// </summary>
        public Boolean IsDirty
        {
            get { return _isDirty; }
        }

        /// <summary>
        /// Mark object as changed.
        /// </summary>
        public void HasChanged()
        {
            _isDirty = true;
        }
    }
}
