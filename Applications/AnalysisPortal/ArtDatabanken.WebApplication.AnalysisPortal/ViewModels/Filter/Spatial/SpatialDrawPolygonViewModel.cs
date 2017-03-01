namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial
{
    /// <summary>
    /// This class is a view model for the spatial filter page
    /// </summary>
    public class SpatialDrawPolygonViewModel
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

        public bool IsSettingsDefault { get; set; }

        private ModelLabels _labels;

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.Resource.FilterSpatialDrawPolygonTitle; } }            
        }
    }
}
