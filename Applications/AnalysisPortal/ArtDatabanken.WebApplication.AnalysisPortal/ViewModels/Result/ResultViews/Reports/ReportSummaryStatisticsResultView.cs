using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
//using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Reports
{
    public class ReportSummaryStatisticsResultView : ResultViewBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultViewSummaryStatistics; }
        }

        public override string Tooltip
        {
            get { return ""; }
        }

        public override ResultType ResultType
        {
            get { return ResultType.SummaryStatisticsReport; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "SummaryStatisticsReport"); }
        }

        public override bool IsActive
        {
            get { return SessionHandler.MySettings.Calculation.SummaryStatistics.HasActiveSettings && SessionHandler.MySettings.Presentation.Report.IsActive; }
        }        
    }
}
