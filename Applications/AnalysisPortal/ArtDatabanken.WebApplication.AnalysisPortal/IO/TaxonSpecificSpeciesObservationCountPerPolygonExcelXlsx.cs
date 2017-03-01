using System;
using System.IO;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SummaryStatistics;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of species observations count per polygon and for each selected taxon.
    /// </summary>
    public class TaxonSpecificSpeciesObservationCountPerPolygonExcelXlsx : ExcelXlsxBase
    {
        private Boolean FormatCountAsOccurrence { get; set; }

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxonSpecificSpeciesObservationCountPerPolygonExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">The user context.</param>
        /// <param name="formatCountAsOccurrence">
        /// If set to <c>true</c> the result cells will be set to 1 if there are any observations; otherwise 0.
        /// </param> 
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public TaxonSpecificSpeciesObservationCountPerPolygonExcelXlsx(IUserContext currentUser, Boolean formatCountAsOccurrence, bool addSettings, bool addProvenance)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
            base.addSettings = addSettings;
            base.addProvenance = addProvenance;
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
                using (var package = new ExcelPackage(memoryStream))
                {
                    var resultCalculator = new SummaryStatisticsPerPolygonResultCalculator(currentUser, SessionHandler.MySettings);
                    var data = resultCalculator.CalculateSpeciesObservationCountPerPolygonAndTaxa(SessionHandler.MySettings.Filter.Taxa.TaxonIds.ToList());
                    
                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet, data);
                    AddSpeciesObservationsData(worksheet, data);
                    FormatHeader(worksheet, 1, data.Taxa.Count + 1);

                    if (autosizeColumnWidth)
                    {
                        worksheet.Cells.AutoFitColumns(0);
                    }
                    // Set first column's width
                    worksheet.Column(1).Width = 38;

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
        private void AddHeaders(ExcelWorksheet worksheet, TaxonSpecificSpeciesObservationCountPerPolygonResult data)
        {
            var columnIndex = 2;

            //Add row with column headers
            worksheet.Cells[1, 1].Value = "Polygon";

            foreach (var taxon in data.Taxa)
            {
                worksheet.Cells[1, columnIndex].Value = string.Format("{0} (TaxonId {1})", taxon.ScientificName, taxon.TaxonId);
                columnIndex++;
            } 
        }

        private void AddSpeciesObservationsData(ExcelWorksheet worksheet, TaxonSpecificSpeciesObservationCountPerPolygonResult data)
        {
            var columnIndex = 1;
            var rowIndex = 2;
            var maxColumnIndex = data.Taxa.Count + 1;

            // Data values
            foreach (var pair in data.SpeciesObservationCountPerPolygon)
            {
                worksheet.Cells[rowIndex, columnIndex].Value = pair.Key.Replace("<br />", "\n");
                columnIndex++;

                foreach (var taxon in data.Taxa)
                {
                    long speciesObservationCount = 0;
                    pair.Value.TryGetValue(taxon.TaxonId, out speciesObservationCount);

                    if (FormatCountAsOccurrence)
                    {
                        int binaryVal = speciesObservationCount > 0 ? 1 : 0;
                        worksheet.Cells[rowIndex, columnIndex].Value = binaryVal;
                    }
                    else
                    {
                        worksheet.Cells[rowIndex, columnIndex].Value = speciesObservationCount;
                    }
                    columnIndex++;
                }
                
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
