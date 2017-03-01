using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons
{
    public class ResultSpeciesObservationTableButtonModel : ButtonModelBase
    {
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Result", "SpeciesObservationTable");
            }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ResultViewSpeciesObservationTable; }
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
