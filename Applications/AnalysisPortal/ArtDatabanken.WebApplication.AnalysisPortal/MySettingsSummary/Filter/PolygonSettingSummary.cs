using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    /// <summary>
    /// This class contains settings summary for polygon settings.
    /// </summary>
    public class PolygonSettingSummary : MySettingsSummaryItemBase
    {        
        public PolygonSettingSummary()
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
                string template = Resources.Resource.MySettingsFilterNumberOfSelectedPolygons;
                string strPolygons = string.Format(template, SpatialSetting.Polygons.Count);
                return strPolygons;
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "SpatialDrawPolygon");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return IsActive && HasSettings; }
        }        

        public List<DataPolygon> GetSettingsSummaryModel()
        {
            return SessionHandler.MySettings.Filter.Spatial.Polygons.ToList();            
        }

        public override int? SettingsSummaryWidth
        {
            get { return 390; }
        }

        public override bool IsActive
        {
            get { return SpatialSetting.IsActive; }
            set { SpatialSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return SpatialSetting.Polygons.Count > 0; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.FilterPolygon; }
        }
    }
}
