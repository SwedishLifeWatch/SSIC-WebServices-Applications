using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Extensions
{
    public static class XmlExtensions
    {
        public static T ToObject<T>(this XElement element)
        {
            T classObject;

            var xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(element.ToString()))
            {
                classObject = (T)xmlSerializer.Deserialize(stringReader);
            }
            return classObject;
        }
    }
}
