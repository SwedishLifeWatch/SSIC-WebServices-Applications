using System;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains extension to the WebLinearRing class.
    /// </summary>
    public static class WebLinearRingExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="linearRing">The linear ring.</param>
        public static void CheckData(this WebLinearRing linearRing)
        {
            if (linearRing.IsNotNull())
            {
                linearRing.Points.CheckNotEmpty("Points");
                foreach (WebPoint point in linearRing.Points)
                {
                    point.CheckData();
                }
                if (linearRing.Points.Count < 3)
                {
                    throw new ArgumentException("A linear ring must contain at least 3 points.");
                }
                if (!linearRing.Points[0].Equal(linearRing.Points[linearRing.Points.Count - 1]))
                {
                    throw new ArgumentException("First and last point of a linear ring must be the same.");
                }
            }
        }
    }
}
