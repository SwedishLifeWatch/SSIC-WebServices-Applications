using System;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservationProvenanceResult;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of species observation provenances.
    /// </summary>
    public class SpeciesObservationProvenanceExcelXlsx : ExcelXlsxBase
    {
        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            var memoryStream = new MemoryStream();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(Resource.SpeciesObservationProvenanceReportSheetName);
                PopulateSheet(worksheet);
                package.SaveAs(memoryStream);
            }

            memoryStream.Position = 0;

            return memoryStream;  
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesObservationProvenanceExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        public SpeciesObservationProvenanceExcelXlsx(IUserContext currentUser)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
        }

        /// <summary>
        /// Creates an excel file.
        /// Writes the content of a list into a worksheet of an excelfile and save the file.
        /// </summary>
        /// <param name="worksheet">Sheet to populate</param>
        /// <param name="autosizeColumnWidth">If true, the columns will be autosized. </param>
        /// <returns>
        /// The <see cref="MemoryStream"/>.
        /// </returns>
        public void PopulateSheet(ExcelWorksheet worksheet, bool autosizeColumnWidth = false)
        {
            var resultCalculator = new SpeciesObservationProvenanceResultCalculator(currentUser, SessionHandler.MySettings);
            var data = resultCalculator.GetSpeciesObservationProvenances();
           
            AddHeaders(worksheet);
            AddSpeciesObservationsData(worksheet, data);
            FormatHeader(worksheet, 1, 3);

            if (autosizeColumnWidth)
            {
                worksheet.Cells.AutoFitColumns(0);
            }      
        }

        /// <summary>
        /// Adds the headers.
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        private void AddHeaders(ExcelWorksheet worksheet)
        {
            //Add row with column headers
            worksheet.Cells[1, 1].Value = Resource.ResultViewSpeciesObservationProvenanceTableColumnNameHeader;
            worksheet.Cells[1, 2].Value = Resource.ResultViewSpeciesObservationProvenanceTableColumnValueHeader;
            worksheet.Cells[1, 3].Value = Resource.ResultViewSpeciesObservationProvenanceTableColumnSpeciesObservationCountHeader;

            //Set column widths           
            worksheet.Column(1).Width = 17;
            worksheet.Column(2).Width = 55;
            worksheet.Column(3).Width = 38;
        }

        private void AddSpeciesObservationsData(ExcelWorksheet worksheet, IEnumerable<SpeciesObservationProvenance> data)
        {
            var rowIndex = 2;

            // Data values
            foreach (var row in data)
            {
                worksheet.Cells[rowIndex, 1].Value = row.Name;
                rowIndex++;

                //Data subvalues
                foreach (var details in row.Values)
                {
                    worksheet.Cells[rowIndex, 1].Value = string.Empty;
                    worksheet.Cells[rowIndex, 2].Value = details.Value ?? "-";
                    worksheet.Cells[rowIndex, 3].Value = details.SpeciesObservationCount;

                    rowIndex++;
                }
            }
        }
    }
}
