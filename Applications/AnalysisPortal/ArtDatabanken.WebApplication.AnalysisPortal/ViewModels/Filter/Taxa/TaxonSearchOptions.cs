using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Enums;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa
{
    /// <summary>
    /// This class contains search settings to use in Taxa search
    /// </summary>
    public class TaxonSearchOptions
    {        
        public SearchStringCompareOperator DefaultNameCompareOperator { get; set; }
        public SearchStringCompareOperator DefaultAuthorCompareOperator { get; set; }

        [AllowHtml]
        public string NameSearchString { get; set; }

        [AllowHtml]
        public string AuthorSearchString { get; set; }

        public bool? IsOkForObsSystems { get; set; }
        public bool? IsRecommended { get; set; }
        public bool? IsUnique { get; set; }
        public bool? IsValidTaxon { get; set; }
        public bool? IsValidTaxonName { get; set; }

        public int? NameCategoryId { get; set; }
        //NameUsageId

        public SearchStringCompareOperator? NameCompareOperator { get; set; }
        public SearchStringCompareOperator? AuthorCompareOperator { get; set; }

        //public int? RestrictToTaxonChildren { get; set; }
        public bool RestrictToTaxonChildren { get; set; }
        public int? RestrictToTaxonId { get; set; }
        public string RestrictToTaxonDescription { get; set; }

        public TaxonSearchOptions()
        {
            IsValidTaxon = true;
            DefaultNameCompareOperator = SearchStringCompareOperator.BeginsWith;
            DefaultAuthorCompareOperator = SearchStringCompareOperator.BeginsWith;
        }

        public TaxonSearchOptions(string nameSearchString)
            : this()
        {
            NameSearchString = nameSearchString;
        }

        public TaxonSearchOptions(
            string nameSearchString, 
            SearchStringCompareOperator? nameCompareOperator, 
            string authorSearchString, 
            SearchStringCompareOperator? authorCompareOperator, 
            bool? isUnique, 
            bool? isValidTaxon, 
            bool? isRecommended, 
            bool? isOkForObsSystems, 
            bool? isValidTaxonName, 
            int? nameCategoryId)
            : this()
        {
            NameSearchString = nameSearchString;
            NameCompareOperator = nameCompareOperator;
            AuthorSearchString = authorSearchString;
            AuthorCompareOperator = authorCompareOperator;
            IsUnique = isUnique;
            IsValidTaxon = isValidTaxon;
            IsRecommended = isRecommended;
            IsOkForObsSystems = isOkForObsSystems;
            IsValidTaxonName = isValidTaxonName;
            NameCategoryId = nameCategoryId;
        }

        private List<StringCompareOperator> GetStringCompareOperator(SearchStringCompareOperator compareOperator)
        {
            switch (compareOperator)
            {
                case SearchStringCompareOperator.BeginsWith:
                    return new List<StringCompareOperator> { StringCompareOperator.BeginsWith };
                case SearchStringCompareOperator.Contains:
                    return new List<StringCompareOperator> { StringCompareOperator.Contains };
                case SearchStringCompareOperator.EndsWith:
                    return new List<StringCompareOperator> { StringCompareOperator.EndsWith };
                case SearchStringCompareOperator.Equal:
                    return new List<StringCompareOperator> { StringCompareOperator.Equal };
                case SearchStringCompareOperator.Iterative:
                    return new List<StringCompareOperator> { StringCompareOperator.Equal, StringCompareOperator.BeginsWith, StringCompareOperator.Contains };
                case SearchStringCompareOperator.Like:
                    return new List<StringCompareOperator> { StringCompareOperator.Like };
                default:
                    return null;
            }
        }

        public TaxonNameSearchCriteria CreateTaxonNameSearchCriteriaObject()
        {
            var taxonNameSearchCriteria = new TaxonNameSearchCriteria();

            if (!string.IsNullOrEmpty(NameSearchString))
            {
                var nameSearchCriteria = new StringSearchCriteria();
                nameSearchCriteria.SearchString = NameSearchString;
                nameSearchCriteria.CompareOperators = new List<StringCompareOperator>();
                if (NameCompareOperator.HasValue)
                {
                    nameSearchCriteria.CompareOperators.AddRange(GetStringCompareOperator(NameCompareOperator.Value));
                }
                else
                {
                    nameSearchCriteria.CompareOperators.AddRange(GetStringCompareOperator(DefaultNameCompareOperator));
                }

                taxonNameSearchCriteria.NameSearchString = nameSearchCriteria;
            }

            if (!string.IsNullOrEmpty(AuthorSearchString))
            {
                var authorSearchCriteria = new StringSearchCriteria();
                authorSearchCriteria.SearchString = AuthorSearchString;
                authorSearchCriteria.CompareOperators = new List<StringCompareOperator>();
                if (AuthorCompareOperator.HasValue)
                {
                    authorSearchCriteria.CompareOperators.AddRange(GetStringCompareOperator(AuthorCompareOperator.Value));
                }
                else
                {
                    authorSearchCriteria.CompareOperators.AddRange(GetStringCompareOperator(DefaultAuthorCompareOperator));
                }

                taxonNameSearchCriteria.AuthorSearchString = authorSearchCriteria;
            }
            
            taxonNameSearchCriteria.IsOkForSpeciesObservation = IsOkForObsSystems;
            taxonNameSearchCriteria.IsRecommended = IsRecommended;
            taxonNameSearchCriteria.IsUnique = IsUnique;
            taxonNameSearchCriteria.IsValidTaxon = IsValidTaxon;
            taxonNameSearchCriteria.IsValidTaxonName = IsValidTaxonName;

            if (NameCategoryId.HasValue && NameCategoryId.Value != -1)
            {
                taxonNameSearchCriteria.Category = taxonNameSearchCriteria.Category = CoreData.TaxonManager.GetTaxonNameCategory(
                    CoreData.UserManager.GetCurrentUser(),
                    NameCategoryId.Value);
            }

            if (RestrictToTaxonId.HasValue && RestrictToTaxonChildren)
            {
                taxonNameSearchCriteria.TaxonIds = new List<int> { RestrictToTaxonId.Value };
            }

            return taxonNameSearchCriteria;
        }

        /// <summary>
        /// decides if the user has set enough parameters to do a search
        /// </summary>
        /// <returns></returns>
        public bool CanSearch()
        {
            if (!string.IsNullOrEmpty(this.NameSearchString))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(this.AuthorSearchString))
            {
                return true;
            }

            if (this.RestrictToTaxonChildren)
            {
                return true;
            }

            return false;
        }

        public void RestoreFromCookie(HttpCookie cookie)
        {
            if (cookie == null)
            {
                return;
            }

            this.IsOkForObsSystems = ParseNullableBoolIntValue(cookie, "isOkForObs");
            this.IsRecommended = ParseNullableBoolIntValue(cookie, "isRec");
            this.IsUnique = ParseNullableBoolIntValue(cookie, "isUnique");
            this.IsValidTaxon = ParseNullableBoolIntValue(cookie, "isValidTaxon");
            this.IsValidTaxonName = ParseNullableBoolIntValue(cookie, "isValidTaxonName");

            this.NameCategoryId = ParseNullableInteger(cookie, "taxonNameCategoryId");

            var minCompareOperatorIndex = (int)Enum.GetValues(typeof(SearchStringCompareOperator)).Cast<SearchStringCompareOperator>().Min();
            var maxCompareOperatorIndex = (int)Enum.GetValues(typeof(SearchStringCompareOperator)).Cast<SearchStringCompareOperator>().Max();
            this.NameCompareOperator = ParseSearchStringCompareOperatorIntValue(cookie, "nameCompOp");
            if (NameCompareOperator != null && ((int)NameCompareOperator < minCompareOperatorIndex || (int)NameCompareOperator > maxCompareOperatorIndex))
            {
                NameCompareOperator = this.DefaultNameCompareOperator;
            }

            this.AuthorCompareOperator = ParseSearchStringCompareOperatorIntValue(cookie, "authCompOp");
            if (AuthorCompareOperator != null && ((int)AuthorCompareOperator < minCompareOperatorIndex || (int)AuthorCompareOperator > maxCompareOperatorIndex))
            {
                AuthorCompareOperator = this.DefaultAuthorCompareOperator;
            }
        }

        public HttpCookie CreateCookie(string name)
        {
            var cookie = new HttpCookie(name);
            cookie.Values.Add("nameCompOp", ((int)NameCompareOperator.GetValueOrDefault()).ToString());
            cookie.Values.Add("authCompOp", ((int)AuthorCompareOperator.GetValueOrDefault()).ToString());

            cookie.Values.Add("isOkForObs", GetNullableBoolIntValue(IsOkForObsSystems).ToString());
            cookie.Values.Add("isRec", GetNullableBoolIntValue(IsRecommended).ToString());
            cookie.Values.Add("isUnique", GetNullableBoolIntValue(IsUnique).ToString());
            cookie.Values.Add("isValidTaxon", GetNullableBoolIntValue(IsValidTaxon).ToString());
            cookie.Values.Add("isValidTaxonName", GetNullableBoolIntValue(IsValidTaxonName).ToString());
            cookie.Values.Add("taxonNameCategoryId", GetNullableIntegerValue(NameCategoryId).ToString());

            cookie.Expires = DateTime.Now.AddYears(1);
            return cookie;
        }

        private SearchStringCompareOperator? ParseSearchStringCompareOperatorIntValue(HttpCookie cookie, string name)
        {
            SearchStringCompareOperator compareOp;
            if (Enum.TryParse(cookie.Values[name], out compareOp))
            {
                return compareOp;
            }
            return null;
        }

        private bool? ParseNullableBoolIntValue(HttpCookie cookie, string name)
        {
            int intValue;
            if (int.TryParse(cookie.Values[name], out intValue))
            {
                switch (intValue)
                {
                    case -1:
                        return null;
                    case 0:
                        return false;
                    case 1:
                        return true;
                    default:
                        return null;
                }
            }
            return null;
        }

        private int GetNullableBoolIntValue(bool? val)
        {
            if (!val.HasValue)
            {
                return -1;
            }

            return val.Value ? 1 : 0;
        }

        private int? ParseNullableInteger(HttpCookie cookie, string name)
        {
            int intValue;
            if (int.TryParse(cookie.Values[name], out intValue))
            {
                return intValue;
            }
            return null;
        }

        private int GetNullableIntegerValue(int? val)
        {
            if (!val.HasValue)
            {
                return -1;
            }

            return val.Value;
        }

        public SelectList CreateTaxonNameCategorySelectList(int? value)
        {
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();
            TaxonNameCategoryList nameCategories = CoreData.TaxonManager.GetTaxonNameCategories(userContext);
            var selectItems = new List<SelectListItem>();
            selectItems.Add(new SelectListItem() { Text = Labels.AnyLabel, Value = null });
            foreach (ITaxonNameCategory category in nameCategories)
            {
                selectItems.Add(new SelectListItem() { Text = category.Name, Value = category.Id.ToString() });
            }
            return new SelectList(selectItems, "Value", "Text", value.HasValue ? value.Value.ToString() : "");
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

        public SelectList CreateCompareOperatorSelectlist(SearchStringCompareOperator? op)
        {
            const SearchStringCompareOperator defaultOperator = SearchStringCompareOperator.Contains;

            return new SelectList(
                new[] 
                {
                    new SelectListItem() { Text = Labels.CompareOpBeginsWithLabel, Value = SearchStringCompareOperator.BeginsWith.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpContainsLabel, Value = SearchStringCompareOperator.Contains.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpEndsWithLabel, Value = SearchStringCompareOperator.EndsWith.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpEqualLabel, Value = SearchStringCompareOperator.Equal.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpIterativeLabel, Value = SearchStringCompareOperator.Iterative.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpLikeLabel, Value = SearchStringCompareOperator.Like.ToString() },
                }, 
                "Value", 
                "Text", 
                op.HasValue ? op.Value.ToString() : defaultOperator.ToString());
        }

        /// <summary>
        /// Returns a list with the search parameters that are
        /// different than default
        /// </summary>
        /// <returns></returns>        
        public List<KeyValuePair<string, string>> GetSearchDescription()
        {
            var list = new List<KeyValuePair<string, string>>();

            // Restrict to taxon
            if (this.RestrictToTaxonChildren)
            {
                list.Add(new KeyValuePair<string, string>(RestrictToTaxonDescription, ""));
            }

            // NameCompareOperator
            if (this.NameCompareOperator != this.DefaultNameCompareOperator)
            {
                SelectList compareOperatorSelectList = CreateCompareOperatorSelectlist(NameCompareOperator);
                foreach (SelectListItem selectListItem in compareOperatorSelectList)
                {
                    if (selectListItem.Value == NameCompareOperator.Value.ToString())
                    {
                        list.Add(new KeyValuePair<string, string>(Labels.TaxonNameCompareOperatorLabel, selectListItem.Text));
                        break;
                    }
                }
            }

            // IsUnique
            if (this.IsUnique != null)
            {
                list.Add(new KeyValuePair<string, string>(Labels.IsUniqueLabel, IsUnique.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // IsValidTaxon
            if (this.IsValidTaxon != null)
            {
                list.Add(new KeyValuePair<string, string>(Labels.IsValidTaxonLabel, IsValidTaxon.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // IsRecommended
            if (this.IsRecommended != null)
            {
                list.Add(new KeyValuePair<string, string>(Labels.IsRecommendedLabel, IsRecommended.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // IsOkForObsSystems
            if (this.IsOkForObsSystems != null)
            {
                list.Add(new KeyValuePair<string, string>(Labels.IsOkForObsSystemLabel, IsOkForObsSystems.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // IsValidTaxonNameLabels
            if (this.IsValidTaxonName != null)
            {
                list.Add(new KeyValuePair<string, string>(Labels.IsValidTaxonNameLabel, IsValidTaxonName.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // NameCategoryId
            if (NameCategoryId.GetValueOrDefault(-1) != -1)
            {
                SelectList taxonNameCategorySelectList = CreateTaxonNameCategorySelectList(NameCategoryId);
                foreach (SelectListItem selectListItem in taxonNameCategorySelectList)
                {
                    if (selectListItem.Value == NameCategoryId.Value.ToString())
                    {
                        list.Add(new KeyValuePair<string, string>(Labels.TaxonNameCategoryLabel, selectListItem.Text));
                        break;
                    }
                }
            }

            // Author
            if (!string.IsNullOrEmpty(this.AuthorSearchString))
            {
                list.Add(new KeyValuePair<string, string>(Labels.AuthorLabel, AuthorSearchString));
            }

            return list;
        }

        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// All localized labels
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        public class ModelLabels
        {
            public string IsValidTaxonLabel { get { return Resources.Resource.TaxonSearchIsValidTaxon; } }
            public string IsValidTaxonNameLabel { get { return Resources.Resource.TaxonSearchIsValidTaxonName; } }
            public string TaxonNameCategoryLabel { get { return Resources.Resource.TaxonSearchNameCategory; } }
            public string IsRecommendedLabel { get { return Resources.Resource.TaxonSearchIsRecommended; } }
            public string IsOkForObsSystemLabel { get { return Resources.Resource.TaxonSearchIsOkForObsSystem; } }
            public string AuthorLabel { get { return Resources.Resource.TaxonSearchAuthor; } }
            public string CompareOpBeginsWithLabel { get { return Resources.Resource.TaxonSearchCompareOpBeginsWith; } }
            public string CompareOpContainsLabel { get { return Resources.Resource.TaxonSearchCompareOpContains; } }
            public string CompareOpEndsWithLabel { get { return Resources.Resource.TaxonSearchCompareOpEndsWith; } }
            public string CompareOpEqualLabel { get { return Resources.Resource.TaxonSearchCompareOpEqual; } }
            public string CompareOpIterativeLabel { get { return Resources.Resource.TaxonSearchCompareOpIterative; } }
            public string CompareOpLikeLabel { get { return Resources.Resource.TaxonSearchCompareOpLike; } }
            public string CompareOpNotEqualLabel { get { return Resources.Resource.TaxonSearchCompareOpNotEqual; } }
            public string TaxonNameCompareOperatorLabel { get { return Resources.Resource.TaxonSearchTaxonNameCompareOperator; } }
            public string CompareOpAutomaticLabel { get { return Resources.Resource.TaxonSearchCompareOpAutomatic; } }
            public string RestrictToLabel { get { return Resources.Resource.TaxonSearchRestrictTo; } }

            public string TaxonNotExistLabel
            {
                get { return Resources.Resource.TaxonSearchTaxonNotExistErrorText; }
            }

            public string TaxonNotInRevisionLabel
            {
                get { return Resources.Resource.TaxonSearchTaxonNotInRevisionErrorText; }
            }

            public string TrueLabel
            {
                get { return Resources.Resource.TaxonSearchTrue; }
            }

            public string FalseLabel
            {
                get { return Resources.Resource.TaxonSearchFalse; }
            }

            public string AnyLabel
            {
                get { return Resources.Resource.TaxonSearchAny; }
            }

            public string IsUniqueLabel
            {
                get { return Resources.Resource.TaxonSearchIsUnique; }
            }
        }
    }
}
