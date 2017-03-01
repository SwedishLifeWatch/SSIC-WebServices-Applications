using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Response when retrieving unique values combined with the count and also the document count from Elasticsearch.
    /// </summary>
    public class DocumentUniqueCountSumResponse
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
        /// List of unique field values that matches search criteria, combined to a key, and the individual count and also the document count.
        /// </summary>
        public List<DocumentUniqueValue> UniqueValues;
    }
}
