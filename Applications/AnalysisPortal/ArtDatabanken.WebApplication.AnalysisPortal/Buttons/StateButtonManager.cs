using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.DataSectionButtons;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons;
using ArtDatabanken.WebApplication.AnalysisPortal.Buttons.SettingsSectionButtons;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons
{
    /// <summary>
    /// This static class manages the buttons that is used in different button groups.    
    /// The static constructor creates all buttons and stores them in application cache as long the application is running.
    /// </summary>
    public static class StateButtonManager
    {
        private static readonly Dictionary<StateButtonIdentifier, StateButtonModel> _buttonDictionary;
        private static readonly ResultViewButtonModel _resultViewButtonModel;
        private static readonly ResultDownloadButtonModel _resultDownloadButtonModel;
        private static readonly ReportResultViewButtonModel _reportResultViewButtonModel;
        private static readonly DiagramResultViewButtonModel _diagramResultViewButtonModel;
        private static readonly TableResultViewButtonModel _tableResultViewButtonModel;
        private static readonly MapResultViewButtonModel _mapResultViewButtonModel;
        private static readonly PresentationReportButtonModel _presentationReportButtonModel;
        private static readonly PresentationDiagramButtonModel _presentationDiagramButtonModel;
        private static readonly PresentationMapButtonModel _presentationMapButtonModel;
        private static readonly PresentationTableButtonModel _presentationTableButtonModel;
        private static readonly CalculationTimeSeriesButtonModel _calculationTimeSeriesButtonModel;
        private static readonly CalculationSummaryStatisticsButtonModel _calculationSummaryStatisticsButtonModel;
        private static readonly CalculationGridStatisticsButtonModel _calculationGridStatisticsButtonModel;
        private static readonly FilterQualityButtonModel _filterQualityButtonModel;
        private static readonly FilterOccurrenceButtonModel _filterOccurrenceButtonModel;
        private static readonly FilterTemporalButtonModel _filterTemporalButtonModel;
        private static readonly FilterAccuracyButtonModel _filterAccuracyButtonModel;
        private static readonly FilterSpatialButtonModel _filterSpatialButtonModel;
        private static readonly FilterTaxaButtonModel _filterTaxaButtonModel;
        private static readonly FilterFieldsButtonModel _filterFieldsButtonModel;        
        private static readonly FilterTaxaTaxonByTaxonAttributesButton _filterTaxaTaxonByTaxonAttributesButton;
        private static readonly DataProvidersWmsLayersButtonModel _dataProvidersWmsLayersButtonModel;
        private static readonly DataProvidersWfsLayersButtonModel _dataProvidersWfsLayersButtonModel;
        private static readonly DataProvidersMetadataSearchButtonModel _dataProvidersMetadataSearchButtonModel;
        private static readonly DataProvidersSpeciesObservationButtonModel _dataProvidersSpeciesObservationButtonModel;
        private static readonly PresentationFileFormatButtonModel _presentationFileFormatButtonModel;

        /// <summary>
        /// Initializes the <see cref="StateButtonManager"/> class.
        /// The static constructor creates all buttons and stores them in application cache as long the application is running.
        /// </summary>
        static StateButtonManager()
        {
            _buttonDictionary = new Dictionary<StateButtonIdentifier, StateButtonModel>();

            _dataProvidersSpeciesObservationButtonModel = new DataProvidersSpeciesObservationButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.DataProvidersSpeciesObservation, DataProvidersSpeciesObservationButton);
            _dataProvidersMetadataSearchButtonModel = new DataProvidersMetadataSearchButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.DataProvidersMetadataSearch, DataProvidersMetadataSearchButton);
            _dataProvidersWfsLayersButtonModel = new DataProvidersWfsLayersButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.DataProvidersWfsLayers, DataProvidersWfsLayersButton);
            _dataProvidersWmsLayersButtonModel = new DataProvidersWmsLayersButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.DataProvidersWmsLayers, DataProvidersWmsLayersButton);

            _filterTaxaButtonModel = new FilterTaxaButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.FilterTaxa, FilterTaxaButton);
            _filterSpatialButtonModel = new FilterSpatialButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.FilterSpatial, FilterSpatialButton);
            _filterTemporalButtonModel = new FilterTemporalButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.FilterTemporal, FilterTemporalButton);
            _filterOccurrenceButtonModel = new FilterOccurrenceButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.FilterOccurrence, FilterOccurrenceButton);
            _filterAccuracyButtonModel = new FilterAccuracyButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.FilterAccuracy, FilterAccuracyButton);
            _filterQualityButtonModel = new FilterQualityButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.FilterQuality, FilterQualityButton);
            _filterFieldsButtonModel = new FilterFieldsButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.FilterFields, FilterFieldsButton);            

            _calculationGridStatisticsButtonModel = new CalculationGridStatisticsButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.CalculationGridStatistics, CalculationGridStatisticsButton);
            _calculationSummaryStatisticsButtonModel = new CalculationSummaryStatisticsButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.CalculationSummaryStatistics, CalculationSummaryStatisticsButton);
            _calculationTimeSeriesButtonModel = new CalculationTimeSeriesButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.CalculationTimeSeries, CalculationTimeSeriesButton);            

            _presentationTableButtonModel = new PresentationTableButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.PresentationTable, PresentationTableButton);
            _presentationMapButtonModel = new PresentationMapButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.PresentationMap, PresentationMapButton);
            _presentationDiagramButtonModel = new PresentationDiagramButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.PresentationDiagram, PresentationDiagramButton);
            _presentationReportButtonModel = new PresentationReportButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.PresentationReport, PresentationReportButton);
            _presentationFileFormatButtonModel = new PresentationFileFormatButtonModel();   
            _buttonDictionary.Add(StateButtonIdentifier.PresentationFileFormat, _presentationFileFormatButtonModel);

            _mapResultViewButtonModel = new MapResultViewButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.MapResultView, MapResultViewButton);
            _tableResultViewButtonModel = new TableResultViewButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.TableResultView, TableResultViewButton);
            _diagramResultViewButtonModel = new DiagramResultViewButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.DiagramResultView, DiagramResultViewButton);
            _reportResultViewButtonModel = new ReportResultViewButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.ReportResultView, ReportResultViewButton);
            _resultViewButtonModel = new ResultViewButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.ResultView, ResultViewButton);
            _resultDownloadButtonModel = new ResultDownloadButtonModel();
            _buttonDictionary.Add(StateButtonIdentifier.ResultDownload, ResultDownloadButton);
        }

        public static DataProvidersSpeciesObservationButtonModel DataProvidersSpeciesObservationButton
        {
            get { return _dataProvidersSpeciesObservationButtonModel; }
        }

        public static DataProvidersMetadataSearchButtonModel DataProvidersMetadataSearchButton
        {
            get { return _dataProvidersMetadataSearchButtonModel; }
        }

        public static DataProvidersWfsLayersButtonModel DataProvidersWfsLayersButton
        {
            get { return _dataProvidersWfsLayersButtonModel; }
        }

        public static DataProvidersWmsLayersButtonModel DataProvidersWmsLayersButton
        {
            get { return _dataProvidersWmsLayersButtonModel; }
        }

        public static FilterTaxaButtonModel FilterTaxaButton
        {
            get { return _filterTaxaButtonModel; }
        }

        public static FilterSpatialButtonModel FilterSpatialButton
        {
            get { return _filterSpatialButtonModel; }
        }

        public static FilterTemporalButtonModel FilterTemporalButton
        {
            get { return _filterTemporalButtonModel; }
        }

        public static FilterAccuracyButtonModel FilterAccuracyButton
        {
            get { return _filterAccuracyButtonModel; }
        }

        public static FilterOccurrenceButtonModel FilterOccurrenceButton
        {
            get { return _filterOccurrenceButtonModel; }
        }

        public static FilterQualityButtonModel FilterQualityButton
        {
            get { return _filterQualityButtonModel; }
        }

        public static FilterFieldsButtonModel FilterFieldsButton
        {
            get { return _filterFieldsButtonModel; }
        }        
        
        public static CalculationGridStatisticsButtonModel CalculationGridStatisticsButton
        {
            get { return _calculationGridStatisticsButtonModel; }
        }

        public static CalculationSummaryStatisticsButtonModel CalculationSummaryStatisticsButton
        {
            get { return _calculationSummaryStatisticsButtonModel; }
        }        

        public static CalculationTimeSeriesButtonModel CalculationTimeSeriesButton
        {
            get { return _calculationTimeSeriesButtonModel; }
        }

        public static PresentationTableButtonModel PresentationTableButton
        {
            get { return _presentationTableButtonModel; }
        }

        public static PresentationMapButtonModel PresentationMapButton
        {
            get { return _presentationMapButtonModel; }
        }

        public static PresentationDiagramButtonModel PresentationDiagramButton
        {
            get { return _presentationDiagramButtonModel; }
        }

        public static PresentationFileFormatButtonModel PresentationFileFormatButton
        {
            get { return _presentationFileFormatButtonModel; }
        }

        public static PresentationReportButtonModel PresentationReportButton
        {
            get { return _presentationReportButtonModel; }
        }

        public static MapResultViewButtonModel MapResultViewButton
        {
            get { return _mapResultViewButtonModel; }
        }

        public static TableResultViewButtonModel TableResultViewButton
        {
            get { return _tableResultViewButtonModel; }
        }

        public static DiagramResultViewButtonModel DiagramResultViewButton
        {
            get { return _diagramResultViewButtonModel; }
        }

        public static ReportResultViewButtonModel ReportResultViewButton
        {
            get { return _reportResultViewButtonModel; }
        }

        public static ResultViewButtonModel ResultViewButton
        {
            get { return _resultViewButtonModel; }
        }

        public static ResultDownloadButtonModel ResultDownloadButton
        {
            get { return _resultDownloadButtonModel; }
        }        

        /// <summary>
        /// Gets the button that corresponds to the identifier
        /// </summary>
        /// <param name="buttonIdentifier">The button identifier.</param>
        /// <returns></returns>
        public static StateButtonModel GetButton(int buttonIdentifier)
        {
            return GetButton((StateButtonIdentifier)buttonIdentifier);
        }

        /// <summary>
        /// Gets the button that corresponds to the identifier
        /// </summary>
        /// <param name="buttonIdentifier">The button identifier.</param>
        /// <returns></returns>
        public static StateButtonModel GetButton(StateButtonIdentifier buttonIdentifier)
        {
            return _buttonDictionary[buttonIdentifier];
        }
    }
}
