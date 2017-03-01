using System;
using System.Globalization;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation
{
    /// <summary>
    /// This class is a view manager for handling summary statistics operations using the MySettings object.
    /// </summary>
    public class SummaryStatisticsViewManager : ViewManagerBase
    {
        /// <summary>
        /// Gets the summary statistics setting that exists in MySettings.
        /// </summary>
        public SummaryStatisticsSetting SummaryStatisticsSetting
        {
            get { return MySettings.Calculation.SummaryStatistics; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryStatisticsViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public SummaryStatisticsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Update summary statistics setting.
        /// </summary>
        /// <param name="model">The SummaryStatisticsViewModel to update.</param>
        public void UpdateSummaryStatistics(SummaryStatisticsViewModel model)
        {
            SummaryStatisticsSetting.CalculateNumberOfObservationsfromObsData = model.CalculateNumberOfObservationsfromObsData;
            SummaryStatisticsSetting.CalculateNumberOfSpeciesfromObsData = model.CalculateNumberOfSpeciesfromObsData;
            SummaryStatisticsSetting.WfsSummaryStatisticsLayerId = model.WfsGridStatisticsLayerId;
        }

        /// <summary>
        /// Creates the view model and set it's data.
        /// </summary>
        /// <returns>Returns a new SummaryStatisticsViewModel.</returns>
        public SummaryStatisticsViewModel CreateViewModel()
        {
            var model = new SummaryStatisticsViewModel();

            model.CalculateNumberOfObservationsfromObsData = SummaryStatisticsSetting.CalculateNumberOfObservationsfromObsData;
            model.CalculateNumberOfSpeciesfromObsData = SummaryStatisticsSetting.CalculateNumberOfSpeciesfromObsData;
            
            // WFS Grid statistics
            model.WfsGridStatisticsLayerId = SummaryStatisticsSetting.WfsSummaryStatisticsLayerId;

            var wfsViewManager = new WfsLayersViewManager(UserContext, MySettings);

            model.WfsLayers = wfsViewManager.CreateWfsLayersList();
            model.IsSettingsDefault = SummaryStatisticsSetting.IsSettingsDefault();
            return model;
        }
    }
}