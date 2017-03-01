namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListImpactItemViewModel
    {
        /// <summary>
        /// Impact id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Impact name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if impact should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}
