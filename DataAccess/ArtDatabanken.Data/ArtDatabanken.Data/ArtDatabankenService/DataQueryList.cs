using System;
using System.Collections;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the IDataQuery interface.
    /// </summary>
    public class DataQueryList : ArrayList
    {
        /// <summary>
        /// Get/set IDataQuery by list index.
        /// </summary>
        public new IDataQuery this[Int32 index]
        {
            get
            {
                return (IDataQuery)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
