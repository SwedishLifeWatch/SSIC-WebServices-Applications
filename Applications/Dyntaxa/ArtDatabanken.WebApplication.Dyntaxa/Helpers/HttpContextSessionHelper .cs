using System.Web;

namespace ArtDatabanken.WebApplication.Dyntaxa.Helpers
{
    public class HttpContextSessionHelper : ISessionHelper
    {
        public static ISessionHelper TestHelper = null;
        public static bool IsInTestMode
        {
            get { return TestHelper != null; }
        }

        private readonly HttpContextBase _context;
       
        public HttpContextSessionHelper(HttpContextBase context)
        {
           _context = context;
            TestHelper = this;
        }

        public HttpContextSessionHelper(HttpContext context)
        {
            _context = new HttpContextWrapper(context);
        }

       public T GetFromSession<T>(string key)
        {
           if (_context.Session != null)
           {
               object obj = _context.Session[key];
               if (obj == null)
               {
                   return default(T);
               }
               return (T)obj;
           }
           return default(T);
        }         

        public void SetInSession<T>(string key, T value)
        {
            if (value == null)
            {
                if (_context.Session != null)
                {
                    _context.Session.Remove(key);
                }
            }
            else
            {
                if (_context.Session != null)
                {
                    _context.Session[key] = value;
                }
            }
        }

        public T GetFromCache<T>(string key)
        {
            if (_context.Session != null)
            {
                object obj = _context.Cache[key];
                if (obj == null)
                {
                    return default(T);
                }
                return (T)obj;
            }
            return default(T);
        }

        public void SetInCache<T>(string key, T value)
        {
            if (value == null)
            {
                if (_context.Session != null)
                {
                    _context.Cache.Remove(key);
                }
            }
            else
            {
                if (_context.Session != null)
                {
                    _context.Cache[key] = value;
                }
            }
        }
    }
}
