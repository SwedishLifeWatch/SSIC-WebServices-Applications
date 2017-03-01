using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.GIS.CoordinateConversion
{
    /// <summary>
    /// Contains extension to classes implementing the IPolygon interface.
    /// </summary>
    public static class IPolygonExtension
    {
        /// <summary>
        /// Get bounding box for provided polygon.
        /// Currently only 2 dimensions are handled.
        /// </summary>
        /// <param name="polygon">This polygon.</param>
        /// <returns>Bounding box for provided polygon.</returns>
        public static BoundingBox GetBoundingBox(this Polygon polygon)
        {
            BoundingBox boundingBox;

            boundingBox = null;
            if (polygon.LinearRings.IsNotEmpty() &&
                polygon.LinearRings[0].Points.IsNotEmpty())
            {
                foreach (Point point in polygon.LinearRings[0].Points)
                {
                    if (boundingBox.IsNull())
                    {
                        boundingBox = new BoundingBox();
                        boundingBox.Max = new Point(point.X, point.Y);
                        boundingBox.Min = new Point(point.X, point.Y);
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

    }
}
