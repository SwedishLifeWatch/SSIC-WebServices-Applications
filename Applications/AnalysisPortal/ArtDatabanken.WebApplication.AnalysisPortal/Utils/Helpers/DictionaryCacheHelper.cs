using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    public class DictionaryCacheHelper : ICacheHelper
    {
        private readonly Dictionary<string, object> _context = new Dictionary<string, object>();

        public T GetFromCache<T>(string key)
        {
            object obj = null;
            _context.TryGetValue(key, out obj);
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;                        
        }

        public void SetInCache<T>(string key, T value)
        {
            if (value == null)
            {
                _context.Remove(key);
            }
            else
            {
                _context[key] = value;
            }
        }
    }
}
