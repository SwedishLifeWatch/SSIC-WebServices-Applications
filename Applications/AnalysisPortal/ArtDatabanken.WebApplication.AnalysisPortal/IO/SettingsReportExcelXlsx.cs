using System;
using Resources;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using OfficeOpenXml;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of observed taxon list Excel file.
    /// </summary>
    public class SettingsReportExcelXlsx : ExcelXlsxBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsReportExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        public SettingsReportExcelXlsx(IUserContext currentUser)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
        }

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            var memoryStream = new MemoryStream();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(Resource.SettingsReportSheetName);
                PopulateSettingsWorkSheet(worksheet);
                package.SaveAs(memoryStream);
            }

            memoryStream.Position = 0;

            return memoryStream;            
        }

        public void PopulateSettingsWorkSheet(ExcelWorksheet worksheet, bool autosizeColumnWidth = true)
        {
            var rowIndex = 1;
            var settingsData = MySettingsSummaryItemManager.GetSettingReportData(currentUser);

            AddHeadersAndContentForCurrentDate(worksheet, DateTime.Now, ref rowIndex);

            foreach (var settingReport in settingsData)
            {
                switch (settingReport.Key)
                {
                    case "DataProviders":
                        AddHeadersForDataProviders(worksheet, ref rowIndex);
                        AddContentDataForDataProviders(
                            worksheet, 
                            (List<DataProviderViewModel>)settingReport.Value,
                            ref rowIndex);

                        break;
                    case "DataWfsLayers":
                        var wfsLayersList = (List<string>)settingReport.Value;

                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.DataWfsLayer);
                        AddContentDataForOneColList(worksheet, wfsLayersList, ref rowIndex);

                        break;
                    case "DataWmsLayers":
                         var wmsLayersList = (List<string>)settingReport.Value;

                         AddHeadersForOneColList(worksheet, ref rowIndex, Resource.StateButtonDataProvidersWfsLayers);
                         AddContentDataForOneColList(worksheet, wmsLayersList, ref rowIndex);

                        break;
                    case "FilterOccurrence":
                        var occurrenceList = (List<string>)settingReport.Value;

                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.StateButtonFilterOccurrence);
                        AddContentDataForOneColList(worksheet, occurrenceList, ref rowIndex);

                        break;
                    case "FilterTaxa":
                        var taxonList = (List<TaxonViewModel>)settingReport.Value;
                        AddHeadersForTaxon(worksheet, ref rowIndex);
                        AddContentDataForTaxon(worksheet, taxonList, ref rowIndex);

                        break;
                    case "FilterRegion":
                        var regions = (List<RegionViewModel>)settingReport.Value;
                        AddHeadersForRegion(worksheet, ref rowIndex);
                        AddContentDataForRegion(worksheet, regions, ref rowIndex);

                        break;
                    case "FilterTemporal":
                        var temporalList = (List<string>)settingReport.Value;
                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.StateButtonFilterTemporal);
                        AddContentDataForOneColList(worksheet, temporalList, ref rowIndex);

                        break;
                    case "FilterAccuracy":
                        var accuracyList = (List<string>)settingReport.Value;
                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.StateButtonFilterAccuracy);
                        AddContentDataForOneColList(worksheet, accuracyList, ref rowIndex);

                        break;
                    case "FilterField":
                        var fields = (IEnumerable<string>)settingReport.Value;

                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.SettingsReportFieldFilter);
                        AddContentDataForOneColList(worksheet, fields, ref rowIndex);
                        break;
                    case "FilterRedList":
                        var categories = (IEnumerable<string>)settingReport.Value;
                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.SettingsReportRedListFilter);
                        AddContentDataForOneColList(worksheet, categories, ref rowIndex);

                        break;
                    case "CalculationSummary":
                        var summaryStatistics = (SummaryStatisticsViewModel)settingReport.Value;
                        var summaryStatisticsStrings = summaryStatistics.GetCalculateStrings();

                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.SummaryStatisticsCalculateNumberOfObservations);
                        AddContentDataForOneColList(worksheet, summaryStatisticsStrings, ref rowIndex);

                        AddHeadersForSummaryStatisticsEnvironmentalData(worksheet, ref rowIndex);
                        AddContentDataForSummaryStatisticsEnvironmentalData(worksheet, summaryStatistics, ref rowIndex);

                        break;
                    case "CalculationGrid":
                        var gridStatistics = (GridStatisticsViewModel)settingReport.Value;

                        AddHeadersForGridStatisticsParameters(worksheet, ref rowIndex);
                        AddContentDataForGridStatisticsParameters(worksheet, gridStatistics, ref rowIndex);

                        var calculateStrings = gridStatistics.GetCalculateStrings();
                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.GridStatisticsCalculations);
                        AddContentDataForOneColList(worksheet, calculateStrings, ref rowIndex);

                        AddHeadersForGridStatisticsEnvironmentalData(worksheet, ref rowIndex);
                        AddContentDataForGridStatisticsEnvironmentalData(worksheet, gridStatistics, ref rowIndex);

                        break;
                    case "CalculationTime":
                        var timeSeriesSettings = (TimeSeriesSettingsViewModel)settingReport.Value;
                        AddHeadersForTimeSeries(worksheet, ref rowIndex);
                        AddContentDataForTimeSeries(worksheet, timeSeriesSettings, ref rowIndex);

                        break;

                    case "PresentationMap":
                        var presentationMapSettings = (List<string>)settingReport.Value;
                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.StateButtonPresentationCoordinateSystem);
                        AddContentDataForOneColList(worksheet, presentationMapSettings, ref rowIndex);
                        break;
                    case "PresentationFileFormat":
                        var fileFormats = (IEnumerable<string>)settingReport.Value;

                        AddHeadersForOneColList(worksheet, ref rowIndex, Resource.PresentationFileFormatTitle);
                        AddContentDataForOneColList(worksheet, fileFormats, ref rowIndex);
                        break;
                }
            }
            if (autosizeColumnWidth)
            {
                worksheet.Cells.AutoFitColumns(0);
            }
        }      

        /// <summary>
        /// Adds the headers and content for current date.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="dateTime">The date time.</param>
        /// <param name="rowIndex">Index of the row.</param>
        private void AddHeadersAndContentForCurrentDate(ExcelWorksheet worksheet, DateTime dateTime, ref int rowIndex)
        {
            List<string> dateList = new List<string>();
            dateList.Add(dateTime.ToShortDateString());            
            AddHeadersForOneColList(worksheet, ref rowIndex, Resource.SharedDateText);
            AddContentDataForOneColList(worksheet, dateList, ref rowIndex);
        }

        /// <summary>
        /// Adds the headers.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void AddHeadersForDataProviders(ExcelWorksheet worksheet, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = Resource.DataProvidersDataProvidersDataProvider;
            worksheet.Cells[rowIndex, 2].Value = "Organisation";
            worksheet.Cells[rowIndex, 3].Value = Resource.DataProvidersDataProvidersNumberOfObservations;
            worksheet.Cells[rowIndex, 4].Value = Resource.DataProvidersDataProvidersNumberOfPublicObservations;

            FormatHeader(worksheet, rowIndex, 4);

            rowIndex++;
        }

        private void AddContentDataForDataProviders(ExcelWorksheet worksheet, IEnumerable<DataProviderViewModel> dataProviders, ref int rowIndex)
        {
            foreach (var dataProvider in dataProviders)
            {
                worksheet.Cells[rowIndex, 1].Value = dataProvider.Name;
                worksheet.Cells[rowIndex, 2].Value = dataProvider.Organization;
                worksheet.Cells[rowIndex, 3].Value = dataProvider.NumberOfObservations;
                worksheet.Cells[rowIndex, 4].Value = dataProvider.NumberOfPublicObservations;

                rowIndex++;
            }

            rowIndex++;
        }

        private void AddHeadersForOneColList(ExcelWorksheet worksheet, ref int rowIndex, string header)
        {
            worksheet.Cells[rowIndex, 1].Value = header;
            FormatHeader(worksheet, rowIndex, 1);
            rowIndex++;
        }

        private void AddContentDataForOneColList(ExcelWorksheet worksheet, IEnumerable<string> list, ref int rowIndex)
        {
            foreach (var item in list)
            {
                worksheet.Cells[rowIndex, 1].Value = item;
                rowIndex++;
            }

            rowIndex++;
        }

        private void AddHeadersForTaxon(ExcelWorksheet worksheet, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = Resource.TaxonSharedTaxonId;
            worksheet.Cells[rowIndex, 2].Value = Resource.TaxonSharedScientificName;
            worksheet.Cells[rowIndex, 3].Value = Resource.TaxonSharedSwedishName;

            FormatHeader(worksheet, rowIndex, 3);

            rowIndex++;
        }

        private void AddContentDataForTaxon(ExcelWorksheet worksheet, IEnumerable<TaxonViewModel> taxonList, ref int rowIndex)
        {
            foreach (var item in taxonList)
            {
                worksheet.Cells[rowIndex, 1].Value = item.TaxonId;
                worksheet.Cells[rowIndex, 2].Value = item.ScientificName;
                worksheet.Cells[rowIndex, 3].Value = item.CommonName;

                rowIndex++;
            }

            rowIndex++;
        }

        private void AddHeadersForRegion(ExcelWorksheet worksheet, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = Resource.SharedRegion;

            FormatHeader(worksheet, rowIndex, 1);

            rowIndex++;
        }

        private void AddContentDataForRegion(ExcelWorksheet worksheet, IEnumerable<RegionViewModel> regions, ref int rowIndex)
        {
            foreach (var region in regions)
            {
                worksheet.Cells[rowIndex, 1].Value = region.Name;
                rowIndex++;
            }

            rowIndex++;
        }

        private void AddHeadersForGridStatisticsParameters(ExcelWorksheet worksheet, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = Resource.GridStatisticsParameters;

            FormatHeader(worksheet, rowIndex, 1);

            rowIndex++;
        }

        private void AddContentDataForGridStatisticsParameters(ExcelWorksheet worksheet, GridStatisticsViewModel gridStatistics, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = string.Format("{0}: {1}", Resource.GridStatisticsCoordinateSystem, gridStatistics.GetSelectedCoordinateSystemName());
            rowIndex++;
            worksheet.Cells[rowIndex, 1].Value = string.Format("{0}: {1}", Resource.GridStatisticsGridSize, gridStatistics.GetGridSizeFormatted());
            rowIndex = rowIndex + 2;
        }

        private void AddHeadersForGridStatisticsEnvironmentalData(ExcelWorksheet worksheet, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = Resource.GridStatisticsEnvironmentalData;

            FormatHeader(worksheet, rowIndex, 1);

            rowIndex++;
        }

        private void AddContentDataForGridStatisticsEnvironmentalData(ExcelWorksheet worksheet, GridStatisticsViewModel gridStatistics, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = string.Format("{0}: {1}", Resource.SharedLayer, gridStatistics.GetSelectedWfsLayerName());
            rowIndex++;
            worksheet.Cells[rowIndex, 1].Value = string.Format("{0}: {1}", Resource.SharedCalculate, gridStatistics.GetWfsCalculationModeText());
            rowIndex = rowIndex + 2;
        }

        private void AddHeadersForSummaryStatisticsEnvironmentalData(ExcelWorksheet worksheet, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = Resource.GridStatisticsEnvironmentalData;

            FormatHeader(worksheet, rowIndex, 1);

            rowIndex++;
        }

        private void AddContentDataForSummaryStatisticsEnvironmentalData(ExcelWorksheet worksheet, SummaryStatisticsViewModel summaryStatistics, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = string.Format("{0}: {1}", Resource.SharedLayer, summaryStatistics.GetSelectedWfsLayerName());
            rowIndex = rowIndex + 2;
        }

        private void AddHeadersForTimeSeries(ExcelWorksheet worksheet, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = Resource.ResultTimeSeriesPeriodicityTitle;

            FormatHeader(worksheet, rowIndex, 1);

            rowIndex++;
        }

        private void AddContentDataForTimeSeries(ExcelWorksheet worksheet, TimeSeriesSettingsViewModel timeSeriesSettings, ref int rowIndex)
        {
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

            worksheet.Cells[rowIndex, 1].Value = periodicity;
            rowIndex = rowIndex + 2;
        }
    }
}
