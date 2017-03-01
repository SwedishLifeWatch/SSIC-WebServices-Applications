using System;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Observations;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of summary statistics per polygon.
    /// </summary>
    public class SummaryStatisticsPerPolygonExcelXlsx : ExcelXlsxBase
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
        /// Initializes a new instance of the <see cref="SummaryStatisticsPerPolygonExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public SummaryStatisticsPerPolygonExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
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
                    var resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(currentUser, SessionHandler.MySettings);
                    var data = resultCalculator.GetSummaryStatisticsPerPolygonFromCacheIfAvailableOrElseCalculate();

                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet);
                    AddSpeciesObservationsData(worksheet, data);
                    AddSpeciesObservationsFormat(worksheet);

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
            worksheet.Cells[1, 1].Value = Resource.SummaryStatisticsObservationCount;
            worksheet.Cells[1, 2].Value = Resource.SummaryStatisticsTaxaCount;
            worksheet.Cells[1, 3].Value = Resource.SummaryStatisticsPolygon;

            //Set column widths           
            worksheet.Column(1).Width = 19;
            worksheet.Column(2).Width = 17;
            worksheet.Column(3).Width = 38;
        }

        private void AddSpeciesObservationsData(ExcelWorksheet worksheet, IEnumerable<SpeciesObservationsCountPerPolygon> data)
        {
            int rowIndex;
            int maxColumnIndex;

            rowIndex = 2;
            maxColumnIndex = 3;

            // Data values
            foreach (SpeciesObservationsCountPerPolygon row in data)
            {
                worksheet.Cells[rowIndex, 1].Value = Int32.Parse(row.SpeciesObservationsCount);
                worksheet.Cells[rowIndex, 2].Value = Int32.Parse(row.SpeciesCount);
                worksheet.Cells[rowIndex, 3].Value = row.Properties.Replace("<br />", "\n");

                rowIndex++;
            }

            // Format style from second row to last row
            using (var range = worksheet.Cells[2, 1, rowIndex, maxColumnIndex])
            {
                range.Style.WrapText = true;
            }
        }

        private void AddSpeciesObservationsFormat(ExcelWorksheet worksheet)
        {
            int maxColumnIndex;

            // Formatting straight columns
            if (IsColumnHeaderBackgroundUsed)
            {
                maxColumnIndex = 3;

                // Format style by columns in first row
                using (var range = worksheet.Cells[1, 1, 1, maxColumnIndex])
                {
                    range.Style.Font.Bold = false;
                    range.Style.Font.Color.SetColor(ExcelHelper.ColorTable[0]);
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ExcelHelper.ColorTable[57]);
                }
            }
        }
    }
}
