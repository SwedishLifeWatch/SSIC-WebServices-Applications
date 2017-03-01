using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Presentation
{
    /// <summary>
    /// This class contains settings summary for data provider settings.
    /// </summary>
    public class MapSettingSummary : MySettingsSummaryItemBase
    {
        public MapSettingSummary()
        {
            SupportDeactivation = false;
        }

        private PresentationMapSetting MapSetting
        {
            get { return SessionHandler.MySettings.Presentation.Map; }
        }

        public override string Title
        {
            get
            {
                return Resources.Resource.StateButtonPresentationCoordinateSystem;                
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Format", "Map");
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
            get { return MapSetting.IsActive; }
            set { MapSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return true; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.PresentationMap; }
        }

        public IEnumerable<string> GetSettingsSummary()
        {
            return new List<string>
            {
                string.Format("{0}: {1}", Resource.PresentationMapPresentationCoordinateSystem, MapSetting.PresentationCoordinateSystemId.GetCoordinateSystemName()),
                string.Format("{0}: {1}", Resource.PresentationMapDownloadCoordinateSystem, MapSetting.DownloadCoordinateSystemId.GetCoordinateSystemName()),
                string.Format("{0}: {1}", Resource.PresentationMapGridMapsCoordinateSystem, ((CoordinateSystemId)SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId.Value).GetCoordinateSystemName())
            };
        }
    }
}
