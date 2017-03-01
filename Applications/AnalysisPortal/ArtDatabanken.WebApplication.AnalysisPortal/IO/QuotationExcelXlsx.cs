using Resources;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using OfficeOpenXml;
using System;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    
    /// <summary>
    /// A class that can be used to add a Quotation Worksheets to the xlsx.
    /// </summary>
    public class QuotationExcelXlsx : ExcelXlsxBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsReportExcelXlsx"/> class.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        public QuotationExcelXlsx(IUserContext currentUser)
        {
            IsColumnHeaderBackgroundUsed = true;
            this.currentUser = currentUser;
        }

        /// <summary>
        /// Gets a stream representation of the xlsx file.
        /// </summary>
        /// <returns>A memory stream.</returns>
        public MemoryStream ToStream()
        {
            var memoryStream = new MemoryStream();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(Resource.QuotationReportSheetName);
                this.PopulateQuotationWorkSheet(worksheet);
                package.SaveAs(memoryStream);
            }

            memoryStream.Position = 0;

            return memoryStream;
        }

        /// <summary>
        /// Populates the Quotation tab with the quoutation text and all the used Datasources.
        /// </summary>
        /// <param name="worksheet">The Worsheet that wou will populate</param>
        /// <param name="autosizeColumnWidth"></param>
        public void PopulateQuotationWorkSheet(ExcelWorksheet worksheet, bool autosizeColumnWidth = true)
        {
            var rowIndex = 1;
            string downloadDate = DateTime.Now.ToShortDateString();
            var settingsData = MySettingsSummaryItemManager.GetSettingReportData(this.currentUser);

            this.AddHeaderForWorksheet(worksheet, ref rowIndex);
            rowIndex++;
            string citationText = Resource.TextCitation;
            string formatedCitationText = string.Empty;

            while (citationText.Contains("<p>"))
            {               
                formatedCitationText = GetBetween(citationText, "<p>", "</p>");

                if (formatedCitationText.Contains("["))
                {                    
                    formatedCitationText = GetBetween("<p>" + formatedCitationText + "</p>", "<p>", "[");
                    this.AddContentDataForDataProviders(worksheet, formatedCitationText + " " + downloadDate + ":", ref rowIndex);            
                    formatedCitationText = GetBetween(citationText, "[", "]");                    
                    foreach (var settingReport in settingsData)
                    {
                        switch (settingReport.Key)
                        {
                            case "DataProviders":
                                this.AddDataSources(worksheet, (List<DataProviderViewModel>)settingReport.Value, ref rowIndex);
                                break;
                        }
                    }
                    formatedCitationText = GetBetween(citationText, "<p>", "</p>");
                    formatedCitationText = GetBetween("<p>" + formatedCitationText + "</p>", "]", "</p>");
                    this.AddContentDataForDataProviders(worksheet, formatedCitationText, ref rowIndex);
                    citationText = citationText.Replace("<p>" + GetBetween(citationText, "<p>", "</p>") + "</p>", "");
                }
                else
                {
                    this.AddContentDataForDataProviders(worksheet, formatedCitationText, ref rowIndex);
                    citationText = citationText.Replace("<p>" + formatedCitationText + "</p>", ""); 
                }                
            }
                worksheet.Cells.AutoFitColumns(0);            
        }

        /// <summary>
        /// Returns a substring between the first accurance of two strings.
        /// </summary>
        /// <param name="strSource">The string to search in</param>
        /// <param name="strStart">The start string </param>
        /// <param name="strEnd">The end string</param>
        /// <returns>Returns a substring between the first accurance of two strings.</returns>
        private static string GetBetween(string strSource, string strStart, string strEnd)
        {
            int start, end;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                start = strSource.IndexOf(strStart, 0) + strStart.Length;
                end = strSource.IndexOf(strEnd, start);
                return strSource.Substring(start, end - start);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Adds all the Data providers in dataProviders to the Excelsheet
        /// </summary>
        /// <param name="worksheet">The current worksheet</param>
        /// <param name="dataProviders">an IEnumerable with DataProviderViewModels</param>
        /// <param name="rowIndex">the current Row index</param>
        private void AddDataSources(ExcelWorksheet worksheet,IEnumerable<DataProviderViewModel> dataProviders, ref int rowIndex)
        {
           foreach (var dataProvider in dataProviders)
            {
                this.AddContentDataForDataProviders(worksheet, dataProvider.Name, ref rowIndex);
            }

        }

        /// <summary>
        /// Adds a Header to the worksheet and moves rowindex to the next row.
        /// </summary>
        /// <param name="worksheet">The current worksheet</param>
        /// <param name="rowIndex">the current Row index</param>
        private void AddHeaderForWorksheet(ExcelWorksheet worksheet, ref int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = Resource.QuotationReportSheetName;
            this.FormatHeader(worksheet, rowIndex, 1);

            rowIndex++;
        }

        /// <summary>
        /// Adds Quotation text string to a cell in the worksheet and moves rowindex to the next row
        /// </summary>
        /// <param name="worksheet">The current worksheet</param>
        /// <param name="quotationText"></param>
        /// <param name="rowIndex">the current Row index</param>
        private void AddContentDataForDataProviders(ExcelWorksheet worksheet, string quotationText, ref int rowIndex)
        {
            if (quotationText.StartsWith(". "))
            {
                quotationText = quotationText.Substring(2, quotationText.Length - 2);
            }

            worksheet.Cells[rowIndex, 1].Value = quotationText;
               
            rowIndex++;
        }
    }
}
