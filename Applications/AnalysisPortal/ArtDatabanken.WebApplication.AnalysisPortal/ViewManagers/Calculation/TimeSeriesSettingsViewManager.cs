using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation
{
    /// <summary>
    /// This class is a view manager for handling time series settings.
    /// </summary>
    public class TimeSeriesSettingsViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the time series settings that exists in MySettings.
        /// </summary>
        public TimeSeriesSetting TimeSeriesSetting
        {
            get { return MySettings.Calculation.TimeSeries; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSeriesSettingsViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public TimeSeriesSettingsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Updates the time series settings.
        /// </summary>
        /// <param name="model">The new settings.</param>
        public void UpdateTimeSeriesSettings(TimeSeriesSettingsViewModel model)
        {
            TimeSeriesSetting.DefaultPeriodicityIndex = model.DefaultPeriodicityIndex;
        }

        /// <summary>
        /// Creates the view model and set it's data.
        /// </summary>
        /// <returns></returns>
        public TimeSeriesSettingsViewModel CreateViewModel()
        {
            var model = new TimeSeriesSettingsViewModel();            
            model.IsSettingsDefault = TimeSeriesSetting.IsSettingsDefault();
            model.DefaultPeriodicityIndex = TimeSeriesSetting.DefaultPeriodicityIndex;
            return model;
        }
    }
}
