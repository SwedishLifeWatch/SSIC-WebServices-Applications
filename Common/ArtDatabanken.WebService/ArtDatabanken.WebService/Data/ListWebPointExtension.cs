using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list
    /// of type WebPoint.
    /// </summary>
    public static class ListWebPointExtension
    {
        /// <summary>
        /// Convert points to JSON format.
        /// </summary>
        /// <param name="points">Points that should be converted.</param>
        /// <returns>Points in JSON format.</returns>
        public static String GetJson(this List<WebPoint> points)
        {
            Boolean firstPoint;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (points.IsNotEmpty())
            {
                firstPoint = true;
                stringBuilder.Append("[");
                foreach (WebPoint point in points)
                {
                    if (firstPoint)
                    {
                        firstPoint = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(point.GetJson());
                }

                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get points as string.
        /// </summary>
        /// <param name="points">Points that should be converted.</param>
        /// <returns>Points as string.</returns>
        public static String WebToString(this List<WebPoint> points)
        {
            Boolean firstPoint;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (points.IsNotEmpty())
            {
                firstPoint = true;
                stringBuilder.Append("Points = [");
                foreach (WebPoint point in points)
                {
                    if (firstPoint)
                    {
                        firstPoint = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(point.GetJson());
                }

                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }
    }
}
