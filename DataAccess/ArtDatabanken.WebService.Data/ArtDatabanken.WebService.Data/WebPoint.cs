using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a geometry coordinate.
    /// Coordinates for geometries may be 2D (x, y),
    /// 3D (x, y, z), 4D (x, y, z, m) with a m value
    /// that is part of a linear reference system or 2D
    /// with a m value (x, y, m).
    /// </summary>
    [DataContract]
    public class WebPoint : WebData
    {
        /// <summary>
        /// Initializes a new instance of the WebPoint class.
        /// </summary>
        public WebPoint()
        {
            X = Double.MinValue;
            Y = Double.MinValue;
            IsZSpecified = false;
            IsMSpecified = false;
        }

        /// <summary>
        /// Initializes a new instance of the WebPoint class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public WebPoint(Double x, Double y)
        {
            X = x;
            Y = y;
            IsZSpecified = false;
            IsMSpecified = false;
        }


        /// <summary>
        /// Initializes a new instance of the WebPoint class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="m">The m.</param>
        public WebPoint(Double x, Double y, Double z, Double m)
        {
            X = x;
            Y = y;
            if (z.IsNotNull())
            {
                Z = z;
                IsZSpecified = true;
            }
            else
            {
                IsZSpecified = false;
            }
            if (m.IsNotNull())
            {
                M = m;
                IsMSpecified = true;
            }
            else
            {
                IsMSpecified = false;
            }
        }

        /// <summary>
        /// Indication if property M has been specified.
        /// </summary>
        [DataMember]
        public Boolean IsMSpecified
        { get; set; }

        /// <summary>
        /// Indication if property Z has been specified.
        /// </summary>
        [DataMember]
        public Boolean IsZSpecified
        { get; set; }

        /// <summary>
        /// This value is part of a linear reference system.
        /// Property IsMSpecified must be set to true if 
        /// property M is used.
        /// </summary>
        [DataMember]
        public Double M
        { get; set; }

        /// <summary>
        /// East-west value of the coordinate.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        [DataMember]
        public Double X
        { get; set; }

        /// <summary>
        /// North-south value of the coordinate.
        /// Which values that are valid depends on which
        /// coordinate system that is used.
        /// </summary>
        [DataMember]
        public Double Y
        { get; set; }

        /// <summary>
        /// Altitude value of the coordinate.
        /// Property IsZSpecified must be set to true if 
        /// property Z is used.
        /// </summary>
        [DataMember]
        public Double Z
        { get; set; }

        /// <summary>
        /// Get a copy of this point.
        /// </summary>
        /// <returns>A copy of this point.</returns>
        public WebPoint Clone()
        {
            return (WebPoint)(MemberwiseClone());
        }
    }
}
