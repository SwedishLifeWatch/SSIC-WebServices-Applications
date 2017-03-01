using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary
{
    /// <summary>
    /// Base class for summary settings that can be viewed as a list of strings.
    /// </summary>
    public abstract class MySettingsListSummaryItem : MySettingsSummaryItemBase
    {
        /// <summary>
        /// Get summary list.
        /// </summary>
        /// <returns>
        /// A list with settings information.
        /// </returns>
        public abstract List<string> GetSummaryList();
    }
}
