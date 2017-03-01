using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Base class for data types that has a string id.
    /// </summary>
    [Serializable()]
    public class DataStringId : ArtDatabankenBase
    {
        private String _id;

        /// <summary>
        /// Create a DataStringId instance.
        /// </summary>
        /// <param name='id'>Id for data.</param>
        public DataStringId(String id)
        {
            _id = id;
        }

        /// <value>
        /// Get id for this data.
        /// </value>
        public String Id
        {
            get { return _id; }
        }
    }
}
