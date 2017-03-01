using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Maps
{
    public class MapsResultGroup : ResultGroupBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultGroupMaps; }
        }

        public override ResultGroupType ResultGroupType
        {
            get { return ResultGroupType.Maps; }
        }
        
        public override string OverviewButtonTooltip
        {
            get { return ""; }
        }

        public override PageInfo OverviewPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "Maps"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapsResultGroup"/> class.
        /// </summary>
        public MapsResultGroup()
        {
            Items.Add(new SpeciesObservationMapResultView());
            Items.Add(new SpeciesObservationGridMapResultView());
            Items.Add(new TaxonGridMapResultView());
            Items.Add(new WfsGridMapResultView());
            Items.Add(new SpeciesObservationHeatMapResultView());
        }
    }
}
