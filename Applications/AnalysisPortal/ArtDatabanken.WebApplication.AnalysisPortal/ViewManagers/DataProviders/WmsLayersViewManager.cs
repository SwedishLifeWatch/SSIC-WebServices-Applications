using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.WFS;
using ArtDatabanken.GIS.WFS.Capabilities;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders
{
    /// <summary>
    /// This class is a view manager class for WFS actions in DataProviders controller
    /// </summary>
    public class WmsLayersViewManager : ViewManagerBase
    {
        public MapLayersSetting MapLayersSetting
        {
            get { return MySettings.DataProvider.MapLayers; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WfsLayersViewManager"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mySettings">MySettings.</param>
        public WmsLayersViewManager(IUserContext user, AnalysisPortal.MySettings.MySettings mySettings)
            : base(user, mySettings)
        {            
        }

        public WmsLayerSetting CreateNewWmsLayer(WmsLayerViewModel wmsLayerViewModel)
        {            
            var wmsLayer = new WmsLayerSetting();
            wmsLayer.Name = wmsLayerViewModel.Name.Trim();

            var uriBuilder = new UriBuilder(wmsLayerViewModel.ServerUrl);                  
            string baseUrl = uriBuilder.Uri.GetLeftPart(UriPartial.Path);
            wmsLayer.ServerUrl = baseUrl;

            if (wmsLayerViewModel.Layers != null)
            {
                wmsLayer.Layers = wmsLayerViewModel.Layers;
            }
            else
            {
                wmsLayer.Layers = new List<string>();
            }

            if (wmsLayerViewModel.SupportedCoordinateSystems != null)
            {
                wmsLayer.SupportedCoordinateSystems = wmsLayerViewModel.SupportedCoordinateSystems;
            }
            else
            {
                wmsLayer.SupportedCoordinateSystems = new List<string>();
            }

            wmsLayer.IsBaseLayer = wmsLayerViewModel.IsBaseLayer;
            MapLayersSetting.AddWmsLayer(wmsLayer);
            return wmsLayer;
        }

        //public void AddWmsLayer(WmsLayerSetting wmsLayer)
        //{
        //    MapLayersSetting.AddWmsLayer(wmsLayer);
        //}

        public List<WmsLayerViewModel> GetWmsLayers()
        {            
            var layers = new List<WmsLayerViewModel>();
            foreach (WmsLayerSetting wmsLayerSetting in MapLayersSetting.WmsLayers)
            {
                WmsLayerViewModel layer = WmsLayerViewModel.Create(wmsLayerSetting);                
                layers.Add(layer);
            }
            return layers;
        }

        public void RemoveWmsLayer(int id)
        {
            MapLayersSetting.RemoveWmsLayer(id);
        }

        /// <summary>
        /// Remove WMS layers.
        /// </summary>
        /// <param name="ids">The ids.</param>
        public void RemoveWmsLayers(int[] ids)
        {
            MapLayersSetting.RemoveWmsLayers(ids);
        }

        public WmsLayerEditorViewModel CreateWmsLayerEditorViewModel(WmsLayerEditorMode? mode, int? id)
        {
            var model = new WmsLayerEditorViewModel
            {
                Id = id, 
                Mode = mode ?? WmsLayerEditorMode.New
            };

            if (model.Mode == WmsLayerEditorMode.Edit && id.HasValue)
            {
                model.WmsLayerViewModel = WmsLayerViewModel.Create(MapLayersSetting.GetWmsLayer(id.Value));
            }      
                  
            return model;
        }

        public void UpdateWmsLayer(int id, WmsLayerViewModel wmsLayerViewModel)
        {
            WmsLayerSetting wmsLayerSetting = MapLayersSetting.GetWmsLayer(id);
            wmsLayerSetting.Name = wmsLayerViewModel.Name;
            wmsLayerSetting.IsBaseLayer = wmsLayerViewModel.IsBaseLayer;
            wmsLayerSetting.ServerUrl = wmsLayerViewModel.ServerUrl;
            wmsLayerSetting.Layers = wmsLayerViewModel.Layers;
            wmsLayerSetting.SupportedCoordinateSystems = wmsLayerViewModel.SupportedCoordinateSystems;
        }

        public WmsLayersViewModel CreateWmsLayersViewModel()
        {
            var model = new WmsLayersViewModel();
            model.IsSettingsDefault = MapLayersSetting.IsWmsSettingsDefault();
            return model;            
        }
    }
}
