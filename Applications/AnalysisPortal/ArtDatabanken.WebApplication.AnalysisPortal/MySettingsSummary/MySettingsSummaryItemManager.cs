using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary
{
    /// <summary>
    /// This class contains a static dictionary with MySettingsSummaryItemBase items.    
    /// </summary>
    public static class MySettingsSummaryItemManager
    {
        private static readonly Dictionary<MySettingsSummaryItemIdentifier, MySettingsSummaryItemBase> _itemDictionary;

        /// <summary>
        /// Initializes the <see cref="MySettingsSummaryItemManager"/> class.
        /// The static constructor creates all items and stores them in application cache as long the application is running.
        /// </summary>
        static MySettingsSummaryItemManager()
        {
            _itemDictionary = new Dictionary<MySettingsSummaryItemIdentifier, MySettingsSummaryItemBase>();

            AddSettingSummary(MySettingsSummaryItemIdentifier.DataProviders, new DataProvidersSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.DataEnvironmentalData, new WfsLayersSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.DataMapLayers, new MapLayersSettingSummary());

            AddSettingSummary(MySettingsSummaryItemIdentifier.FilterOccurrence, new OccurrenceSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.FilterTaxa, new TaxaSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.FilterPolygon, new PolygonSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.FilterLocality, new LocalitySettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.FilterRegion, new RegionSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.FilterTemporal, new TemporalSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.FilterAccuracy, new AccuracySettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.FilterField, new FieldSettingSummary());            

            AddSettingSummary(MySettingsSummaryItemIdentifier.CalculationGridStatistics, new GridStatisticsSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.CalculationSummaryStatistics, new SummaryStatisticsSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.CalculationTimeSeries, new TimeSeriesSettingSummary());

            AddSettingSummary(MySettingsSummaryItemIdentifier.PresentationMap, new MapSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.PresentationTable, new TableSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.PresentationReport, new ReportSettingSummary());
            AddSettingSummary(MySettingsSummaryItemIdentifier.PresentationFileFormat, new FileFormatSettingSummary());            
        }

        private static T AddSettingSummary<T>(MySettingsSummaryItemIdentifier identifier, T model) where T : MySettingsSummaryItemBase
        {
            _itemDictionary.Add(identifier, model);
            return model;            
        }


        /// <summary>
        /// Gets the item that corresponds to the identifier
        /// </summary>
        /// <param name="buttonIdentifier">The item identifier.</param>
        /// <returns></returns>
        public static MySettingsSummaryItemBase GetItem(int buttonIdentifier)
        {
            return GetItem((MySettingsSummaryItemIdentifier)buttonIdentifier);
        }

        /// <summary>
        /// Gets the item that corresponds to the identifier
        /// </summary>
        /// <param name="buttonIdentifier">The item identifier.</param>
        /// <returns></returns>
        public static MySettingsSummaryItemBase GetItem(MySettingsSummaryItemIdentifier buttonIdentifier)
        {
            return _itemDictionary[buttonIdentifier];
        }

        /// <summary>
        /// Get data for setting report
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static IDictionary<string, object> GetSettingReportData(IUserContext currentUser)
        {
            var settingsReport = new Dictionary<string, object>();

            // retrieve data providers
            var dataProvidersSettingSummary = (DataProvidersSettingSummary)GetItem(MySettingsSummaryItemIdentifier.DataProviders);
            if (dataProvidersSettingSummary.HasActiveSettings)
            {
                var dataProviders = dataProvidersSettingSummary.GetSettingsSummaryModel().Where(dataProvider => dataProvider.IsSelected).ToList();

                settingsReport.Add("DataProviders", dataProviders);
            }

            // retrieve wfs layers  summary
            var wfsLayersSettingSummary = (WfsLayersSettingSummary)GetItem(MySettingsSummaryItemIdentifier.DataEnvironmentalData);
            if (wfsLayersSettingSummary.HasActiveSettings)
            {
                settingsReport.Add("DataWfsLayers", wfsLayersSettingSummary.GetSummaryList());
            }

            // retrieve wfs layers  summary
            var wmsLayersSettingSummary = (MapLayersSettingSummary)GetItem(MySettingsSummaryItemIdentifier.DataMapLayers);
            if (wmsLayersSettingSummary.HasActiveSettings)
            {
                settingsReport.Add("DataWmsLayers", wmsLayersSettingSummary.GetSummaryList());
            }

            var occurrenceSummary = (OccurrenceSettingSummary)GetItem(MySettingsSummaryItemIdentifier.FilterOccurrence);
            if (occurrenceSummary.HasActiveSettings)
            {
                settingsReport.Add("FilterOccurrence", occurrenceSummary.GetSummaryList());
            }

            // retrieve filter
            var taxaSettingSummary = (TaxaSettingSummary)GetItem(MySettingsSummaryItemIdentifier.FilterTaxa);
            if (taxaSettingSummary.HasActiveSettings)
            {
                var taxonList = taxaSettingSummary.GetSettingsSummaryModel();

                settingsReport.Add("FilterTaxa", taxonList);
            }

            // retrieve regions
            var regionSettingSummary = (RegionSettingSummary)GetItem(MySettingsSummaryItemIdentifier.FilterRegion);
            if (regionSettingSummary.HasActiveSettings)
            {
                var regions = regionSettingSummary.GetSettingsSummaryModel();

                settingsReport.Add("FilterRegion", regions);
            }

            var polygonSettingSummaryr = (PolygonSettingSummary)GetItem(MySettingsSummaryItemIdentifier.FilterPolygon);
            if (polygonSettingSummaryr.HasActiveSettings)
            {
                var polygons = polygonSettingSummaryr.GetSettingsSummaryModel();
                settingsReport.Add("FilterPolygon", polygons);
            }

            // retrieve fields
            var temporalSettingSummary = (TemporalSettingSummary)GetItem(MySettingsSummaryItemIdentifier.FilterTemporal);
            if (temporalSettingSummary.HasActiveSettings)
            {
                var list = temporalSettingSummary.GetSummaryList();

                settingsReport.Add("FilterTemporal", list);
            }

            // retrieve accuracy
            var accuracySettingSummary = (AccuracySettingSummary)GetItem(MySettingsSummaryItemIdentifier.FilterAccuracy);
            if (accuracySettingSummary.HasActiveSettings)
            {
                var list = accuracySettingSummary.GetSummaryList();

                settingsReport.Add("FilterAccuracy", list);
            }

            // retrieve locality
            var localitySettingSummary = (LocalitySettingSummary)GetItem(MySettingsSummaryItemIdentifier.FilterLocality);
            if (localitySettingSummary.HasActiveSettings)
            {
                var summaryModel = localitySettingSummary.GetSettingsSummaryModel();
                var strings = new List<string>
                {
                    string.Format("{0}: \"{1}\"", Resource.FilterLocalitySearchString, summaryModel.LocalityName),
                    string.Format("{0}: \"{1}\"", Resource.FilterLocalitySearchMethod, summaryModel.CompareOperator.ToResourceName())
                };

                settingsReport.Add("FilterLocality", strings);
            }

            // retrieve fields
            var fieldSettingSummary = (FieldSettingSummary)GetItem(MySettingsSummaryItemIdentifier.FilterField);
            if (fieldSettingSummary.HasActiveSettings)
            {
                var fields = fieldSettingSummary.GetSettingsSummaryModel();

                settingsReport.Add("FilterField", fields);
            }            

            // retrieve summary statistics
            var summaryStatisticsSettingSummary = (SummaryStatisticsSettingSummary)GetItem(MySettingsSummaryItemIdentifier.CalculationSummaryStatistics);
            if (summaryStatisticsSettingSummary.HasActiveSettings)
            {
                var summaryStatistics = summaryStatisticsSettingSummary.GetSettingsSummaryModel(currentUser);

                settingsReport.Add("CalculationSummary", summaryStatistics);
            }

            // retrieve grid statistics
            var gridStatisticsSettingSummary = (GridStatisticsSettingSummary)GetItem(MySettingsSummaryItemIdentifier.CalculationGridStatistics);
            if (gridStatisticsSettingSummary.HasActiveSettings)
            {
                var gridStatisticsModel = gridStatisticsSettingSummary.GetSettingsSummaryModel(currentUser);

                settingsReport.Add("CalculationGrid", gridStatisticsModel);
            }

            // retrieve time series summary
            var timeSeriesSettingSummary = (TimeSeriesSettingSummary)GetItem(MySettingsSummaryItemIdentifier.CalculationTimeSeries);
            if (timeSeriesSettingSummary.HasActiveSettings)
            {
                var timeSeries = timeSeriesSettingSummary.GetSettingsSummaryModel(currentUser);

                settingsReport.Add("CalculationTime", timeSeries);
            }

            var presentationMapSettingSummary = (MapSettingSummary)GetItem(MySettingsSummaryItemIdentifier.PresentationMap);
            if (presentationMapSettingSummary.HasActiveSettings)
            {
                var settings = presentationMapSettingSummary.GetSettingsSummary();

                settingsReport.Add("PresentationMap", settings);
            }

            // retrieve fields
            var presentationFileFormatSettingSummary = (FileFormatSettingSummary)GetItem(MySettingsSummaryItemIdentifier.PresentationFileFormat);
            if (presentationFileFormatSettingSummary.HasActiveSettings)
            {
                var fileFormats = presentationFileFormatSettingSummary.GetSettingsSummaryModel();
                
                settingsReport.Add("PresentationFileFormat", fileFormats);
            }

            return settingsReport;
        }
    }
}
