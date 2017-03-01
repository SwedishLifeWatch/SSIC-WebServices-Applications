using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Map;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Report;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;
using Newtonsoft.Json;
using System.Threading;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller contains Actions that is used to set various format settings affecting output results.
    /// </summary>
    public class FormatController : BaseController
    {
        /// <summary>
        /// Renders a format settings overview page
        /// </summary>
        /// <returns></returns>
        [IndexedBySearchRobots]
        public ActionResult Index()
        {
            string localeIsoCode = Thread.CurrentThread.CurrentCulture.Name;
            AboutViewModel model = AboutManager.GetAboutPresentationFormatViewModel(localeIsoCode);             
            return View(model);
        }

        /// <summary>
        /// Renders map format settings page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Map()
        {
            var viewManager = new MapSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            PresentationMapViewModel model = viewManager.CreatePresentationMapViewModel();
            return View(model);
        }

        /// <summary>
        /// Saves the map format settings.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Redirection to the Map page.</returns>
        [HttpPost]
        public ActionResult Map(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            PresentationMapViewModel model = javascriptSerializer.Deserialize<PresentationMapViewModel>(data);
            var viewManager = new MapSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateMapSettings(model);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationMapSettingsUpdated));
            return RedirectToAction("Map");
        }

        /// <summary>
        /// Updates the coordinate system.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system identifier.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>A redirection to <paramref name="returnUrl"/>.</returns>
        public RedirectResult UpdateCoordinateSystem(CoordinateSystemId coordinateSystemId, string returnUrl)
        {
            var viewManager = new MapSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateCurrentCoordinateSystem(coordinateSystemId);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationMapSettingsUpdated));
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Renders tabele format settings page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Table()
        {
            // Todo: Delete? Redirects to SpeciesObservationTable action instead
            return RedirectToAction("SpeciesObservationTable");
            //var viewManager = new TableSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            //PresentationTableViewModel model = viewManager.CreatePresentationTableViewModel();
            //return View(model);
        }

        [HttpPost]
        public ActionResult Table(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            PresentationTableViewModel tableSettingsModel = javascriptSerializer.Deserialize<PresentationTableViewModel>(data);
            var viewManager = new TableSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateTableSettings(tableSettingsModel);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationTableSettingsUpdated));
            return RedirectToAction("Table");
        }

        public RedirectResult ResetTableSettings(string returnUrl)
        {
            SessionHandler.MySettings.Presentation.Table.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationTableSettingsReset));
            return Redirect(returnUrl);
        }

        [HttpGet]
        public ActionResult FileFormat()
        {
            IUserContext user = GetCurrentUser();
            var viewManager = new FileFormatSettingsViewManager(user, SessionHandler.MySettings);
            FileFormatViewModel model = viewManager.CreateFileFormatViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult FileFormat(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            FileFormatViewModel model = javascriptSerializer.Deserialize<FileFormatViewModel>(data);
            var viewManager = new FileFormatSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateFileFormatSettings(model);
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationFileFormatSettingsUpdated));
            return RedirectToAction("FileFormat");
        }

        public RedirectResult ResetFileFormatSettings(string returnUrl)
        {
            SessionHandler.MySettings.Presentation.FileFormat.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationFileFormatReset));
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Renders report settings page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Report()
        {
            var viewManager = new ReportSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            PresentationReportViewModel model = viewManager.CreatePresentationReportViewModel();
            return View(model);
        }

        /// <summary>
        /// Renders a dialog where the user can create or edit a custom table type.
        /// </summary>
        /// <param name="id">The table id if in edit mode.</param>
        /// <returns>A partial view that will be rendered.</returns>
        public PartialViewResult CustomSpeciesObservationTableDialog(int? id)
        {
            var viewManager = new SpeciesObservationFieldDescriptionViewManager(GetCurrentUser(), SessionHandler.MySettings);
            SpeciesObservationTableTypeViewModel tableType = null;
            if (id.HasValue)
            {
                tableType = viewManager.GetUserDefinedTable(id.Value);
            }
            
            return PartialView(tableType);            
        }

        /// <summary>
        /// Creates a new user defined table type.
        /// </summary>
        /// <param name="data">Table data in JSON format.</param>
        /// <returns>Success if creation goes well; otherwise Error message.</returns>
        public JsonNetResult CreateNewCustomSpeciesObservationTableTypeByAjax(string data)
        {
            JsonModel jsonModel = JsonModel.CreateSuccess("");
            try
            {
                SpeciesObservationTableTypeViewModel tableType = JsonConvert.DeserializeObject<SpeciesObservationTableTypeViewModel>(data);
                var viewManager = new SpeciesObservationFieldDescriptionViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.CreateNewSpeciesObservationTableType(tableType);
                SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationTableCreatedUserDefinedTable));
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Updates a user defined table.
        /// </summary>
        /// <param name="id">The user defined table id.</param>
        /// <param name="data">The table data in JSON format.</param>
        /// <returns>Success if update goes well; otherwise Error message.</returns>
        public JsonNetResult EditUserDefinedTableTypeByAjax(int id, string data)
        {
            JsonModel jsonModel = JsonModel.CreateSuccess("");
            try
            {
                SpeciesObservationTableTypeViewModel tableType = JsonConvert.DeserializeObject<SpeciesObservationTableTypeViewModel>(data);
                var viewManager = new SpeciesObservationFieldDescriptionViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.EditUserDefinedTableType(id, tableType);
                SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationTableEditedUserDefinedTable));
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Deletes a user defined table type.
        /// </summary>
        /// <param name="id">The Id of the table to delete.</param>
        /// <returns>Success if deletion goes well; otherwise Error message.</returns>
        public JsonNetResult DeleteUserDefinedTableTypeByAjax(int id)
        {
            JsonModel jsonModel = JsonModel.CreateSuccess("");
            try
            {                
                var viewManager = new SpeciesObservationFieldDescriptionViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.DeleteUserDefinedTableType(id);
                SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationTableDeletedUserDefinedTable));
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Renders species observation table settings.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SpeciesObservationTable()
        {
            //SubmenuGroup = SubmenuGroupManager.PresentationTableSubmenu;
            var viewManager = new SpeciesObservationTableSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var model = viewManager.CreateSpeciesObservationTableSettingsViewModel();
            return View("SpeciesObservationTable", model);
        }

        [HttpPost]
        public ActionResult SpeciesObservationTable(string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            SpeciesObservationTableSettingsViewModel tableSettingsModel = javascriptSerializer.Deserialize<SpeciesObservationTableSettingsViewModel>(data);
            var viewManager = new SpeciesObservationTableSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateTableSettings(tableSettingsModel);            

            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationSpeciesObservationTableUpdated));
            return RedirectToAction("SpeciesObservationTable");            
        }

        public RedirectResult ResetSpeciesObservationTableSettings(string returnUrl)
        {
            SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationSpeciesObservationTableReset));
            return Redirect(returnUrl);
        }
        
        public ActionResult Diagram(int id)
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Diagram(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }
        
        public JsonNetResult GetTableFields(int tableId, bool useUserDefinedTable)
        {
            JsonModel jsonModel;
            try
            {                
                var viewManager = new SpeciesObservationFieldDescriptionViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<TableFieldDescriptionViewModel> tableFields = viewManager.GetTableFields(tableId, useUserDefinedTable);
                jsonModel = JsonModel.Create(tableFields);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Returns all table fields that can be selected in a custom table.
        /// </summary>
        /// <returns>All table fields that can be selected in a custom table.</returns>
        public JsonNetResult GetAllSelectableTableFields()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new SpeciesObservationFieldDescriptionViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<TableFieldDescriptionViewModel> tableFields = viewManager.GetAllSelectableTableFields();
                jsonModel = JsonModel.Create(tableFields);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        public PartialViewResult TableFieldDescription(int fieldId)
        {
            var viewManager = new SpeciesObservationFieldDescriptionViewManager(GetCurrentUser(), SessionHandler.MySettings);
            TableFieldDescriptionViewModel model = viewManager.GetTableFieldDescription(fieldId);            
            return PartialView(model);
        }

        public RedirectResult ResetMapSettings(string returnUrl)
        {
            SessionHandler.MySettings.Presentation.Map.ResetSettings();
            SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId = (int)CoordinateSystemId.SWEREF99_TM;
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.PresentationMapSettingsReset));
            return Redirect(returnUrl);
        }
    }
}
