using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using System.Linq;
using System.Text;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService
{
    public static class StringExtension
    {
        public static DataRow[] GetRows(this DataTable table, string filterExpression)
        {
            var rows = table.Select(filterExpression);
            //Debug.WriteLine(string.Format("[{0}] {1} rows", filterExpression, rows.Length));
            return rows;
        }

        public static bool IsInteger(this string argument)
        {
            if (argument == null)
            {
                return false;
            }
            int dummyOutValue;
            return int.TryParse(argument, out dummyOutValue);
        }

        public static bool IsDouble(this string argument)
        {
            if (argument == null)
            {
                return false;
            }
            double dummyOutValue;
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign;
            return double.TryParse(argument.Replace(",", "."), styles, CultureInfo.CreateSpecificCulture("en-GB"), out dummyOutValue);
        }

        /// <summary>
        /// Adds a querystring value to the string
        ///  If the value is null or empty nothing is added to the resulting string.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddSimpleQueryValue(this string argument, object value)
        {
            if (value.IsNull())
            {
                return argument;
            }

            return string.Format("{0}{1}", argument, value);
        }

        /// <summary>
        /// Adds a querystring parameter to the string, "&key=value"
        ///  If any of the values are null or empty nothing is added to the resulting string.
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddSimpleQueryParameter(this string argument, string key, object value)
        {
            if (key.IsEmpty() || value.IsNull())
            {
                return argument;
            }

            return string.Format("{0}&{1}={2}", argument, key, value);
        }

        /// <summary>
        /// Adds a querystring range parameter to the string, "&key=lowerValue,upperValue"
        /// If any of the values are null or empty nothing is added to the resulting string.
        /// If either lowerValue or upperValue is a date, it's added to the string in the "yyyy-MM-dd" format
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="key"></param>
        /// <param name="lowerValue"></param>
        /// <param name="upperValue"></param>
        /// <returns></returns>
        public static string AddRangeQueryParameter(this string argument, string key, object lowerValue, object upperValue)
        {
            if (key.IsEmpty() || lowerValue.IsNull() || upperValue.IsNull())
            {
                return argument;
            }

            if (lowerValue is DateTime)
            {
                lowerValue = ((DateTime)lowerValue).ToString("yyyy-MM-dd");
            }
            if (upperValue is DateTime)
            {
                upperValue = ((DateTime)upperValue).ToString("yyyy-MM-dd");
            }

            return string.Format("{0}&{1}={2},{3}", argument, key, lowerValue, upperValue);
        }

        /// <summary>
        /// Replaces any misinterpreted culture specific characters in the string value, like 'Ã¶' to 'ö'
        /// </summary>
        /// <param name="value">String value that should be checked for strange characters.</param>
        /// <returns>String value with invalid characters replaced.</returns>
        public static string ReplaceInvalidCharacters(this string value)
        {
            if (value.IsEmpty())
            {
                return value;
            }
            else
            {
                // Replacements from http://www.i18nqa.com/debug/utf8-debug.html
                return value.
                       Replace("Â²", "²").
                       Replace("Â³", "³").

                       Replace("Ã‰", "É").
                       Replace("Å ", "Š").
                       Replace("Ãœ", "Ü").
                       Replace("Å½", "Ž").
                       Replace("Ã…", "Å").
                       Replace("Ã„", "Ä").
                       Replace("Ã–", "Ö").

                       Replace("Ã ", "à").
                       Replace("Ã©", "é").
                       Replace("Å¡", "š").
                       Replace("Ã¼", "ü").
                       Replace("Å¾", "ž").
                       Replace("Ã¥", "å").
                       Replace("Ã¤", "ä").
                       Replace("Ã¶", "ö");
            }
        }
    }
}
