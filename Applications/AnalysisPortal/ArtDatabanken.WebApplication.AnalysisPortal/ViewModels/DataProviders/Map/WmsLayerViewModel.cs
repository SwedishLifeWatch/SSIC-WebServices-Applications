using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map
{
    public class WmsLayerViewModel
    {
        public int Id { get; set; }
        public string ServerUrl { get; set; }
        public string Name { get; set; }
        public List<string> Layers { get; set; }        
        public List<string> SupportedCoordinateSystems { get; set; }
        public bool IsBaseLayer { get; set; }

        public static WmsLayerViewModel Create(WmsLayerSetting wmsLayerSetting)
        {
            WmsLayerViewModel model = new WmsLayerViewModel();
            model.Id = wmsLayerSetting.Id;
            model.Name = wmsLayerSetting.Name;
            model.ServerUrl = wmsLayerSetting.ServerUrl;
            model.Layers = wmsLayerSetting.Layers;            
            model.IsBaseLayer = wmsLayerSetting.IsBaseLayer;
            model.SupportedCoordinateSystems = wmsLayerSetting.SupportedCoordinateSystems;
            return model;
        }
    }
}
