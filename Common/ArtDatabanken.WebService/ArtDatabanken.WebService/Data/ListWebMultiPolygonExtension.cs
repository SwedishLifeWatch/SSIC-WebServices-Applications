using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list
    /// of type WebMultiPolygon.
    /// </summary>
    public static class ListWebMultiPolygonExtension
    {
        /// <summary>
        /// Get multi polygon list as string.
        /// </summary>
        /// <param name="polygons">Polygons that should be converted.</param>
        /// <returns>Multi polygon list as string.</returns>
        public static String WebToString(this List<WebMultiPolygon> multiPolygons)
        {
            Boolean firstPolygon;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (multiPolygons.IsNotEmpty())
            {
                firstPolygon = true;
                stringBuilder.Append("[");
                foreach (WebMultiPolygon multiPolygon in multiPolygons)
                {
                    if (firstPolygon)
                    {
                        firstPolygon = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(multiPolygon.WebToString());
                }

                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }
    }
}
