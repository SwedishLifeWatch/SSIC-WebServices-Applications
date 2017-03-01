using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Contains a logic OR condition on data that is returned.
    /// This class is used in data query handling.
    /// </summary>
    public class DataOrCondition : DataQuery
    {
        private DataQueryList _dataQueries;

        /// <summary>
        /// Create a DataOrCondition instance.
        /// </summary>
        public DataOrCondition()
            : base(DataQueryType.OrCondition)
        {
            _dataQueries = new DataQueryList();
        }

        /// <summary>
        /// Get data queries.
        /// </summary>
        public DataQueryList DataQueries
        {
            get { return _dataQueries; }
        }
    }
}
