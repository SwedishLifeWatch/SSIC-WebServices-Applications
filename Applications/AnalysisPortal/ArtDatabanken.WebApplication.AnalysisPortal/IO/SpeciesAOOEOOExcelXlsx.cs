using System;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of species observations Excel file.
    /// </summary>
    public class SpeciesAOOEOOExcelXlsx : ExcelXlsxBase
    {
        private const int TaxonIdColumnIndex = 1;
        private const int ScientificNameColumnIndex = 2;
        private const int SwedishNameColumnIndex = 3;
        private const int AooColumnIndex = 4;
        private const int EooColumnIndex = 5;
        private const int HeadersLastColumnIndex = 5;
        private const int HeadersRowIndex = 1;
        private Tuple<int, string, string, string, string>[] _data;
        private IAooEooExcelFormatter aooEooExcelFormatter;

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            return CreateExcelFile(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesAOOEOOExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <param name="data">The data.</param>
        /// <param name="addSettings">if set to <c>true</c> settings sheet should be added.</param>
        /// <param name="addProvenance">if set to <c>true</c> provenance sheet should be added.</param>
        /// <param name="aooEooExcelFormatter">The aoo eoo excel formatter.</param>
        public SpeciesAOOEOOExcelXlsx(
            IUserContext currentUser, 
            Tuple<int, string, string, string, string>[] data, 
            bool addSettings, 
            bool addProvenance,
            IAooEooExcelFormatter aooEooExcelFormatter)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
            base.addSettings = addSettings;
            base.addProvenance = addProvenance;
            _data = data;
            this.aooEooExcelFormatter = aooEooExcelFormatter;
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
                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("AOO EOO");
                    AddHeaders(worksheet);
                    AddData(worksheet);                    
                    FormatHeader(worksheet, HeadersRowIndex, HeadersLastColumnIndex);

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
            CoordinateSystemId coordinateSystemId = (CoordinateSystemId)SessionHandler.MySettings.Calculation.GridStatistics.CoordinateSystemId.Value;

            worksheet.Cells[HeadersRowIndex, TaxonIdColumnIndex].Value = Resource.LabelTaxonId;
            worksheet.Cells[HeadersRowIndex, ScientificNameColumnIndex].Value = Resource.TaxonSharedScientificName;
            worksheet.Cells[HeadersRowIndex, SwedishNameColumnIndex].Value = Resource.TaxonSharedSwedishName;
            worksheet.Cells[HeadersRowIndex, AooColumnIndex].Value = aooEooExcelFormatter.GetAooHeader(coordinateSystemId);
            worksheet.Cells[HeadersRowIndex, EooColumnIndex].Value = aooEooExcelFormatter.GetEooHeader(coordinateSystemId);            
        }

        private void AddData(ExcelWorksheet worksheet)
        {
            var rowIndex = 2;

            foreach (var taxon in _data)
            {
                worksheet.Cells[rowIndex, TaxonIdColumnIndex].Value = taxon.Item1;
                worksheet.Cells[rowIndex, ScientificNameColumnIndex].Value = taxon.Item2;
                worksheet.Cells[rowIndex, SwedishNameColumnIndex].Value = taxon.Item3;
                long result;
                if (aooEooExcelFormatter.TryConvertKm2StringToNumber(taxon.Item4, out result))
                {
                    worksheet.Cells[rowIndex, AooColumnIndex].Value = result;
                }
                else
                {
                    worksheet.Cells[rowIndex, AooColumnIndex].Value = "";
                }

                if (aooEooExcelFormatter.TryConvertKm2StringToNumber(taxon.Item5, out result))
                {
                    worksheet.Cells[rowIndex, EooColumnIndex].Value = result;
                }
                else
                {
                    worksheet.Cells[rowIndex, EooColumnIndex].Value = "";
                }                
                                
                rowIndex++;
            }
        }
    }
}