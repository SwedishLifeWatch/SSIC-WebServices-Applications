using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.SettingsSectionButtons
{
    // Not yet fully implemented
    public class CalculationSummaryStatisticsButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationSummaryStatisticsButtonModel"/> class.
        /// </summary>
        public CalculationSummaryStatisticsButtonModel()
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
            get { return StateButtonIdentifier.CalculationSummaryStatistics; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resource.StateButtonCalculationSummaryStatistics; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Calculation", "SummaryStatistics"); }
        }

        public override string Tooltip
        {
            get { return Resource.StateButtonCalculationSummaryStatisticsToolTip; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is checked.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsChecked
        {
            get { return SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive; }
            set { SessionHandler.MySettings.Calculation.SummaryStatistics.IsActive = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the user has made any settings in the Action page
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return false; } // return SessionHandler.MySettings.Calculation.SummaryStatistics.HasSettings; }
        }

        public override bool IsSettingsDefault
        {
            get { return SessionHandler.MySettings.Calculation.SummaryStatistics.IsSettingsDefault(); }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}
