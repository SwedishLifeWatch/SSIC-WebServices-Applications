using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching taxonnames.
    /// </summary>
    public interface ITaxonNameSearchCriteria
    {
        /// <summary>
        /// Author search string.
        /// </summary>
        IStringSearchCriteria AuthorSearchString { get; set; }

        /// <summary>
        /// Search for taxon names with this taxon name category.
        /// </summary>
        ITaxonNameCategory Category { get; set; }

        /// <summary>
        /// "Author" is included in NameSearchString  - value true
        /// "Author" is not included in NameSearchString  - value false
        /// </summary>
        Boolean IsAuthorIncludedInNameSearchString { get; set; }

        /// <summary>
        /// Filter taxon names based on if they are ok 
        /// for species observation systems or not.
        /// </summary>
        Boolean? IsOkForSpeciesObservation { get; set; }

        /// <summary>
        /// Find names that are "original" - value true
        /// Find names that are "not original" - value false
        /// </summary>
        Boolean? IsOriginalName { get; set; }

        /// <summary>
        /// Find names that are "recommended" - value true
        /// Find names that are "not recommended" - value false
        /// </summary>
        Boolean? IsRecommended { get; set; }

        /// <summary>
        /// Find names that are "unique" - value true
        /// Find names that are "not unique" - value false
        /// </summary>
        Boolean? IsUnique { get; set; }

        /// <summary>
        /// Restrict to names to taxa that are valid - value true
        /// </summary>
        Boolean? IsValidTaxon { get; set; }

        /// <summary>
        /// Restrict to names that are valid - value true
        /// </summary>
        Boolean? IsValidTaxonName { get; set; }

        /// <summary>
        /// Search taxon names updated between LastModifiedStartDate and
        /// LastModifiedEndDate if both are set.
        /// </summary>                
        DateTime? LastModifiedStartDate { get; set; }

        /// <summary>
        /// Search taxon names updated between LastModifiedStartDate and
        /// LastModifiedEndDate if both are set.
        /// </summary>                
        DateTime? LastModifiedEndDate { get; set; }

        /// <summary>
        /// The name search string.
        /// </summary>
        IStringSearchCriteria NameSearchString { get; set; }

        /// <summary>
        /// Search for taxon names with this taxon name status.
        /// </summary>
        ITaxonNameStatus Status { get; set; }

        /// <summary>
        /// List of taxon ids - restrict search with this list as parent taxa
        /// </summary>
        List<Int32> TaxonIds { get; set; }
    }
}
