using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    /// <summary>
    /// This class contains properties for a Grid statistics result
    /// </summary>
    [DataContract]
    public class SpeciesObservationGridResult
    {
        [DataMember]
        public int GridCellCoordinateSystemId { get; set; }
        
        [DataMember]
        public string GridCellCoordinateSystem { get; set; }
        
        [DataMember]
        public int GridCellSize { get; set; }

        [DataMember]
        public List<SpeciesObservationGridCellResult> Cells { get; set; }
        
        /// <summary>
        /// Creates a Grid statistics result object.
        /// </summary>
        /// <param name="cells">Grid statistics cell list returned from web service.</param>
        /// <returns></returns>
        public static SpeciesObservationGridResult Create(IList<IGridCellSpeciesObservationCount> cells)
        {
            var model = new SpeciesObservationGridResult();
            model.Cells = new List<SpeciesObservationGridCellResult>();
            model.GridCellCoordinateSystem = "";
            
            if (cells == null || cells.Count == 0)
            {
                return model;
            }

            model.GridCellSize = cells[0].GridCellSize;
            model.GridCellCoordinateSystemId = (int)cells[0].GridCoordinateSystem;
            model.GridCellCoordinateSystem = cells[0].GridCoordinateSystem.ToString();
            foreach (GridCellSpeciesObservationCount gridCell in cells)
            {
                model.Cells.Add(SpeciesObservationGridCellResult.Create(gridCell));
            }
            return model;
        }
    }
}
