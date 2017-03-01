using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Base class for data types that has sort order and id.
    /// </summary>
    public class DataStringSortOrder : DataStringId, IComparable
    {
        private Int32 _sortOrder;

        /// <summary>
        /// Create a DataSortOrder instance.
        /// </summary>
        /// <param name='id'>Id for data.</param>
        /// <param name='sortOrder'>Sort order among data.</param>
        public DataStringSortOrder(String id, Int32 sortOrder)
            : base(id)
        {
            _sortOrder = sortOrder;
        }

        /// <value>
        /// Get sort order for this data.
        /// </value>
        public Int32 SortOrder
        {
            get { return _sortOrder; }
        }

        /// <summary>
        /// Implementation of the IComparable interface.
        /// Compare this DataStringSortOrder with another DataStringSortOrder.
        /// </summary>
        /// <param name='obj'>Object to compare this object to.</param>
        /// <returns>Comparison value.</returns>
        public int CompareTo(object obj)
        {
            if (obj is DataStringSortOrder)
            {
                DataStringSortOrder otherData = (DataStringSortOrder)obj;
                return SortOrder.CompareTo(otherData.SortOrder);
            }
            else
            {
                throw new ArgumentException("object is not a DataStringSortOrder");
            }
        }
    }
}

