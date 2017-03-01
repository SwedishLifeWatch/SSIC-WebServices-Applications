using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary
{
    /// <summary>
    /// This class is a view model for the MySettingSummary partial view
    /// </summary>
    public class MySettingsReportViewModel
    {
        public IDictionary<string, object> Data { get; private set; } 
        
        public MySettingsReportViewModel(IUserContext currentUser)
        {
            Data = MySettingsSummaryItemManager.GetSettingReportData(currentUser);
        }
    }
}
