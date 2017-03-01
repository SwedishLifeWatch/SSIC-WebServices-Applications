using System;
using System.Collections.Generic;
using System.Drawing;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Polygon = ArtDatabanken.GIS.GeoJSON.Net.Geometry.Polygon;

namespace ArtDatabanken.GIS.IO
{
    /// <summary>
    /// This class generates a distribution map.
    /// </summary>
    public class SpeciesObservationPointMap
    {
        private const String TEXT_FONT = "Arial";
        private static Int32 _xMin, _xMax, _yMin, _yMax, _width, _height;
        private const Int32 _legendBoxWidth = 0; // 76;
        private static Int32 _bitmapHeight = 708;
        private static Int32 _bitmapWidth = 301;
        private Int32 _legendBoxOffsetX = 0;
        private float _scale;
        private IList<IGridCellSpeciesObservationCount> _observationCounts = null;
        private Bitmap _map;
        private static FeatureCollection _geoJsonFile = null;
        private Color _colorLine = Color.Black;
        private Color _colorText = Color.Black;
        private Color _colorBackground = Color.White;        
        private Color _colorLandBackground = Color.FromArgb(220, 250, 180);//Color.Gray;
        private Color _colorLegendBackground = Color.White;
        private Int32 _marginLeft = 70;
        private Int32 _marginRight = 40;
        private Int32 _marginTop = 50;
        private Int32 _marginBottom = 120;


        /// <summary>
        /// This method must be called before a map objects can be constructed. 
        /// The purpose is to set the GeoJson file that will be used as a background map.
        /// As it is a static method it only has to be done once during an application run.
        /// </summary>
        /// <param name="filePath">File name of the GeoJson file</param>
        public static void InitializeMap(String filePath)
        {
            String str = System.IO.File.ReadAllText(filePath);
            FeatureCollection featureCollection = JsonConvert.DeserializeObject(str, typeof(FeatureCollection)) as FeatureCollection;
            _geoJsonFile = featureCollection;
            _xMin = (Int32)featureCollection.BoundingBoxes[0];
            _xMax = (Int32)featureCollection.BoundingBoxes[2];
            _yMin = (Int32)featureCollection.BoundingBoxes[1];
            _yMax = (Int32)featureCollection.BoundingBoxes[3];
            _width = _xMax - _xMin;
            _height = _yMax - _yMin;
        }

        /// <summary>
        /// Constructor of the SpeciesOboservationCountMap.
        /// </summary>
        /// <param name="observationPoints">A list of Grid Cell Species observation counts</param>
        public SpeciesObservationPointMap(IList<IGridCellSpeciesObservationCount> observationPoints)
        {
            FontName = TEXT_FONT;
            _observationCounts = observationPoints;
        }

        /// <summary>
        /// Name of the font that is used to draw text in the
        /// species observation count map image.
        /// </summary>
        public String FontName
        { get; set; }

        /// <summary>
        /// This method saves the species observation count map to a file.
        /// </summary>
        /// <param name="fileName">Filename of the species observation count map, including full path</param>
        /// <param name="fileFormat">The file format of the raster file</param>
        public void Save(String fileName, System.Drawing.Imaging.ImageFormat fileFormat)
        {
            _map = GetBitmap();
            _map.Save(fileName, fileFormat);
        }

        /// <summary>
        /// Generates the map.
        /// </summary>
        /// <returns>Map as bitmap file.</returns>
        private Bitmap GetBitmap()
        {
            Bitmap bitmap;
            float xScale = 1.0F * _bitmapWidth / _width;
            float yScale = 1.0F * _bitmapHeight / _height;

            if (xScale < yScale)
            {
                _scale = xScale;
            }
            else
            {
                _scale = yScale;
            }
            _bitmapHeight += _marginTop + _marginBottom;
            _bitmapWidth += _marginLeft + _marginRight;

            bitmap = new Bitmap(_bitmapWidth + _legendBoxWidth - _legendBoxOffsetX, _bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics canvas = Graphics.FromImage(bitmap))
            {
                using (Pen pen = new Pen(_colorLine, 1))
                {
                    using (Brush backgroundBrush = new SolidBrush(_colorBackground))
                    {                        
                        canvas.FillRectangle(backgroundBrush, 0, 0, _bitmapWidth + _legendBoxWidth - _legendBoxOffsetX, _bitmapHeight);

                        //Draw background map from initialized geojson file
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
                                    List<System.Drawing.Point> points = new List<System.Drawing.Point>();

                                    for (int j = 0; j < polygon.Coordinates[i].Coordinates.Count; j++)
                                    {
                                        GeographicPosition coordinate = polygon.Coordinates[i].Coordinates[j];
                                        System.Drawing.Point point = new System.Drawing.Point(_marginLeft + _legendBoxWidth - _legendBoxOffsetX + (int)((coordinate.Longitude - _xMin) * _scale), _bitmapHeight - _marginBottom - (int)((coordinate.Latitude - _yMin) * _scale));
                                        points.Add(point);
                                    }

                                    using (Brush countyBrush = new SolidBrush(_colorLandBackground))
                                    {
                                        canvas.FillPolygon(countyBrush, points.ToArray());
                                    }
                                }
                            }
                        }

                        //Heat rects
                        if (_observationCounts != null && _observationCounts.Count > 0)
                        {
                            int radius = (Int32)Math.Round(_observationCounts[0].GridCellSize * _scale * 0.5) + 1;

                            foreach (IGridCellSpeciesObservationCount cell in _observationCounts)
                            {
                                Double cellX = cell.OrginalGridCellCentreCoordinate.X;
                                Double cellY = cell.OrginalGridCellCentreCoordinate.Y;
                                System.Drawing.Point point = new System.Drawing.Point(_marginLeft + _legendBoxWidth - _legendBoxOffsetX + (int)((cellX - _xMin) * _scale), _bitmapHeight - _marginBottom - (int)((cellY - _yMin) * _scale));
                                using (Brush heatBrush = GetHeatBrush())
                                {
                                    canvas.FillEllipse(heatBrush, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                                }
                            }

                        }

                        //Draw polygon border lines from initialized geojson file
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
                                    List<System.Drawing.Point> points = new List<System.Drawing.Point>();

                                    for (int j = 0; j < polygon.Coordinates[i].Coordinates.Count; j++)
                                    {
                                        GeographicPosition coordinate = polygon.Coordinates[i].Coordinates[j];
                                        System.Drawing.Point point = new System.Drawing.Point(_marginLeft + _legendBoxWidth - _legendBoxOffsetX + (int)((coordinate.Longitude - _xMin) * _scale), _bitmapHeight - _marginBottom -(int)((coordinate.Latitude - _yMin) * _scale));
                                        points.Add(point);
                                    }

                                    canvas.DrawPolygon(pen, points.ToArray());
                                }
                            }
                        }
                    }
                }
            }

            return bitmap;
        }

        /// <summary>
        /// This method generates a bitmap representing the legend explaination box.
        /// </summary>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        private Bitmap GetLegendBox(long maxCount)
        {
            Bitmap bitmap;
            int bitmapHeight, bitmapWidth, padding, legendColorWidth;

            bitmapWidth = _legendBoxWidth;
            bitmapHeight = 80;
            legendColorWidth = (Int32)Math.Round(0.4 * _legendBoxWidth);
            double increament = Math.Log(maxCount) / bitmapHeight;
            padding = 5;
            double current = Math.Log(1);
            bitmap = new Bitmap(bitmapWidth, bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics canvas = Graphics.FromImage(bitmap))
            {
                using (Brush backgroundBrush = new SolidBrush(_colorBackground))
                {
                    using (Brush blackBrush = new SolidBrush(_colorText))
                    {
                        canvas.FillRectangle(backgroundBrush, 0, 0, bitmapWidth, bitmapHeight);

                        using (Font font = new Font(FontName, 12, FontStyle.Regular, GraphicsUnit.Pixel))
                        {
                            for (int i = 0; i < 80; i++)
                            {
                                using (Pen currentPen = new Pen(GetHeatBrush()))
                                {
                                    canvas.DrawLine(currentPen, 0, i, (Int32)Math.Floor(_legendBoxWidth * 0.5), i);
                                }
                                current = current + increament;
                            }
                            canvas.DrawString(1.ToString(), font, blackBrush, legendColorWidth + 2 * padding, 0);
                            canvas.DrawString(maxCount.ToString(), font, blackBrush, legendColorWidth + 2 * padding, bitmapHeight - 3 * padding);
                        }
                    }
                }
            }

            return bitmap;
        }

        /// <summary>
        /// The color of the background.
        /// </summary>
        public Color BackgroundColor
        {
            get { return _colorBackground; }
            set { _colorBackground = value; }
        }

        /// <summary>
        /// The color of the legend background.
        /// </summary>
        public Color LegendBackgroundColor
        {
            get { return _colorLegendBackground; }
            set { _colorLegendBackground = value; }
        }

        /// <summary>
        /// The bitmap representing the species observation density map.
        /// </summary>
        public Bitmap Bitmap
        {
            get
            {
                _map = GetBitmap();
                return _map;
            }
        }

        /// <summary>
        /// The height of the raster map in pixels.
        /// </summary>
        public Int32 Height
        {
            get { return _bitmapHeight; }
            set
            {
                _bitmapHeight = value;
                _bitmapWidth = (Int32)Math.Round(value * 0.42514, 0);
            }
        }

        /// <summary>
        /// Set margin in pixels.
        /// </summary>
        /// <param name="left">Left margin.</param>
        /// <param name="top">Top margin.</param>
        /// <param name="right">Right margin.</param>
        /// <param name="bottom">Bottom margin.</param>
        public void SetMargin(int left, int top, int right, int bottom)
        {
            _marginLeft = left;
            _marginTop = top;
            _marginRight = right;
            _marginBottom = bottom;
        }

        /// <summary>
        /// Initialize a heat brush.
        /// </summary>
        /// <returns></returns>
        private Brush GetHeatBrush()
        {
            Brush brush;
            brush = new SolidBrush(Color.FromArgb(0, 0, 255));
            return brush;
        }
    }
}
