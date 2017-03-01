namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Account
{
    /// <summary>
    /// View Model for the AccessIsNotAllowed View
    /// </summary>
    public class AccessIsNotAllowedViewModel
    {
        public string Url { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }
        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string Title { get { return Resources.Resource.AccountAccessIsNotAllowedTitle; } }
            public string AccessIsNotAllowed { get { return Resources.Resource.AccountAccessIsNotAllowedDescription; } }
        }
    }
}
