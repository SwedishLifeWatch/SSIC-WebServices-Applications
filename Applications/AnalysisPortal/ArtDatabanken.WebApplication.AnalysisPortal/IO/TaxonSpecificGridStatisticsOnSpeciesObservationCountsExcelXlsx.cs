using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of grid statistics on species observation counts for each selected taxon.
    /// </summary>
    public class TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXlsx : ExcelXlsxBase
    {
        private CoordinateSystemId CoordinateSystem { get; set; }

        public Boolean FormatCountAsOccurrence { get; set; }

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">The user context.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the result cells will be set to 1 if there are any observations; otherwise 0.
        /// </param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public TaxonSpecificGridStatisticsOnSpeciesObservationCountsExcelXlsx(IUserContext currentUser, CoordinateSystemId coordinateSystem, bool formatCountAsOccurrence, bool addSettings, bool addProvenance)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
            base.addSettings = addSettings;
            base.addProvenance = addProvenance;
            CoordinateSystem = coordinateSystem;
            FormatCountAsOccurrence = formatCountAsOccurrence;
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
                    var resultCalculator = new SpeciesObservationGridCalculator(currentUser, SessionHandler.MySettings);
                    var data = resultCalculator.CalculateMultipleSpeciesObservationGrid();
                    
                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet, data);
                    AddContentData(worksheet, data);
                    AddContentFormat(worksheet, data);

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
        private void AddHeaders(ExcelWorksheet worksheet, TaxonSpecificGridSpeciesObservationCountResult data)
        {
            var gridCoordinateSystemDescription = "";
            if (data.GridCells.Count > 0)
            {
                gridCoordinateSystemDescription = data.GridCells.First().Key.GridCoordinateSystem.ToString();
            }

            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = String.Format("Centre coordinate X ({0})", gridCoordinateSystemDescription);
            worksheet.Cells[1, 3].Value = String.Format("Centre coordinate Y ({0})", gridCoordinateSystemDescription);
            worksheet.Cells[1, 4].Value = String.Format("Centre coordinate X ({0})", CoordinateSystem);
            worksheet.Cells[1, 5].Value = String.Format("Centre coordinate Y ({0})", CoordinateSystem);
            worksheet.Cells[1, 6].Value = "Grid cell width (metres)";

            var columnCount = 7;
            foreach (TaxonViewModel taxon in data.Taxa)
            {
                worksheet.Cells[1, columnCount].Value = String.Format("{0} (TaxonId {1})", taxon.ScientificName, taxon.TaxonId);
                columnCount++;
            }            
        }

        private void AddContentData(ExcelWorksheet worksheet, TaxonSpecificGridSpeciesObservationCountResult data)
        {
            var rowIndex = 2;

            // Data values.
            List<IGridCellBase> orderedGridCells = data.GridCells.Keys.OrderBy(x => x.Identifier).ToList();
            foreach (IGridCellBase gridCell in orderedGridCells)
            {
                worksheet.Cells[rowIndex, 1].Value = gridCell.Identifier;
                worksheet.Cells[rowIndex, 2].Value = gridCell.OrginalGridCellCentreCoordinate.X;
                worksheet.Cells[rowIndex, 3].Value = gridCell.OrginalGridCellCentreCoordinate.Y;
                worksheet.Cells[rowIndex, 4].Value = gridCell.GridCellCentreCoordinate.X;
                worksheet.Cells[rowIndex, 5].Value = gridCell.GridCellCentreCoordinate.Y;
                worksheet.Cells[rowIndex, 6].Value = gridCell.GridCellSize;

                var columnIndex = 7;
                foreach (var taxon in data.Taxa)
                {
                    long nrObservations = 0;
                    if (data.GridCells[gridCell].ContainsKey(taxon.TaxonId))
                    {
                        nrObservations = data.GridCells[gridCell][taxon.TaxonId].ObservationCount;
                    }

                    if (FormatCountAsOccurrence)
                    {
                        worksheet.Cells[rowIndex, columnIndex].Value = nrObservations > 0 ? 1 : 0;
                    }
                    else
                    {
                        worksheet.Cells[rowIndex, columnIndex].Value = nrObservations;
                    }
                    columnIndex++;
                }

                rowIndex++;
            }
        }

        private void AddContentFormat(ExcelWorksheet worksheet, TaxonSpecificGridSpeciesObservationCountResult data)
        {
            // Formatting straight columns
            if (IsColumnHeaderBackgroundUsed)
            {
                // Format style by columns
                var maxColumnIndex = 6 + data.Taxa.Count;

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
