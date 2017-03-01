namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListCategoryItemViewModel
    {
        /// <summary>
        /// RedList Category id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// RedList Category factor id.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// RedList Category name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if RedList Category should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// True if RedList Category is in the red listed group.
        /// </summary>
        public bool InRedListedGroup { get; set; }

        /// <summary>
        /// True if RedList Category is in the special list group
        /// </summary>
        public bool InOtherGroup { get; set; }
    }
}
