using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about the coordinate
    /// system that is provided or requested.
    /// Use either Id or WKT property to define a coordinate system.
    /// Set property Id to value None if the WKT property is used.
    /// </summary>
    public class CoordinateSystem : ICoordinateSystem
    {
        private String _wkt;

        /// <summary>
        /// Create an CoordinateSystem instance with no
        /// information about coordinate system.
        /// </summary>
        public CoordinateSystem()
            : this(CoordinateSystemId.None, String.Empty)
        { }

        /// <summary>
        /// Create an CoordinateSystem instance.
        /// </summary>
        /// <param name="id">Coordinate system id.</param>
        public CoordinateSystem(CoordinateSystemId id)
            : this(id, String.Empty)
        { }

        /// <summary>
        /// Create an CoordinateSystem instance.
        /// </summary>
        /// <param name="wkt">Wkt with information about the coordinate system.</param>
        public CoordinateSystem(String wkt)
            : this(CoordinateSystemId.None, wkt)
        { }

        /// <summary>
        /// Create an CoordinateSystem instance.
        /// </summary>
        /// <param name="id">Coordinate system id.</param>
        /// <param name="wkt">Wkt with information about the coordinate system.</param>
        public CoordinateSystem(CoordinateSystemId id,
                                String wkt)
        {
            Id = id;
            WKT = wkt;
        }

        /// <summary>
        /// Use predefined WKT as defined by the
        /// specified enum value in property Id.
        /// Set property Id to value None if property WKT is used.
        /// </summary>
        public CoordinateSystemId Id
        {
            get; set;
        }

        /// <summary>
        /// Coordinate system defined as a string according to 
        /// coordinate system WKT as defined by OGC.
        /// Set property WKT to value None if property Id is used.
        /// </summary>
        public String WKT
        {
            get
            {
                return Id.GetWkt(_wkt);
            }

            set
            {
                _wkt = value;
            }
        }
    }
}
