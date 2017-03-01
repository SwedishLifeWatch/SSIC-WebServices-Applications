using Resources;
using System;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using OfficeOpenXml;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{    
    /// <summary>
    /// A class that can be used for downloads of data provider list Excel file.
    /// </summary>
    public class DataProviderListExcelXlsx : ExcelXlsxBase
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
        /// Initializes a new instance of the <see cref="DataProviderListExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context.</param>
        public DataProviderListExcelXlsx(IUserContext currentUser)
        {
            IsColumnHeaderBackgroundUsed = true;
            base.currentUser = currentUser;
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
                    var viewManager = new DataProvidersViewManager(currentUser, SessionHandler.MySettings);
                    var data = viewManager.CreateDataProvidersViewModel();

                    // Add a new worksheet to the empty workbook.
                    // The name of the sheet can not be longer than 31 characters.
                    var worksheet = package.Workbook.Worksheets.Add("SLW Data");
                    AddHeaders(worksheet);
                    AddContentData(worksheet, data);
                    FormatHeader(worksheet, 1, 4);

                    if (autosizeColumnWidth)
                    {
                        worksheet.Cells.AutoFitColumns(0);
                    }
                    
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
            worksheet.Cells[1, 1].Value = Resource.DataProvidersDataProvidersDataProvider;
            worksheet.Cells[1, 2].Value = "Organisation";
            worksheet.Cells[1, 3].Value = Resource.DataProvidersDataProvidersNumberOfObservations;
            worksheet.Cells[1, 4].Value = Resource.DataProvidersDataProvidersNumberOfPublicObservations;
        }

        private void AddContentData(ExcelWorksheet worksheet, DataProvidersViewModel data)
        {
            var rowIndex = 2;

            foreach (var row in data.DataProviders)
            {
                worksheet.Cells[rowIndex, 1].Value = row.Name;
                worksheet.Cells[rowIndex, 2].Value = row.Organization;
                worksheet.Cells[rowIndex, 3].Value = row.NumberOfObservations;
                worksheet.Cells[rowIndex, 4].Value = row.NumberOfPublicObservations;

                rowIndex++;
            }
        }
    }
}
