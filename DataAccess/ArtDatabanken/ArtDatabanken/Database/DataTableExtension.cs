using System;
using System.Data;

namespace ArtDatabanken.Database
{
    /// <summary>
    /// Contains extension methods to the DataTable class.
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// Delete empty rows from data table.
        /// </summary>
        /// <param name='dataTable'>The data table.</param>
        public static void DeleteEmptyRows(this DataTable dataTable)
        {
            Boolean isDataFound;
            DataRow row;
            Int32 columnIndex, rowIndex;

            if (dataTable.IsNotNull() && dataTable.Rows.IsNotEmpty())
            {
                for (rowIndex = dataTable.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                {
                    isDataFound = false;
                    row = dataTable.Rows[rowIndex];
                    for (columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                    {
                        if (row[columnIndex].ToString().IsNotEmpty())
                        {
                            isDataFound = true;
                            break;
                        }
                    }
                    if (!isDataFound)
                    {
                        // Empty row that should be deleted.
                        dataTable.Rows.RemoveAt(rowIndex);
                    }
                }
            }
        }
    }
}
