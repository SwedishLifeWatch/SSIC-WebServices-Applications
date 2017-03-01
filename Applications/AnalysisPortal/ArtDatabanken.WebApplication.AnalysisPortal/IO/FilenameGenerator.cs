using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// Filename generator.
    /// </summary>
    public static class FilenameGenerator
    {
        /// <summary>
        /// Creates a filename.
        /// </summary>
        /// <param name="name">The name part of the filename.</param>
        /// <param name="fileType">The file type.</param>
        /// <returns>A valid filename.</returns>
        public static string CreateFilename(string name, FileType fileType)
        {
            return CreateFilename(name, fileType, DateTime.Now);
        }

        /// <summary>
        /// Creates a filename.
        /// </summary>
        /// <param name="name">The name part of the filename.</param>
        /// <param name="fileType">The file type.</param>
        /// <param name="date">The date that will be used to generate the date part of the filename.</param>
        /// <returns>A valid filename.</returns>
        public static string CreateFilename(string name, FileType fileType, DateTime date)
        {
            string dateFilenamePart = GenerateDateFilenamePart(date);
            string fileExtension = GetFileExtension(fileType);
            return string.Format("{0}_{1}.{2}", name, dateFilenamePart, fileExtension);
        }

        /// <summary>
        /// Generates the date filename part.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>Date part in a filename.</returns>
        private static string GenerateDateFilenamePart(DateTime date)
        {
            return string.Format("{0:yyyyMMdd}_{1:00}h{2:00}m{3:00}s", date, date.Hour, date.Minute, date.Second);
        }

        /// <summary>
        /// Gets the file extension for given file type.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <returns>File extension for given file type.</returns>        
        private static string GetFileExtension(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.ExcelXlsx:
                    return "xlsx";
                case FileType.ExcelXml:
                    return "xml";
                case FileType.Png:
                    return "png";
                case FileType.GeoJSON:
                    return "geojson";
                case FileType.JSON:
                    return "json";
                case FileType.Csv:
                    return "csv";
                case FileType.Tiff:
                    return "tif";
                case FileType.Zip:
                    return "zip";
                case FileType.Unknown:
                    throw new ArgumentException("GetFileExtension(). Unknown file format");
                default:
                    throw new ArgumentException(string.Format("GetFileExtension(). File format: {0} not handled", fileType));
            }
        }            
    }
}
