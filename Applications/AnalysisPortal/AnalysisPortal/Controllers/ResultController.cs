using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.SessionState;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.ActionFilters;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.CoordinateConversion;
using ArtDatabanken.GIS.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservationProvenanceResult;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Diagrams;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Reports;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;
using ArtDatabanken.WebApplication.AnalysisPortal.Extensions;
using Newtonsoft.Json;
using ArtDatabanken.GIS.GDAL;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Details;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;
using Resources;
using Microsoft.SqlServer.Types;

namespace AnalysisPortal.Controllers
{
    using System.Drawing;
    using System.Web;
    using System.Web.Caching;
    using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;

    using Exception = System.Exception;

    /// <summary>
    /// This Controller contains Actions that renders and calculates the Result pages.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ResultController : BaseController
    {
        [HttpGet]
        public PartialViewResult ExcelExportDialog()
        {
            var downloadCoordinateSystems = CoordinateSystemHelper.GetDownloadMapCoordinateSystems();
            ViewBag.CoordinateSystems = downloadCoordinateSystems;            
            var viewManager = new SpeciesObservationTableSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var tableColumnsModel = viewManager.CreateSpeciesObservationTableSettingsViewModel();
            ViewBag.TableColumnsModel = tableColumnsModel;
            return PartialView();
        }

        //[HttpPost]
        //public FileResult ExcelExportDialog(bool? addSettings, bool? addProvenance, int? presentationCoordinateSystem)
        //{
        //    DownloadController downloadController = new DownloadController();
        //    var result = downloadController.SpeciesObservationsAsExcel(addSettings, addProvenance, presentationCoordinateSystem, null, null);
        //    SetServerDone();
        //    return result;
        //}

        [HttpGet]
        public PartialViewResult _LayerExport(int? layerId = null, string attribute = null, bool preSelectMode = false, LayerExportModel.FileExportFormat exportFormat = LayerExportModel.FileExportFormat.GeoJson )
        {
            var model = new LayerExportModel
            {
                Attribute = attribute,
                DataType = DataType.Float32,
                DataTypes = new[]
                {
                    new SelectListItem() { Text = "Byte", Value = "6" },
                    new SelectListItem() { Text = "Integer", Value = "3" },
                    new SelectListItem() { Text = "Float 32", Value = "7" },
                    new SelectListItem() { Text = "Float 64", Value = "2" }
                },
                ExportFormat = exportFormat,
                ExportFormats = new[]
                {
                    new SelectListItem() { Text = "GeoJson", Value = LayerExportModel.FileExportFormat.GeoJson.ToString() },
                    new SelectListItem() { Text = "GeoTiff", Value = LayerExportModel.FileExportFormat.GeoTiff.ToString() },
                    new SelectListItem() { Text = "Shape", Value = LayerExportModel.FileExportFormat.Shape.ToString() }
                },
                LayerId = layerId,
                PreSelectMode = preSelectMode
            };

            return PartialView(model);
        }

        [HttpPost]
        public FileResult _LayerExport(LayerExportModel model)
        {
            if (!model.LayerId.HasValue)
            {
                return null;
            }

            var fileName = string.Empty;
            var coordinateSystem = SessionHandler.MySettings.Presentation.Map.DisplayCoordinateSystem.Id;
            var parameters = new Dictionary<string, object>()
            {
                { "attribute", model.Attribute },
                { "alphaValue", model.AlphaValue ?? 0 },
                { "useCenterPoint", model.UseCenterPoint }
            };

            var geoJson = MapLayerManager.GetLayerGeojson(GetCurrentUser(), model.LayerId.Value, coordinateSystem, parameters, out fileName, null);
           
            byte[] fileContents = null;
            string contentType = null;

            switch (model.ExportFormat)
            {
                case LayerExportModel.FileExportFormat.GeoJson:
                    fileContents = System.Text.Encoding.UTF8.GetBytes(geoJson);
                    contentType = "application/json";                    
                    fileName = FilenameGenerator.CreateFilename(fileName, FileType.GeoJSON);
                    break;
                case LayerExportModel.FileExportFormat.GeoTiff:
                    if (model.RazterSize == 0)
                    {
                        model.PixelSize = 0;
                    }
                    else
                    {
                        model.PixelsWidth = 0;
                        model.PixelsHeight = 0;
                    }

                    fileContents = GeoJsonManager.CreateTiffFile(geoJson, coordinateSystem, Server.MapPath("~") + "Temp", model.PixelsWidth, model.PixelsHeight, model.PixelSize, model.Attribute, model.DataType);
                    contentType = "image/tif";                    
                    fileName = FilenameGenerator.CreateFilename(fileName, FileType.Tiff);
                    break;
                case LayerExportModel.FileExportFormat.Shape:
                    fileContents = ShapeManager.CreateShapeFilesAsZip(geoJson, Server.MapPath("~") + "Temp\\", fileName);
                    contentType = "application/zip";                    
                    fileName = FilenameGenerator.CreateFilename(fileName, FileType.Zip);
                    break;
            }

            SetServerDone();

            return File(fileContents, contentType, fileName);
        }

        [HttpGet]
        public PartialViewResult _MapExport()
        {
            return PartialView();
        }

        /// <summary>
        /// Export map to image
        /// </summary>
        /// <param name="jsonBase64Model"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult _MapExport(string jsonModel)
        {
            var model = jsonModel.ToObject<MapExportModel>();
            var layerName = string.Empty;
           
            //Convert extent to SWEREF 99 if it's in another coordinate system
            var currentCoordinateSystem = SessionHandler.MySettings.Presentation.Map.DisplayCoordinateSystem;
            if (currentCoordinateSystem.Id != CoordinateSystemId.SWEREF99_TM)
            {
                var cm = new CoordinateConversionManager();
                var lb = cm.ConvertPosition(new GeographicPosition(model.MapExtent.Left, model.MapExtent.Bottom), currentCoordinateSystem, new CoordinateSystem(CoordinateSystemId.SWEREF99_TM));
                var rt = cm.ConvertPosition(new GeographicPosition(model.MapExtent.Right, model.MapExtent.Top), currentCoordinateSystem, new CoordinateSystem(CoordinateSystemId.SWEREF99_TM));

                model.MapExtent.Bottom = lb.Y;
                model.MapExtent.Left = lb.X;
                model.MapExtent.Right = rt.X;
                model.MapExtent.Top = rt.Y;
            }
            
            //Temporary change coordinate system to swe ref 99 to ensure metric scale
            var tmpCoordinateSystem = SessionHandler.MySettings.Presentation.Map.DisplayCoordinateSystem;
            SessionHandler.MySettings.Presentation.Map.PresentationCoordinateSystemId = CoordinateSystemId.SWEREF99_TM;

            var exportLayers = new List<MapExportModel.Layer>();
            //Get geo json for each layer
            foreach (var layer in model.Layers)
            {
                MapLayerManager.SetCustomLayerProperties(layer);
                var parameters = new Dictionary<string, object>()
                {
                    { "attribute", layer.Attribute },
                    { "alphaValue", layer.AlphaValue },
                    { "useCenterPoint", layer.UseCenterPoint ?? true }
                };
                
                var geoJson = MapLayerManager.GetLayerGeojson(GetCurrentUser(), layer.Id, CoordinateSystemId.SWEREF99_TM, parameters, out layerName, model.MapExtent);

                if (!string.IsNullOrEmpty(geoJson))
                {
                    layer.GeoJson = geoJson;
                    exportLayers.Add(layer);
                }
            }

            model.Layers = exportLayers.ToArray();

            //Change back coordinate system
            SessionHandler.MySettings.Presentation.Map.PresentationCoordinateSystemId = tmpCoordinateSystem.Id;

            //Generate image using Python script in QGIS            
            var fileContents = MapExportViewManager.GetMapImage(Server, model, ConfigurationHelper.GetValue("QGISPath", string.Empty), ConfigurationHelper.GetValue("PythonExePath", string.Empty));
            var contentType = "image/png";
            var fileName = "Map.png";

            SetServerDone();
            
            return File(fileContents, contentType, fileName);
        }        

        /// <summary>
        /// Renders the result page.
        /// </summary>
        /// <param name="expand">
        /// The expand.
        /// </param>
        /// <returns>Index View.
        /// </returns>
        [IndexedBySearchRobots]
        public ActionResult Index(ResultGroupType? expand)
        {
            ViewBag.RenderMapScriptTags = true;
            var model = new ResultsViewModel(expand);

            // foreach (var resultGroup in model.ResultGroups)
            // {
            //    foreach (var resultView in resultGroup.Items)
            //    {
            //        System.Diagnostics.Debug.WriteLine("{0}: {1}", resultView.Title, resultView.GetResultStatus(SessionHandler.MySettings));
            //    }
            // }
            // System.Diagnostics.Debug.WriteLine("-----------------------------------------------------------------------------------");
            // System.Diagnostics.Debug.WriteLine("");
            // System.Diagnostics.Debug.WriteLine("");
            return View(model);
        }

        /// <summary>
        /// Renders the result page.
        /// </summary>
        /// <param name="expand">
        /// The expand.
        /// </param>
        /// <returns>
        /// Download View.
        /// </returns>
        [IndexedBySearchRobots]
        public ActionResult Download(ResultGroupType? expand)
        {
            var downloadCoordinateSystems = CoordinateSystemHelper.GetDownloadMapCoordinateSystems();
            ViewBag.CoordinateSystems = downloadCoordinateSystems;
            var viewManager = new SpeciesObservationTableSettingsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var tableColumnsModel = viewManager.CreateSpeciesObservationTableSettingsViewModel();
            ViewBag.TableColumnsModel = tableColumnsModel;
            var model = new ResultsDownloadViewModel(expand);
            return View(model);
        }

        /// <summary>
        /// Renders the help/info page.
        /// </summary>
        /// <returns>
        /// Info View.
        /// </returns>
        public ActionResult Info()
        {
            string localeIsoCode = Thread.CurrentThread.CurrentCulture.Name;
            AboutViewModel model = AboutManager.GetAboutResultFormatViewModel(localeIsoCode);
            return View(model);
        }

        /// <summary>
        /// Renders the map result types overview page.
        /// </summary>
        /// <returns>Map result view.</returns>
        [IndexedBySearchRobots]
        public ViewResult Maps()
        {
            return View(new MapsResultGroup());
            
            // return RedirectToAction("Index", new {expand = ResultGroupType.Maps });
        }
        
        /// <summary>
        /// Renders the table result types overview page.
        /// </summary>
        /// <returns>Returns table result.</returns>
        [IndexedBySearchRobots]
        public ViewResult Tables()
        {
            return View(new TablesResultGroup());
            //// return RedirectToAction("Index", new { expand = ResultGroupType.Tables });
        }

        /// <summary>
        /// Renders the report result types overview page.
        /// </summary>
        /// <returns>Returns reports result group.</returns>
        [IndexedBySearchRobots]
        public ViewResult Reports()
        {
            return View(new ReportsResultGroup());
            
            // return RedirectToAction("Index", new { expand = ResultGroupType.Report });
        }
        
        /// <summary>
        /// Renders the diagram result types overview page.
        /// </summary>
        /// <returns>Diagram view result.</returns>
        [IndexedBySearchRobots]
        public ViewResult Diagrams()
        {
            return View("Diagrams", new DiagramsResultGroup());
            
            // return RedirectToAction("Index", new { expand = ResultGroupType.Diagrams });
        }

        /// <summary>
        /// Renders the workflow page.
        /// </summary>
        /// <returns>Workflow view result.</returns>
        public ViewResult Workflow()
        {
            IUserContext currentUser = GetCurrentUser();
            WorkflowViewModel model = new WorkflowViewModel();
            model.IsUserLoggedIn = currentUser.IsAuthenticated();
            model.IsUserCurrentRolePrivatePerson = currentUser.IsCurrentRolePrivatePerson();
            ////if (currentUser.User.UserName == Resources.AppSettings.Default.ApplicationUserName)
            ////{
            ////    model.IsUserCurrentRolePrivatePerson = true;
            ////}

            return View(model);
        }

        /// <summary>
        /// Renders a partial view containing links to BioVel workflow and its data.
        /// </summary>
        /// <param name="absoluteUrl">The absolute URL to the data (csv file).</param>
        /// <returns>Workflow links partial view.</returns>
        public PartialViewResult WorkflowLinks(string absoluteUrl)
        {
            return PartialView("WorkflowLinks", absoluteUrl);
        }

        /// <summary>
        /// Workflows the links dialog.
        /// </summary>
        /// <param name="absoluteUrl">The absolute URL.</param>
        /// <returns>A partial view.</returns>
        public PartialViewResult WorkflowLinksDialog(string absoluteUrl)
        {
            return PartialView("WorkflowLinksDialog", absoluteUrl);
        }

        /// <summary>
        /// Renders a partial view with information about if a user is logged in and not
        /// has the "Privatperson" role as current role.
        /// </summary>
        /// <param name="model">A WorkflowViewModel.</param>
        /// <returns>A partial view.</returns>
        public PartialViewResult WorkflowUserAccessErrorDialog(WorkflowViewModel model)
        {
            return PartialView(model);
        }

        /// <summary>
        /// Displays species observations as a table view with paging.
        /// </summary>
        /// <returns>Species observation table result view.</returns>
        [IndexedBySearchRobots]
        public ViewResult SpeciesObservationTable()
        {
            var viewManager = new SpeciesObservationTableViewManager(GetCurrentUser(), SessionHandler.MySettings);
            ViewTableViewModel model = viewManager.CreateViewTableViewModel();                        
            return View(model);
        }
        
        ///// <summary>
        ///// Displayes species observations as a table view with paging.
        ///// </summary>
        ///// <returns></returns>
        // public ViewResult PagedSpeciesObservationTable()
        // {
        //    var viewManager = new SpeciesObservationTableViewManager(GetCurrentUser(), SessionHandler.MySettings);
        //    ViewTableViewModel model = viewManager.CreateViewTableViewModel();
        //    return View(model);
        //// }

        /// <summary>
        /// The WFS grid statistics map.
        /// </summary>
        /// <returns>
        /// The <see cref="ViewResult"/>.
        /// </returns>
        [IndexedBySearchRobots]
        public ViewResult WfsGridStatisticsMap()
        {
            var viewManager = new WfsGridStatisticsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            var model = viewManager.CreateViewModel();
            
            // ViewTableViewModel model = viewManager.CreateViewTableViewModel();
            return View(model);
        }

        /// <summary>
        /// The get WFS grid count as JSON.
        /// </summary>
        /// <param name="coordinateSystemId">
        /// The coordinate system id.
        /// </param>
        /// <param name="gridSize">
        /// The grid size.
        /// </param>
        /// <param name="wfsLayerId">
        /// The WFS layer id.
        /// </param>
        /// <returns>
        /// The <see cref="JsonNetResult"/>.
        /// </returns>
        public JsonNetResult GetWfsGridCountAsJSON(int coordinateSystemId, int gridSize, int wfsLayerId)
        {
            JsonModel jsonModel;
            WfsGridCalculator resultCalculator = null;
            try
            {
                resultCalculator = new WfsGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
                WfsStatisticsGridResult model = resultCalculator.CalculateGridResult(GetCurrentUser(), coordinateSystemId, gridSize, wfsLayerId);
                
                // WfsStatisticsGridResult model = resultCalculator.GetResult();
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// The combined grid statistics table.
        /// </summary>
        /// <returns>
        /// The <see cref="ViewResult"/>.
        /// </returns>
        public ViewResult CombinedGridStatisticsTable()
        {
            return View();
        }

        /// <summary>
        /// The get combined grid statistics as JSON.
        /// </summary>
        /// <param name="coordinateSystemId">
        /// The coordinate system id.
        /// </param>
        /// <param name="gridSize">
        /// The grid size.
        /// </param>
        /// <param name="wfsLayerId">
        /// The WFS layer id.
        /// </param>
        /// <returns>
        /// The <see cref="JsonNetResult"/>.
        /// </returns>
        public JsonNetResult GetCombinedGridStatisticsAsJSON(int coordinateSystemId, int gridSize, int wfsLayerId)
        {
            JsonModel jsonModel;
            CombinedGridStatisticsCalculator resultCalculator = null;
            try
            {
                resultCalculator = new CombinedGridStatisticsCalculator(GetCurrentUser(), SessionHandler.MySettings);
                CombinedGridStatisticsResult model = resultCalculator.CalculateCombinedGridResult(coordinateSystemId, gridSize, wfsLayerId);
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Displays species observations taxa as a table.
        /// </summary>
        /// <returns>The SpeciesObservationTaxonTable view.</returns>
        [IndexedBySearchRobots]
        public ViewResult SpeciesObservationTaxonTable()
        {
            SpeciesObservationTaxonTableResultCalculator resultCalculator = new SpeciesObservationTaxonTableResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
            QueryComplexityEstimate queryComplexity = resultCalculator.GetQueryComplexityEstimate();
            return View(queryComplexity);
        }

        /// <summary>
        /// Displays species observations taxa, with related number of observations, as a table.
        /// </summary>
        /// <returns>The SpeciesObservationTaxonWithSpeciesObservationCountTable view.</returns>
        [IndexedBySearchRobots]
        public ViewResult SpeciesObservationTaxonWithSpeciesObservationCountTable()
        {
            SpeciesObservationTaxonSpeciesObservationCountTableResultCalculator resultCalculator = new SpeciesObservationTaxonSpeciesObservationCountTableResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
            QueryComplexityEstimate queryComplexity = resultCalculator.GetQueryComplexityEstimate();
            return View(queryComplexity);
        }

        /// <summary>
        /// Gets all filtered taxa, with related number of observations.
        /// </summary>
        /// <returns>The result in form of a Json object.</returns>        
        public JsonNetResult GetObservationTaxaListAsJSON()
        {
            JsonModel jsonModel;
            SpeciesObservationTaxonTableResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new SpeciesObservationTaxonTableResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
                List<TaxonViewModel> model = resultCalculator.GetResultFromCacheIfAvailableOrElseCalculate();
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);            
        }

        /// <summary>
        /// Gets all filtered taxa.
        /// </summary>
        /// <returns>The result in form of a Json object.</returns>
        public JsonNetResult GetObservationTaxaWithSpeciesObservationCountListAsJSON()
        {
            JsonModel jsonModel;
            SpeciesObservationTaxonSpeciesObservationCountTableResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new SpeciesObservationTaxonSpeciesObservationCountTableResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
                List<TaxonSpeciesObservationCountViewModel> model = resultCalculator.GetResultFromCacheIfAvailableOrElseCalculate();
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Displays summary statistics per polygon as a table.
        /// </summary>
        /// <returns>The SummaryStatisticsPerPolygonTable view.</returns>
        [IndexedBySearchRobots]
        public ViewResult SummaryStatisticsPerPolygonTable()
        {
            SummaryStatisticsPerPolygonResultCalculator resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
            QueryComplexityEstimate queryComplexity = resultCalculator.GetQueryComplexityEstimate();
            return View(queryComplexity);
        }

        /// <summary>
        /// Displays a grid map on numbers of observations per grid cell.
        /// </summary>
        /// <returns>The SpeciesObservationGridMap view.</returns>
        [IndexedBySearchRobots]
        public ActionResult SpeciesObservationGridMap()
        {
            var spartialFilterSettings = SessionHandler.MySettings.Filter.Spatial;

            return View(spartialFilterSettings.HasSettings && spartialFilterSettings.IsActive);
        }

        [IgnorePageInfoManager]
        public ActionResult SpeciesObservationClusterPointMap()
        {
            ViewBag.CalculationCoordinateSystemTitle =
                ((CoordinateSystemId)SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId.GetValueOrDefault()).GetCoordinateSystemName();
            ViewBag.ViewCoordinateSystemTitle = SessionHandler.MySettings.Presentation.Map.PresentationCoordinateSystemId.GetCoordinateSystemName();
            return View();
        }

        [IgnorePageInfoManager]
        public ActionResult SpeciesObservationClusterGridMap()
        {
            return View();
        }

        /// <summary>
        /// Displays a grid map on number of species per grid cell.
        /// </summary>
        /// <returns>The SpeciesRichnessGridMap view.</returns>
        [IndexedBySearchRobots]
        public ActionResult SpeciesRichnessGridMap()
        {
            var spartialFilterSettings = SessionHandler.MySettings.Filter.Spatial;

            return View(spartialFilterSettings.HasSettings && spartialFilterSettings.IsActive);
        }

        /// <summary>
        /// Method that creates a grid map for a specific taxon.
        /// </summary>
        /// <param name="id">
        /// Dyntaxa taxon id.
        /// </param>
        /// <returns>
        /// Grid map view with species observation counts.
        /// </returns>
        [IndexedBySearchRobots]
        public ActionResult DefaultSpeciesObservationGridMap(Int32 id)
        {
            SessionHandler.MySettings.Filter.Taxa.TaxonIds.Clear();
            if (id > 0)
            {
            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(id);
            }

            SessionHandler.MySettings.Filter.Taxa.AddTaxonId(id);
            return View("SpeciesObservationGridMap");
        }

        /// <summary>
        /// Displays a table with grid statistics on species observation counts.
        /// </summary>
        /// <returns>The GridStatisticsTableOnSpeciesObservationCounts view.</returns>
        [IndexedBySearchRobots]
        public ActionResult GridStatisticsTableOnSpeciesObservationCounts()
        {
            var viewManager = new GridStatisticsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            ResultSpeciesObservationGridTableViewModel model = viewManager.CreateResultSpeciesObservationGridTableViewModel();
            return View(model);
        }

        /// <summary>
        /// Displays an histogram representing a time series on species observation counts.
        /// </summary>
        /// <returns>The TimeSeriesHistogramOnSpeciesObservationCounts view.</returns>
        [IndexedBySearchRobots]
        public ViewResult TimeSeriesHistogramOnSpeciesObservationCounts()
        {
            var viewManager = new SpeciesObservationResultViewManager(GetCurrentUser(), SessionHandler.MySettings);
            ResultTimeSeriesOnSpeciesObservationCountsViewModel model = viewManager.CreateResultTimeSeriesOnSpeciesObservationCountsViewModel();

            return View("TimeSeriesHistogramOnSpeciesObservationCounts", model);
        }

        /// <summary>
        /// Displays time series on species observation counts as a table.
        /// </summary>
        /// <returns>The TimeSeriesTableOnSpeciesObservationCounts view.</returns>
        [IndexedBySearchRobots]
        public ViewResult TimeSeriesTableOnSpeciesObservationCounts()
        {
            var viewManager = new SpeciesObservationResultViewManager(GetCurrentUser(), SessionHandler.MySettings);
            ResultTimeSeriesOnSpeciesObservationCountsViewModel model = viewManager.CreateResultTimeSeriesOnSpeciesObservationCountsViewModel();

            return View("TimeSeriesTableOnSpeciesObservationCounts", model);
        }

        /// <summary>
        /// Displays a line representing a time series on abundance index of species observation.
        /// </summary>
        /// <returns>The TimeSeriesDiagramOnSpeciesObservationAbundanceIndex view.</returns>
        [IndexedBySearchRobots]
        public ViewResult TimeSeriesDiagramOnSpeciesObservationAbundanceIndex()
        {
            var viewManager = new SpeciesObservationResultViewManager(GetCurrentUser(), SessionHandler.MySettings);
            ResultTimeSeriesOnSpeciesObservationCountsViewModel model = viewManager.CreateResultTimeSeriesDiagramOnSpeciesObservationAbundanceIndexViewModel();

            return View("TimeSeriesDiagramOnSpeciesObservationAbundanceIndex", model);
        }     

        /// <summary>
        /// Displays a species observation distribution map with clickable points.
        /// This view only shows a limited number of observations at time. Paging is applied.
        /// </summary>
        /// <returns>The SpeciesObservationMap view.</returns>
        [IndexedBySearchRobots]
        public ViewResult SpeciesObservationMap()
        {
            // QueryComplexityManager complexityManager = new QueryComplexityManager(GetCurrentUser(), SessionHandler.MySettings);
            // QueryComplexityEstimate complexityEstimate = complexityManager.GetQueryComplexityEstimate(ResultType.SpeciesObservationMap);            
            SpeciesObservationResultViewManager viewManager = new SpeciesObservationResultViewManager(GetCurrentUser(), SessionHandler.MySettings);
            ResultSpeciesObservationMapViewModel model = viewManager.CreateResultSpeciesObservationMapViewModel();
            return View(model);
        }

        /// <summary>
        /// Displays a table with grid statistics on species richness, i.e. number of species per grid cell.
        /// </summary>
        /// <returns>The GridStatisticsTableOnSpeciesRichness view.</returns>
        [HttpGet]
        [IndexedBySearchRobots]
        public ActionResult GridStatisticsTableOnSpeciesRichness()
        {
            var viewManager = new GridStatisticsViewManager(GetCurrentUser(), SessionHandler.MySettings);
            ResultTaxonGridTableViewModel model = viewManager.CreateResultTaxaGridTableViewModel();
            return View(model);
        }

        /// <summary>
        /// Displays a view with an observations heat map.
        /// </summary>
        /// <returns>The heat map view.</returns>
        [HttpGet]
        ////[IndexedBySearchRobots]
        public ActionResult SpeciesObservationHeatMap()
        {
            string uniqueId = Guid.NewGuid().ToString("N");
   
            new Thread((object mySettings) =>
                {
                    MySettings settings = (MySettings)mySettings;
                    Bitmap image = CreateSpeciesObservationHeatMapImage(null, settings);
                    HttpRuntime.Cache.Add(uniqueId, image, null, DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                    System.Diagnostics.Debug.WriteLine(string.Format("Image with id: {0} is generated", uniqueId));
                }).Start(SessionHandler.MySettings);
            ViewBag.UniqueId = uniqueId;
            return View();
        }

        public JsonNetResult IsSpeciesObservationHeatMapGenerated(string id)
        {
            bool isGenerated = HttpRuntime.Cache[id] != null;
            
            return new JsonNetResult(isGenerated);
        }

        /// <summary>
        /// Creates a map based on county occurrence information from Artfakta.
        /// </summary>
        /// <param name="id">Dyntaxa TaxonId.</param>
        /// <returns>A PNG file.</returns>
        public FileResult FetchGeneratedSpeciesObservationHeatMapImage(string id)
        {
            var map = (Bitmap)HttpRuntime.Cache[id];
                
            if (map == null)
            {
                return null;
            }

            var returnStream = new System.IO.MemoryStream();
            map.Save(returnStream, System.Drawing.Imaging.ImageFormat.Png);
            returnStream.Position = 0;

            //return new FileStreamResult(returnStream, "image/png");
            var fileStream = new FileStreamResult(returnStream, "image/png");
            return File(fileStream.FileStream, "image/png", "SpeciesObservationHeatMapImage.png");
        }

        private Bitmap CreateSpeciesObservationHeatMapImage(int? taxonId, MySettings mySettings)
        {
            var gridCalculator = new SpeciesObservationGridCalculator(GetCurrentUser(), mySettings);
            var countList = gridCalculator.CalculateSpeciesObservationGrid(taxonId, GridCoordinateSystem.Rt90_25_gon_v);
      
            var gridCells = countList.Cast<IGridCellBase>().ToList();
            Func<IGridCellBase, long> countMethod = x => ((IGridCellSpeciesObservationCount)x).ObservationCount;
            var countMap = new MapImage() { Height = 1600 };

            return countMap.GetHeatMap(gridCells, countMethod);
        }

        /// <summary>
        /// Renders the ResultSummaryStatisticsView.
        /// </summary>
        /// <returns>A summery statistics report.</returns>
        [IndexedBySearchRobots]
        public ActionResult SummaryStatisticsReport()
        {
            return View("SummaryStatisticsReport");
        }

        /// <summary>
        /// Renders the ResultProvenanceView.
        /// </summary>
        /// <returns>A summery statistics report.</returns>
        [IndexedBySearchRobots]
        public ActionResult ProvenanceReport()
        {
            return View("SpeciesObservationProvenanceReport");
        }

        [HttpPost]
        public JsonNetResult AllowObservationsExport()
        {
            var resultCalculator = new SpeciesObservationResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
            var maxObservations = ConfigurationHelper.GetValue("MaxObservationCount", 50000);
            var observationCount = resultCalculator.GetSpeciesObservationsCount();
            var allowExport = true;
            string message = null;

            if (observationCount > maxObservations)
            {
                allowExport = false;
                message = string.Format(Resource.ResultSpeciesObservationsExceedMaxForExport, maxObservations);
            }
            else if (observationCount == 0)
            {
                allowExport = false;
                message = Resource.InformationExceptionNoObservationsFound;
            }

            return new JsonNetResult(new { allowExport, message });
        }

        /// <summary>
        /// Gets observations as GeoJSON so the result can be viewed on a map.
        /// The result is based on the current settings.
        /// </summary>
        /// <returns>The result in form of a GeoJson object.</returns>
        public JsonNetResult GetObservationsAsGeoJSON()
        {                       
            JsonModel jsonModel;
            SpeciesObservationResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new SpeciesObservationResultCalculator(GetCurrentUser(), SessionHandler.MySettings);                
                CoordinateSystemId displayCoordinateSystemId = SessionHandler.MySettings.Presentation.Map.DisplayCoordinateSystem.Id;
                var data = resultCalculator.GetSpeciesObservations(displayCoordinateSystemId);
                SpeciesObservationsGeoJsonModel model = SpeciesObservationsGeoJsonModel.CreateResult(data.SpeciesObservationList);
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);            
        }

        /// <summary>
        /// The get observations grid count as JSON.
        /// </summary>
        /// <returns>
        /// The <see cref="JsonNetResult"/>.
        /// </returns>
        public JsonNetResult GetObservationsGridCountAsJSON()
        {
            JsonModel jsonModel;
            SpeciesObservationGridCalculator resultCalculator = null;
            SpeciesObservationGridResult result = null;
            try
            {
                resultCalculator = new SpeciesObservationGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
                result = resultCalculator.GetSpeciesObservationGridResultFromCacheIfAvailableOrElseCalculate();
                jsonModel = JsonModel.CreateFromObject(result);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        public JsonNetResult GetObservationsGridCountExAsJSON(double bottom, double left, double right, double top, int zoom, int? gridSize)
        {
            JsonModel jsonModel;
            SpeciesObservationGridCalculator resultCalculator = null;            
            try
            {
                resultCalculator = new SpeciesObservationGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
                SpeciesObservationGridResult model = resultCalculator.CalculateSpeciesObservationGridEx(bottom, left, right, top, zoom, gridSize);
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Gets the number of species observations from manager class that matches selected settings and method 
        /// name that has been used.
        /// </summary>
        /// <returns>JsonNetResult to View i.e. returning a List(KeyValuePair(string, string)).
        /// Method name as Key and no of observations as Value.</returns>
        public JsonNetResult GetObservationsSummaryCountAsJSON()
        {
            JsonModel jsonModel;
            SummaryStatisticsResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new SummaryStatisticsResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
                List<KeyValuePair<string, string>> result = resultCalculator.GetSummaryStatisticsFromCacheIfAvailableOrElseCalculate();
                jsonModel = JsonModel.CreateFromObject(result);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Get the species observation provenances from manager class that matches selected settings and method 
        /// name that has been used.
        /// </summary>
        /// <returns>JsonNetResult to View i.e. returning a List.</returns>
        public JsonNetResult GetSpeciesObservationProvenancesAsJSON()
        {
            JsonModel jsonModel;
            SpeciesObservationProvenanceResultCalculator provenenceResultCalculator = null;
            try
            {
                provenenceResultCalculator = new SpeciesObservationProvenanceResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
                List<SpeciesObservationProvenance> result = provenenceResultCalculator.GetSpeciesObservationProvenances();
                jsonModel = JsonModel.CreateFromObject(result);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        public ContentResult GetSpeciesObservationAOOEOOAsGeoJson(int? alphaValue, bool? useCenterPoint)
        {
            string layerName;
            var parameters = new Dictionary<string, object>()
            {
                { "alphaValue", alphaValue ?? 0 },
                { "useCenterPoint", useCenterPoint ?? true }
            };
            var geoJson = MapLayerManager.GetLayerGeojson(
                GetCurrentUser(), 
                MapLayerManager.EooConcaveHullLayerId, 
                SessionHandler.MySettings.Presentation.Map.DisplayCoordinateSystem.Id, 
                parameters, 
                out layerName, 
                null);

            return Content(geoJson, "application/json");
        }

        [HttpPost]
        public JsonNetResult AddMapLayerToCompare(string features, string style)
        {            
            //FeatureCollection featureCollection = JsonConvert.DeserializeObject(features, typeof(FeatureCollection)) as FeatureCollection;

            JsonModel jsonModel = JsonModel.CreateSuccess("ok");
            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// The get paged observation list as JSON.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <returns>
        /// The <see cref="JsonNetResult"/>.
        /// </returns>
        public JsonNetResult GetPagedObservationListAsJSON(int page, int start, int limit)
        {
            JsonModel jsonModel;
            PagedSpeciesObservationResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new PagedSpeciesObservationResultCalculator(GetCurrentUser(), SessionHandler.MySettings);                
                long totalCount = resultCalculator.GetTotalCount();
                List<Dictionary<string, string>> result = resultCalculator.GetTablePagedResult(start + 1, limit);
                jsonModel = JsonModel.CreateFromObject(result);
                jsonModel.Total = totalCount;
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return new JsonNetResult(jsonModel, serializerSettings);
        }

        // kanske skicka med total också???
        public JsonNetResult GetPagedObservationListInBboxAsJSON(
            int page, 
            int start, 
            int limit, 
            double bottom, 
            double left,
            double right, 
            double top, 
            long? total)
        {
            JsonModel jsonModel;
            PagedSpeciesObservationResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new PagedSpeciesObservationResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
                long totalCount = resultCalculator.GetTotalBboxCount(bottom, left, right, top);
                
                List<ResultObservationsListItem> items = resultCalculator.GetTablePagedResultInBboxAsItems(start + 1, limit, bottom, left, right, top);
                jsonModel = JsonModel.CreateFromObject(items);

                //List<Dictionary<string, string>> result = resultCalculator.GetTablePagedResultInBbox(start + 1, limit, bottom, left, right, top);
                //jsonModel = JsonModel.CreateFromObject(result);
                jsonModel.Total = totalCount;
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return new JsonNetResult(jsonModel, serializerSettings);            
        }

        public JsonNetResult GetCompareMapLayer()
        {
            JsonModel jsonModel;
            
            try
            {
                CompareMapLayerResult compareMapLayerResult = CompareMapLayerResult.CreateSampleData();
                jsonModel = JsonModel.CreateFromObject(compareMapLayerResult);                
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// The get paged observations as geo JSON.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <returns>
        /// The <see cref="JsonNetResult"/>.
        /// </returns>
        public JsonNetResult GetPagedObservationsAsGeoJSON(int page, int start, int limit, int? taxonId)
        {
            JsonModel jsonModel;
            PagedSpeciesObservationResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new PagedSpeciesObservationResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
                long totalCount = resultCalculator.GetTotalCount(taxonId);
                SpeciesObservationsGeoJsonModel result = resultCalculator.GetMapPagedResult(start + 1, limit, taxonId);
                jsonModel = JsonModel.CreateFromObject(result.Points.Features);
                jsonModel.Total = totalCount;
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// The get observation data.
        /// </summary>
        /// <param name="observationId">
        /// The observation id.
        /// </param>
        /// <returns>
        /// The <see cref="JsonNetResult"/>.
        /// </returns>
        public JsonNetResult GetObservationData(int observationId, int? importance, bool? showDwcTitle, bool? hideEmptyFields)
        {
            JsonModel jsonModel;
            try
            {                                
                ObservationDetailViewManager manager = new ObservationDetailViewManager(GetCurrentUser(), SessionHandler.MySettings);
                var model = manager.CreateObservationDetailsViewModel(observationId.ToString(), false);
                //if (showDwcTitle.GetValueOrDefault(!SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.UseLabelAsColumnHeader))
                //{
                //    foreach (var field in model.Fields)
                //    {
                //        field.Label = field.Name;
                //    }
                //}

                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            var serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return new JsonNetResult(jsonModel, serializerSettings);
        }

        /// <summary>
        /// Gets the number of species observations from manager class that matches selected settings and method 
        /// name that has been used.
        /// </summary>
        /// <param name="periodicityId">
        /// The periodicity Id.
        /// </param>
        /// <returns>
        /// JsonNetResult to View i.e. returning a List(KeyValuePair(string, string)).
        /// Method name as Key and no of observations as Value.
        /// </returns>
        public JsonNetResult GetObservationsDiagramAsJSON(int periodicityId)
        {
            JsonModel jsonModel;
            SpeciesObservationDiagramResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new SpeciesObservationDiagramResultCalculator(GetCurrentUser(), SessionHandler.MySettings);

                // List<KeyValuePair<string, string>> result = resultCalculator.GetResult();
                List<KeyValuePair<string, string>> result = resultCalculator.GetDiagramResult(periodicityId);
                List<KeyValuePair<string, Int64>> modResult = new List<KeyValuePair<string, long>>();
                foreach (KeyValuePair<string, string> keyValuePair in result)
                {
                    string key = keyValuePair.Key;
                    Int64 value = Convert.ToInt64(keyValuePair.Value);
                    KeyValuePair<string, Int64> newKeyValuePair = new KeyValuePair<string, long>(key, value);                   
                    modResult.Add(newKeyValuePair);
                }

                jsonModel = JsonModel.CreateFromObject(modResult);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Gets the number of species observations from manager class that matches selected settings and method 
        /// name that has been used. One taxon has to be supplied.
        /// </summary>
        /// <param name="periodicityId">
        /// The periodicity Id.
        /// </param>
        /// <param name="taxonId">
        /// The taxon Id.
        /// </param>
        /// <returns>
        /// JsonNetResult to View i.e. returning a List(KeyValuePair(string, string)).
        /// Method name as Key and no of observations as Value.
        /// </returns>
        public JsonNetResult GetObservationsAbundanceIndexDiagramAsJSON(int periodicityId, int taxonId)
        {
            JsonModel jsonModel;
            SpeciesObservationAbundanceIndexDiagramResultCalculator resultCalculator = null;
            try
            {
                resultCalculator = new SpeciesObservationAbundanceIndexDiagramResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
                
                List<KeyValuePair<string, string>> result = resultCalculator.GetAbundanceIndexDiagramResult(periodicityId, taxonId);
                List<KeyValuePair<string, object>> modResult = new List<KeyValuePair<string, object>>();

                foreach (KeyValuePair<string, string> keyValuePair in result)
                {
                    string key = keyValuePair.Key;
                    object value;

                    if (string.IsNullOrEmpty(keyValuePair.Value))
                    {
                        value = false;
                    }
                    else
                    {
                        value = Convert.ToDouble(keyValuePair.Value);
                    }

                    KeyValuePair<string, object> newKeyValuePair = new KeyValuePair<string, object>(key, value);

                    modResult.Add(newKeyValuePair);
                }

                jsonModel = JsonModel.CreateFromObject(modResult);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// The get WFS layers as JSON.
        /// </summary>
        /// <returns>
        /// The <see cref="JsonNetResult"/>.
        /// </returns>
        public JsonNetResult GetWfsLayersAsJSON()
        {
            JsonModel jsonModel;
            try
            {
                var viewManager = new WfsLayersViewManager(GetCurrentUser(), SessionHandler.MySettings);                
                
                // WfsLayersViewModel model = new WfsLayersViewModel();
                List<WfsLayerViewModel> model = viewManager.GetWfsLayers();
                jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);      
        }

        /// <summary>
        /// Gets the number of observations and the number of species per polygon in the chosen layer.
        /// </summary>
        /// <returns>The result in form of a Json object.</returns>
        public JsonNetResult GetSummaryStatisticsPerPolygonAsJSON()
        {
            JsonModel jsonModel;
            SummaryStatisticsPerPolygonResultCalculator resultCalculator = null;

            try
            {
                resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(GetCurrentUser(), SessionHandler.MySettings);
                List<SpeciesObservationsCountPerPolygon> result = resultCalculator.GetSummaryStatisticsPerPolygonFromCacheIfAvailableOrElseCalculate();
                jsonModel = JsonModel.CreateFromObject(result);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }         

        /// <summary>
        /// Creates a map based on county occurrence information from Artfakta.
        /// </summary>
        /// <param name="id">Dyntaxa TaxonId.</param>
        /// <returns>A PNG file.</returns>
        public FileResult SpeciesObservationHeatMapImage(int? id)
        {
            try
            {
                var returnStream = new System.IO.MemoryStream();
                var map = CreateSpeciesObservationHeatMapImage(null, SessionHandler.MySettings);

                map.Save(returnStream, System.Drawing.Imaging.ImageFormat.Png);
                returnStream.Position = 0;

                return File(returnStream, "image/png", "SpeciesObservationHeatMapImage.png");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a species richness map based on county occurrence information from Artfakta.
        /// </summary>
        /// <param name="id">Dyntaxa TaxonId.</param>
        /// <returns>A PNG file.</returns>
        public FileResult SpeciesRichnessHeatMap(int? id)
        {
            try
            {
                var gridCalculator = new TaxonGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
                var countList = gridCalculator.CalculateTaxonGrid(id, GridCoordinateSystem.Rt90_25_gon_v);

                var gridCells = countList.Cast<IGridCellBase>().ToList();
                Func<IGridCellBase, Int64> countMethod = x => ((IGridCellSpeciesCount)x).SpeciesCount;
                var countMap = new MapImage { Height = 1600 };
                var map = countMap.GetHeatMap(gridCells, countMethod);
                var returnStream = new System.IO.MemoryStream();
                map.Save(returnStream, System.Drawing.Imaging.ImageFormat.Png);
                returnStream.Position = 0;
                return new FileStreamResult(returnStream, "image/png");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a map based on county occurrence information from Artfakta.
        /// </summary>
        /// <param name="id">Dyntaxa TaxonId.</param>
        /// <returns>A PNG file.</returns>
        [IndexedBySearchRobots]
        public FileResult SpeciesObservationPointMap(int id)
        {
            try
            {
                System.Drawing.Bitmap map = null;
                var returnStream = new System.IO.MemoryStream();
                var gridCalculator = new SpeciesObservationGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
                IList<IGridCellSpeciesObservationCount> countList = gridCalculator.CalculateSpeciesObservationGrid(id, GridCoordinateSystem.Rt90_25_gon_v);
                SpeciesObservationPointMap countMap = new SpeciesObservationPointMap(countList);
                countMap.Height = 1600;
                map = countMap.Bitmap;
                map.Save(returnStream, System.Drawing.Imaging.ImageFormat.Png);
                returnStream.Position = 0;
                return new FileStreamResult(returnStream, "image/png");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The get taxon grid count as JSON.
        /// </summary>
        /// <returns>
        /// The <see cref="JsonNetResult"/>.
        /// </returns>
        public JsonNetResult GetTaxonGridCountAsJSON()
        {
            JsonModel jsonModel;
            TaxonGridCalculator resultCalculator = null;
            try
            {
                resultCalculator = new TaxonGridCalculator(GetCurrentUser(), SessionHandler.MySettings);
                TaxonGridResult model = resultCalculator.GetTaxonGridResultFromCacheIfAvailableOrElseCalculate();
                jsonModel = JsonModel.CreateFromObject(model);

                // IUserContext user = GetCurrentUser();
                // var resultsManager = new ResultManager(user);
                // TaxonGridResult model = resultsManager.GetTaxonGridCellResult();
                // jsonModel = JsonModel.CreateFromObject(model);
            }
            catch (Exception ex)
            {                
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }

            return new JsonNetResult(jsonModel);
        }

        /// <summary>
        /// Gets a MySettings summary report.
        /// </summary>
        /// <returns>Settings report view.</returns>
        public ActionResult SettingsReport()
        {
            var model = new MySettingsReportViewModel(GetCurrentUser());

            return View(model);
        }

        public PartialViewResult HistogramBinSettingsDialog()
        {
            // var viewManager = new DataProvidersViewManager(GetCurrentUser(), SessionHandler.MySettings);
            // DataProviderViewModel model = viewManager.CreateDataProviderViewModel(id);
            return PartialView();
        }

        public PartialViewResult HistogramBinSettingsPartial()
        {
            return PartialView();
        }

#if DEBUG
        [IgnorePageInfoManager]
        public ViewResult HistogramBinSettings()
        {
            return View();
        }
#endif
    }
}
