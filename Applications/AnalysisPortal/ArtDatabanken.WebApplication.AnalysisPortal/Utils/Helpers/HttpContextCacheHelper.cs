using System.Web;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    /// <summary>
    /// This interface is used to set and get cache data.
    /// </summary>
    public interface ICacheHelper
    {
        T GetFromCache<T>(string key);
        void SetInCache<T>(string key, T value);
    }

    /// <summary>
    /// Implementation om ICacheHelper that uses HttpContext to get and set cache data.
    /// </summary>
    public class HttpContextCacheHelper : ICacheHelper
    {        
        /// <summary>
        /// Gets an object from cache.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>An object of type T</returns>
        public T GetFromCache<T>(string key)
        {                        
            object obj = HttpRuntime.Cache[key];
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;            
        }

        /// <summary>
        /// Sets an object in cache.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The value to store in cache.</param>
        public void SetInCache<T>(string key, T value)
        {
            if (value == null)
            {
                HttpRuntime.Cache.Remove(key);
            }
            else
            {
                HttpRuntime.Cache[key] = value;                
            }
        }
    }
}
