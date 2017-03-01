using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result.ResultViews.Tables
{
    public class TablesResultGroup : ResultGroupBase
    {
        public override string Title
        {
            get { return Resources.Resource.ResultGroupTables; }
        }

        public override ResultGroupType ResultGroupType
        {
            get { return ResultGroupType.Tables; }
        }

        public override PageInfo OverviewPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "Tables"); }
        }

        public override string OverviewButtonTooltip
        {
            get { return ""; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TablesResultGroup"/> class.
        /// </summary>
        public TablesResultGroup()
        {
            Items.Add(new SpeciesObservationTableResultView());
            Items.Add(new SpeciesObservationTaxonTableResultView());
            Items.Add(new SpeciesObservationTaxonWithObservationCountTableResultView());
            Items.Add(new SpeciesObservationGridTableResultView());
            Items.Add(new TaxonGridTableResultView());
            Items.Add(new TimeSeriesOnSpeciesObservationCountsTableResultView());
            Items.Add(new SummaryStatisticsPerPolygonTableResultView());
        }
    }
}
