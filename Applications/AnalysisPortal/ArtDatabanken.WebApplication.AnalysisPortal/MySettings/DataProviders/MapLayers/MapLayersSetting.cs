using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using System.Windows.Media;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers
{
    [DataContract]
    public sealed class MapLayersSetting : SettingBase
    {
        private List<WfsLayerSetting> _wfsLayers;
        private List<WmsLayerSetting> _wmsLayers;

        /// <summary>
        /// Gets or sets whether MapLayersSetting is active or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the WFS layers.
        /// </summary>
        [DataMember]
        public List<WfsLayerSetting> WfsLayers
        {
            get
            {
                if (_wfsLayers.IsNull())
                {
                    _wfsLayers = new List<WfsLayerSetting>();
                }
                return _wfsLayers;
            }

            set
            {
                _wfsLayers = value;
            }
        }

        [DataMember]
        public List<WmsLayerSetting> WmsLayers
        {
            get
            {
                if (_wmsLayers.IsNull())
                {
                    _wmsLayers = new List<WmsLayerSetting>();
                }
                return _wmsLayers;
            }

            set
            {
                _wmsLayers = value;
            }
        }

        /// <summary>
        /// Determines whether any settings has been done.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        ///   </returns>
        public override bool HasSettings
        {
            get { return WfsLayers.IsNotEmpty(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLayersSetting"/> class.
        /// </summary>
        public MapLayersSetting()
        {            
#if DEBUG
            AddSampleWfsLayers();
            AddSampleWmsLayers();
#endif
        }

        public int RemoveWfsLayer(int id)
        {
            WfsLayerSetting removeItem = WfsLayers.FirstOrDefault(wfsLayerSetting => wfsLayerSetting.Id == id);
            return WfsLayers.Remove(removeItem) ? 1 : 0;
        }

        public WfsLayerSetting AddWfsLayer(WfsLayerSetting wfsLayerSetting)
        {
            wfsLayerSetting.Id = GetNextWfsLayerSettingUniqueId();
            WfsLayers.Add(wfsLayerSetting);
            return wfsLayerSetting;
        }

        public WfsLayerSetting GetWfsLayer(int id)
        {
            return WfsLayers.FirstOrDefault(wfsLayerSetting => wfsLayerSetting.Id == id);
        }

        private int GetNextWfsLayerSettingUniqueId()
        {
            if (WfsLayers == null || WfsLayers.Count == 0)
            {
                return 0;
            }

            var elements = from element in WfsLayers
                           orderby element.Id descending
                           select element;
            return elements.First().Id + 1;
        }

        public int RemoveWmsLayer(int id)
        {
            WmsLayerSetting removeItem = WmsLayers.FirstOrDefault(wmsLayerSetting => wmsLayerSetting.Id == id);
            return WmsLayers.Remove(removeItem) ? 1 : 0;
        }

        /// <summary>
        /// Removes WMS layers.
        /// </summary>
        /// <param name="ids">The ids.</param>
        public void RemoveWmsLayers(int[] ids)
        {
            foreach (int id in ids)
            {
                RemoveWmsLayer(id);
            }            
        }

        public WmsLayerSetting AddWmsLayer(WmsLayerSetting wmsLayerSetting)
        {
            wmsLayerSetting.Id = GetNextWmsLayerSettingUniqueId();
            WmsLayers.Add(wmsLayerSetting);
            return wmsLayerSetting;
        }

        public WmsLayerSetting GetWmsLayer(int id)
        {
            return WmsLayers.FirstOrDefault(wmsLayerSetting => wmsLayerSetting.Id == id);
        }

        private int GetNextWmsLayerSettingUniqueId()
        {
            if (WmsLayers == null || WmsLayers.Count == 0)
            {
                return 0;
            }

            var elements = from element in WmsLayers
                           orderby element.Id descending
                           select element;
            return elements.First().Id + 1;
        }

        public string CreateMapLayerHexColor()
        {
            var strHexColors = new List<string>();
            foreach (WfsLayerSetting wfsLayerSetting in WfsLayers)
            {
                strHexColors.Add(wfsLayerSetting.Color);
            }

            Color color = ColorManager.GetNextUnusedColor(strHexColors);
            return color.ToHexString();
        }

        private void AddSampleWmsLayers()
        {
            //var layerSetting = new WmsLayerSetting
            //    {
            //        Name = "Topografiska webbkartan",
            //        ServerUrl = "http://pandora.slu.se:8080/geoserver/slu/wms",
            //        Layers = new List<string> { "topowebbkartan" },
            //        IsBaseLayer = true,
            //        SupportedCoordinateSystems = new List<string> { "EPSG:4326", "EPSG:900913", "EPSG:3006", "EPSG:3021" } 
            //        //Layers = "slu:topowebbkartan"
            //    };
            //AddWmsLayer(layerSetting);

            var layerSetting = new WmsLayerSetting
            {
                Name = "Sample WMS",                
                ServerUrl = "http://vmap0.tiles.osgeo.org/wms/vmap0",
                Layers = new List<string> { "basic" },
                IsBaseLayer = true,
                SupportedCoordinateSystems = new List<string> { "EPSG:4326", "EPSG:900913" } //EPSG:4269, 
            };
            AddWmsLayer(layerSetting);

            layerSetting = new WmsLayerSetting
            {
                Name = "Web Map Service luft",
                ServerUrl = "http://gpt.vic-metria.nu:80/wmsconnector/com.esri.wms.Esrimap/luft",                             
                Layers = new List<string> { "0", "1", "2" },
                SupportedCoordinateSystems = new List<string> { "EPSG:4326", "EPSG:3006" } //EPSG:4267, EPSG:4269
            };
            AddWmsLayer(layerSetting);
        }

        private void AddSampleWfsLayers()
        {
            // Create one element just for debug purposes...
            var layersSetting = new WfsLayerSetting
            {
                Name = "Jönköping & Östergötlands län",
                Color = ColorManager.GetColor(0).ToHexString(),
                Filter = "<Filter><Or><PropertyIsEqualTo><PropertyName>länskod</PropertyName><Literal>5</Literal></PropertyIsEqualTo><PropertyIsEqualTo><PropertyName>länskod</PropertyName><Literal>6</Literal></PropertyIsEqualTo></Or></Filter>",
                ServerUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs",
                TypeName = "SLW:artdatabankenslanskarta",
                GeometryName = "geom",                
                GeometryType = GeometryType.Polygon
            };        

            AddWfsLayer(layersSetting);

            layersSetting = new WfsLayerSetting
            {
                Name = "Blekinge",
                Color = ColorManager.GetColor(1).ToHexString(),
                Filter = "<Filter><PropertyIsEqualTo><PropertyName>länskod</PropertyName><Literal>10</Literal></PropertyIsEqualTo></Filter>",
                ServerUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs",
                TypeName = "SLW:artdatabankenslanskarta",
                GeometryName = "geom",
                GeometryType = GeometryType.Polygon
            };
            AddWfsLayer(layersSetting);

            layersSetting = new WfsLayerSetting
            {
                Name = "Dalarna",
                Color = ColorManager.GetColor(2).ToHexString(),
                Filter = "<Filter><PropertyIsEqualTo><PropertyName>länskod</PropertyName><Literal>20</Literal></PropertyIsEqualTo></Filter>",
                ServerUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs",
                TypeName = "SLW:artdatabankenslanskarta",
                GeometryName = "geom",
                GeometryType = GeometryType.Polygon
            };
            AddWfsLayer(layersSetting);

            layersSetting = new WfsLayerSetting
            {
                Name = "ArtDatabankens länskarta",
                Color = ColorManager.GetColor(3).ToHexString(),                
                ServerUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/SLW/wfs",
                TypeName = "SLW:artdatabankenslanskarta",
                GeometryName = "geom",
                GeometryType = GeometryType.Polygon,
                UseSpatialFilterExtentAsBoundingBox = true
            };
            AddWfsLayer(layersSetting);            
        }

        public void ResetWfsSettings()
        {
            WfsLayers = new List<WfsLayerSetting>();
        }

        public bool IsWfsSettingsDefault()
        {
            if (WfsLayers.Count == 0)
            {
                return true;
            }

            return false;            
        }

        public void ResetWmsSettings()
        {
            WmsLayers = new List<WmsLayerSetting>();
        }

        public bool IsWmsSettingsDefault()
        {
            if (WmsLayers.Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes WFS layers.
        /// </summary>
        /// <param name="layerIds">The layer ids.</param>
        public void RemoveWfsLayers(int[] layerIds)
        {
            foreach (int id in layerIds)
            {
                RemoveWfsLayer(id);
            }
        }
    }
}