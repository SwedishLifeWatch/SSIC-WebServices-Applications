using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebMultiPolygon class.
    /// </summary>
    public static class WebMultiPolygonExtension
    {
        /// <summary>
        /// Get bounding box for provided polygon.
        /// Currently only 2 dimensions are handled.
        /// </summary>
        /// <param name="multiPolygon">This polygon.</param>
        /// <returns>Bounding box for provided polygon.</returns>
        public static WebBoundingBox GetBoundingBox(this WebMultiPolygon multiPolygon)
        {
            WebBoundingBox boundingBox;

            boundingBox = null;
            if (multiPolygon.Polygons.IsNotEmpty())
            {
                foreach (WebPolygon polygon in multiPolygon.Polygons)
                {
                    if (boundingBox.IsNull())
                    {
                        boundingBox = polygon.GetBoundingBox();
                    }
                    else
                    {
                        boundingBox.Add(polygon.GetBoundingBox());
                    }
                }
            }
            return boundingBox;
        }

        /// <summary>
        /// Get a SqlGeography instance with same 
        /// information as provided WebMultiPolygon.
        /// </summary>
        /// <param name="multiPolygon">This multi polygon.</param>
        /// <returns>
        /// A SqlGeography instance with same 
        /// information as provided WebMultiPolygon.
        /// </returns>
        public static SqlGeography GetGeography(this WebMultiPolygon multiPolygon)
        {
            Boolean isFirstLinearRing, isFirstPoint, isFirstPolygon;
            StringBuilder wkt;

            wkt = new StringBuilder("MULTIPOLYGON()");

            try
            {
                if (multiPolygon.Polygons.IsNotEmpty())
                {
                    isFirstPolygon = true;
                    foreach (WebPolygon polygon in multiPolygon.Polygons)
                    {
                        if (polygon.LinearRings.IsNotEmpty())
                        {
                            if (isFirstPolygon)
                            {
                                isFirstPolygon = false;
                                wkt.Insert(13, "()");
                            }
                            else
                            {
                                wkt.Insert(13, "(), ");
                            }

                            isFirstLinearRing = true;
                            foreach (WebLinearRing linearRing in polygon.LinearRings)
                            {
                                if (linearRing.Points.IsNotEmpty())
                                {
                                    if (isFirstLinearRing)
                                    {
                                        isFirstLinearRing = false;
                                        wkt.Insert(14, "()");
                                    }
                                    else
                                    {
                                        wkt.Insert(14, "(), ");
                                    }

                                    String komma = String.Empty;

                                    foreach (WebPoint point in linearRing.Points)
                                    {

                                        wkt.Insert(15, point.X.WebToStringR().Replace(",", ".") + " " + (point.Y.WebToStringR().Replace(",", ".")) + komma);

                                        komma = ", ";
                                    }
                                }
                            }
                        }
                    }
                }

                return SqlGeography.Parse(new SqlString(wkt.ToString()));
            }
            catch (Exception)
            {
                // Debug.WriteLine("Parse did not work, try other direction...");
            }

            wkt = new StringBuilder("MULTIPOLYGON");
            wkt.Append("(");
            try
            {
                if (multiPolygon.Polygons.IsNotEmpty())
                {
                    isFirstPolygon = true;
                    foreach (WebPolygon polygon in multiPolygon.Polygons)
                    {
                        if (isFirstPolygon)
                        {
                            isFirstPolygon = false;
                        }
                        else
                        {
                            wkt.Append(", ");
                        }
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
                                        wkt.Append(point.X.WebToStringR().Replace(",", ".") + " " + (point.Y.WebToStringR().Replace(",", ".")));
                                    }
                                }
                                wkt.Append(")");
                            }
                        }
                        wkt.Append(")");
                    }
                }
                wkt.Append(")");

                return SqlGeography.Parse(new SqlString(wkt.ToString()));
            }
            catch (Exception)
            {
                //  Debug.WriteLine("Parse did not work in this direction either...");
            }
            Debug.WriteLine("SqlGeography, no parse possibel, return NULL");
            return null;
        }

        /// <summary>
        /// Get a SqlGeometry instance with same 
        /// information as provided WebMultiPolygon.
        /// </summary>
        /// <param name="multiPolygon">This multi polygon.</param>
        /// <returns>
        /// A SqlGeometry instance with same 
        /// information as provided WebMultiPolygon.
        /// </returns>
        public static SqlGeometry GetGeometry(this WebMultiPolygon multiPolygon)
        {
            StringBuilder wkt = GetGeometryWkt(multiPolygon);
            try
            {
                return SqlGeometry.Parse(new SqlString(wkt.ToString()));
            }
            catch (Exception)
            {
                //  Debug.WriteLine("SqlGeometry Error in Parse return NULL.");
            }
            return null;
        }

        /// <summary>
        /// Gets a string instance with same 
        /// information (wkt) as provided WebMultiPolygon
        /// </summary>
        /// <param name="multiPolygon">This multi polygon.</param>
        /// <returns>A string instance with same 
        /// information as provided WebMultiPolygon.
        /// </returns>
        public static StringBuilder GetGeometryWkt(WebMultiPolygon multiPolygon)
        {
            StringBuilder wkt;
            Boolean isFirstLinearRing, isFirstPoint, isFirstPolygon;
           
            wkt = new StringBuilder("MULTIPOLYGON");
            wkt.Append("(");
            if (multiPolygon.Polygons.IsNotEmpty())
            {
                isFirstPolygon = true;
                foreach (WebPolygon polygon in multiPolygon.Polygons)
                {
                    if (isFirstPolygon)
                    {
                        isFirstPolygon = false;
                    }
                    else
                    {
                        wkt.Append(", ");
                    }
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
                }
            }
            wkt.Append(")");
            return wkt;
        }

        /// <summary>
        /// Get WebMultiPolygon as a string.
        /// </summary>
        /// <param name="multiPolygon">This multi polygon.</param>
        /// <returns>WebMultiPolygon as a string</returns>
        public static String WebToString(this WebMultiPolygon multiPolygon)
        {
            Boolean isFirstLinearRing, isFirstPolygon;
            StringBuilder wkt;

            wkt = new StringBuilder();

            if (multiPolygon.IsNotNull())
            {
                wkt.Append("MULTIPOLYGON()");
                if (multiPolygon.Polygons.IsNotEmpty())
                {
                    isFirstPolygon = true;
                    foreach (WebPolygon polygon in multiPolygon.Polygons)
                    {
                        if (polygon.LinearRings.IsNotEmpty())
                        {
                            if (isFirstPolygon)
                            {
                                isFirstPolygon = false;
                                wkt.Insert(13, "()");
                            }
                            else
                            {
                                wkt.Insert(13, "(), ");
                            }

                            isFirstLinearRing = true;
                            foreach (WebLinearRing linearRing in polygon.LinearRings)
                            {
                                if (linearRing.Points.IsNotEmpty())
                                {
                                    if (isFirstLinearRing)
                                    {
                                        isFirstLinearRing = false;
                                        wkt.Insert(14, "()");
                                    }
                                    else
                                    {
                                        wkt.Insert(14, "(), ");
                                    }

                                    String komma = String.Empty;

                                    foreach (WebPoint point in linearRing.Points)
                                    {

                                        wkt.Insert(
                                            15,
                                            point.X.WebToStringR().Replace(",", ".") + " "
                                            + (point.Y.WebToStringR().Replace(",", ".")) + komma);

                                        komma = ", ";
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return wkt.ToString();
        }
    }
}