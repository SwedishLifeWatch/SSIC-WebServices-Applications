namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.About
{
    /// <summary>
    /// Class that holds instructive information about how to to things in the analysis portal.
    /// </summary>
    public class AboutItem
    {
        /// <summary>
        /// The header text for this about item.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// The short descriptive text about this about item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Url to EpiServer where more information about this about item can be read.
        /// </summary>
        public string ReadMoreLinkUrl { get; set; }

        /// <summary>
        /// The label for the link to EpiServer where more information about this about item can be read.
        /// </summary>
        public string ReadMoreLinkLabel { get; set; }

        /// <summary>
        /// The hint text for the link to EpiServer where more information about this about item can be read.
        /// </summary>
        public string ReadMoreLinkHint { get; set; }
    }
}
