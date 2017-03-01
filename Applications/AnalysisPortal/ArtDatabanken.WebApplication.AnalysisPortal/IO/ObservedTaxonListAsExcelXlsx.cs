using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of observed taxon list Excel file.
    /// </summary>
    public class ObservedTaxonListAsExcelXlsx : ExcelXlsxBase
    {
        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservedTaxonListAsExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public ObservedTaxonListAsExcelXlsx(IUserContext currentUser, bool addSettings, bool addProvenance)
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
                    var resultCalculator = new SpeciesObservationTaxonTableResultCalculator(currentUser, SessionHandler.MySettings);
                    var data = resultCalculator.GetResultFromCacheIfAvailableOrElseCalculate();
                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet);
                    AddContentData(worksheet, data);
                    FormatHeader(worksheet, 1, 7);

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
            worksheet.Cells[1, 1].Value = Resource.LabelTaxon;
            worksheet.Cells[1, 2].Value = Resource.LabelAuthor;
            worksheet.Cells[1, 3].Value = Resource.LabelSwedishName;
            worksheet.Cells[1, 4].Value = Resource.LabelCategory;
            worksheet.Cells[1, 5].Value = "Status";
            worksheet.Cells[1, 6].Value = "Dyntaxa info";
            worksheet.Cells[1, 7].Value = Resource.LabelTaxonId;
        }

        private void AddContentData(ExcelWorksheet worksheet, IEnumerable<TaxonViewModel> data)
        {
            var rowIndex = 2;

            foreach (var row in data)
            {
                worksheet.Cells[rowIndex, 1].Value = row.ScientificName;
                worksheet.Cells[rowIndex, 2].Value = row.Author;
                worksheet.Cells[rowIndex, 3].Value = row.CommonName;
                worksheet.Cells[rowIndex, 4].Value = row.Category;
                worksheet.Cells[rowIndex, 5].Value = row.TaxonStatus;
                worksheet.Cells[rowIndex, 6].Value = @"Http://Dyntaxa.se/Taxon/Info/" + row.TaxonId;
                worksheet.Cells[rowIndex, 7].Value = row.TaxonId;

                rowIndex++;
            }
        }
    }
}
