using System;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.MySettingsSummary
{
    public class MySettingsButtonGroupViewModel
    {
        public bool IsSaveButtonEnabled { get; set; }
        public bool IsLoadButtonEnabled { get; set; }
        public bool IsResetButtonEnabled { get; set; }

        public bool DoesLastSettingsExists { get; set; }
        public DateTime? LastSettingsSaveTime { get; set; }
    }
}
