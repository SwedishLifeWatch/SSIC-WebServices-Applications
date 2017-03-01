using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using Newtonsoft.Json;

namespace ArtDatabanken.GIS.GeoJSON.Net.Converters
{
    /// <summary>
    /// Converts CRSType enum to/from JSON.
    /// </summary>
    public class CRSTypeEnumConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
             CRSType crsType = (CRSType)value;
            switch (crsType)
            {
                case CRSType.Link:
                    writer.WriteValue("link");
                    break;
                case CRSType.Name:
                    writer.WriteValue("name");
                    break;
                default:
                    writer.WriteValue(value.ToString());
                    break;
            }
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
            string enumString = (string)reader.Value;
            if (enumString != null)
            {
                enumString = enumString.ToLower();
            }

            CRSType? crsType = null; 
            if (enumString == "link")
            {
                crsType = CRSType.Link;
            }
            else if (enumString == "name")
            {
                crsType = CRSType.Name;
            }

            return crsType;
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
            //return objectType == typeof(string);
        }
    }
}
