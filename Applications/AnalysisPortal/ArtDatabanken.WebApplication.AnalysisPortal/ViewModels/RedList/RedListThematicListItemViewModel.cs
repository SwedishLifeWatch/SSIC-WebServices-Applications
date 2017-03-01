namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListThematicListItemViewModel
    {
        /// <summary>
        /// Thematic list id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Thematic list name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if thematic list should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}
