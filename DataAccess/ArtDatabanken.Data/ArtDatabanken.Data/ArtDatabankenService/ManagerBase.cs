using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Delegate for event handling of cache refresh.
    /// </summary>
    public delegate void RefreshCacheEventHandler();

    /// <summary>
    /// This class contains functionality that is common
    /// to all Manager classes.
    /// </summary>
    public class ManagerBase
    {
        /// <summary>
        /// Event handling of cache refresh.
        /// </summary>
        public static event RefreshCacheEventHandler RefreshCacheEvent;

        /// <summary>
        /// _lockObject is used as a global lock object
        /// when cached data is handled.
        /// _lockObject is used to make handling of cached
        /// data thread safe.
        /// </summary>
        protected static DateTimeSearchCriteria _lockObject;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ManagerBase()
        {
            _lockObject = new DateTimeSearchCriteria();
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        public static void FireRefreshCache()
        {
            // Fire refresh cache event.
            if (RefreshCacheEvent.IsNotNull())
            {
                RefreshCacheEvent();
            }
        }
    }
}
