using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataModels
{
    /// <summary>
    /// This class contains information about a geometry coordinate.
    /// Coordinates for geometries may be 2D (x, y) or 3D (x, y, z)
    /// </summary>
    [DataContract]
    public class DataPoint : IEquatable<DataPoint>
    {
        public DataPoint(
            Double x,
            Double y,
            Double? z)
        {                       
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// East-west value of the coordinate.
        /// </summary>
        [DataMember]
        public Double X { get; private set; }

        /// <summary>
        /// North-south value of the coordinate.
        /// </summary>
        [DataMember]
        public Double Y { get; private set; }

        /// <summary>
        /// Altitude value of the coordinate.
        /// </summary>
        [DataMember]
        public Double? Z { get; private set; }

        #region Equality members

        public bool Equals(DataPoint other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            bool result = (Math.Abs(X - other.X) < 0.001) && (Math.Abs(Y - other.Y) < 0.001) && 
                ((!Z.HasValue && !other.Z.HasValue) || (Z.HasValue && other.Z.HasValue && (Math.Abs(Z.Value - other.Z.Value) < 0.001)));
            return result;
            //return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((DataPoint)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(DataPoint left, DataPoint right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DataPoint left, DataPoint right)
        {
            return !Equals(left, right);
        }

        #endregion

    }
}
