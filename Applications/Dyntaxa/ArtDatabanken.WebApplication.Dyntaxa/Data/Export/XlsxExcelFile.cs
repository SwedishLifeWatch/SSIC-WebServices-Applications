using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    /// <summary>
    /// Class that represent an xlsx excel file.
    /// It can be used for both exports and imports of data.
    /// </summary>
    public class XlsxExcelFile
    {
        /// <summary>
        /// The _data table.
        /// </summary>
        private DataTable _dataTable = null;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static XlsxExcelFile()
        {
            IsColumnHeaderBackgroundUsed = true;
        }

        /// <summary>
        /// Constructor of an excel file.
        /// This constructer opens an Excel file and with a specified file name and load the content of the first worksheet into the data table property.
        /// </summary>
        /// <param name="filePath">
        /// The file name including both the path of the destination and the file extention.
        /// </param>
        /// <param name="excludeFirstRow">
        /// Exclude first row.
        /// </param>
        public XlsxExcelFile(String filePath, bool excludeFirstRow)
        {
            _dataTable = GetDataTableFromExcel(filePath, excludeFirstRow);
        }

        /// <summary>
        /// Constructor of an excel file.
        /// This constructer opens an Excel file and with a specified file name and load the content of the first worksheet into the data table property.
        /// </summary>
        /// <param name="filePath">
        /// The file name including both the path of the destination and the file extention.
        /// </param>
        public XlsxExcelFile(String filePath)
        {
            _dataTable = GetDataTableFromExcel(filePath, true);
        }

        /// <summary>
        /// Handle if column header background fill pattern is used.
        /// </summary>
        public static Boolean IsColumnHeaderBackgroundUsed { get; set; }

        /// <summary>
        /// Gets the data table instance representing the data of the first worksheet of the Excel file.
        /// </summary>
        public DataTable DataTable
        {
            get { return _dataTable; }
        }

        /// <summary>
        /// Converts Excel (xlsx) file into a DataTable.
        /// </summary>
        /// <param name="filePath">
        /// The excel file path.
        /// </param>
        /// <param name="excludeFirstRow">
        /// If true, don't use the first row.
        /// </param>
        /// <returns>
        /// A <see cref="DataTable"/> representation of the Excel file.
        /// </returns>
        public static DataTable GetDataTableFromExcel(string filePath, bool excludeFirstRow)
        {
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    pck.Load(stream);
                }

                return GetDataTableFromExcelPackage(pck, excludeFirstRow);               
            }
        }

        /// <summary>
        /// Converts Excel (xlsx) stream into a DataTable.
        /// </summary>
        /// <param name="stream">
        /// The excel stream.
        /// </param>
        /// <param name="excludeFirstRow">
        /// If true, don't use the first row.
        /// </param>
        /// <returns>
        /// A <see cref="DataTable"/> representation of the Excel file.
        /// </returns>
        public static DataTable GetDataTableFromExcel(Stream stream, bool excludeFirstRow)
        {
            using (var pck = new ExcelPackage())
            {
                pck.Load(stream);
                return GetDataTableFromExcelPackage(pck, excludeFirstRow);                
            }
        }

        /// <summary>
        /// Converts a loaded Excel package into a DataTable.
        /// </summary>
        /// <param name="pck">
        /// The Excel package.
        /// </param>
        /// <param name="excludeFirstRow">
        /// If true, don't use the first row.
        /// </param>
        /// <returns>
        /// A <see cref="DataTable"/> representation of the Excel file.
        /// </returns>
        private static DataTable GetDataTableFromExcelPackage(ExcelPackage pck, bool excludeFirstRow)
        {
            var ws = pck.Workbook.Worksheets.First();
            DataTable tbl = new DataTable();
            bool hasHeader = excludeFirstRow; // adjust it accordingly( i've mentioned that this is a simple approach)
            foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
            {
                tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
            }

            var startRow = hasHeader ? 2 : 1;
            for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                var row = tbl.NewRow();
                foreach (var cell in wsRow)
                {
                    row[cell.Start.Column - 1] = cell.Text;
                }

                tbl.Rows.Add(row);
            }

            return tbl;
        }

        /// <summary>
        /// Gets an array list representation of an excel file.
        /// </summary>
        /// <param name="filePath">
        /// The excel file path.
        /// </param>
        /// <returns>
        /// An ArrayList containing one ArrayList for each row.
        /// </returns>
        public static ArrayList GetArrayListFromExcelFile(string filePath)
        {
            DataTable dataTable = GetDataTableFromExcel(filePath, false);
            return GetArrayListFromDataTable(dataTable);
        }

        /// <summary>
        /// Gets an array list representation of an excel file.
        /// </summary>
        /// <param name="stream">
        /// The Excel file stream.
        /// </param>
        /// <returns>
        /// An ArrayList containing one ArrayList for each row.
        /// </returns>
        public static ArrayList GetArrayListFromExcelFile(Stream stream)
        {
            DataTable dataTable = GetDataTableFromExcel(stream, false);
            return GetArrayListFromDataTable(dataTable);
        }

        /// <summary>
        /// Converts a DataTable to an ArrayList containing one ArrayList for each row.
        /// </summary>
        /// <param name="dataTable">
        /// The data table.
        /// </param>
        /// <returns>
        /// An ArrayList containing one ArrayList for each row.
        /// </returns>
        private static ArrayList GetArrayListFromDataTable(DataTable dataTable)
        {
            ArrayList arrayList = new ArrayList();            

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                ArrayList itemArrayList = new ArrayList();
                foreach (object obj in row.ItemArray)
                {
                    if (obj == DBNull.Value)
                    {
                        itemArrayList.Add("");
                    }
                    else
                    {
                        itemArrayList.Add(obj.ToString());
                    }
                }

                arrayList.Add(itemArrayList);
            }

            // Remove empty rows in the end of the list
            for (int i = arrayList.Count - 1; i >= 0; i--)
            {
                if (IsEmptyRow((ArrayList)arrayList[i]))
                {
                    arrayList.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }

            return arrayList;
        }

        /// <summary>
        /// Checks if all items in the array list are null or empty string.
        /// </summary>
        /// <param name="arrayList">
        /// The array list.
        /// </param>
        /// <returns>
        /// <c>true</c> if all items are null or empty string; otherwise <c>false</c>.
        /// </returns>
        private static bool IsEmptyRow(ArrayList arrayList)
        {
            if (arrayList == null)
            {
                return true;
            }

            if (arrayList.Count == 0)
            {
                return true;
            }

            foreach (object item in arrayList)
            {
                if (!string.IsNullOrEmpty(item.ToString()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
