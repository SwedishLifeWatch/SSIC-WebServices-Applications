using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class used for deserializing results from Elastisearch 
    /// </summary>
    internal class ElasticsearchHits
    {
        public long total { get; set; }
        public long max_score { get; set; }
        public IList<object> hits { get; set; }
    }
}
