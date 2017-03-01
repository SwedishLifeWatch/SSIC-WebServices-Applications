using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons
{
    public class ResultTaxonWithSpeciesObservationCountsTableButtonModel : ButtonModelBase
    {
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Result", "SpeciesObservationTaxonWithSpeciesObservationCountTable");
            }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ResultViewSpeciesObservationTaxonSpeciesObservationCountTable; }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return ""; }
        }
    }
}