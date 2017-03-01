using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Filters;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter
{
    /// <summary>
    /// This class contains settings summary for locality settings.
    /// </summary>
    public class LocalitySettingSummary : MySettingsSummaryItemBase
    {
        public LocalitySettingSummary()
        {
            SupportDeactivation = true;
        }

        /// <summary>
        /// Gets the spatial setting.
        /// </summary>
        /// <value>
        /// The spatial setting.
        /// </value>
        private SpatialSetting SpatialSetting
        {
            get { return SessionHandler.MySettings.Filter.Spatial; }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get
            {
                string str = string.Format("{0}: \"{1}\"", Resource.MySettingsFilterLocality, SpatialSetting.Locality.LocalityName ?? "-");
                return str;
            }
        }

        /// <summary>
        /// Gets the page info.
        /// </summary>
        public override PageInfo PageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "Locality"); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has settings summary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has settings summary; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettingsSummary
        {
            get { return IsActive && HasSettings; }
        }

        /// <summary>
        /// Gets the settings summary model.
        /// </summary>
        /// <returns>A model with data from MySettings.</returns>
        public LocalityViewModel GetSettingsSummaryModel()
        {
            LocalityViewManager viewManager = new LocalityViewManager(CoreData.UserManager.GetCurrentUser(), SessionHandler.MySettings);
            LocalityViewModel model = viewManager.CreateLocalityViewModel();
            return model;
        }

        /// <summary>
        /// Gets the settings summary view width.
        /// If null, use default.
        /// </summary>
        public override int? SettingsSummaryWidth
        {
            get { return null; }
        }

        /// <summary>
        /// Gets a value indicating whether this setting is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public override bool IsActive
        {
            get { return SpatialSetting.IsActive; }
            set { SpatialSetting.IsActive = value; }
        }

        /// <summary>
        /// Gets a value indicating whether any settings has been done.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return !string.IsNullOrEmpty(SpatialSetting.Locality.LocalityName); }
        }

        /// <summary>
        /// Gets the identifier for this class.
        /// </summary>
        public override MySettingsSummaryItemIdentifier Identifier
        {
            get { return MySettingsSummaryItemIdentifier.FilterLocality; }
        }
    }
}
