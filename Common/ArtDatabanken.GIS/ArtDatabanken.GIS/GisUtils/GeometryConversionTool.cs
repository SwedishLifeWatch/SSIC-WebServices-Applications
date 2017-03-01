using System.Collections.Generic;
using System.Data.SqlTypes;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.Extensions;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Microsoft.SqlServer.Types;
using Point = ArtDatabanken.Data.Point;

namespace ArtDatabanken.GIS.GisUtils
{
    /// <summary>
    /// This class contains methods for converting Geometries.
    /// </summary>
    public class GeometryConversionTool
    {
        /// <summary>
        /// Convert Polygons to SQL geometry.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The converted polygon.</returns>
        public SqlGeometry PolygonToSqlGeometry(IPolygon polygon)
        {
            return SqlGeometry.Parse(new SqlString(polygon.GetWkt()));            
        }

        /// <summary>
        /// Converts a feature to a SqlGeometries.
        /// If the Geometry type is Polygon => 1 converted polygon is returned.
        /// If the Geometry type is MultiPolygon => 2 or more polygons is returned.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>List of SqlGeometry.</returns>        
        public List<SqlGeometry> FeatureGeometryToSqlGeometries(Feature feature)
        {
            List<SqlGeometry> sqlGeometries = new List<SqlGeometry>();
            if (feature.Geometry != null && feature.Geometry.GetType() == typeof(GeoJSON.Net.Geometry.Polygon))
            {
                GeoJSON.Net.Geometry.Polygon polygon = (GeoJSON.Net.Geometry.Polygon)feature.Geometry;
                SqlGeometry sqlGeometry = SqlGeometry.Parse(new SqlString(polygon.GetWkt()));
                sqlGeometries.Add(sqlGeometry);                
            }
            else if (feature.Geometry != null && feature.Geometry.GetType() == typeof(MultiPolygon))
            {
                MultiPolygon multiPolygon = (MultiPolygon)feature.Geometry;
                foreach (GeoJSON.Net.Geometry.Polygon polygon in multiPolygon.Coordinates)
                {
                    SqlGeometry sqlGeometry = SqlGeometry.Parse(new SqlString(polygon.GetWkt()));
                    sqlGeometries.Add(sqlGeometry);                    
                }
            }
            
            return sqlGeometries;
        }
    }
}
