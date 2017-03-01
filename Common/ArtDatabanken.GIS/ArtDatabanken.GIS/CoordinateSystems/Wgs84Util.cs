using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.CoordinateSystems
{
    /// <summary>
    /// This class contains WGS 84 utility functions.
    /// </summary>
    public static class Wgs84Util
    {
        private const double EarthRadiusKm = 6371;        

        /// <summary>
        /// Gets the distance in meters between two WGS84 coordinates.
        /// </summary>
        /// <param name="point1">point1. point1[0]=longitude, point1[1]=latitude.</param>
        /// <param name="point2">point2. point2[0]=longitude, point2[1]=latitude.</param>
        /// <returns></returns>
        public static double GetDistanceInMeters(double[] point1, double[] point2)
        {
            double dLat = ToRad(point2[1] - point1[1]);
            double dLon = ToRad(point2[0] - point1[0]);
            
            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(ToRad(point1[1])) * Math.Cos(ToRad(point2[1])) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = EarthRadiusKm * c * 1000;
            return distance;
        }


        /// <summary>
        /// Translates the coordinate according to the parameters longitudeOffset and latitudeOffset.
        /// </summary>
        /// <param name="point">The point in WGS84. point[0]=longitude, point[1]=latitude. </param>
        /// <param name="longitudeOffset">The longitude offset in meters.</param>
        /// <param name="latitudeOffset">The latitude offset in meters.</param>
        /// <returns>A coordinate in WGS84.</returns>
        public static double[] TranslateCoordinate(double[] point, double longitudeOffset, double latitudeOffset)
        {
            longitudeOffset = longitudeOffset / 1000;
            latitudeOffset = latitudeOffset / 1000;                        
            //Coordinate offsets in radians
            double dLat = latitudeOffset / EarthRadiusKm;
            double dLon = longitudeOffset / (EarthRadiusKm * Math.Cos(Math.PI * point[1] / 180));
            //OffsetPosition, decimal degrees
            double latO = point[1] + dLat * 180 / Math.PI;
            double lonO = point[0] + dLon * 180 / Math.PI;
            return new[] { lonO, latO };
        }

        /// <summary>
        /// Converts a WGS84 coordinate to Google Mercator coordinate.
        /// </summary>
        /// <param name="point">The point. point[0]=longitude, point[1]=latitude.</param>        
        public static double[] WGS84ToGoogleMercator(double[] point)
        {
            return WGS84ToGoogleMercator(point[0], point[1]);
        }

        /// <summary>
        /// Converts a WGS84 coordinate to Google Mercator coordinate.
        /// </summary>
        /// <param name="lon">The longitude.</param>
        /// <param name="lat">The latitude.</param>        
        public static double[] WGS84ToGoogleMercator(double lon, double lat)
        {
            double x = lon * 20037508.34 / 180;
            double y = Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180);
            y = y * 20037508.34 / 180;
            return new[] { x, y };
        }


        /// <summary>
        /// Converts a Google mercator coordinate to WGS84.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public static double[] GoogleMercatorToWGS84(double[] point)
        {
            return GoogleMercatorToWGS84(point[0], point[1]);
        }

        /// <summary>
        /// Converts a Google mercator coordinate to WGS84.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static double[] GoogleMercatorToWGS84(double x, double y)
        {
            double lon = (x / 20037508.34) * 180;
            double lat = (y / 20037508.34) * 180;

            lat = 180 / Math.PI * (2 * Math.Atan(Math.Exp(lat * Math.PI / 180)) - Math.PI / 2);
            return new double[] { lon, lat };
        }


        private static double ToRad(double input)
        {
            return input * (Math.PI / 180);
        }

    }
}
