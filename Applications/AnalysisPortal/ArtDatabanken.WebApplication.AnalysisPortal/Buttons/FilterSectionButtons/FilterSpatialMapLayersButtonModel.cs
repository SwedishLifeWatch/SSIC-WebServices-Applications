using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    public class FilterSpatialMapLayersButtonModel : ButtonModelBase
    {
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "PolygonFromMapLayer");
            }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resource.FilterSpatialMapLayersTitle; }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return Resource.FilterSpatialMapLayersButtonTooltip; }
        }
    }
}
