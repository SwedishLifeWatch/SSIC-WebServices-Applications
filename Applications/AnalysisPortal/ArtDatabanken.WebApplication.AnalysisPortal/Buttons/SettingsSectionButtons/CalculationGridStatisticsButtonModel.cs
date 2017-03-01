using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.SettingsSectionButtons
{
    // Not yet fully implemented    
    public class CalculationGridStatisticsButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationGridStatisticsButtonModel"/> class.
        /// </summary>
        public CalculationGridStatisticsButtonModel()
        {                        
            this.IsEnabled = true;
            this.ShowCheckbox = false;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.CalculationGridStatistics; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Calculation", "GridStatistics"); }
        }

        public override string Tooltip
        {
            get { return Resource.StateButtonCalculationGridStatisticsTooltip; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resource.StateButtonCalculationGridStatistics; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is checked.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsChecked
        {
            get { return SessionHandler.MySettings.Calculation.GridStatistics.IsActive; }
            set { SessionHandler.MySettings.Calculation.GridStatistics.IsActive = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the user has made any settings in the Action page
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return SessionHandler.MySettings.Calculation.GridStatistics.HasSettings; }
        }

        public override bool IsSettingsDefault
        {
            get { return SessionHandler.MySettings.Calculation.GridStatistics.IsSettingsDefault(); }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}
