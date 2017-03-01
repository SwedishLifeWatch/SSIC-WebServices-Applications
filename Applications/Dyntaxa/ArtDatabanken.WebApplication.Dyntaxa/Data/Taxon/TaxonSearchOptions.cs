using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ArtDatabanken.Data;
using System.Web;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class TaxonSearchOptions
    {
        public DyntaxaStringCompareOperator DefaultNameCompareOperator { get; set; }
        public DyntaxaStringCompareOperator DefaultAuthorCompareOperator { get; set; }

        [AllowHtml]
        public string NameSearchString { get; set; }

        [AllowHtml]
        public string AuthorSearchString { get; set; }

        public bool? IsOkForObsSystems { get; set; }
        public bool? IsRecommended { get; set; }
        public bool? IsUnique { get; set; }
        public bool? IsValidTaxon { get; set; }
        public bool? IsValidTaxonName { get; set; }
        public DateTime? LastUpdatedStartDate { get; set; }
        public DateTime? LastUpdatedEndDate { get; set; }

        public int? NameCategoryId { get; set; }
        //NameUsageId

        public DyntaxaStringCompareOperator? NameCompareOperator { get; set; }
        public DyntaxaStringCompareOperator? AuthorCompareOperator { get; set; }

        //public int? RestrictToTaxonChildren { get; set; }
        public bool RestrictToTaxonChildren { get; set; }
        public int? RestrictToTaxonId { get; set; }
        public string RestrictToTaxonDescription { get; set; }
        public bool HideAuthorTextbox { get; set; }

        public TaxonSearchOptions()
        {
            IsValidTaxon = null;
            DefaultNameCompareOperator = DyntaxaStringCompareOperator.Contains;
            DefaultAuthorCompareOperator = DyntaxaStringCompareOperator.Contains;            
        }

        public TaxonSearchOptions(
            string nameSearchString, 
            DyntaxaStringCompareOperator? nameCompareOperator,
            string authorSearchString, 
            DyntaxaStringCompareOperator? authorCompareOperator,
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

        private List<StringCompareOperator> GetStringCompareOperator(DyntaxaStringCompareOperator compareOperator)
        {
            switch (compareOperator)
            {
                case DyntaxaStringCompareOperator.BeginsWith:
                    return new List<StringCompareOperator> { StringCompareOperator.BeginsWith };
                case DyntaxaStringCompareOperator.Contains:
                    return new List<StringCompareOperator> { StringCompareOperator.Contains };
                case DyntaxaStringCompareOperator.EndsWith:
                    return new List<StringCompareOperator> { StringCompareOperator.EndsWith };
                case DyntaxaStringCompareOperator.Equal:
                    return new List<StringCompareOperator> { StringCompareOperator.Equal };
                case DyntaxaStringCompareOperator.Iterative:
                    return new List<StringCompareOperator> { StringCompareOperator.Equal, StringCompareOperator.BeginsWith, StringCompareOperator.Contains };
                case DyntaxaStringCompareOperator.Like:
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
                taxonNameSearchCriteria.Category = CoreData.TaxonManager.GetTaxonNameCategory(
                    CoreData.UserManager.GetCurrentUser(),
                    NameCategoryId.Value);
            }

            if (RestrictToTaxonId.HasValue && RestrictToTaxonChildren)
            {
                taxonNameSearchCriteria.TaxonIds = new List<int> { RestrictToTaxonId.Value };
            }
            
            if (LastUpdatedStartDate.HasValue && LastUpdatedEndDate.HasValue)
            {
                taxonNameSearchCriteria.LastModifiedStartDate = LastUpdatedStartDate.Value;
                taxonNameSearchCriteria.LastModifiedEndDate = LastUpdatedEndDate.Value;
            }
            else if (LastUpdatedStartDate.HasValue && !LastUpdatedEndDate.HasValue)
            {
                taxonNameSearchCriteria.LastModifiedStartDate = LastUpdatedStartDate.Value;
                taxonNameSearchCriteria.LastModifiedEndDate = DateTime.Now;
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

            var minCompareOperatorIndex = (int)Enum.GetValues(typeof(DyntaxaStringCompareOperator)).Cast<DyntaxaStringCompareOperator>().Min(); 
            var maxCompareOperatorIndex = (int)Enum.GetValues(typeof(DyntaxaStringCompareOperator)).Cast<DyntaxaStringCompareOperator>().Max();
            this.NameCompareOperator = ParseStringCompareOperatorIntValue(cookie, "nameCompOp");
            if (NameCompareOperator != null && ((int)NameCompareOperator < minCompareOperatorIndex || (int)NameCompareOperator > maxCompareOperatorIndex))
            {
                NameCompareOperator = this.DefaultNameCompareOperator;
            }

            this.AuthorCompareOperator = ParseStringCompareOperatorIntValue(cookie, "authCompOp");
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

        private DyntaxaStringCompareOperator? ParseStringCompareOperatorIntValue(HttpCookie cookie, string name)
        {
            DyntaxaStringCompareOperator compareOp;
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

        public SelectList CreateIsValidSelectlist(bool? value)
        {
            return new SelectList(
                new[]
                {
                    new SelectListItem() { Text = Labels.AnyLabel, Value = null },
                    new SelectListItem() { Text = DyntaxaResource.TaxonSummaryValitityValueAccepted, Value = bool.TrueString },
                    new SelectListItem() { Text = DyntaxaResource.TaxonSummaryValitityValueNotValid, Value = bool.FalseString }                            
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
        /// Returns a list with the search parameters that are
        /// different than default
        /// </summary>
        /// <returns></returns>
        public List<Tuple<string, string>> GetSearchDescription()
        {            
            var list = new List<Tuple<string, string>>();

            // Restrict to taxon
            if (this.RestrictToTaxonChildren)
            {
                list.Add(new Tuple<string, string>(RestrictToTaxonDescription, ""));
            }                

            // NameCompareOperator
            if (this.NameCompareOperator != this.DefaultNameCompareOperator)
            {
                SelectList compareOperatorSelectList = CreateCompareOperatorSelectlist(NameCompareOperator);                
                foreach (SelectListItem selectListItem in compareOperatorSelectList)
                {
                    if (selectListItem.Value == NameCompareOperator.Value.ToString())
                    {
                        list.Add(new Tuple<string, string>(Labels.TaxonNameCompareOperatorLabel, selectListItem.Text));
                        break;                            
                    }
                }
            }

            // IsUnique
            if (this.IsUnique != null)
            {                    
                list.Add(new Tuple<string, string>(Labels.IsUniqueLabel, IsUnique.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // IsValidTaxon
            if (this.IsValidTaxon != null)
            {
                list.Add(new Tuple<string, string>(Labels.IsValidTaxonLabel, IsValidTaxon.Value ? DyntaxaResource.TaxonSummaryValitityValueAccepted : DyntaxaResource.TaxonSummaryValitityValueNotValid));
            }                

            // IsRecommended
            if (this.IsRecommended != null)
            {                    
                list.Add(new Tuple<string, string>(Labels.IsRecommendedLabel, IsRecommended.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // IsOkForObsSystems
            if (this.IsOkForObsSystems != null)
            {                    
                list.Add(new Tuple<string, string>(Labels.IsOkForObsSystemLabel, IsOkForObsSystems.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // IsValidTaxonNameLabels
            if (this.IsValidTaxonName != null)
            {                    
                list.Add(new Tuple<string, string>(Labels.IsValidTaxonNameLabel, IsValidTaxonName.Value ? Labels.TrueLabel : Labels.FalseLabel));
            }

            // NameCategoryId
            if (NameCategoryId.GetValueOrDefault(-1) != -1)
            {
                SelectList taxonNameCategorySelectList = CreateTaxonNameCategorySelectList(NameCategoryId);                
                foreach (SelectListItem selectListItem in taxonNameCategorySelectList)
                {
                    if (selectListItem.Value == NameCategoryId.Value.ToString())
                    {
                        list.Add(new Tuple<string, string>(Labels.TaxonNameCategoryLabel, selectListItem.Text));
                        break;                            
                    }
                }                    
            }

            // Author
            if (!string.IsNullOrEmpty(this.AuthorSearchString))
            {
                list.Add(new Tuple<string, string>(Labels.AuthorLabel, AuthorSearchString));
            }

            if (LastUpdatedStartDate.HasValue && LastUpdatedEndDate.HasValue)
            {
                string lastUpdatedDescription = string.Format(
                    "{0}: {1}, {2}: {3}",
                    Labels.LastUpdatedStartLabel,
                    LastUpdatedStartDate.Value.ToShortDateString(),
                    Labels.LastUpdatedEndLabel,
                    LastUpdatedEndDate.Value.ToShortDateString());
                list.Add(new Tuple<string, string>(Labels.LastUpdatedLabel, lastUpdatedDescription));
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
            public string IsValidTaxonLabel { get { return Resources.DyntaxaResource.TaxonSearchIsValidTaxon; } }
            public string IsValidTaxonNameLabel { get { return Resources.DyntaxaResource.TaxonSearchIsValidTaxonName; } }
            public string TaxonNameCategoryLabel { get { return Resources.DyntaxaResource.TaxonSearchNameCategory; } }
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

            public string LastUpdatedStartLabel 
            {
                get { return Resources.DyntaxaResource.TaxonSearchLastUpdatedStartDate; }
            }

            public string LastUpdatedEndLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchLastUpdatedEndDate; }
            }

            public string LastUpdatedLabel
            {
                get { return Resources.DyntaxaResource.TaxonSearchLastUpdated; }
            }
        }
    }
}
