using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    /// <summary>
    /// Handles cache storage.
    /// <remarks>
    /// In test mode you can call SetCacheHelper(...) to use a mock storage.
    /// </remarks>   
    /// </summary>
    public static class CacheHandler
    {
        private static ICacheHelper _cacheHelper;

        /// <summary>
        /// Static constructor
        /// </summary>
        static CacheHandler()
        {            
            _cacheHelper = new HttpContextCacheHelper();
        }

        /// <summary>
        /// Sets which cache helper to use.
        /// </summary>
        /// <param name="cacheHelper"></param>
        public static void SetCacheHelper(ICacheHelper cacheHelper) // use this in test
        {
            _cacheHelper = cacheHelper;
        }     

        /// <summary>
        /// Sets the application user context.
        /// <remarks>
        /// There will be one for each language we support.
        /// </remarks>
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="userContext">The user context.</param>
        public static void SetApplicationUserContext(string key, IUserContext userContext)
        {
            _cacheHelper.SetInCache(key, userContext);
        }

        /// <summary>
        /// Gets the application user context.
        /// <remarks>
        /// There will be one for each language we support.
        /// </remarks>
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>An application user context.</returns>
        public static IUserContext GetApplicationUserContext(string key)
        {
            return _cacheHelper.GetFromCache<IUserContext>(key);
        }
    }
}
