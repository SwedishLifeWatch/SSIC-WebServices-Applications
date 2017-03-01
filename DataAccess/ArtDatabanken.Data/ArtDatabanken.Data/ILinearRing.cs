using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// A LinearRing is a LineString that is both closed and simple.
    /// A LineString is a curve with linear interpolation
    /// between points. Each consecutive pair of points
    /// defines a line segment.
    /// </summary>
    public interface ILinearRing
    {
        /// <summary>
        /// Points that defines the LinearRing.
        /// </summary>
        List<IPoint> Points { get; set; }
    }
}
