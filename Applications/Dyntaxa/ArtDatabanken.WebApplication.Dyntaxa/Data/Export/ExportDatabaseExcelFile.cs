using System;
using System.IO;
using System.Collections.Generic;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// Handles database export to Excel file.
    /// </summary>
    public class ExportDatabaseExcelFile
    {
        private readonly System.Data.DataTable _dataTable = null;

        /// <summary>
        /// Handle if column header background fill pattern is used.
        /// </summary>
        public static Boolean IsColumnHeaderBackgroundUsed { get; set; }

        /// <summary>
        /// Gets the data table instance representing the data of the first worksheet of the Excel file.
        /// </summary>
        public System.Data.DataTable DataTable
        {
            get { return _dataTable; }
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ExportDatabaseExcelFile()
        {
            IsColumnHeaderBackgroundUsed = true;
        }

        /// <summary>
        /// Creates an excel file.
        /// Writes the content of a DataTable into a worksheet of an excelfile and save the file.
        /// </summary>
        /// <param name="dataTables">
        /// The DataTables object that should be exported to an excel file.
        /// </param>
        /// <param name="fileFormat">
        /// File format.
        /// </param>
        /// <param name="autosizeColumnWidth">
        /// If true, the columns will be autosized.
        /// </param>
        /// <returns>
        /// The <see cref="MemoryStream"/>.
        /// </returns>
        public MemoryStream CreateExcelFile(List<System.Data.DataTable> dataTables, ExcelFileFormat fileFormat, bool autosizeColumnWidth = false)
        {
            MemoryStream memoryStream;
            ExcelWorksheet worksheet = null;
            memoryStream = new MemoryStream();

            try
            {                
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    for (int tableIndex = 0; tableIndex < dataTables.Count; tableIndex++)
                    {
                        var dataTable = dataTables[tableIndex];
                        if (package.Workbook.Worksheets.Count <= tableIndex)
                        {
                            worksheet = package.Workbook.Worksheets.Add(dataTable.TableName);                            
                        }
                        
                        for (Int32 colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                        {
                            worksheet.Cells[1, colIndex + 1].Value = dataTable.Columns[colIndex].Caption;
                        }

                        // Formating straight columns
                        if (IsColumnHeaderBackgroundUsed)
                        {
                            using (var range = worksheet.Cells[1, 1, 1, dataTable.Columns.Count])
                            {
                                range.Style.Font.Bold = true;
                                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(ExcelHelper.ColorTable[15]);
                            }                            
                        }

                        for (Int32 rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                        {
                            for (Int32 colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                            {
                                worksheet.Cells[rowIndex + 2, colIndex + 1].Value = dataTable.Rows[rowIndex][colIndex];
                            }
                        }

                        if (autosizeColumnWidth)
                        {
                            worksheet.Cells.AutoFitColumns(0);                            
                        }
                    }

                    package.Save();
                }
                
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception)
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }

                throw;
            }
        }
    }
}
