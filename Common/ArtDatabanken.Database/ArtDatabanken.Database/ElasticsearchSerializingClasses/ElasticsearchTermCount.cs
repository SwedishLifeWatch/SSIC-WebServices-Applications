using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class used for deserializing results from Elastisearch 
    /// </summary>
    internal class ElasticsearchTermCount
    {
        public long doc_count_error_upper_bound { get; set; }
        public long sum_other_doc_count { get; set; }
        public IList<ElasticsearchTermCountBucket> buckets { get; set; }
    }
}
