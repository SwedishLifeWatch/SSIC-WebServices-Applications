using System;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultCalculation.SpeciesObservation;
using ArtDatabanken.WebApplication.AnalysisPortal.Result.ResultModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of time series statistics on species observation abundance index.
    /// </summary>
    public class TimeSeriesOnSpeciesObservationAbundanceIndexExcelXlsx : ExcelXlsxBase
    {
        /// <summary>
        /// Periodicity
        /// </summary>
        private readonly Periodicity _periodicity;

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSeriesOnSpeciesObservationAbundanceIndexExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        /// <param name="periodicity">Periodicity</param>
        /// <param name="addSettings">True if settings sheet should be included</param>
        /// <param name="addProvenance">True if provenance sheet should be included.</param>
        public TimeSeriesOnSpeciesObservationAbundanceIndexExcelXlsx(IUserContext currentUser, Periodicity periodicity, bool addSettings, bool addProvenance)
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
            var memoryStream = new MemoryStream();
           
            try
            {
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    var calculator = new SpeciesObservationAbundanceIndexDiagramResultCalculator(currentUser, SessionHandler.MySettings);
                    var data = calculator.GetAbundanceIndexDataResults((int)_periodicity);

                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet);
                    AddSpeciesObservationsData(worksheet, data);
                    FormatHeader(worksheet, 1, 6);

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
            worksheet.Cells[1, 1].Value = "TaxonId";
            worksheet.Cells[1, 2].Value = "Taxon name";
            worksheet.Cells[1, 3].Value = "Time step";
            worksheet.Cells[1, 4].Value = "Abundance index";
            worksheet.Cells[1, 5].Value = "Count";
            worksheet.Cells[1, 6].Value = "Total count";

            //Set column widths
            worksheet.Column(1).Width = 17;
            worksheet.Column(2).Width = 38;
            worksheet.Column(3).Width = 19;
            worksheet.Column(4).Width = 19;
            worksheet.Column(5).Width = 19;
            worksheet.Column(6).Width = 19;
        }

        private void AddSpeciesObservationsData(ExcelWorksheet worksheet, IEnumerable<KeyValuePair<TaxonViewModel, List<AbundanceIndexData>>> data)
        {
            var rowIndex = 2;

            // Data values
            foreach (KeyValuePair<TaxonViewModel, List<AbundanceIndexData>> entry in data)
            {
                foreach (AbundanceIndexData abundanceIndexData in entry.Value)
                {
                    worksheet.Cells[rowIndex, 1].Value = entry.Key.TaxonId;
                    worksheet.Cells[rowIndex, 2].Value = entry.Key.FullName;
                    worksheet.Cells[rowIndex, 3].Value = abundanceIndexData.TimeStep;
                    worksheet.Cells[rowIndex, 4].Value = abundanceIndexData.AbundanceIndex.HasValue
                                                                       ? abundanceIndexData.AbundanceIndex.Value
                                                                       : (double?)null;
                    worksheet.Cells[rowIndex, 5].Value = abundanceIndexData.Count;
                    worksheet.Cells[rowIndex, 6].Value = abundanceIndexData.TotalCount;

                    rowIndex++;
                }
            }
        }
    }
}
