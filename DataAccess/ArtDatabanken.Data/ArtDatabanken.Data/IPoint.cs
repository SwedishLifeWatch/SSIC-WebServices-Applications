using System;

namespace ArtDatabanken.Data
{

    /// <summary>
    /// This interface contains information about a geometry coordinate.
    /// Coordinates for geometries may be 2D (x, y),
    /// 3D (x, y, z), 4D (x, y, z, m) with a m value
    /// that is part of a linear reference system or 2D
    /// with a m value (x, y, m).
    /// </summary>
    public interface IPoint 
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// This value is part of a linear reference system.
        /// </summary>
        Double? M { get; set; }

        /// <summary>
        /// East-west value of the coordinate.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        Double X { get; set; }

        /// <summary>
        /// North-south value of the coordinate.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        Double Y { get; set; }

        /// <summary>
        /// Altitude value of the coordinate.
        /// </summary>
        Double? Z { get; set; }
    }
}
