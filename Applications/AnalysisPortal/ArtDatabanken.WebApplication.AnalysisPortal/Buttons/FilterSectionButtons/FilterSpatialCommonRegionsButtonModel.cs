using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    public class FilterSpatialCommonRegionsButtonModel : ButtonModelBase
    {        
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "SpatialCommonRegions");
            }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resource.FilterSpatialCommonRegionsTitle; }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return Resource.FilterSpatialCommonRegionsButtonTooltip; }
        }
    }
}
