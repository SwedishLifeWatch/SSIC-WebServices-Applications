using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// File types that can be generated in Analysis portal.
    /// </summary>
    public enum FileType
    {
        Unknown = 0,
        ExcelXlsx = 1,
        ExcelXml = 2,
        Png = 3,
        GeoJSON = 4,
        JSON = 5,
        Csv = 6,
        Tiff = 7,
        Zip = 8
    }
}
