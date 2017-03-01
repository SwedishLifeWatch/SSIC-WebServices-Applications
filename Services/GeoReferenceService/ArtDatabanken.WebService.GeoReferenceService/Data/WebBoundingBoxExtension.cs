using System;
using System.Globalization;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebBoundingBox class.
    /// </summary>
    public static class WebBoundingBoxExtension
    {
        /// <summary>
        /// Load data into the WebBoundingBox instance.
        /// </summary>
        /// <param name="boundingBox">This boundingbox.</param>
        /// <param name='dataReader'>An open data reader.</param>
        /// <param name='columnName'>Name of the column in result set to read data from.</param>
        public static void LoadData(this WebBoundingBox boundingBox,
                                    DataReader dataReader,
                                    String columnName)
        {
            char[] delimiter;
            String boundingBoxString;
            String[] coordinates;

            // Get coordinates.
            boundingBoxString = dataReader.GetString(columnName);
            delimiter = new[] { Settings.Default.CoordinateDelimiter };
            coordinates = boundingBoxString.Split(delimiter);
            if (coordinates.Length != 4)
            {
                throw new ApplicationException("Wrong format in bounding box: " + boundingBox);
            }

            // Create points.
            boundingBox.Min = new WebPoint();
            boundingBox.Max = new WebPoint();
            boundingBox.Min.X = Double.Parse(coordinates[0], NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));
            boundingBox.Min.Y = Double.Parse(coordinates[1], NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));
            boundingBox.Max.X = Double.Parse(coordinates[2], NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));
            boundingBox.Max.Y = Double.Parse(coordinates[3], NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));
        }
    }
}
