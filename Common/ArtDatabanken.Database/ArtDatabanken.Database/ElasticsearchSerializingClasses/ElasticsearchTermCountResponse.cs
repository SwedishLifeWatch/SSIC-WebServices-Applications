using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class used for deserializing results from Elastisearch 
    /// </summary>
    internal class ElasticsearchTermCountResponse
    {
        public int took { get; set; }
        public bool timed_out { get; set; }
        public ElasticsearchShards _shards { get; set; }
        public ElasticsearchHits hits { get; set; }
        public ElasticsearchTermCountAggregation aggregations { get; set; }
    }
}
