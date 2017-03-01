namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.DataProviders
{
    /// <summary>
    /// This class is the MySettings summary group for all Data sources settings
    /// </summary>
    public class DataProvidersSummaryGroup : MySettingsSummaryGroupBase
    {
        /// <summary>
        /// Gets the group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.HeadMenuDataProviders; }
        }

        public override string ImageUrl
        {
            get { return "~/Content/Images/DataProvidersSummaryGroup.png"; }
        }
        public override string IconClass
        {
            get { return "icon-sitemap"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvidersSummaryGroup"/> class.
        /// </summary>
        public DataProvidersSummaryGroup()
        {
            Items.Add((DataProvidersSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.DataProviders));
            Items.Add((WfsLayersSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.DataEnvironmentalData));
            Items.Add((MapLayersSettingSummary)MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.DataMapLayers));
        }
    }
}
