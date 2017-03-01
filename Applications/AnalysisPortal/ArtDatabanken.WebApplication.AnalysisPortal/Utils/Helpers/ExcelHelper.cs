using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    /// <summary>
    /// Helper class for handling Excel files.
    /// </summary>
    public static class ExcelHelper
    {
        private static Dictionary<int, Color> _colorTable;

        /// <summary>
        /// Indexed color table that is similar to the indexed color mode in Excel 2003.
        /// </summary>
        /// <value>
        /// The color table.
        /// </value>
        public static Dictionary<int, Color> ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new Dictionary<int, Color>();
                    _colorTable.Add(0, Color.FromArgb(255, 255, 255));
                    _colorTable.Add(1, Color.FromArgb(0, 0, 0));
                    _colorTable.Add(2, Color.FromArgb(255, 255, 255));
                    _colorTable.Add(3, Color.FromArgb(255, 0, 0));
                    _colorTable.Add(4, Color.FromArgb(0, 255, 0));
                    _colorTable.Add(5, Color.FromArgb(0, 0, 255));
                    _colorTable.Add(6, Color.FromArgb(255, 255, 0));
                    _colorTable.Add(7, Color.FromArgb(255, 0, 255));
                    _colorTable.Add(8, Color.FromArgb(0, 255, 255));
                    _colorTable.Add(9, Color.FromArgb(128, 0, 0));
                    _colorTable.Add(10, Color.FromArgb(0, 128, 0));
                    _colorTable.Add(11, Color.FromArgb(0, 0, 128));
                    _colorTable.Add(12, Color.FromArgb(128, 128, 0));
                    _colorTable.Add(13, Color.FromArgb(128, 0, 128));
                    _colorTable.Add(14, Color.FromArgb(0, 128, 128));
                    _colorTable.Add(15, Color.FromArgb(192, 192, 192));
                    _colorTable.Add(16, Color.FromArgb(128, 128, 128));
                    _colorTable.Add(17, Color.FromArgb(153, 153, 255));
                    _colorTable.Add(18, Color.FromArgb(153, 51, 102));
                    _colorTable.Add(19, Color.FromArgb(255, 255, 204));
                    _colorTable.Add(20, Color.FromArgb(204, 255, 255));
                    _colorTable.Add(21, Color.FromArgb(102, 0, 102));
                    _colorTable.Add(22, Color.FromArgb(255, 128, 128));
                    _colorTable.Add(23, Color.FromArgb(0, 102, 204));
                    _colorTable.Add(24, Color.FromArgb(204, 204, 255));
                    _colorTable.Add(25, Color.FromArgb(0, 0, 128));
                    _colorTable.Add(26, Color.FromArgb(255, 0, 255));
                    _colorTable.Add(27, Color.FromArgb(255, 255, 0));
                    _colorTable.Add(28, Color.FromArgb(0, 255, 255));
                    _colorTable.Add(29, Color.FromArgb(128, 0, 128));
                    _colorTable.Add(30, Color.FromArgb(128, 0, 0));
                    _colorTable.Add(31, Color.FromArgb(0, 128, 128));
                    _colorTable.Add(32, Color.FromArgb(0, 0, 255));
                    _colorTable.Add(33, Color.FromArgb(0, 204, 255));
                    _colorTable.Add(34, Color.FromArgb(204, 255, 255));
                    _colorTable.Add(35, Color.FromArgb(204, 255, 204));
                    _colorTable.Add(36, Color.FromArgb(255, 255, 153));
                    _colorTable.Add(37, Color.FromArgb(153, 204, 255));
                    _colorTable.Add(38, Color.FromArgb(255, 153, 204));
                    _colorTable.Add(39, Color.FromArgb(204, 153, 255));
                    _colorTable.Add(40, Color.FromArgb(255, 204, 153));
                    _colorTable.Add(41, Color.FromArgb(51, 102, 255));
                    _colorTable.Add(42, Color.FromArgb(51, 204, 204));
                    _colorTable.Add(43, Color.FromArgb(153, 204, 0));
                    _colorTable.Add(44, Color.FromArgb(255, 204, 0));
                    _colorTable.Add(45, Color.FromArgb(255, 153, 0));
                    _colorTable.Add(46, Color.FromArgb(255, 102, 0));
                    _colorTable.Add(47, Color.FromArgb(102, 102, 153));
                    _colorTable.Add(48, Color.FromArgb(150, 150, 150));
                    _colorTable.Add(49, Color.FromArgb(0, 51, 102));
                    _colorTable.Add(50, Color.FromArgb(51, 153, 102));
                    _colorTable.Add(51, Color.FromArgb(0, 51, 0));
                    _colorTable.Add(52, Color.FromArgb(51, 51, 0));
                    _colorTable.Add(53, Color.FromArgb(153, 51, 0));
                    _colorTable.Add(54, Color.FromArgb(153, 51, 102));
                    _colorTable.Add(55, Color.FromArgb(51, 51, 153));
                    _colorTable.Add(56, Color.FromArgb(51, 51, 51));
                    _colorTable.Add(57, Color.FromArgb(79, 129, 189));
                }
                return _colorTable;
            }
        }
    }
}
