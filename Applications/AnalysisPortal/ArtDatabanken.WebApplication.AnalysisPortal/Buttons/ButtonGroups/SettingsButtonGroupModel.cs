using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups
{
    /// <summary>
    /// This class acts as a view model for the Calculation button group
    /// </summary>
    public class SettingsButtonGroupModel : ButtonGroupModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsButtonGroupModel"/> class.
        /// </summary>
        public SettingsButtonGroupModel()
        {            
            Buttons.Add(StateButtonManager.CalculationSummaryStatisticsButton);
            Buttons.Add(StateButtonManager.CalculationGridStatisticsButton);            
            Buttons.Add(StateButtonManager.CalculationTimeSeriesButton);
            Buttons.Add(StateButtonManager.PresentationMapButton);
            Buttons.Add(StateButtonManager.PresentationTableButton);
            Buttons.Add(StateButtonManager.PresentationFileFormatButton);
        }

        public override PageInfo MainPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Calculation", "Index"); }
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
        /// Gets the button group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ButtonGroupSettings; }            
        }

        /// <summary>
        /// Gets the button group identifier.
        /// </summary>
        public override ButtonGroupIdentifier Identifier
        {
            get { return ButtonGroupIdentifier.Settings; }
        }
    }
}
