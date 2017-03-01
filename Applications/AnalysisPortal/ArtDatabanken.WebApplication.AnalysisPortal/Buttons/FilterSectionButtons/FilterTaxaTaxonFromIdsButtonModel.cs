using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    public class FilterTaxaTaxonFromIdsButtonModel : ButtonModelBase
    {
        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "TaxonFromIds"); }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.FilterTaxonFromIdsButton; }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return Resources.Resource.FilterTaxonFromIdsButtonTooltip; }
        }
    }
}
