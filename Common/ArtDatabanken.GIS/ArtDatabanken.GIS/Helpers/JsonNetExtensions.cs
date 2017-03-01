using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ArtDatabanken.GIS.Helpers
{
    /// <summary>
    /// Extension methods for Json.Net.
    /// </summary>
    public static class JsonNetExtensions
    {
        /// <summary>
        /// Creates the specified .NET type from the Newtonsoft.Json.Linq.Token.
        /// If token is of Type Null, then the default type value is returned.
        /// </summary>
        /// <typeparam name="T">Type to be returned.</typeparam>
        /// <param name="token">The token.</param>
        /// <returns>If not null then the Converted value is returned; otherwise the default type value.</returns>
        public static T ToObjectOrDefault<T>(this JToken token)
        {
            if (token.Type == JTokenType.Null)
            {
                return default(T);
            }

            return token.ToObject<T>();
        }
    }
}
