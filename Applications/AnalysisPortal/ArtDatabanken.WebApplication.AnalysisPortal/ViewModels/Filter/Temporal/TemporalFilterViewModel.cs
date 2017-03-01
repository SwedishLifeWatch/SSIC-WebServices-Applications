using System;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Temporal
{
    public class TemporalFilterDateViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool UseSetting { get; set; }
        public bool Annually { get; set; }
    }

    /// <summary>
    /// This class is a view model for the temporal filter page
    /// </summary>
    public class TemporalFilterViewModel
    {
        public TemporalFilterDateViewModel ObservationDate { get; set; }
        public TemporalFilterDateViewModel RegistrationDate { get; set; }
        public TemporalFilterDateViewModel ChangeDate { get; set; }      
        public bool IsSettingsDefault { get; set; }

        /// <summary>
        /// Indicates whether all temporal settings are disabled.
        /// </summary>
        public bool IsAllTemporalSettingsDisabled { get; set; }
    }
}
