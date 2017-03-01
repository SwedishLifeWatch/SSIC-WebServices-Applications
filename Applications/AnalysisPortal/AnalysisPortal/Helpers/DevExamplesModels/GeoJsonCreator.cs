using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AnalysisPortal.Helpers.DevExamplesModels
{
    public static class GeoJsonCreator
    {
        public static string CreateSampleJsonString()
        {
            var serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
            FeatureCollection sample = CreateSampleJsonObject();
            string strJson = JsonConvert.SerializeObject(sample, serializerSettings);
            return strJson;
        }

        private static FeatureCollection CreateSampleJsonObject()
        {
            // linestring
            var lineString = new LineString(new List<GeographicPosition>()
                                               {
                                                   new GeographicPosition(11.0878902207, 45.1602390564),
                                                   new GeographicPosition(15.01953125, 48.1298828125)
                                               });

            // polygon
            var linearRings = new List<LineString>();
            linearRings.Add(new LineString(new List<GeographicPosition>()
                                               {
                                                   new GeographicPosition(11.0878902207, 45.1602390564),
                                                   new GeographicPosition(14.931640625, 40.9228515625),
                                                   new GeographicPosition(0.8251953125, 41.0986328125),
                                                   new GeographicPosition(7.63671875, 48.96484375),
                                                   new GeographicPosition(11.0878902207, 45.1602390564),
                                               }));
            var polygon = new Polygon(linearRings);

            // point
            var pos = new GeographicPosition(15.87646484375, 44.1748046875);
            var point = new Point(pos);

            var geometryCollection = new GeometryCollection();
            geometryCollection.Geometries.Add(lineString);
            geometryCollection.Geometries.Add(polygon);
            geometryCollection.Geometries.Add(point);

            var featureCollection = new FeatureCollection(new List<Feature>()
                                                              {
                                                                  new Feature(geometryCollection)
                                                              });

            return featureCollection;

            // The result should look like this:
            //var featurecollection = {
            //        "type": "FeatureCollection",    
            //        "features": [
            //        { "geometry": {
            //            "type": "GeometryCollection",
            //            "geometries": [
            //                {
            //                    "type": "LineString",
            //                    "coordinates":
            //                        [[11.0878902207, 45.1602390564],
            //                        [15.01953125, 48.1298828125]]
            //                },
            //                {
            //                    "type": "Polygon",
            //                    "coordinates":
            //                        [[[11.0878902207, 45.1602390564],
            //                          [14.931640625, 40.9228515625],
            //                          [0.8251953125, 41.0986328125],
            //                          [7.63671875, 48.96484375],
            //                          [11.0878902207, 45.1602390564]]]
            //                },
            //                {
            //                    "type": "Point",
            //                    "coordinates": [15.87646484375, 44.1748046875]
            //                }
            //            ]
            //        },
            //            "type": "Feature",
            //            "properties": {}
            //        }
            //      ]
            //    };            
            //}
        }
    }
}