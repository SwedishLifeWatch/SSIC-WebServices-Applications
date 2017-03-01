using System;
using System.Collections.Generic;
using ArtDatabanken.GIS.GeoJSON.Net.Exceptions;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArtDatabanken.GIS.GeoJSON.Net.Converters
{
    /// <summary>
    /// This class converts GeoJson coordinates from object to GeoJson and vice versa.
    /// </summary>
    public class CoordinatesConverter : JsonConverter
    {
        /// <summary>
        /// Write Json value to Json
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.GetType() == typeof(List<Point>))
            {
                WriteCoordinate(writer, (List<Point>)value);
            }
            else if (value.GetType() == typeof(List<LineString>))
            {
                WriteCoordinate(writer, (List<LineString>)value);
            }
            else if (value.GetType() == typeof(List<Polygon>))
            {
                WriteCoordinate(writer, (List<Polygon>)value);
            }
            else if (value.GetType() == typeof(GeographicPosition))
            {
                WriteCoordinate(writer, (GeographicPosition)value);
            }
            else if (value.GetType() == typeof(List<GeographicPosition>))
            {
                WriteCoordinate(writer, (List<GeographicPosition>)value);
            }
        }

        /// <summary>
        /// Serializes the coordinate to GeoJson.        
        /// </summary>
        /// <remarks>
        /// The result will look like this example: [10, 40]
        /// </remarks>
        /// <param name="writer">The writer.</param>
        /// <param name="geographicPosition">The geographic position.</param>
        private void WriteCoordinate(JsonWriter writer, GeographicPosition geographicPosition)
        {            
            if (geographicPosition == null)            
                return;            
            writer.WriteStartArray();
            writer.WriteValue(geographicPosition.Longitude);
            writer.WriteValue(geographicPosition.Latitude);
            if (geographicPosition.Altitude.HasValue)
                writer.WriteValue(geographicPosition.Altitude.Value);
            writer.WriteEndArray();
        }

        /// <summary>
        /// Serializes the coordinates to GeoJson.        
        /// </summary>
        /// <remarks>
        /// The result will look like this example: [[10, 40], [40, 30], [20, 20], [30, 10]]
        /// </remarks>
        /// <param name="writer">The writer.</param>
        /// <param name="points">The points.</param>
        private void WriteCoordinate(JsonWriter writer, IEnumerable<Point> points)
        {            
            writer.WriteStartArray();
            foreach (Point point in points)
            {
                WriteCoordinate(writer, point.Coordinates);
            }
            writer.WriteEndArray();          
        }

        /// <summary>
        /// Serializes the coordinates to GeoJson.        
        /// </summary>
        /// <remarks>
        /// The result will look like this example: [[30, 10], [10, 30], [40, 40]]
        /// </remarks>
        /// <param name="writer">The writer.</param>
        /// <param name="coordinates">The coordinates.</param>
        private void WriteCoordinate(JsonWriter writer, IEnumerable<GeographicPosition> coordinates)
        {
            writer.WriteStartArray();
            foreach (GeographicPosition coordinate in coordinates)
            {
                WriteCoordinate(writer, coordinate);
            }
            writer.WriteEndArray();
        }

        /// <summary>
        /// Serializes the coordinates to GeoJson.        
        /// </summary>
        /// <remarks>
        /// The result will look like this example: [[30, 10], [10, 30], [40, 40]]
        /// </remarks>
        /// <param name="writer">The writer.</param>
        /// <param name="lineString">The linestring.</param>
        private void WriteCoordinate(JsonWriter writer, LineString lineString)
        {            
            writer.WriteStartArray();
            foreach (GeographicPosition geographicPosition in lineString.Coordinates)
            {
                WriteCoordinate(writer, geographicPosition);
            }
            writer.WriteEndArray();
        }

        /// <summary>
        /// Serializes the coordinates to GeoJson.        
        /// </summary>
        /// <remarks>
        /// The result will look like this example: [[[10, 10], [20, 20], [10, 40]], [[40, 40], [30, 30], [40, 20], [30, 10]]]
        /// </remarks>
        /// <param name="writer">The writer.</param>        
        /// <param name="lineStrings">The linestrings.</param>
        private void WriteCoordinate(JsonWriter writer, IEnumerable<LineString> lineStrings)
        {            
            writer.WriteStartArray();
            foreach (LineString lineString in lineStrings)
            {
                WriteCoordinate(writer, lineString);
            }
            writer.WriteEndArray();
        }

        /// <summary>
        /// Serializes the coordinates to GeoJson.        
        /// </summary>
        /// <remarks>
        /// The result will look like this example: [[[30, 20], [45, 40], [10, 40], [30, 20]]]
        /// </remarks>
        /// <param name="writer">The writer.</param>                
        /// <param name="polygon">The polygon.</param>
        private void WriteCoordinate(JsonWriter writer, Polygon polygon)
        {            
            writer.WriteStartArray();
            foreach (LineString lineString in polygon.Coordinates)
            {
                WriteCoordinate(writer, lineString);
            }
            writer.WriteEndArray();
        }

        /// <summary>
        /// Serializes the coordinates to GeoJson.        
        /// </summary>
        /// <remarks>
        /// The result will look like this example: [[[[30, 20], [45, 40], [10, 40], [30, 20]]], [[[15, 5], [40, 10], [10, 20], [5, 10], [15, 5]]]]
        /// </remarks>
        /// <param name="writer">The writer.</param>                        
        /// <param name="polygons">The polygons.</param>
        private void WriteCoordinate(JsonWriter writer, IEnumerable<Polygon> polygons)
        {            
            writer.WriteStartArray();
            foreach (Polygon polygon in polygons)
            {
                WriteCoordinate(writer, polygon);
            }
            writer.WriteEndArray();
        }






        /// <summary>
        /// Parses GeoJson coordinates.        
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(List<GeographicPosition>))
            {
                return ParseListOfGeographicPosition(JToken.Load(reader));
            }
            if (objectType == typeof(List<LineString>))
            {
                return ParseListOfLineStrings(JToken.Load(reader));
            }
            if (objectType == typeof(GeographicPosition))
            {
                return ParseCoordinate(JToken.Load(reader));
            }
            if (objectType == typeof(List<Point>))
            {
                return ParseListOfPoints(JToken.Load(reader));
            }
            if (objectType == typeof(List<Polygon>))
            {
                return ParseListOfPolygons(JToken.Load(reader));
            }

            throw new ArgumentException(string.Format("{0} is not supported", objectType));
        }


        /// <summary>
        /// Parses a coordinate.
        /// </summary>
        /// <param name="jToken">The token object.</param>                
        private GeographicPosition ParseCoordinate(JToken jToken)
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
        /// Parses a list of geographic positions.
        /// </summary>
        /// <param name="jToken">The token object.</param>        
        private List<GeographicPosition> ParseListOfGeographicPosition(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<GeographicPosition> coordinates = new List<GeographicPosition>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken coordinateItem in coordinatesArray)
            {
                GeographicPosition geographicPosition = ParseCoordinate(coordinateItem);
                coordinates.Add(geographicPosition);
            }
            return coordinates;
        }

        /// <summary>
        /// Parses a LineString.
        /// </summary>
        /// <param name="jToken">The token object.</param>
        private LineString ParseLineString(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<GeographicPosition> coordinates = new List<GeographicPosition>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken coordinateItem in coordinatesArray)
            {
                GeographicPosition geographicPosition = ParseCoordinate(coordinateItem);
                coordinates.Add(geographicPosition);
            }

            return new LineString(coordinates);
        }

        /// <summary>
        /// Parses a list of LineStrings.
        /// </summary>
        /// <param name="jToken">The token object.</param>
        private List<LineString> ParseListOfLineStrings(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<LineString> lineStrings = new List<LineString>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken token in coordinatesArray)
            {
                LineString lineString = ParseLineString(token);
                lineStrings.Add(lineString);
            }

            return lineStrings;
        }

        /// <summary>
        /// Parses a list of polygons.
        /// </summary>
        /// <param name="jToken">The token object.</param>
        private List<Polygon> ParseListOfPolygons(JToken jToken)
        {
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<Polygon> polygons = new List<Polygon>();
            JArray coordinatesArray = jToken.Value<JArray>();            
            foreach (JToken token in coordinatesArray)
            {                
                Polygon polygon = new Polygon(ParseListOfLineStrings(token));
                polygons.Add(polygon);
            }
            return polygons;
        }


        /// <summary>
        /// Parses a list of points.
        /// </summary>
        /// <param name="jToken">The token object.</param>
        private List<Point> ParseListOfPoints(JToken jToken)
        {            
            if (jToken.Type != JTokenType.Array)
                throw new ParsingException();

            List<Point> points = new List<Point>();
            JArray coordinatesArray = jToken.Value<JArray>();
            foreach (JToken token in coordinatesArray)
            {
                Point point = new Point(ParseCoordinate(token));
                points.Add(point);
            }

            return points;
        }


        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {            
            if (objectType == typeof (List<Point>))
                return true;
            if (objectType == typeof (List<LineString>))
                return true;
            if (objectType == typeof (List<Polygon>))
                return true;
            if (objectType == typeof (GeographicPosition))
                return true;
            if (objectType == typeof(List<GeographicPosition>))
                return true;            

            return false;
        }
    }
}
