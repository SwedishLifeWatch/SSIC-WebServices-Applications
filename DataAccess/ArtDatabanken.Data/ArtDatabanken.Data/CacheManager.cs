namespace ArtDatabanken.Data
{
    /// <summary>
    /// Delegate for event handling of cache refresh.
    /// </summary>
    /// <param name="userContext">User context.</param>
    public delegate void RefreshCacheEventHandler(IUserContext userContext);

    /// <summary>
    /// This class contains functionality that is common
    /// to all Manager classes.
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// Event handling of cache refresh.
        /// </summary>
        public static event RefreshCacheEventHandler RefreshCacheEvent;

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public static void FireRefreshCache(IUserContext userContext)
        {
            // Fire refresh cache event.
            if (RefreshCacheEvent.IsNotNull())
            {
                RefreshCacheEvent(userContext);
            }
        }
    }
}
