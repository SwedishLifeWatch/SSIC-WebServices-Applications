using System.Web;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    /// <summary>
    /// This interface is used to set and get session data.
    /// </summary>
    public interface ISessionHelper
    {
        T GetFromSession<T>(string key);
        void SetInSession<T>(string key, T value);
    }

    /// <summary>
    /// Implementation om ISessionHelper that uses HttpContext to get and set session data.
    /// </summary>
    public class HttpContextSessionHelper : ISessionHelper
    {
        /// <summary>
        /// Gets an object from session.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="key">The session key.</param>
        /// <returns>An object of type T</returns>
        public T GetFromSession<T>(string key)
        {
            object obj = null;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                obj = HttpContext.Current.Session[key];    
            }
            
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj; 
        }

        /// <summary>
        /// Sets an object in session.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The session key.</param>
        /// <param name="value">The value to store in session.</param>
        public void SetInSession<T>(string key, T value)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
            {
                return;
            }

            if (value == null && HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
            else
            {
                HttpContext.Current.Session[key] = value;
            }
        }
    }
}
