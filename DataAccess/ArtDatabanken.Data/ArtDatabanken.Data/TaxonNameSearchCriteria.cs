using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching taxonnames.
    /// </summary>
    public class TaxonNameSearchCriteria : ITaxonNameSearchCriteria
    {
        /// <summary>
        /// The author search string.
        /// </summary>
        public IStringSearchCriteria AuthorSearchString
        { get; set; }

        /// <summary>
        /// The name search string.
        /// </summary>
        public IStringSearchCriteria NameSearchString
        { get; set; }

        /// <summary>
        /// List id taxon id - restrict search with this list as parent taxa
        /// </summary>
        public List<Int32> TaxonIds
        { get; set; }

        /// <summary>
        /// Search for taxonnames with this namecategory id.
        /// </summary>
        public ITaxonNameCategory Category
        { get; set; }

        /// <summary>
        /// Search for taxonnames with this id of NameUsage.
        /// </summary>
        public ITaxonNameStatus Status
        { get; set; }

        /// <summary>
        /// "Author" is included in NameSearchString  - value true
        /// "Author" is not included in NameSearchString  - value false
        /// </summary>
        public Boolean IsAuthorIncludedInNameSearchString
        { get; set; }

        /// <summary>
        /// Find names that are "ok for Obs systems" - value true
        /// Find names that are "not ok for Obs systems" - value false
        /// </summary>
        public Boolean? IsOkForSpeciesObservation
        { get; set; }

        /// <summary>
        /// Find names that are "original" - value true
        /// Find names that are "not original" - value false
        /// </summary>
        public Boolean? IsOriginalName
        { get; set; }

        /// <summary>
        /// Find names that are "recommended" - value true
        /// Find names that are "not recommended" - value false
        /// </summary>
        public Boolean? IsRecommended
        { get; set; }

        /// <summary>
        /// Find names that are "unique" - value true
        /// Find names that are "not unique" - value false
        /// </summary>
        public Boolean? IsUnique
        { get; set; }

        /// <summary>
        /// Restrict to names to taxa that are valid - value true
        /// </summary>
        public Boolean? IsValidTaxon
        { get; set; }

        /// <summary>
        /// Restrict to names that are valid - value true
        /// </summary>
        public Boolean? IsValidTaxonName
        { get; set; }

        /// <summary>
        /// Search taxon names updated between LastModifiedStartDate and
        /// LastModifiedEndDate if both are set.
        /// </summary>                
        public DateTime? LastModifiedStartDate 
        { get; set; }

        /// <summary>
        /// Search taxon names updated between LastModifiedStartDate and
        /// LastModifiedEndDate if both are set.
        /// </summary>                
        public DateTime? LastModifiedEndDate 
        { get; set; }

    }
}
