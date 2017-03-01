using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// View Model for Taxon Search
    /// </summary>
    public class TaxonSearchViewModel
    {
        private readonly TaxonSearchViewModelLabels _labels = new TaxonSearchViewModelLabels();
        private TaxonSearchOptions _searchOptions = new TaxonSearchOptions();
        public string SearchString { get; set; }
        public TaxonSearchResultList SearchResult { get; set; }
        public TimeSpan SearchTime { get; set; }
        public TaxonSearchOptions SearchOptions
        {
            get { return _searchOptions; }
            set { _searchOptions = value; }
        }

        public int NumberOfResults
        {
            get
            {
                return SearchResult != null ? SearchResult.Count : 0;
            }
        }

        public string LinkController { get; set; }
        public string LinkAction { get; set; }
        public string LinkParamsString { get; set; }        
        public RouteValueDictionary LinkParams { get; set; }        
        public bool IsAmbiguousResult { get; set; }
        public bool IsZeroRowsResult { get; set; }
        public bool IsTaxonExisting { get; set; }
        public bool IsTaxonInRevision { get; set; }
        public string RootTaxonDescription { get; set; }

        public int? RootTaxonId
        {
            get
            {
                return _rootTaxonId;
            }

            set
            {
                bool change = false;
                if ((_rootTaxonId.HasValue && value.HasValue && _rootTaxonId.Value != value.Value) || !_rootTaxonId.HasValue)
                {
                    change = true;
                }
                _rootTaxonId = value;
                if (!change)
                {
                    return;
                }
                if (_rootTaxonId.HasValue)
                {
                    IUserContext userContext = CoreData.UserManager.GetCurrentUser();
                    ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, _rootTaxonId.Value);
                    RootTaxonDescription = taxon.GetScientificAndCommonName();
                    //RootTaxonDescription = Labels.RestrictToLabel + " " + taxon.GetScientificAndCommonName();
                    SearchOptions.RestrictToTaxonId = _rootTaxonId;
                    SearchOptions.RestrictToTaxonDescription = Resources.DyntaxaResource.TaxonSearchRestrictTo.Replace("[TaxonName]", taxon.GetLabel());
                }
                else
                {
                    RootTaxonDescription = null;
                    SearchOptions.RestrictToTaxonId = null;
                    SearchOptions.RestrictToTaxonDescription = null;
                }
            }
        }

        private int? _rootTaxonId;

        public TaxonSearchViewModel()
        {
            this.LinkAction = "Info";
            this.LinkController = "Taxon";
            this.LinkParams = new RouteValueDictionary();

            //this.SearchOptions.IsUnique = false;
        }

        /// <summary>
        /// If the user enters special characters as å,ä,ö in the search url
        /// it by default doesn't work
        /// This function encodes these characters so we can search with å,ä,ö in the url
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public static string ConvertSearchString(string rawUrl)
        {
            char[] specialcharacters = { 'å', 'ä', 'ö' };
            foreach (char c in specialcharacters)
            {
                rawUrl = rawUrl.Replace(c.ToString(), HttpUtility.UrlEncode(c.ToString()));
            }

            int index = rawUrl.LastIndexOf('?');
            if (index >= 0)
            {
                string querystring = rawUrl.Substring(index);
                if (!string.IsNullOrEmpty(querystring))
                {
                    var col = HttpUtility.ParseQueryString(querystring);
                    return col["search"] != null ? HttpUtility.UrlDecode(col["search"]) : null;
                }
                return null;
            }
            return null;
        }

        public SelectList CreateNullableBoolSelectlist(bool? value)
        {
            return new SelectList(
                new[]
                {
                    new SelectListItem() { Text = Labels.AnyLabel, Value = null },
                    new SelectListItem() { Text = Labels.TrueLabel, Value = bool.TrueString },
                    new SelectListItem() { Text = Labels.FalseLabel, Value = bool.FalseString }                            
                }, 
                "Value", 
                "Text", 
                value.HasValue ? value.Value.ToString() : "");
        }

        public SelectList CreateCompareOperatorSelectlist(DyntaxaStringCompareOperator? op)
        {
            const DyntaxaStringCompareOperator defaultOperator = DyntaxaStringCompareOperator.Contains;

            return new SelectList(
                new[]
                {
                    new SelectListItem() { Text = Labels.CompareOpBeginsWithLabel, Value = DyntaxaStringCompareOperator.BeginsWith.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpContainsLabel, Value = DyntaxaStringCompareOperator.Contains.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpEndsWithLabel, Value = DyntaxaStringCompareOperator.EndsWith.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpEqualLabel, Value = DyntaxaStringCompareOperator.Equal.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpIterativeLabel, Value = DyntaxaStringCompareOperator.Iterative.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpLikeLabel, Value = DyntaxaStringCompareOperator.Like.ToString() },
                }, 
                "Value", 
                "Text", 
                op.HasValue ? op.Value.ToString() : defaultOperator.ToString());
        }

        /// <summary>
        /// All localized labels
        /// </summary>
        public TaxonSearchViewModelLabels Labels
        {
            get { return _labels; }
        }

        public class TaxonSearchViewModelLabels
        {            
            public string SearchOptionsLabel { get { return Resources.DyntaxaResource.TaxonSearchOptionsLabel; } }
            public string TaxonSearchLabel { get { return Resources.DyntaxaResource.TaxonSearchTitle; } }
            public string IsValidTaxonLabel { get { return Resources.DyntaxaResource.TaxonSearchIsValidTaxon; } }
            public string IsValidTaxonNameLabel { get { return Resources.DyntaxaResource.TaxonSearchIsValidTaxonName; } }
            public string IsRecommendedLabel { get { return Resources.DyntaxaResource.TaxonSearchIsRecommended; } }
            public string IsOkForObsSystemLabel { get { return Resources.DyntaxaResource.TaxonSearchIsOkForObsSystem; } }
            public string AuthorLabel { get { return Resources.DyntaxaResource.TaxonSearchAuthor; } }
            public string CompareOpBeginsWithLabel { get { return Resources.DyntaxaResource.TaxonSearchCompareOpBeginsWith; } }
            public string CompareOpContainsLabel { get { return Resources.DyntaxaResource.TaxonSearchCompareOpContains; } }
            public string CompareOpEndsWithLabel { get { return Resources.DyntaxaResource.TaxonSearchCompareOpEndsWith; } }
            public string CompareOpEqualLabel { get { return Resources.DyntaxaResource.TaxonSearchCompareOpEqual; } }
            public string CompareOpIterativeLabel { get { return Resources.DyntaxaResource.TaxonSearchCompareOpIterative; } }
            public string CompareOpLikeLabel { get { return Resources.DyntaxaResource.TaxonSearchCompareOpLike; } }
            public string CompareOpNotEqualLabel { get { return Resources.DyntaxaResource.TaxonSearchCompareOpNotEqual; } }
            public string TaxonNameCompareOperatorLabel { get { return Resources.DyntaxaResource.TaxonSearchTaxonNameCompareOperator; } }
            public string CompareOpAutomaticLabel { get { return Resources.DyntaxaResource.TaxonSearchCompareOpAutomatic; } }
            public string RestrictToLabel { get { return Resources.DyntaxaResource.TaxonSearchRestrictTo; } }

            public string TaxonNotExistLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchTaxonNotExistErrorText; }
            }

            public string TaxonNotInRevisionLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchTaxonNotInRevisionErrorText; }
            }
            
            public string TrueLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchTrue; }
            }

            public string FalseLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchFalse; }
            }

            public string AnyLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchAny; }
            }

            public string IsUniqueLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchIsUnique; }
            }
            
            public string TaxonRestMasterViewCurrentTaxon
            {
                get { return Resources.DyntaxaResource.RestMasterViewCurrentTaxonLink; }
            }
        }
    }
  
    /// <summary>
    /// A list with taxon search result items
    /// </summary>
    public class TaxonSearchResultList : List<TaxonSearchResultItem>
    {
        public TaxonSearchResultList(IList<ITaxonName> taxonNames)
        {
            AddSearchResults(taxonNames);
        }

        public TaxonSearchResultList(ITaxon foundTaxon, IList<ITaxonName> taxonNames)
        {
            this.Add(TaxonSearchResultItem.CreateTaxonSearchResultItem(foundTaxon));
            AddSearchResults(taxonNames);
        }

        private void AddSearchResults(IList<ITaxonName> taxonNames)
        {
            for (int i = 0; i < taxonNames.Count; i++)
            {
                ITaxonName taxonName = taxonNames[i];
                Add(TaxonSearchResultItem.CreateTaxonSearchResultItem(taxonName, taxonName.Taxon));
            }            
        }
    }

    /// <summary>
    /// Taxon search result items
    /// </summary>
    public class TaxonSearchResultItem
    {
        public int TaxonId { get; set; }
        public string ScientificName { get; set; }
        public string CommonName { get; set; }
        public string SearchMatchName { get; set; }
        public string SearchMatchAuthor { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public string NameCategory { get; set; }
        public string StatusImageUrl { get; set; }

        public static TaxonSearchResultItem CreateTaxonSearchResultItem(ITaxonName taxonName, ITaxon taxon)
        {   
            var model = new TaxonSearchResultItem();
            
            model.NameCategory = taxonName.Category.Name;
            model.Author = taxon.Author.IsNotEmpty() ? taxon.Author : "";
            model.TaxonId = taxonName.Taxon.Id;
            model.SearchMatchName = taxonName.Name;
            model.SearchMatchAuthor = taxonName.Author;
            model.ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
            model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
            model.Category = taxon.Category != null ? taxon.Category.Name : "";
            model.StatusImageUrl = GetStatusImageUrl(taxon);

            return model;
        }

        public static TaxonSearchResultItem CreateTaxonSearchResultItem(ITaxon taxon)
        {
            var model = new TaxonSearchResultItem();
            
            model.NameCategory = "TaxonId";
            model.Author = taxon.Author.IsNotEmpty() ? taxon.Author : "";
            model.TaxonId = taxon.Id;
            model.SearchMatchName = taxon.Id.ToString();
            model.ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : "";
            model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
            model.Category = taxon.Category != null ? taxon.Category.Name : "";
            model.StatusImageUrl = GetStatusImageUrl(taxon);

            return model;
        }

        private static string GetStatusImageUrl(ITaxon taxon)
        {
            switch ((TaxonAlertStatusId)taxon.AlertStatus.Id)
            {
                case TaxonAlertStatusId.Green:
                    return "~/Images/Icons/info_green.png";
                case TaxonAlertStatusId.Yellow:
                    return "~/Images/Icons/info_yellow.png";
                case TaxonAlertStatusId.Red:
                    return "~/Images/Icons/info_red.png";
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }
    }
}
