using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
//using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.DataProviders
{
    /// <summary>
    /// This class contains settings summary for map layer settings.
    /// </summary>
    public class MapLayersSettingSummary : MySettingsSummaryItemBase
    {
        private MapLayersSetting MapLayersSetting
        {
            get { return SessionHandler.MySettings.DataProvider.MapLayers; }
        }

        public override string Title
        {
            get
            {
                string template = Resources.Resource.MySettingsDataSourcesNumberOfWmsLayers;
                string str = string.Format(template, MapLayersSetting.WmsLayers.Count);
                return str;
            }
            //get
            //{
            //    return Resources.Resource.DataProvidersWmsLayers;
            //}
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Data", "WmsLayers");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return true; }
        }
        
        public override int? SettingsSummaryWidth
        {
            get { return null; }
        }        

        public override bool IsActive
        {
            //get { return MapLayersSetting.IsActive; }
            get { return true; }
            set { }
        }

        public override bool HasSettings
        {
            get { return MapLayersSetting.WmsLayers.Count > 0; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.DataMapLayers; }
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
            foreach (WmsLayerSetting wmsLayerSetting in MapLayersSetting.WmsLayers)
            {
                summarySetting.Add(wmsLayerSetting.Name);
            }

            return summarySetting;
        }

        //public List<WmsLayerViewModel> GetSettingsSummaryModel()
        //{            
        //    IUserContext userContext = CoreData.UserManager.GetCurrentUser();
        //    var viewManager = new WmsLayersViewManager(userContext, SessionHandler.MySettings);
        //    List<WmsLayerViewModel> mapLayers = viewManager.GetWmsLayers();
        //    return mapLayers;            
        //}
    }
}
