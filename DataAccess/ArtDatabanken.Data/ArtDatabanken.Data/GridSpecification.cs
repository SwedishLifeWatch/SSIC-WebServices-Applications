using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    
    /// <summary>
    /// This class holds requested grid information reagaring
    /// gridcell size, coordinat system and bounding box.
    /// </summary>
    public class GridSpecification: IGridSpecification
    {
        
        /// <summary>
        /// Property holding bounding box information.
        /// Only two-dimensional bounding boxes (rectangles) are handled.
        /// </summary>
        public IBoundingBox BoundingBox
        { get; set; }

        /// <summary>
        /// Property holding information om size of the grid cell to be
        /// handeled.
        /// </summary>
         public Int32 GridCellSize
        { get; set; }

         /// <summary>
         /// Enum property contains information about the grid cell coordinate
         /// system that is provided or requested. Only grid cell coordinate systems that are supported
         /// by ArtDatabanken are avaliable.
         /// </summary>
         public GridCoordinateSystem GridCoordinateSystem
        { get; set; }

         /// <summary>
         /// Property holding information if size of the grid cell is set.
         /// </summary>
         public bool IsGridCellSizeSpecified
         { get; set; }

         /// <summary>
         /// Property holding information about what kind of  value should be returned.
         /// </summary> 
         public GridCellGeometryType GridCellGeometryType
         { get; set; }
         
    }

    
}
