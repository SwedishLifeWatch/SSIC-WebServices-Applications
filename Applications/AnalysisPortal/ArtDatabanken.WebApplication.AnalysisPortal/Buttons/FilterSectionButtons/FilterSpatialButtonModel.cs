using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    // Not yet fully implemented
    public class FilterSpatialButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSpatialButtonModel"/> class.
        /// </summary>
        public FilterSpatialButtonModel()
        {                        
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.FilterSpatial; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonFilterSpatial; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "Spatial"); }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonFilterSpatialTooltip; }
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
                return SessionHandler.MySettings.Filter.Spatial.IsActive;
            }
            set
            {
                SessionHandler.MySettings.Filter.Spatial.IsActive = value;
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
            get
            {
                return SessionHandler.MySettings.Filter.Spatial.HasSettings;
            }
        }

        public override bool IsSettingsDefault
        {
            get { return SessionHandler.MySettings.Filter.Spatial.IsSettingsDefault(); }
        }

        protected List<ButtonModelBase> _buttons = new List<ButtonModelBase>();

        public override List<ButtonModelBase> Children
        {
            get
            {
                if (_buttons.IsEmpty())
                {
                    _buttons.Add(new FilterSpatialDrawPolygonButtonModel());
                    _buttons.Add(new FilterSpatialCommonRegionsButtonModel());
                    _buttons.Add(new FilterSpatialMapLayersButtonModel());
                    _buttons.Add(new FilterSpatialLocalityButtonModel());
                }

                return _buttons;
            }
        }
    }
}
