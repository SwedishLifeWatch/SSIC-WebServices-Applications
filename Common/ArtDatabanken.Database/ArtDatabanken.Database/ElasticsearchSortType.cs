using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Establish which type of sorting to use when assembling an aggregate string
    /// </summary>
    public enum ElasticsearchSortType
    {
        /// <summary>Sort by the term values</summary>
        Term,

        /// <summary>Sort by document count</summary>
        DocumentCount
    }
}
