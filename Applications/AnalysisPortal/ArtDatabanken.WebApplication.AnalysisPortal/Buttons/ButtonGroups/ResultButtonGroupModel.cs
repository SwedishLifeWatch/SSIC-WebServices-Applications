using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups
{
    /// <summary>
    /// This class acts as a view model for the Result button group
    /// </summary>
    public class ResultButtonGroupModel : ButtonGroupModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultButtonGroupModel"/> class.
        /// </summary>
        public ResultButtonGroupModel()
        {            
            Buttons.Add(StateButtonManager.ResultViewButton);
            Buttons.Add(StateButtonManager.MapResultViewButton);
            Buttons.Add(StateButtonManager.TableResultViewButton);
            Buttons.Add(StateButtonManager.DiagramResultViewButton);
            Buttons.Add(StateButtonManager.ReportResultViewButton);
            Buttons.Add(StateButtonManager.ResultDownloadButton);
        }

        /// <summary>
        /// Gets the button group title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.ButtonGroupResult; }
        }

        public override PageInfo MainPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "Info"); }
        }

        public override string ImageUrl
        {
            get { return "~/Content/images/table_16.png"; }
        }

        public override string IconClass
        {
            get { return "icon-bar-chart"; }
        }

        /// <summary>
        /// Gets the button group identifier.
        /// </summary>
        public override ButtonGroupIdentifier Identifier
        {
            get { return ButtonGroupIdentifier.Result; }
        }
    }
}
