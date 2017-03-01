using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Template list for data types without id.
    /// </summary>
    [Serializable]
    public class DataList<T> : List<T>
    {
        /// <summary>
        /// Get generic list.
        /// </summary>
        /// <returns>Generic list.</returns>       
        public IList<T> GetGenericList()
        {
            IList<T> list;
            Int32 index;

            list = new List<T>();
            if (this.IsNotEmpty())
            {
                for (index = 0; index < Count; index++)
                {
                    list.Add(this[index]);
                }
            }
            return list;
        }
    }
}
