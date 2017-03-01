namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    /// <summary>
    /// This class is the MySettings summary group for all Filter settings
    /// </summary>
    public class FilterSummaryGroup : MySettingsSummaryGroupBase
    {
        /// <summary>
        /// Gets the group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.HeadMenuFilter; }
        }

        public override string ImageUrl
        {
            get { return "~/Content/images/FilterSummaryGroup.png"; }
        }

        public override string IconClass
        {
            get { return "icon-filter"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSummaryGroup"/> class.
        /// </summary>
        public FilterSummaryGroup()
        {
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterTaxa));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterPolygon));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterRegion));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterLocality));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterTemporal));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterAccuracy));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterOccurrence));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.FilterField));            
        }
    }
}
