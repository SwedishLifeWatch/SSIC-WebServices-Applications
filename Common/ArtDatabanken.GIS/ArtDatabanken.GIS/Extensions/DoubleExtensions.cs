using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.Extensions
{
    /// <summary>
    /// Contains extension methods to the Double class.
    /// </summary>
    public static class DoubleExtension
    {
        private static readonly CultureInfo ConvertCultureInfo = CultureInfo.CreateSpecificCulture("en-GB");

        /// <summary>
        /// Get a fixed string representation of a Double value
        /// to be used when you want the exact value to
        /// be able to convert it back to Double.
        /// </summary>
        /// <param name="value">Double value to convert to a string.</param>
        /// <returns>The Double value as a string.</returns>
        public static String WebToStringR(this Double value)
        {
            return value.ToString("r", ConvertCultureInfo);
        }
    }
}
