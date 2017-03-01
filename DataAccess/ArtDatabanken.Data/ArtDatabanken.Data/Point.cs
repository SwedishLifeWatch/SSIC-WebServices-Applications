using System;

namespace ArtDatabanken.Data
{

    /// This class contains information about a geometry coordinate.
    /// Coordinates for geometries may be 2D (x, y),
    /// 3D (x, y, z), 4D (x, y, z, m) with a m value
    /// that is part of a linear reference system or 2D
    /// with a m value (x, y, m).
    [Serializable]
    public class Point : IPoint
    {
        /// <summary>
        /// Create a Point instance.
        /// </summary>
        public Point()
        {
        }

        /// <summary>
        /// Create a Point instance.
        /// </summary>
        /// <param name='m'>Part of a linear reference system.</param>
        /// <param name='x'>East-west value of the coordinate.</param>
        /// <param name='y'>North-south value of the coordinate.</param>
        /// <param name='z'> Altitude value of the coordinate.</param>
        /// <param name='dataContext'>Data context.</param>
        public Point(Double? m,
                      Double x,
                      Double y,
                      Double? z,
                      IDataContext dataContext)
        {
            // Check data.
            dataContext.CheckNotNull("dataContext");

            // Set data.
            DataContext = dataContext;
            M = m;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Create a Point instance.
        /// </summary>
        /// <param name='x'>East-west value of the coordinate.</param>
        /// <param name='y'>North-south value of the coordinate.</param>
        public Point(Double x, Double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Get data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// This value is part of a linear reference system.
        /// </summary>
        public Double? M { get; set; }

        /// <summary>
        /// East-west value of the coordinate.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        public Double X { get; set; }

        /// <summary>
        /// North-south value of the coordinate.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        public Double Y { get; set; }

        /// <summary>
        /// Altitude value of the coordinate.
        /// </summary>
        public Double? Z { get; set; }
    }
}
