using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName
{
    /// <summary>
    /// A key that is a combination of taxonnamecategory and taxonname
    /// Used by checkboxes in Taxonname/List as a unique identifier.
    /// </summary>
    public struct TaxonNameKey
    {        
        public int TaxonNameCategoryId { get; set; }
        public int TaxonNameId { get; set; }

        public TaxonNameKey(int taxonNameCategoryId, int taxonNameId)
            : this()
        {
            TaxonNameCategoryId = taxonNameCategoryId;
            TaxonNameId = taxonNameId;
        }

        public TaxonNameKey(string strKey)
            : this()
        {
            string[] strs = strKey.Split(';');
            TaxonNameCategoryId = int.Parse(strs[0]);
            TaxonNameId = int.Parse(strs[1]);
        }

        public override string ToString()
        {
            return string.Format("{0};{1}", TaxonNameCategoryId, TaxonNameId);
        }

        public override bool Equals(object obj)
        {
            TaxonNameKey key2 = (TaxonNameKey)obj;
            return this.TaxonNameId == key2.TaxonNameId && this.TaxonNameCategoryId == key2.TaxonNameCategoryId;            
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();            
        }
    }

    /// <summary>
    /// A view model for presenting and edit a taxon name
    /// </summary>
    public class TaxonNameEditingViewModel
    {
        private readonly ITaxonName taxonName;
        private TaxonNameKey taxonNameKey;

        public TaxonNameEditingViewModel(ITaxonName taxonName, bool isPossibleToChangeToRecomended)
        {
            this.taxonName = taxonName;
            this.IsRecommended = taxonName.IsRecommended;
            this.IsPossibleToChangeRecomended = isPossibleToChangeToRecomended;
            if (taxonName.Status.Id != (Int32)TaxonNameStatusId.ApprovedNaming)
            {
                this.IsPossibleToChangeRecomended = false;
            }
            
            this.IsOkForObsSystems = taxonName.IsOkForSpeciesObservation;
            this.taxonNameKey = new TaxonNameKey(this.CategoryId, this.Version);
            this.IsRemoved = false;
            if (taxonName.Status.Id == (Int32)TaxonNameStatusId.Removed)
            {
                this.IsRemoved = true;
            }
        }

        public bool IsRecommended { get; set; }
        public bool IsPossibleToChangeRecomended { get; set; }
        public bool IsOkForObsSystems { get; set; }
        public bool IsStateChanged { get; set; }
        public string Guid { get { return taxonName.Guid; } }
        public bool IsOriginal { get { return taxonName.IsOriginalName; } }
        public bool IsRemoved { get; set; }
        public string Name { get { return taxonName.Name; } }
        public string Author { get { return taxonName.Author; } }
        public string NameStatusName { get { return taxonName.Status.Name; } }
        public string NameUsageName { get { return taxonName.NameUsage.Name; } }
        public List<string> References
        {
            get
            {
                IUserContext userContext = CoreData.UserManager.GetCurrentUser();
                var list = new List<string>();
                if (taxonName.GetReferences(userContext) == null)
                {
                    return list;
                }

                foreach (ReferenceRelation relation in taxonName.GetReferences(userContext))
                {                    
                    IReference reference = relation.GetReference(userContext);
                    Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                    list.Add(string.Format("{0}-{1}", reference.Name, year));
                }
                return list;
            }
        } // todo - hur ska denna fyllas?

        public int CategoryId { get { return taxonName.Category.Id; } }
        public int Version { get { return taxonName.Version; } }
        
        public ITaxonName TaxonNameDataObject
        {
            get { return taxonName; }
        }

        /// <summary>
        /// A unique identifier consisting of categoryId and nameId
        /// </summary>
        public string CustomIdentifier 
        { 
            get { return taxonNameKey.ToString(); }            
        }
    }

    /// <summary>
    /// A view model for presenting a taxon name category and its names
    /// </summary>
    public class TaxonNameCategoryViewModel
    {
        private readonly ITaxonNameCategory _taxonNameCategory;
        private readonly List<TaxonNameEditingViewModel> _names;

        public TaxonNameCategoryViewModel(ITaxon taxon, ITaxonNameCategory taxonNameCategory, IEnumerable<ITaxonName> names)
        {
            this._taxonNameCategory = taxonNameCategory;
            this._names = new List<TaxonNameEditingViewModel>();
            bool possibleToChangeRecommended = false;
            int noOfRecomended = names.Count(taxonName => taxonName.Status.Id == (Int32)TaxonNameStatusId.ApprovedNaming);
            if (noOfRecomended > 1 && taxonNameCategory.Id == (Int32)TaxonNameCategoryId.ScientificName)
            {
                possibleToChangeRecommended = true;
            }
            
            foreach (ITaxonName taxonName in names)
            {
                if (taxonNameCategory.Id != (Int32)TaxonNameCategoryId.ScientificName &&
                    taxonName.Status.Id == (Int32)TaxonNameStatusId.ApprovedNaming)
                {
                    possibleToChangeRecommended = true;
                }

                this._names.Add(new TaxonNameEditingViewModel(taxonName, possibleToChangeRecommended));  
            }
        }

        /// <summary>
        /// A list of the names in this category
        /// </summary>
        public List<TaxonNameEditingViewModel> Names { get { return this._names; } }

        public int Id
        {
            get { return _taxonNameCategory.Id; }
        }

        public string TaxonNameCategory
        {
            get { return _taxonNameCategory.Name; }
        }
    }

    /// <summary>
    /// A view model for presenting a list of all name categories and its names
    /// for a certain taxon
    /// </summary>
    public class ListTaxonNameViewModel
    {        
        private readonly ITaxon _taxon;
        private readonly ITaxonRevision _taxonRevision;
        public List<TaxonNameCategoryViewModel> NameCategories { get; set; }
        public int TaxonId { get { return _taxon.Id; } }
        //public int RevisionId { get { return _revisionId; } }

        /// <summary>
        /// Constructor
        /// </summary>        
        public ListTaxonNameViewModel(ITaxon taxon, ITaxonRevision taxonRevision)
        {
            this._taxonRevision = taxonRevision;
            this._taxon = taxon;

            Dictionary<ITaxonNameCategory, List<ITaxonName>> dicNames = GetTaxonNamesByCategory(taxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser()));

            var nameCategories = new List<TaxonNameCategoryViewModel>();
            foreach (ITaxonNameCategory nameCategory in dicNames.Keys)
            {
                nameCategories.Add(new TaxonNameCategoryViewModel(taxon, nameCategory, dicNames[nameCategory]));
            }

            NameCategories = nameCategories;
        }

        /// <summary>
        /// Create a dictionary, grouped by name category, from a list of taxon names
        /// </summary>        
        private Dictionary<ITaxonNameCategory, List<ITaxonName>> GetTaxonNamesByCategory(IList<ITaxonName> taxonNames)
        {
            var dicNames = new Dictionary<ITaxonNameCategory, List<ITaxonName>>();
            foreach (ITaxonName taxonName in taxonNames)
            {
                if (!dicNames.ContainsKey(taxonName.Category))
                {
                    dicNames.Add(taxonName.Category, new List<ITaxonName>());
                }

                dicNames[taxonName.Category].Add(taxonName);
            }
            var sortedDict = (from entry in dicNames orderby entry.Key.Id ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedDict;
        }

        /// <summary>
        /// Parses an array of taxon name strings
        /// </summary>
        /// <param name="strArr"></param>
        /// <returns>a list with taxon name keys</returns>
        private List<TaxonNameKey> ParseTaxonNameKeys(IEnumerable<string> strArr)
        {
            if (strArr == null)
            {
                return new List<TaxonNameKey>();
            }

            return strArr.Select(str => new TaxonNameKey(str)).ToList();
        }

        private bool IsAnyRecommendedInCategory(IEnumerable<TaxonNameKey> isRecommendedSelection, int categoryId)
        {
            if (isRecommendedSelection == null)
            {
                return false;
            }

            return isRecommendedSelection.Any(taxonNameKey => taxonNameKey.TaxonNameCategoryId == categoryId);
        }

        public TaxonNameList GetChangedTaxonNames(string[] isNotOkForObs, string[] isRecommended)
        {
            List<TaxonNameKey> isRecommendedSelection = ParseTaxonNameKeys(isRecommended);
            List<TaxonNameKey> isNotOkForObsSelection = ParseTaxonNameKeys(isNotOkForObs);

            foreach (TaxonNameCategoryViewModel taxonNameCategory in NameCategories)
            {
                int numberRecommended = 0;
                foreach (TaxonNameEditingViewModel taxonName in taxonNameCategory.Names)
                {
                    var taxonNameKey = new TaxonNameKey(taxonNameCategory.Id, taxonName.Version);
                    if (isRecommendedSelection.Contains(taxonNameKey))
                    {
                        // is recommended    
                        numberRecommended++;

                        if (!taxonName.TaxonNameDataObject.IsRecommended)
                        {
                            taxonName.TaxonNameDataObject.IsRecommended = true;
                            taxonName.IsStateChanged = true;
                        }
                    }
                    else
                    {
                        // not recommended                        
                        if (taxonName.TaxonNameDataObject.IsRecommended)
                        {
                            taxonName.TaxonNameDataObject.IsRecommended = false;

                            // check if there is no other recommended
                            bool existRecommended = IsAnyRecommendedInCategory(isRecommendedSelection, taxonNameCategory.Id);
                            if (!existRecommended)
                            {
                                taxonName.IsStateChanged = true;
                            }
                        }
                    }

                    if (isNotOkForObsSelection.Contains(taxonNameKey))
                    {
                        // is not ok for obs system                            
                        if (taxonName.TaxonNameDataObject.IsOkForSpeciesObservation)
                        {
                            taxonName.TaxonNameDataObject.IsOkForSpeciesObservation = false;
                            taxonName.IsStateChanged = true;
                        }
                    }
                    else
                    {
                        // is ok for obs system                        
                        if (!taxonName.TaxonNameDataObject.IsOkForSpeciesObservation)
                        {
                            taxonName.TaxonNameDataObject.IsOkForSpeciesObservation = true;
                            taxonName.IsStateChanged = true;
                        }
                    }
                }

                //// Validation
                //if (numberRecommended > 1)
                //    throw new Exception("Too many recommendations in name category");
                ////taxon.RecommendedScentificName.NameCategory.Id
                //if (taxonNameCategory.Id == TaxonNameCategoryIds.SCIENTIFIC_NAME && numberRecommended == 0)
                //    throw new Exception("At least one scientific name must be recommended");
            }

            TaxonNameList taxonNames = new TaxonNameList();

            foreach (TaxonNameEditingViewModel taxonName in this.GetAllNamesEnumerator())
            {
                if (taxonName.IsStateChanged)
                {
                    taxonNames.Add(taxonName.TaxonNameDataObject);
                }
            }

            return taxonNames;
        }

        /// <summary>
        /// Get an enumerator that returns all taxon names.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaxonNameEditingViewModel> GetAllNamesEnumerator()
        {
            foreach (TaxonNameCategoryViewModel taxonNameCategory in NameCategories)
            {
                foreach (TaxonNameEditingViewModel taxonName in taxonNameCategory.Names)
                {
                    yield return taxonName;
                }
            }
        }
    }

    /// <summary>
    /// Extension methods for lists
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Swap places of two elements
        /// </summary>
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }         
    }
}
