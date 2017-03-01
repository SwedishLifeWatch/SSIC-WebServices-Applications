using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    /// <summary>
    /// This class contains properties for a Grid statistics result
    /// </summary>
    [DataContract]
    public class WfsStatisticsGridResult
    {
        [DataMember]
        public int GridCellCoordinateSystemId { get; set; }
        
        [DataMember]
        public string GridCellCoordinateSystem { get; set; }
        
        [DataMember]
        public int GridCellSize { get; set; }

        [DataMember]
        public List<WfsStatisticsGridCellResult> Cells { get; set; }
        
        /// <summary>
        /// Creates a Grid statistics result object.
        /// </summary>
        /// <param name="cells">Grid statistics cell list returned from web service.</param>
        /// <returns></returns>
        public static WfsStatisticsGridResult Create(List<IGridCellFeatureStatistics> cells)
        {
            var model = new WfsStatisticsGridResult();
            model.Cells = new List<WfsStatisticsGridCellResult>();
            model.GridCellCoordinateSystem = "";
            
            if (cells == null || cells.Count == 0)
            {
                return model;
            }

            model.GridCellSize = cells[0].GridCellSize;
            model.GridCellCoordinateSystemId = (int)cells[0].GridCoordinateSystem;
            model.GridCellCoordinateSystem = cells[0].GridCoordinateSystem.ToString();
            foreach (IGridCellFeatureStatistics gridCell in cells)
            {
                model.Cells.Add(WfsStatisticsGridCellResult.Create(gridCell));
            }
            return model;
        }
    }
}
