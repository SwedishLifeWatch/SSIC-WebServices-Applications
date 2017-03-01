using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Filter Fields state button.
    /// </summary>
    public class FilterFieldsButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterFieldsButtonModel"/> class.
        /// </summary>
        public FilterFieldsButtonModel()
        {                        
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.FilterFields; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonFilterFields; }
        }
        
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "Field");
            }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonFilterFieldsTooltip; }
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
                return SessionHandler.MySettings.Filter.Field.IsActive;
            }
            set
            {
                SessionHandler.MySettings.Filter.Field.IsActive = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user has made any settings in the Action page.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return SessionHandler.MySettings.Filter.Field.HasSettings; }
        }

        public override bool IsSettingsDefault
        {
            get { return SessionHandler.MySettings.Filter.Field.IsSettingsDefault(); }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}
