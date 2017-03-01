using System;
using System.Collections.Specialized;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    /// <summary>
    /// General extension methods
    /// </summary>
    public static class GeneralExtensions
    {
        /// <summary>
        /// Creates a string where the keys and their corresponding value is written in a 
        /// readable way.
        /// Is for example used to print the Form collection in a request.
        /// </summary>
        /// <param name="collection">The NameValueCollection.</param>
        /// <returns></returns>
        public static string PrettyPrint(this NameValueCollection collection)
        {
            if (collection == null || collection.Count == 0)
            {
                return "";
            }

            try
            {
                var sb = new StringBuilder();
                for (int i = 0; i < collection.AllKeys.Length; i++)
                {
                    string key = collection.AllKeys[i];
                    var val = collection[key];
                    if (i < collection.AllKeys.Length - 1)
                    {
                        sb.AppendFormat("{0}={1}, ", key, val);
                    }
                    else
                    {
                        sb.AppendFormat("{0}={1}", key, val);
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
