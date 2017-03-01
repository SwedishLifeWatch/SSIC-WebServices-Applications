using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Polygon = ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Converters
{
    /// <summary>
    /// This class contains methods to convert objects to GeoJSON objects
    /// </summary>
    public static class GeoJSONConverter
    {
        /// <summary>
        /// Converts a list of DataPolygons to a GeoJSON FeatureCollection.
        /// </summary>
        /// <param name="dataPolygons">The DataPolygons.</param>
        /// <returns></returns>
        public static FeatureCollection ConvertToGeoJSONFeatureCollection(List<DataPolygon> dataPolygons)
        {
            var featureCollection = new FeatureCollection(new List<Feature>());
            foreach (DataPolygon dataPolygon in dataPolygons)
            {
                Polygon polygon = ConvertToGeoJSONPolygon(dataPolygon);
                var feature = new Feature(polygon);
                featureCollection.Features.Add(feature);
            }

            return featureCollection;
        }

        ///// <summary>
        ///// Converts a list of DataPolygons to a GeoJSON FeatureCollection.
        ///// </summary>
        ///// <param name="dataPolygons">The DataPolygons.</param>
        ///// <returns></returns>
        //public static FeatureCollection ConvertToGeoJSONFeatureCollection(List<DataPolygon> dataPolygons, IUserContext userContext, CoordinateSystem fromCoordinateSystem, CoordinateSystem toCoordinateSystem)
        //{
        //    var featureCollection = new FeatureCollection(new List<Feature>());
        //    DataContext dataContext = new DataContext(userContext);
        //    foreach (DataPolygon dataPolygon in dataPolygons)
        //    {
        //        var pol = dataPolygon.ToPolygon(dataContext);
        //        pol = GisTools.CoordinateConversionManager.GetConvertedPolygon(pol, fromCoordinateSystem, toCoordinateSystem);

        //        Polygon polygon = ConvertToGeoJSONPolygon(dataPolygon);
                
        //        var feature = new Feature(polygon);
        //        featureCollection.Features.Add(feature);
        //    }

        //    return featureCollection;
        //}

        /// <summary>
        /// Converts a DataPolygon to a GeoJSON polygon.
        /// </summary>
        /// <param name="dataPolygon">The DataPolygon to convert.</param>
        /// <returns></returns>
        public static Polygon ConvertToGeoJSONPolygon(DataPolygon dataPolygon)
        {
            var polygon = new Polygon(new List<LineString>());
            if (dataPolygon.IsNull() || dataPolygon.LinearRings.IsEmpty())
            {
                return polygon;
            }

            foreach (DataLinearRing dataLinearRing in dataPolygon.LinearRings)
            {
                var coordinates = new List<GeographicPosition>();
                foreach (DataPoint dataPoint in dataLinearRing.Points)
                {
                    var position = new GeographicPosition(dataPoint.X, dataPoint.Y, dataPoint.Z);
                    coordinates.Add(position);
                }
                var lineString = new LineString(coordinates);
                polygon.Coordinates.Add(lineString);
            }
            return polygon;
        }

        /// <summary>
        /// Converts a bounding box to a GeoJSON FeatureCollection.
        /// </summary>
        /// <param name="boundingBox">The bounding box.</param>
        /// <returns>A feature collection containing one feature.</returns>
        public static FeatureCollection ConvertToGeoJSONFeatureCollection(BoundingBox boundingBox)
        {
            FeatureCollection featureCollection = new FeatureCollection(new List<Feature>());
            if (boundingBox == null)
            {
                return featureCollection;
            }

            Polygon polygon = new Polygon(new List<LineString>());
            List<GeographicPosition> coordinates = new List<GeographicPosition>();
            coordinates.Add(new GeographicPosition(boundingBox.Min.X, boundingBox.Min.Y));
            coordinates.Add(new GeographicPosition(boundingBox.Min.X, boundingBox.Max.Y));
            coordinates.Add(new GeographicPosition(boundingBox.Max.X, boundingBox.Max.Y));
            coordinates.Add(new GeographicPosition(boundingBox.Max.X, boundingBox.Min.Y));
            coordinates.Add(new GeographicPosition(boundingBox.Min.X, boundingBox.Min.Y));
            var lineString = new LineString(coordinates);
            polygon.Coordinates.Add(lineString);
            Feature feature = new Feature(polygon);
            featureCollection.Features.Add(feature);

            return featureCollection;
        }
    }
}
