using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.SettingsSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Presentation Table state button
    /// </summary>
    public class PresentationTableButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationTableButtonModel"/> class.
        /// </summary>
        public PresentationTableButtonModel()
        {
            ShowCheckbox = false;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.PresentationTable; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonPresentationTable; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Format", "SpeciesObservationTable"); }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonPresentationTableTooltip; }
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
                return SessionHandler.MySettings.Presentation.Table.IsActive;
            }
            set
            {
                SessionHandler.MySettings.Presentation.Table.IsActive = value;
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
            //get { return SessionHandler.MySettings.Presentation.Table.IsSettingsDefault(); }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}
