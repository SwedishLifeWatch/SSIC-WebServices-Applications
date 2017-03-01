using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using ProjNet.Converters.WellKnownText;
using ProjNet.CoordinateSystems.Transformations;
using Point = ArtDatabanken.Data.Point;
using Polygon = ArtDatabanken.Data.Polygon;

namespace ArtDatabanken.GIS.CoordinateConversion
{
    /// <summary>
    /// Manager of coordinate conversions.
    /// </summary>
    public class CoordinateConversionManager
    {
        /// <summary>
        /// Convert bounding box from one coordinate system to
        /// another coordinate system.
        /// Converted bounding box is returned as a polygon
        /// since it probably is not a rectangle any more.
        /// </summary>
        /// <param name="boundingBox">Bounding box that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>IPolygon with coordinates according to toCoordinateSystem</returns>
        public virtual Polygon GetConvertedBoundingBox(IBoundingBox boundingBox,
                                                          ICoordinateSystem fromCoordinateSystem,
                                                          ICoordinateSystem toCoordinateSystem)
        {
            Polygon toPolygon;

            toPolygon = null;
            if (boundingBox.IsNotNull())
            {
                toPolygon = boundingBox.GetPolygon();
                toPolygon = GetConvertedPolygon(toPolygon,
                                                fromCoordinateSystem,
                                                toCoordinateSystem);
            }

            return toPolygon;
        }

        /// <summary>
        /// Convert linear rings from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="linearRings">Linear rings that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Linear rings with coordinates according to toCoordinateSystem</returns>
        public virtual List<ILinearRing> GetConvertedLinearRings(List<ILinearRing> linearRings,
                                                                   ICoordinateSystem fromCoordinateSystem,
                                                                   ICoordinateSystem toCoordinateSystem)
        {
            Int32 linearRingIndex, pointIndex, toPointIndex;
            List<IPoint> fromPoints, toPoints;
            List<ILinearRing> toLinearRings;

            toLinearRings = new List<ILinearRing>();
            if (linearRings.IsNotEmpty())
            {
                fromPoints = new List<IPoint>();
                for (linearRingIndex = 0; linearRingIndex < linearRings.Count; linearRingIndex++)
                {
                    for (pointIndex = 0; pointIndex < linearRings[linearRingIndex].Points.Count; pointIndex++)
                    {
                        fromPoints.Add(linearRings[linearRingIndex].Points[pointIndex]);
                    }
                }
                toPoints = GetConvertedPoints(fromPoints, fromCoordinateSystem, toCoordinateSystem);
                toLinearRings = new List<ILinearRing>();
                toPointIndex = 0;
                for (linearRingIndex = 0; linearRingIndex < linearRings.Count; linearRingIndex++)
                {
                    toLinearRings.Add(new LinearRing());
                    toLinearRings[linearRingIndex].Points = new List<IPoint>();
                    for (pointIndex = 0; pointIndex < linearRings[linearRingIndex].Points.Count; pointIndex++)
                    {
                        toLinearRings[linearRingIndex].Points.Add(toPoints[toPointIndex]);
                        toPointIndex++;
                    }
                }
            }

            return toLinearRings;
        }




        /// <summary>
        /// Convert a point from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="point">IPoint that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>IPoint with coordinates according to toCoordinateSystem</returns>
        public virtual Point GetConvertedPoint(IPoint point,
                                                  ICoordinateSystem fromCoordinateSystem,
                                                  ICoordinateSystem toCoordinateSystem)
        {
            double[] fromPointValues, toPointValues;
            ICoordinateTransformation transformator;
            Point toPoint;

            fromPointValues = new double[] { point.X, point.Y };

            transformator = GetTransformator(fromCoordinateSystem, toCoordinateSystem);
            toPointValues = transformator.MathTransform.Transform(fromPointValues);

            toPoint = new Point(toPointValues[0], toPointValues[1]);
            return toPoint;
        }


        /// <summary>
        /// Convert points from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="points">Points that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Points with coordinates according to toCoordinateSystem</returns>
        public virtual List<IPoint> GetConvertedPoints(List<IPoint> points,
                                                         ICoordinateSystem fromCoordinateSystem,
                                                         ICoordinateSystem toCoordinateSystem)
        {
            Int32 pointIndex;
            ICoordinateTransformation transformator;
            List<double[]> fromPointsValues, toPointsValues;
            List<IPoint> toPoints;

            toPoints = new List<IPoint>();
            if (points.IsNotEmpty())
            {
                fromPointsValues = new List<double[]>();
                for (pointIndex = 0; pointIndex < points.Count; pointIndex++)
                {
                    fromPointsValues.Add(new double[] { points[pointIndex].X, points[pointIndex].Y });
                }

                transformator = GetTransformator(fromCoordinateSystem, toCoordinateSystem);
                toPointsValues = transformator.MathTransform.TransformList(fromPointsValues);
                toPoints = new List<IPoint>();
                for (pointIndex = 0; pointIndex < points.Count; pointIndex++)
                {
                    toPoints.Add(new Point(toPointsValues[pointIndex][0], toPointsValues[pointIndex][1]));
                }
            }

            return toPoints;
        }

        /// <summary>
        /// Convert polygon from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="polygon">IPolygon that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>IPolygon with coordinates according to toCoordinateSystem</returns>
        public virtual Polygon GetConvertedPolygon(IPolygon polygon,
                                                      ICoordinateSystem fromCoordinateSystem,
                                                      ICoordinateSystem toCoordinateSystem)
        {
            Int32 linearRingIndex, toLinearRingIndex;
            List<ILinearRing> fromLinearRings, toLinearRings;
            Polygon toPolygon;

            toPolygon = null;
            if (polygon.IsNotNull())
            {
                toPolygon = new Polygon();
                fromLinearRings = new List<ILinearRing>();
                for (linearRingIndex = 0; linearRingIndex < polygon.LinearRings.Count; linearRingIndex++)
                {
                    fromLinearRings.Add(polygon.LinearRings[linearRingIndex]);
                }
                toLinearRings = GetConvertedLinearRings(fromLinearRings, fromCoordinateSystem, toCoordinateSystem);
                toLinearRingIndex = 0;
                toPolygon.LinearRings = new List<ILinearRing>();
                for (linearRingIndex = 0; linearRingIndex < polygon.LinearRings.Count; linearRingIndex++)
                {
                    toPolygon.LinearRings.Add(toLinearRings[toLinearRingIndex]);
                    toLinearRingIndex++;
                }
            }

            return toPolygon;
        }

        /// <summary>
        /// Convert polygons from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="polygons">Polygons that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Polygons with coordinates according to toCoordinateSystem</returns>
        public virtual List<IPolygon> GetConvertedPolygons(List<IPolygon> polygons,
                                                             ICoordinateSystem fromCoordinateSystem,
                                                             ICoordinateSystem toCoordinateSystem)
        {
            Int32 linearRingIndex, polygonIndex, toLinearRingIndex;
            List<ILinearRing> fromLinearRings, toLinearRings;
            List<IPolygon> toPolygons;

            toPolygons = new List<IPolygon>();
            if (polygons.IsNotEmpty())
            {
                fromLinearRings = new List<ILinearRing>();
                for (polygonIndex = 0; polygonIndex < polygons.Count; polygonIndex++)
                {
                    for (linearRingIndex = 0; linearRingIndex < polygons[polygonIndex].LinearRings.Count; linearRingIndex++)
                    {
                        fromLinearRings.Add(polygons[polygonIndex].LinearRings[linearRingIndex]);
                    }
                }
                toLinearRings = GetConvertedLinearRings(fromLinearRings, fromCoordinateSystem, toCoordinateSystem);
                toPolygons = new List<IPolygon>();
                toLinearRingIndex = 0;
                for (polygonIndex = 0; polygonIndex < polygons.Count; polygonIndex++)
                {
                    toPolygons.Add(new Polygon());
                    toPolygons[polygonIndex].LinearRings = new List<ILinearRing>();
                    for (linearRingIndex = 0; linearRingIndex < polygons[polygonIndex].LinearRings.Count; linearRingIndex++)
                    {
                        toPolygons[polygonIndex].LinearRings.Add(toLinearRings[toLinearRingIndex]);
                        toLinearRingIndex++;
                    }
                }
            }

            return toPolygons;
        }
        
        /// <summary>
        /// Get coordinate converter transformator.
        /// </summary>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Coordinate converter transformator.</returns>
        private ICoordinateTransformation GetTransformator(ICoordinateSystem fromCoordinateSystem,
                                                           ICoordinateSystem toCoordinateSystem)
        {
            CoordinateTransformationFactory factory;
            ProjNet.CoordinateSystems.ICoordinateSystem fromSystem, toSystem;

            factory = new CoordinateTransformationFactory();
            fromSystem = CoordinateSystemWktReader.Parse(fromCoordinateSystem.GetWkt()) as ProjNet.CoordinateSystems.ICoordinateSystem;
            toSystem = CoordinateSystemWktReader.Parse(toCoordinateSystem.GetWkt()) as ProjNet.CoordinateSystems.ICoordinateSystem;
            return factory.CreateFromCoordinateSystems(fromSystem, toSystem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureCollection"></param>
        /// <param name="fromCoordinateSystem"></param>
        /// <param name="toCoordinateSystem"></param>
        /// <returns></returns>
        public FeatureCollection Convert(FeatureCollection featureCollection, ICoordinateSystem fromCoordinateSystem, ICoordinateSystem toCoordinateSystem)
        {
            if (featureCollection == null || featureCollection.Features == null)
            {
                return null;
            }

            //Create new feature list
            var newFeatures = new List<Feature>();
            //Create coordinate transformer
            var transformator = GetTransformator(fromCoordinateSystem, toCoordinateSystem);
            
            foreach (var currentFeature in featureCollection.Features)
            {
                //Declare new feature
                Feature newFeature = null;

                // GeoJSON.Net.Geometry.
                switch (currentFeature.Geometry.Type)
                {
                    case GeoJSONObjectType.MultiPolygon:
                        //Create a new polygon
                        var newMultiPolygon = ConvertMultiPolygon(transformator, (MultiPolygon)currentFeature.Geometry);

                        //Create new feature
                        newFeature = new Feature(newMultiPolygon, currentFeature.Properties);
                        break;
                    case GeoJSONObjectType.Polygon:
                        //Create a new polygon
                        var newPolygon = ConvertPolygon(transformator, (GeoJSON.Net.Geometry.Polygon) currentFeature.Geometry);

                        //Create new feature
                        newFeature = new Feature(newPolygon, currentFeature.Properties);
                        break;
                    case GeoJSONObjectType.MultiLineString:
                        //Create a new multi line string
                        var newMultiLineString = ConvertMultiLineString(transformator, (MultiLineString)currentFeature.Geometry);

                        //Create new feature
                        newFeature = new Feature(newMultiLineString, currentFeature.Properties);
                        break;
                    case GeoJSONObjectType.LineString:
                        //Create a new line string
                        var newLineString = ConvertLineString(transformator, (LineString)currentFeature.Geometry);
                        
                        //Create new feature
                        newFeature = new Feature(newLineString, currentFeature.Properties);
                        break;
                    case GeoJSONObjectType.Point:
                        //Create a new point
                        var newPoint = ConvertPoint(transformator, (GeoJSON.Net.Geometry.Point)currentFeature.Geometry);

                        //Create new feature 
                        newFeature = new Feature(newPoint, currentFeature.Properties);
                        break;
                    default:
                        //Convert not implemented for current geometry. return input geometry
                        newFeature = currentFeature;
                        break;
                }

                if (currentFeature.BoundingBoxes != null)
                {
                    newFeature.BoundingBoxes = transformator.MathTransform.Transform(currentFeature.BoundingBoxes);
                }
                
                //Add feature to list
                newFeatures.Add(newFeature);
            }

            var newCoordinateSystem = new CoordinateSystem(toCoordinateSystem.Id);
            
            return new FeatureCollection(newFeatures)
            {
                CRS = new NamedCRS(newCoordinateSystem.Id.EpsgCode()),
                BoundingBoxes = featureCollection.BoundingBoxes == null ? null : transformator.MathTransform.Transform(featureCollection.BoundingBoxes)
            };
        }

        public GeographicPosition ConvertPosition(GeographicPosition position, ICoordinateSystem fromCoordinateSystem,
            ICoordinateSystem toCoordinateSystem)
        {
            if (position == null)
            {
                return null;
            }

            var transformator = GetTransformator(fromCoordinateSystem, toCoordinateSystem);

            return ConvertGeographicPosition(transformator, position);
        }

        private MultiPolygon ConvertMultiPolygon(ICoordinateTransformation transformator, MultiPolygon multiPolygon)
        {
            //Create a new list of polygons
            var newPolygons = (from p in multiPolygon.Coordinates select ConvertPolygon(transformator, p)).ToList();

            //Create a new multi polygon
            return new MultiPolygon(newPolygons);
        }

        private GeoJSON.Net.Geometry.Polygon ConvertPolygon(ICoordinateTransformation transformator, GeoJSON.Net.Geometry.Polygon polygon)
        {
            //Create a new list of line strings
            var newLineStrings = (from c in polygon.Coordinates select ConvertLineString(transformator, c)).ToList();

            //Create a new polygon
            return new GeoJSON.Net.Geometry.Polygon(newLineStrings);
        }

        private MultiLineString ConvertMultiLineString(ICoordinateTransformation transformator, MultiLineString multiLineString)
        {
            return new MultiLineString((from ls in multiLineString.Coordinates select ConvertLineString(transformator, ls)).ToList());
        }

        private LineString ConvertLineString(ICoordinateTransformation transformator, LineString lineString)
        {
            return new LineString((from c in lineString.Coordinates select ConvertGeographicPosition(transformator, c)).ToList());
        }

        private GeoJSON.Net.Geometry.Point ConvertPoint(ICoordinateTransformation transformator, GeoJSON.Net.Geometry.Point point)
        {
            //Create a new coordinate
            return new GeoJSON.Net.Geometry.Point(ConvertGeographicPosition(transformator, point.Coordinates));
        }

        private GeographicPosition ConvertGeographicPosition(ICoordinateTransformation transformator, GeographicPosition geographicPosition)
        {
            //Convert point values
            var fromPointValues = new[] { geographicPosition.X, geographicPosition.Y };
            var toPointValues = transformator.MathTransform.Transform(fromPointValues);

            //Create a new coordinate
            return new GeographicPosition(toPointValues[0], toPointValues[1]);
        }
    }
}
