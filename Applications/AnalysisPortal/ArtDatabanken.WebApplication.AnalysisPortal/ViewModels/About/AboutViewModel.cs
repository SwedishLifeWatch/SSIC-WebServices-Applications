using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About
{
    /// <summary>
    /// Viewmodel for information about a part of the application.
    /// </summary>
    public class AboutViewModel
    {
        /// <summary>
        /// The title label of this about view.
        /// </summary>
        public string TitleLabel { get; set; }

        /// <summary>
        /// The summarized description of the content in this about view.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The List of about items belonging to this about view.
        /// </summary>
        public List<AboutItem> Items { get; set; }
    }
}
