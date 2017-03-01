using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables
{
    /// <summary>
    /// Class for SummaryStatisticsPerPolygon.
    /// </summary>
    public class SummaryStatisticsPerPolygonTableResultView : ResultViewBase
    {
        /// <summary>
        /// Title of the button.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ResultViewSummaryStatisticsPerPolygonTable; }
        }

        /// <summary>
        /// Tooltip for the button.
        /// </summary>
        public override string Tooltip
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Associated page.
        /// </summary>
        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "SummaryStatisticsPerPolygonTable"); }
        }

        /// <summary>
        /// Type of result.
        /// </summary>
        public override ResultType ResultType
        {
            get { return ResultType.SummaryStatisticsPerPolygon; }
        }

        /// <summary>
        /// Is functionality active.
        /// </summary>
        public override bool IsActive
        {
            get { return SessionHandler.MySettings.Presentation.Table.IsActive; }
        }        
    }
}