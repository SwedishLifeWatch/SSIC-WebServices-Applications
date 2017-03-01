using System;
using System.Collections.Generic;
using System.Web.Caching;
using ProjNet.Converters.WellKnownText;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

//using DotSpatial;
//using DotSpatial.Projections;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Manager of coordinate conversions.
    /// </summary>
    public class CoordinateConversionManager : ICoordinateConversionManager
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
        /// <returns>Polygon with coordinates according to toCoordinateSystem</returns>
        public virtual WebPolygon GetConvertedBoundingBox(WebBoundingBox boundingBox,
                                                          WebCoordinateSystem fromCoordinateSystem,
                                                          WebCoordinateSystem toCoordinateSystem)
        {
            WebPolygon toPolygon;

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
        public virtual List<WebLinearRing> GetConvertedLinearRings(List<WebLinearRing> linearRings,
                                                                   WebCoordinateSystem fromCoordinateSystem,
                                                                   WebCoordinateSystem toCoordinateSystem)
        {
            Int32 linearRingIndex, pointIndex, toPointIndex;
            List<WebPoint> fromPoints, toPoints;
            List<WebLinearRing> toLinearRings;

            toLinearRings = new List<WebLinearRing>();
            if (linearRings.IsNotEmpty())
            {
                fromPoints = new List<WebPoint>();
                for (linearRingIndex = 0; linearRingIndex < linearRings.Count; linearRingIndex++)
                {
                    for (pointIndex = 0; pointIndex < linearRings[linearRingIndex].Points.Count; pointIndex++)
                    {
                        fromPoints.Add(linearRings[linearRingIndex].Points[pointIndex]);
                    }
                }
                toPoints = GetConvertedPoints(fromPoints, fromCoordinateSystem, toCoordinateSystem);
                toLinearRings = new List<WebLinearRing>();
                toPointIndex = 0;
                for (linearRingIndex = 0; linearRingIndex < linearRings.Count; linearRingIndex++)
                {
                    toLinearRings.Add(new WebLinearRing());
                    toLinearRings[linearRingIndex].Points = new List<WebPoint>();
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
        /// Convert a multi polygon from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="multiPolygon">Multi polygon that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Multi polygon with coordinates according to toCoordinateSystem</returns>
        public virtual WebMultiPolygon GetConvertedMultiPolygon(WebMultiPolygon multiPolygon,
                                                                WebCoordinateSystem fromCoordinateSystem,
                                                                WebCoordinateSystem toCoordinateSystem)
        {
            Int32 polygonIndex, toPolygonIndex;
            WebMultiPolygon toMultiPolygon;
            List<WebPolygon> fromPolygons, toPolygons;

            toMultiPolygon = null;
            if (multiPolygon.IsNotNull())
            {
                fromPolygons = new List<WebPolygon>();
                for (polygonIndex = 0; polygonIndex < multiPolygon.Polygons.Count; polygonIndex++)
                {
                    fromPolygons.Add(multiPolygon.Polygons[polygonIndex]);
                }
                toPolygons = GetConvertedPolygons(fromPolygons, fromCoordinateSystem, toCoordinateSystem);
                toPolygonIndex = 0;
                toMultiPolygon = new WebMultiPolygon();
                toMultiPolygon.Polygons = new List<WebPolygon>();
                for (polygonIndex = 0; polygonIndex < multiPolygon.Polygons.Count; polygonIndex++)
                {
                    toMultiPolygon.Polygons.Add(toPolygons[toPolygonIndex]);
                    toPolygonIndex++;
                }
            }

            return toMultiPolygon;
        }

        /// <summary>
        /// Convert multi polygons from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="multiPolygons">Multi polygons that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Multi polygons with coordinates according to toCoordinateSystem</returns>
        public virtual List<WebMultiPolygon> GetConvertedMultiPolygons(List<WebMultiPolygon> multiPolygons,
                                                                       WebCoordinateSystem fromCoordinateSystem,
                                                                       WebCoordinateSystem toCoordinateSystem)
        {
            Int32 polygonIndex, multiPolygonIndex, toPolygonIndex;
            List<WebMultiPolygon> toMultiPolygons;
            List<WebPolygon> fromPolygons, toPolygons;

            toMultiPolygons = new List<WebMultiPolygon>();
            if (multiPolygons.IsNotEmpty())
            {
                fromPolygons = new List<WebPolygon>();
                for (multiPolygonIndex = 0; multiPolygonIndex < multiPolygons.Count; multiPolygonIndex++)
                {
                    for (polygonIndex = 0; polygonIndex < multiPolygons[multiPolygonIndex].Polygons.Count; polygonIndex++)
                    {
                        fromPolygons.Add(multiPolygons[multiPolygonIndex].Polygons[polygonIndex]);
                    }
                }
                toPolygons = GetConvertedPolygons(fromPolygons, fromCoordinateSystem, toCoordinateSystem);
                toMultiPolygons = new List<WebMultiPolygon>();
                toPolygonIndex = 0;
                for (multiPolygonIndex = 0; multiPolygonIndex < multiPolygons.Count; multiPolygonIndex++)
                {
                    toMultiPolygons.Add(new WebMultiPolygon());
                    toMultiPolygons[multiPolygonIndex].Polygons = new List<WebPolygon>();
                    for (polygonIndex = 0; polygonIndex < multiPolygons[multiPolygonIndex].Polygons.Count; polygonIndex++)
                    {
                        toMultiPolygons[multiPolygonIndex].Polygons.Add(toPolygons[toPolygonIndex]);
                        toPolygonIndex++;
                    }
                }
            }

            return toMultiPolygons;
        }

        /// <summary>
        /// Convert a point from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="point">Point that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>WebPoint with coordinates according to toCoordinateSystem</returns>
        public virtual WebPoint GetConvertedPoint(WebPoint point,
                                                  WebCoordinateSystem fromCoordinateSystem,
                                                  WebCoordinateSystem toCoordinateSystem)
        {
            double[] fromPointValues, toPointValues, middlePointValues;
            ICoordinateTransformation transformator;
            WebPoint toPoint;

            fromPointValues = new double[] { point.X, point.Y };
            middlePointValues = new double[] { point.X, point.Y };
            

            //if (toCoordinateSystem.Id == CoordinateSystemId.ETRS89_LAEA)
            //{
            //    WebCoordinateSystem middle = new WebCoordinateSystem();
            //    middle.Id = CoordinateSystemId.WGS84;
            //    transformator = GetTransformator(fromCoordinateSystem, middle);
            //    middlePointValues = transformator.MathTransform.Transform(fromPointValues);

            //    ProjectionInfo fromProjection = KnownCoordinateSystems.Geographic.World.WGS1984;
            //    //EPSG 4326
            //    ProjectionInfo toProjection = KnownCoordinateSystems.Projected.Europe.ETRS1989LAEA;

            //    //toPointValues = new double[2];
            //    fromZ = new double[] {0};
            //    double[] fromXY = new double[] { middlePointValues[1], middlePointValues[0]};


            //    //ProjectionInfo Lamb = KnownCoordinateSystems.Projected.Europe.ETRS1989LAEA;
            //    //Lamb.LatitudeOfOrigin = 52;
            //    //Lamb.LongitudeOfCenter = 10;
            //    //Lamb.FalseEasting = 4321000;
            //    //Lamb.FalseNorthing = 3210000;

            //    Reproject.ReprojectPoints(fromXY, fromZ, fromProjection, toProjection, 0, 1);



            // //   Reproject.ReprojectPoints(fromXY, fromZ, fromProjection, toProjection, 0, 1);

            //    toPointValues = fromXY;
            //}
            //else
            //{
                transformator = GetTransformator(fromCoordinateSystem, toCoordinateSystem);
                toPointValues = transformator.MathTransform.Transform(fromPointValues);     
         //   }

            toPoint = new WebPoint(toPointValues[0], toPointValues[1]);
            return toPoint;
        }

        /// <summary>
        /// Convert a point from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="point">Point that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>WebPoint with coordinates according to toCoordinateSystem</returns>
        public virtual WebPoint GetConvertedPoint(WebServiceContext context,
                                                  WebPoint point,
                                                  WebCoordinateSystem fromCoordinateSystem,
                                                  WebCoordinateSystem toCoordinateSystem)
        {
            String cacheKey;
            WebPoint toPoint;

            // Get cached information.
            cacheKey = Settings.Default.PointCacheKey +
                       Settings.Default.CacheKeyDelimiter +
                       point.X.WebToString() +
                       Settings.Default.CacheKeyDelimiter +
                       point.Y.WebToString() +
                       Settings.Default.CacheKeyDelimiter +
                       fromCoordinateSystem.GetWkt() +
                       Settings.Default.CacheKeyDelimiter +
                       toCoordinateSystem.GetWkt();
            toPoint = (WebPoint)(context.GetCachedObject(cacheKey));

            // Data not in cache - store it in the cache.
            if (toPoint.IsNull())
            {

                    toPoint = GetConvertedPoint(point,
                                                fromCoordinateSystem,
                                                toCoordinateSystem);
                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        toPoint,
                                        new TimeSpan(0, 1, 0, 0),
                                        CacheItemPriority.Low);
            }

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
        public virtual List<WebPoint> GetConvertedPoints(List<WebPoint> points,
                                                         WebCoordinateSystem fromCoordinateSystem,
                                                         WebCoordinateSystem toCoordinateSystem)
        {
            Int32 pointIndex;
            ICoordinateTransformation transformator;
            List<double[]> fromPointsValues, toPointsValues;
            List<WebPoint> toPoints;

            toPoints = new List<WebPoint>();
            if (points.IsNotEmpty())
            {
                fromPointsValues = new List<double[]>();
                for (pointIndex = 0; pointIndex < points.Count; pointIndex++)
                {
                    fromPointsValues.Add(new double[] { points[pointIndex].X, points[pointIndex].Y });
                }

                    transformator = GetTransformator(fromCoordinateSystem, toCoordinateSystem);
                    toPointsValues = transformator.MathTransform.TransformList(fromPointsValues);
                    toPoints = new List<WebPoint>();
                    for (pointIndex = 0; pointIndex < points.Count; pointIndex++)
                    {
                        toPoints.Add(new WebPoint(toPointsValues[pointIndex][0], toPointsValues[pointIndex][1]));
                    }
            }

            return toPoints;
        }

        /// <summary>
        /// Convert polygon from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="polygon">Polygon that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Polygon with coordinates according to toCoordinateSystem</returns>
        public virtual WebPolygon GetConvertedPolygon(WebPolygon polygon,
                                                      WebCoordinateSystem fromCoordinateSystem,
                                                      WebCoordinateSystem toCoordinateSystem)
        {
            Int32 linearRingIndex, toLinearRingIndex;
            List<WebLinearRing> fromLinearRings, toLinearRings;
            WebPolygon toPolygon;

            toPolygon = null;
            if (polygon.IsNotNull())
            {
                toPolygon = new WebPolygon();
                fromLinearRings = new List<WebLinearRing>();
                for (linearRingIndex = 0; linearRingIndex < polygon.LinearRings.Count; linearRingIndex++)
                {
                    fromLinearRings.Add(polygon.LinearRings[linearRingIndex]);
                }
                toLinearRings = GetConvertedLinearRings(fromLinearRings, fromCoordinateSystem, toCoordinateSystem);
                toLinearRingIndex = 0;
                toPolygon.LinearRings = new List<WebLinearRing>();
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
        public virtual List<WebPolygon> GetConvertedPolygons(List<WebPolygon> polygons,
                                                             WebCoordinateSystem fromCoordinateSystem,
                                                             WebCoordinateSystem toCoordinateSystem)
        {
            Int32 linearRingIndex, polygonIndex, toLinearRingIndex;
            List<WebLinearRing> fromLinearRings, toLinearRings;
            List<WebPolygon> toPolygons;

            toPolygons = new List<WebPolygon>();
            if (polygons.IsNotEmpty())
            {
                fromLinearRings = new List<WebLinearRing>();
                for (polygonIndex = 0; polygonIndex < polygons.Count; polygonIndex++)
                {
                    for (linearRingIndex = 0; linearRingIndex < polygons[polygonIndex].LinearRings.Count; linearRingIndex++)
                    {
                        fromLinearRings.Add(polygons[polygonIndex].LinearRings[linearRingIndex]);
                    }
                }
                toLinearRings = GetConvertedLinearRings(fromLinearRings, fromCoordinateSystem, toCoordinateSystem);
                toPolygons = new List<WebPolygon>();
                toLinearRingIndex = 0;
                for (polygonIndex = 0; polygonIndex < polygons.Count; polygonIndex++)
                {
                    toPolygons.Add(new WebPolygon());
                    toPolygons[polygonIndex].LinearRings = new List<WebLinearRing>();
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
        /// Convert region geography from one coordinate system to
        /// another coordinate system.
        /// </summary>
        /// <param name="regionGeography">Region geography that should be converted.</param>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Region geography with coordinates according to toCoordinateSystem</returns>
        public virtual WebRegionGeography GetConvertedRegionGeography(WebRegionGeography regionGeography,
                                                                      WebCoordinateSystem fromCoordinateSystem,
                                                                      WebCoordinateSystem toCoordinateSystem)
        {
            WebRegionGeography toRegionGeography;

            toRegionGeography = regionGeography;
            toRegionGeography.MultiPolygon = GetConvertedMultiPolygon(regionGeography.MultiPolygon,
                                                                      fromCoordinateSystem,
                                                                      toCoordinateSystem);
            toRegionGeography.BoundingBox = toRegionGeography.MultiPolygon.GetBoundingBox();
            return toRegionGeography;
        }

        /// <summary>
        /// Get coordinate converter transformator.
        /// </summary>
        /// <param name="fromCoordinateSystem">From coordinate system.</param>
        /// <param name="toCoordinateSystem">To coordinate system.</param>
        /// <returns>Coordinate converter transformator.</returns>
        private ICoordinateTransformation GetTransformator(WebCoordinateSystem fromCoordinateSystem,
                                                           WebCoordinateSystem toCoordinateSystem)
        {
            CoordinateTransformationFactory factory;
            ICoordinateSystem fromSystem, toSystem;

            factory = new CoordinateTransformationFactory();
            fromSystem = CoordinateSystemWktReader.Parse(fromCoordinateSystem.GetWkt()) as ICoordinateSystem;
            toSystem = CoordinateSystemWktReader.Parse(toCoordinateSystem.GetWkt()) as ICoordinateSystem;
            return factory.CreateFromCoordinateSystems(fromSystem, toSystem);
        }
    }
}
