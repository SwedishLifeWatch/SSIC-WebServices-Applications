using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Polygon = ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// This class generates grid heat maps where a species disteribution is drawn based on grid cell specific counts.
    /// </summary>
    public class MapImage
    {
        private const string TEXT_FONT = "Arial";
        private const int LEGEND_BOX_WIDTH = 76;
        private const int MAP_OFFSET_LEFT = 25;
        private const int MAP_OFFSET_TOP = 25;
        private const int MAP_OFFSET_RIGHT = 25;
        private const int MAP_OFFSET_BOTTOM = 25;
        private const int CANVAS_EXTRASIZE_X = MAP_OFFSET_LEFT + MAP_OFFSET_RIGHT;
        private const int CANVAS_EXTRASIZE_Y = MAP_OFFSET_TOP + MAP_OFFSET_BOTTOM;

        private static int _xMin, _xMax, _yMin, _yMax, _width, _height;
        private static int _bitmapHeight = 708;
        private static int _bitmapWidth = 301;
        private static FeatureCollection _geoJsonFile = null;

        private int _legendBoxOffsetX = 0;
        private float _scale;

        /// <summary>
        /// This method must be called before a map objects can be constructed. 
        /// The purpose is to set the GeoJson file that will be used as a background map.
        /// As it is a static method it only has to be done once during an application run.
        /// </summary>
        /// <param name="filePath">File name of the GeoJson file</param>
        public static void InitializeMap(String filePath)
        {
            var str = System.IO.File.ReadAllText(filePath);
            _geoJsonFile = JsonConvert.DeserializeObject(str, typeof(FeatureCollection)) as FeatureCollection;

            if (_geoJsonFile == null)
            {
                return;   
            }

            _xMin = (int)_geoJsonFile.BoundingBoxes[0];
            _xMax = (int)_geoJsonFile.BoundingBoxes[2];
            _yMin = (int)_geoJsonFile.BoundingBoxes[1];
            _yMax = (int)_geoJsonFile.BoundingBoxes[3];
            _width = _xMax - _xMin;
            _height = _yMax - _yMin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridHeatMap"/> class.
        /// </summary>
        public MapImage()
        {            
            FontName = TEXT_FONT;

            ColorLine = Color.Black;
            ColorText = Color.Black;
            ColorBackground = Color.LightGray;
            ColorLandBackground = Color.Gray;
            ColorLegendBackground = Color.White;
        }

        /// <summary>
        /// Property for legend background color
        /// </summary>
        public Color ColorLine { get; set; }

        /// <summary>
        /// Property for text color
        /// </summary>
        public Color ColorText { get; set; }

        /// <summary>
        /// Property for background color
        /// </summary>
        public Color ColorBackground { get; set; }

        /// <summary>
        /// Property for land background color
        /// </summary>
        public Color ColorLandBackground { get; set; }

        /// <summary>
        /// Property for legend background color
        /// </summary>
        public Color ColorLegendBackground { get; set; }
        
        /// <summary>
        /// Name of the font that is used to draw text in the
        /// species observation count map image.
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// The height of the raster map in pixcels.
        /// </summary>
        public int Height
        {            
            get
            {
                return _bitmapHeight;
            }

            set
            {
                _bitmapHeight = value;
                _bitmapWidth = (int)Math.Round(value * 0.42514, 0);
            }
        }

        private Bitmap CreateBitmap()
        {
            var xScale = 1.0F * _bitmapWidth / _width;
            var yScale = 1.0F * _bitmapHeight / _height;

            _scale = xScale < yScale ? xScale : yScale;

            return new Bitmap(_bitmapWidth + LEGEND_BOX_WIDTH - _legendBoxOffsetX + CANVAS_EXTRASIZE_X, _bitmapHeight + 2 + CANVAS_EXTRASIZE_Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        private void DrawMap(Graphics canvas)
        {
            using (var pen = new Pen(ColorLine, 1))
            {
                // Draw background map from initialized geojson file
                for (int recordIndex = 0; recordIndex < _geoJsonFile.Features.Count; recordIndex++)
                {
                    Feature feature = _geoJsonFile.Features[recordIndex];
                    List<Polygon> polygonList = new List<Polygon>();
                    if (feature.Geometry.Type == GeoJSONObjectType.MultiPolygon)
                    {
                        MultiPolygon multiPolygon = (MultiPolygon)feature.Geometry;
                        foreach (Polygon polygon in multiPolygon.Coordinates)
                        {
                            polygonList.Add(polygon);
                        }
                    }
                    else if (feature.Geometry.Type == GeoJSONObjectType.Polygon)
                    {
                        Polygon polygon = (Polygon)feature.Geometry;
                        polygonList.Add(polygon);
                    }

                    foreach (Polygon polygon in polygonList)
                    {
                        for (int i = 0; i < polygon.Coordinates.Count; i++)
                        {
                            var points = new List<System.Drawing.Point>();

                            for (int j = 0; j < polygon.Coordinates[i].Coordinates.Count; j++)
                            {
                                var coordinate = polygon.Coordinates[i].Coordinates[j];
                                var point = new System.Drawing.Point(LEGEND_BOX_WIDTH - _legendBoxOffsetX + (int)((coordinate.Longitude - _xMin) * _scale) + MAP_OFFSET_LEFT, _bitmapHeight - (int)((coordinate.Latitude - _yMin) * _scale) + MAP_OFFSET_TOP);
                                points.Add(point);
                            }

                            using (var brush = new SolidBrush(ColorLandBackground))
                            {
                                canvas.FillPolygon(brush, points.ToArray());
                                canvas.DrawPolygon(pen, points.ToArray());
                            }
                        }     
                    }                                                      
                }
            }
        }

        private void DrawPolygons(Graphics canvas, FeatureCollection featureCollection, Color color)
        {
            using (var pen = new Pen(ColorLine, 1))
            {
                // Draw background map from initialized geojson file
                foreach (var feature in featureCollection.Features)
                {                            
                    List<Polygon> polygonList = new List<Polygon>();
                    if (feature.Geometry.Type == GeoJSONObjectType.MultiPolygon)
                    {
                        MultiPolygon multiPolygon = (MultiPolygon)feature.Geometry;
                        foreach (Polygon polygon in multiPolygon.Coordinates)
                        {
                            polygonList.Add(polygon);
                        }
                    }
                    else if (feature.Geometry.Type == GeoJSONObjectType.Polygon)
                    {
                        Polygon polygon = (Polygon)feature.Geometry;
                        polygonList.Add(polygon);
                    }

                    foreach (Polygon polygon in polygonList)
                    {
                        for (int i = 0; i < polygon.Coordinates.Count; i++)
                        {
                            var points = new List<System.Drawing.Point>();

                            for (int j = 0; j < polygon.Coordinates[i].Coordinates.Count; j++)
                            {
                                var coordinate = polygon.Coordinates[i].Coordinates[j];
                                var point = new System.Drawing.Point(LEGEND_BOX_WIDTH - _legendBoxOffsetX + (int)((coordinate.Longitude - _xMin) * _scale) + MAP_OFFSET_LEFT, _bitmapHeight - (int)((coordinate.Latitude - _yMin) * _scale) + MAP_OFFSET_TOP);
                                points.Add(point);
                            }

                            using (var brush = new SolidBrush(color))
                            {
                                canvas.FillPolygon(brush, points.ToArray());
                                    canvas.DrawPolygon(pen, points.ToArray());
                            }
                        }
                    }
                }
            }
        }

        private void DrawObservations(Graphics canvas, IList<IGridCellBase> gridCells, Func<IGridCellBase, long> countMethod)
        {
            using (var pen = new Pen(ColorLine, 1))
            {
                // Heat rects
                if (gridCells.Any())
                {
                    var radius = (int)Math.Round(gridCells[0].GridCellSize * _scale * 0.5) + 1;
                    var maxCount = 0L;

                    foreach (var cell in gridCells)
                    {
                        var count = countMethod(cell);
                        if (count > maxCount)
                        {
                            maxCount = count;
                        }
        }

                    canvas.DrawImage(GetLegendBox(maxCount), 2, 2);

                    foreach (var cell in gridCells)
                    {
                        var cellX = cell.OrginalGridCellCentreCoordinate.X;
                        var cellY = cell.OrginalGridCellCentreCoordinate.Y;
                        var point = new System.Drawing.Point(LEGEND_BOX_WIDTH - _legendBoxOffsetX + (int)((cellX - _xMin) * _scale) + MAP_OFFSET_LEFT, _bitmapHeight - (int)((cellY - _yMin) * _scale) + MAP_OFFSET_TOP);
                        using (Brush heatBrush = GetHeatBrush(Math.Log(countMethod(cell)), Math.Log(1), Math.Log(maxCount)))
                        {
                            canvas.FillRectangle(heatBrush, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method generates a bitmap representing the legend explaination box.
        /// </summary>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        private Bitmap GetLegendBox(long maxCount)
        {
            var bitmapHeight = 80;
            var legendColorWidth = (int)Math.Round(0.4 * LEGEND_BOX_WIDTH);
            var increament = Math.Log(maxCount) / bitmapHeight;

            var current = Math.Log(1);
            var bitmap = new Bitmap(LEGEND_BOX_WIDTH, bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var canvas = Graphics.FromImage(bitmap))
            {
                using (var backgroundBrush = new SolidBrush(ColorBackground))
                {
                    using (var blackBrush = new SolidBrush(ColorText))
                    {
                        canvas.FillRectangle(backgroundBrush, 0, 0, LEGEND_BOX_WIDTH, bitmapHeight);
                        
                        using (var font = new Font(FontName, 12, FontStyle.Regular, GraphicsUnit.Pixel))
                        {
                            for (var i = 0; i < 80; i++)
                            {
                                using (var currentPen = new Pen(GetHeatBrush(current, Math.Log(1), Math.Log(maxCount))))
                                {
                                    canvas.DrawLine(currentPen, 0, i, (Int32)Math.Floor(LEGEND_BOX_WIDTH * 0.5), i);
                                }
                                current = current + increament;
                            }
                            var padding = 5;
                            canvas.DrawString(1.ToString(), font, blackBrush, legendColorWidth + (2 * padding), 0);
                            canvas.DrawString(maxCount.ToString(), font, blackBrush, legendColorWidth + (2 * padding), bitmapHeight - (3 * padding));
                        }
                    }
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Initialize a heat brush.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Brush GetHeatBrush(byte[] color)
            {
            return new SolidBrush(Color.FromArgb(color[0], color[1], color[2]));
        }

        /// <summary>
        /// Initialized a heat brush.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private Brush GetHeatBrush(double count, double min, double max)
            {
           return GetHeatBrush(FauxColourRGB(count, min, max));
        }

        /// <summary>
        /// Generates the RGB values based on current data value.
        /// </summary>
        /// <param name="val">Current value.</param>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <returns></returns>
        private static byte[] FauxColourRGB(double val, double min, double max)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            val = (val - min) / (max - min);
            if (val <= 0.2)
            {
                b = (byte)((val / 0.2) * 255);
            }
            else if (val > 0.2 && val <= 0.7)
            {
                b = (byte)((1.0 - ((val - 0.2) / 0.5)) * 255);
            }
            if (val >= 0.2 && val <= 0.6)
            {
                g = (byte)(((val - 0.2) / 0.4) * 255);
            }
            else if (val > 0.6 && val <= 0.9)
            {
                g = (byte)((1.0 - ((val - 0.6) / 0.3)) * 255);
            }
            if (val >= 0.5)
            {
                r = (byte)(((val - 0.5) / 0.5) * 255);
            }
            return new byte[] { r, g, b };
        }

        public Bitmap GetHeatMap(IList<IGridCellBase> gridCells, Func<IGridCellBase, long> countMethod)
        {
            var bitmap = CreateBitmap();
            using (Graphics canvas = Graphics.FromImage(bitmap))
            {
                using (var backgroundBrush = new SolidBrush(ColorBackground))
        {
                    canvas.FillRectangle(backgroundBrush, 0, 0, bitmap.Width, bitmap.Height);
                    DrawMap(canvas);
                    DrawObservations(canvas, gridCells, countMethod);
                }
        }

            return bitmap;
        }
    }
}
