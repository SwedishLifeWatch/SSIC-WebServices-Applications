using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class used for deserializing results from Elastisearch 
    /// </summary>
    internal class ElasticsearchTermCountBucket
    {
        public object key { get; set; }
        public long doc_count { get; set; }

        public ElasticsearchTermCount term_count { get; set; }
    }
}
