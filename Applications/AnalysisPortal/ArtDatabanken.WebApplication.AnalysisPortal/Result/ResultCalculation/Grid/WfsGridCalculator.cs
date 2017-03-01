using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.QueryComplexity;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Newtonsoft.Json.Linq;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid
{
    public class WfsGridCalculator : ResultCalculatorBase
    {
        public WfsGridCalculator(IUserContext userContext, MySettings.MySettings mySettings) 
            : base(userContext, mySettings)
        {
        }

        public WfsStatisticsGridResult CalculateGridResult(IUserContext userContext, int coordinateSystemId, int gridSize, int wfsLayerId)
        {
            var requestedGridCoordinateSystem = (GridCoordinateSystem)coordinateSystemId;
            var gridSpecification = new GridSpecification
            {
                GridCoordinateSystem = requestedGridCoordinateSystem,
                GridCellSize = gridSize,
                IsGridCellSizeSpecified = true,
                GridCellGeometryType = GridCellGeometryType.Polygon
            };

            if (MySettings.Filter.Spatial.IsActive)
            {
                // Create bounding box from spatial filter
               var polygons = MySettings.Filter.Spatial.Polygons;
                if (polygons.Count > 0)
                {
                    var boundingBox = polygons.GetBoundingBox();                    
                    var toCoordinateSystem = CoordinateSystemHelper.GetCoordinateSystemFromGridCoordinateSystem(gridSpecification.GridCoordinateSystem);
                    var convertedBoundingBoxPolygon = GisTools.CoordinateConversionManager.GetConvertedBoundingBox(boundingBox, MySettings.Filter.Spatial.PolygonsCoordinateSystem, toCoordinateSystem);
                    gridSpecification.BoundingBox = convertedBoundingBoxPolygon.GetBoundingBox();
                }
            }

            var displayCoordinateSystem = MySettings.Presentation.Map.DisplayCoordinateSystem;            
            var wfsLayer = SessionHandler.MySettings.DataProvider.MapLayers.WfsLayers.FirstOrDefault(l => l.Id == wfsLayerId);
            string featuresUrl = null;
            string featureCollectionJson = null;

            if (wfsLayer.IsFile)
            {
                featureCollectionJson = JObject.FromObject(MySettingsManager.GetMapDataFeatureCollection(userContext, wfsLayer.GeometryName, requestedGridCoordinateSystem.GetCoordinateSystemId())).ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(wfsLayer.Filter))
                {
                    featuresUrl = string.Format("{0}?service=wfs&version=1.1.0&request=GetFeature&typeName={1}", wfsLayer.ServerUrl, wfsLayer.TypeName);
                }
                else
                {
                    featuresUrl = string.Format("{0}?service=wfs&version=1.1.0&request=GetFeature&typeName={1}&filter={2}", wfsLayer.ServerUrl, wfsLayer.TypeName, wfsLayer.Filter);
                }
            }

            var list = CoreData.AnalysisManager.GetGridFeatureStatistics(UserContext, null, featuresUrl, featureCollectionJson, gridSpecification, displayCoordinateSystem);
            
            return WfsStatisticsGridResult.Create(list);
        }

        public QueryComplexityEstimate GetQueryComplexityEstimate()
        {
            return new QueryComplexityEstimate();
        }
    }
}