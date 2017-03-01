using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult;
using OfficeOpenXml;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of species observations Excel file.
    /// </summary>
    public class SpeciesObservationsExcelXlsx : ExcelXlsxBase
    {
        private CoordinateSystemId coordinateSystemId;
        private SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId;
        private bool useLabelAsColumnHeader;

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesObservationsExcelXlsx" /> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        /// <param name="coordinateSystemId">Coordinate system</param>
        /// <param name="speciesObservationTableColumnsSetId">The table columns set to use.</param>
        /// <param name="useLabelAsColumnHeader">Use label as column header.</param>
        public SpeciesObservationsExcelXlsx(
            IUserContext currentUser, 
            bool addSettings, 
            bool addProvenance, 
            CoordinateSystemId coordinateSystemId,
            SpeciesObservationTableColumnsSetId speciesObservationTableColumnsSetId, 
            bool useLabelAsColumnHeader)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
            base.addSettings = addSettings;
            base.addProvenance = addProvenance;
            this.coordinateSystemId = coordinateSystemId;
            this.speciesObservationTableColumnsSetId = speciesObservationTableColumnsSetId;
            this.useLabelAsColumnHeader = useLabelAsColumnHeader;
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
                    var resultCalculator = new SpeciesObservationResultCalculator(currentUser, SessionHandler.MySettings);
                    List<Dictionary<ViewTableField, string>> speciesObservations = resultCalculator.GetTableResult(this.coordinateSystemId, this.speciesObservationTableColumnsSetId);

                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet, speciesObservations);
                    AddSpeciesObservationsData(worksheet, speciesObservations);
                    FormatHeader(worksheet, 1, speciesObservations.Any() ? speciesObservations.First().Keys.Count : 1);

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
        private void AddHeaders(ExcelWorksheet worksheet, IEnumerable<Dictionary<ViewTableField, string>> speciesObservations)
        {
            if (!speciesObservations.Any())
            {
                return;
            }

            var columnIndex = 1;            
            foreach (var tableField in speciesObservations.First().Keys)
            {
                if (useLabelAsColumnHeader)
                {
                    worksheet.Cells[1, columnIndex].Value = tableField.Title;
                }
                else
                {
                    worksheet.Cells[1, columnIndex].Value = tableField.DataField;
                }
                
                columnIndex++;
            }
        }

        private void AddSpeciesObservationsData(ExcelWorksheet worksheet, IEnumerable<Dictionary<ViewTableField, string>> speciesObservations)
        {
            var columnIndex = 1;
            var rowIndex = 2;

            foreach (var observation in speciesObservations)
            {
                foreach (var obsKeyValue in observation)
                {
                    worksheet.Cells[rowIndex, columnIndex].Value = obsKeyValue.Value;

                    columnIndex++;
                }
                columnIndex = 1;
                rowIndex++;
            }
        }
    }
}
