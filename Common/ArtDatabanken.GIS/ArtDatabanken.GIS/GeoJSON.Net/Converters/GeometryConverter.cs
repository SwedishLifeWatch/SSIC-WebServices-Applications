// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeometryConverter.cs" company="Jörg Battermann">
//   Copyright © Jörg Battermann 2011
// </copyright>
// <summary>
//   Defines the GeometryConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ArtDatabanken.GIS.Helpers;
using ArtDatabanken.GIS.GeoJSON.Net.Exceptions;

namespace ArtDatabanken.GIS.GeoJSON.Net.Converters
{
    using System;

    using ArtDatabanken.GIS.GeoJSON.Net.Geometry;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the GeometryObject type. Converts to/from a SimpleGeo 'geometry' field
    /// </summary>
    public class GeometryConverter : JsonConverter
    {

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }


        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing value of object being read.</param><param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {            
            if (objectType == typeof(List<IGeometryObject>))
            {
                return ParseGeometries(JArray.Load(reader));
            }
            JObject jsonObject = JObject.Load(reader);
            return ParseGeometryObject(jsonObject);
        }

        /// <summary>
        /// Parses a list of geometries.
        /// </summary>
        /// <param name="jArray">The array of tokens.</param>        
        private List<IGeometryObject> ParseGeometries(JArray jArray)
        {
            List<IGeometryObject> geometries = new List<IGeometryObject>();
            foreach (JToken token in jArray)
            {
                geometries.Add(ParseGeometryObject((JObject)token));
            }
            return geometries;
        }


        /// <summary>
        /// Parses a geometry object.
        /// </summary>
        /// <param name="jObject">The object token.</param>        
        private IGeometryObject ParseGeometryObject(JObject jObject)
        {
            JProperty typeProperty = jObject.Property("type");

            if (typeProperty != null)
            {
                string geometryTypeEx = typeProperty.Value.ToObjectOrDefault<string>().ToLower();
                GeoJSONObject geoJSONObject = null;

                switch (geometryTypeEx)
                {
                    case "geometrycollection":
                        geoJSONObject = ParseGeometryCollection(jObject.Property("geometries").Value);                        
                        break;
                    case "multilinestring":
                        geoJSONObject = ParseMultiLineStringEx(jObject.Property("coordinates").Value);
                        break;
                    case "linestring":
                        geoJSONObject = ParseLineString(jObject.Property("coordinates").Value);
                        break;
                    case "multipolygon":
                        geoJSONObject = ParseMultiPolygon(jObject.Property("coordinates").Value);
                        break;
                    case "polygon":
                        geoJSONObject = ParsePolygon(jObject.Property("coordinates").Value);
                        break;
                    case "multipoint":
                        geoJSONObject = ParseMultiPoint(jObject.Property("coordinates").Value);
                        break;
                    case "point":
                        geoJSONObject = ParsePoint(jObject.Property("coordinates").Value);
                        break;
                }

                ParseBoundingBox(geoJSONObject, jObject.Property("bbox"));
                if (jObject.Property("crs") != null)
                {                    
                    geoJSONObject.CRS = CoordinateReferenceSystemConverter.ParseCrs(jObject.Property("crs").Value.ToObjectOrDefault<JObject>());    
                }
                
                return (IGeometryObject)geoJSONObject;
            }
            throw new ParsingException();
        }

       
        /// <summary>
        /// Parses the bounding box.
        /// </summary>
        /// <remarks>
        /// The bounding boxes will look like this "bbox": [100.0, 0.0, 105.0, 1.0]
        /// </remarks>
        /// <param name="geoJSONObject">The geo JSON object.</param>
        /// <param name="jProperty">The json.net property object.</param>
        private void ParseBoundingBox(GeoJSONObject geoJSONObject, JProperty jProperty)
        {
            if (jProperty == null || !jProperty.HasValues)
                return;
                        
            if (jProperty.Value.Type == JTokenType.Array)
            {
                JArray bboxArray = jProperty.Value.Value<JArray>();                
                double[] bbox = new double[bboxArray.Count];
                for (int i = 0; i < bboxArray.Count; i++)
                {
                    bbox[i] = bboxArray[i].ToObjectOrDefault<double>();                    
                }

                geoJSONObject.BoundingBoxes = bbox;
            }            
        }

        /// <summary>
        /// Parses a GeographicPosition.
        /// </summary>
        /// <param name="jToken">The token.</param>        
        private GeographicPosition ParseGeographicPosition(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            JArray coordinates = jToken.Value<JArray>();
            if (coordinates == null || coordinates.Count < 2)
            {
                throw new ParsingException(string.Format("Point geometry coordinates could not be parsed. Expected something like '[-122.428938,37.766713]' ([lon,lat]), what we received however was: {0}", coordinates));
            }

            string latitude;
            string longitude;
            string altitude = null;
            try
            {
                longitude = coordinates[0].ToString();
                latitude = coordinates[1].ToString();
                if (coordinates.Count == 3)
                    altitude = coordinates[2].ToString();
            }
            catch (Exception ex)
            {
                throw new ParsingException("Could not parse GeoJSON Response. (Latitude or Longitude missing from Point geometry?)", ex);
            }

            GeographicPosition geographicPosition;
            if (coordinates.Count == 2)
                geographicPosition = new GeographicPosition(longitude, latitude);
            else
                geographicPosition = new GeographicPosition(longitude, latitude, altitude);

            return geographicPosition;
        }


        /// <summary>
        /// Parses a Point.
        /// </summary>
        /// <param name="jToken">The token.</param>
        private Point ParsePoint(JToken jToken)
        {
            GeographicPosition coordinate = ParseGeographicPosition(jToken);
            return new Point(coordinate);
        }


        /// <summary>
        /// Parses a MultiPoint.
        /// </summary>
        /// <param name="jToken">The token.</param>        
        private MultiPoint ParseMultiPoint(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<Point> points = new List<Point>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken item in coordinatesArray)
            {
                Point point = ParsePoint(item);
                points.Add(point);
            }
            return new MultiPoint(points);
        }


        /// <summary>
        /// Parses a LineString.
        /// </summary>
        /// <param name="jToken">The token.</param>
        /// <returns></returns>
        /// <exception cref="ParsingException"></exception>
        private LineString ParseLineString(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<GeographicPosition> coordinates = new List<GeographicPosition>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken coordinateItem in coordinatesArray)
            {
                GeographicPosition geographicPosition = ParseGeographicPosition(coordinateItem);
                coordinates.Add(geographicPosition);
            }

            return new LineString(coordinates);
        }


        /// <summary>
        /// Parses a MultiLineString.
        /// </summary>
        /// <param name="jToken">The token.</param>        
        private MultiLineString ParseMultiLineStringEx(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<LineString> lineStrings = new List<LineString>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken item in coordinatesArray)
            {
                LineString lineString = ParseLineString(item);
                lineStrings.Add(lineString);
            }
            return new MultiLineString(lineStrings);
        }


        /// <summary>
        /// Parses a polygon.
        /// </summary>
        /// <param name="jToken">The token.</param>        
        private Polygon ParsePolygon(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<LineString> linearRings = new List<LineString>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken token in coordinatesArray)
            {
                LineString lineString = ParseLineString(token);
                linearRings.Add(lineString);
            }
            return new Polygon(linearRings);
        }


        /// <summary>
        /// Parses a MultiPolygon.
        /// </summary>
        /// <param name="jToken">The token.</param>        
        private MultiPolygon ParseMultiPolygon(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<Polygon> polygons = new List<Polygon>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken item in coordinatesArray)
            {
                Polygon polygon = ParsePolygon(item);
                polygons.Add(polygon);
            }
            return new MultiPolygon(polygons);
        }


        /// <summary>
        /// Parses a GeometryCollection.
        /// </summary>
        /// <param name="jToken">The token.</param>        
        private GeometryCollection ParseGeometryCollection(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<IGeometryObject> geometries = new List<IGeometryObject>();
            JArray geometriesArray = jToken.Value<JArray>();
            foreach (JObject item in geometriesArray)
            {
                IGeometryObject geometry = ParseGeometryObject(item);
                geometries.Add(geometry);
            }
            return new GeometryCollection(geometries);
        }


        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            bool result;
            if (objectType.IsInterface)
            {
                result = objectType == typeof(IGeometryObject);
            }
            else
            {
                result = objectType.GetInterface(typeof(IGeometryObject).Name, true) != null;
            }

            if (objectType == typeof(List<IGeometryObject>))
                return true;
            return result;
        }
    }
}
