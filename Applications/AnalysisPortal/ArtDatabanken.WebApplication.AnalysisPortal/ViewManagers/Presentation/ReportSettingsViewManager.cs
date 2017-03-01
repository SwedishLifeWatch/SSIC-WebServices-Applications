using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Report;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Report;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation
{
    /// <summary>
    /// This class is a view manager for handling data sources operations using the MySettings object.
    /// </summary>
    public class ReportSettingsViewManager : ViewManagerBase
    {
          /// <summary>
        /// Gets the grid statistics setting that exists in MySettings.
        /// </summary>
        public PresentationReportSetting ReportSetting
        {
            get { return MySettings.Presentation.Report; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryStatisticsViewManager"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public ReportSettingsViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        public void UpdatePresentationReportSetting(PresentationReportViewModel reportSettings)
        {
             List<PresentationReportType> selectedReportTypes = new List<PresentationReportType>();
            foreach (int tableTypeId in reportSettings.SelectedReportTypes)
            {
                try
                {
                    selectedReportTypes.Add((PresentationReportType)tableTypeId);
                }
                catch
                {                    
                }
            }
            ReportSetting.SelectedReportTypes = new ObservableCollection<PresentationReportType>(selectedReportTypes);
        }

        public PresentationReportViewModel CreatePresentationReportViewModel()
        {
            var model = new PresentationReportViewModel();
            model.SelectedReportTypes = ReportSetting.SelectedReportTypes.Select(reportType => (int)reportType).ToList();
            model.Reports = new List<PresentationReportTypeViewModel>();

            var summaryStatistics = new PresentationReportTypeViewModel();
            summaryStatistics.Title = Resource.PresentationSummaryStatisticsTitle;
            summaryStatistics.Id = (int)PresentationReportType.SummaryStatistics;
            summaryStatistics.PageInfo = PageInfoManager.GetPageInfo("Result", "Reports");
            summaryStatistics.IsSelected = ReportSetting.SelectedReportTypes.Any(id => id == PresentationReportType.SummaryStatistics);
            model.Reports.Add(summaryStatistics);

            return model;
        }
    }
}
