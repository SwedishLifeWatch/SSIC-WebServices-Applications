using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    // Not yet fully implemented
    public class FilterQualityButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterQualityButtonModel"/> class.
        /// </summary>
        public FilterQualityButtonModel()
        {            
            this.IsEnabled = false;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.FilterQuality; }
        }

        public override PageInfo StaticPageInfo
        {            
            get
            {
                return PageInfoManager.GetPageInfo("Filter", "Quality");
            }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonFilterQuality; }
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
            get { return SessionHandler.MySettings.Filter.Quality.IsSettingsDefault(); }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}
