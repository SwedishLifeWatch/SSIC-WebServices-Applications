using System.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Presentation
{
    /// <summary>
    /// This class is the MySettings summary group for all Data sources settings
    /// </summary>
    public class PresentationSummaryGroup : MySettingsSummaryGroupBase
    {
        /// <summary>
        /// Gets the group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.HeadMenuPresentation; }
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
        /// Initializes a new instance of the <see cref="CalculationSummaryGroup"/> class.
        /// </summary>
        public PresentationSummaryGroup()
        {
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationMap));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationTable));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationReport));
            Items.Add(MySettingsSummaryItemManager.GetItem(MySettingsSummaryItemIdentifier.PresentationFileFormat));
        }
    }
}
