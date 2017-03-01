using System;
using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.GIS.CoordinateConversion
{
    /// <summary>
    /// Contains extension to classes implementing the IBoundingBox interface.
    /// </summary>
    public static class IBoundingBoxExtension
    {
        /// <summary>
        /// Convert a BoundingBox instance into a Polygon
        /// instance with same geographic area.
        /// </summary>
        /// <param name="boundingBox">Bounding box.</param>
        /// <returns>A WebPolygon instance.</returns>
        public static Polygon GetPolygon(this IBoundingBox boundingBox)
        {
            LinearRing linearRing;
            Polygon polygon;

            linearRing = new LinearRing();
            linearRing.Points = new List<IPoint>();
            linearRing.Points.Add(new Point(boundingBox.Min.X, boundingBox.Min.Y));
            linearRing.Points.Add(new Point(boundingBox.Max.X, boundingBox.Min.Y));
            linearRing.Points.Add(new Point(boundingBox.Max.X, boundingBox.Max.Y));
            linearRing.Points.Add(new Point(boundingBox.Min.X, boundingBox.Max.Y));
            linearRing.Points.Add(new Point(boundingBox.Min.X, boundingBox.Min.Y));
            polygon = new Polygon();
            polygon.LinearRings = new List<ILinearRing>();
            polygon.LinearRings.Add(linearRing);
            return polygon;
        }

        /// <summary>
        /// Test if point is located inside bounding box.
        /// Currently only two dimensions are handled.
        /// </summary>
        /// <param name="boundingBox">Bounding box.</param>
        /// <param name='point'>Point.</param>
        /// <returns>True if point is located inside bounding box.</returns>
        public static Boolean IsPointInside(this IBoundingBox boundingBox,
                                            IPoint point)
        {
            return (boundingBox.IsNotNull() &&
                    point.IsNotNull() &&
                    (boundingBox.Max.X >= point.X) &&
                    (boundingBox.Min.X <= point.X) &&
                    (boundingBox.Max.Y >= point.Y) &&
                    (boundingBox.Min.Y <= point.Y));
        }
    }
}