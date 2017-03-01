using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Converters;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GisUtils;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters
{
    /// <summary>
    /// This class is a view manager for handling spatial filter operations using the MySettings object.
    /// </summary>
    public class SpatialFilterViewManager : ViewManagerBase
    {
        public SpatialSetting SpatialSetting
        {
            get { return MySettings.Filter.Spatial; }
        }

        public PresentationMapSetting MapSettings
        {
            get { return MySettings.Presentation.Map; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialFilterViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public SpatialFilterViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        #region Regions

        /// <summary>
        /// Gets all spatial filter regions.
        /// </summary>
        /// <returns></returns>
        public List<RegionViewModel> GetAllRegions()
        {
            List<int> regionIds = MySettings.Filter.Spatial.RegionIds.ToList();
            if (regionIds.Count == 0)
            {
                return new List<RegionViewModel>();
            }
            RegionList regions = CoreData.RegionManager.GetRegionsByIds(UserContext, regionIds);            
            return (from IRegion region in regions select RegionViewModel.CreateFromRegion(region)).ToList();
        }

        /// <summary>
        /// Gets a region.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public RegionViewModel GetRegion(int id)
        {
            RegionList regions = CoreData.RegionManager.GetRegionsByIds(UserContext, new List<int> { id });
            return RegionViewModel.CreateFromRegion(regions[0]);            
        }

        /// <summary>
        /// Adds a region to spatial filter regions.
        /// </summary>
        /// <param name="id">The id.</param>
        public void AddRegion(int id)
        {
            MySettings.Filter.Spatial.AddRegion(id);
        }

        /// <summary>
        /// Adds regions to spatial filter regions.
        /// </summary>
        /// <param name="ids">The ids.</param>
        public void AddRegions(IEnumerable<int> ids)
        {
            MySettings.Filter.Spatial.AddRegions(ids);
        }

        /// <summary>
        /// Removes a region from spatial filter regions.
        /// </summary>
        /// <param name="id">The id.</param>
        public void RemoveRegion(int id)
        {
            MySettings.Filter.Spatial.RemoveRegion(id);
        }

        /// <summary>
        /// Removes regions from spatial filter regions.
        /// </summary>
        /// <param name="ids">The ids.</param>
        public void RemoveRegions(IEnumerable<int> ids)
        {
            MySettings.Filter.Spatial.RemoveRegions(ids);
        }

        /// <summary>
        /// Resets the regions.
        /// </summary>
        public void ResetRegions()
        {
            MySettings.Filter.Spatial.ResetRegions();
        }

        #endregion

        /// <summary>
        /// Updates the spatial filter.
        /// </summary>
        /// <param name="featureCollection">The feature collection.</param>
        public void UpdateSpatialFilter(FeatureCollection featureCollection)
        {
            List<DataPolygon> dataPolygons = DataPolygonConverter.ConvertToDataPolygons(featureCollection);            
            SpatialSetting.SetPolygons(dataPolygons);
        }

        /// <summary>
        /// Updates the spatial filter. All coordinates will be converted to Google Mercator.
        /// </summary>
        /// <param name="featureCollection">The feature collection.</param>
        /// <param name="coordinateSystem">The coordinate system used in featureCollection.</param>
        public void UpdateSpatialFilter(FeatureCollection featureCollection, CoordinateSystem coordinateSystem)
        {
            List<DataPolygon> dataPolygons = DataPolygonConverter.ConvertToDataPolygons(featureCollection);
            if (coordinateSystem.Id != CoordinateSystemId.GoogleMercator)
            {
                DataContext dataContext = new DataContext(UserContext);
                List<IPolygon> polygons = dataPolygons.ToPolygons(dataContext);
                List<IPolygon> convertedPolygons = GisTools.CoordinateConversionManager.GetConvertedPolygons(polygons, coordinateSystem, new CoordinateSystem(CoordinateSystemId.GoogleMercator));
                dataPolygons = DataPolygonConverter.ConvertToDataPolygons(convertedPolygons);
            }

            SpatialSetting.SetPolygons(dataPolygons);
        }

        //public void ValidateFeatureCollection(FeatureCollection featureCollection)
        //{
        //    //GeometryTools geometryTools = new GeometryTools();
        //    //bool isValid = true;

        //    //FeatureCollectionValidation featureCollectionValidation = new FeatureCollectionValidation
        //    //{
        //    //    IsValid = true,
        //    //    InvalidFeatures = new List<FeatureValidationResult>()
        //    //};

        //    //foreach (Feature feature in featureCollection.Features)
        //    //{
        //    //    GeometryValidationResult geometryValidationResult = geometryTools.CheckGeometryValidity(feature);
        //    //    if (!geometryValidationResult.IsValid)
        //    //    {
        //    //        featureCollectionValidation.IsValid = false;
        //    //        featureCollectionValidation.InvalidFeatures.Add(new FeatureValidationResult
        //    //        {
        //    //            Feature = feature,
        //    //            ValidationResult = geometryValidationResult
        //    //        });
        //    //    }
        //    //}

        //    //if (!featureCollectionValidation.IsValid)
        //    //{
        //    //    throw new Exception("Polygon error!");
        //    //}
        //}

        public void ClearSpatialFilter()
        {
            SpatialSetting.Polygons.Clear();
        }

        public FeatureCollection GetSpatialFilterAsFeatureCollection()
        {
            FeatureCollection featureCollection = GeoJSONConverter.ConvertToGeoJSONFeatureCollection(SpatialSetting.Polygons.ToList());
            return featureCollection;
        }

        public int AddPolygons(FeatureCollection featureCollection)
        {
            List<DataPolygon> dataPolygons = DataPolygonConverter.ConvertToDataPolygons(featureCollection);
            if (dataPolygons.Count > 0)
            {                
                foreach (var dataPolygon in dataPolygons)
                {
                    SpatialSetting.Polygons.Add(dataPolygon);
                }
                SpatialSetting.IsActive = true;
            }
            return dataPolygons.Count;
        }

        public PolygonFromMapLayerViewModel CreatePolygonFromMapLayerViewModel()
        {
            var model = new PolygonFromMapLayerViewModel();
            model.IsSettingsDefault = SpatialSetting.IsPolygonSettingsDefault();
            return model;
        }

        public FilterSpatialViewModel CreateFilterSpatialViewModel()
        {
            FilterSpatialViewModel model = new FilterSpatialViewModel();
            model.IsSettingsDefault = SpatialSetting.IsSettingsDefault();
            return model;
        }

        public FeatureCollection GetSpatialFilterBboxAsFeatureCollection()
        {
            BoundingBox boundingBox = SpatialSetting.Polygons.GetBoundingBox();
            if (boundingBox != null)
            {
                Polygon boundingPolygon = GisTools.CoordinateConversionManager.GetConvertedBoundingBox(boundingBox, SpatialSetting.PolygonsCoordinateSystem, MapSettings.DisplayCoordinateSystem);
                boundingBox = boundingPolygon.GetBoundingBox();
            }

            FeatureCollection featureCollection = GeoJSONConverter.ConvertToGeoJSONFeatureCollection(boundingBox);
            return featureCollection;
        }
    }
}
