using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using System.Threading;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace AnalysisPortal.Controllers
{
    public class CalculationController : BaseController
    {        
        // Actions to be added
        //---------------------        
         //TimeSeries
        //Repeat

        [IndexedBySearchRobots]
        public ActionResult Index()
        {
            string localeIsoCode = Thread.CurrentThread.CurrentCulture.Name;
            AboutViewModel model = AboutManager.GetAboutCalculationsViewModel(localeIsoCode);             
            return View(model);
        }

        /// <summary>
        /// Renders the GridStatistics view.
        /// </summary>
        /// <returns> View model. </returns>
        [HttpGet]
        public ActionResult GridStatistics()
        {
            //CurrentButtonGroupIdentifier = ButtonGroupIdentifier.Calculation;
            var viewManager = new GridStatisticsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            GridStatisticsViewModel model = viewManager.CreateGridStatisticsViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult GridStatistics(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            GridStatisticsViewModel gridStatistics = javascriptSerializer.Deserialize<GridStatisticsViewModel>(data);
            var viewManager = new GridStatisticsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateGridStatistics(gridStatistics);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.GridStatisticsUpdated));
            return RedirectToAction("GridStatistics");
        }

        /// <summary>
        /// Renders the SummaryStatistics view.
        /// </summary>
        /// <returns> View model. </returns>
        [HttpGet]
        public ActionResult SummaryStatistics()
        {
            var viewManager = new SummaryStatisticsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            SummaryStatisticsViewModel model = viewManager.CreateViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SummaryStatistics(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            SummaryStatisticsViewModel summaryStatistics = javascriptSerializer.Deserialize<SummaryStatisticsViewModel>(data);
            var viewManager = new SummaryStatisticsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateSummaryStatistics(summaryStatistics);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.SummaryStatisticsUpdated));
            return RedirectToAction("SummaryStatistics");
        }

        public RedirectResult ResetSummaryStatistics(string returnUrl)
        {
            SessionHandler.MySettings.Calculation.SummaryStatistics.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.SummaryStatisticsReset));
            return Redirect(returnUrl);
        }

        public RedirectResult ResetGridStatistics(string returnUrl)
        {
            SessionHandler.MySettings.Calculation.GridStatistics.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.GridStatisticsReset));
            return Redirect(returnUrl);
        }

        [HttpGet]
        public ActionResult TimeSeries()
        {
            TimeSeriesSettingsViewManager viewManager = new TimeSeriesSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            TimeSeriesSettingsViewModel model = viewManager.CreateViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult TimeSeries(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            TimeSeriesSettingsViewModel timeSeriesSettings = javascriptSerializer.Deserialize<TimeSeriesSettingsViewModel>(data);
            var viewManager = new TimeSeriesSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateTimeSeriesSettings(timeSeriesSettings);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.TimeSeriesSettingsUpdated));
            return RedirectToAction("TimeSeries");
        }

        public RedirectResult ResetTimeSeriesSettings(string returnUrl)
        {
            SessionHandler.MySettings.Calculation.TimeSeries.ResetSettings();            
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.TimeSeriesSettingsReset));
            return Redirect(returnUrl);
        }
    }
}
