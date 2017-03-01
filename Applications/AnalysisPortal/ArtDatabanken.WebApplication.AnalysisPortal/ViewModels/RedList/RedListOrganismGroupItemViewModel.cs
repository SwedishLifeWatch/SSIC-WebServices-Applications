namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListOrganismGroupItemViewModel
    {
        /// <summary>
        /// Organism group factor id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Organism group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if organism group should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}
