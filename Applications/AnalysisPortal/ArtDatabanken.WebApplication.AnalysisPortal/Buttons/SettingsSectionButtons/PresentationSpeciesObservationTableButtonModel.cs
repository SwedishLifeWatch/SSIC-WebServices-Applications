using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.SettingsSectionButtons
{
    public class PresentationSpeciesObservationTableButtonModel : ButtonModelBase
    {
        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Format", "SpeciesObservationTable"); }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.PresentationSpeciesObservationTable; }
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
