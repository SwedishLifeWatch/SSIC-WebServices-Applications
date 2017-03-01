using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Search criterias used when retrieving taxon trees.
    /// </summary>
    public interface ITaxonTreeSearchCriteria
    {
        /// <summary>
        /// Indicates if only valid taxon relations and taxa
        /// should be included in returned trees.
        /// </summary>
        Boolean IsValidRequired
        { get; set; }

        /// <summary>
        /// Scope for this taxon tree search.
        /// Defines if relations above or beneath specified taxons
        /// should be returned.
        /// This property is not relevant if property TaxonIds
        /// contains no values.
        /// </summary>
        TaxonTreeSearchScope Scope
        { get; set; }

        /// <summary>
        /// These taxa are used to limit the returned taxon trees.
        /// </summary>
        List<Int32> TaxonIds
        { get; set; }
    }
}
