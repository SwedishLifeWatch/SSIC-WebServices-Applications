using System;
using System.Collections;
using System.Web.Caching;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService.ReferenceService.Test
{
    /// <summary>
    /// Class that handles cache during test.
    /// This class should only be used during local test.
    /// </summary>
    public class WebServiceContextCached : WebServiceContext
    {
        private static readonly Hashtable _cache;

        static WebServiceContextCached()
        {
            _cache = new Hashtable();
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientInformation'>Client information.</param>
        public WebServiceContextCached(WebClientInformation clientInformation)
            : base(clientInformation)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// This constructor should only be used during login.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        public WebServiceContextCached(String userName,
                                       String applicationIdentifier)
            : base(userName, applicationIdentifier)
        {
        }

        /// <summary>
        /// Add object to the local cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <param name='cacheObject'>The object to cache.</param>
        /// <param name='absoluteExpiration'>Time when the object is removed from the cache.</param>
        /// <param name='slidingExpiration'>Information about how much longer from now that the object should be kept in cache.</param>
        /// <param name='priority'>Priority used when deciding which object that should be removed from cache.</param>
        protected override void AddCachedObject(String cacheKey,
                                                Object cacheObject,
                                                DateTime absoluteExpiration,
                                                TimeSpan slidingExpiration,
                                                CacheItemPriority priority)
        {
            // Add information to cache.
            _cache[cacheKey] = cacheObject;
        }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name='checkAccessRight'>
        /// Indicates if user access rights should be checked or not.
        /// Requests to clear cache that are made from Internet should always
        /// check access rights.
        /// </param>
        public override void ClearCache(Boolean checkAccessRight = true)
        {
            // Clear cached data.
            _cache.Clear();
            WebServiceDataServer.ClearCache();
        }

        /// <summary>
        /// Get object from the local cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <returns>Cached object or null if no object was found.</returns>
        public override Object GetCachedObject(String cacheKey)
        {
            return _cache[cacheKey];
        }

        /// <summary>
        /// Remove object from the local cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        public override void RemoveCachedObject(String cacheKey)
        {
            if (_cache.ContainsKey(cacheKey))
            {
                _cache.Remove(cacheKey);
            }
        }
    }
}
