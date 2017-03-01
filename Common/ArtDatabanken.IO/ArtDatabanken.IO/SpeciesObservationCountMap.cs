using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ArtDatabanken.Data;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// This class generates a map where a species disteribution is drawn based on grid cell specific observation counts.
    /// </summary>
    public class SpeciesObservationCountMap
    {
        private const String TEXT_FONT = "Arial";
        private static Int32 _xMin, _xMax, _yMin, _yMax, _width, _height;
        private const Int32 _legendBoxWidth = 76;
        private static Int32 _bitmapHeight = 708;
        private static Int32 _bitmapWidth = 301;
        private Int32 _legendBoxOffsetX = 0;
        private float _scale;
        private IList<IGridCellSpeciesObservationCount> _observationCounts = null;
        private Bitmap _map;
        private static ShapeFile _shapeFile = null;
        private Int32 _shapeFileRecordCount = 0;

        private Color _colorLine = Color.Black;
        private Color _colorText = Color.Black;
        private Color _colorBackground = Color.LightGray;
        private Color _colorLandBackground = Color.Gray;
        private Color _colorLegendBackground = Color.White;

        /// <summary>
        /// This method must be called before a map objects can be constructed. 
        /// The purpose is to set the Shape file that will be used as a background map.
        /// As it is a static method it only has to be done once during an application run.
        /// </summary>
        /// <param name="shapeFileName">File name of the shape file</param>
        public static void InitializeMap(String shapeFileName)
        {
            _shapeFile = new ShapeFile();
            _shapeFile.Read(shapeFileName);
            _xMin = (Int32)_shapeFile.FileHeader.XMin;
            _xMax = (Int32)_shapeFile.FileHeader.XMax;
            _yMin = (Int32)_shapeFile.FileHeader.YMin;
            _yMax = (Int32)_shapeFile.FileHeader.YMax;
            _width = _xMax - _xMin;
            _height = _yMax - _yMin;
        }

        /// <summary>
        /// Constructor of the SpeciesOboservationCountMap.
        /// </summary>
        /// <param name="observationCounts">A list of Grid Cell Species observation counts</param>
        public SpeciesObservationCountMap(IList<IGridCellSpeciesObservationCount> observationCounts)
        {
            FontName = TEXT_FONT;
            _observationCounts = observationCounts;
        }

        /// <summary>
        /// Name of the font that is used to draw text in the
        /// species observation count map image.
        /// </summary>
        public String FontName
        { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public Int32 ShapeFileRecordCount
        {
            get
            {
                if (_shapeFile.IsNotNull())
                {
                    _shapeFileRecordCount = _shapeFile.Records.Count;
                }
                return _shapeFileRecordCount;
            }
        }

        /// <summary>
        /// This method saves the species observation count map to a file.
        /// </summary>
        /// <param name="fileName">Filename of the species observation count map, including full path</param>
        /// <param name="fileFormat">The file format of the raster file</param>
        public void Save(String fileName, System.Drawing.Imaging.ImageFormat fileFormat)
        {
            _map = getBitmap();
            _map.Save(fileName, fileFormat);
        }

        private Bitmap getBitmap()
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

            bitmap = new Bitmap(_bitmapWidth + _legendBoxWidth - _legendBoxOffsetX, _bitmapHeight + 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics canvas = Graphics.FromImage(bitmap))
            {
                using (Pen pen = new Pen(_colorLine, 1))
                {
                    using (Brush backgroundBrush = new SolidBrush(_colorBackground))
                    {
           
                        canvas.FillRectangle(backgroundBrush, 0, 0, _bitmapWidth + _legendBoxWidth - _legendBoxOffsetX, _bitmapHeight + 2);

                        //Draw background map from initialized shape file
                        for (int recordIndex = 0; recordIndex < _shapeFile.Records.Count; recordIndex++)
                        {
                            ShapeFileRecord record = _shapeFile.Records[recordIndex];

                            for (int i = 0; i < record.NumberOfParts; i++)
                            {
                                List<System.Drawing.Point> points = new List<System.Drawing.Point>();

                                // Determine the starting index and the end index
                                // into the points array that defines the figure.
                                int start = record.Parts[i];
                                int end;
                                if (record.NumberOfParts > 1 && i != (record.NumberOfParts - 1))
                                    end = record.Parts[i + 1];
                                else
                                    end = record.NumberOfPoints;


                                for (int j = start; j < end; j++)
                                {
                                    System.Windows.Point pt = record.Points[j];

                                    // Transform from lon/lat to canvas coordinates.
                                    //pt = this._shapeTransform.Transform(pt);

                                    System.Drawing.Point point = new System.Drawing.Point(_legendBoxWidth - _legendBoxOffsetX + (int)((pt.X - _xMin) * _scale), _bitmapHeight - (int)((pt.Y - _yMin) * _scale));
                                    points.Add(point);

                                }

                                using (Brush countyBrush = new SolidBrush(_colorLandBackground))
                                {
                                  canvas.FillPolygon(countyBrush, points.ToArray());
                                }
                            }
                        }

                        //Heat rects
                        if (_observationCounts.IsNotNull())
                        {
                            int radius = (Int32)Math.Round(_observationCounts[0].GridCellSize * _scale * 0.5) + 1;

                            long maxCount = 0;

                            foreach (IGridCellSpeciesObservationCount cell in _observationCounts)
                            {
                                if (cell.ObservationCount > maxCount) { maxCount = cell.ObservationCount; }
                            }

                            canvas.DrawImage(getLegendBox(maxCount), 2, 2);

                            foreach (IGridCellSpeciesObservationCount cell in _observationCounts)
                            {

                                Double cellX = cell.OrginalGridCellCentreCoordinate.X;
                                Double cellY = cell.OrginalGridCellCentreCoordinate.Y;
                                System.Drawing.Point point = new System.Drawing.Point(_legendBoxWidth - _legendBoxOffsetX + (int)((cellX - _xMin) * _scale), _bitmapHeight - (int)((cellY - _yMin) * _scale));
                                using (Brush heatBrush = getHeatBrush(Math.Log(cell.ObservationCount), Math.Log(1), Math.Log(maxCount)))
                                {
                                    canvas.FillRectangle(heatBrush, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
                                }
                            }

                        }

                        //Draw polygon border lines from initialized shape file
                        for (int recordIndex = 0; recordIndex < _shapeFile.Records.Count; recordIndex++)
                        {
                            ShapeFileRecord record = _shapeFile.Records[recordIndex];

                            for (int i = 0; i < record.NumberOfParts; i++)
                            {
                                List<System.Drawing.Point> points = new List<System.Drawing.Point>();

                                // Determine the starting index and the end index
                                // into the points array that defines the figure.
                                int start = record.Parts[i];
                                int end;
                                if (record.NumberOfParts > 1 && i != (record.NumberOfParts - 1))
                                    end = record.Parts[i + 1];
                                else
                                    end = record.NumberOfPoints;


                                for (int j = start; j < end; j++)
                                {
                                    System.Windows.Point pt = record.Points[j];

                                    System.Drawing.Point point = new System.Drawing.Point(_legendBoxWidth - _legendBoxOffsetX + (int)((pt.X - _xMin) * _scale), _bitmapHeight - (int)((pt.Y - _yMin) * _scale));
                                    points.Add(point);

                                }
                                canvas.DrawPolygon(pen, points.ToArray());
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
        private Bitmap getLegendBox(long maxCount)
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
                                using (Pen currentPen = new Pen(getHeatBrush(current, Math.Log(1), Math.Log(maxCount))))
                                {
                                    canvas.DrawLine(currentPen, 0, i, (Int32)Math.Floor(_legendBoxWidth * 0.5), i);
                                }
                                current = current + increament;
                            }
                            canvas.DrawString(1.ToString(), font, blackBrush, legendColorWidth + 2*padding, 0);
                            canvas.DrawString(maxCount.ToString(), font, blackBrush, legendColorWidth + 2*padding, bitmapHeight - 3*padding);
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
                _map = getBitmap();
                return _map;
            }
        }

        /// <summary>
        /// The height of the raster map in pixcels.
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
        /// Generates the RGB values based on current data value.
        /// </summary>
        /// <param name="val">Current value.</param>
        /// <param name="min">Min value.</param>
        /// <param name="max">Max value.</param>
        /// <returns></returns>
        public static byte[] FauxColourRGB(double val, double min, double max)
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

        /// <summary>
        /// Initialize a heat brush.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Brush getHeatBrush(byte[] color)
        {
            Brush brush;
            brush = new SolidBrush(Color.FromArgb(color[0], color[1], color[2]));
            return brush;

        }

        /// <summary>
        /// Initialized a heat brush.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private Brush getHeatBrush(double count, double min, double max)
        {
            Brush brush = getHeatBrush(FauxColourRGB(count, min, max));
            return brush;
        }
    }
}
