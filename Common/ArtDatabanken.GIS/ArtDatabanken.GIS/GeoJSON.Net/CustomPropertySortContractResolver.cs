using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ArtDatabanken.GIS.GeoJSON.Net
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomPropertySortContractResolver : DefaultContractResolver
    {
        private const int MaxPropertiesPerContract = 1000;

        /// <summary>
        /// CreateProperties
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            List<MemberInfo> members = GetSerializableMembers(type);
            if (members == null)
            {
                throw new JsonSerializationException("Null collection of seralizable members returned.");
            }

            var properties = new JsonPropertyCollection(type);

            foreach (MemberInfo member in members)
            {
                JsonProperty property = CreateProperty(member, memberSerialization);

                if (property != null)
                {
                    if (!property.Order.HasValue)
                        property.Order = 1000;
                    properties.AddProperty(property);
                }
            }

            //List<JsonProperty> orderedProperties = properties.OrderBy(p => p.Order + (MaxPropertiesPerContract * GetTypeDepth(p.DeclaringType)) ?? -1).ToList();
            List<JsonProperty> orderedProperties = properties.OrderBy(p => p.Order).ToList();
            return orderedProperties;
        }

        private static int GetTypeDepth(Type type)
        {
            int depth = 0;
            while ((type = type.BaseType) != null)
            {
                depth++;
            }

            return depth;
        }
    }

}
