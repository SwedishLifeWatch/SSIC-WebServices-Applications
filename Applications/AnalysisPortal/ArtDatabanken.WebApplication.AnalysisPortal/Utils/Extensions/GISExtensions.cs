using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    /// <summary>
    /// Contains extensions to geometry classes .
    /// </summary>
    public static class GISExtensions
    {
        /// <summary>
        /// Converts a list of DataPolygon to Polygons.
        /// </summary>
        /// <param name="dataPolygons">The data polygons.</param>
        /// <param name="dataContext">The data context.</param>
        /// <returns></returns>
        public static List<IPolygon> ToPolygons(this List<DataPolygon> dataPolygons, DataContext dataContext)
        {
            var polygons = new List<IPolygon>();
            foreach (DataPolygon dataPolygon in dataPolygons)
            {
                Polygon polygon = dataPolygon.ToPolygon(dataContext);
                polygons.Add(polygon);
            }
            return polygons;
        }

        /// <summary>
        /// Converts a DataPolygon to Polygon.
        /// </summary>
        /// <param name="dataPolygon">The data polygon.</param>
        /// <param name="dataContext">The data context.</param>
        /// <returns></returns>
        public static Polygon ToPolygon(this DataPolygon dataPolygon, DataContext dataContext)
        {
            var polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();

            foreach (DataLinearRing dataLinearRing in dataPolygon.LinearRings)
            {
                LinearRing linearRing = dataLinearRing.ToLinearRing(dataContext);
                polygon.LinearRings.Add(linearRing);
            }

            return polygon;
        }

        /// <summary>
        /// Converts a DataLinearRing to a linear ring.
        /// </summary>
        /// <param name="dataLinearRing">The data linear ring.</param>
        /// <param name="dataContext">The data context.</param>
        /// <returns></returns>
        public static LinearRing ToLinearRing(this DataLinearRing dataLinearRing, DataContext dataContext)
        {
            var linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();

            foreach (DataPoint dataPoint in dataLinearRing.Points)
            {
                Point point = dataPoint.ToPoint(dataContext);
                linearRing.Points.Add(point);
            }

            return linearRing;
        }

        /// <summary>
        /// Converts a DataPoint to a Point.
        /// </summary>
        /// <param name="dataPoint">The data point.</param>
        /// <param name="dataContext">The data context.</param>
        /// <returns></returns>
        public static Point ToPoint(this DataPoint dataPoint, DataContext dataContext)
        {
            var point = new Point(null, dataPoint.X, dataPoint.Y, dataPoint.Z, dataContext);
            return point;
        }

        /// <summary>
        /// Gets the bounding box of an IPolygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>A bounding box</returns>
        public static BoundingBox GetBoundingBox(this IPolygon polygon)
        {
            if (polygon == null || polygon.LinearRings == null || polygon.LinearRings.Count == 0 || polygon.LinearRings[0].Points.Count < 3)
            {
                return null;
            }

            BoundingBox boundingBox = new BoundingBox();
            boundingBox.Min = new Point(polygon.LinearRings[0].Points[0].X, polygon.LinearRings[0].Points[0].Y);
            boundingBox.Max = new Point(polygon.LinearRings[0].Points[0].X, polygon.LinearRings[0].Points[0].Y);
            for (int i = 1; i < polygon.LinearRings[0].Points.Count; i++)
            {
                IPoint point = polygon.LinearRings[0].Points[i];
                if (point.X < boundingBox.Min.X)
                {
                    boundingBox.Min.X = point.X;
                }

                if (point.Y < boundingBox.Min.Y)
                {
                    boundingBox.Min.Y = point.Y;
                }

                if (point.X > boundingBox.Max.X)
                {
                    boundingBox.Max.X = point.X;
                }

                if (point.Y > boundingBox.Max.Y)
                {
                    boundingBox.Max.Y = point.Y;
                }
            }
            return boundingBox;
        }

        /// <summary>
        /// Gets the bounding box of a list of polygons.
        /// </summary>
        /// <param name="polygons">The polygons.</param>
        /// <returns>The bounding box containing all polygons</returns>
        public static BoundingBox GetBoundingBox(this IEnumerable<DataPolygon> polygons)
        {
            BoundingBox boundingBox = null;

            if (polygons == null)
            {
                return null;
            }

            foreach (DataPolygon polygon in polygons)
            {
                BoundingBox itemBoundingBox = GetBoundingBox(polygon);
                if (itemBoundingBox != null)
                {
                    if (boundingBox == null)
                    {
                        boundingBox = new BoundingBox();
                        boundingBox.Min = new Point(itemBoundingBox.Min.X, itemBoundingBox.Min.Y);
                        boundingBox.Max = new Point(itemBoundingBox.Max.X, itemBoundingBox.Max.Y);
                    }
                    else
                    {
                        if (itemBoundingBox.Min.X < boundingBox.Min.X)
                        {
                            boundingBox.Min.X = itemBoundingBox.Min.X;
                        }

                        if (itemBoundingBox.Min.Y < boundingBox.Min.Y)
                        {
                            boundingBox.Min.Y = itemBoundingBox.Min.Y;
                        }

                        if (itemBoundingBox.Max.X > boundingBox.Max.X)
                        {
                            boundingBox.Max.X = itemBoundingBox.Max.X;
                        }

                        if (itemBoundingBox.Max.Y > boundingBox.Max.Y)
                        {
                            boundingBox.Max.Y = itemBoundingBox.Max.Y;
                        }
                    }
                }
            }

            return boundingBox;
        }

        /// <summary>
        /// Gets the bounding box of a DataPolygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns></returns>
        public static BoundingBox GetBoundingBox(this DataPolygon polygon)
        {
            if (polygon == null || polygon.LinearRings == null || polygon.LinearRings.Count == 0 || polygon.LinearRings[0].Points.Count < 3)
            {
                return null;
            }

            BoundingBox boundingBox = new BoundingBox();
            boundingBox.Min = new Point(polygon.LinearRings[0].Points[0].X, polygon.LinearRings[0].Points[0].Y);
            boundingBox.Max = new Point(polygon.LinearRings[0].Points[0].X, polygon.LinearRings[0].Points[0].Y);
            for (int i = 1; i < polygon.LinearRings[0].Points.Count; i++)
            {
                DataPoint point = polygon.LinearRings[0].Points[i];
                if (point.X < boundingBox.Min.X)
                {
                    boundingBox.Min.X = point.X;
                }

                if (point.Y < boundingBox.Min.Y)
                {
                    boundingBox.Min.Y = point.Y;
                }

                if (point.X > boundingBox.Max.X)
                {
                    boundingBox.Max.X = point.X;
                }

                if (point.Y > boundingBox.Max.Y)
                {
                    boundingBox.Max.Y = point.Y;
                }
            }

            return boundingBox;
        }

        public static GeometryType ToGeometryType(this GeoJSONObjectType type)
        {
            switch (type)
            {
                case GeoJSONObjectType.Polygon:
                    return GeometryType.Polygon;
                case GeoJSONObjectType.LineString:
                    return GeometryType.Line;
                default:
                    return GeometryType.Point;
            }
        }
    }
}
