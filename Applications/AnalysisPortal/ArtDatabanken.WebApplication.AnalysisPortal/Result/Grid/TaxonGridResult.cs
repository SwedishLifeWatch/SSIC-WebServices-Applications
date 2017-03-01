using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.GIS.GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.Grid
{
    /// <summary>
    /// This class contains properties for a Grid statistics result
    /// </summary>
    [DataContract]
    public class TaxonGridResult
    {
        [DataMember]
        public int GridCellCoordinateSystemId { get; set; }
        
        [DataMember]
        public string GridCellCoordinateSystem { get; set; }
        
        [DataMember]
        public int GridCellSize { get; set; }

        [DataMember]
        public List<TaxonGridCellResult> Cells { get; set; }
        
        /// <summary>
        /// Creates a Grid statistics result object.
        /// </summary>
        /// <param name="cells">Grid statistics cell list returned from web service.</param>
        /// <returns></returns>
        public static TaxonGridResult Create(IList<IGridCellSpeciesCount> cells)
        {
            var model = new TaxonGridResult();
            model.Cells = new List<TaxonGridCellResult>();
            model.GridCellCoordinateSystem = "";
            
            if (cells == null || cells.Count == 0)
            {
                return model;
            }

            model.GridCellSize = cells[0].GridCellSize;
            model.GridCellCoordinateSystemId = (int)cells[0].GridCoordinateSystem;
            model.GridCellCoordinateSystem = cells[0].GridCoordinateSystem.ToString();
            foreach (GridCellSpeciesCount gridCell in cells)
            {
                model.Cells.Add(TaxonGridCellResult.Create(gridCell));
            }
            return model;
        }
    }
}
