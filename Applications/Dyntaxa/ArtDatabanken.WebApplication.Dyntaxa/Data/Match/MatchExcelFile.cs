using System;
using System.Data.OleDb;
using System.IO;
using System.Web;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Export;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Match
{
    public class MatchExcelFile
    {
        private System.Data.DataTable _dataTable = null;

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
        static MatchExcelFile()
        {
            IsColumnHeaderBackgroundUsed = true;
        }

        /// <summary>
        /// Creates an excel file.
        /// Writes the content of a DataTable into a worksheet of an excelfile and save the file.
        /// </summary>
        /// <param name="dataTable">The DataTable object that should be exported to an excelfile</param>
        /// <param name="fileFormat"></param>
        /// <param name="autosizeColumnWidth"></param>
        public MemoryStream CreateExcelFile(System.Data.DataTable dataTable, ExcelFileFormat fileFormat, bool autosizeColumnWidth = false)
        {
            MemoryStream memoryStream;
            ExcelWorksheet workSheet = null;
            memoryStream = new MemoryStream();

            try
            {
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    // add a new worksheet to the empty workbook
                    workSheet = package.Workbook.Worksheets.Add("Taxa");

                    for (Int32 colIndex = 0; colIndex < dataTable.Columns.Count; colIndex++)
                    {
                        workSheet.Cells[1, colIndex + 1].Value = dataTable.Columns[colIndex].Caption;
                    }

                    // Formating straight columns
                    if (IsColumnHeaderBackgroundUsed)
                    {
                        using (var range = workSheet.Cells[1, 1, 1, dataTable.Columns.Count])
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
                            workSheet.Cells[rowIndex + 2, colIndex + 1].Value = dataTable.Rows[rowIndex][colIndex];
                        }
                    }

                    if (autosizeColumnWidth)
                    {
                        workSheet.Cells.AutoFitColumns(0);                        
                    }

                    package.Save();
                }

                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (Exception ex)
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }

                throw ex;
            }
        }

        public System.Data.DataTable ReadExcelFile(String fileName, bool excludeFirstRow = false)
        {
            String extension = Path.GetExtension(fileName);
            var dataTable = new System.Data.DataTable();

            String connectionStringBase = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""[Version];HDR=[HeaderOption];IMEX=1""";
            String version = "";
            String headerOption = "NO";
            if (excludeFirstRow)
            {
                headerOption = "YES";
            }
            if (extension == ".xlsx")
            {
                version = "Excel 12.0 Xml";
                //dbConnection = new OleDbConnection(String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1""", fileName));
            }
            else
            {
                version = "Excel 8.0";
                //dbConnection = new OleDbConnection(String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=Yes;""", fileName));
            }
            connectionStringBase = connectionStringBase.Replace("[Version]", version);
            connectionStringBase = connectionStringBase.Replace("[HeaderOption]", headerOption);
            var dbConnection = new OleDbConnection(String.Format(connectionStringBase, fileName));
            dbConnection.Open();
            try
            {
                // Get the name of the first worksheet:
                System.Data.DataTable dbSchema = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dbSchema == null || dbSchema.Rows.Count < 1)
                {
                    throw new Exception("Error: Could not determine the name of the first worksheet.");
                }
                string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();

                // Now we have the table name; proceed as before:
                var dbCommand = new OleDbCommand("SELECT * FROM [" + firstSheetName + "]", dbConnection);
                var adapter = new OleDbDataAdapter(dbCommand);
                adapter.Fill(dataTable);
            }
            finally
            {
                dbConnection.Close();
            }

            //ReplaceHexValuesInColumns(dataTable);
            _dataTable = dataTable;

            return _dataTable;
        }

        private string GetFileName(ExcelFileFormat fileformat)
        {
            string tempDirectory = Resources.DyntaxaSettings.Default.PathToTempDirectory;
            string fileName = Path.GetRandomFileName();
            fileName = Path.ChangeExtension(fileName, ExcelFileFormatHelper.GetExtension(fileformat));
            return HttpContext.Current.Server.MapPath(Path.Combine(tempDirectory, fileName));
        }

        private MemoryStream GetMemoryStream(string filename)
        {
            using (FileStream fileStream = File.OpenRead(filename))
            {
                var memStream = new MemoryStream();
                memStream.SetLength(fileStream.Length);
                fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                return memStream;
            }
        }
    }
}
