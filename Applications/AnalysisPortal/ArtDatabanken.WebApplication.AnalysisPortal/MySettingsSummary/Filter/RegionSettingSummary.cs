using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    /// <summary>
    /// This class contains settings summary for region settings.
    /// </summary>
    public class RegionSettingSummary : MySettingsSummaryItemBase
    {
        public RegionSettingSummary()
        {
            SupportDeactivation = true;
        }

        private SpatialSetting SpatialSetting
        {
            get { return SessionHandler.MySettings.Filter.Spatial; }
        }

        public override string Title
        {
            get
            {                
                string template = Resources.Resource.MySettingsFilterNumberOfSelectedRegions;
                string str = string.Format(template, SpatialSetting.RegionIds.Count);
                return str;
            }
        }

        public override PageInfo PageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "SpatialCommonRegions"); }
        }

        public override bool HasSettingsSummary
        {
            get { return IsActive && HasSettings; }
        }

        public List<RegionViewModel> GetSettingsSummaryModel()
        {
            var viewManager = new SpatialFilterViewManager(CoreData.UserManager.GetCurrentUser(), SessionHandler.MySettings);
            List<RegionViewModel> regions = viewManager.GetAllRegions();
            return regions;
        }        

        public override int? SettingsSummaryWidth
        {
            get { return null; }
        }

        public override bool IsActive
        {
            get { return SpatialSetting.IsActive; }
            set { SpatialSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return SpatialSetting.RegionIds.Count > 0; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.FilterRegion; }
        }
    }
}
