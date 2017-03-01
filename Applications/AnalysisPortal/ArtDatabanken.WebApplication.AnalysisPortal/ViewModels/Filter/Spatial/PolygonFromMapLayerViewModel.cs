using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Labels;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial
{
    /// <summary>
    /// This class is a view model for the spatial filter page
    /// </summary>
    public class PolygonFromMapLayerViewModel
    {
        public SharedLabels SharedLabels
        {
            get { return SharedLabels.Instance; }
        }

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
            public string SelectPolygons { get { return Resources.Resource.FilterPolygonFromMapLayerSelectPolygons; } }
            public string AddSelectedPolygons { get { return Resources.Resource.FilterPolygonFromMapLayerAddPolygons; } }
            public string TitleLabel { get { return Resources.Resource.FilterPolygonFromMapLayersTitle; } }
        }
    }
}
