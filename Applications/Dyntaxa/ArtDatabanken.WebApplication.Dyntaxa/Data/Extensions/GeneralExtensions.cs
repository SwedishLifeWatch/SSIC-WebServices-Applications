using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public static class GeneralExtensions
    {
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
