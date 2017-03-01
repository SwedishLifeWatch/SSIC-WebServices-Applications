using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using GeoAPI.Geometries;
using ArtDatabanken.WebService.Data;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.Converters.WellKnownText;
using ICoordinateSystem = ProjNet.CoordinateSystems.ICoordinateSystem;
using ILinearRing = GeoAPI.Geometries.ILinearRing;
using IPoint = GeoAPI.Geometries.IPoint;
using IPolygon = GeoAPI.Geometries.IPolygon;
using LinearRing = NetTopologySuite.Geometries.LinearRing;
using MultiPolygon = NetTopologySuite.Geometries.MultiPolygon;
using Point = NetTopologySuite.Geometries.Point;
using Polygon = NetTopologySuite.Geometries.Polygon;

namespace ArtDatabanken.GIS.Extensions
{
    public static class IGeometryExtensions
    {
        /// <summary>
        /// Cast Polygon to WebPolygon 
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static WebPolygon ToWebPolygon(this IPolygon polygon)
        {
            return new WebPolygon
            {
                LinearRings = polygon.ToWebLinearRings()
            };
        }

        /// <summary>
        /// Cast polygon to list of Web Linear rings
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static List<WebLinearRing> ToWebLinearRings(this IPolygon polygon)
        {
            var linearRings = new List<WebLinearRing>();

            if (polygon.ExteriorRing == null)
            {
                return null; 
            }

            linearRings.Add(polygon.ExteriorRing.Coordinates.ToWebLinearRing());

            if (polygon.Holes != null)
            {
                linearRings.AddRange(from h in polygon.Holes select h.Coordinates.ToWebLinearRing());
            }

            return linearRings;
        }

        /// <summary>
        /// Cast list of linear rings to list of web linear rings
        /// </summary>
        /// <param name="linearRings"></param>
        /// <returns></returns>
        public static IList<WebLinearRing> ToWebLinearRings(this IEnumerable<ILinearRing> linearRings)
        {
            return (from r in linearRings select r.ToWebLinearRing()).ToList();
        }

        /// <summary>
        /// Cast linearring to web linearring
        /// </summary>
        /// <param name="linearRing"></param>
        /// <returns></returns>
        public static WebLinearRing ToWebLinearRing(this ILinearRing linearRing)
        {
            return new WebLinearRing
            {
                Points = (from c in linearRing.Coordinates select c.ToWebPoint()).ToList()
            };
        }

        /// <summary>
        /// Cast linearring to web linearring
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static WebLinearRing ToWebLinearRing(this Coordinate[] coordinates)
        {
            return new WebLinearRing
            {
                Points = (from c in coordinates select c.ToWebPoint()).ToList()
            };
        }

        /// <summary>
        /// Cast Coordinate to webpoint
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public static WebPoint ToWebPoint(this Coordinate coordinate)
        {
            return new WebPoint(coordinate.X, coordinate.Y);
        }

        /// <summary>
        /// Cast point to webpoint
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static WebPoint ToWebPoint(this IPoint point)
        {
            return new WebPoint(point.X, point.Y);
        }

        /// <summary>
        /// Cast Web Polygon to Polygon
        /// </summary>
        /// <param name="webPolygon"></param>
        /// <returns></returns>
        public static IPolygon ToPolygon(this WebPolygon webPolygon)
        {
            //First ring is shell
            var shell = webPolygon.LinearRings[0].ToLinearRing();
            ILinearRing[] holes = null;
            var ringCount = webPolygon.LinearRings.Count;

            //All other rings are holes
            if (ringCount > 1)
            {
                holes = new ILinearRing[ringCount- 1];

                for (var i = 1; i < ringCount; i++)
                {
                    holes[i - 1] = webPolygon.LinearRings[i].ToLinearRing();
                }
            }

            return new Polygon(shell, holes);
        }

        /// <summary>
        /// Cast list of web linear rings to list of linear rings
        /// </summary>
        /// <param name="webLinearRings"></param>
        /// <returns></returns>
        public static IEnumerable<ILinearRing> ToLinearRings(this IEnumerable<WebLinearRing> webLinearRings)
        {
            return (from r in webLinearRings select r.ToLinearRing()).ToList();
        }

        /// <summary>
        /// Cast web linearring to linearring
        /// </summary>
        /// <param name="webBoundingBox"></param>
        /// <returns></returns>
        public static IPolygon ToPolygon(this WebBoundingBox webBoundingBox)
        {
            return new Polygon(new LinearRing(new[]
                {
                    new Coordinate(webBoundingBox.Min.X, webBoundingBox.Min.Y), 
                    new Coordinate(webBoundingBox.Min.X, webBoundingBox.Max.Y), 
                    new Coordinate(webBoundingBox.Max.X, webBoundingBox.Max.Y), 
                    new Coordinate(webBoundingBox.Max.X, webBoundingBox.Min.Y),
                    new Coordinate(webBoundingBox.Min.X, webBoundingBox.Min.Y)
                }));
        }

        /// <summary>
        /// Cast web linearring to linearring
        /// </summary>
        /// <param name="webLinearRing"></param>
        /// <returns></returns>
        public static ILinearRing ToLinearRing(this WebLinearRing webLinearRing)
        {
            return new LinearRing((from p in webLinearRing.Points select p.ToCoordinate()).ToArray());
        }

        /// <summary>
        /// Cast webpoint to Coordinate
        /// </summary>
        /// <param name="webPoint"></param>
        /// <returns></returns>
        public static Coordinate ToCoordinate(this WebPoint webPoint)
        {
            return new Coordinate(webPoint.X, webPoint.Y);
        }

        /// <summary>
        /// Cast webpoint to point
        /// </summary>
        /// <param name="webPoint"></param>
        /// <returns></returns>
        public static IPoint ToPoint(this WebPoint webPoint)
        {
            return new Point(webPoint.X, webPoint.Y);
        }

        /// <summary>
        /// Remove all holes in a MultiPolygon or polygon
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static IGeometry RemoveHoles(this IGeometry geometry)
        {
            if (geometry.GeometryType == "MultiPolygon")
            {
                return ((MultiPolygon)geometry).RemoveHoles();
            }

            return ((IPolygon)geometry).RemoveHoles();
        }

        /// <summary>
        /// Remove all holes in a multipolygon
        /// </summary>
        /// <param name="multiPolygon"></param>
        /// <returns></returns>
        public static IGeometry RemoveHoles(this MultiPolygon multiPolygon)
        {
            IGeometry newGeometry = null;
            for (var i = 0; i < multiPolygon.NumGeometries; i++)
            {
                var polygon = new Polygon((ILinearRing)((IPolygon)multiPolygon.GetGeometryN(i)).ExteriorRing);
                newGeometry = i == 0 ?
                    polygon
                    :
                    newGeometry.Union(polygon);
            }

            return newGeometry;
        }

        /// <summary>
        /// Remove all holes in a polygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static IGeometry RemoveHoles(this IPolygon polygon)
        {
            return new Polygon((ILinearRing)polygon.ExteriorRing);
        }

        public static IGeometry ReProject(this IGeometry geometry, CoordinateSystemId fromCoordinateSystem, CoordinateSystemId toCoordinateSystem)
        {
            if (geometry == null || fromCoordinateSystem == toCoordinateSystem)
            {
                return geometry;
            }

            var factory = new CoordinateTransformationFactory();
            var fromSystem = CoordinateSystemWktReader.Parse(fromCoordinateSystem.GetWkt()) as ICoordinateSystem;
            var toSystem = CoordinateSystemWktReader.Parse(toCoordinateSystem.GetWkt()) as ICoordinateSystem;

            var transformator = factory.CreateFromCoordinateSystems(fromSystem, toSystem);

            switch (geometry.GeometryType)
            {
                case "MultiPolygon":
                    return ((IMultiPolygon)geometry).ReProject(transformator);
                case "Polygon":
                    return ((IPolygon)geometry).ReProject(transformator);
                case "LinearRing":
                    return ((ILinearRing)geometry).ReProject(transformator);
                case "Point":
                    return ((IPoint)geometry).ReProject(transformator);
            }

            return null;
        }

        /// <summary>
        /// Calculate radius of a triangle
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public static double Radius(this IGeometry triangle)
        {
            if (triangle.GeometryType != "Polygon" || triangle.NumPoints != 4)
            {
                throw new ArgumentException("Input must be a Polygon containing three points");
            }

            var p1 = triangle.Coordinates[0];
            var p2 = triangle.Coordinates[1];
            var p3 = triangle.Coordinates[2];

            // Calculate the length of each side of the triangle
            var a = p2.Distance(p3); // side a is opposite point 1
            var b = p1.Distance(p3); // side b is opposite point 2 
            var c = p1.Distance(p2); // side c is opposite point 3

            return  a * b * c / (4 * triangle.Area);
        }

        /// <summary>
        /// Calculate distance between two points
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Distance(this IPoint p1, IPoint p2)
        {
            return Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }

        private static IMultiPolygon ReProject(this IMultiPolygon multiPolygon, ICoordinateTransformation transformator)
        {
            var polygonCount = multiPolygon.NumGeometries;
            var polygons = new IPolygon[polygonCount];
            for (var i = 0; i < polygonCount; i++)
            {
                polygons[i] = ((IPolygon)multiPolygon.GetGeometryN(i)).ReProject(transformator);
            }

            return new MultiPolygon(polygons);
        }

        private static IPolygon ReProject(this IPolygon polygon, ICoordinateTransformation transformator)
        {
            var shell = ((ILinearRing)polygon.ExteriorRing).ReProject(transformator);

            var ringCount = polygon.NumInteriorRings;
            var holes = new ILinearRing[ringCount];
            for (var i = 0; i < ringCount; i++)
            {
                holes[i] = ((ILinearRing)polygon.GetInteriorRingN(i)).ReProject(transformator);
            }

            return new Polygon(shell, holes);
        }

        private static ILinearRing ReProject(this ILinearRing linearRing, ICoordinateTransformation transformator)
        {
            var pointCount = linearRing.NumPoints;
            var coordinates = new Coordinate[pointCount];
            for (var i = 0; i < pointCount; i++)
            {
                coordinates[i] = linearRing.GetCoordinateN(i).ReProject(transformator);
            }

            return new LinearRing(coordinates);
        }

        private static IPoint ReProject(this IPoint point, ICoordinateTransformation transformator)
        {
            return new Point(point.Coordinate.ReProject(transformator));
        }

        private static Coordinate ReProject(this Coordinate coordinate, ICoordinateTransformation transformator)
        {
            //Convert point values
            var fromPointValues = new[] { coordinate.X, coordinate.Y };
            var toPointValues = transformator.MathTransform.Transform(fromPointValues);

            return new Coordinate(toPointValues[0], toPointValues[1]);
        }
    }
}
