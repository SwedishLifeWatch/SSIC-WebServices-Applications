using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>    
    /// A search result model which shows whether a taxon was found and how it was found.
    /// </summary>
    public class TaxonLookupResultViewModel
    {
        /// <summary>
        /// Indicates that eactly one taxon is found.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exactly one taxon is found; otherwise, <c>false</c>.
        /// </value>
        public bool OneTaxonIsFound { get; set; }

        /// <summary>
        /// The taxon Id if exactly one taxon is found.
        /// </summary>        
        public int TaxonId { get; set; }

        /// <summary>
        /// The type of the search parameter. Id or name.
        /// </summary>        
        public TaxonLookupParameterType TaxonLookupParameterType { get; set; }

        /// <summary>
        /// Result type.
        /// </summary>        
        public TaxonLookupResultType TaxonLookupResultType { get; set; }

        /// <summary>
        /// Search result in name search.
        /// </summary>        
        public List<RedListTaxonSearchResultItemViewModel> SearchResult { get; set; }

        /// <summary>
        /// The taxon if one taxon is found.
        /// </summary>
        public ITaxon Taxon { get; set; }

        /// <summary>
        /// The taxon view model if found by name search.
        /// </summary>
        public RedListTaxonSearchResultItemViewModel TaxonSearchViewModel { get; set; }

        /// <summary>
        /// Error message describes why not exactly one taxon could be found.
        /// </summary>        
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Taxon lookup in parameter type.
    /// </summary>
    public enum TaxonLookupParameterType
    {
        TaxonId,
        TaxonName
    }

    /// <summary>
    /// Taxon lookup result type.
    /// </summary>
    public enum TaxonLookupResultType
    {
        SuccessTaxonIdFound,
        SuccessTaxonNameExactlyOneTaxonFound,
        SuccessTaxonNameOneTaxonWithExactNameFoundInMultipleTaxaResult,
        FailureTaxonIdNotFound,
        FailureTaxonNameNotFound,
        FailureTaxonNameMultipleTaxaMatch,
        FailureTaxonNameNotEnoughCharactersInSearch
    }
}
