using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation
{
    /// <summary>
    /// ViewModel handling time series settings
    /// </summary>
    public class TimeSeriesSettingsViewModel 
    {
        public bool IsSettingsDefault { get; set; }

        public int DefaultPeriodicityIndex { get; set; }
    }
}
