using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Filter Taxa state button
    /// </summary>
    public class ResultDownloadButtonModel : StateButtonModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultDownloadButtonModel"/> class.
        /// </summary>
        public ResultDownloadButtonModel()
        {            
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.ResultDownload; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonResultDownload; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Result", "Download"); }
        }

        public override string Tooltip
        {
            get { return Resource.DownloadPageTooltip; }
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
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return true; }
            //get { return SessionHandler.MySettings.HasSettings; }
        }

        public override bool IsSettingsDefault
        {
            get { return true; }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}
