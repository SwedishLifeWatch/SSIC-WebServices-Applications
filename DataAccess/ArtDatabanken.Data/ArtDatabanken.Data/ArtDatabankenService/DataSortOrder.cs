using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Base class for data types that has sort order and id.
    /// </summary>
    [Serializable()]
    public class DataSortOrder : DataId, IComparable
    {
        private Int32 _sortOrder;

        /// <summary>
        /// Create a DataSortOrder instance.
        /// </summary>
        /// <param name='id'>Id for data.</param>
        /// <param name='sortOrder'>Sort order among data.</param>
        public DataSortOrder(Int32 id, Int32 sortOrder)
            : base(id)
        {
            _sortOrder = sortOrder;
        }

        /// <summary>
        /// Get sort order for this data.
        /// </summary>
        public Int32 SortOrder
        {
            get { return _sortOrder; }
        }

        /// <summary>
        /// Implementation of the IComparable interface.
        /// Compare this DataSortOrder with another DataSortOrder.
        /// </summary>
        /// <param name='obj'>Object to compare this object to.</param>
        /// <returns>Comparison value.</returns>
        public int CompareTo(object obj)
        {
            if (obj is DataSortOrder)
            {
                DataSortOrder otherData = (DataSortOrder)obj;
                return SortOrder.CompareTo(otherData.SortOrder);
            }
            else
            {
                throw new ArgumentException("object is not a DataSortOrder");
            }
        }
    }
}
