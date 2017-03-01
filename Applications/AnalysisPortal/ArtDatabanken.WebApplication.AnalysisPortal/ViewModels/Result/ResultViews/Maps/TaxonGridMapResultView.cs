using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps
{
    public class TaxonGridMapResultView : ResultViewBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultViewSpeciesRichnessGridMap; }
        }

        public override string Tooltip
        {
            get { return ""; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "SpeciesRichnessGridMap"); }
        }

        public override ResultType ResultType
        {
            get { return ResultType.SpeciesRichnessGridMap; }
        }

        public override bool IsActive
        {
            get
            {
                return SessionHandler.MySettings.Calculation.GridStatistics.HasActiveSettings &&
                    SessionHandler.MySettings.Calculation.GridStatistics.CalculateNumberOfTaxa;
            }
        }        
    }
}
