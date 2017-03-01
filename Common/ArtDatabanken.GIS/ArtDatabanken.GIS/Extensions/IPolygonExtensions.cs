using System;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.GIS.Extensions
{
    /// <summary>
    /// Extensions methods for classes that implements the IPolygonExtensions interface.
    /// </summary>
    public static class IPolygonExtensions
    {        
        /// <summary>
        /// Get polygon information in WKT format.
        /// </summary>
        /// <param name="polygon">This polygon.</param>
        /// <returns>Polygon information in WKT format.</returns>
        public static String GetWkt(this IPolygon polygon)
        {
            Boolean isFirstLinearRing, isFirstPoint;
            StringBuilder wkt;

            wkt = new StringBuilder("POLYGON");
            wkt.Append("(");
            if (polygon.LinearRings.IsNotEmpty())
            {
                isFirstLinearRing = true;
                foreach (ILinearRing linearRing in polygon.LinearRings)
                {
                    if (isFirstLinearRing)
                    {
                        isFirstLinearRing = false;
                    }
                    else
                    {
                        wkt.Append(", ");
                    }
                    wkt.Append("(");
                    if (linearRing.Points.IsNotEmpty())
                    {
                        isFirstPoint = true;
                        foreach (IPoint point in linearRing.Points)
                        {
                            if (isFirstPoint)
                            {
                                isFirstPoint = false;
                            }
                            else
                            {
                                wkt.Append(", ");
                            }
                            wkt.Append(point.X.WebToStringR().Replace(",", "."));
                            wkt.Append(" " + point.Y.WebToStringR().Replace(",", "."));
                        }
                    }
                    wkt.Append(")");
                }
            }
            wkt.Append(")");
            return wkt.ToString();
        }
    }
}
