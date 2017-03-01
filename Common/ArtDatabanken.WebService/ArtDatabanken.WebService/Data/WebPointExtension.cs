using System;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebPoint class.
    /// </summary>
    public static class WebPointExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="point">This point.</param>
        public static void CheckData(this WebPoint point)
        {
            if (point.IsNotNull())
            {
            }
        }

        /// <summary>
        /// Test if two points has the same coordinates.
        /// </summary>
        /// <param name="point1">Point 1.</param>
        /// <param name="point2">Point 2.</param>
        /// <returns>True if two points has the same coordinates.</returns>
        public static Boolean Equal(this WebPoint point1, WebPoint point2)
        {
            if (point1.IsNull() || point2.IsNull())
            {
                // No coordinates to compare.
                return false;
            }

            if ((point1.IsMSpecified != point2.IsMSpecified) ||
                (point1.IsZSpecified != point2.IsZSpecified))
            {
                // Different dimensions.
                return false;
            }

            return (((Math.Abs(point1.X - point2.X) < Settings.Default.CoordinateDifferenceLimit)) &&
                    ((Math.Abs(point1.Y - point2.Y) < Settings.Default.CoordinateDifferenceLimit)) &&
                    ((!point1.IsZSpecified) ||
                     ((Math.Abs(point1.Z - point2.Z) < Settings.Default.CoordinateDifferenceLimit))) &&
                    ((!point1.IsMSpecified) ||
                     ((Math.Abs(point1.M - point2.M) < Settings.Default.CoordinateDifferenceLimit))));
        }

        /// <summary>
        /// Get a SqlGeography instance with same 
        /// information as provided WebPoint.
        /// </summary>
        /// <param name="point">This point.</param>
        /// <returns>
        /// A SqlGeography instance with same 
        /// information as provided WebPoint.
        /// </returns>
        public static SqlGeography GetGeography(this WebPoint point)
        {
            // Check data.
            if (point.IsZSpecified || point.IsMSpecified)
            {
                throw new ArgumentException("Creating SqlGeography point with M value but no Z value is not supported!");
            }

            return SqlGeography.Parse(new SqlString(point.GetWkt()));
        }

        /// <summary>
        /// Get a SqlGeometry instance with same 
        /// information as provided WebPoint.
        /// </summary>
        /// <param name="point">This point.</param>
        /// <returns>
        /// a SqlGeometry instance with same 
        /// information as provided WebPoint.
        /// </returns>
        public static SqlGeometry GetGeometry(this WebPoint point)
        {
            // Check data.
            if (point.IsMSpecified && !(point.IsZSpecified))
            {
                throw new ArgumentException("Creating SqlGeometry point with M value but no Z value is not supported!");
            }

            return SqlGeometry.Parse(new SqlString(point.GetWkt()));
        }

        /// <summary>
        /// Convert point to JSON format.
        /// </summary>
        /// <param name="point">Point that should be converted.</param>
        /// <returns>Point in JSON format.</returns>
        public static String GetJson(this WebPoint point)
        {
            if (point.IsNotNull())
            {
                return "[" + point.X.WebToStringR() +
                       ", " + point.Y.WebToStringR() + "]";
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Get point in Wkt format.
        /// </summary>
        /// <param name="point">This point.</param>
        /// <returns>Point in Wkt format.</returns>
        public static String GetWkt(this WebPoint point)
        {
            StringBuilder wkt;

            wkt = new StringBuilder("POINT");
            wkt.Append("(");
            wkt.Append(point.X.WebToStringR().Replace(",", "."));
            wkt.Append(" ");
            wkt.Append(point.Y.WebToStringR().Replace(",", "."));
            if (point.IsZSpecified)
            {
                wkt.Append(" ");
                wkt.Append(point.Z.WebToStringR().Replace(",", "."));
            }
            if (point.IsMSpecified)
            {
                wkt.Append(" ");
                wkt.Append(point.M.WebToStringR().Replace(",", "."));
            }
            wkt.Append(")");

            return wkt.ToString();
        }
    }
}
