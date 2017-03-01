using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Enumeration of types that are handled by the SqlGeometry class.
    /// </summary>
    public enum SqlGeometryType
    {
        /// <summary>
        /// Geometry collection.
        /// </summary>
        GeometryCollection,
        /// <summary>
        /// Line string.
        /// </summary>
        LineString,
        /// <summary>
        /// Multi line string.
        /// </summary>
        MultiLineString,
        /// <summary>
        /// Multi point.
        /// </summary>
        MultiPoint,
        /// <summary>
        /// Multi polygon.
        /// </summary>
        MultiPolygon,
        /// <summary>
        /// Point.
        /// </summary>
        Point,
        /// <summary>
        /// Polygon.
        /// </summary>
        Polygon
    }

    /// <summary>
    /// Contains extension to the SqlGeometry class.
    /// </summary>
    public static class SqlGeometryExtension
    {
        /// <summary>
        /// Get type of SqlGeometry.
        /// </summary>
        /// <param name="geometry">Geometry.</param>
        /// <returns>Geometry type.</returns>
        public static SqlGeometryType GetGeometryType(this SqlGeometry geometry)
        {
            return (SqlGeometryType)(Enum.Parse(typeof(SqlGeometryType), geometry.STGeometryType().Value));
        }

        /// <summary>
        /// Get linear ring from SqlGeometry.
        /// </summary>
        /// <param name="geometryLinearRing">Linear ring geometry.</param>
        /// <returns>Linear ring.</returns>
        public static WebLinearRing GetLinearRing(this SqlGeometry geometryLinearRing)
        {
            Int32 pointIndex;
            SqlGeometry geometryPoint;
            WebLinearRing linearRing;

            if (geometryLinearRing.GetGeometryType() != SqlGeometryType.LineString)
            {
                throw new ArgumentException("Wrong geometry data type in GetLinearRing. Expected type 'LineString', actual type : " + geometryLinearRing.GetGeometryType().ToString());
            }

            linearRing = new WebLinearRing();
            linearRing.Points = new List<WebPoint>();
            for (pointIndex = 1; pointIndex <= geometryLinearRing.STNumPoints(); pointIndex++)
            {
                geometryPoint = geometryLinearRing.STPointN(pointIndex);
                linearRing.Points.Add(geometryPoint.GetPoint());
            }
            return linearRing;
        }

        /// <summary>
        /// Get multi polygon from SqlGeometry.
        /// </summary>
        /// <param name="geometryMultiPolygon">Multi polygon geometry.</param>
        /// <returns>Multi polygon.</returns>
        public static WebMultiPolygon GetMultiPolygon(this SqlGeometry geometryMultiPolygon)
        {
            Int32 polygonIndex;
            SqlGeometry geometryPolygon;
            WebMultiPolygon multiPolygon;

            multiPolygon = new WebMultiPolygon();
            multiPolygon.Polygons = new List<WebPolygon>();
            switch (geometryMultiPolygon.GetGeometryType())
            {
                case SqlGeometryType.Polygon:
                    geometryPolygon = geometryMultiPolygon;
                    multiPolygon.Polygons.Add(GetPolygon(geometryPolygon));
                    break;
                case SqlGeometryType.MultiPolygon:
                    for (polygonIndex = 1; polygonIndex <= geometryMultiPolygon.STNumGeometries(); polygonIndex++)
                    {
                        geometryPolygon = geometryMultiPolygon.STGeometryN(polygonIndex);
                        if (geometryPolygon.GetGeometryType() == SqlGeometryType.Polygon)
                        {
                            multiPolygon.Polygons.Add(GetPolygon(geometryPolygon));
                        }
                        else
                        {
                            throw new Exception("Wrong geometry data type in GetMultiPolygon. Expected type 'Polygon', actual type : " + geometryPolygon.GetGeometryType().ToString());
                        }
                    }
                    break;
                default:
                    throw new Exception("Wrong geometry data type in GetPolygon. Expected type 'Polygon' or 'MultiPolygon', actual type : " + geometryMultiPolygon.GetGeometryType().ToString());
            }
            return multiPolygon;
        }

        /// <summary>
        /// Get point from SqlGeometry.
        /// </summary>
        /// <param name="geometryPoint">Point geometry.</param>
        /// <returns>Point.</returns>
        public static WebPoint GetPoint(this SqlGeometry geometryPoint)
        {
            WebPoint point;

            if (geometryPoint.GetGeometryType() != SqlGeometryType.Point)
            {
                throw new ArgumentException("Wrong geometry data type in GetPoint. Expected type 'Point', actual type : " + geometryPoint.GetGeometryType().ToString());
            }

            point = new WebPoint();
            point.X = (Double)geometryPoint.STX;
            point.Y = (Double)geometryPoint.STY;
            point.IsMSpecified = !(geometryPoint.M.ToString().Equals("Null"));
            if (point.IsMSpecified)
            {
                point.M = (Double)geometryPoint.M;
            }
            point.IsZSpecified = !(geometryPoint.Z.ToString().Equals("Null"));
            if (point.IsZSpecified)
            {
                point.Z = (Double)geometryPoint.Z;
            }
            return point;
        }

        /// <summary>
        /// Get polygon from SqlGeometry.
        /// </summary>
        /// <param name="geometryPolygon">Polygon geometry.</param>
        /// <returns>Polygon.</returns>
        public static WebPolygon GetPolygon(this SqlGeometry geometryPolygon)
        {
            Int32 linearRingIndex;
            SqlGeometry geometryLinearRing;
            WebPolygon polygon;

            if (geometryPolygon.GetGeometryType() != SqlGeometryType.Polygon)
            {
                throw new ArgumentException("Wrong geometry data type in GetPolygon. Expected type 'Polygon', actual type : " + geometryPolygon.GetGeometryType().ToString());
            }

            polygon = new WebPolygon();
            polygon.LinearRings = new List<WebLinearRing>();
            geometryLinearRing = geometryPolygon.STExteriorRing();
            polygon.LinearRings.Add(geometryLinearRing.GetLinearRing());
            for (linearRingIndex = 1; linearRingIndex <= geometryPolygon.STNumInteriorRing(); linearRingIndex++)
            {
                geometryLinearRing = geometryPolygon.STInteriorRingN(linearRingIndex);
                polygon.LinearRings.Add(geometryLinearRing.GetLinearRing());
            }
            return polygon;
        }
    }
}
