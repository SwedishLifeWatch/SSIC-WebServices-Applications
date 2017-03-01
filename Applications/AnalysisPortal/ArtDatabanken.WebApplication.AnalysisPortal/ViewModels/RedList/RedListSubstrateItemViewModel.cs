namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListSubstrateItemViewModel
    {
        /// <summary>
        /// Substrate id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Substrate name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if substrate should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}
