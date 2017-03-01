using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebBoundingBox class.
    /// </summary>
    public static class WebBoundingBoxExtension
    {
        /// <summary>
        /// Add bounding box 2 to bounding box 1.
        /// The union of bounding box 1 and bounding box 2
        /// are stored in bounding box 1.
        /// </summary>
        /// <param name="boundingBox1">Bounding box 1.</param>
        /// <param name="boundingBox2">Bounding box 2.</param>
        public static void Add(this WebBoundingBox boundingBox1,
                               WebBoundingBox boundingBox2)
        {
            if (boundingBox1.Max.IsNull())
            {
                boundingBox1.Max = boundingBox2.Max.Clone();
            }
            else
            {
                boundingBox1.Max.X = Math.Max(boundingBox1.Max.X, boundingBox2.Max.X);
                boundingBox1.Max.Y = Math.Max(boundingBox1.Max.Y, boundingBox2.Max.Y);
                if (boundingBox1.Max.IsZSpecified != boundingBox2.Max.IsZSpecified)
                {
                    throw new ArgumentException("Different dimensions in points!");
                }
                boundingBox1.Max.Z = Math.Max(boundingBox1.Max.Z, boundingBox2.Max.Z);
                if (boundingBox1.Max.IsMSpecified != boundingBox2.Max.IsMSpecified)
                {
                    throw new ArgumentException("Different dimensions in points!");
                }
                boundingBox1.Max.M = Math.Max(boundingBox1.Max.M, boundingBox2.Max.M);
            }

            if (boundingBox1.Min.IsNull())
            {
                boundingBox1.Min = boundingBox2.Min.Clone();
            }
            else
            {
                boundingBox1.Min.X = Math.Min(boundingBox1.Min.X, boundingBox2.Min.X);
                boundingBox1.Min.Y = Math.Min(boundingBox1.Min.Y, boundingBox2.Min.Y);
                if (boundingBox1.Min.IsZSpecified != boundingBox2.Min.IsZSpecified)
                {
                    throw new ArgumentException("Different dimensions in points!");
                }
                boundingBox1.Min.Z = Math.Min(boundingBox1.Min.Z, boundingBox2.Min.Z);
                if (boundingBox1.Min.IsMSpecified != boundingBox2.Min.IsMSpecified)
                {
                    throw new ArgumentException("Different dimensions in points!");
                }
                boundingBox1.Min.M = Math.Min(boundingBox1.Min.M, boundingBox2.Min.M);
            }
        }

        /// <summary>
        /// Add buffer to bounding box.
        /// </summary>
        /// <param name="boundingBox">Bounding box.</param>
        /// <param name="buffer">Buffer size in all directions.</param>
        public static void AddBuffer(this WebBoundingBox boundingBox,
                                     Double buffer)
        {
            if (boundingBox.IsNotNull())
            {
                boundingBox.Max.X += buffer;
                boundingBox.Max.Y += buffer;
                boundingBox.Min.X -= buffer;
                boundingBox.Min.Y -= buffer;
            }
        }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="boundingBox">Bounding box.</param>
        public static void CheckData(this WebBoundingBox boundingBox)
        {
            if (boundingBox.IsNotNull())
            {
                // Check points.
                boundingBox.Max.CheckNotNull("Max");
                boundingBox.Max.CheckData();
                boundingBox.Min.CheckNotNull("Min");
                boundingBox.Min.CheckData();

                // Check that points are of the same dimension.
                if (boundingBox.Max.IsMSpecified != 
                    boundingBox.Min.IsMSpecified)
                {
                    throw new ArgumentException("WebBoundingBox: Min and max has different M dimensions");

                }
                if (boundingBox.Max.IsZSpecified !=
                    boundingBox.Min.IsZSpecified)
                {
                    throw new ArgumentException("WebBoundingBox: Min and max has different Z dimensions");

                }

                // Check that all min values are lower than all max values.
                if (boundingBox.Max.X < boundingBox.Min.X)
                {
                    throw new ArgumentException("WebBoundingBox: Max.X is smaller than Min.X");
                }
                if (boundingBox.Max.Y < boundingBox.Min.Y)
                {
                    throw new ArgumentException("WebBoundingBox: Max.Y is smaller than Min.Y");
                }
                if (boundingBox.Max.IsZSpecified &&
                    (boundingBox.Max.Z < boundingBox.Min.Z))
                {
                    throw new ArgumentException("WebBoundingBox: Max.Z is smaller than Min.Z");
                }
                if (boundingBox.Max.IsMSpecified &&
                    (boundingBox.Max.M < boundingBox.Min.M))
                {
                    throw new ArgumentException("WebBoundingBox: Max.M is smaller than Min.M");
                }
            }
        }

        /// <summary>
        /// Convert bounding box to JSON format.
        /// </summary>
        /// <param name="boundingBox">Bounding box that should be converted.</param>
        /// <returns>Bounding box in JSON format.</returns>
        public static String GetJson(this WebBoundingBox boundingBox)
        {
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (boundingBox.IsNotNull())
            {
                stringBuilder.Append("[");
                stringBuilder.Append("[" + boundingBox.Min.X.WebToStringR() +
                                     ", " + boundingBox.Max.Y.WebToStringR() + "],");
                stringBuilder.Append("[" + boundingBox.Max.X.WebToStringR() +
                                     ", " + boundingBox.Min.Y.WebToStringR() + "]");
                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Convert a WebBoundingBox instance into a WebPolygon
        /// instance with same geographic area.
        /// </summary>
        /// <param name="boundingBox">Bounding box.</param>
        /// <returns>A WebPolygon instance.</returns>
        public static WebPolygon GetPolygon(this WebBoundingBox boundingBox)
        {
            WebLinearRing linearRing;
            WebPolygon polygon;

            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            linearRing.Points.Add(new WebPoint(boundingBox.Min.X, boundingBox.Min.Y));
            linearRing.Points.Add(new WebPoint(boundingBox.Max.X, boundingBox.Min.Y));
            linearRing.Points.Add(new WebPoint(boundingBox.Max.X, boundingBox.Max.Y));
            linearRing.Points.Add(new WebPoint(boundingBox.Min.X, boundingBox.Max.Y));
            linearRing.Points.Add(new WebPoint(boundingBox.Min.X, boundingBox.Min.Y));
            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
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
        public static Boolean IsPointInside(this WebBoundingBox boundingBox,
                                            WebPoint point)
        {
            return (boundingBox.IsNotNull() &&
                    point.IsNotNull() &&
                    (boundingBox.Max.X >= point.X) &&
                    (boundingBox.Min.X <= point.X) &&
                    (boundingBox.Max.Y >= point.Y) &&
                    (boundingBox.Min.Y <= point.Y));
        }

        /// <summary>
        /// Convert bounding box to a string.
        /// </summary>
        /// <param name="boundingBox">Bounding box that should be converted.</param>
        /// <returns>Bounding box as a string.</returns>
        public static String WebToString(this WebBoundingBox boundingBox)
        {
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (boundingBox.IsNotNull() && boundingBox.Min.IsNotNull() && boundingBox.Max.IsNotNull())
            {
                stringBuilder.Append("Bounding box = [");
                stringBuilder.Append("[" + boundingBox.Min.X.WebToStringR() +
                                     ", " + boundingBox.Max.Y.WebToStringR() + "],");
                stringBuilder.Append("[" + boundingBox.Max.X.WebToStringR() +
                                     ", " + boundingBox.Min.Y.WebToStringR() + "]");
                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }

    }
}
