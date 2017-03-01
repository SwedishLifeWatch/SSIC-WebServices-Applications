using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.SwedenExtent;

namespace ArtDatabanken.GIS.Grid
{
    /// <summary>
    /// This class handles creation of grid cells.
    /// </summary>
    public class GridCellManager
    {
        /// <summary>
        /// Calculates the number of grid cells that will be generated.
        /// If no bounding box is specified in gridSpecification, 
        /// the sweden extent will be used.
        /// </summary>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <returns>The number of grid cells that will be generated.</returns>
        public long CalculateNumberOfGridCells(GridSpecification gridSpecification)
        {
            if (gridSpecification.GridCellSize <= 0)
            {
                throw new ArgumentException("GridCellSize must be greater than 0.");
            }

            IBoundingBox boundingBox = gridSpecification.BoundingBox;
            if (boundingBox == null)
            {
                boundingBox = SwedenExtentManager.GetSwedenExtentBoundingBox(gridSpecification.GridCoordinateSystem.ToCoordinateSystem());
            }

            double xMin, yMin, xMax, yMax;   
            int cellSize = gridSpecification.GridCellSize;
            xMin = Math.Floor(boundingBox.Min.X / cellSize) * cellSize;
            xMax = Math.Ceiling(boundingBox.Max.X / cellSize) * cellSize;
            yMin = Math.Floor(boundingBox.Min.Y / cellSize) * cellSize;
            yMax = Math.Ceiling(boundingBox.Max.Y / cellSize) * cellSize;
            
            long count = (long)(((xMax - xMin) / cellSize) * ((yMax - yMin) / cellSize));            
            return count;
        }


        /// <summary>
        /// Generates grid cells according to the gridSpecification.
        /// </summary>
        /// <param name="gridSpecification">The grid specification.</param>
        /// <param name="displayCoordinateSystem">The display coordinate system.</param>
        /// <returns>A list of grid cells according to the grid specification.</returns>        
        public List<GridCellBase> GenerateGrid(GridSpecification gridSpecification, ICoordinateSystem displayCoordinateSystem)
        {
            if (gridSpecification.GridCellSize <= 0)
            {
                throw new ArgumentException("GridCellSize must be greater than 0.");
            }

            List<GridCellBase> gridCells = new List<GridCellBase>();

            IBoundingBox boundingBox = gridSpecification.BoundingBox;
            if (boundingBox == null)
            {
                boundingBox = SwedenExtentManager.GetSwedenExtentBoundingBox(gridSpecification.GridCoordinateSystem.ToCoordinateSystem());
            }
            
            double xMin, yMin, xMax, yMax;
            int cellSize;

            cellSize = gridSpecification.GridCellSize;            

            xMin = Math.Floor(boundingBox.Min.X / cellSize) * cellSize;
            xMax = Math.Ceiling(boundingBox.Max.X / cellSize) * cellSize;
            yMin = Math.Floor(boundingBox.Min.Y / cellSize) * cellSize;
            yMax = Math.Ceiling(boundingBox.Max.Y / cellSize) * cellSize;            

            ICoordinateSystem fromCoordinateSystem = gridSpecification.GridCoordinateSystem.ToCoordinateSystem();
            ICoordinateSystem toCoordinateSystem = displayCoordinateSystem;

            // The last grid cell may exceed the boundary box.
            while (xMin < xMax)
            {
                // The last grid cell may exceed the boundary box.
                while (yMin < yMax)
                {
                    GridCellBase gridCell = new GridCellBase();
                    gridCell.GridCoordinateSystem = gridSpecification.GridCoordinateSystem;
                    gridCell.GridCellSize = gridSpecification.GridCellSize;
                    gridCell.OrginalGridCellBoundingBox = new BoundingBox();
                    gridCell.OrginalGridCellBoundingBox.Min = new Point(xMin, yMin);
                    gridCell.OrginalGridCellBoundingBox.Max = new Point(xMin + cellSize, yMin + cellSize);
                    gridCell.OrginalGridCellCentreCoordinate = new Point(Math.Floor(xMin / cellSize) * cellSize + cellSize * 0.5, Math.Floor(yMin / cellSize) * cellSize + cellSize * 0.5);

                    gridCell.GridCellBoundingBox = GisTools.CoordinateConversionManager.GetConvertedBoundingBox(gridCell.OrginalGridCellBoundingBox, fromCoordinateSystem, toCoordinateSystem);
                    gridCell.CoordinateSystem = displayCoordinateSystem;
                    gridCell.GridCellCentreCoordinate = GisTools.CoordinateConversionManager.GetConvertedPoint(gridCell.OrginalGridCellCentreCoordinate, fromCoordinateSystem, toCoordinateSystem);

                    gridCells.Add(gridCell);
                    yMin = yMin + cellSize;
                }

                xMin = xMin + cellSize;
                yMin = Math.Floor(boundingBox.Min.Y / cellSize) * cellSize;
            }

            return gridCells;
        }        


        /// <summary>
        /// Gets the missing grid cells by subtracting 
        /// <paramref name="subtractGridCells"/> from <paramref name="allGridCells"/>.
        /// </summary>
        /// <param name="allGridCells">All grid cells. E.g. grid cells that cover entire Sweden.</param>
        /// <param name="subtractGridCells">The grid cells to subtract. E.g. grid cells in that cover Dalarna.</param>
        /// <returns>All grid cells in <paramref name="allGridCells"/> minus <paramref name="subtractGridCells"/>.</returns>
        public List<IGridCellBase> GetMissingGridCells(List<IGridCellBase> allGridCells, List<IGridCellBase> subtractGridCells)
        {
            HashSet<IGridCellBase> allGridCellsHashSet = new HashSet<IGridCellBase>(new GridCellBaseCenterPointComparer());
            foreach (IGridCellBase gridCell in allGridCells)
            {
                allGridCellsHashSet.Add(gridCell);
            }

            HashSet<IGridCellBase> gridCellsHashSet = new HashSet<IGridCellBase>(new GridCellBaseCenterPointComparer());
            foreach (IGridCellBase gridCell in subtractGridCells)
            {
                gridCellsHashSet.Add(gridCell);
            }

            allGridCellsHashSet.ExceptWith(gridCellsHashSet);
            return allGridCellsHashSet.ToList();            
        }
        
        /// <summary>
        /// Calculates the number of gridcells
        /// </summary>
        /// <param name="gridSpecification"></param>
        /// <param name="polygons"></param>
        /// <param name="regionGuids"></param>
        /// <returns></returns>
        public long CalculateNumberOfGridCells(GridSpecification gridSpecification, List<IPolygon> polygons, List<string> regionGuids)
        {
            throw new NotImplementedException();
        }
    }
}
