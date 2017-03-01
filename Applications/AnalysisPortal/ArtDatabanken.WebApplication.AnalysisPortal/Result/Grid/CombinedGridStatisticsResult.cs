using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    public class CombinedGridStatisticsResult
    {
        public int DisplayCoordinateSystemId { get; set; }
        public string DisplayCoordinateSystemName { get; set; }

        public int CalculationCoordinateSystemId { get; set; }
        public string CalculationCoordinateSystemName { get; set; }

        public int GridCellSize { get; set; }
        public List<CombinedGridStatisticsCellResult> Cells { get; set; }

        /// <summary>
        /// Creates a Grid statistics result object.
        /// </summary>
        /// <param name="calculationCoordinateSystemId">The calculation coordinate system id.</param>
        /// <param name="displayCoordinateSystemId">The display coordinate system id.</param>
        /// <param name="gridCellSize">Size of the grid cell.</param>
        /// <param name="cells">Grid statistics cell list returned from web service.</param>
        /// <returns></returns>
        public static CombinedGridStatisticsResult Create(
            CoordinateSystemId calculationCoordinateSystemId, 
            CoordinateSystemId displayCoordinateSystemId, 
            int gridCellSize,
            IList<IGridCellCombinedStatistics> cells)
        {
            var model = new CombinedGridStatisticsResult();
            model.Cells = new List<CombinedGridStatisticsCellResult>();
            model.CalculationCoordinateSystemId = (int)displayCoordinateSystemId;
            model.CalculationCoordinateSystemName = CoordinateSystemHelper.GetCoordinateSystemName(calculationCoordinateSystemId);
            model.DisplayCoordinateSystemId = (int)calculationCoordinateSystemId;
            model.DisplayCoordinateSystemName = CoordinateSystemHelper.GetCoordinateSystemName(displayCoordinateSystemId);
            model.GridCellSize = gridCellSize;

            if (cells == null || cells.Count == 0)
            {
                return model;
            }
            //model.GridCellSize = cells[0].GridCellSize;
            //model.GridCellCoordinateSystemId = (int)cells[0].GridCoordinateSystem;
            //model.GridCellCoordinateSystem = cells[0].GridCoordinateSystem.ToString();
            foreach (IGridCellCombinedStatistics gridCell in cells)
            {
                model.Cells.Add(CombinedGridStatisticsCellResult.Create(gridCell));
            }
            return model;
        }
    }
}
