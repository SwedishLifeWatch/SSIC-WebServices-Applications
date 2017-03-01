using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Reports
{
    public class ReportSettingsSummaryResultView : ResultViewBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultViewSettingsSummary; }
        }

        public override string Tooltip
        {
            get { return ""; }
        }

        public override ResultType ResultType
        {
            get { return ResultType.SettingsSummary; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "SettingsReport"); }
        }

        public override bool IsActive
        {
            get { return true; }
        }        
    }
}
