using System.Collections.Generic;
using System.Linq;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData
{
    /// <summary>
    /// This class is used to get/set result data from/to session cache.
    /// </summary>
    public class CalculatedDataItemCollection
    {                
        private readonly Dictionary<CalculatedDataItemCacheKey, object> _calculatedDataItemsDictionary = new Dictionary<CalculatedDataItemCacheKey, object>();

        /// <summary>
        /// Gets a result base item.
        /// </summary>
        /// <typeparam name="T">The result base data type.</typeparam>
        /// <param name="calculatedDataItemCacheKey">The result base cache key.</param>
        /// <returns>
        /// Returns a CalculatedDataItem if the CalculatedDataItemType was found in session cache.
        /// Otherwise a NullCalculatedDataItem object is returned.
        /// </returns>
        public CalculatedDataItem<T> GetCalculatedDataItem<T>(CalculatedDataItemCacheKey calculatedDataItemCacheKey)
        {
            object calculatedDataItem;
            _calculatedDataItemsDictionary.TryGetValue(calculatedDataItemCacheKey, out calculatedDataItem);
            if (calculatedDataItem == null)
            {
                calculatedDataItem = new CalculatedDataItem<T>();
                _calculatedDataItemsDictionary.Add(calculatedDataItemCacheKey, calculatedDataItem);
            }
            return (CalculatedDataItem<T>)calculatedDataItem;
        }

        /// <summary>
        /// Gets a result base item.
        /// </summary>
        /// <typeparam name="T">The result base data type.</typeparam>
        /// <param name="calculatedDataItemType">Type of the result base (enum).</param>
        /// <param name="localeIsoCode">The locale ISO code.</param>
        /// <returns>
        /// Returns a CalculatedDataItem if the CalculatedDataItemType was found in session cache.
        /// Otherwise a NullCalculatedDataItem object is returned.
        /// </returns>
        public CalculatedDataItem<T> GetCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType, string localeIsoCode)
        {
            return GetCalculatedDataItem<T>(new CalculatedDataItemCacheKey(calculatedDataItemType, localeIsoCode));
        }

        /// <summary>
        /// Gets a result base item.
        /// </summary>
        /// <typeparam name="T">The result base data type.</typeparam>
        /// <param name="calculatedDataItemType">Type of the result base (enum).</param>        
        /// <returns>
        /// Returns a CalculatedDataItem if the CalculatedDataItemType was found in session cache.
        /// Otherwise a NullCalculatedDataItem object is returned.
        /// </returns>
        public CalculatedDataItem<T> GetCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType)
        {            
            return GetCalculatedDataItem<T>(new CalculatedDataItemCacheKey(calculatedDataItemType));                       
        }

        /// <summary>
        /// Adds a result base item.
        /// </summary>
        /// <typeparam name="T">The result base data type.</typeparam>
        /// <param name="calculatedDataItemType">Type of the result base.</param>
        /// <param name="data">The data.</param>
        /// <returns>The newly created CalculatedDataItem object</returns>
        public CalculatedDataItem<T> AddCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType, T data)
        {
            return AddCalculatedDataItem(new CalculatedDataItemCacheKey(calculatedDataItemType), data);
        }

        /// <summary>
        /// Adds a result base item.
        /// </summary>
        /// <typeparam name="T">The result base data type.</typeparam>
        /// <param name="calculatedDataItemType">Type of the result base.</param>
        /// <param name="localeIsoCode">The locale ISO code.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The newly created CalculatedDataItem object
        /// </returns>
        public CalculatedDataItem<T> AddCalculatedDataItem<T>(CalculatedDataItemType calculatedDataItemType, string localeIsoCode, T data)
        {
            return AddCalculatedDataItem(new CalculatedDataItemCacheKey(calculatedDataItemType, localeIsoCode), data);
        }

        /// <summary>
        /// Adds a result base item.
        /// </summary>
        /// <typeparam name="T">The result base data type.</typeparam>
        /// <param name="calculatedDataItemCacheKey">The result base cache key.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The newly created CalculatedDataItem object
        /// </returns>
        public CalculatedDataItem<T> AddCalculatedDataItem<T>(CalculatedDataItemCacheKey calculatedDataItemCacheKey, T data)
        {
            CalculatedDataItem<T> calculatedDataItem = new CalculatedDataItem<T>(data);
            if (_calculatedDataItemsDictionary.ContainsKey(calculatedDataItemCacheKey))
            {
                _calculatedDataItemsDictionary[calculatedDataItemCacheKey] = calculatedDataItem;
            }
            else
            {
                _calculatedDataItemsDictionary.Add(calculatedDataItemCacheKey, calculatedDataItem);
            }

            return calculatedDataItem;
        }     

        /// <summary>
        /// Clears the result base items.
        /// </summary>
        public void Clear()
        {
            _calculatedDataItemsDictionary.Clear();                                    
        }

        public void ClearCalculatedDataItem(CalculatedDataItemType calculatedDataItemType)
        {
            List<CalculatedDataItemCacheKey> calculatedDataItemsCacheKeys = _calculatedDataItemsDictionary.Keys.Where(calculatedDataItemCacheKey => calculatedDataItemCacheKey.CalculatedDataItemType == calculatedDataItemType).ToList();
            foreach (CalculatedDataItemCacheKey calculatedDataItemCacheKey in calculatedDataItemsCacheKeys)
            {
                _calculatedDataItemsDictionary.Remove(calculatedDataItemCacheKey);
            }
        }
    }
}
