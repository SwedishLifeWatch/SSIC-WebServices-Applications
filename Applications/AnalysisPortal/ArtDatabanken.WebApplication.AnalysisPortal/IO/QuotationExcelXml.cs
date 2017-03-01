using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Spatial;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;
using Resources;
using System;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{

    class QuotationExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Initialize SettingsReportExcelXml
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="createWoorkbook"></param>
        public QuotationExcelXml(IUserContext currentUser, bool createWoorkbook)
        {
            var rowsXml = new StringBuilder();
            var rowCount = 0;
            string downloadDate = DateTime.Now.ToShortDateString();
            var settingsData = MySettingsSummaryItemManager.GetSettingReportData(currentUser);

            string citationText = Resource.TextCitation;
            string formatedCitationText = string.Empty;            

            // Add row with column headers
            this.AddHeaderDataForDataProvider(ref rowsXml, Resource.QuotationReportSheetName, ref rowCount);
            this.AddContentDataForDataProviders(ref rowsXml, string.Empty, ref rowCount);

            while (citationText.Contains("<p>"))
            {
                formatedCitationText = GetBetween(citationText, "<p>", "</p>");

                if (formatedCitationText.Contains("["))
                {
                    formatedCitationText = GetBetween("<p>" + formatedCitationText + "</p>", "<p>", "[");
                    this.AddContentDataForDataProviders(ref rowsXml, formatedCitationText + " " + downloadDate + ":", ref rowCount);                    
                    formatedCitationText = GetBetween(citationText, "[", "]");                
                    foreach (var settingReport in settingsData)
                    {
                        switch (settingReport.Key)
                        {
                            case "DataProviders":
                                var dataProviders = (List<DataProviderViewModel>)settingReport.Value;

                                rowCount += dataProviders.Count + 2;                                
                                // Data values
                                foreach (var row in dataProviders)
                                {
                                    rowsXml.AppendLine(this.GetRowStart());
                                    rowsXml.AppendLine(this.GetDataRowLine("String", row.Name));
                                    rowsXml.AppendLine(this.GetRowEnd());
                                }

                                this.AddContentDataForDataProviders(ref rowsXml, string.Empty, ref rowCount);

                                break;
                        }
                    }
                    formatedCitationText = GetBetween(citationText, "<p>", "</p>");
                    formatedCitationText = GetBetween("<p>" + formatedCitationText + "</p>", "]", "</p>");
                    this.AddContentDataForDataProviders(ref rowsXml, formatedCitationText, ref rowCount);
                    citationText = citationText.Replace("<p>" + GetBetween(citationText, "<p>", "</p>") + "</p>", "");
                }
                else
                {
                    this.AddContentDataForDataProviders(ref rowsXml, formatedCitationText, ref rowCount);
                    citationText = citationText.Replace("<p>" + formatedCitationText + "</p>", "");
                }
            }

            var onlySheet = !createWoorkbook;
            this._xmlBuilder = new StringBuilder();

            // Create initial section or a new worksheet
            this._xmlBuilder.AppendLine(this.GetInitialSectionOrNewWorksheet(ref createWoorkbook, Resource.QuotationReportSheetName));

            // Specify column and row counts
            this._xmlBuilder.AppendLine(this.GetColumnInitialSection(4, rowCount));

            // Specify column widths
            this._xmlBuilder.AppendLine(this.GetColumnWidthLine(300));
            this._xmlBuilder.AppendLine(this.GetColumnWidthLine(270));
            this._xmlBuilder.AppendLine(this.GetColumnWidthLine(140));
            this._xmlBuilder.AppendLine(this.GetColumnWidthLine(140));

            this._xmlBuilder.Append(rowsXml);

            if (onlySheet)
            {
                return;
            }
            // Add final section of the xml document.
            this._xmlBuilder.AppendLine(this.GetFinalSection());
            this._xmlBuilder.Replace("&", "&amp;");
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
        /// AddsHeader text string to a cell in the worksheet and moves rowindex to the next row
        /// </summary>
        /// <param name="rowXml">The XML</param>
        /// <param name="headerText">string containing the header text</param>
        /// <param name="rowIndex">the current Row index</param>
        private void AddHeaderDataForDataProvider(ref StringBuilder rowXml, string headerText, ref int rowIndex)
        {
            rowXml.AppendLine(this.GetRowStart());
            rowXml.AppendLine(this.GetColumnNameRowLine(headerText));
            rowXml.AppendLine(this.GetRowEnd());
        }

        /// <summary>
        /// Adds Quotation text string to a cell in the worksheet and moves rowindex to the next row
        /// </summary>
        /// <param name="rowXml">The XML</param>
        /// <param name="quotationText"></param>
        /// <param name="rowIndex">the current Row index</param>
        private void AddContentDataForDataProviders(ref StringBuilder rowXml, string quotationText, ref int rowIndex)
        {
            if (quotationText.StartsWith(". "))
            {
                quotationText = quotationText.Substring(2, quotationText.Length - 2);
            }

            rowXml.AppendLine(this.GetRowStart());
            rowXml.AppendLine(this.GetDataRowLine("String", quotationText));
            rowXml.AppendLine(this.GetRowEnd());
            rowIndex++;
        }
    }
}