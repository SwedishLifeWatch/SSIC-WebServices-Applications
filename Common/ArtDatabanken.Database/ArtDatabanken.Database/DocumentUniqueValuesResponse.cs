using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Response when retrieving unique field values from Elasticsearch.
    /// </summary>
    public class DocumentUniqueValuesResponse
    {
        /// <summary>
        /// Number of failed shards.
        /// </summary>
        public Int32 ShardFailedCount;

        /// <summary>
        /// Number of successful shards.
        /// </summary>
        public Int32 ShardSuccessfulCount;

        /// <summary>
        /// Number of shards.
        /// </summary>
        public Int32 ShardTotalCount;

        /// <summary>
        /// List of unique field values that matches search criteria, combined to a key, and their individual count.
        /// </summary>
        public Dictionary<string, long> UniqueValues;
    }
}
