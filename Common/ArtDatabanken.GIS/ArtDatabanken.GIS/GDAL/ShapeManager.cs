using System.IO;
using OSGeo.OGR;
using Ionic.Zip;

namespace ArtDatabanken.GIS.GDAL
{
    /// <summary>
    /// TBD?!?
    /// </summary>
    public static class ShapeManager
    {
        private static object key = new object();

        /// <summary>
        /// Convert geojson to shape file in zip container
        /// </summary>
        /// <param name="geoJson">Geo json to convert</param>
        /// <param name="path">Path to temorary store shape related files created</param>
        /// <param name="outputFileName">Name of shape related files</param>
        /// <returns></returns>
        public static byte[] CreateShapeFilesAsZip(string geoJson, string path, string outputFileName)
        {
            //Lock to avoid that files would be deleted when other user doing export 
            lock (key)
            {
                //Register the vector drivers  
                Ogr.RegisterAll();

                //Reading the vector data  
                var dataSource = Ogr.Open(geoJson, 0);

                //Make sure that output files dosn't exists
                DeleteFiles(path, outputFileName);

                var outputFilePath = string.Format("{0}{1}.shp", path, outputFileName);
                //Create new shape  
                var outputDriver = Ogr.GetDriverByName("ESRI Shapefile");
                using (var createDataset = outputDriver.CreateDataSource(outputFilePath, null))
                {
                    var layerCount = dataSource.GetLayerCount();

                    //Cope all layers from source geojson to target shape
                    for (var i = 0; i < layerCount; i++)
                    {
                        var layer = dataSource.GetLayerByIndex(i);
                        createDataset.CopyLayer(layer, layer.GetName(), null);
                    }
                    createDataset.SyncToDisk();
                }

                try
                {
                    //Create a memory stream to store zip file
                    using (var memoryStream = new MemoryStream())
                    {
                        //Create a zip in memory
                        using (var compressedFileStream = new ZipOutputStream(memoryStream))
                        {
                            //Get all shape related files
                            var fileNames = Directory.GetFiles(path, string.Format("{0}.*", outputFileName));
                            foreach (var fileName in fileNames)
                            {
                                //Read file and add it to zip
                                using (var fileStream = File.OpenRead(fileName))
                                {
                                    compressedFileStream.PutNextEntry(fileName.Replace(path, ""));
                                    var buffer = new byte[1024];
                                    var bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                                    while (bytesRead > 0)
                                    {
                                        compressedFileStream.Write(buffer, 0, bytesRead);
                                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                                    }

                                    fileStream.Close();
                                }
                            }
                        }

                        return memoryStream.ToArray();
                    }
                }
                finally
                {
                    DeleteFiles(path, outputFileName);
                }
            }
        }

        private static void DeleteFiles(string path, string filter)
        {
            //Make sure that output files dosn't exists
            var fileNames = Directory.GetFiles(path, string.Format("{0}.*", filter));
            foreach (var fileName in fileNames)
            {
                File.Delete(fileName);
            }
        }
    }
}
