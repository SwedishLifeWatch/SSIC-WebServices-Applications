namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Quality
{
    /// <summary>
    /// This class is a view model for the spatial filter page
    /// </summary>
    public class QualityFilterViewModel
    {
        /// <summary>
        /// Gets the model labels.
        /// </summary>
        public ModelLabels Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new ModelLabels();
                }

                return _labels;
            }
        }
        private ModelLabels _labels;

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string Title { get { return Resources.Resource.FilterQualityTitle; } }
            
            //public string aa { get { return Resources.Resource.aa; } }
        }
    }
}
