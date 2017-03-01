using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Presentation
{
    /// <summary>
    /// This class contains settings summary for data provider settings.
    /// </summary>
    public class TableSettingSummary : MySettingsSummaryItemBase
    {
        public TableSettingSummary()
        {
            SupportDeactivation = false;
        }

        private PresentationTableSetting TableSetting
        {
            get { return SessionHandler.MySettings.Presentation.Table; }
        }

        public override string Title
        {
            get
            {
                return Resources.Resource.StateButtonPresentationTable;
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Format", "SpeciesObservationTable");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return true; }
        }

        //public GridStatisticsViewModel GetSettingsSummaryModel(IUserContext userContext)
        //{
        //    var viewManager = new MySettingsGridStatisticsViewManager(userContext, SessionHandler.MySettings);
        //    GridStatisticsViewModel model = viewManager.CreateViewModel();
        //    return model;
        //}
        
        public override int? SettingsSummaryWidth
        {
            get { return 350; }
        }        

        public override bool IsActive
        {
            get { return TableSetting.IsActive; }
            set { TableSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return true; } //return TableSetting.HasSettings; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.PresentationTable; }
        }
    }
}
