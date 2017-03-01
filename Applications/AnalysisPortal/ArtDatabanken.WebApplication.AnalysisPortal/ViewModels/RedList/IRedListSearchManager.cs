//using System.Collections.Generic;
//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa;

////using RedList.Data.ViewModels.Search;
////using RedList.Data.ViewModels.Taxon;
////using RedList.Data.Enums;

//namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
//{
//    /// <summary>
//    /// This class is used to search for taxa in different ways.
//    /// </summary>
//    public interface IRedListSearchManager
//    {
//        /// <summary>
//        /// Get search criteria view model from database or cache.
//        /// </summary>
//        /// <returns>View model.</returns>
//        SearchViewModel GetSearchViewModel();

//        /// <summary>
//        /// Get list of taxon information by user selections.
//        /// </summary>
//        /// <param name="model">Search view model.</param>
//        /// <param name="allTaxa"></param>
//        /// <returns>List of taxon information.</returns>
//        List<TaxonListInformation> SearchTaxa(SearchViewModel model, List<int> allTaxa = null);

//        /// <summary>
//        /// Searches for taxa using the give search options.
//        /// </summary>
//        /// <param name="searchOptions">The search options to use in the search.</param>
//        /// <returns>List of taxon information view models.</returns>
//        List<TaxonSearchResultItemViewModel> SearchTaxaByNameOrId(TaxonSearchOptions searchOptions);

//        /// <summary>
//        /// Searches for taxa using the give search options (without using sessionsettings)
//        /// </summary>
//        /// <param name="searchOptions">The search options to use in the search.</param>
//        /// <returns>List of taxon information view models.</returns>
//        List<TaxonSearchResultItemViewModel> SearchTaxaByNameOrIdWithoutSessionSettings(TaxonSearchOptions searchOptions);

//        /// <summary>
//        /// Set Current Search Criteria Session Values.
//        /// </summary>
//        /// <param name="model">Contains user selections to be stored in session.</param>
//        /// <returns>True means success.</returns>
//        bool SetCurrentSearchCriteriasSessionValues(SearchViewModel model);

//        /// <summary>
//        /// Clear Current Search Criteria Session Values.
//        /// </summary>
//        /// <returns>True means success.</returns>
//        bool ClearCurrentSearchCriteriasSessionValues();

//        /// <summary>
//        /// Set Selected Taxa Session Values.
//        /// </summary>
//        /// <param name="model">Contains user selections to be stored in session.</param>
//        /// <returns>True means success.</returns>
//        bool SetSelectedTaxaSessionValues(List<TaxonListInformation> model);

//        /// <summary>
//        /// Clear Selected Taxa Session Values.
//        /// </summary>
//        /// <returns>True means success.</returns>
//        bool ClearSelectedTaxaSessionValues();

//        /// <summary>
//        /// Set View Model Selected Taxa Session Values.
//        /// </summary>
//        /// <param name="model">Contains current list of taxon information to be stored in session.</param>
//        /// <returns>True means success.</returns>
//        bool SetViewModelSelectedTaxaSessionValues(List<TaxonListInformation> model);

//        /// <summary>
//        /// Clear View Model Selected Taxa Session Values.
//        /// </summary>
//        /// <returns>True means success.</returns>
//        bool ClearViewModelSelectedTaxaSessionValues();

//        /// <summary>
//        /// Get Search View Model From Session.
//        /// </summary>
//        /// <returns>Search View Model or null.</returns>
//        SearchViewModel GetSearchViewModelFromSession();

//        /// <summary>
//        /// Gets a hash set list of all taxa ids of type "Arter" "Småarter" and "Underarter".
//        /// </summary>
//        /// <returns>
//        /// A hash set of all valid taxon ids
//        /// </returns>
//        HashSet<int> GetAllValidTaxaIds();

//        /// <summary>
//        /// Get all invalid taxa
//        /// </summary>
//        /// <returns></returns>
//        HashSet<int> GetAllInvalidTaxaIds();

//        /// <summary>
//        /// Check if any search criteria enabled and selected.
//        /// </summary>
//        /// <param name="model">
//        /// The model.
//        /// </param>
//        /// <returns>
//        /// True if any search criteria selected. Otherwise return false.
//        /// </returns>
//        bool IsAnySearchCriteriaSelected(SearchViewModel model);

//        /// <summary>
//        /// Get current search result taxa From Session.
//        /// </summary>
//        /// <returns>List of taxon information or null.</returns>
//        List<TaxonListInformation> GetSelectedTaxaListModelFromSession();

//        /// <summary>
//        /// Get current selected taxa From Session.
//        /// </summary>
//        /// <returns>List of taxon information or null.</returns>
//        List<TaxonListInformation> GetViewModelSelectedTaxaListModelFromSession();

//        /// <summary>
//        /// Gets the filtersearch string
//        /// </summary>
//        /// <returns></returns>
//        string GetCurrentFilterSearchStringFromSession();

//        /// <summary>
//        /// Sets the filtersearch string
//        /// </summary>s
//        /// <param name="filterSearchString"></param>
//        void SetCurrentFilterSearchStringInSession(string filterSearchString);

//        /// <summary>
//        /// Sets the namesearch viewmodel
//        /// </summary>
//        /// <param name="model"></param>
//        void SetNameSearchViewModelInSession(List<TaxonSearchResultItemViewModel> model);

//        /// <summary>
//        /// Gets the namesearch viewmodel
//        /// </summary>
//        List<TaxonSearchResultItemViewModel> GetNameSearchViewModelFromSession();

//        /// <summary>
//        /// Gets the resultviewtype
//        /// </summary>
//        /// <returns></returns>
//        ResultViewType GetResultViewTypeFromSession();

//        /// <summary>
//        /// Sets the resultviewtype in the session
//        /// </summary>
//        void SetResultViewTypeInSession(ResultViewType resultViewType);

//        /// <summary>
//        /// Gets the underlying taxa.
//        /// </summary>
//        /// <param name="parentTaxonId">The parent taxon identifier.</param>
//        /// <returns>List of <see cref="TaxonListInformation"/> Child taxa list.</returns>
//        TaxonList GetUnderlyingTaxa(int parentTaxonId);

//        /// <summary>
//        /// Gets the underlying taxa.
//        /// </summary>
//        /// <param name="parentTaxon">The parent taxon.</param>
//        /// <returns>List of <see cref="TaxonListInformation"/> Child taxa list.</returns>
//        TaxonList GetUnderlyingTaxa(ITaxon parentTaxon);

//        /// <summary>
//        /// Gets the underlying taxa containing red list category factor.
//        /// The parent taxon is also included if it have red list category factor.
//        /// </summary>
//        /// <param name="parentTaxonId">The parent taxon identifier.</param>
//        /// <returns>List of taxa that have red list category factor.</returns>
//        List<TaxonListInformation> GetUnderlyingTaxaFromScope(int parentTaxonId);

//        /// <summary>
//        /// Tries to find a taxon by either Taxon id or by taxon name search.
//        /// </summary>
//        /// <param name="id">The identifier. Id or name.</param>
//        /// <returns>A search result model which shows whether a taxon was found and how it was found.</returns>
//        TaxonLookupResultViewModel LookupTaxon(string id);

//        /// <summary>
//        /// Tries to find a taxon by either Taxon id or by taxon name search.
//        /// </summary>
//        /// <param name="taxonName"></param>
//        /// <returns>A search result model which shows whether a taxon was found and how it was found.</returns>
//        TaxonLookupResultViewModel LookupTaxonFromNameWithoutSessionSettings(string taxonName);

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="taxonId"></param>
//        /// <returns></returns>
//        bool IsTaxonInScope(int taxonId);
//    }

//}
