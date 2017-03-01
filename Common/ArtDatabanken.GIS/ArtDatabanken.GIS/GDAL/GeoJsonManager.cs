using System;
using System.IO;
using ArtDatabanken.Data;
using OSGeo.GDAL;
using OSGeo.OGR;
using DataType = OSGeo.GDAL.DataType;
using ArtDatabanken.GIS.Extensions;

namespace ArtDatabanken.GIS.GDAL
{
    /// <summary>
    /// The manager class for handling GeoJson specific functions
    /// </summary>
    public static class GeoJsonManager
    {
        private static int ProgressFunc(double complete, IntPtr message, IntPtr data)
        {
            Console.Write("Processing ... " + complete * 100 + "% Completed.");
            if (message != IntPtr.Zero)
            {
                Console.Write(" Message:" + System.Runtime.InteropServices.Marshal.PtrToStringAnsi(message));
            }
            if (data != IntPtr.Zero)
            {
                Console.Write(" Data:" + System.Runtime.InteropServices.Marshal.PtrToStringAnsi(data));
            }
            Console.WriteLine("");
            return 1;
        }

       

        /// <summary>
        /// Create a .geotiff raster image from a geo json
        /// </summary>
        /// <param name="geoJson">Geojson string</param>
        /// <param name="coordinateSystem">Current coordinate system</param>
        /// <param name="filePathTiff">Path where the .geotiff file will be created</param>
        /// <param name="pixelsX">Tiff image width in pixels (mandatory if not pixelSize is passed)</param>
        /// <param name="pixelsY">Tiff image height in pixels (mandatory if not pixelSize is passed)</param>
        /// <param name="pixelSize">How many meters should each pixel represent (mandatory if not width and height in pixels are passed)</param>
        /// <param name="attribute">Optional attribute to get value from. If not passed 255 will be used for all shapes</param>
        /// <param name="dataType">Attribute data type, default byte</param>
        public static byte[] CreateTiffFile(string geoJson, CoordinateSystemId coordinateSystem, string filePathTiff, int pixelsX = 0, int pixelsY = 0, int pixelSize = 0, string attribute = null, Data.DataType dataType = Data.DataType.Byte)
        {
            if ((pixelsX == 0 || pixelsY == 0) && pixelSize == 0)
            {
                throw new Exception("You have to state image size in pixels or pixel size");
            }

            geoJson = geoJson.ReplaceSwedishChars();
            attribute = attribute.ReplaceSwedishChars();

            //Create a temporary file name
            filePathTiff = Path.Combine(filePathTiff, Guid.NewGuid().ToString() + ".tif");

            try
            {
                //Register the vector drivers  
                Ogr.RegisterAll();

                //Reading the vector data  
                var dataSource = Ogr.Open(geoJson, 0);
                var layer = dataSource.GetLayerByIndex(0);
                var envelope = new Envelope();
                layer.GetExtent(envelope, 1);

                double pixelSizeX = pixelSize;
                double pixelSizeY = pixelSize;

                //Calculate diff between min and max
                var xDiff = envelope.MaxX - envelope.MinX;
                var yDiff = envelope.MaxY - envelope.MinY;

                //Calculate raster size in pixels if pixel size is passed
                if ((pixelsX == 0 || pixelsY == 0))
                {
                    var sumDiff = xDiff + yDiff;

                    //Calculate ratio %
                    var xRatio = xDiff/sumDiff;
                    var yRatio = yDiff/sumDiff;

                    //Calculate pixels width and height
                    pixelsX = (int)(xRatio * pixelSize);
                    pixelsY = (int)(yRatio * pixelSize);
                }
                
                //Calculate pixel size
                pixelSizeX = xDiff / pixelsX;
                pixelSizeY = yDiff / pixelsY;

                //Register the raster drivers  
                Gdal.AllRegister();

                //Check if output raster exists & delete (optional)  
                if (File.Exists(filePathTiff))
                {
                    File.Delete(filePathTiff);
                }

                //Create new tiff   
                var outputDriver = Gdal.GetDriverByName("GTiff");
                var dt = DataType.GDT_Byte;

                switch (dataType)
                {
                    case Data.DataType.Float32:
                        dt = DataType.GDT_Float32;
                        break;
                    case Data.DataType.Float64:
                        dt = DataType.GDT_Float64;
                        break;
                    case Data.DataType.Int32:
                        dt = DataType.GDT_Int32;
                        break;
                }

                using (var createDataset = outputDriver.Create(filePathTiff, pixelsX, pixelsY, 1, dt, null))
                {
                    //Extrac srs from input feature   
                    string inputShapeSrs;

                    var spatialRefrence = layer.GetSpatialRef();
                    spatialRefrence.ExportToWkt(out inputShapeSrs);
                    //Assign input feature srs to outpur raster  
                    createDataset.SetProjection(inputShapeSrs);

                    //Geotransform  
                    var argin = new[] { envelope.MinX, pixelSizeX, 0, envelope.MaxY, 0, -pixelSizeY };
                    createDataset.SetGeoTransform(argin);

                    // Define pixel_size and NoData value of new raster  
                    const double noDataValue = 0;
                    //Set no data  
                    var band = createDataset.GetRasterBand(1);
                    band.SetNoDataValue(noDataValue);
                    //close tiff  
                    createDataset.FlushCache();
                }

                //Feature to raster rasterize layer options  
                //No of bands (1)  
                var bandlist = new[] { 1 };

                //Values to be burn on raster when no attribute name is passed  
                var burnValues = new[] { 255.0 };

                using (var outputDataset = Gdal.Open(filePathTiff, Access.GA_Update))
                {
                    string[] rasterizeOptions = attribute == null ? null : new[] { "ALL_TOUCHED=TRUE", string.Format("ATTRIBUTE={0}", attribute) };
                    Gdal.RasterizeLayer(outputDataset, 1, bandlist, layer, IntPtr.Zero, IntPtr.Zero, 1, burnValues, rasterizeOptions, ProgressFunc, "Raster conversion");
                }

                return File.ReadAllBytes(filePathTiff);
            }
            finally
            {
                if (File.Exists(filePathTiff))
                {
                    File.Delete(filePathTiff);
                }
            }
            
        }
    }
}
