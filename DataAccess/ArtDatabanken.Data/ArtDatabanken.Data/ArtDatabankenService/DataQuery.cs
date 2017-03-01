using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Base class for classes that implements data query
    /// related functionality.
    /// </summary>
    public class DataQuery : IDataQuery
    {
        private DataQueryType _type;

        /// <summary>
        /// Create a DataQuery instance.
        /// </summary>
        /// <param name='type'>Type of data query.</param>
        public DataQuery(DataQueryType type)
        {
            _type = type;
        }

        /// <summary>
        /// Get type of data query element.
        /// </summary>
        public DataQueryType Type
        {
            get { return _type; }
        }
    }
}
