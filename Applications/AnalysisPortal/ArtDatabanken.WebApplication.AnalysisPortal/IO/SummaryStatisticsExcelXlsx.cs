using System;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of summary statistics.
    /// </summary>
    public class SummaryStatisticsExcelXlsx : ExcelXlsxBase
    {
        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryStatisticsExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public SummaryStatisticsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
            base.addSettings = addSettings;
            base.addProvenance = addProvenance;
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
            var memoryStream = new MemoryStream();
           
            try
            {
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    var resultCalculator = new SummaryStatisticsResultCalculator(currentUser, SessionHandler.MySettings);
                    var data = resultCalculator.GetSummaryStatisticsFromCacheIfAvailableOrElseCalculate();

                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
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
            //Add row with column headers
            worksheet.Cells[1, 1].Value = Resource.ResultViewSummaryStatisticsSpeciesObservationTableColumnCalculationHeader;
            worksheet.Cells[1, 2].Value = Resource.ResultViewSummaryStatisticsSpeciesObservationTableColumnCountHeader;

            //Set column widths           
            worksheet.Column(1).Width = 38;
            worksheet.Column(2).Width = 17;
        }

        private void AddSpeciesObservationsData(ExcelWorksheet worksheet, IEnumerable<KeyValuePair<string, string>> data)
        {
            Int32 columnIndex;
            Int32 rowIndex;
            Int32 maxColumnIndex;

            columnIndex = 1;
            rowIndex = 2;
            maxColumnIndex = 2;

            // Data values
            foreach (KeyValuePair<string, string> row in data)
            {
                worksheet.Cells[rowIndex, columnIndex].Value = row.Key;
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = Int32.Parse(row.Value);

                columnIndex = 1;
                rowIndex++;
            }

            // Format style from second row to last row
            using (var range = worksheet.Cells[2, 1, rowIndex, maxColumnIndex])
            {
                range.Style.WrapText = true;
            }
        }
    }
}
