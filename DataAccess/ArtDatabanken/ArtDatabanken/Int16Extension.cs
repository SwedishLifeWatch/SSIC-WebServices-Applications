using System;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the Int32 class.
    /// </summary>
    public static class Int16Extension
    {
        /// <summary>
        /// Get a fixed string representation of a Int32 value
        /// to use over the internet.
        /// </summary>
        /// <param name='value'>Int16 value to convert to a string.</param>
        /// <returns>The Int16 value as a string.</returns>
        public static String WebToString(this Int16 value)
        {
            return value.ToString("D");
        }
    }
}
