using System;
using System.Data.SqlTypes;
using System.Text;
using ArtDatabanken.WebService.Data;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains extension to the WebPolygon class.
    /// </summary>
    public static class WebPolygonExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        public static void CheckData(this WebPolygon polygon)
        {
            if (polygon.IsNotNull())
            {
                polygon.LinearRings.CheckNotEmpty("LinearRings");
                foreach (WebLinearRing linearRing in polygon.LinearRings)
                {
                    linearRing.CheckData();
                }
            }
        }

        /// <summary>
        /// Get bounding box for provided polygon.
        /// Currently only 2 dimensions are handled.
        /// </summary>
        /// <param name="polygon">This polygon.</param>
        /// <returns>Bounding box for provided polygon.</returns>
        public static WebBoundingBox GetBoundingBox(this WebPolygon polygon)
        {
            WebBoundingBox boundingBox;

            boundingBox = null;
            if (polygon.LinearRings.IsNotEmpty() &&
                polygon.LinearRings[0].Points.IsNotEmpty())
            {
                foreach (WebPoint point in polygon.LinearRings[0].Points)
                {
                    if (boundingBox.IsNull())
                    {
                        boundingBox = new WebBoundingBox();
                        boundingBox.Max = point.Clone();
                        boundingBox.Min = point.Clone();
                    }
                    else
                    {
                        if (boundingBox.Max.X < point.X)
                        {
                            boundingBox.Max.X = point.X;
                        }
                        if (boundingBox.Max.Y < point.Y)
                        {
                            boundingBox.Max.Y = point.Y;
                        }
                        if (boundingBox.Min.X > point.X)
                        {
                            boundingBox.Min.X = point.X;
                        }
                        if (boundingBox.Min.Y > point.Y)
                        {
                            boundingBox.Min.Y = point.Y;
                        }
                    }
                }
            }
            return boundingBox;
        }

        /// <summary>
        /// Get a SqlGeography instance with same 
        /// information as provided WebPolygon.
        /// </summary>
        /// <param name="polygon">This polygon.</param>
        /// <returns>
        /// A SqlGeography instance with same 
        /// information as provided WebPolygon.
        /// </returns>
        public static SqlGeography GetGeography(this WebPolygon polygon)
        {
            Boolean isFirstLinearRing;
            String comma;
            StringBuilder reverseWkt;

            try
            {
                return SqlGeography.Parse(new SqlString(polygon.GetWkt()));
            }
            catch (Exception)
            {
            }

            try
            {
                // Try with points in reverse order.
                reverseWkt = new StringBuilder("POLYGON()");

                if (polygon.IsNotNull() &&
                    polygon.LinearRings.IsNotEmpty())
                {
                    isFirstLinearRing = true;
                    foreach (WebLinearRing linearRing in polygon.LinearRings)
                    {
                        if (linearRing.Points.IsNotEmpty())
                        {
                            if (isFirstLinearRing)
                            {
                                isFirstLinearRing = false;
                                reverseWkt.Insert(8, "()");
                            }
                            else
                            {
                                reverseWkt.Insert(8, "(), ");
                            }

                            comma = String.Empty;

                            foreach (WebPoint point in linearRing.Points)
                            {

                                reverseWkt.Insert(9, point.X.WebToStringR().Replace(",", ".") + " " + (point.Y.WebToStringR().Replace(",", ".")) + comma);

                                comma = ", ";
                            }
                        }
                    }
                }

                return SqlGeography.Parse(new SqlString(reverseWkt.ToString()));
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Get a SqlGeometry instance with same 
        /// information as provided WebPolygon.
        /// </summary>
        /// <param name="polygon">This polygon.</param>
        /// <returns>
        /// A SqlGeometry instance with same 
        /// information as provided WebPolygon.
        /// </returns>
        public static SqlGeometry GetGeometry(this WebPolygon polygon)
        {
            return SqlGeometry.Parse(new SqlString(polygon.GetWkt()));
        }

        /// <summary>
        /// Get polygon information in WKT format.
        /// </summary>
        /// <param name="polygon">This polygon.</param>
        /// <returns>Polygon information in WKT format.</returns>
        public static String GetWkt(this WebPolygon polygon)
        {
            Boolean isFirstLinearRing, isFirstPoint;
            StringBuilder wkt;

            wkt = new StringBuilder("POLYGON");
            wkt.Append("(");
            if (polygon.LinearRings.IsNotEmpty())
            {
                isFirstLinearRing = true;
                foreach (WebLinearRing linearRing in polygon.LinearRings)
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
                        foreach (WebPoint point in linearRing.Points)
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

        /// <summary>
        /// Test if point is located inside polygon.
        /// Currently only two dimensions are handled.
        /// </summary>
        /// <param name="polygon">This polygon.</param>
        /// <param name='point'>Point.</param>
        /// <returns>True if point is located inside polygon.</returns>
        public static Boolean IsPointInsideGeometry(this WebPolygon polygon,
                                                    WebPoint point)
        {
            return polygon.GetGeometry().STContains(point.GetGeometry()).Value;
        }
    }
}
