using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of time series statistics on species observation counts.
    /// </summary>
    public class TimeSeriesOnSpeciesObservationCountsExcelXlsx : ExcelXlsxBase
    {
        private readonly Periodicity _periodicity;

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSeriesOnSpeciesObservationCountsExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="periodicity">Enum describing a time step in terms of its temperal extent.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public TimeSeriesOnSpeciesObservationCountsExcelXlsx(
            IUserContext currentUser, 
            Periodicity periodicity, 
            bool addSettings, 
            bool addProvenance)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
            base.addSettings = addSettings;
            base.addProvenance = addProvenance;
            _periodicity = periodicity;
        }

        /// <summary>
        /// Creates an excel file.
        /// Writes the content of a list into a worksheet of an excelfile and save the file.
        /// </summary>
        /// <param name="autosizeColumnWidth">
        /// If true, the columns will be autosized.
        /// </param>
        /// <returns>
        /// The <see cref="MemoryStream"/>.
        /// </returns>
        private MemoryStream CreateExcelFile(bool autosizeColumnWidth = false)
        {
            MemoryStream memoryStream;
            ExcelWorksheet worksheet = null;
            memoryStream = new MemoryStream();

            try
            {
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    var calculator = new SpeciesObservationDiagramResultCalculator(currentUser, SessionHandler.MySettings);
                    var data = calculator.GetDiagramResult((int)_periodicity);

                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet);
                    AddSpeciesObservationsData(worksheet, data);
                    FormatHeader(worksheet, 1, 2);

                    if (autosizeColumnWidth)
                    {
                        worksheet.Cells.AutoFitColumns(0);
                    }

                    //Add aditional sheets if user has request that
                    AddAditionalSheets(package);
                    
                    package.Save();
                }

                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception)
            {
                memoryStream.Dispose();

                throw;
            }
        }

        /// <summary>
        /// Adds the headers.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void AddHeaders(ExcelWorksheet worksheet)
        {
            Int32 columnIndex;
            String columnHeaderName = String.Empty;

            columnIndex = 1;

            switch (_periodicity)
            {
                case Periodicity.Yearly:
                    columnHeaderName = Resource.ResultTimeSeriesPeriodicityYearlyLabel;
                    break;
                case Periodicity.Monthly:
                    columnHeaderName = Resource.ResultTimeSeriesPeriodicityMonthlyLabel;
                    break;
                case Periodicity.Weekly:
                    columnHeaderName = Resource.ResultTimeSeriesPeriodicityWeeklyLabel;
                    break;
                case Periodicity.Daily:
                    columnHeaderName = Resource.ResultTimeSeriesPeriodicityDailyLabel;
                    break;
                case Periodicity.MonthOfTheYear:
                    columnHeaderName = Resource.ResultTimeSeriesPeriodicityMonthOfTheYearLabel;
                    break;
                case Periodicity.WeekOfTheYear:
                    columnHeaderName = Resource.ResultTimeSeriesPeriodicityWeekOfTheYearLabel;
                    break;
                case Periodicity.DayOfTheYear:
                    columnHeaderName = Resource.ResultTimeSeriesPeriodicityDayOfTheYearLabel;
                    break;
            }

            worksheet.Cells[1, columnIndex].Value = columnHeaderName;
            columnIndex++;
            worksheet.Cells[1, columnIndex].Value = Resource.ResultDiagramTimeSeriesNoOfObservationsTitle;
        }

        private void AddSpeciesObservationsData(ExcelWorksheet worksheet, IEnumerable<KeyValuePair<string, string>> data)
        {
            Int32 rowIndex = 2;

            // Data values
            foreach (KeyValuePair<string, string> row in data)
            {
                worksheet.Cells[rowIndex, 1].Value = row.Key;
                worksheet.Cells[rowIndex, 2].Value = Int64.Parse(row.Value);
                rowIndex++;
            }
        }
    }
}
