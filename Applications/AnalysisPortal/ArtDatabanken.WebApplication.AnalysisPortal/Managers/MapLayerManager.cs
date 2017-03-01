using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.WFS.Filter.Formula;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result;
using Newtonsoft.Json;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    public static class MapLayerManager
    {
        public const string YellowHexCode = "#ffff00";
        public const int EooConvexHullLayerId = -100;
        public const int EooConcaveHullLayerId = -101;
        public const int ObservationsLayerId = -102;
        public const int SpeciesRichnessGridLayerId = -1;
        public const int SpeciesObservationGridMapLayerId = -2;
        public const int SpeciesObservationClusterPointMapLayerId = -3;
        public const int CustomLayersStartLayerId = 0;

        public static string GetLayerGeojson(
            IUserContext currentUser, 
            int layerId, 
            CoordinateSystemId coordinateSystemId, 
            IDictionary<string, object> parameters, 
            out string layerName, 
            MapExportModel.Extent mapExtent)
        {
            string geoJson = null;
            layerName = null;

            if (layerId == null)
            {
                return null;
            }

            if (layerId >= CustomLayersStartLayerId)
            {
                var viewManager = new WfsLayersViewManager(currentUser, SessionHandler.MySettings);

                var layer = viewManager.GetWfsLayers().FirstOrDefault(l => l.Id == layerId);
                layerName = layer.Name;

                if (layer.IsFile)
                {
                    geoJson = JsonConvert.SerializeObject(MySettingsManager.GetMapDataFeatureCollection(currentUser, layer.GeometryName, coordinateSystemId));
                }
                else
                {
                    var url = WFSFilterUtils.GetResultingUrl(layer.ServerUrl, layer.TypeName, "1.1.0", layer.Filter, "application%2Fjson", null, string.Format("EPSG%3A{0}", coordinateSystemId.Srid()));

                    var request = WebRequest.Create(url);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    // Get the response.

                    using (var response = request.GetResponse())
                    {
                        using (var dataStream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(dataStream))
                            {
                                geoJson = reader.ReadToEnd();
                                reader.Close();
                            }
                        }
                        response.Close();
                    }
                }
            }
            else
            {
                switch (layerId)
                {
                    case SpeciesRichnessGridLayerId:
                        var taxonGridCalculator = new TaxonGridCalculator(currentUser, SessionHandler.MySettings);
                        geoJson = taxonGridCalculator.GetTaxonGridAsGeoJson();

                        var attribute = (string)(parameters.ContainsKey("attribute") ? parameters["attribute"] : null);

                        layerName = Resource.ResultViewSpeciesRichnessGridMapLayerName;
                        if (attribute != null)
                        {
                            switch (attribute.ToLower().Trim())
                            {
                                case "speciescount":
                                    layerName = Resource.ResultDownloadSpeciesRichnessGridMap;
                                    break;
                                case "observationcount":
                                    layerName = Resource.ResultDownloadObservationsGridMap;
                                    break;
                            }
                        }

                        break;                   
                    case SpeciesObservationGridMapLayerId:
                    case EooConvexHullLayerId:
                    case EooConcaveHullLayerId:
                        var speciesObservationGridCalculator = new SpeciesObservationGridCalculator(currentUser, SessionHandler.MySettings);
                        if (layerId == SpeciesObservationGridMapLayerId)
                        {
                            geoJson = speciesObservationGridCalculator.GetSpeciesObservationGridAsGeoJson();
                            layerName = Resource.ResultViewSpeciesObservationGridMap;
                        }
                        else
                        {
                            var alphaValue = (int?)(parameters.ContainsKey("alphaValue") ? parameters["alphaValue"] : null);
                            var useCenterPoint = (bool?)(parameters.ContainsKey("useCenterPoint") ? parameters["useCenterPoint"] : null);
                            geoJson = speciesObservationGridCalculator.GetSpeciesObservationAOOEOOAsGeoJson(
                                layerId == EooConcaveHullLayerId ? alphaValue : 0, 
                                useCenterPoint ?? true);
                            layerName = Resource.MapEOOLayer;
                        }

                        break;
                    case ObservationsLayerId: //Observations
                    case SpeciesObservationClusterPointMapLayerId:
                        SpeciesObservationResultCalculator resultCalculator = null;
                        try
                        {
                            var displayCoordinateSystemId = SessionHandler.MySettings.Presentation.Map.PresentationCoordinateSystemId;
                            resultCalculator = new SpeciesObservationResultCalculator(currentUser, SessionHandler.MySettings);
                            geoJson = resultCalculator.GetSpeciesObservationsAsGeoJson(displayCoordinateSystemId);
                            layerName = Resource.MapLayerObservations;
                        }
                        catch (Exception)
                        {                            
                        }
                        break;
                }
            }

            return geoJson;
        }

        /// <summary>
        /// Sets custom layer properties for some of the layers.
        /// </summary>
        /// <param name="layer">The layer.</param>        
        public static void SetCustomLayerProperties(MapExportModel.Layer layer)
        {            
            switch (layer.Id)
            {
                case SpeciesObservationClusterPointMapLayerId: // Species observations (grid based)
                    layer.Legends[0].Color = YellowHexCode;
                    break;                    
            }            
        }
    }
}