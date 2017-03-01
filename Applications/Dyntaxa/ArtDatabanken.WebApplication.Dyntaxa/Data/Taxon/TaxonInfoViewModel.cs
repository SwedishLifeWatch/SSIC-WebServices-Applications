using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Resources;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;
using System.Linq;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Tree;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// View model for presenting information about a taxon
    /// </summary>
    public class TaxonInfoViewModel : BaseViewModel
    {
        private readonly TaxonInfoViewModelLabels labels = new TaxonInfoViewModelLabels();        
        private readonly ITaxon _taxon;
        private readonly ITaxonCategory _taxonCategory;
        private readonly IUserContext _user;
        private ITaxonRevision _taxonRevision;
        private int? _revisionTaxonCategorySortOrder;

        // Called by test        
        public TaxonInfoViewModel(
            int taxonId, 
            IUserDataSource userDataSourceRepository,
            ITaxonDataSource taxonDataSourceRepository)
        {
            CoreData.UserManager.DataSource = userDataSourceRepository;
            CoreData.TaxonManager.DataSource = taxonDataSourceRepository;
        }

        // Called "LIVE"
        public TaxonInfoViewModel(ITaxon taxon, IUserContext user, ITaxonRevision taxonRevision, int? revisionTaxonCategorySortOrder)
        {
            this._user = user;
            this._taxon = taxon;
            this._taxonRevision = taxonRevision;
            this._revisionTaxonCategorySortOrder = revisionTaxonCategorySortOrder;
            _taxonCategory = taxon.Category;
            bool isInRevision = IsInRevision;
            bool isUserTaxonEditor = user.IsTaxonEditor();

            //AcceptedNames = GetAcceptedNames(isInRevision, isUserTaxonEditor);
            Synonyms = _taxon.GetSynonymsViewModel(isInRevision, isUserTaxonEditor, false);
            //NewSynonyms = GetNewSynonyms(isInRevision, isUserTaxonEditor);                        
            ProParteSynonyms = _taxon.GetProParteSynonymsViewModel(isInRevision, isUserTaxonEditor);
            MisappliedNames = _taxon.GetMisappliedNamesViewModel(isInRevision, isUserTaxonEditor);
            Identifiers = _taxon.GetIdentfiersViewModel(isInRevision, isUserTaxonEditor);
        }

        private bool IsInRevision
        {
            get
            {                
                return DyntaxaHelper.IsInRevision(_user, _taxonRevision);
            }
        }

        public int TaxonId
        {
            get { return _taxon.Id; }
        }

        public string Guid
        {
            get { return _taxon.Guid; }
        }

        public int TaxonCategorySortOrder
        {
            get { return _taxonCategory.SortOrder; }
        }

        public string Category
        {
            get { return _taxonCategory.Name; }
        }

        public bool IsPublicMode
        {
            get { return !_user.IsAuthenticated(); }
        }

        public string ReferenceViewAction
        {
            get
            {
                if (_user.HasSpeciesFactAuthority())
                {
                    return "Add";
                }
                else
                {
                    return "Info";
                }
            }
        }

        public TaxonNameViewModel ScientificName
        {
            get
            {
                if (_scientificName == null)
                {
                    if (_taxon.ScientificName.IsNotEmpty())
                    {
                        _scientificName = new TaxonNameViewModel(_taxon.GetScientificName(CoreData.UserManager.GetCurrentUser()), _taxon);
                    }
                }
                return _scientificName;
            }
        }
        private TaxonNameViewModel _scientificName;

        public TaxonNameViewModel CommonName
        {
            get
            {
                if (_commonName == null)
                {
                    //IList<ITaxonName> swedishNames = taxon.GetTaxonNamesBySearchCriteria(user, TaxonNameCategoryIds.SWEDISH_NAME, null, (int)TaxonNameUsageId.ApprovedNaming, true, this.IsInRevision, false);
                    //if (swedishNames.Count > 0)
                    //{
                    //    ITaxonName swedishName = swedishNames[0];
                    //    if (swedishName != null && !string.IsNullOrEmpty(swedishName.Name))
                    //    {
                    //        commonName = new TaxonInfoViewModelName(taxon.GetCommonName(CoreData.UserManager.GetCurrentUser()), taxon);
                    //    }
                    //}

                    if (_taxon.CommonName.IsNotEmpty())
                    {
                        _commonName = new TaxonNameViewModel(_taxon.GetCommonName(CoreData.UserManager.GetCurrentUser()), _taxon);
                    }
                }
                return _commonName;
            }
        }
        private TaxonNameViewModel _commonName;

        public TaxonNameViewModel AnamorphName
        {
            get
            {
                if (_anamorphName == null)
                {
                    ITaxonName name = _taxon.GetAnamorphName();
                    if (name != null)
                    {
                        _anamorphName = new TaxonNameViewModel(name, _taxon);
                    }
                }
                return _anamorphName;
            }
        }
        private TaxonNameViewModel _anamorphName;

        ///// <summary>
        ///// All taxon synonyms
        ///// </summary>
        //public List<TaxonNameViewModel> Synonyms
        //{
        //    get
        //    {
        //        if (_synonyms == null)
        //        {
        //            _synonyms = new List<TaxonNameViewModel>();
        //            var synonyms = _taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser());                    
        //            foreach (ITaxonName taxonName in synonyms)
        //            {
        //                _synonyms.Add(new TaxonNameViewModel(taxonName, _taxon));
        //            }                    
        //        }
        //        return _synonyms;
        //    }
        //}
        //private List<TaxonNameViewModel> _synonyms;

        public List<TaxonNameViewModel> Synonyms { get; set; }

        ///// <summary>
        ///// All taxon synonyms
        ///// </summary>
        //public List<TaxonNameViewModel> GetSynonyms(bool isInRevision, bool isEditorUser)
        //{
        //    List<ITaxonName> synonyms = _taxon.GetSynonyms(CoreData.UserManager.GetCurrentUser(), false);
        //    synonyms = synonyms.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
        //    List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
        //    foreach (ITaxonName taxonName in synonyms)
        //    {
        //        if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
        //        {
        //            resultList.Add(new TaxonNameViewModel(taxonName, _taxon));
        //        }
        //    }

        //    return resultList;
        //}

        //public List<TaxonNameViewModel> GetNewSynonyms(bool isInRevision, bool isEditorUser)
        //{
        //    List<ITaxonName> synonyms = _taxon.GetNewSynonyms(CoreData.UserManager.GetCurrentUser());
        //    synonyms = synonyms.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
        //    List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
        //    foreach (ITaxonName taxonName in synonyms)
        //    {
        //        if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
        //        {
        //            resultList.Add(new TaxonNameViewModel(taxonName, _taxon));
        //        }
        //    }

        //    return resultList;
        //}
        //public List<TaxonNameViewModel> NewSynonyms { get; set; }

        ///// <summary>
        ///// Gets the misapplied names that is ok to show to the user.
        ///// </summary>
        ///// <param name="isInRevision">if set to <c>true</c> the session is in revision mode.</param>
        ///// <param name="isEditorUser">if set to <c>true</c> the user is taxon editor.</param>
        ///// <returns>List of misapplied names.</returns>
        //public List<TaxonNameViewModel> GetMisappliedNames(bool isInRevision, bool isEditorUser)
        //{
        //    List<ITaxonName> misappliedNames = _taxon.GetMisappliedNames(CoreData.UserManager.GetCurrentUser());
        //    misappliedNames = misappliedNames.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
        //    List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
        //    foreach (ITaxonName taxonName in misappliedNames)
        //    {
        //        if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
        //        {
        //            resultList.Add(new TaxonNameViewModel(taxonName, _taxon));
        //        }
        //    }

        //    return resultList;
        //}

        /// <summary>
        /// Gets or sets the misapplied names.
        /// </summary>        
        public List<TaxonNameViewModel> MisappliedNames { get; set; }

        //public List<TaxonNameViewModel> GetAcceptedNames(bool isInRevision, bool isEditorUser)
        //{
        //    List<ITaxonName> acceptedNames = _taxon.GetAcceptedNames(CoreData.UserManager.GetCurrentUser());
        //    acceptedNames = acceptedNames.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
        //    List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
        //    foreach (ITaxonName taxonName in acceptedNames)
        //    {
        //        if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
        //        {
        //            resultList.Add(new TaxonNameViewModel(taxonName, _taxon));
        //        }
        //    }

        //    return resultList;
        //}
        //public List<TaxonNameViewModel> AcceptedNames { get; set; }

        ///// <summary>
        ///// Determines whether the specified taxon name is ok to show to the user on the Info page.
        ///// </summary>
        ///// <param name="taxonName">Taxon name.</param>
        ///// <param name="isInRevision">if set to <c>true</c> the session is in revision mode.</param>
        ///// <param name="isEditorUser">if set to <c>true</c> the user is taxon editor.</param>
        ///// <returns><c>true</c> if the name is ok to show; otherwise <c>false</c>.</returns>
        //private bool IsNameOkToShow(ITaxonName taxonName, bool isInRevision, bool isEditorUser)
        //{
        //    // Name with status [Removed] is only visible inside revisions.
        //    if (!isInRevision && taxonName.Status.Id == (int)TaxonNameStatusId.Removed)
        //    {
        //        return false;
        //    }

        //    // Name with status [Preliminary] is only visible inside revisions or for logged in taxon editor.
        //    if (!isInRevision && !isEditorUser &&
        //        taxonName.Status.Id == (int)TaxonNameStatusId.PreliminarySuggestion)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// Gets the proParte synonyms that is ok to show to the user.
        ///// </summary>
        ///// <param name="isInRevision">if set to <c>true</c> the session is in revision mode.</param>
        ///// <param name="isEditorUser">if set to <c>true</c> the user is taxon editor.</param>
        ///// <returns>List of proParte synonyms.</returns>
        //public List<TaxonNameViewModel> GetProParteSynonyms(bool isInRevision, bool isEditorUser)
        //{
        //    List<ITaxonName> proParteSynonyms = _taxon.GetProParteSynonyms(CoreData.UserManager.GetCurrentUser());
        //    proParteSynonyms = proParteSynonyms.OrderBy(t => t.Category.Id).ThenBy(t => t.Status.SortOrder()).ToList();
        //    List<TaxonNameViewModel> resultList = new List<TaxonNameViewModel>();
        //    foreach (ITaxonName taxonName in proParteSynonyms)
        //    {
        //        if (IsNameOkToShow(taxonName, isInRevision, isEditorUser))
        //        {
        //            resultList.Add(new TaxonNameViewModel(taxonName, _taxon));    
        //        }
        //    }

        //    return resultList;
        //}

        /// <summary>
        /// proParte synonyms.
        /// </summary>
        public List<TaxonNameViewModel> ProParteSynonyms { get; set; }

        public List<TaxonNameViewModel> OtherLanguagesNames
        {
            get
            {
                if (_otherLanguagesNames == null)
                {
                    _otherLanguagesNames = new List<TaxonNameViewModel>();
                    List<ITaxonName> otherLanguagesNames = _taxon.GetOtherLanguagesNames(CoreData.UserManager.GetCurrentUser());

                    foreach (ITaxonName taxonName in otherLanguagesNames)
                    {                       
                        _otherLanguagesNames.Add(new TaxonNameViewModel(taxonName, _taxon));                        
                    }                    
                }
                return _otherLanguagesNames;
            }
        }
        private List<TaxonNameViewModel> _otherLanguagesNames;
        
        public IList<ITaxonChildStatistics> TaxonStatistics
        {
            get
            {
                if (_taxonStatisticsList == null)
                {
#if LOG_SPEED
                    var sp = new Stopwatch();
                    sp.Start();
#endif
                    _taxonStatisticsList = new List<ITaxonChildStatistics>();
                    if (_taxon != null)
                    {
                        _taxonStatisticsList = CoreData.TaxonManager.GetTaxonChildStatistics(_user, _taxon);
                    }
#if LOG_SPEED
                    sp.Stop();
                    DyntaxaLogger.WriteMessage("Taxon Info: Statistics: {0:N0} milliseconds", sp.ElapsedMilliseconds);
#endif
                }
                return _taxonStatisticsList;
            }
        }
        private IList<ITaxonChildStatistics> _taxonStatisticsList;

        /// <summary>
        /// Indicates whether or not the taxon has is ranked after genus.
        /// </summary>
        public bool IsBelowGenus
        {
            get
            {
                return _taxon.IsBelowGenus(_user);
            }
        }

        /// <summary>
        /// Indicates whether or not the taxon is valid.
        /// </summary>
        public bool IsValid
        {
            get { return _taxon.IsValid; }
        }

        /// <summary>
        /// All localized labels
        /// </summary>
        public TaxonInfoViewModelLabels Labels
        {
            get { return labels; }
        }

        public SwedishOccurrenceSummaryViewModel SwedishOccurrenceSummary
        {
            get
            {
                if (_swedishOccurrenceSummary == null)
                {                    
                    var manager = new SwedishOccurrenceSummaryManager(_user, _taxonRevision);
                    _swedishOccurrenceSummary = manager.CreateSwedishOccurrenceSummaryViewModel(_taxon);                                
                }
                return _swedishOccurrenceSummary;
            }
        }
        private SwedishOccurrenceSummaryViewModel _swedishOccurrenceSummary;       

        /// <summary>
        /// A list with all parent taxons
        /// </summary>
        public List<RelatedTaxonViewModel> ParentTaxa
        {
            get
            {                
                if (_parentTaxa == null)
                {
                    var allParentTaxa = _taxon.GetAllParentTaxonRelations(_user, null, this.IsInRevision, false, true);
                    var distinctParentTaxa = allParentTaxa.GroupBy(x => x.ParentTaxon.Id).Select(x => x.First().ParentTaxon).ToList();

                    _parentTaxa = new List<RelatedTaxonViewModel>();

                    foreach (ITaxon relatedTaxon in distinctParentTaxa)
                    {
                        if (relatedTaxon.Category.IsTaxonomic)
                        {
                            if (_revisionTaxonCategorySortOrder.HasValue)
                            {
                                if (relatedTaxon.Category.SortOrder < _revisionTaxonCategorySortOrder.Value)
                                {
                                    continue;
                                }
                            }

                            _parentTaxa.Add(new RelatedTaxonViewModel(relatedTaxon, relatedTaxon.Category, null));
                        }
                    }
                }

                //var byDyntaxaTree = ParentTaxaByDyntaxaTree;
                //var strByDyntaxaTree = string.Join(", ", byDyntaxaTree.Select(x => x.ToString()));
                //var strParentTaxa = string.Join(", ", _parentTaxa.Select(x => x.ToString()));

                //Debug.Assert(strParentTaxa == strByDyntaxaTree, "strParentTaxa == strByDyntaxaTree");
                
                //CollectionAssert.AreEquivalent(_parentTaxa.Select(x => x.TaxonId).ToList() , byDyntaxaTree.Select(x => x.TaxonId).ToList());
                return _parentTaxa;
            }
        }
        private List<RelatedTaxonViewModel> _parentTaxa;

        public List<RelatedTaxonViewModel> OtherParentTaxa
        {
            get
            {
                if (_otherParentTaxa == null)
                {
                    _otherParentTaxa = new List<RelatedTaxonViewModel>();                    
                    IList<ITaxon> otherParentTaxaList = _taxon.GetOtherParentTaxa(_user);
                    foreach (ITaxon relatedTaxon in otherParentTaxaList)
                    {
                        _otherParentTaxa.Add(new RelatedTaxonViewModel(relatedTaxon, relatedTaxon.Category, null));                            
                    }                    
                }

                return _otherParentTaxa;
            }
        }
        private List<RelatedTaxonViewModel> _otherParentTaxa;

        public List<RelatedTaxonViewModel> HistoricalParentTaxa
        {
            get
            {
                if (_historicalParentTaxa == null)
                {
                    Int32 taxonRelationIndex;
                    TaxonRelationList parentTaxonRelations;

                    parentTaxonRelations = _taxon.GetNearestNonValidParents(_user);
                    _historicalParentTaxa = new List<RelatedTaxonViewModel>();

                    // Remove taxon relations which taxon still is related to.
                    if (parentTaxonRelations.IsNotEmpty())
                    {
                        for (taxonRelationIndex = parentTaxonRelations.Count - 1; taxonRelationIndex >= 0; taxonRelationIndex--)
                        {
                            if (_parentTaxa.Any(o => o.TaxonId.Equals(parentTaxonRelations[taxonRelationIndex].ParentTaxon.Id)) ||
                                _otherParentTaxa.Any(o => o.TaxonId.Equals(parentTaxonRelations[taxonRelationIndex].ParentTaxon.Id)))
                            {
                                parentTaxonRelations.RemoveAt(taxonRelationIndex);
                            } 
                        }
                        parentTaxonRelations.SortTaxonCategory();
                    }

                    if (parentTaxonRelations.IsNotEmpty()) 
                    {
                        foreach (ITaxonRelation taxonRelation in parentTaxonRelations)
                        {
                            _historicalParentTaxa.Add(new RelatedTaxonViewModel(taxonRelation.ParentTaxon, taxonRelation.ParentTaxon.Category, taxonRelation.ValidToDate));
                        }
                    }
                }

                return _historicalParentTaxa;
            }
        }
        private List<RelatedTaxonViewModel> _historicalParentTaxa;

        /// <summary>
        /// All child taxons
        /// </summary>
        public List<RelatedTaxonViewModel> ChildTaxa
        {
            get
            {
                if (childTaxa == null)
                {
                    childTaxa = new List<RelatedTaxonViewModel>();
                    var childList = _taxon.GetChildTaxonRelations(_user, this.IsInRevision, false);
                    if (childList != null)
                    {                                                
                        foreach (ITaxonRelation taxonRelation in childList)
                        {                            
                            childTaxa.Add(new RelatedTaxonViewModel(taxonRelation.ChildTaxon, taxonRelation.ChildTaxon.Category, null));
                        }
                    }
                }

                return childTaxa;
            }
        }

        private List<RelatedTaxonViewModel> childTaxa;

        public List<TaxonNameViewModel> Identifiers { get; set; }

        //public List<TaxonNameViewModel> Identifiers
        //{
        //    get
        //    {
        //        if (_identifiers == null)
        //        {
        //            _identifiers = new List<TaxonNameViewModel>();
        //            List<ITaxonName> identifiersList = _taxon.GetAllIdentifiers(CoreData.UserManager.GetCurrentUser());
        //            foreach (var taxonName in identifiersList)
        //            {
        //                if (taxonName.Status.Id != (Int32)TaxonNameStatusId.Removed)
        //                {
        //                    _identifiers.Add(new TaxonNameViewModel(taxonName, _taxon));
        //                }                        
        //            }

        //            //if (taxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser()) != null)
        //            //{
        //            //    foreach (ITaxonName taxonName in taxon.GetTaxonNames(CoreData.UserManager.GetCurrentUser()))
        //            //    {
        //            //        switch (taxonName.NameCategory.Id)
        //            //        {
        //            //            case (int)TaxonNameCategoryIds.ERMS_NAME:
        //            //            case (int)TaxonNameCategoryIds.ITIS_NAME:
        //            //            case (int)TaxonNameCategoryIds.ITIS_NUMBER:
        //            //            case (int)TaxonNameCategoryIds.NN_CODE:
        //            //            case (int)TaxonNameCategoryIds.GUID:
        //            //                if (taxonName.NameUsage.Id != (int)TaxonNameUsageId.Removed)
        //            //                {
        //            //                    _identifiers.Add(new TaxonInfoViewModelName(taxonName, taxon));
        //            //                }

        //            //                break;
        //            //        }                            
        //            //    }
        //            //}
        //        }
        //        return _identifiers;

        //    }
        //}
        //private List<TaxonNameViewModel> _identifiers;

        private HashSet<LumpSplitExportModel> CreateLumpExportList(ITaxon taxon)
        {
            return CreateLumpExportList(new[] { taxon });
        }

        private HashSet<LumpSplitExportModel> CreateLumpExportList(IEnumerable<ITaxon> taxa)
        {
            var lumpExportList = new HashSet<LumpSplitExportModel>();
            foreach (ITaxon taxon in taxa)
            {
                LumpSplitEventList lumpEvents = null;
                if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.InvalidDueToLump)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(_user, taxon);
                }
                else if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.ValidAfterLump)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(_user, taxon);
                }

                if (lumpEvents != null && lumpEvents.Count > 0)
                {
                    foreach (ILumpSplitEvent lumpSplitEvent in lumpEvents)
                    {
                        var lumpExportModel = LumpSplitExportModel.Create(_user, lumpSplitEvent);
                        if (!lumpExportList.Contains(lumpExportModel))
                        {
                            lumpExportList.Add(lumpExportModel);
                        }
                    }
                }
            }
            return lumpExportList;
        }

        private HashSet<LumpSplitExportModel> CreateSplitExportList(ITaxon taxon)
        {
            return CreateSplitExportList(new[] { taxon });
        }

        private HashSet<LumpSplitExportModel> CreateSplitExportList(IEnumerable<ITaxon> taxa)
        {
            var splitExportList = new HashSet<LumpSplitExportModel>();
            foreach (ITaxon taxon in taxa)
            {
                LumpSplitEventList lumpEvents = null;
                if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.InvalidDueToSplit)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(_user, taxon);
                }
                else if (taxon.ChangeStatus.Id == (Int32)TaxonChangeStatusId.ValidAfterSplit)
                {
                    lumpEvents = CoreData.TaxonManager.GetLumpSplitEventsByNewReplacingTaxon(_user, taxon);
                }

                if (lumpEvents != null && lumpEvents.Count > 0)
                {
                    foreach (ILumpSplitEvent lumpSplitEvent in lumpEvents)
                    {
                        var lumpExportModel = LumpSplitExportModel.Create(_user, lumpSplitEvent);
                        if (!splitExportList.Contains(lumpExportModel))
                        {
                            splitExportList.Add(lumpExportModel);
                        }
                    }
                }
            }
            return splitExportList;
        }

        /// <summary>
        /// Creates a list of Link items for all links recommended for a certain taxon.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns>A list of link items.</returns>
        public static List<LinkItem> GetRecommendedLinks(ITaxon taxon, bool isInRevision)
        {
            var sp = new Stopwatch();
            sp.Start();

            var links = new List<LinkItem>();
            var linkManager = new LinkManager();
            LinkItem item = null;
            string url = "";
            IUserContext userContext = CoreData.UserManager.GetCurrentUser();

            //Add link to Artfakta
            url = linkManager.GetUrlToArtfakta(taxon.Id.ToString());
            if (url != "")
            {
                item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToArtfaktaLabel, url);
                links.Add(item);
            }

            //Add link to Photos from Artportalen
            url = linkManager.GetUrlToMediaAP(taxon.Id.ToString());
            if (url != "")
            {
                item = new LinkItem(
                    LinkType.Url,
                    LinkQuality.ApprovedByExpert,
                    Resources.DyntaxaResource.LinkToPhotosAPLabel,
                    url);
                links.Add(item);
            }

            //Add link to Nobanis
            url = linkManager.GetUrlToNobanis(taxon.ScientificName);
            if (url != "")
            {
                item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToNobanisLabel, url);
                links.Add(item);
            }

            //Add link to SKUD
            url = linkManager.GetUrlToSkud();
            if (url != "")
            {
                item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToSkudLabel, url);
                links.Add(item);
            }

            //Add link to Naturforskaren taxon information
            url = linkManager.GetUrlToNaturforskaren(taxon.ScientificName);
            if (url != "")
            {
                item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToNaturforskarenLabel, url);
                links.Add(item);
            }

            if (taxon.ScientificName.IsNotEmpty())
            {
                //item = LinkItem.CreateActionLink("RedirectToGBIF", "Taxon", LinkQuality.Automatic, Resources.DyntaxaResource.LinkToGbifLabel, taxon.Id.ToString());
                //links.Add(item);

                //Add link to GBIF
                // 2015-03-10 Link to GBIF broken 
                /*
                url = linkManager.GetUrlToGBIF(taxon.ScientificName);
                if (url != "")
                {
                    item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToGbifLabel, url);
                    links.Add(item);
                }
                 */

                //Add link to EoL
                url = linkManager.GetUrlToEoL(taxon.ScientificName);
                if (url != "")
                {
                    item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToEoLLabel, url);
                    links.Add(item);
                }

                //Add link to Biodiversity Heritage Library.
                url = linkManager.GetUrlToBiodiversityHeritageLibrary(taxon.ScientificName);
                if (url != "")
                {
                    item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToBHLLabel, url);
                    links.Add(item);
                }
            }

            var guidNames = taxon.GetTaxonNamesBySearchCriteria(userContext, (int)TaxonNameCategoryId.Guid, null, null, null, isInRevision, false);
            foreach (var name in guidNames)
            {
                LinkItem linkItem = null;

                //Add a link to PESI Taxon information.
                url = Resources.DyntaxaSettings.Default.UrlToGetPESITaxonInformation.Replace("[GUID]", name.Name);
                if (name.IsRecommended)
                {
                    linkItem = new LinkItem(LinkType.Url, LinkQuality.ApprovedByExpert, Resources.DyntaxaResource.LinkToPesiLabel, url);
                    links.Add(linkItem);
                }
                else
                {
                    linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToPesiLabel, url);
                    links.Add(linkItem);
                }
            }

            //Add a link to ITIS Taxon information.
            var itisNumberNames = taxon.GetTaxonNamesBySearchCriteria(userContext, (int)TaxonNameCategoryId.ItisNumber, null, null, true, isInRevision, false);
            foreach (var name in itisNumberNames)
            {
                LinkItem linkItem = null;
                url = Resources.DyntaxaSettings.Default.UrlToGetITISTaxonInformation.Replace("[Id]", name.Name);
                linkItem = new LinkItem(LinkType.Url, LinkQuality.ApprovedByExpert, Resources.DyntaxaResource.LinkToItisLabel, url);
                links.Add(linkItem);
            }

            foreach (var name in guidNames)
            {
                LinkItem linkItem = null;

                ////Add a link to Algaebase Taxon information.
                //url = linkManager.GetUrlToAlgaebaseTaxonInformation(name.Name);
                //if (url.IsNotEmpty())
                //{
                //    linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToAlgaebaseLabel, url);
                //    links.Add(linkItem);

                //    url = linkManager.GetUrlToNordicMicroalgaeTaxonInformation(taxon.ScientificName);
                //    linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToNordicMicroalgaeLabel, url);
                //    links.Add(linkItem);
                //}

                //Add a link to World Register of Marine Species (WoRMS) Taxon information.
                url = linkManager.GetUrlToWormsTaxonInformation(name.Name);
                if (url.IsNotEmpty())
                {
                    linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToWormsLabel, url);
                    links.Add(linkItem);

                    url = linkManager.GetUrlToNordicMicroalgaeTaxonInformation(taxon.ScientificName);
                    linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToNordicMicroalgaeLabel, url);
                    links.Add(linkItem);
                }

                //Add a link to Fauna Europea Taxon information.
                url = linkManager.GetUrlToFaunaEuropeaTaxonInformation(name.Name);
                if (url.IsNotEmpty())
                {
                    linkItem = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToFaunaEuropeaLabel, url);
                    links.Add(linkItem);
                }
            }

            //Add link to WIKI taxon information
            url = linkManager.GetUrlToWikipedia(taxon.ScientificName);
            if (url != "")
            {
                item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToWikipediaLabel, url);
                links.Add(item);
            }

            //Add link to Google
            url = linkManager.GetUrlToGoogleSearchResults(taxon.ScientificName);
            if (url != "")
            {
                item = new LinkItem(LinkType.Url, LinkQuality.Automatic, Resources.DyntaxaResource.LinkToGoogleLabel, url);
                links.Add(item);
            }

            sp.Stop();
            Debug.WriteLine("TaxonInfo - Retrieving links: {0:N0} milliseconds", sp.ElapsedMilliseconds);
            return links;            
        }

        public List<LinkItem> RecommendedLinks
        {
            get
            {                
                if (_recommendedLinks == null)
                {
                    _recommendedLinks = GetRecommendedLinks(this._taxon, this.IsInRevision);                   
                }                
                return _recommendedLinks;
            }
        }

        private List<LinkItem> _recommendedLinks;
        //private readonly LinkManager linkManager = new LinkManager();
    }

    /// <summary>
    /// A class that represents a taxon name
    /// </summary>
    public class TaxonNameViewModel
    {        
        public TaxonNameViewModel(ITaxonName taxonName, ITaxon taxon)
        {
            this.Id = taxonName.Id;
            this.Version = taxonName.Version;
            this.TaxonCategorySortOrder = taxon.Category.SortOrder;
            this.GUID = taxonName.Guid;
            this.Name = taxonName.Name;
            this.Author = taxonName.Author ?? "";
            this.CategoryName = taxonName.Category.Name;
            this.NameUsage = taxonName.NameUsage.Name;
            this.NameUsageId = taxonName.NameUsage.Id;
            this.NameStatus = taxonName.Status.Name;
            this.NameStatusId = taxonName.Status.Id;
            this.IsOriginal = taxonName.IsOriginalName;
            this.IsRecommended = taxonName.IsRecommended;
            this.CategoryId = taxonName.Category.Id;
            this.CategoryTypeId = taxonName.Category.Type.Id;
        }

        public int Id { get; set; }
        public int Version { get; set; }        
        public int TaxonCategorySortOrder { get; set; }        
        public string GUID { get; set; }                
        public string Name { get; set; }        
        public string Author { get; set; }        
        public string CategoryName { get; set; }
        public string NameUsage { get; set; }
        public int NameUsageId { get; set; }
        public string NameStatus { get; set; }
        public int NameStatusId { get; set; }
        public bool IsOriginal { get; set; }
        public bool IsRecommended { get; set; }
        public int CategoryId { get; set; }
        public int CategoryTypeId { get; set; }

        public bool IsScientificName
        {
            get { return this.CategoryId == (Int32)TaxonNameCategoryId.ScientificName; }
        }

        /// <summary>
        /// Gets a value indicating whether the name should be rendered in red color.
        /// </summary>        
        public bool ShowWarning
        {
            get
            {
                if (NameStatusId == (int)TaxonNameStatusId.Suppressed
                    || NameStatusId == (int)TaxonNameStatusId.IncorrectCitation
                    || NameStatusId == (int)TaxonNameStatusId.Misspelled
                    || NameStatusId == (int)TaxonNameStatusId.Unneccessary
                    || NameStatusId == (int)TaxonNameStatusId.Undescribed
                    || NameStatusId == (int)TaxonNameStatusId.Unpublished
                    || NameStatusId == (int)TaxonNameStatusId.InvalidNaming)
                {
                    return true;
                }

                return false;
            }
        }
    }

    /// <summary>
    /// A class that represents a related taxon
    /// </summary>
    public class RelatedTaxonViewModel
    {        
        public int TaxonId { get; set; }        
        public string Category { get; set; }
        public int SortOrder { get; set; }
        public string ScientificName { get; set; }        
        public DateTime? EndDate { get; set; }
        public string CommonName { get; set; }
        public bool MainRelation { get; set; }
        public RelatedTaxonViewModel(ITaxon taxon, ITaxonCategory taxonCategory, DateTime? endDate, bool mainRelation = false)
        {
            this.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
            this.EndDate = endDate;
            this.ScientificName = taxon.ScientificName.IsEmpty() ? string.Format("({0})", DyntaxaResource.ErrorNameIsMissing) : taxon.ScientificName;
            this.SortOrder = taxonCategory.SortOrder;
            this.Category = taxonCategory.Name;
            this.TaxonId = taxon.Id;
            this.MainRelation = mainRelation;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", ScientificName, TaxonId);
        }
    }

    /// <summary>
    /// Localized labels used in Taxon/Info view
    /// </summary>
    public class TaxonInfoViewModelLabels
    {
        public string TaxonNameInformationLabel
        {
            get { return DyntaxaResource.TaxonInfoNameInformationLabel; }
        }

        public string RecommendedNamesLabel
        {
            get { return DyntaxaResource.TaxonInfoRecommendedNamesLabel; }
        }

        public string ScientificNameLabel
        {
            get { return DyntaxaResource.TaxonInfoScientificNameLabel; }
        }

        public string OtherLanguagesLabel
        {
            get { return DyntaxaResource.TaxonInfoOtherLanguagesLabel; }
        }

        public string TitleLabel
        {
            get { return DyntaxaResource.TaxonInfoTitleLabel; }
        }

        public string CommonNameLabel
        {
            get { return DyntaxaResource.TaxonInfoCommonNameLabel; }
        }

        public string ProParteSynonymsLabel
        {
            get { return DyntaxaResource.TaxonInfoProParteSynonymsLabel; }
        }

        public string MisapplicationsLabel
        {
            get { return DyntaxaResource.TaxonInfoMisapplicationsLabel; }
        }

        public string SynonymsLabel
        {
            get { return DyntaxaResource.TaxonInfoSynonymsLabel; }
        }

        public string TaxonomicHierarchyLabel
        {
            get { return DyntaxaResource.TaxonInfoTaxonomicHierarchyLabel; }
        }

        public string NearestChildTaxaLabel
        {
            get { return DyntaxaResource.TaxonInfoNearestChildTaxaLabel; }
        }

        public string IdentifiersLabel
        {
            get { return DyntaxaResource.TaxonInfoIdentifiersLabel; }
        }

        public string GuidLabel
        {
            get { return DyntaxaResource.TaxonInfoGuidLabel; }
        }

        public string TaxonIdLabel
        {
            get { return DyntaxaResource.TaxonInfoTaxonIdLabel; }
        }

        public string RecommendedLinksLabel
        {
            get { return DyntaxaResource.TaxonInfoRecommendedLinksLabel; }
        }

        public string SwedishOccurrenceSummaryTitle
        {
            get { return DyntaxaResource.TaxonInfoSwedishOccurrenceSummaryTitle; }
        }

        public string QualityChartLabel
        {
            get { return DyntaxaResource.TaxonInfoQualityChartLabel; }
        }
        public string QualityChartUpdatedText
        {
            get { return DyntaxaResource.TaxonInfoQualityChartUpdatedText; }
        }
        
        public string DistributionMapLabel
        {
            get { return DyntaxaResource.TaxonInfoDistributionMapLabel; }
        }

        public string ReferencesLabel
        {
            get { return DyntaxaResource.TaxonInfoReferencesLabel; }
        }

        public string AnamorphNameLabel
        {
            get { return DyntaxaResource.TaxonInfoAnamorphNameLabel; }
        }

        public string TaxonStatisticsLabel
        {
            get { return DyntaxaResource.TaxonInfoTaxonStatisticsLabel; }
        }

        public string TaxonStatisticsCategoryNameLabel
        {
            get { return DyntaxaResource.TaxonInfoTaxonStatisticsCategoryNameLabel; }
        }

        public string TaxonStatisticsInDyntaxaLabel
        {
            get { return DyntaxaResource.TaxonInfoTaxonStatisticsInDyntaxa; }
        }

        public string TaxonStatisticsInSwedenLabel
        {
            get { return DyntaxaResource.TaxonInfoTaxonStatisticsInSweden; }
        }

        public string TaxonStatisticsReproInSwedenLabel
        {
            get { return DyntaxaResource.TaxonInfoTaxonStatisticsReproInSweden; }
        }
        
        public string TaxonRestMasterBackToSearchLabel
        {
            get { return DyntaxaResource.RestMasterBackToSearchLink; }
        }

        // TODO: Temp (is already in taxonInfoQualityChartModelHelper)

        public String[] QualityChartLabelArray
        {
            get
            {
                return new String[4]
                           {
                               DyntaxaResource.TaxonInfoQuality0Label,
                               DyntaxaResource.TaxonInfoQuality1Label,
                               DyntaxaResource.TaxonInfoQuality2Label,
                               DyntaxaResource.TaxonInfoQuality3Label
                           };
            }
        }

         public string OtherParentsLabel
        {
            get { return DyntaxaResource.TaxonInfoOtherParentsHeader; }
        }

        public string HistoricalParentsLabel
        {
            get { return DyntaxaResource.TaxonInfoHistoricalParentsHeader; }
        }

        public string HistoricalParentsUntilText
        {
            get { return DyntaxaResource.TaxonInfoHistoricalParentsUntil; }
        }

        public string NameToolTip
        {
            get { return DyntaxaResource.TaxonInfoNameTooltip; }
        }

        public string DistributionMapImageTitle
        {
            get { return DyntaxaResource.TaxonInfoDistributionMapImageTitle; }
        }

        public string DistributionInSwedenLabel
        {
            get { return DyntaxaResource.TaxonInfoDistributionInSweden; }
        }
    }
}
