using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace AnalysisPortal.Helpers
{
    public static class JavaScriptExtensions
    {
        public static string DeclareWmsLayersJsArray(this HtmlHelper html, string arrayName)
        {                   
            var sb = new StringBuilder();
            var layersSb = new StringBuilder();
            sb.AppendLine(string.Format("var {0} = [];", arrayName));
            var layerIndex = 0;

            foreach (WmsLayerSetting layerSetting in SessionHandler.MySettings.DataProvider.MapLayers.WmsLayers)
            {
                layersSb.Append("[");
                for (int i = 0; i < layerSetting.Layers.Count; i++)
                {
                    string strLayerName = layerSetting.Layers[i];
                    layersSb.Append("'");
                    layersSb.Append(strLayerName);
                    layersSb.Append("'");
                    if (i < layerSetting.Layers.Count - 1)
                    {
                        layersSb.Append(",");
                    }
                }
                var objName = string.Format("wmsObj{0}", layerIndex);
                layersSb.Append("]");
                var scs = DeclareInlineJsStringArray(layerSetting.SupportedCoordinateSystems);
                sb.AppendLine(string.Format("var {0} = {{ id: {1}, name: '{2}', layers: {3}, serverUrl: '{4}', isBaseLayer: {5}, supportedCoordinateSystems: {6}}};", objName, layerSetting.Id, layerSetting.Name, layersSb, layerSetting.ServerUrl, layerSetting.IsBaseLayer.ToString().ToLower(), scs));
                sb.AppendLine(string.Format("{0}.push({1});", arrayName, objName));
                layersSb.Clear();
                layerIndex++;
            }
            return sb.ToString();          
        }

        private static string DeclareInlineJsStringArray(List<string> strings)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < strings.Count; i++)
            {
                sb.Append("\"");
                sb.Append(strings[i]);
                sb.Append("\"");
                if (i < strings.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static string DeclareWfsLayersJsArray(this HtmlHelper html, string arrayName)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("var {0} = [];", arrayName));
            var layerIndex = 0;
            foreach (WfsLayerSetting layerSetting in SessionHandler.MySettings.DataProvider.MapLayers.WfsLayers)
            {
                var objName = string.Format("wfsObj{0}", layerIndex);

                sb.AppendLine(string.Format("var {0} = {{ id: {1}, name: '{2}', visible: false, color: '{3}'}};", objName, layerSetting.Id, layerSetting.Name, layerSetting.Color));
                sb.AppendLine(string.Format("{0}.push({1});", arrayName, objName));
                layerIndex++;
            }
            return sb.ToString();
        }
    }
}