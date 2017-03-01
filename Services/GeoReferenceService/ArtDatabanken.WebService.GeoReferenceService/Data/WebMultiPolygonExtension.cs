using System;
using ArtDatabanken.Database;
using Microsoft.SqlServer.Types;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebMultiPolygon class.
    /// </summary>
    public static class WebMultiPolygonExtension
    {
        /// <summary>
        /// Load data into the WebMultiPolygon instance.
        /// </summary>
        /// <param name="multiPolygon">This multi polygon.</param>
        /// <param name='dataReader'>An open data reader.</param>
        /// <param name='columnName'>Name of the column in result set to read data from.</param>
        public static void LoadData(this WebMultiPolygon multiPolygon,
                                    DataReader dataReader,
                                    String columnName)
        {
            SqlGeometry multiPolygonGeometry;

            multiPolygonGeometry = dataReader.GetSqlGeometry(columnName);
            multiPolygon.Polygons = multiPolygonGeometry.GetMultiPolygon().Polygons;
        }
    }
}
