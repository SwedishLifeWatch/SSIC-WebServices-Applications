using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ArtDatabanken;
using ArtDatabanken.IO;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Json;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using ArtDatabanken.WebApplication.Dyntaxa.Data.SortTaxon;
using System.Web.Routing;
using ArtDatabanken.WebService.Client.TaxonService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;
using Dyntaxa.Helpers;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;

namespace Dyntaxa.Controllers
{    
    public class TaxonController : DyntaxaBaseController
    {
        // Category Id
        private const int RANKLESS = 52;

         // Called "LIVE"
        public TaxonController()
        {
        }

        // Called by test
        public TaxonController(
            IUserDataSource userDataSourceRepository, 
            ITaxonDataSource taxonDataSourceRepository, 
            IPesiNameDataSource pEsiNameDataSourceRepository, 
            ISessionHelper session)
            : base(userDataSourceRepository, taxonDataSourceRepository, session)
        {
            CoreData.TaxonManager.PesiNameDataSource = pEsiNameDataSourceRepository;
        }
        
#if DEBUG
        public ActionResult TestSearchTaxon()
        {
            return View();
        }
#endif       

        public PartialViewResult SearchTaxonPartial()
        {
            return PartialView();
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
            DyntaxaStringCompareOperator? nameCompareOperator,
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
                //IUserContext user = GetCurrentUser();
                var viewManager = new TaxonSearchManager();
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
                    List<TaxonSearchResultItemViewModel> resultList = viewManager.SearchTaxa(GetCurrentUser(), searchOptions);
                    //List<Tuple<string, string>> extra = searchOptions.GetSearchDescription();
                    //string searchSettingsHtml = RenderPartialViewToString("SearchSettingsSummaryPartial", extra);
                    jsonModel = JsonModel.Create(resultList);
                }
                else
                {
                    jsonModel = JsonModel.CreateFailure("Not enough search criterias");
                }
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Delete(string taxonId)
        {
            int revisionId = this.RevisionId.Value;
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            string revErrorId = string.Empty;
            string taxonErrorId = string.Empty;
           
            try
            {
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                } 
                
                var searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ITaxon taxon = searchResult.Taxon;
                IUserContext loggedInUser = GetLoggedInUser();
                ViewBag.Taxon = taxon;
                TaxonModelManager modelManager = new TaxonModelManager();
                TaxonDeleteViewModel model = modelManager.GetDeleteTaxonViewModel(loggedInUser, taxon, revisionId);
                  ViewData.Model = model;
               
                if (model.ErrorMessage.IsNull())
                {
                    return View("Delete", model);
                }
                else
                {
                    taxonErrorId = model.TaxonErrorId;
                    revErrorId = model.RevisionErrorId;
                    errorMsg = model.ErrorMessage;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManager = new ErrorModelManager(new Exception(), "Taxon", "Delete");
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonDropParentHeaderText,
                Resources.DyntaxaResource.TaxonDropParentHeaderText,
                errorMsg, 
                taxonErrorId, 
                revErrorId, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Delete(TaxonDeleteViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            int revisionId = this.RevisionId.Value;
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                if (loggedInUser.IsNotNull())
                {
                    ValidateTaxon(GetCurrentUser(), Int32.Parse(model.TaxonId));

                    if (ModelState.IsValid)
                    {
                        bool isValid = ModelState.IsValid;
                         
                        ITaxon taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, Int32.Parse(model.TaxonId));
                        ITaxonRevision taxonRevision = this.TaxonRevision;
                        // Check taxon and revision validity
                        bool taxonTest = CheckTaxonVaildity(model.TaxonId, taxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }
                        // Check revision
                        taxonRevision = this.TaxonRevision;
                        bool revisionTest = CheckRevisionValidity(model.RevisionId, taxonRevision);
                        if (!revisionTest)
                        {
                            isValid = false;
                        }

                        if (isValid)
                        {
                            IList<ITaxonRelation> parentTaxa = taxon.GetParentTaxonRelations(loggedInUser, true, false, true);
                            int parentId = 0;
                            if (parentTaxa.IsNotNull() && parentTaxa.Count > 0)
                            {
                                parentId = parentTaxa.First().ChildTaxon.Id;
                            }

                            using (ITransaction transaction = loggedInUser.StartTransaction())
                            {
                                CoreData.TaxonManager.DeleteTaxon(loggedInUser, taxon, taxonRevision);
                                transaction.Commit();
                            }
                            
                            if (parentId != 0)
                            {
                                // Reload tree.
                                var id = this.RootTaxonId;
                                if (id != null)
                                {
                                    this.RedrawTree((int)id, (int)parentId);
                                }

                                // Set parent taxon as identifer
                                this.TaxonIdentifier = TaxonIdTuple.CreateFromId((int)parentId);
                                return RedirectToAction("Info", new { @taxonId = parentId });
                            }
                            else
                            {
                                // Reload tree.
                                var id = this.RootTaxonId;
                                if (id != null)
                                {
                                    this.RedrawTree((int)id);
                                }

                                return RedirectToAction("SearchResult");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                    }

                     return View("Delete", model);
                }            
                else
                {
                    errorMsg = Resources.DyntaxaResource.SharedInvalidUserContext;
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManager = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonDeleteInfoHeaderText,
                Resources.DyntaxaResource.TaxonDeleteInfoHeaderText,
                errorMsg, 
                model.TaxonId, 
                model.RevisionId, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// Performs a taxon search
        /// </summary>
        /// <param name="searchString">the search string</param>
        /// <param name="model">the model that will be updated with search results</param>        
        private void DoSearch(string searchString, TaxonSearchViewModel model)
        {
            model.SearchString = searchString;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int taxonId;
            ITaxon taxon = null;
            if (int.TryParse(searchString, out taxonId))
            {                
                try
                {
                    taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId);
                    if (taxon != null)
                    {
                        model.IsTaxonInRevision = true;
                    }
                }
                catch (Exception)
                {
                    // No taxon exist in this revision
                    model.IsTaxonInRevision = false;
                    try
                    {
                        ITaxon taxonOutside = CoreData.TaxonManager.GetTaxon(GetApplicationUser(), taxonId);
                        if (taxonOutside.IsNotNull())
                        {
                            model.IsTaxonExisting = true;
                        }
                    }
                    catch (Exception)
                    {
                        model.IsTaxonExisting = false;
                        model.IsAmbiguousResult = true;
                    }
                }                
            }

            model.RootTaxonId = this.RootTaxonId;
            IList<ITaxonName> foundTaxa = TaxonSearchManager.SearchTaxons(model.SearchOptions.CreateTaxonNameSearchCriteriaObject());
            
            // If not in revision, delete all names in search result that has status [Removed].
            if (RevisionId == null && foundTaxa != null && foundTaxa.Count > 0)
            {
                IList<ITaxonName> prunedList = new List<ITaxonName>();
                foreach (ITaxonName taxonName in foundTaxa)
                {
                    if (taxonName.Status.Id != (int)TaxonNameStatusId.Removed)
                    {
                        prunedList.Add(taxonName);
                    }                    
                }

                foundTaxa = prunedList;
            }

            if (taxon != null)
            {
                model.SearchResult = new TaxonSearchResultList(taxon, foundTaxa);
            }
            else
            {
                model.SearchResult = new TaxonSearchResultList(foundTaxa);
            }

            stopwatch.Stop();
            model.SearchTime = stopwatch.Elapsed;
        }

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult Search(string search)
        {
            var model = new TaxonSearchViewModel();
            if (string.IsNullOrEmpty(search))
            {
                //todo handle empty string
            }
            else
            {                
                return RedirectToAction("SearchResult", "Taxon", new { search });                
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Search(TaxonSearchViewModel model)
        {
            return RedirectToAction("SearchResult", "Taxon", new { @search = model.SearchString });
        }

        [HttpGet]
        public ActionResult ResetSearchOptions()
        {
            RemoveCookie("SearchOptions");            
            return RedirectToAction("SearchResult");
        }

        [HttpGet]
        [ValidateInput(false)]
        public ViewResult SearchResult(string search, string returnParameters, string returnController, string returnAction, string sort, string page, int? taxonId)
        {            
            if (taxonId.HasValue)
            {
                this.TaxonIdentifier = TaxonIdTuple.CreateFromId(taxonId.Value);                
            }

            TaxonSearchViewModel model = null;
            if (TempData.ContainsKey("TaxonSearchViewModel"))
            {
                model = (TaxonSearchViewModel)TempData["TaxonSearchViewModel"];
            }
            else
            {
                model = new TaxonSearchViewModel();
            }

            // restore search options if we are sorting
            if (!string.IsNullOrEmpty(sort) && Session["SearchOptions"] != null)
            {
                model.SearchOptions = (TaxonSearchOptions)Session["SearchOptions"];
            }

            model.SearchOptions.RestoreFromCookie(this.Request.Cookies["SearchOptions"]);

            model.IsTaxonExisting = true;
            model.IsTaxonInRevision = true;
            
            if (!string.IsNullOrEmpty(search))
            {
                search = TaxonSearchViewModel.ConvertSearchString(Request.RawUrl);
            }
            model.SearchString = search;
            model.SearchOptions.NameSearchString = model.SearchString;
            model.RootTaxonId = this.RootTaxonId;
            
            if (model.SearchOptions.LastUpdatedStartDate.HasValue && !model.SearchOptions.LastUpdatedEndDate.HasValue)
            {
                model.SearchOptions.LastUpdatedEndDate = DateTime.Now;
            }
            if (!model.SearchOptions.LastUpdatedStartDate.HasValue)
            {
                model.SearchOptions.LastUpdatedEndDate = null;
            }

            if (string.IsNullOrEmpty(search))
            {
                //todo handle empty string                
            }
            if (!string.IsNullOrEmpty(returnController) && !string.IsNullOrEmpty(returnAction))
            {
                model.IsAmbiguousResult = true;
                if (!string.IsNullOrEmpty(returnParameters))
                {
                    model.LinkParams = DecodeRouteQueryString(returnParameters);
                }

                model.LinkAction = returnAction;
                model.LinkController = returnController;
                model.LinkParamsString = returnParameters;
            }

            // restore search result if we are sorting or paging
            if ((!string.IsNullOrEmpty(sort) || !string.IsNullOrEmpty(page)) && Session["SearchResult"] != null)
            {
                model.SearchResult = (TaxonSearchResultList)Session["SearchResult"];
            }
            else if (model.SearchOptions.CanSearch())
            {
                DoSearch(search, model);
            }
            if (model.IsAmbiguousResult && (model.SearchResult == null || model.SearchResult.Count == 0))
            {
                model.IsZeroRowsResult = true;
            }

            // set the current taxon to the first in search result
            if (!(model.IsAmbiguousResult || model.IsZeroRowsResult))
            {
                if (model.SearchResult != null && model.SearchResult.Count > 0)
                {
                    this.TaxonIdentifier = TaxonIdTuple.CreateFromId(model.SearchResult[0].TaxonId);                    
                }
            }

            if (this.TaxonIdentifier.Id.HasValue)
            {
                ViewBag.Taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), this.TaxonIdentifier.Id.Value);
            }

            Session.Add("SearchOptions", model.SearchOptions);
            Session.Add("SearchResult", model.SearchResult);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SearchResult(TaxonSearchViewModel model)
        {
            var rvd = new RouteValueDictionary();
            rvd.Add("search", model.SearchString);
            if (model.IsAmbiguousResult)
            {
                rvd.Add("returnController", model.LinkController);
                rvd.Add("returnAction", model.LinkAction);
                if (!string.IsNullOrEmpty(model.LinkParamsString))
                {
                    rvd.Add("returnParameters", model.LinkParamsString);
                }
            }
            TempData.Add("TaxonSearchViewModel", model);
            this.Response.AppendCookie(model.SearchOptions.CreateCookie("SearchOptions"));            
            return RedirectToAction("SearchResult", "Taxon", rvd);
        }
        
        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        [JsonpFilter]
        public JsonResult GetCategoryTaxa(int categoryId)
        {
            IUserContext userContext = GetCurrentUser();
            ITaxonSearchCriteria searchCriteria = new TaxonSearchCriteria();
            searchCriteria.TaxonCategoryIds = new List<int>(new[] { categoryId });
            TaxonList taxa = CoreData.TaxonManager.GetTaxa(userContext, searchCriteria);
            
            List<ITaxon> sortedTaxaList = taxa.Cast<ITaxon>().ToList();
            sortedTaxaList = (from taxon in sortedTaxaList orderby taxon.ScientificName select taxon).ToList();

            var objects = new List<object>();
            foreach (ITaxon taxon in sortedTaxaList)
            {                
                objects.Add(new { TaxonId = taxon.Id, Name = taxon.Category.Name + ": " + taxon.ScientificName });
            }
            return Json(objects, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]        
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Move(string taxonId)
        {
            if (taxonId.IsNull())
            {
                taxonId = this.TaxonIdentifier.Id.ToString();
            } 
            
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ITaxon taxon = searchResult.Taxon;
            ViewBag.Taxon = taxon;
            var viewManager = new TaxonMoveViewManager(CoreData.UserManager.GetCurrentUser());
            var model = viewManager.CreateTaxonMoveViewModel(taxon, TaxonRevision);
            return View(model);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]        
        public ActionResult Move(TaxonMoveViewModel moveData)
        {
            IUserContext userContext = GetCurrentUser();
            string errorMessage = string.Empty;
            bool isOkToMove = true;
                
            var viewManager = new TaxonMoveViewManager(CoreData.UserManager.GetCurrentUser());
            if (moveData.NewParentTaxon.HasValue)
            {
                ValidateTaxon(GetCurrentUser(), moveData.NewParentTaxon.Value);
            }
            if (ModelState.IsValid)
            {
                // Get the GENUS (SLÄKTE) taxon category
                ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(CoreData.UserManager.GetCurrentUser(), TaxonCategoryId.Genus);

                ITaxon newParentTaxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), moveData.NewParentTaxon.Value);

                // Don't check category based rules if the new parent has category = RANKLESS 
                if (newParentTaxon.Category.Id != RANKLESS)
                {
                    const int ARTKOMPLEX_ID = 28;
                    foreach (int childTaxonId in moveData.SelectedChildTaxaToMove)
                    {
                        // Check category based rules to see if the move is allowed
                        ITaxon childTaxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), childTaxonId);
                        if ((childTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.SortOrder < 
                                newParentTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.SortOrder)
                            && childTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.Id != RANKLESS
                            && childTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.Id != ARTKOMPLEX_ID)
                        {
                            errorMessage = Resources.DyntaxaResource.TaxonMoveMoveIllegalMoveErrorText + " " + childTaxon.ScientificName;
                            isOkToMove = false;
                            break;
                        }

                        /* REGEL BORTKOMMENTERAD 2013-06-18. DET BLIR INTE RIKTIGT RÄTT. -- GuNy
                        if (childTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.SortOrder > genusTaxonCategory.SortOrder &&
                            newParentTaxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.SortOrder < genusTaxonCategory.SortOrder)
                        {
                            errorMessage = Resources.DyntaxaResource.TaxonMoveMoveGenusCategoryErrorText + " " + childTaxon.ScientificName; 
                            isOkToMove = false;
                            break; 
                        }
                        */
                    }
                }

                if (isOkToMove)
                {
                    viewManager.MoveTaxa(moveData.SelectedChildTaxaToMove, moveData.OldParentTaxonId, moveData.NewParentTaxon.Value, this.TaxonRevision);

                    ITaxon movedTaxon = CoreData.TaxonManager.GetTaxon(userContext, moveData.SelectedChildTaxaToMove[0]);
                    // Reload tree.
                    var id = this.RootTaxonId;
                    if (id != null)
                    {
                        this.RedrawTree((int)id, movedTaxon.Id);
                    }
                    // Set moved taxon as identifer
                    this.TaxonIdentifier = TaxonIdTuple.CreateFromId(movedTaxon.Id);

                    if (movedTaxon.Category.SortOrder >= genusTaxonCategory.SortOrder)
                    {
                        TempData["moveChangeName"] = moveData;                        
                        return this.RedirectToAction("MoveChangeName");                        
                    }

                    return this.RedirectToAction("Info", "Taxon", new { @taxonId = this.TaxonIdentifier.Id });                    
                }
            }
            
            viewManager.ReInitializeTaxonMoveViewModel(moveData, TaxonRevision, isOkToMove);
            moveData.MoveErrorText = errorMessage;
            return View(moveData);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult MoveChangeName(TaxonMoveViewModel moveData)
        {
            moveData = (TaxonMoveViewModel)TempData["moveChangeName"];
            ModelState.Clear(); // for some reason ModelState.IsValid=false when it should be true. Need to clear the error...
            var viewManager = new TaxonMoveViewManager(CoreData.UserManager.GetCurrentUser());
            var model = viewManager.CreateTaxonMoveChangeNameViewModel(moveData.OldParentTaxonId, moveData.NewParentTaxon.Value, moveData.SelectedChildTaxaToMove);            
            return View(model);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult MoveChangeName(TaxonMoveChangeNameViewModel postModel)
        {
            var viewManager = new TaxonMoveViewManager(CoreData.UserManager.GetCurrentUser());
            ValidateTaxon(GetCurrentUser(), this.TaxonIdentifier.Id);
            if (ModelState.IsValid)
            {
                viewManager.SaveNameChanges(postModel.MovedChildTaxons, this.TaxonRevision);
                RedrawTree();
                return this.RedirectToAction("Info", "Taxon", new { @taxonId = this.TaxonIdentifier.Id });                
            }

            var model = viewManager.CreateTaxonMoveChangeNameViewModel(postModel.OldParentTaxonId, postModel.NewParentTaxonId, postModel.SelectedChildTaxaToMove);
            return View(model);
        }

        /// <summary>
        /// GET: Dyntaxa/Taxon/Index 
        /// </summary>
        /// <returns>Redirect to read.</returns>
        public ActionResult Index()
        {
            return RedirectToAction("Info");
        }

        public ActionResult TaxonSummaryTaxon(ITaxon taxon, bool? ignoreExpand)
        {
            if (ignoreExpand.GetValueOrDefault() == true)
            {
                ViewBag.IgnoreExpand = true;
            }
            var model = TaxonSummaryViewModel.Create(GetCurrentUser(), taxon, this.RevisionId);
            return PartialView("TaxonSummary", model);
        }

        public ActionResult TaxonSummary(int taxonId, bool? ignoreExpand)
        {
            if (ignoreExpand.GetValueOrDefault() == true)
            {
                ViewBag.IgnoreExpand = true;
            }
            var model = TaxonSummaryViewModel.Create(GetCurrentUser(), taxonId, this.RevisionId);
            return PartialView(model);
            //if (Request.IsAjaxRequest())

            //{
            //    return Json(model, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return PartialView(model);
            //}           
        }

        /// <summary>
        /// Detail for a taxon.
        /// This method is supposed to be used by AP2
        /// </summary>
        /// <param name="taxonId">Id of requested taxon</param>
        /// <returns></returns>
        [JsonpFilter]
        public JsonResult Detail(int taxonId)
        {
            var model = TaxonSummaryViewModel.Create(GetCurrentUser(), taxonId, this.RevisionId);
            return Json(model, JsonRequestBehavior.AllowGet);
            //return PartialView("TaxonSummary", model);
        }

        /// <summary>
        /// Creates a map based on county occurrence information from Artfakta.
        /// </summary>
        /// <param name="taxonId">TaxonId</param>
        /// <returns></returns>
        public FileResult CountyMap(int taxonId)
        {
            try
            {
#if LOG_SPEED
                var sp = new Stopwatch();
                sp.Start();
#endif
                Bitmap map = null;
                var returnStream = new MemoryStream();
                IUserContext userContext = GetCurrentUser();

                try
                {
                    ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, taxonId);
                    CountyOccurrenceMap countyMapProvider = new CountyOccurrenceMap(userContext, taxon);
                    countyMapProvider.Height = 404;
                    countyMapProvider.UpdateInformation = Resources.DyntaxaResource.TaxonInfoDistributionInSwedenCountyOccurrence;
                    map = countyMapProvider.Bitmap;

                    map.Save(returnStream, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    // Usually when no data in species information exists, shows default image - c:\Dev\ArtDatabanken\Web\Dyntaxa\Dyntaxa\Images\Temp\map_data_missing.png

                    Image image = Image.FromFile(Server.MapPath(Url.Content("~/Images/Temp/map_data_missing.png")));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        // Save image to stream.
                        image.Save(stream, ImageFormat.Png);
                    }

                    map = new Bitmap(image);
                    map.Save(returnStream, System.Drawing.Imaging.ImageFormat.Png);
                }

                returnStream.Position = 0;

#if LOG_SPEED
                sp.Stop();
                DyntaxaLogger.WriteMessage("Taxon Info - Map creation: {0:N0} milliseconds", sp.ElapsedMilliseconds);
#endif
                return new FileStreamResult(returnStream, "image/png");
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        [HttpGet]        
        public ActionResult Info(string taxonId, bool? changeRoot)
        {            
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            ITaxon taxon = searchResult.Taxon;
            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            //TaxonInfoQualityChartModelHelper modelHelper = new TaxonInfoQualityChartModelHelper();
            TaxonInfoViewModel model = new TaxonInfoViewModel(taxon, GetCurrentUser(), this.TaxonRevision, this.RevisionTaxonCategorySortOrder);            
            ViewBag.Taxon = taxon;
            SetTempDataTaxon(taxon);            

            // check if we should replace the root taxon.
            bool isNewRequest = this.Request.UrlReferrer == null;            
            if (isNewRequest || changeRoot.GetValueOrDefault())
            {                
                this.RedrawTree(taxon);                
            }

            return View(model);
        }

        public FileResult TaxonQualitySummaryChart(int taxonId)
        {
            try
            {
#if LOG_SPEED
                var sp = new Stopwatch();
                sp.Start();
#endif
                IUserContext user = GetCurrentUser();
                ITaxon taxon = GetTempDataTaxon(taxonId);
                TaxonChildQualityStatisticsList taxonQualitySummaryList = CoreData.TaxonManager.GetTaxonChildQualityStatistics(user, taxon);
                var helper = new TaxonInfoQualityChartModelHelper();

#if LOG_SPEED
                sp.Stop();
                DyntaxaLogger.WriteMessage("Taxon Info - Chart: {0:N0} milliseconds", sp.ElapsedMilliseconds);
#endif
                return new FileStreamResult(helper.GetQualityChart(taxonQualitySummaryList), "image/png");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Renders the SwedishOccurrenceSummary partial view.
        /// </summary>
        /// <param name="taxon">The taxon.</param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult SwedishOccurrenceSummary(SwedishOccurrenceSummaryViewModel model)
        {            
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult TaxonRecommendedLinks(ITaxon taxon)
        {
            IUserContext user = GetCurrentUser();            
            List<LinkItem> recommendedLinks = TaxonInfoViewModel.GetRecommendedLinks(taxon, DyntaxaHelper.IsInRevision(user, this.TaxonRevision));
            return PartialView(recommendedLinks);
        }

        [HttpGet]
        public PartialViewResult DistributionInSwedenLinks(ITaxon taxon, SwedishOccurrenceSummaryViewModel model)
        {
            DistributionInSwedenViewModel viewModel = new DistributionInSwedenViewModel();
            viewModel.DistributionLink = model.RedListLink;
            if (viewModel.DistributionLink != null)
            {
                viewModel.DistributionLink.LinkText = Resources.DyntaxaResource.TaxonInfoDistributionInSwedenCountyOccurrence;
            }
            //IUserContext user = GetCurrentUser();
            //List<LinkItem> recommendedLinks = TaxonInfoViewModel.GetRecommendedLinks(taxon, DyntaxaHelper.IsInRevision(user, this.TaxonRevision));
            return PartialView(viewModel);
        }

        /// <summary>
        /// GET: Dyntaxa/Taxon/Add/id
        /// Get a add child view for given taxon
        /// </summary>
        /// <returns>A taxon add view.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Add(string taxonId, bool isOkToCreateTaxon = true)
        {
            int revisionId = this.RevisionId.Value;
            string errorMsg = string.Empty;
            string additionalErrorMsg = string.Empty;

            try
            {
                IUserContext loggedInUser = GetLoggedInUser();

                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                }

                // Gets taxon if not redirected to taxon search and when taxon found we continue with add.
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId, null);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);

                ITaxon parentTaxon = searchResult.Taxon;
                // Create our view 
                TaxonModelManager modelManager = new TaxonModelManager();
                ITaxonRevision taxonRevision = this.TaxonRevision;
                IList<ITaxonCategory> possibleTaxonCategories =
                               CoreData.TaxonManager.GetPossibleTaxonCategories(loggedInUser, parentTaxon);
               TaxonAddViewModel model = modelManager.GetAddTaxonViewModel(loggedInUser, parentTaxon, taxonRevision, possibleTaxonCategories);
               model.IsOkToCreateTaxon = isOkToCreateTaxon;

               if (model.ErrorMessage.IsNotNull())
               {
                   // TODO get all view data dropdown lists
                   errorMsg = model.ErrorMessage;
                   ModelState.AddModelError(string.Empty, errorMsg);
                   model.ParentTaxonId = parentTaxon.Id.ToString();
                   if (model.TaxonCategoryList.IsNull())
                   {
                       model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();
                   }
               }
               ViewData.Model = model;
               return View("Add", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManager = new ErrorModelManager(new Exception(), "Taxon", "Add");
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonAddPageTitle,
                Resources.DyntaxaResource.TaxonAddPageHeader,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        // POST 
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        [HttpPost]
        public ActionResult Add(TaxonAddViewModel model)
        {
            string errorMsg = string.Empty;
            int revisionId = this.RevisionId.Value;
            string additionalErrorMsg = null;
            TaxonModelManager modelManager = new TaxonModelManager();
            IList<ITaxonCategory> possibleTaxonCategories = new List<ITaxonCategory>();
            ITaxonCategory category = null;
            try
            {                
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                    IUserContext loggedInUser = GetLoggedInUser();
                    ITaxonRevision taxonRevision = null;
                    ITaxon parentTaxon = null;
                    int parentTaxonId = Int32.Parse(model.ParentTaxonId);
                    bool userTest = base.CheckUserContextValidity(loggedInUser);
                    
                    if (!userTest)
                    {
                        isValid = false;
                    }
                    else
                    {
                        //Get taxon id
                        parentTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, parentTaxonId);
                        // Check taxon and revision validity
                        bool taxonTest = CheckTaxonVaildity(model.ParentTaxonId, parentTaxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }
                        // Check revision
                        taxonRevision = this.TaxonRevision;
                        bool revisionTest = CheckRevisionValidity(taxonRevision.Id.ToString(), taxonRevision);
                        if (!revisionTest)
                        {
                            isValid = false;
                        }

                        // Check category, if not set 
                        if (model.TaxonCategoryId == 0)
                        {
                            errorMsg = Resources.DyntaxaResource.SharedTaxonPickCategoryText;
                            ModelState.AddModelError("TaxonCategoryId", errorMsg);
                            ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                            isValid = false;
                        }
                        else
                        {
                            category = CoreData.TaxonManager.GetTaxonCategory(loggedInUser, model.TaxonCategoryId);
                            if (category.IsNull())
                            {
                                errorMsg = Resources.DyntaxaResource.SharedTaxonInvalidCategoryText;
                                ModelState.AddModelError("TaxonCategoryId", errorMsg);
                                ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                                isValid = false;
                            }
                        }
                        possibleTaxonCategories = CoreData.TaxonManager.GetPossibleTaxonCategories(loggedInUser, parentTaxon);
                    }
                    if (isValid)
                    {
                        ITaxon newTaxon = new Taxon();
                        newTaxon.Id = Int32.MinValue;
                        newTaxon.DataContext = new DataContext(loggedInUser);

                        TaxonAlertStatusId alert = TaxonAlertStatusId.Green;
                        // Update alert status
                        if (model.TaxonIsProblematic)
                        {
                            alert = TaxonAlertStatusId.Yellow;
                        }

                        using (ITransaction transaction = loggedInUser.StartTransaction())
                        {
                            CoreData.TaxonManager.CreateTaxon(loggedInUser, taxonRevision, newTaxon, model.ScientificName, model.CommonName, model.Author, alert, parentTaxon, category, model.Description); 
                            // Create and set default references to taxon and taxon scentific and common names.
                            ReferenceRelationList referencesToAdd = ReferenceHelper.GetDefaultReferences(loggedInUser, newTaxon, taxonRevision, null);
                            referencesToAdd = ReferenceHelper.GetDefaultReferences(loggedInUser, newTaxon.GetScientificName(loggedInUser), taxonRevision, referencesToAdd);
                            referencesToAdd = ReferenceHelper.GetDefaultReferences(loggedInUser, newTaxon.GetCommonName(loggedInUser), taxonRevision, referencesToAdd);
                            CoreData.ReferenceManager.CreateDeleteReferenceRelations(loggedInUser, referencesToAdd, new ReferenceRelationList());
                            transaction.Commit();
                        }
                        // PESI IDs created outside the transactionscope
                        newTaxon.ScientificName = model.ScientificName;
                        
                        using (ITransaction transaction = loggedInUser.StartTransaction())
                        {
                            CoreData.TaxonManager.CreatePESIData(loggedInUser, newTaxon, taxonRevision.GetRevisionEvents(loggedInUser).Last());

                            transaction.Commit();
                        }
                        
                        // Reload tree.
                        var id = this.RootTaxonId;
                        if (id != null)
                        {
                            this.RedrawTree((int)id, newTaxon.Id);
                        }

                        // set new taxon as identifer
                        this.TaxonIdentifier = TaxonIdTuple.CreateFromId(newTaxon.Id);

                        return RedirectToAction("Edit", new { taxonId = newTaxon.Id.ToString(), isTaxonNew = true });
                    }
                    else
                    {
                        modelManager.ReloadTaxonAddViewModel(loggedInUser, parentTaxon, model, category, possibleTaxonCategories);
                        if (model.ErrorMessage.IsNotNull())
                        {
                            errorMsg = model.ErrorMessage;
                            ModelState.AddModelError(string.Empty, errorMsg);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                    model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();
                }

                return View("Add", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
                DyntaxaLogger.WriteMessage("Add taxon: " + errorMsg + " additional errormsg: " + additionalErrorMsg);
            }

            var errorModelManager = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonAddPageTitle,
                Resources.DyntaxaResource.TaxonAddPageHeader,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        public ActionResult DoesScientificNameExist(string scientificName)
        {
            IUserContext user = GetApplicationUser();            

            if (!string.IsNullOrEmpty(scientificName))
            {
                var searchCriteria = new TaxonNameSearchCriteria();
                searchCriteria.NameSearchString = new StringSearchCriteria();
                searchCriteria.NameSearchString.SearchString = scientificName;
                searchCriteria.NameSearchString.CompareOperators = new List<StringCompareOperator>();
                searchCriteria.NameSearchString.CompareOperators.Add(StringCompareOperator.Equal);

                TaxonNameList taxonNames = CoreData.TaxonManager.GetTaxonNames(user, searchCriteria);
                if (taxonNames.IsNotEmpty())
                {
                    ITaxon taxon = taxonNames[0].Taxon;   
                    return Json(new { returnvalue = true, category = taxon.Category.Name ?? "-", scientificName = taxon.ScientificName ?? "-", taxonId = taxon.Id }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { returnvalue = false }, JsonRequestBehavior.AllowGet);
        } 

        /// <summary>
        /// GET: Dyntaxa/Taxon/Edit/id
        /// Ge a edit view for given taxon
        /// </summary>
        /// <returns>A taxon edit view.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Edit(string taxonId, bool isTaxonNew = false)
        {
            int revisionId = this.RevisionId.Value;
            string errorMsg = string.Empty;
            string additionalErrorMsg = string.Empty; 
        
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                }
                // Gets taxon if not redirected to taxon search and when taxon found we continue with add.
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId, null);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
               
                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                // Create our view 
                TaxonModelManager modelManager = new TaxonModelManager();
                ITaxonRevision taxonRevision = this.TaxonRevision;
              //  IList<ITaxonCategory> possibleTaxonCategories = CoreData.TaxonManager.GetPossibleTaxonCategories(loggedInUser, taxon);
                 TaxonCategoryList taxonCategories = CoreData.TaxonManager.GetTaxonCategories(loggedInUser);
                 TaxonEditViewModel model = modelManager.GetEditTaxonViewModel(loggedInUser, taxon, taxonRevision, taxon.Category, taxonCategories, isTaxonNew);

                if (model.ErrorMessage.IsNotNull())
                {
                    // TODO get all view data dropdown lists
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                    model.TaxonId = taxon.Id.ToString();
                }
                
                ViewData.Model = model;
                return View("Edit", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManager = new ErrorModelManager(new Exception(), "Taxon", "Edit");
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonEditHeaderLabel,
                Resources.DyntaxaResource.TaxonEditHeaderLabel,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        // POST 
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        [HttpPost]
        public ActionResult Edit(TaxonEditViewModel model)
        {
            string errorMsg = string.Empty;

            string additionalErrorMsg = null;
            var modelManager = new TaxonModelManager();
            //IList<ITaxonCategory> possibleTaxonCategories = new List<ITaxonCategory>();
            var taxonCategories = new TaxonCategoryList();
            ITaxonCategory category = null;
            try
            {
                // Init all Quality data fist
                modelManager.InitQualityAndCategoryDropDown(model);
                modelManager.InitSpeciesDropDown(model);
                ValidateTaxon(GetCurrentUser(), Int32.Parse(model.TaxonId));
                model.TaxonQualityDescription = model.TaxonQualityDescription.IsNull() ? string.Empty : model.TaxonQualityDescription;

                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                    IUserContext loggedInUser = GetLoggedInUser();
                    ITaxonRevision taxonRevision = null;
                    ITaxon taxon = null;
                    int taxonId = Int32.Parse(model.TaxonId);
                    bool userTest = base.CheckUserContextValidity(loggedInUser);
                    TaxonAlertStatusId alert = TaxonAlertStatusId.Green;
                    
                    if (!userTest)
                    {
                        isValid = false;
                    }
                    else
                    {
                        //Get taxon id
                        taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, taxonId);
                        // Check taxon and revision validity
                        bool taxonTest = CheckTaxonVaildity(model.TaxonId, taxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }
                        // Check revision
                        taxonRevision = this.TaxonRevision;
                        bool revisionTest = CheckRevisionValidity(taxonRevision.Id.ToString(), taxonRevision);
                        if (!revisionTest)
                        {
                            isValid = false;
                        }

                        // Check category
                        if (model.TaxonCategoryId == 0)
                        {
                            errorMsg = Resources.DyntaxaResource.SharedTaxonPickCategoryText;
                            ModelState.AddModelError("TaxonCategoryId", errorMsg);
                            isValid = false;
                        }
                        else
                        {
                            category = CoreData.TaxonManager.GetTaxonCategory(loggedInUser, model.TaxonCategoryId);
                            if (category.IsNull())
                            {
                                errorMsg = Resources.DyntaxaResource.SharedTaxonInvalidCategoryText;
                                ModelState.AddModelError("TaxonCategoryId", errorMsg);
                                ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                                isValid = false;
                            }
                        }
                        // possibleTaxonCategories = CoreData.TaxonManager.GetPossibleTaxonCategories(loggedInUser, taxon);
                         taxonCategories = CoreData.TaxonManager.GetTaxonCategories(loggedInUser);
                         alert = (TaxonAlertStatusId)taxon.AlertStatus.Id;
                        
                         // Update alert status
                        if (taxon.AlertStatus.Id != (Int32)TaxonAlertStatusId.Red)
                        {
                            if (model.TaxonIsProblematic)
                            {
                                alert = TaxonAlertStatusId.Yellow;
                            }
                            else
                            {
                                alert = TaxonAlertStatusId.Green;
                            }
                        }

                        // Validate Artfakta
                        const int SwedishOccurrenceNotFoundStatusId = 498;
                        // Must enter immigration history if Swedish occurrence is set to a value that isn't Not found.
                        if (model.SwedishOccurrenceStatusId != 0 && model.SwedishOccurrenceStatusId != SwedishOccurrenceNotFoundStatusId)
                        {
                            if (model.SwedishImmigrationHistoryStatusId == 0)
                            {
                                ModelState.AddModelError("", Resources.DyntaxaResource.TaxonEditSwedishOccurrenceValidationMessage);
                                isValid = false;
                            }
                        }

                        // If Immigration history Status is set, then Reference and Quality must be set.
                        if (model.SwedishImmigrationHistoryStatusId != 0 &&
                            (model.SwedishImmigrationHistoryQualityId == 0 || model.SwedishImmigrationHistoryReferenceId == 0))
                        {
                            ModelState.AddModelError("", Resources.DyntaxaResource.TaxonEditImmigrationHistoryValidationMessage);
                            isValid = false;
                        }
                        // End validate Artfakta

                        //if (true)
                        //{
                        //    errorMsg = Resources.DyntaxaResource.SharedTaxonInvalidCategoryText;
                        //    ModelState.AddModelError("TaxonCategoryId", errorMsg);
                        //    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                        //    isValid = false;
                        //}
                    }
                    if (isValid)
                    {
                        // Update dyntaxa data
                        using (ITransaction transaction = loggedInUser.StartTransaction())
                        {
                            CoreData.TaxonManager.UpdateTaxon(loggedInUser, taxon, taxonRevision, model.Description, category, alert, model.IsMicrospecies);
                            transaction.Commit();
                        }
                        // Update speciesfact data
                        try
                        {
                            modelManager.SaveQualitySpeciesFact(loggedInUser, model, taxon, true, true);
                            if (taxon.Category.Id >= Resources.DyntaxaSettings.Default.GenusTaxonCategoryId)
                            {
                                modelManager.SaveSwedishOccurrenceAndHistoryToTaxonDatabase(loggedInUser, model, taxon, taxonRevision.Id);
                            }
                            
                            SpeciesFactHelper.RefreshCache(loggedInUser);
                            return RedirectToAction("Edit", new { @taxonId = model.TaxonId });                                
                        }
                        catch (Exception)
                        {
                            modelManager.ReloadTaxonEditViewModel(loggedInUser, taxonRevision.Id, taxon, model, category, taxonCategories); 
                            errorMsg = Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError;
                            ModelState.AddModelError("", errorMsg);
                        }
                    }
                    else
                    {
                        modelManager.ReloadTaxonEditViewModel(loggedInUser, RevisionId, taxon, model, category, taxonCategories);
                        if (model.ErrorMessage.IsNotNull())
                        {
                            errorMsg = model.ErrorMessage;
                            ModelState.AddModelError(string.Empty, errorMsg);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }

                return View("Edit", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManager = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonEditHeaderLabel,
                Resources.DyntaxaResource.TaxonEditHeaderLabel,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// GET: Dyntaxa/Taxon/Edit/id
        /// Get a view for managing references for a given taxon.
        /// </summary>
        /// <returns>A taxon edit view.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEditor)]
        public ActionResult EditReferences(string taxonId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = string.Empty;

            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                }
                // Gets taxon if not redirected to taxon search and when taxon found we continue with add.
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId, null);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);

                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                return RedirectToAction("Add", "Reference", new { @GUID = taxon.Guid, @taxonId = taxon.Id, @showReferenceApplyMode = true });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManager = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.SharedReferenceLabel,
                Resources.DyntaxaResource.SharedReferenceLabel,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// GET: Dyntaxa/Taxon/AddParent/id, revisionId
        /// Adds parent taxa for given taxon
        /// </summary>
        /// <returns>A taxon add parent view, if not ok return an error view.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult AddParent(string taxonId)
        {
            int revisionId = this.RevisionId.Value;
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                }
                // Gets taxon if not redirected to taxon search and when taxon found we continue with add parent.
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId, null);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                IUserContext loggedInUser = GetLoggedInUser();
                TaxonModelManager modelManager = new TaxonModelManager();
                TaxonAddParentViewModel model = modelManager.GetAddParentViewModel(loggedInUser, taxon, this.TaxonRevision);

                ViewData.Model = model;
               
                if (model.ErrorMessage.IsNotNull())
                {
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                }
                return View("AddParent", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManager = new ErrorModelManager(new Exception(), "Taxon", "AddParent");
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonDropParentHeaderText,
                Resources.DyntaxaResource.TaxonDropParentHeaderText,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// POST: Dyntaxa/Taxon/AddParent/model
        /// Adds parent taxa for given taxon
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A taxon add parent view, if not ok return an error view.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        [HttpPost]
        public ActionResult AddParent(TaxonAddParentViewModel model)
        {
            string errorMsg = string.Empty;
            bool isOkToAdd = true;
            string additionalErrorMsg = null;
            try
            {
                ValidateTaxon(GetCurrentUser(), Int32.Parse(model.TaxonId));
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                    IUserContext loggedInUser = GetLoggedInUser();
                    ITaxonRevision taxonRevision = null;
                    ITaxon taxon = null;
                    bool userTest = base.CheckUserContextValidity(loggedInUser);
                    if (!userTest)
                    {
                        isValid = false;
                    }
                    else
                    {
                        taxonRevision = this.TaxonRevision;
                        taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, Int32.Parse(model.TaxonId));
                        
                        // Check taxon and revision validity
                        bool taxonTest = CheckTaxonVaildity(model.TaxonId, taxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }

                        bool revisionTest = CheckRevisionValidity(model.RevisionId, taxonRevision);
                        if (!revisionTest)
                        {
                            isValid = false;
                        }
                    }
                    if (isValid)
                    { 
                        if (model.SelectedTaxonList.IsNotNull() && model.SelectedTaxonList.Count > 0)
                        {
                            ITaxonCategory genusTaxonCategory = CoreData.TaxonManager.GetTaxonCategory(CoreData.UserManager.GetCurrentUser(), TaxonCategoryId.Genus);
                            
                            foreach (string item in model.SelectedTaxonList)
                            {
                                ITaxon newParent = CoreData.TaxonManager.GetTaxon(loggedInUser, Int32.Parse(item));
                                ITaxonCategory newParentTaxonCategory = newParent.GetCheckedOutChangesTaxonProperties(loggedInUser).TaxonCategory;

                                // Check if sort order is correct ie if it is possible to add parent

                                // Check category based rules to see if the move is allowed
                                if (taxon.GetCheckedOutChangesTaxonProperties(loggedInUser).TaxonCategory.SortOrder <
                                        newParent.GetCheckedOutChangesTaxonProperties(loggedInUser).TaxonCategory.SortOrder)
                                    {
                                        errorMsg = Resources.DyntaxaResource.TaxonMoveMoveIllegalMoveErrorText + " " + taxon.ScientificName;
                                        isOkToAdd = false;
                                        break;
                                    }

                                /* REGEL BORTKOMMENTERAD 2013-06-18. DET BLIR INTE RIKTIGT RÄTT. -- GuNy
                                if (taxon.GetCheckedOutChangesTaxonProperties(loggedInUser).TaxonCategory.SortOrder > genusTaxonCategory.SortOrder &&
                                    newParentTaxonCategory.SortOrder < genusTaxonCategory.SortOrder &&
                                    newParentTaxonCategory.IsTaxonomic)
                                    {
                                        errorMsg = Resources.DyntaxaResource.TaxonMoveMoveGenusCategoryErrorText + " " + taxon.ScientificName;
                                        isOkToAdd = false;
                                        break;
                                    }
                                */
                                using (ITransaction transaction = loggedInUser.StartTransaction())
                                {
                                    CoreData.TaxonManager.MoveTaxon(loggedInUser, taxon, null, newParent, taxonRevision);

                                    transaction.Commit();
                                }
                            }
                            if (isOkToAdd)
                            {
                                // Reload tree.
                                var id = this.RootTaxonId;
                                if (id != null)
                                {
                                    this.RedrawTree((int)id, taxon.Id);
                                }
                                // Set first taxon as identifer
                                this.TaxonIdentifier = TaxonIdTuple.CreateFromId(taxon.Id);

                                return RedirectToAction("AddParent", new { @taxonId = model.TaxonId });
                            }
                            else
                            {
                                TaxonModelManager modelManager = new TaxonModelManager();
                                model = modelManager.GetAddParentViewModel(loggedInUser, taxon, this.TaxonRevision, isOkToAdd);
                                model.AddParentErrorText = errorMsg;
                            }
                        }
                        else
                        {
                            // TODO fix so that button is not enabled when no items is selected in list
                            return RedirectToAction("AddParent", new { @taxonId = model.TaxonId }); // Must update model with the list of parent taxa before viewing it again
                        }
                    }
                    else
                    {
                        TaxonModelManager modelManager = new TaxonModelManager();
                        model.TaxonList = modelManager.GetParents(loggedInUser, taxon);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }
                
                return View("AddParent", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManager = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonDropParentHeaderText,
                Resources.DyntaxaResource.TaxonDropParentHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
       }

        /// <summary>
        /// GET: Dyntaxa/Taxon/DropParent/id, revisionId
        /// Delete parent taxa for given taxon
        /// </summary>
        /// <returns>A taxon drop parent view, if not ok return an error view.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult DropParent(string taxonId, bool isReloaded = false)
        {
            int revisionId = this.RevisionId.Value;
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
             try
            {
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.TaxonId;
                }
                 // Gets taxon if not redirected to taxon search and when taxon found we continue with add parent.
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId, null);
                }
                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                IUserContext loggedInUser = GetLoggedInUser();

                TaxonModelManager modelManager = new TaxonModelManager();
                TaxonDropParentViewModel model = modelManager.GetDropParentViewModel(loggedInUser, taxon, this.TaxonRevision, isReloaded);
                model.DialogTextPopUpText = Resources.DyntaxaResource.TaxonDropParentErrorText;
                model.DialogTitlePopUpText = Resources.DyntaxaResource.TaxonDropParentHeaderText;

                ViewData.Model = model;

                if (model.ErrorMessage.IsNotNull())
                {
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                }
                return View("DropParent", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
            var errorModelManager = new ErrorModelManager(new Exception(), "Taxon", "DropParent");
            ErrorViewModel errorModel = errorModelManager.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonDropParentHeaderText, 
                Resources.DyntaxaResource.TaxonDropParentHeaderText, 
                errorMsg, 
                additionalErrorMsg);
           
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// POST: Dyntaxa/Taxon/DropParent/model
        /// Deletes parent taxa for given taxon
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A taxon drop parent view, if not ok return an error view.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        [HttpPost]
        public ActionResult DropParent(TaxonDropParentViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            try
            {
                ValidateTaxon(GetCurrentUser(), Int32.Parse(model.TaxonId));  
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                    IUserContext loggedInUser = GetLoggedInUser();
                    ITaxonRevision taxonRevision = null;
                    ITaxon taxon = null;
                    bool userTest = base.CheckUserContextValidity(loggedInUser);
                    if (!userTest)
                    {
                        isValid = false;
                    }
                    else
                    {
                        taxonRevision = this.TaxonRevision;
                        taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, Int32.Parse(model.TaxonId));
                        // Check taxon and revision validity
                        bool taxonTest = CheckTaxonVaildity(model.TaxonId, taxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }

                        bool revisionTest = CheckRevisionValidity(model.RevisionId, taxonRevision);
                        if (!revisionTest)
                        {
                            isValid = false;
                        }
                    }

                    if (isValid)
                    {                           
                        if (model.SelectedTaxonList.IsNotNull() && model.SelectedTaxonList.Count > 0)
                        {
                            using (ITransaction transaction = loggedInUser.StartTransaction())
                            {
                                foreach (string item in model.SelectedTaxonList)
                                {
                                    ITaxon parentToBeRemoved = CoreData.TaxonManager.GetTaxon(loggedInUser, Int32.Parse(item));
                                    CoreData.TaxonManager.MoveTaxon(loggedInUser, taxon, parentToBeRemoved, null, taxonRevision);
                                }
                                transaction.Commit();
                            }
                            // Reload tree.
                            var id = this.RootTaxonId;
                            if (id != null)
                            {
                                this.RedrawTree((int)id, taxon.Id);
                            }
                            // Set first taxon as identifer
                            this.TaxonIdentifier = TaxonIdTuple.CreateFromId(taxon.Id);

                            return RedirectToAction("DropParent", new { @taxonId = model.TaxonId, isReloaded = true });
                        }
                        else
                        {
                            // TODO fix so that button is not enabled when no items is selected in list
                            return RedirectToAction("DropParent", new { @taxonId = model.TaxonId });
                        }
                    }
                    else
                    {
                        // Must update model with the list of parent taxa before viewing it again
                        TaxonModelManager modelManager = new TaxonModelManager();
                        model.TaxonList = modelManager.GetParents(loggedInUser, taxon);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }
                    
                return View("DropParent", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }
                 
            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonDropParentHeaderText,
                Resources.DyntaxaResource.TaxonDropParentHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Lump(string taxonId, bool isOkToLump = true, bool isReloaded = false)
        {
            int revisionId = this.RevisionId.Value;
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            
            try
            {
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                }
                
                var searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                IUserContext loggedInUser = GetLoggedInUser();

                TaxonReplaceModelManager modelManager = new TaxonReplaceModelManager();

                int? replaceTaxonId = null;
                if (this.ReplaceTaxonId.IsNotNull())
                {
                    replaceTaxonId = this.ReplaceTaxonId.Value;  
                }
                List<int?> lumpTaxonIdList = null;
                if (this.LumpTaxonIdList.IsNotNull() && this.LumpTaxonIdList.Count > 0)
                {
                   lumpTaxonIdList = this.LumpTaxonIdList;
                }
                TaxonLumpViewModel model = modelManager.GetTaxonLumpViewModel(loggedInUser, taxon, revisionId, replaceTaxonId, lumpTaxonIdList, isOkToLump, isReloaded);

                ViewData.Model = model;

                if (model.ErrorMessage.IsNotNull())
                {
                    ModelState.AddModelError("", model.ErrorMessage);
                }
                return View("Lump", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonLumpHeaderText,
                Resources.DyntaxaResource.TaxonLumpHeaderText,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult LumpReset(string taxonId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                if (this.ReplaceTaxonId.IsNotNull())
                {
                    this.ReplaceTaxonId = null;
                }

                if (this.LumpTaxonIdList.IsNotNull())
                {
                    this.LumpTaxonIdList = null;
                }

                return RedirectToAction("Lump", new { taxonId = taxonId });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonLumpHeaderText,
                Resources.DyntaxaResource.TaxonLumpHeaderText,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Lump(TaxonLumpViewModel model, string buttonClicked)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            bool reload = true;
            try
            {
                ValidateTaxon(GetCurrentUser(), Int32.Parse(model.TaxonId));
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                    IUserContext loggedInUser = GetLoggedInUser();
                    bool userTest = base.CheckUserContextValidity(loggedInUser);
                    ITaxonRevision taxonRevision = null;
                    ITaxon taxon = null;
                    TaxonReplaceModelManager modelManager = new TaxonReplaceModelManager();
                    if (!userTest)
                    {
                        isValid = false;
                    }
                    else
                    {
                        taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, Int32.Parse(model.TaxonId));
                        taxonRevision = this.TaxonRevision;
                        // Check taxon and revision validity
                        bool taxonTest = CheckTaxonVaildity(model.TaxonId, taxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }

                        bool revisionTest = CheckRevisionValidity(model.RevisionId, taxonRevision);
                        if (!revisionTest)
                        {
                            isValid = false;
                        }
                    } 
                    if (isValid)
                    {
                        int? replaceTaxonId = Int32.Parse(model.TaxonId);

                        if (buttonClicked.Equals(model.Labels.SetCurrentTaxon))
                        {
                            this.ReplaceTaxonId = replaceTaxonId;
                        }
                        else if (buttonClicked.Equals(model.Labels.AddCurrentTaxonToList))
                        {
                            if (this.LumpTaxonIdList.IsNull())
                            {
                                this.LumpTaxonIdList = new List<int?>();
                            }
                            if (!this.LumpTaxonIdList.Contains(replaceTaxonId))
                            {
                                this.LumpTaxonIdList.Add(replaceTaxonId);
                            }
                        }
                        else if (buttonClicked.Equals(model.Labels.RemoveSelectedTaxon))
                        {
                            if (model.SelectedTaxa != null && this.LumpTaxonIdList.IsNotNull())
                            {
                                foreach (int removingTaxon in model.SelectedTaxa)
                                {
                                    if (this.LumpTaxonIdList.Contains(removingTaxon))
                                    {
                                        this.LumpTaxonIdList.Remove(removingTaxon);
                                    }
                                }
                            }
                        }
                        else if (buttonClicked.Equals(model.Labels.RemoveReplacingTaxon))
                        {
                            if (ReplaceTaxonId.IsNotNull())
                            {
                                ReplaceTaxonId = null;
                            }
                        }
                        // Performe lump
                        else if (buttonClicked.Equals(model.Labels.GetSelectedLump))
                        {
                            if (LumpTaxonIdList.IsNotNull() && LumpTaxonIdList.Count > 0 && ReplaceTaxonId.IsNotNull())
                            {
                                // Check if its ok to lump
                                TaxonList taxaToLump = new TaxonList();
                                foreach (var taxonid in LumpTaxonIdList)
                                {
                                    ITaxon lumpTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, (int)taxonid);
                                    taxaToLump.Add(lumpTaxon);
                                }
                                if (CoreData.TaxonManager.IsOkToLumpTaxa(loggedInUser, taxaToLump, CoreData.TaxonManager.GetTaxon(loggedInUser, (int)ReplaceTaxonId)))
                                {
                                    modelManager.LumpTaxa(loggedInUser, ReplaceTaxonId, taxonRevision, LumpTaxonIdList);
                                    int? newLumpTaxon = ReplaceTaxonId;
                                    LumpTaxonIdList = null;
                                    ReplaceTaxonId = null;
                                   
                                    // Reload tree.
                                    var id = RootTaxonId;
                                    if (id != null)
                                    {
                                        RedrawTree((int)id, (int)newLumpTaxon);
                                    }

                                    // Set lump taxon as identifer
                                    TaxonIdentifier = TaxonIdTuple.CreateFromId((int)newLumpTaxon);
                                    return RedirectToAction("List", "TaxonName", new { taxonId = newLumpTaxon.ToString() });
                                }
                                else
                                {
                                    //model.LumpTaxonList = modelManager.GetTaxonList(loggedInUser, this.LumpTaxonIdList);
                                    //model.ReplacingTaxon = modelManager.GetTaxon(loggedInUser, this.ReplaceTaxonId);
                                    //if (model.ErrorMessage.IsNotNull() && model.ErrorMessage.IsNotEmpty())
                                    //{
                                    //    ModelState.AddModelError("", model.ErrorMessage);
                                    //}
                                    //ModelState.AddModelError("", Resources.DyntaxaResource.TaxonLumpNotPossibleToLumpErrorText);
                                    //return View("Lump", model);
                                    model.IsOkToLump = false;
                                    reload = false;
                                }
                            }
                        }
                        else // Cancelbutton pressed
                        {
                            if (this.ReplaceTaxonId.IsNotNull())
                            {
                                this.ReplaceTaxonId = null;
                            }
                            if (this.LumpTaxonIdList.IsNotNull())
                            {
                                this.LumpTaxonIdList = null;
                            }
                        }
                        return RedirectToAction("Lump", new { taxonId = model.TaxonId, isOkToLump = model.IsOkToLump, isReloaded = reload });  
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }
                return View("Lump", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonLumpHeaderText,
                Resources.DyntaxaResource.TaxonLumpHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// GET: Dyntaxa/Taxon/Spieciesfact/id
        /// Get spiecies fact for selected taxon
        /// </summary>
        /// <returns>A taxon edit view.</returns>
       [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEditor)]
        public ActionResult EditSwedishOccurance(string taxonId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = string.Empty;

            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                }
                // Gets taxon if not redirected to taxon search and when taxon found we continue with add.
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId, null);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);

                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                // Create our view 
                TaxonModelManager modelManger = new TaxonModelManager();
                TaxonSwedishOccuranceEditViewModel model = modelManger.GetEditSwedishOccuranceViewModel(loggedInUser, taxon, RevisionId);

                if (model.ErrorMessage.IsNotNull())
                {
                    // TODO get all view data dropdown lists
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                    model.TaxonId = taxonId;
                }
                ViewData.Model = model;
                return View("EditSwedishOccurance", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), "Taxon", "EditSwedishOccurance");
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonEditSwedishOccurrenceLabel,
                Resources.DyntaxaResource.TaxonEditSwedishOccurrenceLabel,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        // POST 
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEditor)]
        [HttpPost]
        public ActionResult EditSwedishOccurance(TaxonSwedishOccuranceEditViewModel model)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            TaxonModelManager modelManager = new TaxonModelManager();
               
            try
            {
                // Init dropDown first...
                modelManager.InitSpeciesDropDown(model);
                ValidateTaxon(GetCurrentUser(), Int32.Parse(model.TaxonId));
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                    IUserContext loggedInUser = GetLoggedInUser();
                    ITaxon taxon = null;
                    int taxonId = Int32.Parse(model.TaxonId);
                    
                    bool userTest = base.CheckUserContextValidity(loggedInUser);
                    if (!userTest)
                    {
                        isValid = false;
                    }
                    else
                    {
                        //Get taxon id
                        taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, taxonId);
                        // Check taxon and revision validityloggedInUser
                        bool taxonTest = CheckTaxonVaildity(model.TaxonId, taxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }
                    }

                    // Validate Artfakta
                    const int SwedishOccurrenceNotFoundStatusId = 498;
                    // Must enter immigration history if Swedish occurrence is set to a value that isn't Not found.
                    if (model.SwedishOccurrenceStatusId != 0 && model.SwedishOccurrenceStatusId != SwedishOccurrenceNotFoundStatusId)
                    {
                        if (model.SwedishImmigrationHistoryStatusId == 0)
                        {
                            ModelState.AddModelError("", Resources.DyntaxaResource.TaxonEditSwedishOccurrenceValidationMessage);
                            isValid = false;
                        }
                    }

                    // If Immigration history Status is set, then Reference and Quality must be set.
                    if (model.SwedishImmigrationHistoryStatusId != 0 &&
                        (model.SwedishImmigrationHistoryQualityId == 0 || model.SwedishImmigrationHistoryReferenceId == 0))
                    {
                        ModelState.AddModelError("", Resources.DyntaxaResource.TaxonEditImmigrationHistoryValidationMessage);
                        isValid = false;
                    }
                    // End validate Artfakta

                    if (isValid)
                    {
                        if (taxon.Category.Id >= Resources.DyntaxaSettings.Default.GenusTaxonCategoryId)
                        {
                            modelManager.SaveSwedishOccurrenceAndHistoryToTaxonDatabase(loggedInUser, model, taxon, RevisionId);
                        }
                                                
                        SpeciesFactHelper.RefreshCache(loggedInUser);
                        return RedirectToAction("EditSwedishOccurance", new { @taxonId = model.TaxonId });
                    }
                    modelManager.ReloadEditSwedishOccuranceViewModel(loggedInUser, RevisionId, taxon, model);
                    if (model.ErrorMessage.IsNotNull())
                    {
                        // TODO get all view data dropdown lists
                        errorMsg = model.ErrorMessage;
                        ModelState.AddModelError(string.Empty, errorMsg);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }

                return View("EditSwedishOccurance", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonEditSwedishOccurrenceLabel,
                Resources.DyntaxaResource.TaxonEditSwedishOccurrenceLabel,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// GET: Dyntaxa/Taxon/Spieciesfact/id
        /// Get quality for selected taxon
        /// </summary>
        /// <returns>A taxon edit view.</returns>
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEditor)]
        public ActionResult EditQuality(string taxonId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = string.Empty;

            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                }
                // Gets taxon if not redirected to taxon search and when taxon found we continue with add.
                TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId, null);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);

                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                // Create our view 
                TaxonModelManager modelManger = new TaxonModelManager();
                TaxonEditQualityViewModel model = modelManger.GetTaxonEditQualityViewModel(loggedInUser, taxon);

                if (model.ErrorMessage.IsNotNull())
                {
                    errorMsg = model.ErrorMessage;
                    ModelState.AddModelError(string.Empty, errorMsg);
                    model.TaxonId = taxonId;
                }
                ViewData.Model = model;
                return View("EditQuality", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), "Taxon", "EditQuality");
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonQualityLabel,
                Resources.DyntaxaResource.TaxonQualityLabel,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        // POST 
        [DyntaxaAuthorize(Order = RequiredAuthorization.SpeciesFactEditor)]
        [HttpPost]
        public ActionResult EditQuality(TaxonEditQualityViewModel model, QualityApplyMode? applyMode)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            TaxonModelManager modelManger = new TaxonModelManager();

            try
            {
                // Init dropDown first...
                modelManger.InitQualityDropDown(model);

                model.TaxonQualityDescription = model.TaxonQualityDescription.IsNull() ? string.Empty : model.TaxonQualityDescription;
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                    IUserContext loggedInUser = GetLoggedInUser();
                    ITaxon taxon = null;
                    int taxonId = Int32.Parse(model.TaxonId);

                    bool userTest = base.CheckUserContextValidity(loggedInUser);
                    if (!userTest)
                    {
                        isValid = false;
                    }
                    else
                    {
                        //Get taxon id
                        taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, taxonId);
                        // Check taxon and revision validityloggedInUser
                        bool taxonTest = CheckTaxonVaildity(model.TaxonId, taxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }
                    }

                    if (isValid)
                    {
                        if (applyMode.HasValue)
                        {
                            modelManger.SaveQualitySpeciesFact(loggedInUser, model, taxon, true, false, applyMode.Value);
                        }
                        else
                        {
                            modelManger.SaveQualitySpeciesFact(loggedInUser, model, taxon, true, false);
                        }

                        //modelManger.UpdateSpeciesFact(loggedInUser, model, taxon, RevisionId, false, true, false);
                        return RedirectToAction("EditQuality", new { @taxonId = model.TaxonId });
                    }

                    modelManger.ReloadTaxonEditQualityViewModel(loggedInUser, taxon, model);
                    if (model.ErrorMessage.IsNotNull())
                    {
                         errorMsg = model.ErrorMessage;
                        ModelState.AddModelError(string.Empty, errorMsg);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }

                return View("EditQuality", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonQualityLabel,
                Resources.DyntaxaResource.TaxonQualityLabel,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Split(string taxonId, bool isOkToSplit = true, bool isReloaded = false)
        {
            int revisionId = this.RevisionId.Value;
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
           
            try
            {
                if (taxonId.IsNull())
                {
                    taxonId = this.TaxonIdentifier.Id.ToString();
                }
                
                var searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
                if (searchResult.NumberOfMatches != 1)
                {
                    return RedirectToSearch(taxonId);
                }

                this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
                ITaxon taxon = searchResult.Taxon;
                ViewBag.Taxon = taxon;
                IUserContext loggedInUser = GetLoggedInUser();

                TaxonReplaceModelManager modelManager = new TaxonReplaceModelManager();

                int? splitTaxonId = null;
                if (this.SplitTaxonId.IsNotNull())
                {
                    splitTaxonId = this.SplitTaxonId.Value;
                }
                List<int?> replaceTaxonIdList = null;
                if (this.ReplaceTaxonIdList.IsNotNull() && this.ReplaceTaxonIdList.Count > 0)
                {
                    replaceTaxonIdList = this.ReplaceTaxonIdList;
                }
                TaxonSplitViewModel model = modelManager.GetTaxonSplitViewModel(loggedInUser, taxon, revisionId, replaceTaxonIdList, splitTaxonId, isOkToSplit, isReloaded);
                ViewData.Model = model;

                if (model.ErrorMessage.IsNotNull())
                {
                    ModelState.AddModelError("", model.ErrorMessage);
                }
                
                return View("Split", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonSplitHeaderText,
                Resources.DyntaxaResource.TaxonSplitHeaderText,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult SplitReset(string taxonId)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;

            try
            {
                if (this.ReplaceTaxonIdList.IsNotNull())
                {
                    this.ReplaceTaxonIdList = null;
                }

                if (this.SplitTaxonId.IsNotNull())
                {
                    this.SplitTaxonId = null;
                }

                return RedirectToAction("split", new { taxonId = taxonId });
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonSplitHeaderText,
                Resources.DyntaxaResource.TaxonSplitHeaderText,
                errorMsg, 
                additionalErrorMsg);

            return View("ErrorInfo", errorModel);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult Split(TaxonSplitViewModel model, string buttonClicked)
        {
            string errorMsg = string.Empty;
            string additionalErrorMsg = null;
            bool reload = true;
            try
            {
                IUserContext loggedInUser = GetLoggedInUser();
                ValidateTaxon(GetCurrentUser(), Int32.Parse(model.TaxonId));
                if (ModelState.IsValid)
                {
                    bool isValid = ModelState.IsValid;
                    bool userTest = base.CheckUserContextValidity(loggedInUser);
                    ITaxonRevision taxonRevision = null;
                    if (!userTest)
                    {
                        isValid = false;
                    }
                    else
                    {
                        ITaxon taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, Int32.Parse(model.TaxonId));
                        taxonRevision = this.TaxonRevision;
                        // Example on what to do server validation on, might not be relevent in this case though
                        bool taxonTest = CheckTaxonVaildity(model.TaxonId, taxon);
                        if (!taxonTest)
                        {
                            isValid = false;
                        }

                        bool revisionTest = CheckRevisionValidity(model.RevisionId, taxonRevision);
                        if (!revisionTest)
                        {
                            isValid = false;
                        }
                    }
                    if (isValid)
                    {
                        TaxonReplaceModelManager modelManager = new TaxonReplaceModelManager();
                        int? splitTaxonId = Int32.Parse(model.TaxonId);

                        if (buttonClicked.Equals(model.Labels.SetCurrentTaxon))
                        {
                            this.SplitTaxonId = splitTaxonId;
                        }
                        else if (buttonClicked.Equals(model.Labels.AddCurrentTaxonToList))
                        {
                            if (this.ReplaceTaxonIdList.IsNull())
                            {
                                this.ReplaceTaxonIdList = new List<int?>();
                            }
                            if (!this.ReplaceTaxonIdList.Contains(splitTaxonId))
                            {
                                this.ReplaceTaxonIdList.Add(splitTaxonId);
                            }
                        }
                        else if (buttonClicked.Equals(model.Labels.RemoveSelectedTaxon))
                        {
                            if (model.SelectedTaxa != null && this.ReplaceTaxonIdList.IsNotNull())
                            {
                                foreach (int removingTaxon in model.SelectedTaxa)
                                {
                                    if (this.ReplaceTaxonIdList.Contains(removingTaxon))
                                    {
                                        this.ReplaceTaxonIdList.Remove(removingTaxon);
                                    }
                                }
                            }
                        }
                        else if (buttonClicked.Equals(model.Labels.RemoveReplacingTaxon))
                        {
                            if (this.SplitTaxonId.IsNotNull())
                            {
                                this.SplitTaxonId = null;
                            }
                        }
                        // Performe split
                        else if (buttonClicked.Equals(model.Labels.GetSelectedSplit))
                        {
                            if (this.ReplaceTaxonIdList.IsNotNull() && ReplaceTaxonIdList.Count > 0 && SplitTaxonId.IsNotNull())
                            {
                                // Check if its ok to split
                                TaxonList taxaToReplace = new TaxonList();
                                foreach (var taxonid in ReplaceTaxonIdList)
                                {
                                    ITaxon replaceTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, (int)taxonid);
                                    taxaToReplace.Add(replaceTaxon);
                                }

                                if (CoreData.TaxonManager.IsOkToSplitTaxon(loggedInUser, CoreData.TaxonManager.GetTaxon(loggedInUser, (int)SplitTaxonId), taxaToReplace))
                                {
                                    modelManager.SplitTaxon(loggedInUser, SplitTaxonId, taxonRevision, ReplaceTaxonIdList);
                                    // Reload tree.
                                    int selcetdTaxon = ReplaceTaxonIdList.First().Value;
                                    var id = RootTaxonId;
                                    if (id != null)
                                    {
                                        this.RedrawTree((int)id, selcetdTaxon);
                                    }

                                    // Set first replacing taxon as identifer
                                    this.TaxonIdentifier = TaxonIdTuple.CreateFromId(selcetdTaxon);
                                    this.ReplaceTaxonIdList = null;
                                    this.SplitTaxonId = null;

                                    return RedirectToAction("List", "TaxonName", new { taxonId = selcetdTaxon.ToString() });
                                }
                                else
                                {
                                    model.IsOkToSplit = false;
                                    reload = false;
                                }
                            }
                        }
                        else // Cancelbutton pressed
                        {
                            if (this.SplitTaxonId.IsNotNull())
                            {
                                this.SplitTaxonId = null;
                            }
                            if (this.ReplaceTaxonIdList.IsNotNull())
                            {
                                this.ReplaceTaxonIdList = null;
                            }
                        }
                        return RedirectToAction("Split", new { taxonId = model.TaxonId, isOkToSplit = model.IsOkToSplit, isReloaded = reload });  
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.DyntaxaResource.SharedError);
                }

                return View("Split", model);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                additionalErrorMsg = e.StackTrace;
            }

            var errorModelManger = new ErrorModelManager(new Exception(), RouteData.Values["controller"].ToString(), RouteData.Values["action"].ToString());
            ErrorViewModel errorModel = errorModelManger.GetErrorViewModel(
                Resources.DyntaxaResource.TaxonSplitHeaderText,
                Resources.DyntaxaResource.TaxonSplitHeaderText,
                errorMsg, 
                additionalErrorMsg);
            return View("ErrorInfo", errorModel);
        }

        /// <summary>
        /// GET: Dyntaxa/Taxon/SortChildTaxa/id
        /// Get a sorting view for given taxon
        /// </summary>
        /// <returns>A sort child taxa view.</returns>
        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult SortChildTaxa(string taxonId, string sort)
        {            
            if (taxonId.IsNull())
            {
                taxonId = this.TaxonIdentifier.Id.ToString();
            } 
                
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return RedirectToSearch(taxonId);
            }

            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);
            ITaxon taxon = searchResult.Taxon;
            ViewBag.Taxon = taxon;
            var model = new SortChildTaxaViewModel();                

            model.ScientificName = taxon.ScientificName;
            model.RevisionId = this.RevisionId.Value;
            model.TaxonCategory = taxon.Category.Name;
            model.TaxonId = taxon.Id;
            model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
            model.SortChildTaxaList = new List<SortChildTaxonItem>();
            IUserContext userContext = GetCurrentUser();

            foreach (ITaxonRelation taxonRelation in taxon.GetChildTaxonRelations(userContext, true, false))
            {                    
                var childTaxonItem = new SortChildTaxonItem();                
                childTaxonItem.ChildScientificName = taxonRelation.ChildTaxon.ScientificName.IsNotEmpty() ? taxonRelation.ChildTaxon.ScientificName : "Scientific Name";
                childTaxonItem.ChildTaxonId = taxonRelation.ChildTaxon.Id;
                model.SortChildTaxaList.Add(childTaxonItem);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToLower() == "asc")
                {
                    model.SortChildTaxaList.Sort(new SortChildTaxonItemComparer(true));   
                }
                else if (sort.ToLower() == "desc")
                {
                    model.SortChildTaxaList.Sort(new SortChildTaxonItemComparer(false));
                }
            }
                
            return View(model);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.TaxonRevisionEditor)]
        public ActionResult SortChildTaxa(int taxonId, string newSortOrder)
        {
            IUserContext loggedInUser = GetLoggedInUser();
            ValidateTaxon(loggedInUser, taxonId);
            if (ModelState.IsValid)
            {
                var javascriptSerializer = new JavaScriptSerializer();
                var strs = javascriptSerializer.Deserialize<string[]>(newSortOrder);

                var sortedTaxonIds = new List<int>(strs.Length);
                for (int i = 0; i < strs.Length; i++)
                {
                    sortedTaxonIds.Add(int.Parse(strs[i].Split('_')[1]));
                }

                ITaxonRevision taxonRevision = TaxonRevision;
                
                using (ITransaction transaction = loggedInUser.StartTransaction())
                {
                    CoreData.TaxonManager.UpdateTaxonTreeSortOrder(loggedInUser, sortedTaxonIds, taxonId, taxonRevision);

                    transaction.Commit();
                }

                // Reload tree.
                RedrawTree();
                TaxonIdentifier = TaxonIdTuple.CreateFromId(taxonId);

                return RedirectToAction("SortChildTaxa", new { @taxonId = taxonId });                    
            }
            else
            {
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(loggedInUser, taxonId);
                var model = new SortChildTaxaViewModel();

                model.ScientificName = taxon.ScientificName;
                model.RevisionId = RevisionId.Value;
                model.TaxonCategory = taxon.Category.Name;
                model.TaxonId = taxon.Id;
                model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : "";
                model.SortChildTaxaList = new List<SortChildTaxonItem>();
                IUserContext userContext = GetCurrentUser();

                foreach (ITaxonRelation taxonRelation in taxon.GetChildTaxonRelations(userContext, true, false))
                {
                    var childTaxonItem = new SortChildTaxonItem();
                    childTaxonItem.ChildScientificName = taxonRelation.ChildTaxon.ScientificName.IsNotEmpty() ? taxonRelation.ChildTaxon.ScientificName : "Scientific Name";
                    childTaxonItem.ChildTaxonId = taxonRelation.ChildTaxon.Id;
                    model.SortChildTaxaList.Add(childTaxonItem);
                }

                return View(model);
            }
        }

        public RedirectResult RedirectToGBIF(int taxonId)
        {
            try
            {
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId);
                var linkManager = new LinkManager();
                var url = linkManager.GetUrlToGBIF(taxon.ScientificName);
                return Redirect(url);
            }
            catch (Exception ex)
            {                
                throw new Exception("Could not generate link from GBIF Service", ex);
            }            
        }
        
        public ActionResult ClearCache()
        {
            try
            {                
                IUserContext user = GetCurrentUser();
                if (user.IsWebServiceAdministrator())
                {
                    WebServiceProxy.TaxonService.ClearCache(new TaxonDataSource().GetClientInformation(user));
                    CacheManager.FireRefreshCache(GetCurrentUser());
                    return Content("Cache cleared successfully for Taxon Service and Dyntaxa application.");
                }
                else if (user.IsTaxonRevisionAdministrator() || user.IsTaxonEditor())
                {                    
                    CacheManager.FireRefreshCache(GetCurrentUser());
                    return Content("Cache cleared successfully for Dyntaxa application. Taxon Service cache is not cleared! You need Web service administrator authority for that.");
                }
                else
                {
                    return Content("Clear cache failed! You need to have TaxonRevisionAdministrator permission to clear cache.");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
                //return Content("There was an error when tried to clear cache");
            }            
        }

#if DEBUG

        public ActionResult DevClearCache(string returnUrl)
        {
            IUserContext userContext;
            WebClientInformation clientInformation;

            userContext = GetApplicationUser();
            clientInformation = new TaxonDataSource().GetClientInformation(userContext);
            WebServiceProxy.TaxonService.ClearCache(clientInformation);
            CacheManager.FireRefreshCache(userContext);            
            return Redirect(returnUrl);
        }

        public ActionResult DevSelectCanisLupus(string returnUrl)
        {
            //267320 canis lupus (varg)
            //100024 canis lupus lupus 

            //var model = Session["TaxonTree"] as TaxonTreeViewModel;
            ITaxon rootTaxon = CoreData.TaxonManager.GetTaxon(GetApplicationUser(), 0);
            TaxonTreeViewModel model = new TaxonTreeViewModel(rootTaxon, null, null);

            // Reset selected taxa.
            foreach (TaxonTreeViewItem taxonTreeViewItem in model.GetTreeViewItemEnumerator())
            {
                taxonTreeViewItem.IsActive = false;
            }
            
            ITaxon canisLupusTaxon = CoreData.TaxonManager.GetTaxon(GetApplicationUser(), 267320);
            var allParentTaxonRelations = canisLupusTaxon.GetAllParentTaxonRelations(GetApplicationUser(), null, false, false, true);

            // Expand parents and set the last one to Selected.
            for (int i = 0; i < allParentTaxonRelations.Count; i++)
            {
                var parentTaxonRelation = allParentTaxonRelations[i];
                TaxonTreeViewItem parenTaxonTreeViewItem = model.GetTreeViewItemByTaxonId(parentTaxonRelation.ChildTaxon.Id);
                if (parenTaxonTreeViewItem.HasAjaxChildren)
                {
                    parenTaxonTreeViewItem.IsExpanded = true;
                    parenTaxonTreeViewItem.Children = model.LoadChildren(parenTaxonTreeViewItem, false);
                }
                if (i == allParentTaxonRelations.Count - 1) // last element
                {
                    parenTaxonTreeViewItem.IsActive = true;
                }
            }

            Session["TaxonTree"] = model;
            return Redirect(returnUrl);
        }

        public ActionResult DevRedrawTree(string returnUrl, int? newRootTaxonId, int? selectedTaxonId)
        {
            if (newRootTaxonId != null && selectedTaxonId != null)
            {
                this.RedrawTree(newRootTaxonId.Value, selectedTaxonId.Value);
            }
            this.RedrawTree();            
            return Redirect(returnUrl);
        }

        public ActionResult DevSetRevision(string returnUrl)
        {
            //return RedirectToAction("List", "TaxonName", new {taxonId=4000107, revisionId = 1});
            this.RevisionId = 1;
            ITaxonRevision taxonRevision = CoreData.TaxonManager.GetTaxonRevision(GetCurrentUser(), 1);
            this.TaxonRevision = taxonRevision;
            this.RevisionTaxonId = taxonRevision.RootTaxon.Id;
            this.RevisionTaxonCategorySortOrder = taxonRevision.RootTaxon.Category.SortOrder;
            return Redirect(returnUrl);
        }

#endif

        ///// <summary>
        ///// GET: Dyntaxa/Taxon/Photos/id
        ///// Get a read view with selected photos from the Swedish species gateway.
        ///// </summary>
        ///// <param name="id">Taxon id or name.</param>
        ///// <returns>A taxon information view.</returns>
        //public ActionResult Photos(string id)
        //{
        //    ReadHtmlContentModel model = null;
        //    ReadTaxonInformationModel taxonModel = new ReadTaxonInformationModel(id);

        //    if (taxonModel.ShowTaxonInformation)
        //    {
        //        model = new ReadHtmlContentModel(taxonModel.UrlToPhotos);
        //    }
        //    return View("ReadContent", model);
        //}

        ///// <summary>
        ///// GET: Dyntaxa/Taxon/ReadFromRedlist/id
        ///// Get a read view that gives description of the taxon concept.
        ///// </summary>
        ///// <param name="id">Taxon id or name.</param>
        ///// <returns>A taxon information view.</returns>
        //public ActionResult Redlist(string id)
        //{
        //    ReadHtmlContentModel model = null;
        //    ReadTaxonInformationModel taxonModel = new ReadTaxonInformationModel(id);
        //    LinkManager manager = new LinkManager(true);
        //    if (taxonModel.ShowTaxonInformation)
        //    {
        //        model = new ReadHtmlContentModel(manager.GetUrlToRedlist(taxonModel.TaxonId));
        //    }
        //    return View("ReadContent", model);
        //}

        ///// <summary>
        ///// GET: Dyntaxa/Taxon/ReadFromGBIF/id
        ///// Get a read view that gives description of the taxon concept.
        ///// </summary>
        ///// <param name="id">Taxon id or name.</param>
        ///// <returns>A taxon information view.</returns>
        //public ActionResult ReadGBIF(string id)
        //{
        //    ReadHtmlContentModel model = null;
        //    ReadTaxonInformationModel taxonModel = new ReadTaxonInformationModel(id);
        //    LinkManager manager = new LinkManager();
        //    if (taxonModel.ShowTaxonInformation)
        //    {
        //        model = new ReadHtmlContentModel(manager.GetUrlToGBIF(taxonModel.ScientificName));
        //    }
        //    return View("ReadContent", model);
        //}

        ///// <summary>
        ///// GET: Dyntaxa/Taxon/ReadFromwikipedia/id
        ///// Get a read view that gives description of the taxon concept.
        ///// </summary>
        ///// <param name="id">Taxon id or name.</param>
        ///// <returns>A taxon information view.</returns>
        //public ActionResult ReadWikipedia(string id)
        //{
        //    ReadHtmlContentModel model = null;
        //    ReadTaxonInformationModel taxonModel = new ReadTaxonInformationModel(id);
        //    LinkManager manager = new LinkManager();
        //    if (taxonModel.ShowTaxonInformation)
        //    {
        //        model = new ReadHtmlContentModel(manager.GetUrlToWikipedia(taxonModel.ScientificName));
        //    }
        //    return View("ReadContent", model);
        //}

        ///// <summary>
        ///// GET: Dyntaxa/Taxon/GoogleSearch/id
        ///// Get a read view that gives description of the taxon concept.
        ///// </summary>
        ///// <param name="id">Taxon id or name.</param>
        ///// <returns>A taxon information view.</returns>
        //public ActionResult GoogleSearch(string id)
        //{
        //    ReadHtmlContentModel model = null;
        //    ReadTaxonInformationModel taxonModel = new ReadTaxonInformationModel(id);
        //    LinkManager manager = new LinkManager();
        //    if (taxonModel.ShowTaxonInformation)
        //    {
        //        model = new ReadHtmlContentModel(manager.GetUrlToGoogleSearchResults(taxonModel.ScientificName));
        //    }
        //    return View("ReadContent", model);
        //}
    }
}
