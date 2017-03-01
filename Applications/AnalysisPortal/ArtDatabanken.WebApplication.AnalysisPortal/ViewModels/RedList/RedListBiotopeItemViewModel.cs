namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListBiotopeItemViewModel
    {
        /// <summary>
        /// Biotope id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Biotope name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if Biotope should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}
