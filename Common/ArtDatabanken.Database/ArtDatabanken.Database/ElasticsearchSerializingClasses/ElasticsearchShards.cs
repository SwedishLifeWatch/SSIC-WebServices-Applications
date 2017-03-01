using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Class used for deserializing results from Elastisearch 
    /// </summary>
    internal class ElasticsearchShards
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int failed { get; set; }
    }
}
