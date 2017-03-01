using System.IO;
using ArtDatabanken.Data;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using CoordinateSystem = ArtDatabanken.Data.CoordinateSystem;
using NamedCRS = NetTopologySuite.CoordinateSystems.NamedCRS;

namespace ArtDatabanken.GIS.FormatConversion
{
    /// <summary>
    /// Handling shape-file conversions
    /// </summary>
    public static class ShapeConverter
    {
        private static CoordinateSystemId TryToGetCrs(string shapeFilePath)
        {
            try
            {
                //Try to get crs value from .prj file
                var prjFilePath = shapeFilePath.Replace(".shp", ".prj");

                var lines = File.ReadAllLines(prjFilePath);

                foreach (var line in lines)
                {
                    var lineUC = line.ToUpper();

                    if (lineUC.Contains("SWEREF99_TM") || lineUC.Contains("SWEREF99 TM"))
                    {
                        return CoordinateSystemId.SWEREF99_TM;
                    }

                    if (lineUC.Contains("SWEREF99"))
                    {
                        return CoordinateSystemId.SWEREF99;
                    }

                    if (lineUC.Contains("RT90") || lineUC.Contains("RT 90"))
                    {
                        return CoordinateSystemId.Rt90_25_gon_v;
                    }

                    if (lineUC.Contains("GOOGLE"))
                    {
                        return CoordinateSystemId.GoogleMercator;
                    }
                    
                    if (lineUC.Contains("WGS84") || lineUC.Contains("WGS_1984") || lineUC.Contains("WGS 84") || lineUC.Contains("WGS 1984"))
                    {
                        return CoordinateSystemId.WGS84;
                    }
                }
            }
            catch
            {
                return CoordinateSystemId.None;
            }

            return CoordinateSystemId.None;
        }

        /// <summary>
        /// Convert shape-file to geojson-file stream
        /// </summary>
        /// <param name="shapeFilePath"></param>
        public static Stream ConvertToGeoJsonStream(string shapeFilePath)
        {
            var factory = new GeometryFactory();
            //Create a feature collection 
            var featureCollection = new FeatureCollection();

            //Try to get crs from .prj file
            var crs = TryToGetCrs(shapeFilePath);

            //Set crs if we found it
            if (crs != CoordinateSystemId.None)
            {
                featureCollection.CRS = new NamedCRS(new CoordinateSystem(crs).Id.EpsgCode());
            }

            using (var shapeFileDataReader = new ShapefileDataReader(shapeFilePath, factory))
            {
                //Get shape file dbase header
                var header = shapeFileDataReader.DbaseHeader;
                
                //Loop throw all geometries
                while (shapeFileDataReader.Read())
                {
                    var attributesTable = new AttributesTable();
                    var geometry = (Geometry)shapeFileDataReader.Geometry;

                    //Get header fields
                    for (var i = 0; i < header.NumFields; i++)
                    {
                        var fldDescriptor = header.Fields[i];
                        attributesTable.AddAttribute(fldDescriptor.Name, shapeFileDataReader.GetValue(i));
                    }

                    //Create feature using geometry and attributes
                    var feature = new Feature()
                    {
                        Geometry = geometry,
                        Attributes = attributesTable
                    };

                    //Add feature to collection
                    featureCollection.Features.Add(feature);
                }

                //Close and free up any resources
                shapeFileDataReader.Close();
            }

            // Create a stream to write to.
            var outputStream = new MemoryStream();
            var sw = new StreamWriter(outputStream);
            var jsonSerializer = new GeoJsonSerializer(factory);

            //Serialize feature collection to json
            jsonSerializer.Serialize(sw, featureCollection);
            
            //Flush stream writer and reset stream position    
            sw.Flush();
            outputStream.Position = 0;
            return outputStream;
        }
    }
}
