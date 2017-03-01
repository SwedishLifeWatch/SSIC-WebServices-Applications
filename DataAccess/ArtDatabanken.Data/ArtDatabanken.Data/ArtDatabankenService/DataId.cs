using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Base class for data types that has id.
    /// </summary>
    [Serializable()]
    public class DataId
    {
        private Int32 _id;

        /// <summary>
        /// Create a DataId instance.
        /// </summary>
        /// <param name='id'>Id for data.</param>
        public DataId(Int32 id)
        {
            _id = id;
        }

        /// <summary>
        /// Get id for this data.
        /// </summary>
        public Int32 Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Test if two DataId references are equal.
        /// </summary>
        /// <param name='data1'>Data to compare.</param>
        /// <param name='data2'>Data to compare.</param>
        /// <returns>True if both references are null or
        /// if they have the same type and Id.</returns>
        public static Boolean AreEqual(DataId data1, DataId data2)
        {
            if (data1.IsNull() && data2.IsNull())
            {
                return true;
            }
            if (data1.IsNull() || data2.IsNull())
            {
                return false;
            }
            if (data1.GetType() != data2.GetType())
            {
                return false;
            }
            return data1.Id == data2.Id;
        }

        /// <summary>
        /// Test if two DataId references are not equal.
        /// </summary>
        /// <param name='data1'>Data to compare.</param>
        /// <param name='data2'>Data to compare.</param>
        /// <returns>False if both references are null or
        /// if they have the same type and Id.</returns>
        public static Boolean AreNotEqual(DataId data1, DataId data2)
        {
            return !AreEqual(data1, data2);
        }

        /// <summary>
        /// Update id.
        /// </summary>
        /// <param name='id'>New id.</param>
        protected void UpdateId(Int32 id)
        {
            _id = id;
        }
    }
}
