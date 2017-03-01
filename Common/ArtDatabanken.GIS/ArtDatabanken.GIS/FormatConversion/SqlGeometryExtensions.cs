using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.SqlServer.Types;
using NetTopologySuite.CoordinateSystems;
using NetTopologySuite.Features;
using NetTopologySuite.IO;

namespace ArtDatabanken.GIS.FormatConversion
{
    public static class SqlGeometryExtensions
    {
        public static string ToGeoJson(this SqlGeometry geometry, KeyValuePair<string, object>[] attributes = null)
        {
            var wkt = new string(geometry.STAsText().Value);
            var wktReader = new WKTReader();
            var geom = wktReader.Read(wkt);
            
            var attributesTable = new AttributesTable();
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    attributesTable.AddAttribute(attribute.Key, attribute.Value);
                }
            }

            var feature = new Feature(geom, attributesTable);
            var featureCollection = new FeatureCollection()
            {
                CRS = new NamedCRS(string.Format("EPSG:{0}", geometry.STSrid)),
                Features = { feature }
            };
           
            var sb = new StringBuilder();
           
            using (var sw = new StringWriter(sb))
            {
                var serializer = new GeoJsonSerializer()
                {
                    Formatting = Newtonsoft.Json.Formatting.None
                };

                serializer.Serialize(sw, featureCollection);
                sw.Close();
            }

            return sb.ToString();
        }
    }
}
