using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    public class RedListHostItemViewModel
    {
        /// <summary>
        /// Host id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Host name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if Host should be used in search criteria.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Taxon children.
        /// </summary>
        public IEnumerable<RedListHostItemViewModel> Children { get; set; }
    }
}
