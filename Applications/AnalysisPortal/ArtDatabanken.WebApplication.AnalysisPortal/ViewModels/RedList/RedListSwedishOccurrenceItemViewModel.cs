namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListSwedishOccurrenceItemViewModel
    {
        /// <summary>
        /// Swedish Occurrence id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the occurence
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if Swedish Occurence should be used in search criteria
        /// </summary>
        public bool Selected { get; set; }
    }
}
