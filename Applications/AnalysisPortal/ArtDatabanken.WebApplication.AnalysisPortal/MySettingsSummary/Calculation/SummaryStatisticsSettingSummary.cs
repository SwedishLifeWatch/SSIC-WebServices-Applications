using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation
{
    /// <summary>
    /// This class contains settings summary for summary statistics settings.
    /// </summary>
    public class SummaryStatisticsSettingSummary : MySettingsSummaryItemBase
    {
        public SummaryStatisticsSettingSummary()
        {
            SupportDeactivation = false;
        }

        private SummaryStatisticsSetting SummaryStatisticsSetting
        {
            get { return SessionHandler.MySettings.Calculation.SummaryStatistics; }
        }

        public override string Title
        {
            get
            {
                return Resources.Resource.StateButtonCalculationSummaryStatistics;                
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Calculation", "SummaryStatistics");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return true; }
        }

        public SummaryStatisticsViewModel GetSettingsSummaryModel(IUserContext userContext)
        {
            var viewManager = new SummaryStatisticsViewManager(userContext, SessionHandler.MySettings);
            SummaryStatisticsViewModel model = viewManager.CreateViewModel();
            return model;
        }
        
        public override int? SettingsSummaryWidth
        {
            get { return 350; }
        }        

        public override bool IsActive
        {
            get { return SummaryStatisticsSetting.IsActive; }
            set { SummaryStatisticsSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return SummaryStatisticsSetting.HasSettings; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.CalculationSummaryStatistics; }
        }
    }
}
