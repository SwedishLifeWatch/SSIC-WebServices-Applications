using System;
using System.Collections.Generic;
using System.Drawing;
using ArtDatabanken.Data;
using Point = System.Drawing.Point;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// This class generates a county occurrence map for a taxon based on Specie facts. The map is a bitmap which can be saved in eny raster format.
    /// </summary>
    public class CountyOccurrenceMap
    {
        private const String TEXT_FONT = "Arial";

        //private TransformGroup _shapeTransform;
        private static Int32 _xMin, _xMax, _yMin, _yMax, _width, _height;
        private const Int32 _legendBoxWidth = 76;
        private static Int32 _bitmapHeight = 708;
        private static Int32 _bitmapWidth = 301;
        private static Int32 _updateInformationBoxHeight = 16;
        private static Int32 _updateInformationBoxWidth = 152;
        private Int32 _legendBoxOffsetX = 0;
        private float _scale;
        private SpeciesFactList _countyInformation = null;
        private bool _countyInformationExist = true;
        private FactorList _counties = null;
        private Bitmap _map;
        private static ShapeFile _shapeFile = null;
        
        private Color _colorMissing = Color.FromArgb(255, 255, 238);
        private Color _colorExtinct = Color.FromArgb(20, 20, 20);
        private Color _colorOccassional = Color.FromArgb(114, 140, 204);
        private Color _colorResident = Color.FromArgb(65, 119, 65);
        private Color _colorUncertain = Color.FromArgb(249, 214, 104);

        // Previous default color values in PrintObs
        /*
        private Color _colorMissing = Color.White;
        private Color _colorUncertain = Color.FromArgb(100, 255, 255);
        private Color _colorOccassional = Color.FromArgb(192, 255, 255);
        private Color _colorExtinct = Color.Yellow;
        private Color _colorResident = Color.Fuchsia;
        */
        private Color _colorBackground = Color.White;
        private Color _colorLegendBackground = Color.White;

        /// <summary>
        /// This method must be called before a Caunty occurrense map objects can be constructed. The purpose is to set the Shape file that will be used as a background map.
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
        /// Constructor of a County occurrece map object. Make sure that the static methos InitializeMap is called before using this constructor.
        /// </summary>
        /// <param name="userContext">
        /// The user Context.
        /// </param>
        /// <param name="taxon">
        /// A taxon object
        /// </param>
        public CountyOccurrenceMap(IUserContext userContext, ITaxon taxon)
        {
            FontName = TEXT_FONT;
            IFactorSearchCriteria factorSearchCrieteria = new FactorSearchCriteria();
            List<Int32> countyIds = new List<Int32>();
            countyIds.Add((Int32)FactorId.CountyOccurrence);
            factorSearchCrieteria.RestrictSearchToFactorIds = countyIds;
            factorSearchCrieteria.RestrictReturnToScope = ArtDatabanken.Data.FactorSearchScope.LeafFactors;
            _counties = CoreData.FactorManager.GetFactors(userContext, factorSearchCrieteria);
            ISpeciesFactSearchCriteria parameters = new SpeciesFactSearchCriteria();
            parameters.Taxa = new TaxonList { taxon };
            parameters.Factors = new FactorList();
            parameters.Factors = _counties;
            parameters.IncludeNotValidHosts = true;
            parameters.IncludeNotValidTaxa = true;
            _countyInformation = CoreData.SpeciesFactManager.GetSpeciesFacts(userContext, parameters);
            UpdateInformation = null;
            if (!_countyInformation.IsNotEmpty())
            {
                _countyInformationExist = false;
            }

        }

        /// <summary>
        /// Constructor of a County occurrece map object.
        /// Make sure that the static methos InitializeMap is called before using this constructor.
        /// </summary>
        /// <param name="countyOccurrences">Information about county occurrence.</param>
        public CountyOccurrenceMap(SpeciesFactList countyOccurrences)
        {
            FontName = TEXT_FONT;
            _countyInformation = countyOccurrences;
            _counties = new FactorList();
            if (_countyInformation.IsNotEmpty())
            {
                foreach (SpeciesFact fact in _countyInformation)
                {
                    _counties.Add(fact.Factor);
                }
            }

            UpdateInformation = null;
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

            //_shapeTransform = new TransformGroup();
            bitmap = new Bitmap(_bitmapWidth + _legendBoxWidth - _legendBoxOffsetX, _bitmapHeight + 2, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics canvas = Graphics.FromImage(bitmap))
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    using (Brush backgroundBrush = new SolidBrush(_colorBackground))
                    {
                        String countyCode = "";
                        int countyOccurenceKey = 0;

                        canvas.FillRectangle(backgroundBrush, 0, 0, _bitmapWidth + _legendBoxWidth - _legendBoxOffsetX, _bitmapHeight + 2);

                        for (int recordIndex = 0; recordIndex < _shapeFile.Records.Count; recordIndex++)
                        {
                            try
                            {

                           
                            ShapeFileRecord record = _shapeFile.Records[recordIndex];

                            if (_countyInformation.IsNotEmpty())
                            {
                                countyCode = record.Attributes[2].ToString();
                                int countyId = getCountyId(countyCode);
                                if (countyId > 0)
                                {
                                    ISpeciesFact fact = null;
                                    SpeciesFactList facts = null;
                                    if (_counties.Exists(countyId))
                                    {
                                        try
                                        {
                                            facts = _countyInformation.GetSpeciesFacts(_counties.Get(countyId));
                                            if (facts.IsNotEmpty())
                                            {
                                                fact = facts[0];
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                       
                                        if (fact.IsNotNull())
                                        {
                                            countyOccurenceKey = fact.MainField.EnumValue.KeyInt.GetValueOrDefault(0);
                                        }
                                        else
                                        {
                                            countyOccurenceKey = 0;
                                        }
                                    }
                                    else
                                    {
                                        countyOccurenceKey = 0;
                                    }
                                }
                            }
                            else
                            {
                                countyOccurenceKey = -1;
                            }

                            for (int i = 0; i < record.NumberOfParts; i++)
                            {
                                List<Point> points = new List<Point>();

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

                                    Point point = new Point(_legendBoxWidth - _legendBoxOffsetX + (int)((pt.X - _xMin) * _scale), _bitmapHeight - (int)((pt.Y - _yMin) * _scale));
                                    points.Add(point);
                                }

                                using (Brush countyOccurrenceBrush = getBrush(countyOccurenceKey))
                                {
                                    canvas.FillPolygon(countyOccurrenceBrush, points.ToArray());
                                }

                                canvas.DrawPolygon(pen, points.ToArray());
                            }
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }

                        if (_countyInformation.IsEmpty())
                        {
                            using (Font font = new Font("Arial", 24 * _bitmapHeight / 708))
                            {
                                using (Brush brush = new SolidBrush(Color.Red))
                                {
                                    canvas.DrawString("Information saknas", font, brush, _legendBoxWidth, _bitmapHeight / 2);
                                }
                            }
                        }

                        if (UpdateInformation.IsNotEmpty())
                        {
                            canvas.DrawImage(GetUpdateInformationBox(),
                                             0,
                                             0);
                            canvas.DrawImage(getLegendBox(), 0, _updateInformationBoxHeight);
                        }
                        else
                        {
                            canvas.DrawImage(getLegendBox(), 0, 0);
                        }
                    }
                }
            }
    
            return bitmap;
        }

        private Brush getBrush(Int32 occurenceKey)
        {
            Brush brush;
            switch (occurenceKey)
            {
                //Missing
                case 0:
                    brush = new SolidBrush(_colorMissing);
                    break;
                //Uncertain
                case 1:
                    brush = new SolidBrush(_colorUncertain);
                    break;
                //Occasional
                case 2:
                    brush = new SolidBrush(_colorOccassional);
                    break;
                //Extinct
                case 3:
                    brush = new SolidBrush(_colorExtinct);
                    break;
                //Resident
                case 4:
                    brush = new SolidBrush(_colorResident);
                    break;
                default:
                    brush = new SolidBrush(_colorBackground);
                    break;
            }

            return brush;
        }

        private String getLegend(Int32 occurenceKey)
        {
            String legend;
            switch (occurenceKey)
            {
                //Missing
                case 0:
                    legend = "Saknas";
                    break;
                //Uncertain
                case 1:
                    legend = "Osäker";
                    break;
                //Occasional
                case 2:
                    legend = "Tillfällig";
                    break;
                //Extinct
                case 3:
                    legend = "Utdöd";
                    break;
                //Resident
                case 4:
                    legend = "Bofast";
                    break;
                default:
                    legend = "";
                    break;
            }

            return legend;
        }

        private Int32 getCountyId(String countyCode)
        {
            Int32 id = 0;
            switch (countyCode)
            {
                case "T":
                    id = 790;
                    break;
                case "E":
                    id = 785;
                    break;
                case "K":
                    id = 777;
                    break;
                case "X":
                    id = 793;
                    break;
                case "N":
                    id = 783;
                    break;
                case "Z":
                    id = 795;
                    break;
                case "Hf":
                    id = 780;
                    break;
                case "W":
                    id = 792;
                    break;
                case "G":
                    id = 781;
                    break;
                case "BD":
                    id = 797;
                    break;
                case "AB":
                    id = 787;
                    break;
                case "D":
                    id = 786;
                    break;
                case "C":
                    id = 788;
                    break;
                case "S":
                    id = 791;
                    break;
                case "AC":
                    id = 796;
                    break;
                case "Y":
                    id = 794;
                    break;
                case "U":
                    id = 789;
                    break;
                case "M":
                    id = 776;
                    break;
                case "O":
                    id = 784;
                    break;
                case "F":
                    id = 782;
                    break;
                case "I":
                    id = 778;
                    break;
                case "Hö":
                    id = 779;
                    break;
                case "H÷":
                    id = 779;
                    break;
                default:
                    id = -1;
                    break;
            }
            return id;
        }

        /// <summary>
        /// The color of the background.
        /// </summary>
        public Color BackgroundColor
        {
            get { return _colorBackground;}
            set { _colorBackground = value;}
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
        /// The color of reprecetning the county occurrence status "Missing".
        /// </summary>
        public Color MissingColor
        {
            get { return _colorMissing; }
            set { _colorMissing = value; }
        }

        /// <summary>
        /// The color of reprecetning the county occurrence status "Uncertain".
        /// </summary>
        public Color UncertainColor
        {
            get { return _colorUncertain; }
            set { _colorUncertain = value; }
        }

        /// <summary>
        /// The color of reprecetning the county occurrence status "Occassional".
        /// </summary>
        public Color OccassionalColor
        {
            get { return _colorOccassional; }
            set { _colorOccassional = value; }
        }

        /// <summary>
        /// The color of reprecetning the county occurrence status "Extinct".
        /// </summary>
        public Color ExtinctColor
        {
            get { return _colorExtinct; }
            set { _colorExtinct = value; }
        }

        /// <summary>
        /// The color of reprecetning the county occurrence status "Resident".
        /// </summary>
        public Color ResidentColor
        {
            get { return _colorResident; }
            set { _colorResident = value; }
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
        /// The bitmap representing the county occurence map.
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
        /// Returns true if county information exist.
        /// </summary>
        public bool CountyInformationExist
        {
            get
            {
                return _countyInformationExist;
            }
        }

        /// <summary>
        /// Name of the font that is used to draw text in the
        /// county occurrence image.
        /// </summary>
        public String FontName
        { get; set; }

        /// <summary>
        /// Offests the x-position of the legend box 
        /// in pixels realative to orginal with
        /// </summary>
        public Int32 LegendOffsetX
        {
            get { return _legendBoxOffsetX; }
            set { _legendBoxOffsetX = value; }
        }

        /// <summary>
        /// Show information about data update in the map.
        /// </summary>
        public String UpdateInformation
        { get; set; }

        /// <summary>
        /// This method saves the County occurrence map to a file.
        /// </summary>
        /// <param name="fileName">Filename of the County occurence map, including full path</param>
        /// <param name="fileFormat">The file format of the raster file</param>
        public void Save(String fileName, System.Drawing.Imaging.ImageFormat fileFormat)
        {
            _map = getBitmap();
            _map.Save(fileName, fileFormat);
        }

        private Bitmap GetUpdateInformationBox()
        {
            Bitmap bitmap;
            Int32 bitmapHeight, bitmapWidth;

            bitmapWidth = _updateInformationBoxWidth;
            bitmapHeight = _updateInformationBoxHeight;
            bitmap = new Bitmap(bitmapWidth, bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics canvas = Graphics.FromImage(bitmap))
            {
                using (Brush blackBrush = new SolidBrush(Color.Black))
                {
                    using (Font font = new Font(FontName, 12, FontStyle.Regular, GraphicsUnit.Pixel))
                    {
                        canvas.DrawString(UpdateInformation, font, blackBrush, 0, 2);
                    }
                }
            }

            return bitmap;
        }

        private Bitmap getLegendBox()
        {
            Bitmap bitmap;
            int bitmapHeight, bitmapWidth, padding, top;

            bitmapWidth = _legendBoxWidth;
            bitmapHeight = 80;
            bitmap = new Bitmap(bitmapWidth, bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics canvas = Graphics.FromImage(bitmap))
            {
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    using (Brush backgroundBrush = new SolidBrush(_colorLegendBackground))
                    {
                        using (Brush blackBrush = new SolidBrush(Color.Black))
                        {
                            canvas.FillRectangle(backgroundBrush, 0, 0, bitmapWidth, bitmapHeight);
                            top = 0;
                            padding = 5;
                            using (Font font = new Font(FontName, 12, FontStyle.Regular, GraphicsUnit.Pixel))
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    top = top + padding;
                                    using (Brush countyOccurrenceBrush = getBrush(i))
                                    {
                                        canvas.FillRectangle(countyOccurrenceBrush, 2, top, 20, 10);
                                    }
                                    canvas.DrawRectangle(pen, 2, top, 20, 10);

                                    canvas.DrawString(getLegend(i), font, blackBrush, 24, top - 2);
                                    top = top + 10;
                                }
                            }
                        }
                    }
                }
            }

            return bitmap;
        }
    }
}
