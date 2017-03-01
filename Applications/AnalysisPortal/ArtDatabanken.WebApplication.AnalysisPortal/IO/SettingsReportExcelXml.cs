using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of the current settings.
    /// </summary>
    public class SettingsReportExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Initialize SettingsReportExcelXml
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="createWoorkbook"></param>
        public SettingsReportExcelXml(IUserContext currentUser, bool createWoorkbook)
        {
            var rowsXml = new StringBuilder();
            var rowCount = 0;
            var settingsData = MySettingsSummaryItemManager.GetSettingReportData(currentUser);

            foreach (var settingReport in settingsData)
            {
                List<string> calculateStrings;

                switch (settingReport.Key)
                {
                    case "DataProviders":
                        var dataProviders = (List<DataProviderViewModel>)settingReport.Value;

                        rowCount += dataProviders.Count + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.DataProvidersDataProvidersDataProvider));
                        rowsXml.AppendLine(GetColumnNameRowLine("Organisation"));
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.DataProvidersDataProvidersNumberOfObservations));
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.DataProvidersDataProvidersNumberOfPublicObservations));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var row in dataProviders)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", row.Name));
                            rowsXml.AppendLine(GetDataRowLine("String", row.Organization));
                            rowsXml.AppendLine(GetDataRowLine("Number", row.NumberOfObservations.ToString()));
                            rowsXml.AppendLine(GetDataRowLine("Number", row.NumberOfPublicObservations.ToString()));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;

                    case "DataWfsLayers":
                        var wfsLayersList = (List<string>)settingReport.Value;

                        rowCount += wfsLayersList.Count + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.DataWfsLayer));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var item in wfsLayersList)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", item));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "DataWmsLayers":
                        var wmsLayersList = (List<string>)settingReport.Value;

                        rowCount += wmsLayersList.Count + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.StateButtonDataProvidersWfsLayers));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var item in wmsLayersList)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", item));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "FilterOccurrence":
                        var occurrenceList = (List<string>)settingReport.Value;

                        rowCount += occurrenceList.Count + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.StateButtonFilterOccurrence));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var item in occurrenceList)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", item));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());
                        break;
                    case "FilterTaxa":
                        var taxonList = (List<TaxonViewModel>)settingReport.Value;

                        rowCount += taxonList.Count + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.TaxonSharedTaxonId));
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.TaxonSharedScientificName));
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.TaxonSharedSwedishName));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var row in taxonList)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", row.TaxonId.ToString()));
                            rowsXml.AppendLine(GetDataRowLine("String", row.ScientificName));
                            rowsXml.AppendLine(GetDataRowLine("String", row.CommonName));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "FilterRegion":
                        var regions = (List<RegionViewModel>)settingReport.Value;

                        rowCount += regions.Count + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine("Region"));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var row in regions)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", row.Name));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "FilterTemporal":
                        var temporalList = (List<string>)settingReport.Value;

                        rowCount += temporalList.Count + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.StateButtonFilterTemporal));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var item in temporalList)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", item));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "FilterAccuracy":
                        var accuracyList = (List<string>)settingReport.Value;

                        rowCount += accuracyList.Count + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.StateButtonFilterAccuracy));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var item in accuracyList)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", item));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "FilterField":
                        var fields = (IEnumerable<string>)settingReport.Value;

                        rowCount += fields.Count() + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.SettingsReportFieldFilter));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var field in fields)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", field));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "FilterRedList":
                        var categories = (IEnumerable<string>)settingReport.Value;
                        
                        rowCount += categories.Count() + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.SettingsReportRedListFilter));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var category in categories)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", category));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "CalculationSummary":
                        var summaryStatistics = (SummaryStatisticsViewModel)settingReport.Value;

                        calculateStrings = summaryStatistics.GetCalculateStrings();

                        rowCount += calculateStrings.Count + 5;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.SummaryStatisticsCalculateNumberOfObservations));
                        rowsXml.AppendLine(GetRowEnd());

                        // Calculate strings data values
                        foreach (var str in calculateStrings)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", str));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        //Empty row
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.GridStatisticsEnvironmentalData));
                        rowsXml.AppendLine(GetRowEnd());

                        // Summary statistics environmental data
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetDataRowLine("String", Resource.SharedLayer + ": " + summaryStatistics.GetSelectedWfsLayerName()));
                        rowsXml.AppendLine(GetRowEnd());

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "CalculationGrid":
                        var gridStatistics = (GridStatisticsViewModel)settingReport.Value;

                        calculateStrings = gridStatistics.GetCalculateStrings();

                        rowCount += calculateStrings.Count + 11;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.GridStatisticsParameters));
                        rowsXml.AppendLine(GetRowEnd());

                        // Grid statistics parameters data
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetDataRowLine("String", Resource.GridStatisticsCoordinateSystem + ": " + gridStatistics.GetSelectedCoordinateSystemName()));
                        rowsXml.AppendLine(GetRowEnd());
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetDataRowLine("String", Resource.GridStatisticsGridSize + ": " + gridStatistics.GetGridSizeFormatted()));
                        rowsXml.AppendLine(GetRowEnd());

                        //Empty row
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.GridStatisticsCalculations));
                        rowsXml.AppendLine(GetRowEnd());
                        
                        // Calculate strings data values
                        foreach (var str in calculateStrings)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", str));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        //Empty row
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.GridStatisticsEnvironmentalData));
                        rowsXml.AppendLine(GetRowEnd());

                        // Grid statistics environmental data
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetDataRowLine("String", Resource.SharedLayer + ": " + gridStatistics.GetSelectedWfsLayerName()));
                        rowsXml.AppendLine(GetRowEnd());
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetDataRowLine("String", Resource.SharedCalculate + ": " + gridStatistics.GetWfsCalculationModeText()));
                        rowsXml.AppendLine(GetRowEnd());

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    
                    case "CalculationTime":
                        var timeSeriesSettings = (TimeSeriesSettingsViewModel)settingReport.Value;

                        rowCount += 3;
                       
                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.ResultTimeSeriesPeriodicityTitle));
                        rowsXml.AppendLine(GetRowEnd());

                        // Time series settings data
                        var periodicity = string.Empty;

                        switch ((Periodicity)timeSeriesSettings.DefaultPeriodicityIndex)
                        {
                            case Periodicity.MonthOfTheYear:
                                periodicity = Resource.ResultTimeSeriesPeriodicityMonthOfTheYearLabel;
                                break;
                            case Periodicity.WeekOfTheYear:
                                periodicity = Resource.ResultTimeSeriesPeriodicityWeekOfTheYearLabel;
                                break;
                            case Periodicity.DayOfTheYear:
                                periodicity = Resource.ResultTimeSeriesPeriodicityDayOfTheYearLabel;
                                break;
                            case Periodicity.Yearly:
                                periodicity = Resource.ResultTimeSeriesPeriodicityYearlyLabel;
                                break;
                            case Periodicity.Monthly:
                                periodicity = Resource.ResultTimeSeriesPeriodicityMonthlyLabel;
                                break;
                            case Periodicity.Weekly:
                                periodicity = Resource.ResultTimeSeriesPeriodicityWeeklyLabel;
                                break;
                            case Periodicity.Daily:
                                periodicity = Resource.ResultTimeSeriesPeriodicityDailyLabel;
                                break;
                        }
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetDataRowLine("String", periodicity));
                        rowsXml.AppendLine(GetRowEnd());

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "PresentationMap":
                        var presentationMapSettings = (List<string>)settingReport.Value;

                        rowCount += presentationMapSettings.Count() + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.StateButtonPresentationMap));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var setting in presentationMapSettings)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", setting));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                    case "PresentationFileFormat":
                        var fileFormats = (IEnumerable<string>)settingReport.Value;

                        rowCount += fileFormats.Count() + 2;

                        // Add row with column headers
                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetColumnNameRowLine(Resource.PresentationFileFormatTitle));
                        rowsXml.AppendLine(GetRowEnd());

                        // Data values
                        foreach (var fileFormat in fileFormats)
                        {
                            rowsXml.AppendLine(GetRowStart());
                            rowsXml.AppendLine(GetDataRowLine("String", fileFormat));
                            rowsXml.AppendLine(GetRowEnd());
                        }

                        rowsXml.AppendLine(GetRowStart());
                        rowsXml.AppendLine(GetRowEnd());

                        break;
                }
            }

            var onlySheet = !createWoorkbook;
            _xmlBuilder = new StringBuilder();

            // Create initial section or a new worksheet
            _xmlBuilder.AppendLine(GetInitialSectionOrNewWorksheet(ref createWoorkbook, Resource.SettingsReportSheetName));

            // Specify column and row counts
            _xmlBuilder.AppendLine(GetColumnInitialSection(4, rowCount));

            // Specify column widths
            _xmlBuilder.AppendLine(GetColumnWidthLine(300));
            _xmlBuilder.AppendLine(GetColumnWidthLine(270));
            _xmlBuilder.AppendLine(GetColumnWidthLine(140));
            _xmlBuilder.AppendLine(GetColumnWidthLine(140));

            _xmlBuilder.Append(rowsXml);

            if (onlySheet)
            {
                return;
            }
            // Add final section of the xml document.
            _xmlBuilder.AppendLine(GetFinalSection());
            _xmlBuilder.Replace("&", "&amp;");
        }
    }
}