using System;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the Int64 class.
    /// </summary>
    public static class Int64Extension
    {
        /// <summary>
        /// Get a fixed string representation of a Int64 value
        /// to use over the internet.
        /// </summary>
        /// <param name='value'>Int64 value to convert to a string.</param>
        /// <returns>The Int64 value as a string.</returns>
        public static String WebToString(this Int64 value)
        {
            return value.ToString("D");
        }
    }
}
