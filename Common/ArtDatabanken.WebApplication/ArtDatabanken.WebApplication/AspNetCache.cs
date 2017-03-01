using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace ArtDatabanken.WebApplication
{
    /// <summary>
    /// Handles ASP.NET cache for web application.
    /// </summary>
    public static class AspNetCache
    {
        /// <summary>
        /// Add object to the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <param name='cacheObject'>The object to cache.</param>
        /// <param name='absoluteExpiration'>Time when the object is removed from the cache.</param>
        /// <param name='priority'>Priority used when deciding which object that should be removed from cache.</param>
        public static void AddCachedObject(String cacheKey,
                                    Object cacheObject,
                                    DateTime absoluteExpiration,
                                    CacheItemPriority priority)
        {
            AddCachedObject(cacheKey,
                            cacheObject,
                            absoluteExpiration,
                            Cache.NoSlidingExpiration,
                            priority);
        }

        /// <summary>
        /// Add object to the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <param name='cacheObject'>The object to cache.</param>
        /// <param name='slidingExpiration'>Information about how much longer from now that the object should be keept in cache.</param>
        /// <param name='priority'>Priority used when deciding which object that should be removed from cache.</param>
        public static void AddCachedObject(String cacheKey,
                                    Object cacheObject,
                                    TimeSpan slidingExpiration,
                                    CacheItemPriority priority)
        {
            AddCachedObject(cacheKey,
                            cacheObject,
                            Cache.NoAbsoluteExpiration,
                            slidingExpiration,
                            priority);
        }

        /// <summary>
        /// Add object to the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <param name='cacheObject'>The object to cache.</param>
        /// <param name='absoluteExpiration'>Time when the object is removed from the cache.</param>
        /// <param name='slidingExpiration'>Information about how much longer from now that the object should be keept in cache.</param>
        /// <param name='priority'>Priority used when deciding which object that should be removed from cache.</param>
        private static void AddCachedObject(String cacheKey,
                                               Object cacheObject,
                                               DateTime absoluteExpiration,
                                               TimeSpan slidingExpiration,
                                               CacheItemPriority priority)
        {
            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                lock (HttpContext.Current.Cache)
                {
                    if (HttpContext.Current.Cache.Get(cacheKey).IsNull())
                    {
                        HttpContext.Current.Cache.Add(cacheKey,
                                                      cacheObject,
                                                      null,
                                                      absoluteExpiration,
                                                      slidingExpiration,
                                                      priority,
                                                      null);
                    }
                    else
                    {
                        HttpContext.Current.Cache.Insert(cacheKey,
                                                         cacheObject,
                                                         null,
                                                         absoluteExpiration,
                                                         slidingExpiration,
                                                         priority,
                                                         null);
                    }
                }
            }
        }

        /// <summary>
        /// Get object from the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <returns>Cached object or null if no object was found.</returns>
        public static Object GetCachedObject(String cacheKey)
        {
            Object cachedObject = null;

            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                cachedObject = HttpContext.Current.Cache.Get(cacheKey);
            }
            return cachedObject;
        }

        /// <summary>
        /// Clear ASP.NET cache in web application.
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator cacheEnum;
            List<String> cacheKeys;

            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                lock (HttpContext.Current.Cache)
                {
                    // Empty cache in ASP.NET.
                    cacheEnum = HttpContext.Current.Cache.GetEnumerator();
                    cacheKeys = new List<String>();
                    while (cacheEnum.MoveNext())
                    {
                        cacheKeys.Add((String)(cacheEnum.Key));
                    }

                    foreach (String cacheKey in cacheKeys)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }
            }
        }

        /// <summary>
        /// Remove object from the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        public static void RemoveCachedObject(String cacheKey)
        {
            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                lock (HttpContext.Current.Cache)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

    }
}
