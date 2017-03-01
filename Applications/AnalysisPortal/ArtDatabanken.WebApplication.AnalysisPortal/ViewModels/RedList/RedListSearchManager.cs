using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using System.Web.Caching;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// This class is used to search for taxa in different ways.
    /// </summary>
    public class RedListSearchManager //: IRedListSearchManager
    {
        private readonly IUserContext mUserContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonSearchManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        public RedListSearchManager(IUserContext userContext)
        {
            mUserContext = userContext;
        }

        ///// <summary>
        ///// Get search criteria view model from database or cache.
        ///// </summary>
        ///// <returns>View model.</returns>
        //public SearchViewModel GetSearchViewModel()
        //{
        //    // If values are stored in session, update current values
        //    SearchViewModel searchViewModelFromSession = GetSearchViewModelFromSession();
        //    if (searchViewModelFromSession.IsNull())
        //    {
        //        searchViewModelFromSession = new SearchViewModel();
        //    }

        //    var searchViewModel = new SearchViewModel();
        //    searchViewModel.InitSearchViewModel(mUserContext, searchViewModelFromSession);
        //    searchViewModel.InitSwedishOccurrenceInformation();
        //    searchViewModel.InitBiotopeInformation();
        //    searchViewModel.InitCountyOccurrencesInformation();
        //    searchViewModel.InitThematicListInformation();
        //    searchViewModel.InitRedListCategories();
        //    searchViewModel.InitRedListTaxonCategories();
        //    searchViewModel.InitTaxonScope();
        //    searchViewModel.InitOrganismGroups();
        //    searchViewModel.InitLandscapeTypes();
        //    searchViewModel.InitSubstrate();
        //    searchViewModel.InitImpact();
        //    searchViewModel.InitLifeForms();
        //    searchViewModel.InitHost();

        //    return searchViewModel;
        //}

        /// <summary>
        /// Get list of taxon information by user selections.
        /// </summary>
        /// <param name="model">Search view model.</param>
        /// <param name="allTaxa"></param>
        /// <returns>List of taxon information.</returns>
        public List<TaxonListInformation> SearchTaxa(SearchViewModel model, List<int> allTaxa)
        {
            if (model == null)
            {
                return null;
            }

            List<TaxonListInformation> taxonListInformations = null;

            if (allTaxa == null)
            {
                bool useNonCategorizedTaxaIdOnly;

                IList<RedListCategoryItemViewModel> selectedCategories;
                TaxonIdList taxonIds = GetTaxaBySearchCriteria(
                    model,
                    out useNonCategorizedTaxaIdOnly,
                    out selectedCategories);

                taxonListInformations = TaxonListInformationManager.Instance.GetTaxonListInformation(
                        taxonIds,
                        useNonCategorizedTaxaIdOnly,
                        selectedCategories);
            }
            else
            {
                var resList = new TaxonIdList();
                foreach (var id in allTaxa)
                {
                    resList.Add(new TaxonIdImplementation(id));
                }

                taxonListInformations = TaxonListInformationManager.Instance.GetTaxonListInformation(
                    resList,
                    false,
                    null);
            }

            return taxonListInformations;
        }

        ///// <summary>
        ///// Check dataselection and if only red listed is selected then check if taxa has factor 743/red listed. 
        ///// Remove other taxa then from result depending on data selection LC/NE/NA taxa could be included or excluded. 
        ///// </summary>
        ///// <param name="result">
        ///// Result taxa list from taxa selection search.
        ///// </param>
        //private void RemoveTaxaNotRedlistedOrRedlistedEnsuredFromList(List<TaxonListInformation> result)
        //{
        //    // Make a copy of the list
        //    var tempResult = new List<TaxonListInformation>();
        //    if (result.IsNotEmpty())
        //    {
        //        tempResult.AddRange(result);
        //    }

        //    // Check dataselection and if only redlisted is selected then check if taxa has factor 743/redlisted and is not of type 
        //    // LC/NE/NA. Remove other taxa then from result. 
        //    if (SessionHandler.UseOnlyRedlistedData)
        //    {
        //        foreach (TaxonListInformation taxonListInformation in tempResult.Where(taxonListInformation => !taxonListInformation.IsRedListed))
        //        {
        //            result.RemoveAll(x => x.Id == taxonListInformation.Id);
        //        }
        //    }
        //}

        /// <summary>
        /// Get taxa by user selections.
        /// </summary>
        /// <param name="model">
        /// Search view model.
        /// </param>
        /// <param name="useNonCategorizedTaxaIdOnly">
        /// Indicates if only taxa without category red listed (743) set.
        /// </param>
        /// <param name="selectedCategories"> returns selected categories to filter on if selection of non categorized data is choosen to be viewed.</param>
        /// <returns>
        /// List of taxon ids.
        /// </returns>
        private TaxonIdList GetTaxaBySearchCriteria(SearchViewModel model, out bool useNonCategorizedTaxaIdOnly, out IList<RedListCategoryItemViewModel> selectedCategories)
        {
            var searchCriteria = new AnalysisSearchCriteria();

            bool onlyNonCategorizedTaxaIdUsed = false;
            useNonCategorizedTaxaIdOnly = false;
            selectedCategories = null;

            if (model.RedListCategories.IsNotEmpty())
            {
                // Get number of selected 
                selectedCategories = model.RedListCategories.Where(item => item.Selected).ToList();
                const int NonCategorizedTaxaId = 1000;

                // Check if non categorizedtaxa is the selected then we have to get all taxa.
                if (model.RedListCategories.Any(redListCategory =>
                    (redListCategory.Id == NonCategorizedTaxaId) && redListCategory.Selected))
                {
                    onlyNonCategorizedTaxaIdUsed = true;
                    useNonCategorizedTaxaIdOnly = true;
                }
            }

            if (!onlyNonCategorizedTaxaIdUsed)
            {
                searchCriteria.InitRedListCategories(model);
            }
            else
            {
                // Getting alla taxa for all categories; must filter below on data not categorized.
                searchCriteria.RedListCategories = null;
            }

            //// Red list taxon categories / (Arter/Småarter/Underarter)
            //searchCriteria.InitRedListTaxonCategories(model);

            //// Scope 
            //searchCriteria.InitTaxonScope(model);

            //// Swedish occurrence
            //searchCriteria.InitSwedishOccurrenceInformation(model);

            //// Organism / Organismgrupp
            //searchCriteria.InitOrganismGroups(model);

            //// Landscape / Landskapstyp
            //searchCriteria.InitLandscapeTypes(model);

            //// County / Län
            //searchCriteria.InitCountyOccurrencesInformation(model);

            //// Biotope / Biotop
            //searchCriteria.InitBiotopeInformation(model);

            //// Substrate / Substrat
            //searchCriteria.InitSubstrate(model);

            //// Impact / Påverkan
            //searchCriteria.InitImpact(model);

            //// Lifeforms / Livsform
            //searchCriteria.InitLifeForms(model);

            //// Host / Värd
            //searchCriteria.InitHost(model);

            //// Thematic listing / Tematisk lista
            //searchCriteria.InitThematicListInformation(model);

            // Fetch the taxonids
            TaxonIdList taxonIds = CoreData.AnalysisManager.GetTaxonIds(searchCriteria);

            return taxonIds;
        }

        ///// <summary>
        ///// Searches for taxa using the give search options.
        ///// </summary>
        ///// <param name="searchOptions">The search options to use in the search.</param>
        ///// <returns>List of taxon information view models.</returns>
        //public List<TaxonSearchResultItemViewModel> SearchTaxaByNameOrId(TaxonSearchOptions searchOptions)
        //{
        //    int taxonIdValue;
        //    var resultList = int.TryParse(searchOptions.NameSearchString, out taxonIdValue) ?
        //        GetTaxaById(taxonIdValue, true) :
        //        GetTaxaByName(searchOptions);

        //    return resultList;
        //}

        ///// <summary>
        ///// Searches for taxa using the give search options (without using sessionsettings)
        ///// </summary>
        ///// <param name="searchOptions">The search options to use in the search.</param>
        ///// <returns>List of taxon information view models.</returns>
        //public List<TaxonSearchResultItemViewModel> SearchTaxaByNameOrIdWithoutSessionSettings(TaxonSearchOptions searchOptions)
        //{
        //    int taxonIdValue;
        //    List<TaxonSearchResultItemViewModel> resultList = int.TryParse(searchOptions.NameSearchString, out taxonIdValue) ?
        //        GetTaxaById(taxonIdValue, true) :
        //        GetTaxaByNameWithoutSessionSettings(searchOptions);

        //    return resultList;
        //}

        ///// <summary>
        ///// Gets taxa by using the searchstring (as id number)
        ///// </summary>
        ///// <param name="taxonId"></param>
        ///// <param name="onlyValidTaxa"></param>
        ///// <returns></returns>
        //private List<TaxonSearchResultItemViewModel> GetTaxaById(int taxonId, bool onlyValidTaxa = false)
        //{
        //    var resultList = new List<TaxonSearchResultItemViewModel>();
        //    var taxonFoundById = SearchTaxonById(taxonId);

        //    if (onlyValidTaxa)
        //    {
        //        if (taxonFoundById != null && taxonFoundById.IsValid)
        //        {
        //            resultList.Add(TaxonSearchResultItemViewModel.CreateFromTaxon(taxonFoundById, taxonId));
        //        }
        //    }
        //    else
        //    {
        //        resultList.Add(TaxonSearchResultItemViewModel.CreateFromTaxon(taxonFoundById, taxonId));
        //    }

        //    return resultList;
        //}

        ///// <summary>
        ///// Identifies taxa using the searchstring (as name)
        ///// </summary>
        ///// <param name="searchOptions"></param>
        ///// <returns></returns>
        //private List<TaxonSearchResultItemViewModel> GetTaxaByName(TaxonSearchOptions searchOptions)
        //{
        //    var resultList = new List<TaxonSearchResultItemViewModel>();

        //    if (searchOptions.NameSearchString.IsNotEmpty() &&
        //        (searchOptions.NameSearchString.Length > 2))
        //    {
        //        searchOptions.NameCompareOperator = SearchStringCompareOperator.Contains;
        //    }
        //    else
        //    {
        //        searchOptions.NameCompareOperator = SearchStringCompareOperator.BeginsWith;
        //    }

        //    var taxonNameSearchCriteria = searchOptions.CreateTaxonNameSearchCriteriaObject();
        //    var taxonNames = CoreData.TaxonManager.GetTaxonNames(mUserContext, taxonNameSearchCriteria);

        //    var taxonNameSearchResults = new TaxonNameSearchResultList();
        //    taxonNameSearchResults.AddRange(taxonNames);

        //    foreach (TaxonNameSearchResult taxonNameSearchResult in taxonNameSearchResults)
        //    {
        //        if (resultList.Count < searchOptions.Limit)
        //        {
        //            var taxonItemResultViewItemModel =
        //                TaxonSearchResultItemViewModel.CreateFromTaxonName(taxonNameSearchResult,
        //                    searchOptions.NameSearchString);

        //            if (taxonItemResultViewItemModel == null)
        //                continue;

        //            if (SessionHandler.UseOnlyRedlistedData)
        //            {
        //                if (taxonItemResultViewItemModel.IsRedListed)
        //                {
        //                    resultList.Add(taxonItemResultViewItemModel);
        //                }
        //            }
        //            else if (SessionHandler.UseSwedishOccurrence)
        //            {
        //                if (taxonItemResultViewItemModel.IsSwedish)
        //                {
        //                    resultList.Add(taxonItemResultViewItemModel);
        //                }
        //            }
        //            else
        //            {
        //                resultList.Add(taxonItemResultViewItemModel);
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    return resultList;
        //}

        ///// <summary>
        ///// Identifies all taxa using the searchstring (as name)
        ///// </summary>
        ///// <param name="searchOptions"></param>
        ///// <returns></returns>
        //private List<TaxonSearchResultItemViewModel> GetTaxaByNameWithoutSessionSettings(TaxonSearchOptions searchOptions)
        //{
        //    var resultList = new List<TaxonSearchResultItemViewModel>();

        //    if (searchOptions.NameSearchString.IsNotEmpty() &&
        //        (searchOptions.NameSearchString.Length > 2))
        //    {
        //        searchOptions.NameCompareOperator = SearchStringCompareOperator.Contains;
        //    }
        //    else
        //    {
        //        searchOptions.NameCompareOperator = SearchStringCompareOperator.BeginsWith;
        //    }

        //    var taxonNameSearchCriteria = searchOptions.CreateTaxonNameSearchCriteriaObject();
        //    var taxonNames = CoreData.TaxonManager.GetTaxonNames(mUserContext, taxonNameSearchCriteria);

        //    var taxonNameSearchResults = new TaxonNameSearchResultList();
        //    taxonNameSearchResults.AddRange(taxonNames);

        //    foreach (TaxonNameSearchResult taxonNameSearchResult in taxonNameSearchResults)
        //    {
        //        if (resultList.Count < searchOptions.Limit)
        //        {
        //            var taxonItemResultViewItemModel =
        //                TaxonSearchResultItemViewModel.CreateFromTaxonName(taxonNameSearchResult,
        //                    searchOptions.NameSearchString);

        //            if (taxonItemResultViewItemModel == null)
        //                continue;

        //            resultList.Add(taxonItemResultViewItemModel);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }

        //    return resultList;
        //}

        /// <summary>
        /// Gets a hash set list of all taxa ids of type "Arter" "Småarter" and "Underarter".
        /// </summary>
        /// <returns>
        /// A hash set of taxon id which is red listed.
        /// </returns>
        public HashSet<int> GetAllValidTaxaIds()
        {
            var cachedObject = AspNetCache.GetCachedObject("allValidTaxaIds");
            if (cachedObject != null)
            {
                return (HashSet<int>)cachedObject;
            }

            return UpdateValidTaxaIdListCache();
        }

        /// <summary>
        /// Update valid taxaidlist cache if not present
        /// </summary>
        /// <returns></returns>
        private HashSet<int> UpdateValidTaxaIdListCache()
        {
            ITaxonSearchCriteria searchCriteria = new TaxonSearchCriteria()
            {
                IsValidTaxon = true
            };

            var taxonList = CoreData.TaxonManager.GetTaxa(mUserContext, searchCriteria);

            var taxaListTaxonIdsSet = new HashSet<int>();
            foreach (var taxon in taxonList)
            {
                taxaListTaxonIdsSet.Add(taxon.Id);
            }

            AspNetCache.AddCachedObject(
                "allValidTaxaIds",
                taxaListTaxonIdsSet,
                DateTime.Now + new TimeSpan(0, 24, 0, 0),
                CacheItemPriority.Normal);

            return taxaListTaxonIdsSet;
        }

        /// <summary>
        /// Gets all invalid taxa ids
        /// </summary>
        /// <returns></returns>
        public HashSet<int> GetAllInvalidTaxaIds()
        {
            var cachedObject = AspNetCache.GetCachedObject("allInvalidTaxaIds");
            if (cachedObject != null)
            {
                return (HashSet<int>)cachedObject;
            }

            return UpdateInvalidTaxaIdListCache();
        }

        /// <summary>
        /// Update invalid taxaidlist cache if not present
        /// </summary>
        /// <returns></returns>
        private HashSet<int> UpdateInvalidTaxaIdListCache()
        {
            ITaxonSearchCriteria searchCriteria = new TaxonSearchCriteria()
            {
                IsValidTaxon = false
            };

            var taxonList = CoreData.TaxonManager.GetTaxa(mUserContext, searchCriteria);

            var taxaListTaxonIdsSet = new HashSet<int>();
            foreach (var taxon in taxonList)
            {
                taxaListTaxonIdsSet.Add(taxon.Id);
            }

            AspNetCache.AddCachedObject(
                "allInvalidTaxaIds",
                taxaListTaxonIdsSet,
                DateTime.Now + new TimeSpan(0, 24, 0, 0),
                CacheItemPriority.Normal);

            return taxaListTaxonIdsSet;
        }

        /// <summary>
        /// Searches for a taxon by TaxonId.
        /// </summary>
        /// <param name="taxonId">The id to search for</param>
        /// <returns>Taxon or null.</returns>
        private ITaxon SearchTaxonById(int taxonId)
        {
            return CoreData.TaxonManager.GetTaxon(mUserContext, taxonId);
        }

        ///// <summary>
        ///// Set Current Search Criteria Session Values.
        ///// </summary>
        ///// <param name="model">Contains user selections to be stored in session.</param>
        ///// <returns>True means success.</returns>
        //public bool SetCurrentSearchCriteriasSessionValues(SearchViewModel model)
        //{
        //    SessionHandler.CurrentSearchCriterias = model;
        //    return true;
        //}

        ///// <summary>
        ///// Clear Current Search Criteria Session Values.
        ///// </summary>
        ///// <returns>True means success.</returns>
        //public bool ClearCurrentSearchCriteriasSessionValues()
        //{
        //    SessionHandler.CurrentSearchCriterias = null;
        //    return true;
        //}

        ///// <summary>
        ///// Set Selected Taxa Session Values.
        ///// </summary>
        ///// <param name="model">Contains user selections to be stored in session.</param>
        ///// <returns>True means success.</returns>
        //public bool SetSelectedTaxaSessionValues(List<TaxonListInformation> model)
        //{
        //    SessionHandler.CurrentSelectedTaxa = model;
        //    return true;
        //}

        ///// <summary>
        ///// Clear Selected Taxa Session Values.
        ///// </summary>
        ///// <returns>True means success.</returns>
        //public bool ClearSelectedTaxaSessionValues()
        //{
        //    SessionHandler.CurrentSelectedTaxa = null;
        //    return true;
        //}

        ///// <summary>
        ///// Set View Model Selected Taxa Session Values.
        ///// </summary>
        ///// <param name="model">Contains current list of taxon information to be stored in session.</param>
        ///// <returns>True means success.</returns>
        //public bool SetViewModelSelectedTaxaSessionValues(List<TaxonListInformation> model)
        //{
        //    SessionHandler.CurrentViewModelSelectedTaxa = model;
        //    return true;
        //}

        ///// <summary>
        ///// Clear View Model Selected Taxa Session Values.
        ///// </summary>
        ///// <returns>True means success.</returns>
        //public bool ClearViewModelSelectedTaxaSessionValues()
        //{
        //    SessionHandler.CurrentViewModelSelectedTaxa = null;
        //    return true;
        //}

        ///// <summary>
        ///// Get Search View Model From Session.
        ///// </summary>
        ///// <returns>Search View Model or null.</returns>
        //public SearchViewModel GetSearchViewModelFromSession()
        //{
        //    if (SessionHandler.CurrentSearchCriterias.IsNotNull())
        //    {
        //        return SessionHandler.CurrentSearchCriterias;
        //    }

        //    return null;
        //}

        /// <summary>
        /// Check if any search criteria enabled and selected.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// True if any search criteria selected. Otherwise return false.
        /// </returns>
        public bool IsAnySearchCriteriaSelected(SearchViewModel model)
        {
            return (model.IsRedListCategoriesEnabled && model.RedListCategories.Any(r => r.Selected))
            || (model.IsSwedishOccurrenceEnabled && model.SwedishOccurrences.Any(r => r.Selected))
            || (model.IsRedListTaxonTypeEnabled && model.RedListTaxonTypes.Any(r => r.Selected))
            || (model.IsOrganismGroupsEnabled && model.OrganismGroups.Any(r => r.Selected))
            || (model.IsLandscapeTypesEnabled && model.LandscapeTypes.Any(r => r.Selected))
            || (model.IsCountyOccurrenceEnabled && model.CountyOccurrences.Any(r => r.Selected))
            || (model.IsBiotopeEnabled && model.Biotopes.Any(r => r.Selected))
            || (model.IsSubstrateEnabled && model.Substrates.Any(r => r.Selected))
            || (model.IsImpactEnabled && model.Impacts.Any(r => r.Selected))
            || (model.IsLifeFormEnabled && model.LifeForms.Any(r => r.Selected))
            || (model.IsHostEnabled && model.Hosts.Any(r => r.Selected))
            || (model.IsThematicListEnabled && model.ThematicLists.Any(r => r.Selected));
        }

        ///// <summary>
        ///// Get current search result taxa From Session.
        ///// </summary>
        ///// <returns>List of taxon information or null.</returns>
        //public List<TaxonListInformation> GetSelectedTaxaListModelFromSession()
        //{
        //    if (SessionHandler.CurrentSelectedTaxa.IsNotNull())
        //    {
        //        return SessionHandler.CurrentSelectedTaxa;
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// Get current selected taxa From Session.
        ///// </summary>
        ///// <returns>List of taxon information or null.</returns>
        //public List<TaxonListInformation> GetViewModelSelectedTaxaListModelFromSession()
        //{
        //    if (SessionHandler.CurrentViewModelSelectedTaxa.IsNotNull())
        //    {
        //        return SessionHandler.CurrentViewModelSelectedTaxa;
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// Gets the namesearch string
        ///// </summary>
        ///// <returns></returns>
        //public string GetCurrentFilterSearchStringFromSession()
        //{
        //    return SessionHandler.FilterSearchString;
        //}

        ///// <summary>
        ///// Sets the filtersearch string
        ///// </summary>
        ///// <param name="filterSearchString"></param>
        //public void SetCurrentFilterSearchStringInSession(string filterSearchString)
        //{
        //    SessionHandler.FilterSearchString = filterSearchString;
        //}

        ///// <summary>
        ///// Sets the viewmodel from the latest namesearch
        ///// </summary>
        ///// <param name="model"></param>
        //public void SetNameSearchViewModelInSession(List<TaxonSearchResultItemViewModel> model)
        //{
        //    SessionHandler.TaxonSearchViewModel = model;
        //}

        //public List<TaxonSearchResultItemViewModel> GetNameSearchViewModelFromSession()
        //{
        //    return SessionHandler.TaxonSearchViewModel;
        //}

        ///// <summary>
        ///// Gets the resultviewtype from the session
        ///// </summary>
        ///// <returns></returns>
        //public ResultViewType GetResultViewTypeFromSession()
        //{
        //    return SessionHandler.ResultViewType;
        //}

        ///// <summary>
        ///// Sets the resultviewtype in the session
        ///// </summary>
        //public void SetResultViewTypeInSession(ResultViewType resultViewType)
        //{
        //    SessionHandler.ResultViewType = resultViewType;
        //}

        ///// <summary>
        ///// Gets the underlying taxa. Checks for redlist occurence and swedish occurrence.
        ///// The parent taxon is also included if it have red list category factor. Only taxa with categories below and
        ///// equal to species are returned.
        ///// </summary>
        ///// <param name="parentTaxonId">The parent taxon identifier.</param>
        ///// <returns>List of <see cref="TaxonListInformation"/> that have red list category factor.</returns>
        //public List<TaxonListInformation> GetUnderlyingTaxaFromScope(int parentTaxonId)
        //{
        //    TaxonList childTaxa = GetUnderlyingTaxa(parentTaxonId);
        //    var taxonIdList = new TaxonIdList();

        //    // Select all valid taxa ids (from "artfakta db")
        //    HashSet<int> taxonIdsSet = GetAllValidTaxaIds();
        //    childTaxa.RemoveAll(y => !taxonIdsSet.Contains(y.Id));

        //    foreach (ITaxon taxon in childTaxa)
        //    {
        //        taxonIdList.Add(new TaxonIdImplementation(taxon.Id));
        //    }

        //    List<TaxonListInformation> taxonListInformations = TaxonListInformationManager.Instance.GetTaxonListInformation(taxonIdList, false, null);

        //    // Remove data if only redlisted is to be shown
        //    if (SessionHandler.UseOnlyRedlistedData)
        //    {
        //        // Remove other taxa then from result depending on data selection LC/NE/NA taxa could be included or excluded.
        //        RemoveTaxaNotRedlistedOrRedlistedEnsuredFromList(taxonListInformations);
        //    }

        //    // Check for SwedishOccurrence
        //    if (SessionHandler.UseSwedishOccurrence)
        //    {
        //        taxonListInformations = taxonListInformations.Where(x => x.SwedishOccurrenceId > AppSettings.Default.SwedishOccurrenceExist).ToList();
        //    }

        //    return taxonListInformations;
        //}

        /// <summary>
        /// Gets the underlying taxa.
        /// </summary>
        /// <param name="parentTaxonId">The parent taxon identifier.</param>
        /// <returns>List of taxa that have red list category factor.</returns>
        public TaxonList GetUnderlyingTaxa(int parentTaxonId)
        {
            ITaxon parentTaxon = CoreData.TaxonManager.GetTaxon(mUserContext, parentTaxonId);
            TaxonList taxonList = parentTaxon.GetChildTaxonTree(mUserContext, true).GetChildTaxa();
            taxonList.Insert(0, parentTaxon);

            return taxonList;
        }

        /// <summary>
        /// Gets the underlying taxa.
        /// </summary>
        /// <param name="parentTaxon">The parent taxon.</param>
        /// <returns>List of taxa that have red list category factor.</returns>
        public TaxonList GetUnderlyingTaxa(ITaxon parentTaxon)
        {
            TaxonList taxonList = parentTaxon.GetChildTaxonTree(mUserContext, true).GetChildTaxa();
            taxonList.Insert(0, parentTaxon);

            return taxonList;
        }

        ///// <summary>
        ///// Tries to find a taxon by either Taxon id or by taxon name search.
        ///// </summary>
        ///// <param name="taxonIdentifier">The identifier. Id or name.</param>
        ///// <returns>A search result model which shows whether a taxon was found and how it was found.</returns>
        //public TaxonLookupResultViewModel LookupTaxon(string taxonIdentifier)
        //{
        //    int taxonId;
        //    var resultModel = new TaxonLookupResultViewModel();

        //    if (int.TryParse(taxonIdentifier, out taxonId))
        //    {
        //        // Get taxon by Id.
        //        resultModel.TaxonLookupParameterType = TaxonLookupParameterType.TaxonId;
        //        try
        //        {
        //            // Taxon id is found.
        //            ITaxon taxon = CoreData.TaxonManager.GetTaxon(mUserContext, taxonId);
        //            resultModel.TaxonLookupResultType = TaxonLookupResultType.SuccessTaxonIdFound;
        //            resultModel.Taxon = taxon;
        //            resultModel.TaxonId = taxonId;
        //            resultModel.OneTaxonIsFound = true;
        //        }
        //        catch (Exception)
        //        {
        //            // Taxon id not found.
        //            resultModel.ErrorMessage = string.Format(RedListResource.TaxonLookupErrorTaxonIdNotFoundTemplate, taxonId);
        //            resultModel.TaxonLookupResultType = TaxonLookupResultType.FailureTaxonIdNotFound;
        //        }

        //        return resultModel;
        //    }

        //    BigInteger bigInteger;
        //    if (BigInteger.TryParse(taxonIdentifier, out bigInteger))
        //    {
        //        // Taxon id not found. Number too large.
        //        resultModel.ErrorMessage = string.Format(RedListResource.TaxonLookupErrorTaxonIdNotFoundTemplate, bigInteger);
        //        resultModel.TaxonLookupParameterType = TaxonLookupParameterType.TaxonId;
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.FailureTaxonIdNotFound;
        //        return resultModel;
        //    }

        //    // Get taxon by name.
        //    resultModel.TaxonLookupParameterType = TaxonLookupParameterType.TaxonName;

        //    // Search for taxon name.
        //    string nameSearchString = Uri.UnescapeDataString(taxonIdentifier);
        //    if (string.IsNullOrEmpty(nameSearchString) || nameSearchString.Length < 2)
        //    {
        //        // Not enough characters in search.
        //        resultModel.ErrorMessage = RedListResource.TaxonSearchMustEnterAtLeastTwoCharacters;
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.FailureTaxonNameNotEnoughCharactersInSearch;
        //        return resultModel;
        //    }

        //    List<TaxonSearchResultItemViewModel> result = SearchTaxaByName(nameSearchString);
        //    resultModel.SearchResult = result;
        //    if (result.IsEmpty() || result.Count == 0)
        //    {
        //        // No taxa is found in search.
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.FailureTaxonNameNotFound;
        //        resultModel.ErrorMessage = string.Format(RedListResource.TaxonLookupErrorTaxonNameNotFoundTemplate, nameSearchString);
        //        return resultModel;
        //    }

        //    if (result.IsNotEmpty() && result.Count == 1)
        //    {
        //        // Exactly one taxon is found.
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.SuccessTaxonNameExactlyOneTaxonFound;
        //        resultModel.TaxonId = result[0].TaxonId;
        //        resultModel.Taxon = CoreData.TaxonManager.GetTaxon(mUserContext, result[0].TaxonId);
        //        resultModel.TaxonSearchViewModel = result[0];
        //        resultModel.OneTaxonIsFound = true;

        //        return resultModel;
        //    }

        //    // Check if any taxon in list has exact name
        //    result = result.Where(r => r.SearchMatchName.ToLower().Equals(nameSearchString.ToLower())).ToList();

        //    if (result.IsNotEmpty() && result.Count == 1)
        //    {
        //        // One taxon match.
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.SuccessTaxonNameOneTaxonWithExactNameFoundInMultipleTaxaResult;
        //        resultModel.TaxonId = result[0].TaxonId;
        //        resultModel.Taxon = CoreData.TaxonManager.GetTaxon(mUserContext, result[0].TaxonId);
        //        resultModel.TaxonSearchViewModel = result[0];
        //        resultModel.OneTaxonIsFound = true;
        //        return resultModel;
        //    }

        //    // Multiple taxon match.
        //    resultModel.TaxonLookupResultType = TaxonLookupResultType.FailureTaxonNameMultipleTaxaMatch;
        //    resultModel.ErrorMessage = string.Format(RedListResource.TaxonLookupErrorMultipleTaxonMatchTemplate, resultModel.SearchResult.Count, nameSearchString);
        //    return resultModel;
        //}

        ///// <summary>
        ///// Lookup a taxon by name (without using sessionsettings)
        ///// </summary>
        ///// <param name="taxonIdentifier"></param>
        ///// <returns></returns>
        //public TaxonLookupResultViewModel LookupTaxonFromNameWithoutSessionSettings(string taxonIdentifier)
        //{
        //    var resultModel = new TaxonLookupResultViewModel
        //    {
        //        TaxonLookupParameterType = TaxonLookupParameterType.TaxonName
        //    };

        //    // Search for taxon name.
        //    string nameSearchString = Uri.UnescapeDataString(taxonIdentifier);
        //    if (string.IsNullOrEmpty(nameSearchString) || nameSearchString.Length < 2)
        //    {
        //        // Not enough characters in search.
        //        resultModel.ErrorMessage = RedListResource.TaxonSearchMustEnterAtLeastTwoCharacters;
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.FailureTaxonNameNotEnoughCharactersInSearch;
        //        return resultModel;
        //    }

        //    List<TaxonSearchResultItemViewModel> result = SearchTaxaByUrlName(taxonIdentifier);
        //    resultModel.SearchResult = result;
        //    if (result.IsEmpty() || result.Count == 0)
        //    {
        //        // No taxa is found in search.
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.FailureTaxonNameNotFound;
        //        resultModel.ErrorMessage = string.Format(RedListResource.TaxonLookupErrorTaxonNameNotFoundTemplate, nameSearchString);
        //        return resultModel;
        //    }

        //    if (result.IsNotEmpty() && result.Count == 1)
        //    {
        //        // Exactly one taxon is found.
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.SuccessTaxonNameExactlyOneTaxonFound;
        //        resultModel.TaxonId = result[0].TaxonId;
        //        resultModel.Taxon = CoreData.TaxonManager.GetTaxon(mUserContext, result[0].TaxonId);
        //        resultModel.TaxonSearchViewModel = result[0];
        //        resultModel.OneTaxonIsFound = true;

        //        return resultModel;
        //    }

        //    // Check if any taxon in list has exact name
        //    result = result.Where(r => r.SearchMatchName.ToLower().Equals(nameSearchString.ToLower())).ToList();

        //    // Filter out synonyms
        //    result = result.Where(r => !r.MatchIsSynonym).ToList();

        //    if (result.IsNotEmpty() && result.Count == 1)
        //    {
        //        // One taxon match.
        //        resultModel.TaxonLookupResultType = TaxonLookupResultType.SuccessTaxonNameOneTaxonWithExactNameFoundInMultipleTaxaResult;
        //        resultModel.TaxonId = result[0].TaxonId;
        //        resultModel.Taxon = CoreData.TaxonManager.GetTaxon(mUserContext, result[0].TaxonId);
        //        resultModel.TaxonSearchViewModel = result[0];
        //        resultModel.OneTaxonIsFound = true;
        //        return resultModel;
        //    }

        //    resultModel.TaxonLookupResultType = TaxonLookupResultType.FailureTaxonNameMultipleTaxaMatch;
        //    resultModel.ErrorMessage = string.Format(RedListResource.TaxonLookupErrorMultipleTaxonMatchTemplate, resultModel.SearchResult.Count, nameSearchString);
        //    return resultModel;
        //}

        ///// <summary>
        ///// Search taxa by name by using BeginsWith comparision.
        ///// </summary>
        ///// <param name="taxonName">Name of the taxon.</param>
        ///// <returns>List of Taxon search result item view models, or null.</returns>
        //private List<TaxonSearchResultItemViewModel> SearchTaxaByName(string taxonName)
        //{
        //    SearchStringCompareOperator? nameCompareOperator = SearchStringCompareOperator.BeginsWith;
        //    List<TaxonSearchResultItemViewModel> result = null;
        //    var searchOptions = new TaxonSearchOptions
        //    {
        //        NameSearchString = taxonName,
        //        NameCompareOperator = nameCompareOperator,
        //        AuthorSearchString = null,
        //        AuthorCompareOperator = null,
        //        IsUnique = null,
        //        IsValidTaxon = true,
        //        IsRecommended = null,
        //        IsOkForObsSystems = null,
        //        IsValidTaxonName = null,
        //        NameCategoryId = null,
        //        Limit = AppSettings.Default.NoOfTaxaInDropDownList
        //    };

        //    if (searchOptions.CanSearch())
        //    {
        //        result = SearchTaxaByNameOrId(searchOptions);
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Search taxa by name by using BeginsWith comparision.
        ///// </summary>
        ///// <returns>List of Taxon search result item view models, or null.</returns>
        //private List<TaxonSearchResultItemViewModel> SearchTaxaByUrlName(string taxonName)
        //{
        //    SearchStringCompareOperator? nameCompareOperator = SearchStringCompareOperator.BeginsWith;
        //    List<TaxonSearchResultItemViewModel> result = null;
        //    var searchOptions = new TaxonSearchOptions
        //    {
        //        NameSearchString = taxonName,
        //        NameCompareOperator = nameCompareOperator,
        //        AuthorSearchString = null,
        //        AuthorCompareOperator = null,
        //        IsUnique = null,
        //        IsValidTaxon = true,
        //        IsRecommended = null,
        //        IsOkForObsSystems = null,
        //        IsValidTaxonName = null,
        //        NameCategoryId = null,
        //        Limit = AppSettings.Default.NoOfTaxaInDropDownList
        //    };

        //    if (searchOptions.CanSearch())
        //    {
        //        result = SearchTaxaByNameOrIdWithoutSessionSettings(searchOptions);
        //    }

        //    return result;
        //}

        /// <summary>
        /// Checks if a specific taxon is in scope
        /// </summary>
        /// <param name="taxonId"></param>
        /// <returns></returns>
        public bool IsTaxonInScope(int taxonId)
        {
            var taxaIds = GetAllValidTaxaIds();
            return taxaIds.Contains(taxonId);
        }
    }
}
