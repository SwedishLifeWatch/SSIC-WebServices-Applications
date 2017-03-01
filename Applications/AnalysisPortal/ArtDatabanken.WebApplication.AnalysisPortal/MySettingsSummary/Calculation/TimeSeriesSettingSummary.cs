using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation
{
    /// <summary>
    /// This class contains settings summary for Periodicity settings (Calculation).
    /// </summary>
    public class TimeSeriesSettingSummary : MySettingsSummaryItemBase
    {
        public TimeSeriesSettingSummary()
        {
            SupportDeactivation = false;
        }

        private TimeSeriesSetting TimeSeriesSetting
        {
            get
            {
                return SessionHandler.MySettings.Calculation.TimeSeries;
            }
        }

        public override string Title
        {
            get
            {
                return Resources.Resource.StateButtonCalculationTimeSeries;                
            }
        }

        public override PageInfo PageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Calculation", "TimeSeries");
            }
        }

        public override bool HasSettingsSummary
        {
            get { return true; }
        }

        public TimeSeriesSettingsViewModel GetSettingsSummaryModel(IUserContext userContext)
        {
            var viewManager = new TimeSeriesSettingsViewManager(userContext, SessionHandler.MySettings);
            TimeSeriesSettingsViewModel model = viewManager.CreateViewModel();
            return model;
        }
        
        public override int? SettingsSummaryWidth
        {
            get { return 350; }
        }        

        public override bool IsActive
        {
            get
            {
                return TimeSeriesSetting.IsActive;
            }
            set { TimeSeriesSetting.IsActive = value; }
        }

        public override bool HasSettings
        {
            get { return TimeSeriesSetting.HasSettings; }
        }

        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.CalculationTimeSeries; }
        }
    }
}
