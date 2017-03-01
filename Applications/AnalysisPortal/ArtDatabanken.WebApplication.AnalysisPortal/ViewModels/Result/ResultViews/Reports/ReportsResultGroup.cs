using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Reports
{
    public class ReportsResultGroup : ResultGroupBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultGroupReports; }
        }

        public override ResultGroupType ResultGroupType
        {
            get { return ResultGroupType.Report; }
        }

        public override string OverviewButtonTooltip
        {
            get { return ""; }
        }

        public override PageInfo OverviewPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "Reports"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsResultGroup"/> class.
        /// </summary>
        public ReportsResultGroup()
        {
            Items.Add(new ReportSummaryStatisticsResultView());
            Items.Add(new ReportProvenanceResultView());
            Items.Add(new ReportSettingsSummaryResultView());
        }
    }
}
