using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Contains extension methods to the IDataId32 interface.
    /// </summary>
    public static class IDataId32Extension
    {
        /// <summary>
        /// Test if two IDataId32 references are equal.
        /// </summary>
        /// <param name='data1'>First data to compare.</param>
        /// <param name='data2'>Second data to compare.</param>
        /// <returns>True if both references are null or
        /// if they have the same type and Id.</returns>
        public static Boolean AreEqual(this IDataId32 data1,
                                       IDataId32 data2)
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
        /// Test if two IDataId32 references are not equal.
        /// </summary>
        /// <param name='data1'>First data to compare.</param>
        /// <param name='data2'>Second data to compare.</param>
        /// <returns>
        /// False if both references are null or
        /// if they have the same type and id.
        /// </returns>
        public static Boolean AreNotEqual(this IDataId32 data1,
                                          IDataId32 data2)
        {
            return !AreEqual(data1, data2);
        }
    }
}
