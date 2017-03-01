namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Home
{
    /// <summary>
    /// View model for Home/VersionNumber
    /// </summary>
    public class VersionNumberViewModel
    {
        /// <summary>
        /// Gets or sets the CreationDate.
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// Server name.
        /// </summary>        
        public string ServerName { get; set; }

        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels class
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel
            {
                get { return Resources.Resource.HomeVersionNumberTitle; }
            }

            public string VersionLabel
            {
                get { return Resources.Resource.HomeVersionNumberVersion; }
            }

            public string DateLabel
            {
                get { return Resources.Resource.HomeVersionNumberDate; }
            }
        }
    }
}
