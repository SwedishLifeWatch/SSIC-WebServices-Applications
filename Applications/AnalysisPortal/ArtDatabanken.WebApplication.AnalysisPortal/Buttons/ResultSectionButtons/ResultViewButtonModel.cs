using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Filter Taxa state button
    /// </summary>
    public class ResultViewButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultViewButtonModel"/> class.
        /// </summary>
        public ResultViewButtonModel()
        {            
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.ResultView; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonResultView; }
        }

        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Result", "Index");
            }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonResultViewTooltip; }
        }

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
        /// TODO: Make a better check if any presentation "format" is checked
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes and checked presentation format; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get
            {
                return SessionHandler.MySettings.HasSettings && (SessionHandler.MySettings.Presentation.Map.IsActive 
                                                                 || SessionHandler.MySettings.Presentation.Table.IsActive
                                                                 || SessionHandler.MySettings.Presentation.Report.IsActive);
            }
        }

        public override bool IsSettingsDefault
        {
            get { return true; }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}
