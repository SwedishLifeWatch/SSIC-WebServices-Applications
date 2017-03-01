using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables
{
    public class SpeciesObservationGridTableResultView : ResultViewBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultViewSpeciesObservationGridTable; }
        }

        public override string Tooltip
        {
            get { return ""; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "GridStatisticsTableOnSpeciesObservationCounts"); }
        }

        public override ResultType ResultType
        {
            get { return ResultType.SpeciesObservationGridTable; }
        }

        public override bool IsActive
        {
            get
            {
                return SessionHandler.MySettings.Calculation.GridStatistics.HasActiveSettings &&
                SessionHandler.MySettings.Calculation.GridStatistics.CalculateNumberOfObservations;
            }
                  //&& SessionHandler.MySettings.Presentation.Table.SelectedTableTypes.Any(m => m == PresentationTableType.GridStatisticsTable); }
        }
    }
}
