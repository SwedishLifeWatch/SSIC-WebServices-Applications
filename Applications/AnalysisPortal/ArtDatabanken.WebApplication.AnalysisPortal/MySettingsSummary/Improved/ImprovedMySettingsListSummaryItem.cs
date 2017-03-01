using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved
{
    /// <summary>
    /// Base class for summary settings that can be viewed as a list of strings.
    /// </summary>
    public abstract class ImprovedMySettingsListSummaryItem : ImprovedMySettingsSummaryItemBase
    {
        protected ImprovedMySettingsListSummaryItem(IUserContext userContext, MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Get summary list.
        /// </summary>
        /// <returns>
        /// A list with settings information.
        /// </returns>
        public abstract ImprovedMySettingsItem<string> GetSummaryList(PageInfo pageInfo);

        public ImprovedMySettingsItem<string> SummaryList { get; set; }
    }
}
