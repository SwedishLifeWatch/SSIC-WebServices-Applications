using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJsonString<T>(this T obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);

                return Encoding.UTF8.GetString(ms.ToArray());
            }    
        }

        public static T ToObject<T>(this string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
                return obj;
            } 
        }
    }
}
