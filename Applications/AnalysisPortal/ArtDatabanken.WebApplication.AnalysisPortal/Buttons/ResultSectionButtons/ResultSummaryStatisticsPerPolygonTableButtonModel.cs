using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons
{
    /// <summary>
    /// Class for Summary statistics per polygon button model.
    /// </summary>
    public class ResultSummaryStatisticsPerPolygonTableButtonModel : ButtonModelBase
    {
        /// <summary>
        /// Gets the page info.
        /// </summary>
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Result", "SummaryStatisticsPerPolygonTable");
            }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ResultViewSummaryStatisticsPerPolygonTable; }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return string.Empty; }
        }
    }
}