using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{    
    /// <summary>
    /// This class is a view model for a search result item.
    /// </summary>
    public class RedListTaxonSearchResultItemViewModel
    {
        public int TaxonId { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public string SearchMatchName { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string Author { get; set; }
        public string NameCategory { get; set; }
        public TaxonAlertStatusId TaxonStatus { get; set; }
        public SpeciesProtectionLevelEnum SpeciesProtectionLevel { get; set; }
        public bool HasChildren { get; set; }
        public int NrOfChildren { get; set; }
        public int SingleChildTaxonId { get; set; }
        public string SearchMatchNameString { get; set; }

        // Indicates if it's a valid taxon
        public bool IsValid { get; set; }

        // Matching synonym
        public string Synonym { get; set; }

        // If the match is a synonym write it.
        public bool MatchIsSynonym { get; set; }

        /// <summary>
        /// Information about red list status. Sets if taxon has factor 743/redlisted and is not of type 
        // LC/NE/NA.
        /// </summary>
        public bool IsRedListed { get; set; }

        /// <summary>
        /// Information if taxon exists in sweden ie has swedish occurnce factor set.
        /// </summary>
        public bool IsSwedish { get; set; }

        /// <summary>
        /// Information about red list status. Sets if taxon has factor 743/redlisted.
        /// </summary>
        public bool IsRedListedEnsured { get; set; }

        /// <summary>
        /// Information if this taxon is in searchscope or not (used in presentation when creating the links in namesearch)
        /// </summary>
        public bool IsInTaxonSearchScope { get; set; }

        ///// <summary>
        ///// Creates a taxonsearchresultitemviewmodel from a taxon
        ///// </summary>
        ///// <param name="searchResult"></param>
        ///// <param name="searchString"></param>
        ///// <returns></returns>
        //public static RedListTaxonSearchResultItemViewModel CreateFromTaxonName(TaxonNameSearchResult searchResult, string searchString)
        //{
        //    // Get taxon name search information.
        //    TaxonNameSearchInformation taxonNameSearchInformation = TaxonNameSearchManager.Instance.GetInformation(searchResult.Taxon.Id);
        //    if (taxonNameSearchInformation.IsNull() ||
        //        !(taxonNameSearchInformation.HasChildrenInTaxonSearchScope ||
        //          taxonNameSearchInformation.IsInTaxonSearchScope))
        //    {
        //        return null;
        //    }

        //    // Create view model.
        //    var taxonSearchResultItemViewModel = new RedListTaxonSearchResultItemViewModel
        //    {
        //        HasChildren = taxonNameSearchInformation.HasChildren,
        //        NrOfChildren = taxonNameSearchInformation.NrOfChildren,
        //        NameCategory = searchResult.TaxonName.Category.Name,
        //        Author = searchResult.Taxon.Author.IsNotEmpty() ? searchResult.Taxon.Author : "",
        //        TaxonId = searchResult.Taxon.Id,
        //        SearchMatchName = searchResult.TaxonName.Name,
        //        ScientificName = searchResult.Taxon.ScientificName.IsNotEmpty()
        //            ? searchResult.Taxon.ScientificName
        //            : "",
        //        CommonName = searchResult.Taxon.CommonName.IsNotEmpty()
        //            ? searchResult.Taxon.CommonName
        //            : "",
        //        Category = searchResult.Taxon.Category != null ? searchResult.Taxon.Category.Name : "",
        //        TaxonStatus = (TaxonAlertStatusId)searchResult.Taxon.AlertStatus.Id,
        //        SearchMatchNameString = searchString,
        //        IsValid = searchResult.Taxon.IsValid
        //    };

        //    if (searchResult.Taxon.Category != null)
        //    {
        //        taxonSearchResultItemViewModel.CategoryId = searchResult.Taxon.Category.Id;
        //        taxonSearchResultItemViewModel.ParentCategoryId = searchResult.Taxon.Category.ParentId;
        //    }

        //    if (searchResult.IsSynonymous)
        //    {
        //        // If the commonname contains the searchstring, we don't show the synonyms, else we try to find a match
        //        if (searchResult.Taxon.CommonName.ContainsIgnoreCase(searchString))
        //        {
        //            taxonSearchResultItemViewModel.MatchIsSynonym = false;
        //            taxonSearchResultItemViewModel.Synonym = string.Empty;
        //        }
        //        else
        //        {
        //            taxonSearchResultItemViewModel.MatchIsSynonym = true;
        //            taxonSearchResultItemViewModel.Synonym = GetMatch(searchString, searchResult.Synonymous);
        //        }

        //    }
        //    else
        //    {
        //        taxonSearchResultItemViewModel.MatchIsSynonym = false;
        //        taxonSearchResultItemViewModel.Synonym = string.Empty;
        //    }

        //    taxonSearchResultItemViewModel.IsRedListed = taxonNameSearchInformation.IsRedListed ||
        //                                                 taxonNameSearchInformation.HasRedListedChildren;
        //    taxonSearchResultItemViewModel.IsSwedish = taxonNameSearchInformation.HasSwedishOccurrence ||
        //                                               taxonNameSearchInformation.HasSwedishOccurrenceChildren;

        //    taxonSearchResultItemViewModel.IsInTaxonSearchScope = taxonNameSearchInformation.IsInTaxonSearchScope;

        //    return taxonSearchResultItemViewModel;
        //}

        ///// <summary>
        ///// Gets a namematch on synonyms using the following logic
        ///// 1. Exact match
        ///// 2. Begins with
        ///// 3. Contains
        ///// (Each compare is made case isensitive).
        ///// </summary>
        ///// <param name="searchString"></param>
        ///// <param name="taxonSynonymList"></param>
        ///// <returns></returns>
        //private static string GetMatch(string searchString, IEnumerable<ITaxonName> taxonSynonymList)
        //{
        //    if (string.IsNullOrWhiteSpace(searchString) ||
        //        taxonSynonymList.IsNull())
        //        return string.Empty;

        //    var matchedString = string.Empty;
        //    foreach (var taxonName in taxonSynonymList)
        //    {
        //        // Exact match
        //        if (string.Compare(searchString, taxonName.Name, true, CultureInfo.InvariantCulture) == 0)
        //        {
        //            matchedString = taxonName.Name;
        //        }

        //        // Begins with
        //        if (taxonName.Name.StartsWith(searchString, true, CultureInfo.InvariantCulture))
        //        {
        //            matchedString = taxonName.Name;
        //        }

        //        // Contains
        //        if (taxonName.Name.ContainsIgnoreCase(searchString))
        //        {
        //            matchedString = taxonName.Name;
        //        }
        //    }

        //    return matchedString;
        //}

        ///// <summary>
        ///// Creates a search result item from a Taxon object.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <param name="searchId"></param>
        ///// <returns></returns>
        //public static RedListTaxonSearchResultItemViewModel CreateFromTaxon(ITaxon taxon, int searchId)
        //{
        //    return CreateFromTaxon(taxon, searchId.ToString(CultureInfo.InvariantCulture));
        //}

        ///// <summary>
        ///// Creates a search result item from a Taxon object.
        ///// </summary>
        ///// <param name="taxon">The taxon.</param>
        ///// <param name="searchString"></param>
        ///// <returns></returns>
        //public static RedListTaxonSearchResultItemViewModel CreateFromTaxon(ITaxon taxon, string searchString)
        //{
        //    IUserContext userContext = CoreData.UserManager.GetCurrentUser();

        //    var model = new TaxonSearchResultItemViewModel
        //    {
        //        NameCategory = "TaxonId",
        //        Author = taxon.Author.IsNotEmpty() ? taxon.Author : "",
        //        TaxonId = taxon.Id,
        //        SearchMatchName = taxon.Id.ToString(CultureInfo.InvariantCulture),
        //        ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "",
        //        CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "",
        //        Category = taxon.Category != null ? taxon.Category.Name : "",
        //        TaxonStatus = (TaxonAlertStatusId)taxon.AlertStatus.Id,
        //        SearchMatchNameString = searchString,
        //        IsValid = taxon.IsValid
        //    };

        //    if (taxon.Category != null)
        //    {
        //        model.CategoryId = taxon.Category.Id;
        //        model.ParentCategoryId = taxon.Category.ParentId;
        //    }

        //    if (!(model.ScientificName.ContainsIgnoreCase(searchString)) && !(model.CommonName.ContainsIgnoreCase(searchString)))
        //    {
        //        var synonyms = taxon.GetSynonyms(userContext);
        //        foreach (ITaxonName taxonName in synonyms)
        //        {
        //            if (taxonName.Name.IsNotEmpty() && taxonName.Name.ContainsIgnoreCase(searchString))
        //            {
        //                model.MatchIsSynonym = true;
        //                model.Synonym = taxonName.Name;
        //            }
        //        }
        //    }

        //    // Check redlisted value for taxon.
        //    bool isRedListedEnsured;
        //    bool isRedListed;
        //    CheckRedlistValueForTaxon(taxon, out isRedListed, out isRedListedEnsured);
        //    model.IsRedListed = isRedListed;
        //    model.IsRedListedEnsured = isRedListedEnsured;

        //    bool isSwedish;
        //    CheckSwedishOccurrenceForTaxon(taxon, out isSwedish);
        //    model.IsSwedish = isSwedish;

        //    // Check if children exist for selected taxon
        //    var children = taxon.GetChildTaxonRelations(userContext, false, false);
        //    model.HasChildren = children.Count > 0;

        //    // Only performe check if childern exist and parentTaxon is not redlited and not ensured.
        //    if (children.Count > 0 && !model.IsRedListed)
        //    {
        //        int depth = 1;
        //        bool redlisted = false;
        //        bool ensured = false;
        //        CheckIfChildTaxonIsRedlistedRecurse(userContext, taxon, ref depth, ref redlisted, ref ensured);

        //        // Check if redlisted and update model
        //        if (redlisted)
        //        {
        //            model.IsRedListed = true;
        //        }

        //        if (ensured)
        //        {
        //            model.IsRedListedEnsured = true;
        //        }
        //    }

        //    // Check if childern exist and they exist in sweden.
        //    if (children.Count > 0 && !model.IsSwedish)
        //    {
        //        int depth = 1;
        //        bool swedish = false;
        //        CheckIfChildTaxonIsSwedishRecurse(userContext, taxon, ref depth, ref swedish);

        //        // Check if swedish and update model
        //        if (swedish)
        //        {
        //            model.IsSwedish = true;
        //        }
        //    }

        //    model.IsInTaxonSearchScope = true;

        //    return model;
        //}

        /// <summary>
        /// Determines whether this object equals <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The other object to compare.</param>
        /// <returns><c>true</c> if the objects are equal; otherwise <c>false</c>.</returns>
        protected bool Equals(RedListTaxonSearchResultItemViewModel other)
        {
            return TaxonId == other.TaxonId && string.Equals(ScientificName, other.ScientificName) && string.Equals(CommonName, other.CommonName) && string.Equals(SearchMatchName, other.SearchMatchName) && string.Equals(Category, other.Category) && string.Equals(Author, other.Author) && string.Equals(NameCategory, other.NameCategory) && TaxonStatus == other.TaxonStatus;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((RedListTaxonSearchResultItemViewModel)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = TaxonId;
                hashCode = (hashCode * 397) ^ (ScientificName != null ? ScientificName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CommonName != null ? CommonName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SearchMatchName != null ? SearchMatchName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Category != null ? Category.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NameCategory != null ? NameCategory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)TaxonStatus;
                return hashCode;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(RedListTaxonSearchResultItemViewModel left, RedListTaxonSearchResultItemViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(RedListTaxonSearchResultItemViewModel left, RedListTaxonSearchResultItemViewModel right)
        {
            return !Equals(left, right);
        }

        ///// <summary>
        ///// Recursive call for checking if child taxon is red listed or ensured. If red listed child is found 
        ///// call is aborted and red listed parameter is set to true. If problems with recursive children regarding indefinite loops
        ///// a max recursive depth is set so that this function is aborted id max recursive depth is reached.
        ///// </summary>
        ///// <param name="userContext">User context information.</param>
        ///// <param name="taxon">Parent taxon to be evaluated if there are any red listed children.</param>
        ///// <param name="depth">Max depth for recursive loops.</param>
        ///// <param name="redlisted">Parameter indicating if "any" child-taxon is red listed.</param>
        ///// <param name="enusured">Parameter indicating if "any" child-taxon is ensured.</param>
        //private static void CheckIfChildTaxonIsRedlistedRecurse(IUserContext userContext, ITaxon taxon, ref int depth, ref bool redlisted, ref bool enusured)
        //{
        //    if (redlisted || (depth > AppSettings.Default.MaxRecursiveSteps))
        //    {
        //        return;
        //    }

        //    foreach (ITaxonRelation childTaxonRelation in taxon.GetAllChildTaxonRelations(userContext))
        //    {
        //        ITaxon childTaxon = childTaxonRelation.ChildTaxon;

        //        // Check if redlisted
        //        bool isRedListedEnsured;
        //        bool isRedListed;
        //        CheckRedlistValueForTaxon(childTaxon, out isRedListed, out isRedListedEnsured);

        //        if (isRedListed)
        //        {
        //            redlisted = true;
        //            enusured = true;
        //        }
        //        else if (isRedListedEnsured)
        //        {
        //            enusured = true;
        //        }

        //        depth++;
        //        if (!redlisted)
        //        {
        //            CheckIfChildTaxonIsRedlistedRecurse(userContext, childTaxonRelation.ChildTaxon, ref depth, ref redlisted, ref enusured);
        //            if (redlisted || (depth > AppSettings.Default.MaxRecursiveSteps))
        //            {
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}

        //private static void CheckIfChildTaxonIsSwedishRecurse(IUserContext userContext, ITaxon taxon, ref int depth, ref bool swedish)
        //{
        //    if (swedish || (depth > AppSettings.Default.MaxRecursiveSteps))
        //    {
        //        return;
        //    }

        //    foreach (ITaxonRelation childTaxonRelation in taxon.GetAllChildTaxonRelations(userContext))
        //    {
        //        ITaxon childTaxon = childTaxonRelation.ChildTaxon;

        //        // Check if redlisted
        //        bool isSwedish;
        //        CheckSwedishOccurrenceForTaxon(childTaxon, out isSwedish);

        //        if (isSwedish)
        //        {
        //            swedish = true;
        //        }

        //        depth++;
        //        if (!swedish)
        //        {
        //            CheckSwedishOccurrenceForTaxon(childTaxon, out isSwedish);
        //            if (swedish || (depth > AppSettings.Default.MaxRecursiveSteps))
        //            {
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Check if a specific taxon is red listed. This is used when searching for names or taxon id using
        ///// TaxonService and an information on red list categories is required.
        ///// </summary>
        ///// <param name="taxon"> Taxon to be checked.</param>
        ///// <param name="isRedListed">Sets to true if taxon is red listed i.e less or equal to ArtDatabanken.Data.RedListCategory.NT
        ///// else false.</param>
        ///// <param name="isRedListedEnsured">Sets to true if taxon is red listed i.e less or equal to ArtDatabanken.Data.RedListCategory.NE
        ///// else false.</param>
        //private static void CheckRedlistValueForTaxon(ITaxon taxon, out bool isRedListed, out bool isRedListedEnsured)
        //{
        //    IUserContext userContext = CoreData.UserManager.GetCurrentUser();
        //    bool redListedEnsured = false;
        //    bool redListed = false;

        //    // Check cached data first
        //    TaxonListInformation taxonInfo = TaxonListInformationManager.Instance.GetTaxonInformationFromCache(taxon);
        //    if (taxonInfo.IsNotNull() && taxonInfo.IsRedListed)
        //    {
        //        redListed = true;
        //        redListedEnsured = true;
        //    }
        //    else if (taxonInfo.IsNotNull() && taxonInfo.IsRedListedEnsured)
        //    {
        //        redListedEnsured = true;
        //    }
        //    else if (taxonInfo.IsNotNull() && !taxonInfo.IsRedListed && !taxonInfo.IsRedListedEnsured)
        //    {
        //        // Do nothing
        //    }
        //    else
        //    {
        //        // Get redlisted category from speciesfact, no data avaliable in cache.
        //        ITaxon speciesTaxon = CoreData.TaxonManager.GetTaxon(userContext, taxon.Id);
        //        ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
        //        searchCriteria.Taxa = new TaxonList();
        //        searchCriteria.AddTaxon(speciesTaxon);
        //        searchCriteria.Factors = new FactorList();

        //        // Set Redlist factor.
        //        IFactor factor = CoreData.FactorManager.GetFactor(userContext, FactorId.RedlistCategory);
        //        searchCriteria.Factors.Add(factor);

        //        // Set default individual category
        //        searchCriteria.IndividualCategories = new IndividualCategoryList();
        //        IIndividualCategory individualCategory = CoreData.FactorManager.GetDefaultIndividualCategory(userContext);
        //        searchCriteria.IndividualCategories.Add(individualCategory);

        //        // Set redlisted period (this value of selected redlistes category is set up elsewere.
        //        searchCriteria.Periods = new PeriodList();
        //        IPeriod period = CoreData.FactorManager.GetCurrentRedListPeriod(userContext);
        //        searchCriteria.Periods.Add(period);
        //        ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();

        //        // Get speciesfact that matches choosen searchcriteria
        //        SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(userContext, searchCriteria);
        //        if (speciesFacts.IsNotEmpty())
        //        {
        //            // Get first (there should be only one).
        //            if (speciesFacts.Count > 1)
        //            {
        //                throw new Exception("To many speciesfacts are avaliable for the following settings: taxonid=" + speciesTaxon.Id + " FactorId= "
        //                        + factor.Id + " defaultcategory=" + individualCategory.Id + " period=" + period.Id);
        //            }

        //            // Get speciesfact
        //            ISpeciesFact speciesFact = speciesFacts.First();

        //            if (speciesFact.Field1.IsNotNull() && speciesFact.Field1.EnumValue.IsNotNull()
        //                && speciesFact.Field1.EnumValue.OriginalLabel.IsNotEmpty() && speciesFact.MainField.IsNotNull())
        //            {
        //                // Get redlisted information from speciesFact
        //                string redListCategory = speciesFact.Field1.EnumValue.OriginalLabel.Substring(
        //                    0,
        //                    speciesFact.Field1.EnumValue.OriginalLabel.Length - 4) + "(" + speciesFact.MainField.StringValue + ")";
        //                var category = (RedListCategory)(Enum.Parse(typeof(RedListCategory), redListCategory.Substring(redListCategory.IndexOf("(", StringComparison.Ordinal) + 1, 2)));

        //                redListed = RedListedHelper.IsRedListedDdToNt((int)category);
        //                redListedEnsured = RedListedHelper.IsRedListedDdToNe((int)category);
        //            }
        //            else
        //            {
        //                throw new Exception("speciesFact.Field1.EnumValue.OriginalLabel or speciesFact.MainField is null for speciesfact id= " + speciesFact.Id);
        //            }
        //        }
        //    }

        //    isRedListed = redListed;
        //    isRedListedEnsured = redListedEnsured;
        //}

        ///// <summary>
        ///// Check if a specific taxon exists in sweden. This is used when searching for names or taxon id using
        ///// TaxonService and an information on swedish occurence is required.
        ///// </summary>
        ///// <param name="taxon"> Taxon to be checked.</param>
        ///// <param name="isSwedish">Sets to true if taxon is exist in sweden. 
        ///// else false.</param>
        //private static void CheckSwedishOccurrenceForTaxon(ITaxon taxon, out bool isSwedish)
        //{
        //    IUserContext userContext = CoreData.UserManager.GetCurrentUser();
        //    bool swedish = false;

        //    // Check cached data first
        //    TaxonListInformation taxonInfo = TaxonListInformationManager.Instance.GetTaxonInformationFromCache(taxon);
        //    if (taxonInfo.IsNotNull() && taxonInfo.SwedishOccurrenceId > AppSettings.Default.SwedishOccurrenceExist)
        //    {
        //        swedish = true;
        //    }
        //    else if (taxonInfo.IsNotNull() && taxonInfo.SwedishOccurrenceId <= AppSettings.Default.SwedishOccurrenceExist)
        //    {
        //        // Do nothing
        //    }
        //    else
        //    {
        //        // Get redlisted category from speciesfact, no data avaliable in cache.
        //        ITaxon speciesTaxon = CoreData.TaxonManager.GetTaxon(userContext, taxon.Id);
        //        ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();
        //        searchCriteria.Taxa = new TaxonList();
        //        searchCriteria.AddTaxon(speciesTaxon);
        //        searchCriteria.Factors = new FactorList();

        //        // Set Redlist factor.
        //        IFactor factor = CoreData.FactorManager.GetFactor(userContext, FactorId.SwedishOccurrence);
        //        searchCriteria.Factors.Add(factor);

        //        // Set default individual category
        //        searchCriteria.IndividualCategories = new IndividualCategoryList();
        //        IIndividualCategory individualCategory = CoreData.FactorManager.GetDefaultIndividualCategory(userContext);
        //        searchCriteria.IndividualCategories.Add(individualCategory);

        //        // Set redlisted period (this value of selected redlistes category is set up elsewere.
        //        searchCriteria.Periods = new PeriodList();
        //        IPeriod period = CoreData.FactorManager.GetCurrentRedListPeriod(userContext);
        //        searchCriteria.Periods.Add(period);
        //        ISpeciesFactDataSource speciesFactDataSource = new RedListSpeciesFactDataSource();

        //        // Get speciesfact that matches choosen searchcriteria
        //        SpeciesFactList speciesFacts = speciesFactDataSource.GetSpeciesFacts(userContext, searchCriteria);
        //        if (speciesFacts.IsNotEmpty())
        //        {
        //            // Get first (there should be only one).
        //            if (speciesFacts.Count > 1)
        //            {
        //                throw new Exception("To many speciesfacts are avaliable for the following settings: taxonid=" + speciesTaxon.Id + " FactorId= "
        //                        + factor.Id + " defaultcategory=" + individualCategory.Id + " period=" + period.Id);
        //            }

        //            // Get speciesfact
        //            ISpeciesFact speciesFact = speciesFacts.First();
        //            if (speciesFact.IsNotNull() && speciesFact.MainField.IsNotNull() && speciesFact.MainField.HasValue)
        //            {
        //                int swedishOccurrenceId = Convert.ToInt32(speciesFact.MainField.EnumValue.KeyInt);
        //                if (swedishOccurrenceId > AppSettings.Default.SwedishOccurrenceExist)
        //                {
        //                    swedish = true;
        //                }
        //                // Now we must add it to the cache
        //            }
        //            else
        //            {
        //                throw new Exception("speciesFact.Field1.EnumValue.OriginalLabel or speciesFact.MainField is null for speciesfact id= " + speciesFact.Id);
        //            }
        //        }

        //    }

        //    isSwedish = swedish;
        //}
    }
}
