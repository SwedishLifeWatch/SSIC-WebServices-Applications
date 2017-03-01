using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching taxon revisions.
    /// </summary>
    public class TaxonRevisionSearchCriteria : ITaxonRevisionSearchCriteria
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Find revisions who have RevisionStateIds related
        /// to a specified revision state.
        /// </summary>
        public List<Int32> StateIds { get; set; }

        /// <summary>
        /// Find taxa who have taxonIds related
        /// to the specified revisin.
        /// </summary>
        public List<Int32> TaxonIds { get; set; }
    }
}
