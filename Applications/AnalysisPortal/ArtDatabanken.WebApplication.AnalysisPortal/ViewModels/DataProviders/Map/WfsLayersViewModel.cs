using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Labels;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map
{
    /// <summary>
    /// This class is a view model for the Dataproviders/MapLayers action
    /// </summary>
    public class WfsLayersViewModel
    {
        public bool IsSettingsDefault { get; set; }

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

        private ModelLabels _labels;

        /// <summary>
        /// This class holds localized labels for the TaxaSetting class.
        /// </summary>
        public class ModelLabels
        {
            public string DeleteWfsLayerTitle { get { return Resources.Resource.DataProvidersMapLayersDeleteWfsLayerTitle; } }
            public string DeleteWfsLayerMessage { get { return Resources.Resource.DataProvidersMapLayersDeleteWfsLayerMessage; } }            
            public string Title { get { return Resources.Resource.StateButtonDataProvidersWfsLayers; } }
            public string CreateNewLayer { get { return Resources.Resource.DataProvidersMapLayersCreateNewLayer; } }
            public string CreateNewLayerTooltip { get { return Resources.Resource.DataProvidersMapLayersCreateNewLayerTooltip; } }
            public string EditLayer { get { return Resources.Resource.DataProvidesMapLayersEditLayer; } }
            public string EditLayerTooltip { get { return Resources.Resource.DataProvidesMapLayersEditLayerTooltip; } }
            public string RemoveLayer { get { return Resources.Resource.DataProvidesMapLayersRemoveLayer; } }
            public string RemoveLayerTooltip { get { return Resources.Resource.DataProvidesMapLayersRemoveLayerTooltip; } }
            public string WfsLayersGridTitle { get { return Resources.Resource.DataProvidesMapLayersWfsLayersGridTitle; } }
        }
    }
}
