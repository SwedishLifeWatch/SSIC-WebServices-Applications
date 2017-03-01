using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Details;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains actions for handling details
    /// </summary>
    public class DetailsController : BaseController
    {
        public ActionResult ObservationDetail(string id)
        {
            ViewBag.ObservationId = id;
            return View("ObservationDetail");
        }        

        public PartialViewResult ObservationDetailPartial(string id, bool? dialog)
        {
            ObservationDetailViewModel model = null;            
            ObservationDetailViewManager manager = new ObservationDetailViewManager(GetCurrentUser(), SessionHandler.MySettings);
            model = manager.CreateObservationDetailsViewModel(id, dialog.GetValueOrDefault(false));            
            return PartialView(model);
        }

        public PartialViewResult TaxonSummary(int id)
        {
            TaxonSummaryViewModel taxonSummaryViewModel = TaxonSummaryViewModel.Create(id, GetCurrentUser());
            return PartialView(taxonSummaryViewModel);
        }

        public PartialViewResult TaxonSummaryDialog(int id)
        {
            TaxonSummaryViewModel taxonSummaryViewModel = TaxonSummaryViewModel.Create(id, GetCurrentUser());
            return PartialView(taxonSummaryViewModel);
        }
    }
}
