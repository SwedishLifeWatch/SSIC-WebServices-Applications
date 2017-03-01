using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class holds grid information regarding
    /// gridcell size, coordinat system and bounding box.
    /// </summary>
    [DataContract]
    public class WebGridSpecification : WebData
    {
        /// <summary>
        /// Property holding bounding box information.
        /// Only two-dimensional bounding boxes (rectangles) are handled
        /// in the first version of AnalysisService.
        /// </summary>
        [DataMember]
        public WebBoundingBox BoundingBox { get; set; }

        /// <summary>
        /// Property holding information about what kind of  value should be returned.
        /// </summary> 
        [DataMember]
        public GridCellGeometryType GridCellGeometryType
        { get; set; }

        /// <summary>
        /// Property holding information about size of the grid cell
        /// to be handled.
        /// </summary>
        [DataMember]
        public Int32 GridCellSize { get; set; }

        /// <summary>
        /// Property contains information about coordinate
        /// system that is used for performing grid calculations in when performing grid search.¨Allowed 
        /// coordinate systems are RT 90 2.5 gon V 0:-15 and SWEREF 99.
        /// </summary>
        [DataMember]
        public GridCoordinateSystem GridCoordinateSystem { get; set; }

        /// <summary>
        /// Property holding information if size of the grid cell is set.
        /// </summary>
        [DataMember]
        public Boolean IsGridCellSizeSpecified { get; set; }
    }
}
