using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Establish which type of term to use when assembling an aggregate string
    /// </summary>
    public enum ElasticsearchTermType
    {
        /// <summary>The search term is a field(column) name</summary>
        Field,

        /// <summary>The search term is a script, used to calculate a 'field value'</summary>
        Script
    }

}
