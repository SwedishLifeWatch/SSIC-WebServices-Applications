using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for handling grid specifications.
    /// </summary>
    public interface IGridSpecification
    {
        /// <summary>
        /// Property holding bounding box information.
        /// Only two-dimensional bounding boxes (rectangles) are handled.
        /// </summary>
        IBoundingBox BoundingBox
        { get; set; }

        /// <summary>
        /// Property holding information if size of the grid cell is set.
        /// </summary>
        bool IsGridCellSizeSpecified
        { get; set; }

        /// <summary>
        /// Property holding information om size of the grid cell to be
        /// handeled.
        /// </summary>
        Int32 GridCellSize
        { get; set; }

        /// <summary>
        /// Enum property contains information about the grid cell coordinate
        /// system that is provided or requested. Only grid cell coordinate systems that are supported
        /// by ArtDatabanken are avaliable.
        /// </summary>
        GridCoordinateSystem GridCoordinateSystem
        { get; set; }

        /// <summary>
        /// Property holding information about what kind of value should be returned.
        /// </summary>
       GridCellGeometryType GridCellGeometryType
        { get; set; }

    }
}