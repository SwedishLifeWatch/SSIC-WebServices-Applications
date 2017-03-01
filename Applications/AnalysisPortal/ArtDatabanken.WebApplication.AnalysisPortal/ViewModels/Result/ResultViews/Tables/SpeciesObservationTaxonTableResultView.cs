using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables
{
    public class SpeciesObservationTaxonTableResultView : ResultViewBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultViewSpeciesObservationTaxonTable; }
        }

        public override string Tooltip
        {
            get { return ""; }
        }        

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "SpeciesObservationTaxonTable");  }
        }

        public override ResultType ResultType
        {
            get { return ResultType.SpeciesObservationTaxonTable; }
        }

        public override bool IsActive
        {
            get { return SessionHandler.MySettings.Presentation.Table.IsActive; }
        }        
    }
}
