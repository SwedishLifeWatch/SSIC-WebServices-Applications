using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Converters;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Report;
using Newtonsoft.Json;
using Resources;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains Actions that is used to handle MySettings.
    /// </summary>
    public class MySettingsController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySettingsController"/> class.
        /// </summary>
        public MySettingsController()
        {
        }

        // Called by test
        public MySettingsController(IUserDataSource userDataSourceRepository, ISessionHelper session)
            : base(userDataSourceRepository, session)
        {
        }
        
        /// <summary>
        /// Renders a partial view that shows a short summary of
        /// the current settings.
        /// </summary>
        /// <returns></returns>
        ////[ChildActionOnly]
        public PartialViewResult MySettingsSummary()
        {
            try
            {
                var model = new MySettingsSummaryViewModel();
                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);                
            }            
        }

        /// <summary>
        /// Renders Load, Save & Restore buttons.
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult MySettingsSummaryButtonGroup()
        {
            try
            {
                var viewManager = new MySettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
                var model = viewManager.CreateMySettingsButtonGroupViewModel();
                return PartialView(model);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);                
            }
        }

        // public RedirectResult ChangeButtonCheckState(int identifier, string returnUrl)
        // {
        //    StateButtonModel buttonModel = StateButtonManager.GetButton(identifier);
        //    buttonModel.IsChecked = !buttonModel.IsChecked;          
        //    return Redirect(returnUrl);
        // }

        /// <summary>
        /// Changes a button check state by using ajax.
        /// </summary>
        /// <param name="identifier">The button identifier.</param>
        /// <param name="checkValue">Set the button check state to this value.</param>
        /// <returns>A JSON object that contains which button was modified and its new check state value.</returns>
        [HttpPost]
        public JsonNetResult ChangeButtonCheckStateAjax(int identifier, bool checkValue)
        {
            JsonModel jsonModel;
            try
            {
                StateButtonModel buttonModel = StateButtonManager.GetButton(identifier);
                buttonModel.IsChecked = checkValue;
                jsonModel = JsonModel.CreateFromObject(new { Identifier = identifier, IsChecked = buttonModel.IsChecked });
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Active/deactivate setting
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonNetResult ActivateDeactivateSetting(MySettingsSummaryItemIdentifier identifier, bool checkValue)
        {                        
            JsonModel jsonModel;
            try
            {
                var model = new MySettingsSummaryViewModel();
                var setting = (from sg in model.SettingGroups from i in sg.Items where i.Identifier == identifier select i).FirstOrDefault();
                setting.IsActive = checkValue;
                
                jsonModel = JsonModel.CreateFromObject(true);                
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }
        
        /// <summary>
        /// Update the summary statistics setting
        /// </summary>
        /// <param name="data"></param>
        /// <returns>JsonNetResult</returns>
        public JsonNetResult UpdateSummaryStatisticsSetting(string data)
        {
            JsonModel jsonModel;
            try
            {                
                var javascriptSerializer = new JavaScriptSerializer();
                SummaryStatisticsViewModel summaryStatistics = javascriptSerializer.Deserialize<SummaryStatisticsViewModel>(data);
                var viewManager = new SummaryStatisticsViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.UpdateSummaryStatistics(summaryStatistics);
                jsonModel = JsonModel.CreateSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        public RedirectResult LoadAutoSavedSettings(string returnUrl, string userName)
        {
            if (SessionHandler.UserContext != null)
            {
                SessionHandler.MySettings = MySettingsManager.LoadLastSettings(SessionHandler.UserContext);
                SessionHandler.UserMessages.Add(new UserMessage(Resource.SharedAutosavedSettingsLoaded, UserMessageType.Info));
            }
            else if (userName != null)
            {                
                HttpCookie httpCookie = Request.Cookies["ASP.NET_SessionId"];
                if (httpCookie != null)
                {
                    string sessionId = httpCookie.Value;
                    if (LastUserSessionIdManager.IsLastUserSessionIdOk(userName, sessionId))
                    {
                        SessionHandler.MySettings = MySettingsManager.LoadLastSettings(userName);
                    }
                }                
            }

            return Redirect(returnUrl.ToLower());
        }

        public JsonNetResult UpdatePresentationReportSettings(string data)
        {
            JsonModel jsonModel;
            try
            {                
                var javascriptSerializer = new JavaScriptSerializer();
                PresentationReportViewModel reportSettingsModel = javascriptSerializer.Deserialize<PresentationReportViewModel>(data);
                var viewManager = new ReportSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.UpdatePresentationReportSetting(reportSettingsModel);
                jsonModel = JsonModel.CreateSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }
        
        /// <summary>
        /// Returns all Regions stored in MySettings.
        /// </summary>
        /// <returns></returns>
        public JsonNetResult GetAllRegions()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<RegionViewModel> regions = viewManager.GetAllRegions();
                jsonModel = JsonModel.Create(regions);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }
        
        public JsonNetResult AddRegion(string data)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                var javascriptSerializer = new JavaScriptSerializer();
                RegionViewModel regionViewModel = javascriptSerializer.Deserialize<RegionViewModel>(data);
                viewManager.AddRegion(regionViewModel.Id);
                jsonModel = JsonModel.CreateSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        public JsonNetResult AddRegionById(int id)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.AddRegion(id);
                jsonModel = JsonModel.CreateSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }
        
        public JsonNetResult RemoveRegion(string data)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new SpatialFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);                
                var javascriptSerializer = new JavaScriptSerializer();
                RegionViewModel regionViewModel = javascriptSerializer.Deserialize<RegionViewModel>(data);
                viewManager.RemoveRegion(regionViewModel.Id);
                jsonModel = JsonModel.CreateSuccess(string.Empty);                
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Resets the current MySettings object.
        /// </summary>        
        /// <returns></returns>
        public RedirectToRouteResult ResetMySettings()
        {
            SessionHandler.MySettings = new MySettings();
            SessionHandler.Results.Clear();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.MySettingsReset));
            return RedirectToAction("Index", "Home");            
        }

        /// <summary>
        /// Loads a MySettings object from disk and uses this
        /// as the current settings.
        /// </summary>        
        /// <returns></returns>
        public RedirectToRouteResult LoadMySettings()
        {
            try
            {
                IUserContext userContext = GetCurrentUser();
                if (MySettingsManager.DoesNameExistOnDisk(userContext, MySettingsManager.SettingsName))
                {
                    MySettings mySettings = MySettingsManager.LoadFromDisk(userContext, MySettingsManager.SettingsName);
                    SessionHandler.MySettings = mySettings;
                    RemoveCookie("MapState");
                }                
            }
            catch (Exception)
            {                                
            }

            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.MySettingsLoaded));
            return RedirectToAction("Index", "Home");            
        }

        #region GetSummaryItemSettings

        /// <summary>
        /// Gets a summary settings item as HTML.
        /// </summary>
        /// <param name="identifier">The summary setting identifier.</param>
        /// <returns></returns>
        public PartialViewResult GetSummaryItemSettings(int identifier)
        {
            try
            {       
                var model = MySettingsSummaryItemManager.GetItem((MySettingsSummaryItemIdentifier)identifier);
                if (model.GetType().IsSubclassOf(typeof(MySettingsListSummaryItem)))
                {                    
                    var mySettingsListSummaryItem = (MySettingsListSummaryItem)model;
                    return PartialView("List", mySettingsListSummaryItem.GetSummaryList());
                }

                switch (model.Identifier)
                {
                    case MySettingsSummaryItemIdentifier.FilterTaxa:
                        return TaxaSummary();
                    case MySettingsSummaryItemIdentifier.DataProviders:
                        return DataProvidersSummary();
                    case MySettingsSummaryItemIdentifier.DataMapLayers:
                        return MapLayersSummary();
                    case MySettingsSummaryItemIdentifier.FilterPolygon:
                        return SpatialPolygonsSummary();
                    case MySettingsSummaryItemIdentifier.FilterLocality:
                        return LocalitySummary();
                    case MySettingsSummaryItemIdentifier.FilterRegion:
                        return RegionSummary();
                    case MySettingsSummaryItemIdentifier.FilterOccurrence:
                        return OccurrenceSummary();
                    case MySettingsSummaryItemIdentifier.FilterTemporal:
                        return TemporalSummary();
                    case MySettingsSummaryItemIdentifier.FilterAccuracy:
                        return AccuracySummary();
                    case MySettingsSummaryItemIdentifier.FilterField:
                        return FieldSummary();
                    case MySettingsSummaryItemIdentifier.CalculationGridStatistics:
                        return GridStatisticsSummary();
                    case MySettingsSummaryItemIdentifier.CalculationSummaryStatistics:
                        return SummaryStatisticsSummary();
                    case MySettingsSummaryItemIdentifier.CalculationTimeSeries:
                        return TimeSeriesSummary();
                    case MySettingsSummaryItemIdentifier.PresentationMap:
                        return PresentationMapSummary();
                    case MySettingsSummaryItemIdentifier.PresentationTable:
                        return TableSummary();
                    case MySettingsSummaryItemIdentifier.PresentationReport:
                        return ReportSummary();
                    case MySettingsSummaryItemIdentifier.PresentationFileFormat:
                        return FileFormatSummary();
                    case MySettingsSummaryItemIdentifier.DataEnvironmentalData:
                        return EnvironmentalDataSummary();
                    default:
                        return PartialView("~/Views/Shared/NotImplemented_Partial.cshtml");
                }
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/Empty_Partial.cshtml");
            }
        }

        /// <summary>
        /// Gets the taxa summary.
        /// </summary>        
        public PartialViewResult TaxaSummary()
        {
            try
            {
                var model = (TaxaSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterTaxa);
                var taxonList = model.GetSettingsSummaryModel();
                return PartialView("TaxaSummary", taxonList);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }  
        }

        /// <summary>
        /// Gets the data providers summary.
        /// </summary>        
        /// <returns>
        /// The <see cref="PartialViewResult"/>.
        /// </returns>
        public PartialViewResult DataProvidersSummary()
        {
            try
            {
                var model = (DataProvidersSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.DataProviders);
                List<DataProviderViewModel> dataProviders = model.GetSettingsSummaryModel();
                return PartialView("List", model.GetSummaryList());
                // return PartialView("DataProvidersSummary", dataProviders);              
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }  
        }

        public PartialViewResult PresentationMapSummary()
        {
            try
            {
                var model = (MapSettingSummary) MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationMap);                
                return PartialView("List", model.GetSettingsSummary());                
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }  
        }

        /// <summary>
        /// Gets the map layers summary.
        /// </summary>        
        public PartialViewResult MapLayersSummary()
        {
            try
            {
                MapLayersSettingSummary model = (MapLayersSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.DataMapLayers);
                List<string> mapLayers = model.GetSummaryList();
                return PartialView("MapLayersSummary", mapLayers);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult ReportSummary()
        {
            try
            {
                ReportSettingsViewManager viewManager = new ReportSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
                PresentationReportViewModel model = viewManager.CreatePresentationReportViewModel();
                return PartialView("ReportSummary", model);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult FileFormatSummary()
        {
            try
            {
                FileFormatSettingsViewManager viewManager = new FileFormatSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
                FileFormatViewModel model = viewManager.CreateFileFormatViewModel();
                return PartialView("FileFormatSummary", model);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult TableSummary()
        {
            try
            {
                TableSettingsViewManager viewManager = new TableSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
                PresentationTableViewModel model = viewManager.CreatePresentationTableViewModel();                
                return PartialView("TableSummary", model);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult GridStatisticsSummary()
        {
            try
            {
                var model = (GridStatisticsSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationGridStatistics);
                var gridStatisticsModel = model.GetSettingsSummaryModel(GetCurrentUser());
                return PartialView("GridStatisticsSummary", gridStatisticsModel);                
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult TimeSeriesSummary()
        {
            try
            {
                var model = (TimeSeriesSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationTimeSeries);
                var timeSeriesModel = model.GetSettingsSummaryModel(GetCurrentUser());
                return PartialView("TimeSeriesSummary", timeSeriesModel);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult TemporalSummary()
        {
            try
            {
                var viewManager = new TemporalFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<string> temporalSettingsSummary = viewManager.GetTemporalSettingsSummary();
                return PartialView("List", temporalSettingsSummary);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult AccuracySummary()
        {
            try
            {
                var viewManager = new AccuracyFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<string> accuracySettingsSummary = viewManager.GetAccuracySettingsSummary();
                return PartialView("List", accuracySettingsSummary);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        /// <summary>
        /// Gets the field summary.
        /// </summary>        
        public PartialViewResult FieldSummary()
        {
            try
            {
                var model = (FieldSettingSummary) MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterField);
                var fieldList = model.GetSettingsSummaryModel();
                return PartialView("List", fieldList);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult OccurrenceSummary()
        {
            try
            {                
                var viewManager = new OccurrenceFilterViewManager(GetCurrentUser(), SessionHandler.MySettings);
                var occurrenceSettingsSummary = viewManager.GetOccurrenceSettingsSummary();
                return PartialView("List", occurrenceSettingsSummary);

                // return PartialView("OccurrenceSummary", occurrenceModel);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        /// <summary>
        /// Gets the region summary.
        /// </summary>        
        public PartialViewResult RegionSummary()
        {
            try
            {
                var model = (RegionSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterRegion);
                var regions = model.GetSettingsSummaryModel();
                var strings = regions.Select(region => region.Name).ToList();                
                return PartialView("List", strings);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        public PartialViewResult EnvironmentalDataSummary()
        {
            try
            {
                var wfsViewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                var model = wfsViewManager.CreateWfsLayersList();
                return PartialView("EnvironmentalDataSummary", model);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        /// <summary>
        /// Gets the region summary.
        /// </summary>
        /// <returns>A partial view.</returns>
        public PartialViewResult LocalitySummary()
        {
            try
            {
                var model = (LocalitySettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterLocality);
                var summaryModel = model.GetSettingsSummaryModel();
                var strings = new List<string>();
                strings.Add(string.Format("{0}: \"{1}\"", Resource.FilterLocalitySearchString, summaryModel.LocalityName));                
                strings.Add(string.Format("{0}: \"{1}\"", Resource.FilterLocalitySearchMethod, summaryModel.CompareOperator.ToResourceName()));                
                return PartialView("List", strings);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        /// <summary>
        /// Gets the polygon summary.
        /// </summary>        
        public PartialViewResult SpatialPolygonsSummary()
        {
            try
            {                
                List<DataPolygon> polygons = SessionHandler.MySettings.Filter.Spatial.Polygons.ToList();                
                string strJson = string.Empty;
                if (polygons.Count > 0)
                {
                    var featureCollection = GeoJSONConverter.ConvertToGeoJSONFeatureCollection(polygons);
                    strJson = JsonConvert.SerializeObject(featureCollection);
                }
                return PartialView("SpatialPolygonsSummary", strJson);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        /// <summary>
        /// Returns a page that contains the basic layout and a map with the spatial filter polygons.
        /// This page can be used when we want to print this map to a png file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public ActionResult SpatialPolygonsSummaryPrintable(string filename)
        {
            ObservableCollection<DataPolygon> polygons = (ObservableCollection<DataPolygon>)HttpRuntime.Cache[filename];
            return View("SpatialPolygonsSummaryPrintable", polygons);
        }

        /// <summary>
        /// Prints a the spatial filter polygons to a png file.
        /// This function uses PhantomJs http://phantomjs.org/
        /// which is a headless WebKit with JavaScript API that can be used
        /// to save images of a web page.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        [ExcludeFilter(typeof(NoCacheFilterAttribute), Order = 0)]
        [OutputCache(Duration = 60 * 20, VaryByParam = "filename", Order = 1)]
        public FileStreamResult SpatialPolygonsSummaryImage(string filename)
        {
            Process process = null;
            try
            {
                string filePath = Path.Combine(Server.MapPath("~/Temp/Images/"), filename);
                
                // temporary save Polygons in cache for 30 seconds
                HttpRuntime.Cache.Insert(filename, ObjectCopier.Clone(SessionHandler.MySettings.Filter.Spatial.Polygons), null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration);
                
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                process = CreatePhantomJsProcess(filePath, filename);         
                process.Start();

                // wait max 20 seconds                
                if (process.WaitForExit(20 * 1000))
                {                    
                    byte[] fileContent = System.IO.File.ReadAllBytes(filePath);
                    System.IO.File.Delete(filePath);
                    MemoryStream ms = new MemoryStream(fileContent);
                    return new FileStreamResult(ms, "image/png");     
                }
                // timeout
                else 
                {                    
                    process.Kill();
                    string errorFilePath = Server.MapPath("~/Content/images/error.png");
                    byte[] fileContent = System.IO.File.ReadAllBytes(errorFilePath);                    
                    MemoryStream ms = new MemoryStream(fileContent);
                    return new FileStreamResult(ms, "image/png");     
                }
            }
            catch (Exception)
            {                
                if (process != null && !process.HasExited)
                {
                    process.Kill();
                }
                string errorFilePath = Server.MapPath("~/Content/images/error.png");
                byte[] fileContent = System.IO.File.ReadAllBytes(errorFilePath);                    
                MemoryStream ms = new MemoryStream(fileContent);
                return new FileStreamResult(ms, "image/png");     
            }
        }

        private Process CreatePhantomJsProcess(string filePath, string filename)
        {            
            string strUrl = this.Request.Url.GetLeftPart(UriPartial.Authority) + "/MySettings/SpatialPolygonsSummaryPrintable/?filename=" + filename;            
            string strExecutable = Server.MapPath("~/bin/phantomjs.exe");
            string flags = "--proxy-type=none --ignore-ssl-errors=true";
            string jsFileArgument = "\"" + Server.MapPath("~/Scripts/Phantomjs/rasterize_element2.js") + "\"";
            string urlArgument = strUrl;
            string filePathArgument = "\"" + filePath + "\"";
            string elementIdArgument = "'#mapSummaryControl'";
            string zoomArgument = "2";
            string arguments = flags + " " + jsFileArgument + " " + urlArgument + " " + filePathArgument + " " + elementIdArgument + " " + zoomArgument;

            ProcessStartInfo processStartInfo;
            processStartInfo = new ProcessStartInfo();
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = arguments;
            processStartInfo.FileName = strExecutable;
            Process process = new Process();
            process.StartInfo = processStartInfo;
            return process;
        }

        public PartialViewResult SummaryStatisticsSummary()
        {
            try
            {
                var model = (SummaryStatisticsSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationSummaryStatistics);
                var summaryStatisticsModel = model.GetSettingsSummaryModel(GetCurrentUser());
                return PartialView("SummaryStatisticsSummary", summaryStatisticsModel);
            }
            catch (Exception ex)
            {
                return PartialView("PartialViewError", ex);
            }
        }

        #endregion
        
        /// <summary>
        /// Saves the current settings.
        /// </summary>        
        public RedirectResult SaveMySettings(string returnUrl)
        {                        
            IUserContext userContext = GetCurrentUser();
            if (!userContext.IsAuthenticated())
            {
                throw new Exception("User is not logged in");
            }

            MySettings mySettings = SessionHandler.MySettings;
            MySettingsManager.SaveToDisk(userContext, MySettingsManager.SettingsName, mySettings);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.MySettingsSaved));
            return Redirect(returnUrl);
        }
    }    
}
