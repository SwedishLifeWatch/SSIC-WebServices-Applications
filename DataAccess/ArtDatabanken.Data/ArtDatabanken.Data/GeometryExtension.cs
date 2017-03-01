using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains extension methods for geometry classes.
    /// </summary>
    public static class GeometryExtension
    {
        /// <summary>
        /// Convert a IPolygon instance to a WebPolygon instance.
        /// </summary>
        /// <param name="polygon">An IPolygon instance.</param>
        /// <returns>A WebPolygon instance.</returns>
        public static WebPolygon ToWebPolygon(this IPolygon polygon)
        {
            WebPolygon webPolygon;

            webPolygon = new WebPolygon();
            webPolygon.LinearRings = new List<WebLinearRing>();

            foreach (ILinearRing linearRing in polygon.LinearRings)
            {
                webPolygon.LinearRings.Add(ToWebLinearRing(linearRing));
            }

            return webPolygon;
        }

        /// <summary>
        /// Convert a IPolygon list to a WebPolygon list.
        /// </summary>
        /// <param name="polygons">A IPolygon list instance.</param>
        /// <returns>A WebPolygon list.</returns>
        public static List<WebPolygon> ToPolygons(this List<IPolygon> polygons)
        {
            List<WebPolygon> webPolygons;

            webPolygons = null;
            if (polygons.IsNotEmpty())
            {
                webPolygons = new List<WebPolygon>();
                foreach (IPolygon polygon in polygons)
                {
                    webPolygons.Add(ToWebPolygon(polygon));
                }
            }

            return webPolygons;
        }

        /// <summary>
        /// Convert a ILinearRing instance to a WebLinearRing instance.
        /// </summary>
        /// <param name="linearRing">An ILinearRing instance.</param>
        /// <returns>A WebLinearRing instance.</returns>
        public static WebLinearRing ToWebLinearRing(this ILinearRing linearRing)
        {
            WebLinearRing webLinearRing;

            webLinearRing = null;
            if (linearRing.IsNotNull())
            {
                webLinearRing = new WebLinearRing();
                if (linearRing.Points.IsNotEmpty())
                {
                    webLinearRing.Points = new List<WebPoint>();

                    foreach (IPoint point in linearRing.Points)
                    {
                        webLinearRing.Points.Add(ToWebPoint(point));
                    }
                }
            }

            return webLinearRing;
        }


        /// <summary>
        /// Convert a IPoint instance to a WebPoint instance.
        /// </summary>
        /// <param name="point">An IPoint instance.</param>
        /// <returns>A WebPoint instance.</returns>
        public static WebPoint ToWebPoint(this IPoint point)
        {
            WebPoint webPoint;

            webPoint = null;
            if (point.IsNotNull())
            {
                webPoint = new WebPoint();
                webPoint.IsMSpecified = point.M.HasValue;
                webPoint.IsZSpecified = point.Z.HasValue;
                if (point.M.HasValue)
                {
                    webPoint.M = point.M.Value;
                }

                webPoint.X = point.X;
                webPoint.Y = point.Y;
                if (point.Z.HasValue)
                {
                    webPoint.Z = point.Z.Value;
                }
            }

            return webPoint;
        }
    }
}
