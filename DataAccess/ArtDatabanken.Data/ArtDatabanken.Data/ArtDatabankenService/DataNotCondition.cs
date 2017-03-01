using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Contains a logic NOT condition on data that is returned.
    /// This class is used in data query handling.
    /// </summary>
    public class DataNotCondition : DataQuery
    {
        /// <summary>
        /// Create a DataNotCondition instance.
        /// </summary>
        public DataNotCondition()
            : base(DataQueryType.NotCondition)
        {
            DataQuery = null;
        }

        /// <summary>
        /// Data query.
        /// </summary>
        public IDataQuery DataQuery
        { get; set; }
    }
}
