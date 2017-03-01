using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.ActionFilters;
using System.Threading;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.WFS;
using ArtDatabanken.WebApplication.AnalysisPortal.Exceptions;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.MetadataSearch;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Resources;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller has Actions that is used to select data sources.
    /// </summary>
    public class DataController : BaseController
    {
        private string GetLayerName(string fileName)
        {
            return fileName.IndexOf(".", StringComparison.CurrentCultureIgnoreCase) == -1
                ? fileName
                : fileName.Substring(0, fileName.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase - 1));
        }

        private bool AddFileWfsLayer(string fileName, FeatureCollection featureCollection)
        {
            var layerName = GetLayerName(fileName);

            var action = Url.Action("", "Wfs");
            var url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, action);

            var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);

            var layer = viewManager.GetWfsLayers().FirstOrDefault(l => l.Name == layerName);
            if (layer == null)
            {
                var hexColor = SessionHandler.MySettings.DataProvider.MapLayers.CreateMapLayerHexColor();
                var wfsLayer = viewManager.CreateNewWfsFileLayer(layerName, fileName, Server.UrlEncode(fileName), "", url, featureCollection.Features[0].Geometry.Type.ToGeometryType(), hexColor, false);

                viewManager.AddWfsLayer(wfsLayer);

                return true;
            }

            return false;
        }

        // Actions to be added
        //---------------------
        //Index
        //SpeciesObservations
        //Environmental

        /// <summary>
        /// Gets an Overview of the Data sources.
        /// </summary>
        /// <returns></returns>
        [IndexedBySearchRobots]
        public ActionResult Index()
        {
            var localeIsoCode = Thread.CurrentThread.CurrentCulture.Name;
            AboutViewModel model = AboutManager.GetAboutDataProvidersViewModel(localeIsoCode);
            return View(model);
        }

        /// <summary>
        /// Gets a view where the user can select data providers.
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        [IndexedBySearchRobots]
        public ActionResult DataProviders()
        {
            var viewManager = new DataProvidersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            DataProvidersViewModel model = viewManager.CreateDataProvidersViewModel();
            ViewBag.IsSettingsDefault = viewManager.IsDataProvidersDefault();            
            return View(model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult DataProviders(string data)
        {
            var viewManager = new DataProvidersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var javascriptSerializer = new JavaScriptSerializer();
            string[] selectedDataProviders = javascriptSerializer.Deserialize<string[]>(data);
            if (selectedDataProviders == null || selectedDataProviders.Length == 0)
            {
                ModelState.AddModelError("", Resources.Resource.DataProvidersDataProvidersAtLeastOneProviderMustBeSelected);                
                DataProvidersViewModel model = viewManager.CreateDataProvidersViewModel();
                ViewBag.IsSettingsDefault = viewManager.IsDataProvidersDefault();
                return View(model);
            }
            
            viewManager.UpdateDataProviders(selectedDataProviders.ToList());
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.DataProvidersDataProvidersUpdated));
            return RedirectToAction("DataProviders");
        }

        public RedirectResult ResetDataProviders(string returnUrl)
        {            
            SessionHandler.MySettings.DataProvider.DataProviders.ResetSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.DataProvidersDataProvidersReset));
            return Redirect(returnUrl);                 
        }

        /// <summary>
        /// Returns a view where the user can search for metadata (external links).
        /// </summary>
        /// <returns></returns>
        public ActionResult MetadataSearch()
        {
            var model = new MetadataSearchViewModel();
            return View(model);
        }

        /// <summary>
        /// Returns a view where the user can see an overview of map layers.
        /// </summary>
        /// <returns></returns>
        public ActionResult WfsLayers()
        {
            WfsLayersViewManager viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var model = viewManager.CreateWfsLayersViewModel();           
            return View(model);
        }
      
        public ActionResult WmsLayers()
        {
            WmsLayersViewManager viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var model = viewManager.CreateWmsLayersViewModel();
            return View(model);
        }

        public JsonNetResult CreateWmsLayer(string data)
        {
            JsonModel jsonModel;
            try
            {
                var javascriptSerializer = new JavaScriptSerializer();
                WmsLayerViewModel wmsLayerViewModel = javascriptSerializer.Deserialize<WmsLayerViewModel>(data);
                var viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.CreateNewWmsLayer(wmsLayerViewModel);
                jsonModel = JsonModel.CreateSuccess("");
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Returns all WMS layers stored in MySettings
        /// </summary>
        /// <returns></returns>
        public JsonNetResult GetWmsLayers()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<WmsLayerViewModel> wmsLayers = viewManager.GetWmsLayers();
                jsonModel = JsonModel.Create(wmsLayers);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        public JsonNetResult RemoveWmsLayer(string data)
        {
            JsonModel jsonModel;
            try
            {
                var javascriptSerializer = new JavaScriptSerializer();
                WmsLayerViewModel wmsLayerViewModel = javascriptSerializer.Deserialize<WmsLayerViewModel>(data);
                var viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);                
                viewManager.RemoveWmsLayer(wmsLayerViewModel.Id);
                jsonModel = JsonModel.CreateSuccess("");
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Removes the specified WFS layers.
        /// </summary>
        /// <param name="wfsLayerIds">WFS layer ids.</param>
        /// <returns></returns>
        public JsonNetResult RemoveWfsLayers(int[] wfsLayerIds)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.RemoveWfsLayers(wfsLayerIds);

                jsonModel = JsonModel.CreateSuccess("");
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Adds selected uploaded gis layers to WFS layers.
        /// </summary>
        /// <param name="filenames">The filenames.</param>
        /// <returns></returns>
        public JsonNetResult AddUploadedGisLayers(string[] filenames)
        {
            JsonModel jsonModel = JsonModel.CreateSuccess("Ok");
            try
            {
                foreach(string filename in filenames)
                { 
                    if (string.IsNullOrEmpty(filename))
                    {
                        jsonModel = JsonModel.CreateFailure("No file selected.");
                        return new JsonNetResult(jsonModel);
                    }

                    try
                    {
                        var featureCollection = MySettingsManager.GetMapDataFeatureCollection(
                            GetCurrentUser(), 
                            filename,
                            CoordinateSystemId.None);

                        if (featureCollection == null)
                        {
                            jsonModel = JsonModel.CreateFailure("File not found.");
                            return new JsonNetResult(jsonModel);
                        }

                        if (AddFileWfsLayer(filename, featureCollection))
                        {
                            jsonModel = JsonModel.CreateSuccess("Ok");
                        }
                        else
                        {
                            jsonModel = JsonModel.CreateFailure(Resource.SharedLayerAlreadyExists);
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        jsonModel = JsonModel.CreateFailure(Resource.FilterSpatialUnableToParseGeoJsonFile);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Deletes the selected uploaded gis layers.
        /// </summary>
        /// <param name="filenames">The filenames.</param>
        /// <returns></returns>
        public JsonNetResult DeleteUploadedGisLayers(string[] filenames)
        {
            JsonModel jsonModel;
            try
            {
                jsonModel = JsonModel.CreateSuccess("Ok");

                foreach (string filename in filenames)
                {
                    if (!MySettingsManager.DeleteMapDataFile(GetCurrentUser(), filename))
                    {
                        jsonModel = JsonModel.CreateFailure("Deleting file failed.");
                    }

                    //Check if file layer is in use, if so, remove it
                    var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                    var layer = viewManager.GetWfsLayers().FirstOrDefault(l => l.Name == GetLayerName(filename));

                    if (layer != null)
                    {
                        viewManager.RemoveWfsLayer(layer.Id);
                    }
                }

                return new JsonNetResult(jsonModel);                
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);            
        }

        /// <summary>
        /// Removes the specified WMS layers.
        /// </summary>
        /// <param name="wmsLayerIds">WMS layer ids.</param>
        /// <returns></returns>
        public JsonNetResult RemoveWmsLayers(int[] wmsLayerIds)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);                
                viewManager.RemoveWmsLayers(wmsLayerIds);
                
                jsonModel = JsonModel.CreateSuccess("");
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Saves the changes made to a WMS layer
        /// </summary>        
        /// <returns></returns>
        [ValidateInput(false)]
        public RedirectToRouteResult WmsLayerEditorSaveChanges(int id, string data)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            WmsLayerViewModel wmsLayerViewModel = javascriptSerializer.Deserialize<WmsLayerViewModel>(data);
            var viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateWmsLayer(id, wmsLayerViewModel);
            return RedirectToAction("WmsLayers");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult AddWmsLayer()
        {
            var viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            WmsLayerEditorViewModel model = viewManager.CreateWmsLayerEditorViewModel(WmsLayerEditorMode.New, null);
            return View("WmsLayerEditor", model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult AddWmsLayer(WmsLayerEditorMode mode, int? id, string data)
        {
            return WmsLayerEditorPost(mode, id, data);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult EditWmsLayer(int id)
        {
            var viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            WmsLayerEditorViewModel model = viewManager.CreateWmsLayerEditorViewModel(WmsLayerEditorMode.Edit, id);
            return View("WmsLayerEditor", model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult EditWmsLayer(WmsLayerEditorMode mode, int? id, string data)
        {
            return WmsLayerEditorPost(mode, id, data);
        }
      
        /// <summary>
        /// Handle AddWmsLayer and EditWmsLayer Post. 
        /// </summary>        
        private ActionResult WmsLayerEditorPost(WmsLayerEditorMode mode, int? id, string data)
        {
            var viewManager = new WmsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var javascriptSerializer = new JavaScriptSerializer();
            WmsLayerViewModel wmsLayerViewModel = javascriptSerializer.Deserialize<WmsLayerViewModel>(data);
            if (ModelState.IsValid)
            {                
                if (mode == WmsLayerEditorMode.Edit)
                {
                    viewManager.UpdateWmsLayer(id.Value, wmsLayerViewModel);
                }
                else
                {
                    viewManager.CreateNewWmsLayer(wmsLayerViewModel);
                }

                return RedirectToAction("WmsLayers");
            }
            WmsLayerEditorViewModel model = viewManager.CreateWmsLayerEditorViewModel(mode, id);
            return View("WmsLayerEditor", model);
        }

        /// <summary>
        /// Returns all WFS layers stored in MySettings
        /// </summary>
        /// <returns></returns>
        public JsonNetResult GetPolygonWfsLayer()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<WfsLayerViewModel> wfsLayers = viewManager.CreatePolygonWfsLayersList();
                jsonModel = JsonModel.Create(wfsLayers);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Returns all WFS layers stored in MySettings
        /// </summary>
        /// <returns></returns>
        public JsonNetResult GetWfsLayer()
        {
            JsonModel jsonModel;
            try
            {                
                var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                List<WfsLayerViewModel> wfsLayers = viewManager.CreateWfsLayersList();                
                jsonModel = JsonModel.Create(wfsLayers);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Gets the logged in users all uploaded gis layers.
        /// </summary>
        /// <returns></returns>
        public JsonNetResult GetAllUploadedGisLayers()
        {
            JsonModel jsonModel;
            try
            {                
                var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                var uploadedGisLayers = viewManager.GetAllUploadedGisFiles();
                jsonModel = JsonModel.Create(uploadedGisLayers);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Removes the specified WFS layer.
        /// </summary>
        /// <param name="data">WfsLayer Id as JSON.</param>
        /// <returns></returns>
        public JsonNetResult RemoveWfsLayer(string data)
        {            
            JsonModel jsonModel;
            try
            {                
                var javascriptSerializer = new JavaScriptSerializer();
                WfsLayerViewModel wfsLayerViewModel = javascriptSerializer.Deserialize<WfsLayerViewModel>(data);
                var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                viewManager.RemoveWfsLayer(wfsLayerViewModel.Id);
                jsonModel = JsonModel.CreateSuccess("");
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }
        
        /// <summary>
        /// Renders a view where the user can search for WFS layers
        /// given an WFS Server URL.
        /// </summary>
        /// <param name="url">The WFS Server URL.</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public ActionResult AddWfsLayer(string url)
        {
            var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var model = viewManager.CreateAddWfsLayerViewModel(ModelState, url, Url.Action("UploadMapDataFile"), Url.Action("WfsLayers"));

            return View(model);
        }

        /// <summary>
        /// Renders a view where the user can search for WFS layers
        /// given an WFS Server URL.
        /// </summary>
        /// <param name="url">The WFS Server URL.</param>
        /// <param name="wfsVersion">The WFS version.</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult AddWfsLayer(string url, WFSVersion wfsVersion)
        {
            string wfsUrl = null;
            try
            {
                wfsUrl = WFSManager.CreateGetCapabiltiesRequestUrl(url, wfsVersion);
            }
            catch (UriFormatException)
            {
                ModelState.AddModelError("", "The URL has wrong format");                
            }

            if (ModelState.IsValid)
            {
                if (wfsVersion == WFSVersion.Unknown)
                {
                    wfsVersion = WFSManager.GetWFSVersionFromRequestUrl(url);
                    if (wfsVersion == WFSVersion.Unknown)
                    {
                        wfsVersion = WFSVersion.Ver110;
                    }
                }                               
                return RedirectToAction("AddWfsLayer", new { @url = wfsUrl });
            }
            var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var model = viewManager.CreateAddWfsLayerViewModel(ModelState, url, Url.Action("UploadMapDataFile"), Url.Action("WfsLayers"));

            return View(model);
        }

        /// <summary>
        /// Uploads a geojson file to be used as spatial filter.
        /// </summary>
        /// <param name="coordinateSystemId">The coordinate system id.</param>
        /// <returns>A JsonModel.</returns>
        [System.Web.Mvc.HttpPost]
        public JsonNetResult UploadMapDataFile(int? coordinateSystemId)
        {
            JsonModel jsonModel;
            
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

            var file = Request.Files[0];
            
            if (file == null || file.ContentLength == 0)
            {
                jsonModel = JsonModel.CreateFailure("The file has no content.");
                return new JsonNetResult(jsonModel);
            }

            const long MAX_FILE_SIZE = 5120000; //5MB
            if (file.ContentLength > MAX_FILE_SIZE)
            {
                jsonModel = JsonModel.CreateFailure(string.Format("Max file size {0} MB", MAX_FILE_SIZE / 1024000));
                return new JsonNetResult(jsonModel);
            }

            //Set file name for uploaded file and remove file suffix for layer name
            var fileName = file.FileName;

            try
            {
                var jsonString = string.Empty;
                if (file.ContentType == "application/zip" || file.ContentType == "application/x-zip-compressed" ||
                    fileName.ToLower().EndsWith(".zip")) //|| file.ContentType == "application/octet-stream")
                {
                    jsonString = MySettingsManager.GetGeoJsonFromShapeZip(GetCurrentUser(), file.FileName,
                        file.InputStream);

                    //Change file suffix since we converted shape to geojson
                    fileName = fileName.Replace(".zip", ".geojson");
                }
                else
                {
                    jsonString = new StreamReader(file.InputStream).ReadToEnd();
                }

                /*if (MySettingsManager.MapDataFileExists(GetCurrentUser(), fileName))
                {
                    jsonModel = JsonModel.CreateFailure(Resource.SharedFileAlreadyExists);
                    return new JsonNetResult(jsonModel);
                }*/

                var featureCollection =
                    JsonConvert.DeserializeObject(jsonString, typeof(FeatureCollection)) as FeatureCollection;

                if (featureCollection == null)
                {
                    jsonModel = JsonModel.CreateFailure(Resource.UploadFileJsonSerialaztionFaild);
                    return new JsonNetResult(jsonModel);
                }

                CoordinateSystem coordinateSystem = null;
                if (coordinateSystemId.HasValue)
                {
                    coordinateSystem = new CoordinateSystem((CoordinateSystemId) coordinateSystemId.Value);
                }
                else
                {
                    coordinateSystem = GisTools.GeoJsonUtils.FindCoordinateSystem(featureCollection);
                    if (coordinateSystem.Id == CoordinateSystemId.None)
                    {
                        jsonModel = JsonModel.CreateFailure(Resource.FilterSpatialUnableToDetermineCoordinateSystem);
                        return new JsonNetResult(jsonModel);
                    }
                }

                //Make sure crs is saved in file
                if (featureCollection.CRS == null)
                {
                    featureCollection.CRS = new NamedCRS(coordinateSystem.Id.EpsgCode());
                }

                //create a Json object from string
                var jsonObject = JObject.FromObject(featureCollection);

                //Sava json to file
                using (var fileStream = jsonObject.ToString().ToStream())
                {
                    MySettingsManager.SaveMapDataFile(GetCurrentUser(), fileName, fileStream);
                }

                if (AddFileWfsLayer(fileName, featureCollection))
                {
                    jsonModel = JsonModel.CreateSuccess("Ok");
                }
                else
                {
                    jsonModel = JsonModel.CreateFailure(Resource.UploafFileLayerAlreadyAdded);
                }
            }
            catch (ImportGisFileException importGisFileException)
            {
                jsonModel = JsonModel.CreateFailure(importGisFileException.Message);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(Resource.FilterSpatialUnableToParseGeoJsonFile);
            }

            return new JsonNetResult(jsonModel);
        }

        [System.Web.Mvc.HttpPost]
        public JsonNetResult AddMapDataFileLayer(string fileName)
        {
            JsonModel jsonModel;

            if (string.IsNullOrEmpty(fileName))
            {
                jsonModel = JsonModel.CreateFailure("No file selected.");
                return new JsonNetResult(jsonModel);
            }
            
            try
            {
                var featureCollection = MySettingsManager.GetMapDataFeatureCollection(GetCurrentUser(), fileName, CoordinateSystemId.None);
                
                if (featureCollection == null)
                {
                    jsonModel = JsonModel.CreateFailure("File not found.");
                    return new JsonNetResult(jsonModel);
                }

                if (AddFileWfsLayer(fileName, featureCollection))
                {
                    jsonModel = JsonModel.CreateSuccess("Ok");
                }
                else
                {
                    jsonModel = JsonModel.CreateFailure(Resource.SharedLayerAlreadyExists);
                }
            }
            catch (Exception)
            {
                jsonModel = JsonModel.CreateFailure(Resource.FilterSpatialUnableToParseGeoJsonFile);
            }

            return new JsonNetResult(jsonModel);
        }

        [System.Web.Mvc.HttpPost]
        public JsonNetResult DeleteMapDataFile(string fileName)
        {
            var jsonModel = JsonModel.CreateSuccess("Ok");

            if (!MySettingsManager.DeleteMapDataFile(GetCurrentUser(), fileName))
            {
                jsonModel = JsonModel.CreateFailure("Deleting file failed.");
            }

            //Check if file layer is in use, if so, remove it
            var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var layer = viewManager.GetWfsLayers().FirstOrDefault(l => l.Name == GetLayerName(fileName));

            if (layer != null)
            {
                viewManager.RemoveWfsLayer(layer.Id);
            }

            return new JsonNetResult(jsonModel);
        }

        [System.Web.Mvc.HttpPost]
        public RedirectToRouteResult CreateWfsLayer(string url, string name, string typeName)
        {
            var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var hexColor = SessionHandler.MySettings.DataProvider.MapLayers.CreateMapLayerHexColor();
            var wfsLayer = viewManager.CreateNewWfsLayer(name, "", url, typeName, hexColor, false);
          
            viewManager.AddWfsLayer(wfsLayer);
            return RedirectToAction("WfsLayers");
        }

        /// <summary>
        /// Saves the changes made to a WFS layer
        /// </summary>
        /// <param name="id">The WFS layer Id.</param>
        /// <param name="name">The new name.</param>
        /// <param name="filter">The new filter.</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public RedirectToRouteResult WfsLayerEditorSaveChanges(int id, string name, string filter, string color, bool useBbox)
        {
            var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            viewManager.UpdateWfsLayer(id, name, filter, color, useBbox);
            return RedirectToAction("WfsLayers");
        }

        /// <summary>
        /// Creates a new WFS layer an redirects to MapLayers overview.
        /// </summary>
        /// <param name="name">The layer name.</param>
        /// <param name="filter">The layer filter.</param>
        /// <param name="serverUrl">The WFS server URL.</param>
        /// <param name="typeName">The FeatureType.</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public RedirectToRouteResult WfsLayerEditorCreateNewLayer(string name, string filter, string serverUrl, string typeName, string color, bool useBbox) 
        {
            var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            WfsLayerSetting wfsLayer = viewManager.CreateNewWfsLayer(name, filter, serverUrl, typeName, color, useBbox);
            viewManager.AddWfsLayer(wfsLayer);            
            return RedirectToAction("WfsLayers");
        }

        /// <summary>
        /// Render a WFS layer editor view.
        /// </summary>
        /// <param name="url">The WFS Server URL.</param>
        /// <param name="typeName">FeatureType.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="id">The id (used if mode is Edit).</param>
        /// <returns></returns>
        public ActionResult WfsLayerEditor(string url, string typeName, string filter, WfsLayerEditorMode? mode, int? id)
        {
            ViewBag.RenderMapScriptTags = true;
            var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);            
            WfsLayerEditorViewModel model = viewManager.CreateWfsLayerEditorViewModel(url, typeName, filter, mode, id);            
            return View(model);
        }

        public JsonNetResult GetWfsLayerSettings(int id)
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);
                WfsLayerViewModel model = viewManager.CreateWfsLayerViewModel(id);
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        public RedirectResult ResetWfsLayers(string returnUrl)
        {
            SessionHandler.MySettings.DataProvider.MapLayers.ResetWfsSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.DataProvidersWfsLayersReset));
            return Redirect(returnUrl);
        }

        public RedirectResult ResetWmsLayers(string returnUrl)
        {
            SessionHandler.MySettings.DataProvider.MapLayers.ResetWmsSettings();
            SessionHandler.UserMessages.Add(new UserMessage(Resources.Resource.DataProvidersWmsLayersReset));
            return Redirect(returnUrl);
        }

        public PartialViewResult DataProviderInfoDialog(int id)
        {
            var viewManager = new DataProvidersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            DataProviderViewModel model = viewManager.CreateDataProviderViewModel(id);
            return PartialView(model);            
        }
    }
}
