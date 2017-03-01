using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.WFS;
using ArtDatabanken.GIS.WFS.Capabilities;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Shared;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders
{
    /// <summary>
    /// This class is a view manager class for WFS actions in DataProviders controller
    /// </summary>
    public class WfsLayersViewManager : ViewManagerBase
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
        public WfsLayersViewManager(IUserContext user, AnalysisPortal.MySettings.MySettings mySettings)
            : base(user, mySettings)
        {            
        }
        
        /// <summary>
        /// Creates a list with the polygon WFS layers stored in MySettings.
        /// </summary>
        /// <returns></returns>
        public List<WfsLayerViewModel> CreatePolygonWfsLayersList()
        {
            var layers = new List<WfsLayerViewModel>();
            foreach (WfsLayerSetting wfsLayerSetting in MapLayersSetting.WfsLayers)
            {
                if (wfsLayerSetting.GeometryType == GeometryType.Polygon)
                {
                    WfsLayerViewModel layer = WfsLayerViewModel.Create(wfsLayerSetting);
                    layers.Add(layer);
                }
            }
            return layers;
        }

        /// <summary>
        /// Creates a list with the WFS layers stored in MySettings.
        /// </summary>
        /// <returns></returns>
        public List<WfsLayerViewModel> CreateWfsLayersList()
        {
            var layers = new List<WfsLayerViewModel>();
            foreach (WfsLayerSetting wfsLayerSetting in MapLayersSetting.WfsLayers)
            {
                WfsLayerViewModel layer = WfsLayerViewModel.Create(wfsLayerSetting);                
                layers.Add(layer);
            }
            return layers;
        }

        /// <summary>
        /// Creates a view model to use in WfsLayerEditor when we edit an existing layer.
        /// </summary>
        /// <param name="id">The layer id.</param>
        /// <returns></returns>
        public WfsLayerEditorViewModel CreateWfsLayerEditorViewModelInEditMode(int id)
        {
            var wfsLayerSetting = MapLayersSetting.GetWfsLayer(id);
            var model = new WfsLayerEditorViewModel
            {
                Id = id,
                Mode = WfsLayerEditorMode.Edit,
                WfsLayerSetting = wfsLayerSetting
            };
            
            if (wfsLayerSetting.IsFile)
            {
                model.FeatureType = new WfsFeatureType
                {
                    Title = wfsLayerSetting.Name
                };
                model.ServerUrl = wfsLayerSetting.ServerUrl;
            }
            else
            {
                var wfsCapabilities = WFSManager.GetWFSCapabilitiesAndMergeDescribeFeatureType(wfsLayerSetting.ServerUrl, wfsLayerSetting.TypeName);
                model.WfsCapabilities = wfsCapabilities;
                var featureType = wfsCapabilities.FeatureTypes.FirstOrDefault(feature => feature.Name.FullName == wfsLayerSetting.TypeName);
                model.FeatureType = featureType;
                model.ServerUrl = wfsCapabilities.Capability.Requests.GetFeaturesRequest.GetUrlBase;
            }
            
            return model;
        }

        /// <summary>
        /// Creates a view model to use in WfsLayerEditor when we create a new layer.
        /// </summary>
        /// <param name="url">The server URL.</param>
        /// <param name="typeName">The typename.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public WfsLayerEditorViewModel CreateWfsLayerEditorViewModelInCreateNewMode(string url, string typeName, string filter)
        {            
            WFSCapabilities wfsCapabilities = WFSManager.GetWFSCapabilitiesAndMergeDescribeFeatureType(url, typeName);
            ArtDatabanken.GIS.WFS.Capabilities.WfsFeatureType featureType = wfsCapabilities.FeatureTypes.FirstOrDefault(feature => feature.Name.FullName == typeName);

            var model = new WfsLayerEditorViewModel();
            model.Mode = WfsLayerEditorMode.New;
            model.WfsCapabilities = wfsCapabilities;
            model.FeatureType = featureType;
            model.ServerUrl = wfsCapabilities.Capability.Requests.GetFeaturesRequest.GetUrlBase;            
            var wfsLayerSetting = new WfsLayerSetting();
            wfsLayerSetting.Filter = "";
            wfsLayerSetting.Name = "";
            wfsLayerSetting.Color = MapLayersSetting.CreateMapLayerHexColor();
            model.WfsLayerSetting = wfsLayerSetting;

            return model;
        }

        public void RemoveWfsLayer(int id)
        {
            MapLayersSetting.RemoveWfsLayer(id);
        }

        public AddWfsLayerViewModel CreateAddWfsLayerViewModel(ModelStateDictionary modelState, string wfsUrl, string uploadUrl, string successUrl)
        {
            AddWfsLayerViewModel model;
            if (!string.IsNullOrEmpty(wfsUrl))
            {
                try
                {
                    var wfsCapabilities = WFSManager.GetWFSCapabilitiesAndMergeDescribeFeatureTypes(wfsUrl);
                    var uri = new Uri(wfsUrl);
                    var baseUrl = uri.GetLeftPart(UriPartial.Path);

                    model = AddWfsLayerViewModel.Create(baseUrl, wfsCapabilities);
                }
                catch (Exception)
                {
                    var baseUrl = new UriBuilder(wfsUrl).Uri.GetLeftPart(UriPartial.Path);
                    modelState.AddModelError("", string.Format("The server: {0}, didn't respond correct", baseUrl));
                    model = AddWfsLayerViewModel.Create(baseUrl);
                }
            }
            else
            {
                model = new AddWfsLayerViewModel();
            }

            model.UploadGeoJsonViewModel = new UploadGeoJsonViewModel(uploadUrl, successUrl)
            {
                FileFormatDescription = Resource.DataAddWfsLayeraFileUploadForm,
                FileNameRegEx = "geojson|zip",  
            };

            model.ShowFile = base.UserContext.User.PersonId != null;

            if (model.ShowFile)
            {
                var fileNames = MySettingsManager.GetSavedMapDataFiles(base.UserContext);

                if (fileNames != null)
                {
                    model.Files =
                        (from fn in fileNames
                         select new AddWfsLayerViewModel.FileViewModel
                        {
                            FileName = fn,
                            Name = fn.IndexOf(".", StringComparison.CurrentCulture) == -1
                            ? fn
                            : fn.Substring(0, fn.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase - 1))
                }).ToArray();
                }
            }
           
            return model;
        }

        /// <summary>
        /// Gets all uploaded gis files.
        /// </summary>
        /// <returns>A list with the logged in users all uploaded GIS-files.</returns>
        public List<AddWfsLayerViewModel.FileViewModel> GetAllUploadedGisFiles()
        {
            var fileNames = MySettingsManager.GetSavedMapDataFiles(base.UserContext);
            if (fileNames == null)
            {
                return new List<AddWfsLayerViewModel.FileViewModel>();
            }
            
            List<AddWfsLayerViewModel.FileViewModel> files =
                (from fn in fileNames
                    select new AddWfsLayerViewModel.FileViewModel
                    {
                        FileName = fn,
                        Name = fn.IndexOf(".", StringComparison.CurrentCulture) == -1
                    ? fn
                    : fn.Substring(0, fn.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase - 1))
                    }).ToList();
            return files;
        }

        public void UpdateWfsLayer(int id, string name, string filter, string color, bool useBbox)
        {
            WfsLayerSetting wfsLayerSetting = MapLayersSetting.GetWfsLayer(id);
            wfsLayerSetting.Filter = filter;
            wfsLayerSetting.Name = name;
            wfsLayerSetting.Color = color;
            wfsLayerSetting.UseSpatialFilterExtentAsBoundingBox = useBbox;
        }

        public WfsLayerSetting CreateNewWfsLayer(string name, string filter, string serverUrl, string typeName, string color, bool useBbox)
        {            
            var wfsLayer = new WfsLayerSetting();
            wfsLayer.Filter = filter;
            wfsLayer.Name = name;
            wfsLayer.ServerUrl = serverUrl;
            wfsLayer.TypeName = typeName;
            WFSDescribeFeatureType wfsDescribeFeatureType = WFSManager.GetWFSDescribeFeatureType(serverUrl, typeName);
            wfsLayer.GeometryName = wfsDescribeFeatureType.GeometryField.Name;
            wfsLayer.GeometryType = wfsDescribeFeatureType.GeometryType;
            wfsLayer.Color = color;
            wfsLayer.UseSpatialFilterExtentAsBoundingBox = useBbox;
            return wfsLayer;
        }

        public WfsLayerSetting CreateNewWfsFileLayer(string layerName, string fileName, string typeName, string filter, string serverUrl, GeometryType geometryType, string color, bool useBbox)
        {
            var wfsLayer = new WfsLayerSetting
            {
                Color = color,
                Filter = filter,
                GeometryName = fileName,
                GeometryType = GeometryType.Polygon,
                IsFile = true,
                Name = layerName,
                ServerUrl = serverUrl,
                TypeName = typeName,
                UseSpatialFilterExtentAsBoundingBox = useBbox
            };

            return wfsLayer;
        }

        public void AddWfsLayer(WfsLayerSetting wfsLayer)
        {
            MapLayersSetting.AddWfsLayer(wfsLayer);
        }

        public WfsLayerEditorViewModel CreateWfsLayerEditorViewModel(string url, string typeName, string filter, WfsLayerEditorMode? mode, int? id)
        {
            if (mode.GetValueOrDefault(WfsLayerEditorMode.New) == WfsLayerEditorMode.New)
            {
                return CreateWfsLayerEditorViewModelInCreateNewMode(url, typeName, filter);
            }
            else
            {
                return CreateWfsLayerEditorViewModelInEditMode(id.Value);
            }            
        }

        public WfsLayerViewModel CreateWfsLayerViewModel(int id)
        {
            var layerSetting = MapLayersSetting.GetWfsLayer(id);

            return WfsLayerViewModel.Create(layerSetting);
        }

        public List<WfsLayerViewModel> GetWfsLayers()
        {            
            return (from l in MapLayersSetting.WfsLayers select WfsLayerViewModel.Create(l)).ToList();
        }

        public WfsLayersViewModel CreateWfsLayersViewModel()
        {
            return new WfsLayersViewModel
            {
                IsSettingsDefault = MapLayersSetting.IsWfsSettingsDefault()
            };
        }

        /// <summary>
        /// Removes WFS layers.
        /// </summary>
        /// <param name="wfsLayerIds">The WFS layer ids.</param>
        public void RemoveWfsLayers(int[] wfsLayerIds)
        {
            MapLayersSetting.RemoveWfsLayers(wfsLayerIds);
        }
    }
}