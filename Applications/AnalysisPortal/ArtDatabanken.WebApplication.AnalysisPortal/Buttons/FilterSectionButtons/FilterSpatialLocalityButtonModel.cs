using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    /// <summary>
    /// This class is button model for Filter/Locality.
    /// </summary>
    public class FilterSpatialLocalityButtonModel : ButtonModelBase
    {
        /// <summary>
        /// Gets the page info.
        /// </summary>
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "Locality");
            }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resource.FilterSpatialLocalityTitle; }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return Resource.FilterSpatialLocalityButtonTooltip; }
        }
    }
}
