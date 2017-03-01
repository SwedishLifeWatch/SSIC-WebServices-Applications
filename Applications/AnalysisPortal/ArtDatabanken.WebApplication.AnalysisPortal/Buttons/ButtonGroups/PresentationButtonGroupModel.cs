using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups
{
    /// <summary>
    /// This class acts as a view model for the Presentation button group
    /// </summary>
    public class PresentationButtonGroupModel : ButtonGroupModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationButtonGroupModel"/> class.
        /// </summary>
        public PresentationButtonGroupModel()
        {
            Buttons.Add(StateButtonManager.PresentationMapButton);
            Buttons.Add(StateButtonManager.PresentationTableButton);
            Buttons.Add(StateButtonManager.PresentationFileFormatButton);
            //Buttons.Add(StateButtonManager.PresentationDiagramButton);
            //Buttons.Add(StateButtonManager.PresentationReportButton);
        }

        /// <summary>
        /// Gets the button group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ButtonGroupPresentation; }            
        }

        public override PageInfo MainPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "Index"); }
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
        /// Gets the button group identifier.
        /// </summary>
        public override ButtonGroupIdentifier Identifier
        {
            get { return ButtonGroupIdentifier.Presentation; }
        }
    }
}
