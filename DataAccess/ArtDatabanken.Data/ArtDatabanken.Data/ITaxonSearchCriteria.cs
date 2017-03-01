using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching taxon.
    /// </summary>
    public interface ITaxonSearchCriteria
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Restrict to names to taxa that are valid - value true
        /// </summary>
        Boolean? IsValidTaxon { get; set; }

        /// <summary>
        /// Taxon search scope.
        /// AllChildTaxa or AllParentTaxa
        /// </summary>
        TaxonSearchScope Scope { get; set; }

        /// <summary>
        /// Find taxa who have "Swedish immigration history" value
        /// set to one on the int values in this list.
        /// </summary>
        List<Int32> SwedishImmigrationHistory { get; set; }

        /// <summary>
        /// Find taxa who have "Swedish occurrence" value
        /// set to one on the int values in this list.
        /// </summary>
        List<Int32> SwedishOccurrence { get; set; }

        /// <summary>
        /// Find taxa who have taxonCategoryIds related
        /// to a specified taxon.
        /// </summary>
        List<Int32> TaxonCategoryIds
        { get; set; }

        /// <summary>
        /// Find taxa who have taxonIds related
        /// to the specified taxon.
        /// </summary>
        List<Int32> TaxonIds
        { get; set; }

        /// <summary>
        /// Find taxon that belongs to taxon name that
        /// matches taxon name search string.
        /// Wildcard characters may be used.
        /// </summary>
        String TaxonNameSearchString
        { get; set; }
    }
}
