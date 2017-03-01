using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved
{
    /// <summary>
    /// This class is an abstract base class for MySettings summary items.
    /// </summary>
    public abstract class ImprovedMySettingsSummaryItemBase
    {
        protected readonly IUserContext UserContext;
        protected readonly AnalysisPortal.MySettings.MySettings MySettings;

        protected ImprovedMySettingsSummaryItemBase(IUserContext userContext, MySettings.MySettings mySettings)
        {
            UserContext = userContext;
            MySettings = mySettings;
        }

        /// <summary>
        /// Gets the identifier for this class.
        /// </summary>
        public abstract MySettingsSummaryItemIdentifier Identifier { get; }

        public abstract MySettingsSummaryItemIdentifierModel IdentifierModel { get; }

        public abstract List<MySettingsSummaryItemSubIdentifier> SubIdentifiers { get; }

        public abstract List<MySettingsSummaryItemIdentifier> SubIdentifiers2 { get; }

        public bool CanSettingAffectResult(ResultType resultType)
        {
            return MySettingsSummaryItemIdentifierManager.CanSettingAffectResult(resultType, Identifier);
        }

        public bool CanSubSettingsAffectResult(ResultType resultType, MySettingsSummaryItemSubIdentifier subIdentifier)
        {
            return MySettingsSummaryItemIdentifierManager.CanSubsettingAffectResult(resultType, subIdentifier);
        }

        public List<MySettingsSummaryItemSubIdentifier> GetUsedSubSettingsInResult(ResultType resultType)
        {
            return MySettingsSummaryItemIdentifierManager.GetSubsettingsThatCanAffectResult(resultType, SubIdentifiers);
        }

        public bool CanSubSettingsAffectResult2(ResultType resultType, MySettingsSummaryItemIdentifier subIdentifier)
        {
            return MySettingsSummaryItemIdentifierManager.CanSubsettingAffectResult2(resultType, subIdentifier);
        }

        public List<MySettingsSummaryItemIdentifier> GetUsedSubSettingsInResult2(ResultType resultType)
        {
            return MySettingsSummaryItemIdentifierManager.GetSubsettingsThatCanAffectResult2(resultType, SubIdentifiers2);
        }

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
        public abstract bool IsActive { get; }

        /// <summary>
        /// Gets a value indicating whether any settings has been done.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasSettings { get; }

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
    }
}
