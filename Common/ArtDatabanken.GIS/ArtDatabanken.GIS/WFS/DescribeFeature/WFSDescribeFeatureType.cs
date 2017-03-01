using System.Collections.Generic;
using ArtDatabanken.GIS.WFS.Capabilities;

namespace ArtDatabanken.GIS.WFS.DescribeFeature
{
    /// <summary>
    /// TBD ?!?
    /// </summary>
    public enum GeometryType
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        Polygon,

        /// <summary>
        /// TBD ?!?
        /// </summary>
        Line,

        /// <summary>
        /// TBD ?!?
        /// </summary>
        Point,

        /// <summary>
        /// TBD ?!?
        /// </summary>
        Geometry
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WFSDescribeFeatureType
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public List<Field> Fields { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public Field GeometryField { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public GeometryType GeometryType { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WfsTypeName Name { get; set; }        
    }

    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class Field
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string DataType { get; set; }
    }
}
