namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListLandscapeTypeItemViewModel
    {
        /// <summary>
        /// Landscape Type id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Landscape Type factor id.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Landscape Type name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if Landscape Type should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}
