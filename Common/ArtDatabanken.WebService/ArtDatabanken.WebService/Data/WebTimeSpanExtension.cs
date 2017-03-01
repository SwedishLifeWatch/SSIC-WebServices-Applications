using System;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebTimeSpan class.
    /// </summary>
    public static class WebTimeSpanExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        public static void CheckData(this WebTimeSpan timeSpan)
        {
            if (timeSpan.IsNotNull())
            {
                if (timeSpan.IsDaysSpecified && (timeSpan.Days < 0))
                {
                    // Days must be zero or larger.
                    throw new ArgumentException("Days must be zero or larger. Current timeSpan = " + timeSpan.Days);
                }

                if (timeSpan.IsHoursSpecified && (timeSpan.Hours < 0))
                {
                    // Hours must be zero or larger.
                    throw new ArgumentException("Hours must be zero or larger. Current timeSpan = " + timeSpan.Hours);
                }

                if (timeSpan.IsMinutesSpecified && (timeSpan.Minutes < 0))
                {
                    // Minutes must be zero or larger.
                    throw new ArgumentException("Minutes must be zero or larger. Current timeSpan = " + timeSpan.Minutes);
                }

                if (timeSpan.IsSecondsSpecified && (timeSpan.Seconds < 0))
                {
                    // Seconds must be zero or larger.
                    throw new ArgumentException("Seconds must be zero or larger. Current timeSpan = " + timeSpan.Seconds);
                }

                if (timeSpan.IsNanoSecondsSpecified && (timeSpan.NanoSeconds < 0))
                {
                    // Nano seconds must be zero or larger.
                    throw new ArgumentException("Nano seconds must be zero or larger. Current timeSpan = " + timeSpan.NanoSeconds);
                }
            }
        }

        /// <summary>
        /// Gets the timeSpan of the current WebTimeSpan 
        /// class expressed in whole seconds.
        /// </summary>
        /// <param name='timeSpan'>Int64 timeSpan to convert to a string.</param>
        /// <returns>The timeSpan of the current WebTimeSpan class expressed in whole seconds.</returns>
        public static Int64 GetTotalSeconds(this WebTimeSpan timeSpan)
        {
            Int64 seconds;

            seconds = 0;
            if (timeSpan.IsNotNull())
            {
                if (timeSpan.IsSecondsSpecified)
                {
                    seconds += timeSpan.Seconds;
                }

                if (timeSpan.IsMinutesSpecified)
                {
                    seconds += timeSpan.Minutes * 60;
                }

                if (timeSpan.IsHoursSpecified)
                {
                    seconds += timeSpan.Hours * 3600;
                }
                
                if (timeSpan.IsDaysSpecified)
                {
                    seconds += timeSpan.Days * 86400;
                }
            }

            return seconds;
        }
    }
}
