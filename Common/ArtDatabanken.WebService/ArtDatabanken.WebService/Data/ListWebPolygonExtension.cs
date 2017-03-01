using System;
using System.Collections.Generic;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list
    /// of type WebPolygon.
    /// </summary>
    public static class ListWebPolygonExtension
    {
        /// <summary>
        /// Get bounding box for provided polygons.
        /// Currently only 2 dimensions are handled.
        /// </summary>
        /// <param name="polygons">This polygons.</param>
        /// <returns>Bounding box for provided polygons.</returns>
        public static WebBoundingBox GetBoundingBox(this List<WebPolygon> polygons)
        {
            WebBoundingBox boundingBox;

            boundingBox = null;
            if (polygons.IsNotEmpty())
            {
                foreach (WebPolygon polygon in polygons)
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
        /// Get envelope for polygons in JSON format.
        /// </summary>
        /// <param name="polygons">Get envelope for these polygons.</param>
        /// <param name="buffer">Add this buffer to envelope. Unit is meters.</param>
        /// <param name="coordinateSystem">Polygons are defined in this coordinate system.</param>
        /// <returns>Envelope for polygons in JSON format.</returns>
        public static String GetEnvelopeJson(this List<WebPolygon> polygons,
                                             WebCoordinateSystem coordinateSystem,
                                             Int32 buffer)
        {
            String envelopeJson;
            WebBoundingBox boundingBox;
            WebCoordinateSystem googleCoordinateSystem;

            envelopeJson = null;
            if (polygons.IsNotEmpty())
            {
                // Convert polygons to Google Mercator.
                googleCoordinateSystem = new WebCoordinateSystem();
                googleCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
                if (googleCoordinateSystem.GetWkt() != coordinateSystem.GetWkt())
                {
                    polygons = WebServiceData.CoordinateConversionManager.GetConvertedPolygons(polygons,
                                                                                               coordinateSystem,
                                                                                               googleCoordinateSystem);
                }

                // Get bounding box with buffer.
                boundingBox = polygons.GetBoundingBox();
                boundingBox.AddBuffer(buffer);
                if (googleCoordinateSystem.GetWkt() != coordinateSystem.GetWkt())
                {
                    boundingBox = WebServiceData.CoordinateConversionManager.GetConvertedBoundingBox(boundingBox,
                                                                                                     googleCoordinateSystem,
                                                                                                     coordinateSystem).GetBoundingBox();
                }

                envelopeJson = boundingBox.GetJson();
            }

            return envelopeJson;
        }

        /// <summary>
        /// Convert polygons to JSON format.
        /// </summary>
        /// <param name="polygons">Polygons that should be converted.</param>
        /// <returns>Polygons in JSON format.</returns>
        public static String GetJson(this List<WebPolygon> polygons)
        {
            Boolean firstPolygon;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (polygons.IsNotEmpty())
            {
                firstPolygon = true;
                stringBuilder.Append("[");
                foreach (WebPolygon polygon in polygons)
                {
                    if (firstPolygon)
                    {
                        firstPolygon = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(polygon.GetJson());
                }

                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get polygons as string.
        /// </summary>
        /// <param name="polygons">Polygons that should be converted.</param>
        /// <returns>Polygons as string.</returns>
        public static String WebToString(this List<WebPolygon> polygons)
        {
            Boolean firstPolygon;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (polygons.IsNotEmpty())
            {
                firstPolygon = true;
                stringBuilder.Append("Polygons = [");
                foreach (WebPolygon polygon in polygons)
                {
                    if (firstPolygon)
                    {
                        firstPolygon = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(polygon.GetJson());
                }

                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }
    }
}
