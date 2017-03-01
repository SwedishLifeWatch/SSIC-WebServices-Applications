using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Polygon = ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Converters
{
    /// <summary>
    /// This class contains methods to convert objects to DataPolygons.
    /// </summary>
    public static class DataPolygonConverter
    {
        /// <summary>
        /// Converts a GeoJSON FeatureCollection to a list with data polygons.
        /// </summary>
        /// <param name="featureCollection">The GeoJSON FeatureCollection.</param>
        /// <returns>Converted polygons.</returns>
        public static List<DataPolygon> ConvertToDataPolygons(FeatureCollection featureCollection)
        {
            var dataPolygons = new List<DataPolygon>();
            if (featureCollection.IsNull())
            {
                return dataPolygons;
            }

            foreach (Feature feature in featureCollection.Features)
            {
                ////if (feature.Type == GeoJSONObjectType.Polygon)
                if (feature.Geometry != null && feature.Geometry.GetType() == typeof(ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon))
                {
                    ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon polygon = (ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon)feature.Geometry;
                    DataPolygon dataPolygon = ConvertToDataPolygon(polygon);
                    dataPolygons.Add(dataPolygon);
                }
                else if (feature.Geometry != null && feature.Geometry.GetType() == typeof(ArtDatabanken.GIS.GeoJSON.Net.Geometry.MultiPolygon))
                {
                    ArtDatabanken.GIS.GeoJSON.Net.Geometry.MultiPolygon multiPolygon = (ArtDatabanken.GIS.GeoJSON.Net.Geometry.MultiPolygon)feature.Geometry;
                    foreach (ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon polygon in multiPolygon.Coordinates)
                    {
                        DataPolygon dataPolygon = ConvertToDataPolygon(polygon);
                        dataPolygons.Add(dataPolygon);    
                    }                    
                }
            }            

            return dataPolygons;
        }

        /// <summary>
        /// Converts a GeoJSON Polygon to a DataPolygon.
        /// </summary>
        /// <param name="polygon">The GeoJSON polygon.</param>
        /// <returns>Converted DataPolygon.</returns>
        public static DataPolygon ConvertToDataPolygon(ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon polygon)
        {
            var dataPolygon = new DataPolygon();
            dataPolygon.LinearRings = new List<DataLinearRing>();
            foreach (ArtDatabanken.GIS.GeoJSON.Net.Geometry.LineString lineString in polygon.Coordinates)
            {
                var dataLinearRing = new DataLinearRing();
                dataLinearRing.Points = new List<DataPoint>();
                foreach (GeographicPosition pos in lineString.Coordinates)
                {
                    var dataPoint = new DataPoint(pos.Longitude, pos.Latitude, pos.Altitude);
                    dataLinearRing.Points.Add(dataPoint);
                }

                dataPolygon.LinearRings.Add(dataLinearRing);
            }

            return dataPolygon;
        }

        /// <summary>
        /// Converts to data polygons.
        /// </summary>
        /// <param name="polygons">The polygons to convert.</param>
        /// <returns>Converted DataPolygons.</returns>
        public static List<DataPolygon> ConvertToDataPolygons(List<IPolygon> polygons)
        {
            List<DataPolygon> dataPolygon = new List<DataPolygon>();
            foreach (IPolygon polygon in polygons)
            {
                dataPolygon.Add(ConvertToDataPolygon(polygon));
            }

            return dataPolygon;
        }

        /// <summary>
        /// Converts to data polygon.
        /// </summary>
        /// <param name="polygon">The polygon to convert.</param>
        /// <returns>Converted DataPolygon.</returns>
        private static DataPolygon ConvertToDataPolygon(IPolygon polygon)
        {
            DataPolygon dataPolygon = new DataPolygon();
            dataPolygon.LinearRings = new List<DataLinearRing>();
            if (polygon.LinearRings == null)
            {
                return dataPolygon;
            }

            foreach (ILinearRing linearRing in polygon.LinearRings)
            {
                if (linearRing.Points == null || linearRing.Points.Count == 0)
                {
                    continue;
                }

                DataLinearRing dataLinearRing = new DataLinearRing();
                dataLinearRing.Points = new List<DataPoint>();
                foreach (IPoint point in linearRing.Points)
                {
                    var dataPoint = new DataPoint(point.X, point.Y, point.Z);
                    dataLinearRing.Points.Add(dataPoint);
                }

                dataPolygon.LinearRings.Add(dataLinearRing);
            }

            return dataPolygon;
        }
    }
}
