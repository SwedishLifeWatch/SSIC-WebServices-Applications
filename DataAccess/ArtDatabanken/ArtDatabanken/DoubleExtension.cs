using System;
using System.Globalization;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the Double class.
    /// </summary>
    public static class DoubleExtension
    {
        /// <summary>
        /// Get a fixed string representation of a Double value
        /// to use over the internet.
        /// </summary>
        /// <param name='value'>Double value to convert to a string.</param>
        /// <returns>The Double value as a string.</returns>
        public static String WebToString(this Double value)
        {
            return value.ToString("E10", CultureInfo.CreateSpecificCulture("en-GB"));
        }
    }
}
