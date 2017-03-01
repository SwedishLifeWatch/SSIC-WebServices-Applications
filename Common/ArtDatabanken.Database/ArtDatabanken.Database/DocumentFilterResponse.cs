using System;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Response when retrieving documents from Elasticsearch.
    /// </summary>
    public class DocumentFilterResponse
    {
        /// <summary>
        /// Elapsed time when species observations was retrived.
        /// Unit is millisecond.
        /// </summary>
        public Int32 ElapsedTime;

        /// <summary>
        /// indicates how well the response matches the request.
        /// </summary>
        public Double MaxScore;

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
        /// Number of retrieved documents.
        /// </summary>
        public Int64 DocumentCount;

        /// <summary>
        /// Retrieved documents in Json format.
        /// </summary>
        public String DocumentsJson;

        /// <summary>
        /// Indicates if operation timed out.
        /// </summary>
        public Boolean TimedOut;
    }
}
