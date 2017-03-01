using System;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the TimeSpan structure.
    /// </summary>
    public static class TimeSpanExtension
    {
        /// <summary>
        /// Gets the value of the current TimeSpan 
        /// structure expressed in whole seconds.
        /// </summary>
        /// <param name='value'>Int64 value to convert to a string.</param>
        /// <returns>The value of the current TimeSpan structure expressed in whole seconds.</returns>
        public static Int64 GetTotalSeconds(this TimeSpan value)
        {
            Int64 seconds;

            seconds = 0;
            if (value.IsNotNull())
            {
                seconds += value.Seconds;
                seconds += value.Minutes * 60;
                seconds += value.Hours * 3600;
                seconds += value.Days * 86400;
            }

            return seconds;
        }
    }
}
