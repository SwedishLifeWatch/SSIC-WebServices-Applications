using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Labels;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.MetadataSearch
{
    /// <summary>
    /// This class is a view model for the Datasources/MapLayers action
    /// </summary>
    public class MetadataSearchViewModel
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

        private ModelLabels _labels;

        /// <summary>
        /// This class holds localized labels for the MetadataSearch class.
        /// </summary>
        public class ModelLabels
        {
            public string Title { get { return Resources.Resource.StateButtonDataProvidersMetadataSearch; } }
            public string PageDescription { get { return Resources.Resource.MetadataSearchDescription; } }
            public string ECDSLink { get { return Resources.Resource.MetadatasearchEcdsLinkText; } }
            public string GeodataLink { get { return Resources.Resource.MetadatasearchGeodataLinkText; } }
            public string BioCatLink { get { return Resources.Resource.MetadatasearchBiodiversityCatLinkText; } }
            public string SMHIOpenDataLink { get { return Resources.Resource.MetadatasearchSMHIOpenDataLinkText; } }
            public string SGUGeoServerLink { get { return Resources.Resource.MetadatasearchSGUGeoServerLinkText; } }

            //public string Title = "Metadata Search";
        }
    }
}
