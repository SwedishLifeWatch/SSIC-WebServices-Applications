using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.General;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Debug;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using Newtonsoft.Json;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller has Actions that is good to have when debugging
    /// the application. All Actions is only available when compiling in
    /// DEBUG mode
    /// </summary>
    public class DebugController : BaseController
    {
#if DEBUG

        public ActionResult TaxonTree()
        {
            const int CarnivoraTaxonId = 3000303;
            TaxonTreeViewManager viewManager = new TaxonTreeViewManager(GetCurrentUser(), SessionHandler.MySettings);

            List<ITaxon> allSpecies = viewManager.GetAllSpecies(CarnivoraTaxonId);
            viewManager.GetTaxonTree(CarnivoraTaxonId);

            return Content("Hej");
        }

        public ActionResult ResetSession(string returnUrl)
        {
            Session.Clear();
            Session.Abandon();
            return Redirect(returnUrl);
        }

        public ActionResult CoordinateConverter()
        {
            return View();
        }

        public ActionResult AddDefaultSpatialFilter(string returnUrl)
        {
            string strPolygon = "{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"properties\":{},\"geometry\":{\"type\":\"Polygon\",\"coordinates\":[[[1668773.2012895,8173729.9392108],[1668773.2012895,8560195.5541668],[2035670.9370073,8560195.5541668],[2035670.9370073,8173729.9392108],[1668773.2012895,8173729.9392108]]]}}]}";
            FeatureCollection featureCollection = JsonConvert.DeserializeObject(strPolygon, typeof(FeatureCollection)) as FeatureCollection;
            var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateSpatialFilter(featureCollection);
            return Redirect(returnUrl);
        }

        public PartialViewResult DebugView()
        {            
            return PartialView();
        }

        public PartialViewResult SessionVariables()
        {
            DebugSessionVariablesViewModel model = DebugSessionVariablesViewModel.Create();
            return PartialView(model);
        }

        public PartialViewResult DebugActions()
        {
            return PartialView();
        }

        public RedirectResult SetProjectParameterFilter(string returnUrl)
        {            
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>() { 106752 };
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;

            SessionHandler.MySettings.Filter.Temporal.ObservationDate.StartDate = new DateTime(2013, 10, 6);
            SessionHandler.MySettings.Filter.Temporal.ObservationDate.EndDate = new DateTime(2013, 10, 6);
            SessionHandler.MySettings.Filter.Temporal.ObservationDate.UseSetting = true;
            SessionHandler.MySettings.Filter.Temporal.IsActive = true;

            return Redirect(returnUrl);
        }

        public RedirectResult AddDefaultTaxa(string returnUrl)
        {
            //SessionHandler.MySettings.Filter.Taxa.TaxonIds = new List<int>() {1,2,3,4};            
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>() { 5, 7, 8, 11, 14, 16 };
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;

            return Redirect(returnUrl);
        }

        public RedirectResult AddDefaultTaxa2(string returnUrl)
        {
            //SessionHandler.MySettings.Filter.Taxa.TaxonIds = new List<int>() {1,2,3,4};            
            SessionHandler.MySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>() { 7, 8, 11, 14, 16, 17, 18, 19, 20, 25, 21, 22, 24 };
            SessionHandler.MySettings.Filter.Taxa.IsActive = true;

            return Redirect(returnUrl);
        }

        public const string SettingsName = "MySettings";

        public RedirectResult SaveCurrentMySettings(string returnUrl)
        {
            MySettings mySettings = SessionHandler.MySettings;
            if (mySettings.IsNotNull())
            {
                MySettingsManager.SaveToDisk(SettingsName, mySettings);    
            }            
            return Redirect(returnUrl);
        }

        public ActionResult TaxonInfoFromDyntaxa(int id)
        {
            return View(id);
            //return PartialView("TaxonInfoFromDyntaxa", id);
        }
        
        public RedirectResult LoadMySettings(string returnUrl)
        {
            try
            {            
                if (MySettingsManager.DoesNameExistOnDisk(SettingsName))
                {
                    MySettings mySettings = MySettingsManager.LoadFromDisk(SettingsName);
                    SessionHandler.MySettings = mySettings;
                }
            }
            catch (Exception)
            {
            }

            return Redirect(returnUrl);
        }

        public ActionResult SpeciesObservationSearchCriteriaAPI()
        {
            return View();
        }
#endif

    }
}
