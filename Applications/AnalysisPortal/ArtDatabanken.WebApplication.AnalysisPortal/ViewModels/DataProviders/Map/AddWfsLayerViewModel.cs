using ArtDatabanken.GIS.WFS.Capabilities;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Shared;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map
{
    /// <summary>
    /// This class is a view model for the DataProviders/AddWfsLayers action
    /// </summary>
    public class AddWfsLayerViewModel
    {
        public WFSCapabilities WfsCapabilities { get; set; }
        public string ServerUrl { get; set; }

        public UploadGeoJsonViewModel UploadGeoJsonViewModel { get; set; }

        public bool ShowFile { get; set; }

        public static AddWfsLayerViewModel Create()
        {
            var model = new AddWfsLayerViewModel();
            model.ServerUrl = "";
            return model;
        }

        public static AddWfsLayerViewModel Create(string url)
        {
            var model = new AddWfsLayerViewModel();
            model.ServerUrl = url;
            return model;
        }

        public static AddWfsLayerViewModel Create(string serverUrl, WFSCapabilities wfsCapabilities)
        {
            var model = new AddWfsLayerViewModel();
            model.WfsCapabilities = wfsCapabilities;
            model.ServerUrl = serverUrl;
            return model;
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
        private FileViewModel[] _files;

        public FileViewModel[] Files
        {
            get
            {
                if (_files == null)
                {
                }
                return _files;
            }

            set
            {
                _files = value;
            }
        }

        /// <summary>
        /// This class holds localized labels for the TaxaSetting class.
        /// </summary>
        public class ModelLabels
        {
            public string Search
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerSearch; }
            }

            public string Title
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerTitle; }
            }

            public string ServerUrl
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerServerUrl; }
            }

            public string SampleUrls
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerSampleUrls; }
            }

            public string WfsVersion
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerWfsVersion; }
            }

            public string WfsLayers
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerWfsLayers; }
            }

            public string HeaderLayerTitle
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerHeaderLayerTitle; }
            }

            public string HeaderLayerNamespace
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerHeaderLayerNamespace; }
            }

            public string HeaderName
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerHeaderName; }
            }

            public string ChooseLayer
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerChooseLayer; }
            }

            public string SelectBoxADbGeoInspire
            {
                get { return Resources.Resource.DataProvidesSearchWfsInspireLayerSelectDefault; }
            }

            public string SelectBoxDefault
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerSelectDefault; }
            }

            public string SelectBoxSLUGeo
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerSelectSLUGeo; }
            }

            public string SelectBoxADbGeo
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerSelectADbGeo; }
            }

            public string SelectBoxSMHIGeo
            {
                get { return Resources.Resource.DataProvidesSearchWfsLayerSelectSMHIGeo; }
            }
        }

        public class FileViewModel
        {
            public string Name { get; set; }

            public string FileName { get; set; }
        }
    }
}
