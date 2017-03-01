using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    public class ElasticsearchTerm
    {
        public ElasticsearchTerm(string term)
        {
            SortOrder = ElasticsearchSortOrder.Ascending;
            SortType = ElasticsearchSortType.Term;
            Term = term;
            TermType = ElasticsearchTermType.Field;
            Top = 0;
        }

        /// <summary>
        /// The type of order to use when assembling the aggregate string
        /// </summary>
        public ElasticsearchSortOrder SortOrder { get; set; }

        /// <summary>
        /// The type of sorting to use when assembling the aggregate string
        /// </summary>
        public ElasticsearchSortType SortType { get; set; }

        /// <summary>
        /// The search term, could be either a field name or a script to calculate a 'field value'
        /// </summary>
        public string Term { get; private set; }

        /// <summary>
        /// The type of term to use when assembling the aggregate string
        /// </summary>
        public ElasticsearchTermType TermType { get; set; }

        /// <summary>
        /// Limits the number of returned rows (set to 0 zero to get all rows, that's also the defalt value)
        /// Should be used with caution, Elasticsearch dosument counts are approximate and there's a possibility that the returned values are not the top X spread over all of the Elasticsearch shards!
        /// See also https://www.elastic.co/guide/en/elasticsearch/reference/current/search-aggregations-bucket-terms-aggregation.html
        /// </summary>
        public int Top { get; set; }
    }
}
