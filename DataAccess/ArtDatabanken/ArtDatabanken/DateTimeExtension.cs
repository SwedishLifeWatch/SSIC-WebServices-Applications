using System;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the DateTime class.
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Get a fixed string representation of a DateTime value
        /// to use over the internet.
        /// </summary>
        /// <param name='value'>DateTime value to convert to a string.</param>
        /// <returns>The DateTime value as a string.</returns>
        public static String WebToString(this DateTime value)
        {
            return value.ToString("o");
        }
    }
}
