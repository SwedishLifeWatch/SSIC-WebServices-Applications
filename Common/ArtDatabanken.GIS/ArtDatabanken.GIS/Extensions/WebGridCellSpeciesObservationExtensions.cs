using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.WebService.Data;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Triangulate;

namespace ArtDatabanken.GIS.Extensions
{
    /// <summary>
    /// Extensions for WebGridCellSpeciesObservation class
    /// </summary>
    public static class WebGridCellSpeciesObservationExtensions
    {
        /// <summary>
        /// Get the convex hull for a list of grid cells
        /// </summary>
        /// <param name="gridCells"></param>
        /// <returns></returns>
        public static IGeometry ConvexHull(this List<WebGridCellSpeciesObservationCount> gridCells)
        {
            if (gridCells == null || !gridCells.Any())
            {
                return null;
            }

            var polygons = (from c in gridCells select c.OrginalBoundingBox.ToPolygon()).ToArray();

            return new MultiPolygon(polygons).ConvexHull();
        }

        /// <summary>
        /// Calculte concave hull for grid cells
        /// </summary>
        /// <param name="gridCells"></param>
        /// <param name="alphaValue"></param>
        /// <param name="useCenterPoint">Used when concave hull is calculated. Grid corner coordinates used when false</param>
        /// <returns></returns>
        public static IGeometry ConcaveHull(this List<WebGridCellSpeciesObservationCount> gridCells, double alphaValue, bool useCenterPoint)
        {
            if (gridCells == null || gridCells.Count < 3)
            {
                return null;
            }

            IPoint[] points;
            if (useCenterPoint)
            {
                //Create a geometry with all grid cell points, this is much faster than using the gridcells because it's less coordinates, the generated geometry will also look better
                points = (from c in gridCells select c.OrginalCentreCoordinate.ToPoint()).ToArray();
            }
            else
            {
                points = new IPoint[gridCells.Count * 4];

                for (var i = 0; i < gridCells.Count; i++)
                {
                    var gridCell = gridCells[i];
                    var boundingBox = gridCell.OrginalBoundingBox;
                    
                    if (boundingBox == null)
                    {
                        continue;
                    }
               
                    var startIndex = i * 4;
                    points[startIndex] = new Point(new Coordinate(boundingBox.Min.X, boundingBox.Min.Y));
                    points[startIndex + 1] = new Point(new Coordinate(boundingBox.Min.X, boundingBox.Max.Y));
                    points[startIndex + 2] = new Point(new Coordinate(boundingBox.Max.X, boundingBox.Max.Y));
                    points[startIndex + 3] = new Point(new Coordinate(boundingBox.Max.X, boundingBox.Min.Y));
                } 
            }
            
            //Triangulate all points
            var triangulationBuilder = new ConformingDelaunayTriangulationBuilder();
            triangulationBuilder.SetSites(new MultiPoint(points));

            var geometryFactory = new GeometryFactory();
            var triangles = triangulationBuilder.GetTriangles(geometryFactory);
            
            IGeometry alphaGeometry = null;
            for (var i = 0; i < triangles.Count; i++)
            {
                var triangle = triangles[i];
                var radius = triangle.Radius() / 1000; //Div radius by 1000 to (commonly) keep alpha values in a range of 1 - 1000

                if (radius < alphaValue)
                {
                    alphaGeometry = alphaGeometry == null ? triangle : alphaGeometry.Union(triangle);
                }
            }

            return alphaGeometry;
        }
    }
}
