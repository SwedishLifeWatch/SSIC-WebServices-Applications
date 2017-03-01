using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.FileFormat;
using Resources;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    /// <summary>
    /// This class contains extension methods for enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Tries to parse an enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="retVal">The return value.</param>
        /// <returns>True if the enum is defined; Otherwise false.</returns>
        public static bool TryParseEnum<TEnum>(this int enumValue, out TEnum retVal)
        {
            retVal = default(TEnum);
            bool success = Enum.IsDefined(typeof(TEnum), enumValue);
            if (success)
            {
                retVal = (TEnum)Enum.ToObject(typeof(TEnum), enumValue);
            }

            return success;
        }

        /// <summary>        
        /// Converts a value of type <see cref="ArtDatabanken.Data.StringCompareOperator"/> to a localized string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A localized string representation.</returns>
        public static string ToResourceName(this StringCompareOperator value)
        {
            switch (value)
            {
                case StringCompareOperator.BeginsWith:
                    return Resource.StringCompareOperatorBeginsWith;
                case StringCompareOperator.Contains:
                    return Resource.StringCompareOperatorContains;
                case StringCompareOperator.EndsWith:
                    return Resource.StringCompareOperatorEndsWith;
                case StringCompareOperator.Equal:
                    return Resource.StringCompareOperatorEqual;
                case StringCompareOperator.Like:
                    return Resource.StringCompareOperatorLike;
                case StringCompareOperator.NotEqual:
                    return Resource.StringCompareOperatorNotEqual;
                default:
                    return value.ToString();
            }
        }

        public static string ToText(this CsvSeparator separator)
        {
            switch (separator)
            {
                case CsvSeparator.Comma:
                    return Resource.PresentationFileFormatCsvSeparatorComma;
                case CsvSeparator.Pipe:
                    return Resource.PresentationFileFormatCsvSeparatorPipe;
                case CsvSeparator.Semicolon:
                    return Resource.PresentationFileFormatCsvSeparatorSemicolon;
                case CsvSeparator.Tab:
                    return Resource.PresentationFileFormatCsvSeparatorTab;
                default:
                    return string.Empty;
            }
        }

        public static string ToText(this ExcelFileType excelFileType)
        {
            switch (excelFileType)
            {
                case ExcelFileType.Xlsx:
                    return Resource.PresentationExcelFileFormatXlsx;
                case ExcelFileType.Xml:
                    return Resource.PresentationExcelFileFormatXml;
                default:
                    return string.Empty;
            }
        }
    }
}
