using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Presentation
{
    /// <summary>
    /// This class contains settings summary for data provider settings.
    /// </summary>
    public class ReportSettingSummary : MySettingsSummaryItemBase
    {
        public ReportSettingSummary()
        {
            SupportDeactivation = true;
        }

        private PresentationReportSetting ReportSetting
        {
            get { return SessionHandler.MySettings.Presentation.Report; }
        }

        public override string Title
        {
            get
            {
                return Resources.Resource.StateButtonPresentationReport;                
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Result", "Reports");
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
            get { return ReportSetting.IsActive; }
            set { ReportSetting.IsActive = value; }
        }

        // Todo: is set to false to prevent from showing in my settings summary
        public override bool HasSettings
        {
            get { return false; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.PresentationReport; }
        }
    }
}
