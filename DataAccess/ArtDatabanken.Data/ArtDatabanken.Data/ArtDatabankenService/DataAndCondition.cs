using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Contains a logic AND condition on data that is returned.
    /// This class is used in data query handling.
    /// </summary>
    public class DataAndCondition : DataQuery
    {
        private DataQueryList _dataQueries;

        /// <summary>
        /// Create a DataAndCondition instance.
        /// </summary>
        public DataAndCondition()
            : base(DataQueryType.AndCondition)
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
