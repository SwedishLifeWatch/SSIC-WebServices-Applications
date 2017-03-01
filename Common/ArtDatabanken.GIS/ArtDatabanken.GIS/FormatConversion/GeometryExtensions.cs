using System.Collections.Generic;
using System.IO;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.CoordinateSystems;
using NetTopologySuite.Features;
using NetTopologySuite.IO;

namespace ArtDatabanken.GIS.FormatConversion
{
    public static class GeometryExtensions
    {
        public static string ToGeoJson(this IGeometry geometry, int srId, KeyValuePair<string, object>[] attributes = null)
        {
            var attributesTable = new AttributesTable();
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    attributesTable.AddAttribute(attribute.Key, attribute.Value);
                }
            }

            var feature = new Feature(geometry, attributesTable);
           
            var featureCollection = new FeatureCollection()
            {
                CRS = new NamedCRS(string.Format("EPSG:{0}", srId)),
                Features = { feature }
            };

            var geoJsonSerializer = new GeoJsonSerializer(geometry.Factory);
            var stringBuilder = new StringBuilder();
            using (var sw = new StringWriter(stringBuilder))
                geoJsonSerializer.Serialize(sw, featureCollection);
            return stringBuilder.ToString();
        }
    }
}
