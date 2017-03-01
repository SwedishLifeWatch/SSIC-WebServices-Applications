using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.DataSectionButtons
{
    public class DataProvidersWfsLayersButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvidersWfsLayersButtonModel"/> class.
        /// </summary>
        public DataProvidersWfsLayersButtonModel()
        {                        
            ShowCheckbox = false;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.DataProvidersWfsLayers; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonDataProvidersWfsLayers; }
        }

        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Data", "WfsLayers");
            }
        }

        public override PageInfo DynamicPageInfo
        {
            get
            {
                if (SessionHandler.MySettings.DataProvider.MapLayers.IsWfsSettingsDefault())
                {
                    return PageInfoManager.GetPageInfo("Data", "AddWfsLayer");
                }

                return PageInfoManager.GetPageInfo("Data", "WfsLayers");
            }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonDataProvidersWfsLayersTooltip; }
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
            //get { return SessionHandler.MySettings.DataProvider.MapLayers.IsWfsSettingsDefault(); }
        }

        public override List<ButtonModelBase> Children { get { return null; } }        
    }
}
