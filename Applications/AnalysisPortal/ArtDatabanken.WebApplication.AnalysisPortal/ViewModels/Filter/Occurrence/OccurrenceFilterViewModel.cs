namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Occurrence
{
    /// <summary>
    /// This class is a view model for the spatial filter page
    /// </summary>
    public class OccurrenceFilterViewModel
    {
        public bool IncludeNeverFoundObservations { get; set; }
        public bool IncludeNotRediscoveredObservations { get; set; }
        public bool IncludePositiveObservations { get; set; }
        public bool IsNaturalOccurrence { get; set; }
        public bool IsNotNaturalOccurrence { get; set; }
        public bool IncludeAbsence { get; set; }
        public bool IncludePresence { get; set; }

        public bool IsSettingsDefault { get; set; }
    }
}
