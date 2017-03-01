using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Reports;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Diagrams
{
    public class DiagramsResultGroup : ResultGroupBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultGroupDiagrams; }
        }

        public override ResultGroupType ResultGroupType
        {
            get { return ResultGroupType.Diagrams; }
        }
        
        public override string OverviewButtonTooltip
        {
            get { return ""; }
        }

        public override PageInfo OverviewPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "Diagrams"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramsResultGroup"/> class.
        /// </summary>
        public DiagramsResultGroup()
        {
            Items.Add(new SpeciesObservationDiagramResultView());
            Items.Add(new SpeciesObservationAbundanceIndexDiagramResultView());
        }
    }
}
