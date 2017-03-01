using System;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class that contains scroll information when documents
    /// in Elasticsearch are scrolled through.
    /// </summary>
    public class ElasticsearchScroll
    {
        /// <summary>
        /// Number of minutes that the search context should be keept alive.
        /// Next call to this scroll must be executed before this time limit.
        /// </summary>
        public Int32 KeepAlive { get; set; }

        /// <summary>
        /// Id of the scroll.
        /// This value is changed after each scroll call.
        /// </summary>
        public String ScrollId { get; set; }
    }
}
