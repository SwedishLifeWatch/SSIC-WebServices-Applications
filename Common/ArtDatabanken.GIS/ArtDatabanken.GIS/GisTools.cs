using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.GIS.CoordinateConversion;
using ArtDatabanken.GIS.GeoJSON.Net;
using ArtDatabanken.GIS.GisUtils;
using ArtDatabanken.GIS.Grid;

namespace ArtDatabanken.GIS
{
    /// <summary>
    /// This class contains other classes that manages
    /// different types of GIS data.    
    /// </summary>
    public static class GisTools
    {
        /// <summary>
        /// Gets or sets the coordinate conversion manager instance.
        /// </summary>        
        public static CoordinateConversionManager CoordinateConversionManager { get; set; }

        /// <summary>
        /// Gets or sets the grid cell manager instance.
        /// </summary>        
        public static GridCellManager GridCellManager { get; set; }

        /// <summary>
        /// Gets or sets the GeoJson utils instance.
        /// </summary>        
        public static GeoJsonUtils GeoJsonUtils { get; set; }

        /// <summary>
        /// Gets or sets the geometry tools instance.
        /// </summary>        
        public static GeometryTools GeometryTools { get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="GisTools"/> class. 
        /// </summary>
        static GisTools()
        {
            CoordinateConversionManager = new CoordinateConversionManager();
            GridCellManager = new GridCellManager();
            GeoJsonUtils = new GeoJsonUtils();
            GeometryTools = new GeometryTools();
        }
    }
}
