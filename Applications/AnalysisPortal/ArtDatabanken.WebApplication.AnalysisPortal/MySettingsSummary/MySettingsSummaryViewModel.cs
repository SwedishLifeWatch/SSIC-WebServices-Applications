using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Presentation;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary
{
    /// <summary>
    /// This class is a view model for the MySettingSummary partial view
    /// </summary>
    public class MySettingsSummaryViewModel
    {
        /// <summary>
        /// Gets the setting groups.
        /// </summary>
        public List<MySettingsSummaryGroupBase> SettingGroups { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MySettingsSummaryViewModel"/> class.
        /// </summary>
        public MySettingsSummaryViewModel()
        {
            SettingGroups = new List<MySettingsSummaryGroupBase>();
            SettingGroups.Add(new DataProvidersSummaryGroup());
            SettingGroups.Add(new FilterSummaryGroup());
            SettingGroups.Add(new SettingsSummaryGroup());            
        }
    }
}
