using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps
{
    public class WfsGridMapResultView : ResultViewBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultViewWfsGridMap; }
        }

        public override string Tooltip
        {
            get { return ""; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "WfsGridStatisticsMap"); }
        }

        public override ResultType ResultType
        {
            get { return ResultType.WfsStatisticsGridMap; }
        }

        public override bool IsActive
        {
            get { return true; }
        }        
    }
}
