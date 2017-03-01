using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.GIS.Test.GeoJSON.Net
{
    [TestClass]
    public class DataContractSerializationTest
    {
        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void SerializePoint()
        {
            var pos = new GeographicPosition(15.87646484375, 44.1748046875);
            var point1 = new Point(pos);

            var memoryStream = new MemoryStream();
            // save                       
            var ser = new DataContractSerializer(typeof(Point)); //, knownTypes);
            ser.WriteObject(memoryStream, point1);
            memoryStream.Position = 0;

            // load            
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, new XmlDictionaryReaderQuotas()))
            {                
                Point point2 = (Point)ser.ReadObject(reader, true);
                Assert.IsTrue(point2 != null);                
                Assert.IsTrue(Math.Abs(point1.Coordinates.Latitude - point2.Coordinates.Latitude) < 0.0001);
                Assert.IsTrue(Math.Abs(point1.Coordinates.Longitude - point2.Coordinates.Longitude) < 0.0001);
            }    

        }


        //[TestMethod]
        //public void DeSerializePoint()
        //{
        //    string geoJson = "{\"type\":\"Point\",\"coordinates\":[15.87646484375,44.1748046875],\"crs\":null,\"bbox\":null}";
        //    Point point = JsonConvert.DeserializeObject(geoJson, typeof(Point)) as Point;

        //    Assert.IsTrue(point != null, "GeoJSON deserialization of point failed.");
        //}

        //[TestMethod]
        //public void SerializeLinestring()
        //{
        //    var lineString = new LineString(new List<IPosition>()
        //                                       {
        //                                           new GeographicPosition(11.0878902207, 45.1602390564),
        //                                           new GeographicPosition(15.01953125, 48.1298828125)
        //                                       });

        //    var serializerSettings = new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
        //    string geoJson = JsonConvert.SerializeObject(lineString, serializerSettings);

        //    Assert.IsTrue(!string.IsNullOrEmpty(geoJson), "GeoJSON serialization of lineString failed.");
        //}


    }
}
