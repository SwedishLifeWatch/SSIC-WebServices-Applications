using Resources;
using System;
using System.IO;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of grid statistics on species counts.
    /// </summary>
    public class GridStatisticsOnSpeciesCountExcelXlsx : ExcelXlsxBase
    {
        private CoordinateSystemId CoordinateSystem { get; set; }

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridStatisticsOnSpeciesCountExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public GridStatisticsOnSpeciesCountExcelXlsx(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool addSettings, bool addProvenance)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
            CoordinateSystem = coordinateSystem;
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
                    var resultCalculator = new TaxonGridCalculator(currentUser, SessionHandler.MySettings);
                    var data = resultCalculator.GetTaxonGridResultFromCacheIfAvailableOrElseCalculate();

                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet, data);
                    AddContentData(worksheet, data);
                    FormatHeader(worksheet, 1, 8);

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
        private void AddHeaders(ExcelWorksheet worksheet, TaxonGridResult data)
        {
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = Resource.GridStatisticsTaxaCount;
            worksheet.Cells[1, 3].Value = Resource.GridStatisticsObservationCount;
            worksheet.Cells[1, 4].Value = String.Format("Centre coordinate X ({0})", data.GridCellCoordinateSystem);
            worksheet.Cells[1, 5].Value = String.Format("Centre coordinate Y ({0})", data.GridCellCoordinateSystem);
            worksheet.Cells[1, 6].Value = String.Format("Centre coordinate X ({0})", CoordinateSystem);
            worksheet.Cells[1, 7].Value = String.Format("Centre coordinate Y ({0})", CoordinateSystem);
            worksheet.Cells[1, 8].Value = "Grid cell width (metres)";
        }

        private void AddContentData(ExcelWorksheet worksheet, TaxonGridResult data)
        {
            var rowIndex = 2;

            foreach (var row in data.Cells)
            {
                worksheet.Cells[rowIndex, 1].Value = row.Identifier;
                worksheet.Cells[rowIndex, 2].Value = row.SpeciesCount;
                worksheet.Cells[rowIndex, 3].Value = row.ObservationCount;
                worksheet.Cells[rowIndex, 4].Value = row.OriginalCentreCoordinateX;
                worksheet.Cells[rowIndex, 5].Value = row.OriginalCentreCoordinateY;
                worksheet.Cells[rowIndex, 6].Value = row.CentreCoordinateX;
                worksheet.Cells[rowIndex, 7].Value = row.CentreCoordinateY;
                worksheet.Cells[rowIndex, 8].Value = data.GridCellSize;

                rowIndex++;
            }
        }
    }
}
