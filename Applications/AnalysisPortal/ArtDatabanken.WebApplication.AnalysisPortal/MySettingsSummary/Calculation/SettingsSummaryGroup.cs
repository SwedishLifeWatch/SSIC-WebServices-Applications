namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation
{
    /// <summary>
    /// This class is the MySettings summary group for all settings summary settings
    /// </summary>
    public class SettingsSummaryGroup : MySettingsSummaryGroupBase
    {
        /// <summary>
        /// Gets the group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.HeadMenuSettings; }
        }

        public override string ImageUrl
        {
            get { return "~/Content/images/PresentationSummaryGroup.png"; }
        }

        public override string IconClass
        {
            get { return "icon-gears"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsSummaryGroup"/> class.
        /// </summary>
        public SettingsSummaryGroup()
        {
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationSummaryStatistics));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationGridStatistics));            
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationTimeSeries));

            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationMap));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationTable));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationReport));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationFileFormat));
        }
    }
}
