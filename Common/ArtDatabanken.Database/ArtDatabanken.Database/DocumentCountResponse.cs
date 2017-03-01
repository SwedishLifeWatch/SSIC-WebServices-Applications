using System;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Response when retrieving species observation count from Elasticsearch.
    /// </summary>
    public class DocumentCountResponse
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
        /// Number of documents that matchs search criteria.
        /// </summary>
        public Int64 DocumentCount;
    }
}
