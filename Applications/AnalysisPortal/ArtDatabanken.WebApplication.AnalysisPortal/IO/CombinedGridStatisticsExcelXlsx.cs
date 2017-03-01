using System;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of combined grid statistics.
    /// </summary>
    public class CombinedGridStatisticsExcelXlsx : ExcelXlsxBase
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
        /// Initializes a new instance of the <see cref="CombinedGridStatisticsExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public CombinedGridStatisticsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
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
                using (var package = new ExcelPackage(memoryStream))
                {
                    var gridStatisticsSetting = SessionHandler.MySettings.Calculation.GridStatistics;
                    var coordinateSystemId = gridStatisticsSetting.CoordinateSystemId.GetValueOrDefault((int)GridCoordinateSystem.SWEREF99_TM);
                    var gridSize = gridStatisticsSetting.GridSize.GetValueOrDefault(10000);
                    var wfsLayerId = gridStatisticsSetting.WfsGridStatisticsLayerId.GetValueOrDefault(-1);
                    if (wfsLayerId < 0)
                    {
                        throw new Exception("Error when trying to create Excel file. You must select an environmental data layer in Grid statistics settings.");
                    }

                    var resultCalculator = new CombinedGridStatisticsCalculator(currentUser, SessionHandler.MySettings);
                    var data = resultCalculator.CalculateCombinedGridResult(coordinateSystemId, gridSize, wfsLayerId);

                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet, data);
                    AddSpeciesObservationsData(worksheet, data);
                    FormatHeader(worksheet, 1, 11);

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
        private void AddHeaders(ExcelWorksheet worksheet, CombinedGridStatisticsResult data)
        {
            //Add row with column headers
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = Resource.GridStatisticsTaxaCount;
            worksheet.Cells[1, 3].Value = Resource.GridStatisticsObservationCount;
            worksheet.Cells[1, 4].Value = string.Format("{1} ({0})", Resource.GridStatisticsEnvironmentalData, Resource.GridStatisticsCalculationModeCount);
            worksheet.Cells[1, 5].Value = string.Format("{1} ({0})", Resource.GridStatisticsEnvironmentalData, Resource.GridStatisticsCalculationModeArea);
            worksheet.Cells[1, 6].Value = string.Format("{1} ({0})", Resource.GridStatisticsEnvironmentalData, Resource.GridStatisticsCalculationModeLength);
            worksheet.Cells[1, 7].Value = string.Format("{0} X - {1} ({2})", Resource.GridStatisticsCentreCoordinate, Resource.GridStatisticsCalculation, data.CalculationCoordinateSystemName);
            worksheet.Cells[1, 8].Value = string.Format("{0} Y - {1} ({2})", Resource.GridStatisticsCentreCoordinate, Resource.GridStatisticsCalculation, data.CalculationCoordinateSystemName);
            worksheet.Cells[1, 9].Value = string.Format("{0} X - {1} ({2})", Resource.GridStatisticsCentreCoordinate, Resource.GridStatisticsDisplay, data.DisplayCoordinateSystemName);
            worksheet.Cells[1, 10].Value = string.Format("{0} Y - {1} ({2})", Resource.GridStatisticsCentreCoordinate, Resource.GridStatisticsDisplay, data.DisplayCoordinateSystemName);
            worksheet.Cells[1, 11].Value = Resource.GridStatisticsCellSizeMeters;

            //Set column widths           
            worksheet.Column(1).Width = 38;
            worksheet.Column(2).Width = 19;
            worksheet.Column(3).Width = 19;
            worksheet.Column(4).Width = 23;
            worksheet.Column(5).Width = 23;
            worksheet.Column(6).Width = 23;
            worksheet.Column(7).Width = 23;
            worksheet.Column(8).Width = 36;
            worksheet.Column(9).Width = 36;
            worksheet.Column(10).Width = 36;
            worksheet.Column(11).Width = 36;
        }

        private void AddSpeciesObservationsData(ExcelWorksheet worksheet, CombinedGridStatisticsResult data)
        {
            Int32 columnIndex;
            Int32 rowIndex;

            columnIndex = 1;
            rowIndex = 2;

            // Data values
            foreach (var row in data.Cells)
            {
                worksheet.Cells[rowIndex, columnIndex].Value = row.Identifier;
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.SpeciesCount;
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.ObservationCount;
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.FeatureCount;
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.FeatureArea;
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.FeatureLength;
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.OriginalCentreCoordinateX; //"Centrum X (SWEREF 99)"
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.OriginalCentreCoordinateY; //"Centrum Y (SWEREF 99)"
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.CentreCoordinateX; //"Centrum X (Google Mercator)"
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.CentreCoordinateY; //"Centrum Y (Google Mercator)"
                columnIndex++;
                worksheet.Cells[rowIndex, columnIndex].Value = row.GridCellSize; //"Centrum X (Google Mercator)"

                columnIndex = 1;
                rowIndex++;
            }
        }
    }
}
