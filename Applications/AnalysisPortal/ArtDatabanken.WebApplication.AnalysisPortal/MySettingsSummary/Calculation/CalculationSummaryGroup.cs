namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation
{
    /// <summary>
    /// This class is the MySettings summary group for all calculation summary settings
    /// </summary>
    public class CalculationSummaryGroup : MySettingsSummaryGroupBase
    {
        /// <summary>
        /// Gets the group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.HeadMenuCalculation; }
        }

        public override string ImageUrl
        {
            get { return "~/Content/images/CalculationSummaryGroup.png"; }
        }

        public override string IconClass
        {
            get { return "icon-gears"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationSummaryGroup"/> class.
        /// </summary>
        public CalculationSummaryGroup()
        {
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationSummaryStatistics));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationGridStatistics));            
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.CalculationTimeSeries));
        }
    }
}
