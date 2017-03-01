using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps
{
    public class SpeciesObservationGridMapResultView : ResultViewBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultViewSpeciesObservationGridMap; }
        }

        public override string Tooltip
        {
            get { return ""; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "SpeciesObservationGridMap"); }
        }

        public override ResultType ResultType
        {
            get { return ResultType.SpeciesObservationGridMap; }
        }

        public override bool IsActive
        {
            get
            {
                return SessionHandler.MySettings.Calculation.GridStatistics.HasActiveSettings &&
                       SessionHandler.MySettings.Calculation.GridStatistics.CalculateNumberOfObservations;
            }
        }        
    }
}
