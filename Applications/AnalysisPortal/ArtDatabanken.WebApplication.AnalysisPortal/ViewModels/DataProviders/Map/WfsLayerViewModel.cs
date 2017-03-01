using Resources;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map
{
    /// <summary>
    /// This class is a view model for a WFS layer stored in MySettings
    /// </summary>
    public class WfsLayerViewModel
    {
        //public int RowId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ServerUrl { get; set; }
        public string TypeName { get; set; }
        public string GeometryName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public GeometryType GeometryType { get; set; }
        public string Filter { get; set; }
        public string Color { get; set; }
        public bool UseSpatialFilterExtentAsBoundingBox { get; set; }
        public string MediaName { get; set; }
        public bool IsFile { get; set; }
        //public string EditUrl { get; set; }

        public static WfsLayerViewModel Create(WfsLayerSetting wfsLayerSetting)
        {
            var layer = new WfsLayerViewModel
            {
                Id = wfsLayerSetting.Id,
                Name = wfsLayerSetting.Name,
                ServerUrl = wfsLayerSetting.ServerUrl,
                TypeName = wfsLayerSetting.TypeName,
                GeometryName = wfsLayerSetting.GeometryName,
                GeometryType = wfsLayerSetting.GeometryType,
                Filter = wfsLayerSetting.Filter,
                Color = wfsLayerSetting.Color,
                UseSpatialFilterExtentAsBoundingBox = wfsLayerSetting.UseSpatialFilterExtentAsBoundingBox,
                MediaName = wfsLayerSetting.IsFile ? Resource.SharedFile : Resource.SharedWfsService,
                IsFile = wfsLayerSetting.IsFile
            };
            
            return layer;
        }
    }
}
