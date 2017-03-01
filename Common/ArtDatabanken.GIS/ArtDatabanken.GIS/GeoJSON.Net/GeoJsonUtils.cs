using System;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;

namespace ArtDatabanken.GIS.GeoJSON.Net
{
    /// <summary>
    /// Contains GeoJson utils.
    /// </summary>
    public class GeoJsonUtils
    {
        /// <summary>
        /// Tries to finds the coordinate system crs property used.
        /// </summary>
        /// <param name="featureCollection">The feature collection.</param>
        /// <returns>The coordinate system used. If no crs was found, the coordinate system Id property is set to None.</returns>
        public CoordinateSystem  FindCoordinateSystem(FeatureCollection featureCollection)
        {
            CoordinateSystemId coordinateSystemId = FindCoordinateSystemId(featureCollection);
            return new CoordinateSystem(coordinateSystemId);
        }

        /// <summary>
        /// Tries to finds the coordinate system crs property used.
        /// </summary>
        /// <param name="featureCollection">The feature collection.</param>
        /// <returns>The coordinate system id used. If no crs was found, None is returned.</returns>
        public CoordinateSystemId FindCoordinateSystemId(FeatureCollection featureCollection)
        {
            if (featureCollection.CRS == null)
            {
                return CoordinateSystemId.None;
            }

            return FindCoordinateSystemId(featureCollection.CRS);
        }

        /// <summary>
        /// Tries to finds the coordinate system crs property used.
        /// </summary>        
        /// <param name="crsObject">The crs object.</param>
        /// <returns>The coordinate system id used. If no crs was found, None is returned.</returns>
        public CoordinateSystemId FindCoordinateSystemId(ICRSObject crsObject)
        {
            if (crsObject.Type == CRSType.Link)
            {
                // We don't support link conversion
                return CoordinateSystemId.None;
            }

            if (crsObject.Type == CRSType.Name)
            {
                return FindCoordinateSystemId((NamedCRS) crsObject);
            }

            if (crsObject.Type == CRSType.EPSG)
            {
                var crs = (EPSGCRS) crsObject;

                switch (crs.Code)
                {
                    case 3006:
                        return CoordinateSystemId.SWEREF99_TM;
                    case 2400:
                    case 3021:
                        return CoordinateSystemId.Rt90_25_gon_v;
                    case 900913:
                    case 3857:
                        return CoordinateSystemId.GoogleMercator;
                    case 4619:
                        return CoordinateSystemId.SWEREF99;
                    case 4326:
                        return CoordinateSystemId.WGS84;
                    default:
                        return CoordinateSystemId.None;
                } 
            }

            throw new ArgumentException(string.Format("{0} is not supported.", crsObject.Type));
        }

        /// <summary>
        /// Tries to finds the coordinate system crs property used.
        /// </summary>                
        /// <param name="namedCrs">The crs object.</param>
        /// <returns>The coordinate system id used. If no crs was found, None is returned.</returns>
        public CoordinateSystemId FindCoordinateSystemId(NamedCRS namedCrs)
        {
            if (namedCrs == null || string.IsNullOrEmpty(namedCrs.Name))
            {
                return CoordinateSystemId.None;
            }

            // todo - should check that the found search string has no trailing numbers.
            string name = namedCrs.Name.ToLower();
            if (name.Contains("4326") || name.Contains(":crs84") || name.Contains(":84"))
            {
                // Here are some examples of WGS84 definitions that are approved:
                // epsg:4326, EPSG:4326, urn:ogc:def:crs:EPSG::4326, urn:ogc:def:crs:OGC:1.3:CRS84
                // urn:ogc:def:crs:OGC:2:84, urn:ogc:def:crs:EPSG::4326
                return CoordinateSystemId.WGS84;
            }

            if (name.Contains("3006"))
            {
                return CoordinateSystemId.SWEREF99_TM;
            }

            if (name.Contains("4619"))
            {
                return CoordinateSystemId.SWEREF99;
            }

            if (name.Contains("3021") || name.Contains("2400"))
            {
                return CoordinateSystemId.Rt90_25_gon_v;
            }

            if (name.Contains("900913") || name.Contains("3857"))
            {
                return CoordinateSystemId.GoogleMercator;
            }

            return CoordinateSystemId.None;            
        }
    }
}
