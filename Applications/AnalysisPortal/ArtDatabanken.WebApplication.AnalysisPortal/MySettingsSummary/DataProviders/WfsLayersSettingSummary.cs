using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.DataProviders
{
    /// <summary>
    /// This class contains settings summary for wfs layers settings.
    /// </summary>
    public class WfsLayersSettingSummary : MySettingsSummaryItemBase
    {
        /// <summary>
        /// Gets the map layers setting from MySettings.
        /// </summary>
        /// <value>
        /// The map layers setting.
        /// </value>
        private MapLayersSetting MapLayersSetting
        {
            get { return SessionHandler.MySettings.DataProvider.MapLayers; }
        }

        /// <summary>        
        /// Gets the title.        
        /// </summary>
        public override string Title
        {
            get
            {
                string template = Resources.Resource.MySettingsDataSourcesNumberOfWfsLayers;
                string str = string.Format(template, MapLayersSetting.WfsLayers.Count);
                return str;
            }
        }

        /// <summary>
        /// Gets the page info.
        /// </summary>
        public override PageInfo PageInfo
        {
            get { return PageInfoManager.GetPageInfo("Data", "WfsLayers"); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has settings summary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has settings summary; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettingsSummary
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the settings summary view width.
        /// If null, use default.
        /// </summary>
        public override int? SettingsSummaryWidth
        {
            get { return null; }
        }

        /// <summary>
        /// Gets a value indicating whether this setting is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public override bool IsActive
        {
            get { return true; }
            set { }
        }

        /// <summary>
        /// Gets a value indicating whether any settings has been done.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return MapLayersSetting.WfsLayers.Count > 0; }
        }

        /// <summary>
        /// Gets the identifier for this class.
        /// </summary>
        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.DataEnvironmentalData; }
        }

        /// <summary>
        /// Get summary list.
        /// </summary>
        /// <returns>
        /// A list with settings information.
        /// </returns>
        public List<string> GetSummaryList()
        {
            List<string> summarySetting = new List<string>();
            foreach (WfsLayerSetting wfsLayerSetting in MapLayersSetting.WfsLayers)
            {
                summarySetting.Add(wfsLayerSetting.Name);
            }

            return summarySetting;
        }
    }
}
