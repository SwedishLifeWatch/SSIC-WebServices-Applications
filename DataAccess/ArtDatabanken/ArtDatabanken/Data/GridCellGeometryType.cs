using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enumeration of what kind of grid cell geometry information that
    /// should be returned together with requested grid statistics.
    /// </summary>
    [DataContract]
    public enum GridCellGeometryType
    {
        /// <summary>Centre point.</summary>
        [EnumMember]
        CentrePoint,

        /// <summary>Polygon.</summary>
        [EnumMember]
        Polygon
    }    
}
