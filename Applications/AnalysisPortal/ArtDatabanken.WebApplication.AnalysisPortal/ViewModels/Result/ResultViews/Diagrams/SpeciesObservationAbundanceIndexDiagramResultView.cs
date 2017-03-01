using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Diagrams
{
    public class SpeciesObservationAbundanceIndexDiagramResultView : ResultViewBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultViewSpeciesObservationAbundanceIndexDiagram; }
        }

        public override string Tooltip
        {
            get { return ""; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "TimeSeriesDiagramOnSpeciesObservationAbundanceIndex"); }
        }

        public override ResultType ResultType
        {
            get { return ResultType.TimeSeriesOnSpeciesObservationAbundanceIndexHistogram; }
        }

        public override bool IsActive
        {
            get { return SessionHandler.MySettings.Presentation.Table.IsActive; }
        }        
    }
}