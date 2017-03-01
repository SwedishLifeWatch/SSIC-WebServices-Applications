using System;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of combined grid statistics.
    /// </summary>
    public class ExcelXlsxBase
    {
        /// <summary>
        /// Holds current user context
        /// </summary>
        protected IUserContext currentUser;

        /// <summary>
        /// True if settings sheet should be included in report
        /// </summary>
        protected bool addSettings;

        /// <summary>
        /// True if provenance sheet should be included in report
        /// </summary>
        protected bool addProvenance;

        /// <summary>
        /// True if provenance sheet should be included in report
        /// </summary>
        protected bool addQuotation = true;

        /// <summary>
        /// Handle if column header background fill pattern is used.
        /// </summary>
        public static Boolean IsColumnHeaderBackgroundUsed { get; set; }

        protected void FormatHeader(ExcelWorksheet worksheet, int rowIndex, int lastColumnIndex)
        {
            // Formatting straight columns
            if (IsColumnHeaderBackgroundUsed)
            {
                // Format style by columns in first row
                using (var range = worksheet.Cells[rowIndex, 1, rowIndex, lastColumnIndex])
                {
                    range.Style.Font.Bold = false;
                    range.Style.Font.Color.SetColor(ExcelHelper.ColorTable[0]);
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(ExcelHelper.ColorTable[57]);
                }
            }
        }

        protected void AddAditionalSheets(ExcelPackage package)
        {
            if (addProvenance)
            {
                //Create a new sheet for settings and populate it
                var provenanceWorksheet = package.Workbook.Worksheets.Add(Resource.SpeciesObservationProvenanceReportSheetName);
                var provenanceReport = new SpeciesObservationProvenanceExcelXlsx(currentUser);
                provenanceReport.PopulateSheet(provenanceWorksheet);
            }

            if (addSettings)
            {
                //Create a new sheet for settings and populate it
                var settingWorksheet = package.Workbook.Worksheets.Add(Resource.SettingsReportSheetName);
                var settingsReport = new SettingsReportExcelXlsx(currentUser);
                settingsReport.PopulateSettingsWorkSheet(settingWorksheet);
            }
            if (addQuotation)
            {
                //Create a new sheet for settings and populate it
                var quotationWorksheet = package.Workbook.Worksheets.Add(Resource.QuotationReportSheetName);
                var quotationReport = new QuotationExcelXlsx(currentUser);
                quotationReport.PopulateQuotationWorkSheet(quotationWorksheet);
            }
        }
    }
}
