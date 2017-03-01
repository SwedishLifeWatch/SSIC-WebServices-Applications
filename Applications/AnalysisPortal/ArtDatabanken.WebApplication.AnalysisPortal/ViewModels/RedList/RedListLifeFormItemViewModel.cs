namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListLifeFormItemViewModel
    {
        /// <summary>
        /// Life form id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Life form name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if life form should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }
    }
}
