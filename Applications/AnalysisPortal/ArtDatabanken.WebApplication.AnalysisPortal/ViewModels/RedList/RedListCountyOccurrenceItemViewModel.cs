namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListCountyOccurrenceItemViewModel
    {
        /// <summary>
        /// County Occurrence id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// County Occurrence factor id.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// County Occurrence name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if County Occurrence should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}
