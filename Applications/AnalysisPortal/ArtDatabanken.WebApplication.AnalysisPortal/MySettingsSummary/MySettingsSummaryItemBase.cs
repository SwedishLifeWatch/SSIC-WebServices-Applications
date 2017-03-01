using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary
{
    /// <summary>
    /// This class is an abstract base class for MySettings summary items.
    /// </summary>
    public abstract class MySettingsSummaryItemBase
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Gets the page info.
        /// </summary>
        public abstract PageInfo PageInfo { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has settings summary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has settings summary; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasSettingsSummary { get; }

        /// <summary>
        /// Gets the settings summary view width.
        /// If null, use default.
        /// </summary>
        public abstract int? SettingsSummaryWidth { get; }

        /// <summary>
        /// Gets a value indicating whether this setting is active.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsActive { get; set; }

        /// <summary>
        /// Gets a value indicating whether any settings has been done.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasSettings { get; }
        
        /// <summary>
        /// Gets the identifier for this class.
        /// </summary>
        public abstract MySettingsSummaryItemIdentifier Identifier { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has active settings.
        /// I.e. the item is active and has settings.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has active settings; otherwise, <c>false</c>.
        /// </value>
        public bool HasActiveSettings
        {
            get { return IsActive && HasSettings; }
        }

        /// <summary>
        /// True if setting can be deactivated by user
        /// </summary>
        public bool SupportDeactivation { get; protected set; }
    }
}
