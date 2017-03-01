using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.SettingsSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Presentation File format state button.
    /// </summary>
    public class PresentationFileFormatButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFileFormatButtonModel"/> class.
        /// </summary>
        public PresentationFileFormatButtonModel()
        {
            ShowCheckbox = false;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.PresentationFileFormat; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonPresentationFileFormat; }
        }

        /// <summary>
        /// Gets the page info.
        /// </summary>
        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Format", "FileFormat"); }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonPresentationFileFormatTooltip; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsChecked
        {
            get { return true; }
            set { }
        }

        /// <summary>
        /// Gets a value indicating whether the user has made any settings in the Action page
        /// </summary>
        /// <value>
        /// <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get
            {
                return false; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the related settings to this buttons is default settings.
        /// </summary>
        public override bool IsSettingsDefault
        {
            get
            {
                return true;
                //return SessionHandler.MySettings.Presentation.Map.IsSettingsDefault();
            }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }    
}
