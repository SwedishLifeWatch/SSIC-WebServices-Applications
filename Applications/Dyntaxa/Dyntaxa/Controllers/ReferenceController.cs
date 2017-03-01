using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Reference;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;
using Dyntaxa.Helpers;
using System.Web.Routing;

namespace Dyntaxa.Controllers
{
    public class ReferenceController : DyntaxaBaseController
    {
        [ChildActionOnly]
        public PartialViewResult GuidObjectInfo(string guid)
        {
            var viewManager = new ReferenceViewManager(GetCurrentUser());
            GuidObjectViewModel model = viewManager.CreateGuidObjectViewModel(guid);
            return PartialView(model);
        }

        public PartialViewResult ListReferences(string guid)
        {
            var viewManager = new ReferenceViewManager(GetCurrentUser());
            var model = viewManager.CreateReferenceInfoViewModel(guid);
            return PartialView(model);
        }

        public PartialViewResult ListSpeciesFactReference(IReference reference)
        {
            var viewManager = new ReferenceViewManager(GetCurrentUser());            
            ReferenceInfoViewModel model = viewManager.CreateReferenceInfoViewModel(reference);
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult List()
        {
            var viewManager = new ReferenceViewManager(CoreData.UserManager.GetCurrentUser());
            var model = viewManager.CreateReferenceListViewModel();
            return View(model);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.EditReference, ChangeCurrentRole = false)]
        public ActionResult Add(string guid, int? taxonId, string returnParameters, string returnController, string returnAction, bool showReferenceApplyMode = false)
        {
            if (guid == null)
            {
                var errorModelManger = new ErrorModelManager(new Exception(), "Reference", "Add");                
                ErrorViewModel errorModel = errorModelManger.GetErrorViewModel("Add reference error", "Add reference error", "No GUID (unique id) is specified.", null);
                return View("ErrorInfo", errorModel);
            }

            //// DyntaxaObjectType objectType = GuidHelper.GetObjectTypeFromGuid(guid);
            
            var viewManager = new ReferenceViewManager(CoreData.UserManager.GetCurrentUser());
            ReferenceAddViewModel model = viewManager.CreateReferenceAddViewModel(guid, showReferenceApplyMode);
            var dicRouteValues = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(returnController) && !string.IsNullOrEmpty(returnAction))
            {
                //if (!string.IsNullOrEmpty(returnParameters))
                //    model.LinkParams = DecodeRouteQueryString(returnParameters);
                model.ReturnAction = returnAction;
                model.ReturnController = returnController;
                model.ReturnParameters = returnParameters;
                
                if (!string.IsNullOrEmpty(returnParameters))
                {
                    dicRouteValues = DecodeRouteQueryString(returnParameters);
                }

                if (!dicRouteValues.ContainsKey("taxonId") && taxonId.HasValue)
                {
                    dicRouteValues.Add("taxonId", taxonId.Value);
                }
            }

            model.TaxonId = taxonId.HasValue ? taxonId.Value : TaxonIdentifier.Id;
            model.RouteValues = dicRouteValues;
            
            return View(model);
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.EditReference, ChangeCurrentRole = false)]
        public ActionResult Add(string guid, string selectedReferences, string returnParameters, string returnController, string returnAction, int? taxonId, ReferenceApplyMode? applyMode)
        {
            var javascriptSerializer = new JavaScriptSerializer();            
            var references = javascriptSerializer.Deserialize<ReferenceViewModel[]>(selectedReferences);
            
            var viewManager = new ReferenceViewManager(GetCurrentUser());
            if (applyMode.HasValue && taxonId.HasValue)
            {
                ITaxon taxon = CoreData.TaxonManager.GetTaxon(GetCurrentUser(), taxonId.Value);
                viewManager.UpdateReferenceRelations(taxon, guid, references, applyMode.Value);
            }
            else
            {
                viewManager.UpdateReferenceRelations(guid, references);
            }
            
            var dic = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(returnParameters))
            {
                dic = DecodeRouteQueryString(returnParameters);
            }

            if (!dic.ContainsKey("taxonId") && taxonId.HasValue)
            {
                dic.Add("taxonId", taxonId.Value);
            }

            return RedirectToAction(returnAction, returnController, dic);
        }

        [HttpGet]
        [DyntaxaAuthorize(Order = RequiredAuthorization.EditReference, ChangeCurrentRole = false)]
        public ActionResult New()
        {
            var viewManager = new ReferenceViewManager(GetCurrentUser());
            var model = viewManager.CreateNewReferenceViewModel();
            return View(model);            
        }

        [HttpPost]
        [DyntaxaAuthorize(Order = RequiredAuthorization.EditReference, ChangeCurrentRole = false)]
        public ActionResult New(CreateNewReferenceViewModel model)
        {            
            if (!ModelState.IsValid)
            {
                return PartialView("New", model);
            }

            var viewManager = new ReferenceViewManager(GetCurrentUser());
            viewManager.CreateNewReference(model.Reference);            
            return Json(new { success = true }); 
        }

        [HttpGet]
        public JsonResult SearchReference(string searchString)
        {
            var viewManager = new ReferenceViewManager(CoreData.UserManager.GetCurrentUser());
            List<ReferenceViewModel> references = viewManager.SearchReference(searchString);
            return Json(references, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReferenceData()
        {
            var viewManager = new ReferenceViewManager(CoreData.UserManager.GetCurrentUser());
            var model = viewManager.CreateReferenceListViewModel();
            
            //var obj = new {iTotalRecords = 2, iTotalDisplayRecords = 2, sEcho = 2, aaData = model.References};
            var obj = new { aaData = model.References }; 
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Info(string guid, int? taxonId)
        {
            if (guid == null)
            {
                var errorModelManger = new ErrorModelManager(new Exception(), "Reference", "info");
                ErrorViewModel errorModel = errorModelManger.GetErrorViewModel("Reference info error", "Reference info error", "No GUID (unique id) is specified.", null);
                return View("ErrorInfo", errorModel);
            }

            var viewManager = new ReferenceViewManager(GetCurrentUser());
            var model = viewManager.CreateReferenceInfoViewModel(guid);
            model.TaxonId = taxonId.HasValue ? taxonId.Value : TaxonIdentifier.Id;
            return View(model);
        }
    }
}
