using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using ArtDatabanken;
using AnalysisPortal.Helpers;
using ArtDatabanken.GIS;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.WebApplication.AnalysisPortal.Enums;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Accuracy;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Field;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Occurrence;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Quality;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Temporal;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Fields;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Shared;
using Newtonsoft.Json;
using Resources;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains Actions that is used to Filter out data
    /// from the data sources.
    /// </summary>
    public class FilterController : BaseController
    {        
        /// <summary>
        /// Renders an overview of the Filters
        /// </summary>
        /// <returns></returns>
        [IndexedBySearchRobots]
        public ActionResult Index()
        {
            string localeIsoCode = Thread.CurrentThread.CurrentCulture.Name;
            AboutViewModel model = AboutManager.GetAboutFiltersViewModel(localeIsoCode);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult _AddChildTaxon(int[] parentTaxonIds)
        {
            return PartialView(parentTaxonIds);
        }

        [HttpGet]
        public ActionResult Temporal()
        {
            IUserContext user = GetCurrentUser();
            var viewManager = new TemporalFilterViewManager(user, SessionHandler.MySettings);
            TemporalFilterViewModel model = viewManager.CreateTemporalFilterViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Temporal(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            TemporalFilterViewModel model = javascriptSerializer.Deserialize<TemporalFilterViewModel>(data);
            var viewManager = new TemporalFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateTemporalFilter(model);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.FilterTemporalSettingsUpdated));
            return RedirectToAction("Temporal");
        }

        [HttpGet]
        public ActionResult Quality()
        {
            var model = new QualityFilterViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Occurrence()
        {
            var viewManager = new OccurrenceFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            OccurrenceFilterViewModel model = viewManager.CreateOccurrenceFilterViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Occurrence(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            OccurrenceFilterViewModel model = javascriptSerializer.Deserialize<OccurrenceFilterViewModel>(data);
            var viewManager = new OccurrenceFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateOccurrenceSetting(model);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.FilterOccurrenceUpdated));
            return RedirectToAction("Occurrence");
        }

        /// <summary>
        /// Renders a page where the user can set locality filter.
        /// </summary>
        /// <returns>The view.</returns>
        [HttpGet]
        public ActionResult Locality()
        {
            LocalityViewManager localityViewManager = new LocalityViewManager(GetCurrentUser(), SessionHandler.MySettings);
            LocalityViewModel model = localityViewManager.CreateLocalityViewModel();
            return View(model);
        }

        /// <summary>
        /// Saves the locality filter settings.
        /// </summary>
        /// <param name="data">The filter settings as Json string.</param>
        /// <returns>Redirection to Locality page.</returns>
        [HttpPost]
        public RedirectToRouteResult Locality(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            LocalityViewModel model = javascriptSerializer.Deserialize<LocalityViewModel>(data);
            LocalityViewManager localityViewManager = new LocalityViewManager(GetCurrentUser(), SessionHandler.MySettings);
            localityViewManager.UpdateLocalitySetting(model);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.FilterLocalityUpdated));
            return RedirectToAction("Locality");
        }

        /// <summary>
        /// Resets the locality filter.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Redirection to returnUrl.</returns>
        public RedirectResult ResetLocality(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Spatial.Locality.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.FilterLocalityReset));
            return Redirect(returnUrl);
        }

        [HttpGet]
        public ActionResult Accuracy()
        {
            var viewManager = new AccuracyFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            AccuracyFilterViewModel model = viewManager.CreateAccuracyFilterViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Accuracy(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var javascriptSerializer = new JavaScriptSerializer();
                AccuracyFilterViewModel model = javascriptSerializer.Deserialize<AccuracyFilterViewModel>(data);

                var viewManager = new AccuracyFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.UpdateAccuracyFilter(model);
                SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.FilterAccuracyUpdated));
                return RedirectToAction("Accuracy");
            }

            return View();
        }

        public ActionResult Taxa()
        {
            var model = new TaxaViewModel();
            model.IsSettingsDefault = SessionHandler.MySettings.Filter.Taxa.IsSettingsDefault();
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult SelectedTaxaPartial(bool? showTaxaSelectionCheckboxes)
        {
            var model = new SelectedTaxaPartialViewModel
            {
                ShowTaxaSelectionCheckBoxes = showTaxaSelectionCheckboxes.GetValueOrDefault(false)
            };
            return PartialView(model);
        }

        /// <summary>
        /// Returns a view where the user can search for taxon
        /// by taxon ids and add them to the taxon filter.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaxonFromIds()
        {
            var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var model = viewManager.CreateTaxonFromIdsViewModel();
            ViewData["RowDelimiter"] = model.RowDelimiterSelectList;
            ViewBag.CurrentPage = SessionHandler.CurrentPage;
            ViewBag.ParentPages = SessionHandler.CurrentPage.GetParentPages();
            return View(model);
        }

        /// <summary>
        /// Renders the TaxonFromSearch page.
        /// </summary>
        /// <returns></returns>
        public ActionResult TaxonFromSearch()
        {
            var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            TaxonFromSearchViewModel model = viewManager.CreateTaxonFromSearchViewModel();
            return View(model);
        }

        /// <summary>
        /// Renders the TaxonByTaxonAttributes page.
        /// </summary>
        /// <returns></returns>
        public ActionResult TaxonByTaxonAttributes()
        {
            var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            TaxonByTaxonAttributesViewModel model = viewManager.CreateTaxonByTaxonAttributesViewModel();
            return View(model);
        }

        /// <summary>
        /// Renders the Field page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Field()
        {
            var viewManager = new FieldsFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            FieldViewModel model = viewManager.CreateFieldViewModel();
            return View(model);
        }

        /// <summary>
        /// Renders the TaxonFromSearch page.
        /// </summary>
        /// <returns></returns>
        public ActionResult RedList()
        {
            var viewManager = new RedListFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);

            var model = viewManager.CreateRedListViewModel();
            return View(model);
        }

        /// <summary>
        /// Applies red list filter.
        /// Adds the taxa that is matching the specified red list categories to the taxa filter.
        /// </summary>
        /// <param name="categories">The red list categories.</param>
        /// <returns>JsonNetResult with the number of taxa changed in taxa filter as status message.</returns>
        public JsonNetResult FilterRedListAddToCurrentSelection(IEnumerable<RedListCategory> categories)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new RedListFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);                
                int taxonIdsBefore = SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count;
                viewManager.AddToCurrentSelection(categories);
                int taxonIdsAfter = SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count;
                string msg = CreateNumberOfTaxaChangedStatusMessage(taxonIdsBefore, taxonIdsAfter);                
                jsonModel = JsonModel.CreateSuccess(msg, new { IsTaxaListEmpty = SessionHandler.MySettings.Filter.Taxa.NumberOfSelectedTaxa == 0 });
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);            
        }

        /// <summary>
        /// Applies red list filter.
        /// Removes the taxa that not matching the specified red list categories from the taxa filter.
        /// </summary>
        /// <param name="categories">The red list categories.</param>
        /// <returns>JsonNetResult with the number of taxa changed in taxa filter as status message.</returns>
        public JsonNetResult FilterRedListFilterCurrentSelection(IEnumerable<RedListCategory> categories)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new RedListFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                int taxonIdsBefore = SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count;
                viewManager.FilterCurrentSelection(categories);
                int taxonIdsAfter = SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count;
                string msg = CreateNumberOfTaxaChangedStatusMessage(taxonIdsBefore, taxonIdsAfter);

                jsonModel = JsonModel.CreateSuccess(msg, new { IsTaxaListEmpty = SessionHandler.MySettings.Filter.Taxa.NumberOfSelectedTaxa == 0 });                
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Applies red list filter.
        /// The taxa matching the specified red list categories is set as new taxa filter.
        /// </summary>
        /// <param name="categories">The red list categories.</param>
        /// <returns>JsonNetResult with the number of taxa changed in taxa filter as status message.</returns>
        public JsonNetResult FilterRedListUseAsCurrentSelection(IEnumerable<RedListCategory> categories)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new RedListFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                int taxonIdsBefore = SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count;
                viewManager.UseAsCurrentSelection(categories);
                int taxonIdsAfter = SessionHandler.MySettings.Filter.Taxa.TaxonIds.Count;
                string msg = CreateNumberOfTaxaChangedStatusMessage(taxonIdsBefore, taxonIdsAfter);

                jsonModel = JsonModel.CreateSuccess(msg, new { IsTaxaListEmpty = SessionHandler.MySettings.Filter.Taxa.NumberOfSelectedTaxa == 0 });                
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Creates the number of taxa changed status message.
        /// </summary>
        /// <param name="numberOfTaxonIdsBefore">The number of taxon ids before.</param>
        /// <param name="numberOfTaxonIdsAfter">The number of taxon ids after.</param>
        /// <returns>Number of taxa changed status message.</returns>
        private string CreateNumberOfTaxaChangedStatusMessage(int numberOfTaxonIdsBefore, int numberOfTaxonIdsAfter)
        {
            string str = "";
            if (numberOfTaxonIdsAfter > numberOfTaxonIdsBefore)
            {
                int nrAdded = numberOfTaxonIdsAfter - numberOfTaxonIdsBefore;
                str = string.Format("{0:N0} {1}", nrAdded, Resource.SharedTaxaAddedToTaxaList);
            }
            else if (numberOfTaxonIdsAfter < numberOfTaxonIdsBefore)
            {
                int nrRemoved = numberOfTaxonIdsBefore - numberOfTaxonIdsAfter;
                str = string.Format("{0:N0} {1}", nrRemoved, Resource.SharedTaxaRemovedFromTaxaList);
            }
            else
            {
                str = Resource.SharedNoTaxaAddedOrRemovedFromTaxaList;
            }

            return str;
        }

        public class TreeNode
        {
            [JsonProperty("compositeId")]
            public int CompositeId { get; set; }

            [JsonProperty("parentFactorId")]
            public int ParentFactorId { get; set; }

            [JsonProperty("nodeFactorId")]
            public int NodeFactorId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("loaded")]
            public bool Loaded { get; set; }

            [JsonProperty("expanded")]
            public bool Expanded { get; set; }

            [JsonProperty("leaf")]
            public bool Leaf { get; set; }

            [JsonProperty("children")]
            public List<TreeNode> Children { get; set; }
        }

        /// <summary>
        /// Parses a string with taxon ids and returns a list of corresponding taxon view models in JSON format.
        /// </summary>
        /// <param name="compositeId">The composite id of the taxon and its parents.</param>
        /// <param name="nodeFactorId">The id of the taxon.</param>
        /// <returns>A list of TaxonViewModel in JSON format</returns>
        public JsonNetResult GetTaxaTreeNodes(int nodeFactorId = 0, int compositeId = 0)
        {
            try
            {
                // Get list of factors
                var factorTreeNode = CoreData.FactorManager.GetFactorTree(GetCurrentUser(), nodeFactorId);

                FactorTreeNodeList factorTreeNodeList = null;
                if (factorTreeNode.IsNotNull() && factorTreeNode.Children.IsNotNull())
                {
                    factorTreeNodeList = factorTreeNode.Children;
                }

                return new JsonNetResult(ConvertFactorTreeNodeListToTreeNodeList(factorTreeNodeList));
            }
            catch (Exception ex)
            {
                return new JsonNetResult(JsonModel.CreateFailure(ex.Message));
            }
        }

        private List<TreeNode> ConvertFactorTreeNodeListToTreeNodeList(FactorTreeNodeList factorTreeNodeList, bool isMaxDepthReached = false)
        {
            var nodeModels = new List<TreeNode>();

            if (factorTreeNodeList != null)
            {
                foreach (var node in factorTreeNodeList)
                {
                    var parentFactorId = 0;
                    var compositeId = string.Empty;
                    if (node.Parents.Any())
                    {
                        parentFactorId = node.Parents.First().Factor.Id;
                        compositeId = string.Format("{0}{1}", parentFactorId, node.Factor.Id);
                    }

                    var nodeModel = new TreeNode
                    {
                        CompositeId = int.Parse(compositeId),
                        ParentFactorId = parentFactorId,
                        NodeFactorId = node.Factor.Id,
                        Name = node.Factor.Label,
                        Leaf = node.Factor.IsLeaf,
                        Loaded = false,
                        Expanded = false
                    };
                    if (!isMaxDepthReached)
                    {
                        nodeModel.Children = ConvertFactorTreeNodeListToTreeNodeList(node.Children, true);
                    }

                    nodeModels.Add(nodeModel);
                }
            }

            return nodeModels;
        }

        /// <summary>
        /// Parses a string with taxon ids and returns a list of corresponding taxon view models in JSON format.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="rowDelimiter">Row delimiter. For example: Semicolon, Return Line feed, Tab, ...</param>
        /// <returns>A list of TaxonViewModel in JSON format</returns>
        public JsonNetResult GetMatchingTaxa(string text, RowDelimiter? rowDelimiter)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<ITaxon> taxa = viewManager.GetMatchingTaxaFromText(text, rowDelimiter ?? RowDelimiter.ReturnLinefeed);
                List<TaxonViewModel> taxonList = taxa.ToTaxonViewModelList();

                ArtDatabanken.WebApplication.AnalysisPortal.Managers.SpeciesFactManager speciesFactManager = new ArtDatabanken.WebApplication.AnalysisPortal.Managers.SpeciesFactManager(GetCurrentUser());

                IEnumerable<ITaxon> protectedTaxonList = speciesFactManager.GetProtectedTaxons();

                // Set protection level for each taxon; public or not
                taxonList.ForEach(t => t.SpeciesProtectionLevel = protectedTaxonList.Any(ptl => ptl.Id == t.TaxonId) ? SpeciesProtectionLevelEnum.Protected1 : SpeciesProtectionLevelEnum.Public);

                jsonModel = JsonModel.Create(taxonList);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Searches for taxa according to the parameters and returns a list of corresponding taxon view models in JSON format.
        /// </summary>
        /// <param name="nameSearchString">The name search string.</param>
        /// <param name="nameCompareOperator">The name compare operator.</param>
        /// <param name="authorSearchString">The author search string.</param>
        /// <param name="isUnique">Is the taxon unique.</param>
        /// <param name="isValidTaxon">Is the taxon valid.</param>
        /// <param name="isRecommended">Is the taxon recommended.</param>
        /// <param name="isOkForObsSystems">Is the taxon ok for obs systems.</param>
        /// <param name="isValidTaxonName">Is the taxon valid.</param>
        /// <param name="nameCategoryId">The name category id.</param>
        /// <returns></returns>
        public JsonNetResult GetTaxaBySearch(
            string nameSearchString,
            SearchStringCompareOperator? nameCompareOperator,
            string authorSearchString,
            bool? isUnique,
            bool? isValidTaxon,
            bool? isRecommended,
            bool? isOkForObsSystems,
            bool? isValidTaxonName,
            int? nameCategoryId)
        {
            JsonModel jsonModel;
            try
            {
                IUserContext user = GetCurrentUser();
                var viewManager = new TaxonSearchManager(user);
                var searchOptions = new TaxonSearchOptions(
                    nameSearchString,
                    nameCompareOperator,
                    authorSearchString,
                    null,
                    isUnique,
                    isValidTaxon,
                    isRecommended,
                    isOkForObsSystems,
                    isValidTaxonName,
                    nameCategoryId);

                if (searchOptions.CanSearch())
                {
                    List<TaxonSearchResultItemViewModel> resultList = viewManager.SearchTaxa(searchOptions);
                    List<KeyValuePair<string, string>> extra = searchOptions.GetSearchDescription();
                    string searchSettingsHtml = RenderPartialViewToString("SearchSettingsSummaryPartial", extra);
                    jsonModel = JsonModel.Create(resultList, searchSettingsHtml);
                }
                else
                {
                    jsonModel = JsonModel.CreateFailure(Resource.TaxonSearchNotEnoughSearchCriteriaError);
                }
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Get all child taxon for requested parents
        /// </summary>
        /// <param name="parentTaxaIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonNetResult GetChildTaxa(int[] parentTaxaIds)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new TaxonSearchManager(GetCurrentUser());
                var resultList = viewManager.GetChildTaxa(parentTaxaIds);

                jsonModel = JsonModel.Create(resultList);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        public JsonNetResult GetTaxaByRedList(string nameSearchString, int[] categories)
        {
            JsonModel jsonModel;

            try
            {
                var userContext = GetCurrentUser();

                // Get red listed taxon ids.
                /*  var analysisSearchCriteria = new AnalysisSearchCriteria
                  {
                      RedListCategories = new List<int>()
                  };

                  analysisSearchCriteria.RedListCategories.AddRange(RedListedHelper.GetRedListCategoriesDdToNtAsIntList());

                  var redListedTaxonIds = CoreData.AnalysisManager.GetTaxonIds(analysisSearchCriteria);
                  */

                var speciesFactSearchCriteria = new SpeciesFactSearchCriteria
                {
                    IncludeNotValidHosts = true,
                    IncludeNotValidTaxa = true,
                    Taxa = new TaxonList(),
                    Factors = new FactorList()
                };

                speciesFactSearchCriteria.Add(CoreData.FactorManager.GetCurrentPublicPeriod(userContext));
                speciesFactSearchCriteria.Add(CoreData.FactorManager.GetDefaultIndividualCategory(userContext));
                speciesFactSearchCriteria.Factors.Add(CoreData.FactorManager.GetFactor(userContext, (int)FactorId.RedlistCategory));

                var taxonIds = new List<int> { 100024, 103025, 1603, 206046 }; // add varg, blåmes, alpklöver, älg
                var taxa = CoreData.TaxonManager.GetTaxa(userContext, taxonIds);

                foreach (var taxon in taxa)
                {
                    speciesFactSearchCriteria.Taxa.Add(taxon);
                }

                var speciesFactList = CoreData.SpeciesFactManager.GetSpeciesFacts(userContext, speciesFactSearchCriteria);

                var resultList = (from s in speciesFactList
                                  select TaxonSearchResultItemViewModel.CreateFromTaxon(s.Taxon)).ToList();

                jsonModel = JsonModel.Create(resultList);

                /*
                var dic = speciesFactList.ToDictionary(speciesFact => speciesFact.Taxon.Id);

                // Get all possible red list category values
                var enumValues = new List<IFactorFieldEnumValue>();
                foreach (var enumValue in dic[100024].Factor.DataType.Field1.Enum.Values)
                {
                    enumValues.Add(enumValue);
                }
                */
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        public JsonNetResult GetTaxaByFactor(string factorFieldViewModels, Int32 factorId, Boolean restrictToCurrentTaxonFilter)
        {
            JsonModel jsonModel;

            try
            {
                List<TaxonSearchFactorFieldViewModel> taxonSearchFactorFieldViewModels = JsonConvert.DeserializeObject(factorFieldViewModels, typeof(List<TaxonSearchFactorFieldViewModel>)) as List<TaxonSearchFactorFieldViewModel>;

                IUserContext user = GetCurrentUser();

                var viewManager = new TaxonSearchManager(user);

                List<TaxonSearchResultItemViewModel> resultList = viewManager.SearchTaxa(taxonSearchFactorFieldViewModels, factorId, restrictToCurrentTaxonFilter, SessionHandler.MySettings);

                jsonModel = JsonModel.Create(resultList);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Add TaxonIds to the filter.
        /// </summary>
        /// <param name="strTaxonIds">The selected taxa to add.</param>
        /// <param name="returnUrl">The return URL.</param>        
        /// <returns>A redirection to returnUrl.</returns>
        [HttpPost]
        public RedirectResult AddTaxaToFilter(string strTaxonIds, string returnUrl)
        {
            int[] taxonIds = JsonConvert.DeserializeObject(strTaxonIds, typeof(int[])) as int[];
            if (taxonIds.IsNotNull())
            {
                var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.AddTaxonIds(taxonIds);
            }

            return Redirect(returnUrl);
        }

        /// <summary>
        /// Add TaxonIds to the filter.
        /// </summary>
        /// <param name="strTaxonIds">The selected taxa to add.</param>
        /// <returns>A JsonNetResult.</returns>
        public JsonNetResult AddFilteredTaxaToFilter(string strTaxonIds)
        {
            JsonModel jsonModel = JsonModel.CreateSuccess("");
            try
            {
                int[] taxonIds = JsonConvert.DeserializeObject(strTaxonIds, typeof(int[])) as int[];
                if (taxonIds.IsNotNull())
                {
                    var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                    viewManager.AddTaxonIds(taxonIds);
                }
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Add underlying species to the filter.
        /// </summary>
        /// <param name="strTaxonIds">The selected taxa to search underlying species from.</param>
        /// <param name="returnUrl">The return URL.</param>        
        /// <returns>A redirection to returnUrl.</returns>
        [HttpPost]
        public RedirectResult AddUnderlyingSpeciesToFilter(string strTaxonIds, string returnUrl)
        {
            int[] parentTaxonIds = JsonConvert.DeserializeObject(strTaxonIds, typeof(int[])) as int[];
            if (parentTaxonIds.IsNotNull())
            {
                var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.AddUnderlyingSpeciesTaxonIds(parentTaxonIds);
            }

            return Redirect(returnUrl);
        }

        /// <summary>
        /// First remove existing taxon from filter, then add selected taxon to filter.
        /// </summary>
        /// <param name="strTaxonIds">The selected taxa to add.</param>
        /// <returns>A JsonNetResult.</returns>
        public JsonNetResult ReplaceTaxaInFilter(string strTaxonIds)
        {
            JsonModel jsonModel = JsonModel.CreateSuccess("");
            try
            {
                int[] taxonIds = JsonConvert.DeserializeObject(strTaxonIds, typeof(int[])) as int[];
                if (taxonIds.IsNotNull())
                {
                    var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                    viewManager.RemoveAllTaxa();
                    viewManager.AddTaxonIds(taxonIds);
                }
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Removes a taxon from the filter.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns></returns>
        [HttpPost]
        public RedirectResult RemoveFilteredTaxon(string returnUrl, int taxonId)
        {
            var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.RemoveTaxonId(taxonId);
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Removes taxa from the filter.
        /// </summary>
        /// <param name="strTaxonIds">The taxon ids to remove.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        public RedirectResult RemoveTaxaFromFilter(string strTaxonIds, string returnUrl)
        {
            int[] taxonIds = JsonConvert.DeserializeObject(strTaxonIds, typeof(int[])) as int[];
            var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.RemoveTaxonIds(taxonIds);
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Removes the specified Taxon Id from MySettings.
        /// </summary>
        /// <param name="data">Taxon Id as JSON.</param>
        /// <returns></returns>
        public JsonNetResult RemoveTaxonId(string data)
        {
            JsonModel jsonModel;
            try
            {
                var javascriptSerializer = new JavaScriptSerializer();
                TaxonViewModel taxonViewModel = javascriptSerializer.Deserialize<TaxonViewModel>(data);
                var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.RemoveTaxonId(taxonViewModel.TaxonId);
                jsonModel = JsonModel.CreateSuccess("");
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        ///// <summary>
        ///// Removes the specified Taxon Ids from MySettings.
        ///// </summary>
        ///// <param name="data">Taxon Ids as JSON.</param>
        ///// <returns></returns>
        //public JsonNetResult RemoveTaxonIds(string data)
        //{
        //    JsonModel jsonModel;
        //    try
        //    {
        //        var javascriptSerializer = new JavaScriptSerializer();
        //        int[] taxonIds = javascriptSerializer.Deserialize<int[]>(data);
        //        var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
        //        viewManager.RemoveTaxonIds(taxonIds);
        //        jsonModel = JsonModel.CreateSuccess("");
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonModel = JsonModel.CreateFailure(ex.Message);
        //    }

        //    return new JsonNetResult(jsonModel);
        //}

        /// <summary>
        /// Removes the specified Taxon Ids from MySettings.
        /// </summary>
        /// <param name="data">Taxon Ids as JSON.</param>
        /// <returns></returns>
        public JsonNetResult RemoveTaxonIdsFromFilter(int[] taxonIds)
        {
            JsonModel jsonModel;
            try
            {                
                var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.RemoveTaxonIds(taxonIds);
                jsonModel = JsonModel.CreateSuccess("");
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Removes all filtered taxon.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        public RedirectResult RemoveAllFilteredTaxon(string returnUrl)
        {
            var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.RemoveAllTaxa();
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Gets all filtered taxa.
        /// </summary>
        /// <returns></returns>
        public JsonNetResult GetFilteredTaxa()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<TaxonViewModel> taxaList = viewManager.GetAllTaxaViewModel();
                jsonModel = JsonModel.Create(taxaList);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Get field filter expression.
        /// </summary>
        /// <returns></returns>
        public String GetFieldFilterExpression()
        {
            var viewManager = new FieldsFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);

            return viewManager.GetFieldFilterExpression();
        }

        /// <summary>
        /// Get field filter logical operator as string.
        /// </summary>
        /// <returns></returns>
        public String GetFieldLogicalOperatorString()
        {
            var viewManager = new FieldsFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);

            return viewManager.GetFieldFilterLogicalOperatorAsString();
        }

        /// <summary>
        /// Get fields by class name.
        /// </summary>
        /// <returns></returns>
        public JsonNetResult GetFieldsByClassName(String className)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new FieldsFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<FieldListItemViewModel> fields = viewManager.GetFieldsViewModelByClassName(className);
                jsonModel = JsonModel.Create(fields);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Add field to the filter by converting values into a SpeciesObservationFieldSearchCriteria.
        /// </summary>
        /// <param name="strClassName">The class name to add.</param>
        /// <param name="strFieldValue">The field value to add.</param>
        /// <param name="strFieldId">The field id to add.</param>
        /// <param name="strFieldName">The field name to add.</param>
        /// <param name="strCompareOperator">The field compare operator to add.</param>
        /// <param name="strLogicalOperator">The field logical combine operator.</param>
        /// <param name="returnUrl">The return URL.</param>        
        /// <returns>A redirection to returnUrl.</returns>
        [HttpPost]
        public RedirectResult AddFieldToFilter(
            string strClassName,
            string strFieldValue,
            string strFieldId,
            string strFieldName,
            string strCompareOperator,
            string strLogicalOperator,
            string returnUrl)
        {
            Int32 fieldId;
            strClassName = strClassName.Trim('"');
            strFieldValue = strFieldValue.Trim('"');
            strFieldId = strFieldId.Trim('"');
            strFieldName = strFieldName.Trim('"');
            strCompareOperator = strCompareOperator.Trim('"');
            strLogicalOperator = strLogicalOperator.Trim('"');

            if (strFieldName.IsNotEmpty() && Int32.TryParse(strFieldId, out fieldId))
            {
                var viewManager = new FieldsFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);

                if (strClassName.IsEmpty())
                {
                    Dictionary<Int32, string> fieldClassMap = viewManager.GetFieldClassDictionary();
                    strClassName = fieldClassMap[fieldId];
                }

                CompareOperator compareOperator = (CompareOperator)Enum.Parse(typeof(CompareOperator), strCompareOperator, true);
                LogicalOperator logicalOperator = (LogicalOperator)Enum.Parse(typeof(LogicalOperator), strLogicalOperator, true);
                SpeciesObservationClassId classId = (SpeciesObservationClassId)Enum.Parse(typeof(SpeciesObservationClassId), strClassName, true);
                SpeciesObservationPropertyId propertyId;
                SpeciesObservationProperty property;

                if (Enum.TryParse(strFieldName, true, out propertyId))
                {
                    property = new SpeciesObservationProperty { Id = propertyId };
                }
                else
                {
                    ISpeciesObservationFieldDescription fieldDescription = CoreData.MetadataManager.GetSpeciesObservationFieldDescriptions(GetCurrentUser()).Get(strFieldId.WebParseInt32());
                    ISpeciesObservationFieldMapping fieldMapping = fieldDescription.Mappings[0];
                    property = new SpeciesObservationProperty { Id = SpeciesObservationPropertyId.None, Identifier = fieldMapping.PropertyIdentifier };
                }

                // Get data type associated to current field by class name
                DataType dataType = viewManager.GetTypeByFieldAndClass(fieldId, strClassName);

                var fieldSearchCriteria = new SpeciesObservationFieldSearchCriteria()
                {
                    Class = new SpeciesObservationClass { Id = classId },
                    Property = property,
                    Operator = compareOperator,
                    Type = dataType,
                    Value = strFieldValue
                };

                viewManager.AddFieldFilterExpressions(fieldSearchCriteria);
                viewManager.SetFieldFilterLogicalOperator(logicalOperator);
            }

            return Redirect(returnUrl);
        }

        [ChildActionOnly]
        public PartialViewResult SearchSettingsSummaryPartial(List<KeyValuePair<string, string>> searchSettings)
        {
            return PartialView(searchSettings);
        }

        /// <summary>
        /// Uploads a geojson file to be used as spatial filter.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system id.</param>
        /// <returns>A JsonModel.</returns>
        [HttpPost]
        public JsonNetResult UploadSpatialFilterFile(int? coordinateSystemId)
        {
            JsonModel jsonModel;
            HttpPostedFileBase file;

            if (Request.Files.Count == 0)
            {
                jsonModel = JsonModel.CreateFailure("No files uploaded.");
                return new JsonNetResult(jsonModel);
            }

            if (Request.Files.Count > 1)
            {
                jsonModel = JsonModel.CreateFailure("Too many files uploaded.");
                return new JsonNetResult(jsonModel);
            }

            file = Request.Files[0];
            if (file == null || file.ContentLength == 0)
            {
                jsonModel = JsonModel.CreateFailure("The file has no content.");
                return new JsonNetResult(jsonModel);
            }

            bool identifyCrsFromFileContent = !coordinateSystemId.HasValue;
            SpatialFilterViewManager viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            try
            {
                string str = new StreamReader(file.InputStream).ReadToEnd();
                FeatureCollection featureCollection = JsonConvert.DeserializeObject(str, typeof(FeatureCollection)) as FeatureCollection;
                CoordinateSystem coordinateSystem = null;
                if (identifyCrsFromFileContent)
                {
                    coordinateSystem = GisTools.GeoJsonUtils.FindCoordinateSystem(featureCollection);
                    if (coordinateSystem.Id == CoordinateSystemId.None)
                    {
                        jsonModel = JsonModel.CreateFailure(Resource.FilterSpatialUnableToDetermineCoordinateSystem);
                        jsonModel.Status = CoordinateSystemId.SWEREF99_TM.ToString();
                        return new JsonNetResult(jsonModel);
                    }
                }

                if (coordinateSystemId.HasValue)
                {
                    coordinateSystem = new CoordinateSystem((CoordinateSystemId)coordinateSystemId.Value);
                }
                var featureCollectionValidationResult = GisTools.GeometryTools.ValidateFeatureCollectionGeometries(featureCollection);
                if (!featureCollectionValidationResult.IsValid)
                {
                    var firstValidationError = featureCollectionValidationResult.InvalidFeatureResults.First();
                    string strError = string.Format("Geometry error! {0}\r\n", firstValidationError.ValidationResult.Description);
                    strError += " The feature with the invalid geometry has the following properties: \r\n";
                    strError += string.Join(", \r\n", firstValidationError.Feature.Properties.Select(x => x.Key + "=" + x.Value).ToArray());
                    jsonModel = JsonModel.CreateFailure(strError);
                    return new JsonNetResult(jsonModel);
                }

                viewManager.UpdateSpatialFilter(featureCollection, coordinateSystem);
                SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterSpatialSettingsUpdated));
                jsonModel = JsonModel.CreateSuccess("Ok");
            }
            catch (Exception)
            {
                jsonModel = JsonModel.CreateFailure(Resource.FilterSpatialUnableToParseGeoJsonFile);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Updates the spatial filter.
        /// </summary>
        /// <param name="geojson">The polygons as a geojson string.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonNetResult UpdateSpatialFilter(string geojson)
        {
            JsonModel jsonModel;
            try
            {
                FeatureCollection featureCollection = JsonConvert.DeserializeObject(geojson, typeof(FeatureCollection)) as FeatureCollection;
                var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.UpdateSpatialFilter(featureCollection);
                jsonModel = JsonModel.CreateSuccess(Resource.FilterSpatialUpdateSuccess);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        public RedirectResult ResetPolygons(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Spatial.Polygons.Clear();
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterSpatialClearSuccess));
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Clears the spatial filter.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonNetResult ClearSpatialFilter()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.ClearSpatialFilter();
                jsonModel = JsonModel.CreateSuccess(Resource.FilterSpatialClearSuccess);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Gets the spatial filter as GeoJSON.
        /// </summary>
        /// <returns></returns>
        public JsonNetResult GetSpatialFilterAsGeoJSON()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                FeatureCollection featureCollection = viewManager.GetSpatialFilterAsFeatureCollection();
                jsonModel = JsonModel.CreateFromObject(featureCollection);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Gets the spatial filter bounding box as GeoJSON.
        /// </summary>
        /// <returns>The bounding box of the spatial filter in the coordinate system set in <see cref="PresentationMapSetting"/>.</returns>
        public JsonNetResult GetSpatialFilterBboxAsGeoJSON()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                FeatureCollection featureCollection = viewManager.GetSpatialFilterBboxAsFeatureCollection();
                jsonModel = JsonModel.CreateFromObject(featureCollection);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        [HttpPost]
        public JsonNetResult AddTaxaByIdToFilter(int[] taxaIds)
        {
            if (taxaIds.IsNotNull())
            {
                var viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.AddTaxonIds(taxaIds);
                return new JsonNetResult(true);
            }

            return new JsonNetResult(false);
        }

        /// <summary>
        /// Shows a dialog where the user can upload a GeoJson file to be used as spatial filter.
        /// </summary>
        /// <returns>Partial view that should be rendered.</returns>
        public PartialViewResult SetFilterFromGeoJsonDialog()
        {
            var model = new UploadGeoJsonViewModel(Url.Action("UploadSpatialFilterFile"), Url.Action("Spatial"))
            {
                FileNameRegEx = "geojson",
                FileFormatDescription = Resource.FilterPolygonFromMapLayerFileUploadFormats
            };

            return PartialView(model);
        }

        /// <summary>
        /// Shows a dialog where the user can get taxon list by factors to be used in taxon filter.
        /// </summary>
        /// <returns>Partial view that should be rendered.</returns>
        public PartialViewResult SetTaxonFilterFromFactorDialog(Int32 factorId)
        {
            IFactor factor = CoreData.FactorManager.GetFactor(GetCurrentUser(), factorId);
            TaxaFilterViewManager viewManager = new TaxaFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            TaxaFilterFromFactorViewModel model = viewManager.CreateTaxaFilterFromFactorViewModel(factor);

            if (model.Factor.DataType == null)
            {
                return PartialView("FactorIsHeadLineDialog", model);
            }

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Spatial()
        {
            ViewBag.RenderMapScriptTags = true;
            var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            FilterSpatialViewModel model = viewManager.CreateFilterSpatialViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Spatial(string strPolygons, string strRegions)
        {
            var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var javascriptSerializer = new JavaScriptSerializer();
            int[] selectedRegions = javascriptSerializer.Deserialize<int[]>(strRegions);
            if (viewManager.GetAllRegions().Count == 0 && selectedRegions.Length > 0)
            {
                SessionHandler.MySettings.Filter.Spatial.IsActive = true;
            }

            viewManager.ResetRegions();
            viewManager.AddRegions(selectedRegions);

            FeatureCollection featureCollection = JsonConvert.DeserializeObject(strPolygons, typeof(FeatureCollection)) as FeatureCollection;
            viewManager.UpdateSpatialFilter(featureCollection);

            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterSpatialSettingsUpdated));
            return RedirectToAction("Spatial");
        }

        public RedirectResult ResetSpatialSettings(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Spatial.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterSpatialSettingsReset));
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Renders the spatial filter view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SpatialDrawPolygon()
        {
            ViewBag.RenderMapScriptTags = true;
            var model = new SpatialDrawPolygonViewModel();
            model.IsSettingsDefault = SessionHandler.MySettings.Filter.Spatial.IsPolygonSettingsDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult SpatialDrawPolygon(string data)
        {
            FeatureCollection featureCollection = JsonConvert.DeserializeObject(data, typeof(FeatureCollection)) as FeatureCollection;
            var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateSpatialFilter(featureCollection);
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterSpatialSettingsUpdated));
            return RedirectToAction("SpatialDrawPolygon");
        }

        [HttpGet]
        public ActionResult SpatialCommonRegions()
        {
            var viewManager = new CommonRegionsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            CommonRegionsViewModel model = viewManager.CreateCommonRegionsViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SpatialCommonRegions(string data)
        {
            var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var javascriptSerializer = new JavaScriptSerializer();
            int[] selectedRegions = javascriptSerializer.Deserialize<int[]>(data);
            if (viewManager.GetAllRegions().Count == 0 && selectedRegions.Length > 0)
            {
                SessionHandler.MySettings.Filter.Spatial.IsActive = true;
            }
            viewManager.ResetRegions();
            viewManager.AddRegions(selectedRegions);
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterCommonRegionsUpdated));
            return RedirectToAction("SpatialCommonRegions");
        }

        public ActionResult PolygonFromMapLayer()
        {
            ViewBag.RenderMapScriptTags = true;
            var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var model = viewManager.CreatePolygonFromMapLayerViewModel();
            return View(model);
        }

        public RedirectToRouteResult AddPolygonsFromMapLayer(string strGeoJson)
        {
            FeatureCollection featureCollection = JsonConvert.DeserializeObject(strGeoJson, typeof(FeatureCollection)) as FeatureCollection;

            ////LineString s = ((GeoJSON.Net.Geometry.Polygon) featureCollection.Features[0].Geometry).Coordinates[0];
            ////StringBuilder sb = new StringBuilder();
            ////foreach (GeographicPosition pos in s.Coordinates)
            ////{
            ////    sb.Append(string.Format("{0} {1}, ", pos.Longitude.ToString(CultureInfo.InvariantCulture), pos.Latitude.ToString(CultureInfo.InvariantCulture)));

            ////}
            ////string s1 = sb.ToString();

            var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            int nrAdded = viewManager.AddPolygons(featureCollection);
            if (nrAdded > 0)
            {
                HttpCookie cookie = Request.Cookies["MapState"];
                if (cookie != null)
                {
                    cookie.Values.Add("action", "zoomToFeatures");
                    Response.AppendCookie(cookie);
                }
            }
            return RedirectToAction("Spatial");
        }

        // OpenLayers links
        //http://openlayers.org/dev/examples/regular-polygons.html
        //http://openlayers.org/dev/examples/editingtoolbar.html
        //http://openlayers.org/dev/examples/editingtoolbar-outside.html
        //http://openlayers.org/dev/examples/draw-feature.html  
        //http://dev.openlayers.org/sandbox/camptocamp/feature/examples/remove-feature.html

        public RedirectResult ResetOccurrence(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Occurrence.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterOccurrenceReset));
            return Redirect(returnUrl);
        }

        public RedirectResult ResetCommonRegions(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Spatial.ResetRegions();
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterCommonRegionsReset));
            return Redirect(returnUrl);
        }

        public RedirectResult ResetTemporal(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Temporal.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterTemporalSettingsReset));
            return Redirect(returnUrl);
        }

        public RedirectResult ResetAccuracy(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Accuracy.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterAccuracySettingsReset));
            return Redirect(returnUrl);
        }

        public RedirectResult ResetTaxa(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Taxa.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterTaxaSettingsReset));
            return Redirect(returnUrl);
        }

        public RedirectResult ResetFields(string returnUrl)
        {
            SessionHandler.MySettings.Filter.Field.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resource.FilterFieldSettingsReset));
            return Redirect(returnUrl);
        }
    }
}
