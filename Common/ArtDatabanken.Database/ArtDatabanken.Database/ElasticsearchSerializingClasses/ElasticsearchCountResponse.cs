using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class used for deserializing results from Elastisearch 
    /// </summary>
    internal class ElasticsearchCountResponse
    {
        public long count { get; set; }
        public ElasticsearchShards _shards { get; set; }
    }
}
