using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dyntaxa.Helpers
{
    /// <summary>
    /// Helper methods for dictionaries
    /// </summary>
    public static class DictionaryHelper
    {
        /// <summary>
        /// Merge two dictionaries. The keys in the original dictionary will
        /// be overwritten if there are duplicates.
        /// </summary>
        public static RouteValueDictionary MergeDictionaries(RouteValueDictionary original, object newDictionary)
        {
            return MergeDictionaries(original, new RouteValueDictionary(newDictionary));
        }

        /// <summary>
        /// Merge two dictionaries. The keys in the original dictionary will
        /// be overwritten if there are duplicates.
        /// </summary>        
        public static RouteValueDictionary MergeDictionaries(RouteValueDictionary original, RouteValueDictionary newDic)
        {
            if (original == null)
            {
                return newDic;
            }

            if (newDic == null)
            {
                return original;
            }

            RouteValueDictionary merged = new RouteValueDictionary(original);
            foreach (var f in newDic)
            {
                if (merged.ContainsKey(f.Key))
                {
                    merged[f.Key] = f.Value;
                }
                else
                {
                    merged.Add(f.Key, f.Value);
                }
            }

            return merged;
        }

        /// <summary>
        /// Convert a NameValueCollection to RouteValueDictionary
        /// </summary>        
        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection)
        {
            RouteValueDictionary dic = new RouteValueDictionary();
            foreach (string key in collection.AllKeys)
            {
                dic.Add(key, collection[key]);
            }

            return dic;
        } 
    }
}