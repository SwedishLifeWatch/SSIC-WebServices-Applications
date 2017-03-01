using System;
using System.Collections;
using System.Web.Caching;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test
{
    /// <summary>
    /// Class that handles cache during test.
    /// This class should only be used during local test.
    /// </summary>
    public class WebServiceContextCached : WebServiceContext
    {
        private static Hashtable _cache;

        static WebServiceContextCached()
        {
            _cache = new Hashtable();
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        public WebServiceContextCached(String clientToken)
            : base(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, null, null, null, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Token with information about current user and connection</param>
        /// <param name='checkUser'>Information about if user authority should be checked.
        ///                         It should only be set to false during login.</param>
        public WebServiceContextCached(WebClientToken clientToken,
                                       Boolean checkUser)
            : base(clientToken, checkUser, null, null, null, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        public WebServiceContextCached(String clientToken,
                                       String traceMethod)
            : base(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, traceMethod, null, null, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        /// <param name='traceArgument1'>Argument 1 to the calling method. Used during tracing.</param>
        public WebServiceContextCached(String clientToken,
                                       String traceMethod,
                                       Object traceArgument1)
            : base(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, traceMethod, traceArgument1, null, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        /// <param name='traceArgument1'>Argument 1 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument2'>Argument 2 to the calling method. Used during tracing.</param>
        public WebServiceContextCached(String clientToken,
                                       String traceMethod,
                                       Object traceArgument1,
                                       Object traceArgument2)
            : base(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, traceMethod, traceArgument1, traceArgument2, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        /// <param name='traceArgument1'>Argument 1 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument2'>Argument 2 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument3'>Argument 3 to the calling method. Used during tracing.</param>
        public WebServiceContextCached(String clientToken,
                                       String traceMethod,
                                       Object traceArgument1,
                                       Object traceArgument2,
                                       Object traceArgument3)
            : base(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, traceMethod, traceArgument1, traceArgument2, traceArgument3)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Token with information about current user and connection</param>
        /// <param name='checkUser'>Information about if user authority should be checked.
        ///                         It should only be set to false during login.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        /// <param name='traceArgument1'>Argument 1 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument2'>Argument 2 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument3'>Argument 3 to the calling method. Used during tracing.</param>
        public WebServiceContextCached(WebClientToken clientToken,
                                       Boolean checkUser,
                                       String traceMethod,
                                       Object traceArgument1,
                                       Object traceArgument2,
                                       Object traceArgument3)
            : base(clientToken, checkUser, traceMethod, traceArgument1, traceArgument2, traceArgument3)
        {
        }

        /// <summary>
        /// Add object to the local cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <param name='cacheObject'>The object to cache.</param>
        /// <param name='absoluteExpiration'>Time when the object is removed from the cache.</param>
        /// <param name='slidingExpiration'>Information about how much longer from now that the object should be keept in cache.</param>
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
        /// Get object from the local cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <returns>Cached object or null if no object was found.</returns>
        public override Object GetCachedObject(String cacheKey)
        {
            return _cache[cacheKey];
        }
    }
}
