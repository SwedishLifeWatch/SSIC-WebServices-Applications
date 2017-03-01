using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// taxon searching revisions.
    /// </summary>
    public interface ITaxonRevisionSearchCriteria
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Find revisions who have RevisionStateIds related
        /// to a specified revision state.
        /// </summary>
        List<Int32> StateIds { get; set; }

        /// <summary>
        /// Find taxa who have taxonIds related
        /// to the specified revision.
        /// </summary>
        List<Int32> TaxonIds { get; set; }
    }
}
