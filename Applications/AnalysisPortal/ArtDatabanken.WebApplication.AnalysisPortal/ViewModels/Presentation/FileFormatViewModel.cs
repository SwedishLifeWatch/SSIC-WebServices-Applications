using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.FileFormat;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Presentation
{
    /// <summary>
    /// View model for ~/Format/FileFormat.
    /// </summary>
    public class FileFormatViewModel
    {        
        public bool CsvQuoteAllColumns { get; set; }
        public CsvSeparator CsvEnumSeparator { get; set; }
        public ExcelFileType ExcelFileFormatType { get; set; }
        public bool IsSettingsDefault { get; set; }

        /// <summary>
        /// Gets the CSV quote columns description.
        /// </summary>
        /// <returns>CSV quote columns description.</returns>
        public string GetCsvQuoteColumnsDescription()
        {
            return CsvQuoteAllColumns ? Resource.SharedBoolTrueText : Resource.SharedBoolFalseText;
        }

        /// <summary>
        /// Gets a CSV column separator description.
        /// </summary>
        /// <returns>A CSV column separator description.</returns>
        public string GetCsvColumnSeparatorDescription()
        {
            switch (CsvEnumSeparator)
            {
                case MySettings.Presentation.FileFormat.CsvSeparator.Comma:
                    return Resource.PresentationFileFormatCsvSeparatorComma;
                case MySettings.Presentation.FileFormat.CsvSeparator.Semicolon:
                    return Resource.PresentationFileFormatCsvSeparatorSemicolon;
                case MySettings.Presentation.FileFormat.CsvSeparator.Pipe:
                    return Resource.PresentationFileFormatCsvSeparatorPipe;
                case MySettings.Presentation.FileFormat.CsvSeparator.Tab:
                    return Resource.PresentationFileFormatCsvSeparatorTab;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Get a Excel file format description.
        /// </summary>
        /// <returns>A Excel file format description.</returns>
        public string GetExcelFileFormatDescription()
        {
            switch (ExcelFileFormatType)
            {
                case ExcelFileType.Xml:
                    return Resource.PresentationExcelFileFormatXml;
                case ExcelFileType.Xlsx:
                    return Resource.PresentationExcelFileFormatXlsx;
                default:
                    return string.Empty;
            }
        }
    }
}
