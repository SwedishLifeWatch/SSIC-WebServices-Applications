using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.GIS.Helpers;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace ArtDatabanken.GIS.GeoJSON.Net.Converters
{
    /// <summary>
    /// Converts the crs property in the GeoJson format.
    /// </summary>
    public class CoordinateReferenceSystemConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {            
            serializer.Serialize(writer, value);
        }

        /// <summary>
        /// Parses the CRS information.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <returns>A NamedCRS, LinkedCRS or Null.</returns>
        public static ICRSObject ParseCrs(JObject jsonObject)
        {
            if (jsonObject == null || !jsonObject.HasValues)
            {
                return null;
            }

            JProperty typeProperty = jsonObject.Property("type");
            if (typeProperty != null)
            {
                string type = typeProperty.Value.ToObjectOrDefault<string>().ToLower().Trim();
                CRSBase crs = null;
                if (type == "name")
                {
                    crs = new NamedCRS();
                }
                else if (type == "link")
                {
                    crs = new LinkedCRS();
                }
                else if (type == "epsg")
                {
                    crs = new EPSGCRS();
                }
                else
                {
                    return null;
                }

                if (jsonObject.Property("properties") != null)
                {
                    foreach (JToken jToken in jsonObject.Property("properties").Value)
                    {
                        var item = (JProperty)jToken;
                        string key = item.Name;
                        object val = item.Value.ToObjectOrDefault<object>();
                        crs.Properties.Add(key, val);
                    }
                }

                if (crs.Properties.Count == 0)
                {
                    return null;
                }

                return crs;               
            }

            return null;          
        }

        /// <summary>
        /// Reads the JSON representation of the object.
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
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject jsonObject = JObject.Load(reader);
            return ParseCrs(jsonObject);         
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
            return true;
        }
    }
}
