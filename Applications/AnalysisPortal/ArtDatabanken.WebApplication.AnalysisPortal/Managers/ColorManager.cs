using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    public static class ColorExtension
    {
        public static string ToHexString(this Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }

    public static class ColorManager
    {
        private static readonly Random Random = new Random();
        private static readonly List<Color> Colors = new List<Color>();

        /// <summary>
        /// Initializes the <see cref="ColorManager"/> class.
        /// </summary>
        static ColorManager()
        {
            Colors = new List<Color>();
            Colors.Add(ColorFromHtml("#99CCFF"));
            Colors.Add(ColorFromHtml("#CCFFCC"));
            Colors.Add(ColorFromHtml("#FFFF99"));
            Colors.Add(ColorFromHtml("#CC99CC"));
            Colors.Add(ColorFromHtml("#CCFFFF"));
            Colors.Add(ColorFromHtml("#FF9999"));
            Colors.Add(ColorFromHtml("#FF9666"));
            Colors.Add(ColorFromHtml("#CCCCCC"));
            Colors.Add(ColorFromHtml("#CCCCFF"));
            Colors.Add(ColorFromHtml("#99CC99"));
            Colors.Add(ColorFromHtml("#FFCCCC"));
            Colors.Add(ColorFromHtml("#CC99FF"));
            Colors.Add(ColorFromHtml("#FFCC99"));
            Colors.Add(ColorFromHtml("#99CCCC"));
            Colors.Add(ColorFromHtml("#9999FF"));
            Colors.Add(ColorFromHtml("#FFCC66"));
            Colors.Add(ColorFromHtml("#CC9999"));
            Colors.Add(ColorFromHtml("#99FFCC"));
            Colors.Add(ColorFromHtml("#66CCFF"));
            Colors.Add(ColorFromHtml("#CCCCFF"));
        }

        public static Color ColorFromHtml(string strHexColor)
        {
            object col = ColorConverter.ConvertFromString(strHexColor);
            if (col != null)
            {
                var color = (Color)col;                
                return color;
            }
            throw new ArgumentException("Argument isn't a hex color");
        }

        public static Color GetColor(int index)
        {
            return Colors[index];
        }

        public static Color GetRandomColor()
        {
            int index = Random.Next(Colors.Count - 1);
            return Colors[index];
        }

        public static Color GetNextUnusedColor(List<Color> usedColors)
        {
            foreach (Color color in Colors)
            {
                if (usedColors.All(usedColor => color != usedColor))
                {
                    return color;
                }
            }
            return Colors[0];
        }

        public static Color GetNextUnusedColor(List<string> usedColors)
        {
            if (usedColors == null)
            {
                return Colors[0];
            }

            var colors = new List<Color>();
            foreach (string strUsedColor in usedColors)
            {
                object col = ColorConverter.ConvertFromString(strUsedColor);
                if (col != null)
                {                    
                    var color = (Color)col;
                    colors.Add(color);                    
                }
            }
            return GetNextUnusedColor(colors);            
        }

        //public static void Convert()
        //{
        //    string colorcode = "#FFFFFF00";
        //    int argb = Int32.Parse(colorcode.Replace("#", ""), NumberStyles.HexNumber);
        //    Color clr = Color.FromArgb(argb);
        //}

        //public static void Test()
        //{
            
        //    var c = (Color)ColorConverter.ConvertFromString("#FFDFD991");    

        //    System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml("#FFCC66");
        //}

        ////public static Color Color = Color.FromRgb()
        //Color colour = System.Drawing.ColorTranslator.FromHtml(hexValue);
    }
}
