using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved
{
    public class ImprovedMySettingsItem<T>
    {
        public T Setting { get; set; }

        bool? IsUsedByResult { get; set; }

        public ImprovedMySettingsItem(T setting, bool? isUsedByResult)
        {
            Setting = setting;
            IsUsedByResult = isUsedByResult;
        }

        public ImprovedMySettingsItem(T setting)
        {
            Setting = setting;
        }
    }
}
