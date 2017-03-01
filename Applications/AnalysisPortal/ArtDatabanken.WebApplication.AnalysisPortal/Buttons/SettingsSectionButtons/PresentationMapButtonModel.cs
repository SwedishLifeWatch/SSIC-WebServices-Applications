using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.SettingsSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Presentation Map state button.
    /// </summary>
    public class PresentationMapButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationMapButtonModel"/> class.
        /// </summary>
        public PresentationMapButtonModel()
        {
            ShowCheckbox = false;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.PresentationMap; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resource.StateButtonPresentationCoordinateSystem; }
        }

        /// <summary>
        /// Gets the page info.
        /// </summary>
        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Format", "Map"); }
        }

        /// <summary>
        /// Gets the button tooltip.
        /// </summary>
        public override string Tooltip
        {
            get { return Resource.StateButtonPresentationMapTooltip; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is checked.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsChecked
        {
            get
            {
                return SessionHandler.MySettings.Presentation.Map.IsActive;
            }
            set
            {
                SessionHandler.MySettings.Presentation.Map.IsActive = value;                
            }
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
            get { return SessionHandler.MySettings.Presentation.Map.IsSettingsDefault(); }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }    
}