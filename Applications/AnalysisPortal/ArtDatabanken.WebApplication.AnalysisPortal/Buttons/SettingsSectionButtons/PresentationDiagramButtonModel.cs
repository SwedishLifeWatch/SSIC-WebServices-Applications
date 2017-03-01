using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.SettingsSectionButtons
{
    // Not yet fully implemented
    public class PresentationDiagramButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationDiagramButtonModel"/> class.
        /// </summary>
        public PresentationDiagramButtonModel()
        {            
            this.IsEnabled = false;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.PresentationDiagram; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonPresentationDiagram; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Home", "NotImplemented"); }
        }

        public override string Tooltip
        {
            get { return ""; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is checked.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsChecked
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user has made any settings in the Action page
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return false; }
        }

        public override bool IsSettingsDefault
        {
            get { return true; }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}
